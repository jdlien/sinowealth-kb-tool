# USB Traffic Capture Instructions for Lofree Flow Lite Firmware Update

## Prerequisites

1. **Windows Machine** (physical, not VM - USB passthrough can interfere)
2. **Working Lofree Flow Lite keyboard** (for capturing normal update process)
3. **Admin privileges** on Windows

## Step 1: Install Required Software

1. **Download Wireshark**
   - Go to https://www.wireshark.org/download.html
   - Download Windows 64-bit installer
   - During installation, **CHECK** "Install USBPcap" option (critical!)
   - If you miss this, download USBPcap separately from https://desowin.org/usbpcap/

2. **Install Lofree Key Mapper**
   - Use the official software that came with your keyboard

## Step 2: Prepare for Capture

1. **Close all unnecessary USB software**
   - Disconnect other USB devices if possible
   - This reduces noise in the capture

2. **Connect the working keyboard**
   - Ensure it's in normal mode (VID:0x3554, PID:0xF811)
   - Test that it's functioning normally

3. **Open Command Prompt as Administrator**
   ```cmd
   # List USB devices to find the keyboard
   USBPcapCMD.exe
   ```
   Note the device number for your keyboard (look for VID_3554)

## Step 3: Start USB Capture

1. **Open Wireshark as Administrator**

2. **Select USBPcap Interface**
   - You'll see interfaces like "USBPcap1", "USBPcap2", etc.
   - Select the one corresponding to your keyboard's USB root hub

3. **Configure Capture Filter** (optional but recommended)
   - In capture options, add filter: `usb.idVendor == 0x3554`
   - This captures only Lofree traffic

4. **Start Capture**
   - Click the blue shark fin button
   - You should see some initial USB traffic

## Step 4: Perform Firmware Update

1. **Open Lofree Key Mapper**

2. **Load firmware file**
   - Use `KB_RGB_V1.56_VID-3554_PID-F811_0106.bin`

3. **Start the update process**
   - Watch Wireshark - you'll see traffic spike
   - The keyboard will disconnect and reconnect (switching to bootloader mode)
   - Continue until update completes

4. **Stop Wireshark capture** immediately after completion

## Step 5: Save the Capture

1. **Save the capture**
   - File → Save As → `lofree_firmware_update.pcapng`

2. **Apply display filters** to analyze:
   ```
   # Show only Lofree device traffic
   usb.idVendor == 0x3554
   
   # Show only HID reports
   usb.idVendor == 0x3554 && usb.transfer_type == 0x03
   
   # Show control transfers (mode switching)
   usb.idVendor == 0x3554 && usb.transfer_type == 0x02
   ```

## Step 6: Analyze Key Events

Look for these critical moments:

1. **Mode Switch to Bootloader**
   ```
   # Filter for control transfers when PID changes
   usb.idProduct == 0xf811 || usb.idProduct == 0xf808
   ```
   - Note the control transfer that triggers bootloader mode
   - Record the exact bytes sent

2. **Initial Bootloader Handshake**
   ```
   # Filter for first HID reports after mode switch
   usb.idProduct == 0xf808 && frame.number > [mode_switch_frame]
   ```
   - These contain initialization commands
z
3. **Firmware Transfer Pattern**
   - Look for repeating patterns in HID reports
   - Note report IDs, packet sizes, and structure

4. **Write Commands**
   - Identify packets that contain actual firmware data
   - Look for address information and data chunks

## Step 7: Export Protocol Details

1. **Export as Text**
   - Select interesting packets
   - File → Export Packet Dissections → As Plain Text
   - Focus on Data payload sections

2. **Create Protocol Map**
   ```
   Example structure to document:
   
   1. Enter Bootloader Mode
      - Control Transfer: [hex bytes]
      - Device reconnects as F808
   
   2. Initialize Flash
      - Report ID: 0x06
      - Command: [hex pattern]
      - Response: [hex pattern]
   
   3. Write Flash
      - Report ID: 0x06
      - Format: [addr_bytes][data_bytes][checksum]
      - Chunk size: X bytes
   
   4. Verify/Complete
      - Final command: [hex bytes]
   ```

## Important Analysis Points

### 1. Report Structure
- First byte is usually Report ID
- Look for command bytes (often 2nd byte)
- Address fields (usually 2-4 bytes)
- Data payload
- Checksum/CRC (often last 1-2 bytes)

### 2. Command Patterns
Common commands might be:
- Erase: Often 0x20, 0x30, or 0x52
- Write: Often 0x31, 0x40, or 0x57
- Read: Often 0x03, 0x0B, or 0x52
- Reset: Often 0xFF, 0x00, or specific sequence

### 3. Address Mapping
- Note if addresses are absolute or relative
- Check if they match the memory layout (0x0000-0xF000)

## Troubleshooting

### No USBPcap interfaces in Wireshark
- Reinstall Wireshark with USBPcap
- Reboot after installation
- Run as Administrator

### Too much USB traffic
- Disconnect other devices
- Use capture filters
- Focus on specific VID/PID

### Missing packets
- Ensure you're capturing on the correct root hub
- Try capturing on all USBPcap interfaces
- Check Windows Event Viewer for USB errors

## Alternative: USB Monitor Software

If Wireshark proves difficult, try:
- **Free**: USBlyzer (trial version)
- **Paid**: Device Monitoring Studio
- **Free**: API Monitor (for HID API calls)

These tools sometimes provide cleaner output for HID devices.

## Next Steps

Once you capture this data, you'll be able to:
1. Identify the exact protocol used by Lofree
2. Document the command structure
3. Implement support in sinowealth-kb-tool
4. Create a recovery tool for bricked devices

Focus especially on:
- The initialization sequence after entering bootloader mode
- The format of write commands
- Any handshake or acknowledgment patterns
- Error handling and retry mechanisms

This information will be crucial for implementing Lofree support in sinowealth-kb-tool.