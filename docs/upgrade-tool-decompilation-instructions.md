# Decompiling the Lofree Flow Lite Upgrade Tool

This guide documents how to decompile and analyze the Lofree Flow Lite firmware upgrade tool (`Lofree-Flow-Lite_Unpaired_OE921_V1.61_DLL_Upgrade.exe`) on macOS.

## Overview

The Lofree upgrade tool is a Windows executable that handles firmware updates for the keyboard. It contains an embedded firmware file and can recover bricked keyboards in DFU/bootloader mode. By decompiling it, we can understand the update protocol and potentially extract the firmware.

## Prerequisites

- macOS system with Homebrew installed
- The Windows executable file to decompile

## Step 1: Identify the Executable Type

First, check what type of executable we're dealing with:

```bash
file "Lofree/Lofree-Flow-Lite_Unpaired_OE921_V1.61_DLL_Upgrade.exe"
```

Output:
```
Lofree/Lofree-Flow-Lite_Unpaired_OE921_V1.61_DLL_Upgrade.exe: PE32 executable (GUI) Intel 80386 Mono/.Net assembly, for MS Windows
```

This tells us it's a .NET assembly, which is excellent news - .NET assemblies can be decompiled to near-original source code.

## Step 2: Install Required Tools

### Install .NET SDK

The decompiler requires .NET runtime. Install both .NET 9 (latest) and .NET 8 (for compatibility):

```bash
# Install latest .NET
brew install dotnet

# Install .NET 8 for compatibility with some tools
brew install dotnet@8
```

### Install ILSpy (GUI Decompiler)

```bash
brew install --cask ilspy
```

This installs the ILSpy GUI application to `/Applications/ILSpy.app`.

### Install ILSpy Command Line Tool

```bash
# Install the command-line version
dotnet tool install ilspycmd -g
```

Note: The tool will be installed to `~/.dotnet/tools/`. You may need to add this to your PATH.

### Install Additional Binary Analysis Tools

```bash
# Install radare2 for general binary analysis
brew install radare2

# Install binwalk for firmware extraction (if not already installed)
brew install binwalk
```

## Step 3: Decompile the Executable

Use the ILSpy command-line tool to decompile the entire application:

```bash
# Set the correct .NET environment
export DOTNET_ROOT="/opt/homebrew/opt/dotnet@8/libexec"

# Decompile the executable
/Users/$USER/.dotnet/tools/ilspycmd "Lofree/Lofree-Flow-Lite_Unpaired_OE921_V1.61_DLL_Upgrade.exe" -p -o decompiled_output
```

The `-p` flag creates a project file, and `-o` specifies the output directory.

## Step 4: Explore the Decompiled Code

After decompilation, you'll have a complete source tree:

```bash
ls -la decompiled_output/
```

Key directories and files to explore:

- `USBUpdateTool/` - Main application logic
- `USBUpdateTool.UpgradeFile/` - Firmware file handling
- `DriverLib/` - USB driver interface
- `WindUsb/` - Windows USB communication layer

## Step 5: Extract Embedded Resources

The decompiler extracts all embedded resources. Look for firmware files:

```bash
# Check for embedded binary files
find decompiled_output -name "*.bin" -type f

# Examine the embedded firmware file
hexdump -C decompiled_output/USBUpdateTool.MakeUpgradeTool.Files.CompxUpgradeBinFile.bin | head -20
```

In this case, we found:
- `USBUpdateTool.MakeUpgradeTool.Files.CompxUpgradeBinFile.bin` (65,224 bytes)

## Step 6: Analyze the Firmware Structure

The firmware file header reveals important information:

```
00000000  06 25 55 55 d0 02 00 00  c8 de 00 00 00 00 00 00  |.%UU............|
00000010  00 00 00 00 d1 00 00 43  6f 6d 55 73 62 55 70 67  |.......ComUsbUpg|
00000020  72 61 64 65 46 69 6c 65  00 00 00 00 00 00 00 00  |radeFile........|
```

Key findings:
- Magic bytes: `06 25 55 55` (identifies Sinowealth firmware format)
- File identifier: "ComUsbUpgradeFile"
- Chip type: CX53730
- VID/PID information embedded in the header

## Understanding the Code Structure

### Key Classes to Examine

1. **FileHeader.cs** - Parses firmware file headers
2. **FormMain.cs** - Main application window
3. **UpgradeManager.cs** - Handles the upgrade process
4. **Program.cs** - Application entry point

### USB Communication

The tool uses different VID/PID pairs for different modes:
- Bootloader mode: VID=3554, PID=F808
- Normal mode: VID=05AC, PID=024F

## Alternative: Using ILSpy GUI

If you prefer a graphical interface:

1. Open ILSpy from Applications
2. File → Open → Select the .exe file
3. Navigate through the tree structure
4. Right-click on any node → Save Code to export

## Tips for Further Analysis

1. **Search for encryption/obfuscation**: Look for methods that handle firmware encryption
2. **Protocol analysis**: Find USB communication methods in the `WindUsb` namespace
3. **Firmware validation**: Check how the tool validates firmware files before flashing
4. **Update sequences**: Trace through `UpgradeManager` to understand the update flow

## Troubleshooting

### .NET Version Issues

If you encounter ".NET not found" errors:
```bash
# Use the correct .NET version
export DOTNET_ROOT="/opt/homebrew/opt/dotnet@8/libexec"
```

### Path Issues

If `ilspycmd` is not found:
```bash
# Use the full path
~/.dotnet/tools/ilspycmd [arguments]
```

## Next Steps

With the decompiled source code, you can:

1. Understand the firmware update protocol
2. Extract and analyze the embedded firmware
3. Potentially modify the tool to accept custom firmware
4. Implement the update protocol in your own tools
5. Study the USB communication patterns

## Security Note

This decompilation is for educational and development purposes. Always respect software licenses and intellectual property rights when analyzing commercial software.