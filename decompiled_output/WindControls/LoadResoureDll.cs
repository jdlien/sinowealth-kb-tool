using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace WindControls;

public static class LoadResoureDll
{
	private static Dictionary<string, Assembly> LoadedDlls = new Dictionary<string, Assembly>();

	private static Dictionary<string, object> Assemblies = new Dictionary<string, object>();

	private static Assembly AssemblyResolve(object sender, ResolveEventArgs args)
	{
		try
		{
			string fullName = new AssemblyName(args.Name).FullName;
			if (LoadedDlls.TryGetValue(fullName, out var value) && value != null)
			{
				LoadedDlls[fullName] = null;
				return value;
			}
			throw new DllNotFoundException(fullName);
		}
		catch
		{
			return null;
		}
	}

	public static void RegistDLL(string pattern = "*.dll")
	{
		Directory.GetFiles("", "");
		Assembly assembly = new StackTrace(0).GetFrame(1).GetMethod().Module.Assembly;
		if (Assemblies.ContainsKey(assembly.FullName))
		{
			return;
		}
		Assemblies.Add(assembly.FullName, null);
		AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolve;
		string[] manifestResourceNames = assembly.GetManifestResourceNames();
		Regex regex = new Regex("^" + pattern.Replace(".", "\\.").Replace("*", ".*").Replace("_", ".") + "$", RegexOptions.IgnoreCase);
		string[] array = manifestResourceNames;
		foreach (string text in array)
		{
			if (!regex.IsMatch(text) || text.ToLower().Contains("costura"))
			{
				continue;
			}
			try
			{
				Stream manifestResourceStream = assembly.GetManifestResourceStream(text);
				byte[] array2 = new byte[manifestResourceStream.Length];
				manifestResourceStream.Read(array2, 0, (int)manifestResourceStream.Length);
				Assembly assembly2 = Assembly.Load(array2);
				if (!LoadedDlls.ContainsKey(assembly2.FullName))
				{
					LoadedDlls[assembly2.FullName] = assembly2;
				}
			}
			catch
			{
			}
		}
	}
}
