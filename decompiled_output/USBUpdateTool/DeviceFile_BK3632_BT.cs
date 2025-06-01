using WindControls;

namespace USBUpdateTool;

public class DeviceFile_BK3632_BT : DeviceFile
{
	public DeviceFile_BK3632_BT(string _icName, string fileNameWithoutExtension, byte[] sourceFile)
		: base(_icName, fileNameWithoutExtension, sourceFile)
	{
		BootInputEndPoint = "&col01";
		BootOutputEndPoint = "&col01";
		NormalInputEndPoint = "&mi_01&col01";
		NormalOutputEndPoint = "&mi_01&col01";
	}

	public override string[] GetFileFilter()
	{
		return new string[1] { "bk3632" };
	}

	public override void SetFileType(CXFILE_TYPE fileType)
	{
		bootEndPoints.Clear();
		normalEndPoints.Clear();
		bootVid = "3554";
		bootPid = "f60b";
		normalVid = "3554";
		normalPid = "f60a";
		switch (fileType)
		{
		case CXFILE_TYPE.Keyboard:
			bootEndPoints.Add(new string[2] { "vid_3554&pid_f60b&col01", "vid_3554&pid_f60b&col01" });
			normalEndPoints.Add(new string[2] { "vid_3554&pid_f60a&mi_01&col01", "vid_3554&pid_f60a&mi_01&col01" });
			break;
		case CXFILE_TYPE.Mouse:
			FileType = CXFILE_TYPE.Mouse;
			bootEndPoints.Add(new string[2] { "vid_3554&pid_f60b&col01", "vid_3554&pid_f60b&col01" });
			normalEndPoints.Add(new string[2] { "vid_3554&pid_f60a&mi_01&col01", "vid_3554&pid_f60a&mi_01&col01" });
			break;
		case CXFILE_TYPE.Dongle:
			FileType = CXFILE_TYPE.Dongle;
			bootEndPoints.Add(new string[2] { "vid_3554&pid_f60b&col01", "vid_3554&pid_f60b&col01" });
			normalEndPoints.Add(new string[2] { "vid_3554&pid_f60a&mi_01&col01", "vid_3554&pid_f60a&mi_01&col01" });
			break;
		}
	}
}
