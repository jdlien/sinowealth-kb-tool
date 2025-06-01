use hidapi::{HidApi, HidDevice};
use std::fs;

const LOFREE_VID: u16 = 0x3554;
const LOFREE_PID: u16 = 0xF808;

// Standard Sinowealth ISP commands
const CMD_ENABLE_FIRMWARE: u8 = 0x55;
const CMD_INIT_READ: u8 = 0x52;
const CMD_INIT_WRITE: u8 = 0x57;
const CMD_ERASE: u8 = 0x45;
const CMD_REBOOT: u8 = 0x5a;

fn main() -> Result<(), Box<dyn std::error::Error>> {
    println!("Lofree Direct Bootloader Test");
    println!("=============================\n");
    
    let api = HidApi::new()?;
    
    // Find and open device
    let device = api.open(LOFREE_VID, LOFREE_PID)?;
    println!("Device opened successfully!");
    
    // Test 1: Try to read firmware/version (safe operation)
    test_read_firmware(&device)?;
    
    // Test 2: Try enable firmware command
    test_enable_firmware(&device)?;
    
    println!("\nIMPORTANT: Not attempting write/erase operations to avoid bricking.");
    println!("If read operations work, we could cautiously try write operations.");
    
    Ok(())
}

fn test_read_firmware(device: &HidDevice) -> Result<(), Box<dyn std::error::Error>> {
    println!("\nTest 1: Attempting to read firmware/version...");
    
    // Try standard Sinowealth read command with report ID 6
    // Format: [report_id, cmd, addr_hi, addr_mid, addr_lo, length]
    let read_commands = vec![
        ("Read at 0x0000", vec![0x06, CMD_INIT_READ, 0x00, 0x00, 0x00, 0x40]),
        ("Read at 0xF000", vec![0x06, CMD_INIT_READ, 0xF0, 0x00, 0x00, 0x40]),
        ("Read version", vec![0x06, 0x83, 0x00, 0x00, 0x00, 0x00]),
    ];
    
    for (name, cmd) in read_commands {
        println!("\n  Testing {}: {:02X?}", name, cmd);
        
        match device.write(&cmd) {
            Ok(bytes) => {
                println!("    Sent {} bytes", bytes);
                
                // Try to read data packet (report ID 6)
                let mut data_buf = vec![0u8; 64];
                match device.read_timeout(&mut data_buf, 1000) {
                    Ok(size) => {
                        if size > 0 {
                            println!("    *** DATA RECEIVED: {} bytes", size);
                            println!("    First 16 bytes: {:02X?}", &data_buf[..size.min(16)]);
                            
                            // If this is actual firmware data, it might start with valid code
                            if size >= 4 && data_buf[0] != 0xFF && data_buf[0] != 0x00 {
                                println!("    Looks like valid data!");
                            }
                        }
                    }
                    Err(e) => {
                        println!("    No data response: {}", e);
                    }
                }
            }
            Err(e) => {
                println!("    Send failed: {}", e);
            }
        }
    }
    
    Ok(())
}

fn test_enable_firmware(device: &HidDevice) -> Result<(), Box<dyn std::error::Error>> {
    println!("\n\nTest 2: Testing enable firmware command...");
    
    // Standard Sinowealth enable firmware command
    let cmd = vec![0x06, CMD_ENABLE_FIRMWARE, 0x00, 0x00, 0x00, 0x00];
    println!("  Sending: {:02X?}", cmd);
    
    match device.write(&cmd) {
        Ok(bytes) => {
            println!("  Sent {} bytes", bytes);
            
            // Check for any response
            let mut response = vec![0u8; 64];
            match device.read_timeout(&mut response, 1000) {
                Ok(size) => {
                    if size > 0 {
                        println!("  Response: {} bytes: {:02X?}", size, &response[..size.min(16)]);
                    }
                }
                Err(_) => {
                    println!("  No response (this might be normal)");
                }
            }
        }
        Err(e) => {
            println!("  Send failed: {}", e);
        }
    }
    
    Ok(())
}