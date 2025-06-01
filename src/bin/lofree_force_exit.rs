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
    
    // Try a sequence of commands that might force exit from bootloader
    
    // Command 1: Send jump to firmware command (LJMP style)
    println!("Trying LJMP-style commands...");
    let mut ljmp_cmd = vec![0x06, 0x02, 0x16, 0x00, 0x00]; // 0x02 = LJMP, 0x1600 = firmware start
    ljmp_cmd.resize(65, 0x00);
    if let Err(e) = device.send_feature_report(&ljmp_cmd) {
        println!("LJMP command failed: {}", e);
    } else {
        println!("LJMP command sent");
        thread::sleep(time::Duration::from_millis(1000));
    }
    
    // Command 2: Try firmware enable command similar to other Sinowealth devices
    println!("Trying firmware enable...");
    let enable_cmd = vec![0x06, 0x55, 0x00, 0x00, 0x00, 0x00]; // 0x55 = enable firmware
    if let Err(e) = device.send_feature_report(&enable_cmd) {
        println!("Enable firmware failed: {}", e);
    } else {
        println!("Enable firmware sent");
        thread::sleep(time::Duration::from_millis(1000));
    }
    
    // Command 3: Try disconnect/reconnect simulation
    println!("Trying disconnect simulation...");
    let mut disconnect_cmd = vec![0x06, 0xff, 0xff, 0xff, 0xff, 0xff];
    disconnect_cmd.resize(65, 0xff);
    if let Err(e) = device.send_feature_report(&disconnect_cmd) {
        println!("Disconnect simulation failed: {}", e);
    }
    thread::sleep(time::Duration::from_millis(2000));
    
    // Command 4: Try sending the finalize sequence one more time with exact timing
    println!("Final attempt with precise timing...");
    
    // First finalize command
    let mut finalize1 = vec![0x06, 0x5b, 0xb5, 0x05, 0x00];
    finalize1.resize(65, 0x00);
    device.send_feature_report(&finalize1)?;
    println!("Sent finalize 1");
    thread::sleep(time::Duration::from_millis(100));
    
    // Second finalize command with reboot flag
    let mut finalize2 = vec![0x06, 0x5b, 0xb5, 0x88, 0x00];
    finalize2.resize(65, 0x00);
    device.send_feature_report(&finalize2)?;
    println!("Sent finalize 2 (reboot)");
    thread::sleep(time::Duration::from_millis(500));
    
    // Final reboot command
    let reboot_cmd = vec![0x06, 0x5a, 0x00, 0x00, 0x00, 0x00];
    device.send_feature_report(&reboot_cmd)?;
    println!("Sent final reboot command");
    
    println!("All commands sent. Waiting for device to reboot...");
    thread::sleep(time::Duration::from_millis(3000));
    
    println!("Check device status now.");
    Ok(())
}