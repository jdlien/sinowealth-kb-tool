use hidapi::HidApi;
use std::{thread, time};

fn main() -> Result<(), Box<dyn std::error::Error>> {
    let api = HidApi::new()?;
    
    // Find the Lofree bootloader device
    let device_info = api.device_list()
        .find(|d| d.vendor_id() == 0x3554 && d.product_id() == 0xf808)
        .ok_or("Lofree bootloader device not found")?;
        
    println!("Found Lofree bootloader at: {:?}", device_info.path());
    let device = api.open_path(device_info.path())?;
    
    println!("Trying different reboot command sequences...");
    
    // Method 1: Standard reboot command with different report IDs
    println!("Method 1: Standard reboot (report ID 6)...");
    let cmd1 = vec![0x06, 0x5a, 0x00, 0x00, 0x00, 0x00];
    if let Err(e) = device.send_feature_report(&cmd1) {
        println!("Method 1 failed: {}", e);
    } else {
        println!("Method 1 sent successfully");
        thread::sleep(time::Duration::from_millis(2000));
    }
    
    // Method 2: Try sending finalize commands again with longer delays
    println!("Method 2: Re-send finalize commands with delays...");
    let mut finalize1 = vec![0x06, 0x5b, 0xb5, 0x05, 0x00];
    finalize1.resize(65, 0x00);
    device.send_feature_report(&finalize1)?;
    thread::sleep(time::Duration::from_millis(500));
    
    let mut finalize2 = vec![0x06, 0x5b, 0xb5, 0x88, 0x00];
    finalize2.resize(65, 0x00);
    device.send_feature_report(&finalize2)?;
    thread::sleep(time::Duration::from_millis(1000));
    
    // Method 3: Try reset/restart commands
    println!("Method 3: Reset command...");
    let reset_cmd = vec![0x06, 0x00, 0x00, 0x00, 0x00, 0x00];
    if let Err(e) = device.send_feature_report(&reset_cmd) {
        println!("Reset failed: {}", e);
    }
    thread::sleep(time::Duration::from_millis(1000));
    
    // Method 4: Try exit bootloader command
    println!("Method 4: Exit bootloader command...");
    let mut exit_cmd = vec![0x06, 0x5b, 0xb5, 0x99, 0x00];
    exit_cmd.resize(65, 0x00);
    if let Err(e) = device.send_feature_report(&exit_cmd) {
        println!("Exit command failed: {}", e);
    } else {
        println!("Exit command sent");
        thread::sleep(time::Duration::from_millis(2000));
    }
    
    // Method 5: Try power management command
    println!("Method 5: Power management commands...");
    let mut power_cmd = vec![0x06, 0x01, 0x00, 0x00, 0x00, 0x00];
    power_cmd.resize(65, 0x00);
    if let Err(e) = device.send_feature_report(&power_cmd) {
        println!("Power command failed: {}", e);
    }
    
    println!("All methods attempted. Please check device status...");
    Ok(())
}