using WindControls;

namespace USBUpdateTool;

public class DeviceFile_CH32V305 : DeviceFile
{
	public DeviceFile_CH32V305(string _icName, string fileNameWithoutExtension, byte[] sourceFile)
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
		bootPid = "f401";
		normalVid = "3554";
		normalPid = "f510";
		switch (fileType)
		{
		case CXFILE_TYPE.Keyboard:
			FileType = CXFILE_TYPE.Keyboard;
			break;
		case CXFILE_TYPE.Mouse:
			FileType = CXFILE_TYPE.Mouse;
			bootEndPoints.Add(new string[2] { "vid_3554&pid_f401&col01", "vid_3554&pid_f401&col01" });
			normalEndPoints.Add(new string[2] { "vid_3554&pid_f510&mi_01&col05", "vid_3554&pid_f510&mi_01&col05" });
			break;
		case CXFILE_TYPE.Dongle:
			FileType = CXFILE_TYPE.Dongle;
			bootEndPoints.Add(new string[2] { "vid_3554&pid_f401&col01", "vid_3554&pid_f401&col01" });
			normalEndPoints.Add(new string[2] { "vid_3554&pid_f510&mi_01&col05", "vid_3554&pid_f510&mi_01&col05" });
			break;
		}
	}
}
