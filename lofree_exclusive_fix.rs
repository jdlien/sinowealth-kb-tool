use hidapi::{HidApi, HidDevice, HidError};
use std::thread;
use std::time::Duration;

/// Fixed version of lofree_wait_for_ack that uses exclusive access on macOS
/// This should replace the implementation in isp_device.rs
pub fn lofree_wait_for_ack_fixed(cmd_device: &HidDevice) -> Result<(), String> {
    const MAX_ATTEMPTS: u32 = 100;
    let mut attempts = 0;

    eprintln!("Waiting for bootloader ACK...");

    // For macOS, we need exclusive access to read feature reports from simulation/vendor pages
    #[cfg(target_os = "macos")]
    {
        // Get device info from the current device
        let device_info = cmd_device.get_device_info()
            .map_err(|e| format!("Failed to get device info: {}", e))?;
        
        // Create a new HidApi instance with EXCLUSIVE access
        let exclusive_api = HidApi::new()
            .map_err(|e| format!("Failed to create HID API: {}", e))?;
        
        // Set exclusive access (this is the critical fix!)
        exclusive_api.set_open_exclusive(true);
        
        // Open the device with exclusive access
        let exclusive_device = exclusive_api.open_path(device_info.path())
            .map_err(|e| format!("Failed to open device with exclusive access: {}. Make sure no other process is using the device.", e))?;
        
        eprintln!("✓ Opened device with exclusive access on macOS");
        
        // Now try to read the ACK with the exclusive device
        loop {
            let mut buffer = vec![0x06; 17]; // 17-byte buffer with report ID
            
            match exclusive_device.get_feature_report(&mut buffer) {
                Ok(len) if len >= 4 => {
                    eprintln!("Received {} bytes from feature report", len);
                    
                    if buffer[1] == 0x5B && buffer[2] == 0xB6 {
                        match buffer[3] {
                            0x11 => {
                                eprintln!("✓ Success ACK received (5B B6 11)");
                                return Ok(());
                            }
                            0x10 => {
                                eprintln!("✗ Failure ACK received (5B B6 10)");
                                return Err("Bootloader verification failed".to_string());
                            }
                            other => {
                                eprintln!("Unknown ACK byte: {:02X}", other);
                            }
                        }
                    }
                }
                Ok(len) => {
                    eprintln!("Feature report returned {} bytes (expected at least 4)", len);
                }
                Err(e) => {
                    eprintln!("Feature report error: {}", e);
                }
            }
            
            attempts += 1;
            if attempts > MAX_ATTEMPTS {
                return Err("Timeout waiting for ACK".to_string());
            }
            
            thread::sleep(Duration::from_millis(10));
        }
    }
    
    // Non-macOS platforms use the standard approach
    #[cfg(not(target_os = "macos"))]
    {
        loop {
            let mut buffer = vec![0x06; 17];
            
            match cmd_device.get_feature_report(&mut buffer) {
                Ok(len) if len >= 4 => {
                    if buffer[1] == 0x5B && buffer[2] == 0xB6 {
                        match buffer[3] {
                            0x11 => {
                                eprintln!("✓ Success ACK received (5B B6 11)");
                                return Ok(());
                            }
                            0x10 => {
                                eprintln!("✗ Failure ACK received (5B B6 10)");
                                return Err("Bootloader verification failed".to_string());
                            }
                            _ => continue,
                        }
                    }
                }
                _ => {
                    attempts += 1;
                    if attempts > MAX_ATTEMPTS {
                        return Err("Timeout waiting for ACK".to_string());
                    }
                    thread::sleep(Duration::from_millis(10));
                }
            }
        }
    }
}

/// Alternative implementation using rusb for direct USB control transfers
/// This bypasses HID entirely and should work on all platforms
#[cfg(feature = "rusb")]
pub fn lofree_wait_for_ack_rusb() -> Result<(), String> {
    use rusb::{Direction, RequestType, Recipient};
    
    const MAX_ATTEMPTS: u32 = 100;
    let mut attempts = 0;
    
    eprintln!("Using rusb for ACK reading (bypassing HID)...");
    
    // Open the bootloader device
    let context = rusb::Context::new()
        .map_err(|e| format!("Failed to create USB context: {}", e))?;
        
    let mut handle = context.open_device_with_vid_pid(0x3554, 0xf808)
        .ok_or("Failed to find Lofree bootloader device (3554:f808)")?;
        
    // Claim interface 0 (the HID interface)
    handle.claim_interface(0)
        .map_err(|e| format!("Failed to claim interface 0: {}", e))?;
    
    loop {
        let mut buf = [0u8; 17];
        
        // Construct GET_REPORT control transfer
        let req_type = rusb::request_type(Direction::In,
                                          RequestType::Class,
                                          Recipient::Interface); // 0xA1
        let value = (3 << 8) | 0x06; // (Feature = 3) << 8 | ReportID 6
        
        match handle.read_control(req_type, 0x01, value, 0, &mut buf, 500) {
            Ok(len) if len >= 4 => {
                eprintln!("Control transfer returned {} bytes", len);
                
                if buf[1] == 0x5B && buf[2] == 0xB6 {
                    match buf[3] {
                        0x11 => {
                            eprintln!("✓ Success ACK received (5B B6 11)");
                            return Ok(());
                        }
                        0x10 => {
                            eprintln!("✗ Failure ACK received (5B B6 10)");
                            return Err("Bootloader verification failed".to_string());
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
                eprintln!("Control transfer error: {}", e);
            }
        }
        
        attempts += 1;
        if attempts > MAX_ATTEMPTS {
            return Err("Timeout waiting for ACK".to_string());
        }
        
        thread::sleep(Duration::from_millis(10));
    }
}

/// Send commands using rusb (for complete USB-based solution)
#[cfg(feature = "rusb")]
pub fn lofree_send_command_rusb(command: &[u8]) -> Result<(), String> {
    use rusb::{Direction, RequestType, Recipient};
    
    let context = rusb::Context::new()
        .map_err(|e| format!("Failed to create USB context: {}", e))?;
        
    let mut handle = context.open_device_with_vid_pid(0x3554, 0xf808)
        .ok_or("Failed to find Lofree bootloader device (3554:f808)")?;
        
    handle.claim_interface(0)
        .map_err(|e| format!("Failed to claim interface 0: {}", e))?;
    
    // Construct SET_REPORT control transfer
    let req_type = rusb::request_type(Direction::Out,
                                      RequestType::Class,
                                      Recipient::Interface); // 0x21
    let value = (3 << 8) | command[0]; // (Feature = 3) << 8 | ReportID
    
    let written = handle.write_control(req_type, 0x09, value, 0, command, 500)
        .map_err(|e| format!("Failed to send command: {}", e))?;
        
    eprintln!("Sent {} bytes via USB control transfer", written);
    Ok(())
}

#[cfg(test)]
mod tests {
    use super::*;
    
    #[test]
    fn test_exclusive_access_flow() {
        // This test would require an actual device to be meaningful
        println!("Test would verify exclusive access allows ACK reading");
    }
}