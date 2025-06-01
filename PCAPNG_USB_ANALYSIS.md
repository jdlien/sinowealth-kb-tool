# Lofree Flow Lite USB Capture Analysis

## Overview
Analysis of the pcapng USB capture file `lofree-flow-lite-oe921-keyboard-upgrade-tool-capture.pcapng` showing complete firmware upgrade sequence from runtime mode to bootloader and back to runtime.

## Device Mode Transitions

### 1. Initial Runtime Mode Detection
- **Timestamp**: 0.000000000s
- **Device**: VID:05AC PID:024F (Apple/Lofree runtime mode)
- **Frame**: 2

### 2. Bootloader Entry Sequence
- **Timestamp**: 9.302923s - 9.883926s
- **Critical Frames**: 45-62
- **Device Transition**: Runtime (05AC:024F) → Bootloader (3554:F808)

#### Key Commands for Bootloader Entry:
```
Frame 45-50: Interface disable sequence (endpoint 0x81/0x82/0x83 with 0xfe transfer type)
Frame 51: USB device descriptor request
Frame 52: NEW DEVICE DETECTED - VID:3554 PID:F808 (Sinowealth bootloader)
```

#### Bootloader Entry Command Sequence:
From frame 61 hex analysis:
```
09 06 02 00 00 31 00 06 b0 00 00 00 00 00 de c8
21 a4 c9 07 00 00 00 00 00 00 00 00 00 00 00 00
```

This appears to be a vendor-specific command that triggers bootloader entry.

### 3. Firmware Upload Process
- **Duration**: 9.883926s - 16.281245s (~6.4 seconds)
- **Pattern**: Continuous USB interrupt transfers on endpoint 0x81
- **Data Transfer**: Extensive data exchange via control transfers (endpoint 0x00)

#### Firmware Writing Pattern:
- Control transfers (0x00, type 0x02) for firmware data
- Interrupt transfers (0x81, type 0x01) for status/acknowledgment
- Regular 2ms intervals between transfers

### 4. Critical Bootloader Exit Sequence
- **Timestamp**: 16.281245s - 18.007947s
- **Duration**: ~1.7 seconds gap
- **Key Frames**: 7210-7214

#### Bootloader Exit Commands:
```
Frame 7211-7212: Final endpoint 0x81 transfers with type 0xfe
  - This appears to be the "exit bootloader" command
  - Transfer type 0xfe indicates vendor-specific operation
  - ~1.7 second delay after this command

Frame 7213: USB device reset/re-enumeration begins
Frame 7214: NEW DEVICE DETECTED - VID:05AC PID:024F (back to runtime mode)
```

### 5. Runtime Mode Recovery
- **Timestamp**: 18.007947s
- **Device**: VID:05AC PID:024F (Apple/Lofree runtime mode)
- **Frame**: 7214

## Critical Findings

### Bootloader Exit Mechanism
The key discovery is the **endpoint 0x81 with transfer type 0xfe** sequence in frames 7211-7212:

1. **Final Command**: Interrupt transfer on endpoint 0x81 with type 0xfe
2. **Timing**: ~1.7 second delay after this command  
3. **Result**: Device re-enumerates as runtime device (05AC:024F)

### Interface Usage Pattern
- **Endpoint 0x00**: Control transfers for firmware data
- **Endpoint 0x81**: Interrupt transfers for status and exit commands
- **Endpoint 0x82/0x83**: Additional interfaces used during transitions

### Timing Critical Points
1. **Entry Delay**: ~9.3 seconds from start to bootloader entry
2. **Upload Duration**: ~6.4 seconds for firmware transfer
3. **Exit Delay**: ~1.7 seconds from exit command to re-enumeration
4. **Total Process**: ~18 seconds end-to-end

## Command Sequences

### Bootloader Entry Command
```
USB Control Transfer:
09 06 02 00 00 31 00 06 b0 00 00 00 00 00 de c8
21 a4 c9 07 [padding...]
```

### Bootloader Exit Command
```
USB Interrupt Transfer (Endpoint 0x81, Type 0xFE):
[Empty payload - the transfer type itself is the command]
```

## Implementation Recommendations

### For Rust Implementation
1. **Entry Sequence**: Send vendor command `09 06 02 00 00 31 00 06 b0 00...` 
2. **Upload Process**: Use control transfers on endpoint 0x00 with 2ms intervals
3. **Exit Sequence**: Send interrupt transfer to endpoint 0x81 with type 0xfe
4. **Timing**: Wait 1.7+ seconds after exit command before expecting runtime device

### Critical Success Factors
1. **Exact Command**: The bootloader exit requires endpoint 0x81 with transfer type 0xfe
2. **Timing**: Must wait for device re-enumeration (1.7s observed)
3. **Interface Selection**: Use correct USB interface for each command type
4. **Error Handling**: Monitor for device VID/PID changes during transitions

## Comparison with Previous Analysis
This capture confirms the missing piece from previous Wireshark analysis - the **transfer type 0xfe on endpoint 0x81** is the specific command that triggers bootloader exit and device re-enumeration to runtime mode.