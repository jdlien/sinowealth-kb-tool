using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace WindControls;

public class BaseControl : UserControl
{
	public Color CurrentBackColor = Color.White;

	public Color CurrentFrameColor = Color.Transparent;

	public bool TextVisable = true;

	private int m_Radius = 12;

	private Color m_FrameColor = Color.DarkGray;

	private int m_FrameWidth = 1;

	private Color m_MouseUpForeColor = Color.Black;

	private Color m_MouseUpBackColor = Color.White;

	private Color m_MouseEnterForeColor = Color.Black;

	private Color m_MouseEnterBackColor = Color.DarkGray;

	private Color m_MouseDownForeColor = Color.Black;

	private Color m_MouseDownBackColor = Color.White;

	private Color m_DisableForeColor = Color.OrangeRed;

	private Color m_DisableBackColor = Color.Transparent;

	private GraphicsHelper.RoundStyle m_FrameMode = GraphicsHelper.RoundStyle.All;

	private StringHelper.TextAlignment m_TextAlignment = StringHelper.TextAlignment.Center;

	private bool m_EnableColorChange = true;

	private Color enabledColor = Color.White;

	[EditorBrowsable(EditorBrowsableState.Always)]
	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	[Bindable(true)]
	public override string Text { get; set; }

	[Category("自定义")]
	[Description("自定义：圆角度数，为0表示没有圆角")]
	public int Radius
	{
		get
		{
			return m_Radius;
		}
		set
		{
			m_Radius = value;
			((Control)this).Invalidate();
		}
	}

	[Category("自定义")]
	[Description("自定义：边框颜色")]
	public Color FrameColor
	{
		get
		{
			return m_FrameColor;
		}
		set
		{
			m_FrameColor = value;
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

	[Category("自定义")]
	[Description("自定义：正常状态时的前景色")]
	public Color MouseUpForeColor
	{
		get
		{
			return m_MouseUpForeColor;
		}
		set
		{
			m_MouseUpForeColor = value;
			((ContainerControl)this).Validate();
		}
	}

	[Category("自定义")]
	[Description("自定义：正常状态时的背景色")]
	public Color MouseUpBackColor
	{
		get
		{
			return m_MouseUpBackColor;
		}
		set
		{
			m_MouseUpBackColor = value;
			((ContainerControl)this).Validate();
		}
	}

	[Category("自定义")]
	[Description("自定义：鼠标进入时的前景色")]
	public Color MouseEnterForeColor
	{
		get
		{
			return m_MouseEnterForeColor;
		}
		set
		{
			m_MouseEnterForeColor = value;
		}
	}

	[Category("自定义")]
	[Description("自定义：鼠标进入时的背景色")]
	public Color MouseEnterBackColor
	{
		get
		{
			return m_MouseEnterBackColor;
		}
		set
		{
			m_MouseEnterBackColor = value;
		}
	}

	[Category("自定义")]
	[Description("自定义：鼠标按下时前景色")]
	public Color MouseDownForeColor
	{
		get
		{
			return m_MouseDownForeColor;
		}
		set
		{
			m_MouseDownForeColor = value;
			((Control)this).Invalidate();
		}
	}

	[Category("自定义")]
	[Description("自定义：鼠标按下时的背景色")]
	public Color MouseDownBackColor
	{
		get
		{
			return m_MouseDownBackColor;
		}
		set
		{
			m_MouseDownBackColor = value;
		}
	}

	[Category("自定义")]
	[Description("自定义：禁止时前景色")]
	public Color DisableForeColor
	{
		get
		{
			return m_DisableForeColor;
		}
		set
		{
			m_DisableForeColor = value;
		}
	}

	[Category("自定义")]
	[Description("自定义：禁止时背景色")]
	public Color DisableBackColor
	{
		get
		{
			return m_DisableBackColor;
		}
		set
		{
			m_DisableBackColor = value;
		}
	}

	[Category("自定义")]
	[Description("自定义：边框样式，四个角是否显示圆角，各自是独立的")]
	public GraphicsHelper.RoundStyle FrameMode
	{
		get
		{
			return m_FrameMode;
		}
		set
		{
			m_FrameMode = value;
			((Control)this).Invalidate();
		}
	}

	[Category("自定义")]
	[Description("自定义：文本对齐格式")]
	public StringHelper.TextAlignment TextAlignment
	{
		get
		{
			return m_TextAlignment;
		}
		set
		{
			m_TextAlignment = value;
		}
	}

	[Category("自定义")]
	[Description("自定义：鼠标进出控件时是否允许改变颜色")]
	public bool EnableColorChange
	{
		get
		{
			return m_EnableColorChange;
		}
		set
		{
			m_EnableColorChange = value;
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		((Control)this).OnPaint(e);
		if (((Control)this).Visible)
		{
			GraphicsHelper.FillRect(frameRect: new Rectangle(0, 0, ((Control)this).Width - 1, ((Control)this).Height - 1), g: e.Graphics, Radius: Radius, backColor: CurrentBackColor, frameWidth: FrameWidth, frameColor: CurrentFrameColor, roundStyle: FrameMode);
			if (TextVisable && ((Control)this).Text != null && ((Control)this).Text != "")
			{
				StringHelper.DrawText(e.Graphics, ((Control)this).ClientRectangle, ((Control)this).Text, ((Control)this).Font, ((Control)this).ForeColor, TextAlignment);
			}
		}
	}

	public BaseControl()
	{
		InitializeComponent();
		((Control)this).BackColor = Color.Transparent;
		CurrentFrameColor = FrameColor;
		enabledColor = ((Control)this).ForeColor;
		((Control)this).EnabledChanged += BaseControl_EnabledChanged;
	}

	protected override void OnCreateControl()
	{
		CurrentBackColor = MouseUpBackColor;
		CurrentFrameColor = FrameColor;
		BaseControl_EnabledChanged(null, null);
	}

	private void BaseControl_EnabledChanged(object sender, EventArgs e)
	{
		if (((Control)this).Enabled)
		{
			((Control)this).ForeColor = enabledColor;
			SetCurrentBackColor(MouseUpBackColor);
		}
		else
		{
			((Control)this).ForeColor = Color.Gray;
			SetCurrentBackColor(Color.LightGray);
		}
	}

	private void InitializeComponent()
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		((Control)this).SuspendLayout();
		((Control)this).BackColor = SystemColors.Control;
		((Control)this).Name = "BaseControl";
		((Control)this).Size = new Size(151, 44);
		((Control)this).MouseDown += new MouseEventHandler(BaseButton_MouseDown);
		((Control)this).MouseEnter += BaseButton_MouseEnter;
		((Control)this).MouseLeave += BaseButton_MouseLeave;
		((Control)this).MouseUp += new MouseEventHandler(BaseButton_MouseUp);
		((Control)this).ResumeLayout(false);
	}

	public virtual void SetCurrentBackColor(Color color)
	{
		if (EnableColorChange)
		{
			CurrentBackColor = color;
			((Control)this).Invalidate();
		}
	}

	public virtual void SetCurrentFrameColor(Color color)
	{
		CurrentFrameColor = color;
		((Control)this).Invalidate();
	}

	private void BaseButton_MouseEnter(object sender, EventArgs e)
	{
		if (((Control)this).Enabled)
		{
			SetCurrentBackColor(MouseEnterBackColor);
		}
	}

	private void BaseButton_MouseLeave(object sender, EventArgs e)
	{
		if (((Control)this).Enabled)
		{
			SetCurrentBackColor(MouseUpForeColor);
		}
	}

	private void BaseButton_MouseDown(object sender, MouseEventArgs e)
	{
		if (((Control)this).Enabled)
		{
			SetCurrentBackColor(MouseDownBackColor);
		}
	}

	private void BaseButton_MouseUp(object sender, MouseEventArgs e)
	{
		if (((Control)this).Enabled)
		{
			SetCurrentBackColor(MouseUpForeColor);
		}
	}
}
