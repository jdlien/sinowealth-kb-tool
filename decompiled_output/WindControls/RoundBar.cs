using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WindControls;

public class RoundBar : WindProgressBar
{
	private IContainer components = null;

	[Category("自定义")]
	[Description("自定义：类型")]
	public override BAR_STYLE BarStyle => BAR_STYLE.RoundBar;

	public RoundBar()
	{
		InitializeComponent();
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Expected O, but got Unknown
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Expected O, but got Unknown
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Expected O, but got Unknown
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Expected O, but got Unknown
		((Control)this).OnPaint(e);
		e.Graphics.SetGDIHigh();
		Rectangle rectangle = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
		int num = ((Control)this).Height;
		GraphicsPath roundedRectPath = GraphicsHelper.GetRoundedRectPath(rectangle, num, GraphicsHelper.RoundStyle.All);
		e.Graphics.FillPath((Brush)new SolidBrush(Color.FromArgb(255, BarBackColor)), roundedRectPath);
		rectangle.Width = base.Value * rectangle.Width / base.MaxValue;
		if (rectangle.Width < num)
		{
			num = rectangle.Width;
			rectangle.Height = rectangle.Width;
			rectangle.Y = (((Control)this).Height - rectangle.Height) / 2;
		}
		roundedRectPath = GraphicsHelper.GetRoundedRectPath(rectangle, num, GraphicsHelper.RoundStyle.All);
		if (rectangle.Width > 0 && rectangle.Height > 0)
		{
			LinearGradientBrush val = new LinearGradientBrush(rectangle, BarBackColor, BarForeColor, (LinearGradientMode)0);
			ColorBlend val2 = new ColorBlend();
			val2.Colors = new Color[2] { BarBackColor, BarForeColor };
			val2.Positions = new float[2] { 0f, 1f };
			val.InterpolationColors = val2;
			e.Graphics.FillPath((Brush)(object)val, roundedRectPath);
			((Brush)val).Dispose();
		}
		if (base.ShowPercent && base.Value > 0)
		{
			Rectangle rectangle2 = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
			string text = ((float)base.Value / (float)base.MaxValue).ToString("0%");
			SizeF sizeF = e.Graphics.MeasureString(text, ((Control)this).Font);
			rectangle2.X = (int)((float)(rectangle.X + rectangle.Width) - sizeF.Width - 8f);
			rectangle2.Y += (rectangle2.Height - (int)sizeF.Height) / 2;
			if (rectangle2.X < 0)
			{
				rectangle2.X = 0;
			}
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
		((ContainerControl)this).AutoScaleDimensions = new SizeF(10f, 21f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Font = new Font("微软雅黑", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)this).Margin = new Padding(5, 5, 5, 5);
		((Control)this).Name = "RoundBar";
		((Control)this).Size = new Size(292, 39);
		((Control)this).ResumeLayout(false);
	}
}
