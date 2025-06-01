// Test program to parse Lofree firmware files
use std::fs;
use std::env;

// We need to include the main crate modules
use sinowealth_kb_tool::devices::lofree::{LofreeFirmwareFile, VidPid};

fn main() -> Result<(), Box<dyn std::error::Error>> {
    let args: Vec<String> = env::args().collect();
    if args.len() < 2 {
        eprintln!("Usage: {} <firmware_file>", args[0]);
        std::process::exit(1);
    }
    
    let firmware_path = &args[1];
    println!("Parsing firmware file: {}", firmware_path);
    
    // Read the firmware file
    let firmware_data = fs::read(firmware_path)?;
    println!("File size: {} bytes", firmware_data.len());
    
    // Try to parse as Lofree firmware
    match LofreeFirmwareFile::from_bytes(&firmware_data) {
        Ok(firmware) => {
            println!("\n✅ Successfully parsed Lofree firmware file!");
            
            let header = &firmware.header;
            println!("📋 Firmware Information:");
            let version_raw = header.version;
            println!("  Version: {} (raw: 0x{:08X})", firmware.version_string(), version_raw);
            
            // Copy packed fields to avoid alignment issues
            let head_length = header.head_length;
            let fw_length = header.fw_length;
            let device_type = header.device_type;
            let cid = header.cid;
            let mid = header.mid;
            let head_crc = header.head_crc;
            
            println!("  Header Length: {} bytes", head_length);
            println!("  Firmware Length: {} bytes", fw_length);
            println!("  Device Type: {}", device_type);
            println!("  CID: 0x{:02X}", cid);
            println!("  MID: 0x{:02X}", mid);
            
            if !header.product_name_str().is_empty() {
                println!("  Product: {}", header.product_name_str());
            }
            if !header.ic_name_str().is_empty() {
                println!("  IC Name: {}", header.ic_name_str());
            }
            if !header.file_id_str().is_empty() {
                println!("  File ID: {}", header.file_id_str());
            }
            
            println!("  CRC32: 0x{:08X}", head_crc);
            println!("  CRC Valid: {}", firmware.validate_crc());
            
            println!("\n🔍 Device Compatibility:");
            let supported_devices = VidPid::lofree_combinations();
            for vid_pid in supported_devices {
                println!("  {} ({})", vid_pid, vid_pid.mode_description());
            }
            
            println!("\n📊 Firmware Data:");
            println!("  Binary size: {} bytes", firmware.firmware_data.len());
            if firmware.firmware_data.len() >= 16 {
                println!("  First 16 bytes: {:02X?}", &firmware.firmware_data[..16]);
            }
        }
        Err(e) => {
            println!("❌ Failed to parse as Lofree firmware: {}", e);
            
            // Try to analyze the raw data
            println!("\n🔍 Raw file analysis:");
            if firmware_data.len() >= 720 {
                println!("  File is large enough for Lofree header (720 bytes)");
                
                // Check first few bytes
                println!("  First 32 bytes: {:02X?}", &firmware_data[..32.min(firmware_data.len())]);
                
                // Look for potential version info
                if firmware_data.len() >= 24 {
                    let potential_version = u32::from_le_bytes([
                        firmware_data[20], firmware_data[21], firmware_data[22], firmware_data[23]
                    ]);
                    println!("  Potential version (offset 20): 0x{:08X}", potential_version);
                }
            } else {
                println!("  File too small for Lofree header (need 720 bytes, got {})", firmware_data.len());
            }
        }
    }
    
    Ok(())
}