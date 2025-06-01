using WindControls;

namespace USBUpdateTool;

public class FileManager
{
	public static string GetUpgradeBinFileName()
	{
		string text = RegistryStorage.OpenAfterStart("HidUsbUpdateToolFileName");
		if (text != null)
		{
			return text;
		}
		return "";
	}

	public static void SaveUpgradeBinFileName(string fileName)
	{
		string upgradeBinFileName = GetUpgradeBinFileName();
		if (upgradeBinFileName != fileName)
		{
			RegistryStorage.SaveBeforeExit("HidUsbUpdateToolFileName", fileName);
		}
	}

	public static string GetCustomNormalVidPid(string icName)
	{
		if (icName != "")
		{
			string text = RegistryStorage.OpenAfterStart("HidUsbUpdateToolNormalVidPid_" + icName);
			if (text != null)
			{
				return text;
			}
		}
		return "";
	}

	public static void SaveCustomNormalVidPid(string icName, string vidPid)
	{
		if (icName != "")
		{
			string customNormalVidPid = GetCustomNormalVidPid(icName);
			if (customNormalVidPid != vidPid)
			{
				RegistryStorage.SaveBeforeExit("HidUsbUpdateToolNormalVidPid_" + icName, vidPid);
			}
		}
	}

	public static string GetCustomBootVidPid(string icName)
	{
		if (icName != "")
		{
			string text = RegistryStorage.OpenAfterStart("HidUsbUpdateToolBootVidPid_" + icName);
			if (text != null)
			{
				return text;
			}
		}
		return "";
	}

	public static void SaveCustomBootVidPid(string icName, string vidPid)
	{
		if (icName != "")
		{
			string customBootVidPid = GetCustomBootVidPid(icName);
			if (customBootVidPid != vidPid)
			{
				RegistryStorage.SaveBeforeExit("HidUsbUpdateToolBootVidPid_" + icName, vidPid);
			}
		}
	}

	public static string GetCidMid()
	{
		string text = RegistryStorage.OpenAfterStart("HidUsbUpdateToolCidMid");
		if (text != null)
		{
			return text;
		}
		return "";
	}

	public static void SaveCidMid(string cidMid)
	{
		string cidMid2 = GetCidMid();
		if (cidMid2 != cidMid)
		{
			RegistryStorage.SaveBeforeExit("HidUsbUpdateToolCidMid", cidMid);
		}
	}

	public static string GetLastICType()
	{
		string text = RegistryStorage.OpenAfterStart("HidUsbUpdateToolICSelect");
		if (text != null)
		{
			return text;
		}
		return "";
	}

	public static void SaveLastICType(string icName)
	{
		string lastICType = GetLastICType();
		if (lastICType != icName)
		{
			RegistryStorage.SaveBeforeExit("HidUsbUpdateToolICSelect", icName);
		}
	}

	public static string GetLastPairICType()
	{
		string text = RegistryStorage.OpenAfterStart("HidUsbUpdateToolPairICSelect");
		if (text != null)
		{
			return text;
		}
		return "";
	}

	public static void SaveLastPairICType(string icName)
	{
		string lastPairICType = GetLastPairICType();
		if (lastPairICType != icName)
		{
			RegistryStorage.SaveBeforeExit("HidUsbUpdateToolPairICSelect", icName);
		}
	}
}
