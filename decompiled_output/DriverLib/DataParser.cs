using System;
using System.Runtime.InteropServices;

namespace DriverLib;

public class DataParser
{
	public const int MAX_DPI_CONFIG = 8;

	public const int MAX_KEY_COUNT = 16;

	public const int MAX_KEY_MACRO_COUNT = 70;

	public const int MAX_MACRO_NAME_LENGTH = 30;

	public const int MAX_FLASH_DATA_SIZE = 6912;

	public const int MAX_MACRO_COUNT = 70;

	public const int MAX_NAME_LENGTH = 30;

	public const int MAX_SHURTCUT_ACTION_COUNT = 6;

	public const int USB_PACKET_DATA_SIZE = 10;

	public const int USB_PACKET_SIZE = 17;

	[DllImport("HIDUsb.dll", CallingConvention = CallingConvention.Cdecl)]
	private static extern void CS_GetCidMid(byte[] data, IntPtr deviceCidMid);

	[DllImport("HIDUsb.dll", CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs(UnmanagedType.I1)]
	private static extern bool CS_isDeviceOnLine(byte[] data);

	[DllImport("HIDUsb.dll", CallingConvention = CallingConvention.Cdecl)]
	private static extern void CS_GetDeviceBatteryStatus(byte[] data, IntPtr batteryStatus);

	[DllImport("HIDUsb.dll", CallingConvention = CallingConvention.Cdecl)]
	private static extern void CS_GetDeviceStatusChanged(byte[] data, IntPtr deviceStatusChanged);

	[DllImport("HIDUsb.dll", CallingConvention = CallingConvention.Cdecl)]
	private static extern int CS_GetDeviceVersion(byte[] data);

	[DllImport("HIDUsb.dll", CallingConvention = CallingConvention.Cdecl)]
	private static extern void CS_ProtocolDataParser(byte[] data, IntPtr fashDataMap);

	[DllImport("HIDUsb.dll", CallingConvention = CallingConvention.Cdecl)]
	private static extern void CS_ProtocolDataUpdate(IntPtr fashDataMap);

	[DllImport("HIDUsb.dll", CallingConvention = CallingConvention.Cdecl)]
	private static extern void CS_BufferToDPILed(byte[] data, IntPtr dpiLed);

	[DllImport("HIDUsb.dll", CallingConvention = CallingConvention.Cdecl)]
	private static extern void CS_BufferToLedBar(byte[] data, IntPtr ledBar);

	[DllImport("HIDUsb.dll", CallingConvention = CallingConvention.Cdecl)]
	private static extern void CS_SetDllProtocolData(in FlashDataMap flashDataMap);

	public static DeviceInfo GetDeviceInfo(byte[] buffer)
	{
		DeviceInfo deviceInfo = default(DeviceInfo);
		IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(DeviceInfo)));
		CS_GetCidMid(buffer, intPtr);
		deviceInfo = (DeviceInfo)Marshal.PtrToStructure(intPtr, typeof(DeviceInfo));
		Marshal.FreeHGlobal(intPtr);
		return deviceInfo;
	}

	public static bool isDeviceOnLine(byte[] buffer)
	{
		return CS_isDeviceOnLine(buffer);
	}

	public static BatteryStatus GetDeviceBatteryStatus(byte[] buffer)
	{
		BatteryStatus batteryStatus = default(BatteryStatus);
		IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(BatteryStatus)));
		CS_GetDeviceBatteryStatus(buffer, intPtr);
		batteryStatus = (BatteryStatus)Marshal.PtrToStructure(intPtr, typeof(BatteryStatus));
		Marshal.FreeHGlobal(intPtr);
		return batteryStatus;
	}

	public static DeviceStatusChanged GetDeviceStatusChanged(byte[] buffer)
	{
		DeviceStatusChanged deviceStatusChanged = default(DeviceStatusChanged);
		IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(DeviceStatusChanged)));
		CS_GetDeviceStatusChanged(buffer, intPtr);
		deviceStatusChanged = (DeviceStatusChanged)Marshal.PtrToStructure(intPtr, typeof(DeviceStatusChanged));
		Marshal.FreeHGlobal(intPtr);
		return deviceStatusChanged;
	}

	public static int GetDeviceVersion(byte[] buffer)
	{
		return CS_GetDeviceVersion(buffer);
	}

	public static void ProtocolParser(byte[] buffer, ref FlashDataMap flashDataMap)
	{
		IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(FlashDataMap)));
		CS_ProtocolDataParser(buffer, intPtr);
		flashDataMap = (FlashDataMap)Marshal.PtrToStructure(intPtr, typeof(FlashDataMap));
		Marshal.FreeHGlobal(intPtr);
	}

	public static void Update(FlashDataMap flashDataMap)
	{
		IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(FlashDataMap)));
		Marshal.StructureToPtr(flashDataMap, intPtr, fDeleteOld: true);
		CS_ProtocolDataUpdate(intPtr);
		Marshal.FreeHGlobal(intPtr);
	}

	public static void BufferToDPILed(byte[] data, ref DPILed dpiLed)
	{
		IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(DPILed)));
		CS_BufferToDPILed(data, intPtr);
		dpiLed = (DPILed)Marshal.PtrToStructure(intPtr, typeof(DPILed));
		Marshal.FreeHGlobal(intPtr);
	}

	public static void BufferToLedBar(byte[] data, ref LedBar ledBar)
	{
		IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(LedBar)));
		CS_BufferToLedBar(data, intPtr);
		ledBar = (LedBar)Marshal.PtrToStructure(intPtr, typeof(LedBar));
		Marshal.FreeHGlobal(intPtr);
	}

	public static void SetDllProtocolData(in FlashDataMap flashDataMap)
	{
		CS_SetDllProtocolData(in flashDataMap);
	}
}
