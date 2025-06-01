using System.Runtime.InteropServices;

namespace DriverLib;

public struct FlashDataMap
{
	public MouseConfig mouseConfig;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
	public DPIConfig[] dpiConfig;

	public DPILed dpiLed;

	public LedBar ledBar;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
	public KeyFunMap[] keys;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
	public ShortCutKey[] shortCutKey;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
	public MacroKey[] macroKey;

	public FlashDataMap(int key)
	{
		mouseConfig = default(MouseConfig);
		dpiConfig = new DPIConfig[8];
		for (int i = 0; i < 8; i++)
		{
			dpiConfig[i] = new DPIConfig(0);
		}
		dpiLed = default(DPILed);
		ledBar = default(LedBar);
		keys = new KeyFunMap[16];
		for (int j = 0; j < 16; j++)
		{
			keys[j] = new KeyFunMap(0);
		}
		shortCutKey = new ShortCutKey[16];
		for (int k = 0; k < 16; k++)
		{
			shortCutKey[k] = new ShortCutKey(0);
		}
		macroKey = new MacroKey[16];
		for (int l = 0; l < 16; l++)
		{
			macroKey[l] = new MacroKey(0);
		}
		ledBar = new LedBar(0);
	}
}
