use hidapi::HidApi;
use std::{thread, time::Duration};

fn main() -> Result<(), Box<dyn std::error::Error>> {
    let api = HidApi::new()?;
    
    #[cfg(target_os = "macos")]
    api.set_open_exclusive(false);
    
    println!("Looking for Lofree Flow Lite runtime device (05ac:024f)...");
    
    // Find the device - we need interface 1
    let device_info = api.device_list()
        .find(|d| {
            d.vendor_id() == 0x05ac && 
            d.product_id() == 0x024f && 
            d.interface_number() == 1
        })
        .ok_or("Lofree runtime device not found")?;
    
    println!("Found device at path: {:?}", device_info.path());
    let device = api.open_path(device_info.path())?;
    println!("Device opened successfully!");
    
    // Based on pcapng analysis, we need to send these commands in sequence:
    
    // Step 1: Send status command 0x0803
    println!("\nStep 1: Sending status command 0x0803...");
    let cmd1 = [0x08, 0x03, 0x00, 0x00, 0x00, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xca];
    match device.write(&cmd1) {
        Ok(len) => println!("✓ Sent {} bytes", len),
        Err(e) => println!("✗ Write failed: {}", e),
    }
    thread::sleep(Duration::from_millis(100));
    
    // Try to read response
    let mut response = [0u8; 64];
    match device.read_timeout(&mut response, 100) {
        Ok(len) => println!("  Response ({} bytes): {:02x?}", len, &response[..len.min(17)]),
        Err(e) => println!("  No response: {}", e),
    }
    
    // Step 2: Send status command 0x0804
    println!("\nStep 2: Sending status command 0x0804...");
    let cmd2 = [0x08, 0x04, 0x00, 0x00, 0x00, 0x84, 0x64, 0x01, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xc9];
    match device.write(&cmd2) {
        Ok(len) => println!("✓ Sent {} bytes", len),
        Err(e) => println!("✗ Write failed: {}", e),
    }
    thread::sleep(Duration::from_millis(100));
    
    // Try to read response
    match device.read_timeout(&mut response, 100) {
        Ok(len) => println!("  Response ({} bytes): {:02x?}", len, &response[..len.min(17)]),
        Err(e) => println!("  No response: {}", e),
    }
    
    // Step 3: Send mode switch command using OUTPUT report (not feature report)
    println!("\nStep 3: Sending mode switch command 0x080d as OUTPUT report...");
    let mode_switch = [0x08, 0x0d, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40];
    
    // According to pcapng, this needs to be a SET_REPORT control transfer with Output report type
    // In HIDAPI, we can try send_feature_report with report ID 8
    match device.send_feature_report(&mode_switch) {
        Ok(_) => {
            println!("✓ Mode switch command sent as feature report");
            println!("Device should now disconnect and re-enumerate as bootloader (3554:f808)...");
        }
        Err(e) => {
            println!("Feature report failed: {}, trying write()...", e);
            match device.write(&mode_switch) {
                Ok(len) => println!("✓ Mode switch sent via write ({} bytes)", len),
                Err(e2) => println!("✗ Both methods failed: {}", e2),
            }
        }
    }
    
    // Close device to allow re-enumeration
    drop(device);
    
    // Wait for re-enumeration
    println!("\nWaiting for bootloader device...");
    for i in 1..=10 {
        thread::sleep(Duration::from_millis(500));
        if api.device_list().any(|d| d.vendor_id() == 0x3554 && d.product_id() == 0xf808) {
            println!("✅ SUCCESS! Bootloader device (3554:f808) detected after {}ms!", i * 500);
            return Ok(());
        }
    }
    
    println!("❌ Bootloader device not found after 5 seconds");
    println!("Note: The device might need the exact control transfer format, not interrupt writes");
    
    Ok(())
}