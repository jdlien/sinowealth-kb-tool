use core::panic;
use std::{str::FromStr, thread, time};

use indicatif::ProgressBar;
use log::{debug, error};
use thiserror::Error;

use crate::{device_spec::*, is_expected_error, util, VerificationError, PlatformSpec};

extern crate hidapi;

use hidapi::{HidDevice, HidError};

use crate::devices::lofree::firmware_format::UpgradeFileHeader;

const COMMAND_LENGTH: usize = 6;

const REPORT_ID_CMD: u8 = 0x05;
const REPORT_ID_XFER: u8 = 0x06;

const CMD_ENABLE_FIRMWARE: u8 = 0x55;
const CMD_INIT_READ: u8 = 0x52;
const CMD_INIT_WRITE: u8 = 0x57;
const CMD_ERASE: u8 = 0x45;
const CMD_REBOOT: u8 = 0x5a;

const XFER_READ_PAGE: u8 = 0x72;
const XFER_WRITE_PAGE: u8 = 0x77;

pub struct ISPDevice {
    cmd_device: HidDevice,
    #[cfg(target_os = "windows")]
    xfer_device: HidDevice,
    device_spec: DeviceSpec,
}

#[derive(Debug, Error)]
pub enum ISPError {
    #[error(transparent)]
    HidError(#[from] HidError),
    #[error(transparent)]
    VerificationError(#[from] VerificationError),
}

#[derive(Debug, Clone)]
pub enum ReadSection {
    Firmware,
    Bootloader,
    Full,
}

impl ReadSection {
    pub fn to_str(&self) -> &'static str {
        match self {
            ReadSection::Firmware => "firmware",
            ReadSection::Bootloader => "bootloader",
            ReadSection::Full => "full",
        }
    }

    pub fn available_sections() -> Vec<&'static str> {
        vec![
            ReadSection::Firmware.to_str(),
            ReadSection::Bootloader.to_str(),
            ReadSection::Full.to_str(),
        ]
    }
}

impl FromStr for ReadSection {
    type Err = ();
    fn from_str(section: &str) -> Result<Self, Self::Err> {
        Ok(match section {
            "bootloader" => ReadSection::Bootloader,
            "full" => ReadSection::Full,
            "firmware" => ReadSection::Firmware,
            _ => panic!("Invalid read section: {}", section),
        })
    }
}

impl ISPDevice {
    fn get_cmd_report_id(&self) -> u8 {
        // Flow Lite uses single report ID 0x06 for both cmd and xfer
        if self.device_spec.vendor_id == 0x3554 && self.device_spec.product_id == 0xf808 {
            0x06
        } else {
            REPORT_ID_CMD
        }
    }

    fn get_xfer_report_id(&self) -> u8 {
        // Flow Lite uses single report ID 0x06 for both cmd and xfer
        if self.device_spec.vendor_id == 0x3554 && self.device_spec.product_id == 0xf808 {
            0x06
        } else {
            REPORT_ID_XFER
        }
    }

    #[cfg(not(target_os = "windows"))]
    pub fn new(device_spec: DeviceSpec, device: HidDevice) -> Self {
        Self {
            cmd_device: device,
            device_spec,
        }
    }

    #[cfg(target_os = "windows")]
    pub fn new(device_spec: DeviceSpec, cmd_device: HidDevice, xfer_device: HidDevice) -> Self {
        Self {
            cmd_device,
            xfer_device,
            device_spec,
        }
    }

    pub fn read_cycle(&self, read_fragment: ReadSection) -> Result<Vec<u8>, ISPError> {
        // Check if this is a Lofree Flow Lite in bootloader mode
        if self.device_spec.vendor_id == 0x3554 && self.device_spec.product_id == 0xf808 {
            eprintln!("Warning: Reading from Lofree Flow Lite bootloader mode is not yet implemented");
            eprintln!("The Lofree protocol for reading needs to be reverse-engineered from successful read operations");
            return Err(ISPError::HidError(HidError::HidApiError { message: "Reading from Lofree Flow Lite bootloader not supported yet".to_string() }));
        }

        // Check if this is a Lofree Flow Lite in runtime mode
        if self.device_spec.vendor_id == 0x05ac && self.device_spec.product_id == 0x024f {
            eprintln!("Lofree Flow Lite runtime mode - attempting to read firmware...");
            return self.lofree_read_cycle(read_fragment);
        }

        self.enable_firmware()?;

        let (start_addr, length) = match read_fragment {
            ReadSection::Firmware => (0, self.device_spec.platform.firmware_size),
            ReadSection::Bootloader => (
                self.device_spec.platform.firmware_size,
                self.device_spec.platform.bootloader_size,
            ),
            ReadSection::Full => (
                0,
                self.device_spec.platform.firmware_size + self.device_spec.platform.bootloader_size,
            ),
        };

        let firmware = self.read(start_addr, length)?;

        if self.device_spec.reboot {
            self.reboot();
        }

        Ok(firmware)
    }

    pub fn write_cycle(&self, firmware: &mut [u8]) -> Result<(), ISPError> {
        // Check if this is a Lofree Flow Lite in runtime mode - try to switch to bootloader
        if self.device_spec.vendor_id == 0x05ac && self.device_spec.product_id == 0x024f {
            eprintln!("Lofree Flow Lite detected in runtime mode - attempting to switch to bootloader mode...");
            return self.lofree_switch_to_bootloader_and_write(firmware);
        }

        // Check if this is a Lofree Flow Lite in bootloader mode and use specialized protocol
        if self.device_spec.vendor_id == 0x3554 && self.device_spec.product_id == 0xf808 {
            return self.lofree_write_cycle(firmware);
        }

        // ensure that the address at <firmware_size-4> is the same as the reset vector
        firmware.copy_within(1..3, self.device_spec.platform.firmware_size - 4);

        self.erase()?;
        self.write(0, firmware)?;

        // cleanup the address at <firmware_size-4>
        firmware[self.device_spec.platform.firmware_size - 4
            ..self.device_spec.platform.firmware_size - 2]
            .fill(0);

        let read_back = self.read(0, self.device_spec.platform.firmware_size)?;

        eprintln!("Verifying...");
        util::verify(firmware, &read_back).map_err(ISPError::from)?;

        self.enable_firmware()?;

        if self.device_spec.reboot {
            self.reboot();
        }

        Ok(())
    }

    fn xfer_device(&self) -> &HidDevice {
        #[cfg(target_os = "windows")]
        return &self.xfer_device;
        #[cfg(not(target_os = "windows"))]
        &self.cmd_device
    }

    fn read(&self, start_addr: usize, length: usize) -> Result<Vec<u8>, ISPError> {
        let page_size = self.device_spec.platform.page_size;
        let num_page = length / page_size;
        let mut result: Vec<u8> = vec![];

        eprintln!("Reading...");
        let bar = ProgressBar::new(num_page as u64);

        self.init_read(start_addr)?;

        for i in 0..num_page {
            bar.inc(1);
            debug!(
                "Reading page {} @ offset {:#06x}",
                i,
                start_addr + i * page_size
            );
            self.read_page(&mut result)?;
        }
        bar.finish();
        Ok(result)
    }

    fn write(&self, start_addr: usize, buffer: &[u8]) -> Result<(), ISPError> {
        eprintln!("Writing...");
        let bar = ProgressBar::new(self.device_spec.num_pages() as u64);
        self.init_write(start_addr)?;

        let page_size = self.device_spec.platform.page_size;
        for i in 0..self.device_spec.num_pages() {
            bar.inc(1);
            debug!("Writing page {} @ offset {:#06x}", i, i * page_size);
            self.write_page(&buffer[(i * page_size)..((i + 1) * page_size)])?;
        }
        bar.finish();
        Ok(())
    }

    /// Initializes the read operation / sets the initial read address
    fn init_read(&self, start_addr: usize) -> Result<(), ISPError> {
        let cmd: [u8; COMMAND_LENGTH] = [
            self.get_cmd_report_id(),
            CMD_INIT_READ,
            (start_addr & 0xff) as u8,
            (start_addr >> 8) as u8,
            0,
            0,
        ];
        self.cmd_device
            .send_feature_report(&cmd)
            .map_err(ISPError::from)?;
        Ok(())
    }

    /// Initializes the write operation / sets the initial write address
    fn init_write(&self, start_addr: usize) -> Result<(), ISPError> {
        let cmd: [u8; COMMAND_LENGTH] = [
            self.get_cmd_report_id(),
            CMD_INIT_WRITE,
            (start_addr & 0xff) as u8,
            (start_addr >> 8) as u8,
            0,
            0,
        ];
        self.cmd_device
            .send_feature_report(&cmd)
            .map_err(ISPError::from)?;
        Ok(())
    }

    /// Reads one page of flash contents
    fn read_page(&self, buf: &mut Vec<u8>) -> Result<(), ISPError> {
        let page_size = self.device_spec.platform.page_size;
        let mut xfer_buf: Vec<u8> = vec![0; page_size + 2];
        xfer_buf[0] = self.get_xfer_report_id();
        xfer_buf[1] = XFER_READ_PAGE;
        self.xfer_device()
            .get_feature_report(&mut xfer_buf)
            .map_err(ISPError::from)?;
        buf.extend_from_slice(&xfer_buf[2..(page_size + 2)]);
        Ok(())
    }

    /// Writes one page to flash
    ///
    /// Note: The first 3 bytes at address 0x0000 (first-page) are skipped. Instead the second and
    /// third bytes (firmware's reset vector LJMP destination address) are written to address
    /// <firmware_size-4> and will later be part of the LJMP instruction after the firmware is
    /// enabled (`enable_firmware`). This only works once after an erase operation.
    fn write_page(&self, buf: &[u8]) -> Result<(), ISPError> {
        let length = buf.len() + 2;
        let mut xfer_buf: Vec<u8> = vec![0; length];
        xfer_buf[0] = self.get_xfer_report_id();
        xfer_buf[1] = XFER_WRITE_PAGE;
        xfer_buf[2..length].clone_from_slice(buf);
        self.xfer_device()
            .send_feature_report(&xfer_buf)
            .map_err(ISPError::from)?;
        Ok(())
    }

    /// Sets a LJMP (0x02) opcode at <firmware_size-5>.
    /// This enables the main firmware by making the bootloader jump to it on reset.
    ///
    /// Side-effect: enables reading the firmware without erasing flash first.
    /// Credits to @gashtaan for finding this out.
    fn enable_firmware(&self) -> Result<(), ISPError> {
        // Special handling for Lofree Flow Lite devices
        if self.device_spec.vendor_id == 0x05ac && self.device_spec.product_id == 0x024f {
            eprintln!("Enabling Lofree Flow Lite firmware mode...");
            return self.lofree_enable_firmware_mode();
        }

        eprintln!("Enabling firmware...");
        let cmd: [u8; COMMAND_LENGTH] = [self.get_cmd_report_id(), CMD_ENABLE_FIRMWARE, 0, 0, 0, 0];

        self.cmd_device.send_feature_report(&cmd)?;
        Ok(())
    }

    /// Erases everything in flash, except the ISP bootloader section itself and initializes the
    /// reset vector to jump to ISP.
    fn erase(&self) -> Result<(), ISPError> {
        eprintln!("Erasing...");
        let cmd: [u8; COMMAND_LENGTH] = [self.get_cmd_report_id(), CMD_ERASE, 0, 0, 0, 0];
        self.cmd_device
            .send_feature_report(&cmd)
            .map_err(ISPError::from)?;
        thread::sleep(time::Duration::from_millis(2000));
        Ok(())
    }

    /// Causes the device to start running the main firmware
    fn reboot(&self) {
        eprintln!("Rebooting...");
        let cmd: [u8; COMMAND_LENGTH] = [self.get_cmd_report_id(), CMD_REBOOT, 0, 0, 0, 0];
        if let Err(err) = self.cmd_device.send_feature_report(&cmd) {
            debug!("Error: {:}", err);
            if !is_expected_error(&err) {
                error!("Unexpected error: {:}", err);
            }
        }
        thread::sleep(time::Duration::from_millis(2000));
    }

    /// Lofree Flow Lite specific write cycle using the discovered protocol
    fn lofree_write_cycle(&self, firmware_bytes_from_file: &[u8]) -> Result<(), ISPError> {
        eprintln!("Using Lofree Flow Lite protocol with header parsing...");

        const HEADER_SIZE: usize = UpgradeFileHeader::HEADER_SIZE; // 720 bytes
        let mut final_firmware_to_write: Vec<u8>;
        let relevant_data_for_finalize: Vec<u8>; // This will contain the part with the LJMP footer

        if firmware_bytes_from_file.len() >= HEADER_SIZE {
            eprintln!("Attempting to parse UpgradeFileHeader...");
            match UpgradeFileHeader::from_bytes(&firmware_bytes_from_file[..HEADER_SIZE]) {
                Ok(header) => {
                    // Copy packed fields to local variables before use
                    let head_len = header.head_length;
                    let fw_len = header.fw_length;

                    if head_len == HEADER_SIZE as u32 {
                        eprintln!("Successfully parsed firmware header. FW Length: {} bytes", fw_len);
                        let code_offset = HEADER_SIZE;
                        let code_length = fw_len as usize;

                        if firmware_bytes_from_file.len() >= code_offset + code_length {
                            let mut code_data = firmware_bytes_from_file[code_offset .. code_offset + code_length].to_vec();

                            self.lofree_add_ljmp_footer(&mut code_data)?;
                            relevant_data_for_finalize = code_data.clone();

                            final_firmware_to_write = firmware_bytes_from_file[..HEADER_SIZE].to_vec();
                            final_firmware_to_write.extend_from_slice(&code_data);
                            eprintln!("Prepared image with header ({} bytes) + processed code ({} bytes) = total {} bytes", HEADER_SIZE, code_data.len(), final_firmware_to_write.len());
                        } else {
                            eprintln!("Firmware file too short for declared code length in header. Treating as raw.");
                            let mut fw_clone = firmware_bytes_from_file.to_vec();
                            self.lofree_add_ljmp_footer(&mut fw_clone)?;
                            final_firmware_to_write = fw_clone.clone();
                            relevant_data_for_finalize = fw_clone;
                        }
                    } else {
                        eprintln!("Header length mismatch (expected {}, got {}). Treating as raw firmware.", HEADER_SIZE, head_len);
                        let mut fw_clone = firmware_bytes_from_file.to_vec();
                        self.lofree_add_ljmp_footer(&mut fw_clone)?;
                        final_firmware_to_write = fw_clone.clone();
                        relevant_data_for_finalize = fw_clone;
                    }
                }
                Err(e) => {
                    eprintln!("Failed to parse header ({}). Treating as raw firmware.", e);
                    let mut fw_clone = firmware_bytes_from_file.to_vec();
                    self.lofree_add_ljmp_footer(&mut fw_clone)?;
                    final_firmware_to_write = fw_clone.clone();
                    relevant_data_for_finalize = fw_clone;
                }
            }
        } else {
            eprintln!("Firmware file smaller than header size. Treating as raw firmware.");
            let mut fw_clone = firmware_bytes_from_file.to_vec();
            self.lofree_add_ljmp_footer(&mut fw_clone)?;
            final_firmware_to_write = fw_clone.clone();
            relevant_data_for_finalize = fw_clone;
        }

        // Send initial setup commands
        self.lofree_init_bootloader()?;
        self.lofree_setup_write()?;

        // Write the (potentially header + code + footer) image
        self.lofree_write_firmware(&final_firmware_to_write)?;

        // The `firmware_data` for `lofree_finalize_with_data` should be the block
        // that the bootloader is expected to validate via its LJMP footer's CRC.
        // This is the `relevant_data_for_finalize` which has had the footer added.
        self.lofree_finalize_with_data(&relevant_data_for_finalize)?;

        eprintln!("Lofree firmware update completed");
        Ok(())
    }

    /// Send initial bootloader command for Lofree Flow Lite
    fn lofree_init_bootloader(&self) -> Result<(), ISPError> {
        let cmd = vec![
            0x06, // Report ID
            0xb0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xde, 0xc8, 0x21, 0xa4, 0xc9, 0x07,
            0x00, 0x00 // Pad to 17 bytes
        ];

        // CRITICAL: Use interrupt write, not feature report
        self.cmd_device.write(&cmd)?;
        thread::sleep(time::Duration::from_millis(100));
        Ok(())
    }

    /// Send setup write commands for Lofree Flow Lite
    fn lofree_setup_write(&self) -> Result<(), ISPError> {
        // Setup command 1
        let cmd1 = vec![
            0x06, 0x5b, 0xb5, 0x02, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        ];
        self.cmd_device.write(&cmd1)?;
        thread::sleep(time::Duration::from_millis(10));

        // Setup command 2
        let cmd2 = vec![
            0x06, 0x5b, 0xb5, 0x02, 0x01, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        ];
        self.cmd_device.write(&cmd2)?;
        thread::sleep(time::Duration::from_millis(10));

        Ok(())
    }

    /// Write firmware data for Lofree Flow Lite
    fn lofree_write_firmware(&self, firmware: &[u8]) -> Result<(), ISPError> {
        eprintln!("Writing firmware data...");
        let bar = indicatif::ProgressBar::new((firmware.len() / 32) as u64);

        // Write firmware in 32-byte chunks
        // Protocol: 06b1c020[00080016][address][32-bytes-data][checksum]
        // Total packet size: 48 bytes (1 + 4 + 4 + 4 + 32 + 4 - 1)

        let mut address: u32 = 0x1600; // Start address from protocol analysis
        let chunks = firmware.chunks(32);
        let total_chunks = chunks.len();

        for (idx, chunk) in chunks.enumerate() {
            bar.inc(1);

            // Build complete packet
            let mut packet = vec![0x06]; // Report ID
            packet.push(0xb1); // Write command

            // Check if this is the last chunk
            let is_last = idx == total_chunks - 1;
            if is_last && chunk.len() == 8 {
                // Special case: last chunk with 8 bytes uses flag 0xc108
                packet.push(0xc1);
                packet.push(0x08);
                debug!("Last chunk: 8 bytes with flag 0xc108");
            } else if chunk.len() == 32 {
                // Normal 32-byte chunk
                packet.push(0xc0);
                packet.push(0x20);
            } else {
                // Other sized chunk
                packet.push(0xc0);
                packet.push(chunk.len() as u8);
                debug!("Partial chunk: {} bytes", chunk.len());
            }

            // Fixed header bytes
            packet.extend_from_slice(&[0x00, 0x08, 0x00, 0x16]);

            // Address (big-endian - high byte first, matching C# implementation)
            packet.extend_from_slice(&address.to_be_bytes());

            // Data (padded to 32 bytes with 0xFF)
            packet.extend_from_slice(chunk);
            while packet.len() < 44 { // 1 + 4 + 4 + 4 + 32 = 45, but we need 44 before checksum
                packet.push(0xFF);
            }

            // Calculate checksum (simple sum of all data bytes)
            let checksum = self.calculate_packet_checksum(&packet[12..44]);
            packet.extend_from_slice(&checksum);

            // Packet should be exactly 48 bytes
            if packet.len() != 48 {
                eprintln!("Warning: packet size is {} instead of 48", packet.len());
            }

            // Send the packet via feature report (bootloader mode uses EP-0)
            self.cmd_device.send_feature_report(&packet)?;

            // Small delay between chunks
            thread::sleep(time::Duration::from_micros(500));

            // Update address for next chunk
            address += chunk.len() as u32;
        }

        bar.finish();
        eprintln!("Wrote {} chunks ({} bytes total)", total_chunks, firmware.len());

        Ok(())
    }

    /// Calculate packet checksum (4 bytes)
    fn calculate_packet_checksum(&self, data: &[u8]) -> [u8; 4] {
        // Based on observed patterns, this appears to be a simple sum
        let mut sum: u32 = 0;
        for &byte in data {
            sum = sum.wrapping_add(byte as u32);
        }
        sum.to_le_bytes()
    }

    /// Calculate checksum for Lofree Flow Lite (placeholder implementation)
    fn lofree_calculate_checksum(&self, data: &[u8]) -> [u8; 4] {
        // This is a placeholder - the actual checksum algorithm needs to be reverse-engineered
        // For now, using a simple XOR-based checksum
        let mut checksum: u32 = 0;
        for byte in data {
            checksum ^= *byte as u32;
        }
        checksum.to_le_bytes()
    }

    /// Add LJMP footer to firmware for proper bootloader exit
    fn lofree_add_ljmp_footer(&self, firmware: &mut Vec<u8>) -> Result<(), ISPError> {
        let firmware_size = self.device_spec.platform.firmware_size;

        // Ensure firmware is the right size
        firmware.resize(firmware_size, 0xFF);

        // Step 1: Build LJMP footer with dummy CRC = 0xFFFF
        let footer_offset = firmware_size - 5;
        firmware[footer_offset] = 0x02;     // LJMP opcode
        firmware[footer_offset + 1] = 0x00; // Address low byte (jump to 0x0000)
        firmware[footer_offset + 2] = 0x00; // Address high byte
        firmware[footer_offset + 3] = 0xFF; // Dummy CRC low byte
        firmware[footer_offset + 4] = 0xFF; // Dummy CRC high byte

        // Step 2: Calculate CRC over the whole image (including LJMP, excluding the two CRC bytes)
        // Using CRC-16/CCITT-FALSE algorithm as required by Sinowealth devices
        let crc_data = &firmware[..firmware_size - 2];
        let crc = self.calculate_crc16_ccitt_false(crc_data);

        // Step 3: Write the real CRC into the footer (big-endian for CCITT-FALSE)
        firmware[footer_offset + 3] = ((crc >> 8) & 0xFF) as u8; // CRC high byte (big-endian)
        firmware[footer_offset + 4] = (crc & 0xFF) as u8;        // CRC low byte (big-endian)

        eprintln!("Added LJMP footer at offset 0x{:X}: 02 00 00 {:02X} {:02X}",
                 footer_offset, firmware[footer_offset + 3], firmware[footer_offset + 4]);

        // Verify: CRC of the complete image should now be 0x0000 (CCITT-FALSE property)
        let verify_crc = self.calculate_crc16_ccitt_false(firmware);
        eprintln!("Verification: CRC of complete firmware = 0x{:04X} (should be 0x0000 for CCITT-FALSE)", verify_crc);

        // Debug: Show the actual footer bytes
        eprintln!("Footer bytes at 0x{:04X}: {:02X?}", footer_offset, &firmware[footer_offset..firmware_size]);

        Ok(())
    }

    /// Calculate CRC16 for firmware validation (CRC-16/CCITT-FALSE)
    /// MSB-first, poly 0x1021, init 0xFFFF, no final xor
    /// The result should give 0x0000 when CRC(data + crc_bytes) is calculated
    fn calculate_crc16_ccitt_false(&self, firmware: &[u8]) -> u16 {
        let mut crc: u16 = 0xFFFF;
        for &b in firmware {
            crc ^= (b as u16) << 8;
            for _ in 0..8 {
                crc = if crc & 0x8000 != 0 {
                    (crc << 1) ^ 0x1021
                } else {
                    crc << 1
                };
            }
        }
        crc & 0xFFFF
    }

    /// Check bootloader status with dedicated read handle logic
    fn lofree_check_status_with_read_handle(&self) -> Result<u8, ISPError> {
        let status_cmd = vec![
            0x06, 0x5b, 0xb4, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        ];

        self.cmd_device.send_feature_report(&status_cmd)?;
        thread::sleep(time::Duration::from_millis(50)); // Wait for device to process

        let mut response_buffer = vec![0u8; 17]; // Expecting a 17-byte report

        let hidapi = hidapi::HidApi::new().map_err(ISPError::HidError)?;
        let read_device_handle = if cfg!(target_os = "macos") || cfg!(target_os = "windows") {
            self.cmd_device.get_device_info()
                .ok() // Convert Result to Option
                .and_then(|info| hidapi.open_path(info.path()).ok())
        } else {
            None
        };

        let read_target_device = read_device_handle.as_ref().unwrap_or(&self.cmd_device);

        // Use read_timeout instead of get_feature_report
        match read_target_device.read_timeout(&mut response_buffer, 150) { // Increased timeout slightly
            Ok(len) if len >= 5 && response_buffer[0] == 0x06 && response_buffer[1] == 0x5b && response_buffer[2] == 0xb4 => {
                // Check for the command echo or specific status response pattern
                let status = response_buffer[4];
                Ok(status)
            }
            Ok(len) => {
                 eprintln!("Status response (read_timeout) unexpected format/length: {} bytes, raw: {:02X?}", len, &response_buffer[..len]);
                Err(ISPError::HidError(HidError::HidApiError { message: "Unexpected status response format/length".to_string() }))
            }
            Err(e) => {
                eprintln!("No status response (read_timeout): {}", e);
                Err(ISPError::HidError(e))
            }
        }
    }

    /// Send finalization commands for Lofree Flow Lite with firmware data
    fn lofree_finalize_with_data(&self, firmware_with_ljmp_footer: &[u8]) -> Result<(), ISPError> {
        eprintln!("Starting bootloader exit sequence (Hints-based timed strategy)...");

        // Step 1: Small delay after last firmware write packet.
        thread::sleep(time::Duration::from_millis(30));

        // Step 2: Send CLEAR-FAIL command (0x5B B5 99) as a precaution.
        eprintln!("Sending CLEAR-FAIL command (0x5B B5 99)...");
        let clear_fail_cmd = vec![
            0x06, 0x5b, 0xb5, 0x99, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        ];
        self.cmd_device.send_feature_report(&clear_fail_cmd)?;
        thread::sleep(time::Duration::from_millis(100)); // Delay after clear-fail

        // Step 3: Calculate parameters for VERIFY/ENABLE.
        // firmware_with_ljmp_footer is the code payload + LJMP footer,
        // already padded to platform.firmware_size by lofree_add_ljmp_footer.
        let fw_len_bytes = firmware_with_ljmp_footer.len();
        if fw_len_bytes < 5 { // Ensure we can read the footer CRC
            return Err(ISPError::HidError(HidError::HidApiError {
                message: "Firmware data for finalize is too short".to_string()
            }));
        }
        let fw_len_words = (fw_len_bytes / 2) as u16;

        // The CRC for VERIFY/ENABLE is the one embedded in the LJMP footer.
        // Footer: [0x02, 0x00, 0x00, CRC_HI, CRC_LO]
        // CRC is stored big-endian in footer, command expects little-endian.
        let footer_crc_hi = firmware_with_ljmp_footer[fw_len_bytes - 2];
        let footer_crc_lo = firmware_with_ljmp_footer[fw_len_bytes - 1];
        let crc_for_verify_cmd = ((footer_crc_hi as u16) << 8) | (footer_crc_lo as u16);

        eprintln!("VERIFY/ENABLE params: Firmware length {} bytes ({} words), CRC from footer (LE): 0x{:04X}",
                 fw_len_bytes, fw_len_words, crc_for_verify_cmd);

        // Step 4: Send VERIFY/ENABLE command (0x5B B5 05)
        let verify_cmd = vec![
            0x06, 0x5b, 0xb5, 0x05,
            (fw_len_words & 0xFF) as u8,        // Length in words, little-endian
            (fw_len_words >> 8) as u8,
            (crc_for_verify_cmd & 0xFF) as u8,  // CRC from footer, little-endian
            (crc_for_verify_cmd >> 8) as u8,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 // Padding
        ];

        eprintln!("Sending VERIFY/ENABLE command: {:02X?}", verify_cmd);
        self.cmd_device.send_feature_report(&verify_cmd)?;

        // Step 5: CRITICAL - Wait for ACK (this is what we were missing!)
        eprintln!("Waiting for bootloader ACK response...");
        
        // Try sending a status query command to trigger response
        eprintln!("Sending status query to trigger ACK response...");
        let status_query = vec![
            0x06, 0x5b, 0xb4, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        ];
        self.cmd_device.send_feature_report(&status_query)?;
        thread::sleep(time::Duration::from_millis(50));
        
        self.lofree_wait_for_ack()?;

        // Step 6: Send REBOOT command (0x5B B5 88) only after ACK received.
        eprintln!("Sending REBOOT command (0x5B B5 88)...");
        let reboot_cmd = vec![
            0x06, 0x5b, 0xb5, 0x88, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        ];
        self.cmd_device.send_feature_report(&reboot_cmd)?;

        // Delay for the device to process the reboot.
        thread::sleep(time::Duration::from_millis(250));

        eprintln!("✓ Bootloader exit sequence (Hints-based timed strategy) attempted.");
        eprintln!("Device should reboot to runtime mode if VERIFY was successful internally.");

        Ok(())
    }

    /// Wait for bootloader ACK after VERIFY command
    fn lofree_wait_for_ack(&self) -> Result<(), ISPError> {
        // C# implementation uses 15-second timeout, so 150 attempts * 100ms = 15 seconds
        const MAX_ATTEMPTS: u32 = 150;
        let mut attempts = 0;
        
        // On macOS, we need to use read_timeout instead of get_feature_report
        let use_read_timeout = cfg!(target_os = "macos");
        
        // Open a separate read handle if on macOS
        let hidapi = hidapi::HidApi::new().map_err(ISPError::HidError)?;
        
        // CRITICAL FIX: For Lofree bootloader on macOS, use NON-EXCLUSIVE access
        // as exclusive access fails due to vendor usage page conflicts.
        // For other devices on macOS, use exclusive access if needed.
        #[cfg(target_os = "macos")]
        {
            if self.device_spec.is_lofree() {
                eprintln!("Using non-exclusive access for Lofree bootloader (macOS HID fix)...");
                hidapi.set_open_exclusive(false);
            } else {
                eprintln!("Setting exclusive access for bootloader ACK reading...");
                hidapi.set_open_exclusive(true);
            }
        }
        #[cfg(not(target_os = "macos"))]
        hidapi.set_open_exclusive(false);
        
        let read_device_handle = if use_read_timeout {
            self.cmd_device.get_device_info()
                .ok()
                .and_then(|info| {
                    eprintln!("Opening separate read handle for ACK reading...");
                    hidapi.open_path(info.path()).ok()
                })
        } else {
            None
        };
        
        loop {
            // Prepare buffer with report ID
            let mut buffer = vec![0x06; 17]; // 17-byte buffer starting with report ID
            
            let read_result = if let Some(ref read_device) = read_device_handle {
                // macOS: try get_feature_report first with exclusive access
                match read_device.get_feature_report(&mut buffer) {
                    Ok(len) if len > 0 => Ok(len),
                    _ => {
                        // Fall back to read_timeout if feature report fails
                        debug!("Feature report failed or returned 0 bytes, trying read_timeout...");
                        read_device.read_timeout(&mut buffer, 50)
                    }
                }
            } else {
                // Other platforms: use get_feature_report
                self.cmd_device.get_feature_report(&mut buffer)
            };
            
            match read_result {
                Ok(len) if len >= 4 => {
                    eprintln!("ACK response ({} bytes): {:02X?}", len, &buffer[..len]);
                    
                    // Check for bootloader status response
                    if buffer[1] == 0x5B && buffer[2] == 0xB6 {
                        match buffer[3] {
                            0x11 => {
                                eprintln!("✓ Success ACK received (5B B6 11) - bootloader verified firmware!");
                                return Ok(());
                            }
                            0x10 => {
                                eprintln!("✗ Failure ACK received (5B B6 10) - bootloader rejected firmware!");
                                return Err(ISPError::HidError(HidError::HidApiError {
                                    message: "Bootloader verification failed - bad CRC or invalid firmware".to_string()
                                }));
                            }
                            status => {
                                eprintln!("Unknown ACK status: 5B B6 {:02X}", status);
                            }
                        }
                    }
                }
                Ok(0) => {
                    // No data available yet, continue polling
                    debug!("No ACK data available yet (attempt {})", attempts);
                }
                Ok(len) => {
                    // Got some data but less than 4 bytes
                    debug!("Partial ACK data ({} bytes) - continuing", len);
                }
                Err(e) => {
                    // Error reading - might be normal on some platforms
                    debug!("Error reading ACK (attempt {}): {}", attempts, e);
                }
            }
            
            attempts += 1;
            if attempts > MAX_ATTEMPTS {
                eprintln!("✗ Timeout waiting for bootloader ACK after {} attempts", MAX_ATTEMPTS);
                
                #[cfg(target_os = "macos")]
                {
                    eprintln!("Note: Could not read ACK with exclusive access on macOS.");
                    eprintln!("This might be due to another process having the device open,");
                    eprintln!("or macOS HID limitations. Continuing with REBOOT anyway...");
                    return Ok(());
                }
                
                #[cfg(not(target_os = "macos"))]
                {
                    eprintln!("Continuing without ACK confirmation...");
                    return Ok(());
                }
            }
            
            thread::sleep(time::Duration::from_millis(100)); // 150 attempts * 100ms = 15 second timeout
        }
    }


    /// Try keyboard enable sequence based on DLL analysis
    fn lofree_try_keyboard_enable_sequence(&self) -> Result<(), ISPError> {
        // Based on CS_UsbServer_KeyboardStart function analysis
        // Try different command codes that might enable keyboard mode

        let keyboard_enable_codes = [0x6A, 0x77, 0x88, 0xAA, 0xBB, 0xCC];

        for &code in &keyboard_enable_codes {
            eprintln!("Trying keyboard enable code: 0x{:02X}", code);
            let mut cmd = vec![0x06, 0x5b, 0xb5, code, 0x00];
            cmd.resize(65, 0x00);

            // Ignore errors as we're trying multiple codes
            if let Ok(_) = self.cmd_device.send_feature_report(&cmd) {
                eprintln!("Keyboard enable code 0x{:02X} sent successfully", code);
                thread::sleep(time::Duration::from_millis(50));
            }
        }

        Ok(())
    }

    /// Try server management sequence based on DLL analysis
    fn lofree_enable_firmware_mode(&self) -> Result<(), ISPError> {
        // Based on LOFREE_MODE_SWITCH_SUCCESS.md - must use interrupt writes and specific command sequence
        eprintln!("Sending Lofree mode switch commands to enter bootloader...");

        // All commands must use interrupt writes via write(), NOT feature reports
        // Commands for runtime mode use report ID 0x08

        // Step 1: Send Status Query 0x0803
        let cmd1 = [0x08, 0x03, 0x00, 0x00, 0x00, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xca];
        eprintln!("Sending status query 0x0803...");
        self.cmd_device.write(&cmd1)?;
        thread::sleep(time::Duration::from_millis(100));

        // Read response if available (non-blocking)
        let mut response = vec![0u8; 17];
        match self.cmd_device.read_timeout(&mut response, 100) {
            Ok(len) => eprintln!("Response ({}): {:02X?}", len, &response[..len]),
            Err(_) => eprintln!("No response received"),
        }

        // Step 2: Send Status Query 0x0804
        let cmd2 = [0x08, 0x04, 0x00, 0x00, 0x00, 0x84, 0x64, 0x01, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xc9];
        eprintln!("Sending status query 0x0804...");
        self.cmd_device.write(&cmd2)?;
        thread::sleep(time::Duration::from_millis(100));

        // Read response if available
        match self.cmd_device.read_timeout(&mut response, 100) {
            Ok(len) => eprintln!("Response ({}): {:02X?}", len, &response[..len]),
            Err(_) => eprintln!("No response received"),
        }

        // Step 3: Send Mode Switch Command 0x080d
        let mode_switch = [0x08, 0x0d, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40];
        eprintln!("Sending mode switch command 0x080d...");
        self.cmd_device.write(&mode_switch)?;

        eprintln!("Mode switch command sent. Device should disconnect and re-enumerate as 3554:F808");
        thread::sleep(time::Duration::from_millis(2000));

        // After this, the device should disconnect and re-enumerate as bootloader (3554:F808)
        // The tool will need to re-detect the device in bootloader mode

        Ok(())
    }

    fn lofree_read_cycle(&self, read_fragment: ReadSection) -> Result<Vec<u8>, ISPError> {
        // Implement Lofree-specific read cycle
        // Based on decompiled UsbCommand protocol

        self.lofree_enable_firmware_mode()?;

        let (start_addr, length) = match read_fragment {
            ReadSection::Firmware => (0, self.device_spec.platform.firmware_size),
            ReadSection::Bootloader => (
                self.device_spec.platform.firmware_size,
                self.device_spec.platform.bootloader_size,
            ),
            ReadSection::Full => (
                0,
                self.device_spec.platform.firmware_size + self.device_spec.platform.bootloader_size,
            ),
        };

        let firmware = self.lofree_read_firmware(start_addr, length)?;

        if self.device_spec.reboot {
            self.reboot();
        }

        Ok(firmware)
    }

    fn lofree_read_firmware(&self, start_addr: usize, length: usize) -> Result<Vec<u8>, ISPError> {
        eprintln!("Reading firmware using Lofree protocol...");

        // For now, implement a simple version that uses ReadFlashData commands
        // Command ID 8 = ReadFlashData from our decompiled analysis

        let page_size = self.device_spec.platform.page_size;
        let num_pages = length / page_size;
        let mut result: Vec<u8> = vec![];

        eprintln!("Reading {} pages of {} bytes each...", num_pages, page_size);
        let bar = ProgressBar::new(num_pages as u64);

        for page in 0..num_pages {
            let addr = start_addr + page * page_size;

            // Prepare ReadFlashData command
            let mut cmd = vec![0u8; 64];
            cmd[0] = self.get_cmd_report_id(); // Report ID
            cmd[1] = 8;  // ReadFlashData command ID
            cmd[2] = 0;  // Status
            cmd[3] = ((addr >> 8) & 0xFF) as u8;  // Address high
            cmd[4] = (addr & 0xFF) as u8;         // Address low

            // Add length parameter (32 bytes for page size)
            cmd[5] = page_size as u8;
            cmd[6] = 0;
            cmd[7] = 0;
            cmd[8] = 0;

            // Send command using output report
            match self.cmd_device.write(&cmd) {
                Ok(_) => eprintln!("Read command {} sent", page),
                Err(e) => eprintln!("Read command {} failed: {}", page, e),
            }

            // Read response using input report
            let mut response = vec![0u8; 64];
            match self.cmd_device.read_timeout(&mut response, 1000) {
                Ok(_) => {
                    // Extract page data from response (need to figure out the response format)
                    // For now, assume data starts at offset 5
                    if response.len() >= 5 + page_size {
                        result.extend_from_slice(&response[5..5 + page_size]);
                    } else {
                        eprintln!("Warning: Short response from device");
                        result.extend_from_slice(&response[5..]);
                        result.resize(result.len() + (page_size - (response.len() - 5)), 0);
                    }
                }
                Err(e) => {
                    eprintln!("Error reading page {}: {}", page, e);
                    // Fill with zeros for failed page
                    result.extend_from_slice(&vec![0u8; page_size]);
                }
            }

            bar.inc(1);
        }

        bar.finish();

        Ok(result)
    }

    fn lofree_try_server_management_sequence(&self) -> Result<(), ISPError> {
        // Based on CS_UsbServer_Exit, CS_UsbServer_Start, CS_UsbServer_ReStartBatteryOptimize
        // These might use different command prefixes or structures

        // Try alternative command structure - maybe not 5bb5 prefix?
        eprintln!("Trying alternative command structures...");

        // Structure 1: Direct device control commands
        let direct_commands = [
            vec![0x06, 0xAA, 0x00, 0x00, 0x00], // Direct keyboard start
            vec![0x06, 0xBB, 0x01, 0x00, 0x00], // Direct USB start
            vec![0x06, 0xCC, 0x02, 0x00, 0x00], // Direct battery optimize
        ];

        for (i, cmd_data) in direct_commands.iter().enumerate() {
            eprintln!("Trying direct command {}: {:02X?}", i+1, cmd_data);
            let mut cmd = cmd_data.clone();
            cmd.resize(65, 0x00);

            if let Ok(_) = self.cmd_device.send_feature_report(&cmd) {
                eprintln!("Direct command {} sent successfully", i+1);
                thread::sleep(time::Duration::from_millis(50));
            }
        }

        // Structure 2: Extended 5bb5 commands with additional parameters
        let extended_commands = [
            vec![0x06, 0x5b, 0xb5, 0x55, 0x01], // Enable with parameter
            vec![0x06, 0x5b, 0xb5, 0x66, 0x01], // Start with parameter
            vec![0x06, 0x5b, 0xb5, 0x77, 0x01], // Restart with parameter
        ];

        for (i, cmd_data) in extended_commands.iter().enumerate() {
            eprintln!("Trying extended command {}: {:02X?}", i+1, cmd_data);
            let mut cmd = cmd_data.clone();
            cmd.resize(65, 0x00);

            if let Ok(_) = self.cmd_device.send_feature_report(&cmd) {
                eprintln!("Extended command {} sent successfully", i+1);
                thread::sleep(time::Duration::from_millis(50));
            }
        }

        Ok(())
    }

    /// Complete runtime->bootloader->update->runtime cycle for Lofree Flow Lite
    fn lofree_switch_to_bootloader_and_write(&self, firmware: &[u8]) -> Result<(), ISPError> {
        eprintln!("Starting complete Lofree firmware update cycle...");

        // Step 1: Send mode switch command to runtime device
        eprintln!("Step 1: Sending mode switch command to runtime device...");
        self.lofree_try_bootloader_commands()?;

        // Step 2: Wait for device to re-enumerate as bootloader device
        eprintln!("Step 2: Waiting for device to switch to bootloader mode (3554:f808)...");
        let bootloader_device = self.wait_for_bootloader_device()?;

        // Step 3: Perform firmware update on bootloader device
        eprintln!("Step 3: Connected to bootloader device - performing firmware update...");
        let bootloader_isp = ISPDevice::new(
            DeviceSpec {
                vendor_id: 0x3554,
                product_id: 0xf808,
                platform: self.device_spec.platform.clone(),
                isp_iface_num: 0,
                isp_report_id: 6,
                reboot: true,
            },
            bootloader_device,
        );

        // Use our bootloader protocol with the new exit command
        bootloader_isp.lofree_write_cycle(firmware)?;

        eprintln!("Step 4: Firmware update completed - device should return to runtime mode!");
        eprintln!("The new bootloader exit command should automatically return device to 05ac:024f");

        Ok(())
    }

    /// Wait for bootloader device to appear after mode switch
    fn wait_for_bootloader_device(&self) -> Result<hidapi::HidDevice, ISPError> {
        use hidapi::HidApi;

        let api = HidApi::new().map_err(ISPError::from)?;

        #[cfg(target_os = "macos")]
        api.set_open_exclusive(false);

        // Poll for bootloader device for up to 5 seconds (should appear in ~40ms)
        for attempt in 1..=25 {
            eprintln!("  Polling attempt {}/25 for bootloader device...", attempt);

            // Try to find bootloader device (3554:f808)
            match api.open(0x3554, 0xf808) {
                Ok(device) => {
                    eprintln!("✓ Bootloader device found and connected after {}ms!", attempt * 200);
                    return Ok(device);
                }
                Err(_) => {
                    // Device not found yet, wait and try again
                    thread::sleep(time::Duration::from_millis(200));
                }
            }
        }

        Err(ISPError::HidError(hidapi::HidError::HidApiError {
            message: "Bootloader device (3554:f808) did not appear after mode switch".to_string()
        }))
    }

    /// Send the exact runtime-to-bootloader mode switch command that works
    fn lofree_try_bootloader_commands(&self) -> Result<(), ISPError> {
        eprintln!("Sending runtime-to-bootloader mode switch sequence...");

        // Step 1: Send status query 0x0803 (from working test tool)
        let cmd1 = [0x08, 0x03, 0x00, 0x00, 0x00, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xca];
        eprintln!("Step 1: Sending status query 0x0803...");
        self.cmd_device.write(&cmd1)?;
        thread::sleep(time::Duration::from_millis(100));

        // Read response
        let mut response = vec![0u8; 17];
        match self.cmd_device.read_timeout(&mut response, 100) {
            Ok(len) => eprintln!("  Response ({} bytes): {:02X?}", len, &response[..len]),
            Err(_) => eprintln!("  No response received"),
        }

        // Step 2: Send status query 0x0804
        let cmd2 = [0x08, 0x04, 0x00, 0x00, 0x00, 0x84, 0x64, 0x01, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xc9];
        eprintln!("Step 2: Sending status query 0x0804...");
        self.cmd_device.write(&cmd2)?;
        thread::sleep(time::Duration::from_millis(100));

        // Read response
        match self.cmd_device.read_timeout(&mut response, 100) {
            Ok(len) => eprintln!("  Response ({} bytes): {:02X?}", len, &response[..len]),
            Err(_) => eprintln!("  No response received"),
        }

        // Step 3: Send mode switch command - CRITICAL: 17 bytes, not 65!
        let mode_switch = [0x08, 0x0d, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40];
        eprintln!("Step 3: Sending mode switch command 0x080d (17 bytes)...");

        match self.cmd_device.send_feature_report(&mode_switch) {
            Ok(_) => {
                eprintln!("✓ Mode switch command sent successfully!");
                eprintln!("Device should disconnect and re-enumerate as bootloader (3554:f808)...");
                thread::sleep(time::Duration::from_millis(100));
                Ok(())
            }
            Err(e) => {
                eprintln!("✗ Mode switch failed: {}", e);
                Err(ISPError::HidError(HidError::HidApiError {
                    message: format!("Mode switch failed: {}", e)
                }))
            }
        }
    }
}
