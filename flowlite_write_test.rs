use hidapi::HidApi;
use std::thread;
use std::time::Duration;

const VENDOR_ID: u16 = 0x3554;
const PRODUCT_ID: u16 = 0xf808;

fn main() -> Result<(), Box<dyn std::error::Error>> {
    let api = HidApi::new()?;
    
    #[cfg(target_os = "macos")]
    api.set_open_exclusive(false);
    
    let device = api.open(VENDOR_ID, PRODUCT_ID)?;
    
    println!("Testing if writes actually work by writing test pattern...");
    
    // Step 1: Erase
    println!("1. Erasing flash...");
    let mut cmd = [0u8; 6];
    cmd[0] = 0x06;
    cmd[1] = 0x45; // Erase
    device.send_feature_report(&cmd)?;
    thread::sleep(Duration::from_millis(2000));
    
    // Step 2: Init write at address 0
    println!("2. Initializing write at address 0x0000...");
    cmd[1] = 0x57; // Init write
    cmd[2] = 0x00; // Low address
    cmd[3] = 0x00; // High address
    device.send_feature_report(&cmd)?;
    thread::sleep(Duration::from_millis(100));
    
    // Step 3: Write a single page with a test pattern
    println!("3. Writing test pattern...");
    let mut write_buf = vec![0u8; 2050]; // Report ID + cmd + 2048 bytes
    write_buf[0] = 0x06; // Report ID
    write_buf[1] = 0x77; // Write page
    
    // Fill with a recognizable pattern (alternating 0xAA, 0x55)
    for i in 2..write_buf.len() {
        write_buf[i] = if (i % 2) == 0 { 0xAA } else { 0x55 };
    }
    
    device.send_feature_report(&write_buf)?;
    println!("✓ Test pattern written");
    
    // Step 4: Try to enable firmware (this should fail if our pattern was written)
    println!("4. Attempting to enable firmware...");
    cmd[1] = 0x55; // Enable firmware
    cmd[2] = 0x00;
    cmd[3] = 0x00;
    device.send_feature_report(&cmd)?;
    thread::sleep(Duration::from_millis(500));
    
    // Step 5: Try reboot
    println!("5. Attempting reboot...");
    cmd[1] = 0x5a; // Reboot
    device.send_feature_report(&cmd)?;
    
    println!("6. If our write worked, the device should now be bricked or behave differently.");
    println!("   If it comes back normally, our writes might not be taking effect.");
    
    thread::sleep(Duration::from_secs(3));
    
    Ok(())
}