#!/usr/bin/env python3

import sys

with open('Lofree-Flow-Lite_Unpaired_OE921_V1.61_DLL_Upgrade.exe', 'rb') as f:
    data = f.read()
    
print(f"File size: {len(data)} bytes")

# Look for blocks of non-zero data around 64KB size
block_size = 65536
candidates = []

for i in range(0, len(data) - block_size, 1024):
    block = data[i:i+block_size]
    non_zero = sum(1 for b in block if b != 0)
    if non_zero > 30000:  # Significant non-zero content
        candidates.append((i, non_zero, block))
        print(f'Found potential firmware at offset 0x{i:x}, non-zero bytes: {non_zero}')
        
        # Check for LJMP patterns
        if block[0] == 0x02:  # Starts with LJMP
            print(f'  Starts with LJMP! First 16 bytes: {block[:16].hex()}')
        if len(block) >= 5 and block[-5:-2] == b'\x02\x00\x00':  # Has LJMP footer
            print(f'  Has LJMP footer! Last 16 bytes: {block[-16:].hex()}')
            
        # Look for typical firmware patterns
        ljmp_count = block.count(b'\x02\x00\x00')
        if ljmp_count > 0:
            print(f'  Contains {ljmp_count} LJMP patterns')

# Extract most promising candidate
if candidates:
    best = max(candidates, key=lambda x: x[1])
    print(f"\nExtracting best candidate from offset 0x{best[0]:x}")
    with open('extracted_v1.61_firmware.bin', 'wb') as f:
        f.write(best[2])
    print("Saved as extracted_v1.61_firmware.bin")