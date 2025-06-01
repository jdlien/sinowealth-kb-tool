using WindControls;

namespace USBUpdateTool;

public class DeviceFile_CX53730 : DeviceFile
{
	public DeviceFile_CX53730(string _icName, string fileNameWithoutExtension, byte[] sourceFile)
		: base(_icName, fileNameWithoutExtension, sourceFile)
	{
		BootInputEndPoint = "&col01";
		BootOutputEndPoint = "&col01";
		NormalInputEndPoint = "&mi_01&col05";
		NormalOutputEndPoint = "&mi_01&col05";
	}

	public override string[] GetFileFilter()
	{
		return new string[1] { "cx53730" };
	}

	public override void SetFileType(CXFILE_TYPE fileType)
	{
		bootEndPoints.Clear();
		normalEndPoints.Clear();
		bootVid = "3554";
		bootPid = "f808";
		normalVid = "3554";
		normalPid = "f809";
		switch (fileType)
		{
		case CXFILE_TYPE.Keyboard:
			FileType = CXFILE_TYPE.Keyboard;
			bootEndPoints.Add(new string[2] { "vid_3554&pid_f808&col01", "vid_3554&pid_f808&col01" });
			normalEndPoints.Add(new string[2] { "vid_3554&pid_f809&mi_01&col05", "vid_3554&pid_f809&mi_01&col05" });
			break;
		case CXFILE_TYPE.Mouse:
			FileType = CXFILE_TYPE.Mouse;
			bootEndPoints.Add(new string[2] { "vid_3554&pid_f808&col01", "vid_3554&pid_f808&col01" });
			normalEndPoints.Add(new string[2] { "vid_3554&pid_f809&mi_01&col05", "vid_3554&pid_f809&mi_01&col05" });
			break;
		case CXFILE_TYPE.Dongle:
			FileType = CXFILE_TYPE.Dongle;
			break;
		}
	}
}
