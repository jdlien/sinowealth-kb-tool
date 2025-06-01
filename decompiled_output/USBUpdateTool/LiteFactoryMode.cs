using System;
using System.IO;
using System.Text;

namespace USBUpdateTool;

public class LiteFactoryMode
{
	public bool IsFactoryMode = false;

	public LiteFactoryMode()
	{
		GetFactoryFile();
	}

	private void GetFactoryFile()
	{
		string path = AppDomain.CurrentDomain.BaseDirectory + "\\HidToolFactoryFile.txt";
		if (!File.Exists(path))
		{
			return;
		}
		try
		{
			using FileStream stream = new FileStream(path, FileMode.Open);
			using StreamReader streamReader = new StreamReader(stream, Encoding.Default);
			string text;
			if ((text = streamReader.ReadLine()) != null && text.IndexOf("HidToolFactoryMode=true") >= 0)
			{
				IsFactoryMode = true;
			}
		}
		catch
		{
		}
	}
}
