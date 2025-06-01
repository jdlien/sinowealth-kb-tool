using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace WindControls;

public class IconChanger
{
	public class Icons : List<Icon>
	{
		public byte[] ToGroupData(int startindex = 1)
		{
			using MemoryStream memoryStream = new MemoryStream();
			using BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
			int num = 0;
			binaryWriter.Write((ushort)0);
			binaryWriter.Write((ushort)1);
			binaryWriter.Write((ushort)base.Count);
			using (Enumerator enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Icon current = enumerator.Current;
					binaryWriter.Write(current.Width);
					binaryWriter.Write(current.Height);
					binaryWriter.Write(current.Colors);
					binaryWriter.Write((byte)0);
					binaryWriter.Write(current.ColorPlanes);
					binaryWriter.Write(current.BitsPerPixel);
					binaryWriter.Write(current.Size);
					binaryWriter.Write((ushort)(startindex + num));
					num++;
				}
			}
			memoryStream.Position = 0L;
			return memoryStream.ToArray();
		}
	}

	public class Icon
	{
		public byte Width { get; set; }

		public byte Height { get; set; }

		public byte Colors { get; set; }

		public uint Size { get; set; }

		public uint Offset { get; set; }

		public ushort ColorPlanes { get; set; }

		public ushort BitsPerPixel { get; set; }

		public byte[] Data { get; set; }
	}

	public class IconReader
	{
		public Icons Icons = new Icons();

		public IconReader(Stream input)
		{
			try
			{
				using BinaryReader binaryReader = new BinaryReader(input);
				binaryReader.ReadUInt16();
				ushort num = binaryReader.ReadUInt16();
				if (num != 1)
				{
					throw new Exception("Invalid type. The stream is not an icon file");
				}
				ushort num2 = binaryReader.ReadUInt16();
				for (int i = 0; i < num2; i++)
				{
					byte width = binaryReader.ReadByte();
					byte height = binaryReader.ReadByte();
					byte colors = binaryReader.ReadByte();
					binaryReader.ReadByte();
					ushort colorPlanes = binaryReader.ReadUInt16();
					ushort bitsPerPixel = binaryReader.ReadUInt16();
					uint size = binaryReader.ReadUInt32();
					uint offset = binaryReader.ReadUInt32();
					Icons.Add(new Icon
					{
						Colors = colors,
						Height = height,
						Width = width,
						Offset = offset,
						Size = size,
						ColorPlanes = colorPlanes,
						BitsPerPixel = bitsPerPixel
					});
				}
				foreach (Icon icon in Icons)
				{
					if (binaryReader.BaseStream.Position < icon.Offset)
					{
						int count = (int)(icon.Offset - binaryReader.BaseStream.Position);
						binaryReader.ReadBytes(count);
					}
					byte[] data = binaryReader.ReadBytes((int)icon.Size);
					icon.Data = data;
				}
			}
			catch
			{
				new FormMessageBox("Icon error,please select new icon!").Show(null);
			}
		}
	}

	public enum ICResult
	{
		Success,
		FailBegin,
		FailUpdate,
		FailEnd
	}

	private const uint RT_ICON = 3u;

	private const uint RT_GROUP_ICON = 14u;

	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern int UpdateResource(IntPtr hUpdate, uint lpType, ushort lpName, ushort wLanguage, byte[] lpData, uint cbData);

	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern IntPtr BeginUpdateResource(string pFileName, [MarshalAs(UnmanagedType.Bool)] bool bDeleteExistingResources);

	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern bool EndUpdateResource(IntPtr hUpdate, bool fDiscard);

	public ICResult ChangeIcon(string exeFilePath, string iconFilePath)
	{
		using FileStream input = new FileStream(iconFilePath, FileMode.Open, FileAccess.Read);
		IconReader iconReader = new IconReader(input);
		IconChanger iconChanger = new IconChanger();
		return iconChanger.ChangeIcon(exeFilePath, iconReader.Icons);
	}

	public ICResult ChangeIcon(string exeFilePath, MemoryStream iconsStream)
	{
		IconReader iconReader = new IconReader(iconsStream);
		IconChanger iconChanger = new IconChanger();
		return iconChanger.ChangeIcon(exeFilePath, iconReader.Icons);
	}

	public ICResult ChangeIcon(string exeFilePath, Icons icons)
	{
		IntPtr hUpdate = BeginUpdateResource(exeFilePath, bDeleteExistingResources: false);
		bool flag = false;
		ushort num = 1;
		ushort num2 = num;
		ICResult iCResult = ICResult.Success;
		int num3 = 1;
		foreach (Icon icon in icons)
		{
			num3 = UpdateResource(hUpdate, 3u, num2, 0, icon.Data, icon.Size);
			num2++;
		}
		byte[] array = icons.ToGroupData();
		num3 = UpdateResource(hUpdate, 14u, 32512, 0, array, (uint)array.Length);
		if (num3 == 1)
		{
			if (EndUpdateResource(hUpdate, fDiscard: false))
			{
				return ICResult.Success;
			}
			return ICResult.FailEnd;
		}
		return ICResult.FailUpdate;
	}
}
