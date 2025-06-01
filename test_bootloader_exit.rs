use hidapi::HidApi;
use std::{thread, time::Duration};

fn main() -> Result<(), Box<dyn std::error::Error>> {
    let api = HidApi::new()?;
    
    #[cfg(target_os = "macos")]
    api.set_open_exclusive(false);
    
    println!("Looking for Lofree Flow Lite bootloader device (3554:f808)...");
    
    // Find bootloader device on interface 0
    let device_info = api.device_list()
        .find(|d| {
            d.vendor_id() == 0x3554 && 
            d.product_id() == 0xf808 && 
            d.interface_number() == 0
        })
        .ok_or("Lofree bootloader device not found")?;
    
    println!("Found device at path: {:?}", device_info.path());
    let device = api.open_path(device_info.path())?;
    println!("Device opened successfully!");
    
    println!("\nSending bootloader exit sequence using INTERRUPT WRITES...");
    
    // Command 1: COPY_FOOTER (0x5b b5 88)
    let copy_footer = [
        0x06, 0x5b, 0xb5, 0x88, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
    ];
    
    println!("Sending COPY_FOOTER (5b b5 88)...");
    match device.write(&copy_footer) {
        Ok(len) => println!("✓ Sent {} bytes", len),
        Err(e) => println!("✗ Failed: {}", e),
    }
    thread::sleep(Duration::from_millis(50));
    
    // Command 2: REBOOT (0x5b b5 99)
    let reboot = [
        0x06, 0x5b, 0xb5, 0x99, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
    ];
    
    println!("Sending REBOOT (5b b5 99)...");
    match device.write(&reboot) {
        Ok(len) => println!("✓ Sent {} bytes", len),
        Err(e) => println!("✗ Failed: {}", e),
    }
    
    // Close device and wait for re-enumeration
    drop(device);
    println!("\nDevice connection closed. Waiting for re-enumeration...");
    
    // Check for runtime device
    for i in 1..=20 {
        thread::sleep(Duration::from_millis(250));
        if api.device_list().any(|d| d.vendor_id() == 0x05ac && d.product_id() == 0x024f) {
            println!("✅ SUCCESS! Runtime device (05ac:024f) detected after {}ms!", i * 250);
            return Ok(());
        }
    }
    
    // Check if still in bootloader
    if api.device_list().any(|d| d.vendor_id() == 0x3554 && d.product_id() == 0xf808) {
        println!("❌ Device still in bootloader mode (3554:f808)");
    } else {
        println!("❌ Device not found in either mode");
    }
    
    Ok(())
}