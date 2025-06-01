using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Management;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using DriverLib;
using WindControls;

namespace USBUpdateTool;

public class AppLog
{
	private enum UpgradeStartState
	{
		NoError,
		FileError,
		OpenUsbError,
		DeviceCountError,
		NotFoundDeviceError
	}

	public static bool lgoEnable = false;

	private static string logFileName = "";

	public AppLog()
	{
		Init();
	}

	public static void Init()
	{
		logFileName = FileHelper.GetAppDomainDirectory() + "applog.txt";
		if (File.Exists(logFileName))
		{
			lgoEnable = true;
			File.WriteAllText(logFileName, string.Empty);
		}
		else
		{
			lgoEnable = false;
		}
	}

	private static string GetTime()
	{
		return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":     ";
	}

	public static void LogDeviceInfo()
	{
		if (lgoEnable)
		{
			List<string> list = new List<string>();
			list.Add(GetTime() + "event = DeviceInfo\r\n{\r\n");
			list.AddRange(GetOSVersion().ToArray());
			list.Add(GetCPUNumber());
			list.Add("APP Lite");
			list.Add(LiteResources.appConfig.fileConfig.fileName);
			list.Add("Lite Normal Vid Pid: " + LiteResources.appConfig.UpgradeDevice.normalVid + "-" + LiteResources.appConfig.UpgradeDevice.normalPid);
			list.Add("Lite Boot Vid Pid: " + LiteResources.appConfig.UpgradeDevice.bootVid + "-" + LiteResources.appConfig.UpgradeDevice.bootPid);
			list.Add("Lite Cid Mid: " + LiteResources.appConfig.UpgradeDevice.cid + "-" + LiteResources.appConfig.UpgradeDevice.mid);
			list.Add("Lite icName: " + LiteResources.appConfig.UpgradeDevice.icName);
			string[] dllVersion = UsbFinder.GetDllVersion();
			list.Add("DLL Version :" + dllVersion[0]);
			list.Add(".Net Version :");
			list.AddRange(GetDotNetVersions());
			list.AddRange(UsbFinder.EnumHidDeviceList());
			list.Add("}\r\n");
			File.AppendAllLines(logFileName, list);
		}
	}

	public static List<string> GetDotNetVersions()
	{
		string path = Environment.SystemDirectory + "\\..\\Microsoft.NET\\Framework";
		DirectoryInfo[] directories = new DirectoryInfo(path).GetDirectories("v?.?.*");
		List<string> list = new List<string>();
		DirectoryInfo[] array = directories;
		foreach (DirectoryInfo directoryInfo in array)
		{
			list.Add(directoryInfo.Name.Substring(1));
		}
		return list;
	}

	public static string GetCPUNumber()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		string result = "";
		ManagementObjectSearcher val = new ManagementObjectSearcher("Select * from Win32_Processor");
		ManagementObjectEnumerator enumerator = val.Get().GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				ManagementObject val2 = (ManagementObject)enumerator.Current;
				result = ((ManagementBaseObject)val2)["Name"].ToString();
			}
		}
		finally
		{
			((IDisposable)enumerator)?.Dispose();
		}
		((Component)(object)val).Dispose();
		return result;
	}

	public static List<string> GetOSVersion()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		List<string> list = new List<string>();
		try
		{
			ManagementObjectSearcher val = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
			ManagementObjectEnumerator enumerator = val.Get().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					ManagementObject val2 = (ManagementObject)enumerator.Current;
					list.Add(((ManagementBaseObject)val2)["Name"].ToString().Trim() + "     " + (Environment.Is64BitOperatingSystem ? "64bit" : "32bit"));
					list.Add(((ManagementBaseObject)val2)["SerialNumber"].ToString().Trim());
					list.Add(((ManagementBaseObject)val2)["OSLanguage"].ToString().Trim());
					list.Add(((ManagementBaseObject)val2)["Manufacturer"].ToString().Trim());
				}
			}
			finally
			{
				((IDisposable)enumerator)?.Dispose();
			}
		}
		catch
		{
			list.Add("null");
		}
		return list;
	}

	public static void LogCreateUpgradeFile(string icName, List<DeviceFile> deviceFileList)
	{
		if (!lgoEnable)
		{
			return;
		}
		List<string> list = new List<string>();
		list.Add(GetTime() + "event = CreateUpgradeFile\r\n{\r\n");
		list.Add("icName = " + icName);
		if (deviceFileList != null)
		{
			for (int i = 0; i < deviceFileList.Count; i++)
			{
				list.Add("File Type = " + deviceFileList[i].FileType.ToString() + "    " + deviceFileList[i].ToString());
				for (int j = 0; j < deviceFileList[i].bootEndPoints.Count; j++)
				{
					list.Add("boot input = " + deviceFileList[i].bootEndPoints[j][0]);
					list.Add("boot output = " + deviceFileList[i].bootEndPoints[j][1]);
				}
				for (int k = 0; k < deviceFileList[i].normalEndPoints.Count; k++)
				{
					list.Add("normal input = " + deviceFileList[i].normalEndPoints[k][0]);
					list.Add("normal output = " + deviceFileList[i].normalEndPoints[k][1]);
				}
				list.AddRange(GetFileHead(deviceFileList[i].upgradeFile));
			}
		}
		else
		{
			list.Add("deviceFileList = null");
		}
		list.Add("}\r\n");
		File.AppendAllLines(logFileName, list);
	}

	private static List<string> GetFileHead(byte[] sourceFile)
	{
		List<string> list = new List<string>();
		if (!DeviceFile.GetUpgradeFileHeader(sourceFile, out var fileHeader) || sourceFile == null)
		{
			list.Add("FileHead invalid");
		}
		list.Add("FileHead Info");
		list.Add("fwLength = " + fileHeader.fwLength);
		list.Add("fileId = " + DeviceFile.BytesToString(fileHeader.fileId));
		list.Add("icName = " + DeviceFile.BytesToString(fileHeader.icName));
		list.Add("boot input = " + DeviceFile.BytesToString(fileHeader.bootInputEndPoint));
		list.Add("boot output = " + DeviceFile.BytesToString(fileHeader.bootOutputEndPoint));
		list.Add("resetToUpdateMode = " + StringHelper.ByteToHexString(fileHeader.resetToUpdateModeCmd));
		list.Add("prepareDownLoad = " + StringHelper.ByteToHexString(fileHeader.prepareDownLoadCmd));
		list.Add("dataDownLoad = " + StringHelper.ByteToHexString(fileHeader.dataDownLoadCmd));
		return list;
	}

	public static string GetUpgradeResult(int value)
	{
		UpgradeStartState upgradeStartState = (UpgradeStartState)value;
		return upgradeStartState.ToString();
	}

	public static void LogUpgradeStartNomal(List<string[]> devicesList, List<string[]> founddevicesList)
	{
		if (!lgoEnable)
		{
			return;
		}
		List<string> list = new List<string>();
		list.Add(GetTime() + "event = UpgradeStartNomal\r\n{\r\n");
		if (devicesList != null)
		{
			list.Add("want to find device count = " + devicesList.Count);
			for (int i = 0; i < devicesList.Count; i++)
			{
				list.Add("find input devices = " + devicesList[i][0]);
				list.Add("find output devices = " + devicesList[i][1]);
			}
			list.Add("found device count = " + founddevicesList.Count);
		}
		else
		{
			list.Add("want to find device count = null error");
		}
		if (founddevicesList != null)
		{
			for (int j = 0; j < founddevicesList.Count; j++)
			{
				list.Add("found input devices = " + founddevicesList[j][0]);
				list.Add("found output devices = " + founddevicesList[j][1]);
			}
		}
		else
		{
			list.Add("found device count = null error");
		}
		list.Add("}\r\n");
		File.AppendAllLines(logFileName, list);
	}

	public static void LogUpgradeStartNomalResult(bool result)
	{
		if (lgoEnable)
		{
			List<string> list = new List<string>();
			list.Add(GetTime() + "event = UpgradeStartNomalResult\r\n{\r\n");
			list.Add("Normal UsbUpgrade Start DLL Result = " + result);
			list.Add("}\r\n");
			File.AppendAllLines(logFileName, list);
		}
	}

	public static void LogUpgradeStartBoot(string[] bootDevices)
	{
		if (lgoEnable)
		{
			List<string> list = new List<string>();
			if (bootDevices == null)
			{
				bootDevices = new string[1] { "" };
			}
			list.Add(GetTime() + "event = UpgradeStartBoot\r\n{\r\n");
			list.Add("  Boot UsbUpgrade Start, found device count = " + bootDevices.Length);
			for (int i = 0; i < bootDevices.Length; i++)
			{
				list.Add("boot found input devices = " + bootDevices[i][0]);
				list.Add("boot found output devices = " + bootDevices[i][1]);
			}
			list.Add("}\r\n");
			File.AppendAllLines(logFileName, list);
		}
	}

	public static void LogUpgradeStartBootResult(bool result)
	{
		if (lgoEnable)
		{
			List<string> list = new List<string>();
			list.Add(GetTime() + "event = UpgradeStartBootResult\r\n{\r\n");
			list.Add("Boot UsbUpgrade Start Result = " + result);
			list.Add("}\r\n");
			File.AppendAllLines(logFileName, list);
		}
	}

	public static void LogOnUsbDeviceChanged(List<DeviceFile> deviceFileList, int foundIndex, bool waitBootMode, bool isStarted)
	{
		if (!lgoEnable)
		{
			return;
		}
		List<string> list = new List<string>();
		list.Add(GetTime() + "event = OnUsbDeviceChanged\r\n{\r\n");
		list.Add("foundIndex = " + foundIndex + "   waitBootMode = " + waitBootMode + "    isStarted = " + isStarted);
		if (deviceFileList != null)
		{
			for (int i = 0; i < deviceFileList.Count; i++)
			{
				list.Add("File Type = " + deviceFileList[i].FileType.ToString() + "    " + deviceFileList[i].ToString());
				for (int j = 0; j < deviceFileList[i].bootEndPoints.Count; j++)
				{
					list.Add("boot input = " + deviceFileList[i].bootEndPoints[j][0]);
					list.Add("boot output = " + deviceFileList[i].bootEndPoints[j][1]);
				}
				for (int k = 0; k < deviceFileList[i].normalEndPoints.Count; k++)
				{
					list.Add("normal input = " + deviceFileList[i].normalEndPoints[k][0]);
					list.Add("normal output = " + deviceFileList[i].normalEndPoints[k][1]);
				}
				list.AddRange(GetFileHead(deviceFileList[i].upgradeFile));
			}
		}
		else
		{
			list.Add("deviceFileList = null");
		}
		list.Add("}\r\n");
		File.AppendAllLines(logFileName, list);
	}

	public static void LogReadCidMid(DeviceFile deviceFile, List<string[]> normalDevicesList, DeviceInfo deviceInfo)
	{
		if (!lgoEnable)
		{
			return;
		}
		List<string> list = new List<string>();
		list.Add(GetTime() + "event = ReadCidMid\r\n{\r\n");
		list.Add("want to find device:");
		if (deviceFile != null)
		{
			if (deviceFile.normalEndPoints != null)
			{
				for (int i = 0; i < deviceFile.normalEndPoints.Count; i++)
				{
					list.Add("normal input = " + deviceFile.normalEndPoints[i][0]);
					list.Add("normal output = " + deviceFile.normalEndPoints[i][1]);
				}
			}
			else
			{
				list.Add("deviceFile.normalEndPoints = null error");
			}
		}
		else
		{
			list.Add("deviceFile = null error");
		}
		list.Add("found device:");
		if (normalDevicesList != null)
		{
			for (int j = 0; j < normalDevicesList.Count; j++)
			{
				if (normalDevicesList[j].Length > 1)
				{
					list.Add("normal input = " + normalDevicesList[j][0]);
					list.Add("normal output = " + normalDevicesList[j][1]);
				}
			}
		}
		else
		{
			list.Add("normalDevicesList = null error");
		}
		list.Add("cid:  " + deviceInfo.CID);
		list.Add("mid:  " + deviceInfo.MID);
		list.Add("DeviceType:  " + deviceInfo.DeviceType);
		list.Add("}\r\n");
		File.AppendAllLines(logFileName, list);
	}

	public static void LogSaveUpgradeCommand(byte[] command)
	{
		if (lgoEnable)
		{
			if (command != null)
			{
				File.AppendAllText(logFileName, GetTime() + "Upgrade callback data - " + StringHelper.ByteToHexString(command) + "\r\n");
			}
			else
			{
				File.AppendAllText(logFileName, GetTime() + "Upgrade callback data - null error\r\n");
			}
		}
	}

	public static void LogApplication_ThreadException(string msg)
	{
		if (lgoEnable)
		{
			List<string> list = new List<string>();
			list.Add(GetTime() + "event = Application_ThreadException\r\n{\r\n");
			list.Add(msg);
			list.Add("}\r\n");
			File.AppendAllLines(logFileName, list);
		}
	}

	public static void LogCurrentDomain_UnhandledException(string msg)
	{
		if (lgoEnable)
		{
			List<string> list = new List<string>();
			list.Add(GetTime() + "event = CurrentDomain_UnhandledException\r\n{\r\n");
			list.Add(msg);
			list.Add("}\r\n");
			File.AppendAllLines(logFileName, list);
		}
	}

	public static void LogControls(Form form)
	{
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Expected O, but got Unknown
		if (!lgoEnable)
		{
			return;
		}
		List<string> list = new List<string>();
		list.Add(GetTime() + "event = Controls Information\r\n{\r\n");
		ResizeControl.GetDpi();
		list.Add("Screen DPI = " + ResizeControl.ScreenDPI);
		list.Add("Screen Scaling  = " + ResizeControl.ScreenDPI * 100 / 96 + "%");
		list.Add("Screen Width = " + Screen.PrimaryScreen.Bounds.Width);
		list.Add("Screen Height = " + Screen.PrimaryScreen.Bounds.Height);
		list.Add("Window Location = " + form.Location.X + "  " + form.Location.Y);
		list.Add("Window Width = " + ((Control)form).Width);
		list.Add("Window Height = " + ((Control)form).Height);
		list.Add("BackImage Width = " + ((Control)form).BackgroundImage.Width);
		list.Add("BackImage Height = " + ((Control)form).BackgroundImage.Height);
		foreach (Control item in (ArrangedElementCollection)((Control)form).Controls)
		{
			Control val = item;
			if (val.Visible)
			{
				list.Add("  {\r\n");
				list.Add("  Control Name:" + val.Name);
				list.Add("  Control Text:" + val.Text);
				list.Add("  Control Location:" + val.Location.X + "  " + val.Location.Y);
				list.Add("  Control Size:" + val.Width + "  " + val.Height);
				list.Add("  }\r\n");
			}
		}
		list.Add("}\r\n");
		File.AppendAllLines(logFileName, list);
	}

	public static void LogConfigTXT(List<string> configTxtList)
	{
		if (!lgoEnable)
		{
			return;
		}
		List<string> list = new List<string>();
		list.Add(GetTime() + "event = ConfigTXT\r\n{\r\n");
		if (configTxtList != null)
		{
			for (int i = 0; i < configTxtList.Count; i++)
			{
				list.Add(configTxtList[i]);
			}
		}
		list.Add("}\r\n");
		File.AppendAllLines(logFileName, list);
	}

	public static void LogUpgradeLog(string[] upgradeLogs)
	{
		if (!lgoEnable)
		{
			return;
		}
		List<string> list = new List<string>();
		list.Add(GetTime() + "event = UpgradeLog\r\n{\r\n");
		if (upgradeLogs != null)
		{
			for (int i = 0; i < upgradeLogs.Length; i++)
			{
				list.Add(upgradeLogs[i]);
			}
		}
		else
		{
			list.Add("upgradeLogs = null error");
		}
		list.Add("}\r\n");
		File.AppendAllLines(logFileName, list);
	}

	public static void LogConfigTxt(List<string> configTxtList)
	{
		if (!lgoEnable)
		{
			return;
		}
		List<string> list = new List<string>();
		list.Add(GetTime() + "event = LiteConfigTxt\r\n{\r\n");
		if (configTxtList != null)
		{
			for (int i = 0; i < configTxtList.Count; i++)
			{
				list.Add(configTxtList[i]);
			}
		}
		else
		{
			list.Add("LiteConfigTxt = null error");
		}
		list.Add("}\r\n");
		File.AppendAllLines(logFileName, list);
	}

	public static void LogCreateExeException(string msg)
	{
		if (lgoEnable)
		{
			List<string> list = new List<string>();
			list.Add(GetTime() + "event = LogCreateExeException\r\n{\r\n");
			list.Add(msg);
			list.Add("}\r\n");
			File.AppendAllLines(logFileName, list);
		}
	}

	public static void LogMessage(string msg)
	{
		if (lgoEnable)
		{
			List<string> list = new List<string>();
			list.Add(GetTime() + "event = Message\r\n{\r\n");
			list.Add(msg);
			list.Add("}\r\n");
			File.AppendAllLines(logFileName, list);
		}
	}
}
