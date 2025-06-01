using System.Collections.Generic;
using System.Text;
using USBUpdateTool;

namespace WindControls;

public class DeviceFile
{
	public string bootVid = "";

	public string bootPid = "";

	public string normalVid = "";

	public string normalPid = "";

	public string icName = "";

	public string BootInputEndPoint = "&mi_01&col01";

	public string BootOutputEndPoint = "&mi_01&col02";

	public string NormalInputEndPoint = "&mi_01&col05";

	public string NormalOutputEndPoint = "&mi_01&col05";

	public string FeatureEndPoint = "";

	public int featureInterfaceId = 1;

	public int featureDeviceId = 6;

	public List<string[]> bootEndPoints = new List<string[]>();

	public List<string[]> normalEndPoints = new List<string[]>();

	public byte[] upgradeFile = null;

	public CXFILE_TYPE FileType = CXFILE_TYPE.Boot;

	public byte cid = 0;

	public byte mid = 0;

	public bool driverPairMode = true;

	public FIRMWARE_CLASS firmwareClass = (FIRMWARE_CLASS)0;

	public virtual string[] GetFileFilter()
	{
		return new string[1] { "GenericStandard" };
	}

	public virtual void SetFileType(CXFILE_TYPE fileType)
	{
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

	public virtual void SetFileType(string fileName)
	{
		SetFileType(GetFileTypeFromFileName(fileName));
	}

	public virtual void SetFirmWare(FIRMWARE_CLASS newFirmWareClass)
	{
		firmwareClass = newFirmWareClass;
	}

	public virtual void SetBootVidPid(string vid, string pid)
	{
		if (vid.Length == 4 && pid.Length == 4)
		{
			bootVid = vid.ToLower();
			bootPid = pid.ToLower();
			string text = "vid_" + bootVid + "&pid_" + bootPid + BootInputEndPoint;
			string text2 = "vid_" + bootVid + "&pid_" + bootPid + BootOutputEndPoint;
			bootEndPoints.Clear();
			bootEndPoints.Add(new string[2] { text, text2 });
			SetBootEndPoint(bootEndPoints[0][0], bootEndPoints[0][1]);
		}
	}

	public virtual void SetNormalVidPid(string vid, string pid)
	{
		if (vid.Length == 4 && pid.Length == 4)
		{
			normalVid = vid.ToLower();
			normalPid = pid.ToLower();
			string text = "vid_" + normalVid + "&pid_" + normalPid + NormalInputEndPoint;
			string text2 = "vid_" + normalVid + "&pid_" + normalPid + NormalOutputEndPoint;
			FeatureEndPoint = "vid_" + normalVid + "&pid_" + normalPid + "&mi_0" + featureInterfaceId + "&col0" + featureDeviceId;
			normalEndPoints.Clear();
			normalEndPoints.Add(new string[2] { text, text2 });
			SetNormalEndPoint(normalEndPoints[0][0], normalEndPoints[0][1]);
		}
	}

	public bool Contain(string key)
	{
		for (int i = 0; i < bootEndPoints.Count; i++)
		{
			if (bootEndPoints[i][0] == key)
			{
				return true;
			}
		}
		for (int j = 0; j < normalEndPoints.Count; j++)
		{
			if (normalEndPoints[j][0] == key)
			{
				return true;
			}
		}
		return false;
	}

	public virtual CXFILE_TYPE GetFileTypeFromFileName(string fileName)
	{
		string text = fileName.ToLower().Replace(" ", "");
		string[] fileFilter = GetFileFilter();
		for (int i = 0; i < fileFilter.Length; i++)
		{
			string value = fileFilter[i] + "n";
			string value2 = fileFilter[i] + "p";
			if (text.IndexOf(fileFilter[i].ToLower()) >= 0 && !text.Contains(value) && !text.Contains(value2))
			{
				if (text.IndexOf("kb") > 0)
				{
					return CXFILE_TYPE.Keyboard;
				}
				if (text.IndexOf("mouse") > 0 || text.IndexOf("鼠标") > 0)
				{
					return CXFILE_TYPE.Mouse;
				}
				if (text.IndexOf("dongle") > 0)
				{
					return CXFILE_TYPE.Dongle;
				}
				return CXFILE_TYPE.Boot;
			}
		}
		return CXFILE_TYPE.Boot;
	}

	public DeviceFile()
	{
		SetFileType(CXFILE_TYPE.Boot);
	}

	public DeviceFile(CXFILE_TYPE fileType)
	{
		SetFileType(fileType);
	}

	public static string BytesToString(byte[] value)
	{
		if (value != null)
		{
			for (int i = 0; i < value.Length; i++)
			{
				if (value[i] == 0)
				{
					return Encoding.Default.GetString(value, 0, i);
				}
			}
			return Encoding.Default.GetString(value);
		}
		return "null";
	}

	public static bool GetUpgradeFileHeader(byte[] sourceFile, out UpgradeFileHeader fileHeader)
	{
		fileHeader = UsbUpgradeFile.ByteToStruct(sourceFile);
		return UsbUpgradeFile.IsValidUpgradeFile(sourceFile);
	}

	public DeviceFile(string _icName, string fileNameWithoutExtension, byte[] sourceFile, FIRMWARE_CLASS _firmwareClass)
	{
		firmwareClass = _firmwareClass;
		DeviceFileInit(_icName, fileNameWithoutExtension, sourceFile);
	}

	public DeviceFile(string _icName, string fileNameWithoutExtension, byte[] sourceFile)
	{
		DeviceFileInit(_icName, fileNameWithoutExtension, sourceFile);
	}

	private void DeviceFileInit(string _icName, string fileNameWithoutExtension, byte[] sourceFile)
	{
		icName = _icName;
		SetFileType(GetFileTypeFromFileName(fileNameWithoutExtension));
		if (GetUpgradeFileHeader(sourceFile, out var _))
		{
			upgradeFile = sourceFile;
			return;
		}
		if (sourceFile == null)
		{
			upgradeFile = null;
			return;
		}
		upgradeFile = UsbUpgradeFile.CreateUpgradeFile(icName, sourceFile);
		SetDeviceType((uint)FileType);
		SetFirmwareClass((uint)firmwareClass);
		if (normalEndPoints.Count > 0)
		{
			SetNormalEndPoint(normalEndPoints[0][0], normalEndPoints[0][1]);
		}
		if (bootEndPoints.Count > 0)
		{
			SetBootEndPoint(bootEndPoints[0][0], bootEndPoints[0][1]);
		}
	}

	public void SetNormalEndPoint(string inputEndPoint, string outputEndPoint)
	{
		upgradeFile = UsbUpgradeFile.SetNormalEndPoint(icName, upgradeFile, inputEndPoint, outputEndPoint);
	}

	public void SetBootEndPoint(string inputEndPoint, string outputEndPoint)
	{
		upgradeFile = UsbUpgradeFile.SetBootEndPoint(icName, upgradeFile, inputEndPoint, outputEndPoint);
	}

	public void SetVersion(uint version)
	{
		upgradeFile = UsbUpgradeFile.SetVersion(icName, upgradeFile, version);
	}

	public void SetDeviceType(uint deviceType)
	{
		upgradeFile = UsbUpgradeFile.SetDeviceType(icName, upgradeFile, deviceType);
	}

	public void SetFirmwareClass(uint firmwareClass)
	{
		upgradeFile = UsbUpgradeFile.SetFirmwareClass(icName, upgradeFile, firmwareClass);
	}

	public void SetCidMid(byte cid, byte mid)
	{
		upgradeFile = UsbUpgradeFile.SetCidMid(icName, upgradeFile, cid, mid);
	}

	public void SetSensorName(string sensorName)
	{
		upgradeFile = UsbUpgradeFile.SetSensorName(icName, upgradeFile, sensorName);
	}

	public void SetProductName(string productName)
	{
		upgradeFile = UsbUpgradeFile.SetProductName(icName, upgradeFile, productName);
	}
}
