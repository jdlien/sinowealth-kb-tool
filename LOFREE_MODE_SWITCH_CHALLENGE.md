# Lofree Flow Lite Runtime-to-Bootloader Mode Switch Challenge

## Overview

We are working on implementing firmware updates for the Lofree Flow Lite keyboard (CX53730 MCU) and have successfully reverse-engineered the complete firmware update protocol. However, we're facing a specific challenge with reliably switching the device from runtime mode (VID:05AC PID:024F) to bootloader mode (VID:3554 PID:F808) via software commands.

## What We've Successfully Accomplished

### 1. Complete Protocol Reverse Engineering
- **Runtime Device**: VID:05AC PID:024F (appears as Apple keyboard)
- **Bootloader Device**: VID:3554 PID:F808 (Sinowealth bootloader)
- **Complete firmware update protocol** discovered via Wireshark analysis
- **Critical bootloader exit command** identified: `06b1c108000800f4c0000000000000009122e5bd9c945a63ffffffffffffffffffffffffffffffffffffffffffffffff`

### 2. Firmware Update Implementation
- Full 32-byte chunked firmware writing protocol implemented
- Proper LJMP footer generation with CRC16 validation
- Complete initialization and finalization command sequences
- **NEW**: Proper bootloader exit command that prevents devices getting stuck in DFU mode

### 3. Device Detection and Interface Handling
- Correct interface identification (runtime uses interface 1, report ID 8)
- Proper HID report structure (65-byte reports with report ID 0x06)
- Device enumeration and specification handling

## The Challenge: Runtime-to-Bootloader Mode Switch

### Previous Success
The user reports that in earlier iterations, we were able to successfully get the device into bootloader mode, but this capability was lost during subsequent development.

### Current Mode Switch Attempts

#### 1. Original Wireshark Command
From our capture analysis, we identified this command sequence:
```
080d000000000000000000000000000040
```

**Implementation**:
```rust
let mode_switch_cmd = vec![0x08, 0x0d, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40];
self.cmd_device.write(&mode_switch_cmd) // Sent as output report
```

**Result**: Command sends successfully (no errors), but device doesn't switch to bootloader mode.

#### 2. Bootloader Initialization Command
Based on protocol analysis, we tried sending the bootloader init command to runtime device:
```
06b00000000000dec821a4c907000000000000000000000000000000000000000000000000000000000000000000000000
```

**Result**: `IOHIDDeviceSetReport failed: (0xE0005000)` - Runtime device rejects bootloader commands (expected).

#### 3. ISP Mode Commands
Tried various ISP-related commands based on previous test files:
- `0x75` (ISP mode)
- `0x5a` (reboot)
- `0x55` (enable firmware)

**Result**: Commands send without errors but no mode transition occurs.

### Interface and Report ID Analysis

**Runtime Device (05AC:024F)**:
- Interface 0: Standard keyboard (usage_page=0x0001 usage=0x0006)
- Interface 1: **Vendor-specific** (usage_page=0xff02 usage=0x0002) with **Report ID 8**
- Interface 2: Mouse functionality

**Our Implementation**:
- Using interface 1 (correct for vendor commands)
- Using report ID 8 (matches device capabilities)
- Commands send successfully (no HID errors)

### Current Status

1. **Mode switch commands send without errors** - indicates correct interface/report usage
2. **Device remains in runtime mode** - commands aren't triggering the expected transition
3. **Previous implementation worked** - suggests we're missing a key detail
4. **Official tool works perfectly** - demonstrates the transition is definitely possible

## Technical Environment

- **Platform**: macOS with HIDAPI
- **Device Interface**: Using interface 1, report ID 8 for vendor commands
- **HID Access**: Proper exclusive access handling for macOS
- **Command Format**: Following exact Wireshark capture format

## Specific Questions for Analysis

1. **Command Timing**: Are there specific timing requirements between commands or after sending the mode switch command?

2. **Command Sequence**: Does the mode switch require a specific sequence of commands rather than a single command?

3. **Device State**: Could the device need to be in a specific state (e.g., specific firmware version, configuration) to accept mode switch commands?

4. **Hardware Requirements**: Do some Lofree Flow Lite variants require physical intervention (key combinations during power-on) to enter bootloader mode?

5. **Alternative Protocols**: Could there be a different protocol or command structure for mode switching that we haven't discovered?

6. **USB Descriptor Analysis**: Should we examine the device's USB descriptors more carefully for additional interfaces or capabilities?

## What We Need

**Primary Goal**: Reliable software-triggered transition from runtime mode (05AC:024F) to bootloader mode (3554:F808).

**Secondary Goals**: 
- Understanding why our previous implementation worked but current one doesn't
- Identifying any missing steps in the mode switch sequence
- Determining if hardware intervention is required for some device variants

## Additional Context

- The firmware update protocol itself is fully working when device is in bootloader mode
- We have the complete command sequences for firmware writing and bootloader exit
- The critical achievement is that we've solved the "stuck in bootloader" problem with the proper exit command
- Manual bootloader entry (if available) could validate our complete implementation

## Request

We need guidance on identifying the missing piece for reliable runtime-to-bootloader mode switching. Given that this worked previously and the official tools accomplish this reliably, there must be a discoverable solution.

Any insights into alternative command sequences, timing requirements, device state prerequisites, or analysis techniques would be greatly appreciated.