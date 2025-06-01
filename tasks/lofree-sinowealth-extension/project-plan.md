# Lofree Flow Lite Support for sinowealth-kb-tool

## Project Overview

Extend the sinowealth-kb-tool Rust project to support Lofree Flow Lite keyboard firmware updates by integrating the firmware format, communication protocols, and device handling logic from the decompiled Windows updater tool.

## Project Goals

- Add Lofree Flow Lite keyboard support to sinowealth-kb-tool
- Provide cross-platform (macOS, Linux, Windows) firmware update capability
- Maintain compatibility with existing sinowealth-kb-tool functionality
- Create a reliable, safe firmware update process

## Technical Requirements

### Device Support
- **Target Device**: Lofree Flow Lite Keyboard
- **VID/PID Combinations**:
  - `3554:F811` (Lofree branded)
  - `05AC:024F` (Apple-compatible mode)
- **Firmware Versions**: V1.54 through V1.66
- **Communication**: USB HID interface

### Firmware Format
- **Header Size**: 720 bytes
- **Structure**: Header + Firmware Binary + Padding
- **Validation**: CRC32 checksum, size verification
- **Embedded Resources**: Support for embedded firmware binaries

## Implementation Phases

### Phase 1: Core Integration (Week 1)
**Objective**: Basic device detection and firmware parsing

#### Tasks:
1. **Device Detection**
   - Add Lofree VID/PID combinations to device database
   - Implement device-specific initialization sequences
   - Add safety checks for device compatibility

2. **Firmware File Parsing**
   - Port 720-byte header structure from `UpgradeFileHeader.cs`
   - Implement CRC32 validation
   - Add firmware metadata extraction (version, VID/PID, dates)

3. **Basic HID Communication**
   - Adapt USB command structures from `UsbCommand.cs`
   - Implement device information queries
   - Add upgrade mode detection and switching

**Deliverables**:
- Device detection working
- Firmware file validation
- Basic device communication

### Phase 2: Firmware Operations (Week 2)
**Objective**: Read and write firmware functionality

#### Tasks:
1. **Firmware Reading**
   - Implement device firmware extraction
   - Add version detection and reporting
   - Create backup functionality

2. **Firmware Writing**
   - Port firmware flashing protocol
   - Implement progress reporting
   - Add verification after write

3. **Safety Mechanisms**
   - Device compatibility validation
   - Firmware version compatibility checks
   - Rollback capability for failed updates

**Deliverables**:
- Firmware backup functionality
- Firmware update capability
- Safety validation system

### Phase 3: Advanced Features (Week 3)
**Objective**: Production-ready features

#### Tasks:
1. **Embedded Firmware Support**
   - Extract embedded firmware from upgrade packages
   - Support for multiple firmware variants
   - Automatic firmware selection based on device

2. **Configuration Management**
   - Device-specific configuration options
   - VID/PID modification support (if safe)
   - Custom firmware injection

3. **Error Handling**
   - Comprehensive error reporting
   - Recovery procedures for failed updates
   - Device state restoration

**Deliverables**:
- Embedded firmware extraction
- Advanced configuration options
- Robust error handling

### Phase 4: Testing & Documentation (Week 4)
**Objective**: Validation and user documentation

#### Tasks:
1. **Hardware Testing**
   - Test with multiple Lofree Flow Lite units
   - Validate across firmware versions V1.54-V1.66
   - Cross-platform testing (macOS, Linux, Windows)

2. **Integration Testing**
   - Ensure compatibility with existing sinowealth-kb-tool features
   - Performance and reliability testing
   - Edge case handling

3. **Documentation**
   - Usage documentation for Lofree devices
   - Safety warnings and disclaimers
   - Contributing guidelines for additional Lofree models

**Deliverables**:
- Tested, production-ready code
- Comprehensive documentation
- Safety guidelines

## Technical Architecture

### Code Organization
```
src/
├── devices/
│   ├── lofree/
│   │   ├── mod.rs              # Main Lofree module
│   │   ├── protocol.rs         # USB communication protocol
│   │   ├── firmware.rs         # Firmware parsing and validation
│   │   └── device_info.rs      # Device detection and info
│   └── mod.rs
├── firmware/
│   ├── lofree_format.rs        # Lofree firmware file format
│   └── embedded.rs             # Embedded firmware handling
└── utils/
    ├── crc32.rs                # CRC32 validation
    └── usb_commands.rs         # USB command structures
```

### Key Components to Port

1. **Firmware Header Structure** (`UpgradeFileHeader.cs`)
   - 720-byte header format
   - Version information
   - Device compatibility data

2. **USB Communication** (`UsbCommand.cs`, `UsbServer.cs`)
   - Command protocol definitions
   - Device state management
   - Upgrade mode handling

3. **Device Information** (`DeviceInfo.cs`, `DeviceList.cs`)
   - Device identification
   - Version detection
   - Compatibility checking

4. **Firmware Validation** (`UsbUpgradeFile.cs`)
   - File format validation
   - CRC checking
   - Size verification

## Risk Mitigation

### Technical Risks
- **Device Bricking**: Implement comprehensive validation before any write operations
- **Compatibility Issues**: Extensive testing across firmware versions
- **Protocol Differences**: Careful analysis of communication patterns

### Safety Measures
- **Backup Creation**: Always create firmware backup before updates
- **Validation Checks**: Multiple layers of compatibility verification
- **Recovery Procedures**: Clear procedures for handling failed updates
- **User Warnings**: Prominent disclaimers about update risks

## Success Criteria

1. **Functionality**: Successfully read and write Lofree Flow Lite firmware
2. **Safety**: Zero device bricks during testing phase
3. **Compatibility**: Works across all supported firmware versions (V1.54-V1.66)
4. **Cross-Platform**: Functional on macOS, Linux, and Windows
5. **Integration**: Seamlessly integrates with existing sinowealth-kb-tool codebase

## Dependencies

### Rust Crates
- `hidapi`: HID device communication (already used by sinowealth-kb-tool)
- `crc32fast`: CRC32 validation
- `byteorder`: Binary data handling
- `anyhow`: Error handling

### Development Tools
- Rust toolchain (1.70+)
- Cross-compilation support for target platforms
- USB debugging tools for protocol analysis

## Timeline

- **Week 1**: Core integration and basic functionality
- **Week 2**: Firmware operations and safety mechanisms
- **Week 3**: Advanced features and configuration
- **Week 4**: Testing, validation, and documentation

**Total Duration**: 4 weeks for MVP, additional 2-4 weeks for comprehensive testing and polish.

## Future Enhancements

1. **Additional Lofree Models**: Support for other Lofree keyboards
2. **GUI Interface**: Optional graphical interface for non-technical users
3. **Automated Updates**: Integration with firmware repositories
4. **Custom Firmware**: Support for community-developed firmware
5. **Configuration Backup**: Device settings backup and restore

## Reference Implementation Files

A complete Rust reference implementation has been created in `reference-code/` with the following files:

### Core Implementation Files

1. **`firmware_format.rs`** - Firmware File Format Handling
   - `UpgradeFileHeader` struct (720-byte header)
   - CRC32 validation functions
   - VID/PID structures and known device combinations
   - String field parsing for header metadata
   - Device compatibility checking

2. **`usb_protocol.rs`** - USB Communication Protocol
   - Complete `UsbCommandID` enum (ported from UsbCommandID.cs)
   - `UsbCommand` struct for command serialization/deserialization
   - Device endpoints configuration for Lofree/Apple modes
   - Command creation helpers (enter_update_mode, read_version, etc.)
   - Protocol constants and version parsing utilities

3. **`firmware_operations.rs`** - Firmware Operations
   - `FirmwareFile` struct for parsing firmware files
   - `FirmwareUpgrader` for device operations (placeholder for HID integration)
   - Embedded firmware resource handling
   - Known firmware version database with metadata
   - File validation and compatibility checking

4. **`mod.rs`** - Public API and Device Detection
   - `LofreeLiteDevice` main entry point
   - Device mode helpers (lofree_mode(), apple_mode())
   - Supported device detection
   - Integration points for sinowealth-kb-tool

### Supporting Files

5. **`Cargo.toml`** - Rust Project Configuration
   - Dependencies: crc32fast for CRC validation
   - Example targets for testing
   - Integration notes for sinowealth-kb-tool dependencies

6. **`examples/parse_firmware.rs`** - Firmware File Parser Example
   - Complete firmware file analysis tool
   - Header information display
   - Device compatibility checking
   - Version validation against known firmware database

7. **`examples/device_detection.rs`** - Device Detection Example
   - VID/PID combination testing
   - Endpoint configuration examples
   - Integration guidance for sinowealth-kb-tool
   - Command-line interface examples

8. **`README.md`** - Reference Implementation Documentation
   - Integration instructions for sinowealth-kb-tool
   - API usage examples
   - Safety guidelines and validation procedures
   - File structure explanation

### Key Features Implemented

- **Complete Protocol Support**: All USB commands from original Windows tool
- **Safety Validation**: CRC32 checking, device compatibility verification
- **Cross-Platform Design**: Pure Rust implementation using only standard libraries
- **Firmware Version Database**: Support for V1.54 through V1.66
- **Device Mode Support**: Both Lofree branded (3554:F811) and Apple-compatible (05AC:024F) modes

### Integration Notes

The reference code is designed to integrate directly into sinowealth-kb-tool's architecture:

```
sinowealth-kb-tool/src/devices/lofree/
├── mod.rs              # From reference-code/mod.rs
├── protocol.rs         # From reference-code/usb_protocol.rs
├── firmware.rs         # From reference-code/firmware_operations.rs
└── device_info.rs      # From reference-code/firmware_format.rs
```

All Windows-specific dependencies have been removed or abstracted for cross-platform compatibility.

## Resources

- **Reference Implementation**: Complete Rust code in `reference-code/` directory
- **Original Source**: Decompiled Windows updater (this project)
- **Hardware**: Lofree Flow Lite keyboards for testing
- **Firmware Samples**: Collection of firmware versions V1.54-V1.66
- **Protocol Analysis**: USB capture data from original updater