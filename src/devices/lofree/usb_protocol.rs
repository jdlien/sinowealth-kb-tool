// Lofree Flow Lite USB Communication Protocol
// Ported from UsbCommand.cs, UsbCommandID.cs, and UsbServer.cs

use std::fmt;

/// USB Command IDs used by the Lofree Flow Lite
/// These match the enum values from UsbCommandID.cs
#[repr(u8)]
#[derive(Debug, Clone, Copy, PartialEq, Eq)]
pub enum UsbCommandID {
    EncryptionData = 1,
    PCDriverStatus = 2,
    DeviceOnLine = 3,
    BatteryLevel = 4,
    DongleEnterPair = 5,
    GetPairState = 6,
    WriteFlashData = 7,
    ReadFlashData = 8,
    ClearSetting = 9,
    StatusChanged = 10,
    SetDeviceVidPid = 11,
    SetDeviceDescriptorString = 12,
    EnterUsbUpdateMode = 13,
    GetCurrentConfig = 14,
    SetCurrentConfig = 15,
    ReadCIDMID = 16,
    EnterMTKMode = 17,
    ReadVersionID = 18,
    Set4KDongleRGB = 20,
    Get4KDongleRGBValue = 21,
    SetLongRangeMode = 22,
    GetLongRangeMode = 23,
    WriteKBCIdMID = 240,
    ReadKBCIdMID = 241,
}

impl From<u8> for UsbCommandID {
    fn from(value: u8) -> Self {
        match value {
            1 => UsbCommandID::EncryptionData,
            2 => UsbCommandID::PCDriverStatus,
            3 => UsbCommandID::DeviceOnLine,
            4 => UsbCommandID::BatteryLevel,
            5 => UsbCommandID::DongleEnterPair,
            6 => UsbCommandID::GetPairState,
            7 => UsbCommandID::WriteFlashData,
            8 => UsbCommandID::ReadFlashData,
            9 => UsbCommandID::ClearSetting,
            10 => UsbCommandID::StatusChanged,
            11 => UsbCommandID::SetDeviceVidPid,
            12 => UsbCommandID::SetDeviceDescriptorString,
            13 => UsbCommandID::EnterUsbUpdateMode,
            14 => UsbCommandID::GetCurrentConfig,
            15 => UsbCommandID::SetCurrentConfig,
            16 => UsbCommandID::ReadCIDMID,
            17 => UsbCommandID::EnterMTKMode,
            18 => UsbCommandID::ReadVersionID,
            20 => UsbCommandID::Set4KDongleRGB,
            21 => UsbCommandID::Get4KDongleRGBValue,
            22 => UsbCommandID::SetLongRangeMode,
            23 => UsbCommandID::GetLongRangeMode,
            240 => UsbCommandID::WriteKBCIdMID,
            241 => UsbCommandID::ReadKBCIdMID,
            _ => UsbCommandID::DeviceOnLine, // Default fallback
        }
    }
}

/// USB Command structure for communication with the device
/// Matches the UsbCommand struct from UsbCommand.cs
#[derive(Debug, Clone)]
pub struct UsbCommand {
    pub report_id: u8,
    pub id: UsbCommandID,
    pub command_status: u8,
    pub address: u32,          // Note: i32 in C# but we use u32 for addresses
    pub command: Vec<u8>,
    pub received_data: Vec<u8>,
}

impl UsbCommand {
    pub const MAX_CMD_LENGTH: usize = 64;
    
    /// Create a new USB command
    pub fn new(id: UsbCommandID) -> Self {
        Self {
            report_id: 0,
            id,
            command_status: 0,
            address: 0,
            command: Vec::new(),
            received_data: Vec::new(),
        }
    }
    
    /// Create command to enter USB update mode
    pub fn enter_update_mode() -> Self {
        Self::new(UsbCommandID::EnterUsbUpdateMode)
    }
    
    /// Create command to read device version
    pub fn read_version() -> Self {
        Self::new(UsbCommandID::ReadVersionID)
    }
    
    /// Create command to read CID/MID
    pub fn read_cid_mid() -> Self {
        Self::new(UsbCommandID::ReadCIDMID)
    }
    
    /// Create command to read flash data
    pub fn read_flash_data(start_address: u32, length: u32) -> Self {
        let mut cmd = Self::new(UsbCommandID::ReadFlashData);
        cmd.address = start_address;
        
        // Pack length into command data (following the original protocol)
        cmd.command = length.to_le_bytes().to_vec();
        
        cmd
    }
    
    /// Create command to write flash data
    pub fn write_flash_data(address: u32, data: &[u8]) -> Self {
        let mut cmd = Self::new(UsbCommandID::WriteFlashData);
        cmd.address = address;
        cmd.command = data.to_vec();
        cmd
    }
    
    /// Serialize command to bytes for HID transmission
    /// Based on the UserUsbDataReceived function in UsbServer.cs
    pub fn to_bytes(&self) -> Vec<u8> {
        let mut bytes = Vec::new();
        
        bytes.push(self.report_id);
        bytes.push(self.id as u8);
        bytes.push(self.command_status);
        
        // Address as 2 bytes (big-endian based on original code)
        bytes.push((self.address >> 8) as u8);
        bytes.push(self.address as u8);
        
        // Add command data
        bytes.extend_from_slice(&self.command);
        
        // Pad to expected length if needed
        while bytes.len() < Self::MAX_CMD_LENGTH {
            bytes.push(0);
        }
        
        bytes
    }
    
    /// Parse command from received bytes
    pub fn from_bytes(data: &[u8]) -> Result<Self, &'static str> {
        if data.len() < 5 {
            return Err("Command data too short");
        }
        
        let report_id = data[0];
        let id = UsbCommandID::from(data[1]);
        let command_status = data[2];
        
        // Address from bytes 3-4 (big-endian)
        let address = ((data[3] as u32) << 8) | (data[4] as u32);
        
        let command = if data.len() > 5 {
            data[5..].to_vec()
        } else {
            Vec::new()
        };
        
        Ok(Self {
            report_id,
            id,
            command_status,
            address,
            command,
            received_data: Vec::new(),
        })
    }
}

/// Command status codes
#[repr(u8)]
#[derive(Debug, Clone, Copy, PartialEq, Eq)]
pub enum CommandStatus {
    Success = 0,
    Error = 1,
    Busy = 2,
    InvalidCommand = 3,
    InvalidParameter = 4,
    DeviceNotReady = 5,
}

impl From<u8> for CommandStatus {
    fn from(value: u8) -> Self {
        match value {
            0 => CommandStatus::Success,
            1 => CommandStatus::Error,
            2 => CommandStatus::Busy,
            3 => CommandStatus::InvalidCommand,
            4 => CommandStatus::InvalidParameter,
            5 => CommandStatus::DeviceNotReady,
            _ => CommandStatus::Error,
        }
    }
}

/// Device endpoints for different modes
/// Based on the endpoint arrays in UpgradeFileHeader
#[derive(Debug, Clone)]
pub struct DeviceEndpoints {
    pub boot_input: String,
    pub boot_output: String,
    pub normal_input: String,
    pub normal_output: String,
}

impl DeviceEndpoints {
    /// Default endpoints for Lofree Flow Lite
    pub fn lofree_defaults() -> Self {
        Self {
            boot_input: "HID\\VID_3554&PID_F811&MI_01\\HIDUSAGE_01".to_string(),
            boot_output: "HID\\VID_3554&PID_F811&MI_01\\HIDUSAGE_02".to_string(),
            normal_input: "HID\\VID_3554&PID_F811&MI_00\\HIDUSAGE_01".to_string(),
            normal_output: "HID\\VID_3554&PID_F811&MI_00\\HIDUSAGE_02".to_string(),
        }
    }
    
    /// Apple-compatible mode endpoints
    pub fn apple_mode() -> Self {
        Self {
            boot_input: "HID\\VID_05AC&PID_024F&MI_01\\HIDUSAGE_01".to_string(),
            boot_output: "HID\\VID_05AC&PID_024F&MI_01\\HIDUSAGE_02".to_string(),
            normal_input: "HID\\VID_05AC&PID_024F&MI_00\\HIDUSAGE_01".to_string(),
            normal_output: "HID\\VID_05AC&PID_024F&MI_00\\HIDUSAGE_02".to_string(),
        }
    }
    
    /// Bootloader mode endpoints
    pub fn bootloader_mode() -> Self {
        Self {
            boot_input: "HID\\VID_3554&PID_F808&MI_00\\HIDUSAGE_01".to_string(),
            boot_output: "HID\\VID_3554&PID_F808&MI_00\\HIDUSAGE_02".to_string(),
            normal_input: "HID\\VID_3554&PID_F808&MI_00\\HIDUSAGE_01".to_string(),
            normal_output: "HID\\VID_3554&PID_F808&MI_00\\HIDUSAGE_02".to_string(),
        }
    }
}

/// Firmware upgrade constants
pub mod upgrade_constants {
    pub const BOOT_SIZE: usize = 8192;
    pub const MAX_COMMAND_LENGTH: usize = 64;
    
    /// Firmware version parsing
    /// Lofree versions use BCD format: 0x0166 = V1.66
    pub fn parse_version_string(version: u32) -> String {
        if version <= 0xFFFF {
            // BCD format: 0x0166 = V1.66
            let major = (version >> 8) & 0xFF;
            let minor = version & 0xFF;
            // Convert BCD to decimal
            let major_bcd = (major >> 4) * 10 + (major & 0x0F);
            let minor_bcd = (minor >> 4) * 10 + (minor & 0x0F);
            format!("V{}.{:02}", major_bcd, minor_bcd)
        } else {
            // Original byte-packed format fallback
            let major = (version >> 24) & 0xFF;
            let minor = (version >> 16) & 0xFF;
            let patch = (version >> 8) & 0xFF;
            let build = version & 0xFF;
            
            format!("V{}.{:02}_{:02}{:02}", major, minor, patch, build)
        }
    }
    
    /// Common firmware versions for Lofree Flow Lite
    pub const KNOWN_VERSIONS: &[u32] = &[
        0x01540104, // V1.54_0104
        0x01550103, // V1.55_0103  
        0x01550105, // V1.55_0105
        0x01560106, // V1.56_0106
        0x01640101, // V1.64_0101
        0x01660102, // V1.66_0102
    ];
}

impl fmt::Display for UsbCommand {
    fn fmt(&self, f: &mut fmt::Formatter<'_>) -> fmt::Result {
        write!(
            f,
            "UsbCommand {{ id: {:?}, status: {}, addr: 0x{:04X}, cmd_len: {}, data_len: {} }}",
            self.id,
            self.command_status,
            self.address,
            self.command.len(),
            self.received_data.len()
        )
    }
}

#[cfg(test)]
mod tests {
    use super::*;
    
    #[test]
    fn test_command_serialization() {
        let cmd = UsbCommand::read_flash_data(0x1000, 256);
        let bytes = cmd.to_bytes();
        
        assert_eq!(bytes[0], cmd.report_id);
        assert_eq!(bytes[1], UsbCommandID::ReadFlashData as u8);
        assert_eq!(bytes[2], cmd.command_status);
        
        // Check address bytes
        assert_eq!(bytes[3], 0x10);
        assert_eq!(bytes[4], 0x00);
    }
    
    #[test]
    fn test_command_deserialization() {
        let data = vec![0, UsbCommandID::ReadVersionID as u8, 0, 0x10, 0x00];
        let cmd = UsbCommand::from_bytes(&data).unwrap();
        
        assert_eq!(cmd.id, UsbCommandID::ReadVersionID);
        assert_eq!(cmd.address, 0x1000);
    }
    
    #[test]
    fn test_version_parsing() {
        let version_str = upgrade_constants::parse_version_string(0x01540104);
        assert_eq!(version_str, "V1.54_0104");
    }
}