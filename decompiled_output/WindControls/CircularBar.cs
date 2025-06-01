using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WindControls;

public class CircularBar : WindProgressBar
{
	private IContainer components = null;

	[Category("自定义")]
	[Description("自定义：类型")]
	public override BAR_STYLE BarStyle => BAR_STYLE.CircularBar;

	public CircularBar()
	{
		InitializeComponent();
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Expected O, but got Unknown
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Expected O, but got Unknown
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Expected O, but got Unknown
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Expected O, but got Unknown
		((Control)this).OnPaint(e);
		e.Graphics.SetGDIHigh();
		int num = 12;
		Rectangle rectangle = new Rectangle(num - 1, num - 1, ((Control)this).Width - num * 2 + 2, ((Control)this).Height - num * 2 + 2);
		LinearGradientBrush val = new LinearGradientBrush(rectangle, BarBackColor, BarForeColor, (LinearGradientMode)3);
		ColorBlend val2 = new ColorBlend();
		val2.Colors = new Color[2] { BarBackColor, BarForeColor };
		val2.Positions = new float[2] { 0f, 1f };
		val.InterpolationColors = val2;
		e.Graphics.FillEllipse((Brush)(object)val, rectangle);
		((Brush)val).Dispose();
		Pen val3 = new Pen(BarForeColor, (float)num);
		int num2 = base.Value * 360 / base.MaxValue;
		rectangle = new Rectangle(num / 2, num / 2, ((Control)this).Width - num, ((Control)this).Height - num);
		if (num2 > 0)
		{
			LinearGradientBrush val4 = new LinearGradientBrush(rectangle, BarBackColor, BarForeColor, (LinearGradientMode)2);
			ColorBlend val5 = new ColorBlend();
			val5.Colors = new Color[2] { BarBackColor, BarForeColor };
			val5.Positions = new float[2] { 0f, 1f };
			val4.InterpolationColors = val5;
			val3.Brush = (Brush)(object)val4;
			e.Graphics.DrawArc(val3, rectangle, 270f, (float)num2);
			((Brush)val4).Dispose();
		}
		if (base.ShowPercent && base.Value > 0)
		{
			Rectangle rectangle2 = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
			string text = ((float)base.Value / (float)base.MaxValue).ToString("0%");
			SizeF sizeF = e.Graphics.MeasureString(text, ((Control)this).Font);
			rectangle2.X += (rectangle2.Width - (int)sizeF.Width) / 2;
			rectangle2.Y += (rectangle2.Height - (int)sizeF.Height) / 2;
			e.Graphics.DrawString(text, ((Control)this).Font, (Brush)new SolidBrush(TextColor), (RectangleF)rectangle2);
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).SuspendLayout();
		((ContainerControl)this).AutoScaleDimensions = new SizeF(12f, 27f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Font = new Font("微软雅黑", 15f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)this).Margin = new Padding(6, 7, 6, 7);
		((Control)this).Name = "CircularBar";
		((Control)this).Size = new Size(126, 103);
		((Control)this).ResumeLayout(false);
	}
}
