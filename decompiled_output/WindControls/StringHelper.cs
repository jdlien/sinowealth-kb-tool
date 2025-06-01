using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace WindControls;

public static class StringHelper
{
	public enum TextAlignment
	{
		Left = 1,
		Right = 2,
		Center = 4
	}

	private static IList<char> HexSet = new List<char>
	{
		'0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
		'A', 'B', 'C', 'D', 'E', 'F', 'a', 'b', 'c', 'd',
		'e', 'f'
	};

	public static int GetSpaceCountInEnd(string str)
	{
		int num = 0;
		if (str != null && str.Length > 0)
		{
			char[] array = str.ToCharArray();
			for (int i = 0; i < array.Length && array[array.Length - i - 1] == ' '; i++)
			{
				num++;
			}
		}
		return num;
	}

	public static void DrawText(Graphics g, Rectangle rect, string text, Font font, Color color, TextAlignment textAligment)
	{
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Expected O, but got Unknown
		Point point = default(Point);
		string[] array = text.Replace("\r", "").Replace("\\r", "").Replace("\\n", "\n")
			.Split(new char[1] { '\n' });
		SizeF sizeF = g.MeasureString(" ", font);
		SizeF sizeF2 = g.MeasureString(array[0], font);
		int num = (int)sizeF2.Height;
		point.Y = (rect.Height - (int)sizeF2.Height * array.Length) / 2;
		if (point.Y < 0 || point.Y > rect.Height)
		{
			point.Y = rect.Y;
		}
		else
		{
			point.Y += rect.Y;
		}
		for (int i = 0; i < array.Length; i++)
		{
			sizeF2 = g.MeasureString(array[i], font);
			sizeF2.Width += (float)GetSpaceCountInEnd(array[i]) * sizeF.Width;
			if ((textAligment & TextAlignment.Left) == TextAlignment.Left)
			{
				point.X = rect.X;
			}
			else if ((textAligment & TextAlignment.Right) == TextAlignment.Right)
			{
				point.X = rect.X + (rect.Width - (int)sizeF2.Width);
			}
			else
			{
				point.X = rect.X + (rect.Width - (int)sizeF2.Width) / 2;
			}
			g.DrawString(array[i], font, (Brush)new SolidBrush(color), (PointF)point);
			point.Y += num;
		}
	}

	internal static string ByteToHexString(byte[] byteArrary)
	{
		string text = string.Empty;
		if (byteArrary != null)
		{
			for (int i = 0; i < byteArrary.Length; i++)
			{
				text = text + byteArrary[i].ToString("X2") + " ";
			}
		}
		return text;
	}

	internal static string ByteToASCII(byte[] byteArrary)
	{
		return Encoding.ASCII.GetString(byteArrary);
	}

	internal static byte[] strToHexByte(string hexString)
	{
		hexString = hexString.Replace(" ", "");
		hexString = hexString.Replace("\r", "");
		hexString = hexString.Replace("\n", "");
		if (hexString.Length % 2 != 0)
		{
			hexString = "0" + hexString;
		}
		byte[] array = new byte[hexString.Length / 2];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
		}
		return array;
	}

	public static bool IsIHexString(string hex)
	{
		if (hex == null || hex == "")
		{
			return false;
		}
		foreach (char value in hex)
		{
			if (!Enumerable.Contains(HexSet, value))
			{
				return false;
			}
		}
		return true;
	}

	internal static SizeF GetStringSize(Graphics g, Font font, string str)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		g.PageUnit = (GraphicsUnit)2;
		g.SmoothingMode = (SmoothingMode)2;
		StringFormat val = new StringFormat();
		val.FormatFlags = (StringFormatFlags)2048;
		return g.MeasureString(str, font);
	}

	public static string CutString(string source, string startChar, string endChar)
	{
		int num = ((!(startChar == "")) ? source.IndexOf(startChar) : 0);
		int num2;
		if (endChar == "")
		{
			num2 = source.Length;
		}
		else
		{
			num2 = source.IndexOf(endChar);
			if (num2 < 0)
			{
				num2 = source.Length;
			}
		}
		string result = "";
		if (num >= 0 && num2 >= 0)
		{
			num += startChar.Length;
			result = source.Substring(num, num2 - num);
		}
		return result;
	}
}
