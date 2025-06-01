using WindControls;

namespace USBUpdateTool;

public class DeviceFile_NRF52810 : DeviceFile
{
	public DeviceFile_NRF52810(string _icName, string fileNameWithoutExtension, byte[] sourceFile)
		: base(_icName, fileNameWithoutExtension, sourceFile)
	{
		BootInputEndPoint = "&mi_01&col07";
		BootOutputEndPoint = "&mi_01&col07";
		NormalInputEndPoint = "&mi_01&col05";
		NormalOutputEndPoint = "&mi_01&col05";
		if (DeviceFile.GetUpgradeFileHeader(sourceFile, out var _))
		{
			upgradeFile = UsbUpgradeFile.SplitFromUpgradeFile(sourceFile, 1);
		}
	}

	public override string[] GetFileFilter()
	{
		return new string[1] { "nrf52810" };
	}

	public override void SetFileType(CXFILE_TYPE fileType)
	{
		bootEndPoints.Clear();
		normalEndPoints.Clear();
		bootVid = "3554";
		bootPid = "f505";
		normalVid = "3554";
		normalPid = "f505";
		switch (fileType)
		{
		case CXFILE_TYPE.Keyboard:
			FileType = CXFILE_TYPE.Keyboard;
			break;
		case CXFILE_TYPE.Mouse:
			FileType = CXFILE_TYPE.Mouse;
			bootEndPoints.Add(new string[2] { "vid_3554&pid_f505&mi_01&col07", "vid_3554&pid_f505&mi_01&col07" });
			normalEndPoints.Add(new string[2] { "vid_3554&pid_f505&mi_01&col05", "vid_3554&pid_f505&mi_01&col05" });
			break;
		case CXFILE_TYPE.Dongle:
			FileType = CXFILE_TYPE.Dongle;
			bootEndPoints.Add(new string[2] { "vid_3554&pid_f505&mi_01&col07", "vid_3554&pid_f505&mi_01&col07" });
			normalEndPoints.Add(new string[2] { "vid_3554&pid_f505&mi_01&col05", "vid_3554&pid_f505&mi_01&col05" });
			break;
		}
	}
}
