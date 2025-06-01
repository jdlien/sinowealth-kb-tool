using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using WindControls;

namespace USBUpdateTool;

public class ImageControl : UserControl
{
	private bool m_isSelected = false;

	private Image m_ForeImage = null;

	private string m_BackText = "";

	private IContainer components = null;

	[Category("自定义")]
	[Description("自定义：是否被选中")]
	public bool isSelected
	{
		get
		{
			return m_isSelected;
		}
		set
		{
			m_isSelected = value;
			((Control)this).Invalidate();
		}
	}

	[Category("自定义")]
	[Description("自定义：正常状态时的前景色")]
	public Image ForeImage
	{
		get
		{
			return m_ForeImage;
		}
		set
		{
			m_ForeImage = value;
			((Control)this).Invalidate();
		}
	}

	[Category("自定义")]
	[Description("自定义：背景文本")]
	public string BackText
	{
		get
		{
			return m_BackText;
		}
		set
		{
			m_BackText = value;
			((Control)this).Invalidate();
		}
	}

	public ImageControl()
	{
		InitializeComponent();
	}

	private void ImageControl_Paint(object sender, PaintEventArgs e)
	{
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Expected O, but got Unknown
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Expected O, but got Unknown
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Expected O, but got Unknown
		if (ForeImage != null)
		{
			e.Graphics.SetGDIHigh();
			e.Graphics.FillRectangle((Brush)new SolidBrush(Color.Transparent), new RectangleF(0f, 0f, ((Control)this).Width, ((Control)this).Height));
			e.Graphics.DrawImage(ForeImage, 0, 0, ForeImage.Width, ForeImage.Height);
			if (isSelected)
			{
				Pen val = new Pen(Color.Red);
				val.DashStyle = (DashStyle)1;
				val.DashPattern = new float[2] { 3f, 3f };
				e.Graphics.DrawRectangle(val, new Rectangle(0, 0, ((Control)this).Width - 1, ((Control)this).Height - 1));
			}
		}
		else
		{
			Pen val2 = new Pen(SystemColors.ControlDark);
			val2.DashStyle = (DashStyle)2;
			e.Graphics.DrawRectangle(val2, new Rectangle(0, 0, ((Control)this).Width - 1, ((Control)this).Height - 1));
			SizeF sizeF = e.Graphics.MeasureString(BackText, ((Control)this).Font);
			PointF pointF = new PointF
			{
				X = ((float)((Control)this).Width - sizeF.Width) / 2f,
				Y = ((float)((Control)this).Height - sizeF.Height) / 2f
			};
			e.Graphics.DrawString(BackText, ((Control)this).Font, (Brush)new SolidBrush(((Control)this).ForeColor), pointF);
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		((Control)this).SuspendLayout();
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 12f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).DoubleBuffered = true;
		((Control)this).Name = "ImageControl";
		((Control)this).Size = new Size(407, 45);
		((Control)this).Paint += new PaintEventHandler(ImageControl_Paint);
		((Control)this).ResumeLayout(false);
	}
}
