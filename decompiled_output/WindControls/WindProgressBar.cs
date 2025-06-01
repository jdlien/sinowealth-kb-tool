using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace WindControls;

public class WindProgressBar : UserControl
{
	private int m_MaxValue = 100;

	private int m_Value = 50;

	private Point m_TextLocation = default(Point);

	private int m_FrameWidth = 2;

	private Color m_TextColor = Color.Black;

	private Color m_BarForeColor = Color.RoyalBlue;

	private Color m_BarBackColor = Color.DarkGray;

	private bool m_BarSliderMovable = true;

	private bool m_ShowPercent = true;

	private Rectangle m_BarSliderImageRect = default(Rectangle);

	private Image m_BarSliderImage = null;

	private Rectangle m_BarForeImageRect = default(Rectangle);

	private Image m_BarForeImage = null;

	private Rectangle m_BarBackImageRect = default(Rectangle);

	private Image m_BarBackImage = null;

	private IContainer components = null;

	[Category("自定义")]
	[Description("自定义：类型")]
	public virtual BAR_STYLE BarStyle => BAR_STYLE.HaloBar;

	[Category("自定义")]
	[Description("自定义：最大值")]
	public int MaxValue
	{
		get
		{
			return m_MaxValue;
		}
		set
		{
			m_MaxValue = value;
		}
	}

	[Category("自定义")]
	[Description("自定义：当前值")]
	public int Value
	{
		get
		{
			return m_Value;
		}
		set
		{
			if (m_Value != value)
			{
				if (value > MaxValue)
				{
					m_Value = MaxValue;
				}
				else
				{
					m_Value = value;
				}
				((Control)this).Invalidate();
			}
		}
	}

	[Category("自定义")]
	[Description("自定义：文本偏移")]
	public Point TextLocation
	{
		get
		{
			return m_TextLocation;
		}
		set
		{
			m_TextLocation = value;
			((Control)this).Invalidate();
		}
	}

	[Category("自定义")]
	[Description("自定义：边框宽度")]
	public int FrameWidth
	{
		get
		{
			return m_FrameWidth;
		}
		set
		{
			m_FrameWidth = value;
		}
	}

	public virtual Color TextColor
	{
		get
		{
			return m_TextColor;
		}
		set
		{
			m_TextColor = value;
			((Control)this).Invalidate();
		}
	}

	public virtual Color BarForeColor
	{
		get
		{
			return m_BarForeColor;
		}
		set
		{
			m_BarForeColor = value;
		}
	}

	public virtual Color BarBackColor
	{
		get
		{
			return m_BarBackColor;
		}
		set
		{
			m_BarBackColor = value;
		}
	}

	[Category("自定义")]
	[Description("自定义：滑条是否移动")]
	public bool BarSliderMovable
	{
		get
		{
			return m_BarSliderMovable;
		}
		set
		{
			m_BarSliderMovable = value;
		}
	}

	[Category("自定义")]
	[Description("自定义：是否显示百分比")]
	public bool ShowPercent
	{
		get
		{
			return m_ShowPercent;
		}
		set
		{
			m_ShowPercent = value;
		}
	}

	[Category("自定义")]
	[Description("自定义：滑条起始区域")]
	public Rectangle BarSliderImageRect
	{
		get
		{
			return m_BarSliderImageRect;
		}
		set
		{
			m_BarSliderImageRect = value;
		}
	}

	[Category("自定义")]
	[Description("自定义：滑条图片")]
	public virtual Image BarSliderImage
	{
		get
		{
			return m_BarSliderImage;
		}
		set
		{
			m_BarSliderImage = value;
		}
	}

	[Category("自定义")]
	[Description("自定义：前景图片区域")]
	public Rectangle BarForeImageRect
	{
		get
		{
			return m_BarForeImageRect;
		}
		set
		{
			m_BarForeImageRect = value;
		}
	}

	[Category("自定义")]
	[Description("自定义：前景图片")]
	public virtual Image BarForeImage
	{
		get
		{
			return m_BarForeImage;
		}
		set
		{
			m_BarForeImage = value;
			((Control)this).Invalidate();
		}
	}

	[Category("自定义")]
	[Description("自定义：背景图片区域")]
	public Rectangle BarBackImageRect
	{
		get
		{
			return m_BarBackImageRect;
		}
		set
		{
			m_BarBackImageRect = value;
		}
	}

	[Category("自定义")]
	[Description("自定义：背景图片")]
	public virtual Image BarBackImage
	{
		get
		{
			return m_BarBackImage;
		}
		set
		{
			m_BarBackImage = value;
			((Control)this).Invalidate();
		}
	}

	public WindProgressBar()
	{
		InitializeComponent();
		SetStyles();
	}

	private void SetStyles()
	{
		((Control)this).SetStyle((ControlStyles)204818, true);
		((Control)this).UpdateStyles();
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)0;
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
		((Control)this).SuspendLayout();
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 12f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Name = "ProgressBar";
		((Control)this).Size = new Size(227, 35);
		((Control)this).ResumeLayout(false);
	}
}
