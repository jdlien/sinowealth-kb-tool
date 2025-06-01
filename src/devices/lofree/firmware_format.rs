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

impl Default for UpgradeFileHeader {
    fn default() -> Self {
        Self::new()
    }
}

/// Wrapper for Lofree firmware files with header and binary data
#[derive(Debug, Clone)]
pub struct LofreeFirmwareFile {
    pub header: UpgradeFileHeader,
    pub firmware_data: Vec<u8>,
}

impl LofreeFirmwareFile {
    /// Load firmware file from bytes
    pub fn from_bytes(data: &[u8]) -> Result<Self, &'static str> {
        if data.len() < UpgradeFileHeader::HEADER_SIZE {
            return Err("File too small to contain header");
        }
        
        let header = UpgradeFileHeader::from_bytes(&data[..UpgradeFileHeader::HEADER_SIZE])?;
        
        // Extract firmware data
        let firmware_start = UpgradeFileHeader::HEADER_SIZE;
        let firmware_end = firmware_start + header.fw_length as usize;
        
        if data.len() < firmware_end {
            return Err("File too small to contain firmware data");
        }
        
        let firmware_data = data[firmware_start..firmware_end].to_vec();
        
        Ok(Self {
            header,
            firmware_data,
        })
    }
    
    /// Convert to bytes for writing
    pub fn to_bytes(&self) -> Vec<u8> {
        let mut result = Vec::new();
        result.extend_from_slice(&self.header.to_bytes());
        result.extend_from_slice(&self.firmware_data);
        result
    }
    
    /// Get firmware version string
    pub fn version_string(&self) -> String {
        crate::devices::lofree::usb_protocol::upgrade_constants::parse_version_string(self.header.version)
    }
    
    /// Validate CRC32 if present
    pub fn validate_crc(&self) -> bool {
        // TODO: Implement CRC32 validation
        // For now, just check if CRC is non-zero
        self.header.head_crc != 0
    }
}

/// CRC32 validation utilities
pub mod crc32 {
    /// Calculate CRC32 for data
    pub fn calculate(data: &[u8]) -> u32 {
        // Simple CRC32 implementation - replace with crc32fast crate for production
        let mut crc = 0xFFFFFFFF_u32;
        
        for &byte in data {
            crc ^= u32::from(byte);
            for _ in 0..8 {
                if crc & 1 != 0 {
                    crc = (crc >> 1) ^ 0xEDB88320;
                } else {
                    crc >>= 1;
                }
            }
        }
        
        !crc
    }
    
    /// Validate CRC32 for firmware header
    pub fn validate_header(header: &super::UpgradeFileHeader) -> bool {
        let header_bytes = header.to_bytes();
        let calculated_crc = calculate(&header_bytes[4..]); // Skip CRC field itself
        calculated_crc == header.head_crc
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
    fn test_crc32_basic() {
        let data = b"hello";
        let crc = crc32::calculate(data);
        assert_ne!(crc, 0); // Should produce some non-zero CRC
    }
}