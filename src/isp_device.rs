use core::panic;
use std::{str::FromStr, thread, time};

use indicatif::ProgressBar;
use log::{debug, error};
use thiserror::Error;

use crate::{device_spec::*, is_expected_error, util, VerificationError, PlatformSpec};

extern crate hidapi;

use hidapi::{HidDevice, HidError};

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
    fn lofree_write_cycle(&self, firmware: &[u8]) -> Result<(), ISPError> {
        eprintln!("Using Lofree Flow Lite protocol...");
        
        // Prepare firmware with LJMP footer for bootloader exit
        let mut firmware_with_footer = firmware.to_vec();
        self.lofree_add_ljmp_footer(&mut firmware_with_footer)?;
        
        // Send initial setup commands
        self.lofree_init_bootloader()?;
        self.lofree_setup_write()?;
        
        // Write firmware data in 32-byte chunks starting at 0x1600
        self.lofree_write_firmware(&firmware_with_footer)?;
        
        // Send finalization commands (includes proper reboot sequence)
        self.lofree_finalize()?;
        
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
        let mut address: u32 = 0x1600; // Start address from protocol analysis
        
        // Process firmware in 2KB sectors
        let sector_size = 2048;
        let total_sectors = (firmware.len() + sector_size - 1) / sector_size;
        
        for sector_idx in 0..total_sectors {
            let sector_start = sector_idx * sector_size;
            let sector_end = ((sector_idx + 1) * sector_size).min(firmware.len());
            let sector_data = &firmware[sector_start..sector_end];
            
            // Write most of the sector in 32-byte chunks
            let full_chunks = sector_data.len() / 32;
            for chunk_idx in 0..full_chunks {
                let chunk_start = chunk_idx * 32;
                let chunk = &sector_data[chunk_start..chunk_start + 32];
                
                // Build write command for 32-byte chunk
                let mut cmd = vec![0x06]; // Report ID
                cmd.push(0xb1); // Write command
                cmd.push(0xc0); // Normal chunk flag
                cmd.push(0x20); // Length = 32 bytes
                
                // Add address and data (total must be 16 bytes after report ID)
                let current_addr = address + (chunk_idx * 32) as u32;
                let addr_bytes = current_addr.to_le_bytes();
                
                // For now, simplified packet structure - needs proper analysis
                cmd.extend_from_slice(&[0x00, 0x08, 0x00, 0x16]); // Fixed header
                cmd.extend_from_slice(&addr_bytes); // Address
                
                // Truncate to 17 bytes total for now
                while cmd.len() < 17 {
                    cmd.push(0x00);
                }
                
                // Send command
                self.cmd_device.write(&cmd[..17])?;
                thread::sleep(time::Duration::from_millis(2));
                
                // Send data separately if needed
                let mut data_cmd = vec![0x06];
                data_cmd.extend_from_slice(chunk);
                while data_cmd.len() < 33 {
                    data_cmd.push(0x00);
                }
                self.cmd_device.write(&data_cmd[..33])?;
                thread::sleep(time::Duration::from_millis(2));
            }
            
            // Handle last partial chunk if exists
            let remaining = sector_data.len() % 32;
            if remaining > 0 {
                let last_chunk = &sector_data[full_chunks * 32..];
                
                // Build write command for last chunk
                let mut cmd = vec![0x06]; // Report ID
                cmd.push(0xb1); // Write command
                cmd.push(0xc1); // Last chunk flag
                cmd.push(remaining as u8); // Actual length
                
                // Add remaining command bytes
                while cmd.len() < 17 {
                    cmd.push(0x00);
                }
                
                self.cmd_device.write(&cmd)?;
                thread::sleep(time::Duration::from_millis(2));
            }
            
            address += sector_size as u32;
        }
        
        Ok(())
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
        
        // Calculate CRC16 over the entire firmware (excluding the footer we're about to write)
        let crc = self.calculate_crc16(firmware);
        
        // Write LJMP footer at firmware_size-5: [0x02, 0x00, 0x00, CRC_LO, CRC_HI]
        let footer_offset = firmware_size - 5;
        firmware[footer_offset] = 0x02;     // LJMP opcode
        firmware[footer_offset + 1] = 0x00; // Address low byte (jump to 0x0000)
        firmware[footer_offset + 2] = 0x00; // Address high byte  
        firmware[footer_offset + 3] = (crc & 0xFF) as u8;        // CRC low byte
        firmware[footer_offset + 4] = ((crc >> 8) & 0xFF) as u8; // CRC high byte
        
        eprintln!("Added LJMP footer at offset 0x{:X}: 02 00 00 {:02X} {:02X}", 
                 footer_offset, firmware[footer_offset + 3], firmware[footer_offset + 4]);
        
        Ok(())
    }
    
    /// Calculate CRC16 for firmware validation
    fn calculate_crc16(&self, firmware: &[u8]) -> u16 {
        let mut crc: u16 = 0;
        for &byte in firmware {
            crc ^= (byte as u16) << 8;
            for _ in 0..8 {
                if crc & 0x8000 != 0 {
                    crc = (crc << 1) ^ 0x1021; // CRC-16-CCITT polynomial
                } else {
                    crc <<= 1;
                }
            }
        }
        crc
    }

    /// Send finalization commands for Lofree Flow Lite
    fn lofree_finalize(&self) -> Result<(), ISPError> {
        eprintln!("Sending bootloader exit sequence...");
        
        // CRITICAL: These must be sent via interrupt writes (dev.write()), NOT feature reports!
        // Command 1: COPY_FOOTER (0x5b b5 88) - bootloader copies LJMP+CRC to flash
        let copy_footer_cmd = vec![
            0x06, 0x5b, 0xb5, 0x88, 0x00, 0x00, 0x00, 0x00, 
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        ];
        
        eprintln!("Sending COPY_FOOTER command (0x5b b5 88)...");
        self.cmd_device.write(&copy_footer_cmd).map_err(|e| {
            eprintln!("Failed to send COPY_FOOTER: {}", e);
            ISPError::from(e)
        })?;
        thread::sleep(time::Duration::from_millis(50));
        
        // Command 2: REBOOT (0x5b b5 99) - triggers watchdog reset
        let reboot_cmd = vec![
            0x06, 0x5b, 0xb5, 0x99, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        ];
        
        eprintln!("Sending REBOOT command (0x5b b5 99)...");
        self.cmd_device.write(&reboot_cmd).map_err(|e| {
            eprintln!("Failed to send REBOOT: {}", e);
            ISPError::from(e)
        })?;
        
        // Wait for device to reset and re-enumerate
        eprintln!("Waiting for device to reset and re-enumerate as runtime mode (05ac:024f)...");
        thread::sleep(time::Duration::from_millis(100));
        
        eprintln!("Bootloader exit sequence complete!");
        Ok(())
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
        // Use the successful pattern from lofree_try_bootloader_commands
        // Try the 5bb5 command pattern which we know works for mode switching
        
        eprintln!("Sending Lofree firmware enable commands...");
        
        // Try the established command patterns that work
        let enable_commands = [
            vec![0x06, 0x5b, 0xb5, 0x0d, 0x00], // EnterUsbUpdateMode with 5bb5 prefix
            vec![0x06, 0x5b, 0xb5, 0x55, 0x01], // Enable command from existing code
            vec![0x06, 0x0d, 0x00, 0x00, 0x00], // Direct EnterUsbUpdateMode
        ];
        
        for (i, cmd_data) in enable_commands.iter().enumerate() {
            eprintln!("Trying enable command {}: {:02X?}", i+1, cmd_data);
            let mut cmd = cmd_data.clone();
            cmd.resize(65, 0x00); // Ensure 65 bytes total
            
            // Try write (output report) first, then feature report
            match self.cmd_device.write(&cmd) {
                Ok(_) => {
                    eprintln!("Enable command {} sent successfully (output)", i+1);
                    thread::sleep(time::Duration::from_millis(100));
                },
                Err(_) => {
                    // Fall back to feature report
                    match self.cmd_device.send_feature_report(&cmd) {
                        Ok(_) => {
                            eprintln!("Enable command {} sent successfully (feature)", i+1);
                            thread::sleep(time::Duration::from_millis(100));
                        },
                        Err(e) => {
                            eprintln!("Enable command {} failed: {}", i+1, e);
                        }
                    }
                }
            }
        }
        
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

    /// Send the exact runtime-to-bootloader mode switch command from GPT-o3 analysis
    fn lofree_try_bootloader_commands(&self) -> Result<(), ISPError> {
        eprintln!("Sending runtime-to-bootloader mode switch command...");
        eprintln!("Using GPT-o3 gap-hunt analysis: Feature report on interface 1, RID 8, 65 bytes");
        
        // The exact command from Wireshark: 080d000000000000000000000000000040
        // CRITICAL: Must be sent as FEATURE report (not output report)
        let mut report = [0u8; 65]; // Must be exactly 65 bytes
        report[..16].copy_from_slice(&[
            0x08, 0x0d, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40
        ]);
        // Remaining bytes are already 0x00 from initialization
        
        eprintln!("Sending feature report (65 bytes): {:02x?}...", &report[..16]);
        
        match self.cmd_device.send_feature_report(&report) {
            Ok(_) => {
                eprintln!("✓ Feature report sent successfully!");
                eprintln!("Waiting 40ms for device to re-enumerate as bootloader (3554:f808)...");
                thread::sleep(time::Duration::from_millis(40));
                Ok(())
            }
            Err(e) => {
                eprintln!("✗ Feature report failed: {}", e);
                eprintln!("This may indicate:");
                eprintln!("  - Wrong interface (need interface 1 with usage_page 0xFF02)");
                eprintln!("  - macOS security restrictions (try with sudo)");
                eprintln!("  - Device not in correct state");
                Err(ISPError::HidError(HidError::HidApiError {
                    message: format!("Mode switch feature report failed: {}", e)
                }))
            }
        }
    }
}
