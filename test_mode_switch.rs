use hidapi::HidApi;

fn main() -> Result<(), Box<dyn std::error::Error>> {
    let api = HidApi::new()?;
    
    #[cfg(target_os = "macos")]
    api.set_open_exclusive(false);
    
    println!("Looking for Lofree Flow Lite runtime device (05ac:024f) on interface 1...");
    
    // Find the device with interface 1 and usage_page 0xFF02
    let device_info = api.device_list()
        .find(|d| {
            d.vendor_id() == 0x05ac && 
            d.product_id() == 0x024f && 
            d.interface_number() == 1 &&
            d.usage_page() == 0xFF02
        })
        .ok_or("Lofree runtime device interface 1 not found")?;
    
    println!("Found device at path: {:?}", device_info.path());
    println!("Usage page: 0x{:04x}, Usage: 0x{:04x}", device_info.usage_page(), device_info.usage());
    
    // Open by specific path
    let device = api.open_path(device_info.path())?;
    println!("Device opened successfully!");
    
    // Prepare the exact mode switch command from GPT-o3 analysis
    let mut report = [0u8; 65];
    report[..16].copy_from_slice(&[
        0x08, 0x0d, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40
    ]);
    
    println!("Sending mode switch feature report (65 bytes)...");
    println!("Command: {:02x?}", &report[..16]);
    
    match device.send_feature_report(&report) {
        Ok(_) => {
            println!("✓ Feature report sent successfully!");
            // Close the device to allow re-enumeration
            drop(device);
            println!("Device connection closed");
            
            println!("Waiting for device to re-enumerate as bootloader (3554:f808)...");
            std::thread::sleep(std::time::Duration::from_millis(2000));
            
            // Check if bootloader device appears
            if api.device_list().any(|d| d.vendor_id() == 0x3554 && d.product_id() == 0xf808) {
                println!("✅ SUCCESS! Bootloader device (3554:f808) detected!");
            } else {
                println!("❌ Bootloader device not found after 2 seconds");
            }
        }
        Err(e) => {
            println!("✗ Feature report failed: {}", e);
        }
    }
    
    Ok(())
}