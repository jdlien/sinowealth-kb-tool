use rusb::{Context, Direction, RequestType, Recipient, UsbContext};
use std::time::Duration;
use std::thread;

fn main() -> Result<(), Box<dyn std::error::Error>> {
    println!("Testing complete Lofree bootloader exit sequence with rusb...\n");
    
    let context = Context::new()?;
    
    // Find the Lofree bootloader device
    let device = context
        .devices()?
        .iter()
        .find(|d| {
            if let Ok(desc) = d.device_descriptor() {
                desc.vendor_id() == 0x3554 && desc.product_id() == 0xf808
            } else {
                false
            }
        })
        .ok_or("Lofree bootloader device not found (3554:f808)")?;
    
    println!("Found Lofree bootloader device!");
    
    // Open the device
    let handle = device.open()?;
    
    // Detach kernel driver if necessary
    let interface = 0;
    if handle.kernel_driver_active(interface).unwrap_or(false) {
        println!("Detaching kernel driver...");
        handle.detach_kernel_driver(interface)?;
    }
    
    // Claim the interface
    handle.claim_interface(interface)?;
    println!("Claimed interface 0");
    
    // Step 1: Send CLEAR-FAIL command
    println!("\n=== Step 1: Sending CLEAR-FAIL command ===");
    let mut clear_fail_cmd = vec![0x06, 0x5B, 0xB5, 0x99];
    clear_fail_cmd.resize(65, 0x00);
    send_command(&handle, &clear_fail_cmd)?;
    thread::sleep(Duration::from_millis(100));
    
    // Step 2: Send VERIFY/ENABLE command
    println!("\n=== Step 2: Sending VERIFY/ENABLE command ===");
    // Use the values from the last firmware write
    let fw_length = 62144u32;  // From the firmware write output
    let fw_crc = 0x7C41u16;     // From the firmware write output
    let length_words = (fw_length + 1) / 2;
    
    let mut verify_cmd = vec![0x06, 0x5B, 0xB5, 0x05];
    verify_cmd.push((length_words >> 8) as u8);
    verify_cmd.push(length_words as u8);
    verify_cmd.push((fw_crc >> 8) as u8);
    verify_cmd.push(fw_crc as u8);
    verify_cmd.resize(65, 0x00);
    
    println!("Length: {} bytes ({} words), CRC: 0x{:04X}", 
             fw_length, length_words, fw_crc);
    send_command(&handle, &verify_cmd)?;
    
    // Step 3: Try to read ACK
    println!("\n=== Step 3: Reading ACK ===");
    
    let mut attempts = 0;
    let max_attempts = 50;
    
    loop {
        let mut buffer = [0u8; 17];
        
        // USB control transfer for HID GET_REPORT
        let request_type = rusb::request_type(
            Direction::In,
            RequestType::Class,
            Recipient::Interface
        ); // 0xA1
        
        let request = 0x01; // GET_REPORT
        let value = (3 << 8) | 0x06; // (Feature Report = 3) << 8 | Report ID 6
        
        match handle.read_control(
            request_type,
            request,
            value,
            interface as u16,
            &mut buffer,
            Duration::from_millis(100)
        ) {
            Ok(len) => {
                println!("\n✓ Control transfer returned {} bytes!", len);
                print!("Data: ");
                for i in 0..len {
                    print!("{:02X} ", buffer[i]);
                }
                println!();
                
                if len >= 4 && buffer[1] == 0x5B && buffer[2] == 0xB6 {
                    match buffer[3] {
                        0x11 => {
                            println!("\n✅ Success ACK received (5B B6 11)");
                            
                            // Step 4: Send REBOOT command
                            println!("\n=== Step 4: Sending REBOOT command ===");
                            let mut reboot_cmd = vec![0x06, 0x5B, 0xB5, 0x88];
                            reboot_cmd.resize(65, 0x00);
                            send_command(&handle, &reboot_cmd)?;
                            
                            println!("✓ Reboot command sent!");
                            break;
                        }
                        0x10 => {
                            println!("\n❌ Failure ACK received (5B B6 10)");
                            println!("Bootloader rejected the firmware!");
                            break;
                        }
                        other => {
                            println!("\nUnknown ACK byte: {:02X}", other);
                        }
                    }
                }
            }
            Err(e) => {
                if attempts % 10 == 0 {
                    println!("Waiting for ACK... (attempt {}/{}): {}", attempts, max_attempts, e);
                }
            }
        }
        
        attempts += 1;
        if attempts > max_attempts {
            println!("\n⚠️  Timeout waiting for ACK after {} attempts", max_attempts);
            break;
        }
        
        thread::sleep(Duration::from_millis(50));
    }
    
    // Release the interface
    handle.release_interface(interface)?;
    
    // Wait a moment and check if device rebooted
    thread::sleep(Duration::from_secs(2));
    
    println!("\n=== Checking device status ===");
    
    // Try to find runtime mode device
    if context.devices()?.iter().any(|d| {
        if let Ok(desc) = d.device_descriptor() {
            (desc.vendor_id() == 0x05AC && desc.product_id() == 0x024F) ||
            (desc.vendor_id() == 0x3554 && desc.product_id() == 0xF811)
        } else {
            false
        }
    }) {
        println!("✅ SUCCESS! Device has exited bootloader mode!");
    } else if context.devices()?.iter().any(|d| {
        if let Ok(desc) = d.device_descriptor() {
            desc.vendor_id() == 0x3554 && desc.product_id() == 0xf808
        } else {
            false
        }
    }) {
        println!("❌ Device is still in bootloader mode");
    } else {
        println!("⚠️  Cannot find device - it may be rebooting");
    }
    
    Ok(())
}

fn send_command(handle: &rusb::DeviceHandle<Context>, command: &[u8]) -> Result<(), Box<dyn std::error::Error>> {
    if command.is_empty() {
        return Err("Command cannot be empty".into());
    }
    
    // USB control transfer for HID SET_REPORT
    let request_type = rusb::request_type(
        Direction::Out,
        RequestType::Class,
        Recipient::Interface
    ); // 0x21
    
    let request = 0x09; // SET_REPORT
    let value = (3 << 8) | command[0] as u16; // (Feature Report = 3) << 8 | Report ID
    let interface = 0u16;
    
    let bytes_written = handle.write_control(
        request_type,
        request,
        value,
        interface,
        command,
        Duration::from_millis(500)
    )?;
    
    println!("Sent {} bytes via USB control transfer", bytes_written);
    Ok(())
}