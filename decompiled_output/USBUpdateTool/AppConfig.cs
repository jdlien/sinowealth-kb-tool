using System.Collections.Generic;
using System.Drawing;

namespace USBUpdateTool;

public class AppConfig
{
	public List<string> configTxtList = new List<string>();

	public static char splitChar = '^';

	public FileConfig fileConfig = new FileConfig();

	public DeviceConfig UpgradeDevice = new DeviceConfig();

	public ControlConfig SuccessUpgradeButton = new ControlConfig();

	public ControlConfig FailUpgradeButton = new ControlConfig();

	public ControlConfig MiniButton = new ControlConfig();

	public ControlConfig CloseButton = new ControlConfig();

	public ControlConfig CaptionTextBox = new ControlConfig();

	public ControlConfig CurFwVersionTextBox = new ControlConfig();

	public ControlConfig NewFwVersionTextBox = new ControlConfig();

	public ControlConfig ToolVersionTextBox = new ControlConfig();

	public ControlConfig ProgressControl = new ControlConfig();

	public ControlConfig LogoPicture = new ControlConfig();

	public ControlConfig Pair4KButton = new ControlConfig();

	public ControlConfig DefaultButton = new ControlConfig();

	public ControlConfig PairSettingButton = new ControlConfig();

	public ImageConfig imageConfig = new ImageConfig();

	public string iconFileName = "";

	public List<DeviceConfig> PairVidOnlyDevices = new List<DeviceConfig>();

	public List<DeviceConfig> PairVidPidDevices = new List<DeviceConfig>();

	public void SetOffset(Point offset)
	{
		SuccessUpgradeButton.SetOffset(offset);
		FailUpgradeButton.SetOffset(offset);
		MiniButton.SetOffset(offset);
		CloseButton.SetOffset(offset);
		CaptionTextBox.SetOffset(offset);
		CurFwVersionTextBox.SetOffset(offset);
		NewFwVersionTextBox.SetOffset(offset);
		ToolVersionTextBox.SetOffset(offset);
		ProgressControl.SetOffset(offset);
		LogoPicture.SetOffset(offset);
		Pair4KButton.SetOffset(offset);
		DefaultButton.SetOffset(offset);
		PairSettingButton.SetOffset(offset);
	}

	public string ToConfigTxt()
	{
		string text = "FileConfig=" + fileConfig.ToConfigString() + "\r\n";
		text = text + "UpgradeDevice=" + UpgradeDevice.ToConfigString() + "\r\n";
		text = text + "SuccessUpgradeButton=" + SuccessUpgradeButton.ToConfigString() + "\r\n";
		text = text + "FailUpgradeButton=" + FailUpgradeButton.ToConfigString() + "\r\n";
		text = text + "MiniButton=" + MiniButton.ToConfigString() + "\r\n";
		text = text + "CloseButton=" + CloseButton.ToConfigString() + "\r\n";
		text = text + "CaptionTextBox=" + CaptionTextBox.ToConfigString() + "\r\n";
		text = text + "CurFwVersionTextBox=" + CurFwVersionTextBox.ToConfigString() + "\r\n";
		text = text + "NewFwVersionTextBox=" + NewFwVersionTextBox.ToConfigString() + "\r\n";
		text = text + "ToolVersionTextBox=" + ToolVersionTextBox.ToConfigString() + "\r\n";
		text = text + "ProgressControl=" + ProgressControl.ToConfigString() + "\r\n";
		text = text + "ProgressConfig=" + imageConfig.ToPorgressConfig() + "\r\n";
		text = text + "LogoPictureControl=" + LogoPicture.ToConfigString() + "\r\n";
		text = text + "Pair4KControl=" + Pair4KButton.ToConfigString() + "\r\n";
		text = text + "DefaultButtonControl=" + DefaultButton.ToConfigString() + "\r\n";
		text = text + "PairSettingButtonControl=" + PairSettingButton.ToConfigString() + "\r\n";
		for (int i = 0; i < PairVidOnlyDevices.Count; i++)
		{
			text = text + "PairVidOnlyDevice=" + PairVidOnlyDevices[i].ToConfigString() + "\r\n";
		}
		for (int j = 0; j < PairVidPidDevices.Count; j++)
		{
			text = text + "PairVidPidDevice=" + PairVidPidDevices[j].ToConfigString() + "\r\n";
		}
		return text;
	}

	private string FindValue(string source, ref string value)
	{
		int num = source.IndexOf("=");
		string result = "error";
		if (num >= 0)
		{
			result = source.Substring(0, num + 1);
			value = source.Substring(num + 1);
		}
		return result;
	}

	public void ToConfig(List<string> configList)
	{
		string text = "";
		string value = "";
		configTxtList.Clear();
		configTxtList.AddRange(configList);
		PairVidOnlyDevices.Clear();
		PairVidPidDevices.Clear();
		for (int i = 0; i < configList.Count; i++)
		{
			switch (FindValue(configList[i], ref value))
			{
			case "FileConfig=":
				fileConfig.ToFile(value);
				break;
			case "UpgradeDevice=":
				UpgradeDevice.ToDevice(value);
				break;
			case "SuccessUpgradeButton=":
				SuccessUpgradeButton.ToControl(value);
				break;
			case "FailUpgradeButton=":
				FailUpgradeButton.ToControl(value);
				break;
			case "MiniButton=":
				MiniButton.ToControl(value);
				break;
			case "CloseButton=":
				CloseButton.ToControl(value);
				break;
			case "CaptionTextBox=":
				CaptionTextBox.ToControl(value);
				break;
			case "CurFwVersionTextBox=":
				CurFwVersionTextBox.ToControl(value);
				break;
			case "NewFwVersionTextBox=":
				NewFwVersionTextBox.ToControl(value);
				break;
			case "ToolVersionTextBox=":
				ToolVersionTextBox.ToControl(value);
				break;
			case "ProgressControl=":
				ProgressControl.ToControl(value);
				break;
			case "ProgressConfig=":
				imageConfig.ToPorgressControl(value);
				break;
			case "LogoPictureControl=":
				LogoPicture.ToControl(value);
				break;
			case "Pair4KControl=":
				Pair4KButton.ToControl(value);
				break;
			case "DefaultButtonControl=":
				DefaultButton.ToControl(value);
				break;
			case "PairSettingButtonControl=":
				PairSettingButton.ToControl(value);
				break;
			case "PairVidOnlyDevice=":
			{
				DeviceConfig deviceConfig = new DeviceConfig();
				deviceConfig.ToDevice(value);
				PairVidOnlyDevices.Add(deviceConfig);
				break;
			}
			case "PairVidPidDevice=":
			{
				DeviceConfig deviceConfig = new DeviceConfig();
				deviceConfig.ToDevice(value);
				PairVidPidDevices.Add(deviceConfig);
				break;
			}
			}
		}
	}
}
