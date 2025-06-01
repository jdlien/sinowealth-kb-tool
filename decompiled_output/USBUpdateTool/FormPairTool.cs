using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DriverLib;
using USBUpdateTool.Properties;
using WindControls;

namespace USBUpdateTool;

public class FormPairTool : Form
{
	public static string[] DongleIC_ITEMs = new string[4] { "CX52650N", "CH305+n52810", "CX52650P", "BK2481" };

	public SkinForm skinForm = new SkinForm(_movable: true);

	private PairTool pairTool = new PairTool();

	public FormCompxMore formMore;

	private ICSelect iCSelect = new ICSelect();

	private VidPidManager vidPidManager = new VidPidManager();

	private IContainer components = null;

	public Label label_pairStatus;

	private WindImageButton wibutton_StartPairing;

	private WindImageButton wiButton_mini;

	private WindImageButton wiButton_Close;

	public WindComboBox wcobBox_IC_type;

	public WindCheckBox wCheckBox_IC_Enable;

	private EditValueControl editNormalVidPid;

	private EditValueControl editCidMid;

	public FormPairTool(FormCompxMore _formMore)
	{
		InitializeComponent();
		SetStyles();
		skinForm.InitSkin((Form)(object)this, 12, 16);
		formMore = _formMore;
		((Control)label_pairStatus).Text = "";
	}

	private void SetStyles()
	{
		((Control)this).SetStyle((ControlStyles)204818, true);
		((Control)this).UpdateStyles();
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)0;
	}

	private void InitVidPid()
	{
		wcobBox_IC_type.Clear();
		wcobBox_IC_type.Add(DongleIC_ITEMs);
		vidPidManager.icName = wcobBox_IC_type.SelectedText;
		vidPidManager.SetEditValueControls(editNormalVidPid, null, editCidMid);
		vidPidManager.Init();
	}

	private void wiButton_mini_Click(object sender, EventArgs e)
	{
		((Form)skinForm).WindowState = (FormWindowState)1;
		((Form)this).WindowState = (FormWindowState)1;
	}

	private void wiButton_Close_Click(object sender, EventArgs e)
	{
		skinForm.AllHide();
		formMore.skinForm.AllShow();
		UsbServer.Exit();
	}

	public void ShowWindow()
	{
		skinForm.AllShow((Form)(object)formMore);
		((Form)this).Location = ((Form)formMore).Location;
		UsbFinder.SetUsbChangedCallBack(OnUsbChangedEvent, 600);
		FindPairDevice();
	}

	private void FindPairDevice()
	{
		string fileNameWithoutExtension = wcobBox_IC_type.SelectedText + "_Dongle.bin";
		iCSelect.CreateUpgradeFile(fileNameWithoutExtension, null, wcobBox_IC_type.SelectedText, editNormalVidPid.GetLeftValue(), editNormalVidPid.GetRightValue(), "", "");
		int deviceId = 0;
		List<string> list = pairTool.FindDongleCount(iCSelect, ref deviceId);
		((Control)wibutton_StartPairing).Enabled = list.Count > 0;
	}

	private void OnUsbChangedEvent(bool isPlug)
	{
		FindPairDevice();
	}

	private void wibutton_StartPairing_Click(object sender, EventArgs e)
	{
		pairTool.Start((Form)(object)this, iCSelect, onUsbDataReceived);
	}

	public void onUsbDataReceived(UsbCommand command, string displayText)
	{
		switch ((UsbCommandID)command.id)
		{
		case UsbCommandID.DongleEnterPair:
			((Control)this).Invoke((Delegate)(Action)delegate
			{
				((Control)wibutton_StartPairing).Enabled = false;
				((Control)label_pairStatus).Text = displayText;
				((Control)label_pairStatus).ForeColor = Color.RoyalBlue;
			});
			break;
		case UsbCommandID.GetPairState:
			if (command.receivedData[0] == 1)
			{
				((Control)this).Invoke((Delegate)(Action)delegate
				{
					((Control)label_pairStatus).Text = displayText;
				});
			}
			else if (command.receivedData[0] == 2)
			{
				((Control)this).Invoke((Delegate)(Action)delegate
				{
					((Control)wibutton_StartPairing).Enabled = true;
					((Control)label_pairStatus).Text = displayText;
					((Control)label_pairStatus).ForeColor = Color.Red;
				});
			}
			else if (command.receivedData[0] == 3)
			{
				((Control)this).Invoke((Delegate)(Action)delegate
				{
					((Control)wibutton_StartPairing).Enabled = true;
					((Control)label_pairStatus).Text = displayText;
					((Control)label_pairStatus).ForeColor = Color.Green;
				});
			}
			break;
		}
	}

	private void FormPairTool_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 13)
		{
			wibutton_StartPairing_Click(null, null);
		}
	}

	private void wCheckBox_IC_Enable_CheckedChanged(object sender, EventArgs e)
	{
		if (wCheckBox_IC_Enable.Enabled)
		{
			((Control)wcobBox_IC_type).Enabled = wCheckBox_IC_Enable.Checked;
		}
	}

	private void wcobBox_IC_type_SelectedIndexChanged(object sender, EventArgs e)
	{
		wCheckBox_IC_Enable.Checked = false;
		((Control)wcobBox_IC_type).Enabled = false;
		vidPidManager.Update(wcobBox_IC_type.SelectedText);
		FindPairDevice();
	}

	private void FormPairTool_Load(object sender, EventArgs e)
	{
		InitVidPid();
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
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Expected O, but got Unknown
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Expected O, but got Unknown
		//IL_07d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e1: Expected O, but got Unknown
		//IL_08d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a1b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a25: Expected O, but got Unknown
		//IL_0b7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b86: Expected O, but got Unknown
		//IL_0c58: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c62: Expected O, but got Unknown
		//IL_0dca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dd4: Expected O, but got Unknown
		//IL_0de9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0df3: Expected O, but got Unknown
		//IL_0e37: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e41: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FormPairTool));
		label_pairStatus = new Label();
		wibutton_StartPairing = new WindImageButton();
		wiButton_mini = new WindImageButton();
		wiButton_Close = new WindImageButton();
		wcobBox_IC_type = new WindComboBox();
		wCheckBox_IC_Enable = new WindCheckBox();
		editNormalVidPid = new EditValueControl();
		editCidMid = new EditValueControl();
		((Control)this).SuspendLayout();
		((Control)label_pairStatus).AutoSize = true;
		((Control)label_pairStatus).BackColor = Color.Transparent;
		((Control)label_pairStatus).Font = new Font("微软雅黑", 24f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)label_pairStatus).Location = new Point(172, 283);
		((Control)label_pairStatus).Name = "label_pairStatus";
		((Control)label_pairStatus).Size = new Size(72, 41);
		((Control)label_pairStatus).TabIndex = 44;
		((Control)label_pairStatus).Text = "CID";
		((Control)wibutton_StartPairing).BackColor = Color.Transparent;
		((Control)wibutton_StartPairing).BackgroundImage = (Image)(object)Resources.button_1;
		((Control)wibutton_StartPairing).BackgroundImageLayout = (ImageLayout)3;
		wibutton_StartPairing.DisableBackColor = Color.Transparent;
		wibutton_StartPairing.DisableForeColor = Color.DarkGray;
		wibutton_StartPairing.DisableImage = (Image)(object)Resources.button_2;
		((Control)wibutton_StartPairing).Font = new Font("微软雅黑", 15f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wibutton_StartPairing.FrameMode = GraphicsHelper.RoundStyle.All;
		wibutton_StartPairing.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wibutton_StartPairing.IconName = "";
		wibutton_StartPairing.IconOffset = new Point(0, 0);
		wibutton_StartPairing.IconSize = 32;
		((Control)wibutton_StartPairing).Location = new Point(647, 349);
		wibutton_StartPairing.MouseDownBackColor = Color.Gray;
		wibutton_StartPairing.MouseDownForeColor = Color.Black;
		wibutton_StartPairing.MouseDownImage = (Image)(object)Resources.button_2;
		wibutton_StartPairing.MouseEnterBackColor = Color.DarkGray;
		wibutton_StartPairing.MouseEnterForeColor = Color.Black;
		wibutton_StartPairing.MouseEnterImage = (Image)(object)Resources.button_2;
		wibutton_StartPairing.MouseUpBackColor = Color.Transparent;
		wibutton_StartPairing.MouseUpForeColor = Color.Black;
		wibutton_StartPairing.MouseUpImage = (Image)(object)Resources.button_1;
		((Control)wibutton_StartPairing).Name = "wibutton_StartPairing";
		wibutton_StartPairing.Radius = 12;
		((Control)wibutton_StartPairing).Size = new Size(114, 60);
		((Control)wibutton_StartPairing).TabIndex = 45;
		((Control)wibutton_StartPairing).Text = "Pair";
		wibutton_StartPairing.TextAlignment = StringHelper.TextAlignment.Center;
		wibutton_StartPairing.TextDynOffset = new Point(0, 0);
		wibutton_StartPairing.TextFixLocation = new Point(0, 0);
		wibutton_StartPairing.TextFixLocationEnable = false;
		((Control)wibutton_StartPairing).Click += wibutton_StartPairing_Click;
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
		((Control)wcobBox_IC_type).BackgroundImage = (Image)(object)Resources.下拉正常状态;
		((Control)wcobBox_IC_type).BackgroundImageLayout = (ImageLayout)3;
		wcobBox_IC_type.DropDownLoctionOffset = new Point(-32, 0);
		wcobBox_IC_type.DropDownMaxRowCount = 8;
		wcobBox_IC_type.DropDownWidthDelta = 32;
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
		wcobBox_IC_type.Items.Add("CX52650N");
		wcobBox_IC_type.Items.Add("CH305+n52810");
		((Control)wcobBox_IC_type).Location = new Point(44, 55);
		((Control)wcobBox_IC_type).Margin = new Padding(4, 5, 4, 5);
		wcobBox_IC_type.MouseDownImage = (Image)(object)Resources.下拉鼠标点击状态;
		wcobBox_IC_type.MouseEnterImage = (Image)(object)Resources.下拉鼠标指过去状态;
		wcobBox_IC_type.MovingSelectedBackColor = Color.LightSkyBlue;
		((Control)wcobBox_IC_type).Name = "wcobBox_IC_type";
		wcobBox_IC_type.NormalImage = (Image)(object)Resources.下拉正常状态;
		wcobBox_IC_type.ReadOnly = true;
		((Control)wcobBox_IC_type).RightToLeft = (RightToLeft)0;
		wcobBox_IC_type.SelectedIndex = 0;
		wcobBox_IC_type.SelectedText = "CX52650N";
		((Control)wcobBox_IC_type).Size = new Size(106, 26);
		wcobBox_IC_type.SplitLineColor = Color.LightGray;
		((Control)wcobBox_IC_type).TabIndex = 130;
		wcobBox_IC_type.SelectedIndexChanged += wcobBox_IC_type_SelectedIndexChanged;
		((Control)wCheckBox_IC_Enable).BackColor = Color.Transparent;
		((Control)wCheckBox_IC_Enable).BackgroundImageLayout = (ImageLayout)0;
		wCheckBox_IC_Enable.Checked = false;
		wCheckBox_IC_Enable.DisableSelectedImage = (Image)(object)Resources.checkbox_1;
		wCheckBox_IC_Enable.DisableUnSelectedImage = (Image)(object)Resources.checkbox_2;
		((Control)wCheckBox_IC_Enable).Font = new Font("微软雅黑", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wCheckBox_IC_Enable.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wCheckBox_IC_Enable.IconOffset = new Point(0, 0);
		wCheckBox_IC_Enable.IconSize = 36;
		((Control)wCheckBox_IC_Enable).Location = new Point(11, 53);
		((Control)wCheckBox_IC_Enable).Name = "wCheckBox_IC_Enable";
		wCheckBox_IC_Enable.SelectedIconColor = Color.Red;
		wCheckBox_IC_Enable.SelectedIconName = "";
		wCheckBox_IC_Enable.SelectedImage = (Image)(object)Resources.checkbox_1;
		((Control)wCheckBox_IC_Enable).Size = new Size(30, 32);
		((Control)wCheckBox_IC_Enable).TabIndex = 129;
		((Control)wCheckBox_IC_Enable).Text = " ";
		wCheckBox_IC_Enable.TextOffset = new Point(4, 4);
		wCheckBox_IC_Enable.UnSelectedIconColor = Color.Gray;
		wCheckBox_IC_Enable.UnSelectedIconName = "";
		wCheckBox_IC_Enable.UnSelectedImage = (Image)(object)Resources.checkbox_2;
		wCheckBox_IC_Enable.CheckedChanged += wCheckBox_IC_Enable_CheckedChanged;
		((Control)editNormalVidPid).BackColor = Color.Transparent;
		editNormalVidPid.CheckedText = "  Edit VID/PID";
		((Control)editNormalVidPid).Font = new Font("微软雅黑", 10.5f);
		editNormalVidPid.isChecked = false;
		editNormalVidPid.LeftLabelText = "VID";
		((Control)editNormalVidPid).Location = new Point(160, 53);
		((Control)editNormalVidPid).Name = "editNormalVidPid";
		editNormalVidPid.RightLabelText = "PID";
		((Control)editNormalVidPid).Size = new Size(368, 77);
		((Control)editNormalVidPid).TabIndex = 135;
		editNormalVidPid.UnCheckedText = " Default VID/PID";
		editNormalVidPid.xUI = false;
		((Control)editCidMid).BackColor = Color.Transparent;
		editCidMid.CheckedText = "  Edit CID/MID";
		((Control)editCidMid).Font = new Font("微软雅黑", 10.5f);
		editCidMid.isChecked = false;
		editCidMid.LeftLabelText = "VID";
		((Control)editCidMid).Location = new Point(160, 136);
		((Control)editCidMid).Name = "editCidMid";
		editCidMid.RightLabelText = "PID";
		((Control)editCidMid).Size = new Size(368, 77);
		((Control)editCidMid).TabIndex = 136;
		editCidMid.UnCheckedText = " Default CID/MID";
		editCidMid.xUI = false;
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)0;
		((Control)this).BackgroundImage = (Image)(object)Resources.cidmid;
		((Form)this).ClientSize = new Size(802, 421);
		((Control)this).Controls.Add((Control)(object)editCidMid);
		((Control)this).Controls.Add((Control)(object)editNormalVidPid);
		((Control)this).Controls.Add((Control)(object)wcobBox_IC_type);
		((Control)this).Controls.Add((Control)(object)wCheckBox_IC_Enable);
		((Control)this).Controls.Add((Control)(object)wiButton_mini);
		((Control)this).Controls.Add((Control)(object)wiButton_Close);
		((Control)this).Controls.Add((Control)(object)wibutton_StartPairing);
		((Control)this).Controls.Add((Control)(object)label_pairStatus);
		((Control)this).Font = new Font("微软雅黑", 10.5f);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Form)this).KeyPreview = true;
		((Control)this).Name = "FormPairTool";
		((Form)this).StartPosition = (FormStartPosition)0;
		((Control)this).Text = "EditCidMid";
		((Form)this).Load += FormPairTool_Load;
		((Control)this).KeyDown += new KeyEventHandler(FormPairTool_KeyDown);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
