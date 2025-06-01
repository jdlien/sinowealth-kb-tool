using WindControls;

namespace USBUpdateTool;

public class DeviceFile_NRF52840 : DeviceFile
{
	public DeviceFile_NRF52840(string _icName, string fileNameWithoutExtension, byte[] sourceFile)
		: base(_icName, fileNameWithoutExtension, sourceFile)
	{
		BootInputEndPoint = "&col01";
		BootOutputEndPoint = "&col01";
		NormalInputEndPoint = "&mi_01&col05";
		NormalOutputEndPoint = "&mi_01&col05";
	}

	public override string[] GetFileFilter()
	{
		return new string[1] { "nrf52840" };
	}

	public override void SetFileType(CXFILE_TYPE fileType)
	{
		bootEndPoints.Clear();
		normalEndPoints.Clear();
		bootVid = "3554";
		bootPid = "f403";
		normalVid = "3554";
		normalPid = "f50f";
		switch (fileType)
		{
		case CXFILE_TYPE.Keyboard:
			FileType = CXFILE_TYPE.Keyboard;
			bootEndPoints.Add(new string[2] { "vid_3554&pid_f403&col01", "vid_3554&pid_f403&col01" });
			break;
		case CXFILE_TYPE.Mouse:
			FileType = CXFILE_TYPE.Mouse;
			bootEndPoints.Add(new string[2] { "vid_3554&pid_f403&col01", "vid_3554&pid_f403&col01" });
			normalEndPoints.Add(new string[2] { "vid_3554&pid_f50f&mi_01&col05", "vid_3554&pid_f50f&mi_01&col05" });
			break;
		case CXFILE_TYPE.Dongle:
			FileType = CXFILE_TYPE.Dongle;
			bootEndPoints.Add(new string[2] { "vid_3554&pid_f403&col01", "vid_3554&pid_f403&col01" });
			break;
		}
	}

	public override void SetFileType(string fileName)
	{
		CXFILE_TYPE fileTypeFromFileName = GetFileTypeFromFileName(fileName);
		SetFileType(fileTypeFromFileName);
	}
}
