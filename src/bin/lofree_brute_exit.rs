use hidapi::HidApi;
use std::{thread, time};

fn main() -> Result<(), Box<dyn std::error::Error>> {
    let api = HidApi::new()?;
    
    // Find the Lofree bootloader device
    let device_info = api.device_list()
        .find(|d| d.vendor_id() == 0x3554 && d.product_id() == 0xf808)
        .ok_or("Lofree bootloader device not found")?;
        
    println!("Found Lofree bootloader device");
    let device = api.open_path(device_info.path())?;
    
    // Try the "brute force" method from the guide
    println!("Trying brute force exit method...");
    
    // Send legacy reboot opcode (0x5A)
    println!("Sending legacy reboot opcode...");
    let legacy_reboot = vec![0x06, 0x5a, 0x00, 0x00, 0x00, 0x00];
    if let Err(e) = device.send_feature_report(&legacy_reboot) {
        println!("Legacy reboot failed: {}", e);
    } else {
        println!("Legacy reboot sent successfully");
    }
    
    println!("Waiting for device to reboot...");
    thread::sleep(time::Duration::from_millis(2000));
    
    Ok(())
}