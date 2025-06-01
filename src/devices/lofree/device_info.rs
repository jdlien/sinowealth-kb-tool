// Lofree Flow Lite Device Information
// Ported from DeviceInfo.cs and VidPid structures

/// Device information structure
#[repr(C)]
#[derive(Debug, Clone, Copy)]
pub struct DeviceInfo {
    pub cid: u8,            // Company/Customer ID
    pub mid: u8,            // Module ID  
    pub device_type: u8,    // Device type
}

impl DeviceInfo {
    pub fn new(cid: u8, mid: u8, device_type: u8) -> Self {
        Self { cid, mid, device_type }
    }
    
    /// Known Lofree Flow Lite device information
    pub fn lofree_flow_lite() -> Self {
        Self::new(0x35, 0x54, 0x01) // Based on analysis
    }
}

/// VID/PID pair for device identification
#[derive(Debug, Clone, Copy, PartialEq, Eq)]
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
            VidPid::new(0x3554, 0xF811), // Lofree branded mode
            VidPid::new(0x05AC, 0x024F), // Apple-compatible mode
            VidPid::new(0x3554, 0xF808), // Bootloader mode
        ]
    }
    
    /// Check if this VID/PID is a known Lofree Flow Lite
    pub fn is_lofree_flow_lite(&self) -> bool {
        Self::lofree_combinations().contains(self)
    }
    
    /// Get device mode description
    pub fn mode_description(&self) -> &'static str {
        match (self.vid, self.pid) {
            (0x3554, 0xF811) => "Lofree branded mode",
            (0x05AC, 0x024F) => "Apple-compatible mode", 
            (0x3554, 0xF808) => "Bootloader mode",
            _ => "Unknown mode",
        }
    }
}

impl std::fmt::Display for VidPid {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        write!(f, "{:04X}:{:04X}", self.vid, self.pid)
    }
}

/// Device compatibility checking
pub struct CompatibilityChecker;

impl CompatibilityChecker {
    /// Check if firmware is compatible with device
    pub fn is_firmware_compatible(
        firmware_cid: u8,
        firmware_mid: u8, 
        firmware_device_type: u8,
        device_info: &DeviceInfo,
    ) -> bool {
        firmware_cid == device_info.cid &&
        firmware_mid == device_info.mid &&
        firmware_device_type == device_info.device_type
    }
    
    /// Check if VID/PID matches expected for firmware
    pub fn is_vidpid_compatible(
        vid_pid: &VidPid,
        expected_vid: u16,
        expected_pid: u16,
    ) -> bool {
        vid_pid.vid == expected_vid && vid_pid.pid == expected_pid
    }
    
    /// Get expected VID/PID for device in different modes
    pub fn get_expected_vid_pid(mode: DeviceMode) -> VidPid {
        match mode {
            DeviceMode::LofreeBranded => VidPid::new(0x3554, 0xF811),
            DeviceMode::AppleCompatible => VidPid::new(0x05AC, 0x024F),
            DeviceMode::Bootloader => VidPid::new(0x3554, 0xF808),
        }
    }
}

/// Device operating modes
#[derive(Debug, Clone, Copy, PartialEq, Eq)]
pub enum DeviceMode {
    LofreeBranded,
    AppleCompatible,
    Bootloader,
}

impl DeviceMode {
    /// Detect mode from VID/PID
    pub fn from_vid_pid(vid: u16, pid: u16) -> Option<DeviceMode> {
        match (vid, pid) {
            (0x3554, 0xF811) => Some(DeviceMode::LofreeBranded),
            (0x05AC, 0x024F) => Some(DeviceMode::AppleCompatible),
            (0x3554, 0xF808) => Some(DeviceMode::Bootloader),
            _ => None,
        }
    }
    
    /// Get description
    pub fn description(&self) -> &'static str {
        match self {
            DeviceMode::LofreeBranded => "Lofree branded mode",
            DeviceMode::AppleCompatible => "Apple-compatible mode",
            DeviceMode::Bootloader => "Bootloader mode",
        }
    }
    
    /// Check if device is in runtime mode (not bootloader)
    pub fn is_runtime(&self) -> bool {
        !matches!(self, DeviceMode::Bootloader)
    }
}

#[cfg(test)]
mod tests {
    use super::*;
    
    #[test]
    fn test_vidpid_creation() {
        let vid_pid = VidPid::from_hex_strings("3554", "F811").unwrap();
        assert_eq!(vid_pid.vid, 0x3554);
        assert_eq!(vid_pid.pid, 0xF811);
    }
    
    #[test]
    fn test_lofree_detection() {
        let lofree_vid_pid = VidPid::new(0x3554, 0xF811);
        assert!(lofree_vid_pid.is_lofree_flow_lite());
        
        let other_vid_pid = VidPid::new(0x1234, 0x5678);
        assert!(!other_vid_pid.is_lofree_flow_lite());
    }
    
    #[test]
    fn test_device_mode_detection() {
        assert_eq!(DeviceMode::from_vid_pid(0x3554, 0xF811), Some(DeviceMode::LofreeBranded));
        assert_eq!(DeviceMode::from_vid_pid(0x05AC, 0x024F), Some(DeviceMode::AppleCompatible));
        assert_eq!(DeviceMode::from_vid_pid(0x3554, 0xF808), Some(DeviceMode::Bootloader));
        assert_eq!(DeviceMode::from_vid_pid(0x1234, 0x5678), None);
    }
    
    #[test]
    fn test_compatibility_checking() {
        let device_info = DeviceInfo::lofree_flow_lite();
        assert!(CompatibilityChecker::is_firmware_compatible(
            0x35, 0x54, 0x01, &device_info
        ));
        assert!(!CompatibilityChecker::is_firmware_compatible(
            0x00, 0x00, 0x00, &device_info
        ));
    }
}