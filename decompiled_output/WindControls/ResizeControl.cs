using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace WindControls;

public class ResizeControl
{
	private const int HORZRES = 8;

	private const int VERTRES = 10;

	private const int LOGPIXELSX = 88;

	private const int LOGPIXELSY = 90;

	private const int DESKTOPVERTRES = 117;

	private const int DESKTOPHORZRES = 118;

	public const int DefaultDPI = 96;

	public static int ScreenDPI;

	[DllImport("user32.dll")]
	private static extern IntPtr GetDC(IntPtr ptr);

	[DllImport("gdi32.dll")]
	private static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

	[DllImport("user32.dll")]
	private static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDc);

	public void Resize(Form form)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		GetDpi();
		if (ScreenDPI > 96)
		{
			int scaleValue = GetScaleValue(((Control)form).BackgroundImage.Width);
			int scaleValue2 = GetScaleValue(((Control)form).BackgroundImage.Height);
			Bitmap bitmap = new Bitmap(((Control)form).BackgroundImage);
			((Control)form).BackgroundImage = (Image)(object)bitmap.ScaleToSize(scaleValue, scaleValue2);
			Resize((Control)(object)form, ScreenDPI);
		}
	}

	private static void Resize(Control father, int scale)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		foreach (Control item in (ArrangedElementCollection)father.Controls)
		{
			Control val = item;
			val.Width = GetScaleValue(val.Width);
			val.Height = GetScaleValue(val.Height);
			val.Location = ToScalePoint(val.Location);
			if (!(val is WindProgressBar) && val.HasChildren)
			{
				Resize(val, scale);
			}
		}
	}

	public static int GetScaleValue(int value)
	{
		if (ScreenDPI == 0)
		{
			GetDpi();
		}
		if (ScreenDPI > 96)
		{
			return value * ScreenDPI / 96;
		}
		return value;
	}

	public static Rectangle ToScaleRect(Rectangle rectangle)
	{
		int scaleValue = GetScaleValue(rectangle.X);
		int scaleValue2 = GetScaleValue(rectangle.Y);
		int scaleValue3 = GetScaleValue(rectangle.Width);
		int scaleValue4 = GetScaleValue(rectangle.Height);
		return new Rectangle(scaleValue, scaleValue2, scaleValue3, scaleValue4);
	}

	public static Point ToScalePoint(Point point)
	{
		int scaleValue = GetScaleValue(point.X);
		int scaleValue2 = GetScaleValue(point.Y);
		return new Point(scaleValue, scaleValue2);
	}

	public static Image GetScaleImage(Image image)
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		if (ScreenDPI == 0)
		{
			GetDpi();
		}
		if (ScreenDPI > 96 && image != null)
		{
			int width = image.Width * ScreenDPI / 96;
			int height = image.Height * ScreenDPI / 96;
			return (Image)(object)GraphicsHelper.ScaleToSize(new Bitmap(image), width, height);
		}
		return image;
	}

	public static int GetReverseScaleValue(int value)
	{
		if (ScreenDPI > 96)
		{
			return value * 96 / ScreenDPI;
		}
		return value;
	}

	public static Rectangle ToReverseScaleRect(Rectangle rectangle)
	{
		int reverseScaleValue = GetReverseScaleValue(rectangle.X);
		int reverseScaleValue2 = GetReverseScaleValue(rectangle.Y);
		int reverseScaleValue3 = GetReverseScaleValue(rectangle.Width);
		int reverseScaleValue4 = GetReverseScaleValue(rectangle.Height);
		return new Rectangle(reverseScaleValue, reverseScaleValue2, reverseScaleValue3, reverseScaleValue4);
	}

	public static Point ToReverseScalePoint(Point point)
	{
		int reverseScaleValue = GetReverseScaleValue(point.X);
		int reverseScaleValue2 = GetReverseScaleValue(point.Y);
		return new Point(reverseScaleValue, reverseScaleValue2);
	}

	public static void GetDpi()
	{
		IntPtr dC = GetDC(IntPtr.Zero);
		ScreenDPI = GetDeviceCaps(dC, 88);
		ScreenDPI = GetDeviceCaps(dC, 90);
		ReleaseDC(IntPtr.Zero, dC);
	}
}
