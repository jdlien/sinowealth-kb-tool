# Lofree Flow Lite Firmware Update Protocol Analysis

## Overview
This analysis is based on Wireshark capture of the official Lofree Key Mapper tool performing a firmware update on the Lofree Flow Lite keyboard. The capture shows the complete USB HID protocol used for firmware updates.

## Device Information
- **Runtime Device**: VID: 0x05AC, PID: 0x024F (appears as Apple keyboard)
- **Bootloader Device**: VID: 0x3554, PID: 0xF808 (Sinowealth bootloader)
- **MCU**: CX53730 (Sinowealth)

## Protocol Analysis

### 1. Device Enumeration
The update process starts with the keyboard in runtime mode (VID:05AC PID:024F). The keyboard then switches to bootloader mode (VID:3554 PID:F808) where the actual firmware update occurs.

### 2. Command Structure
All commands are 65-byte HID reports with the following structure:
- **Report ID**: 0x06 (first byte)
- **Command/Data**: 64 bytes

### 3. Update Protocol Commands

#### Initial Setup Commands
```
06b00000000000dec821a4c907000000000000000000000000000000000000000000000000000000000000000000000000
065bb502000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
065bb502010000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
```

**Command 1**: `06b0000000...` - Initial bootloader command
**Command 2**: `065bb50200...` - Setup command (possibly enter write mode)
**Command 3**: `065bb50201...` - Setup command variant

#### Data Write Commands
The bulk of the firmware is written using commands with this pattern:
```
06b1c02000080016[ADDR][DATA][CHECKSUM]
```

**Structure**:
- `06` - Report ID
- `b1` - Write command
- `c020` - Unknown (possibly flags)
- `00080016` - Chunk info (8 bytes, starting at 0x1600)
- `[ADDR]` - 4-byte address (little-endian)
- `[DATA]` - 32 bytes of firmware data
- `[CHECKSUM]` - 4-byte checksum

**Address Pattern**:
- Starts at 0x00001600
- Increments by 0x20 (32 bytes) for each chunk
- Last address observed: 0x0000F4C0

#### Final Commands
```
06b1c108000800f4c000000000000000009122e5bd9c945a63ffffffffffffffffffffffffffffffffffffffffffffffff
065bb505000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
065bb588000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
```

**Final Write**: Uses `c108` instead of `c020` - possibly indicating last chunk
**Finalize 1**: `065bb50500...` - Finalization command
**Finalize 2**: `065bb50588...` - Final command (possibly exit/reboot)

### 4. Address Range
- **Start Address**: 0x00001600
- **End Address**: 0x0000F4C0
- **Total Size**: ~62KB (62,144 bytes)
- **Chunk Size**: 32 bytes per packet

### 5. Command Types Identified

| Command | Description |
|---------|-------------|
| `06b0...` | Bootloader initialization |
| `065bb502XX` | Setup/mode commands |
| `06b1c020...` | Data write (normal chunks) |
| `06b1c108...` | Data write (final chunk) |
| `065bb505XX` | Finalization commands |

### 6. Implementation Notes

1. **Checksums**: Each data packet includes a 4-byte checksum at the end
2. **Sequential Writes**: Data must be written sequentially from 0x1600 upward
3. **Fixed Chunk Size**: Always 32 bytes per write command
4. **Address Alignment**: Addresses are 32-byte aligned (0x20 increments)

### 7. Firmware File Structure
The firmware appears to be loaded starting at offset 0x1600 in the flash memory. The binary file should be:
- Padded to 32-byte boundaries
- Written sequentially starting at 0x1600
- Total size approximately 62KB

### 8. Error Handling
The capture shows no error responses, suggesting the original firmware was successfully written. Error handling would need to be implemented based on device responses.

## Next Steps for Implementation

1. **Entry Mode**: Implement command to switch device to bootloader mode
2. **Setup Sequence**: Send the initial setup commands (0x5bb502XX series)
3. **Data Writing**: Break firmware into 32-byte chunks and send with proper addressing
4. **Checksums**: Calculate and include proper checksums for each chunk
5. **Finalization**: Send completion commands to finalize and reboot device

## Sample Implementation Pseudocode

```rust
// Enter bootloader mode (if needed)
send_command(0x06, [0xb0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xde, 0xc8, 0x21, 0xa4, 0xc9, 0x07, ...]);

// Setup commands
send_command(0x06, [0x5b, 0xb5, 0x02, 0x00, ...]);
send_command(0x06, [0x5b, 0xb5, 0x02, 0x01, ...]);

// Write firmware data
let mut address = 0x1600u32;
for chunk in firmware.chunks(32) {
    let mut cmd = vec![0xb1, 0xc0, 0x20, 0x00, 0x08, 0x00, 0x16];
    cmd.extend_from_slice(&address.to_le_bytes());
    cmd.extend_from_slice(chunk);
    // Calculate and append checksum
    let checksum = calculate_checksum(&cmd);
    cmd.extend_from_slice(&checksum);
    send_command(0x06, cmd);
    address += 32;
}

// Finalize
send_command(0x06, [0x5b, 0xb5, 0x05, 0x00, ...]);
send_command(0x06, [0x5b, 0xb5, 0x88, 0x00, ...]);
```