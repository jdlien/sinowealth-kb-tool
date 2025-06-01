using System;

namespace USBUpdateTool;

public class FileConfig
{
	public byte[] fileArray;

	public string fileName = "";

	public string exeCaption = "";

	public EXE_TYPE exeType = EXE_TYPE.PAIR;

	public void SetFile(EXE_TYPE _exeType, string _fileName, string _exeCaption)
	{
		exeType = _exeType;
		fileName = _fileName;
		exeCaption = _exeCaption;
	}

	public string ToConfigString()
	{
		int num = (int)exeType;
		string text = num.ToString();
		text = text + AppConfig.splitChar + fileName;
		return text + AppConfig.splitChar + exeCaption;
	}

	public void ToFile(string config)
	{
		string[] array = config.Split(new char[1] { AppConfig.splitChar });
		exeType = (EXE_TYPE)Convert.ToInt32(array[0]);
		fileName = array[1];
		if (array.Length >= 3)
		{
			exeCaption = array[2];
		}
	}
}
