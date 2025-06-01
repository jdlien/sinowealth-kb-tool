Here's a summary of some work I did with it to try to get the keyboard working again. I tried for about six hours in conjunction with GPT o3 and Claude 4 Opus in Claude Code working with modifications to this tool, but I haven't had any luck writing firmware to it using that tool. My initial attempts kept throwing errors so I kept trying with different tweaks to the application. It seems I couldn't quite nail the protocol needed to complete the firmware update, although I was able to connect to the device and communicate with it.

Maybe this technical deep dive will be helpful, but ultimately I'm still not able to write firmware even using a low level tool like this.

**Date:** May 28, 2025  
**Device:** Lofree Flow Lite Keyboard  
**Issue:** Firmware update attempts fail - device remains unresponsive after flashing

## Executive Summary

We conducted extensive analysis of the Lofree Flow Lite firmware update process using the sinowealth-kb-tool. While we successfully established communication with the device's bootloader and confirmed that write operations are accepted, the device consistently fails to exit bootloader mode and remains non-functional after firmware writes.

## Device Information

**Current State:**
- **VID:PID:** 3554:f808 (Bootloader mode)
- **Expected Application PID:** f811 (based on firmware filename)
- **Status:** Device shows as "USB Input Device" with minimal functionality
- **Interface:** 0
- **Report ID:** 0x06

**HID Report Descriptor:**
```
[06 02 FF 09 02 A1 01 85 06 15 00 26 FF 00 75 08 95 30 09 02 81 00 09 02 91 00 C0 
 05 0C 09 01 A1 01 85 B0 15 00 26 3C 02 19 00 2A 3C 02 75 10 95 01 81 00 C0]
```

## Key Findings

### ✅ Confirmed Working

1. **Bootloader Communication**
   - Device is already in ISP (In-System Programming) mode
   - No need to send ISP entry commands
   - Single report ID (0x06) used for all communication

2. **Write Commands Accepted**
   - Erase command (0x45): ✓ Success
   - Init Write command (0x57): ✓ Success  
   - Write Page command (0x77): ✓ Success
   - Enable Firmware command (0x55): ✓ Success
   - Reboot command (0x5a): ✓ Success

3. **Tool Modifications**
   - Successfully modified sinowealth-kb-tool for Flow Lite support
   - Added `--bootloader_ready` flag to skip ISP mode entry
   - Implemented single report ID (0x06) for both command and transfer operations

### ❌ Issues Identified

1. **Read Operations Fail**
   - All read attempts return: `IOHIDDeviceGetReport failed: (0xE0005000)`
   - Unable to verify firmware writes
   - Device appears to not support read-back operations

2. **Device Remains in Bootloader Mode**
   - After successful firmware write + enable + reboot sequence
   - Device always returns as VID:PID 3554:f808
   - Never appears as expected application PID f811
   - No keyboard functionality restored

## Firmware Files Tested

| Firmware File | Size | Result |
|---------------|------|--------|
| `flowlite_fw.bin` | 61,440 bytes | Write claimed success, no function |
| `KB_RGB_V1.54_VID-3554_PID-F811_0104.bin` | 65,252 bytes | Write claimed success, no function |
| `KB_RGB_V1.56_VID-3554_PID-F811_0106.bin` | 65,256 bytes | Write claimed success, no function |
| `KB_RGB_V1.66_VID-05AC_PID-024F_0102.bin` | 65,224 bytes | Write claimed success, no function |

**Note:** The v1.66 firmware has different VID:PID (05AC:024F) in filename, suggesting possible variant compatibility issues.

## Device Configuration Used

```rust
DEVICE_LOFREE_FLOW_LITE: DeviceSpec = {
    vendor_id: 0x3554,
    product_id: 0xf808,  // Bootloader PID
    platform: {
        firmware_size: 61440,
        bootloader_size: 4096, 
        page_size: 2048,
    },
    isp_iface_num: 0,
    isp_report_id: 6,  // Corrected from 5 to 6
    reboot: false,  // Disabled automatic reboot
}
```

## Commands Tested

### Standard ISP Sequence
1. **Erase:** `[06, 45, 00, 00, 00, 00]` → ✓ Accepted
2. **Init Write:** `[06, 57, 00, 00, 00, 00]` → ✓ Accepted  
3. **Write Pages:** `[06, 77, <2048 bytes data>]` → ✓ Accepted
4. **Enable Firmware:** `[06, 55, 00, 00, 00, 00]` → ✓ Accepted
5. **Reboot:** `[06, 5a, 00, 00, 00, 00]` → ✓ Accepted

### Alternative Exit Sequences Tested
- **Opcode 0x75:** `[06, 75, 00, 00, 00, 00]` → ✓ Sent, no effect
- **Direct reboot without enable:** `[06, 5a, 00, 00, 00, 00]` → ✓ Sent, no effect
- **Init write to specific address + enable:** Both accepted, no effect

## Diagnostic Results

### Communication Test Results
```
=== Testing ERASE command (0x45) ===
✓ Command sent successfully

=== Testing ENABLE_FIRMWARE command (0x55) ===  
✓ Command sent successfully

=== Testing INIT_READ command (0x52) ===
✓ Command sent successfully
✗ No response: IOHIDDeviceGetReport failed: (0xE0005000)

=== Testing INIT_WRITE command (0x57) ===
✓ Command sent successfully  
✗ No response: IOHIDDeviceGetReport failed: (0xE0005000)

=== Testing write/read operations ===
✓ Write page command sent
✗ Read page failed: IOHIDDeviceGetReport failed: (0xE0005000)
```

## Possible Root Causes

1. **Firmware Format Incompatibility**
   - Binary files may not be in correct format for this Flow Lite variant
   - Possible encryption or signing requirements

2. **Missing Vendor-Specific Steps**
   - Lofree may use additional proprietary commands
   - Authentication or unlock sequence may be required

3. **Hardware Write Protection**
   - Device may have firmware protection preventing modification
   - Flash memory may be locked or corrupted

4. **Protocol Variant**
   - Flow Lite may use modified ISP protocol not fully compatible with standard sinowealth-kb-tool

5. **Enable Firmware Command Issues**
   - Command 0x55 may not be correct opcode for Flow Lite
   - Different sequence or timing may be required

## Tools Developed

We created several diagnostic tools during this analysis:

1. **flowlite-debug** - Tests individual ISP commands
2. **flowlite-exit-simple** - Tests basic enable+reboot sequence  
3. **flowlite-alt-exit** - Tests alternative exit sequences
4. **Modified sinowealth-kb-tool** - Added Flow Lite support with `--bootloader_ready` flag

## Recommendations for Lofree Support

1. **Verify Firmware Files**
   - Confirm which firmware file is correct for this Flow Lite variant
   - Check if files require preprocessing or conversion

2. **Document ISP Exit Sequence**  
   - Provide correct command sequence to exit bootloader mode
   - Specify any timing requirements or additional steps

3. **Clarify Device Variants**
   - Explain VID:PID differences (3554:f808 vs 3554:f811 vs 05AC:024F)
   - Document which firmware works with which variant

4. **Recovery Procedure**
   - Provide official recovery method for devices stuck in bootloader mode
   - Share any internal tools or procedures

5. **Protocol Documentation**
   - Document any Lofree-specific ISP protocol extensions
   - Clarify if standard Sinowealth ISP protocol applies

## Files Available for Analysis

We have access to these firmware files from the Lofree distribution:
- `KB_RGB_V1.54_VID-3554_PID-F811_0104.bin`
- `KB_RGB_V1.56_VID-3554_PID-F811_0106.bin` 
- `KB_RGB_V1.66_VID-05AC_PID-024F_0102_6C54C39C_20241122.bin`
- Lofree Key Mapper software and associated tools

## Further Information

If you'd like more information about what I was working with here or would like to see some of the code, I can provide that at your convenience.

Cheers,
JD Lien