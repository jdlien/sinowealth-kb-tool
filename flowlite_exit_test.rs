use hidapi::HidApi;

const VENDOR_ID: u16 = 0x3554;
const PRODUCT_ID: u16 = 0xf808;

fn main() -> Result<(), Box<dyn std::error::Error>> {
    let api = HidApi::new()?;
    
    #[cfg(target_os = "macos")]
    api.set_open_exclusive(false);
    
    let device = api.open(VENDOR_ID, PRODUCT_ID)?;
    
    println!("Connected to Flow Lite - testing bootloader exit sequences");
    
    // Try different potential "exit bootloader" command sequences
    let sequences = [
        ("Standard enable firmware", vec![0x55]),
        ("Enable + Reboot", vec![0x55, 0x5a]),
        ("Just Reboot", vec![0x5a]),
        ("Unknown opcode 0x75 (from user's notes)", vec![0x75]),
        ("Enable + Unknown + Reboot", vec![0x55, 0x75, 0x5a]),
        ("Double Enable", vec![0x55, 0x55]),
        ("Reset sequence", vec![0x00, 0x55]),
    ];
    
    for (i, (name, opcodes)) in sequences.iter().enumerate() {
        println!("\n=== Test {}: {} ===", i+1, name);
        
        for &opcode in opcodes {
            let mut cmd = [0u8; 6];
            cmd[0] = 0x06; // Report ID
            cmd[1] = opcode;
            
            println!("Sending opcode 0x{:02x}: {:02x?}", opcode, cmd);
            
            match device.send_feature_report(&cmd) {
                Ok(_) => {
                    println!("✓ Command sent successfully");
                    std::thread::sleep(std::time::Duration::from_millis(500));
                }
                Err(e) => {
                    println!("✗ Command failed: {}", e);
                    break;
                }
            }
        }
        
        println!("Waiting 2 seconds for device to potentially re-enumerate...");
        std::thread::sleep(std::time::Duration::from_secs(2));
        
        println!("Test {} complete. Please check if device changed.", i+1);
        println!("Press Enter to continue to next test (or Ctrl+C to stop)...");
        
        let mut input = String::new();
        std::io::stdin().read_line(&mut input)?;
    }
    
    println!("\nAll tests complete!");
    
    Ok(())
}