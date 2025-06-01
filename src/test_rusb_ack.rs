use rusb::{Context, Direction, RequestType, Recipient, UsbContext};
use std::time::Duration;

fn main() -> Result<(), Box<dyn std::error::Error>> {
    println!("Testing Lofree bootloader ACK reading with rusb...\n");
    
    let context = Context::new()?;
    
    // Find the Lofree bootloader device
    let device = context
        .devices()?
        .iter()
        .find(|d| {
            if let Ok(desc) = d.device_descriptor() {
                desc.vendor_id() == 0x3554 && desc.product_id() == 0xf808
            } else {
                false
            }
        })
        .ok_or("Lofree bootloader device not found (3554:f808)")?;
    
    println!("Found Lofree bootloader device!");
    
    // Open the device
    let mut handle = device.open()?;
    
    // Detach kernel driver if necessary
    let interface = 0;
    if handle.kernel_driver_active(interface).unwrap_or(false) {
        println!("Detaching kernel driver...");
        handle.detach_kernel_driver(interface)?;
    }
    
    // Claim the interface
    handle.claim_interface(interface)?;
    println!("Claimed interface 0");
    
    // Try to read feature report using USB control transfer
    println!("\nAttempting to read ACK via USB control transfer...");
    
    let mut buffer = [0u8; 17];
    
    // USB control transfer for HID GET_REPORT
    let request_type = rusb::request_type(
        Direction::In,
        RequestType::Class,
        Recipient::Interface
    ); // 0xA1
    
    let request = 0x01; // GET_REPORT
    let value = (3 << 8) | 0x06; // (Feature Report = 3) << 8 | Report ID 6
    
    match handle.read_control(
        request_type,
        request,
        value,
        interface as u16,
        &mut buffer,
        Duration::from_millis(500)
    ) {
        Ok(len) => {
            println!("\n✓ Control transfer returned {} bytes!", len);
            print!("Data: ");
            for i in 0..len {
                print!("{:02X} ", buffer[i]);
            }
            println!();
            
            if len >= 4 && buffer[1] == 0x5B && buffer[2] == 0xB6 {
                match buffer[3] {
                    0x11 => println!("\n✅ Success ACK received (5B B6 11)"),
                    0x10 => println!("\n❌ Failure ACK received (5B B6 10)"),
                    other => println!("\nUnknown ACK byte: {:02X}", other),
                }
            }
        }
        Err(e) => {
            println!("\n❌ Control transfer failed: {}", e);
            println!("This might be normal if no ACK is pending");
        }
    }
    
    // Release the interface
    handle.release_interface(interface)?;
    
    println!("\n✓ Test complete!");
    
    Ok(())
}