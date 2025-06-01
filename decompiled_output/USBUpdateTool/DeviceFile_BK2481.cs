using WindControls;

namespace USBUpdateTool;

public class DeviceFile_BK2481 : DeviceFile
{
	public DeviceFile_BK2481(string _icName, string fileNameWithoutExtension, byte[] sourceFile)
		: base(_icName, fileNameWithoutExtension, sourceFile)
	{
		BootInputEndPoint = "&mi_01&col01";
		BootOutputEndPoint = "&mi_01&col02";
		NormalInputEndPoint = "&mi_01&col05";
		NormalOutputEndPoint = "&mi_01&col05";
	}

	public override string[] GetFileFilter()
	{
		return new string[1] { "bk2481" };
	}

	public override void SetFileType(CXFILE_TYPE fileType)
	{
		bootEndPoints.Clear();
		normalEndPoints.Clear();
		bootVid = "0000";
		bootPid = "0000";
		normalVid = "3554";
		normalPid = "f501";
		switch (fileType)
		{
		case CXFILE_TYPE.Keyboard:
			FileType = CXFILE_TYPE.Keyboard;
			break;
		case CXFILE_TYPE.Mouse:
			FileType = CXFILE_TYPE.Mouse;
			break;
		case CXFILE_TYPE.Dongle:
			FileType = CXFILE_TYPE.Dongle;
			normalEndPoints.Add(new string[2] { "vid_3554&pid_f501&mi_01&col05", "vid_3554&pid_f501&mi_01&col05" });
			break;
		}
	}
}
