use hidapi::{HidApi, HidDevice};
use std::thread;
use std::time::Duration;

const VENDOR_ID: u16 = 0x3554;
const PRODUCT_ID: u16 = 0xf808;
const REPORT_ID: u8 = 0x06;
const REPORT_SIZE: usize = 33;

fn main() {
    println!("Flow Lite Debug Tool");
    println!("====================");
    
    // Initialize HID API
    let api = match HidApi::new() {
        Ok(api) => api,
        Err(e) => {
            eprintln!("Failed to initialize HID API: {}", e);
            return;
        }
    };
    
    // Find and open the device
    println!("\nSearching for Flow Lite device (VID: {:04x}, PID: {:04x})...", VENDOR_ID, PRODUCT_ID);
    
    let device = match api.open(VENDOR_ID, PRODUCT_ID) {
        Ok(device) => {
            println!("✓ Device found and opened successfully!");
            device
        }
        Err(e) => {
            eprintln!("✗ Failed to open device: {}", e);
            eprintln!("\nMake sure:");
            eprintln!("- The Flow Lite is connected");
            eprintln!("- You have the necessary permissions");
            eprintln!("- No other application is using the device");
            return;
        }
    };
    
    // Get device info
    if let Ok(info) = device.get_device_info() {
        println!("\nDevice Info:");
        println!("  Manufacturer: {:?}", info.manufacturer_string());
        println!("  Product: {:?}", info.product_string());
        println!("  Serial: {:?}", info.serial_number());
        println!("  Interface: {}", info.interface_number());
    }
    
    // Test each command
    println!("\nTesting commands...\n");
    
    // Test 1: Erase command (0x45)
    test_command(&device, 0x45, "Erase (0x45)");
    
    // Test 2: Enable firmware command (0x55)
    test_command(&device, 0x55, "Enable Firmware (0x55)");
    
    // Test 3: Init read command (0x52)
    test_command(&device, 0x52, "Init Read (0x52)");
    
    // Test 4: Init write command (0x57)
    test_command(&device, 0x57, "Init Write (0x57)");
    
    // Test 5: Reboot command (0x5a)
    println!("\nTesting Reboot (0x5a) - This may disconnect the device!");
    test_command(&device, 0x5a, "Reboot (0x5a)");
    
    println!("\nDebug test complete!");
}

fn test_command(device: &HidDevice, cmd: u8, name: &str) {
    println!("Testing {}:", name);
    
    // Prepare buffer with report ID and command
    let mut buffer = vec![0u8; REPORT_SIZE];
    buffer[0] = REPORT_ID;
    buffer[1] = cmd;
    
    // Display what we're sending
    println!("  → Sending: {:02x?}", &buffer[0..8]);
    
    // Send the command
    match device.write(&buffer) {
        Ok(bytes) => {
            println!("  ✓ Write successful: {} bytes sent", bytes);
        }
        Err(e) => {
            println!("  ✗ Write failed: {}", e);
            println!("    Error details: {:?}", e);
            return;
        }
    }
    
    // Give device time to process
    thread::sleep(Duration::from_millis(100));
    
    // Try to read response
    let mut response = vec![0u8; REPORT_SIZE];
    match device.read_timeout(&mut response, 1000) {
        Ok(bytes) => {
            if bytes > 0 {
                println!("  ← Response: {} bytes", bytes);
                println!("    Data: {:02x?}", &response[0..bytes.min(16)]);
                
                // Check for error indicators
                if response.len() > 1 && response[0] == REPORT_ID {
                    if response[1] == 0x00 {
                        println!("    Status: Success (0x00)");
                    } else if response[1] == 0xFF {
                        println!("    Status: Error (0xFF)");
                    } else {
                        println!("    Status: Unknown (0x{:02x})", response[1]);
                    }
                }
            } else {
                println!("  ← No response received");
            }
        }
        Err(e) => {
            println!("  ← Read error: {}", e);
        }
    }
    
    println!();
    thread::sleep(Duration::from_millis(500));
}