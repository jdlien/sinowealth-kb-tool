using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace WindControls;

public static class GraphicsHelper
{
	public enum RoundStyle
	{
		None = 0,
		TopLeft = 1,
		TopRight = 2,
		BottomLeft = 4,
		BottomRight = 8,
		Top = 3,
		Bottom = 12,
		All = 15
	}

	public static void SetGDIHigh(this Graphics g)
	{
		g.SmoothingMode = (SmoothingMode)4;
		g.InterpolationMode = (InterpolationMode)7;
		g.CompositingQuality = (CompositingQuality)2;
		g.TextRenderingHint = (TextRenderingHint)4;
	}

	public static Region SetWindowRegion(PaintEventArgs e, int width, int height, int radius, Color borderColor, RoundStyle roundStyle)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		Rectangle rect = new Rectangle(0, 0, width, height);
		GraphicsPath roundedRectPath = GetRoundedRectPath(rect, radius, roundStyle);
		Pen val = new Pen(borderColor, 1f);
		e.Graphics.DrawPath(val, roundedRectPath);
		return new Region(roundedRectPath);
	}

	public static Region GetWindowRegion(int width, int height, int radius, Color borderColor, RoundStyle roundStyle)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		Rectangle rect = new Rectangle(0, 0, width, height);
		GraphicsPath roundedRectPath = GetRoundedRectPath(rect, radius, roundStyle);
		return new Region(roundedRectPath);
	}

	public static GraphicsPath GetRoundedRectPath(Rectangle rect, int radius, RoundStyle roundStyle)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		Rectangle rectangle = new Rectangle(rect.Location, new Size(radius, radius));
		GraphicsPath val = new GraphicsPath();
		val.StartFigure();
		if ((roundStyle & RoundStyle.TopLeft) > RoundStyle.None && radius > 0)
		{
			val.AddArc(rectangle, 180f, 90f);
		}
		else
		{
			val.AddLine(new Point(rect.Left, rect.Top), new Point(rect.Left, rect.Top + radius));
			val.AddLine(new Point(rect.Left, rect.Top), new Point(rect.Left + radius, rect.Top));
		}
		rectangle.X = rect.Right - radius;
		if ((roundStyle & RoundStyle.TopRight) > RoundStyle.None && radius > 0)
		{
			val.AddArc(rectangle, 270f, 90f);
		}
		else
		{
			val.AddLine(new Point(rect.Right, rect.Top), new Point(rect.Right - radius, rect.Top));
			val.AddLine(new Point(rect.Right, rect.Top), new Point(rect.Right, rect.Top + radius));
		}
		rectangle.Y = rect.Bottom - radius;
		if ((roundStyle & RoundStyle.BottomRight) > RoundStyle.None && radius > 0)
		{
			val.AddArc(rectangle, 0f, 90f);
		}
		else
		{
			val.AddLine(new Point(rect.Right, rect.Bottom), new Point(rect.Right, rect.Bottom - radius));
			val.AddLine(new Point(rect.Right, rect.Bottom), new Point(rect.Right + radius, rect.Bottom));
		}
		rectangle.X = rect.Left;
		if ((roundStyle & RoundStyle.BottomLeft) > RoundStyle.None && radius > 0)
		{
			val.AddArc(rectangle, 90f, 90f);
		}
		else
		{
			val.AddLine(new Point(rect.Left, rect.Bottom), new Point(rect.Left - radius, rect.Bottom));
			val.AddLine(new Point(rect.Left, rect.Bottom), new Point(rect.Left, rect.Bottom - radius));
		}
		val.CloseFigure();
		return val;
	}

	public static void FillRect(Graphics g, Rectangle frameRect, int Radius, Color backColor, RoundStyle roundStyle)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		GraphicsPath val;
		if (Radius > 0)
		{
			val = GetRoundedRectPath(frameRect, Radius, roundStyle);
		}
		else
		{
			val = new GraphicsPath();
			val.AddRectangle(frameRect);
			val.CloseFigure();
		}
		g.FillRectangle((Brush)new SolidBrush(Color.Transparent), frameRect);
		g.FillPath((Brush)new SolidBrush(backColor), val);
	}

	public static void FillRect(Graphics g, Rectangle frameRect, int Radius, Color backColor, int frameWidth, Color frameColor, RoundStyle roundStyle)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected O, but got Unknown
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		g.SetGDIHigh();
		GraphicsPath val;
		if (Radius > 0)
		{
			val = GetRoundedRectPath(frameRect, Radius, roundStyle);
		}
		else
		{
			val = new GraphicsPath();
			val.AddRectangle(frameRect);
			val.CloseFigure();
		}
		g.FillPath((Brush)new SolidBrush(backColor), val);
		if (frameWidth > 0)
		{
			Pen val2 = new Pen(frameColor, (float)frameWidth);
			g.DrawPath(val2, val);
		}
	}

	public static int GetScreenWidth()
	{
		return Screen.PrimaryScreen.Bounds.Width;
	}

	public static int GetScreenHeight()
	{
		return Screen.PrimaryScreen.Bounds.Height;
	}

	public static GraphicsPath GetImageGraphicsPath(Bitmap image, byte alphaFilter)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		int height = ((Image)image).Height;
		int width = ((Image)image).Width;
		GraphicsPath val = new GraphicsPath();
		for (int i = 0; i < height; i++)
		{
			for (int j = 0; j < width; j++)
			{
				for (; j < width && image.GetPixel(j, i).A < alphaFilter; j++)
				{
				}
				int num = j;
				for (; j < width && image.GetPixel(j, i).A > alphaFilter; j++)
				{
				}
				int num2 = j;
				if (num2 > num)
				{
					val.AddRectangle(new Rectangle(num, i, num2 - num, 1));
				}
			}
		}
		return val;
	}

	public static Region GetImageRegion(Bitmap bitmap, Size formSize, byte alphaFilter)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		Bitmap image = bitmap.ScaleToSize(formSize);
		GraphicsPath imageGraphicsPath = GetImageGraphicsPath(image, alphaFilter);
		return new Region(imageGraphicsPath);
	}

	public static Bitmap ScaleToSize(this Bitmap bitmap, int width, int height)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		if (((Image)bitmap).Width == width && ((Image)bitmap).Height == height)
		{
			return bitmap;
		}
		Bitmap val = new Bitmap(width, height);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		try
		{
			val2.InterpolationMode = (InterpolationMode)7;
			val2.DrawImage((Image)(object)bitmap, 0, 0, width, height);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		return val;
	}

	public static Bitmap ScaleToSize(this Bitmap bitmap, Size size)
	{
		return bitmap.ScaleToSize(size.Width, size.Height);
	}

	public static Bitmap FillBitmapWithColor(Bitmap bitmap, Color srcColor, Color dstColor)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Expected O, but got Unknown
		Bitmap val = new Bitmap((Image)(object)bitmap);
		for (int i = 0; i < ((Image)bitmap).Height; i++)
		{
			for (int j = 0; j < ((Image)bitmap).Width; j++)
			{
				Color pixel = bitmap.GetPixel(j, i);
				if (pixel == srcColor)
				{
					val.SetPixel(j, i, dstColor);
				}
				else
				{
					val.SetPixel(j, i, pixel);
				}
			}
		}
		return val;
	}

	public static void FillBitmapWithColor(Bitmap bitmap, Rectangle fillRect, Color fillColor)
	{
		Color color = Color.FromArgb(0, 0, 0, 0);
		Rectangle rectangle = Rectangle.Intersect(fillRect, new Rectangle(0, 0, ((Image)bitmap).Width, ((Image)bitmap).Height));
		bool flag = false;
		for (int i = rectangle.Y; i < rectangle.Height; i++)
		{
			for (int j = rectangle.X; j < rectangle.Width; j++)
			{
				if (bitmap.GetPixel(j, i) == color)
				{
					bitmap.SetPixel(j, i, fillColor);
				}
			}
		}
	}

	public static Bitmap CreateBitmap(int width, int height, Color color, int frameWidth, Color frameColor)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		Bitmap val = new Bitmap(width, height);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		val2.SmoothingMode = (SmoothingMode)4;
		val2.Clear(color);
		if (frameWidth > 0)
		{
			Pen val3 = new Pen(frameColor, (float)frameWidth);
			val3.Alignment = (PenAlignment)1;
			val2.DrawRectangle(val3, new Rectangle(0, 0, width, height));
		}
		return val;
	}

	public static void DrawSmoothLine(Graphics graphics, Color color, int x, int y, int width, int lineWidth)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		int num = y + lineWidth / 2;
		graphics.FillEllipse((Brush)new SolidBrush(color), x, num, lineWidth, lineWidth);
		graphics.FillRectangle((Brush)new SolidBrush(color), x + lineWidth / 2, num, width - lineWidth, lineWidth);
		graphics.FillEllipse((Brush)new SolidBrush(color), x + width - lineWidth, num, lineWidth, lineWidth);
	}

	public static void DrawSmoothRect(string text, Graphics graphics, Color color, int lineWidth, int x, int y, int width, int height)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Expected O, but got Unknown
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Expected O, but got Unknown
		int num = 32;
		graphics.DrawArc(new Pen(color, (float)lineWidth), new Rectangle(x + lineWidth / 2, y + lineWidth / 2, num, num), 180f, 90f);
		graphics.FillRectangle((Brush)new SolidBrush(color), x + num / 2, y, width - num, lineWidth);
		graphics.DrawArc(new Pen(color, (float)lineWidth), new Rectangle(x + width - num - lineWidth / 2, y + lineWidth / 2, num, num), 270f, 90f);
		graphics.FillRectangle((Brush)new SolidBrush(color), x, y + num / 2, lineWidth, height - num / 2);
		graphics.FillRectangle((Brush)new SolidBrush(color), x + width - lineWidth, y + num / 2, lineWidth, height - num / 2);
	}
}
