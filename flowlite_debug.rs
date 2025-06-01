use hidapi::HidApi;

const VENDOR_ID: u16 = 0x3554;
const PRODUCT_ID: u16 = 0xf808;

const CMD_ERASE: u8 = 0x45;
const CMD_ENABLE_FIRMWARE: u8 = 0x55;
const CMD_INIT_READ: u8 = 0x52;
const CMD_INIT_WRITE: u8 = 0x57;
const CMD_REBOOT: u8 = 0x5a;

fn main() -> Result<(), Box<dyn std::error::Error>> {
    let api = HidApi::new()?;
    
    #[cfg(target_os = "macos")]
    api.set_open_exclusive(false);
    
    let device = api.open(VENDOR_ID, PRODUCT_ID)?;
    
    println!("Connected to Flow Lite ({}:{:04x})", VENDOR_ID, PRODUCT_ID);
    
    // Test each command individually
    let commands = [
        ("ERASE", CMD_ERASE),
        ("ENABLE_FIRMWARE", CMD_ENABLE_FIRMWARE), 
        ("INIT_READ", CMD_INIT_READ),
        ("INIT_WRITE", CMD_INIT_WRITE),
        ("REBOOT", CMD_REBOOT),
    ];
    
    for (name, opcode) in commands {
        println!("\n=== Testing {} command (0x{:02x}) ===", name, opcode);
        
        let mut cmd = [0u8; 6];
        cmd[0] = 0x06; // Report ID
        cmd[1] = opcode;
        
        println!("Sending: {:02x?}", cmd);
        
        match device.send_feature_report(&cmd) {
            Ok(_) => {
                println!("✓ Command sent successfully");
                
                // For init commands, try to get a response
                if matches!(opcode, CMD_INIT_READ | CMD_INIT_WRITE) {
                    let mut response = [0u8; 48]; // Try larger buffer
                    response[0] = 0x06;
                    
                    match device.get_feature_report(&mut response) {
                        Ok(len) => {
                            println!("✓ Response received ({} bytes): {:02x?}", len, &response[..len.min(16)]);
                        }
                        Err(e) => {
                            println!("✗ No response: {}", e);
                        }
                    }
                }
                
                // Add small delay between commands
                std::thread::sleep(std::time::Duration::from_millis(100));
            }
            Err(e) => {
                println!("✗ Command failed: {}", e);
            }
        }
    }
    
    println!("\n=== Testing write/read operations ===");
    
    // Test a simple write page operation
    println!("\nTesting write page...");
    let mut write_buf = vec![0u8; 2050]; // Report ID + cmd + 2048 bytes
    write_buf[0] = 0x06; // Report ID
    write_buf[1] = 0x77; // Write page opcode
    // Fill with test pattern
    for i in 2..write_buf.len() {
        write_buf[i] = (i % 256) as u8;
    }
    
    match device.send_feature_report(&write_buf) {
        Ok(_) => println!("✓ Write page command sent"),
        Err(e) => println!("✗ Write page failed: {}", e),
    }
    
    // Test a read page operation  
    println!("\nTesting read page...");
    let mut read_buf = vec![0u8; 2050];
    read_buf[0] = 0x06; // Report ID
    read_buf[1] = 0x72; // Read page opcode
    
    match device.get_feature_report(&mut read_buf) {
        Ok(len) => println!("✓ Read page response ({} bytes): {:02x?}", len, &read_buf[..16.min(len)]),
        Err(e) => println!("✗ Read page failed: {}", e),
    }
    
    println!("\n=== Debug complete ===");
    
    Ok(())
}