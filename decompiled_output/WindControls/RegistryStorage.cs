using System;
using Microsoft.Win32;

namespace WindControls;

public class RegistryStorage
{
	private static Version version;

	public static string OpenAfterStart(string registryKeyString)
	{
		string result = "";
		RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\RegistryStorage");
		if (registryKey != null)
		{
			result = (string)registryKey.GetValue(registryKeyString);
			registryKey.Close();
		}
		return result;
	}

	public static void SaveBeforeExit(string registryKeyString, string text)
	{
		RegistryKey registryKey = Registry.CurrentUser.CreateSubKey("Software\\RegistryStorage");
		registryKey.SetValue(registryKeyString, text);
		registryKey.Close();
	}

	public static Version GetFrameworkVersion()
	{
		if (version == null)
		{
			string name = "SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v4\\Full\\";
			Version result = new Version(0, 0);
			try
			{
				using RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(name);
				if (!(registryKey?.GetValue("Release") is int num) || 1 == 0)
				{
					return result;
				}
				if (num >= 528040)
				{
					return new Version(4, 8);
				}
				if (num >= 461808)
				{
					return new Version(4, 7, 2);
				}
				if (num >= 461308)
				{
					return new Version(4, 7, 1);
				}
				if (num >= 460798)
				{
					return new Version(4, 7);
				}
				if (num >= 394802)
				{
					return new Version(4, 6, 2);
				}
				if (num >= 394254)
				{
					return new Version(4, 6, 1);
				}
				if (num >= 393295)
				{
					return new Version(4, 6);
				}
				if (num >= 379893)
				{
					return new Version(4, 5, 2);
				}
				if (num >= 378675)
				{
					return new Version(4, 5, 1);
				}
				if (num >= 378389)
				{
					return new Version(4, 5);
				}
			}
			catch (Exception)
			{
			}
			return new Version(0, 0);
		}
		return version;
	}
}
