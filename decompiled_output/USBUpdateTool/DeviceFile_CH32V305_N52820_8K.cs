using WindControls;

namespace USBUpdateTool;

public class DeviceFile_CH32V305_N52820_8K : DeviceFile
{
	public DeviceFile_CH32V305_N52820_8K(string _icName, string fileNameWithoutExtension, byte[] sourceFile)
		: base(_icName, fileNameWithoutExtension, sourceFile)
	{
		BootInputEndPoint = "&col01";
		BootOutputEndPoint = "&col01";
		NormalInputEndPoint = "&mi_01&col05";
		NormalOutputEndPoint = "&mi_01&col05";
		if (DeviceFile.GetUpgradeFileHeader(sourceFile, out var _))
		{
			upgradeFile = UsbUpgradeFile.SplitFromUpgradeFile(sourceFile, 0);
		}
	}

	public override string[] GetFileFilter()
	{
		return new string[2] { "ch32v305", "ch305" };
	}

	public override void SetFileType(CXFILE_TYPE fileType)
	{
		bootEndPoints.Clear();
		normalEndPoints.Clear();
		bootVid = "3554";
		bootPid = "f40a";
		normalVid = "3554";
		normalPid = "f517";
		switch (fileType)
		{
		case CXFILE_TYPE.Keyboard:
			FileType = CXFILE_TYPE.Keyboard;
			break;
		case CXFILE_TYPE.Mouse:
			FileType = CXFILE_TYPE.Mouse;
			bootEndPoints.Add(new string[2] { "vid_3554&pid_f40a&col01", "vid_3554&pid_f40a&col01" });
			normalEndPoints.Add(new string[2] { "vid_3554&pid_f517&mi_01&col05", "vid_3554&pid_f517&mi_01&col05" });
			break;
		case CXFILE_TYPE.Dongle:
			FileType = CXFILE_TYPE.Dongle;
			bootEndPoints.Add(new string[2] { "vid_3554&pid_f40a&col01", "vid_3554&pid_f40a&col01" });
			normalEndPoints.Add(new string[2] { "vid_3554&pid_f517&mi_01&col05", "vid_3554&pid_f517&mi_01&col05" });
			break;
		}
	}
}
