# Lofree Flow Lite Recovery Notes

## Hardware Specifications
- **MCU:** SinoWealth/Compx Tech CX53730Q (CX53-series 8051 SoC, 64-pin TQFP)
- **Bootloader VID/PID:** 0x3554:0xF808 (HID-DFU mode shows as USB Input Device)
- **Runtime VID/PID:** 0x3554:0xF811 (normal operation mode)

## Firmware Details
- **File:** KB_RGB_V1.56_VID-3554_PID-F811_0106.bin
- **Container:** ComUsbUpgradeFile (Q$UU magic)
- **Header length:** 0x02D0 (720 bytes)
- **Payload length:** 0xDEE8 (57,064 bytes)
- **Total file size:** 65,256 bytes (0xFE70)

## Memory Layout
| Region    | Start  | Size                 |
|-----------|--------|----------------------|
| User FW   | 0x0000 | 0xF000 (61,440 B)   |
| DFU stub  | 0xF000 | 0x1000 (4,096 B)    |

Write granularity (page): 0x0800 (2,048 bytes)

## Flash Commands

### Option A: Quick & Dirty (using existing binary with preset alias)
```bash
sudo ./sinowealth-kb-tool write \
  --vendor_id 0x3554 --product_id 0xF808 \
  --part nuphy-air60 \
  --firmware_size 61440 \
  flowlite_fw.hex
```

### Option B: Preferred (custom-device build)
```bash
# Build with custom-device feature
cargo build --release --features custom-device

# Flash directly without header stripping
sudo ./target/release/sinowealth-kb-tool write-raw \
  --vendor_id 0x3554 --product_id 0xF808 \
  --firmware_size 61440 --bootloader_size 4096 --page_size 2048 \
  KB_RGB_V1.56_VID-3554_PID-F811_0106.bin

# Add --isp_iface_num 1 if interface-0 claim fails
```

## Converting to HEX (if needed for older binary)
```bash
# Extract firmware payload (skip 720-byte header)
dd if=KB_RGB_V1.56_VID-3554_PID-F811_0106.bin of=flowlite_fw.bin \
   bs=1 skip=720 count=61440

# Convert BIN to Intel HEX
srec_cat flowlite_fw.bin -Binary -o flowlite_fw.hex -Intel
```

## Platform-Specific Notes
- **macOS:** Run with sudo, no extra driver needed
- **Windows:** Install WinUSB for VID 3554/PID F808 via Zadig
- **Linux:** Use sudo or create udev rule

## Adding Board Definition (Optional)
Edit `src/boards.rs` after building once:
```rust
define_board!(
    lofree_flow_lite,
    "Lofree Flow Lite",
    61440,   // firmware_size
    4096,    // bootloader_size
    2048,    // page_size
    CX,
    0x3554,  // VID
    0xF808   // DFU PID
);
```

Then rebuild and use:
```bash
sudo ./sinowealth-kb-tool write \
  --part lofree-flow-lite \
  KB_RGB_V1.56_VID-3554_PID-F811_0106.bin
```

## Current Issue: Protocol Incompatibility

### Background
The keyboard was bricked during a failed firmware update attempt while connected to a virtual machine. The USB passthrough in the VM likely interrupted during device re-enumeration when switching between normal and bootloader modes.

### Current State
- Keyboard is stuck in bootloader mode (VID: 0x3554, PID: 0xF808)
- Device is detected and accessible via HID
- sinowealth-kb-tool can connect to the device but cannot flash firmware

### Attempted Solutions

1. **Built sinowealth-kb-tool from source**
   - Successfully compiled the tool
   - Note: The `--features custom-device` flag doesn't exist in this version

2. **Added Lofree Flow Lite device definition**
   - Modified `src/device_spec.rs` to add DEVICE_LOFREE_FLOW_LITE
   - Added entry to DEVICES map for "lofree-flow-lite"
   - Specified correct memory layout (61440 firmware, 4096 bootloader, 2048 page size)

3. **Modified device selector logic**
   - Updated `find_device()` to handle devices without feature reports
   - Modified `find_isp_device()` to recognize Lofree's bootloader VID/PID
   - Added special handling for Lofree in the macOS/Linux code path

4. **Attempted various flash commands**
   - Direct binary flash with device profile
   - Extracted firmware payload (skipped 720-byte header)
   - Converted to Intel HEX format
   - Tried with --force flag
   - Tested different interface numbers and report IDs

### Technical Findings

1. **Device Detection**
   - Device shows up with interface_number=0
   - Report descriptor shows Report ID 0x06 for vendor-specific usage (0xff02)
   - Report ID 0xB0 for consumer control
   - No feature_report_ids (empty array)

2. **Protocol Mismatch**
   - sinowealth-kb-tool expects standard Sinowealth ISP protocol
   - Uses Report ID 0x05 for commands (CMD) and 0x06 for transfers (XFER)
   - Lofree appears to use Report ID 0x06 for vendor-specific communication
   - All erase attempts fail with: `hidapi error: IOHIDDeviceSetReport failed: (0xE0005000) unknown error code`

3. **Key Observations**
   - Device is already in bootloader/ISP mode (no need to switch)
   - Standard Sinowealth ISP commands are rejected by the device
   - Suggests Lofree uses a proprietary protocol or modified variant

### Conclusion
The Lofree Flow Lite bootloader appears to use a different protocol than the standard Sinowealth ISP protocol implemented by sinowealth-kb-tool. Without reverse engineering the official Lofree software or protocol documentation, the tool cannot communicate with the bootloader.

### Recommended Next Steps
1. Use the official "Lofree Flowlite Key Mapper" software on a physical Windows machine
2. Once restored to normal operation (VID: 0x3554, PID: 0xF811), investigate:
   - Capture USB traffic during official firmware update
   - Analyze the protocol differences
   - Modify sinowealth-kb-tool to support Lofree's protocol variant