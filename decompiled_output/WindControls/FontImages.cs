using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace WindControls;

public static class FontImages
{
	public static PrivateFontCollection m_fontawesomeCollection;

	public static PrivateFontCollection m_elegantthemesCollection;

	static FontImages()
	{
		m_fontawesomeCollection = LoadFontResource("WindControls.IconFont.fontawesomewebfont.ttf");
		m_elegantthemesCollection = LoadFontResource("WindControls.IconFont.ElegantIcons.ttf");
	}

	public static PrivateFontCollection LoadFontResource(string _fileName)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Expected O, but got Unknown
		Stream resourceStream = FileHelper.GetResourceStream(_fileName);
		PrivateFontCollection val = new PrivateFontCollection();
		if (resourceStream != null)
		{
			byte[] array = new byte[resourceStream.Length];
			resourceStream.Read(array, 0, array.Length);
			IntPtr intPtr = Marshal.AllocCoTaskMem(array.Length);
			Marshal.Copy(array, 0, intPtr, array.Length);
			val.AddMemoryFont(intPtr, array.Length);
			Marshal.FreeCoTaskMem(intPtr);
		}
		return val;
	}

	public static Icon GetIcon(string iconText, int imageSize, Color foreColor, Color backColor)
	{
		Bitmap image = GetImage(iconText, imageSize, foreColor, backColor);
		return (image != null) ? ToIcon(image, imageSize) : null;
	}

	public static Bitmap GetImage(string iconText, int imageSize, Color foreColor, Color backColor)
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Expected O, but got Unknown
		if (((FontCollection)m_fontawesomeCollection).Families.Length == 0 || imageSize == 0 || iconText == null || iconText == "")
		{
			return null;
		}
		FontIcons utf = (FontIcons)Enum.Parse(typeof(FontIcons), iconText);
		Bitmap val = new Bitmap(imageSize, imageSize);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		try
		{
			string text = char.ConvertFromUtf32((int)utf);
			val2.Clear(backColor);
			val2.SetGDIHigh();
			Brush val3 = (Brush)new SolidBrush(foreColor);
			try
			{
				FontFamily val4 = (iconText.StartsWith("A_") ? ((FontCollection)m_fontawesomeCollection).Families[0] : ((FontCollection)m_elegantthemesCollection).Families[0]);
				Font val5;
				SizeF sizeF;
				while (true)
				{
					val5 = new Font(val4, (float)imageSize, (FontStyle)0, (GraphicsUnit)2);
					sizeF = val2.MeasureString(text, val5);
					if ((((Image)val).Width < (int)(sizeF.Width + 0.5f) || ((Image)val).Height < (int)(sizeF.Height + 0.5f)) && imageSize > 4)
					{
						imageSize--;
						continue;
					}
					break;
				}
				float num = ((float)((Image)val).Width - sizeF.Width) / 2f;
				float num2 = ((float)((Image)val).Height - sizeF.Height) / 2f;
				val2.DrawString(text, val5, val3, num, num2);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		return val;
	}

	private static Icon ToIcon(Bitmap srcBitmap, int size)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Expected O, but got Unknown
		if (srcBitmap == null)
		{
			throw new ArgumentNullException("srcBitmap");
		}
		Icon result;
		using (MemoryStream memoryStream = new MemoryStream())
		{
			((Image)new Bitmap((Image)(object)srcBitmap, new Size(size, size))).Save((Stream)memoryStream, ImageFormat.Png);
			Stream stream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(stream);
			if (stream.Length <= 0)
			{
				return null;
			}
			binaryWriter.Write((byte)0);
			binaryWriter.Write((byte)0);
			binaryWriter.Write((short)1);
			binaryWriter.Write((short)1);
			binaryWriter.Write((byte)size);
			binaryWriter.Write((byte)size);
			binaryWriter.Write((byte)0);
			binaryWriter.Write((byte)0);
			binaryWriter.Write((short)0);
			binaryWriter.Write((short)32);
			binaryWriter.Write((int)memoryStream.Length);
			binaryWriter.Write(22);
			binaryWriter.Write(memoryStream.ToArray());
			binaryWriter.Flush();
			binaryWriter.Seek(0, SeekOrigin.Begin);
			result = new Icon(stream);
			stream.Dispose();
		}
		return result;
	}
}
