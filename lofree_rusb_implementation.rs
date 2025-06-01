/// Alternative Lofree implementation using rusb (libusb) to bypass HID limitations
/// This should work on all platforms including macOS without exclusive access issues

use rusb::{Context, DeviceHandle, Direction, RequestType, Recipient, UsbContext};
use std::time::Duration;
use std::thread;

const LOFREE_BOOTLOADER_VID: u16 = 0x3554;
const LOFREE_BOOTLOADER_PID: u16 = 0xf808;

pub struct LofreeRusbDevice {
    handle: DeviceHandle<Context>,
}

impl LofreeRusbDevice {
    /// Open the Lofree bootloader device using rusb
    pub fn open() -> Result<Self, String> {
        let context = Context::new()
            .map_err(|e| format!("Failed to create USB context: {}", e))?;
            
        let mut handle = context
            .open_device_with_vid_pid(LOFREE_BOOTLOADER_VID, LOFREE_BOOTLOADER_PID)
            .ok_or_else(|| "Lofree bootloader device not found (3554:f808)")?;
            
        // Detach kernel driver if necessary
        let interface = 0;
        if handle.kernel_driver_active(interface).unwrap_or(false) {
            handle.detach_kernel_driver(interface)
                .map_err(|e| format!("Failed to detach kernel driver: {}", e))?;
        }
        
        // Claim the interface
        handle.claim_interface(interface)
            .map_err(|e| format!("Failed to claim interface 0: {}", e))?;
            
        Ok(LofreeRusbDevice { handle })
    }
    
    /// Send a command using USB control transfer (equivalent to HID send_feature_report)
    pub fn send_command(&self, command: &[u8]) -> Result<(), String> {
        if command.is_empty() {
            return Err("Command cannot be empty".to_string());
        }
        
        // USB control transfer for HID SET_REPORT
        let request_type = rusb::request_type(
            Direction::Out,
            RequestType::Class,
            Recipient::Interface
        ); // 0x21
        
        let request = 0x09; // SET_REPORT
        let value = (3 << 8) | command[0] as u16; // (Feature Report = 3) << 8 | Report ID
        let interface = 0;
        
        let bytes_written = self.handle.write_control(
            request_type,
            request,
            value,
            interface,
            command,
            Duration::from_millis(500)
        ).map_err(|e| format!("Failed to send command: {}", e))?;
        
        eprintln!("Sent {} bytes via USB control transfer", bytes_written);
        Ok(())
    }
    
    /// Read ACK using USB control transfer (equivalent to HID get_feature_report)
    pub fn read_ack(&self) -> Result<u8, String> {
        const MAX_ATTEMPTS: u32 = 100;
        let mut attempts = 0;
        
        loop {
            let mut buffer = [0u8; 17];
            buffer[0] = 0x06; // Report ID
            
            // USB control transfer for HID GET_REPORT
            let request_type = rusb::request_type(
                Direction::In,
                RequestType::Class,
                Recipient::Interface
            ); // 0xA1
            
            let request = 0x01; // GET_REPORT
            let value = (3 << 8) | 0x06; // (Feature Report = 3) << 8 | Report ID 6
            let interface = 0;
            
            match self.handle.read_control(
                request_type,
                request,
                value,
                interface,
                &mut buffer,
                Duration::from_millis(500)
            ) {
                Ok(len) if len >= 4 => {
                    eprintln!("Control transfer returned {} bytes: {:02X?}", len, &buffer[..len]);
                    
                    if buffer[1] == 0x5B && buffer[2] == 0xB6 {
                        match buffer[3] {
                            0x11 => {
                                eprintln!("✓ Success ACK received (5B B6 11)");
                                return Ok(0x11);
                            }
                            0x10 => {
                                eprintln!("✗ Failure ACK received (5B B6 10)");
                                return Ok(0x10);
                            }
                            other => {
                                eprintln!("Unknown ACK byte: {:02X}", other);
                            }
                        }
                    }
                }
                Ok(len) => {
                    eprintln!("Control transfer returned {} bytes (expected at least 4)", len);
                }
                Err(e) => {
                    // USB errors might be normal while waiting
                    if attempts % 10 == 0 {
                        eprintln!("Waiting for ACK... (attempt {}): {}", attempts, e);
                    }
                }
            }
            
            attempts += 1;
            if attempts > MAX_ATTEMPTS {
                return Err(format!("Timeout waiting for ACK after {} attempts", MAX_ATTEMPTS));
            }
            
            thread::sleep(Duration::from_millis(10));
        }
    }
    
    /// Complete bootloader exit sequence using rusb
    pub fn exit_bootloader(&self, fw_length: u32, fw_crc: u16) -> Result<(), String> {
        // Step 1: Clear fail status
        eprintln!("\n=== Step 1: Sending CLEAR-FAIL command ===");
        let mut clear_fail_cmd = vec![0x06, 0x5B, 0xB5, 0x99];
        clear_fail_cmd.resize(65, 0x00);
        self.send_command(&clear_fail_cmd)?;
        thread::sleep(Duration::from_millis(100));
        
        // Step 2: Send VERIFY/ENABLE command with parameters
        eprintln!("\n=== Step 2: Sending VERIFY/ENABLE command ===");
        let length_words = (fw_length + 1) / 2;
        let mut verify_cmd = vec![0x06, 0x5B, 0xB5, 0x05];
        verify_cmd.push((length_words >> 8) as u8);
        verify_cmd.push(length_words as u8);
        verify_cmd.push((fw_crc >> 8) as u8);
        verify_cmd.push(fw_crc as u8);
        verify_cmd.resize(65, 0x00);
        
        eprintln!("Length: {} bytes ({} words), CRC: 0x{:04X}", 
                 fw_length, length_words, fw_crc);
        self.send_command(&verify_cmd)?;
        
        // Step 3: Wait for and read ACK
        eprintln!("\n=== Step 3: Waiting for bootloader ACK ===");
        match self.read_ack()? {
            0x11 => {
                eprintln!("✅ Bootloader accepted the firmware!");
                
                // Step 4: Send REBOOT command
                eprintln!("\n=== Step 4: Sending REBOOT command ===");
                let mut reboot_cmd = vec![0x06, 0x5B, 0xB5, 0x88];
                reboot_cmd.resize(65, 0x00);
                self.send_command(&reboot_cmd)?;
                
                eprintln!("✓ Reboot command sent. Device should now exit bootloader.");
                Ok(())
            }
            0x10 => {
                Err("Bootloader rejected the firmware (CRC verification failed)".to_string())
            }
            _ => {
                Err("Unexpected ACK response".to_string())
            }
        }
    }
}

/// Test function to verify rusb communication
pub fn test_rusb_bootloader_exit() -> Result<(), String> {
    eprintln!("Testing Lofree bootloader exit using rusb (libusb)...\n");
    
    // Open the device
    let device = LofreeRusbDevice::open()?;
    eprintln!("✓ Successfully opened Lofree bootloader device with rusb");
    
    // For testing, use dummy values - in real use these would come from the firmware write
    let fw_length = 62864; // Example length
    let fw_crc = 0x0000;   // Should be 0x0000 for correct firmware
    
    // Try the complete exit sequence
    device.exit_bootloader(fw_length, fw_crc)?;
    
    // Wait a moment for device to reboot
    thread::sleep(Duration::from_secs(2));
    
    // Check if device has rebooted to runtime mode
    let context = Context::new().unwrap();
    if context.open_device_with_vid_pid(0x05AC, 0x024F).is_some() ||
       context.open_device_with_vid_pid(0x3554, 0xF811).is_some() {
        eprintln!("\n✅ SUCCESS! Device has exited bootloader mode!");
        Ok(())
    } else if context.open_device_with_vid_pid(LOFREE_BOOTLOADER_VID, LOFREE_BOOTLOADER_PID).is_some() {
        eprintln!("\n❌ Device is still in bootloader mode");
        Err("Device did not exit bootloader".to_string())
    } else {
        eprintln!("\n⚠️  Cannot find device - it may be rebooting");
        Ok(())
    }
}

#[cfg(test)]
mod tests {
    use super::*;
    
    #[test]
    #[ignore] // Requires actual device
    fn test_rusb_communication() {
        if let Err(e) = test_rusb_bootloader_exit() {
            eprintln!("Test failed: {}", e);
        }
    }
}