using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using USBUpdateTool.Properties;
using WindControls;

namespace USBUpdateTool;

public class FormMore : Form
{
	private FormMakeUpgradeTool formMakeUpgradeTool;

	private FormMakePairTool formMakePairTool;

	private FormCompxMore formCompxMore;

	private CompxConfig compxConfig = new CompxConfig();

	public SkinForm skinForm = new SkinForm(_movable: true);

	public FormMain formMain;

	private ICSelect iCSelect = new ICSelect();

	private bool passwordSuccess = false;

	private IContainer components = null;

	private WindImageButton wibutton_Make_Upgrade_package;

	private WindImageButton wibutton_More;

	private WindImageButton wiButton_mini;

	private WindImageButton wiButton_Close;

	private WindImageButton wibutton_Make_Pairing_package;

	public FormMore(FormMain _formMain)
	{
		InitializeComponent();
		SetStyles();
		skinForm.InitSkin((Form)(object)this, 12, 16);
		formMain = _formMain;
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
		skinForm.AllHide();
		formMain.skinForm.AllShow();
		formMain.upgradeManager.SetUsbChangedCallBack();
	}

	public void ShowWindow()
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Invalid comparison between Unknown and I4
		skinForm.AllShow((Form)(object)formMain);
		((Form)this).Location = ((Form)formMain).Location;
		if (!passwordSuccess)
		{
			FormPasswordInput formPasswordInput = new FormPasswordInput(0);
			if ((int)formPasswordInput.ShowDialog((Form)(object)this) == 1)
			{
				passwordSuccess = true;
				((Form)skinForm).Activate();
				((Form)this).Activate();
			}
			else
			{
				wiButton_Close_Click(null, null);
			}
		}
	}

	private void wibutton_Make_Upgrade_package_Click(object sender, EventArgs e)
	{
		if (formMakeUpgradeTool == null)
		{
			formMakeUpgradeTool = new FormMakeUpgradeTool(this);
		}
		skinForm.AllHide();
		formMakeUpgradeTool.ShowWindow();
	}

	private void wibutton_Make_Pairing_package_Click(object sender, EventArgs e)
	{
		if (formMakePairTool == null)
		{
			formMakePairTool = new FormMakePairTool(this);
		}
		skinForm.AllHide();
		formMakePairTool.ShowWindow();
	}

	private void wibutton_More_Click(object sender, EventArgs e)
	{
		if (formCompxMore == null)
		{
			formCompxMore = new FormCompxMore(this);
		}
		skinForm.AllHide();
		formCompxMore.ShowWindow();
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
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Expected O, but got Unknown
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0388: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0770: Unknown result type (might be due to invalid IL or missing references)
		//IL_077a: Expected O, but got Unknown
		//IL_07f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ca: Expected O, but got Unknown
		//IL_0a45: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c38: Expected O, but got Unknown
		//IL_0c4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c57: Expected O, but got Unknown
		//IL_0c5d: Unknown result type (might be due to invalid IL or missing references)
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FormMore));
		wibutton_Make_Pairing_package = new WindImageButton();
		wiButton_mini = new WindImageButton();
		wiButton_Close = new WindImageButton();
		wibutton_More = new WindImageButton();
		wibutton_Make_Upgrade_package = new WindImageButton();
		((Control)this).SuspendLayout();
		((Control)wibutton_Make_Pairing_package).BackColor = Color.Transparent;
		((Control)wibutton_Make_Pairing_package).BackgroundImage = (Image)(object)Resources.button_1;
		((Control)wibutton_Make_Pairing_package).BackgroundImageLayout = (ImageLayout)3;
		wibutton_Make_Pairing_package.DisableBackColor = Color.Transparent;
		wibutton_Make_Pairing_package.DisableForeColor = Color.DarkGray;
		wibutton_Make_Pairing_package.DisableImage = (Image)(object)Resources.button_2;
		((Control)wibutton_Make_Pairing_package).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wibutton_Make_Pairing_package.FrameMode = GraphicsHelper.RoundStyle.All;
		wibutton_Make_Pairing_package.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wibutton_Make_Pairing_package.IconName = "";
		wibutton_Make_Pairing_package.IconOffset = new Point(0, 0);
		wibutton_Make_Pairing_package.IconSize = 32;
		((Control)wibutton_Make_Pairing_package).Location = new Point(464, 142);
		((Control)wibutton_Make_Pairing_package).Margin = new Padding(4, 5, 4, 5);
		wibutton_Make_Pairing_package.MouseDownBackColor = Color.Gray;
		wibutton_Make_Pairing_package.MouseDownForeColor = Color.Black;
		wibutton_Make_Pairing_package.MouseDownImage = (Image)(object)Resources.点击状态2;
		wibutton_Make_Pairing_package.MouseEnterBackColor = Color.DarkGray;
		wibutton_Make_Pairing_package.MouseEnterForeColor = Color.Black;
		wibutton_Make_Pairing_package.MouseEnterImage = (Image)(object)Resources.点击状态2;
		wibutton_Make_Pairing_package.MouseUpBackColor = Color.Transparent;
		wibutton_Make_Pairing_package.MouseUpForeColor = Color.Black;
		wibutton_Make_Pairing_package.MouseUpImage = (Image)(object)Resources.默认状态2;
		((Control)wibutton_Make_Pairing_package).Name = "wibutton_Make_Pairing_package";
		wibutton_Make_Pairing_package.Radius = 12;
		((Control)wibutton_Make_Pairing_package).Size = new Size(200, 60);
		((Control)wibutton_Make_Pairing_package).TabIndex = 150;
		((Control)wibutton_Make_Pairing_package).Text = "Make pairing package";
		wibutton_Make_Pairing_package.TextAlignment = StringHelper.TextAlignment.Center;
		wibutton_Make_Pairing_package.TextDynOffset = new Point(0, 0);
		wibutton_Make_Pairing_package.TextFixLocation = new Point(0, 0);
		wibutton_Make_Pairing_package.TextFixLocationEnable = false;
		((Control)wibutton_Make_Pairing_package).Visible = false;
		((Control)wibutton_Make_Pairing_package).Click += wibutton_Make_Pairing_package_Click;
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
		((Control)wiButton_mini).Location = new Point(733, 22);
		((Control)wiButton_mini).Margin = new Padding(4, 5, 4, 5);
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
		((Control)wiButton_Close).Location = new Point(771, 22);
		((Control)wiButton_Close).Margin = new Padding(4, 5, 4, 5);
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
		((Control)wibutton_More).BackColor = Color.Transparent;
		((Control)wibutton_More).BackgroundImage = (Image)(object)Resources.button_1;
		((Control)wibutton_More).BackgroundImageLayout = (ImageLayout)3;
		wibutton_More.DisableBackColor = Color.Transparent;
		wibutton_More.DisableForeColor = Color.DarkGray;
		wibutton_More.DisableImage = (Image)(object)Resources.button_2;
		((Control)wibutton_More).Font = new Font("微软雅黑", 15f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wibutton_More.FrameMode = GraphicsHelper.RoundStyle.All;
		wibutton_More.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wibutton_More.IconName = "";
		wibutton_More.IconOffset = new Point(0, 0);
		wibutton_More.IconSize = 32;
		((Control)wibutton_More).Location = new Point(220, 271);
		((Control)wibutton_More).Margin = new Padding(4, 5, 4, 5);
		wibutton_More.MouseDownBackColor = Color.Gray;
		wibutton_More.MouseDownForeColor = Color.Black;
		wibutton_More.MouseDownImage = (Image)(object)Resources.点击状态1;
		wibutton_More.MouseEnterBackColor = Color.DarkGray;
		wibutton_More.MouseEnterForeColor = Color.Black;
		wibutton_More.MouseEnterImage = (Image)(object)Resources.点击状态1;
		wibutton_More.MouseUpBackColor = Color.Transparent;
		wibutton_More.MouseUpForeColor = Color.Black;
		wibutton_More.MouseUpImage = (Image)(object)Resources.默认状态1;
		((Control)wibutton_More).Name = "wibutton_More";
		wibutton_More.Radius = 12;
		((Control)wibutton_More).Size = new Size(200, 60);
		((Control)wibutton_More).TabIndex = 46;
		((Control)wibutton_More).Text = "More...";
		wibutton_More.TextAlignment = StringHelper.TextAlignment.Center;
		wibutton_More.TextDynOffset = new Point(0, 0);
		wibutton_More.TextFixLocation = new Point(0, 0);
		wibutton_More.TextFixLocationEnable = false;
		((Control)wibutton_More).Click += wibutton_More_Click;
		((Control)wibutton_Make_Upgrade_package).BackColor = Color.Transparent;
		((Control)wibutton_Make_Upgrade_package).BackgroundImage = (Image)(object)Resources.button_1;
		((Control)wibutton_Make_Upgrade_package).BackgroundImageLayout = (ImageLayout)3;
		wibutton_Make_Upgrade_package.DisableBackColor = Color.Transparent;
		wibutton_Make_Upgrade_package.DisableForeColor = Color.DarkGray;
		wibutton_Make_Upgrade_package.DisableImage = (Image)(object)Resources.button_2;
		((Control)wibutton_Make_Upgrade_package).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wibutton_Make_Upgrade_package.FrameMode = GraphicsHelper.RoundStyle.All;
		wibutton_Make_Upgrade_package.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wibutton_Make_Upgrade_package.IconName = "";
		wibutton_Make_Upgrade_package.IconOffset = new Point(0, 0);
		wibutton_Make_Upgrade_package.IconSize = 32;
		((Control)wibutton_Make_Upgrade_package).Location = new Point(220, 142);
		((Control)wibutton_Make_Upgrade_package).Margin = new Padding(4, 5, 4, 5);
		wibutton_Make_Upgrade_package.MouseDownBackColor = Color.Gray;
		wibutton_Make_Upgrade_package.MouseDownForeColor = Color.Black;
		wibutton_Make_Upgrade_package.MouseDownImage = (Image)(object)Resources.button_2;
		wibutton_Make_Upgrade_package.MouseEnterBackColor = Color.DarkGray;
		wibutton_Make_Upgrade_package.MouseEnterForeColor = Color.Black;
		wibutton_Make_Upgrade_package.MouseEnterImage = (Image)(object)Resources.点击状态2;
		wibutton_Make_Upgrade_package.MouseUpBackColor = Color.Transparent;
		wibutton_Make_Upgrade_package.MouseUpForeColor = Color.Black;
		wibutton_Make_Upgrade_package.MouseUpImage = (Image)(object)Resources.默认状态2;
		((Control)wibutton_Make_Upgrade_package).Name = "wibutton_Make_Upgrade_package";
		wibutton_Make_Upgrade_package.Radius = 12;
		((Control)wibutton_Make_Upgrade_package).Size = new Size(200, 60);
		((Control)wibutton_Make_Upgrade_package).TabIndex = 45;
		((Control)wibutton_Make_Upgrade_package).Text = "Make upgrade package";
		wibutton_Make_Upgrade_package.TextAlignment = StringHelper.TextAlignment.Center;
		wibutton_Make_Upgrade_package.TextDynOffset = new Point(0, 0);
		wibutton_Make_Upgrade_package.TextFixLocation = new Point(0, 0);
		wibutton_Make_Upgrade_package.TextFixLocationEnable = false;
		((Control)wibutton_Make_Upgrade_package).Click += wibutton_Make_Upgrade_package_Click;
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)0;
		((Control)this).BackgroundImage = (Image)(object)Resources.cidmid;
		((Form)this).ClientSize = new Size(817, 463);
		((Control)this).Controls.Add((Control)(object)wibutton_Make_Pairing_package);
		((Control)this).Controls.Add((Control)(object)wiButton_mini);
		((Control)this).Controls.Add((Control)(object)wiButton_Close);
		((Control)this).Controls.Add((Control)(object)wibutton_More);
		((Control)this).Controls.Add((Control)(object)wibutton_Make_Upgrade_package);
		((Control)this).DoubleBuffered = true;
		((Control)this).Font = new Font("微软雅黑", 10.5f);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Form)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "FormMore";
		((Form)this).StartPosition = (FormStartPosition)0;
		((Control)this).Text = "EditCidMid";
		((Control)this).ResumeLayout(false);
	}
}
