using WindControls;

namespace USBUpdateTool;

public class DeviceFile_CX52850 : DeviceFile
{
	public DeviceFile_CX52850(string _icName, string fileNameWithoutExtension, byte[] sourceFile)
		: base(_icName, fileNameWithoutExtension, sourceFile)
	{
		BootInputEndPoint = "&mi_01&col01";
		BootOutputEndPoint = "&mi_01&col02";
		NormalInputEndPoint = "&mi_01&col05";
		NormalOutputEndPoint = "&mi_01&col07";
	}

	public override string[] GetFileFilter()
	{
		return new string[1] { "cx52850" };
	}

	public override void SetFileType(CXFILE_TYPE fileType)
	{
		bootEndPoints.Clear();
		normalEndPoints.Clear();
		bootVid = "25a7";
		bootPid = "fabc";
		normalVid = "25a7";
		normalPid = "fa7b";
		switch (fileType)
		{
		case CXFILE_TYPE.Keyboard:
			FileType = CXFILE_TYPE.Keyboard;
			bootEndPoints.Add(new string[2] { "vid_25a7&pid_fabc&mi_01&col01", "vid_25a7&pid_fabc&mi_01&col02" });
			break;
		case CXFILE_TYPE.Mouse:
			FileType = CXFILE_TYPE.Mouse;
			bootEndPoints.Add(new string[2] { "vid_25a7&pid_fabc&mi_01&col01", "vid_25a7&pid_fabc&mi_01&col02" });
			normalEndPoints.Add(new string[2] { "vid_25a7&pid_fa7b&mi_01&col05", "vid_25a7&pid_fa7b&mi_01&col07" });
			break;
		case CXFILE_TYPE.Dongle:
			FileType = CXFILE_TYPE.Dongle;
			bootEndPoints.Add(new string[2] { "vid_25a7&pid_fabc&mi_01&col01", "vid_25a7&pid_fabc&mi_01&col02" });
			break;
		}
	}
}
