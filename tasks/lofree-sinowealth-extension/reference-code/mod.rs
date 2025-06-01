// Lofree Flow Lite Support for sinowealth-kb-tool
// Reference implementation ported from Windows updater tool

pub mod firmware_format;
pub mod usb_protocol;
pub mod firmware_operations;

// Re-export key types for convenience
pub use firmware_format::{UpgradeFileHeader, DeviceInfo, VidPid};
pub use usb_protocol::{UsbCommand, UsbCommandID, DeviceEndpoints};
pub use firmware_operations::{FirmwareFile, FirmwareUpgrader, EmbeddedFirmware};

/// Main entry point for Lofree Flow Lite support
pub struct LofreeLiteDevice {
    pub vid_pid: VidPid,
    pub device_info: Option<DeviceInfo>,
    pub endpoints: DeviceEndpoints,
}

impl LofreeLiteDevice {
    /// Create new device instance for known VID/PID
    pub fn new(vid: u16, pid: u16) -> Self {
        let vid_pid = VidPid::new(vid, pid);
        let endpoints = match (vid, pid) {
            (0x3554, 0xF811) => DeviceEndpoints::lofree_defaults(),
            (0x05AC, 0x024F) => DeviceEndpoints::apple_mode(),
            _ => DeviceEndpoints::lofree_defaults(), // Default fallback
        };
        
        Self {
            vid_pid,
            device_info: None,
            endpoints,
        }
    }
    
    /// Create device for Lofree branded mode
    pub fn lofree_mode() -> Self {
        Self::new(0x3554, 0xF811)
    }
    
    /// Create device for Apple-compatible mode
    pub fn apple_mode() -> Self {
        Self::new(0x05AC, 0x024F)
    }
    
    /// Check if given VID/PID is a supported Lofree Flow Lite
    pub fn is_supported_device(vid: u16, pid: u16) -> bool {
        matches!((vid, pid), (0x3554, 0xF811) | (0x05AC, 0x024F))
    }
    
    /// Get list of all supported VID/PID combinations
    pub fn supported_devices() -> Vec<VidPid> {
        VidPid::lofree_combinations()
    }
}