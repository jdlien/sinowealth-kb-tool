using System.IO;
using WindControls;

namespace USBUpdateTool.UpgradeFile;

public class FileHeader
{
	public UpgradeFileHeader fileHeader = default(UpgradeFileHeader);

	public string icName = "";

	public string fileId = "";

	public string bootInputEndPoint = "";

	public string bootOutputEndPoint = "";

	public string normalInputEndPoint = "";

	public string normalOutputEndPoint = "";

	public string senserName = "";

	public string productName = "";

	public string bootVid = "";

	public string bootPid = "";

	public string normalVid = "";

	public string normalPid = "";

	public string version = "";

	public CXFILE_TYPE DeciveType = CXFILE_TYPE.Boot;

	public bool Valid = false;

	public FileHeader(byte[] _upgradeFile)
	{
		Parser(_upgradeFile);
	}

	private void CheckCRC(byte[] value)
	{
		uint num = 0u;
		for (int i = 8; i < 8192; i++)
		{
			num += value[i];
		}
		uint num2 = 1431655765 - num;
	}

	public FileHeader(string fileName)
	{
		if (File.Exists(fileName))
		{
			byte[] upgradeFile = File.ReadAllBytes(fileName);
			Parser(upgradeFile);
		}
		else
		{
			Valid = false;
		}
	}

	public void Parser(byte[] _upgradeFile)
	{
		if (_upgradeFile != null && DeviceFile.GetUpgradeFileHeader(_upgradeFile, out fileHeader))
		{
			fileId = DeviceFile.BytesToString(fileHeader.fileId);
			icName = DeviceFile.BytesToString(fileHeader.icName);
			if (fileHeader.nextFileAddress != 0)
			{
				byte[] array = UsbUpgradeFile.SplitFromUpgradeFile(_upgradeFile, 1);
				UpgradeFileHeader upgradeFileHeader = default(UpgradeFileHeader);
				string text = "";
				if (array != null && DeviceFile.GetUpgradeFileHeader(array, out upgradeFileHeader))
				{
					text = DeviceFile.BytesToString(upgradeFileHeader.icName);
				}
				if (icName == "CH32V305" && text == "NRF52810")
				{
					icName = "CH305+n52810";
				}
				else if (icName == "CH32V305" && text == "NRF52840_8K")
				{
					icName = "CH305+n52840_8K";
				}
				else if (icName == "CH32V305" && text == "CX52650PH")
				{
					icName = "CH305+cx52650ph";
				}
				else if (icName == "CH32V305" && text == "NRF52820_8K")
				{
					icName = "CH305+n52820_8K";
				}
				else if (icName == "CH32V305" && text == "NRF52833_8K")
				{
					icName = "CH305+n52833_8K";
				}
				else if (icName == "BS21" && text == "PY32F040B")
				{
					icName = "BS21x+PY32";
				}
			}
			bootInputEndPoint = DeviceFile.BytesToString(fileHeader.bootInputEndPoint);
			bootOutputEndPoint = DeviceFile.BytesToString(fileHeader.bootOutputEndPoint);
			normalInputEndPoint = DeviceFile.BytesToString(fileHeader.normalInputEndPoint);
			normalOutputEndPoint = DeviceFile.BytesToString(fileHeader.normalOutputEndPoint);
			senserName = DeviceFile.BytesToString(fileHeader.senserName);
			productName = DeviceFile.BytesToString(fileHeader.productName);
			DeciveType = (CXFILE_TYPE)fileHeader.DeciveType;
			bootVid = FindEndpointValue(bootInputEndPoint, "vid_");
			bootPid = FindEndpointValue(bootInputEndPoint, "pid_");
			normalVid = FindEndpointValue(normalInputEndPoint, "vid_");
			normalPid = FindEndpointValue(normalOutputEndPoint, "pid_");
			version = fileHeader.version.ToString("X");
			Valid = true;
		}
		else
		{
			Valid = false;
		}
	}

	private string FindEndpointValue(string endpoint, string startKey)
	{
		int num = endpoint.IndexOf(startKey);
		if (num >= 0)
		{
			return endpoint.Substring(num + startKey.Length, 4);
		}
		return "";
	}
}
