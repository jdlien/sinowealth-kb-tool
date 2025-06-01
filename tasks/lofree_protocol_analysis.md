# Lofree Flow Lite Protocol Analysis

## Overview
Analysis of the Lofree Key Mapper Windows software to understand the USB HID protocol used for firmware updates.

## Key Findings

### 1. DLL Function Names (from HIDUsb.dll)
The following functions suggest the protocol structure:

- `CS_UsbServer_EnterUsbUpdateMode` - Enters firmware update mode
- `UsbServer_InitWriteCommand` - Initializes write commands
- `UsbServer_WriteCommand` - Sends write commands
- `CS_UsbServer_ReadAllFlashData` - Reads flash memory
- `CS_ProtocolDataUpdate` - Updates protocol data
- `CS_ProtocolDataParser` - Parses protocol data
- `UsbFinder_WriteDeviceData` - Writes data to device
- `UsbFinder_WriteCidMid` - Writes CID/MID (possibly device identifiers)

### 2. Configuration Details (from Config.ini)

#### Device Identification
```
VID=MDVBQw==,MzU1NA==  (Base64 encoded)
D_PID=MDI0Rg==,RkEwQQ==,RjgxMQ==,RjgwOA==  (Base64 encoded)
Interface=1
Port=5
```

Decoding the Base64:
- VID values: "05AC" (0x05AC), "3554" (0x3554)
- D_PID values: "024F", "FA0A", "F811", "F808"

This confirms:
- Runtime PID: 0xF811
- Bootloader PID: 0xF808
- VID: 0x3554

#### Device Information
```
KBM=CX53730  (Keyboard model - matches the MCU!)
DM=2481
MID=1  (Model ID)
Col=6
Row=22
```

### 3. Protocol Observations

#### Interface Number
The config shows `Interface=1`, but our device enumeration showed interface 0. This might be configurable or the Windows software might use a different interface.

#### Report Structure
The software appears to use:
- Multiple report types for different operations
- Separate commands for entering update mode vs. actual flashing
- CID/MID writing suggests device identification is part of the protocol

#### Update Process Flow (inferred)
1. `EnterUsbUpdateMode` - Switch device to update mode
2. `InitWriteCommand` - Prepare for writing
3. `WriteCommand` - Send firmware data
4. Device reboots automatically

### 4. Potential Command Structure

Based on the function names and standard patterns:
- Commands likely include: Enter Update Mode, Init Write, Write Data, Read Flash
- The protocol includes data parsing/validation (`ProtocolDataParser`)
- There's a comparison update function (`ProtocolDataCompareUpdate`) suggesting differential updates

### 5. Missing Information

We still don't have:
- Exact command byte values

- Report ID mappings
- Packet structure/format
- Checksum/validation methods

## Recommendations

1. **USB Traffic Capture**: The most reliable way to understand the protocol would be to capture USB traffic during a successful firmware update using the Windows software.

2. **Reverse Engineering**: The DLL could be further analyzed with tools like:
   - IDA Pro or Ghidra for disassembly
   - API Monitor to trace HID API calls
   - WireShark with USBPcap for traffic analysis

3. **Alternative Approach**: Since the device uses interface 1 according to the config, try:
   ```bash
   ./sinowealth-kb-tool write --vendor_id 0x3554 --product_id 0xF808 \
     --isp_iface_num 1 --device lofree-flow-lite \
     Lofree/KB_RGB_V1.56_VID-3554_PID-F811_0106.bin
   ```

4. **Safety Note**: The presence of `ReadAllFlashData` function suggests it might be possible to create a backup before attempting any writes.

## Additional Analysis from DLL

### Exported Functions
From analyzing the DLL exports, we found these key functions:
- `UsbServer_EnterUsbUpdateMode` - Entry point for firmware update
- `UsbServer_InitWriteCommand` - Initialize write operation
- `UsbServer_WriteCommand` - Send write commands
- `UsbServer_ReadAllFlashData` - Read flash contents
- `UsbUpgrade_Start` - Main upgrade function
- `UsbUpgrade_FileSplit` - Suggests firmware is sent in chunks

### Function Groups
The DLL exports suggest several operational modes:
1. **Update Mode**: EnterUsbUpdateMode, InitWriteCommand, WriteCommand
2. **Pairing Mode**: EnterDonglePair, EnterDonglePairWithCidMid
3. **MTK Mode**: EnterMTKMode (possibly MediaTek related)
4. **Read Operations**: ReadAllFlashData, ReadConfig, ReadCidMid, ReadBatteryLevel

### Protocol Insights
1. **Two-stage Process**: The presence of both `InitWriteCommand` and `WriteCommand` suggests a two-stage write process
2. **File Splitting**: `UsbUpgrade_FileSplit` indicates the firmware is sent in chunks
3. **CID/MID**: Device identification uses CID (Customer ID) and MID (Model ID)
4. **Multiple Interfaces**: The DLL supports multiple device types (keyboard, dongle, mouse)

## Experimental Approach

Based on the analysis, we could try creating a minimal test program that:
1. Opens the device on interface 1 (as per config)
2. Attempts to read device info first (safer than writing)
3. Uses standard HID report structures

However, without knowing the exact command format and report structure, this remains risky.

## Next Steps

1. **Safe Recovery**: Use the official Windows software on a physical machine
2. **Future Investigation**: Once the keyboard is restored, USB traffic capture would reveal the exact protocol
3. **Alternative Tools**: Check if other Sinowealth-based keyboard tools support the CX53730 chip