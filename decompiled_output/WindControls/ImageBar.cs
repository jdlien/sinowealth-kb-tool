using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace WindControls;

public class ImageBar : WindProgressBar
{
	private Image m_StepImage = null;

	private int m_IntervalWidth = 4;

	private IContainer components = null;

	[Category("自定义")]
	[Description("自定义：类型")]
	public override BAR_STYLE BarStyle => BAR_STYLE.ImageBar;

	[Category("自定义")]
	[Description("自定义：进度条内小格图片")]
	public Image StepImage
	{
		get
		{
			return m_StepImage;
		}
		set
		{
			m_StepImage = value;
		}
	}

	[Category("自定义")]
	[Description("自定义：进度条内每个小格之间宽度")]
	public int IntervalWidth
	{
		get
		{
			return m_IntervalWidth;
		}
		set
		{
			m_IntervalWidth = value;
		}
	}

	public ImageBar()
	{
		InitializeComponent();
	}

	private Bitmap GetBarForeImage(Image image, int value)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		if (value >= base.MaxValue)
		{
			return new Bitmap(image);
		}
		if (value <= 0)
		{
			return null;
		}
		Bitmap val = new Bitmap(image);
		Rectangle rectangle = new Rectangle(0, 0, value * ((Image)val).Width / base.MaxValue, ((Image)val).Height);
		return val.Clone(rectangle, (PixelFormat)925707);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_03e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f9: Expected O, but got Unknown
		((Control)this).OnPaint(e);
		e.Graphics.SetGDIHigh();
		int num = IntervalWidth;
		Point point;
		Image val;
		Image val2;
		Image val3;
		Image val4;
		Rectangle rectangle;
		Rectangle rectangle2;
		Rectangle rectangle3;
		if (ResizeControl.ScreenDPI > 96)
		{
			point = ResizeControl.ToScalePoint(base.TextLocation);
			num = ResizeControl.GetScaleValue(IntervalWidth);
			val = ResizeControl.GetScaleImage(StepImage);
			val2 = ResizeControl.GetScaleImage(BarSliderImage);
			val3 = ResizeControl.GetScaleImage(BarForeImage);
			val4 = ResizeControl.GetScaleImage(BarBackImage);
			rectangle = ResizeControl.ToScaleRect(base.BarSliderImageRect);
			rectangle2 = ResizeControl.ToScaleRect(base.BarForeImageRect);
			rectangle3 = ResizeControl.ToScaleRect(base.BarBackImageRect);
		}
		else
		{
			point = base.TextLocation;
			val = StepImage;
			val2 = BarSliderImage;
			val3 = BarForeImage;
			val4 = BarBackImage;
			rectangle = base.BarSliderImageRect;
			rectangle2 = base.BarForeImageRect;
			rectangle3 = base.BarBackImageRect;
		}
		if (val != null)
		{
			if (val4 != null)
			{
				e.Graphics.DrawImage(val4, rectangle3.X, rectangle3.Y, ((Control)this).Width, ((Control)this).Height);
			}
			int num2 = ((Control)this).Width / (val.Width + num);
			int num3 = base.Value * num2 / base.MaxValue;
			int num4 = num + 2;
			int num5 = (((Control)this).Height - val.Height) / 2;
			for (int i = 0; i < num3; i++)
			{
				e.Graphics.DrawImage(val, num4, num5, val.Width, val.Height);
				num4 += num + val.Width;
			}
			return;
		}
		if (val4 != null)
		{
			e.Graphics.DrawImage(val4, rectangle3.X, rectangle3.Y, val4.Width, val4.Height);
		}
		if (val3 != null)
		{
			Bitmap barForeImage = GetBarForeImage(val3, base.Value);
			if (barForeImage != null)
			{
				e.Graphics.DrawImage((Image)(object)barForeImage, rectangle2.X, rectangle2.Y, ((Image)barForeImage).Width, ((Image)barForeImage).Height);
			}
		}
		string text = base.Value * 100 / base.MaxValue + "%";
		Point location = rectangle.Location;
		if (base.BarSliderMovable)
		{
			if (val3 != null)
			{
				location.X += val3.Width * base.Value / base.MaxValue;
			}
			else if (val4 != null)
			{
				location.X += val4.Width * base.Value / base.MaxValue;
			}
			else
			{
				location.X += ((Control)this).Width * base.Value / base.MaxValue;
			}
		}
		SizeF sizeF = e.Graphics.MeasureString(text, ((Control)this).Font);
		point.X += location.X;
		point.Y += location.Y;
		if (base.BarSliderMovable)
		{
			if (val2 != null && base.Value > 0)
			{
				e.Graphics.DrawImage(val2, location.X, location.Y, val2.Width, val2.Height);
			}
		}
		else if (val2 != null)
		{
			e.Graphics.DrawImage(val2, location.X, location.Y, val2.Width, val2.Height);
		}
		if (base.ShowPercent && base.Value > 0)
		{
			e.Graphics.DrawString(text, ((Control)this).Font, (Brush)new SolidBrush(TextColor), (PointF)point);
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
		((Control)this).SuspendLayout();
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 12f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Name = "ImageBar";
		((Control)this).Size = new Size(218, 37);
		((Control)this).ResumeLayout(false);
	}
}
