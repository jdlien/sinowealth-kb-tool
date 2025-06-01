// Lofree Flow Lite Firmware Format Structures
// Ported from UpgradeFileHeader.cs and related files

use std::mem;

/// Main firmware upgrade file header structure
/// Total size: 720 bytes as used in the original Windows tool
#[repr(C, packed)]
#[derive(Debug, Clone)]
pub struct UpgradeFileHeader {
    pub head_crc: u32,               // CRC32 of the header
    pub head_length: u32,            // Length of the header (should be 720)
    pub fw_length: u32,              // Length of the firmware binary
    pub next_file_address: u32,      // Address of next file (for multi-file upgrades)
    pub version: u32,                // Firmware version
    pub device_type: u8,             // Device type identifier
    pub cid: u8,                     // Company/Customer ID
    pub mid: u8,                     // Module ID
    pub reserved: u8,                // Padding for alignment
    
    // Fixed-size byte arrays (64 bytes each)
    pub file_id: [u8; 64],           // File identifier string
    pub ic_name: [u8; 64],           // IC/Chip name
    pub boot_input_endpoint: [u8; 64],    // Boot mode input endpoint
    pub boot_output_endpoint: [u8; 64],   // Boot mode output endpoint  
    pub normal_input_endpoint: [u8; 64],  // Normal mode input endpoint
    pub normal_output_endpoint: [u8; 64], // Normal mode output endpoint
    pub reset_to_update_mode_cmd: [u8; 64], // Command to enter update mode
    pub prepare_download_cmd: [u8; 64],     // Command to prepare download
    pub data_download_cmd: [u8; 64],        // Command for data download
    pub sensor_name: [u8; 64],              // Sensor name string
    pub product_name: [u8; 64],             // Product name string
}

impl UpgradeFileHeader {
    pub const HEADER_SIZE: usize = 720;
    
    /// Create a new header with default values
    pub fn new() -> Self {
        Self {
            head_crc: 0,
            head_length: Self::HEADER_SIZE as u32,
            fw_length: 0,
            next_file_address: 0,
            version: 0,
            device_type: 0,
            cid: 0,
            mid: 0,
            reserved: 0,
            file_id: [0; 64],
            ic_name: [0; 64],
            boot_input_endpoint: [0; 64],
            boot_output_endpoint: [0; 64],
            normal_input_endpoint: [0; 64],
            normal_output_endpoint: [0; 64],
            reset_to_update_mode_cmd: [0; 64],
            prepare_download_cmd: [0; 64],
            data_download_cmd: [0; 64],
            sensor_name: [0; 64],
            product_name: [0; 64],
        }
    }
    
    /// Parse header from bytes
    pub fn from_bytes(data: &[u8]) -> Result<Self, &'static str> {
        if data.len() < Self::HEADER_SIZE {
            return Err("Data too short for upgrade file header");
        }
        
        // Safety: We've checked the length, and the struct is repr(C, packed)
        unsafe {
            let header = std::ptr::read(data.as_ptr() as *const Self);
            
            // Validate basic fields
            if header.head_length != Self::HEADER_SIZE as u32 {
                return Err("Invalid header length");
            }
            
            Ok(header)
        }
    }
    
    /// Convert header to bytes
    pub fn to_bytes(&self) -> Vec<u8> {
        unsafe {
            let ptr = self as *const Self as *const u8;
            std::slice::from_raw_parts(ptr, Self::HEADER_SIZE).to_vec()
        }
    }
    
    /// Get string from fixed-size byte array
    pub fn get_string_field(field: &[u8; 64]) -> String {
        // Find null terminator or use full length
        let end = field.iter().position(|&b| b == 0).unwrap_or(64);
        String::from_utf8_lossy(&field[..end]).to_string()
    }
    
    /// Set string field (null-padded)
    pub fn set_string_field(field: &mut [u8; 64], value: &str) {
        field.fill(0);
        let bytes = value.as_bytes();
        let len = std::cmp::min(bytes.len(), 63); // Leave space for null terminator
        field[..len].copy_from_slice(&bytes[..len]);
    }
    
    // Getter methods for string fields
    pub fn file_id_str(&self) -> String {
        Self::get_string_field(&self.file_id)
    }
    
    pub fn ic_name_str(&self) -> String {
        Self::get_string_field(&self.ic_name)
    }
    
    pub fn product_name_str(&self) -> String {
        Self::get_string_field(&self.product_name)
    }
    
    pub fn sensor_name_str(&self) -> String {
        Self::get_string_field(&self.sensor_name)
    }
}

/// Device information structure
#[repr(C)]
#[derive(Debug, Clone, Copy)]
pub struct DeviceInfo {
    pub cid: u8,            // Company/Customer ID
    pub mid: u8,            // Module ID  
    pub device_type: u8,    // Device type
}

/// VID/PID pair for device identification
#[derive(Debug, Clone)]
pub struct VidPid {
    pub vid: u16,
    pub pid: u16,
}

impl VidPid {
    pub fn new(vid: u16, pid: u16) -> Self {
        Self { vid, pid }
    }
    
    /// Parse from hex strings (e.g., "3554", "F811")
    pub fn from_hex_strings(vid_str: &str, pid_str: &str) -> Result<Self, std::num::ParseIntError> {
        let vid = u16::from_str_radix(vid_str, 16)?;
        let pid = u16::from_str_radix(pid_str, 16)?;
        Ok(Self::new(vid, pid))
    }
    
    /// Known Lofree Flow Lite VID/PID combinations
    pub fn lofree_combinations() -> Vec<VidPid> {
        vec![
            VidPid::new(0x3554, 0xF811), // Lofree branded
            VidPid::new(0x05AC, 0x024F), // Apple-compatible mode
        ]
    }
}

#[cfg(test)]
mod tests {
    use super::*;
    
    #[test]
    fn test_header_size() {
        assert_eq!(mem::size_of::<UpgradeFileHeader>(), UpgradeFileHeader::HEADER_SIZE);
    }
    
    #[test]
    fn test_string_fields() {
        let mut header = UpgradeFileHeader::new();
        UpgradeFileHeader::set_string_field(&mut header.product_name, "Lofree Flow Lite");
        assert_eq!(header.product_name_str(), "Lofree Flow Lite");
    }
    
    #[test]
    fn test_vidpid() {
        let vid_pid = VidPid::from_hex_strings("3554", "F811").unwrap();
        assert_eq!(vid_pid.vid, 0x3554);
        assert_eq!(vid_pid.pid, 0xF811);
    }
}