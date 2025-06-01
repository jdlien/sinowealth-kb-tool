# Lofree Flow Lite Firmware Development Notes

## Overview
This document consolidates all technical knowledge about the Lofree Flow Lite keyboard firmware update process, gathered through extensive reverse engineering, USB protocol analysis, and implementation in the sinowealth-kb-tool project.

## Device Information

### Hardware Specifications
- **Keyboard Model**: Lofree Flow Lite (Air84@Lofree)
- **MCU**: Sinowealth CX53730 (CX53-series 8051 SoC, 64-pin TQFP)
- **USB Modes**:
  - **Runtime Mode**: VID:05AC PID:024F (spoofs Apple keyboard)
  - **Bootloader Mode**: VID:3554 PID:F808 (Sinowealth bootloader)
  - **Alternative PIDs**: F811 (some firmware versions)

### Memory Layout
| Region         | Start Address | Size        | Notes                    |
|----------------|---------------|-------------|--------------------------|
| Bootloader     | 0x0000       | 0x1600      | 5,632 bytes protected    |
| User Firmware  | 0x1600       | 0xF4C0      | 62,144 bytes writable    |
| Total Flash    | 0x0000       | 0x10000     | 64KB total              |

### USB HID Configuration
- **Interface Count**: 3 (keyboard, vendor-specific, mouse)
- **Report IDs**:
  - Runtime Mode: Report ID 0x08 on interface 1
  - Bootloader Mode: Report ID 0x06 on interface 0
- **Report Size**: 65 bytes (1 byte report ID + 64 bytes data)

## Protocol Analysis

### Mode Switching

#### Runtime to Bootloader Transition
Successfully reverse-engineered from Wireshark captures:

1. **Prerequisites** (via interrupt writes on interface 1):
   ```
   Command 1: 08 03 00 00 00 80 00 00 00 00 00 00 00 00 00 00 ca
   Response:  08 03 00 00 00 84 01 09 79 98 00 00 00 00 00 00 ab
   
   Command 2: 08 04 00 00 00 84 64 01 10 00 00 00 00 00 00 00 c9
   Response:  08 04 01 00 00 84 64 01 10 00 00 00 00 00 00 00 4f
   ```

2. **Mode Switch Command**:
   ```
   08 0d 00 00 00 00 00 00 00 00 00 00 00 00 00 00 40
   ```

3. **Result**: Device disconnects and re-enumerates as VID:3554 PID:F808

### Firmware Update Protocol

#### Command Structure
All bootloader commands use 65-byte HID reports:
- **Report ID**: 0x06 (first byte)
- **Command/Data**: 64 bytes

#### Update Sequence

1. **Bootloader Initialization**:
   ```
   06 b0 00 00 00 00 00 de c8 21 a4 c9 07 00 00 00 ... (padded to 65 bytes)
   ```

2. **Setup Commands**:
   ```
   06 5b b5 02 00 00 00 00 00 00 00 00 00 00 00 00 ... (enter write mode)
   06 5b b5 02 01 00 00 00 00 00 00 00 00 00 00 00 ... (setup variant)
   ```

3. **Data Write Commands**:
   ```
   Structure: 06 b1 c0 20 00 08 00 16 [ADDR-4bytes] [DATA-32bytes] [CHECKSUM-4bytes]
   
   - Command: b1 (write command)
   - Flags: c0 20 (normal chunk) or c1 08 (final chunk)
   - Address: Little-endian, starts at 0x1600, increments by 0x20
   - Data: 32 bytes of firmware
   - Checksum: 4-byte checksum
   ```

4. **LJMP Footer** (Critical Discovery):
   Must be written at firmware_size - 5 bytes:
   ```
   02 00 00 [CRC_LO] [CRC_HI]
   
   - 02 00 00: LJMP instruction to address 0x0000
   - CRC: CRC16-CCITT of entire firmware
   ```

5. **Bootloader Exit Command** (Key Discovery):
   ```
   06 b1 c1 08 00 08 00 f4 c0 00 00 00 00 00 00 00 00 
   91 22 e5 bd 9c 94 5a 63 ff ff ff ff ff ff ff ff ... (padded to 65 bytes)
   
   This specific command with the signature bytes triggers bootloader exit!
   ```

6. **Finalization Commands**:
   ```
   06 5b b5 05 00 00 00 00 00 00 00 00 00 00 00 00 ... (finalization)
   06 5b b5 88 00 00 00 00 00 00 00 00 00 00 00 00 ... (trigger reboot)
   ```

### Checksum Algorithm
Simple additive checksum:
```rust
fn calculate_checksum(data: &[u8]) -> u32 {
    data.iter().map(|&b| b as u32).sum()
}
```

## Implementation Status

### Working Features ✅
1. **Device Detection**: Both runtime and bootloader modes
2. **Mode Switching**: Runtime to bootloader transition
3. **Firmware Writing**: Complete 32-byte chunked protocol
4. **Bootloader Exit**: Proper exit sequence prevents devices getting stuck
5. **macOS Compatibility**: Full HID access (after closing conflicting apps)

### Key Implementation Files
- `src/device_spec.rs`: Device specifications for both modes
- `src/isp_device.rs`: Complete protocol implementation
- `src/device_selector.rs`: Mode switching logic

### Critical Success Factors
1. **Interface Selection**: Must use interface 1 for runtime commands
2. **Transfer Types**: Use interrupt writes, not feature reports
3. **Command Sequence**: Status queries required before mode switch
4. **LJMP Footer**: Essential for bootloader to recognize valid firmware
5. **Exit Command**: Specific signature bytes trigger proper exit
6. **macOS Apps**: Must close Karabiner-Elements and BetterTouchTool

## Common Issues and Solutions

### Problem: Device Stuck in Bootloader Mode
**Cause**: Missing or incorrect bootloader exit command
**Solution**: Use the discovered exit command with signature bytes:
```
06 b1 c1 08 00 08 00 f4 c0 00 00 00 00 00 00 00 00 
91 22 e5 bd 9c 94 5a 63 ff ff ff ff ff ff ff ff ...
```

### Problem: macOS HID Access Denied
**Cause**: Conflicting applications holding exclusive access
**Solution**: Close Karabiner-Elements and BetterTouchTool before running

### Problem: Mode Switch Not Working
**Cause**: Missing prerequisite status queries
**Solution**: Send status commands (0x0803, 0x0804) before mode switch

### Problem: Firmware Validation Fails
**Cause**: Missing LJMP footer with CRC
**Solution**: Write LJMP instruction and CRC16 at firmware_size - 5

## Firmware File Format

### Container Structure (ComUsbUpgradeFile)
```
Magic: Q$UU
Header Length: 0x02D0 (720 bytes)
Payload Length: Variable (~57-62KB)
Total Size: ~65KB
```

### Firmware Versions Tested
- V1.54 (VID-3554_PID-F811): 65,252 bytes
- V1.56 (VID-3554_PID-F811): 65,256 bytes
- V1.64 (VID-05AC_PID-024F): 65,224 bytes
- V1.66 (VID-05AC_PID-024F): 65,224 bytes

## USB Capture Analysis Insights

### Timing Requirements
- Mode switch delay: ~1.5 seconds for re-enumeration
- Inter-command delay: 100ms sufficient
- Total update time: ~18 seconds end-to-end

### Critical USB Events
1. Interface disable sequence before mode switch
2. Device descriptor request triggers enumeration
3. Endpoint 0x81 with transfer type 0xfe for bootloader exit

## Development Recommendations

### For New Implementations
1. Study the complete Wireshark captures for timing
2. Implement proper error handling for device disconnection
3. Use interrupt transfers, not control transfers for commands
4. Always write LJMP footer with correct CRC
5. Include the magic exit command sequence

### For Debugging
1. Use USB analyzer to verify command structure
2. Check interface and report ID selection
3. Verify firmware size matches device expectations
4. Monitor device enumeration events
5. Test with official firmware files first

## Future Work

### Protocol Extensions
- Investigate read-back verification protocol
- Document error response codes
- Analyze alternative firmware formats

### Tool Improvements
- Add automatic mode detection and switching
- Implement firmware verification after write
- Support for encrypted firmware formats
- Better error messages and recovery options

## References

### Source Files
- Protocol analysis: `LOFREE_PROTOCOL_ANALYSIS.md`
- Mode switch details: `LOFREE_MODE_SWITCH_SUCCESS.md`
- Bootloader exit: `LOFREE_BOOTLOADER_EXIT_CHALLENGE.md`
- Implementation notes: `LOFREE_IMPLEMENTATION_SUMMARY.md`
- Technical analysis: `LOFREE_TECHNICAL_ANALYSIS_FOR_GPT_O3.md`
- Test results: `LOFREE_TEST_NOTES.md`

### Captures
- `lofree-flow-lite-key-mapper-upgrade-capture.pcapng`
- `lofree-flow-lite-oe921-keyboard-upgrade-tool-capture.pcapng`

### Official Tools
- Lofree Key Mapper (Windows)
- Lofree Flow Lite Upgrade Tool

## Summary

The Lofree Flow Lite uses a modified Sinowealth bootloader protocol with specific requirements:
1. Custom mode switch sequence with status queries
2. 32-byte chunked firmware writes with checksums
3. LJMP footer with CRC16 for firmware validation
4. Magic exit command with signature bytes
5. Proper timing between commands

The implementation in sinowealth-kb-tool successfully handles all aspects of the firmware update process, providing a complete open-source solution for Lofree Flow Lite firmware management.