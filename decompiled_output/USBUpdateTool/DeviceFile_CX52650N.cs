using WindControls;

namespace USBUpdateTool;

public class DeviceFile_CX52650N : DeviceFile
{
	public DeviceFile_CX52650N(string _icName, string fileNameWithoutExtension, byte[] sourceFile)
		: base(_icName, fileNameWithoutExtension, sourceFile)
	{
		BootInputEndPoint = "&mi_01&col01";
		BootOutputEndPoint = "&mi_01&col02";
		NormalInputEndPoint = "&mi_01&col05";
		NormalOutputEndPoint = "&mi_01&col05";
	}

	public override string[] GetFileFilter()
	{
		return new string[1] { "cx52650n" };
	}

	public override void SetFileType(CXFILE_TYPE fileType)
	{
		bootEndPoints.Clear();
		normalEndPoints.Clear();
		bootVid = "3554";
		bootPid = "f402";
		normalVid = "3554";
		normalPid = "f50d";
		switch (fileType)
		{
		case CXFILE_TYPE.Keyboard:
			FileType = CXFILE_TYPE.Keyboard;
			bootEndPoints.Add(new string[2] { "vid_3554&pid_f402&mi_01&col01", "vid_3554&pid_f402&mi_01&col02" });
			break;
		case CXFILE_TYPE.Mouse:
			FileType = CXFILE_TYPE.Mouse;
			bootEndPoints.Add(new string[2] { "vid_3554&pid_f402&mi_01&col01", "vid_3554&pid_f402&mi_01&col02" });
			break;
		case CXFILE_TYPE.Dongle:
			FileType = CXFILE_TYPE.Dongle;
			bootEndPoints.Add(new string[2] { "vid_3554&pid_f402&mi_01&col01", "vid_3554&pid_f402&mi_01&col02" });
			normalEndPoints.Add(new string[2] { "vid_3554&pid_f50d&mi_01&col05", "vid_3554&pid_f50d&mi_01&col05" });
			break;
		}
	}

	public override void SetFileType(string fileName)
	{
		CXFILE_TYPE fileTypeFromFileName = GetFileTypeFromFileName(fileName);
		SetFileType(fileTypeFromFileName);
	}
}
