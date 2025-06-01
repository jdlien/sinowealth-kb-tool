using System.Runtime.InteropServices;

namespace USBUpdateTool;

public struct UpgradeFileHeader
{
	public uint headCRC;

	public uint headLength;

	public uint fwLength;

	public uint nextFileAddress;

	public uint version;

	public byte DeciveType;

	public byte Cid;

	public byte Mid;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
	public byte[] fileId;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
	public byte[] icName;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
	public byte[] bootInputEndPoint;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
	public byte[] bootOutputEndPoint;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
	public byte[] normalInputEndPoint;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
	public byte[] normalOutputEndPoint;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
	public byte[] resetToUpdateModeCmd;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
	public byte[] prepareDownLoadCmd;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
	public byte[] dataDownLoadCmd;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
	public byte[] senserName;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
	public byte[] productName;
}
