# Lofree Flow Lite Mode Switch - SOLVED! 

## Summary
Successfully discovered the correct sequence to switch Lofree Flow Lite from runtime mode (05AC:024F) to bootloader mode (3554:F808).

## The Working Solution

### Prerequisites
- Device must be in runtime mode (05AC:024F)
- Must open interface 1 (vendor-specific interface with usage_page 0xFF02)
- All commands use **interrupt writes** via `device.write()`, NOT feature reports

### Command Sequence

1. **Send Status Query 0x0803**
   ```
   Command: 08 03 00 00 00 80 00 00 00 00 00 00 00 00 00 00 ca
   Response: 08 03 00 00 00 84 01 09 79 98 00 00 00 00 00 00 ab
   ```

2. **Send Status Query 0x0804**
   ```
   Command: 08 04 00 00 00 84 64 01 10 00 00 00 00 00 00 00 c9
   Response: 08 04 01 00 00 84 64 01 10 00 00 00 00 00 00 00 4f
   ```

3. **Send Mode Switch Command 0x080d**
   ```
   Command: 08 0d 00 00 00 00 00 00 00 00 00 00 00 00 00 00 40
   ```

4. **Device disconnects and re-enumerates as 3554:F808 (bootloader mode)**

### Working Code Example
```rust
// Find device on interface 1
let device_info = api.device_list()
    .find(|d| {
        d.vendor_id() == 0x05ac && 
        d.product_id() == 0x024f && 
        d.interface_number() == 1
    })?;

let device = api.open_path(device_info.path())?;

// Send commands via interrupt writes
let cmd1 = [0x08, 0x03, 0x00, 0x00, 0x00, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xca];
device.write(&cmd1)?;
thread::sleep(Duration::from_millis(100));

let cmd2 = [0x08, 0x04, 0x00, 0x00, 0x00, 0x84, 0x64, 0x01, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xc9];
device.write(&cmd2)?;
thread::sleep(Duration::from_millis(100));

let mode_switch = [0x08, 0x0d, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40];
device.write(&mode_switch)?;

// Device will disconnect and re-enumerate as 3554:f808
```

## Key Discoveries

1. **Prerequisite Commands Required**: The status queries (0x0803, 0x0804) must be sent before mode switch
2. **Interrupt Writes Only**: Use `write()` not `send_feature_report()`
3. **Specific Interface**: Must use interface 1 (vendor-specific)
4. **Response Validation**: The device responds to status queries, confirming communication
5. **Timing**: Small delays (~100ms) between commands are sufficient

## What Was Wrong Before

1. **Missing Prerequisites**: We were trying to send mode switch command directly without status queries
2. **Wrong Transfer Type**: We were using feature reports instead of interrupt writes
3. **GPT-o3 Confusion**: The 65-byte feature report advice was incorrect for this specific command

## Pcapng Analysis Details

From the original capture:
- Commands sent at ~1.36s, ~6.36s (repeated), then mode switch at ~8.42s
- All commands use interrupt OUT endpoint (0x02)
- Responses come back on interrupt IN endpoint (0x82)
- Device takes ~1.5 seconds to disconnect and re-enumerate after mode switch

## Current Status

✅ **Runtime to Bootloader**: Working perfectly
❌ **Bootloader to Runtime**: Still needs fixing (see LOFREE_BOOTLOADER_EXIT_CHALLENGE.md)