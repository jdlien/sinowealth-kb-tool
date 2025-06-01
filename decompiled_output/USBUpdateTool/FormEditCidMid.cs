using System;
using System.ComponentModel;
using System.Drawing;
using System.Timers;
using System.Windows.Forms;
using USBUpdateTool.Properties;
using WindControls;

namespace USBUpdateTool;

public class FormEditCidMid : Form
{
	public SkinForm skinForm = new SkinForm(_movable: true);

	private VidPidManager vidPidManager = new VidPidManager();

	public FormCompxMore formMore;

	private ICSelect iCSelect = new ICSelect();

	private Timer cidmidTimer = new Timer();

	private byte Cid = 0;

	private byte Mid = 0;

	private byte CommandId = 0;

	private IContainer components = null;

	public Label label_cidmid;

	private WindImageButton wibutton_CIDMID_Write;

	private WindImageButton wibutton_CIDMID_Read;

	private WindImageButton wiButton_mini;

	private WindImageButton wiButton_Close;

	public WindComboBox wcobBox_IC_type;

	public WindCheckBox wCheckBox_IC_Enable;

	private EditValueControl editNormalVidPid;

	private EditValueControl editCidMid;

	public FormEditCidMid(FormCompxMore _formMore)
	{
		InitializeComponent();
		SetStyles();
		skinForm.InitSkin((Form)(object)this, 12, 16);
		formMore = _formMore;
		((Control)label_cidmid).Text = "";
		InitTimer();
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
		wcobBox_IC_type.Add(ICSelect.IC_ITEMs);
		wcobBox_IC_type.SetSelectedText(FileManager.GetLastICType());
		vidPidManager.SetEditValueControls(editNormalVidPid, null, editCidMid);
		vidPidManager.Init();
	}

	public void InitTimer()
	{
		cidmidTimer.Interval = 300.0;
		cidmidTimer.Elapsed += delegate
		{
			((Control)this).Invoke((Delegate)(Action)delegate
			{
				((Control)label_cidmid).ForeColor = Color.Green;
				if (CommandId == 240)
				{
					((Control)label_cidmid).Text = "Write Success!     CID: " + Cid.ToString("D3") + "      MID: " + Mid.ToString("D3");
				}
				else if (CommandId == 241)
				{
					((Control)label_cidmid).Text = "Read Success!     CID: " + Cid.ToString("D3") + "      MID: " + Mid.ToString("D3");
				}
				else
				{
					((Control)label_cidmid).Text = "";
				}
			});
			cidmidTimer.Stop();
		};
		cidmidTimer.Stop();
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
	}

	public void ShowWindow()
	{
		skinForm.AllShow((Form)(object)formMore);
		((Form)this).Location = ((Form)formMore).Location;
	}

	private void CreateICFIle()
	{
		string fileNameWithoutExtension = wcobBox_IC_type.SelectedText + "_KB.bin";
		iCSelect.CreateUpgradeFile(fileNameWithoutExtension, null, wcobBox_IC_type.SelectedText, editNormalVidPid.GetLeftValue(), editNormalVidPid.GetRightValue(), "", "");
	}

	private void wibutton_CIDMID_Write_Click(object sender, EventArgs e)
	{
		CreateICFIle();
		byte cid = (byte)editCidMid.GetLeftDecimalValue();
		byte mid = (byte)editCidMid.GetRightDecimalValue();
		if (iCSelect.WriteCidMid(cid, mid, 0, out var deviceInfo))
		{
			((Control)label_cidmid).ForeColor = Color.Green;
			((Control)label_cidmid).Text = "Write Success!     CID: " + deviceInfo.CID.ToString("D3") + "      MID: " + deviceInfo.MID.ToString("D3");
		}
		else
		{
			((Control)label_cidmid).Text = "Write Fail!";
			((Control)label_cidmid).ForeColor = Color.Red;
		}
	}

	private void wibutton_CIDMID_Read_Click(object sender, EventArgs e)
	{
		CreateICFIle();
		if (iCSelect.ReadCidMid(isKeyboard: false, out var deviceInfo))
		{
			((Control)label_cidmid).ForeColor = Color.Green;
			((Control)label_cidmid).Text = "Read Success!     CID: " + deviceInfo.CID.ToString("D3") + "      MID: " + deviceInfo.MID.ToString("D3");
		}
		else
		{
			((Control)label_cidmid).Text = "Read Fail!";
			((Control)label_cidmid).ForeColor = Color.Red;
		}
	}

	private void wcobBox_IC_type_SelectedIndexChanged(object sender, EventArgs e)
	{
		wCheckBox_IC_Enable.Checked = false;
		((Control)wcobBox_IC_type).Enabled = false;
		FileManager.SaveLastICType(wcobBox_IC_type.SelectedText);
		vidPidManager.Update(wcobBox_IC_type.SelectedText);
	}

	private void wCheckBox_IC_Enable_CheckedChanged(object sender, EventArgs e)
	{
		if (wCheckBox_IC_Enable.Enabled)
		{
			((Control)wcobBox_IC_type).Enabled = wCheckBox_IC_Enable.Checked;
		}
	}

	private void FormEditCidMid_Load(object sender, EventArgs e)
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
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Expected O, but got Unknown
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Expected O, but got Unknown
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Expected O, but got Unknown
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Expected O, but got Unknown
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_0262: Expected O, but got Unknown
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Expected O, but got Unknown
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02de: Expected O, but got Unknown
		//IL_03ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b6: Expected O, but got Unknown
		//IL_03f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0401: Expected O, but got Unknown
		//IL_0419: Unknown result type (might be due to invalid IL or missing references)
		//IL_0423: Expected O, but got Unknown
		//IL_04c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d1: Expected O, but got Unknown
		//IL_0505: Unknown result type (might be due to invalid IL or missing references)
		//IL_050f: Expected O, but got Unknown
		//IL_0543: Unknown result type (might be due to invalid IL or missing references)
		//IL_054d: Expected O, but got Unknown
		//IL_061b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0625: Expected O, but got Unknown
		//IL_0666: Unknown result type (might be due to invalid IL or missing references)
		//IL_0670: Expected O, but got Unknown
		//IL_0711: Unknown result type (might be due to invalid IL or missing references)
		//IL_071b: Expected O, but got Unknown
		//IL_074f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0759: Expected O, but got Unknown
		//IL_078d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0797: Expected O, but got Unknown
		//IL_0861: Unknown result type (might be due to invalid IL or missing references)
		//IL_086b: Expected O, but got Unknown
		//IL_08ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b6: Expected O, but got Unknown
		//IL_0957: Unknown result type (might be due to invalid IL or missing references)
		//IL_0961: Expected O, but got Unknown
		//IL_0995: Unknown result type (might be due to invalid IL or missing references)
		//IL_099f: Expected O, but got Unknown
		//IL_09d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_09dd: Expected O, but got Unknown
		//IL_0a96: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa0: Expected O, but got Unknown
		//IL_0b01: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b0b: Expected O, but got Unknown
		//IL_0c7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca4: Expected O, but got Unknown
		//IL_0cb6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cc0: Expected O, but got Unknown
		//IL_0cf4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cfe: Expected O, but got Unknown
		//IL_0dc2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dcc: Expected O, but got Unknown
		//IL_0dde: Unknown result type (might be due to invalid IL or missing references)
		//IL_0de8: Expected O, but got Unknown
		//IL_0e00: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e0a: Expected O, but got Unknown
		//IL_0e9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ea4: Expected O, but got Unknown
		//IL_0f22: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f2c: Expected O, but got Unknown
		//IL_0f77: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f81: Expected O, but got Unknown
		//IL_1053: Unknown result type (might be due to invalid IL or missing references)
		//IL_105d: Expected O, but got Unknown
		//IL_11d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_11e1: Expected O, but got Unknown
		//IL_11f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1200: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FormEditCidMid));
		label_cidmid = new Label();
		wibutton_CIDMID_Write = new WindImageButton();
		wibutton_CIDMID_Read = new WindImageButton();
		wiButton_mini = new WindImageButton();
		wiButton_Close = new WindImageButton();
		wcobBox_IC_type = new WindComboBox();
		wCheckBox_IC_Enable = new WindCheckBox();
		editNormalVidPid = new EditValueControl();
		editCidMid = new EditValueControl();
		((Control)this).SuspendLayout();
		((Control)label_cidmid).AutoSize = true;
		((Control)label_cidmid).BackColor = Color.Transparent;
		((Control)label_cidmid).Font = new Font("微软雅黑", 24f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)label_cidmid).Location = new Point(160, 266);
		((Control)label_cidmid).Name = "label_cidmid";
		((Control)label_cidmid).Size = new Size(72, 41);
		((Control)label_cidmid).TabIndex = 44;
		((Control)label_cidmid).Text = "CID";
		((Control)wibutton_CIDMID_Write).BackColor = Color.Transparent;
		((Control)wibutton_CIDMID_Write).BackgroundImage = (Image)componentResourceManager.GetObject("wibutton_CIDMID_Write.BackgroundImage");
		((Control)wibutton_CIDMID_Write).BackgroundImageLayout = (ImageLayout)3;
		wibutton_CIDMID_Write.DisableBackColor = Color.Transparent;
		wibutton_CIDMID_Write.DisableForeColor = Color.DarkGray;
		wibutton_CIDMID_Write.DisableImage = (Image)componentResourceManager.GetObject("wibutton_CIDMID_Write.DisableImage");
		((Control)wibutton_CIDMID_Write).Font = new Font("微软雅黑", 14.25f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wibutton_CIDMID_Write.FrameMode = GraphicsHelper.RoundStyle.All;
		wibutton_CIDMID_Write.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wibutton_CIDMID_Write.IconName = "";
		wibutton_CIDMID_Write.IconOffset = new Point(0, 0);
		wibutton_CIDMID_Write.IconSize = 32;
		((Control)wibutton_CIDMID_Write).Location = new Point(176, 340);
		wibutton_CIDMID_Write.MouseDownBackColor = Color.Gray;
		wibutton_CIDMID_Write.MouseDownForeColor = Color.Black;
		wibutton_CIDMID_Write.MouseDownImage = (Image)componentResourceManager.GetObject("wibutton_CIDMID_Write.MouseDownImage");
		wibutton_CIDMID_Write.MouseEnterBackColor = Color.DarkGray;
		wibutton_CIDMID_Write.MouseEnterForeColor = Color.Black;
		wibutton_CIDMID_Write.MouseEnterImage = (Image)componentResourceManager.GetObject("wibutton_CIDMID_Write.MouseEnterImage");
		wibutton_CIDMID_Write.MouseUpBackColor = Color.Transparent;
		wibutton_CIDMID_Write.MouseUpForeColor = Color.Black;
		wibutton_CIDMID_Write.MouseUpImage = (Image)componentResourceManager.GetObject("wibutton_CIDMID_Write.MouseUpImage");
		((Control)wibutton_CIDMID_Write).Name = "wibutton_CIDMID_Write";
		wibutton_CIDMID_Write.Radius = 12;
		((Control)wibutton_CIDMID_Write).Size = new Size(114, 60);
		((Control)wibutton_CIDMID_Write).TabIndex = 45;
		((Control)wibutton_CIDMID_Write).Text = "Write";
		wibutton_CIDMID_Write.TextAlignment = StringHelper.TextAlignment.Center;
		wibutton_CIDMID_Write.TextDynOffset = new Point(0, 0);
		wibutton_CIDMID_Write.TextFixLocation = new Point(0, 0);
		wibutton_CIDMID_Write.TextFixLocationEnable = false;
		((Control)wibutton_CIDMID_Write).Click += wibutton_CIDMID_Write_Click;
		((Control)wibutton_CIDMID_Read).BackColor = Color.Transparent;
		((Control)wibutton_CIDMID_Read).BackgroundImage = (Image)componentResourceManager.GetObject("wibutton_CIDMID_Read.BackgroundImage");
		((Control)wibutton_CIDMID_Read).BackgroundImageLayout = (ImageLayout)3;
		wibutton_CIDMID_Read.DisableBackColor = Color.Transparent;
		wibutton_CIDMID_Read.DisableForeColor = Color.DarkGray;
		wibutton_CIDMID_Read.DisableImage = (Image)componentResourceManager.GetObject("wibutton_CIDMID_Read.DisableImage");
		((Control)wibutton_CIDMID_Read).Font = new Font("微软雅黑", 14.25f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wibutton_CIDMID_Read.FrameMode = GraphicsHelper.RoundStyle.All;
		wibutton_CIDMID_Read.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wibutton_CIDMID_Read.IconName = "";
		wibutton_CIDMID_Read.IconOffset = new Point(0, 0);
		wibutton_CIDMID_Read.IconSize = 32;
		((Control)wibutton_CIDMID_Read).Location = new Point(351, 340);
		wibutton_CIDMID_Read.MouseDownBackColor = Color.Gray;
		wibutton_CIDMID_Read.MouseDownForeColor = Color.Black;
		wibutton_CIDMID_Read.MouseDownImage = (Image)componentResourceManager.GetObject("wibutton_CIDMID_Read.MouseDownImage");
		wibutton_CIDMID_Read.MouseEnterBackColor = Color.DarkGray;
		wibutton_CIDMID_Read.MouseEnterForeColor = Color.Black;
		wibutton_CIDMID_Read.MouseEnterImage = (Image)componentResourceManager.GetObject("wibutton_CIDMID_Read.MouseEnterImage");
		wibutton_CIDMID_Read.MouseUpBackColor = Color.Transparent;
		wibutton_CIDMID_Read.MouseUpForeColor = Color.Black;
		wibutton_CIDMID_Read.MouseUpImage = (Image)componentResourceManager.GetObject("wibutton_CIDMID_Read.MouseUpImage");
		((Control)wibutton_CIDMID_Read).Name = "wibutton_CIDMID_Read";
		wibutton_CIDMID_Read.Radius = 12;
		((Control)wibutton_CIDMID_Read).Size = new Size(114, 60);
		((Control)wibutton_CIDMID_Read).TabIndex = 46;
		((Control)wibutton_CIDMID_Read).Text = "Read";
		wibutton_CIDMID_Read.TextAlignment = StringHelper.TextAlignment.Center;
		wibutton_CIDMID_Read.TextDynOffset = new Point(0, 0);
		wibutton_CIDMID_Read.TextFixLocation = new Point(0, 0);
		wibutton_CIDMID_Read.TextFixLocationEnable = false;
		((Control)wibutton_CIDMID_Read).Click += wibutton_CIDMID_Read_Click;
		((Control)wiButton_mini).BackColor = Color.Transparent;
		((Control)wiButton_mini).BackgroundImage = (Image)componentResourceManager.GetObject("wiButton_mini.BackgroundImage");
		((Control)wiButton_mini).BackgroundImageLayout = (ImageLayout)2;
		wiButton_mini.DisableBackColor = Color.Transparent;
		wiButton_mini.DisableForeColor = Color.DarkGray;
		wiButton_mini.DisableImage = (Image)componentResourceManager.GetObject("wiButton_mini.DisableImage");
		wiButton_mini.FrameMode = GraphicsHelper.RoundStyle.All;
		wiButton_mini.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wiButton_mini.IconName = "";
		wiButton_mini.IconOffset = new Point(0, 0);
		wiButton_mini.IconSize = 32;
		((Control)wiButton_mini).Location = new Point(743, 21);
		wiButton_mini.MouseDownBackColor = Color.Gray;
		wiButton_mini.MouseDownForeColor = Color.DarkRed;
		wiButton_mini.MouseDownImage = (Image)componentResourceManager.GetObject("wiButton_mini.MouseDownImage");
		wiButton_mini.MouseEnterBackColor = Color.DarkGray;
		wiButton_mini.MouseEnterForeColor = Color.OrangeRed;
		wiButton_mini.MouseEnterImage = (Image)componentResourceManager.GetObject("wiButton_mini.MouseEnterImage");
		wiButton_mini.MouseUpBackColor = Color.Transparent;
		wiButton_mini.MouseUpForeColor = Color.Red;
		wiButton_mini.MouseUpImage = (Image)componentResourceManager.GetObject("wiButton_mini.MouseUpImage");
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
		((Control)wiButton_Close).BackgroundImage = (Image)componentResourceManager.GetObject("wiButton_Close.BackgroundImage");
		((Control)wiButton_Close).BackgroundImageLayout = (ImageLayout)2;
		wiButton_Close.DisableBackColor = Color.Transparent;
		wiButton_Close.DisableForeColor = Color.DarkGray;
		wiButton_Close.DisableImage = (Image)componentResourceManager.GetObject("wiButton_Close.DisableImage");
		wiButton_Close.FrameMode = GraphicsHelper.RoundStyle.All;
		wiButton_Close.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wiButton_Close.IconName = "";
		wiButton_Close.IconOffset = new Point(0, 0);
		wiButton_Close.IconSize = 32;
		((Control)wiButton_Close).Location = new Point(772, 21);
		wiButton_Close.MouseDownBackColor = Color.Gray;
		wiButton_Close.MouseDownForeColor = Color.DarkRed;
		wiButton_Close.MouseDownImage = (Image)componentResourceManager.GetObject("wiButton_Close.MouseDownImage");
		wiButton_Close.MouseEnterBackColor = Color.DarkGray;
		wiButton_Close.MouseEnterForeColor = Color.OrangeRed;
		wiButton_Close.MouseEnterImage = (Image)componentResourceManager.GetObject("wiButton_Close.MouseEnterImage");
		wiButton_Close.MouseUpBackColor = Color.Transparent;
		wiButton_Close.MouseUpForeColor = Color.Red;
		wiButton_Close.MouseUpImage = (Image)componentResourceManager.GetObject("wiButton_Close.MouseUpImage");
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
		((Control)wcobBox_IC_type).BackgroundImage = (Image)componentResourceManager.GetObject("wcobBox_IC_type.BackgroundImage");
		((Control)wcobBox_IC_type).BackgroundImageLayout = (ImageLayout)3;
		wcobBox_IC_type.DropDownLoctionOffset = new Point(-32, 0);
		wcobBox_IC_type.DropDownMaxRowCount = 8;
		wcobBox_IC_type.DropDownWidthDelta = 36;
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
		((Control)wcobBox_IC_type).Location = new Point(46, 54);
		((Control)wcobBox_IC_type).Margin = new Padding(4, 5, 4, 5);
		wcobBox_IC_type.MouseDownImage = (Image)componentResourceManager.GetObject("wcobBox_IC_type.MouseDownImage");
		wcobBox_IC_type.MouseEnterImage = (Image)componentResourceManager.GetObject("wcobBox_IC_type.MouseEnterImage");
		wcobBox_IC_type.MovingSelectedBackColor = Color.LightSkyBlue;
		((Control)wcobBox_IC_type).Name = "wcobBox_IC_type";
		wcobBox_IC_type.NormalImage = (Image)componentResourceManager.GetObject("wcobBox_IC_type.NormalImage");
		wcobBox_IC_type.ReadOnly = true;
		((Control)wcobBox_IC_type).RightToLeft = (RightToLeft)0;
		wcobBox_IC_type.SelectedIndex = 0;
		wcobBox_IC_type.SelectedText = "CX52850";
		((Control)wcobBox_IC_type).Size = new Size(106, 27);
		wcobBox_IC_type.SplitLineColor = Color.LightGray;
		((Control)wcobBox_IC_type).TabIndex = 133;
		wcobBox_IC_type.SelectedIndexChanged += wcobBox_IC_type_SelectedIndexChanged;
		((Control)wCheckBox_IC_Enable).BackColor = Color.Transparent;
		((Control)wCheckBox_IC_Enable).BackgroundImageLayout = (ImageLayout)0;
		wCheckBox_IC_Enable.Checked = false;
		wCheckBox_IC_Enable.DisableSelectedImage = (Image)componentResourceManager.GetObject("wCheckBox_IC_Enable.DisableSelectedImage");
		wCheckBox_IC_Enable.DisableUnSelectedImage = (Image)componentResourceManager.GetObject("wCheckBox_IC_Enable.DisableUnSelectedImage");
		((Control)wCheckBox_IC_Enable).Font = new Font("微软雅黑", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wCheckBox_IC_Enable.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wCheckBox_IC_Enable.IconOffset = new Point(0, 0);
		wCheckBox_IC_Enable.IconSize = 36;
		((Control)wCheckBox_IC_Enable).Location = new Point(10, 53);
		((Control)wCheckBox_IC_Enable).Name = "wCheckBox_IC_Enable";
		wCheckBox_IC_Enable.SelectedIconColor = Color.Red;
		wCheckBox_IC_Enable.SelectedIconName = "";
		wCheckBox_IC_Enable.SelectedImage = (Image)componentResourceManager.GetObject("wCheckBox_IC_Enable.SelectedImage");
		((Control)wCheckBox_IC_Enable).Size = new Size(30, 31);
		((Control)wCheckBox_IC_Enable).TabIndex = 132;
		((Control)wCheckBox_IC_Enable).Text = " ";
		wCheckBox_IC_Enable.TextOffset = new Point(4, 4);
		wCheckBox_IC_Enable.UnSelectedIconColor = Color.Gray;
		wCheckBox_IC_Enable.UnSelectedIconName = "";
		wCheckBox_IC_Enable.UnSelectedImage = (Image)componentResourceManager.GetObject("wCheckBox_IC_Enable.UnSelectedImage");
		wCheckBox_IC_Enable.CheckedChanged += wCheckBox_IC_Enable_CheckedChanged;
		((Control)editNormalVidPid).BackColor = Color.Transparent;
		editNormalVidPid.CheckedText = "  Edit VID/PID";
		((Control)editNormalVidPid).Font = new Font("微软雅黑", 10.5f);
		editNormalVidPid.isChecked = false;
		editNormalVidPid.LeftLabelText = "VID";
		((Control)editNormalVidPid).Location = new Point(167, 54);
		((Control)editNormalVidPid).Name = "editNormalVidPid";
		editNormalVidPid.RightLabelText = "PID";
		((Control)editNormalVidPid).Size = new Size(368, 77);
		((Control)editNormalVidPid).TabIndex = 142;
		editNormalVidPid.UnCheckedText = " Default VID/PID";
		editNormalVidPid.xUI = false;
		((Control)editCidMid).BackColor = Color.Transparent;
		editCidMid.CheckedText = "  Edit CID/MID";
		((Control)editCidMid).Font = new Font("微软雅黑", 10.5f);
		editCidMid.isChecked = false;
		editCidMid.LeftLabelText = "VID";
		((Control)editCidMid).Location = new Point(167, 136);
		((Control)editCidMid).Name = "editCidMid";
		editCidMid.RightLabelText = "PID";
		((Control)editCidMid).Size = new Size(368, 77);
		((Control)editCidMid).TabIndex = 144;
		editCidMid.UnCheckedText = " Default CID/MID";
		editCidMid.xUI = false;
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)0;
		((Control)this).BackgroundImage = (Image)(object)Resources.cidmid;
		((Form)this).ClientSize = new Size(822, 465);
		((Control)this).Controls.Add((Control)(object)editCidMid);
		((Control)this).Controls.Add((Control)(object)editNormalVidPid);
		((Control)this).Controls.Add((Control)(object)wcobBox_IC_type);
		((Control)this).Controls.Add((Control)(object)wCheckBox_IC_Enable);
		((Control)this).Controls.Add((Control)(object)wiButton_mini);
		((Control)this).Controls.Add((Control)(object)wiButton_Close);
		((Control)this).Controls.Add((Control)(object)wibutton_CIDMID_Read);
		((Control)this).Controls.Add((Control)(object)wibutton_CIDMID_Write);
		((Control)this).Controls.Add((Control)(object)label_cidmid);
		((Control)this).Font = new Font("微软雅黑", 10.5f);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Control)this).Name = "FormEditCidMid";
		((Form)this).StartPosition = (FormStartPosition)0;
		((Control)this).Text = "EditCidMid";
		((Form)this).Load += FormEditCidMid_Load;
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
