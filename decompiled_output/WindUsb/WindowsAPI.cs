using System;
using System.Runtime.InteropServices;

namespace WindUSB;

public class WindowsAPI
{
	public enum DIGCF
	{
		DIGCF_DEFAULT = 1,
		DIGCF_PRESENT = 2,
		DIGCF_ALLCLASSES = 4,
		DIGCF_PROFILE = 8,
		DIGCF_DEVICEINTERFACE = 0x10
	}

	public enum HID_STATUS
	{
		SUCCESS,
		NO_DEVICE,
		NO_FIND,
		OPEND,
		WRITE_FAID,
		READ_FAID
	}

	public struct SP_DEVICE_INTERFACE_DATA
	{
		public int cbSize;

		public Guid interfaceClassGuid;

		public int flags;

		public int reserved;
	}

	[StructLayout(LayoutKind.Sequential)]
	public class SP_DEVINFO_DATA
	{
		public int cbSize = Marshal.SizeOf(typeof(SP_DEVINFO_DATA));

		public Guid classGuid = Guid.Empty;

		public int devInst = 0;

		public int reserved = 0;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	internal struct SP_DEVICE_INTERFACE_DETAIL_DATA
	{
		internal int cbSize;

		internal short devicePath;
	}

	private static class FLAGSANDATTRIBUTES
	{
		public const uint FILE_FLAG_WRITE_THROUGH = 2147483648u;

		public const uint FILE_FLAG_OVERLAPPED = 1073741824u;

		public const uint FILE_FLAG_NO_BUFFERING = 536870912u;

		public const uint FILE_FLAG_RANDOM_ACCESS = 268435456u;

		public const uint FILE_FLAG_SEQUENTIAL_SCAN = 134217728u;

		public const uint FILE_FLAG_DELETE_ON_CLOSE = 67108864u;

		public const uint FILE_FLAG_BACKUP_SEMANTICS = 33554432u;

		public const uint FILE_FLAG_POSIX_SEMANTICS = 16777216u;

		public const uint FILE_FLAG_OPEN_REPARSE_POINT = 2097152u;

		public const uint FILE_FLAG_OPEN_NO_RECALL = 1048576u;

		public const uint FILE_FLAG_FIRST_PIPE_INSTANCE = 524288u;
	}

	private static class CREATIONDISPOSITION
	{
		public const uint CREATE_NEW = 1u;

		public const uint CREATE_ALWAYS = 2u;

		public const uint OPEN_EXISTING = 3u;

		public const uint OPEN_ALWAYS = 4u;

		public const uint TRUNCATE_EXISTING = 5u;
	}

	private static class DESIREDACCESS
	{
		public const uint GENERIC_READ = 2147483648u;

		public const uint GENERIC_WRITE = 1073741824u;

		public const uint GENERIC_EXECUTE = 536870912u;

		public const uint GENERIC_ALL = 268435456u;
	}

	[DllImport("hid.dll")]
	private static extern void HidD_GetHidGuid(ref Guid HidGuid);

	[DllImport("setupapi.dll", SetLastError = true)]
	private static extern IntPtr SetupDiGetClassDevs(ref Guid ClassGuid, uint Enumerator, IntPtr HwndParent, DIGCF Flags);

	[DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
	private static extern bool SetupDiEnumDeviceInterfaces(IntPtr deviceInfoSet, IntPtr deviceInfoData, ref Guid interfaceClassGuid, uint memberIndex, ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData);

	[DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
	private static extern bool SetupDiGetDeviceInterfaceDetail(IntPtr deviceInfoSet, ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData, IntPtr deviceInterfaceDetailData, int deviceInterfaceDetailDataSize, ref int requiredSize, SP_DEVINFO_DATA deviceInfoData);

	[DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
	private static extern bool SetupDiDestroyDeviceInfoList(IntPtr deviceInfoSet);

	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern IntPtr CreateFile(string FileName, uint DesiredAccess, uint ShareMode, uint SecurityAttributes, uint CreationDisposition, uint FlagsAndAttributes, int hTemplateFile);

	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern bool CloseHandle(IntPtr hObject);

	[DllImport("kernel32.dll", CallingConvention = CallingConvention.Cdecl)]
	private static extern bool WriteFile(IntPtr hFile, byte[] lpBuffer);

	[DllImport("hid.dll")]
	internal static extern bool HidD_SetFeature(IntPtr hidDeviceObject, byte[] lpReportBuffer, int reportBufferLength);

	[DllImport("hid.dll")]
	internal static extern bool HidD_SetOutputReport(IntPtr hidDeviceObject, byte[] lpReportBuffer, int reportBufferLength);

	[DllImport("hid.dll")]
	internal static extern bool HidD_GetFeature(IntPtr hidDeviceObject, byte[] lpReportBuffer, int reportBufferLength);

	[DllImport("hid.dll")]
	private static extern bool HidD_GetAttributes(IntPtr hidDeviceObject, out HIDD_ATTRIBUTES attributes);

	[DllImport("hid.dll")]
	private static extern bool HidD_GetPreparsedData(IntPtr hidDeviceObject, out IntPtr PreparsedData);

	[DllImport("hid.dll")]
	private static extern uint HidP_GetCaps(IntPtr PreparsedData, out HIDP_CAPS Capabilities);

	[DllImport("hid.dll")]
	private static extern bool HidD_FreePreparsedData(IntPtr PreparsedData);

	internal void GetDeviceGuid(ref Guid HIDGuid)
	{
		HidD_GetHidGuid(ref HIDGuid);
	}

	internal IntPtr GetClassDevOfHandle(Guid HIDGuid)
	{
		return SetupDiGetClassDevs(ref HIDGuid, 0u, IntPtr.Zero, (DIGCF)18);
	}

	internal bool GetEnumDeviceInterfaces(IntPtr HIDInfoSet, ref Guid HIDGuid, uint index, ref SP_DEVICE_INTERFACE_DATA interfaceInfo)
	{
		return SetupDiEnumDeviceInterfaces(HIDInfoSet, IntPtr.Zero, ref HIDGuid, index, ref interfaceInfo);
	}

	internal bool GetDeviceInterfaceDetail(IntPtr HIDInfoSet, ref SP_DEVICE_INTERFACE_DATA interfaceInfo, IntPtr pDetail, ref int buffsize)
	{
		return SetupDiGetDeviceInterfaceDetail(HIDInfoSet, ref interfaceInfo, pDetail, buffsize, ref buffsize, null);
	}

	internal void DestroyDeviceInfoList(IntPtr HIDInfoSet)
	{
		SetupDiDestroyDeviceInfoList(HIDInfoSet);
	}

	internal IntPtr CreateDeviceFile(string device)
	{
		return CreateFile(device, 3221225472u, 0u, 0u, 3u, 0u, 0);
	}

	internal bool CloseDeviceFile(IntPtr device)
	{
		return CloseHandle(device);
	}

	internal bool WriteHFile(IntPtr ptrFile, byte[] data)
	{
		return WriteFile(ptrFile, data);
	}

	internal bool WriteFeature(IntPtr ptrFile, byte[] data)
	{
		return HidD_SetFeature(ptrFile, data, data.Length);
	}

	internal bool GetFeature(IntPtr ptrFile, byte[] data)
	{
		return HidD_GetFeature(ptrFile, data, data.Length);
	}

	internal void GETDeviceAttribute(IntPtr device, out HIDD_ATTRIBUTES attributes)
	{
		HidD_GetAttributes(device, out attributes);
	}

	internal void GetPreparseData(IntPtr device, out IntPtr preparseData)
	{
		HidD_GetPreparsedData(device, out preparseData);
	}

	internal void GetCaps(IntPtr preparseData, out HIDP_CAPS caps)
	{
		HidP_GetCaps(preparseData, out caps);
	}

	internal void FreePreparseData(IntPtr preparseData)
	{
		HidD_FreePreparsedData(preparseData);
	}
}
