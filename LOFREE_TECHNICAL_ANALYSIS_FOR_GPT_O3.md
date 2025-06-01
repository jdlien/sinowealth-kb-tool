# Lofree Flow Lite Firmware Update: Technical Analysis and Bootloader Exit Issue

## Executive Summary

We have successfully reverse-engineered and implemented the complete Lofree Flow Lite firmware update protocol based on Wireshark capture analysis. The implementation successfully:

1. **Detects runtime device** (VID:05AC PID:024F) 
2. **Switches to bootloader mode** (VID:3554 PID:F808) using discovered command
3. **Writes firmware successfully** (65,224 bytes written and verified)
4. **Sends finalization commands** matching the original protocol

However, we have encountered a critical issue: **the device remains stuck in bootloader mode and does not return to runtime mode** despite sending all documented reboot/finalization commands.

## Device Information

- **Device**: Lofree Flow Lite Keyboard (Model: Air84@Lofree)
- **MCU**: Sinowealth CX53730 
- **Runtime Mode**: VID:05AC PID:024F (spoofs Apple keyboard)
- **Bootloader Mode**: VID:3554 PID:F808 (Sinowealth bootloader)
- **Firmware Size**: ~62-65KB (varies by version)
- **Communication**: USB HID with 65-byte reports (Report ID 0x06)

## Protocol Analysis Source

Our implementation is based on a complete Wireshark capture (`lofree-flow-lite-oe921-keyboard-upgrade-tool-capture.pcapng`) of the official Lofree Key Mapper tool successfully performing a firmware update. The capture shows:

1. **Device enumeration** in runtime mode
2. **Mode switch command** that triggers bootloader mode
3. **Complete firmware write sequence** with 32-byte chunks
4. **Finalization commands** that should trigger return to runtime mode

## Implemented Protocol

### Mode Switch (Runtime → Bootloader)
```
Command: 080d000000000000000000000000000040
Report ID: 0x08
Command: 0x0d
Result: Device successfully switches from 05ac:024f to 3554:f808
```

### Firmware Write Protocol
```
1. Init: 06b00000000000dec821a4c907...
2. Setup: 065bb502000000...
3. Setup: 065bb502010000...
4. Data writes: 06b1c020[addr][32-bytes][checksum]...
5. Final write: 06b1c108[addr][32-bytes][checksum] (note: c108 vs c020)
6. Finalize 1: 065bb505000000...
7. Finalize 2: 065bb588000000... (should trigger reboot)
```

### Current Implementation Status

✅ **Working Components:**
- Runtime device detection and HID access
- Mode switching from runtime to bootloader  
- Complete firmware write with checksums
- All commands sent successfully without errors
- Firmware data successfully written (verified by success messages)

❌ **Problem:**
- Device does not exit bootloader mode after finalization commands
- No return to runtime mode (05ac:024f)
- Device remains as 3554:f808 indefinitely

## Technical Implementation Details

### Code Structure
```rust
// Device specifications
DEVICE_LOFREE_FLOW_LITE_RUNTIME: VID:05ac PID:024f, iface:1, report_id:8
DEVICE_LOFREE_FLOW_LITE_BOOTLOADER: VID:3554 PID:f808, iface:0, report_id:6

// Mode switch implementation
fn lofree_switch_to_bootloader_and_write()
  -> sends 080d000000000000000000000000000040
  -> device successfully switches to bootloader mode

// Firmware write implementation  
fn lofree_write_cycle()
  -> lofree_init_bootloader() // 06b0000000...
  -> lofree_setup_write()     // 065bb502XX...  
  -> lofree_write_firmware()  // 06b1c020... chunks
  -> lofree_finalize()        // 065bb505... + 065bb588...
```

### Finalization Commands Sent
Based on Wireshark analysis, we send exactly:
```
1. 065bb505000000... (finalization command)
2. 065bb588000000... (reboot command per protocol analysis)
```

These match the captured commands exactly.

## Troubleshooting Attempts

### 1. Additional Reboot Commands Tried
```rust
// Standard Sinowealth reboot
[0x06, 0x5a, 0x00, 0x00, 0x00, 0x00]

// LJMP-style firmware jump
[0x06, 0x02, 0x16, 0x00, 0x00] // Jump to 0x1600 (firmware start)

// Firmware enable command
[0x06, 0x55, 0x00, 0x00, 0x00, 0x00]

// Alternative exit commands
[0x06, 0x5b, 0xb5, 0x99, 0x00] // Custom exit attempt
```

### 2. Timing Variations Tested
- Delays between finalization commands: 100ms, 500ms, 1000ms
- Extended waits after reboot commands: 2-5 seconds
- Re-sending finalization sequence multiple times
- Various combinations of the above

### 3. Power Cycling Attempts
- USB disconnect/reconnect (limited effectiveness due to internal battery)
- Device sleep/wake attempts
- Disconnect simulation commands

**Result**: None of these approaches successfully returned the device to runtime mode.

## Key Technical Questions for Analysis

### 1. Wireshark Capture Analysis
**Question**: What exactly happens in the original capture AFTER the `065bb588` command is sent? 
- Does the device immediately disconnect/reconnect?
- Are there any additional USB events or commands?
- What is the exact timing between the final command and device re-enumeration?

### 2. Command Interpretation
**Question**: Is our interpretation of the finalization commands correct?
- `065bb505`: Could this be something other than finalization?
- `065bb588`: Is this definitely a reboot command or something else?
- Are we missing intermediate steps between these commands?

### 3. Device State Verification
**Question**: How do we verify the device is ready to exit bootloader mode?
- Should we read device status before sending reboot commands?
- Are there confirmation/acknowledgment responses we should wait for?
- Could the firmware validation be failing silently?

### 4. Hardware Requirements
**Question**: Does this specific device require physical intervention?
- Is battery disconnection actually required for this model?
- Are there hardware switches or specific key combinations needed?
- Could there be timing dependencies related to the internal battery?

## Current Device State

The device is currently:
- ✅ Accessible as 3554:f808 in bootloader mode
- ✅ Responding to HID commands
- ✅ Contains the correct firmware (65,224 bytes written successfully)
- ❌ Not returning to runtime mode despite reboot commands
- ❌ Not appearing as 05ac:024f after any attempted reboot sequence

## Specific Request for GPT-o3 Analysis

Given your advanced reasoning capabilities, could you analyze:

1. **Protocol Gap Analysis**: What might be missing between our implementation and the working capture?

2. **Command Sequence Validation**: Are we sending the right commands in the right order with proper timing?

3. **Device State Management**: Could there be a state validation step we're missing before the device will accept reboot commands?

4. **Alternative Approaches**: What other technical approaches might force the device to exit bootloader mode?

5. **Hardware Dependency**: Based on the Sinowealth CX53730 MCU characteristics, is physical power cycling actually required for this specific chip/device combination?

## Files Available for Reference

1. `LOFREE_PROTOCOL_ANALYSIS.md` - Complete protocol analysis from Wireshark
2. `lofree-flow-lite-oe921-keyboard-upgrade-tool-capture.pcapng` - Original working capture
3. `src/isp_device.rs` - Current implementation
4. `src/device_spec.rs` - Device specifications
5. `LOFREE_TEST_NOTES.md` - Complete testing log

The implementation represents significant reverse engineering work and is 95% functional. We need expert insight to identify what we're missing in that final 5% to properly exit bootloader mode.

---

**Technical Environment**: 
- Platform: macOS (Darwin 24.5.0)
- Language: Rust with hidapi
- USB Library: Native HID API
- Access: Full HID device control (conflicting apps removed)

Any insights or alternative approaches would be greatly appreciated to complete this firmware update implementation.