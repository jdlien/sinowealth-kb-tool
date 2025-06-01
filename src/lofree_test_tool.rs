use hidapi::{HidApi, HidDevice};
use std::thread;
use std::time::Duration;
use std::fs;

const LOFREE_VID: u16 = 0x3554;
const LOFREE_PID: u16 = 0xF808;

fn main() -> Result<(), Box<dyn std::error::Error>> {
    println!("Lofree Flow Lite Protocol Test Tool");
    println!("===================================");
    
    let api = HidApi::new()?;
    
    // Try different interface numbers
    for interface in 0..2 {
        println!("\nTrying interface {}...", interface);
        
        // List all devices with our VID/PID
        for device in api.device_list() {
            if device.vendor_id() == LOFREE_VID && device.product_id() == LOFREE_PID {
                if device.interface_number() == interface {
                    println!("Found device on interface {}", interface);
                    println!("  Path: {:?}", device.path());
                    
                    // Try to open the device
                    match api.open_path(device.path()) {
                        Ok(device) => {
                            println!("  Opened successfully!");
                            test_device(&device)?;
                        }
                        Err(e) => {
                            println!("  Failed to open: {}", e);
                        }
                    }
                }
            }
        }
    }
    
    Ok(())
}

fn test_device(device: &HidDevice) -> Result<(), Box<dyn std::error::Error>> {
    println!("\nTesting device communication...");
    
    // Test 1: Try to read device info (safest)
    test_read_device_info(device)?;
    
    // Test 2: Try different report IDs with safe read commands
    test_report_ids(device)?;
    
    // Test 3: Check if device responds to standard Sinowealth commands
    test_sinowealth_commands(device)?;
    
    // Test 4: Test with full 48-byte packets
    test_full_packets(device)?;
    
    // Test 5: Try to find the enter update mode command
    test_enter_update_mode(device)?;
    
    Ok(())
}

fn test_enter_update_mode(device: &HidDevice) -> Result<(), Box<dyn std::error::Error>> {
    println!("\n5. Testing potential 'enter update mode' commands...");
    
    // Based on the DLL function EnterUsbUpdateMode
    // Try various command patterns that might trigger update mode
    let commands = vec![
        // Try standard Sinowealth enter ISP (0x75)
        ("Sinowealth ISP", vec![0x06, 0x75, 0x00, 0x00, 0x00, 0x00]),
        
        // Try variations
        ("Update cmd 1", vec![0x06, 0x55, 0x00, 0x00, 0x00, 0x00]),
        ("Update cmd 2", vec![0x06, 0xAA, 0x00, 0x00, 0x00, 0x00]),
        ("Update cmd 3", vec![0x06, 0xFF, 0x00, 0x00, 0x00, 0x00]),
        
        // Try with magic bytes
        ("Magic Q$UU", vec![0x06, 0x51, 0x24, 0x55, 0x55, 0x00]),
        ("Magic reversed", vec![0x06, 0x55, 0x55, 0x24, 0x51, 0x00]),
        
        // Try common bootloader patterns
        ("Boot pattern 1", vec![0x06, 0x7F, 0x00, 0x00, 0x00, 0x00]),
        ("Boot pattern 2", vec![0x06, 0x80, 0x00, 0x00, 0x00, 0x00]),
        ("Boot pattern 3", vec![0x06, 0xF0, 0x00, 0x00, 0x00, 0x00]),
        
        // Try with CID/MID from config (CID=1, MID=1)
        ("With CID/MID", vec![0x06, 0x75, 0x01, 0x01, 0x00, 0x00]),
    ];
    
    for (name, cmd) in commands {
        println!("\n  Testing {}: {:02X?}", name, cmd);
        
        match device.write(&cmd) {
            Ok(bytes) => {
                println!("    Sent {} bytes", bytes);
                
                // Give device time to process
                thread::sleep(Duration::from_millis(100));
                
                // Try to read any response
                let mut response = vec![0u8; 64];
                match device.read_timeout(&mut response, 1000) {
                    Ok(size) => {
                        if size > 0 {
                            println!("    *** RESPONSE: {} bytes: {:02X?}", size, &response[..size]);
                        }
                    }
                    Err(_) => {}
                }
                
                // Check if device status changed
                match device.get_manufacturer_string() {
                    Ok(Some(s)) => {
                        if !s.is_empty() {
                            println!("    Device status changed! Manufacturer: {}", s);
                        }
                    }
                    _ => {}
                }
            }
            Err(e) => {
                println!("    Send failed: {}", e);
            }
        }
    }
    
    Ok(())
}

fn test_full_packets(device: &HidDevice) -> Result<(), Box<dyn std::error::Error>> {
    println!("\n4. Testing with full 48-byte packets (as per report descriptor)...");
    
    // Create 48-byte packet with report ID 6
    let mut packet = vec![0u8; 49]; // 1 byte report ID + 48 bytes data
    packet[0] = 0x06; // Report ID
    
    // Test different command bytes
    let commands = vec![
        (0x01, "Get Info"),
        (0x10, "Get Version"), 
        (0xAA, "Test AA"),
        (0x55, "Test 55"),
    ];
    
    for (cmd, name) in commands {
        packet[1] = cmd;
        println!("\n  Testing {} (cmd=0x{:02X})", name, cmd);
        
        match device.write(&packet) {
            Ok(bytes) => {
                println!("    Sent {} bytes", bytes);
                
                // Read with 48-byte buffer
                let mut response = vec![0u8; 48];
                match device.read_timeout(&mut response, 1000) {
                    Ok(size) => {
                        if size > 0 {
                            println!("    *** Response ({} bytes): {:02X?}", size, &response[..size.min(16)]);
                        }
                    }
                    Err(_) => {}
                }
            }
            Err(e) => {
                println!("    Send failed: {}", e);
            }
        }
    }
    
    Ok(())
}

fn test_read_device_info(device: &HidDevice) -> Result<(), Box<dyn std::error::Error>> {
    println!("\n1. Testing device info reads...");
    
    // Try to get manufacturer string
    match device.get_manufacturer_string() {
        Ok(Some(s)) => println!("  Manufacturer: {}", s),
        Ok(None) => println!("  Manufacturer: (none)"),
        Err(e) => println!("  Manufacturer read error: {}", e),
    }
    
    // Try to get product string
    match device.get_product_string() {
        Ok(Some(s)) => println!("  Product: {}", s),
        Ok(None) => println!("  Product: (none)"),
        Err(e) => println!("  Product read error: {}", e),
    }
    
    Ok(())
}

fn test_report_ids(device: &HidDevice) -> Result<(), Box<dyn std::error::Error>> {
    println!("\n2. Testing report IDs...");
    
    // Common report IDs to test
    let report_ids = vec![0x00, 0x01, 0x05, 0x06, 0xB0];
    
    for report_id in report_ids {
        print!("  Report ID 0x{:02X}: ", report_id);
        
        // Try to read with this report ID
        let mut buf = vec![0u8; 64];
        buf[0] = report_id;
        
        match device.get_feature_report(&mut buf) {
            Ok(size) => {
                println!("Read {} bytes: {:02X?}", size, &buf[..size.min(16)]);
            }
            Err(e) => {
                println!("Error: {}", e);
            }
        }
        
        thread::sleep(Duration::from_millis(50));
    }
    
    Ok(())
}

fn test_sinowealth_commands(device: &HidDevice) -> Result<(), Box<dyn std::error::Error>> {
    println!("\n3. Testing potential commands...");
    
    // Test various command structures
    let test_commands = vec![
        // Standard Sinowealth commands with report ID 6
        ("Read Version", vec![0x06, 0x83, 0x00, 0x00, 0x00, 0x00]),
        ("Read Flash", vec![0x06, 0x52, 0x00, 0x00, 0x00, 0x40]),
        ("Get Status", vec![0x06, 0x84, 0x00, 0x00, 0x00, 0x00]),
        
        // Try common bootloader commands
        ("Get Info 1", vec![0x06, 0x01, 0x00, 0x00, 0x00, 0x00]),
        ("Get Info 2", vec![0x06, 0x02, 0x00, 0x00, 0x00, 0x00]),
        ("Get Version", vec![0x06, 0x10, 0x00, 0x00, 0x00, 0x00]),
        
        // Try with longer packets (48 bytes as per report descriptor)
        ("Long packet 1", vec![0x06, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00]),
        ("Long packet 2", vec![0x06, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00]),
        
        // Try potential Lofree-specific commands
        ("Lofree cmd 1", vec![0x06, 0xAA, 0x55, 0x00, 0x00, 0x00]),
        ("Lofree cmd 2", vec![0x06, 0x55, 0xAA, 0x00, 0x00, 0x00]),
        
        // Try commands based on ComUsbUpgradeFile
        ("Q$UU check", vec![0x06, 0x51, 0x24, 0x55, 0x55, 0x00]),
    ];
    
    for (name, cmd) in test_commands.iter() {
        println!("\n  Testing {}: {:02X?}", name, cmd);
        
        // Send command
        match device.write(cmd) {
            Ok(bytes) => {
                println!("    Sent {} bytes", bytes);
                
                // Try to read response with different sizes
                for read_size in &[64, 48, 32, 16] {
                    let mut response = vec![0u8; *read_size];
                    match device.read_timeout(&mut response, 500) {
                        Ok(size) => {
                            println!("    Response ({} bytes): {:02X?}", size, &response[..size.min(16)]);
                            
                            // If we get a response, this might be a valid command!
                            if size > 0 {
                                println!("    *** VALID RESPONSE DETECTED ***");
                                // Try to interpret the response
                                if size >= 4 {
                                    println!("    First 4 bytes as u32: 0x{:08X}", 
                                        u32::from_le_bytes([response[0], response[1], response[2], response[3]]));
                                }
                            }
                            break;
                        }
                        Err(_) => {
                            // Try next size
                        }
                    }
                }
            }
            Err(e) => {
                println!("    Send failed: {}", e);
            }
        }
        
        thread::sleep(Duration::from_millis(100));
    }
    
    Ok(())
}

fn load_firmware(path: &str) -> Result<Vec<u8>, Box<dyn std::error::Error>> {
    let data = fs::read(path)?;
    
    // Check for ComUsbUpgradeFile header
    if data.len() > 4 && &data[0..4] == b"Q$UU" {
        println!("Detected ComUsbUpgradeFile container");
        println!("  Total size: {} bytes", data.len());
        println!("  Header size: 720 bytes");
        println!("  Payload size: {} bytes", data.len() - 720);
        
        // Extract payload (skip 720-byte header)
        if data.len() > 720 {
            Ok(data[720..].to_vec())
        } else {
            Err("File too small".into())
        }
    } else {
        Ok(data)
    }
}