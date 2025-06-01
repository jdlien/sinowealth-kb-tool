use hidapi::HidApi;

const VENDOR_ID: u16 = 0x3554;
const PRODUCT_ID: u16 = 0xf808;

fn main() -> Result<(), Box<dyn std::error::Error>> {
    let api = HidApi::new()?;
    
    #[cfg(target_os = "macos")]
    api.set_open_exclusive(false);
    
    let device = api.open(VENDOR_ID, PRODUCT_ID)?;
    
    println!("Attempting to exit bootloader mode...");
    
    // Try enable firmware
    let mut cmd = [0u8; 6];
    cmd[0] = 0x06;
    cmd[1] = 0x55; // Enable firmware
    
    println!("Sending enable firmware command...");
    device.send_feature_report(&cmd)?;
    std::thread::sleep(std::time::Duration::from_millis(500));
    
    // Try reboot
    cmd[1] = 0x5a; // Reboot
    println!("Sending reboot command...");
    device.send_feature_report(&cmd)?;
    
    println!("Commands sent. Device should reboot...");
    std::thread::sleep(std::time::Duration::from_secs(3));
    
    Ok(())
}