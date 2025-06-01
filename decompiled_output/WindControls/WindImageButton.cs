using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WindControls;

[DefaultEvent("Click")]
public class WindImageButton : UserControl
{
	private ButtonStatus buttonStatus = ButtonStatus.MouseUp;

	private bool MouseUpChanged = false;

	private bool MouseEnterChanged = false;

	private bool MouseDownChanged = false;

	private bool DisableChanged = false;

	private string m_text = "";

	private bool m_TextFixLocationEnable = false;

	private Point m_TextFixLocation = default(Point);

	private int m_Radius = 12;

	private GraphicsHelper.RoundStyle m_FrameMode = GraphicsHelper.RoundStyle.All;

	private StringHelper.TextAlignment m_TextAlignment = StringHelper.TextAlignment.Center;

	private string m_IconName = "A_fa_close";

	private int m_IconSize = 32;

	private Point m_TextDynOffset = new Point(0, 0);

	private Point m_IconOffset = new Point(0, 0);

	private Rectangle m_IconBackColorRect = new Rectangle(0, 0, 0, 0);

	private Color m_MouseUpForeColor = Color.Black;

	private Color m_MouseUpBackColor = Color.Transparent;

	private Image m_MouseUpImage = null;

	private Color m_MouseDownForeColor = Color.DimGray;

	private Color m_MouseDownBackColor = Color.Transparent;

	private Image m_MouseDownImage = null;

	private Color m_MouseEnterForeColor = Color.Gray;

	private Color m_MouseEnterBackColor = Color.Transparent;

	private Image m_MouseEnterImage = null;

	private Color m_DisableForeColor = Color.DarkGray;

	private Color m_DisableBackColor = Color.Transparent;

	private Image m_DisableImage = null;

	public Point textPoint = default(Point);

	[EditorBrowsable(EditorBrowsableState.Always)]
	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	[Bindable(true)]
	public override string Text
	{
		get
		{
			return m_text;
		}
		set
		{
			if (m_text != value)
			{
				m_text = value;
				((Control)this).Invalidate();
			}
		}
	}

	[Category("自定义")]
	[Description("自定义：文本是否在固定位置显示")]
	public bool TextFixLocationEnable
	{
		get
		{
			return m_TextFixLocationEnable;
		}
		set
		{
			if (m_TextFixLocationEnable != value)
			{
				m_TextFixLocationEnable = value;
				((Control)this).Invalidate();
			}
		}
	}

	[Category("自定义")]
	[Description("自定义：文本在固定位置显示")]
	public Point TextFixLocation
	{
		get
		{
			return m_TextFixLocation;
		}
		set
		{
			m_TextFixLocation = value;
			((Control)this).Invalidate();
		}
	}

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
			MouseUpChanged = true;
			MouseEnterChanged = true;
			MouseDownChanged = true;
			((Control)this).Invalidate();
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
			MouseUpChanged = true;
			MouseEnterChanged = true;
			MouseDownChanged = true;
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
			MouseUpChanged = true;
			((Control)this).Invalidate();
		}
	}

	[Category("自定义属性")]
	[Description("自定义：图标名字：在http://www.fontawesome.com.cn/faicons/中搜索")]
	public string IconName
	{
		get
		{
			return m_IconName;
		}
		set
		{
			m_IconName = value;
			MouseUpChanged = true;
			MouseEnterChanged = true;
			MouseDownChanged = true;
			((Control)this).Invalidate();
		}
	}

	[Category("自定义属性")]
	[Description("自定义：图标大小，整数类型")]
	public int IconSize
	{
		get
		{
			return m_IconSize;
		}
		set
		{
			m_IconSize = value;
			MouseUpChanged = true;
			MouseEnterChanged = true;
			MouseDownChanged = true;
			((Control)this).Invalidate();
		}
	}

	[Category("自定义属性")]
	[Description("当mouse enter事件时，文字发生偏移")]
	public Point TextDynOffset
	{
		get
		{
			return m_TextDynOffset;
		}
		set
		{
			m_TextDynOffset = value;
			((Control)this).Invalidate();
		}
	}

	[Category("自定义")]
	[Description("自定义：图像坐标偏移")]
	public Point IconOffset
	{
		get
		{
			return m_IconOffset;
		}
		set
		{
			m_IconOffset = value;
		}
	}

	[Category("自定义")]
	[Description("自定义：背景色透明部分区域")]
	public Rectangle IconBackColorRect
	{
		get
		{
			return m_IconBackColorRect;
		}
		set
		{
			if (m_IconBackColorRect != value)
			{
				m_IconBackColorRect = value;
				MouseUpChanged = true;
				MouseEnterChanged = true;
				MouseDownChanged = true;
				((Control)this).Invalidate();
			}
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
			MouseUpChanged = true;
			((Control)this).Invalidate();
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
			MouseUpChanged = true;
			((ContainerControl)this).Validate();
		}
	}

	[DefaultValue(null)]
	[Localizable(true)]
	[Category("自定义")]
	[Description("自定义：控件在正常状态下显示的图片")]
	public Image MouseUpImage
	{
		get
		{
			return m_MouseUpImage;
		}
		set
		{
			m_MouseUpImage = value;
			MouseUpChanged = true;
			((Control)this).Invalidate();
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
			MouseDownChanged = true;
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
			MouseDownChanged = true;
			((Control)this).Invalidate();
		}
	}

	[DefaultValue(null)]
	[Localizable(true)]
	[Category("自定义")]
	[Description("自定义：鼠标在控件上方按下按钮时显示的图片")]
	public Image MouseDownImage
	{
		get
		{
			return m_MouseDownImage;
		}
		set
		{
			m_MouseDownImage = value;
			MouseDownChanged = true;
			((Control)this).Invalidate();
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
			MouseEnterChanged = true;
			((Control)this).Invalidate();
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
			MouseEnterChanged = true;
			((Control)this).Invalidate();
		}
	}

	[DefaultValue(null)]
	[Localizable(true)]
	[Category("自定义")]
	[Description("自定义：鼠标指针移过控件时显示的图片")]
	public Image MouseEnterImage
	{
		get
		{
			return m_MouseEnterImage;
		}
		set
		{
			m_MouseEnterImage = value;
			MouseEnterChanged = true;
			((Control)this).Invalidate();
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
			DisableChanged = true;
			((Control)this).Invalidate();
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
			DisableChanged = true;
			((Control)this).Invalidate();
		}
	}

	[DefaultValue(null)]
	[Localizable(true)]
	[Category("自定义")]
	[Description("自定义：禁止时图片")]
	public Image DisableImage
	{
		get
		{
			return m_DisableImage;
		}
		set
		{
			m_DisableImage = value;
			DisableChanged = true;
			((Control)this).Invalidate();
		}
	}

	public override Color ForeColor
	{
		get
		{
			return ((Control)this).ForeColor;
		}
		set
		{
			MouseUpForeColor = value;
			((Control)this).ForeColor = value;
		}
	}

	public override Color BackColor
	{
		get
		{
			return ((Control)this).BackColor;
		}
		set
		{
			MouseUpBackColor = value;
			((Control)this).BackColor = value;
		}
	}

	public WindImageButton()
	{
		InitializeStyles();
		InitializeComponent();
	}

	private void InitializeComponent()
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Expected O, but got Unknown
		((Control)this).SuspendLayout();
		((Control)this).BackColor = Color.Transparent;
		((Control)this).DoubleBuffered = true;
		((Control)this).Name = "WindImageButton";
		((Control)this).Size = new Size(24, 24);
		((Control)this).MouseDown += new MouseEventHandler(OnMouseDown);
		((Control)this).MouseEnter += FontIconButton_MouseEnter;
		((Control)this).MouseLeave += FontIconButton_MouseLeave;
		((Control)this).MouseUp += new MouseEventHandler(FontIconButton_MouseUp);
		((Control)this).ResumeLayout(false);
	}

	private void InitializeStyles()
	{
		((Control)this).SetStyle((ControlStyles)141330, true);
		((Control)this).SetStyle((ControlStyles)512, false);
		((Control)this).UpdateStyles();
	}

	private ViewMode GetViewMode()
	{
		return (!(IconName == "")) ? ViewMode.ICON : ViewMode.IMAGE;
	}

	private void DrawImageMode(PaintEventArgs e)
	{
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Expected O, but got Unknown
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Expected O, but got Unknown
		//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f6: Expected O, but got Unknown
		//IL_037d: Unknown result type (might be due to invalid IL or missing references)
		//IL_038d: Expected O, but got Unknown
		Point point = new Point(0, 0);
		if (((Control)this).Text != null && ((Control)this).Text != "")
		{
			if (TextFixLocationEnable)
			{
				point = TextFixLocation;
			}
			else
			{
				SizeF sizeF = e.Graphics.MeasureString(((Control)this).Text, ((Control)this).Font);
				point.X = (((Control)this).Width - (int)sizeF.Width) / 2;
				point.Y = (((Control)this).Height - (int)sizeF.Height) / 2;
				textPoint = point;
				if (buttonStatus == ButtonStatus.MouseEnter || buttonStatus == ButtonStatus.MouseDown)
				{
					point.X += TextDynOffset.X;
					point.Y += TextDynOffset.Y;
				}
			}
		}
		e.Graphics.SetGDIHigh();
		if (!((Control)this).Enabled)
		{
			if (DisableImage == null)
			{
				GraphicsHelper.FillRect(e.Graphics, e.ClipRectangle, Radius, DisableBackColor, FrameMode);
			}
			else
			{
				e.Graphics.DrawImage(MouseUpImage, 0, 0, ((Control)this).Width, ((Control)this).Height);
			}
			DisableChanged = false;
			e.Graphics.CompositingQuality = (CompositingQuality)0;
			e.Graphics.DrawString(((Control)this).Text, ((Control)this).Font, (Brush)new SolidBrush(DisableForeColor), (PointF)point);
			return;
		}
		switch (buttonStatus)
		{
		case ButtonStatus.MouseUp:
			if (MouseUpImage == null)
			{
				GraphicsHelper.FillRect(e.Graphics, e.ClipRectangle, Radius, MouseUpBackColor, FrameMode);
			}
			else
			{
				e.Graphics.DrawImage(MouseUpImage, 0, 0, ((Control)this).Width, ((Control)this).Height);
			}
			MouseUpChanged = false;
			e.Graphics.CompositingQuality = (CompositingQuality)0;
			e.Graphics.DrawString(((Control)this).Text, ((Control)this).Font, (Brush)new SolidBrush(MouseUpForeColor), (PointF)point);
			break;
		case ButtonStatus.MouseEnter:
			if (MouseEnterImage == null)
			{
				GraphicsHelper.FillRect(e.Graphics, e.ClipRectangle, Radius, MouseEnterBackColor, FrameMode);
			}
			else
			{
				e.Graphics.DrawImage(MouseEnterImage, 0, 0, ((Control)this).Width, ((Control)this).Height);
			}
			MouseEnterChanged = false;
			e.Graphics.CompositingQuality = (CompositingQuality)0;
			e.Graphics.DrawString(((Control)this).Text, ((Control)this).Font, (Brush)new SolidBrush(MouseEnterForeColor), (PointF)point);
			break;
		case ButtonStatus.MouseDown:
			if (MouseDownImage == null)
			{
				GraphicsHelper.FillRect(e.Graphics, e.ClipRectangle, Radius, MouseDownBackColor, FrameMode);
			}
			else
			{
				e.Graphics.DrawImage(MouseDownImage, 0, 0, ((Control)this).Width, ((Control)this).Height);
			}
			MouseDownChanged = false;
			e.Graphics.CompositingQuality = (CompositingQuality)0;
			e.Graphics.DrawString(((Control)this).Text, ((Control)this).Font, (Brush)new SolidBrush(MouseDownForeColor), (PointF)point);
			break;
		}
	}

	private Image CreateIconImage(Color foreColor, Color backColor)
	{
		Bitmap image = FontImages.GetImage(IconName, IconSize, foreColor, backColor);
		GraphicsHelper.FillBitmapWithColor(image, IconBackColorRect, Color.FromArgb(((Component)this).DesignMode ? 100 : 2, foreColor));
		return (Image)(object)image;
	}

	private void DrawIcon(Graphics graphics, Image image)
	{
		Point point = new Point((((Control)this).Width - image.Width) / 2, (((Control)this).Height - image.Height) / 2);
		point.X += IconOffset.X;
		point.Y += IconOffset.Y;
		graphics.DrawImage(image, point.X, point.Y, image.Width, image.Height);
	}

	private void DrawIconMode(PaintEventArgs e)
	{
		e.Graphics.SetGDIHigh();
		if (!((Control)this).Enabled)
		{
			if (DisableImage == null || DisableChanged)
			{
				DisableImage = CreateIconImage(DisableForeColor, DisableBackColor);
			}
			DrawIcon(e.Graphics, DisableImage);
			DisableChanged = false;
			return;
		}
		switch (buttonStatus)
		{
		case ButtonStatus.MouseUp:
			if (MouseUpImage == null || MouseUpChanged)
			{
				MouseUpImage = CreateIconImage(MouseUpForeColor, MouseUpBackColor);
			}
			DrawIcon(e.Graphics, MouseUpImage);
			MouseUpChanged = false;
			break;
		case ButtonStatus.MouseEnter:
			if (MouseEnterImage == null || MouseEnterChanged)
			{
				MouseEnterImage = CreateIconImage(MouseEnterForeColor, MouseEnterBackColor);
			}
			DrawIcon(e.Graphics, MouseEnterImage);
			MouseEnterChanged = false;
			break;
		case ButtonStatus.MouseDown:
			if (MouseDownImage == null || MouseDownChanged)
			{
				MouseDownImage = CreateIconImage(MouseDownForeColor, MouseDownBackColor);
			}
			DrawIcon(e.Graphics, MouseDownImage);
			MouseDownChanged = false;
			break;
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		switch (GetViewMode())
		{
		case ViewMode.IMAGE:
			DrawImageMode(e);
			break;
		case ViewMode.ICON:
			DrawIconMode(e);
			break;
		}
	}

	public void SetMouseForeColor(Color color)
	{
		MouseUpForeColor = color;
		MouseEnterForeColor = color;
		MouseDownForeColor = color;
		((Control)this).Invalidate();
	}

	public void SetMouseBackColor(Color color)
	{
		MouseUpBackColor = color;
		MouseEnterBackColor = color;
		MouseDownBackColor = color;
		((Control)this).Invalidate();
	}

	protected override void OnCreateControl()
	{
	}

	private void SetButtonStatus(ButtonStatus state)
	{
		buttonStatus = state;
		((Control)this).Invalidate();
	}

	private void OnMouseDown(object sender, MouseEventArgs e)
	{
		SetButtonStatus(ButtonStatus.MouseDown);
	}

	private void FontIconButton_MouseUp(object sender, MouseEventArgs e)
	{
		SetButtonStatus(ButtonStatus.MouseUp);
	}

	private void FontIconButton_MouseEnter(object sender, EventArgs e)
	{
		SetButtonStatus(ButtonStatus.MouseEnter);
	}

	private void FontIconButton_MouseLeave(object sender, EventArgs e)
	{
		SetButtonStatus(ButtonStatus.MouseUp);
	}
}
