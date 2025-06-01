using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace WindControls;

[DefaultEvent("TextChanged")]
public class WindTextBox : Panel
{
	private Color foreColor = Color.Black;

	private bool promptTextShow = false;

	private string m_PasswordChar = "";

	private Point m_TextBoxOffset = new Point(0, 0);

	private string m_PromptText = "";

	private Color m_PromptTextColor = Color.Gray;

	private Color m_PromptTextForeColor = Color.LightGray;

	private Image m_SelectedBackImage = null;

	private Image m_UnSelectedBackImage = null;

	private Image m_DisableBackImage = null;

	private Color m_FrameColor = Color.White;

	private IContainer components = null;

	public WindBaseTextBox wbTextBox;

	[Description("自定义：显示文本")]
	[Category("自定义")]
	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	[EditorBrowsable(EditorBrowsableState.Always)]
	[Bindable(true)]
	public override string Text
	{
		get
		{
			return ((Control)wbTextBox).Text;
		}
		set
		{
			((Control)wbTextBox).Text = value;
		}
	}

	[Description("自定义：文本是否只读")]
	[Category("自定义")]
	public bool ReadOnly
	{
		get
		{
			return ((TextBoxBase)wbTextBox).ReadOnly;
		}
		set
		{
			((TextBoxBase)wbTextBox).ReadOnly = value;
		}
	}

	[Description("自定义：隐藏字符")]
	[Category("自定义")]
	public string PasswordChar
	{
		get
		{
			return m_PasswordChar;
		}
		set
		{
			m_PasswordChar = value;
		}
	}

	[Description("自定义：文本偏移")]
	[Category("自定义")]
	public Point TextBoxOffset
	{
		get
		{
			return m_TextBoxOffset;
		}
		set
		{
			m_TextBoxOffset = value;
			((Control)wbTextBox).Location = value;
		}
	}

	[Description("自定义：文本最大长度")]
	[Category("自定义")]
	public int MaxLength
	{
		get
		{
			return ((TextBoxBase)wbTextBox).MaxLength;
		}
		set
		{
			((TextBoxBase)wbTextBox).MaxLength = value;
		}
	}

	[Description("自定义：文本是否只允许10进制或16进制")]
	[Category("自定义")]
	public WindBaseTextBox.TextBoxInputMode InputMode
	{
		get
		{
			return wbTextBox.InputMode;
		}
		set
		{
			wbTextBox.InputMode = value;
		}
	}

	[Description("自定义：文本字体")]
	[Category("自定义")]
	public override Font Font
	{
		get
		{
			return ((Control)this).Font;
		}
		set
		{
			((Control)this).Font = value;
			((Control)wbTextBox).Font = value;
		}
	}

	[Description("自定义：提示文本")]
	[Category("自定义")]
	public string PromptText
	{
		get
		{
			return m_PromptText;
		}
		set
		{
			m_PromptText = value;
		}
	}

	[Description("自定义：提示文本颜色")]
	[Category("自定义")]
	public Color PromptTextColor
	{
		get
		{
			return m_PromptTextColor;
		}
		set
		{
			m_PromptTextColor = value;
		}
	}

	[Description("自定义：提示文本颜色")]
	[Category("自定义")]
	public Color PromptTextForeColor
	{
		get
		{
			return m_PromptTextForeColor;
		}
		set
		{
			m_PromptTextForeColor = value;
		}
	}

	[Description("自定义：选中时背景色")]
	[Category("自定义")]
	public Image SelectedBackImage
	{
		get
		{
			return m_SelectedBackImage;
		}
		set
		{
			m_SelectedBackImage = value;
		}
	}

	[Description("自定义：未选中时背景色")]
	[Category("自定义")]
	public Image UnSelectedBackImage
	{
		get
		{
			return m_UnSelectedBackImage;
		}
		set
		{
			m_UnSelectedBackImage = value;
		}
	}

	[Description("自定义：Disable时背景色")]
	[Category("自定义")]
	public Image DisableBackImage
	{
		get
		{
			return m_DisableBackImage;
		}
		set
		{
			m_DisableBackImage = value;
		}
	}

	[Description("自定义：边框颜色")]
	[Category("自定义")]
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

	[Description("自定义：边框颜色")]
	[Category("自定义")]
	public override Color ForeColor
	{
		get
		{
			return ((Control)this).ForeColor;
		}
		set
		{
			((Control)this).ForeColor = value;
			((Control)wbTextBox).ForeColor = value;
		}
	}

	[Browsable(true)]
	public event EventHandler TextChanged;

	public WindTextBox()
	{
		InitializeComponent();
		SetStyles();
		foreColor = ((Control)wbTextBox).ForeColor;
		((Control)wbTextBox).GotFocus += WbTextBox_GotFocus;
		((Control)wbTextBox).LostFocus += WbTextBox_LostFocus;
		((Control)wbTextBox).Location = TextBoxOffset;
	}

	private void SetStyles()
	{
		((Control)this).SetStyle((ControlStyles)204818, true);
		((Control)this).UpdateStyles();
	}

	private void WbTextBox_TextChanged(object sender, EventArgs e)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		((Control)wbTextBox).Font = new Font(((Control)this).Font.FontFamily, ((Control)this).Font.Size, ((Control)this).Font.Style);
		if (this.TextChanged != null)
		{
			this.TextChanged(sender, e);
		}
	}

	protected override void OnCreateControl()
	{
		((Control)this).OnCreateControl();
		WindTextBox_EnabledChanged(null, null);
		UpdatePromptText();
	}

	private void WbTextBox_LostFocus(object sender, EventArgs e)
	{
		if (UnSelectedBackImage != null)
		{
			((Control)this).BackgroundImage = UnSelectedBackImage;
		}
	}

	private void WbTextBox_GotFocus(object sender, EventArgs e)
	{
		if (SelectedBackImage != null)
		{
			((Control)this).BackgroundImage = SelectedBackImage;
		}
	}

	private void WindTextBox_SizeChanged(object sender, EventArgs e)
	{
		((Control)wbTextBox).Width = ((Control)this).Width;
		((Control)wbTextBox).Height = ((Control)this).Height;
	}

	private void WindTextBox_EnabledChanged(object sender, EventArgs e)
	{
		((Control)wbTextBox).Enabled = ((Control)this).Enabled;
		if (((Control)this).Enabled)
		{
			if (((Control)wbTextBox).Focused)
			{
				if (SelectedBackImage != null)
				{
					((Control)this).BackgroundImage = SelectedBackImage;
				}
			}
			else if (UnSelectedBackImage != null)
			{
				((Control)this).BackgroundImage = UnSelectedBackImage;
			}
		}
		else if (DisableBackImage != null)
		{
			((Control)this).BackgroundImage = DisableBackImage;
		}
	}

	private void wbTextBox_MouseDown(object sender, MouseEventArgs e)
	{
		((Control)this).OnMouseDown(e);
	}

	private void wbTextBox_MouseEnter(object sender, EventArgs e)
	{
		((Control)this).OnMouseEnter(e);
	}

	private void wbTextBox_MouseLeave(object sender, EventArgs e)
	{
		((Control)this).OnMouseLeave(e);
	}

	private void wbTextBox_MouseMove(object sender, MouseEventArgs e)
	{
		((Control)this).OnMouseMove(e);
	}

	private void wbTextBox_MouseUp(object sender, MouseEventArgs e)
	{
		((Control)this).OnMouseUp(e);
	}

	private void WindTextBox_LocationChanged(object sender, EventArgs e)
	{
		((Control)wbTextBox).Location = TextBoxOffset;
		((Control)wbTextBox).Invalidate();
	}

	private void UpdatePromptText()
	{
		if (((Control)wbTextBox).Text == "")
		{
			if (PromptText != "" && !promptTextShow)
			{
				promptTextShow = true;
				((Control)wbTextBox).Invalidate();
			}
		}
		else if (promptTextShow)
		{
			promptTextShow = false;
			((Control)wbTextBox).Invalidate();
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Expected O, but got Unknown
		if (promptTextShow && PromptText != "")
		{
			SizeF sizeF = e.Graphics.MeasureString(PromptText, ((Control)wbTextBox).Font);
			Point point = new Point(4, (((Control)wbTextBox).Height - (int)sizeF.Height) / 2);
			e.Graphics.DrawString(PromptText, ((Control)wbTextBox).Font, (Brush)new SolidBrush(PromptTextColor), (PointF)point);
		}
	}

	private void wbTextBox_TextChanged(object sender, EventArgs e)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		UpdatePromptText();
		((Control)this).Font = new Font(((Control)this).Font, ((Control)this).Font.Style);
	}

	public void UserInvalidate()
	{
		int width = ((Control)this).Width;
		((Control)this).Width = width + 1;
		width = ((Control)this).Width;
		((Control)this).Width = width - 1;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((Control)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Expected O, but got Unknown
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Expected O, but got Unknown
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Expected O, but got Unknown
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Expected O, but got Unknown
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		wbTextBox = new WindBaseTextBox();
		((Control)this).SuspendLayout();
		((TextBoxBase)wbTextBox).BorderStyle = (BorderStyle)0;
		((Control)wbTextBox).Font = new Font("微软雅黑", 15f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)wbTextBox).ForeColor = Color.Black;
		wbTextBox.InputMode = WindBaseTextBox.TextBoxInputMode.Normal;
		((Control)wbTextBox).Location = new Point(6, 6);
		((Control)wbTextBox).Margin = new Padding(6);
		((Control)wbTextBox).Name = "wbTextBox";
		((Control)wbTextBox).Size = new Size(104, 31);
		((Control)wbTextBox).TabIndex = 0;
		((Control)wbTextBox).Text = "";
		((TextBoxBase)wbTextBox).Multiline = false;
		((Control)wbTextBox).TextChanged += wbTextBox_TextChanged;
		((Control)wbTextBox).MouseDown += new MouseEventHandler(wbTextBox_MouseDown);
		((Control)wbTextBox).MouseEnter += wbTextBox_MouseEnter;
		((Control)wbTextBox).MouseLeave += wbTextBox_MouseLeave;
		((Control)wbTextBox).MouseMove += new MouseEventHandler(wbTextBox_MouseMove);
		((Control)wbTextBox).MouseUp += new MouseEventHandler(wbTextBox_MouseUp);
		((Control)this).Controls.Add((Control)(object)wbTextBox);
		((Control)this).Font = new Font("微软雅黑", 15f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)this).Margin = new Padding(6);
		((Control)this).Name = "WindTextBox";
		((Control)this).Size = new Size(124, 43);
		((Control)this).EnabledChanged += WindTextBox_EnabledChanged;
		((Control)this).LocationChanged += WindTextBox_LocationChanged;
		((Control)this).SizeChanged += WindTextBox_SizeChanged;
		((Control)this).ResumeLayout(false);
	}
}
