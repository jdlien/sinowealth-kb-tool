using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WindControls;

public class SliderBar : WindProgressBar
{
	private IContainer components = null;

	[Category("自定义")]
	[Description("自定义：类型")]
	public override BAR_STYLE BarStyle => BAR_STYLE.SliderBar;

	public SliderBar()
	{
		InitializeComponent();
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Expected O, but got Unknown
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Expected O, but got Unknown
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Expected O, but got Unknown
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Expected O, but got Unknown
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Expected O, but got Unknown
		((Control)this).OnPaint(e);
		e.Graphics.SetGDIHigh();
		int num = ((Control)this).Height / 3;
		int height = ((Control)this).Height;
		int height2 = (((Control)this).Height - num) * 2 / 3;
		int x = base.Value * (((Control)this).Width - height) / base.MaxValue;
		Rectangle rectangle = new Rectangle(x, 0, height, height2);
		Rectangle rect = new Rectangle(rectangle.Width / 2, ((Control)this).Height - num, ((Control)this).Width - rectangle.Width, num);
		int num2 = num;
		GraphicsPath roundedRectPath = GraphicsHelper.GetRoundedRectPath(rect, num2, GraphicsHelper.RoundStyle.All);
		e.Graphics.FillPath((Brush)new SolidBrush(Color.FromArgb(255, BarBackColor)), roundedRectPath);
		rect.Width = base.Value * rect.Width / base.MaxValue;
		if (rect.Width < num2)
		{
			rect.Height = rect.Width;
			num2 = rect.Width;
			rect.Y += (num - rect.Height) / 2;
		}
		roundedRectPath = GraphicsHelper.GetRoundedRectPath(rect, num2, GraphicsHelper.RoundStyle.All);
		e.Graphics.FillPath((Brush)new SolidBrush(Color.FromArgb(255, BarForeColor)), roundedRectPath);
		if (base.Value > 0)
		{
			GraphicsPath roundedRectPath2 = GraphicsHelper.GetRoundedRectPath(rectangle, 8, GraphicsHelper.RoundStyle.All);
			e.Graphics.FillPath((Brush)new SolidBrush(BarForeColor), roundedRectPath2);
			Point point = new Point(rectangle.X + rectangle.Width / 3, rectangle.Bottom - 1);
			Point point2 = new Point(rectangle.Right - rectangle.Width / 3, rectangle.Bottom - 1);
			Point point3 = new Point(rectangle.X + rectangle.Width / 2, rect.Y);
			Point[] array = new Point[3] { point, point2, point3 };
			e.Graphics.FillPolygon((Brush)new SolidBrush(BarForeColor), array);
			if (base.ShowPercent)
			{
				string text = ((float)base.Value / (float)base.MaxValue).ToString("0%");
				SizeF sizeF = e.Graphics.MeasureString(text, ((Control)this).Font);
				rectangle.X += (rectangle.Width - (int)sizeF.Width) / 2;
				rectangle.Y += (rectangle.Height - (int)sizeF.Height) / 2;
				e.Graphics.DrawString(text, ((Control)this).Font, (Brush)new SolidBrush(TextColor), (RectangleF)rectangle);
			}
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
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).SuspendLayout();
		((ContainerControl)this).AutoScaleDimensions = new SizeF(10f, 21f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Font = new Font("微软雅黑", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)this).Margin = new Padding(5);
		((Control)this).Name = "SliderBar";
		((Control)this).Size = new Size(280, 39);
		((Control)this).ResumeLayout(false);
	}
}
