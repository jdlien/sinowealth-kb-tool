using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using Mono.Cecil;
using Mono.Collections.Generic;

namespace USBUpdateTool;

public class LoadExeResource
{
	private string[] ResourcesName = new string[13]
	{
		"USBUpdateTool.MakeUpgradeTool.Files.LiteConfig.txt", "USBUpdateTool.MakeUpgradeTool.Files.multiLogoIcon.ico", "USBUpdateTool.MakeUpgradeTool.Files.UpgradeToolBackImage.png", "USBUpdateTool.MakeUpgradeTool.Files.ButtonMouseUp.png", "USBUpdateTool.MakeUpgradeTool.Files.ButtonMouseEnter.png", "USBUpdateTool.MakeUpgradeTool.Files.ButtonMouseDown.png", "USBUpdateTool.MakeUpgradeTool.Files.FailButtonMouseUp.png", "USBUpdateTool.MakeUpgradeTool.Files.FailButtonMouseEnter.png", "USBUpdateTool.MakeUpgradeTool.Files.FailButtonMouseDown.png", "USBUpdateTool.MakeUpgradeTool.Files.ProgressSliderImage.png",
		"USBUpdateTool.MakeUpgradeTool.Files.ProgressForeImage.png", "USBUpdateTool.MakeUpgradeTool.Files.ProgressBackImage.png", "USBUpdateTool.MakeUpgradeTool.Files.pairBackImage.png"
	};

	public List<string> LiteConfigTextList = new List<string>();

	public byte[] multiLogoIcon = null;

	public Image UpgradeToolBackImage;

	public Image UpgradeButtonMouseUpImage = null;

	public Image UpgradeButtonMouseEnterImage = null;

	public Image UpgradeButtonMouseDownImage = null;

	public Image UpgradeFailButtonMouseUpImage = null;

	public Image UpgradeFailButtonMouseEnterImage = null;

	public Image UpgradeFailButtonMouseDownImage = null;

	public Image ProgressSliderImage = null;

	public Image ProgressForeImage = null;

	public Image ProgressBackImage = null;

	public Image LogoImage = null;

	public Image PairBackImage = null;

	private MemoryStream GetResourceData(EmbeddedResource resource)
	{
		Stream resourceStream = resource.GetResourceStream();
		byte[] array = new byte[resourceStream.Length];
		resourceStream.Read(array, 0, array.Length);
		MemoryStream memoryStream = new MemoryStream();
		memoryStream.Write(array, 0, array.Length);
		memoryStream.Position = 0L;
		return memoryStream;
	}

	private byte[] GetResourceBytes(EmbeddedResource resource)
	{
		Stream resourceStream = resource.GetResourceStream();
		byte[] array = new byte[resourceStream.Length];
		resourceStream.Read(array, 0, array.Length);
		return array;
	}

	private void GetLiteConfigTxt(MemoryStream memoryStream)
	{
		byte[] bytes = memoryStream.ToArray();
		string text = Encoding.Default.GetString(bytes);
		string[] collection = text.Split(new string[1] { "\r\n" }, StringSplitOptions.None);
		LiteConfigTextList.Clear();
		LiteConfigTextList.AddRange(collection);
	}

	private Image GetImage(MemoryStream memoryStream)
	{
		return Image.FromStream((Stream)memoryStream, true);
	}

	public void Load(string fileName, ref AppConfig appConfig)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Expected O, but got Unknown
		AssemblyDefinition val = AssemblyDefinition.ReadAssembly(fileName);
		try
		{
			for (int i = 0; i < ResourcesName.Length; i++)
			{
				Enumerator<Resource> enumerator = val.MainModule.Resources.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						Resource current = enumerator.Current;
						if (current.Name == ResourcesName[i])
						{
							MemoryStream resourceData = GetResourceData((EmbeddedResource)current);
							switch (ResourcesName[i])
							{
							case "USBUpdateTool.MakeUpgradeTool.Files.LiteConfig.txt":
								GetLiteConfigTxt(resourceData);
								break;
							case "USBUpdateTool.MakeUpgradeTool.Files.multiLogoIcon.ico":
								multiLogoIcon = GetResourceBytes((EmbeddedResource)current);
								break;
							case "USBUpdateTool.MakeUpgradeTool.Files.UpgradeToolBackImage.png":
								UpgradeToolBackImage = GetImage(resourceData);
								break;
							case "USBUpdateTool.MakeUpgradeTool.Files.ButtonMouseUp.png":
								UpgradeButtonMouseUpImage = GetImage(resourceData);
								break;
							case "USBUpdateTool.MakeUpgradeTool.Files.ButtonMouseEnter.png":
								UpgradeButtonMouseEnterImage = GetImage(resourceData);
								break;
							case "USBUpdateTool.MakeUpgradeTool.Files.ButtonMouseDown.png":
								UpgradeButtonMouseDownImage = GetImage(resourceData);
								break;
							case "USBUpdateTool.MakeUpgradeTool.Files.FailButtonMouseUp.png":
								UpgradeFailButtonMouseUpImage = GetImage(resourceData);
								break;
							case "USBUpdateTool.MakeUpgradeTool.Files.FailButtonMouseEnter.png":
								UpgradeFailButtonMouseEnterImage = GetImage(resourceData);
								break;
							case "USBUpdateTool.MakeUpgradeTool.Files.FailButtonMouseDown.png":
								UpgradeFailButtonMouseDownImage = GetImage(resourceData);
								break;
							case "USBUpdateTool.MakeUpgradeTool.Files.ProgressSliderImage.png":
								ProgressSliderImage = GetImage(resourceData);
								break;
							case "USBUpdateTool.MakeUpgradeTool.Files.ProgressForeImage.png":
								ProgressForeImage = GetImage(resourceData);
								break;
							case "USBUpdateTool.MakeUpgradeTool.Files.ProgressBackImage.png":
								ProgressBackImage = GetImage(resourceData);
								break;
							case "USBUpdateTool.MakeUpgradeTool.Files.PairLogo.png":
								LogoImage = GetImage(resourceData);
								break;
							case "USBUpdateTool.MakeUpgradeTool.Files.pairBackImage.png":
								PairBackImage = GetImage(resourceData);
								break;
							}
						}
					}
				}
				finally
				{
					((IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
				}
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		appConfig.imageConfig.multiLogoIcon = multiLogoIcon;
		appConfig.imageConfig.ProgressBackImage = ProgressBackImage;
		appConfig.imageConfig.ProgressSliderImage = ProgressSliderImage;
		appConfig.imageConfig.ProgressForeImage = ProgressForeImage;
		appConfig.imageConfig.UpgradeMouseUpImage = UpgradeButtonMouseUpImage;
		appConfig.imageConfig.UpgradeMouseEnterImage = UpgradeButtonMouseEnterImage;
		appConfig.imageConfig.UpgradeMouseDownImage = UpgradeButtonMouseDownImage;
		appConfig.imageConfig.UpgradeFailMouseUpImage = UpgradeFailButtonMouseUpImage;
		appConfig.imageConfig.UpgradeFailMouseEnterImage = UpgradeFailButtonMouseEnterImage;
		appConfig.imageConfig.UpgradeFailMouseDownImage = UpgradeFailButtonMouseDownImage;
		appConfig.imageConfig.LogoImage = LogoImage;
		appConfig.imageConfig.PairBackImage = PairBackImage;
		appConfig.ToConfig(LiteConfigTextList);
	}
}
