using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using USBUpdateTool.Properties;
using WindControls;

namespace USBUpdateTool;

public class FormVidPidSetting : Form
{
	public SkinForm skinForm = new SkinForm(_movable: true);

	private Form form;

	private List<DeviceConfig> deviceVidOnlyConfigs;

	private List<DeviceConfig> deviceVidPidConfigs;

	private OpenFileDialog openFileDialog = new OpenFileDialog();

	private IContainer components = null;

	private WindImageButton wiButton_mini;

	private WindImageButton wiButton_Close;

	private WindImageButton wiButton_OK;

	private VidPidControl vidPidControl1;

	private VidPidControl vidPidControl2;

	private WindImageButton wiButton_loadVidPid;

	public FormVidPidSetting(List<DeviceConfig> _deviceVidOnlyConfigs, List<DeviceConfig> _deviceVidPidConfigs)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		InitializeComponent();
		SetStyles();
		skinForm.InitSkin((Form)(object)this, 12, 16);
		SetVidPid(_deviceVidOnlyConfigs, _deviceVidPidConfigs);
		InitDialog();
	}

	private void SetStyles()
	{
		((Control)this).SetStyle((ControlStyles)204818, true);
		((Control)this).UpdateStyles();
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)0;
	}

	private void InitDialog()
	{
		((FileDialog)openFileDialog).Title = "Open File";
		((FileDialog)openFileDialog).Filter = "EXE Files|*.exe";
		((FileDialog)openFileDialog).FilterIndex = 1;
		openFileDialog.Multiselect = false;
		((FileDialog)openFileDialog).RestoreDirectory = true;
	}

	public void SetVidPid(List<DeviceConfig> _deviceVidOnlyConfigs, List<DeviceConfig> _deviceVidPidConfigs)
	{
		deviceVidOnlyConfigs = _deviceVidOnlyConfigs;
		deviceVidPidConfigs = _deviceVidPidConfigs;
		vidPidControl1.SetVidPid(deviceVidOnlyConfigs);
		vidPidControl2.SetVidPid(deviceVidPidConfigs);
	}

	private void wiButton_mini_Click(object sender, EventArgs e)
	{
		((Form)skinForm).WindowState = (FormWindowState)1;
		((Form)this).WindowState = (FormWindowState)1;
	}

	private void wiButton_Close_Click(object sender, EventArgs e)
	{
		skinForm.AllHide();
		form.Activate();
	}

	public void ShowWindow(Form _form)
	{
		form = _form;
		skinForm.AllShow(_form);
	}

	private void wiButton_OK_Click(object sender, EventArgs e)
	{
		List<DeviceConfig> vidPidList = vidPidControl1.GetVidPidList();
		List<DeviceConfig> vidPidList2 = vidPidControl2.GetVidPidList();
		if (vidPidList2 == null)
		{
			new FormMessageBox("Vid Pid Error").Show((Form)(object)this);
			return;
		}
		((FormMakeMultiPairTool)(object)form).GetVidPidList(vidPidList, vidPidList2);
		wiButton_Close_Click(null, null);
	}

	private void wiButton_loadVidPid_Click(object sender, EventArgs e)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Invalid comparison between Unknown and I4
		if ((int)((CommonDialog)openFileDialog).ShowDialog() == 1)
		{
			AppConfig appConfig = new AppConfig();
			LoadExeResource loadExeResource = new LoadExeResource();
			loadExeResource.Load(((FileDialog)openFileDialog).FileName, ref appConfig);
			SetVidPid(appConfig.PairVidOnlyDevices, appConfig.PairVidPidDevices);
		}
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
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Expected O, but got Unknown
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Expected O, but got Unknown
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Expected O, but got Unknown
		//IL_07d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07dd: Expected O, but got Unknown
		//IL_0a11: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a1b: Expected O, but got Unknown
		//IL_0a30: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a3a: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FormVidPidSetting));
		vidPidControl2 = new VidPidControl();
		vidPidControl1 = new VidPidControl();
		wiButton_OK = new WindImageButton();
		wiButton_mini = new WindImageButton();
		wiButton_Close = new WindImageButton();
		wiButton_loadVidPid = new WindImageButton();
		((Control)this).SuspendLayout();
		((Control)vidPidControl2).Font = new Font("微软雅黑", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)vidPidControl2).Location = new Point(373, 52);
		((Control)vidPidControl2).Name = "vidPidControl2";
		vidPidControl2.PidVisable = true;
		((Control)vidPidControl2).Size = new Size(362, 448);
		((Control)vidPidControl2).TabIndex = 164;
		vidPidControl2.TipText = "4K接收器";
		((Control)vidPidControl1).Font = new Font("微软雅黑", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)vidPidControl1).Location = new Point(13, 52);
		((Control)vidPidControl1).Name = "vidPidControl1";
		vidPidControl1.PidVisable = true;
		((Control)vidPidControl1).Size = new Size(354, 448);
		((Control)vidPidControl1).TabIndex = 163;
		vidPidControl1.TipText = "普通接收器";
		((Control)wiButton_OK).BackColor = Color.Transparent;
		wiButton_OK.DisableBackColor = Color.DarkGray;
		wiButton_OK.DisableForeColor = Color.DimGray;
		((Control)wiButton_OK).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wiButton_OK.FrameMode = GraphicsHelper.RoundStyle.All;
		wiButton_OK.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wiButton_OK.IconName = "";
		wiButton_OK.IconOffset = new Point(0, 0);
		wiButton_OK.IconSize = 32;
		((Control)wiButton_OK).Location = new Point(743, 416);
		wiButton_OK.MouseDownBackColor = Color.MediumTurquoise;
		wiButton_OK.MouseDownForeColor = Color.Black;
		wiButton_OK.MouseEnterBackColor = Color.Turquoise;
		wiButton_OK.MouseEnterForeColor = Color.Black;
		wiButton_OK.MouseUpBackColor = Color.LightSeaGreen;
		wiButton_OK.MouseUpForeColor = Color.Black;
		((Control)wiButton_OK).Name = "wiButton_OK";
		wiButton_OK.Radius = 16;
		((Control)wiButton_OK).Size = new Size(65, 84);
		((Control)wiButton_OK).TabIndex = 162;
		((Control)wiButton_OK).Tag = "0";
		((Control)wiButton_OK).Text = "确定";
		wiButton_OK.TextAlignment = StringHelper.TextAlignment.Center;
		wiButton_OK.TextDynOffset = new Point(0, 0);
		wiButton_OK.TextFixLocation = new Point(0, 0);
		wiButton_OK.TextFixLocationEnable = false;
		((Control)wiButton_OK).Click += wiButton_OK_Click;
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
		((Control)wiButton_loadVidPid).BackColor = Color.Transparent;
		wiButton_loadVidPid.DisableBackColor = Color.DarkGray;
		wiButton_loadVidPid.DisableForeColor = Color.DimGray;
		((Control)wiButton_loadVidPid).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wiButton_loadVidPid.FrameMode = GraphicsHelper.RoundStyle.All;
		wiButton_loadVidPid.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wiButton_loadVidPid.IconName = "";
		wiButton_loadVidPid.IconOffset = new Point(0, 0);
		wiButton_loadVidPid.IconSize = 32;
		((Control)wiButton_loadVidPid).Location = new Point(741, 52);
		wiButton_loadVidPid.MouseDownBackColor = Color.MediumTurquoise;
		wiButton_loadVidPid.MouseDownForeColor = Color.Black;
		wiButton_loadVidPid.MouseEnterBackColor = Color.Turquoise;
		wiButton_loadVidPid.MouseEnterForeColor = Color.Black;
		wiButton_loadVidPid.MouseUpBackColor = Color.LightSeaGreen;
		wiButton_loadVidPid.MouseUpForeColor = Color.Black;
		((Control)wiButton_loadVidPid).Name = "wiButton_loadVidPid";
		wiButton_loadVidPid.Radius = 16;
		((Control)wiButton_loadVidPid).Size = new Size(65, 42);
		((Control)wiButton_loadVidPid).TabIndex = 165;
		((Control)wiButton_loadVidPid).Tag = "0";
		((Control)wiButton_loadVidPid).Text = "加载列表";
		wiButton_loadVidPid.TextAlignment = StringHelper.TextAlignment.Center;
		wiButton_loadVidPid.TextDynOffset = new Point(0, 0);
		wiButton_loadVidPid.TextFixLocation = new Point(0, 0);
		wiButton_loadVidPid.TextFixLocationEnable = false;
		((Control)wiButton_loadVidPid).Click += wiButton_loadVidPid_Click;
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)0;
		((Control)this).BackgroundImage = (Image)(object)Resources.pairBack;
		((Form)this).ClientSize = new Size(816, 517);
		((Control)this).Controls.Add((Control)(object)wiButton_loadVidPid);
		((Control)this).Controls.Add((Control)(object)vidPidControl2);
		((Control)this).Controls.Add((Control)(object)vidPidControl1);
		((Control)this).Controls.Add((Control)(object)wiButton_OK);
		((Control)this).Controls.Add((Control)(object)wiButton_mini);
		((Control)this).Controls.Add((Control)(object)wiButton_Close);
		((Control)this).Font = new Font("微软雅黑", 10.5f);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Control)this).Name = "FormVidPidSetting";
		((Form)this).StartPosition = (FormStartPosition)0;
		((Control)this).Text = "EditCidMid";
		((Control)this).ResumeLayout(false);
	}
}
