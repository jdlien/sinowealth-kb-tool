using System;
using System.IO;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Collections.Generic;
using USBUpdateTool;

namespace WindControls;

public class ExeResourceManager
{
	public bool BulidExe(AppConfig config, string exefileName)
	{
		string text = CreateTempFile(exefileName);
		if (File.Exists(text))
		{
			ReplaceResource(text, config, exefileName);
			IconChanger iconChanger = new IconChanger();
			MemoryStream iconsStream = new MemoryStream(config.imageConfig.multiLogoIcon);
			iconChanger.ChangeIcon(exefileName, iconsStream);
			return true;
		}
		return false;
	}

	public void ReplaceResource(string tempPath, AppConfig config, string exeFileName)
	{
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Expected O, but got Unknown
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Expected O, but got Unknown
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Expected O, but got Unknown
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Expected O, but got Unknown
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Expected O, but got Unknown
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Expected O, but got Unknown
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Expected O, but got Unknown
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02be: Expected O, but got Unknown
		//IL_0301: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Expected O, but got Unknown
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Expected O, but got Unknown
		//IL_0395: Unknown result type (might be due to invalid IL or missing references)
		//IL_039c: Expected O, but got Unknown
		//IL_03df: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e6: Expected O, but got Unknown
		//IL_0429: Unknown result type (might be due to invalid IL or missing references)
		//IL_0430: Expected O, but got Unknown
		//IL_0473: Unknown result type (might be due to invalid IL or missing references)
		//IL_047a: Expected O, but got Unknown
		//IL_04bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c4: Expected O, but got Unknown
		//IL_0507: Unknown result type (might be due to invalid IL or missing references)
		//IL_050e: Expected O, but got Unknown
		string[] array = new string[15]
		{
			"USBUpdateTool.MakeUpgradeTool.Files.CompxUpgradeBinFile.bin", "USBUpdateTool.MakeUpgradeTool.Files.LiteConfig.txt", "USBUpdateTool.MakeUpgradeTool.Files.multiLogoIcon.ico", "USBUpdateTool.MakeUpgradeTool.Files.UpgradeToolBackImage.png", "USBUpdateTool.MakeUpgradeTool.Files.ButtonMouseUp.png", "USBUpdateTool.MakeUpgradeTool.Files.ButtonMouseEnter.png", "USBUpdateTool.MakeUpgradeTool.Files.ButtonMouseDown.png", "USBUpdateTool.MakeUpgradeTool.Files.FailButtonMouseUp.png", "USBUpdateTool.MakeUpgradeTool.Files.FailButtonMouseEnter.png", "USBUpdateTool.MakeUpgradeTool.Files.FailButtonMouseDown.png",
			"USBUpdateTool.MakeUpgradeTool.Files.ProgressSliderImage.png", "USBUpdateTool.MakeUpgradeTool.Files.ProgressForeImage.png", "USBUpdateTool.MakeUpgradeTool.Files.ProgressBackImage.png", "USBUpdateTool.MakeUpgradeTool.Files.PairLogo.png", "USBUpdateTool.MakeUpgradeTool.Files.pairBackImage.png"
		};
		CustomResolver assemblyResolver = new CustomResolver();
		ReaderParameters val = new ReaderParameters
		{
			AssemblyResolver = (IAssemblyResolver)(object)assemblyResolver,
			ReadSymbols = false
		};
		AssemblyDefinition val2 = AssemblyDefinition.ReadAssembly(tempPath, val);
		try
		{
			for (int i = 0; i < array.Length; i++)
			{
				for (int j = 0; j < val2.MainModule.Resources.Count; j++)
				{
					if (val2.MainModule.Resources[j].Name == array[i])
					{
						val2.MainModule.Resources.RemoveAt(j);
						break;
					}
				}
			}
			EmbeddedResource val3 = new EmbeddedResource("USBUpdateTool.MakeUpgradeTool.Files.CompxUpgradeBinFile.bin", (ManifestResourceAttributes)1, config.fileConfig.fileArray);
			val2.MainModule.Resources.Add((Resource)(object)val3);
			string s = config.ToConfigTxt();
			byte[] bytes = Encoding.Default.GetBytes(s);
			val3 = new EmbeddedResource("USBUpdateTool.MakeUpgradeTool.Files.LiteConfig.txt", (ManifestResourceAttributes)1, bytes);
			val2.MainModule.Resources.Add((Resource)(object)val3);
			byte[] array2 = config.imageConfig.multiLogoIcon.ToArray();
			val3 = new EmbeddedResource("USBUpdateTool.MakeUpgradeTool.Files.multiLogoIcon.ico", (ManifestResourceAttributes)1, array2);
			val2.MainModule.Resources.Add((Resource)(object)val3);
			array2 = ImageHelper.GetBytesFromImage(config.imageConfig.BackgroundImage);
			val3 = new EmbeddedResource("USBUpdateTool.MakeUpgradeTool.Files.UpgradeToolBackImage.png", (ManifestResourceAttributes)1, array2);
			val2.MainModule.Resources.Add((Resource)(object)val3);
			if (config.imageConfig.UpgradeMouseUpImage != null)
			{
				array2 = ImageHelper.GetBytesFromImage(config.imageConfig.UpgradeMouseUpImage);
				val3 = new EmbeddedResource("USBUpdateTool.MakeUpgradeTool.Files.ButtonMouseUp.png", (ManifestResourceAttributes)1, array2);
				val2.MainModule.Resources.Add((Resource)(object)val3);
			}
			if (config.imageConfig.UpgradeMouseEnterImage != null)
			{
				array2 = ImageHelper.GetBytesFromImage(config.imageConfig.UpgradeMouseEnterImage);
				val3 = new EmbeddedResource("USBUpdateTool.MakeUpgradeTool.Files.ButtonMouseEnter.png", (ManifestResourceAttributes)1, array2);
				val2.MainModule.Resources.Add((Resource)(object)val3);
			}
			if (config.imageConfig.UpgradeMouseDownImage != null)
			{
				array2 = ImageHelper.GetBytesFromImage(config.imageConfig.UpgradeMouseDownImage);
				val3 = new EmbeddedResource("USBUpdateTool.MakeUpgradeTool.Files.ButtonMouseDown.png", (ManifestResourceAttributes)1, array2);
				val2.MainModule.Resources.Add((Resource)(object)val3);
			}
			if (config.imageConfig.UpgradeFailMouseUpImage != null)
			{
				array2 = ImageHelper.GetBytesFromImage(config.imageConfig.UpgradeFailMouseUpImage);
				val3 = new EmbeddedResource("USBUpdateTool.MakeUpgradeTool.Files.FailButtonMouseUp.png", (ManifestResourceAttributes)1, array2);
				val2.MainModule.Resources.Add((Resource)(object)val3);
			}
			if (config.imageConfig.UpgradeFailMouseEnterImage != null)
			{
				array2 = ImageHelper.GetBytesFromImage(config.imageConfig.UpgradeFailMouseEnterImage);
				val3 = new EmbeddedResource("USBUpdateTool.MakeUpgradeTool.Files.FailButtonMouseEnter.png", (ManifestResourceAttributes)1, array2);
				val2.MainModule.Resources.Add((Resource)(object)val3);
			}
			if (config.imageConfig.UpgradeFailMouseDownImage != null)
			{
				array2 = ImageHelper.GetBytesFromImage(config.imageConfig.UpgradeFailMouseDownImage);
				val3 = new EmbeddedResource("USBUpdateTool.MakeUpgradeTool.Files.FailButtonMouseDown.png", (ManifestResourceAttributes)1, array2);
				val2.MainModule.Resources.Add((Resource)(object)val3);
			}
			if (config.imageConfig.ProgressSliderImage != null)
			{
				array2 = ImageHelper.GetBytesFromImage(config.imageConfig.ProgressSliderImage);
				val3 = new EmbeddedResource("USBUpdateTool.MakeUpgradeTool.Files.ProgressSliderImage.png", (ManifestResourceAttributes)1, array2);
				val2.MainModule.Resources.Add((Resource)(object)val3);
			}
			if (config.imageConfig.ProgressForeImage != null)
			{
				array2 = ImageHelper.GetBytesFromImage(config.imageConfig.ProgressForeImage);
				val3 = new EmbeddedResource("USBUpdateTool.MakeUpgradeTool.Files.ProgressForeImage.png", (ManifestResourceAttributes)1, array2);
				val2.MainModule.Resources.Add((Resource)(object)val3);
			}
			if (config.imageConfig.ProgressBackImage != null)
			{
				array2 = ImageHelper.GetBytesFromImage(config.imageConfig.ProgressBackImage);
				val3 = new EmbeddedResource("USBUpdateTool.MakeUpgradeTool.Files.ProgressBackImage.png", (ManifestResourceAttributes)1, array2);
				val2.MainModule.Resources.Add((Resource)(object)val3);
			}
			if (config.imageConfig.LogoImage != null)
			{
				array2 = ImageHelper.GetBytesFromImage(config.imageConfig.LogoImage);
				val3 = new EmbeddedResource("USBUpdateTool.MakeUpgradeTool.Files.PairLogo.png", (ManifestResourceAttributes)1, array2);
				val2.MainModule.Resources.Add((Resource)(object)val3);
			}
			if (config.imageConfig.PairBackImage != null)
			{
				array2 = ImageHelper.GetBytesFromImage(config.imageConfig.PairBackImage);
				val3 = new EmbeddedResource("USBUpdateTool.MakeUpgradeTool.Files.pairBackImage.png", (ManifestResourceAttributes)1, array2);
				val2.MainModule.Resources.Add((Resource)(object)val3);
			}
			val2.Write(exeFileName);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		File.Delete(tempPath);
	}

	private string CreateTempFile(string fileName)
	{
		string text = "";
		Stream resourceStream = FileHelper.GetResourceStream("MakeUpgradeTool.Files.USBUpdateTool.exe");
		if (resourceStream != null)
		{
			byte[] array = new byte[resourceStream.Length];
			resourceStream.Read(array, 0, (int)resourceStream.Length);
			text = Path.GetTempFileName();
			FileStream fileStream = new FileStream(text, FileMode.Create);
			fileStream.Write(array, 0, array.Length);
			fileStream.Close();
		}
		return text;
	}

	public void AddResource(string path, string resourceName, byte[] resource)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		AssemblyDefinition val = AssemblyDefinition.ReadAssembly(path);
		EmbeddedResource val2 = new EmbeddedResource(resourceName, (ManifestResourceAttributes)1, resource);
		val.MainModule.Resources.Add((Resource)(object)val2);
		val.Write(path);
	}

	public MemoryStream GetResource(string path, string resourceName)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		AssemblyDefinition val = AssemblyDefinition.ReadAssembly(path);
		Enumerator<Resource> enumerator = val.MainModule.Resources.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Resource current = enumerator.Current;
				if (current.Name == resourceName)
				{
					EmbeddedResource val2 = (EmbeddedResource)current;
					Stream resourceStream = val2.GetResourceStream();
					byte[] array = new byte[resourceStream.Length];
					resourceStream.Read(array, 0, array.Length);
					MemoryStream memoryStream = new MemoryStream();
					memoryStream.Write(array, 0, array.Length);
					memoryStream.Position = 0L;
					return memoryStream;
				}
			}
		}
		finally
		{
			((IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
		}
		return null;
	}
}
