use hidapi::{HidApi, HidDevice};
use std::thread;
use std::time::Duration;

const LOFREE_VID: u16 = 0x3554;
const LOFREE_PID: u16 = 0xF808;

fn main() -> Result<(), Box<dyn std::error::Error>> {
    let api = HidApi::new()?;
    
    // Find the device
    let device = api.open(LOFREE_VID, LOFREE_PID)?;
    println!("Connected to Lofree Flow Lite bootloader");
    
    // Try reading first (safer than writing)
    test_read_commands(&device)?;
    
    Ok(())
}

fn test_read_commands(device: &HidDevice) -> Result<(), Box<dyn std::error::Error>> {
    // Common bootloader read commands to try
    let test_commands = vec![
        // Report ID, Command, Address (3 bytes), Length
        vec![0x05, 0x52, 0x00, 0x00, 0x00, 0x40], // Standard Sinowealth read
        vec![0x06, 0x52, 0x00, 0x00, 0x00, 0x40], // Using report ID 6
        vec![0x00, 0x52, 0x00, 0x00, 0x00, 0x40], // No report ID
        vec![0x06, 0x01, 0x00, 0x00, 0x00, 0x40], // Generic read cmd
        vec![0x06, 0x10, 0x00, 0x00, 0x00, 0x40], // Another common read
    ];
    
    for cmd in test_commands {
        println!("Trying command: {:02X?}", cmd);
        
        // Send command
        match device.write(&cmd) {
            Ok(bytes) => {
                println!("  Sent {} bytes", bytes);
                
                // Try to read response
                let mut buf = [0u8; 64];
                match device.read_timeout(&mut buf, 1000) {
                    Ok(size) => {
                        println!("  Response ({} bytes): {:02X?}", size, &buf[..size]);
                        // If we get a response, this might be the right protocol!
                    }
                    Err(e) => {
                        println!("  No response: {}", e);
                    }
                }
            }
            Err(e) => {
                println!("  Failed to send: {}", e);
            }
        }
        
        thread::sleep(Duration::from_millis(100));
    }
    
    Ok(())
}