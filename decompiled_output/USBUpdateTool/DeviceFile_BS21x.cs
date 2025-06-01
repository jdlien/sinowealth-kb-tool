using WindControls;

namespace USBUpdateTool;

public class DeviceFile_BS21x : DeviceFile
{
	public DeviceFile_BS21x(string _icName, string fileNameWithoutExtension, byte[] sourceFile, FIRMWARE_CLASS _firmwareClass)
		: base(_icName, fileNameWithoutExtension, sourceFile, _firmwareClass)
	{
		BootInputEndPoint = "&mi_01&col01";
		BootOutputEndPoint = "&mi_01&col02";
		NormalInputEndPoint = "&mi_01&col05";
		NormalOutputEndPoint = "&mi_01&col05";
		if (DeviceFile.GetUpgradeFileHeader(sourceFile, out var _))
		{
			upgradeFile = UsbUpgradeFile.SplitFromUpgradeFile(sourceFile, 0);
		}
	}

	public override string[] GetFileFilter()
	{
		return new string[1] { "bs21" };
	}

	public override void SetFileType(CXFILE_TYPE fileType)
	{
		bootEndPoints.Clear();
		normalEndPoints.Clear();
		bootVid = "3554";
		bootPid = "f2ff";
		normalVid = "3554";
		normalPid = "f200";
		switch (fileType)
		{
		case CXFILE_TYPE.Keyboard:
			FileType = CXFILE_TYPE.Keyboard;
			bootEndPoints.Add(new string[2] { "vid_3554&pid_f202&mi_01&col01", "vid_3554&pid_f202&mi_01&col02" });
			break;
		case CXFILE_TYPE.Mouse:
			FileType = CXFILE_TYPE.Mouse;
			bootEndPoints.Add(new string[2] { "vid_3554&pid_f200&mi_01&col01", "vid_3554&pid_f200&mi_01&col02" });
			break;
		case CXFILE_TYPE.Dongle:
			FileType = CXFILE_TYPE.Dongle;
			bootEndPoints.Add(new string[2] { "vid_3554&pid_f2ff&mi_01&col01", "vid_3554&pid_f2ff&mi_01&col02" });
			normalEndPoints.Add(new string[2] { "vid_3554&pid_f201&mi_01&col05", "vid_3554&pid_f201&mi_01&col05" });
			break;
		}
	}

	public override void SetFileType(string fileName)
	{
		CXFILE_TYPE fileTypeFromFileName = GetFileTypeFromFileName(fileName);
		SetFileType(fileTypeFromFileName);
	}
}
