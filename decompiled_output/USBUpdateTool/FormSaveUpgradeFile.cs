using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using USBUpdateTool.Properties;
using USBUpdateTool.UpgradeFile;
using WindControls;

namespace USBUpdateTool;

public class FormSaveUpgradeFile : Form
{
	public SkinForm skinForm = new SkinForm(_movable: true);

	public FormCompxMore formMore;

	private ICSelect iCSelect = new ICSelect();

	private FileHeader fileHeader = new FileHeader("");

	private VidPidManager vidPidManager = new VidPidManager();

	private IContainer components = null;

	private WindImageButton wibutton_SaveUpgradeFile;

	private WindImageButton wiButton_mini;

	private WindImageButton wiButton_Close;

	public WindCheckBox wCheckBox_IC_Enable;

	private EditValueControl editNormalVidPid;

	private EditValueControl editCidMid;

	public EditValueControl editBootVidPid;

	public WindComboBox wComboBox_SensorName;

	public WindTextBox wTextBox_productName;

	private Label label_productName;

	private Label label_Version;

	public WindTextBox wTextBox_Version;

	public WindImageButton wibutton_LoadFile;

	public WindTextBox wTextBox_fileName;

	private Label label1;

	public WindComboBox wcobBox_IC_type;

	public FormSaveUpgradeFile(FormCompxMore _formMore)
	{
		InitializeComponent();
		SetStyles();
		skinForm.InitSkin((Form)(object)this, 12, 16);
		formMore = _formMore;
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
		vidPidManager.icName = wcobBox_IC_type.SelectedText;
		vidPidManager.SetEditValueControls(editNormalVidPid, editBootVidPid, editCidMid);
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
	}

	public void ShowWindow()
	{
		skinForm.AllShow((Form)(object)formMore);
		((Form)this).Location = ((Form)formMore).Location;
		formMore.formMore.formMain.loadBinFile.LoadLastFile(wTextBox_fileName);
	}

	private void wcobBox_IC_type_SelectedIndexChanged(object sender, EventArgs e)
	{
		wCheckBox_IC_Enable.Checked = false;
		((Control)wcobBox_IC_type).Enabled = false;
		FileManager.SaveLastICType(wcobBox_IC_type.SelectedText);
		vidPidManager.Update(wcobBox_IC_type.SelectedText);
	}

	public void CheckUpgradeFile(string fileName)
	{
		fileHeader = new FileHeader(fileName);
		if (fileHeader.Valid)
		{
			wcobBox_IC_type.SetSelectedText(fileHeader.icName);
			editBootVidPid.SetValue(isChecked: true, fileHeader.bootVid, fileHeader.bootPid);
			wCheckBox_IC_Enable.Enabled = false;
			wCheckBox_IC_Enable.Checked = true;
			((Control)wcobBox_IC_type).Enabled = false;
			((Control)wTextBox_productName).Text = fileHeader.productName;
			((Control)wTextBox_Version).Text = fileHeader.version;
		}
		else
		{
			wCheckBox_IC_Enable.Enabled = true;
			wCheckBox_IC_Enable.Checked = false;
			((Control)wcobBox_IC_type).Enabled = false;
			((Control)wTextBox_productName).Text = "";
			((Control)wTextBox_Version).Text = "";
			wcobBox_IC_type.SetSelectedText(iCSelect.FindIcName(fileName));
		}
	}

	private void wibutton_LoadFile_Click(object sender, EventArgs e)
	{
		formMore.formMore.formMain.loadBinFile.OpenFile(wTextBox_fileName);
		CheckUpgradeFile(((Control)wTextBox_fileName).Text);
	}

	private void wCheckBox_IC_Enable_CheckedChanged(object sender, EventArgs e)
	{
		if (wCheckBox_IC_Enable.Enabled)
		{
			((Control)wcobBox_IC_type).Enabled = wCheckBox_IC_Enable.Checked;
		}
	}

	private void wibutton_SaveUpgradeFile_Click(object sender, EventArgs e)
	{
	}

	private void FormSaveUpgradeFile_Load(object sender, EventArgs e)
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
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Expected O, but got Unknown
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Expected O, but got Unknown
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Expected O, but got Unknown
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Expected O, but got Unknown
		//IL_0488: Unknown result type (might be due to invalid IL or missing references)
		//IL_0492: Expected O, but got Unknown
		//IL_04d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fb: Expected O, but got Unknown
		//IL_063c: Unknown result type (might be due to invalid IL or missing references)
		//IL_075a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0764: Expected O, but got Unknown
		//IL_07a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ea: Expected O, but got Unknown
		//IL_09c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad4: Expected O, but got Unknown
		//IL_0ba9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb3: Expected O, but got Unknown
		//IL_0c88: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c92: Expected O, but got Unknown
		//IL_0d96: Unknown result type (might be due to invalid IL or missing references)
		//IL_0da0: Expected O, but got Unknown
		//IL_135c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1366: Expected O, but got Unknown
		//IL_1550: Unknown result type (might be due to invalid IL or missing references)
		//IL_155a: Expected O, but got Unknown
		//IL_161f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1629: Expected O, but got Unknown
		//IL_179c: Unknown result type (might be due to invalid IL or missing references)
		//IL_19d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_19e2: Expected O, but got Unknown
		//IL_19f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a01: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FormSaveUpgradeFile));
		label_productName = new Label();
		label_Version = new Label();
		wibutton_LoadFile = new WindImageButton();
		wTextBox_fileName = new WindTextBox();
		wTextBox_Version = new WindTextBox();
		wTextBox_productName = new WindTextBox();
		wComboBox_SensorName = new WindComboBox();
		editBootVidPid = new EditValueControl();
		editCidMid = new EditValueControl();
		editNormalVidPid = new EditValueControl();
		wCheckBox_IC_Enable = new WindCheckBox();
		wiButton_mini = new WindImageButton();
		wiButton_Close = new WindImageButton();
		wibutton_SaveUpgradeFile = new WindImageButton();
		label1 = new Label();
		wcobBox_IC_type = new WindComboBox();
		((Control)this).SuspendLayout();
		((Control)label_productName).AutoSize = true;
		((Control)label_productName).BackColor = Color.Transparent;
		((Control)label_productName).Font = new Font("微软雅黑", 15f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)label_productName).Location = new Point(514, 122);
		((Control)label_productName).Name = "label_productName";
		((Control)label_productName).Size = new Size(106, 27);
		((Control)label_productName).TabIndex = 146;
		((Control)label_productName).Text = "名称(可选)";
		((Control)label_Version).AutoSize = true;
		((Control)label_Version).BackColor = Color.Transparent;
		((Control)label_Version).Font = new Font("微软雅黑", 15f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)label_Version).Location = new Point(494, 174);
		((Control)label_Version).Name = "label_Version";
		((Control)label_Version).Size = new Size(126, 27);
		((Control)label_Version).TabIndex = 150;
		((Control)label_Version).Text = "版本号(可选)";
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
		((Control)wibutton_LoadFile).Location = new Point(648, 303);
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
		((Control)wibutton_LoadFile).TabIndex = 154;
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
		((Control)wTextBox_fileName).Location = new Point(165, 305);
		((Control)wTextBox_fileName).Margin = new Padding(6);
		wTextBox_fileName.MaxLength = int.MaxValue;
		((Control)wTextBox_fileName).Name = "wTextBox_fileName";
		wTextBox_fileName.PasswordChar = "";
		wTextBox_fileName.PromptText = "";
		wTextBox_fileName.PromptTextColor = Color.Gray;
		wTextBox_fileName.PromptTextForeColor = Color.LightGray;
		wTextBox_fileName.ReadOnly = true;
		wTextBox_fileName.SelectedBackImage = (Image)(object)Resources.选中文件框;
		((Control)wTextBox_fileName).Size = new Size(460, 40);
		((Control)wTextBox_fileName).TabIndex = 153;
		wTextBox_fileName.TextBoxOffset = new Point(6, 6);
		wTextBox_fileName.UnSelectedBackImage = (Image)(object)Resources.选中文件框;
		((Control)wTextBox_Version).BackgroundImage = (Image)(object)Resources.选中文件框;
		((Control)wTextBox_Version).BackgroundImageLayout = (ImageLayout)3;
		wTextBox_Version.DisableBackImage = null;
		((Control)wTextBox_Version).Font = new Font("微软雅黑", 15f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wTextBox_Version.FrameColor = Color.White;
		wTextBox_Version.InputMode = WindBaseTextBox.TextBoxInputMode.Normal;
		((Control)wTextBox_Version).Location = new Point(629, 166);
		((Control)wTextBox_Version).Margin = new Padding(6);
		wTextBox_Version.MaxLength = int.MaxValue;
		((Control)wTextBox_Version).Name = "wTextBox_Version";
		wTextBox_Version.PasswordChar = "";
		wTextBox_Version.PromptText = "";
		wTextBox_Version.PromptTextColor = Color.Gray;
		wTextBox_Version.PromptTextForeColor = Color.LightGray;
		wTextBox_Version.ReadOnly = false;
		wTextBox_Version.SelectedBackImage = (Image)(object)Resources.选中文件框;
		((Control)wTextBox_Version).Size = new Size(158, 40);
		((Control)wTextBox_Version).TabIndex = 149;
		wTextBox_Version.TextBoxOffset = new Point(6, 6);
		wTextBox_Version.UnSelectedBackImage = (Image)(object)Resources.选中文件框;
		((Control)wTextBox_productName).BackgroundImage = (Image)(object)Resources.选中文件框;
		((Control)wTextBox_productName).BackgroundImageLayout = (ImageLayout)3;
		wTextBox_productName.DisableBackImage = null;
		((Control)wTextBox_productName).Font = new Font("微软雅黑", 15f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wTextBox_productName.FrameColor = Color.White;
		wTextBox_productName.InputMode = WindBaseTextBox.TextBoxInputMode.Normal;
		((Control)wTextBox_productName).Location = new Point(629, 114);
		((Control)wTextBox_productName).Margin = new Padding(6);
		wTextBox_productName.MaxLength = int.MaxValue;
		((Control)wTextBox_productName).Name = "wTextBox_productName";
		wTextBox_productName.PasswordChar = "";
		wTextBox_productName.PromptText = "";
		wTextBox_productName.PromptTextColor = Color.Gray;
		wTextBox_productName.PromptTextForeColor = Color.LightGray;
		wTextBox_productName.ReadOnly = false;
		wTextBox_productName.SelectedBackImage = (Image)(object)Resources.选中文件框;
		((Control)wTextBox_productName).Size = new Size(158, 40);
		((Control)wTextBox_productName).TabIndex = 145;
		wTextBox_productName.TextBoxOffset = new Point(6, 6);
		wTextBox_productName.UnSelectedBackImage = (Image)(object)Resources.选中文件框;
		((Control)wComboBox_SensorName).BackgroundImage = (Image)(object)Resources.下拉正常状态;
		((Control)wComboBox_SensorName).BackgroundImageLayout = (ImageLayout)3;
		wComboBox_SensorName.DropDownLoctionOffset = new Point(0, 0);
		wComboBox_SensorName.DropDownMaxRowCount = 6;
		wComboBox_SensorName.DropDownWidthDelta = 0;
		((Control)wComboBox_SensorName).Font = new Font("微软雅黑", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)wComboBox_SensorName).ForeColor = Color.Green;
		wComboBox_SensorName.FrameColor = Color.Gray;
		wComboBox_SensorName.FrameWidth = 1;
		wComboBox_SensorName.IconMouseDownColor = Color.Gray;
		wComboBox_SensorName.IconMouseDownName = "";
		wComboBox_SensorName.IconMouseEnterColor = Color.DarkGray;
		wComboBox_SensorName.IconMouseEnterName = "";
		wComboBox_SensorName.IconNormalColor = Color.DimGray;
		wComboBox_SensorName.IconNormalName = "";
		wComboBox_SensorName.IconSize = 36;
		wComboBox_SensorName.Items.Add("3395");
		((Control)wComboBox_SensorName).Location = new Point(630, 65);
		((Control)wComboBox_SensorName).Margin = new Padding(4, 5, 4, 5);
		wComboBox_SensorName.MouseDownImage = (Image)(object)Resources.下拉鼠标点击状态;
		wComboBox_SensorName.MouseEnterImage = (Image)(object)Resources.下拉鼠标指过去状态;
		wComboBox_SensorName.MovingSelectedBackColor = Color.LightSkyBlue;
		((Control)wComboBox_SensorName).Name = "wComboBox_SensorName";
		wComboBox_SensorName.NormalImage = (Image)(object)Resources.下拉正常状态;
		wComboBox_SensorName.ReadOnly = true;
		((Control)wComboBox_SensorName).RightToLeft = (RightToLeft)0;
		wComboBox_SensorName.SelectedIndex = 0;
		wComboBox_SensorName.SelectedText = "3395";
		((Control)wComboBox_SensorName).Size = new Size(157, 33);
		wComboBox_SensorName.SplitLineColor = Color.LightGray;
		((Control)wComboBox_SensorName).TabIndex = 144;
		((Control)editBootVidPid).BackColor = Color.Transparent;
		editBootVidPid.CheckedText = "  Edit Boot VID/PID";
		((Control)editBootVidPid).Font = new Font("微软雅黑", 10.5f);
		editBootVidPid.isChecked = false;
		editBootVidPid.LeftLabelText = "VID";
		((Control)editBootVidPid).Location = new Point(160, 136);
		((Control)editBootVidPid).Name = "editBootVidPid";
		editBootVidPid.RightLabelText = "PID";
		((Control)editBootVidPid).Size = new Size(368, 77);
		((Control)editBootVidPid).TabIndex = 142;
		editBootVidPid.UnCheckedText = " Default Boot VID/PID";
		editBootVidPid.xUI = false;
		((Control)editCidMid).BackColor = Color.Transparent;
		editCidMid.CheckedText = "  Edit CID/MID";
		((Control)editCidMid).Font = new Font("微软雅黑", 10.5f);
		editCidMid.isChecked = false;
		editCidMid.LeftLabelText = "VID";
		((Control)editCidMid).Location = new Point(160, 219);
		((Control)editCidMid).Name = "editCidMid";
		editCidMid.RightLabelText = "PID";
		((Control)editCidMid).Size = new Size(368, 77);
		((Control)editCidMid).TabIndex = 136;
		editCidMid.UnCheckedText = " Default CID/MID";
		editCidMid.xUI = false;
		((Control)editNormalVidPid).BackColor = Color.Transparent;
		editNormalVidPid.CheckedText = "  Edit Normal VID/PID";
		((Control)editNormalVidPid).Font = new Font("微软雅黑", 10.5f);
		editNormalVidPid.isChecked = false;
		editNormalVidPid.LeftLabelText = "VID";
		((Control)editNormalVidPid).Location = new Point(160, 53);
		((Control)editNormalVidPid).Name = "editNormalVidPid";
		editNormalVidPid.RightLabelText = "PID";
		((Control)editNormalVidPid).Size = new Size(368, 77);
		((Control)editNormalVidPid).TabIndex = 135;
		editNormalVidPid.UnCheckedText = " Default Normal VID/PID";
		editNormalVidPid.xUI = false;
		((Control)wCheckBox_IC_Enable).BackColor = Color.Transparent;
		((Control)wCheckBox_IC_Enable).BackgroundImageLayout = (ImageLayout)0;
		wCheckBox_IC_Enable.Checked = false;
		wCheckBox_IC_Enable.DisableSelectedImage = (Image)(object)Resources.checkbox_3;
		wCheckBox_IC_Enable.DisableUnSelectedImage = (Image)(object)Resources.checkbox_2;
		((Control)wCheckBox_IC_Enable).Font = new Font("微软雅黑", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wCheckBox_IC_Enable.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wCheckBox_IC_Enable.IconOffset = new Point(0, 0);
		wCheckBox_IC_Enable.IconSize = 36;
		((Control)wCheckBox_IC_Enable).Location = new Point(11, 56);
		((Control)wCheckBox_IC_Enable).Name = "wCheckBox_IC_Enable";
		wCheckBox_IC_Enable.SelectedIconColor = Color.Red;
		wCheckBox_IC_Enable.SelectedIconName = "";
		wCheckBox_IC_Enable.SelectedImage = (Image)(object)Resources.checkbox_1;
		((Control)wCheckBox_IC_Enable).Size = new Size(30, 31);
		((Control)wCheckBox_IC_Enable).TabIndex = 129;
		((Control)wCheckBox_IC_Enable).Text = " ";
		wCheckBox_IC_Enable.TextOffset = new Point(4, 4);
		wCheckBox_IC_Enable.UnSelectedIconColor = Color.Gray;
		wCheckBox_IC_Enable.UnSelectedIconName = "";
		wCheckBox_IC_Enable.UnSelectedImage = (Image)(object)Resources.checkbox_2;
		wCheckBox_IC_Enable.CheckedChanged += wCheckBox_IC_Enable_CheckedChanged;
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
		((Control)wibutton_SaveUpgradeFile).BackColor = Color.Transparent;
		((Control)wibutton_SaveUpgradeFile).BackgroundImage = (Image)(object)Resources.button_1;
		((Control)wibutton_SaveUpgradeFile).BackgroundImageLayout = (ImageLayout)3;
		wibutton_SaveUpgradeFile.DisableBackColor = Color.Transparent;
		wibutton_SaveUpgradeFile.DisableForeColor = Color.DarkGray;
		wibutton_SaveUpgradeFile.DisableImage = (Image)(object)Resources.button_2;
		((Control)wibutton_SaveUpgradeFile).Font = new Font("微软雅黑", 15f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wibutton_SaveUpgradeFile.FrameMode = GraphicsHelper.RoundStyle.All;
		wibutton_SaveUpgradeFile.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wibutton_SaveUpgradeFile.IconName = "";
		wibutton_SaveUpgradeFile.IconOffset = new Point(0, 0);
		wibutton_SaveUpgradeFile.IconSize = 32;
		((Control)wibutton_SaveUpgradeFile).Location = new Point(648, 357);
		wibutton_SaveUpgradeFile.MouseDownBackColor = Color.Gray;
		wibutton_SaveUpgradeFile.MouseDownForeColor = Color.Black;
		wibutton_SaveUpgradeFile.MouseDownImage = (Image)(object)Resources.UpgradeButtonSuccessDownImage;
		wibutton_SaveUpgradeFile.MouseEnterBackColor = Color.DarkGray;
		wibutton_SaveUpgradeFile.MouseEnterForeColor = Color.Black;
		wibutton_SaveUpgradeFile.MouseEnterImage = (Image)(object)Resources.UpgradeButtonSuccessEnterImage;
		wibutton_SaveUpgradeFile.MouseUpBackColor = Color.Transparent;
		wibutton_SaveUpgradeFile.MouseUpForeColor = Color.Black;
		wibutton_SaveUpgradeFile.MouseUpImage = (Image)(object)Resources.UpgradeButtonSuccessImage;
		((Control)wibutton_SaveUpgradeFile).Name = "wibutton_SaveUpgradeFile";
		wibutton_SaveUpgradeFile.Radius = 12;
		((Control)wibutton_SaveUpgradeFile).Size = new Size(114, 60);
		((Control)wibutton_SaveUpgradeFile).TabIndex = 45;
		((Control)wibutton_SaveUpgradeFile).Text = "Save……";
		wibutton_SaveUpgradeFile.TextAlignment = StringHelper.TextAlignment.Center;
		wibutton_SaveUpgradeFile.TextDynOffset = new Point(0, 0);
		wibutton_SaveUpgradeFile.TextFixLocation = new Point(0, 0);
		wibutton_SaveUpgradeFile.TextFixLocationEnable = false;
		((Control)wibutton_SaveUpgradeFile).Click += wibutton_SaveUpgradeFile_Click;
		((Control)label1).AutoSize = true;
		((Control)label1).BackColor = Color.Transparent;
		((Control)label1).Font = new Font("微软雅黑", 15f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)label1).Location = new Point(497, 68);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(126, 27);
		((Control)label1).TabIndex = 159;
		((Control)label1).Text = "传感器(可选)";
		((Control)wcobBox_IC_type).BackgroundImage = (Image)(object)Resources.下拉正常状态;
		((Control)wcobBox_IC_type).BackgroundImageLayout = (ImageLayout)3;
		wcobBox_IC_type.DropDownLoctionOffset = new Point(-33, 0);
		wcobBox_IC_type.DropDownMaxRowCount = 8;
		wcobBox_IC_type.DropDownWidthDelta = 36;
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
		((Control)wcobBox_IC_type).Location = new Point(45, 58);
		((Control)wcobBox_IC_type).Margin = new Padding(4, 5, 4, 5);
		wcobBox_IC_type.MouseDownImage = (Image)(object)Resources.下拉鼠标点击状态;
		wcobBox_IC_type.MouseEnterImage = (Image)(object)Resources.下拉鼠标指过去状态;
		wcobBox_IC_type.MovingSelectedBackColor = Color.LightSkyBlue;
		((Control)wcobBox_IC_type).Name = "wcobBox_IC_type";
		wcobBox_IC_type.NormalImage = (Image)(object)Resources.下拉正常状态;
		wcobBox_IC_type.ReadOnly = true;
		((Control)wcobBox_IC_type).RightToLeft = (RightToLeft)0;
		wcobBox_IC_type.SelectedIndex = 0;
		wcobBox_IC_type.SelectedText = "CX52850";
		((Control)wcobBox_IC_type).Size = new Size(106, 27);
		wcobBox_IC_type.SplitLineColor = Color.LightGray;
		((Control)wcobBox_IC_type).TabIndex = 163;
		wcobBox_IC_type.SelectedIndexChanged += wcobBox_IC_type_SelectedIndexChanged;
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)0;
		((Control)this).BackgroundImage = (Image)(object)Resources.cidmid;
		((Form)this).ClientSize = new Size(802, 421);
		((Control)this).Controls.Add((Control)(object)wcobBox_IC_type);
		((Control)this).Controls.Add((Control)(object)label1);
		((Control)this).Controls.Add((Control)(object)wibutton_LoadFile);
		((Control)this).Controls.Add((Control)(object)wTextBox_fileName);
		((Control)this).Controls.Add((Control)(object)label_Version);
		((Control)this).Controls.Add((Control)(object)wTextBox_Version);
		((Control)this).Controls.Add((Control)(object)label_productName);
		((Control)this).Controls.Add((Control)(object)wTextBox_productName);
		((Control)this).Controls.Add((Control)(object)wComboBox_SensorName);
		((Control)this).Controls.Add((Control)(object)editBootVidPid);
		((Control)this).Controls.Add((Control)(object)editCidMid);
		((Control)this).Controls.Add((Control)(object)editNormalVidPid);
		((Control)this).Controls.Add((Control)(object)wCheckBox_IC_Enable);
		((Control)this).Controls.Add((Control)(object)wiButton_mini);
		((Control)this).Controls.Add((Control)(object)wiButton_Close);
		((Control)this).Controls.Add((Control)(object)wibutton_SaveUpgradeFile);
		((Control)this).Font = new Font("微软雅黑", 10.5f);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Form)this).KeyPreview = true;
		((Control)this).Name = "FormSaveUpgradeFile";
		((Form)this).StartPosition = (FormStartPosition)0;
		((Control)this).Text = "EditCidMid";
		((Form)this).Load += FormSaveUpgradeFile_Load;
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
