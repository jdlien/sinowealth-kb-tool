using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Timers;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using DriverLib;
using USBUpdateTool.Properties;
using WindControls;

namespace USBUpdateTool;

public class Form4KPairing : Form
{
	private const int FrameReduce = 12;

	public Form4KPairBack form4KPairBack = new Form4KPairBack();

	public SkinForm skinForm = new SkinForm(_movable: true);

	private PairTool pairTool = new PairTool();

	private FormMain formMain;

	private ICSelect iCSelect = new ICSelect();

	private Point parentOffsetPoint = default(Point);

	private Timer switch4KTimer = new Timer();

	private Timer defaultTimer = new Timer();

	public bool isStarted = false;

	private IContainer components = null;

	public PictureBox pictureBox_Logo;

	public Label label_ToolVersion;

	public WindImageButton wiButton_Close_lite;

	public Label label_tool_caption;

	public WindImageButton wibutton_StartUpgrade;

	private WindRadioButton windRadioButton_4K;

	private WindRadioButton windRadioButton_1K;

	public WindImageButton wIButton_default;

	public Form4KPairing(FormMain _formMain, int scaleValue)
	{
		formMain = _formMain;
		InitializeComponent();
		SetStyles();
		pairTool.isFindPid = true;
		((Control)this).BackgroundImage = ((Control)formMain).BackgroundImage;
		((Control)pictureBox_Logo).Location = ((Control)formMain.pictureBox_Logo).Location;
		((Control)pictureBox_Logo).BackgroundImage = ((Control)formMain.pictureBox_Logo).BackgroundImage;
		CloneButton(wibutton_StartUpgrade, formMain.wIButton_Lite_Upgrade);
		CloneButton(wIButton_default, formMain.wIButton_default);
		WindImageButton windImageButton = wIButton_default;
		((Control)windImageButton).Width = ((Control)windImageButton).Width + 16;
		Point location = new Point(((Control)formMain.wiButton_Close_lite).Location.X - 12, ((Control)formMain.wiButton_Close_lite).Location.Y);
		((Control)wiButton_Close_lite).Location = location;
		wiButton_Close_lite.SetMouseForeColor(formMain.wiButton_Close_lite.MouseUpForeColor);
		((Control)label_tool_caption).Location = ((Control)formMain.label_tool_caption).Location;
		((Control)label_tool_caption).Width = ((Control)formMain.label_tool_caption).Width;
		((Control)label_tool_caption).Height = ((Control)formMain.label_tool_caption).Height;
		((Control)label_tool_caption).Font = ((Control)formMain.label_tool_caption).Font;
		((Control)label_tool_caption).ForeColor = ((Control)formMain.label_tool_caption).ForeColor;
		((Control)label_ToolVersion).Location = ((Control)formMain.label_ToolVersion).Location;
		((Control)label_ToolVersion).Width = ((Control)formMain.label_ToolVersion).Width;
		((Control)label_ToolVersion).Height = ((Control)formMain.label_ToolVersion).Height;
		((Control)label_ToolVersion).Text = ((Control)formMain.label_ToolVersion).Text;
		((Control)label_ToolVersion).Font = ((Control)formMain.label_ToolVersion).Font;
		((Control)label_ToolVersion).ForeColor = ((Control)formMain.label_ToolVersion).ForeColor;
		if (scaleValue != 100)
		{
			ScaleWindow(scaleValue);
		}
		skinForm.InitSkin((Form)(object)this, 12, 16);
		skinForm.AddControl((Control)(object)pictureBox_Logo);
		skinForm.AddControl((Control)(object)label_tool_caption);
		skinForm.AddControl((Control)(object)label_ToolVersion);
		InitTimer();
		iCSelect.CreatePairDeviceFile(LiteResources.appConfig.PairVidPidDevices);
	}

	private void CloneButton(WindImageButton dstButton, WindImageButton srcButton)
	{
		((Control)dstButton).Enabled = ((Control)srcButton).Enabled;
		((Control)dstButton).Location = ((Control)srcButton).Location;
		((Control)dstButton).Width = ((Control)srcButton).Width;
		((Control)dstButton).Height = ((Control)srcButton).Height;
		((Control)dstButton).ForeColor = ((Control)srcButton).ForeColor;
		dstButton.MouseUpImage = srcButton.MouseUpImage;
		dstButton.MouseUpForeColor = srcButton.MouseUpForeColor;
		dstButton.MouseUpBackColor = srcButton.MouseUpBackColor;
		dstButton.MouseEnterImage = srcButton.MouseEnterImage;
		dstButton.MouseEnterForeColor = srcButton.MouseEnterForeColor;
		dstButton.MouseEnterBackColor = srcButton.MouseEnterBackColor;
		dstButton.MouseDownImage = srcButton.MouseDownImage;
		dstButton.MouseDownForeColor = srcButton.MouseDownForeColor;
		dstButton.MouseDownBackColor = srcButton.MouseDownBackColor;
		dstButton.DisableForeColor = srcButton.DisableForeColor;
		dstButton.DisableBackColor = srcButton.DisableBackColor;
		dstButton.DisableImage = srcButton.DisableImage;
	}

	private void SetStyles()
	{
		((Control)this).SetStyle((ControlStyles)204818, true);
		((Control)this).UpdateStyles();
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)0;
	}

	public void InitTimer()
	{
		switch4KTimer.Interval = 600.0;
		switch4KTimer.Elapsed += delegate
		{
			switch4KTimer.Stop();
			FlashDataMap writeFlashDataMap = new FlashDataMap(0);
			FlashDataMap compareFlashDataMap = new FlashDataMap(0);
			if (windRadioButton_4K.Checked)
			{
				compareFlashDataMap.mouseConfig.reportRate = 1;
				writeFlashDataMap.mouseConfig.reportRate = 32;
			}
			else
			{
				compareFlashDataMap.mouseConfig.reportRate = 32;
				writeFlashDataMap.mouseConfig.reportRate = 1;
			}
			UsbServer.ProtocolDataCompareUpdate(writeFlashDataMap, compareFlashDataMap);
		};
		switch4KTimer.Stop();
		defaultTimer.Interval = 3000.0;
		defaultTimer.Elapsed += delegate
		{
			defaultTimer.Stop();
			new FormMessageBox("Failed to restore factory settings").Show((Form)(object)this);
		};
		defaultTimer.Stop();
	}

	public void onUsbDataReceived(UsbCommand command)
	{
		switch ((UsbCommandID)command.id)
		{
		case UsbCommandID.WriteFlashData:
			if (windRadioButton_4K.Checked)
			{
				if (command.receivedData[0] == 32)
				{
					((Control)wibutton_StartUpgrade).Text = "Pair Success";
				}
			}
			else if (command.receivedData[0] == 1)
			{
				((Control)wibutton_StartUpgrade).Text = "Pair Success";
			}
			break;
		case UsbCommandID.ClearSetting:
			defaultTimer.Stop();
			new FormMessageBox("Successfully restored factory settings").Show((Form)(object)this);
			break;
		}
	}

	private int GetScaleValue(int srcValue, int scaleValue)
	{
		return srcValue * scaleValue / 100;
	}

	private void ScaleWindow(int scaleValue)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected O, but got Unknown
		((Control)this).BackgroundImage = (Image)(object)GraphicsHelper.ScaleToSize(size: new Size(GetScaleValue(((Control)this).BackgroundImage.Width, scaleValue), GetScaleValue(((Control)this).BackgroundImage.Height, scaleValue)), bitmap: new Bitmap(((Control)this).BackgroundImage));
		foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
		{
			Control val = item;
			Point location = new Point(GetScaleValue(val.Location.X, scaleValue), GetScaleValue(val.Location.Y, scaleValue));
			if (location.X < 12)
			{
				location.X = 12;
			}
			if (location.Y < 12)
			{
				location.Y = 12;
			}
			val.Location = location;
			if (val.Name != "wiButton_Close_lite")
			{
				val.Width = GetScaleValue(val.Width, scaleValue);
				val.Height = GetScaleValue(val.Height, scaleValue);
			}
		}
	}

	private void wiButton_Close_Click(object sender, EventArgs e)
	{
		((Form)this).DialogResult = (DialogResult)2;
	}

	public DialogResult ShowDialog(Form Owner)
	{
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		form4KPairBack.Show(Owner);
		((Form)skinForm).Owner = (Form)(object)form4KPairBack;
		int x = Owner.Location.X + (((Control)Owner).Width - ((Control)this).Width) / 2;
		int y = Owner.Location.Y + (((Control)Owner).Height - ((Control)this).Height) / 2;
		((Form)this).Location = new Point(x, y);
		UsbFinder.SetUsbChangedCallBack(OnUsbChangedEvent, 600);
		DialogResult val = ((Form)this).ShowDialog();
		Owner.Activate();
		return (DialogResult)1;
	}

	public static List<string[]> FindHidDevicesEndPoint(List<string[]> endpointList)
	{
		List<string[]> list = new List<string[]>();
		for (int i = 0; i < endpointList.Count; i++)
		{
			string[] array = UsbFinder.FindHidDevices(endpointList[i][0]);
			string[] array2 = UsbFinder.FindHidDevices(endpointList[i][1]);
			if (array != null && array2 != null && array.Length == array2.Length)
			{
				for (int j = 0; j < array.Length; j++)
				{
					list.Add(new string[2]
					{
						array[j],
						array2[j]
					});
				}
			}
		}
		return list;
	}

	public bool FindDevice(DeviceFile deviceFile)
	{
		List<string[]> list = new List<string[]>();
		list = FindHidDevicesEndPoint(deviceFile.normalEndPoints);
		return list.Count > 0;
	}

	private void OnUsbChangedEvent(bool isPlug)
	{
		((Control)this).Invoke((Delegate)(Action)delegate
		{
			int num = -1;
			for (int i = 0; i < iCSelect.deviceFileList.Count; i++)
			{
				if (FindDevice(iCSelect.deviceFileList[i]))
				{
					num = i;
					if (!pairTool.isStarted)
					{
						((Control)wibutton_StartUpgrade).Enabled = true;
						((Control)wIButton_default).Enabled = true;
						break;
					}
				}
			}
			if (num == -1)
			{
				((Control)wibutton_StartUpgrade).Enabled = false;
				((Control)wIButton_default).Enabled = false;
			}
		});
	}

	private void FormPasswordInput_FormClosing(object sender, FormClosingEventArgs e)
	{
		((Form)form4KPairBack).Close();
	}

	private void wibutton_confirm_Click(object sender, EventArgs e)
	{
	}

	private void wiButton_Close_lite_Click(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void Form4KPairing_LocationChanged(object sender, EventArgs e)
	{
		if (parentOffsetPoint.X != 0)
		{
			((Form)formMain).Location = new Point(((Form)this).Location.X - parentOffsetPoint.X, ((Form)this).Location.Y - parentOffsetPoint.Y);
			form4KPairBack.SetLoction(((Form)formMain).Location);
		}
	}

	private void Form4KPairing_Load(object sender, EventArgs e)
	{
		parentOffsetPoint.X = ((Form)this).Location.X - ((Form)formMain).Location.X;
		parentOffsetPoint.Y = ((Form)this).Location.Y - ((Form)formMain).Location.Y;
		OnUsbChangedEvent(isPlug: true);
	}

	private void wibutton_StartUpgrade_Click(object sender, EventArgs e)
	{
		pairTool.Start((Form)(object)this, iCSelect, PairHandler);
	}

	public void PairHandler(UsbCommand command, string displayText)
	{
		((Control)formMain).Invoke((Delegate)(Action)delegate
		{
			switch ((UsbCommandID)command.id)
			{
			case UsbCommandID.DongleEnterPair:
				((Control)wibutton_StartUpgrade).Text = displayText;
				((Control)wiButton_Close_lite).Enabled = false;
				((Control)wibutton_StartUpgrade).Enabled = false;
				wibutton_StartUpgrade.TextFixLocation = wibutton_StartUpgrade.textPoint;
				wibutton_StartUpgrade.TextFixLocationEnable = true;
				windRadioButton_4K.Enabled = false;
				windRadioButton_1K.Enabled = false;
				break;
			case UsbCommandID.GetPairState:
				if (command.receivedData[0] == 1)
				{
					((Control)wibutton_StartUpgrade).Text = displayText;
				}
				else if (command.receivedData[0] == 2)
				{
					((Control)wiButton_Close_lite).Enabled = true;
					((Control)wibutton_StartUpgrade).Enabled = true;
					((Control)wibutton_StartUpgrade).Text = displayText;
					windRadioButton_4K.Enabled = true;
					windRadioButton_1K.Enabled = true;
					wibutton_StartUpgrade.TextFixLocationEnable = false;
				}
				else if (command.receivedData[0] == 3)
				{
					wibutton_StartUpgrade.TextFixLocationEnable = false;
					((Control)wiButton_Close_lite).Enabled = true;
					((Control)wibutton_StartUpgrade).Enabled = true;
					windRadioButton_4K.Enabled = true;
					windRadioButton_1K.Enabled = true;
					if (StartDriver())
					{
						switch4KTimer.Start();
					}
				}
				break;
			case UsbCommandID.StatusChanged:
				((Control)wiButton_Close_lite).Enabled = true;
				((Control)wibutton_StartUpgrade).Text = displayText;
				windRadioButton_4K.Enabled = true;
				windRadioButton_1K.Enabled = true;
				wibutton_StartUpgrade.TextFixLocationEnable = false;
				break;
			}
		});
	}

	private bool StartDriver()
	{
		int deviceId = 0;
		List<string> list = pairTool.FindDongleCount(iCSelect, driverPairMode: true, ref deviceId);
		if (list.Count == 1)
		{
			UsbServer.Start(list[0], list[0], onUsbDataReceived);
			return true;
		}
		return false;
	}

	private void Form4KPairing_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Invalid comparison between Unknown and I4
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Invalid comparison between Unknown and I4
		if (((int)e.KeyCode == 13 || (int)e.KeyCode == 32) && ((Control)wibutton_StartUpgrade).Enabled)
		{
			wibutton_StartUpgrade_Click(null, null);
		}
	}

	private void Form4KPairing_Activated(object sender, EventArgs e)
	{
	}

	private void Form4KPairing_Deactivate(object sender, EventArgs e)
	{
	}

	private void Form4KPairing_MouseDown(object sender, MouseEventArgs e)
	{
	}

	private void wIButton_default_Click(object sender, EventArgs e)
	{
		if (StartDriver())
		{
			UsbServer.SetClearSetting();
			defaultTimer.Start();
		}
	}

	private void wcheck_LongRange_CheckedChanged(object sender, EventArgs e)
	{
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
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Expected O, but got Unknown
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Expected O, but got Unknown
		//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02eb: Expected O, but got Unknown
		//IL_0504: Unknown result type (might be due to invalid IL or missing references)
		//IL_050e: Expected O, but got Unknown
		//IL_0681: Unknown result type (might be due to invalid IL or missing references)
		//IL_068b: Expected O, but got Unknown
		//IL_07e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ee: Expected O, but got Unknown
		//IL_09c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d0: Expected O, but got Unknown
		//IL_0c70: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c7a: Expected O, but got Unknown
		//IL_0c8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c99: Expected O, but got Unknown
		//IL_0ca7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ced: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf7: Expected O, but got Unknown
		//IL_0d26: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d30: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(Form4KPairing));
		pictureBox_Logo = new PictureBox();
		label_ToolVersion = new Label();
		label_tool_caption = new Label();
		wIButton_default = new WindImageButton();
		windRadioButton_1K = new WindRadioButton();
		windRadioButton_4K = new WindRadioButton();
		wiButton_Close_lite = new WindImageButton();
		wibutton_StartUpgrade = new WindImageButton();
		((ISupportInitialize)pictureBox_Logo).BeginInit();
		((Control)this).SuspendLayout();
		((Control)pictureBox_Logo).BackColor = Color.Transparent;
		((Control)pictureBox_Logo).BackgroundImage = (Image)(object)Resources.PairLogo;
		((Control)pictureBox_Logo).BackgroundImageLayout = (ImageLayout)3;
		((Control)pictureBox_Logo).Location = new Point(26, 12);
		((Control)pictureBox_Logo).Name = "pictureBox_Logo";
		((Control)pictureBox_Logo).Size = new Size(132, 36);
		pictureBox_Logo.TabIndex = 152;
		pictureBox_Logo.TabStop = false;
		((Control)label_ToolVersion).AutoSize = true;
		((Control)label_ToolVersion).BackColor = Color.Transparent;
		((Control)label_ToolVersion).Font = new Font("微软雅黑", 9f);
		((Control)label_ToolVersion).ForeColor = Color.White;
		((Control)label_ToolVersion).Location = new Point(173, 31);
		((Control)label_ToolVersion).Name = "label_ToolVersion";
		((Control)label_ToolVersion).Size = new Size(93, 17);
		((Control)label_ToolVersion).TabIndex = 156;
		((Control)label_ToolVersion).Text = "Tool ver: v0.32";
		((Control)label_tool_caption).AutoSize = true;
		((Control)label_tool_caption).BackColor = Color.Transparent;
		((Control)label_tool_caption).Font = new Font("微软雅黑", 15f);
		((Control)label_tool_caption).ForeColor = Color.LightSalmon;
		((Control)label_tool_caption).Location = new Point(48, 80);
		((Control)label_tool_caption).Name = "label_tool_caption";
		((Control)label_tool_caption).Size = new Size(310, 27);
		((Control)label_tool_caption).TabIndex = 159;
		((Control)label_tool_caption).Text = "4K dongle配对软件（样机专用）";
		((Control)wIButton_default).BackColor = Color.Transparent;
		((Control)wIButton_default).BackgroundImage = (Image)(object)Resources.button_1;
		((Control)wIButton_default).BackgroundImageLayout = (ImageLayout)3;
		wIButton_default.DisableBackColor = Color.Transparent;
		wIButton_default.DisableForeColor = Color.DarkGray;
		wIButton_default.DisableImage = (Image)(object)Resources.button_2;
		((Control)wIButton_default).Enabled = false;
		((Control)wIButton_default).Font = new Font("微软雅黑", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wIButton_default.FrameMode = GraphicsHelper.RoundStyle.All;
		wIButton_default.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wIButton_default.IconName = "";
		wIButton_default.IconOffset = new Point(0, 0);
		wIButton_default.IconSize = 32;
		((Control)wIButton_default).Location = new Point(40, 266);
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
		((Control)wIButton_default).TabIndex = 204;
		((Control)wIButton_default).Text = "恢复默认";
		wIButton_default.TextAlignment = StringHelper.TextAlignment.Center;
		wIButton_default.TextDynOffset = new Point(0, 0);
		wIButton_default.TextFixLocation = new Point(0, 0);
		wIButton_default.TextFixLocationEnable = false;
		((Control)wIButton_default).Click += wIButton_default_Click;
		((Control)windRadioButton_1K).BackColor = Color.Transparent;
		((Control)windRadioButton_1K).BackgroundImageLayout = (ImageLayout)0;
		windRadioButton_1K.Checked = false;
		windRadioButton_1K.DisableSelectedImage = (Image)(object)Resources.CheckBoxDisState;
		windRadioButton_1K.DisableUnSelectedImage = (Image)(object)Resources.CheckBoxUnState;
		((Control)windRadioButton_1K).Font = new Font("微软雅黑", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)windRadioButton_1K).ForeColor = Color.White;
		windRadioButton_1K.IconOffset = new Point(0, 0);
		windRadioButton_1K.IconSize = 36;
		((Control)windRadioButton_1K).Location = new Point(434, 266);
		((Control)windRadioButton_1K).Name = "windRadioButton_1K";
		windRadioButton_1K.SelectedIconColor = Color.LightSalmon;
		windRadioButton_1K.SelectedIconName = "";
		windRadioButton_1K.SelectedImage = (Image)(object)Resources.CheckBoxState;
		((Control)windRadioButton_1K).Size = new Size(125, 39);
		((Control)windRadioButton_1K).TabIndex = 163;
		((Control)windRadioButton_1K).Text = "1K 回报率";
		windRadioButton_1K.TextOffset = new Point(4, 0);
		windRadioButton_1K.UnSelectedIconColor = Color.Gray;
		windRadioButton_1K.UnSelectedIconName = "";
		windRadioButton_1K.UnSelectedImage = (Image)(object)Resources.CheckBoxUnState;
		((Control)windRadioButton_4K).BackColor = Color.Transparent;
		((Control)windRadioButton_4K).BackgroundImageLayout = (ImageLayout)0;
		windRadioButton_4K.Checked = true;
		windRadioButton_4K.DisableSelectedImage = (Image)(object)Resources.CheckBoxDisState;
		windRadioButton_4K.DisableUnSelectedImage = (Image)(object)Resources.CheckBoxUnState;
		((Control)windRadioButton_4K).Font = new Font("微软雅黑", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)windRadioButton_4K).ForeColor = Color.White;
		windRadioButton_4K.IconOffset = new Point(0, 0);
		windRadioButton_4K.IconSize = 36;
		((Control)windRadioButton_4K).Location = new Point(434, 221);
		((Control)windRadioButton_4K).Name = "windRadioButton_4K";
		windRadioButton_4K.SelectedIconColor = Color.LightSalmon;
		windRadioButton_4K.SelectedIconName = "";
		windRadioButton_4K.SelectedImage = (Image)(object)Resources.CheckBoxState;
		((Control)windRadioButton_4K).Size = new Size(125, 39);
		((Control)windRadioButton_4K).TabIndex = 162;
		((Control)windRadioButton_4K).Text = "4K 回报率";
		windRadioButton_4K.TextOffset = new Point(4, 0);
		windRadioButton_4K.UnSelectedIconColor = Color.Gray;
		windRadioButton_4K.UnSelectedIconName = "";
		windRadioButton_4K.UnSelectedImage = (Image)(object)Resources.CheckBoxUnState;
		((Control)wiButton_Close_lite).BackColor = Color.Transparent;
		wiButton_Close_lite.DisableBackColor = Color.Transparent;
		wiButton_Close_lite.DisableForeColor = Color.DarkGray;
		((Control)wiButton_Close_lite).Font = new Font("微软雅黑", 24f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wiButton_Close_lite.FrameMode = GraphicsHelper.RoundStyle.All;
		wiButton_Close_lite.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wiButton_Close_lite.IconName = "";
		wiButton_Close_lite.IconOffset = new Point(0, 0);
		wiButton_Close_lite.IconSize = 32;
		((Control)wiButton_Close_lite).Location = new Point(556, 24);
		wiButton_Close_lite.MouseDownBackColor = Color.Transparent;
		wiButton_Close_lite.MouseDownForeColor = Color.White;
		wiButton_Close_lite.MouseEnterBackColor = Color.Transparent;
		wiButton_Close_lite.MouseEnterForeColor = Color.White;
		wiButton_Close_lite.MouseUpBackColor = Color.Transparent;
		wiButton_Close_lite.MouseUpForeColor = Color.White;
		((Control)wiButton_Close_lite).Name = "wiButton_Close_lite";
		wiButton_Close_lite.Radius = 0;
		((Control)wiButton_Close_lite).Size = new Size(24, 24);
		((Control)wiButton_Close_lite).TabIndex = 158;
		((Control)wiButton_Close_lite).Text = "×";
		wiButton_Close_lite.TextAlignment = StringHelper.TextAlignment.Center;
		wiButton_Close_lite.TextDynOffset = new Point(1, 1);
		wiButton_Close_lite.TextFixLocation = new Point(0, 0);
		wiButton_Close_lite.TextFixLocationEnable = false;
		((Control)wiButton_Close_lite).Click += wiButton_Close_lite_Click;
		((Control)wibutton_StartUpgrade).BackColor = Color.Transparent;
		wibutton_StartUpgrade.DisableBackColor = Color.DarkGray;
		wibutton_StartUpgrade.DisableForeColor = Color.DimGray;
		((Control)wibutton_StartUpgrade).Enabled = false;
		((Control)wibutton_StartUpgrade).Font = new Font("微软雅黑", 14.25f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wibutton_StartUpgrade.FrameMode = GraphicsHelper.RoundStyle.All;
		wibutton_StartUpgrade.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wibutton_StartUpgrade.IconName = "";
		wibutton_StartUpgrade.IconOffset = new Point(0, 0);
		wibutton_StartUpgrade.IconSize = 32;
		((Control)wibutton_StartUpgrade).Location = new Point(176, 153);
		wibutton_StartUpgrade.MouseDownBackColor = Color.LightSeaGreen;
		wibutton_StartUpgrade.MouseDownForeColor = Color.Tomato;
		wibutton_StartUpgrade.MouseDownImage = (Image)(object)Resources.Pair点击状态;
		wibutton_StartUpgrade.MouseEnterBackColor = Color.LightSeaGreen;
		wibutton_StartUpgrade.MouseEnterForeColor = Color.Tomato;
		wibutton_StartUpgrade.MouseEnterImage = (Image)(object)Resources.Pair鼠标指过去;
		wibutton_StartUpgrade.MouseUpBackColor = Color.LightSeaGreen;
		wibutton_StartUpgrade.MouseUpForeColor = Color.Tomato;
		wibutton_StartUpgrade.MouseUpImage = (Image)(object)Resources.Pair默认;
		((Control)wibutton_StartUpgrade).Name = "wibutton_StartUpgrade";
		wibutton_StartUpgrade.Radius = 16;
		((Control)wibutton_StartUpgrade).Size = new Size(212, 38);
		((Control)wibutton_StartUpgrade).TabIndex = 153;
		((Control)wibutton_StartUpgrade).Tag = "4";
		((Control)wibutton_StartUpgrade).Text = "Pair";
		wibutton_StartUpgrade.TextAlignment = StringHelper.TextAlignment.Center;
		wibutton_StartUpgrade.TextDynOffset = new Point(1, 1);
		wibutton_StartUpgrade.TextFixLocation = new Point(0, 0);
		wibutton_StartUpgrade.TextFixLocationEnable = false;
		((Control)wibutton_StartUpgrade).Click += wibutton_StartUpgrade_Click;
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)0;
		((Control)this).BackgroundImage = (Image)(object)Resources.pairBackImage;
		((Form)this).ClientSize = new Size(606, 344);
		((Control)this).Controls.Add((Control)(object)wIButton_default);
		((Control)this).Controls.Add((Control)(object)windRadioButton_1K);
		((Control)this).Controls.Add((Control)(object)windRadioButton_4K);
		((Control)this).Controls.Add((Control)(object)label_tool_caption);
		((Control)this).Controls.Add((Control)(object)wiButton_Close_lite);
		((Control)this).Controls.Add((Control)(object)wibutton_StartUpgrade);
		((Control)this).Controls.Add((Control)(object)label_ToolVersion);
		((Control)this).Controls.Add((Control)(object)pictureBox_Logo);
		((Control)this).DoubleBuffered = true;
		((Control)this).Font = new Font("微软雅黑", 15f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Form)this).KeyPreview = true;
		((Form)this).Margin = new Padding(6, 7, 6, 7);
		((Control)this).Name = "Form4KPairing";
		((Form)this).StartPosition = (FormStartPosition)0;
		((Control)this).Text = "Input Password";
		((Form)this).Activated += Form4KPairing_Activated;
		((Form)this).FormClosing += new FormClosingEventHandler(FormPasswordInput_FormClosing);
		((Form)this).Load += Form4KPairing_Load;
		((Control)this).LocationChanged += Form4KPairing_LocationChanged;
		((Control)this).KeyDown += new KeyEventHandler(Form4KPairing_KeyDown);
		((ISupportInitialize)pictureBox_Logo).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
