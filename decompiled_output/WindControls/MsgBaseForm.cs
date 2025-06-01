using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace WindControls;

public class MsgBaseForm : Form
{
	private int m_Radius = 12;

	private Color m_FrameColor = Color.DarkGray;

	private int m_FrameWidth = 1;

	private int ButtonHeight = 0;

	private Point movePoint;

	private IContainer components = null;

	private WindImageButton textButton_cancel;

	private WindImageButton textButton_OK;

	private SplitLine splitLine_top;

	private SplitLine splitLine_button;

	private SplitLine splitLine_bottom;

	private WindImageButton fIButton_Close;

	private BaseControl baseControl_title;

	[Description("自定义：圆角度数，为0表示没有圆角")]
	[Category("自定义")]
	public int Radius
	{
		get
		{
			return m_Radius;
		}
		set
		{
			m_Radius = value;
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

	[Description("自定义：圆角度数，为0表示没有圆角")]
	[Category("自定义")]
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

	public MsgBaseForm(string title, int width, int height, int buttonHeight)
	{
		InitializeComponent();
		SetTitle(title);
		ResizeWindow(width, height, buttonHeight);
	}

	public MsgBaseForm()
		: this("", 800, 600, 120)
	{
	}

	public void SetTitle(string title)
	{
		((Control)baseControl_title).Text = title;
	}

	public Rectangle GetClientRect()
	{
		return new Rectangle
		{
			X = ((Control)baseControl_title).Location.X,
			Y = ((Control)splitLine_top).Location.Y + ((Control)splitLine_top).Height,
			Width = ((Control)baseControl_title).Width,
			Height = ((Control)splitLine_bottom).Location.Y - ((Control)splitLine_top).Location.Y - ((Control)splitLine_top).Height
		};
	}

	public void ResizeWindow(int width, int height, int buttonHeight)
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		((Control)this).Width = width;
		((Control)this).Height = height;
		ButtonHeight = buttonHeight;
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		Rectangle rect = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
		((Control)this).Region = new Region(GraphicsHelper.GetRoundedRectPath(rect, Radius, GraphicsHelper.RoundStyle.All));
		baseControl_title.Radius = Radius;
		baseControl_title.FrameMode = GraphicsHelper.RoundStyle.Top;
		((Control)baseControl_title).Location = new Point(FrameWidth, FrameWidth);
		((Control)baseControl_title).Width = ((Control)this).Width - FrameWidth * 2;
		int num = (((Control)baseControl_title).Height - ((Control)fIButton_Close).Height) / 2;
		if (num < FrameWidth)
		{
			num = FrameWidth;
		}
		((Control)fIButton_Close).Location = new Point(((Control)baseControl_title).Width - ((Control)fIButton_Close).Width, num);
		((Control)splitLine_top).Height = 1;
		((Control)splitLine_top).Width = ((Control)baseControl_title).Width;
		((Control)splitLine_top).Location = new Point(((Control)baseControl_title).Location.X, ((Control)baseControl_title).Location.Y + ((Control)baseControl_title).Height);
		((Control)textButton_cancel).Width = (((Control)this).Width - FrameWidth * 2 - 1 - 2) / 2 - 1;
		((Control)textButton_cancel).Height = ((buttonHeight == 0) ? 54 : buttonHeight);
		textButton_cancel.Radius = Radius;
		textButton_cancel.FrameMode = GraphicsHelper.RoundStyle.BottomLeft;
		((Control)textButton_cancel).Location = new Point(((Control)baseControl_title).Location.X + 1, ((Control)this).Height - FrameWidth - ((Control)textButton_cancel).Height);
		((Control)textButton_OK).Width = ((Control)textButton_cancel).Width;
		((Control)textButton_OK).Height = ((Control)textButton_cancel).Height;
		textButton_OK.Radius = Radius;
		textButton_OK.FrameMode = GraphicsHelper.RoundStyle.BottomRight;
		((Control)textButton_OK).Location = new Point(((Control)textButton_cancel).Location.X + ((Control)textButton_cancel).Width + 3, ((Control)textButton_cancel).Location.Y);
		((Control)splitLine_button).Width = 1;
		((Control)splitLine_button).Height = ((Control)textButton_cancel).Height;
		((Control)splitLine_button).Location = new Point(((Control)textButton_OK).Location.X - 2, ((Control)textButton_OK).Location.Y);
		((Control)splitLine_bottom).Height = 1;
		((Control)splitLine_bottom).Width = ((Control)baseControl_title).Width;
		((Control)splitLine_bottom).Location = new Point(((Control)baseControl_title).Location.X, ((Control)textButton_OK).Location.Y - ((Control)splitLine_bottom).Height);
	}

	private void FormMsgDialogYesNo_Paint(object sender, PaintEventArgs e)
	{
		if (((Control)this).Visible)
		{
			GraphicsHelper.FillRect(frameRect: new Rectangle(0, 0, ((Control)this).Width - 1, ((Control)this).Height - 1), g: e.Graphics, Radius: Radius, backColor: ((Control)this).BackColor, frameWidth: FrameWidth, frameColor: FrameColor, roundStyle: GraphicsHelper.RoundStyle.All);
		}
	}

	private void fIButton_Close_Click(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void baseControl_title_MouseDown(object sender, MouseEventArgs e)
	{
		movePoint = new Point(e.X, e.Y);
	}

	private void baseControl_title_MouseMove(object sender, MouseEventArgs e)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Invalid comparison between Unknown and I4
		if ((int)e.Button == 1048576)
		{
			_ = movePoint;
			if (true)
			{
				((Form)this).Location = new Point(((Form)this).Location.X + e.X - movePoint.X, ((Form)this).Location.Y + e.Y - movePoint.Y);
			}
		}
	}

	private void textButton_cancel_Click(object sender, EventArgs e)
	{
		((Form)this).DialogResult = (DialogResult)2;
	}

	private void textButton_OK_Click(object sender, EventArgs e)
	{
		((Form)this).DialogResult = (DialogResult)1;
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		if ((int)keyData == 13)
		{
			textButton_OK_Click(null, null);
		}
		return ((Form)this).ProcessCmdKey(ref msg, keyData);
	}

	private void MsgBaseForm_Load(object sender, EventArgs e)
	{
		ResizeWindow(((Control)this).Width, ((Control)this).Height, ButtonHeight);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((Form)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Expected O, but got Unknown
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Expected O, but got Unknown
		//IL_0322: Unknown result type (might be due to invalid IL or missing references)
		//IL_032c: Expected O, but got Unknown
		//IL_04a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ac: Expected O, but got Unknown
		//IL_062b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0635: Expected O, but got Unknown
		//IL_0717: Unknown result type (might be due to invalid IL or missing references)
		//IL_0721: Expected O, but got Unknown
		//IL_072f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0739: Expected O, but got Unknown
		//IL_083b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0845: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(MsgBaseForm));
		fIButton_Close = new WindImageButton();
		splitLine_bottom = new SplitLine();
		splitLine_button = new SplitLine();
		splitLine_top = new SplitLine();
		textButton_OK = new WindImageButton();
		textButton_cancel = new WindImageButton();
		baseControl_title = new BaseControl();
		((Control)this).SuspendLayout();
		((Control)fIButton_Close).BackColor = Color.White;
		fIButton_Close.FrameMode = GraphicsHelper.RoundStyle.All;
		fIButton_Close.IconName = "A_fa_window_close";
		fIButton_Close.IconSize = 48;
		((Control)fIButton_Close).Location = new Point(449, 12);
		fIButton_Close.MouseDownBackColor = Color.Transparent;
		fIButton_Close.MouseDownImage = (Image)componentResourceManager.GetObject("fIButton_Close.MouseDownImage");
		fIButton_Close.MouseDownForeColor = Color.DarkRed;
		fIButton_Close.MouseEnterBackColor = Color.Transparent;
		fIButton_Close.MouseEnterImage = (Image)componentResourceManager.GetObject("fIButton_Close.MouseEnterImage");
		fIButton_Close.MouseEnterForeColor = Color.OrangeRed;
		((Control)fIButton_Close).Name = "fIButton_Close";
		fIButton_Close.MouseUpImage = (Image)componentResourceManager.GetObject("fIButton_Close.NormalImage");
		fIButton_Close.MouseUpForeColor = Color.Red;
		fIButton_Close.Radius = 12;
		((Control)fIButton_Close).Size = new Size(46, 49);
		((Control)fIButton_Close).TabIndex = 0;
		((Control)fIButton_Close).Text = null;
		fIButton_Close.TextAlignment = StringHelper.TextAlignment.Center;
		((Control)fIButton_Close).Click += fIButton_Close_Click;
		((Control)splitLine_bottom).BackColor = Color.LightGray;
		((Control)splitLine_bottom).Location = new Point(-4, 297);
		((Control)splitLine_bottom).Name = "splitLine_bottom";
		((Control)splitLine_bottom).Size = new Size(501, 1);
		((Control)splitLine_bottom).TabIndex = 5;
		((Control)splitLine_button).BackColor = Color.LightGray;
		((Control)splitLine_button).Location = new Point(250, 310);
		((Control)splitLine_button).Name = "splitLine_button";
		((Control)splitLine_button).RightToLeft = (RightToLeft)0;
		((Control)splitLine_button).Size = new Size(2, 48);
		((Control)splitLine_button).TabIndex = 4;
		((Control)splitLine_top).BackColor = Color.LightGray;
		((Control)splitLine_top).Location = new Point(1, 67);
		((Control)splitLine_top).Name = "splitLine_top";
		((Control)splitLine_top).Size = new Size(502, 1);
		((Control)splitLine_top).TabIndex = 3;
		((Control)textButton_OK).BackColor = Color.White;
		((Control)textButton_OK).Font = new Font("微软雅黑", 21.75f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)textButton_OK).ForeColor = Color.Red;
		textButton_OK.FrameMode = GraphicsHelper.RoundStyle.BottomRight;
		textButton_OK.IconName = "";
		textButton_OK.IconSize = 32;
		((Control)textButton_OK).Location = new Point(255, 300);
		textButton_OK.MouseDownBackColor = Color.LightPink;
		textButton_OK.MouseDownImage = null;
		textButton_OK.MouseDownForeColor = Color.DarkRed;
		textButton_OK.MouseEnterBackColor = Color.WhiteSmoke;
		textButton_OK.MouseEnterImage = null;
		textButton_OK.MouseEnterForeColor = Color.OrangeRed;
		((Control)textButton_OK).Name = "textButton_OK";
		textButton_OK.MouseUpImage = null;
		textButton_OK.MouseUpForeColor = Color.Red;
		textButton_OK.Radius = 0;
		((Control)textButton_OK).Size = new Size(249, 67);
		((Control)textButton_OK).TabIndex = 1;
		((Control)textButton_OK).Text = "确定";
		textButton_OK.TextAlignment = StringHelper.TextAlignment.Center;
		((Control)textButton_OK).Click += textButton_OK_Click;
		((Control)textButton_cancel).BackColor = Color.White;
		((Control)textButton_cancel).Font = new Font("微软雅黑", 21.75f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)textButton_cancel).ForeColor = Color.Blue;
		textButton_cancel.FrameMode = GraphicsHelper.RoundStyle.BottomLeft;
		textButton_cancel.IconName = "";
		textButton_cancel.IconSize = 32;
		((Control)textButton_cancel).Location = new Point(2, 305);
		textButton_cancel.MouseDownBackColor = Color.LightSteelBlue;
		textButton_cancel.MouseDownImage = null;
		textButton_cancel.MouseDownForeColor = Color.DarkRed;
		textButton_cancel.MouseEnterBackColor = Color.WhiteSmoke;
		textButton_cancel.MouseEnterImage = null;
		textButton_cancel.MouseEnterForeColor = Color.OrangeRed;
		((Control)textButton_cancel).Name = "textButton_cancel";
		textButton_cancel.MouseUpImage = null;
		textButton_cancel.MouseUpForeColor = Color.Red;
		textButton_cancel.Radius = 0;
		((Control)textButton_cancel).Size = new Size(245, 59);
		((Control)textButton_cancel).TabIndex = 0;
		((Control)textButton_cancel).Text = "取消";
		textButton_cancel.TextAlignment = StringHelper.TextAlignment.Center;
		((Control)textButton_cancel).Click += textButton_cancel_Click;
		((Control)baseControl_title).BackColor = Color.Transparent;
		baseControl_title.EnableColorChange = false;
		((Control)baseControl_title).Font = new Font("微软雅黑", 21.75f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)baseControl_title).ForeColor = SystemColors.ControlText;
		baseControl_title.FrameColor = Color.Transparent;
		baseControl_title.FrameMode = GraphicsHelper.RoundStyle.All;
		baseControl_title.FrameWidth = 0;
		((Control)baseControl_title).Location = new Point(2, 3);
		baseControl_title.MouseDownBackColor = Color.Transparent;
		baseControl_title.MouseEnterBackColor = Color.Transparent;
		((Control)baseControl_title).Name = "baseControl_title";
		baseControl_title.Radius = 12;
		((Control)baseControl_title).Size = new Size(502, 59);
		((Control)baseControl_title).TabIndex = 7;
		((Control)baseControl_title).Text = "标题";
		baseControl_title.TextAlignment = StringHelper.TextAlignment.Center;
		((Control)baseControl_title).MouseDown += new MouseEventHandler(baseControl_title_MouseDown);
		((Control)baseControl_title).MouseMove += new MouseEventHandler(baseControl_title_MouseMove);
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 12f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).ClientSize = new Size(507, 371);
		((Control)this).Controls.Add((Control)(object)fIButton_Close);
		((Control)this).Controls.Add((Control)(object)splitLine_bottom);
		((Control)this).Controls.Add((Control)(object)splitLine_button);
		((Control)this).Controls.Add((Control)(object)splitLine_top);
		((Control)this).Controls.Add((Control)(object)textButton_OK);
		((Control)this).Controls.Add((Control)(object)textButton_cancel);
		((Control)this).Controls.Add((Control)(object)baseControl_title);
		((Form)this).KeyPreview = true;
		((Control)this).Name = "MsgBaseForm";
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "MsgBase";
		((Form)this).Load += MsgBaseForm_Load;
		((Control)this).Paint += new PaintEventHandler(FormMsgDialogYesNo_Paint);
		((Control)this).ResumeLayout(false);
	}
}
