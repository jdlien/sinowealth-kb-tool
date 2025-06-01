using System.Collections.Generic;
using System.IO;
using DriverLib;
using WindControls;

namespace USBUpdateTool;

public class ICSelect
{
	public static string[] IC_ITEMs = new string[19]
	{
		"BK3635", "BK3632-OTA", "CX53710", "CH305+n52810", "NRF52833", "NRF52840", "CX52850", "CX52650N", "CX52650", "CX52650P",
		"CX52850P", "CH305+cx52650ph", "CH305+n52840_8K", "CX53730", "CH305+n52820_8K", "CX52850E", "CH305+n52833_8K", "BS21x", "BS21x+PY32"
	};

	public List<DeviceFile> deviceFileList = new List<DeviceFile>();

	public byte[] customFile = null;

	public bool binFileTooLarge = false;

	public string FindIcName(string fileName)
	{
		List<string> list = new List<string>();
		fileName = Path.GetFileNameWithoutExtension(fileName).ToLower();
		for (int i = 0; i < IC_ITEMs.Length; i++)
		{
			if (fileName.Contains(IC_ITEMs[i].ToLower()))
			{
				list.Add(IC_ITEMs[i]);
			}
		}
		if ((fileName.Contains("ch305") || fileName.Contains("ch32")) && fileName.Contains("cx52650ph"))
		{
			return "CH305+cx52650ph";
		}
		if ((fileName.Contains("ch305") || fileName.Contains("ch32")) && fileName.Contains("52820") && fileName.Contains("8k"))
		{
			return "CH305+n52820_8K";
		}
		if ((fileName.Contains("ch305") || fileName.Contains("ch32")) && fileName.Contains("52840") && fileName.Contains("8k"))
		{
			return "CH305+n52840_8K";
		}
		if ((fileName.Contains("ch305") || fileName.Contains("ch32")) && fileName.Contains("52833") && fileName.Contains("8k"))
		{
			return "CH305+n52833_8K";
		}
		if (fileName.Contains("bs21") && fileName.Contains("py32"))
		{
			return "BS21x+PY32";
		}
		if (list.Count > 0)
		{
			string text = list[0];
			for (int j = 1; j < list.Count; j++)
			{
				if (text.Length < list[j].Length)
				{
					text = list[j];
				}
			}
			return text;
		}
		if ((fileName.Contains("ch305") || fileName.Contains("ch32")) && fileName.Contains("52810"))
		{
			return "CH305+n52810";
		}
		if (fileName.Contains("bk3632") && fileName.Contains("ota"))
		{
			return "BK3632-OTA";
		}
		return "";
	}

	public void CreateDeviceFile(string fileNamePath, string icName, string normalVid, string normalPid, string bootVid, string bootPid)
	{
	}

	public byte[] CreateUpgradeFile(string fileNameWithoutExtension, byte[] sourceFile, string icName, string normalVid, string normalPid, string bootVid, string bootPid)
	{
		byte[] array = null;
		deviceFileList.Clear();
		binFileTooLarge = false;
		switch (icName)
		{
		case "NRF52833":
		{
			DeviceFile deviceFile2 = new DeviceFile_NRF52833(icName, fileNameWithoutExtension, sourceFile);
			deviceFile2.SetNormalVidPid(normalVid, normalPid);
			deviceFile2.SetBootVidPid(bootVid, bootPid);
			deviceFileList.Add(deviceFile2);
			array = deviceFile2.upgradeFile;
			break;
		}
		case "NRF52840":
		{
			DeviceFile deviceFile2 = new DeviceFile_NRF52840(icName, fileNameWithoutExtension, sourceFile);
			deviceFile2.SetNormalVidPid(normalVid, normalPid);
			deviceFile2.SetBootVidPid(bootVid, bootPid);
			deviceFileList.Add(deviceFile2);
			array = deviceFile2.upgradeFile;
			break;
		}
		case "BK3635":
		{
			DeviceFile deviceFile2 = new DeviceFile_BK3635(icName, fileNameWithoutExtension, sourceFile);
			deviceFile2.SetNormalVidPid(normalVid, normalPid);
			deviceFile2.SetBootVidPid(bootVid, bootPid);
			deviceFileList.Add(deviceFile2);
			array = deviceFile2.upgradeFile;
			if (array != null && array.Length > 163328)
			{
				binFileTooLarge = true;
			}
			break;
		}
		case "BK3632-OTA":
		{
			DeviceFile deviceFile2 = new DeviceFile_BK3632_BT(icName, fileNameWithoutExtension, sourceFile);
			deviceFile2.SetNormalVidPid(normalVid, normalPid);
			deviceFile2.SetBootVidPid(bootVid, bootPid);
			deviceFileList.Add(deviceFile2);
			array = deviceFile2.upgradeFile;
			if (array != null && array.Length > 163328)
			{
				binFileTooLarge = true;
			}
			break;
		}
		case "CX53710":
		{
			DeviceFile deviceFile2 = new DeviceFile_CX53710(icName, fileNameWithoutExtension, sourceFile);
			deviceFile2.SetNormalVidPid(normalVid, normalPid);
			deviceFile2.SetBootVidPid(bootVid, bootPid);
			deviceFileList.Add(deviceFile2);
			array = deviceFile2.upgradeFile;
			if (array != null && array.Length > 163328)
			{
				binFileTooLarge = true;
			}
			break;
		}
		case "CX52850":
		{
			DeviceFile deviceFile2 = new DeviceFile_CX52850(icName, fileNameWithoutExtension, sourceFile);
			deviceFile2.SetNormalVidPid(normalVid, normalPid);
			deviceFile2.SetBootVidPid(bootVid, bootPid);
			deviceFileList.Add(deviceFile2);
			array = deviceFile2.upgradeFile;
			break;
		}
		case "CH32+52810":
		case "CH305+n52810":
		{
			DeviceFile deviceFile3 = new DeviceFile_CH32V305("CH32V305", fileNameWithoutExtension, sourceFile);
			deviceFile3.SetNormalVidPid(normalVid, normalPid);
			deviceFile3.SetBootVidPid(bootVid, bootPid);
			deviceFileList.Add(deviceFile3);
			DeviceFile deviceFile2 = new DeviceFile_NRF52810("NRF52810", fileNameWithoutExtension, sourceFile);
			deviceFile2.SetNormalVidPid(normalVid, normalPid);
			deviceFile2.SetBootVidPid(normalVid, normalPid);
			deviceFileList.Add(deviceFile2);
			array = UsbUpgradeFile.AppendUpgradeFile(deviceFile3.upgradeFile, deviceFile2.upgradeFile);
			break;
		}
		case "CH305+cx52650ph":
		{
			DeviceFile deviceFile3 = new DeviceFile_CH32V305PH("CH32V305", fileNameWithoutExtension, sourceFile);
			deviceFile3.SetNormalVidPid(normalVid, normalPid);
			deviceFile3.SetBootVidPid(bootVid, bootPid);
			deviceFileList.Add(deviceFile3);
			DeviceFile deviceFile2 = new DeviceFile_CX52650PH("CX52650PH", fileNameWithoutExtension, sourceFile);
			deviceFile2.SetNormalVidPid(normalVid, normalPid);
			deviceFile2.SetBootVidPid(normalVid, normalPid);
			deviceFileList.Add(deviceFile2);
			array = UsbUpgradeFile.AppendUpgradeFile(deviceFile3.upgradeFile, deviceFile2.upgradeFile);
			break;
		}
		case "CH305+n52840_8K":
		{
			DeviceFile deviceFile3 = new DeviceFile_CH32V305_8K("CH32V305", fileNameWithoutExtension, sourceFile);
			deviceFile3.SetNormalVidPid(normalVid, normalPid);
			deviceFile3.SetBootVidPid(bootVid, bootPid);
			deviceFileList.Add(deviceFile3);
			DeviceFile deviceFile2 = new DeviceFile_NRF52840_8K("NRF52840_8K", fileNameWithoutExtension, sourceFile);
			deviceFile2.SetNormalVidPid(normalVid, normalPid);
			deviceFile2.SetBootVidPid(normalVid, normalPid);
			deviceFileList.Add(deviceFile2);
			array = UsbUpgradeFile.AppendUpgradeFile(deviceFile3.upgradeFile, deviceFile2.upgradeFile);
			break;
		}
		case "CH305+n52833_8K":
		{
			DeviceFile deviceFile3 = new DeviceFile_CH32V305_8K("CH32V305", fileNameWithoutExtension, sourceFile);
			deviceFile3.SetNormalVidPid(normalVid, normalPid);
			deviceFile3.SetBootVidPid("3554", "F40B");
			deviceFile3.SetBootVidPid(bootVid, bootPid);
			deviceFileList.Add(deviceFile3);
			DeviceFile deviceFile2 = new DeviceFile_NRF52833_8K("NRF52833_8K", fileNameWithoutExtension, sourceFile);
			deviceFile2.SetNormalVidPid(normalVid, normalPid);
			deviceFile2.SetBootVidPid(normalVid, normalPid);
			deviceFileList.Add(deviceFile2);
			array = UsbUpgradeFile.AppendUpgradeFile(deviceFile3.upgradeFile, deviceFile2.upgradeFile);
			break;
		}
		case "CH305+n52820_8K":
		{
			DeviceFile deviceFile3 = new DeviceFile_CH32V305_N52820_8K("CH32V305", fileNameWithoutExtension, sourceFile);
			deviceFile3.SetNormalVidPid(normalVid, normalPid);
			deviceFile3.SetBootVidPid(bootVid, bootPid);
			deviceFileList.Add(deviceFile3);
			DeviceFile deviceFile2 = new DeviceFile_NRF52820_8K("NRF52820_8K", fileNameWithoutExtension, sourceFile);
			deviceFile2.SetNormalVidPid(normalVid, normalPid);
			deviceFile2.SetBootVidPid(normalVid, normalPid);
			deviceFileList.Add(deviceFile2);
			array = UsbUpgradeFile.AppendUpgradeFile(deviceFile3.upgradeFile, deviceFile2.upgradeFile);
			break;
		}
		case "CH32V305":
		{
			DeviceFile deviceFile2 = new DeviceFile_CH32V305(icName, fileNameWithoutExtension, sourceFile);
			deviceFile2.SetNormalVidPid(normalVid, normalPid);
			deviceFile2.SetBootVidPid(bootVid, bootPid);
			deviceFileList.Add(deviceFile2);
			array = deviceFile2.upgradeFile;
			break;
		}
		case "CX52650":
		{
			DeviceFile deviceFile2 = new DeviceFile_CX52650(icName, fileNameWithoutExtension, sourceFile);
			deviceFile2.SetNormalVidPid(normalVid, normalPid);
			deviceFile2.SetBootVidPid(bootVid, bootPid);
			deviceFileList.Add(deviceFile2);
			array = deviceFile2.upgradeFile;
			break;
		}
		case "CX52650N":
		{
			DeviceFile deviceFile2 = new DeviceFile_CX52650N(icName, fileNameWithoutExtension, sourceFile);
			deviceFile2.SetNormalVidPid(normalVid, normalPid);
			deviceFile2.SetBootVidPid(bootVid, bootPid);
			deviceFileList.Add(deviceFile2);
			array = deviceFile2.upgradeFile;
			break;
		}
		case "CX52650P":
		{
			DeviceFile deviceFile2 = new DeviceFile_CX52650P(icName, fileNameWithoutExtension, sourceFile);
			deviceFile2.SetNormalVidPid(normalVid, normalPid);
			deviceFile2.SetBootVidPid(bootVid, bootPid);
			deviceFileList.Add(deviceFile2);
			array = deviceFile2.upgradeFile;
			break;
		}
		case "CX52850P":
		{
			DeviceFile deviceFile2 = new DeviceFile_CX52850P(icName, fileNameWithoutExtension, sourceFile);
			deviceFile2.SetNormalVidPid(normalVid, normalPid);
			deviceFile2.SetBootVidPid(bootVid, bootPid);
			deviceFileList.Add(deviceFile2);
			array = deviceFile2.upgradeFile;
			break;
		}
		case "CX52850E":
		{
			DeviceFile deviceFile2 = new DeviceFile_CX52850E(icName, fileNameWithoutExtension, sourceFile);
			deviceFile2.SetNormalVidPid(normalVid, normalPid);
			deviceFile2.SetBootVidPid(bootVid, bootPid);
			deviceFileList.Add(deviceFile2);
			array = deviceFile2.upgradeFile;
			break;
		}
		case "BK2481":
		{
			DeviceFile deviceFile2 = new DeviceFile_BK2481(icName, fileNameWithoutExtension, sourceFile);
			deviceFile2.SetNormalVidPid(normalVid, normalPid);
			deviceFile2.SetBootVidPid(bootVid, bootPid);
			deviceFileList.Add(deviceFile2);
			array = deviceFile2.upgradeFile;
			break;
		}
		case "CX53730":
		{
			DeviceFile deviceFile2 = new DeviceFile_CX53730(icName, fileNameWithoutExtension, sourceFile);
			deviceFile2.SetNormalVidPid(normalVid, normalPid);
			deviceFile2.SetBootVidPid(bootVid, bootPid);
			deviceFileList.Add(deviceFile2);
			array = deviceFile2.upgradeFile;
			break;
		}
		case "GenericStandard":
		{
			DeviceFile deviceFile2 = new DeviceFile(icName, fileNameWithoutExtension, sourceFile);
			deviceFile2.SetNormalVidPid(normalVid, normalPid);
			deviceFile2.SetBootVidPid(bootVid, bootPid);
			deviceFileList.Add(deviceFile2);
			array = deviceFile2.upgradeFile;
			break;
		}
		case "BS21x":
		{
			DeviceFile deviceFile2 = new DeviceFile_BS21x(icName, fileNameWithoutExtension, sourceFile, FIRMWARE_CLASS.BS21x_ONLY);
			deviceFile2.SetNormalVidPid(normalVid, normalPid);
			deviceFile2.SetBootVidPid(bootVid, bootPid);
			deviceFileList.Add(deviceFile2);
			array = deviceFile2.upgradeFile;
			break;
		}
		case "BS21x+PY32":
		{
			DeviceFile deviceFile = new DeviceFile_BS21x("BS21x", fileNameWithoutExtension, sourceFile, FIRMWARE_CLASS.BS21x_PY32F040B);
			deviceFile.SetNormalVidPid(normalVid, normalPid);
			deviceFile.SetBootVidPid(bootVid, bootPid);
			deviceFileList.Add(deviceFile);
			DeviceFile deviceFile2 = new DeviceFile_PY32F040B("PY32F040B", fileNameWithoutExtension, sourceFile, FIRMWARE_CLASS.BS21x_PY32F040B);
			deviceFile2.SetNormalVidPid(normalVid, normalPid);
			deviceFile2.SetBootVidPid(normalVid, normalPid);
			deviceFile2.SetFirmwareClass(4u);
			deviceFileList.Add(deviceFile2);
			array = UsbUpgradeFile.AppendUpgradeFile(deviceFile.upgradeFile, deviceFile2.upgradeFile);
			break;
		}
		}
		AppLog.LogCreateUpgradeFile(icName, deviceFileList);
		return array;
	}

	public void CreatePairDeviceFile(List<DeviceConfig> deviceConfigs)
	{
		List<DeviceFile> list = new List<DeviceFile>();
		for (int i = 0; i < deviceConfigs.Count; i++)
		{
			string fileNameWithoutExtension = deviceConfigs[i].icName + "_Dongle.bin";
			CreateUpgradeFile(fileNameWithoutExtension, null, deviceConfigs[i].icName, deviceConfigs[i].normalVid, deviceConfigs[i].normalPid, deviceConfigs[i].bootVid, deviceConfigs[i].bootPid);
			if (deviceFileList.Count > 0)
			{
				deviceFileList[0].cid = deviceConfigs[i].cid;
				deviceFileList[0].mid = deviceConfigs[i].mid;
				deviceFileList[0].driverPairMode = deviceConfigs[i].isDriverPairMode;
				list.Add(deviceFileList[0]);
			}
		}
		deviceFileList.Clear();
		if (list.Count > 0)
		{
			deviceFileList.AddRange(list);
		}
		AppLog.LogCreateUpgradeFile("STD Pair", deviceFileList);
	}

	public bool ReadCidMid(bool isKeyboard, out DeviceInfo deviceInfo)
	{
		deviceInfo.CID = 0;
		deviceInfo.MID = 0;
		deviceInfo.DeviceType = 0;
		for (int i = 0; i < deviceFileList.Count; i++)
		{
			List<string[]> list = UpgradeManager.FindHidDevicesEndPoint(deviceFileList[i].normalEndPoints);
			if (list.Count == 1)
			{
				UsbFinder.GetDeviceInfo(list[0][0], isKeyboard, out deviceInfo);
				AppLog.LogReadCidMid(deviceFileList[i], list, deviceInfo);
				return true;
			}
			AppLog.LogReadCidMid(deviceFileList[i], list, deviceInfo);
		}
		return false;
	}

	public bool WriteCidMid(byte cid, byte mid, byte deviceType, out DeviceInfo deviceInfo)
	{
		deviceInfo.CID = 0;
		deviceInfo.MID = 0;
		deviceInfo.DeviceType = 0;
		for (int i = 0; i < deviceFileList.Count; i++)
		{
			List<string[]> list = UpgradeManager.FindHidDevicesEndPoint(deviceFileList[i].normalEndPoints);
			if (list.Count == 1)
			{
				UsbFinder.WriteCidMid(list[0][0], cid, mid, deviceType, out deviceInfo);
				return true;
			}
		}
		return false;
	}

	public int GetDeviceFilePacketCount(DeviceFile deviceFile)
	{
		int num = 0;
		if (deviceFile.upgradeFile != null)
		{
			num = deviceFile.upgradeFile.Length / 32;
			if (num * 32 < deviceFile.upgradeFile.Length)
			{
				num++;
			}
		}
		return num;
	}

	private int CalcDeviceFilePacketTotalCount(List<DeviceFile> deviceFiles)
	{
		int num = 0;
		for (int i = 0; i < deviceFiles.Count; i++)
		{
			num += GetDeviceFilePacketCount(deviceFiles[i]);
		}
		return num;
	}
}
