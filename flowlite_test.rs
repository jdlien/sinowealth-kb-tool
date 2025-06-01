use hidapi::{HidApi, HidDevice};

const VENDOR_ID: u16 = 0x3554;
const PRODUCT_ID: u16 = 0xf808;

fn main() -> Result<(), Box<dyn std::error::Error>> {
    let api = HidApi::new()?;
    
    #[cfg(target_os = "macos")]
    api.set_open_exclusive(false);
    
    let device = api.open(VENDOR_ID, PRODUCT_ID)?;
    
    println!("Connected to Flow Lite");
    
    // Try different opcodes to find what works
    let opcodes = [0x10, 0x52, 0x55, 0x75]; // read-info, init-read, enable-fw, isp-mode
    
    for opcode in opcodes {
        println!("\nTrying opcode 0x{:02x}", opcode);
        
        let mut cmd = [0u8; 6];
        cmd[0] = 0x06; // Report ID
        cmd[1] = opcode;
        
        println!("Sending command: {:02x?}", cmd);
        
        match device.send_feature_report(&cmd) {
            Ok(_) => {
                println!("Command sent successfully");
                
                // Try to get response
                let mut response = [0u8; 22];
                response[0] = 0x06; // Report ID
                
                match device.get_feature_report(&mut response) {
                    Ok(len) => {
                        println!("Response ({} bytes): {:02x?}", len, &response[..len]);
                    }
                    Err(e) => {
                        println!("Error getting response: {}", e);
                    }
                }
            }
            Err(e) => {
                println!("Error sending command: {}", e);
            }
        }
    }
    
    Ok(())
}