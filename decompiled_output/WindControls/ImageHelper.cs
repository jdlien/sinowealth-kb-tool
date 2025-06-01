using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace WindControls;

internal class ImageHelper
{
	public static bool IsRealImage(string path)
	{
		if (path == null || path == "")
		{
			return false;
		}
		try
		{
			Image val = Image.FromFile(path);
			val.Dispose();
			return true;
		}
		catch
		{
			return false;
		}
	}

	public static Image GetImage(string fileName)
	{
		Image result = null;
		if (File.Exists(fileName) && IsRealImage(fileName))
		{
			FileStream fileStream = new FileStream(fileName, FileMode.Open);
			result = Image.FromStream((Stream)fileStream);
			fileStream.Close();
		}
		return result;
	}

	public static byte[] GetBytesFromImage(Image image)
	{
		MemoryStream memoryStream = new MemoryStream();
		if (image == null)
		{
			return new byte[memoryStream.Length];
		}
		image.Save((Stream)memoryStream, ImageFormat.Png);
		byte[] array = new byte[memoryStream.Length];
		return memoryStream.GetBuffer();
	}

	public static Icon IconFromByte(byte[] buffer)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return Icon.FromHandle(new Bitmap((Stream)new MemoryStream(buffer)).GetHicon());
	}

	public static byte[] IconToByte(Icon icon)
	{
		Version frameworkVersion = RegistryStorage.GetFrameworkVersion();
		using MemoryStream memoryStream = new MemoryStream();
		icon.Save((Stream)memoryStream);
		return memoryStream.ToArray();
	}

	private static ImageCodecInfo GetEncoderInfo(string mimeType)
	{
		ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
		for (int i = 0; i < imageEncoders.Length; i++)
		{
			if (imageEncoders[i].MimeType == mimeType)
			{
				return imageEncoders[i];
			}
		}
		return null;
	}

	public static Image ByteToImage(byte[] btImage)
	{
		if (btImage.Length == 0)
		{
			return null;
		}
		MemoryStream memoryStream = new MemoryStream(btImage);
		return Image.FromStream((Stream)memoryStream);
	}

	public static Image IconToImage(Icon icon)
	{
		Version frameworkVersion = RegistryStorage.GetFrameworkVersion();
		if (frameworkVersion.Major >= 4 && frameworkVersion.Minor >= 6)
		{
			return (Image)(object)icon.ToBitmap();
		}
		MemoryStream memoryStream = new MemoryStream();
		icon.Save((Stream)memoryStream);
		return Image.FromStream((Stream)memoryStream);
	}

	public static byte[] GetBytesFromImagePath(string strFile)
	{
		byte[] result = null;
		if (IsRealImage(strFile))
		{
			using FileStream fileStream = new FileStream(strFile, FileMode.Open, FileAccess.Read);
			using BinaryReader binaryReader = new BinaryReader(fileStream);
			result = binaryReader.ReadBytes((int)fileStream.Length);
		}
		return result;
	}

	public static Image GetImageFromBytes(byte[] bytes)
	{
		Image result = null;
		using (MemoryStream memoryStream = new MemoryStream(bytes))
		{
			memoryStream.Write(bytes, 0, bytes.Length);
			result = Image.FromStream((Stream)memoryStream, true);
		}
		return result;
	}

	public static Bitmap ReadImageFile(string fileName)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		Bitmap result = null;
		if (File.Exists(fileName) && IsRealImage(fileName))
		{
			try
			{
				FileStream fileStream = File.OpenRead(fileName);
				int num = 0;
				num = (int)fileStream.Length;
				byte[] buffer = new byte[num];
				fileStream.Read(buffer, 0, num);
				Image val = Image.FromStream((Stream)fileStream);
				fileStream.Close();
				result = new Bitmap(val);
			}
			catch
			{
			}
		}
		return result;
	}

	public static Bitmap GetImageWithRect(string fileName, Rectangle rect)
	{
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Expected O, but got Unknown
		Bitmap result = null;
		try
		{
			Bitmap val = ReadImageFile(fileName);
			if (val != null)
			{
				int num = 0;
				int num2 = 0;
				int width = ((Image)val).Width;
				int height = ((Image)val).Height;
				int width2 = rect.Size.Width;
				int height2 = rect.Size.Height;
				if (height > height2 || width > width2)
				{
					if (width * height2 > height * width2)
					{
						num = width2;
						num2 = width2 * height / width;
					}
					else
					{
						num2 = height2;
						num = width * height2 / height;
					}
				}
				else
				{
					num = width;
					num2 = height;
				}
				result = new Bitmap((Image)(object)val, new Size(num, num2));
				((Image)val).Dispose();
			}
		}
		catch
		{
		}
		return result;
	}

	public static Size GetImageSize(string ImageFileName)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Expected O, but got Unknown
		Bitmap val = new Bitmap(ImageFileName);
		try
		{
			return ((Image)val).Size;
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public static Icon GetIconFromFile(string fileName)
	{
		if (!File.Exists(fileName))
		{
			return null;
		}
		return Icon.ExtractAssociatedIcon(fileName);
	}

	public static Icon GetIcon(string ImageFileName, Size size)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Expected O, but got Unknown
		if (File.Exists(ImageFileName))
		{
			Bitmap val = new Bitmap(ImageFileName);
			try
			{
				string text = Path.GetExtension(ImageFileName).ToLower();
				if (((Image)val).Width == size.Width && ((Image)val).Height == size.Height && text.Equals(".ico"))
				{
					Bitmap val2 = new Bitmap((Image)(object)val);
					try
					{
						return Icon.FromHandle(val2.GetHicon());
					}
					finally
					{
						((IDisposable)val2)?.Dispose();
					}
				}
				Bitmap val3 = new Bitmap((Image)(object)val, size);
				try
				{
					return Icon.FromHandle(val3.GetHicon());
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		return null;
	}

	public static Icon GetIcon(Image image)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Expected O, but got Unknown
		Bitmap val = new Bitmap(image);
		try
		{
			return Icon.FromHandle(val.GetHicon());
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public static byte[] GetIconBytes(string ImageFileName, Size size)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Expected O, but got Unknown
		if (File.Exists(ImageFileName))
		{
			Bitmap val = new Bitmap(ImageFileName);
			try
			{
				string text = Path.GetExtension(ImageFileName).ToLower();
				if (((Image)val).Width == 32 && ((Image)val).Height == 32 && text.Equals(".ico"))
				{
					using (MemoryStream memoryStream = new MemoryStream())
					{
						((Image)val).Save((Stream)memoryStream, ImageFormat.Bmp);
						return memoryStream.GetBuffer();
					}
				}
				Bitmap val2 = new Bitmap((Image)(object)val, size);
				try
				{
					using MemoryStream memoryStream2 = new MemoryStream();
					((Image)val2).Save((Stream)memoryStream2, ImageFormat.Bmp);
					return memoryStream2.GetBuffer();
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		return null;
	}

	public static Bitmap ImageTailor(Bitmap src, Rectangle rect)
	{
		if (rect.X + (uint)rect.Width > ((Image)src).Width || (uint)rect.Width > ((Image)src).Width)
		{
			rect.Width = ((Image)src).Width - rect.X;
		}
		if (rect.Y + (uint)rect.Height > ((Image)src).Height || (uint)rect.Height > ((Image)src).Height)
		{
			rect.Height = ((Image)src).Height - rect.Y;
		}
		return src.Clone(rect, (PixelFormat)0);
	}

	public static Image GetImageFromResource(string fileName)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		Stream resourceStream = FileHelper.GetResourceStream(fileName);
		if (resourceStream != null && resourceStream.Length > 0)
		{
			byte[] buffer = new byte[resourceStream.Length];
			resourceStream.Read(buffer, 0, (int)resourceStream.Length);
			return (Image)new Bitmap(resourceStream);
		}
		return null;
	}

	public unsafe static GraphicsPath SubGraphicsPath(Image img)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		if (img == null)
		{
			return null;
		}
		GraphicsPath val = new GraphicsPath((FillMode)0);
		Bitmap val2 = new Bitmap(img);
		int width = ((Image)val2).Width;
		int height = ((Image)val2).Height;
		BitmapData val3 = val2.LockBits(new Rectangle(0, 0, width, height), (ImageLockMode)3, (PixelFormat)137224);
		byte* ptr = (byte*)(void*)val3.Scan0;
		int num = val3.Stride - width * 3;
		int num2 = *ptr;
		int num3 = ptr[1];
		int num4 = ptr[2];
		int num5 = -1;
		for (int i = 0; i < height; i++)
		{
			for (int j = 0; j < width; j++)
			{
				if (num5 == -1 && (*ptr != num2 || ptr[1] != num3 || ptr[2] != num4))
				{
					num5 = j;
				}
				else if (num5 > -1 && *ptr == num2 && ptr[1] == num3 && ptr[2] == num4)
				{
					val.AddRectangle(new Rectangle(num5, i, j - num5, 1));
					num5 = -1;
				}
				if (j == width - 1 && num5 > -1)
				{
					val.AddRectangle(new Rectangle(num5, i, j - num5 + 1, 1));
					num5 = -1;
				}
				ptr += 3;
			}
			ptr += num;
		}
		val2.UnlockBits(val3);
		((Image)val2).Dispose();
		return val;
	}

	public static void ControlTrans(Control control, Image img)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		GraphicsPath val = SubGraphicsPath(img);
		if (val != null)
		{
			control.Region = new Region(val);
		}
	}

	public static MemoryStream ConvertStreamToMemoryStream(Stream stream)
	{
		MemoryStream memoryStream = new MemoryStream();
		if (stream != null)
		{
			byte[] array = new byte[stream.Length];
			stream.Read(array, 0, array.Length);
			if (array != null)
			{
				BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
				binaryWriter.Write(array);
			}
		}
		return memoryStream;
	}
}
