use hidapi::{HidApi, HidDevice, HidError};
use std::thread;
use std::time::Duration;

/// Test program to verify that exclusive HID access allows reading feature reports on macOS
fn main() -> Result<(), Box<dyn std::error::Error>> {
    println!("Testing exclusive HID access for Lofree bootloader ACK reading...\n");

    // First, let's test with non-exclusive access (current behavior)
    println!("=== Test 1: Non-exclusive access (current implementation) ===");
    test_non_exclusive_access()?;
    
    thread::sleep(Duration::from_secs(1));
    
    // Then test with exclusive access (proposed solution)
    println!("\n=== Test 2: Exclusive access (proposed solution) ===");
    test_exclusive_access()?;
    
    // Also test rusb approach
    println!("\n=== Test 3: rusb/libusb control transfer approach ===");
    test_rusb_approach()?;
    
    Ok(())
}

fn test_non_exclusive_access() -> Result<(), Box<dyn std::error::Error>> {
    let api = HidApi::new()?;
    
    // On macOS, set non-exclusive (this is what we currently do)
    #[cfg(target_os = "macos")]
    api.set_open_exclusive(false);
    
    // Try to find the bootloader device
    if let Some(device) = api.device_list().find(|d| {
        d.vendor_id() == 0x3554 && d.product_id() == 0xf808
    }) {
        println!("Found Lofree bootloader device (3554:f808)");
        
        let hid_device = api.open_path(device.path())?;
        
        // Try to read feature report
        let mut buffer = vec![0x06; 17]; // 17-byte buffer with report ID
        
        match hid_device.get_feature_report(&mut buffer) {
            Ok(len) => {
                println!("get_feature_report returned {} bytes", len);
                if len > 0 {
                    print_hex("Received data", &buffer[..len]);
                } else {
                    println!("⚠️  Received 0 bytes (expected on macOS with non-exclusive access)");
                }
            }
            Err(e) => {
                println!("❌ get_feature_report failed: {}", e);
            }
        }
        
        // Also try read_timeout
        let mut read_buffer = vec![0u8; 64];
        match hid_device.read_timeout(&mut read_buffer, 100) {
            Ok(len) => {
                println!("read_timeout returned {} bytes", len);
                if len > 0 {
                    print_hex("Read data", &read_buffer[..len]);
                }
            }
            Err(e) => {
                println!("read_timeout error: {}", e);
            }
        }
        
    } else {
        println!("Lofree bootloader device not found. Make sure it's in bootloader mode (3554:f808)");
    }
    
    Ok(())
}

fn test_exclusive_access() -> Result<(), Box<dyn std::error::Error>> {
    let api = HidApi::new()?;
    
    // On macOS, try exclusive access (the proposed fix)
    #[cfg(target_os = "macos")]
    api.set_open_exclusive(true);  // This is the key change!
    
    // Try to find the bootloader device
    if let Some(device) = api.device_list().find(|d| {
        d.vendor_id() == 0x3554 && d.product_id() == 0xf808
    }) {
        println!("Found Lofree bootloader device (3554:f808)");
        
        match api.open_path(device.path()) {
            Ok(hid_device) => {
                println!("✓ Successfully opened device with exclusive access");
                
                // Try to read feature report
                let mut buffer = vec![0x06; 17]; // 17-byte buffer with report ID
                
                match hid_device.get_feature_report(&mut buffer) {
                    Ok(len) => {
                        println!("✓ get_feature_report returned {} bytes", len);
                        if len > 0 {
                            print_hex("Received data", &buffer[..len]);
                            
                            // Check if it's an ACK
                            if len >= 4 && buffer[1] == 0x5B && buffer[2] == 0xB6 {
                                match buffer[3] {
                                    0x11 => println!("✅ Success ACK received (5B B6 11)"),
                                    0x10 => println!("❌ Failure ACK received (5B B6 10)"),
                                    _ => println!("Unknown ACK type: {:02X}", buffer[3]),
                                }
                            }
                        } else {
                            println!("⚠️  Still received 0 bytes with exclusive access");
                        }
                    }
                    Err(e) => {
                        println!("❌ get_feature_report failed: {}", e);
                    }
                }
            }
            Err(e) => {
                println!("❌ Failed to open device with exclusive access: {}", e);
                println!("Note: This may fail if another process has the device open");
            }
        }
        
    } else {
        println!("Lofree bootloader device not found. Make sure it's in bootloader mode (3554:f808)");
    }
    
    Ok(())
}

fn test_rusb_approach() -> Result<(), Box<dyn std::error::Error>> {
    // Note: This requires adding 'rusb' to Cargo.toml dependencies
    println!("rusb approach would bypass HID entirely and use USB control transfers");
    println!("This is an alternative if exclusive HID access doesn't work");
    
    // The actual implementation would look like:
    /*
    use rusb::{Direction, RequestType, Recipient};
    
    let context = rusb::Context::new()?;
    if let Some(handle) = context.open_device_with_vid_pid(0x3554, 0xf808) {
        handle.claim_interface(0)?;  // bootloader is interface 0
        
        let mut buf = [0u8; 17];
        let req_type = rusb::request_type(Direction::In,
                                          RequestType::Class,
                                          Recipient::Interface);  // 0xA1
        let value = (3 << 8) | 0x06;  // (Feature = 3) << 8 | ReportID 6
        let len = handle.read_control(req_type, 0x01, value, 0, &mut buf, 500)?;
        
        println!("Control transfer returned {} bytes", len);
        if len > 0 {
            print_hex("Received data", &buf[..len]);
        }
    }
    */
    
    Ok(())
}

fn print_hex(label: &str, data: &[u8]) {
    print!("{}: ", label);
    for byte in data {
        print!("{:02X} ", byte);
    }
    println!();
}