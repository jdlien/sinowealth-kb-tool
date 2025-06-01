using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace WindControls;

public static class FileHelper
{
	public static Stream GetResourceStream(string _fileName)
	{
		return Assembly.GetExecutingAssembly().GetManifestResourceStream(GetResourceFilePath(_fileName));
	}

	public static string GetResourceFilePath(string _fileName)
	{
		string text = Assembly.GetExecutingAssembly().GetName().Name.ToString();
		return text + "." + _fileName;
	}

	public static string GetAppDomainDirectory()
	{
		return AppDomain.CurrentDomain.BaseDirectory;
	}

	public static byte[] GetResourceBytes(string _fileName)
	{
		Stream resourceStream = GetResourceStream(_fileName);
		byte[] array = new byte[resourceStream.Length];
		resourceStream.Read(array, 0, array.Length);
		return array;
	}

	public static List<string> GetTXTFile(string FileName)
	{
		if (File.Exists(FileName))
		{
			StreamReader streamReader = new StreamReader(FileName);
			List<string> list = new List<string>();
			string item;
			while ((item = streamReader.ReadLine()) != null)
			{
				list.Add(item);
			}
			streamReader.Close();
			return list;
		}
		return new List<string>();
	}
}
