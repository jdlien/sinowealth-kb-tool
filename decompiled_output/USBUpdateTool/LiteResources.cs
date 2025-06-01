using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using WindControls;

namespace USBUpdateTool;

public class LiteResources
{
	public static AppConfig appConfig = new AppConfig();

	public static Stream GetMultiLogoIconStream()
	{
		return FileHelper.GetResourceStream("MakeUpgradeTool.Files.multiLogoIcon.ico");
	}

	public static byte[] GetMultiLogoIconBytes()
	{
		Stream resourceStream = FileHelper.GetResourceStream("MakeUpgradeTool.Files.multiLogoIcon.ico");
		byte[] array = new byte[resourceStream.Length];
		resourceStream.Read(array, 0, array.Length);
		return array;
	}

	public static Icon GetIcon()
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		Stream multiLogoIconStream = GetMultiLogoIconStream();
		if (multiLogoIconStream != null && multiLogoIconStream.Length > 0)
		{
			byte[] buffer = new byte[multiLogoIconStream.Length];
			multiLogoIconStream.Read(buffer, 0, (int)multiLogoIconStream.Length);
			Image val = Image.FromStream(multiLogoIconStream);
			Bitmap val2 = new Bitmap(val, 256, 256);
			return Icon.FromHandle(val2.GetHicon());
		}
		return null;
	}

	private static Image GetImage(string fileName)
	{
		Stream resourceStream = FileHelper.GetResourceStream(fileName);
		if (resourceStream != null && resourceStream.Length > 0)
		{
			byte[] array = new byte[resourceStream.Length];
			resourceStream.Read(array, 0, (int)resourceStream.Length);
			return ImageHelper.GetImageFromBytes(array);
		}
		return null;
	}

	private static void LoadImages()
	{
		appConfig.imageConfig.BackgroundImage = GetImage("MakeUpgradeTool.Files.UpgradeToolBackImage.png");
		appConfig.imageConfig.PairBackImage = GetImage("MakeUpgradeTool.Files.pairBackImage.png");
		appConfig.imageConfig.UpgradeMouseUpImage = GetImage("MakeUpgradeTool.Files.ButtonMouseUp.png");
		appConfig.imageConfig.UpgradeMouseEnterImage = GetImage("MakeUpgradeTool.Files.ButtonMouseEnter.png");
		appConfig.imageConfig.UpgradeMouseDownImage = GetImage("MakeUpgradeTool.Files.ButtonMouseDown.png");
		appConfig.imageConfig.UpgradeFailMouseUpImage = GetImage("MakeUpgradeTool.Files.FailButtonMouseUp.png");
		appConfig.imageConfig.UpgradeFailMouseEnterImage = GetImage("MakeUpgradeTool.Files.FailButtonMouseEnter.png");
		appConfig.imageConfig.UpgradeFailMouseDownImage = GetImage("MakeUpgradeTool.Files.FailButtonMouseDown.png");
		appConfig.imageConfig.ProgressSliderImage = GetImage("MakeUpgradeTool.Files.ProgressSliderImage.png");
		appConfig.imageConfig.ProgressForeImage = GetImage("MakeUpgradeTool.Files.ProgressForeImage.png");
		appConfig.imageConfig.ProgressBackImage = GetImage("MakeUpgradeTool.Files.ProgressBackImage.png");
		appConfig.imageConfig.LogoImage = GetImage("MakeUpgradeTool.Files.PairLogo.png");
	}

	public static void LoadResourceFile()
	{
		LoadImages();
		Stream resourceStream = FileHelper.GetResourceStream("MakeUpgradeTool.Files.CompxUpgradeBinFile.bin");
		if (resourceStream != null && resourceStream.Length > 0)
		{
			appConfig.fileConfig.fileArray = new byte[resourceStream.Length];
			resourceStream.Read(appConfig.fileConfig.fileArray, 0, (int)resourceStream.Length);
		}
		resourceStream = FileHelper.GetResourceStream("MakeUpgradeTool.Files.LiteConfig.txt");
		if (resourceStream != null)
		{
			StreamReader streamReader = new StreamReader(resourceStream, Encoding.Default);
			List<string> list = new List<string>();
			string item;
			while ((item = streamReader.ReadLine()) != null)
			{
				list.Add(item);
			}
			AppLog.LogConfigTXT(list);
			appConfig.ToConfig(list);
		}
	}
}
