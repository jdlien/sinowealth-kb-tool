#!/usr/bin/env python3
import pyshark
import sys

# Open the capture file
cap = pyshark.FileCapture('/Users/jdlien/code/sinowealth-kb-tool/Lofree/lofree-flow-lite-oe921-keyboard-upgrade-tool-capture.pcapng', 
                          display_filter='usb')

print("Analyzing USB traffic for mode switch sequence...")
print("=" * 80)

current_vid_pid = None
mode_switch_found = False
pre_switch_packets = []
post_switch_packets = []

for packet in cap:
    try:
        # Extract USB info
        frame_num = packet.frame_info.number
        time_rel = float(packet.frame_info.time_relative)
        
        # Check if this packet has USB data
        if hasattr(packet, 'usb'):
            # Get VID:PID if available
            vid = packet.usb.idVendor if hasattr(packet.usb, 'idVendor') else None
            pid = packet.usb.idProduct if hasattr(packet.usb, 'idProduct') else None
            
            # Check for device enumeration change
            if vid and pid:
                new_vid_pid = f"{vid}:{pid}"
                if current_vid_pid and current_vid_pid != new_vid_pid:
                    print(f"\n*** DEVICE CHANGE DETECTED at frame {frame_num}, time {time_rel:.6f}s ***")
                    print(f"    From: {current_vid_pid} -> To: {new_vid_pid}")
                    mode_switch_found = True
                current_vid_pid = new_vid_pid
            
            # Get endpoint and data
            endpoint = packet.usb.endpoint_address if hasattr(packet.usb, 'endpoint_address') else None
            data_len = packet.usb.data_len if hasattr(packet.usb, 'data_len') else None
            
            # Check for USB HID data
            capdata = None
            if hasattr(packet, 'usb') and hasattr(packet.usb, 'capdata'):
                capdata = packet.usb.capdata
            elif hasattr(packet, 'usbhid') and hasattr(packet.usbhid, 'data'):
                capdata = packet.usbhid.data
                
            # Store packets around mode switch
            if not mode_switch_found:
                # Keep last 20 packets before switch
                pre_switch_packets.append({
                    'frame': frame_num,
                    'time': time_rel,
                    'endpoint': endpoint,
                    'data_len': data_len,
                    'data': capdata,
                    'vid_pid': current_vid_pid
                })
                if len(pre_switch_packets) > 20:
                    pre_switch_packets.pop(0)
            else:
                # Collect 10 packets after switch
                if len(post_switch_packets) < 10:
                    post_switch_packets.append({
                        'frame': frame_num,
                        'time': time_rel,
                        'endpoint': endpoint,
                        'data_len': data_len,
                        'data': capdata,
                        'vid_pid': current_vid_pid
                    })
                    
    except AttributeError:
        continue

# Print packets before mode switch
print("\n\nPACKETS BEFORE MODE SWITCH:")
print("-" * 80)
for pkt in pre_switch_packets[-10:]:  # Last 10 packets
    if pkt['data']:
        print(f"Frame {pkt['frame']}: Time={pkt['time']:.6f}s, "
              f"Endpoint={pkt['endpoint']}, Len={pkt['data_len']}, "
              f"VID:PID={pkt['vid_pid']}")
        print(f"  Data: {pkt['data']}")

# Print packets after mode switch  
print("\n\nPACKETS AFTER MODE SWITCH:")
print("-" * 80)
for pkt in post_switch_packets:
    if pkt['data']:
        print(f"Frame {pkt['frame']}: Time={pkt['time']:.6f}s, "
              f"Endpoint={pkt['endpoint']}, Len={pkt['data_len']}, "
              f"VID:PID={pkt['vid_pid']}")
        print(f"  Data: {pkt['data']}")

cap.close()