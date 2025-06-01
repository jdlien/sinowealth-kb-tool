using System;
using System.ComponentModel;
using System.Drawing;
using System.Timers;
using System.Windows.Forms;
using USBUpdateTool.Properties;
using USBUpdateTool.UpgradeFile;
using WindControls;

namespace USBUpdateTool;

public class FormMain : Form
{
	public AppLog appLog = new AppLog();

	public SkinForm skinForm = new SkinForm(_movable: true);

	public LoadBinFile loadBinFile = new LoadBinFile();

	public UpgradeManager upgradeManager;

	public FormMore formMore;

	public ICSelect iCSelect = new ICSelect();

	public FileHeader fileHeader = new FileHeader("");

	private VidPidManager vidPidManager = new VidPidManager();

	public static ResizeControl resizeControl = new ResizeControl();

	public Timer defaultTimer = new Timer();

	public BleOTAUpgrade bleOTAUpgrade = null;

	private PairTool pairTool;

	public Form4KPairing form4KPairing;

	private FormPairingSetting formPairingSetting;

	private LiteModeManager liteModeManager;

	private bool ctrlKeyDown = false;

	private IContainer components = null;

	public WindImageButton wiButton_Close;

	public WindImageButton wiButton_about;

	public Label label_caption;

	public WindImageButton wibutton_LoadFile;

	public WindImageButton wibutton_StartUpgrade;

	public Label label_ToolVersion;

	public Label label_newFwVersion;

	public Label label_curFwVersion;

	public Label label_tool_caption;

	public WindImageButton wIButton_Lite_Upgrade;

	public WindImageButton wiButton_mini;

	public WindImageButton wiButton_mini_lite;

	public WindImageButton wiButton_Close_lite;

	public WindCheckBox wCheckBox_IC_Enable;

	public Label label3;

	public WindTextBox wTextBox_fileName;

	public WindImageButton wImageButton_More;

	public EditValueControl editNormalVidPid;

	public EditValueControl editBootVidPid;

	public WindComboBox wcobBox_IC_type;

	public ImageBar imageBar1;

	public PictureBox pictureBox_Logo;

	public WindImageButton wIButton_4KPair;

	public WindImageButton wIButton_default;

	public WindImageButton wIButton_Setting;

	public WindCheckBox windCheckBox_autoDownLoad;

	public Label label_version;

	public FormMain()
	{
		InitializeComponent();
		SetStyles();
		imageBar1.Value = 0;
		wcobBox_IC_type.Clear();
		wcobBox_IC_type.Add(ICSelect.IC_ITEMs);
		wcobBox_IC_type.SetForm((Form)(object)this);
		loadBinFile.LoadLastFile(wTextBox_fileName);
		((Control)label_caption).Text = wcobBox_IC_type.SelectedText + "  Upgrade";
		AppLog.LogDeviceInfo();
		liteModeManager = new LiteModeManager();
		pairTool = new PairTool();
		((Form)this).Icon = LiteResources.GetIcon();
		liteModeManager.LoadLiteInformation(this);
		AppLog.LogConfigTXT(LiteResources.appConfig.configTxtList);
		iCSelect.customFile = iCSelect.CreateUpgradeFile(LiteResources.appConfig.fileConfig.fileName, LiteResources.appConfig.fileConfig.fileArray, LiteResources.appConfig.UpgradeDevice.icName, LiteResources.appConfig.UpgradeDevice.normalVid, LiteResources.appConfig.UpgradeDevice.normalPid, LiteResources.appConfig.UpgradeDevice.bootVid, LiteResources.appConfig.UpgradeDevice.bootPid);
		upgradeManager = new UpgradeManager(this);
		if (Screen.PrimaryScreen.Bounds.Width > 3600 || Screen.PrimaryScreen.Bounds.Height > 3600)
		{
			skinForm.InitSkin((Form)(object)this, 0, 0);
		}
		else
		{
			skinForm.InitSkin((Form)(object)this, 12, 16);
		}
		skinForm.AddControl((Control)(object)label3);
		InitTimer();
		ShowAutoUpgrade();
		((Control)label_version).Text = "";
	}

	public void InitTimer()
	{
		defaultTimer.Interval = 3000.0;
		defaultTimer.Elapsed += delegate
		{
			defaultTimer.Stop();
			new FormMessageBox("Failed to restore factory settings").Show((Form)(object)this);
		};
		defaultTimer.Stop();
	}

	private void SetStyles()
	{
		((Control)this).SetStyle((ControlStyles)204818, true);
		((Control)this).UpdateStyles();
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)0;
	}

	private void ShowAutoUpgrade()
	{
		((Control)windCheckBox_autoDownLoad).Visible = wcobBox_IC_type.SelectedText.Equals("BK3635");
	}

	public bool isAutoUpgrade()
	{
		if (((Control)windCheckBox_autoDownLoad).Visible)
		{
			return windCheckBox_autoDownLoad.Checked;
		}
		return false;
	}

	private void InitVidPid()
	{
		wcobBox_IC_type.Clear();
		wcobBox_IC_type.Add(ICSelect.IC_ITEMs);
		wcobBox_IC_type.SetSelectedText(FileManager.GetLastICType());
		vidPidManager.icName = wcobBox_IC_type.SelectedText;
		VidPidManager obj = vidPidManager;
		obj.OnNormalVidPidChangedEvent = (VidPidManager.VidPidChangedEvent)Delegate.Combine(obj.OnNormalVidPidChangedEvent, new VidPidManager.VidPidChangedEvent(StartFindDevices));
		VidPidManager obj2 = vidPidManager;
		obj2.OnBootVidPidChangedEvent = (VidPidManager.VidPidChangedEvent)Delegate.Combine(obj2.OnBootVidPidChangedEvent, new VidPidManager.VidPidChangedEvent(StartFindDevices));
		vidPidManager.SetEditValueControls(editNormalVidPid, editBootVidPid, null);
		vidPidManager.Init();
	}

	private void wiButton_Close_Click(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void wiButton_mini_Click(object sender, EventArgs e)
	{
		((Form)skinForm).WindowState = (FormWindowState)1;
		((Form)this).WindowState = (FormWindowState)1;
	}

	private void CreateDeviceFile()
	{
		iCSelect.CreateDeviceFile(((Control)wTextBox_fileName).Text, wcobBox_IC_type.SelectedText, editNormalVidPid.GetLeftValue(), editNormalVidPid.GetRightValue(), editBootVidPid.GetLeftValue(), editBootVidPid.GetRightValue());
	}

	public void StartFindDevices()
	{
		CreateDeviceFile();
		if (isOTAMode())
		{
			if (bleOTAUpgrade != null)
			{
				bleOTAUpgrade.StartBleDeviceChanged(enable: true, iCSelect);
			}
		}
		else if (upgradeManager != null)
		{
			upgradeManager.onUsbDeviceChanged(isUsbPluged: true);
		}
	}

	private void wcobBox_IC_type_SelectedIndexChanged(object sender, EventArgs e)
	{
	}

	private void wiButton_about_Click(object sender, EventArgs e)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		FormSetting formSetting = new FormSetting();
		formSetting.ShowDialog((Form)(object)this);
	}

	public void CheckUpgradeFile(string fileName)
	{
		fileHeader = new FileHeader(fileName);
		if (fileHeader.Valid)
		{
			wcobBox_IC_type.SetSelectedText(fileHeader.icName);
			editNormalVidPid.SetValue(isChecked: false, fileHeader.normalVid, fileHeader.normalPid);
			editBootVidPid.SetValue(isChecked: false, fileHeader.bootVid, fileHeader.bootPid);
			wCheckBox_IC_Enable.Enabled = false;
			wCheckBox_IC_Enable.Checked = true;
			((Control)wcobBox_IC_type).Enabled = false;
		}
		else
		{
			if (!wCheckBox_IC_Enable.Enabled)
			{
				wCheckBox_IC_Enable.Enabled = true;
				wCheckBox_IC_Enable.Checked = false;
			}
			wcobBox_IC_type.SetSelectedText(iCSelect.FindIcName(fileName));
		}
		((Control)label_caption).Text = wcobBox_IC_type.SelectedText + "  Upgrade";
	}

	private void wibutton_LoadFile_Click(object sender, EventArgs e)
	{
		if (loadBinFile.OpenFile(wTextBox_fileName))
		{
			CheckUpgradeFile(((Control)wTextBox_fileName).Text);
			ShowAutoUpgrade();
		}
	}

	private bool isOTAMode()
	{
		return wcobBox_IC_type.SelectedText.ToLower().Contains("ota");
	}

	private void wibutton_StartUpgrade_Click(object sender, EventArgs e)
	{
		CreateDeviceFile();
		if (LiteResources.appConfig.fileConfig.exeType == EXE_TYPE.PAIR)
		{
			pairTool.Start((Form)(object)this, iCSelect, upgradeManager.PairHandler);
		}
		else if (isOTAMode())
		{
			if (bleOTAUpgrade != null)
			{
				bleOTAUpgrade.StartOTA(iCSelect);
			}
		}
		else if (iCSelect.deviceFileList.Count > 0)
		{
			if (iCSelect.binFileTooLarge)
			{
				new FormMessageBox("Upgrade file too large").Show((Form)(object)this);
				return;
			}
			upgradeManager.UpgradeStart(iCSelect);
			((Control)label_version).Text = "当前设备版本：";
		}
	}

	private void FormMain_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Invalid comparison between Unknown and I4
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Invalid comparison between Unknown and I4
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 17)
		{
			ctrlKeyDown = true;
		}
		else if (((int)e.KeyCode == 13 || (int)e.KeyCode == 32) && ((Control)wIButton_Lite_Upgrade).Enabled)
		{
			wibutton_StartUpgrade_Click(null, null);
		}
	}

	public void StartUpgrade()
	{
		if (((Control)wibutton_StartUpgrade).Enabled)
		{
			wibutton_StartUpgrade_Click(null, null);
		}
	}

	private void FormMain_KeyUp(object sender, KeyEventArgs e)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 17)
		{
			ctrlKeyDown = false;
		}
	}

	private void FormMain_Load(object sender, EventArgs e)
	{
		liteModeManager.CheckUILocation(this);
		StartFindDevices();
		AppLog.LogControls((Form)(object)this);
	}

	private void wCheckBox_IC_Enable_CheckedChanged(object sender, EventArgs e)
	{
		if (wCheckBox_IC_Enable.Enabled)
		{
			((Control)wcobBox_IC_type).Enabled = wCheckBox_IC_Enable.Checked;
		}
	}

	private void wImageButton_More_Click(object sender, EventArgs e)
	{
	}

	private void wIButton_4KPair_Click(object sender, EventArgs e)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		if (form4KPairing == null)
		{
			form4KPairing = new Form4KPairing(this, 80);
		}
		form4KPairing.ShowDialog((Form)(object)this);
		upgradeManager.SetUsbChangedCallBack();
		if (upgradeManager != null)
		{
			upgradeManager.onUsbDeviceChanged(isUsbPluged: true);
		}
		form4KPairing = new Form4KPairing(this, 80);
	}

	private void wIButton_default_Click(object sender, EventArgs e)
	{
		if (LiteResources.appConfig.fileConfig.exeType == EXE_TYPE.PAIR && pairTool.DefaultSetting((Form)(object)this, iCSelect, upgradeManager.PairHandler))
		{
			defaultTimer.Start();
		}
	}

	private void wIButton_Setting_Click(object sender, EventArgs e)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		if (formPairingSetting == null)
		{
			formPairingSetting = new FormPairingSetting(this, 80);
		}
		formPairingSetting.ShowDialog((Form)(object)this);
		upgradeManager.SetUsbChangedCallBack();
		if (upgradeManager != null)
		{
			upgradeManager.onUsbDeviceChanged(isUsbPluged: true);
		}
		formPairingSetting = new FormPairingSetting(this, 80);
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
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Expected O, but got Unknown
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Expected O, but got Unknown
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dc: Expected O, but got Unknown
		//IL_0378: Unknown result type (might be due to invalid IL or missing references)
		//IL_0382: Expected O, but got Unknown
		//IL_041e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0428: Expected O, but got Unknown
		//IL_04cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d5: Expected O, but got Unknown
		//IL_06c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d1: Expected O, but got Unknown
		//IL_086e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0878: Expected O, but got Unknown
		//IL_0aa5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aaf: Expected O, but got Unknown
		//IL_0ced: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf7: Expected O, but got Unknown
		//IL_0fb2: Unknown result type (might be due to invalid IL or missing references)
		//IL_10c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_10d0: Expected O, but got Unknown
		//IL_1243: Unknown result type (might be due to invalid IL or missing references)
		//IL_135c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1366: Expected O, but got Unknown
		//IL_143b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1445: Expected O, but got Unknown
		//IL_1561: Unknown result type (might be due to invalid IL or missing references)
		//IL_156b: Expected O, but got Unknown
		//IL_1784: Unknown result type (might be due to invalid IL or missing references)
		//IL_178e: Expected O, but got Unknown
		//IL_18fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_1904: Expected O, but got Unknown
		//IL_1ad9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ae3: Expected O, but got Unknown
		//IL_1cb8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cc2: Expected O, but got Unknown
		//IL_1eca: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ed4: Expected O, but got Unknown
		//IL_2102: Unknown result type (might be due to invalid IL or missing references)
		//IL_210c: Expected O, but got Unknown
		//IL_2303: Unknown result type (might be due to invalid IL or missing references)
		//IL_230d: Expected O, but got Unknown
		//IL_234e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c91: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c9b: Expected O, but got Unknown
		//IL_2cb0: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cba: Expected O, but got Unknown
		//IL_2cc8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d18: Expected O, but got Unknown
		//IL_2d21: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d2b: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FormMain));
		label_caption = new Label();
		label_ToolVersion = new Label();
		label_newFwVersion = new Label();
		label_curFwVersion = new Label();
		label_tool_caption = new Label();
		label3 = new Label();
		pictureBox_Logo = new PictureBox();
		label_version = new Label();
		windCheckBox_autoDownLoad = new WindCheckBox();
		wIButton_Setting = new WindImageButton();
		wIButton_default = new WindImageButton();
		wIButton_4KPair = new WindImageButton();
		imageBar1 = new ImageBar();
		wcobBox_IC_type = new WindComboBox();
		editBootVidPid = new EditValueControl();
		editNormalVidPid = new EditValueControl();
		wImageButton_More = new WindImageButton();
		wCheckBox_IC_Enable = new WindCheckBox();
		wiButton_mini_lite = new WindImageButton();
		wiButton_Close_lite = new WindImageButton();
		wIButton_Lite_Upgrade = new WindImageButton();
		wibutton_StartUpgrade = new WindImageButton();
		wibutton_LoadFile = new WindImageButton();
		wTextBox_fileName = new WindTextBox();
		wiButton_about = new WindImageButton();
		wiButton_mini = new WindImageButton();
		wiButton_Close = new WindImageButton();
		((ISupportInitialize)pictureBox_Logo).BeginInit();
		((Control)this).SuspendLayout();
		((Control)label_caption).AutoSize = true;
		((Control)label_caption).BackColor = Color.Transparent;
		((Control)label_caption).Font = new Font("微软雅黑", 18f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)label_caption).ForeColor = Color.DimGray;
		((Control)label_caption).Location = new Point(329, 57);
		((Control)label_caption).Name = "label_caption";
		((Control)label_caption).Size = new Size(181, 31);
		((Control)label_caption).TabIndex = 15;
		((Control)label_caption).Text = "Upgrade v6.13";
		((Control)label_ToolVersion).AutoSize = true;
		((Control)label_ToolVersion).BackColor = Color.Transparent;
		((Control)label_ToolVersion).Font = new Font("微软雅黑", 9f);
		((Control)label_ToolVersion).Location = new Point(848, 320);
		((Control)label_ToolVersion).Name = "label_ToolVersion";
		((Control)label_ToolVersion).Size = new Size(93, 17);
		((Control)label_ToolVersion).TabIndex = 104;
		((Control)label_ToolVersion).Text = "Tool ver: v0.32";
		((Control)label_ToolVersion).Visible = false;
		((Control)label_newFwVersion).AutoSize = true;
		((Control)label_newFwVersion).BackColor = Color.Transparent;
		((Control)label_newFwVersion).Font = new Font("微软雅黑", 9f);
		((Control)label_newFwVersion).Location = new Point(848, 192);
		((Control)label_newFwVersion).Name = "label_newFwVersion";
		((Control)label_newFwVersion).Size = new Size(107, 17);
		((Control)label_newFwVersion).TabIndex = 103;
		((Control)label_newFwVersion).Text = "New FW Version:";
		((Control)label_newFwVersion).Visible = false;
		((Control)label_curFwVersion).AutoSize = true;
		((Control)label_curFwVersion).BackColor = Color.Transparent;
		((Control)label_curFwVersion).Font = new Font("微软雅黑", 9f);
		((Control)label_curFwVersion).Location = new Point(848, 167);
		((Control)label_curFwVersion).Name = "label_curFwVersion";
		((Control)label_curFwVersion).Size = new Size(124, 17);
		((Control)label_curFwVersion).TabIndex = 102;
		((Control)label_curFwVersion).Text = "Current FW Version:";
		((Control)label_curFwVersion).Visible = false;
		((Control)label_tool_caption).AutoSize = true;
		((Control)label_tool_caption).BackColor = Color.Transparent;
		((Control)label_tool_caption).Font = new Font("微软雅黑", 15f);
		((Control)label_tool_caption).Location = new Point(846, 108);
		((Control)label_tool_caption).Name = "label_tool_caption";
		((Control)label_tool_caption).Size = new Size(215, 27);
		((Control)label_tool_caption).TabIndex = 101;
		((Control)label_tool_caption).Text = "Mouse Upgrade Tool";
		((Control)label_tool_caption).Visible = false;
		((Control)label3).AutoSize = true;
		((Control)label3).BackColor = Color.Transparent;
		((Control)label3).Font = new Font("微软雅黑", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)label3).ForeColor = Color.White;
		((Control)label3).Location = new Point(148, 27);
		((Control)label3).Name = "label3";
		((Control)label3).Size = new Size(119, 21);
		((Control)label3).TabIndex = 121;
		((Control)label3).Text = "Tool ver: v6.13";
		((Control)pictureBox_Logo).BackColor = Color.Transparent;
		((Control)pictureBox_Logo).BackgroundImage = (Image)(object)Resources.PairLogo;
		((Control)pictureBox_Logo).BackgroundImageLayout = (ImageLayout)3;
		((Control)pictureBox_Logo).Location = new Point(835, 400);
		((Control)pictureBox_Logo).Name = "pictureBox_Logo";
		((Control)pictureBox_Logo).Size = new Size(132, 36);
		pictureBox_Logo.TabIndex = 151;
		pictureBox_Logo.TabStop = false;
		((Control)pictureBox_Logo).Visible = false;
		((Control)label_version).AutoSize = true;
		((Control)label_version).BackColor = Color.Transparent;
		((Control)label_version).Location = new Point(161, 416);
		((Control)label_version).Name = "label_version";
		((Control)label_version).Size = new Size(50, 20);
		((Control)label_version).TabIndex = 166;
		((Control)label_version).Text = "label1";
		((Control)windCheckBox_autoDownLoad).BackColor = Color.Transparent;
		((Control)windCheckBox_autoDownLoad).BackgroundImageLayout = (ImageLayout)0;
		windCheckBox_autoDownLoad.Checked = false;
		windCheckBox_autoDownLoad.DisableSelectedImage = (Image)(object)Resources.checkbox_3;
		windCheckBox_autoDownLoad.DisableUnSelectedImage = (Image)(object)Resources.checkbox_2;
		((Control)windCheckBox_autoDownLoad).Font = new Font("微软雅黑", 14.25f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		windCheckBox_autoDownLoad.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		windCheckBox_autoDownLoad.IconOffset = new Point(0, 0);
		windCheckBox_autoDownLoad.IconSize = 36;
		((Control)windCheckBox_autoDownLoad).Location = new Point(636, 402);
		((Control)windCheckBox_autoDownLoad).Name = "windCheckBox_autoDownLoad";
		windCheckBox_autoDownLoad.SelectedIconColor = Color.Red;
		windCheckBox_autoDownLoad.SelectedIconName = "";
		windCheckBox_autoDownLoad.SelectedImage = (Image)(object)Resources.checkbox_1;
		((Control)windCheckBox_autoDownLoad).Size = new Size(163, 30);
		((Control)windCheckBox_autoDownLoad).TabIndex = 164;
		((Control)windCheckBox_autoDownLoad).Text = " AutoUpgrade";
		windCheckBox_autoDownLoad.TextOffset = new Point(-8, 0);
		windCheckBox_autoDownLoad.UnSelectedIconColor = Color.Gray;
		windCheckBox_autoDownLoad.UnSelectedIconName = "";
		windCheckBox_autoDownLoad.UnSelectedImage = (Image)(object)Resources.checkbox_2;
		((Control)windCheckBox_autoDownLoad).Visible = false;
		((Control)wIButton_Setting).BackColor = Color.Transparent;
		((Control)wIButton_Setting).BackgroundImage = (Image)(object)Resources.button_1;
		((Control)wIButton_Setting).BackgroundImageLayout = (ImageLayout)3;
		wIButton_Setting.DisableBackColor = Color.Transparent;
		wIButton_Setting.DisableForeColor = Color.DarkGray;
		wIButton_Setting.DisableImage = (Image)(object)Resources.button_2;
		((Control)wIButton_Setting).Font = new Font("微软雅黑", 14.25f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wIButton_Setting.FrameMode = GraphicsHelper.RoundStyle.All;
		wIButton_Setting.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wIButton_Setting.IconName = "";
		wIButton_Setting.IconOffset = new Point(0, 0);
		wIButton_Setting.IconSize = 32;
		((Control)wIButton_Setting).Location = new Point(1179, 390);
		wIButton_Setting.MouseDownBackColor = Color.Gray;
		wIButton_Setting.MouseDownForeColor = Color.Black;
		wIButton_Setting.MouseDownImage = (Image)(object)Resources.高级设置2;
		wIButton_Setting.MouseEnterBackColor = Color.DarkGray;
		wIButton_Setting.MouseEnterForeColor = Color.Black;
		wIButton_Setting.MouseEnterImage = (Image)(object)Resources.高级设置1;
		wIButton_Setting.MouseUpBackColor = Color.Transparent;
		wIButton_Setting.MouseUpForeColor = Color.Black;
		wIButton_Setting.MouseUpImage = (Image)(object)Resources.高级设置;
		((Control)wIButton_Setting).Name = "wIButton_Setting";
		wIButton_Setting.Radius = 12;
		((Control)wIButton_Setting).Size = new Size(46, 46);
		((Control)wIButton_Setting).TabIndex = 162;
		wIButton_Setting.TextAlignment = StringHelper.TextAlignment.Center;
		wIButton_Setting.TextDynOffset = new Point(0, 0);
		wIButton_Setting.TextFixLocation = new Point(0, 0);
		wIButton_Setting.TextFixLocationEnable = false;
		((Control)wIButton_Setting).Visible = false;
		((Control)wIButton_Setting).Click += wIButton_Setting_Click;
		((Control)wIButton_default).BackColor = Color.Transparent;
		((Control)wIButton_default).BackgroundImage = (Image)(object)Resources.button_1;
		((Control)wIButton_default).BackgroundImageLayout = (ImageLayout)3;
		wIButton_default.DisableBackColor = Color.Transparent;
		wIButton_default.DisableForeColor = Color.DarkGray;
		wIButton_default.DisableImage = (Image)(object)Resources.button_2;
		((Control)wIButton_default).Font = new Font("微软雅黑", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wIButton_default.FrameMode = GraphicsHelper.RoundStyle.All;
		wIButton_default.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wIButton_default.IconName = "";
		wIButton_default.IconOffset = new Point(0, 0);
		wIButton_default.IconSize = 32;
		((Control)wIButton_default).Location = new Point(1083, 400);
		wIButton_default.MouseDownBackColor = Color.Gray;
		wIButton_default.MouseDownForeColor = Color.Tomato;
		wIButton_default.MouseDownImage = (Image)(object)Resources.defaultDown;
		wIButton_default.MouseEnterBackColor = Color.DarkGray;
		wIButton_default.MouseEnterForeColor = Color.Tomato;
		wIButton_default.MouseEnterImage = (Image)(object)Resources.defaultEnter;
		wIButton_default.MouseUpBackColor = Color.Transparent;
		wIButton_default.MouseUpForeColor = Color.Tomato;
		wIButton_default.MouseUpImage = (Image)(object)Resources.defaultNormal;
		((Control)wIButton_default).Name = "wIButton_default";
		wIButton_default.Radius = 12;
		((Control)wIButton_default).Size = new Size(90, 36);
		((Control)wIButton_default).TabIndex = 160;
		((Control)wIButton_default).Text = "恢复默认";
		wIButton_default.TextAlignment = StringHelper.TextAlignment.Center;
		wIButton_default.TextDynOffset = new Point(0, 0);
		wIButton_default.TextFixLocation = new Point(0, 0);
		wIButton_default.TextFixLocationEnable = false;
		((Control)wIButton_default).Visible = false;
		((Control)wIButton_default).Click += wIButton_default_Click;
		((Control)wIButton_4KPair).BackColor = Color.Transparent;
		((Control)wIButton_4KPair).BackgroundImage = (Image)(object)Resources.button_1;
		((Control)wIButton_4KPair).BackgroundImageLayout = (ImageLayout)3;
		wIButton_4KPair.DisableBackColor = Color.Transparent;
		wIButton_4KPair.DisableForeColor = Color.DarkGray;
		wIButton_4KPair.DisableImage = (Image)(object)Resources.button_2;
		((Control)wIButton_4KPair).Font = new Font("微软雅黑", 14.25f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wIButton_4KPair.FrameMode = GraphicsHelper.RoundStyle.All;
		wIButton_4KPair.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wIButton_4KPair.IconName = "";
		wIButton_4KPair.IconOffset = new Point(0, 0);
		wIButton_4KPair.IconSize = 32;
		((Control)wIButton_4KPair).Location = new Point(993, 400);
		wIButton_4KPair.MouseDownBackColor = Color.Gray;
		wIButton_4KPair.MouseDownForeColor = Color.Black;
		wIButton_4KPair.MouseDownImage = (Image)(object)Resources.Btn4KDown;
		wIButton_4KPair.MouseEnterBackColor = Color.DarkGray;
		wIButton_4KPair.MouseEnterForeColor = Color.Black;
		wIButton_4KPair.MouseEnterImage = (Image)(object)Resources.Btn4KEnter;
		wIButton_4KPair.MouseUpBackColor = Color.Transparent;
		wIButton_4KPair.MouseUpForeColor = Color.Black;
		wIButton_4KPair.MouseUpImage = (Image)(object)Resources.Btn4KNormal;
		((Control)wIButton_4KPair).Name = "wIButton_4KPair";
		wIButton_4KPair.Radius = 12;
		((Control)wIButton_4KPair).Size = new Size(68, 32);
		((Control)wIButton_4KPair).TabIndex = 153;
		wIButton_4KPair.TextAlignment = StringHelper.TextAlignment.Center;
		wIButton_4KPair.TextDynOffset = new Point(0, 0);
		wIButton_4KPair.TextFixLocation = new Point(0, 0);
		wIButton_4KPair.TextFixLocationEnable = false;
		((Control)wIButton_4KPair).Visible = false;
		((Control)wIButton_4KPair).Click += wIButton_4KPair_Click;
		((Control)imageBar1).BackColor = Color.Transparent;
		((Control)imageBar1).BackgroundImage = (Image)(object)Resources.进度条背景;
		((Control)imageBar1).BackgroundImageLayout = (ImageLayout)3;
		imageBar1.BarBackColor = Color.DarkGray;
		imageBar1.BarBackImage = (Image)(object)Resources.进度条背景;
		imageBar1.BarBackImageRect = new Rectangle(0, 0, 0, 0);
		imageBar1.BarForeColor = Color.RoyalBlue;
		imageBar1.BarForeImage = null;
		imageBar1.BarForeImageRect = new Rectangle(0, 0, 0, 0);
		imageBar1.BarSliderImage = null;
		imageBar1.BarSliderImageRect = new Rectangle(0, 0, 0, 0);
		imageBar1.BarSliderMovable = true;
		imageBar1.FrameWidth = 2;
		imageBar1.IntervalWidth = 4;
		((Control)imageBar1).Location = new Point(181, 192);
		((Control)imageBar1).Margin = new Padding(4, 5, 4, 5);
		imageBar1.MaxValue = 100;
		((Control)imageBar1).Name = "imageBar1";
		imageBar1.ShowPercent = true;
		((Control)imageBar1).Size = new Size(437, 31);
		imageBar1.StepImage = (Image)(object)Resources.进度条步进;
		((Control)imageBar1).TabIndex = 147;
		imageBar1.TextColor = Color.Black;
		imageBar1.TextLocation = new Point(0, 0);
		imageBar1.Value = 50;
		((Control)wcobBox_IC_type).BackgroundImage = (Image)(object)Resources.下拉正常状态;
		((Control)wcobBox_IC_type).BackgroundImageLayout = (ImageLayout)3;
		wcobBox_IC_type.DropDownLoctionOffset = new Point(-32, 0);
		wcobBox_IC_type.DropDownMaxRowCount = 8;
		wcobBox_IC_type.DropDownWidthDelta = 48;
		((Control)wcobBox_IC_type).Enabled = false;
		((Control)wcobBox_IC_type).Font = new Font("微软雅黑", 9f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)wcobBox_IC_type).ForeColor = Color.Green;
		wcobBox_IC_type.FrameColor = Color.Gray;
		wcobBox_IC_type.FrameWidth = 1;
		wcobBox_IC_type.IconMouseDownColor = Color.Gray;
		wcobBox_IC_type.IconMouseDownName = "";
		wcobBox_IC_type.IconMouseEnterColor = Color.DarkGray;
		wcobBox_IC_type.IconMouseEnterName = "";
		wcobBox_IC_type.IconNormalColor = Color.DimGray;
		wcobBox_IC_type.IconNormalName = "";
		wcobBox_IC_type.IconSize = 36;
		wcobBox_IC_type.Items.Add("CX52850");
		wcobBox_IC_type.Items.Add("CX52650");
		wcobBox_IC_type.Items.Add("CX52650N");
		wcobBox_IC_type.Items.Add("NRF52833");
		wcobBox_IC_type.Items.Add("NRF52840");
		wcobBox_IC_type.Items.Add("BK3635");
		wcobBox_IC_type.Items.Add("CX53710");
		wcobBox_IC_type.Items.Add("CH32V305FB");
		((Control)wcobBox_IC_type).Location = new Point(45, 57);
		((Control)wcobBox_IC_type).Margin = new Padding(4, 5, 4, 5);
		wcobBox_IC_type.MouseDownImage = (Image)(object)Resources.下拉鼠标点击状态;
		wcobBox_IC_type.MouseEnterImage = (Image)(object)Resources.下拉鼠标指过去状态;
		wcobBox_IC_type.MovingSelectedBackColor = Color.LightSkyBlue;
		((Control)wcobBox_IC_type).Name = "wcobBox_IC_type";
		wcobBox_IC_type.NormalImage = (Image)(object)Resources.下拉正常状态;
		wcobBox_IC_type.ReadOnly = true;
		((Control)wcobBox_IC_type).RightToLeft = (RightToLeft)0;
		wcobBox_IC_type.SelectedIndex = 0;
		wcobBox_IC_type.SelectedText = "BK3635";
		((Control)wcobBox_IC_type).Size = new Size(106, 26);
		wcobBox_IC_type.SplitLineColor = Color.LightGray;
		((Control)wcobBox_IC_type).TabIndex = 144;
		wcobBox_IC_type.SelectedIndexChanged += wcobBox_IC_type_SelectedIndexChanged;
		((Control)editBootVidPid).BackColor = Color.Transparent;
		editBootVidPid.CheckedText = "  Edit Boot VID/PID";
		((Control)editBootVidPid).Font = new Font("微软雅黑", 10.5f);
		editBootVidPid.isChecked = false;
		editBootVidPid.LeftLabelText = "VID";
		((Control)editBootVidPid).Location = new Point(174, 320);
		((Control)editBootVidPid).Name = "editBootVidPid";
		editBootVidPid.RightLabelText = "PID";
		((Control)editBootVidPid).Size = new Size(368, 77);
		((Control)editBootVidPid).TabIndex = 141;
		editBootVidPid.UnCheckedText = " Default Boot VID/PID";
		editBootVidPid.xUI = false;
		((Control)editNormalVidPid).BackColor = Color.Transparent;
		editNormalVidPid.CheckedText = "  Edit Normal VID/PID";
		((Control)editNormalVidPid).Font = new Font("微软雅黑", 10.5f);
		editNormalVidPid.isChecked = false;
		editNormalVidPid.LeftLabelText = "VID";
		((Control)editNormalVidPid).Location = new Point(174, 238);
		((Control)editNormalVidPid).Name = "editNormalVidPid";
		editNormalVidPid.RightLabelText = "PID";
		((Control)editNormalVidPid).Size = new Size(368, 77);
		((Control)editNormalVidPid).TabIndex = 140;
		editNormalVidPid.UnCheckedText = " Default Normal VID/PID";
		editNormalVidPid.xUI = false;
		((Control)wImageButton_More).BackColor = Color.Transparent;
		((Control)wImageButton_More).BackgroundImage = (Image)(object)Resources.button_1;
		((Control)wImageButton_More).BackgroundImageLayout = (ImageLayout)3;
		wImageButton_More.DisableBackColor = Color.Transparent;
		wImageButton_More.DisableForeColor = Color.DarkGray;
		wImageButton_More.DisableImage = (Image)(object)Resources.button_2;
		((Control)wImageButton_More).Font = new Font("微软雅黑", 14.25f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wImageButton_More.FrameMode = GraphicsHelper.RoundStyle.All;
		wImageButton_More.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wImageButton_More.IconName = "";
		wImageButton_More.IconOffset = new Point(0, 0);
		wImageButton_More.IconSize = 32;
		((Control)wImageButton_More).Location = new Point(22, 388);
		wImageButton_More.MouseDownBackColor = Color.Gray;
		wImageButton_More.MouseDownForeColor = Color.Black;
		wImageButton_More.MouseDownImage = (Image)(object)Resources.buttonMoreDown;
		wImageButton_More.MouseEnterBackColor = Color.DarkGray;
		wImageButton_More.MouseEnterForeColor = Color.Black;
		wImageButton_More.MouseEnterImage = (Image)(object)Resources.buttonMoreDown;
		wImageButton_More.MouseUpBackColor = Color.Transparent;
		wImageButton_More.MouseUpForeColor = Color.Black;
		wImageButton_More.MouseUpImage = (Image)(object)Resources.buttonMoreNormal;
		((Control)wImageButton_More).Name = "wImageButton_More";
		wImageButton_More.Radius = 12;
		((Control)wImageButton_More).Size = new Size(110, 48);
		((Control)wImageButton_More).TabIndex = 137;
		((Control)wImageButton_More).Text = "More...";
		wImageButton_More.TextAlignment = StringHelper.TextAlignment.Center;
		wImageButton_More.TextDynOffset = new Point(0, 0);
		wImageButton_More.TextFixLocation = new Point(0, 0);
		wImageButton_More.TextFixLocationEnable = false;
		((Control)wImageButton_More).Click += wImageButton_More_Click;
		((Control)wCheckBox_IC_Enable).BackColor = Color.Transparent;
		((Control)wCheckBox_IC_Enable).BackgroundImageLayout = (ImageLayout)0;
		wCheckBox_IC_Enable.Checked = false;
		wCheckBox_IC_Enable.DisableSelectedImage = (Image)(object)Resources.checkbox_3;
		wCheckBox_IC_Enable.DisableUnSelectedImage = (Image)(object)Resources.checkbox_2;
		((Control)wCheckBox_IC_Enable).Font = new Font("微软雅黑", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wCheckBox_IC_Enable.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wCheckBox_IC_Enable.IconOffset = new Point(0, 0);
		wCheckBox_IC_Enable.IconSize = 36;
		((Control)wCheckBox_IC_Enable).Location = new Point(10, 56);
		((Control)wCheckBox_IC_Enable).Name = "wCheckBox_IC_Enable";
		wCheckBox_IC_Enable.SelectedIconColor = Color.Red;
		wCheckBox_IC_Enable.SelectedIconName = "";
		wCheckBox_IC_Enable.SelectedImage = (Image)(object)Resources.checkbox_1;
		((Control)wCheckBox_IC_Enable).Size = new Size(31, 30);
		((Control)wCheckBox_IC_Enable).TabIndex = 119;
		((Control)wCheckBox_IC_Enable).Text = " ";
		wCheckBox_IC_Enable.TextOffset = new Point(4, 4);
		wCheckBox_IC_Enable.UnSelectedIconColor = Color.Gray;
		wCheckBox_IC_Enable.UnSelectedIconName = "";
		wCheckBox_IC_Enable.UnSelectedImage = (Image)(object)Resources.checkbox_2;
		wCheckBox_IC_Enable.CheckedChanged += wCheckBox_IC_Enable_CheckedChanged;
		((Control)wiButton_mini_lite).BackColor = Color.Transparent;
		wiButton_mini_lite.DisableBackColor = Color.Transparent;
		wiButton_mini_lite.DisableForeColor = Color.DarkGray;
		((Control)wiButton_mini_lite).Font = new Font("微软雅黑", 24f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wiButton_mini_lite.FrameMode = GraphicsHelper.RoundStyle.All;
		wiButton_mini_lite.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wiButton_mini_lite.IconName = "";
		wiButton_mini_lite.IconOffset = new Point(0, 0);
		wiButton_mini_lite.IconSize = 32;
		((Control)wiButton_mini_lite).Location = new Point(1336, 22);
		wiButton_mini_lite.MouseDownBackColor = Color.Transparent;
		wiButton_mini_lite.MouseDownForeColor = Color.Black;
		wiButton_mini_lite.MouseEnterBackColor = Color.Transparent;
		wiButton_mini_lite.MouseEnterForeColor = Color.Black;
		wiButton_mini_lite.MouseUpBackColor = Color.Transparent;
		wiButton_mini_lite.MouseUpForeColor = Color.Black;
		((Control)wiButton_mini_lite).Name = "wiButton_mini_lite";
		wiButton_mini_lite.Radius = 0;
		((Control)wiButton_mini_lite).Size = new Size(24, 24);
		((Control)wiButton_mini_lite).TabIndex = 108;
		((Control)wiButton_mini_lite).Text = "-";
		wiButton_mini_lite.TextAlignment = StringHelper.TextAlignment.Center;
		wiButton_mini_lite.TextDynOffset = new Point(1, 1);
		wiButton_mini_lite.TextFixLocation = new Point(0, 0);
		wiButton_mini_lite.TextFixLocationEnable = false;
		((Control)wiButton_mini_lite).Visible = false;
		((Control)wiButton_mini_lite).Click += wiButton_mini_Click;
		((Control)wiButton_Close_lite).BackColor = Color.Transparent;
		wiButton_Close_lite.DisableBackColor = Color.Transparent;
		wiButton_Close_lite.DisableForeColor = Color.DarkGray;
		((Control)wiButton_Close_lite).Font = new Font("微软雅黑", 24f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wiButton_Close_lite.FrameMode = GraphicsHelper.RoundStyle.All;
		wiButton_Close_lite.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wiButton_Close_lite.IconName = "";
		wiButton_Close_lite.IconOffset = new Point(0, 0);
		wiButton_Close_lite.IconSize = 32;
		((Control)wiButton_Close_lite).Location = new Point(1371, 22);
		wiButton_Close_lite.MouseDownBackColor = Color.Transparent;
		wiButton_Close_lite.MouseDownForeColor = Color.Black;
		wiButton_Close_lite.MouseEnterBackColor = Color.Transparent;
		wiButton_Close_lite.MouseEnterForeColor = Color.Black;
		wiButton_Close_lite.MouseUpBackColor = Color.Transparent;
		wiButton_Close_lite.MouseUpForeColor = Color.Black;
		((Control)wiButton_Close_lite).Name = "wiButton_Close_lite";
		wiButton_Close_lite.Radius = 0;
		((Control)wiButton_Close_lite).Size = new Size(24, 24);
		((Control)wiButton_Close_lite).TabIndex = 107;
		((Control)wiButton_Close_lite).Text = "×";
		wiButton_Close_lite.TextAlignment = StringHelper.TextAlignment.Center;
		wiButton_Close_lite.TextDynOffset = new Point(1, 1);
		wiButton_Close_lite.TextFixLocation = new Point(0, 0);
		wiButton_Close_lite.TextFixLocationEnable = false;
		((Control)wiButton_Close_lite).Visible = false;
		((Control)wiButton_Close_lite).Click += wiButton_Close_Click;
		((Control)wIButton_Lite_Upgrade).BackColor = Color.Transparent;
		wIButton_Lite_Upgrade.DisableBackColor = Color.Silver;
		wIButton_Lite_Upgrade.DisableForeColor = Color.DarkGray;
		((Control)wIButton_Lite_Upgrade).Font = new Font("微软雅黑", 14.25f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wIButton_Lite_Upgrade.FrameMode = GraphicsHelper.RoundStyle.All;
		wIButton_Lite_Upgrade.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wIButton_Lite_Upgrade.IconName = "";
		wIButton_Lite_Upgrade.IconOffset = new Point(0, 0);
		wIButton_Lite_Upgrade.IconSize = 32;
		((Control)wIButton_Lite_Upgrade).Location = new Point(1255, 233);
		wIButton_Lite_Upgrade.MouseDownBackColor = Color.MediumSeaGreen;
		wIButton_Lite_Upgrade.MouseDownForeColor = Color.Black;
		wIButton_Lite_Upgrade.MouseEnterBackColor = Color.LightSeaGreen;
		wIButton_Lite_Upgrade.MouseEnterForeColor = Color.Black;
		wIButton_Lite_Upgrade.MouseUpBackColor = Color.LightSeaGreen;
		wIButton_Lite_Upgrade.MouseUpForeColor = Color.Black;
		((Control)wIButton_Lite_Upgrade).Name = "wIButton_Lite_Upgrade";
		wIButton_Lite_Upgrade.Radius = 16;
		((Control)wIButton_Lite_Upgrade).Size = new Size(105, 46);
		((Control)wIButton_Lite_Upgrade).TabIndex = 105;
		((Control)wIButton_Lite_Upgrade).Text = "Upgrade";
		wIButton_Lite_Upgrade.TextAlignment = StringHelper.TextAlignment.Center;
		wIButton_Lite_Upgrade.TextDynOffset = new Point(1, 1);
		wIButton_Lite_Upgrade.TextFixLocation = new Point(0, 0);
		wIButton_Lite_Upgrade.TextFixLocationEnable = false;
		((Control)wIButton_Lite_Upgrade).Visible = false;
		((Control)wIButton_Lite_Upgrade).Click += wibutton_StartUpgrade_Click;
		((Control)wibutton_StartUpgrade).BackColor = Color.Transparent;
		((Control)wibutton_StartUpgrade).BackgroundImage = (Image)(object)Resources.button_1;
		((Control)wibutton_StartUpgrade).BackgroundImageLayout = (ImageLayout)3;
		wibutton_StartUpgrade.DisableBackColor = Color.Transparent;
		wibutton_StartUpgrade.DisableForeColor = Color.DarkGray;
		wibutton_StartUpgrade.DisableImage = (Image)(object)Resources.button_2;
		((Control)wibutton_StartUpgrade).Font = new Font("微软雅黑", 14.25f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wibutton_StartUpgrade.FrameMode = GraphicsHelper.RoundStyle.All;
		wibutton_StartUpgrade.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wibutton_StartUpgrade.IconName = "";
		wibutton_StartUpgrade.IconOffset = new Point(0, 0);
		wibutton_StartUpgrade.IconSize = 32;
		((Control)wibutton_StartUpgrade).Location = new Point(636, 254);
		wibutton_StartUpgrade.MouseDownBackColor = Color.Gray;
		wibutton_StartUpgrade.MouseDownForeColor = Color.Black;
		wibutton_StartUpgrade.MouseDownImage = (Image)(object)Resources.button_2;
		wibutton_StartUpgrade.MouseEnterBackColor = Color.DarkGray;
		wibutton_StartUpgrade.MouseEnterForeColor = Color.Black;
		wibutton_StartUpgrade.MouseEnterImage = (Image)(object)Resources.button_2;
		wibutton_StartUpgrade.MouseUpBackColor = Color.Transparent;
		wibutton_StartUpgrade.MouseUpForeColor = Color.Black;
		wibutton_StartUpgrade.MouseUpImage = (Image)(object)Resources.button_1;
		((Control)wibutton_StartUpgrade).Name = "wibutton_StartUpgrade";
		wibutton_StartUpgrade.Radius = 12;
		((Control)wibutton_StartUpgrade).Size = new Size(114, 60);
		((Control)wibutton_StartUpgrade).TabIndex = 26;
		((Control)wibutton_StartUpgrade).Text = "Upgrade";
		wibutton_StartUpgrade.TextAlignment = StringHelper.TextAlignment.Center;
		wibutton_StartUpgrade.TextDynOffset = new Point(0, 0);
		wibutton_StartUpgrade.TextFixLocation = new Point(0, 0);
		wibutton_StartUpgrade.TextFixLocationEnable = false;
		((Control)wibutton_StartUpgrade).Click += wibutton_StartUpgrade_Click;
		((Control)wibutton_LoadFile).BackColor = Color.Transparent;
		((Control)wibutton_LoadFile).BackgroundImage = (Image)(object)Resources.buttonSmall_1;
		((Control)wibutton_LoadFile).BackgroundImageLayout = (ImageLayout)3;
		wibutton_LoadFile.DisableBackColor = Color.Transparent;
		wibutton_LoadFile.DisableForeColor = Color.DarkGray;
		wibutton_LoadFile.DisableImage = (Image)(object)Resources.buttonSmall_2;
		((Control)wibutton_LoadFile).Font = new Font("微软雅黑", 14.25f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wibutton_LoadFile.FrameMode = GraphicsHelper.RoundStyle.All;
		wibutton_LoadFile.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wibutton_LoadFile.IconName = "";
		wibutton_LoadFile.IconOffset = new Point(0, 0);
		wibutton_LoadFile.IconSize = 32;
		((Control)wibutton_LoadFile).Location = new Point(636, 144);
		wibutton_LoadFile.MouseDownBackColor = Color.Gray;
		wibutton_LoadFile.MouseDownForeColor = Color.Black;
		wibutton_LoadFile.MouseDownImage = (Image)(object)Resources.buttonSmall_2;
		wibutton_LoadFile.MouseEnterBackColor = Color.DarkGray;
		wibutton_LoadFile.MouseEnterForeColor = Color.Black;
		wibutton_LoadFile.MouseEnterImage = (Image)(object)Resources.buttonSmall_2;
		wibutton_LoadFile.MouseUpBackColor = Color.Transparent;
		wibutton_LoadFile.MouseUpForeColor = Color.Black;
		wibutton_LoadFile.MouseUpImage = (Image)(object)Resources.buttonSmall_1;
		((Control)wibutton_LoadFile).Name = "wibutton_LoadFile";
		wibutton_LoadFile.Radius = 12;
		((Control)wibutton_LoadFile).Size = new Size(114, 42);
		((Control)wibutton_LoadFile).TabIndex = 25;
		((Control)wibutton_LoadFile).Text = "Load File";
		wibutton_LoadFile.TextAlignment = StringHelper.TextAlignment.Center;
		wibutton_LoadFile.TextDynOffset = new Point(0, 0);
		wibutton_LoadFile.TextFixLocation = new Point(0, 0);
		wibutton_LoadFile.TextFixLocationEnable = false;
		((Control)wibutton_LoadFile).Click += wibutton_LoadFile_Click;
		((Control)wTextBox_fileName).BackgroundImage = (Image)(object)Resources.选中文件框;
		((Control)wTextBox_fileName).BackgroundImageLayout = (ImageLayout)3;
		wTextBox_fileName.DisableBackImage = null;
		((Control)wTextBox_fileName).Font = new Font("微软雅黑", 15f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wTextBox_fileName.FrameColor = Color.White;
		wTextBox_fileName.InputMode = WindBaseTextBox.TextBoxInputMode.Normal;
		((Control)wTextBox_fileName).Location = new Point(181, 144);
		((Control)wTextBox_fileName).Margin = new Padding(6);
		wTextBox_fileName.MaxLength = int.MaxValue;
		((Control)wTextBox_fileName).Name = "wTextBox_fileName";
		wTextBox_fileName.PasswordChar = "";
		wTextBox_fileName.PromptText = "";
		wTextBox_fileName.PromptTextColor = Color.Gray;
		wTextBox_fileName.PromptTextForeColor = Color.LightGray;
		wTextBox_fileName.ReadOnly = true;
		wTextBox_fileName.SelectedBackImage = (Image)(object)Resources.选中文件框;
		((Control)wTextBox_fileName).Size = new Size(437, 40);
		((Control)wTextBox_fileName).TabIndex = 16;
		wTextBox_fileName.TextBoxOffset = new Point(6, 6);
		wTextBox_fileName.UnSelectedBackImage = (Image)(object)Resources.选中文件框;
		((Control)wiButton_about).BackColor = Color.Transparent;
		((Control)wiButton_about).BackgroundImage = (Image)(object)Resources.setting_2;
		((Control)wiButton_about).BackgroundImageLayout = (ImageLayout)2;
		wiButton_about.DisableBackColor = Color.Transparent;
		wiButton_about.DisableForeColor = Color.OrangeRed;
		wiButton_about.DisableImage = (Image)(object)Resources.setting_1;
		wiButton_about.FrameMode = GraphicsHelper.RoundStyle.All;
		wiButton_about.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wiButton_about.IconName = "";
		wiButton_about.IconOffset = new Point(0, 0);
		wiButton_about.IconSize = 32;
		((Control)wiButton_about).Location = new Point(719, 22);
		wiButton_about.MouseDownBackColor = Color.Gray;
		wiButton_about.MouseDownForeColor = Color.DarkRed;
		wiButton_about.MouseDownImage = (Image)(object)Resources.setting_1;
		wiButton_about.MouseEnterBackColor = Color.DarkGray;
		wiButton_about.MouseEnterForeColor = Color.OrangeRed;
		wiButton_about.MouseEnterImage = (Image)(object)Resources.setting_1;
		wiButton_about.MouseUpBackColor = Color.Transparent;
		wiButton_about.MouseUpForeColor = Color.Red;
		wiButton_about.MouseUpImage = (Image)(object)Resources.setting_2;
		((Control)wiButton_about).Name = "wiButton_about";
		wiButton_about.Radius = 12;
		((Control)wiButton_about).Size = new Size(18, 18);
		((Control)wiButton_about).TabIndex = 2;
		((Control)wiButton_about).Text = null;
		wiButton_about.TextAlignment = StringHelper.TextAlignment.Center;
		wiButton_about.TextDynOffset = new Point(0, 0);
		wiButton_about.TextFixLocation = new Point(0, 0);
		wiButton_about.TextFixLocationEnable = false;
		((Control)wiButton_about).Click += wiButton_about_Click;
		((Control)wiButton_mini).BackColor = Color.Transparent;
		((Control)wiButton_mini).BackgroundImage = (Image)(object)Resources.MiniButtonNormalImage;
		((Control)wiButton_mini).BackgroundImageLayout = (ImageLayout)2;
		wiButton_mini.DisableBackColor = Color.Transparent;
		wiButton_mini.DisableForeColor = Color.OrangeRed;
		wiButton_mini.DisableImage = (Image)(object)Resources.MiniButtonMouseDownImage;
		wiButton_mini.FrameMode = GraphicsHelper.RoundStyle.All;
		wiButton_mini.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wiButton_mini.IconName = "";
		wiButton_mini.IconOffset = new Point(0, 0);
		wiButton_mini.IconSize = 32;
		((Control)wiButton_mini).Location = new Point(748, 22);
		wiButton_mini.MouseDownBackColor = Color.Gray;
		wiButton_mini.MouseDownForeColor = Color.DarkRed;
		wiButton_mini.MouseDownImage = (Image)(object)Resources.MiniButtonMouseDownImage;
		wiButton_mini.MouseEnterBackColor = Color.DarkGray;
		wiButton_mini.MouseEnterForeColor = Color.OrangeRed;
		wiButton_mini.MouseEnterImage = (Image)(object)Resources.MiniButtonMouseEnterImage;
		wiButton_mini.MouseUpBackColor = Color.Transparent;
		wiButton_mini.MouseUpForeColor = Color.Red;
		wiButton_mini.MouseUpImage = (Image)(object)Resources.MiniButtonNormalImage;
		((Control)wiButton_mini).Name = "wiButton_mini";
		wiButton_mini.Radius = 12;
		((Control)wiButton_mini).Size = new Size(18, 18);
		((Control)wiButton_mini).TabIndex = 1;
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
		wiButton_Close.DisableForeColor = Color.OrangeRed;
		wiButton_Close.DisableImage = (Image)(object)Resources.close_3;
		wiButton_Close.FrameMode = GraphicsHelper.RoundStyle.All;
		wiButton_Close.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wiButton_Close.IconName = "";
		wiButton_Close.IconOffset = new Point(0, 0);
		wiButton_Close.IconSize = 32;
		((Control)wiButton_Close).Location = new Point(777, 22);
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
		((Control)wiButton_Close).TabIndex = 0;
		((Control)wiButton_Close).Text = null;
		wiButton_Close.TextAlignment = StringHelper.TextAlignment.Center;
		wiButton_Close.TextDynOffset = new Point(0, 0);
		wiButton_Close.TextFixLocation = new Point(0, 0);
		wiButton_Close.TextFixLocationEnable = false;
		((Control)wiButton_Close).Click += wiButton_Close_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackgroundImage = (Image)(object)Resources.MainBackImage;
		((Form)this).ClientSize = new Size(1430, 562);
		((Control)this).Controls.Add((Control)(object)label_version);
		((Control)this).Controls.Add((Control)(object)windCheckBox_autoDownLoad);
		((Control)this).Controls.Add((Control)(object)wIButton_Setting);
		((Control)this).Controls.Add((Control)(object)wIButton_default);
		((Control)this).Controls.Add((Control)(object)wIButton_4KPair);
		((Control)this).Controls.Add((Control)(object)pictureBox_Logo);
		((Control)this).Controls.Add((Control)(object)imageBar1);
		((Control)this).Controls.Add((Control)(object)wcobBox_IC_type);
		((Control)this).Controls.Add((Control)(object)editBootVidPid);
		((Control)this).Controls.Add((Control)(object)editNormalVidPid);
		((Control)this).Controls.Add((Control)(object)wImageButton_More);
		((Control)this).Controls.Add((Control)(object)label3);
		((Control)this).Controls.Add((Control)(object)wCheckBox_IC_Enable);
		((Control)this).Controls.Add((Control)(object)wiButton_mini_lite);
		((Control)this).Controls.Add((Control)(object)wiButton_Close_lite);
		((Control)this).Controls.Add((Control)(object)wIButton_Lite_Upgrade);
		((Control)this).Controls.Add((Control)(object)label_ToolVersion);
		((Control)this).Controls.Add((Control)(object)label_newFwVersion);
		((Control)this).Controls.Add((Control)(object)label_curFwVersion);
		((Control)this).Controls.Add((Control)(object)label_tool_caption);
		((Control)this).Controls.Add((Control)(object)wibutton_StartUpgrade);
		((Control)this).Controls.Add((Control)(object)wibutton_LoadFile);
		((Control)this).Controls.Add((Control)(object)wTextBox_fileName);
		((Control)this).Controls.Add((Control)(object)label_caption);
		((Control)this).Controls.Add((Control)(object)wiButton_about);
		((Control)this).Controls.Add((Control)(object)wiButton_mini);
		((Control)this).Controls.Add((Control)(object)wiButton_Close);
		((Control)this).DoubleBuffered = true;
		((Control)this).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Form)this).KeyPreview = true;
		((Form)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "FormMain";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "UpdateTool";
		((Form)this).Load += FormMain_Load;
		((Control)this).KeyDown += new KeyEventHandler(FormMain_KeyDown);
		((Control)this).KeyUp += new KeyEventHandler(FormMain_KeyUp);
		((ISupportInitialize)pictureBox_Logo).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
