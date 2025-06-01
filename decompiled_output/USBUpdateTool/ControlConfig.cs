using System;
using System.Drawing;
using System.Windows.Forms;
using WindControls;

namespace USBUpdateTool;

public class ControlConfig
{
	private Point pointOffset = default(Point);

	public int style = 0;

	public Rectangle Rect = default(Rectangle);

	public Color textColor = Color.Black;

	public Color foreColor = Color.White;

	public Color backColor = Color.Transparent;

	public string Text = "";

	public string controlFont = "微软雅黑|18|0";

	public Font font = new Font("微软雅黑", 18f, (FontStyle)0, (GraphicsUnit)3, (byte)134);

	public bool visable = true;

	public void SetOffset(Point point)
	{
		pointOffset = point;
	}

	public void SetControl(Control control)
	{
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Expected I4, but got Unknown
		if (control.Location.X >= pointOffset.X)
		{
			Rect.X = control.Location.X - pointOffset.X;
		}
		else
		{
			Rect.X = 0;
		}
		if (control.Location.Y >= pointOffset.Y)
		{
			Rect.Y = control.Location.Y - pointOffset.Y;
		}
		else
		{
			Rect.Y = 0;
		}
		Rect.Width = control.Width;
		Rect.Height = control.Height;
		Text = control.Text;
		controlFont = control.Font.Name + "|" + (int)control.Font.Size + "|" + (int)control.Font.Style;
		visable = !control.Text.Contains(FormMakeMultiPairTool.hideKey);
	}

	public void SetColors(Color _textColor, Color _foreColor, Color _backColor)
	{
		textColor = _textColor;
		foreColor = _foreColor;
		backColor = _backColor;
	}

	public string ToConfigString()
	{
		Rectangle rectangle = ResizeControl.ToReverseScaleRect(Rect);
		string text = rectangle.X.ToString();
		text = text + AppConfig.splitChar + rectangle.Y;
		text = text + AppConfig.splitChar + rectangle.Width;
		text = text + AppConfig.splitChar + rectangle.Height;
		text = text + AppConfig.splitChar + textColor.ToArgb().ToString("X8");
		text = text + AppConfig.splitChar + foreColor.ToArgb().ToString("X8");
		text = text + AppConfig.splitChar + backColor.ToArgb().ToString("X8");
		text = text + AppConfig.splitChar + style;
		text = text + AppConfig.splitChar + Text;
		text = text + AppConfig.splitChar + controlFont;
		if (visable)
		{
			return text + AppConfig.splitChar + "1";
		}
		return text + AppConfig.splitChar + "0";
	}

	public void ToControl(string config)
	{
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Expected O, but got Unknown
		string[] array = config.Split(new char[1] { AppConfig.splitChar });
		int[] array2 = new int[4];
		for (int i = 0; i < array2.Length; i++)
		{
			if (StringHelper.IsIHexString(array[i]))
			{
				array2[i] = Convert.ToInt32(array[i]);
			}
		}
		Rect = new Rectangle(array2[0], array2[1], array2[2], array2[3]);
		array2 = new int[3];
		for (int j = 0; j < array2.Length; j++)
		{
			if (StringHelper.IsIHexString(array[j + 4]))
			{
				array2[j] = Convert.ToInt32(array[j + 4], 16);
			}
		}
		textColor = Color.FromArgb(array2[0]);
		foreColor = Color.FromArgb(array2[1]);
		backColor = Color.FromArgb(array2[2]);
		style = Convert.ToInt32(array[7]);
		Text = array[8];
		string[] array3 = array[9].Split(new char[1] { '|' });
		int num = Convert.ToInt32(array3[1]);
		int num2 = Convert.ToInt32(array3[2]);
		font = new Font(array3[0], (float)num, (FontStyle)num2);
		if (array.Length > 10)
		{
			visable = !(array[10] == "0");
		}
		else
		{
			visable = true;
		}
	}
}
