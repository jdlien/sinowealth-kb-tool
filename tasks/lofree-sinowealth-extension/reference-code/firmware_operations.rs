// Lofree Flow Lite Firmware Operations
// Ported from UsbUpgradeFile.cs, LiteResources.cs, and related firmware handling

use crate::firmware_format::{UpgradeFileHeader, DeviceInfo, VidPid};
use crate::usb_protocol::{UsbCommand, UsbCommandID};
use std::io::{Read, Write};
use std::fs::File;

/// Firmware file validation and parsing
pub struct FirmwareFile {
    pub header: UpgradeFileHeader,
    pub firmware_data: Vec<u8>,
    pub raw_data: Vec<u8>,
}

impl FirmwareFile {
    /// Load and parse a firmware file
    pub fn load_from_file(path: &str) -> Result<Self, Box<dyn std::error::Error>> {
        let mut file = File::open(path)?;
        let mut raw_data = Vec::new();
        file.read_to_end(&mut raw_data)?;
        
        Self::from_bytes(raw_data)
    }
    
    /// Parse firmware from raw bytes
    pub fn from_bytes(raw_data: Vec<u8>) -> Result<Self, Box<dyn std::error::Error>> {
        if raw_data.len() < UpgradeFileHeader::HEADER_SIZE {
            return Err("File too small to contain valid header".into());
        }
        
        // Parse header
        let header = UpgradeFileHeader::from_bytes(&raw_data[..UpgradeFileHeader::HEADER_SIZE])?;
        
        // Validate header CRC
        if !Self::validate_header_crc(&header, &raw_data[..UpgradeFileHeader::HEADER_SIZE]) {
            return Err("Header CRC validation failed".into());
        }
        
        // Extract firmware data
        let fw_start = UpgradeFileHeader::HEADER_SIZE;
        let fw_end = fw_start + header.fw_length as usize;
        
        if raw_data.len() < fw_end {
            return Err("File too small for declared firmware size".into());
        }
        
        let firmware_data = raw_data[fw_start..fw_end].to_vec();
        
        Ok(Self {
            header,
            firmware_data,
            raw_data,
        })
    }
    
    /// Validate header CRC32
    fn validate_header_crc(header: &UpgradeFileHeader, header_bytes: &[u8]) -> bool {
        // Calculate CRC32 of header (excluding the CRC field itself)
        let mut crc_data = header_bytes.to_vec();
        
        // Zero out the CRC field (first 4 bytes) for calculation
        crc_data[0..4].fill(0);
        
        let calculated_crc = crc32fast::hash(&crc_data);
        calculated_crc == header.head_crc
    }
    
    /// Check if this firmware is compatible with the given device
    pub fn is_compatible_with_device(&self, device_info: &DeviceInfo, vid_pid: &VidPid) -> bool {
        // Check device type, CID, MID
        if self.header.device_type != device_info.device_type ||
           self.header.cid != device_info.cid ||
           self.header.mid != device_info.mid {
            return false;
        }
        
        // Additional VID/PID checks could go here
        // For now, assume compatibility if basic device info matches
        true
    }
    
    /// Get firmware version as string
    pub fn version_string(&self) -> String {
        let version = self.header.version;
        let major = (version >> 24) & 0xFF;
        let minor = (version >> 16) & 0xFF;
        let patch = (version >> 8) & 0xFF;
        let build = version & 0xFF;
        
        format!("V{}.{:02}_{:02}{:02}", major, minor, patch, build)
    }
    
    /// Extract embedded firmware from upgrade package
    /// Based on CS_SplitFromUpgradeFile functionality
    pub fn extract_firmware_only(&self) -> Vec<u8> {
        self.firmware_data.clone()
    }
    
    /// Create a new upgrade file with custom firmware
    /// Based on CS_CreateUpgradeFile functionality
    pub fn create_upgrade_file(
        ic_name: &str,
        firmware_data: &[u8],
        device_info: DeviceInfo,
        version: u32
    ) -> Result<Self, Box<dyn std::error::Error>> {
        let mut header = UpgradeFileHeader::new();
        
        // Set basic info
        header.fw_length = firmware_data.len() as u32;
        header.version = version;
        header.device_type = device_info.device_type;
        header.cid = device_info.cid;
        header.mid = device_info.mid;
        
        // Set IC name
        UpgradeFileHeader::set_string_field(&mut header.ic_name, ic_name);
        
        // Set default endpoints for Lofree Flow Lite
        let endpoints = crate::usb_protocol::DeviceEndpoints::lofree_defaults();
        UpgradeFileHeader::set_string_field(&mut header.normal_input_endpoint, &endpoints.normal_input);
        UpgradeFileHeader::set_string_field(&mut header.normal_output_endpoint, &endpoints.normal_output);
        UpgradeFileHeader::set_string_field(&mut header.boot_input_endpoint, &endpoints.boot_input);
        UpgradeFileHeader::set_string_field(&mut header.boot_output_endpoint, &endpoints.boot_output);
        
        // Set default commands (these would need to be customized for Lofree)
        UpgradeFileHeader::set_string_field(&mut header.reset_to_update_mode_cmd, "ENTER_UPDATE_MODE");
        UpgradeFileHeader::set_string_field(&mut header.prepare_download_cmd, "PREPARE_DOWNLOAD");
        UpgradeFileHeader::set_string_field(&mut header.data_download_cmd, "DATA_DOWNLOAD");
        
        // Calculate and set header CRC
        let mut header_bytes = header.to_bytes();
        header_bytes[0..4].fill(0); // Zero CRC field for calculation
        header.head_crc = crc32fast::hash(&header_bytes);
        
        // Build complete file
        let mut raw_data = header.to_bytes();
        raw_data.extend_from_slice(firmware_data);
        
        // Pad to alignment if needed
        while raw_data.len() % 512 != 0 {
            raw_data.push(0xFF);
        }
        
        Ok(Self {
            header,
            firmware_data: firmware_data.to_vec(),
            raw_data,
        })
    }
    
    /// Save firmware file to disk
    pub fn save_to_file(&self, path: &str) -> Result<(), Box<dyn std::error::Error>> {
        let mut file = File::create(path)?;
        file.write_all(&self.raw_data)?;
        Ok(())
    }
}

/// Firmware upgrade operations
pub struct FirmwareUpgrader {
    device_vid_pid: VidPid,
    device_info: DeviceInfo,
}

impl FirmwareUpgrader {
    pub fn new(vid_pid: VidPid, device_info: DeviceInfo) -> Self {
        Self {
            device_vid_pid: vid_pid,
            device_info,
        }
    }
    
    /// Read current firmware from device
    /// This would integrate with the HID communication layer
    pub fn read_device_firmware(&self) -> Result<Vec<u8>, Box<dyn std::error::Error>> {
        // This is a placeholder - actual implementation would:
        // 1. Enter upgrade mode
        // 2. Read flash data in chunks
        // 3. Assemble complete firmware
        
        // For now, return error indicating this needs HID implementation
        Err("HID communication layer needed for device operations".into())
    }
    
    /// Write firmware to device
    pub fn write_firmware(&self, firmware: &FirmwareFile) -> Result<(), Box<dyn std::error::Error>> {
        // Validate compatibility first
        if !firmware.is_compatible_with_device(&self.device_info, &self.device_vid_pid) {
            return Err("Firmware not compatible with this device".into());
        }
        
        // This is a placeholder - actual implementation would:
        // 1. Enter upgrade mode
        // 2. Prepare download
        // 3. Write firmware in chunks
        // 4. Verify write
        // 5. Reset device
        
        Err("HID communication layer needed for device operations".into())
    }
    
    /// Enter device upgrade mode
    pub fn enter_upgrade_mode(&self) -> Result<(), Box<dyn std::error::Error>> {
        // Send EnterUsbUpdateMode command
        let _cmd = UsbCommand::enter_update_mode();
        
        // This would be sent via HID
        Err("HID communication layer needed".into())
    }
    
    /// Read device version and info
    pub fn read_device_info(&self) -> Result<(DeviceInfo, u32), Box<dyn std::error::Error>> {
        // Send commands to read CID/MID and version
        let _cid_mid_cmd = UsbCommand::read_cid_mid();
        let _version_cmd = UsbCommand::read_version();
        
        // This would be sent via HID and responses parsed
        Err("HID communication layer needed".into())
    }
}

/// Embedded firmware resources
/// Based on LiteResources.cs functionality
pub struct EmbeddedFirmware {
    pub firmware_data: Vec<u8>,
    pub config_data: String,
}

impl EmbeddedFirmware {
    /// Load embedded firmware (in real implementation, this would be embedded in binary)
    /// For reference, this shows how the original tool loads embedded firmware
    pub fn load_embedded() -> Result<Self, Box<dyn std::error::Error>> {
        // In the original tool, this loads:
        // - MakeUpgradeTool.Files.CompxUpgradeBinFile.bin (firmware)
        // - MakeUpgradeTool.Files.LiteConfig.txt (configuration)
        
        // For reference implementation, we'll show the pattern:
        let firmware_data = include_bytes!("../embedded/CompxUpgradeBinFile.bin").to_vec();
        let config_data = include_str!("../embedded/LiteConfig.txt").to_string();
        
        Ok(Self {
            firmware_data,
            config_data,
        })
    }
    
    /// Parse configuration from the embedded config text
    pub fn parse_config(&self) -> Vec<String> {
        self.config_data
            .lines()
            .map(|line| line.trim().to_string())
            .filter(|line| !line.is_empty() && !line.starts_with('#'))
            .collect()
    }
}

/// Known firmware versions for Lofree Flow Lite
pub mod known_firmware {
    use super::*;
    
    pub struct FirmwareVersion {
        pub version: u32,
        pub version_string: String,
        pub vid: u16,
        pub pid: u16,
        pub filename: String,
    }
    
    /// Known firmware versions from the original tool
    pub const KNOWN_VERSIONS: &[FirmwareVersion] = &[
        FirmwareVersion {
            version: 0x01540104,
            version_string: "V1.54".to_string(),
            vid: 0x3554,
            pid: 0xF811,
            filename: "KB_RGB_V1.54_VID-3554_PID-F811_0104.bin".to_string(),
        },
        FirmwareVersion {
            version: 0x01550103,
            version_string: "V1.55".to_string(),
            vid: 0x05AC,
            pid: 0x024F,
            filename: "KB_RGB_V1.55_VID-05AC_PID-024F_0103.bin".to_string(),
        },
        FirmwareVersion {
            version: 0x01550105,
            version_string: "V1.55".to_string(),
            vid: 0x05AC,
            pid: 0x024F,
            filename: "KB_RGB_V1.55_VID-05AC_PID-024F_0105.bin".to_string(),
        },
        FirmwareVersion {
            version: 0x01560106,
            version_string: "V1.56".to_string(),
            vid: 0x3554,
            pid: 0xF811,
            filename: "KB_RGB_V1.56_VID-3554_PID-F811_0106.bin".to_string(),
        },
        FirmwareVersion {
            version: 0x01640101,
            version_string: "V1.64".to_string(),
            vid: 0x05AC,
            pid: 0x024F,
            filename: "KB_RGB_V1.64_VID-05AC_PID-024F_0101_8EEBFBEA_20241122.bin".to_string(),
        },
        FirmwareVersion {
            version: 0x01660102,
            version_string: "V1.66".to_string(),
            vid: 0x05AC,
            pid: 0x024F,
            filename: "KB_RGB_V1.66_VID-05AC_PID-024F_0102_6C54C39C_20241122.bin".to_string(),
        },
    ];
}

#[cfg(test)]
mod tests {
    use super::*;
    
    #[test]
    fn test_firmware_version_parsing() {
        let file = FirmwareFile {
            header: UpgradeFileHeader {
                version: 0x01540104,
                ..UpgradeFileHeader::new()
            },
            firmware_data: vec![],
            raw_data: vec![],
        };
        
        assert_eq!(file.version_string(), "V1.54_0104");
    }
    
    #[test]
    fn test_device_compatibility() {
        let device_info = DeviceInfo {
            cid: 0x12,
            mid: 0x34,
            device_type: 0x01,
        };
        
        let mut header = UpgradeFileHeader::new();
        header.cid = 0x12;
        header.mid = 0x34;
        header.device_type = 0x01;
        
        let file = FirmwareFile {
            header,
            firmware_data: vec![],
            raw_data: vec![],
        };
        
        let vid_pid = VidPid::new(0x3554, 0xF811);
        assert!(file.is_compatible_with_device(&device_info, &vid_pid));
    }
}