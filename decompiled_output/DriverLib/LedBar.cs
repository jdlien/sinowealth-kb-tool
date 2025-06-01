using System.Runtime.InteropServices;

namespace DriverLib;

public struct LedBar
{
	public byte mode;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
	public byte[] color;

	public byte speed;

	public byte brightness;

	public byte enable;

	public LedBar(int key)
	{
		mode = 0;
		speed = 0;
		brightness = 0;
		enable = 0;
		color = new byte[3];
	}
}
