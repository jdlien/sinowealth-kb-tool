using WindControls;

namespace USBUpdateTool;

public class DeviceFile_BK3635 : DeviceFile
{
	public DeviceFile_BK3635(string _icName, string fileNameWithoutExtension, byte[] sourceFile)
		: base(_icName, fileNameWithoutExtension, sourceFile)
	{
		BootInputEndPoint = "&col01";
		BootOutputEndPoint = "&col01";
		NormalInputEndPoint = "&mi_01&col01";
		NormalOutputEndPoint = "&mi_01&col01";
	}

	public override string[] GetFileFilter()
	{
		return new string[1] { "bk3635" };
	}

	public override void SetFileType(CXFILE_TYPE fileType)
	{
		bootEndPoints.Clear();
		normalEndPoints.Clear();
		bootVid = "3554";
		bootPid = "f600";
		normalVid = "3554";
		normalPid = "f608";
		switch (fileType)
		{
		case CXFILE_TYPE.Keyboard:
			FileType = CXFILE_TYPE.Keyboard;
			bootEndPoints.Add(new string[2] { "vid_3554&pid_f600&col01", "vid_3554&pid_f600&col01" });
			normalEndPoints.Add(new string[2] { "vid_3554&pid_f608&mi_01&col01", "vid_3554&pid_f608&mi_01&col01" });
			break;
		case CXFILE_TYPE.Mouse:
			FileType = CXFILE_TYPE.Mouse;
			normalVid = "3554";
			normalPid = "f609";
			bootEndPoints.Add(new string[2] { "vid_3554&pid_f600&col01", "vid_3554&pid_f600&col01" });
			normalEndPoints.Add(new string[2] { "vid_3554&pid_f609&mi_01&col01", "vid_3554&pid_f609&mi_01&col01" });
			break;
		case CXFILE_TYPE.Dongle:
			FileType = CXFILE_TYPE.Dongle;
			normalVid = "25a7";
			normalPid = "faa6";
			bootEndPoints.Add(new string[2] { "vid_3554&pid_f600&col01", "vid_3554&pid_f600&col01" });
			normalEndPoints.Add(new string[2] { "vid_25a7&pid_faa6&mi_01&col01", "vid_25a7&pid_faa6&mi_01&col01" });
			break;
		}
	}
}
