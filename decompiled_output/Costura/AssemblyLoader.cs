using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace Costura;

[CompilerGenerated]
internal static class AssemblyLoader
{
	private static object nullCacheLock = new object();

	private static Dictionary<string, bool> nullCache = new Dictionary<string, bool>();

	private static string tempBasePath;

	private static Dictionary<string, string> assemblyNames = new Dictionary<string, string>();

	private static Dictionary<string, string> symbolNames = new Dictionary<string, string>();

	private static List<string> preload32List = new List<string>();

	private static List<string> preload64List = new List<string>();

	private static Dictionary<string, string> checksums = new Dictionary<string, string>();

	private static int isAttached;

	private static string CultureToString(CultureInfo culture)
	{
		if (culture == null)
		{
			return "";
		}
		return culture.Name;
	}

	private static Assembly ReadExistingAssembly(AssemblyName name)
	{
		AppDomain currentDomain = AppDomain.CurrentDomain;
		Assembly[] assemblies = currentDomain.GetAssemblies();
		Assembly[] array = assemblies;
		foreach (Assembly assembly in array)
		{
			AssemblyName name2 = assembly.GetName();
			if (string.Equals(name2.Name, name.Name, StringComparison.InvariantCultureIgnoreCase) && string.Equals(CultureToString(name2.CultureInfo), CultureToString(name.CultureInfo), StringComparison.InvariantCultureIgnoreCase))
			{
				return assembly;
			}
		}
		return null;
	}

	private static Assembly ReadFromDiskCache(string tempBasePath, AssemblyName requestedAssemblyName)
	{
		string text = requestedAssemblyName.Name.ToLowerInvariant();
		if (requestedAssemblyName.CultureInfo != null && !string.IsNullOrEmpty(requestedAssemblyName.CultureInfo.Name))
		{
			text = requestedAssemblyName.CultureInfo.Name + "." + text;
		}
		string path = ((IntPtr.Size == 8) ? "64" : "32");
		string path2 = Path.Combine(tempBasePath, text + ".dll");
		if (File.Exists(path2))
		{
			return Assembly.LoadFile(path2);
		}
		path2 = Path.ChangeExtension(path2, "exe");
		if (File.Exists(path2))
		{
			return Assembly.LoadFile(path2);
		}
		path2 = Path.Combine(Path.Combine(tempBasePath, path), text + ".dll");
		if (File.Exists(path2))
		{
			return Assembly.LoadFile(path2);
		}
		path2 = Path.ChangeExtension(path2, "exe");
		if (File.Exists(path2))
		{
			return Assembly.LoadFile(path2);
		}
		return null;
	}

	private static void CopyTo(Stream source, Stream destination)
	{
		byte[] array = new byte[81920];
		int count;
		while ((count = source.Read(array, 0, array.Length)) != 0)
		{
			destination.Write(array, 0, count);
		}
	}

	private static Stream LoadStream(string fullName)
	{
		Assembly executingAssembly = Assembly.GetExecutingAssembly();
		if (fullName.EndsWith(".compressed"))
		{
			using (Stream stream = executingAssembly.GetManifestResourceStream(fullName))
			{
				using DeflateStream source = new DeflateStream(stream, CompressionMode.Decompress);
				MemoryStream memoryStream = new MemoryStream();
				CopyTo(source, memoryStream);
				memoryStream.Position = 0L;
				return memoryStream;
			}
		}
		return executingAssembly.GetManifestResourceStream(fullName);
	}

	private static Stream LoadStream(Dictionary<string, string> resourceNames, string name)
	{
		if (resourceNames.TryGetValue(name, out var value))
		{
			return LoadStream(value);
		}
		return null;
	}

	private static byte[] ReadStream(Stream stream)
	{
		byte[] array = new byte[stream.Length];
		stream.Read(array, 0, array.Length);
		return array;
	}

	private static Assembly ReadFromEmbeddedResources(Dictionary<string, string> assemblyNames, Dictionary<string, string> symbolNames, AssemblyName requestedAssemblyName)
	{
		string text = requestedAssemblyName.Name.ToLowerInvariant();
		if (requestedAssemblyName.CultureInfo != null && !string.IsNullOrEmpty(requestedAssemblyName.CultureInfo.Name))
		{
			text = requestedAssemblyName.CultureInfo.Name + "." + text;
		}
		byte[] rawAssembly;
		using (Stream stream = LoadStream(assemblyNames, text))
		{
			if (stream == null)
			{
				return null;
			}
			rawAssembly = ReadStream(stream);
		}
		using (Stream stream2 = LoadStream(symbolNames, text))
		{
			if (stream2 != null)
			{
				byte[] rawSymbolStore = ReadStream(stream2);
				return Assembly.Load(rawAssembly, rawSymbolStore);
			}
		}
		return Assembly.Load(rawAssembly);
	}

	public static Assembly ResolveAssembly(object sender, ResolveEventArgs e)
	{
		lock (nullCacheLock)
		{
			if (nullCache.ContainsKey(e.Name))
			{
				return null;
			}
		}
		AssemblyName assemblyName = new AssemblyName(e.Name);
		Assembly assembly = ReadExistingAssembly(assemblyName);
		if ((object)assembly != null)
		{
			return assembly;
		}
		assembly = ReadFromDiskCache(tempBasePath, assemblyName);
		if ((object)assembly != null)
		{
			return assembly;
		}
		assembly = ReadFromEmbeddedResources(assemblyNames, symbolNames, assemblyName);
		if ((object)assembly == null)
		{
			lock (nullCacheLock)
			{
				nullCache[e.Name] = true;
			}
			if ((assemblyName.Flags & AssemblyNameFlags.Retargetable) != AssemblyNameFlags.None)
			{
				assembly = Assembly.Load(assemblyName);
			}
		}
		return assembly;
	}

	static AssemblyLoader()
	{
		checksums.Add("costura64.hidusb.dll", "D53AAE124861C43BDF25D88219820E226855431C");
		checksums.Add("costura64.usbfile.dll", "DA773A923DD26D5668ECEB91337E0C423A009CB8");
		checksums.Add("costura64.msvcp140.dll", "AC62D2E6B9D454CE43CF6A4B218112F5AD773AD3");
		checksums.Add("costura64.oleaut32.dll", "D9D46F3BE08DD8B988B873C4E034C622BC0FB119");
		checksums.Add("costura64.vcruntime140.dll", "6FD102EB3E0B0E6EEF08118D71F28702D1A9067C");
		checksums.Add("costura64.vcruntime140_1.dll", "324C2A99E39F8992459495182677E91656A05206");
		checksums.Add("costura32.hidusb32.dll", "C26E6576E3ACFB177261755AC6104C245635CA9B");
		checksums.Add("costura32.oleaut32.dll", "D9D46F3BE08DD8B988B873C4E034C622BC0FB119");
		checksums.Add("costura32.usbfile32.dll", "F7CAC3A80107AF8A9AF30EEE44E697789DA5BC32");
		checksums.Add("costura32.vcruntime140_1.dll", "324C2A99E39F8992459495182677E91656A05206");
		checksums.Add("costura32.ucrtbased.dll", "F6238FDD0F5F0336D03E2E762D5DD645BA5992A9");
		assemblyNames.Add("costura", "costura.costura.dll.compressed");
		symbolNames.Add("costura", "costura.costura.pdb.compressed");
		assemblyNames.Add("inthehand.net.personal", "costura.inthehand.net.personal.dll.compressed");
		assemblyNames.Add("mono.cecil", "costura.mono.cecil.dll.compressed");
		assemblyNames.Add("system.diagnostics.diagnosticsource", "costura.system.diagnostics.diagnosticsource.dll.compressed");
		preload32List.Add("costura32.hidusb32.dll");
		preload32List.Add("costura32.oleaut32.dll");
		preload32List.Add("costura32.ucrtbased.dll");
		preload32List.Add("costura32.usbfile32.dll");
		preload32List.Add("costura32.vcruntime140_1.dll");
		preload64List.Add("costura64.hidusb.dll");
		preload64List.Add("costura64.msvcp140.dll");
		preload64List.Add("costura64.oleaut32.dll");
		preload64List.Add("costura64.usbfile.dll");
		preload64List.Add("costura64.vcruntime140.dll");
		preload64List.Add("costura64.vcruntime140_1.dll");
	}

	private static void CreateDirectory(string tempBasePath)
	{
		if (!Directory.Exists(tempBasePath))
		{
			Directory.CreateDirectory(tempBasePath);
		}
	}

	private static string ResourceNameToPath(string lib)
	{
		string text = ((IntPtr.Size == 8) ? "64" : "32");
		string text2 = lib;
		if (lib.StartsWith("costura" + text + "."))
		{
			text2 = Path.Combine(text, lib.Substring(10));
		}
		else if (lib.StartsWith("costura."))
		{
			text2 = lib.Substring(8);
		}
		if (text2.EndsWith(".compressed"))
		{
			text2 = text2.Substring(0, text2.Length - 11);
		}
		return text2;
	}

	private static string CalculateChecksum(string filename)
	{
		using FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
		using BufferedStream inputStream = new BufferedStream(stream);
		using SHA1CryptoServiceProvider sHA1CryptoServiceProvider = new SHA1CryptoServiceProvider();
		byte[] array = sHA1CryptoServiceProvider.ComputeHash(inputStream);
		StringBuilder stringBuilder = new StringBuilder(2 * array.Length);
		byte[] array2 = array;
		foreach (byte b in array2)
		{
			stringBuilder.AppendFormat("{0:X2}", b);
		}
		return stringBuilder.ToString();
	}

	[DllImport("kernel32.dll")]
	private static extern uint SetErrorMode(uint uMode);

	[DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
	private static extern IntPtr LoadLibraryEx(string lpFileName, IntPtr hReservedNull, uint dwFlags);

	private static void InternalPreloadUnmanagedLibraries(string tempBasePath, IList<string> libs, Dictionary<string, string> checksums)
	{
		foreach (string lib in libs)
		{
			string path = ResourceNameToPath(lib);
			string text = Path.Combine(tempBasePath, path);
			if (File.Exists(text))
			{
				string text2 = CalculateChecksum(text);
				if (text2 != checksums[lib])
				{
					File.Delete(text);
				}
			}
			if (File.Exists(text))
			{
				continue;
			}
			using Stream source = LoadStream(lib);
			using FileStream destination = File.OpenWrite(text);
			CopyTo(source, destination);
		}
		uint errorMode = 32771u;
		uint errorMode2 = SetErrorMode(errorMode);
		foreach (string lib2 in libs)
		{
			string path = ResourceNameToPath(lib2);
			if (path.EndsWith(".dll"))
			{
				string lpFileName = Path.Combine(tempBasePath, path);
				LoadLibraryEx(lpFileName, IntPtr.Zero, 8u);
			}
		}
		SetErrorMode(errorMode2);
	}

	private static void PreloadUnmanagedLibraries(string hash, string tempBasePath, List<string> libs, Dictionary<string, string> checksums)
	{
		string name = "Costura" + hash;
		using Mutex mutex = new Mutex(initiallyOwned: false, name);
		bool flag = false;
		try
		{
			try
			{
				flag = mutex.WaitOne(60000, exitContext: false);
				if (!flag)
				{
					throw new TimeoutException("Timeout waiting for exclusive access");
				}
			}
			catch (AbandonedMutexException)
			{
				flag = true;
			}
			string path = ((IntPtr.Size == 8) ? "64" : "32");
			CreateDirectory(Path.Combine(tempBasePath, path));
			InternalPreloadUnmanagedLibraries(tempBasePath, libs, checksums);
		}
		finally
		{
			if (flag)
			{
				mutex.ReleaseMutex();
			}
		}
	}

	public static void Attach()
	{
		if (Interlocked.Exchange(ref isAttached, 1) == 1)
		{
			return;
		}
		AppDomain currentDomain = AppDomain.CurrentDomain;
		object obj = currentDomain.GetType()?.GetProperty("SetupInformation")?.GetValue(currentDomain);
		PropertyInfo propertyInfo = obj?.GetType()?.GetProperty("TargetFrameworkName");
		if ((object)propertyInfo != null && propertyInfo.GetValue(obj) == null)
		{
			string text = ((TargetFrameworkAttribute)(Assembly.GetCallingAssembly()?.GetCustomAttribute(typeof(TargetFrameworkAttribute))))?.FrameworkName;
			if (text != null)
			{
				currentDomain.SetData("TargetFrameworkName", text);
			}
		}
		string text2 = "915B5FCF816A377C4B6D6231C98E68FB";
		string path = Path.Combine(Path.GetTempPath(), "Costura");
		tempBasePath = Path.Combine(path, text2);
		List<string> libs = ((IntPtr.Size == 8) ? preload64List : preload32List);
		PreloadUnmanagedLibraries(text2, tempBasePath, libs, checksums);
		currentDomain.AssemblyResolve += delegate(object sender, ResolveEventArgs e)
		{
			lock (nullCacheLock)
			{
				if (nullCache.ContainsKey(e.Name))
				{
					return (Assembly)null;
				}
			}
			AssemblyName assemblyName = new AssemblyName(e.Name);
			Assembly assembly = ReadExistingAssembly(assemblyName);
			if ((object)assembly != null)
			{
				return assembly;
			}
			assembly = ReadFromDiskCache(tempBasePath, assemblyName);
			if ((object)assembly != null)
			{
				return assembly;
			}
			assembly = ReadFromEmbeddedResources(assemblyNames, symbolNames, assemblyName);
			if ((object)assembly == null)
			{
				lock (nullCacheLock)
				{
					nullCache[e.Name] = true;
				}
				if ((assemblyName.Flags & AssemblyNameFlags.Retargetable) != AssemblyNameFlags.None)
				{
					assembly = Assembly.Load(assemblyName);
				}
			}
			return assembly;
		};
	}
}
