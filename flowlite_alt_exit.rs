use hidapi::HidApi;

const VENDOR_ID: u16 = 0x3554;
const PRODUCT_ID: u16 = 0xf808;

fn main() -> Result<(), Box<dyn std::error::Error>> {
    let api = HidApi::new()?;
    
    #[cfg(target_os = "macos")]
    api.set_open_exclusive(false);
    
    let device = api.open(VENDOR_ID, PRODUCT_ID)?;
    
    println!("Testing alternative exit sequences for Flow Lite...");
    
    // Sequence 1: Try opcode 0x75 (from your original notes)
    println!("\n=== Trying 0x75 opcode (from original notes) ===");
    let mut cmd = [0u8; 6];
    cmd[0] = 0x06;
    cmd[1] = 0x75; // This was mentioned in your original notes
    
    match device.send_feature_report(&cmd) {
        Ok(_) => {
            println!("✓ 0x75 command sent successfully");
            std::thread::sleep(std::time::Duration::from_secs(2));
        }
        Err(e) => println!("✗ 0x75 command failed: {}", e),
    }
    
    // Sequence 2: Try writing to a specific address (some bootloaders need this)
    println!("\n=== Trying init_write + specific address ===");
    cmd[1] = 0x57; // init_write
    cmd[2] = 0x00; // Low byte of address
    cmd[3] = 0x00; // High byte of address
    
    match device.send_feature_report(&cmd) {
        Ok(_) => {
            println!("✓ Init write to 0x0000 sent");
            
            // Now try enable
            cmd[1] = 0x55;
            cmd[2] = 0x00;
            cmd[3] = 0x00;
            
            match device.send_feature_report(&cmd) {
                Ok(_) => {
                    println!("✓ Enable firmware sent");
                    std::thread::sleep(std::time::Duration::from_secs(2));
                }
                Err(e) => println!("✗ Enable failed: {}", e),
            }
        }
        Err(e) => println!("✗ Init write failed: {}", e),
    }
    
    // Sequence 3: Try without enable_firmware, just reboot
    println!("\n=== Trying direct reboot without enable ===");
    cmd[1] = 0x5a; // reboot
    cmd[2] = 0x00;
    cmd[3] = 0x00;
    
    match device.send_feature_report(&cmd) {
        Ok(_) => {
            println!("✓ Direct reboot sent");
            std::thread::sleep(std::time::Duration::from_secs(3));
        }
        Err(e) => println!("✗ Direct reboot failed: {}", e),
    }
    
    println!("\nAll sequences attempted. Check device status...");
    
    Ok(())
}