using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;

namespace WindControls;

internal class ExeIconReplace
{
	private struct ICONDIRENTRY
	{
		private byte bWidth;

		private byte bHeight;

		private byte bColorCount;

		private byte bReserved;

		private short wPlanes;

		private short wBitCount;

		public int dwBytesInRes;

		public int dwImageOffset;
	}

	private struct ICONDIR
	{
		private short idReserved;

		private short idType;

		public short idCount;
	}

	private struct GRPICONDIRENTRY
	{
		private byte bWidth;

		private byte bHeight;

		private byte bColorCount;

		private byte bReserved;

		private short wPlanes;

		private short wBitCount;

		private int dwBytesInRes;

		public short nID;
	}

	private struct GRPICONDIR
	{
		public short idReserved;

		public short idType;

		public short idCount;

		public GRPICONDIRENTRY idEntries;
	}

	[DllImport("kernel32.dll", SetLastError = true)]
	public static extern IntPtr BeginUpdateResource(string pFileName, [MarshalAs(UnmanagedType.Bool)] bool bDeleteExistingResources);

	[DllImport("kernel32.dll", SetLastError = true)]
	public static extern bool UpdateResource(IntPtr hUpdate, uint lpType, uint lpName, ushort wLanguage, byte[] lpData, uint cbData);

	[DllImport("kernel32.dll", SetLastError = true)]
	public static extern bool UpdateResource(IntPtr hUpdate, string lpType, string lpName, ushort wLanguage, IntPtr lpData, uint cbData);

	[DllImport("kernel32.dll", SetLastError = true)]
	public static extern bool EndUpdateResource(IntPtr hUpdate, bool fDiscard);

	public static void ReplaceExeIco(string exepath, string icopath)
	{
		try
		{
			using FileStream fileStream = new FileStream(icopath, FileMode.Open, FileAccess.Read);
			byte[] array = new byte[fileStream.Length];
			fileStream.Read(array, 0, array.Length);
			ReplaceExeIco(exepath, array);
		}
		catch
		{
		}
	}

	public static void ReplaceExeIcon(Icon icon, string exeFileName)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		if (!File.Exists(exeFileName))
		{
			return;
		}
		Bitmap val = new Bitmap(ImageHelper.IconToImage(icon));
		try
		{
			string tempFileName = Path.GetTempFileName();
			using (Stream stream = new FileStream(tempFileName, FileMode.Create))
			{
				icon.Save(stream);
			}
			ReplaceExeIco(exeFileName, tempFileName);
			File.Delete(tempFileName);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public static void ReplaceExeIco(string exepath, byte[] iconFileByte)
	{
		try
		{
			int num = 0;
			uint num2 = 0u;
			byte[] array = new byte[4];
			Array.Copy(iconFileByte, 14, array, 0, 4);
			num2 = BitConverter.ToUInt32(array, 0);
			byte[] array2 = new byte[4];
			Array.Copy(iconFileByte, 18, array2, 0, 4);
			num = BitConverter.ToInt32(array2, 0);
			byte[] array3 = new byte[num2];
			Array.Copy(iconFileByte, num, array3, 0L, num2);
			IntPtr intPtr = BeginUpdateResource(exepath, bDeleteExistingResources: false);
			if (intPtr == IntPtr.Zero)
			{
				throw new Win32Exception(Marshal.GetLastWin32Error());
			}
			IntPtr intPtr2 = Marshal.AllocHGlobal(array3.Length);
			Marshal.Copy(array3, 0, intPtr2, array3.Length);
			if (!UpdateResource(intPtr, "Icon Group", "32512", 0, intPtr2, num2))
			{
				throw new Win32Exception(Marshal.GetLastWin32Error());
			}
			Marshal.FreeHGlobal(intPtr2);
			for (uint num3 = 1u; num3 < 9; num3++)
			{
				if (!UpdateResource(intPtr, 3u, num3, 0, array3, num2))
				{
					throw new Win32Exception(Marshal.GetLastWin32Error());
				}
			}
			if (!EndUpdateResource(intPtr, fDiscard: false))
			{
				throw new Win32Exception(Marshal.GetLastWin32Error());
			}
		}
		catch
		{
			new FormMessageBox("ICON error").Show(null);
		}
	}

	public static T ToStruct<T>(byte[] bytes, int index) where T : struct
	{
		int num = Marshal.SizeOf(typeof(T));
		IntPtr intPtr = Marshal.AllocHGlobal(num);
		try
		{
			Marshal.Copy(bytes, index, intPtr, num);
			return (T)Marshal.PtrToStructure(intPtr, typeof(T));
		}
		finally
		{
			Marshal.FreeHGlobal(intPtr);
		}
	}

	public static void ReplaceExeIcon(string exePath, string iconPath, uint iconStartId = 2u, uint iconCount = 8u)
	{
		byte[] array = File.ReadAllBytes(iconPath);
		IntPtr hUpdate = BeginUpdateResource(exePath, bDeleteExistingResources: false);
		uint num = 3u;
		bool flag = UpdateResource(hUpdate, 14u, 32512u, 0, array, (uint)array.Length);
		EndUpdateResource(hUpdate, fDiscard: false);
	}
}
