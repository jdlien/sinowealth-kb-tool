use core::time;
use std::{ffi::CStr, thread, time::Duration};

use hidapi::{BusType, DeviceInfo, HidDevice, HidError, MAX_REPORT_DESCRIPTOR_SIZE};
use hidparser::parse_report_descriptor;
use indicatif::ProgressBar;
use itertools::Itertools;
use log::{debug, error, info};
use thiserror::Error;

use crate::{
    hid_tree::{DeviceNode, InterfaceNode},
    is_expected_error, DeviceSpec, ISPDevice,
};

#[cfg(any(target_os = "macos", target_os = "windows"))]
use crate::hid_tree::ItemNode;

const REPORT_ID_ISP: u8 = 0x05;
const CMD_ISP_MODE: u8 = 0x75;

const REPORT_ID_XFER: u8 = 0x06;

const GAMING_KB_VENDOR_ID: u16 = 0x0603;
const GAMING_KB_PRODUCT_ID: u16 = 0x1020;
const GAMING_KB_V2_PRODUCT_ID: u16 = 0x1021;
const GAMING_KB_IFACE: i32 = 0;

const COMMAND_LENGTH: usize = 6;

#[derive(Debug, Error)]
pub enum DeviceSelectorError {
    #[error("Device not found")]
    NotFound,
    #[error(transparent)]
    HidError(#[from] HidError),
    #[error("Failed to parse report descriptor {0:?}")]
    ReportDescriptorError(hidparser::report_descriptor_parser::ReportDescriptorError),
    #[error("Unexpected device count")]
    UnexpectedDeviceCount,
}

pub struct DeviceSelector {
    api: hidapi::HidApi,
}

impl DeviceSelector {
    pub fn new() -> Result<Self, DeviceSelectorError> {
        let api = hidapi::HidApi::new().map_err(DeviceSelectorError::from)?;

        #[cfg(target_os = "macos")]
        api.set_open_exclusive(false); // macOS will throw a privilege violation error otherwise

        Ok(Self { api })
    }

    fn sorted_usb_device_list(&self) -> Vec<&DeviceInfo> {
        let mut devices: Vec<_> = self
            .api
            .device_list()
            .filter(|d| d.bus_type() as u32 == BusType::Usb as u32)
            .collect();
        // TODO: move out the platform specific sorting
        devices.sort_by_key(|d| {
            #[cfg(not(target_os = "linux"))]
            return (
                d.vendor_id(),
                d.product_id(),
                d.interface_number(),
                d.path(),
                d.usage_page(),
                d.usage(),
            );
            #[cfg(target_os = "linux")]
            return (
                d.vendor_id(),
                d.product_id(),
                d.interface_number(),
                d.path(),
            );
        });
        devices
    }

    fn unique_usb_device_list(&self) -> Vec<&DeviceInfo> {
        let mut devices: Vec<_> = self.sorted_usb_device_list();
        devices.dedup_by_key(|d| {
            (
                d.vendor_id(),
                d.product_id(),
                d.interface_number(),
                d.path(),
            )
        });
        devices
    }

    fn get_feature_report_ids_from_path(
        &self,
        path: &CStr,
    ) -> Result<Vec<u32>, DeviceSelectorError> {
        let dev = self
            .api
            .open_path(path)
            .map_err(DeviceSelectorError::from)?;
        self.get_feature_report_ids_from_device(&dev)
    }

    fn get_feature_report_ids_from_device(
        &self,
        dev: &HidDevice,
    ) -> Result<Vec<u32>, DeviceSelectorError> {
        let mut buf: [u8; MAX_REPORT_DESCRIPTOR_SIZE] = [0; MAX_REPORT_DESCRIPTOR_SIZE];
        let size: usize = dev
            .get_report_descriptor(&mut buf)
            .map_err(DeviceSelectorError::from)?;
        let descriptor = buf[..size].to_vec();
        self.get_feature_report_ids_from_descriptor(&descriptor)
    }

    fn get_feature_report_ids_from_descriptor(
        &self,
        descriptor: &[u8],
    ) -> Result<Vec<u32>, DeviceSelectorError> {
        let report_descriptor = parse_report_descriptor(descriptor)
            .map_err(DeviceSelectorError::ReportDescriptorError)?;
        let res = report_descriptor
            .features
            .iter()
            .filter_map(|item| item.report_id)
            .map(|report_id| report_id.into())
            .collect();
        Ok(res)
    }

    fn get_report_descriptor(&self, dev: &HidDevice) -> Result<Vec<u8>, DeviceSelectorError> {
        let mut buf: [u8; MAX_REPORT_DESCRIPTOR_SIZE] = [0; MAX_REPORT_DESCRIPTOR_SIZE];
        let size: usize = dev
            .get_report_descriptor(&mut buf)
            .map_err(DeviceSelectorError::from)?;
        Ok(buf[..size].to_vec())
    }

    fn get_descriptor_with_features(
        &self,
        path: &CStr,
    ) -> (
        Result<Vec<u8>, DeviceSelectorError>,
        Result<Vec<u32>, DeviceSelectorError>,
    ) {
        let descriptor: Result<Vec<u8>, DeviceSelectorError>;
        let feature_report_ids: Result<Vec<u32>, DeviceSelectorError>;
        match self.api.open_path(path) {
            Ok(ref dev) => {
                descriptor = self.get_report_descriptor(dev);
                match descriptor {
                    Ok(ref report) => {
                        feature_report_ids = self.get_feature_report_ids_from_descriptor(report);
                    }
                    Err(_) => {
                        feature_report_ids = Err(DeviceSelectorError::NotFound);
                    }
                }
            }
            Err(err) => {
                descriptor = Err(DeviceSelectorError::from(err));
                feature_report_ids = Err(DeviceSelectorError::NotFound);
            }
        }
        (descriptor, feature_report_ids)
    }

    #[cfg(target_os = "windows")]
    fn get_devices_for_report_ids<'a, I: IntoIterator<Item = &'a DeviceInfo>>(
        &self,
        devices: I,
        report_ids: &[u32],
    ) -> Result<Vec<&'a DeviceInfo>, DeviceSelectorError> {
        let mut matched_devices: Vec<Option<&DeviceInfo>> = vec![None; report_ids.len()];

        for d in devices {
            let retrieved_ids = self.get_feature_report_ids_from_path(d.path())?;
            for id in retrieved_ids {
                for (i, expected_id) in report_ids.iter().enumerate() {
                    if id == *expected_id {
                        if matched_devices[i].is_some() {
                            return Err(DeviceSelectorError::UnexpectedDeviceCount);
                        }
                        matched_devices[i] = Some(d);
                    }
                }
            }
        }

        if matched_devices.iter().all(|d| d.is_some()) {
            let matched_devices: Vec<&DeviceInfo> =
                matched_devices.into_iter().map(|d| d.unwrap()).collect();
            Ok(matched_devices)
        } else {
            Err(DeviceSelectorError::NotFound)
        }
    }

    #[cfg(any(target_os = "macos", target_os = "linux"))]
    fn get_device_for_report_ids<'a, I: IntoIterator<Item = &'a DeviceInfo>>(
        &self,
        devices: I,
        report_ids: &[u32],
    ) -> Result<&'a DeviceInfo, DeviceSelectorError> {
        let mut matching_devices = vec![];

        for d in devices {
            let retrieved_ids = self.get_feature_report_ids_from_path(d.path())?;
            if report_ids.iter().all(|id| retrieved_ids.contains(id)) {
                matching_devices.push(d);
            }
        }

        match matching_devices.len() {
            1 => Ok(matching_devices[0]),
            len if len > 1 => Err(DeviceSelectorError::UnexpectedDeviceCount),
            _ => Err(DeviceSelectorError::NotFound),
        }
    }

    fn find_isp_device(&self, device_spec: DeviceSpec) -> Result<ISPDevice, DeviceSelectorError> {
        let sorted_devices = self.unique_usb_device_list();
        let isp_devices: Vec<_> = sorted_devices
            .clone()
            .into_iter()
            .filter(|d| {
                // Special case for Lofree Flow Lite already in ISP mode
                if device_spec.vendor_id == 0x3554 && device_spec.product_id == 0xf808 {
                    return d.vendor_id() == device_spec.vendor_id
                        && d.product_id() == device_spec.product_id
                        && d.interface_number() == device_spec.isp_iface_num;
                }
                
                d.vendor_id() == GAMING_KB_VENDOR_ID
                    && matches!(
                        d.product_id(),
                        GAMING_KB_PRODUCT_ID | GAMING_KB_V2_PRODUCT_ID
                    )
                    && d.interface_number() == GAMING_KB_IFACE
            })
            .collect();

        let device_count = isp_devices.len();
        if device_count == 0 {
            return Err(DeviceSelectorError::NotFound);
        }

        #[cfg(any(target_os = "macos", target_os = "linux"))]
        return {
            // Special handling for Lofree Flow Lite
            if device_spec.vendor_id == 0x3554 && device_spec.product_id == 0xf808 {
                if let Some(device) = isp_devices.first() {
                    debug!("ISP device (Lofree): {}", device.info());
                    let handle = self
                        .api
                        .open_path(device.path())
                        .map_err(DeviceSelectorError::from)?;
                    return Ok(ISPDevice::new(device_spec, handle));
                }
            }
            
            let device = self.get_device_for_report_ids(
                isp_devices.clone(),
                &[REPORT_ID_ISP as u32, REPORT_ID_XFER as u32],
            )?;
            debug!("ISP device: {}", device.info());

            let handle = self
                .api
                .open_path(device.path())
                .map_err(DeviceSelectorError::from)?;

            Ok(ISPDevice::new(device_spec, handle))
        };

        #[cfg(target_os = "windows")]
        return {
            let devices = self.get_devices_for_report_ids(
                isp_devices.clone(),
                &[REPORT_ID_ISP as u32, REPORT_ID_XFER as u32],
            )?;

            let cmd_device = devices[0];
            debug!("ISP CMD device: {}", cmd_device.info());

            let xfer_device = devices[1];
            debug!("ISP XFER device: {}", xfer_device.info());

            let cmd_handle = self
                .api
                .open_path(cmd_device.path())
                .map_err(DeviceSelectorError::from)?;
            let xfer_handle = self
                .api
                .open_path(xfer_device.path())
                .map_err(DeviceSelectorError::from)?;

            Ok(ISPDevice::new(device_spec, cmd_handle, xfer_handle))
        };
    }

    fn find_device(&self, device_spec: DeviceSpec) -> Result<HidDevice, DeviceSelectorError> {
        let filtered_devices = self.unique_usb_device_list().into_iter().filter(|d| {
            d.vendor_id() == device_spec.vendor_id
                && d.product_id() == device_spec.product_id
                && d.interface_number() == device_spec.isp_iface_num
        });

        let mut cmd_device_info: Option<&DeviceInfo> = None;
        for d in filtered_devices {
            // Special case for Lofree Flow Lite devices which don't have standard feature reports
            if (device_spec.vendor_id == 0x3554 && device_spec.product_id == 0xf808) ||
               (device_spec.vendor_id == 0x05ac && device_spec.product_id == 0x024f) {
                cmd_device_info = Some(d);
                break;
            }
            
            let ids = self
                .get_feature_report_ids_from_path(d.path())
                .map_err(|_| DeviceSelectorError::NotFound)?;
            for id in ids {
                if id == device_spec.isp_report_id {
                    cmd_device_info = Some(d);
                }
            }
        }

        let Some(cmd_device_info) = cmd_device_info else {
            info!("Device didn't come up...");
            return Err(DeviceSelectorError::NotFound);
        };

        debug!("Opening: {:?}", cmd_device_info.path());
        let device = self
            .api
            .open_path(cmd_device_info.path())
            .map_err(DeviceSelectorError::from)?;
        Ok(device)
    }

    fn switch_to_isp_device(
        &mut self,
        device: HidDevice,
        device_spec: DeviceSpec,
    ) -> Result<ISPDevice, DeviceSelectorError> {
        // Special case for Lofree devices - use device directly without ISP mode switching
        if (device_spec.vendor_id == 0x3554 && device_spec.product_id == 0xf808) ||
           (device_spec.vendor_id == 0x05ac && device_spec.product_id == 0x024f) {
            debug!("Lofree device detected - using device directly without ISP mode switch");
            return Ok(ISPDevice::new(device_spec, device));
        }
        
        if let Err(err) = self.enter_isp_mode(&device) {
            debug!("Error: {:}", err);
            match err {
                DeviceSelectorError::HidError(err) if is_expected_error(&err) => {}
                _ => {
                    error!("Unexpected: {:}", err);
                    info!("Waiting...");
                    thread::sleep(time::Duration::from_secs(2));
                    return Err(err);
                }
            }
        }

        info!("Waiting for ISP device...");
        thread::sleep(time::Duration::from_secs(2));

        self.api.refresh_devices()?;

        let Ok(isp_device) = self.find_isp_device(device_spec) else {
            info!("ISP device didn't come up...");
            return Err(DeviceSelectorError::NotFound);
        };
        Ok(isp_device)
    }

    pub fn try_fetch_isp_device(
        &mut self,
        device_spec: DeviceSpec,
        retries: usize,
        bootloader_ready: bool,
    ) -> Result<ISPDevice, DeviceSelectorError> {
        eprintln!(
            "Looking for {:04x}:{:04x} (isp_iface_num={} isp_report_id={})",
            device_spec.vendor_id,
            device_spec.product_id,
            device_spec.isp_iface_num,
            device_spec.isp_report_id
        );

        let bar = ProgressBar::new_spinner()
            .with_message(format!("Searching for device... Attempt {}/{}", 1, retries));
        bar.enable_steady_tick(Duration::from_millis(100));

        for attempt in 1..retries + 1 {
            if attempt > 1 {
                bar.set_message(format!("Retrying... Attempt {}/{}", attempt, retries));
                info!("Retrying... Attempt {}/{}", attempt, retries);
                self.api.refresh_devices()?;
                thread::sleep(time::Duration::from_millis(1000));
            }

            if !bootloader_ready {
                match self.find_device(device_spec) {
                    Ok(device) => {
                        bar.set_message("Device found. Switching to ISP mode...");
                        match self.switch_to_isp_device(device, device_spec) {
                            Ok(isp_device) => {
                                bar.finish_and_clear();
                                eprintln!("Connected!");
                                return Ok(isp_device);
                            }
                            Err(DeviceSelectorError::NotFound) => {}
                            Err(err) => {
                                return Err(err);
                            }
                        }
                    }
                    Err(DeviceSelectorError::NotFound) => {}
                    Err(err) => {
                        return Err(err);
                    }
                }
            }

            info!("Device not found. Trying ISP device...");
            match self.find_isp_device(device_spec) {
                Ok(isp_device) => {
                    bar.finish_and_clear();
                    eprintln!("Connected!");
                    return Ok(isp_device);
                }
                Err(DeviceSelectorError::NotFound) => {}
                Err(err) => {
                    return Err(err);
                }
            }
        }
        bar.finish_and_clear();
        Err(DeviceSelectorError::NotFound)
    }

    fn enter_isp_mode(&self, handle: &HidDevice) -> Result<(), DeviceSelectorError> {
        let cmd: [u8; COMMAND_LENGTH] = [REPORT_ID_ISP, CMD_ISP_MODE, 0x00, 0x00, 0x00, 0x00];
        handle.send_feature_report(&cmd)?;
        Ok(())
    }

    pub fn connected_devices_tree(&self) -> Result<Vec<DeviceNode>, DeviceSelectorError> {
        let devices: Vec<_> = self.sorted_usb_device_list();

        let id_chunks = devices
            .into_iter()
            .chunk_by(|d| (d.vendor_id(), d.product_id()));

        let mut device_tree_devices: Vec<DeviceNode> = vec![];

        for (key, devices) in &id_chunks {
            let (vid, pid) = key;

            let mut interface_nodes: Vec<InterfaceNode> = vec![];

            // for some reason on linux-libusb the same device might not have the same manufacturer string in some cases
            let mut manufacturer_string: Option<String> = None;
            let mut product_string: Option<String> = None;

            let path_chunks = devices.chunk_by(|d| (d.path(), d.interface_number()));

            for (key, devices) in &path_chunks {
                let (path, interface_number) = key;

                #[cfg(any(target_os = "macos", target_os = "windows"))]
                let mut children: Vec<ItemNode> = vec![];

                for d in devices {
                    if manufacturer_string.is_none() {
                        manufacturer_string = d.manufacturer_string().map(str::to_string);
                    }
                    if product_string.is_none() {
                        product_string = d.product_string().map(str::to_string);
                    }
                    #[cfg(target_os = "macos")]
                    children.push(ItemNode {
                        usage_page: d.usage_page(),
                        usage: d.usage(),
                    });
                    #[cfg(target_os = "windows")]
                    {
                        let (descriptor, feature_report_ids) =
                            self.get_descriptor_with_features(path);
                        children.push(ItemNode {
                            path: path.to_str().unwrap().to_string(),
                            usage_page: d.usage_page(),
                            usage: d.usage(),
                            descriptor,
                            feature_report_ids,
                        });
                    }
                }

                #[cfg(any(target_os = "macos", target_os = "linux"))]
                let (descriptor, feature_report_ids) = self.get_descriptor_with_features(path);
                let interface_node = InterfaceNode {
                    #[cfg(any(target_os = "macos", target_os = "linux"))]
                    path: path.to_str().unwrap().to_string(),
                    interface_number,
                    #[cfg(any(target_os = "macos", target_os = "linux"))]
                    descriptor,
                    #[cfg(any(target_os = "macos", target_os = "linux"))]
                    feature_report_ids,
                    #[cfg(any(target_os = "macos", target_os = "windows"))]
                    children,
                };

                interface_nodes.push(interface_node);
            }

            device_tree_devices.push(DeviceNode {
                vendor_id: vid,
                product_id: pid,
                manufacturer_string: manufacturer_string.clone().unwrap_or("None".to_string()),
                product_string: product_string.clone().unwrap_or("None".to_string()),
                children: interface_nodes,
            });
        }
        Ok(device_tree_devices)
    }
}

trait PlatformSpecificInfo {
    fn info(&self) -> String;
}

impl PlatformSpecificInfo for DeviceInfo {
    fn info(&self) -> String {
        #[cfg(not(target_os = "linux"))]
        return format!(
            "{:#06x} {:#06x} {:?} {} {:#06x} {:#06x}",
            self.vendor_id(),
            self.product_id(),
            self.path(),
            self.interface_number(),
            self.usage_page(),
            self.usage()
        );
        #[cfg(target_os = "linux")]
        format!(
            "{:#06x} {:#06x} {:?}",
            self.vendor_id(),
            self.product_id(),
            self.path()
        )
    }
}
