using System.Runtime.InteropServices;

namespace DriverLib;

public struct DPIConfig
{
	public byte xDPI;

	public byte yDPI;

	public byte DPIex;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
	public byte[] color;

	public DPIConfig(int key)
	{
		xDPI = 0;
		yDPI = 0;
		DPIex = 0;
		color = new byte[3];
	}
}
