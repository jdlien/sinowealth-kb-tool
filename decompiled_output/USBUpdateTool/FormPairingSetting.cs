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

public class FormPairingSetting : Form
{
	private const int FrameReduce = 12;

	public Form4KPairBack form4KPairBack = new Form4KPairBack();

	public SkinForm skinForm = new SkinForm(_movable: true);

	private PairTool pairTool = new PairTool();

	private Point parentOffsetPoint = default(Point);

	private FormMain formMain;

	private ICSelect iCSelect = new ICSelect();

	private Timer defaultTimer = new Timer();

	private IContainer components = null;

	public PictureBox pictureBox_Logo;

	public WindImageButton wiButton_Close_lite;

	private SwitchButton switchButton1;

	private Label label_foundDevices;

	public FormPairingSetting(FormMain _formMain, int scaleValue)
	{
		formMain = _formMain;
		InitializeComponent();
		((Control)label_foundDevices).Text = "";
		SetStyles();
		((Control)this).BackgroundImage = ((Control)formMain).BackgroundImage;
		((Control)pictureBox_Logo).Location = ((Control)formMain.pictureBox_Logo).Location;
		((Control)pictureBox_Logo).Width = ((Control)formMain.pictureBox_Logo).Width;
		((Control)pictureBox_Logo).Height = ((Control)formMain.pictureBox_Logo).Height;
		((Control)pictureBox_Logo).BackgroundImage = ((Control)formMain.pictureBox_Logo).BackgroundImage;
		Point location = new Point(((Control)formMain.wiButton_Close_lite).Location.X - 12, ((Control)formMain.wiButton_Close_lite).Location.Y);
		((Control)wiButton_Close_lite).Location = location;
		wiButton_Close_lite.SetMouseForeColor(formMain.wiButton_Close_lite.MouseUpForeColor);
		if (scaleValue != 100)
		{
			ScaleWindow(scaleValue);
		}
		skinForm.InitSkin((Form)(object)this, 12, 16);
		skinForm.AddControl((Control)(object)pictureBox_Logo);
		InitTimer();
		iCSelect.CreatePairDeviceFile(LiteResources.appConfig.PairVidOnlyDevices);
		switchButton1.SetStatus(SwitchButtonStatus.Disable, Color.Gray);
	}

	private void SetStyles()
	{
		((Control)this).SetStyle((ControlStyles)204818, true);
		((Control)this).UpdateStyles();
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)0;
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

	private int GetScaleValue(int srcValue, int scaleValue)
	{
		return srcValue * scaleValue / 100;
	}

	private void ScaleWindow(int scaleValue)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
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
		list = UpgradeManager.FindHidDevicesEndPointOnlyVid(deviceFile.normalVid, "");
		return list.Count > 0;
	}

	private void OnUsbChangedEvent(bool isPlug)
	{
		((Control)this).Invoke((Delegate)(Action)delegate
		{
			int num = 0;
			for (int i = 0; i < iCSelect.deviceFileList.Count; i++)
			{
				if (FindDevice(iCSelect.deviceFileList[i]))
				{
					num++;
				}
			}
			((Control)label_foundDevices).Text = "发现" + num + "个设备";
			if (num == 1)
			{
				if (!StartDriver())
				{
					Label obj = label_foundDevices;
					((Control)obj).Text = ((Control)obj).Text + "，操作失败";
				}
			}
			else
			{
				switchButton1.SetStatus(SwitchButtonStatus.Disable, Color.Gray);
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

	private void Form4KPairing_Load(object sender, EventArgs e)
	{
	}

	public void onUsbDataReceived(UsbCommand command)
	{
		switch ((UsbCommandID)command.id)
		{
		case UsbCommandID.SetLongRangeMode:
		case UsbCommandID.GetLongRangeMode:
			if (command.receivedData == null)
			{
				break;
			}
			((Control)this).Invoke((Delegate)(Action)delegate
			{
				if (command.receivedData[0] == 1)
				{
					switchButton1.SetStatus(SwitchButtonStatus.ON, Color.Tomato);
				}
				else
				{
					switchButton1.SetStatus(SwitchButtonStatus.OFF, Color.White);
				}
			});
			break;
		case UsbCommandID.ClearSetting:
			defaultTimer.Stop();
			new FormMessageBox("Successfully restored factory settings").Show((Form)(object)this);
			break;
		}
	}

	private bool StartDriver()
	{
		int deviceId = 0;
		List<string> list = pairTool.FindDongleCount(iCSelect, driverPairMode: true, ref deviceId);
		if (list.Count == 1)
		{
			UsbServer.Start(list[0], list[0], onUsbDataReceived);
			UsbServer.GetLongRangeMode();
			return true;
		}
		return false;
	}

	private void FormPairingSetting_LocationChanged(object sender, EventArgs e)
	{
		if (parentOffsetPoint.X != 0)
		{
			((Form)formMain).Location = new Point(((Form)this).Location.X - parentOffsetPoint.X, ((Form)this).Location.Y - parentOffsetPoint.Y);
			form4KPairBack.SetLoction(((Form)formMain).Location);
		}
	}

	private void FormPairingSetting_Load(object sender, EventArgs e)
	{
		parentOffsetPoint.X = ((Form)this).Location.X - ((Form)formMain).Location.X;
		parentOffsetPoint.Y = ((Form)this).Location.Y - ((Form)formMain).Location.Y;
		OnUsbChangedEvent(isPlug: true);
	}

	private void switchButton1_Click(object sender, EventArgs e)
	{
		UsbServer.SetLongRangeMode(switchButton1.GetSwitchButtonStatus() != SwitchButtonStatus.ON);
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
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Expected O, but got Unknown
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Expected O, but got Unknown
		//IL_04e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ec: Expected O, but got Unknown
		//IL_0501: Unknown result type (might be due to invalid IL or missing references)
		//IL_050b: Expected O, but got Unknown
		//IL_0519: Unknown result type (might be due to invalid IL or missing references)
		//IL_054c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0556: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FormPairingSetting));
		pictureBox_Logo = new PictureBox();
		label_foundDevices = new Label();
		switchButton1 = new SwitchButton();
		wiButton_Close_lite = new WindImageButton();
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
		((Control)label_foundDevices).AutoSize = true;
		((Control)label_foundDevices).BackColor = Color.Transparent;
		((Control)label_foundDevices).Font = new Font("微软雅黑", 9f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)label_foundDevices).ForeColor = Color.White;
		((Control)label_foundDevices).Location = new Point(22, 278);
		((Control)label_foundDevices).Name = "label_foundDevices";
		((Control)label_foundDevices).Size = new Size(43, 17);
		((Control)label_foundDevices).TabIndex = 210;
		((Control)label_foundDevices).Text = "label1";
		((Control)switchButton1).BackColor = Color.Transparent;
		((Control)switchButton1).ForeColor = Color.White;
		((Control)switchButton1).Location = new Point(48, 84);
		((Control)switchButton1).Margin = new Padding(6, 7, 6, 7);
		((Control)switchButton1).Name = "switchButton1";
		switchButton1.OffImage = (Image)(object)Resources.switchButtonOff;
		switchButton1.OffText = "     普通模式";
		switchButton1.OnImage = (Image)(object)Resources.switchButtonOn;
		switchButton1.OnText = "  远距离模式";
		((Control)switchButton1).Size = new Size(165, 29);
		((Control)switchButton1).TabIndex = 209;
		switchButton1.TextPoint = new Point(0, 0);
		((Control)switchButton1).Click += switchButton1_Click;
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
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)0;
		((Control)this).BackgroundImage = (Image)(object)Resources.pairBackImage;
		((Form)this).ClientSize = new Size(606, 344);
		((Control)this).Controls.Add((Control)(object)label_foundDevices);
		((Control)this).Controls.Add((Control)(object)switchButton1);
		((Control)this).Controls.Add((Control)(object)wiButton_Close_lite);
		((Control)this).Controls.Add((Control)(object)pictureBox_Logo);
		((Control)this).DoubleBuffered = true;
		((Control)this).Font = new Font("微软雅黑", 15f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Form)this).KeyPreview = true;
		((Form)this).Margin = new Padding(6, 7, 6, 7);
		((Control)this).Name = "FormPairingSetting";
		((Form)this).StartPosition = (FormStartPosition)0;
		((Control)this).Text = "Input Password";
		((Form)this).FormClosing += new FormClosingEventHandler(FormPasswordInput_FormClosing);
		((Form)this).Load += FormPairingSetting_Load;
		((Control)this).LocationChanged += FormPairingSetting_LocationChanged;
		((ISupportInitialize)pictureBox_Logo).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
