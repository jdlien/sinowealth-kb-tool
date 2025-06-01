using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using USBUpdateTool.Properties;
using WindControls;

namespace USBUpdateTool;

public class FormImageBarLib : Form
{
	private Form form;

	public SkinForm skinForm = new SkinForm(_movable: true);

	private ProgressAnimation progressAnimation;

	private ImageBar imageBar;

	private IContainer components = null;

	private WindImageButton wiButton_mini;

	private WindImageButton wiButton_Close;

	private WindImageButton wiButton_OK;

	public FormImageBarLib(ImageBar _imageBar)
	{
		InitializeComponent();
		SetStyles();
		skinForm.InitSkin((Form)(object)this, 12, 16);
		imageBar = _imageBar;
		progressAnimation = new ProgressAnimation((Form)(object)this);
	}

	public void SetProgressBar(ImageBar _imageBar)
	{
	}

	private void SetStyles()
	{
		((Control)this).SetStyle((ControlStyles)204818, true);
		((Control)this).UpdateStyles();
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)0;
	}

	private void wiButton_mini_Click(object sender, EventArgs e)
	{
		((Form)skinForm).WindowState = (FormWindowState)1;
		((Form)this).WindowState = (FormWindowState)1;
	}

	private void wiButton_Close_Click(object sender, EventArgs e)
	{
		progressAnimation.Stop();
		skinForm.AllHide();
		form.Activate();
	}

	public void ShowWindow(Form _form)
	{
		form = _form;
		skinForm.AllShow(form);
		((Form)this).Location = form.Location;
	}

	private void wIBtn_progTextColor_Click(object sender, EventArgs e)
	{
	}

	private void FormImageBarEdit_FormClosing(object sender, FormClosingEventArgs e)
	{
		progressAnimation.Stop();
	}

	private void CloneImageBar(ImageBar dstBar, ImageBar srcBar)
	{
		dstBar.BarSliderMovable = srcBar.BarSliderMovable;
		dstBar.BarSliderImage = srcBar.BarSliderImage;
		dstBar.BarForeImage = srcBar.BarForeImage;
		dstBar.BarBackImage = srcBar.BarBackImage;
		dstBar.BarSliderImageRect = srcBar.BarSliderImageRect;
		dstBar.BarForeImageRect = srcBar.BarForeImageRect;
		dstBar.BarBackImageRect = srcBar.BarBackImageRect;
		((Control)dstBar).Width = ((Control)srcBar).Width;
		((Control)dstBar).Height = ((Control)srcBar).Height;
		dstBar.TextLocation = srcBar.TextLocation;
		((Control)dstBar).Font = ((Control)srcBar).Font;
		dstBar.TextColor = srcBar.TextColor;
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
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Expected O, but got Unknown
		//IL_0694: Unknown result type (might be due to invalid IL or missing references)
		//IL_069e: Expected O, but got Unknown
		//IL_06b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bd: Expected O, but got Unknown
		//IL_06ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f8: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FormImageBarLib));
		wiButton_OK = new WindImageButton();
		wiButton_mini = new WindImageButton();
		wiButton_Close = new WindImageButton();
		((Control)this).SuspendLayout();
		((Control)wiButton_OK).BackColor = Color.Transparent;
		wiButton_OK.DisableBackColor = Color.DarkGray;
		wiButton_OK.DisableForeColor = Color.DimGray;
		((Control)wiButton_OK).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wiButton_OK.FrameMode = GraphicsHelper.RoundStyle.All;
		wiButton_OK.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wiButton_OK.IconName = "";
		wiButton_OK.IconOffset = new Point(0, 0);
		wiButton_OK.IconSize = 32;
		((Control)wiButton_OK).Location = new Point(11, 562);
		wiButton_OK.MouseDownBackColor = Color.MediumTurquoise;
		wiButton_OK.MouseDownForeColor = Color.Black;
		wiButton_OK.MouseEnterBackColor = Color.Turquoise;
		wiButton_OK.MouseEnterForeColor = Color.Black;
		wiButton_OK.MouseUpBackColor = Color.LightSeaGreen;
		wiButton_OK.MouseUpForeColor = Color.Black;
		((Control)wiButton_OK).Name = "wiButton_OK";
		wiButton_OK.Radius = 16;
		((Control)wiButton_OK).Size = new Size(136, 35);
		((Control)wiButton_OK).TabIndex = 162;
		((Control)wiButton_OK).Tag = "0";
		((Control)wiButton_OK).Text = "确定";
		wiButton_OK.TextAlignment = StringHelper.TextAlignment.Center;
		wiButton_OK.TextDynOffset = new Point(0, 0);
		wiButton_OK.TextFixLocation = new Point(0, 0);
		wiButton_OK.TextFixLocationEnable = false;
		((Control)wiButton_mini).BackColor = Color.Transparent;
		((Control)wiButton_mini).BackgroundImage = (Image)(object)Resources.MiniButtonNormalImage;
		((Control)wiButton_mini).BackgroundImageLayout = (ImageLayout)2;
		wiButton_mini.DisableBackColor = Color.Transparent;
		wiButton_mini.DisableForeColor = Color.DarkGray;
		wiButton_mini.DisableImage = (Image)(object)Resources.MiniButtonMouseDownImage;
		wiButton_mini.FrameMode = GraphicsHelper.RoundStyle.All;
		wiButton_mini.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wiButton_mini.IconName = "";
		wiButton_mini.IconOffset = new Point(0, 0);
		wiButton_mini.IconSize = 32;
		((Control)wiButton_mini).Location = new Point(743, 21);
		wiButton_mini.MouseDownBackColor = Color.Gray;
		wiButton_mini.MouseDownForeColor = Color.DarkRed;
		wiButton_mini.MouseDownImage = (Image)(object)Resources.MiniButtonMouseEnterImage;
		wiButton_mini.MouseEnterBackColor = Color.DarkGray;
		wiButton_mini.MouseEnterForeColor = Color.OrangeRed;
		wiButton_mini.MouseEnterImage = (Image)(object)Resources.MiniButtonMouseDownImage;
		wiButton_mini.MouseUpBackColor = Color.Transparent;
		wiButton_mini.MouseUpForeColor = Color.Red;
		wiButton_mini.MouseUpImage = (Image)(object)Resources.MiniButtonNormalImage;
		((Control)wiButton_mini).Name = "wiButton_mini";
		wiButton_mini.Radius = 12;
		((Control)wiButton_mini).Size = new Size(18, 18);
		((Control)wiButton_mini).TabIndex = 48;
		((Control)wiButton_mini).Text = null;
		wiButton_mini.TextAlignment = StringHelper.TextAlignment.Center;
		wiButton_mini.TextDynOffset = new Point(0, 0);
		wiButton_mini.TextFixLocation = new Point(0, 0);
		wiButton_mini.TextFixLocationEnable = false;
		((Control)wiButton_mini).Click += wiButton_mini_Click;
		((Control)wiButton_Close).BackColor = Color.Transparent;
		((Control)wiButton_Close).BackgroundImage = (Image)(object)Resources.close_2;
		((Control)wiButton_Close).BackgroundImageLayout = (ImageLayout)2;
		wiButton_Close.DisableBackColor = Color.Transparent;
		wiButton_Close.DisableForeColor = Color.DarkGray;
		wiButton_Close.DisableImage = (Image)(object)Resources.close_3;
		wiButton_Close.FrameMode = GraphicsHelper.RoundStyle.All;
		wiButton_Close.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wiButton_Close.IconName = "";
		wiButton_Close.IconOffset = new Point(0, 0);
		wiButton_Close.IconSize = 32;
		((Control)wiButton_Close).Location = new Point(772, 21);
		wiButton_Close.MouseDownBackColor = Color.Gray;
		wiButton_Close.MouseDownForeColor = Color.DarkRed;
		wiButton_Close.MouseDownImage = (Image)(object)Resources.close_1;
		wiButton_Close.MouseEnterBackColor = Color.DarkGray;
		wiButton_Close.MouseEnterForeColor = Color.OrangeRed;
		wiButton_Close.MouseEnterImage = (Image)(object)Resources.close_1;
		wiButton_Close.MouseUpBackColor = Color.Transparent;
		wiButton_Close.MouseUpForeColor = Color.Red;
		wiButton_Close.MouseUpImage = (Image)(object)Resources.close_2;
		((Control)wiButton_Close).Name = "wiButton_Close";
		wiButton_Close.Radius = 12;
		((Control)wiButton_Close).Size = new Size(18, 18);
		((Control)wiButton_Close).TabIndex = 47;
		((Control)wiButton_Close).Text = null;
		wiButton_Close.TextAlignment = StringHelper.TextAlignment.Center;
		wiButton_Close.TextDynOffset = new Point(0, 0);
		wiButton_Close.TextFixLocation = new Point(0, 0);
		wiButton_Close.TextFixLocationEnable = false;
		((Control)wiButton_Close).Click += wiButton_Close_Click;
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)0;
		((Control)this).BackgroundImage = (Image)(object)Resources.backBig;
		((Form)this).ClientSize = new Size(822, 618);
		((Control)this).Controls.Add((Control)(object)wiButton_OK);
		((Control)this).Controls.Add((Control)(object)wiButton_mini);
		((Control)this).Controls.Add((Control)(object)wiButton_Close);
		((Control)this).Font = new Font("微软雅黑", 10.5f);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Form)this).KeyPreview = true;
		((Control)this).Name = "FormImageBarLib";
		((Form)this).StartPosition = (FormStartPosition)0;
		((Control)this).Text = "EditCidMid";
		((Form)this).FormClosing += new FormClosingEventHandler(FormImageBarEdit_FormClosing);
		((Control)this).ResumeLayout(false);
	}
}
