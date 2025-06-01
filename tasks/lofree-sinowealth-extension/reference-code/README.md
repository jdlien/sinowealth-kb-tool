# Lofree Flow Lite Reference Implementation

This directory contains a reference implementation of Lofree Flow Lite keyboard support, ported from the decompiled Windows firmware updater tool. This code is designed to be integrated into the [sinowealth-kb-tool](https://github.com/carlossless/sinowealth-kb-tool) project to provide cross-platform firmware update capabilities.

## Overview

The reference implementation provides:

- **Firmware Format Parsing**: Complete 720-byte header structure and firmware file validation
- **USB Protocol**: Command structures and communication patterns 
- **Device Support**: VID/PID detection for both Lofree branded (3554:F811) and Apple-compatible (05AC:024F) modes
- **Safety Features**: Firmware compatibility validation and CRC checking

## File Structure

```
reference-code/
├── mod.rs                    # Main module with public API
├── firmware_format.rs        # Firmware file structure and parsing
├── usb_protocol.rs          # USB communication protocol
├── firmware_operations.rs   # Firmware read/write operations
├── examples/
│   ├── parse_firmware.rs    # Example firmware file parser
│   └── device_detection.rs  # Example device detection
├── Cargo.toml              # Rust project configuration
└── README.md               # This file
```

## Key Features

### Firmware Format Support
- 720-byte upgrade file header parsing
- CRC32 validation for integrity checking
- Support for embedded firmware extraction
- Version parsing and compatibility checking

### Device Detection  
- Lofree Flow Lite VID/PID combinations:
  - `3554:F811` (Lofree branded mode)
  - `05AC:024F` (Apple-compatible mode)
- Endpoint configuration for different device modes
- Device information structures (CID/MID/DeviceType)

### USB Communication
- Complete command set from original Windows tool
- Command serialization/deserialization
- Upgrade mode handling
- Flash read/write operations

## Integration with sinowealth-kb-tool

To integrate this into sinowealth-kb-tool:

### 1. Add Device Support
```rust
// In src/devices/mod.rs
pub mod lofree;

// Add Lofree device detection
```

### 2. Create Lofree Module
```
src/devices/lofree/
├── mod.rs              # Main lofree module  
├── protocol.rs         # USB communication (from usb_protocol.rs)
├── firmware.rs         # Firmware handling (from firmware_operations.rs)
└── device_info.rs      # Device detection (from firmware_format.rs)
```

### 3. Extend Command Line Interface
```bash
# Read firmware
sinowealth-kb-tool --device lofree-flow-lite --vid 3554 --pid F811 --read-firmware backup.bin

# Write firmware  
sinowealth-kb-tool --device lofree-flow-lite --vid 3554 --pid F811 --write-firmware custom.bin --verify

# Device info
sinowealth-kb-tool --device lofree-flow-lite --vid 3554 --pid F811 --info
```

## Supported Firmware Versions

The implementation supports firmware versions V1.54 through V1.66:

- V1.54_0104 (VID:3554 PID:F811)
- V1.55_0103 (VID:05AC PID:024F) 
- V1.55_0105 (VID:05AC PID:024F)
- V1.56_0106 (VID:3554 PID:F811)
- V1.64_0101 (VID:05AC PID:024F)
- V1.66_0102 (VID:05AC PID:024F)

## Safety Features

### Validation Checks
- Firmware file CRC32 validation
- Device compatibility verification (CID/MID/DeviceType)
- Version compatibility checking
- Size validation for firmware files

### Error Handling
- Comprehensive error messages
- Safe fallbacks for unknown devices
- Validation before any write operations

## Example Usage

### Parse Firmware File
```rust
use lofree_flow_lite_reference::FirmwareFile;

let firmware = FirmwareFile::load_from_file("firmware.bin")?;
println!("Version: {}", firmware.version_string());
println!("Compatible with device: {}", 
    firmware.is_compatible_with_device(&device_info, &vid_pid));
```

### Device Detection
```rust
use lofree_flow_lite_reference::LofreeLiteDevice;

if LofreeLiteDevice::is_supported_device(0x3554, 0xF811) {
    let device = LofreeLiteDevice::lofree_mode();
    // Proceed with operations
}
```

## Dependencies

The reference implementation uses minimal dependencies:

- `crc32fast`: CRC32 calculation for firmware validation
- Standard library only for core functionality

When integrating with sinowealth-kb-tool, you'll also use:
- `hidapi`: HID device communication (already used by sinowealth-kb-tool)
- `anyhow`: Error handling (already used by sinowealth-kb-tool)

## Testing

Run the examples to test firmware parsing:

```bash
# Parse a firmware file
cargo run --example parse_firmware -- /path/to/firmware.bin

# Test device detection
cargo run --example device_detection
```

## Implementation Notes

### Windows DLL Replacement
The original Windows tool uses two native DLLs (`hidusb.dll`, `usbfile.dll`) with 60+ P/Invoke calls. This reference implementation shows the protocol structures and logic, but actual HID communication needs to be implemented using `hidapi` or similar cross-platform libraries.

### Firmware Format
The 720-byte header format is well-defined and portable. The structure uses packed C representation for exact byte layout compatibility.

### Command Protocol
USB commands follow a standard pattern with report ID, command ID, status, address, and data fields. The protocol is HID-based and should work with standard HID libraries.

### Safety First
Always validate firmware compatibility before writing. The reference implementation includes multiple validation layers to prevent device damage.

## Contributing

When extending this code:

1. Maintain compatibility with existing firmware versions
2. Add comprehensive validation for new features  
3. Test thoroughly with real hardware
4. Follow the existing error handling patterns
5. Add appropriate safety warnings for destructive operations

## License

This reference implementation is based on analysis of the decompiled Lofree firmware updater tool. Use at your own risk - firmware updates can potentially damage devices if done incorrectly.