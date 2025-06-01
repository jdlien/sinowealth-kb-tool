// Example: Parse Lofree Flow Lite firmware file
// Usage: cargo run --example parse_firmware -- /path/to/firmware.bin

use lofree_flow_lite_reference::FirmwareFile;
use std::env;

fn main() -> Result<(), Box<dyn std::error::Error>> {
    let args: Vec<String> = env::args().collect();
    if args.len() != 2 {
        eprintln!("Usage: {} <firmware_file>", args[0]);
        std::process::exit(1);
    }
    
    let firmware_path = &args[1];
    println!("Parsing firmware file: {}", firmware_path);
    
    // Load and parse firmware
    let firmware = FirmwareFile::load_from_file(firmware_path)?;
    
    // Display header information
    println!("\n=== Firmware Header ===");
    println!("Header CRC: 0x{:08X}", firmware.header.head_crc);
    println!("Header Length: {} bytes", firmware.header.head_length);
    println!("Firmware Length: {} bytes", firmware.header.fw_length);
    println!("Version: {} (0x{:08X})", firmware.version_string(), firmware.header.version);
    println!("Device Type: 0x{:02X}", firmware.header.device_type);
    println!("CID: 0x{:02X}", firmware.header.cid);
    println!("MID: 0x{:02X}", firmware.header.mid);
    
    // Display string fields
    println!("\n=== Device Information ===");
    println!("IC Name: {}", firmware.header.ic_name_str());
    println!("Product Name: {}", firmware.header.product_name_str());
    println!("Sensor Name: {}", firmware.header.sensor_name_str());
    println!("File ID: {}", firmware.header.file_id_str());
    
    // Display endpoints
    println!("\n=== USB Endpoints ===");
    println!("Boot Input: {}", 
        std::str::from_utf8(&firmware.header.boot_input_endpoint)
            .unwrap_or("(invalid)")
            .trim_end_matches('\0'));
    println!("Boot Output: {}", 
        std::str::from_utf8(&firmware.header.boot_output_endpoint)
            .unwrap_or("(invalid)")
            .trim_end_matches('\0'));
    println!("Normal Input: {}", 
        std::str::from_utf8(&firmware.header.normal_input_endpoint)
            .unwrap_or("(invalid)")
            .trim_end_matches('\0'));
    println!("Normal Output: {}", 
        std::str::from_utf8(&firmware.header.normal_output_endpoint)
            .unwrap_or("(invalid)")
            .trim_end_matches('\0'));
    
    // Display commands
    println!("\n=== Update Commands ===");
    println!("Reset to Update Mode: {}", 
        std::str::from_utf8(&firmware.header.reset_to_update_mode_cmd)
            .unwrap_or("(invalid)")
            .trim_end_matches('\0'));
    println!("Prepare Download: {}", 
        std::str::from_utf8(&firmware.header.prepare_download_cmd)
            .unwrap_or("(invalid)")
            .trim_end_matches('\0'));
    println!("Data Download: {}", 
        std::str::from_utf8(&firmware.header.data_download_cmd)
            .unwrap_or("(invalid)")
            .trim_end_matches('\0'));
    
    // Display firmware stats
    println!("\n=== Firmware Data ===");
    println!("Firmware Size: {} bytes", firmware.firmware_data.len());
    println!("Total File Size: {} bytes", firmware.raw_data.len());
    
    // Calculate some basic statistics
    let first_bytes = &firmware.firmware_data[..std::cmp::min(16, firmware.firmware_data.len())];
    println!("First 16 bytes: {:02X?}", first_bytes);
    
    if firmware.firmware_data.len() >= 16 {
        let last_bytes = &firmware.firmware_data[firmware.firmware_data.len()-16..];
        println!("Last 16 bytes: {:02X?}", last_bytes);
    }
    
    // Check for known version
    println!("\n=== Compatibility ===");
    let known_versions = crate::firmware_operations::known_firmware::KNOWN_VERSIONS;
    if let Some(known) = known_versions.iter().find(|v| v.version == firmware.header.version) {
        println!("✅ Known firmware version: {}", known.version_string);
        println!("   Expected VID:PID: {:04X}:{:04X}", known.vid, known.pid);
        println!("   Original filename: {}", known.filename);
    } else {
        println!("⚠️  Unknown firmware version - proceed with caution");
    }
    
    Ok(())
}