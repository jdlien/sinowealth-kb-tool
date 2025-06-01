using System.Collections.Generic;
using System.IO;
using WindControls;

namespace USBUpdateTool;

public class CompxConfig
{
	public string password1 = "";

	public string password2 = "";

	public static bool isDriverMode = true;

	public static bool isCompxSelf = false;

	public CompxConfig()
	{
		string text = FileHelper.GetAppDomainDirectory() + "compxconfig.txt";
		if (!File.Exists(text))
		{
			return;
		}
		List<string> tXTFile = FileHelper.GetTXTFile(text);
		for (int i = 0; i < tXTFile.Count; i++)
		{
			if (tXTFile[i].Contains("password1="))
			{
				password1 = tXTFile[i].Substring("password1=".Length);
				UsbUpgradeFile.SetPassward1(password1);
			}
			else if (tXTFile[i].Contains("password2="))
			{
				password2 = tXTFile[i].Substring("password2=".Length);
				UsbUpgradeFile.SetPassward2(password2);
			}
			else if (tXTFile[i].Contains("DriverMode=0"))
			{
				isDriverMode = false;
			}
		}
		if (UsbUpgradeFile.isPassward1() && UsbUpgradeFile.isPassward2())
		{
			isCompxSelf = true;
		}
	}
}
