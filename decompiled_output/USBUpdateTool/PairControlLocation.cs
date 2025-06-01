using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using WindControls;

namespace USBUpdateTool;

public class PairControlLocation
{
	private FormMakePairTool toolForm;

	private Image backImage;

	private Point imageOffset;

	private Font font = new Font("微软雅黑", 9f, (FontStyle)0, (GraphicsUnit)3, (byte)134);

	public PairControlLocation(FormMakePairTool _form, Point _imageOffset)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		toolForm = _form;
		imageOffset = _imageOffset;
	}

	public void AddControl(Control control)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		control.MouseEnter += Control_MouseEnter;
		control.MouseDown += new MouseEventHandler(Control_MouseDown);
		control.MouseMove += new MouseEventHandler(Control_MouseMove);
		control.MouseUp += new MouseEventHandler(Control_MouseUp);
		control.MouseLeave += Control_MouseLeave;
	}

	private void Control_MouseEnter(object sender, EventArgs e)
	{
	}

	private void Control_MouseDown(object sender, MouseEventArgs e)
	{
		((Control)toolForm).Cursor = Cursors.Hand;
	}

	private void Control_MouseMove(object sender, MouseEventArgs e)
	{
	}

	private void Control_MouseUp(object sender, MouseEventArgs e)
	{
		((Control)toolForm).Cursor = Cursors.Default;
	}

	private void Control_MouseLeave(object sender, EventArgs e)
	{
	}

	public void SetBackImage(Image image)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		backImage = ResizeControl.GetScaleImage((Image)new Bitmap(image));
	}

	public Image DrawImage(Image image, Point point, Control control)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Expected O, but got Unknown
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected O, but got Unknown
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Expected O, but got Unknown
		Image val = (Image)new Bitmap(image);
		Graphics val2 = Graphics.FromImage(val);
		int num = 1;
		float[] dashPattern = new float[2] { 4f, 4f };
		Pen val3 = new Pen(Color.Blue, (float)num);
		val3.DashPattern = dashPattern;
		AdjustableArrowCap customEndCap = (AdjustableArrowCap)(object)(val3.CustomStartCap = (CustomLineCap)new AdjustableArrowCap(4f, 4f, true));
		val3.CustomEndCap = (CustomLineCap)(object)customEndCap;
		val2.DrawLine(val3, new Point(0, control.Location.Y - point.Y), new Point(image.Width, control.Location.Y - point.Y));
		val2.DrawLine(val3, new Point(control.Location.X - point.X, 0), new Point(control.Location.X - point.X, image.Height));
		int num2 = control.Location.X - point.X;
		int num3 = control.Location.Y - point.Y;
		Point point2 = new Point(num2 + 2, num3 - 16);
		string text = "(" + num2 + "," + num3 + ")";
		val2.DrawString(text, font, (Brush)new SolidBrush(Color.Blue), (PointF)point2);
		return val;
	}
}
