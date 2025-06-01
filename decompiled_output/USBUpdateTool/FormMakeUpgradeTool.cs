using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using USBUpdateTool.Properties;
using USBUpdateTool.UpgradeFile;
using WindControls;

namespace USBUpdateTool;

public class FormMakeUpgradeTool : Form
{
	private bool UserLocker = false;

	public FormMore formMore;

	private VidPidManager vidPidManager = new VidPidManager();

	private ICSelect iCSelect = new ICSelect();

	private OpenFileDialog openFileDialog = new OpenFileDialog();

	private SaveFileDialog saveFileDialog = new SaveFileDialog();

	public SkinForm skinForm = new SkinForm(_movable: true);

	public Image customBackImage = (Image)(object)Resources.UpgradeTool;

	private ExeResourceManager exeResourceManager = new ExeResourceManager();

	private AppConfig appConfig = new AppConfig();

	private ColorDialog colorDialog;

	private FontDialog fontDialog;

	private FormProgressStyle formProgressStyle;

	private FormButtonSetting formButtonSetting;

	private UpgradeControlLocation controlLocation;

	private LoadBinFile loadBinFile = new LoadBinFile();

	private List<Control> controlList = new List<Control>();

	private List<ControlMove> controlCMList = new List<ControlMove>();

	private List<WindProgressBar> progressBarList = new List<WindProgressBar>();

	private List<ControlMove> progressBarCMList = new List<ControlMove>();

	private Control fontControl;

	private IContainer components = null;

	private WindImageButton wiButton_Mini;

	public WindImageButton wiButton_Close;

	private PictureBox pictureBox_icon;

	private WindTextBox wTextBox_caption;

	private WindTextBox wTextBox_curFwVersion;

	private WindTextBox wTextBox_newFwVersion;

	private WindTextBox wTextBox_ToolVersion;

	private WindImageButton wiButton_createExe;

	private WindImageButton wiButton_custom_mini;

	private WindImageButton wiButton_custom_close;

	private WindImageButton wibutton_StartUpgrade;

	private PictureBox pictureBox1;

	public WindCheckBox wCheckBox_IC_Enable;

	public WindComboBox wcobBox_IC_type;

	private WindImageButton wIButton_upgradetool;

	private WindImageButton wiButton_Icon;

	private WindImageButton wiButton_textColor;

	private WindImageButton wiButton_buttonSet;

	private WindImageButton wiButton_load_code;

	private EditValueControl editCidMid;

	private WindTextBox wTextBox_fileName;

	private EditValueControl editBootVidPid;

	private EditValueControl editNormalVidPid;

	private WindImageButton wImageButton_progressStyle;

	private HaloBar haloBar1;

	private SliderBar sliderBar1;

	private RoundBar roundBar1;

	private DragControlRect dragControlRect1;

	private WindImageButton wImageButton_loadConfig;

	private CellBar cellBar1;

	private WindImageButton wiButton_textFont;

	private WindImageButton wibutton_FailStartUpgrade;

	private ToolTip toolTip1;

	private WindImageButton wImageButton_saveConfig;

	private ImageBar imageBar1;

	private TipControl tipControl1;

	private WindImageButton wIButton_showHide;

	private Label label2;

	public WindTextBox wTextBox_ExeCaption;

	public FormMakeUpgradeTool(FormMore _formMore)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		//IL_037e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0388: Expected O, but got Unknown
		InitializeComponent();
		formMore = _formMore;
		SetStyles();
		appConfig.fileConfig.exeType = EXE_TYPE.UPGRADE;
		((Control)haloBar1).Location = new Point(220, 265);
		((Control)roundBar1).Location = new Point(220, 268);
		((Control)sliderBar1).Location = new Point(192, 230);
		((Control)cellBar1).Location = new Point(220, 270);
		((Control)imageBar1).Location = new Point(200, 232);
		((Control)tipControl1).Location = new Point(24, 96);
		skinForm.InitSkin((Form)(object)this, 16, 24);
		appConfig.SetOffset(((Control)pictureBox1).Location);
		controlLocation = new UpgradeControlLocation(this, ((Control)pictureBox1).Location);
		Rectangle rect = new Rectangle(((Control)pictureBox1).Location, ((Control)pictureBox1).Size);
		controlList.Clear();
		controlList.Add((Control)(object)wiButton_custom_mini);
		controlList.Add((Control)(object)wiButton_custom_close);
		controlList.Add((Control)(object)wibutton_StartUpgrade);
		controlList.Add((Control)(object)wTextBox_caption);
		controlList.Add((Control)(object)wTextBox_curFwVersion);
		controlList.Add((Control)(object)wTextBox_newFwVersion);
		controlList.Add((Control)(object)wTextBox_ToolVersion);
		controlCMList.Clear();
		for (int i = 0; i < controlList.Count; i++)
		{
			controlCMList.Add(new ControlMove(controlList[i], rect));
			controlLocation.AddControl(controlList[i]);
		}
		progressBarList.Clear();
		progressBarList.Add(haloBar1);
		progressBarList.Add(roundBar1);
		progressBarList.Add(sliderBar1);
		progressBarList.Add(cellBar1);
		progressBarList.Add(imageBar1);
		progressBarCMList.Clear();
		for (int j = 0; j < progressBarList.Count; j++)
		{
			progressBarCMList.Add(new ControlMove((Control)(object)progressBarList[j], rect));
			controlLocation.AddControl((Control)(object)progressBarList[j]);
		}
		controlLocation.SetBackImage((Image)new Bitmap(customBackImage));
		appConfig.imageConfig.multiLogoIcon = LiteResources.GetMultiLogoIconBytes();
		appConfig.imageConfig.BackgroundImage = customBackImage;
		appConfig.fileConfig.fileArray = FileHelper.GetResourceBytes("MakeUpgradeTool.Files.CompxUpgradeBinFile.bin");
		UserLockerUI(enable: false);
		((Control)dragControlRect1).Width = 16;
		((Control)dragControlRect1).Height = 16;
	}

	private void InitVidPid()
	{
		wcobBox_IC_type.Clear();
		wcobBox_IC_type.Add(ICSelect.IC_ITEMs);
		wcobBox_IC_type.SetSelectedText(FileManager.GetLastICType());
		vidPidManager.icName = wcobBox_IC_type.SelectedText;
		editNormalVidPid.SetXUI(240, "Edit Normal Vid/Pid", "Default Normal Vid/Pid");
		editBootVidPid.SetXUI(240, "Edit Boot Vid/Pid", "Default Boot Vid/Pid");
		editCidMid.SetXUI(240, "Edit Cid/Mid (10进制)", "Default Cid/Mid (10进制)");
		vidPidManager.SetEditValueControls(editNormalVidPid, editBootVidPid, editCidMid);
		vidPidManager.Init();
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
		((FileDialog)openFileDialog).Filter = "Png Files|*.png";
		((FileDialog)openFileDialog).FilterIndex = 1;
		openFileDialog.Multiselect = false;
		((FileDialog)openFileDialog).RestoreDirectory = true;
		((FileDialog)saveFileDialog).Filter = "exe File|*.exe";
		((FileDialog)saveFileDialog).FilterIndex = 1;
		((FileDialog)saveFileDialog).RestoreDirectory = true;
	}

	private void wiButton_Close_Click(object sender, EventArgs e)
	{
		skinForm.AllHide();
		formMore.skinForm.AllShow();
	}

	private void wiButton_Mini_Click(object sender, EventArgs e)
	{
		((Form)skinForm).WindowState = (FormWindowState)1;
		((Form)this).WindowState = (FormWindowState)1;
	}

	public void ShowWindow()
	{
		((Form)this).Location = ((Form)formMore).Location;
		skinForm.AllShow((Form)(object)formMore);
		loadBinFile.LoadLastFile(wTextBox_fileName);
		wCheckBox_IC_Enable.Checked = false;
	}

	private void FormMakeUpgradeTool_Paint(object sender, PaintEventArgs e)
	{
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Expected O, but got Unknown
		if (customBackImage != null)
		{
			e.Graphics.SetGDIHigh();
			e.Graphics.DrawImage(customBackImage, ((Control)pictureBox1).Location.X, ((Control)pictureBox1).Location.Y, ResizeControl.GetScaleValue(customBackImage.Width), ResizeControl.GetScaleValue(customBackImage.Height));
		}
		int num = 4;
		float[] dashPattern = new float[2] { 4f, 4.1f };
		Pen val = new Pen(Color.DarkGray, (float)num);
		val.DashPattern = dashPattern;
		int x = ((Control)pictureBox1).Location.X;
		int y = ((Control)pictureBox1).Location.Y;
		e.Graphics.DrawLine(val, new Point(x, y), new Point(x, y + ((Control)pictureBox1).Height));
		e.Graphics.DrawLine(val, new Point(x, y), new Point(x + ((Control)pictureBox1).Width, y));
		e.Graphics.DrawLine(val, new Point(x, y + ((Control)pictureBox1).Height), new Point(x + ((Control)pictureBox1).Width, y + ((Control)pictureBox1).Height));
		e.Graphics.DrawLine(val, new Point(x + ((Control)pictureBox1).Width, y), new Point(x + ((Control)pictureBox1).Width, y + ((Control)pictureBox1).Height));
	}

	private void wiButton_textColor_Click(object sender, EventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Invalid comparison between Unknown and I4
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		if (colorDialog == null)
		{
			colorDialog = new ColorDialog();
		}
		if ((int)((CommonDialog)colorDialog).ShowDialog() == 1 && fontControl != null)
		{
			fontControl.ForeColor = colorDialog.Color;
			if (fontControl.Name == "haloBar1" || fontControl.Name == "roundBar1" || fontControl.Name == "sliderBar1" || fontControl.Name == "cellBar1" || fontControl.Name == "imageBar1")
			{
				((WindProgressBar)(object)fontControl).TextColor = colorDialog.Color;
			}
			if (((object)fontControl).GetType().Name == "WindImageButton")
			{
				((WindImageButton)(object)fontControl).SetMouseForeColor(colorDialog.Color);
			}
		}
	}

	private void wiButton_textFont_Click(object sender, EventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Invalid comparison between Unknown and I4
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		if (fontDialog == null)
		{
			fontDialog = new FontDialog();
		}
		if ((int)((CommonDialog)fontDialog).ShowDialog() == 1 && fontControl != null)
		{
			fontControl.Font = fontDialog.Font;
		}
	}

	private void wIButton_upgradetool_Click(object sender, EventArgs e)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Invalid comparison between Unknown and I4
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		((FileDialog)openFileDialog).Filter = "Png Files|*.png";
		if ((int)((CommonDialog)openFileDialog).ShowDialog() == 1)
		{
			appConfig.imageConfig.BackgroundImage = ImageHelper.GetImage(((FileDialog)openFileDialog).FileName);
			customBackImage = ImageHelper.GetImage(((FileDialog)openFileDialog).FileName);
			controlLocation.SetBackImage((Image)new Bitmap(customBackImage));
			wTextBox_caption.UserInvalidate();
			wTextBox_curFwVersion.UserInvalidate();
			wTextBox_newFwVersion.UserInvalidate();
			wTextBox_ToolVersion.UserInvalidate();
			((Control)this).Invalidate();
		}
	}

	private void wiButton_Icon_Click(object sender, EventArgs e)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Invalid comparison between Unknown and I4
		((FileDialog)openFileDialog).Filter = "ICO Files|*.ico";
		if ((int)((CommonDialog)openFileDialog).ShowDialog() == 1)
		{
			pictureBox_icon.Load(((FileDialog)openFileDialog).FileName);
			Size imageSize = ImageHelper.GetImageSize(((FileDialog)openFileDialog).FileName);
			appConfig.iconFileName = ((FileDialog)openFileDialog).FileName;
			appConfig.imageConfig.multiLogoIcon = File.ReadAllBytes(((FileDialog)openFileDialog).FileName);
		}
	}

	private void GetControl()
	{
		appConfig.UpgradeDevice.SetICName(wcobBox_IC_type.SelectedText);
		appConfig.MiniButton.SetControl((Control)(object)wiButton_custom_mini);
		appConfig.MiniButton.SetColors(((Control)wiButton_custom_mini).ForeColor, wiButton_custom_mini.MouseUpForeColor, wiButton_custom_mini.MouseDownBackColor);
		appConfig.CloseButton.SetControl((Control)(object)wiButton_custom_close);
		appConfig.CloseButton.SetColors(((Control)wiButton_custom_close).ForeColor, wiButton_custom_mini.MouseUpForeColor, wiButton_custom_mini.MouseDownBackColor);
		appConfig.CaptionTextBox.SetControl((Control)(object)wTextBox_caption);
		appConfig.CaptionTextBox.SetColors(((Control)wTextBox_caption).ForeColor, ((Control)wTextBox_caption).ForeColor, ((Control)wTextBox_caption).BackColor);
		appConfig.CurFwVersionTextBox.SetControl((Control)(object)wTextBox_curFwVersion);
		appConfig.CurFwVersionTextBox.SetColors(((Control)wTextBox_curFwVersion).ForeColor, ((Control)wTextBox_curFwVersion).ForeColor, ((Control)wTextBox_curFwVersion).BackColor);
		appConfig.NewFwVersionTextBox.SetControl((Control)(object)wTextBox_newFwVersion);
		appConfig.NewFwVersionTextBox.SetColors(((Control)wTextBox_newFwVersion).ForeColor, ((Control)wTextBox_newFwVersion).ForeColor, ((Control)wTextBox_newFwVersion).BackColor);
		appConfig.ToolVersionTextBox.SetControl((Control)(object)wTextBox_ToolVersion);
		appConfig.ToolVersionTextBox.SetColors(((Control)wTextBox_ToolVersion).ForeColor, ((Control)wTextBox_ToolVersion).ForeColor, ((Control)wTextBox_ToolVersion).BackColor);
		appConfig.SuccessUpgradeButton.style = ((wibutton_StartUpgrade.MouseUpImage != null) ? 1 : 0);
		appConfig.SuccessUpgradeButton.SetControl((Control)(object)wibutton_StartUpgrade);
		appConfig.SuccessUpgradeButton.SetColors(((Control)wibutton_StartUpgrade).ForeColor, wibutton_StartUpgrade.MouseUpForeColor, wibutton_StartUpgrade.MouseDownBackColor);
		appConfig.FailUpgradeButton.style = appConfig.SuccessUpgradeButton.style;
		appConfig.FailUpgradeButton.SetControl((Control)(object)wibutton_FailStartUpgrade);
		appConfig.FailUpgradeButton.SetColors(((Control)wibutton_FailStartUpgrade).ForeColor, wibutton_FailStartUpgrade.MouseUpForeColor, wibutton_FailStartUpgrade.MouseDownBackColor);
		appConfig.imageConfig.UpgradeMouseUpImage = wibutton_StartUpgrade.MouseUpImage;
		appConfig.imageConfig.UpgradeMouseEnterImage = wibutton_StartUpgrade.MouseEnterImage;
		appConfig.imageConfig.UpgradeMouseDownImage = wibutton_StartUpgrade.MouseDownImage;
		appConfig.imageConfig.UpgradeFailMouseUpImage = wibutton_FailStartUpgrade.MouseUpImage;
		appConfig.imageConfig.UpgradeFailMouseEnterImage = wibutton_FailStartUpgrade.MouseEnterImage;
		appConfig.imageConfig.UpgradeFailMouseDownImage = wibutton_FailStartUpgrade.MouseDownImage;
		appConfig.imageConfig.ProgressSliderImage = imageBar1.BarSliderImage;
		appConfig.imageConfig.ProgressForeImage = imageBar1.BarForeImage;
		appConfig.imageConfig.ProgressBackImage = imageBar1.BarBackImage;
		appConfig.imageConfig.PorgressTextLocation = imageBar1.TextLocation;
		appConfig.imageConfig.PorgressSliderImageRect = imageBar1.BarSliderImageRect;
		appConfig.imageConfig.PorgressForeImageRect = imageBar1.BarForeImageRect;
		appConfig.imageConfig.PorgressBackImageRect = imageBar1.BarBackImageRect;
		appConfig.imageConfig.PorgressSliderMovable = imageBar1.BarSliderMovable;
		appConfig.imageConfig.PorgressShowPercent = imageBar1.ShowPercent;
		for (int i = 0; i < progressBarList.Count; i++)
		{
			if (((Control)progressBarList[i]).Visible)
			{
				appConfig.ProgressControl.style = (int)progressBarList[i].BarStyle;
				appConfig.ProgressControl.SetControl((Control)(object)progressBarList[i]);
				appConfig.ProgressControl.SetColors(progressBarList[i].TextColor, progressBarList[i].BarForeColor, progressBarList[i].BarBackColor);
				break;
			}
		}
	}

	private void wiButton_createExe_Click(object sender, EventArgs e)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Invalid comparison between Unknown and I4
		((FileDialog)saveFileDialog).Filter = "exe File|*.exe";
		((FileDialog)saveFileDialog).FilterIndex = 1;
		((FileDialog)saveFileDialog).RestoreDirectory = true;
		if ((int)((CommonDialog)saveFileDialog).ShowDialog() != 1)
		{
			return;
		}
		if (iCSelect.FindIcName(((Control)wTextBox_fileName).Text) != wcobBox_IC_type.SelectedText)
		{
			new FormMessageBox("The file does not match the selected IC！").Show((Form)(object)this);
			return;
		}
		if (!File.Exists(((Control)wTextBox_fileName).Text))
		{
			new FormMessageBox("Upgrade file does not exist").Show((Form)(object)this);
			return;
		}
		((Control)wiButton_Close).Enabled = false;
		byte[] sourceFile = File.ReadAllBytes(((Control)wTextBox_fileName).Text);
		appConfig.UpgradeDevice.SetNormalVidPid(editNormalVidPid.GetLeftValue(), editNormalVidPid.GetRightValue());
		appConfig.UpgradeDevice.SetBootVidPid(editBootVidPid.GetLeftValue(), editBootVidPid.GetRightValue());
		appConfig.UpgradeDevice.SetCidMid(editCidMid.GetLeftValue(), editCidMid.GetRightValue());
		string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(((Control)wTextBox_fileName).Text);
		appConfig.fileConfig.fileArray = iCSelect.CreateUpgradeFile(fileNameWithoutExtension, sourceFile, wcobBox_IC_type.SelectedText, editNormalVidPid.GetLeftValue(), editNormalVidPid.GetRightValue(), editBootVidPid.GetLeftValue(), editBootVidPid.GetRightValue());
		if (((Control)wTextBox_ExeCaption).Text != "")
		{
			appConfig.fileConfig.SetFile(EXE_TYPE.UPGRADE, fileNameWithoutExtension, ((Control)wTextBox_ExeCaption).Text);
		}
		else
		{
			appConfig.fileConfig.SetFile(EXE_TYPE.UPGRADE, fileNameWithoutExtension, ((Control)wTextBox_caption).Text);
		}
		if (appConfig.fileConfig.fileArray == null)
		{
			new FormMessageBox("Bin File null").Show((Form)(object)this);
			((Control)wiButton_Close).Enabled = true;
			return;
		}
		GetControl();
		new Thread(((Action)delegate
		{
			if (!exeResourceManager.BulidExe(appConfig, ((FileDialog)saveFileDialog).FileName))
			{
				new FormMessageBox("Create Fail").Show((Form)(object)this);
			}
			((Control)this).Invoke((Delegate)(Action)delegate
			{
				((Control)wiButton_Close).Enabled = true;
			});
		}).Invoke).Start();
	}

	private void wiButton_load_code_Click(object sender, EventArgs e)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Invalid comparison between Unknown and I4
		((FileDialog)openFileDialog).Filter = "Bin Files|*.bin|fwpkg Files|*.fwpkg";
		if ((int)((CommonDialog)openFileDialog).ShowDialog() == 1)
		{
			FileHeader fileHeader = new FileHeader(((FileDialog)openFileDialog).FileName);
			if (fileHeader.Valid)
			{
				new FormMessageBox("该文件为转换后的文件，请选择源文件！").Show((Form)(object)this);
			}
			else if (iCSelect.FindIcName(((FileDialog)openFileDialog).FileName) != wcobBox_IC_type.SelectedText)
			{
				new FormMessageBox("The file does not match the selected IC！").Show((Form)(object)this);
			}
			else
			{
				((Control)wTextBox_fileName).Text = ((FileDialog)openFileDialog).FileName;
			}
		}
	}

	private void wCheckBox_IC_Enable_CheckedChanged(object sender, EventArgs e)
	{
		if (wCheckBox_IC_Enable.Enabled)
		{
			((Control)wcobBox_IC_type).Enabled = wCheckBox_IC_Enable.Checked;
		}
		if (!UserLocker)
		{
			UserLocker = true;
			((Control)tipControl1).Visible = false;
			UserLockerUI(enable: true);
			vidPidManager.Update(wcobBox_IC_type.SelectedText);
		}
	}

	private void UserLockerUI(bool enable)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
		{
			Control val = item;
			val.Enabled = enable;
		}
		((Control)pictureBox_icon).Visible = enable;
		((Control)wiButton_Mini).Enabled = true;
		((Control)wiButton_Close).Enabled = true;
		wCheckBox_IC_Enable.Enabled = true;
		((Control)tipControl1).Enabled = true;
		((Control)wiButton_textFont).Enabled = ((Control)dragControlRect1).Visible;
		((Control)wiButton_textColor).Enabled = ((Control)wiButton_textFont).Enabled;
		((Control)wIButton_showHide).Enabled = ((Control)wiButton_textFont).Enabled;
	}

	private void wcobBox_IC_type_SelectedIndexChanged(object sender, EventArgs e)
	{
		wCheckBox_IC_Enable.Checked = false;
		((Control)wcobBox_IC_type).Enabled = false;
		FileManager.SaveLastICType(wcobBox_IC_type.SelectedText);
		vidPidManager.Update(wcobBox_IC_type.SelectedText);
	}

	private void FormMakeUpgradeTool_Load(object sender, EventArgs e)
	{
		InitDialog();
		InitVidPid();
	}

	private WindProgressBar GetWindProgressBar()
	{
		for (int i = 0; i < progressBarList.Count; i++)
		{
			if (((Control)progressBarList[i]).Visible)
			{
				return progressBarList[i];
			}
		}
		return haloBar1;
	}

	private void wImageButton_progressStyle_Click(object sender, EventArgs e)
	{
		if (formProgressStyle == null)
		{
			formProgressStyle = new FormProgressStyle(progressBarList);
		}
		WindProgressBar windProgressBar = GetWindProgressBar();
		formProgressStyle.SetProgressBar(windProgressBar.TextColor, windProgressBar.BarForeColor, windProgressBar.BarBackColor, imageBar1);
		formProgressStyle.ShowWindow((Form)(object)this);
	}

	private void wiButton_buttonSet_Click(object sender, EventArgs e)
	{
		if (formButtonSetting == null)
		{
			formButtonSetting = new FormButtonSetting(wibutton_StartUpgrade, wibutton_FailStartUpgrade);
		}
		formButtonSetting.SetButtons(wibutton_StartUpgrade, wibutton_FailStartUpgrade, wibutton_StartUpgrade, wibutton_FailStartUpgrade);
		formButtonSetting.ShowWindow((Form)(object)this);
	}

	private void DragRect_MouseDown(object sender, MouseEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		((Control)dragControlRect1).Visible = false;
		fontControl = (Control)sender;
	}

	private void DragRect_MouseUp(object sender, MouseEventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		dragControlRect1.SetControl((Control)sender);
	}

	protected override bool ProcessDialogKey(Keys keyData)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Invalid comparison between Unknown and I4
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Invalid comparison between Unknown and I4
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Invalid comparison between Unknown and I4
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Invalid comparison between Unknown and I4
		if ((int)keyData == 38 || (int)keyData == 40 || (int)keyData == 37 || (int)keyData == 39)
		{
			return false;
		}
		return ((Form)this).ProcessDialogKey(keyData);
	}

	private void FormMakeUpgradeTool_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Invalid comparison between Unknown and I4
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Invalid comparison between Unknown and I4
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Invalid comparison between Unknown and I4
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Invalid comparison between Unknown and I4
		if (((Control)dragControlRect1).Visible)
		{
			Point location = ((Control)dragControlRect1).Location;
			if ((int)e.KeyCode == 38)
			{
				location.Y--;
				((Control)dragControlRect1).Location = location;
				dragControlRect1.UpdateControl(0, -1);
			}
			else if ((int)e.KeyCode == 40)
			{
				location.Y++;
				((Control)dragControlRect1).Location = location;
				dragControlRect1.UpdateControl(0, 1);
			}
			else if ((int)e.KeyCode == 37)
			{
				location.X--;
				((Control)dragControlRect1).Location = location;
				dragControlRect1.UpdateControl(-1, 0);
			}
			else if ((int)e.KeyCode == 39)
			{
				location.X++;
				((Control)dragControlRect1).Location = location;
				dragControlRect1.UpdateControl(1, 0);
			}
		}
	}

	private void GetFont(Control control)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Invalid comparison between Unknown and I4
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		if (fontDialog == null)
		{
			fontDialog = new FontDialog();
		}
		if ((int)((CommonDialog)fontDialog).ShowDialog() == 1)
		{
			control.Font = fontDialog.Font;
		}
	}

	private void selectFontToolStripMenuItem_Click(object sender, EventArgs e)
	{
		if (fontControl != null)
		{
			GetFont(fontControl);
		}
	}

	private void wImageButton_saveConfig_Click(object sender, EventArgs e)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Invalid comparison between Unknown and I4
		((FileDialog)saveFileDialog).Filter = "TxT File|*.txt";
		((FileDialog)saveFileDialog).FilterIndex = 1;
		((FileDialog)saveFileDialog).RestoreDirectory = true;
		if ((int)((CommonDialog)saveFileDialog).ShowDialog() == 1)
		{
			appConfig.UpgradeDevice.SetNormalVidPid(editNormalVidPid.GetLeftValue(), editNormalVidPid.GetRightValue());
			appConfig.UpgradeDevice.SetBootVidPid(editBootVidPid.GetLeftValue(), editBootVidPid.GetRightValue());
			appConfig.UpgradeDevice.SetCidMid(editCidMid.GetLeftValue(), editCidMid.GetRightValue());
			GetControl();
			string contents = appConfig.ToConfigTxt();
			File.WriteAllText(((FileDialog)saveFileDialog).FileName, contents);
		}
	}

	private void dragControlRect1_VisibleChanged(object sender, EventArgs e)
	{
		((Control)wiButton_textFont).Enabled = ((Control)dragControlRect1).Visible;
		((Control)wiButton_textColor).Enabled = ((Control)wiButton_textFont).Enabled;
		((Control)wIButton_showHide).Enabled = ((Control)wiButton_textFont).Enabled;
	}

	private void wImageButton_loadConfig_Click(object sender, EventArgs e)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Invalid comparison between Unknown and I4
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Expected O, but got Unknown
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Expected O, but got Unknown
		((FileDialog)openFileDialog).Title = "Open File";
		((FileDialog)openFileDialog).Filter = "EXE Files|*.exe";
		((FileDialog)openFileDialog).FilterIndex = 1;
		openFileDialog.Multiselect = false;
		((FileDialog)openFileDialog).RestoreDirectory = true;
		if ((int)((CommonDialog)openFileDialog).ShowDialog() == 1)
		{
			LoadExeResource loadExeResource = new LoadExeResource();
			loadExeResource.Load(((FileDialog)openFileDialog).FileName, ref appConfig);
			UpdateUI();
			editNormalVidPid.SetValue(isChecked: true, appConfig.UpgradeDevice.normalVid, appConfig.UpgradeDevice.normalPid);
			editBootVidPid.SetValue(isChecked: true, appConfig.UpgradeDevice.bootVid, appConfig.UpgradeDevice.bootPid);
			editCidMid.SetValue(isChecked: true, appConfig.UpgradeDevice.cid.ToString(), appConfig.UpgradeDevice.mid.ToString());
			customBackImage = loadExeResource.UpgradeToolBackImage;
			controlLocation.SetBackImage((Image)new Bitmap(customBackImage));
			appConfig.imageConfig.BackgroundImage = customBackImage;
			MemoryStream memoryStream = new MemoryStream(appConfig.imageConfig.multiLogoIcon);
			pictureBox_icon.Image = (Image)new Bitmap((Stream)memoryStream);
			((Control)wTextBox_ExeCaption).Text = appConfig.fileConfig.exeCaption;
			WindProgressBar windProgressBar = GetWindProgressBar();
			if (formProgressStyle == null)
			{
				formProgressStyle = new FormProgressStyle(progressBarList);
			}
			formProgressStyle.SetProgressBar(windProgressBar.TextColor, windProgressBar.BarForeColor, windProgressBar.BarBackColor, imageBar1);
			((Control)this).Invalidate();
		}
	}

	private void SetControl(Control control, ControlConfig controlConfig)
	{
		if (controlConfig.Rect.Width > 0 && controlConfig.Rect.Height > 0)
		{
			control.Location = new Point(controlConfig.Rect.X + ((Control)pictureBox1).Location.X, controlConfig.Rect.Y + ((Control)pictureBox1).Location.Y);
			control.Width = controlConfig.Rect.Width;
			control.Height = controlConfig.Rect.Height;
			control.Text = controlConfig.Text;
			control.Font = controlConfig.font;
			control.ForeColor = controlConfig.textColor;
			control.Visible = true;
		}
	}

	private void UpdateUI()
	{
		SetControl((Control)(object)wTextBox_caption, appConfig.CaptionTextBox);
		SetControl((Control)(object)wTextBox_caption, appConfig.CaptionTextBox);
		SetControl((Control)(object)wTextBox_curFwVersion, appConfig.CurFwVersionTextBox);
		SetControl((Control)(object)wTextBox_newFwVersion, appConfig.NewFwVersionTextBox);
		SetControl((Control)(object)wTextBox_ToolVersion, appConfig.ToolVersionTextBox);
		SetControl((Control)(object)wibutton_StartUpgrade, appConfig.SuccessUpgradeButton);
		wibutton_StartUpgrade.SetMouseForeColor(appConfig.SuccessUpgradeButton.foreColor);
		wibutton_StartUpgrade.SetMouseBackColor(appConfig.SuccessUpgradeButton.backColor);
		wibutton_StartUpgrade.MouseUpImage = appConfig.imageConfig.UpgradeMouseUpImage;
		wibutton_StartUpgrade.MouseEnterImage = appConfig.imageConfig.UpgradeMouseEnterImage;
		wibutton_StartUpgrade.MouseDownImage = appConfig.imageConfig.UpgradeMouseDownImage;
		SetControl((Control)(object)wibutton_FailStartUpgrade, appConfig.FailUpgradeButton);
		wibutton_FailStartUpgrade.SetMouseForeColor(appConfig.FailUpgradeButton.foreColor);
		wibutton_FailStartUpgrade.SetMouseBackColor(appConfig.FailUpgradeButton.backColor);
		wibutton_FailStartUpgrade.MouseUpImage = appConfig.imageConfig.UpgradeMouseUpImage;
		wibutton_FailStartUpgrade.MouseEnterImage = appConfig.imageConfig.UpgradeMouseEnterImage;
		wibutton_FailStartUpgrade.MouseDownImage = appConfig.imageConfig.UpgradeMouseDownImage;
		((Control)wibutton_FailStartUpgrade).Visible = false;
		SetControl((Control)(object)wiButton_custom_mini, appConfig.MiniButton);
		wiButton_custom_mini.SetMouseForeColor(appConfig.MiniButton.foreColor);
		SetControl((Control)(object)wiButton_custom_close, appConfig.CloseButton);
		wiButton_custom_close.SetMouseForeColor(appConfig.CloseButton.foreColor);
		imageBar1.BarSliderImage = appConfig.imageConfig.ProgressSliderImage;
		imageBar1.BarForeImage = appConfig.imageConfig.ProgressForeImage;
		imageBar1.BarBackImage = appConfig.imageConfig.ProgressBackImage;
		imageBar1.TextLocation = appConfig.imageConfig.PorgressTextLocation;
		imageBar1.BarSliderImageRect = appConfig.imageConfig.PorgressSliderImageRect;
		imageBar1.BarForeImageRect = appConfig.imageConfig.PorgressForeImageRect;
		imageBar1.BarBackImageRect = appConfig.imageConfig.PorgressBackImageRect;
		imageBar1.BarSliderMovable = appConfig.imageConfig.PorgressSliderMovable;
		imageBar1.ShowPercent = appConfig.imageConfig.PorgressShowPercent;
		for (int i = 0; i < progressBarList.Count; i++)
		{
			((Control)progressBarList[i]).Visible = progressBarList[i].BarStyle == (BAR_STYLE)appConfig.ProgressControl.style;
			if (((Control)progressBarList[i]).Visible)
			{
				SetControl((Control)(object)progressBarList[i], appConfig.ProgressControl);
			}
			progressBarList[i].TextColor = appConfig.ProgressControl.textColor;
			progressBarList[i].BarForeColor = appConfig.ProgressControl.foreColor;
			progressBarList[i].BarBackColor = appConfig.ProgressControl.backColor;
			progressBarList[i].Value = 50;
		}
	}

	private void wIButton_showHide_Click(object sender, EventArgs e)
	{
		if (fontControl != null)
		{
			if (fontControl.Text.Contains(FormMakeMultiPairTool.hideKey))
			{
				fontControl.Text = fontControl.Text.Replace(FormMakeMultiPairTool.hideKey, "");
				return;
			}
			Control obj = fontControl;
			obj.Text += FormMakeMultiPairTool.hideKey;
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
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Expected O, but got Unknown
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0329: Expected O, but got Unknown
		//IL_0347: Unknown result type (might be due to invalid IL or missing references)
		//IL_0351: Expected O, but got Unknown
		//IL_0374: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04be: Expected O, but got Unknown
		//IL_04fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_05aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b4: Expected O, but got Unknown
		//IL_05c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cc: Expected O, but got Unknown
		//IL_0617: Unknown result type (might be due to invalid IL or missing references)
		//IL_0621: Expected O, but got Unknown
		//IL_07fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0806: Expected O, but got Unknown
		//IL_0a0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a14: Expected O, but got Unknown
		//IL_0c4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c58: Expected O, but got Unknown
		//IL_0ca5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d51: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d5b: Expected O, but got Unknown
		//IL_0d69: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d73: Expected O, but got Unknown
		//IL_0dbe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dc8: Expected O, but got Unknown
		//IL_0f6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_108f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1099: Expected O, but got Unknown
		//IL_10c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1175: Unknown result type (might be due to invalid IL or missing references)
		//IL_117f: Expected O, but got Unknown
		//IL_118d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1197: Expected O, but got Unknown
		//IL_1255: Unknown result type (might be due to invalid IL or missing references)
		//IL_125f: Expected O, but got Unknown
		//IL_128f: Unknown result type (might be due to invalid IL or missing references)
		//IL_133b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1345: Expected O, but got Unknown
		//IL_1353: Unknown result type (might be due to invalid IL or missing references)
		//IL_135d: Expected O, but got Unknown
		//IL_142e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1438: Expected O, but got Unknown
		//IL_146b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1516: Unknown result type (might be due to invalid IL or missing references)
		//IL_1520: Expected O, but got Unknown
		//IL_152e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1538: Expected O, but got Unknown
		//IL_1583: Unknown result type (might be due to invalid IL or missing references)
		//IL_158d: Expected O, but got Unknown
		//IL_175b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1765: Expected O, but got Unknown
		//IL_191c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1926: Expected O, but got Unknown
		//IL_1a0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a15: Expected O, but got Unknown
		//IL_1a56: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b64: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b6e: Expected O, but got Unknown
		//IL_1c43: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c4d: Expected O, but got Unknown
		//IL_1d3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d44: Expected O, but got Unknown
		//IL_1f1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f29: Expected O, but got Unknown
		//IL_20f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_20fe: Expected O, but got Unknown
		//IL_22c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_22d3: Expected O, but got Unknown
		//IL_24b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_24c2: Expected O, but got Unknown
		//IL_2655: Unknown result type (might be due to invalid IL or missing references)
		//IL_265f: Expected O, but got Unknown
		//IL_27d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_2900: Unknown result type (might be due to invalid IL or missing references)
		//IL_290a: Expected O, but got Unknown
		//IL_2ac5: Unknown result type (might be due to invalid IL or missing references)
		//IL_2acf: Expected O, but got Unknown
		//IL_2add: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ae7: Expected O, but got Unknown
		//IL_2b32: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b3c: Expected O, but got Unknown
		//IL_2caf: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cb9: Expected O, but got Unknown
		//IL_2cc7: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cd1: Expected O, but got Unknown
		//IL_2d1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d26: Expected O, but got Unknown
		//IL_2e99: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ea3: Expected O, but got Unknown
		//IL_2eb1: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ebb: Expected O, but got Unknown
		//IL_2f06: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f10: Expected O, but got Unknown
		//IL_30c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_30d1: Expected O, but got Unknown
		//IL_3112: Unknown result type (might be due to invalid IL or missing references)
		//IL_3212: Unknown result type (might be due to invalid IL or missing references)
		//IL_321c: Expected O, but got Unknown
		//IL_322a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3234: Expected O, but got Unknown
		//IL_326a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3274: Expected O, but got Unknown
		//IL_32b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_33b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_33bf: Expected O, but got Unknown
		//IL_33cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_33d7: Expected O, but got Unknown
		//IL_340d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3417: Expected O, but got Unknown
		//IL_3458: Unknown result type (might be due to invalid IL or missing references)
		//IL_3558: Unknown result type (might be due to invalid IL or missing references)
		//IL_3562: Expected O, but got Unknown
		//IL_3570: Unknown result type (might be due to invalid IL or missing references)
		//IL_357a: Expected O, but got Unknown
		//IL_35a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_35b3: Expected O, but got Unknown
		//IL_35f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_36f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_36fb: Expected O, but got Unknown
		//IL_3709: Unknown result type (might be due to invalid IL or missing references)
		//IL_3713: Expected O, but got Unknown
		//IL_3b89: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b93: Expected O, but got Unknown
		//IL_3dd7: Unknown result type (might be due to invalid IL or missing references)
		//IL_3de1: Expected O, but got Unknown
		//IL_3e1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4201: Unknown result type (might be due to invalid IL or missing references)
		//IL_420b: Expected O, but got Unknown
		//IL_4220: Unknown result type (might be due to invalid IL or missing references)
		//IL_422a: Expected O, but got Unknown
		//IL_426e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4278: Expected O, but got Unknown
		//IL_4281: Unknown result type (might be due to invalid IL or missing references)
		//IL_428b: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FormMakeUpgradeTool));
		pictureBox_icon = new PictureBox();
		pictureBox1 = new PictureBox();
		toolTip1 = new ToolTip(components);
		tipControl1 = new TipControl();
		imageBar1 = new ImageBar();
		wImageButton_saveConfig = new WindImageButton();
		wibutton_FailStartUpgrade = new WindImageButton();
		wiButton_textFont = new WindImageButton();
		cellBar1 = new CellBar();
		wImageButton_loadConfig = new WindImageButton();
		dragControlRect1 = new DragControlRect();
		roundBar1 = new RoundBar();
		sliderBar1 = new SliderBar();
		haloBar1 = new HaloBar();
		wImageButton_progressStyle = new WindImageButton();
		wiButton_load_code = new WindImageButton();
		editCidMid = new EditValueControl();
		wTextBox_fileName = new WindTextBox();
		editBootVidPid = new EditValueControl();
		editNormalVidPid = new EditValueControl();
		wiButton_buttonSet = new WindImageButton();
		wiButton_textColor = new WindImageButton();
		wiButton_Icon = new WindImageButton();
		wIButton_upgradetool = new WindImageButton();
		wCheckBox_IC_Enable = new WindCheckBox();
		wcobBox_IC_type = new WindComboBox();
		wibutton_StartUpgrade = new WindImageButton();
		wiButton_custom_mini = new WindImageButton();
		wiButton_custom_close = new WindImageButton();
		wiButton_createExe = new WindImageButton();
		wTextBox_ToolVersion = new WindTextBox();
		wTextBox_newFwVersion = new WindTextBox();
		wTextBox_curFwVersion = new WindTextBox();
		wTextBox_caption = new WindTextBox();
		wiButton_Mini = new WindImageButton();
		wiButton_Close = new WindImageButton();
		wIButton_showHide = new WindImageButton();
		label2 = new Label();
		wTextBox_ExeCaption = new WindTextBox();
		((ISupportInitialize)pictureBox_icon).BeginInit();
		((ISupportInitialize)pictureBox1).BeginInit();
		((Control)this).SuspendLayout();
		((Control)pictureBox_icon).BackColor = Color.Transparent;
		((Control)pictureBox_icon).BackgroundImageLayout = (ImageLayout)3;
		((Control)pictureBox_icon).Location = new Point(117, 105);
		((Control)pictureBox_icon).Name = "pictureBox_icon";
		((Control)pictureBox_icon).Size = new Size(32, 32);
		pictureBox_icon.SizeMode = (PictureBoxSizeMode)1;
		pictureBox_icon.TabIndex = 51;
		pictureBox_icon.TabStop = false;
		((Control)pictureBox1).BackColor = Color.Transparent;
		((Control)pictureBox1).BackgroundImage = (Image)(object)Resources.UpgradeTool;
		((Control)pictureBox1).Location = new Point(173, 53);
		((Control)pictureBox1).Name = "pictureBox1";
		((Control)pictureBox1).Size = new Size(600, 340);
		pictureBox1.TabIndex = 49;
		pictureBox1.TabStop = false;
		((Control)pictureBox1).Visible = false;
		((Control)tipControl1).BackColor = Color.Transparent;
		((Control)tipControl1).BackgroundImage = (Image)componentResourceManager.GetObject("tipControl1.BackgroundImage");
		((Control)tipControl1).BackgroundImageLayout = (ImageLayout)3;
		((Control)tipControl1).Font = new Font("微软雅黑", 10.5f);
		((Control)tipControl1).Location = new Point(878, 66);
		((Control)tipControl1).Margin = new Padding(4, 5, 4, 5);
		((Control)tipControl1).Name = "tipControl1";
		((Control)tipControl1).Size = new Size(182, 89);
		((Control)tipControl1).TabIndex = 193;
		((Control)tipControl1).Text = "请选择芯片类型";
		tipControl1.TextForeColor = Color.Red;
		((Control)imageBar1).BackColor = Color.Transparent;
		imageBar1.BarBackColor = Color.DarkGray;
		imageBar1.BarBackImage = (Image)(object)Resources.BarBackImage;
		imageBar1.BarBackImageRect = new Rectangle(18, 32, 384, 32);
		imageBar1.BarForeColor = Color.RoyalBlue;
		imageBar1.BarForeImage = (Image)(object)Resources.BarForeImage;
		imageBar1.BarForeImageRect = new Rectangle(23, 36, 376, 25);
		imageBar1.BarSliderImage = (Image)(object)Resources.BarSliderImage;
		imageBar1.BarSliderImageRect = new Rectangle(0, 0, 40, 25);
		imageBar1.BarSliderMovable = true;
		((Control)imageBar1).Font = new Font("微软雅黑", 12f, (FontStyle)2, (GraphicsUnit)3, (byte)134);
		imageBar1.FrameWidth = 2;
		imageBar1.IntervalWidth = 4;
		((Control)imageBar1).Location = new Point(191, 244);
		((Control)imageBar1).Margin = new Padding(5, 5, 5, 5);
		imageBar1.MaxValue = 100;
		((Control)imageBar1).Name = "imageBar1";
		imageBar1.ShowPercent = true;
		((Control)imageBar1).Size = new Size(421, 67);
		imageBar1.StepImage = null;
		((Control)imageBar1).TabIndex = 189;
		imageBar1.TextColor = Color.Black;
		imageBar1.TextLocation = new Point(0, 3);
		imageBar1.Value = 50;
		((Control)imageBar1).MouseDown += new MouseEventHandler(DragRect_MouseDown);
		((Control)imageBar1).MouseUp += new MouseEventHandler(DragRect_MouseUp);
		((Control)wImageButton_saveConfig).BackColor = Color.Transparent;
		wImageButton_saveConfig.DisableBackColor = Color.DarkGray;
		wImageButton_saveConfig.DisableForeColor = Color.DimGray;
		((Control)wImageButton_saveConfig).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wImageButton_saveConfig.FrameMode = GraphicsHelper.RoundStyle.All;
		wImageButton_saveConfig.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wImageButton_saveConfig.IconName = "";
		wImageButton_saveConfig.IconOffset = new Point(0, 0);
		wImageButton_saveConfig.IconSize = 32;
		((Control)wImageButton_saveConfig).Location = new Point(11, 423);
		wImageButton_saveConfig.MouseDownBackColor = Color.DimGray;
		wImageButton_saveConfig.MouseDownForeColor = Color.Black;
		wImageButton_saveConfig.MouseEnterBackColor = Color.Gray;
		wImageButton_saveConfig.MouseEnterForeColor = Color.Black;
		wImageButton_saveConfig.MouseUpBackColor = Color.DarkGray;
		wImageButton_saveConfig.MouseUpForeColor = Color.Black;
		((Control)wImageButton_saveConfig).Name = "wImageButton_saveConfig";
		wImageButton_saveConfig.Radius = 8;
		((Control)wImageButton_saveConfig).Size = new Size(136, 24);
		((Control)wImageButton_saveConfig).TabIndex = 187;
		((Control)wImageButton_saveConfig).Text = "保存配置";
		wImageButton_saveConfig.TextAlignment = StringHelper.TextAlignment.Center;
		wImageButton_saveConfig.TextDynOffset = new Point(0, 0);
		wImageButton_saveConfig.TextFixLocation = new Point(0, 0);
		wImageButton_saveConfig.TextFixLocationEnable = false;
		((Control)wImageButton_saveConfig).Visible = false;
		((Control)wImageButton_saveConfig).Click += wImageButton_saveConfig_Click;
		((Control)wibutton_FailStartUpgrade).BackColor = Color.Transparent;
		wibutton_FailStartUpgrade.DisableBackColor = Color.DarkGray;
		wibutton_FailStartUpgrade.DisableForeColor = Color.DimGray;
		((Control)wibutton_FailStartUpgrade).Font = new Font("微软雅黑", 14.25f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wibutton_FailStartUpgrade.FrameMode = GraphicsHelper.RoundStyle.All;
		wibutton_FailStartUpgrade.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wibutton_FailStartUpgrade.IconName = "";
		wibutton_FailStartUpgrade.IconOffset = new Point(0, 0);
		wibutton_FailStartUpgrade.IconSize = 32;
		((Control)wibutton_FailStartUpgrade).Location = new Point(656, 345);
		wibutton_FailStartUpgrade.MouseDownBackColor = Color.Red;
		wibutton_FailStartUpgrade.MouseDownForeColor = Color.Black;
		wibutton_FailStartUpgrade.MouseDownImage = (Image)(object)Resources.ButtonMouseDown;
		wibutton_FailStartUpgrade.MouseEnterBackColor = Color.Red;
		wibutton_FailStartUpgrade.MouseEnterForeColor = Color.Black;
		wibutton_FailStartUpgrade.MouseEnterImage = (Image)(object)Resources.ButtonMouseEnter;
		wibutton_FailStartUpgrade.MouseUpBackColor = Color.Red;
		wibutton_FailStartUpgrade.MouseUpForeColor = Color.Black;
		wibutton_FailStartUpgrade.MouseUpImage = (Image)(object)Resources.ButtonMouseUp;
		((Control)wibutton_FailStartUpgrade).Name = "wibutton_FailStartUpgrade";
		wibutton_FailStartUpgrade.Radius = 16;
		((Control)wibutton_FailStartUpgrade).Size = new Size(104, 42);
		((Control)wibutton_FailStartUpgrade).TabIndex = 179;
		((Control)wibutton_FailStartUpgrade).Text = "Fail";
		wibutton_FailStartUpgrade.TextAlignment = StringHelper.TextAlignment.Center;
		wibutton_FailStartUpgrade.TextDynOffset = new Point(1, 1);
		wibutton_FailStartUpgrade.TextFixLocation = new Point(0, 0);
		wibutton_FailStartUpgrade.TextFixLocationEnable = false;
		((Control)wibutton_FailStartUpgrade).Visible = false;
		((Control)wiButton_textFont).BackColor = Color.Transparent;
		wiButton_textFont.DisableBackColor = Color.DarkGray;
		wiButton_textFont.DisableForeColor = Color.DimGray;
		((Control)wiButton_textFont).Enabled = false;
		((Control)wiButton_textFont).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wiButton_textFont.FrameMode = GraphicsHelper.RoundStyle.All;
		wiButton_textFont.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wiButton_textFont.IconName = "";
		wiButton_textFont.IconOffset = new Point(0, 0);
		wiButton_textFont.IconSize = 32;
		((Control)wiButton_textFont).Location = new Point(13, 251);
		wiButton_textFont.MouseDownBackColor = Color.DimGray;
		wiButton_textFont.MouseDownForeColor = Color.Black;
		wiButton_textFont.MouseEnterBackColor = Color.Gray;
		wiButton_textFont.MouseEnterForeColor = Color.Black;
		wiButton_textFont.MouseUpBackColor = Color.DarkGray;
		wiButton_textFont.MouseUpForeColor = Color.Black;
		((Control)wiButton_textFont).Name = "wiButton_textFont";
		wiButton_textFont.Radius = 8;
		((Control)wiButton_textFont).Size = new Size(136, 24);
		((Control)wiButton_textFont).TabIndex = 177;
		((Control)wiButton_textFont).Text = "字体";
		wiButton_textFont.TextAlignment = StringHelper.TextAlignment.Center;
		wiButton_textFont.TextDynOffset = new Point(0, 0);
		wiButton_textFont.TextFixLocation = new Point(0, 0);
		wiButton_textFont.TextFixLocationEnable = false;
		((Control)wiButton_textFont).Click += wiButton_textFont_Click;
		((Control)cellBar1).BackColor = Color.Transparent;
		cellBar1.BarBackColor = Color.DarkGray;
		cellBar1.BarBackImage = null;
		cellBar1.BarBackImageRect = new Rectangle(0, 0, 0, 0);
		cellBar1.BarForeColor = Color.RoyalBlue;
		cellBar1.BarForeImage = null;
		cellBar1.BarForeImageRect = new Rectangle(0, 0, 0, 0);
		cellBar1.BarSliderImage = null;
		cellBar1.BarSliderImageRect = new Rectangle(0, 0, 0, 0);
		cellBar1.BarSliderMovable = true;
		((Control)cellBar1).Font = new Font("微软雅黑", 12f);
		cellBar1.FrameWidth = 2;
		cellBar1.GridWidth = 8;
		cellBar1.IntervalWidth = 4;
		((Control)cellBar1).Location = new Point(878, 277);
		((Control)cellBar1).Margin = new Padding(5, 5, 5, 5);
		cellBar1.MaxValue = 100;
		((Control)cellBar1).Name = "cellBar1";
		cellBar1.ShowPercent = true;
		((Control)cellBar1).Size = new Size(374, 18);
		((Control)cellBar1).TabIndex = 170;
		cellBar1.TextColor = Color.Black;
		cellBar1.TextLocation = new Point(0, 0);
		cellBar1.Value = 50;
		((Control)cellBar1).Visible = false;
		((Control)cellBar1).MouseDown += new MouseEventHandler(DragRect_MouseDown);
		((Control)cellBar1).MouseUp += new MouseEventHandler(DragRect_MouseUp);
		((Control)wImageButton_loadConfig).BackColor = Color.Transparent;
		wImageButton_loadConfig.DisableBackColor = Color.DarkGray;
		wImageButton_loadConfig.DisableForeColor = Color.DimGray;
		((Control)wImageButton_loadConfig).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wImageButton_loadConfig.FrameMode = GraphicsHelper.RoundStyle.All;
		wImageButton_loadConfig.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wImageButton_loadConfig.IconName = "";
		wImageButton_loadConfig.IconOffset = new Point(0, 0);
		wImageButton_loadConfig.IconSize = 32;
		((Control)wImageButton_loadConfig).Location = new Point(11, 393);
		wImageButton_loadConfig.MouseDownBackColor = Color.DimGray;
		wImageButton_loadConfig.MouseDownForeColor = Color.Black;
		wImageButton_loadConfig.MouseEnterBackColor = Color.Gray;
		wImageButton_loadConfig.MouseEnterForeColor = Color.Black;
		wImageButton_loadConfig.MouseUpBackColor = Color.DarkGray;
		wImageButton_loadConfig.MouseUpForeColor = Color.Black;
		((Control)wImageButton_loadConfig).Name = "wImageButton_loadConfig";
		wImageButton_loadConfig.Radius = 8;
		((Control)wImageButton_loadConfig).Size = new Size(136, 24);
		((Control)wImageButton_loadConfig).TabIndex = 168;
		((Control)wImageButton_loadConfig).Text = "加载工程";
		wImageButton_loadConfig.TextAlignment = StringHelper.TextAlignment.Center;
		wImageButton_loadConfig.TextDynOffset = new Point(0, 0);
		wImageButton_loadConfig.TextFixLocation = new Point(0, 0);
		wImageButton_loadConfig.TextFixLocationEnable = false;
		((Control)wImageButton_loadConfig).Click += wImageButton_loadConfig_Click;
		((Control)dragControlRect1).Location = new Point(1164, 105);
		((Control)dragControlRect1).Margin = new Padding(4);
		((Control)dragControlRect1).Name = "dragControlRect1";
		((Control)dragControlRect1).Size = new Size(16, 16);
		((Control)dragControlRect1).TabIndex = 163;
		((Control)dragControlRect1).Visible = false;
		((Control)dragControlRect1).VisibleChanged += dragControlRect1_VisibleChanged;
		((Control)roundBar1).BackColor = Color.Transparent;
		roundBar1.BarBackColor = Color.DarkGray;
		roundBar1.BarBackImage = null;
		roundBar1.BarBackImageRect = new Rectangle(0, 0, 0, 0);
		roundBar1.BarForeColor = Color.RoyalBlue;
		roundBar1.BarForeImage = null;
		roundBar1.BarForeImageRect = new Rectangle(0, 0, 0, 0);
		roundBar1.BarSliderImage = null;
		roundBar1.BarSliderImageRect = new Rectangle(0, 0, 0, 0);
		roundBar1.BarSliderMovable = true;
		((Control)roundBar1).Font = new Font("微软雅黑", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		roundBar1.FrameWidth = 2;
		((Control)roundBar1).Location = new Point(878, 345);
		((Control)roundBar1).Margin = new Padding(5);
		roundBar1.MaxValue = 100;
		((Control)roundBar1).Name = "roundBar1";
		roundBar1.ShowPercent = true;
		((Control)roundBar1).Size = new Size(374, 24);
		((Control)roundBar1).TabIndex = 160;
		roundBar1.TextColor = Color.Black;
		roundBar1.TextLocation = new Point(0, 0);
		roundBar1.Value = 50;
		((Control)roundBar1).Visible = false;
		((Control)roundBar1).MouseDown += new MouseEventHandler(DragRect_MouseDown);
		((Control)roundBar1).MouseUp += new MouseEventHandler(DragRect_MouseUp);
		((Control)sliderBar1).BackColor = Color.Transparent;
		sliderBar1.BarBackColor = Color.DarkGray;
		sliderBar1.BarBackImage = null;
		sliderBar1.BarBackImageRect = new Rectangle(0, 0, 0, 0);
		sliderBar1.BarForeColor = Color.RoyalBlue;
		sliderBar1.BarForeImage = null;
		sliderBar1.BarForeImageRect = new Rectangle(0, 0, 0, 0);
		sliderBar1.BarSliderImage = null;
		sliderBar1.BarSliderImageRect = new Rectangle(0, 0, 0, 0);
		sliderBar1.BarSliderMovable = true;
		((Control)sliderBar1).Font = new Font("微软雅黑", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		sliderBar1.FrameWidth = 2;
		((Control)sliderBar1).Location = new Point(848, 407);
		((Control)sliderBar1).Margin = new Padding(5);
		sliderBar1.MaxValue = 100;
		((Control)sliderBar1).Name = "sliderBar1";
		sliderBar1.ShowPercent = true;
		((Control)sliderBar1).Size = new Size(422, 61);
		((Control)sliderBar1).TabIndex = 159;
		sliderBar1.TextColor = Color.Black;
		sliderBar1.TextLocation = new Point(0, 0);
		sliderBar1.Value = 50;
		((Control)sliderBar1).Visible = false;
		((Control)sliderBar1).MouseDown += new MouseEventHandler(DragRect_MouseDown);
		((Control)sliderBar1).MouseUp += new MouseEventHandler(DragRect_MouseUp);
		((Control)haloBar1).BackColor = Color.Transparent;
		haloBar1.BarBackColor = Color.DarkGray;
		haloBar1.BarBackImage = null;
		haloBar1.BarBackImageRect = new Rectangle(0, 0, 0, 0);
		haloBar1.BarForeColor = Color.RoyalBlue;
		haloBar1.BarForeImage = null;
		haloBar1.BarForeImageRect = new Rectangle(0, 0, 0, 0);
		haloBar1.BarSliderImage = null;
		haloBar1.BarSliderImageRect = new Rectangle(0, 0, 0, 0);
		haloBar1.BarSliderMovable = true;
		haloBar1.BarWidth = 8;
		haloBar1.EnableMouse = false;
		((Control)haloBar1).Font = new Font("微软雅黑", 12f);
		haloBar1.FrameWidth = 2;
		((Control)haloBar1).Location = new Point(878, 165);
		((Control)haloBar1).Margin = new Padding(5, 5, 5, 5);
		haloBar1.MaxValue = 100;
		((Control)haloBar1).Name = "haloBar1";
		haloBar1.ShowPercent = true;
		((Control)haloBar1).Size = new Size(374, 35);
		((Control)haloBar1).TabIndex = 158;
		haloBar1.TextColor = Color.Black;
		haloBar1.TextLocation = new Point(0, 0);
		haloBar1.Value = 0;
		((Control)haloBar1).Visible = false;
		((Control)haloBar1).MouseDown += new MouseEventHandler(DragRect_MouseDown);
		((Control)haloBar1).MouseUp += new MouseEventHandler(DragRect_MouseUp);
		((Control)wImageButton_progressStyle).BackColor = Color.Transparent;
		wImageButton_progressStyle.DisableBackColor = Color.DarkGray;
		wImageButton_progressStyle.DisableForeColor = Color.DimGray;
		((Control)wImageButton_progressStyle).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wImageButton_progressStyle.FrameMode = GraphicsHelper.RoundStyle.All;
		wImageButton_progressStyle.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wImageButton_progressStyle.IconName = "";
		wImageButton_progressStyle.IconOffset = new Point(0, 0);
		wImageButton_progressStyle.IconSize = 32;
		((Control)wImageButton_progressStyle).Location = new Point(11, 319);
		wImageButton_progressStyle.MouseDownBackColor = Color.DimGray;
		wImageButton_progressStyle.MouseDownForeColor = Color.Black;
		wImageButton_progressStyle.MouseEnterBackColor = Color.Gray;
		wImageButton_progressStyle.MouseEnterForeColor = Color.Black;
		wImageButton_progressStyle.MouseUpBackColor = Color.DarkGray;
		wImageButton_progressStyle.MouseUpForeColor = Color.Black;
		((Control)wImageButton_progressStyle).Name = "wImageButton_progressStyle";
		wImageButton_progressStyle.Radius = 8;
		((Control)wImageButton_progressStyle).Size = new Size(136, 24);
		((Control)wImageButton_progressStyle).TabIndex = 156;
		((Control)wImageButton_progressStyle).Text = "进度条样式";
		wImageButton_progressStyle.TextAlignment = StringHelper.TextAlignment.Center;
		wImageButton_progressStyle.TextDynOffset = new Point(0, 0);
		wImageButton_progressStyle.TextFixLocation = new Point(0, 0);
		wImageButton_progressStyle.TextFixLocationEnable = false;
		((Control)wImageButton_progressStyle).Click += wImageButton_progressStyle_Click;
		((Control)wiButton_load_code).BackColor = Color.Transparent;
		wiButton_load_code.DisableBackColor = Color.DarkGray;
		wiButton_load_code.DisableForeColor = Color.DimGray;
		((Control)wiButton_load_code).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wiButton_load_code.FrameMode = GraphicsHelper.RoundStyle.All;
		wiButton_load_code.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wiButton_load_code.IconName = "";
		wiButton_load_code.IconOffset = new Point(0, 0);
		wiButton_load_code.IconSize = 32;
		((Control)wiButton_load_code).Location = new Point(691, 407);
		wiButton_load_code.MouseDownBackColor = Color.DimGray;
		wiButton_load_code.MouseDownForeColor = Color.Black;
		wiButton_load_code.MouseEnterBackColor = Color.Gray;
		wiButton_load_code.MouseEnterForeColor = Color.Black;
		wiButton_load_code.MouseUpBackColor = Color.LightGray;
		wiButton_load_code.MouseUpForeColor = Color.Black;
		((Control)wiButton_load_code).Name = "wiButton_load_code";
		wiButton_load_code.Radius = 16;
		((Control)wiButton_load_code).Size = new Size(82, 35);
		((Control)wiButton_load_code).TabIndex = 141;
		((Control)wiButton_load_code).Text = "加载code";
		wiButton_load_code.TextAlignment = StringHelper.TextAlignment.Center;
		wiButton_load_code.TextDynOffset = new Point(0, 0);
		wiButton_load_code.TextFixLocation = new Point(0, 0);
		wiButton_load_code.TextFixLocationEnable = false;
		((Control)wiButton_load_code).Click += wiButton_load_code_Click;
		((Control)editCidMid).BackColor = Color.Transparent;
		editCidMid.CheckedText = "";
		((Control)editCidMid).Font = new Font("微软雅黑", 10.5f);
		editCidMid.isChecked = false;
		editCidMid.LeftLabelText = "CID";
		((Control)editCidMid).Location = new Point(167, 560);
		((Control)editCidMid).Name = "editCidMid";
		editCidMid.RightLabelText = "MID";
		((Control)editCidMid).Size = new Size(589, 44);
		((Control)editCidMid).TabIndex = 144;
		editCidMid.UnCheckedText = "";
		editCidMid.xUI = true;
		((Control)wTextBox_fileName).BackgroundImage = (Image)(object)Resources.长输入框;
		((Control)wTextBox_fileName).BackgroundImageLayout = (ImageLayout)3;
		wTextBox_fileName.DisableBackImage = null;
		((Control)wTextBox_fileName).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wTextBox_fileName.FrameColor = Color.White;
		wTextBox_fileName.InputMode = WindBaseTextBox.TextBoxInputMode.Normal;
		((Control)wTextBox_fileName).Location = new Point(173, 407);
		((Control)wTextBox_fileName).Margin = new Padding(4);
		wTextBox_fileName.MaxLength = int.MaxValue;
		((Control)wTextBox_fileName).Name = "wTextBox_fileName";
		wTextBox_fileName.PasswordChar = "";
		wTextBox_fileName.PromptText = "";
		wTextBox_fileName.PromptTextColor = Color.Gray;
		wTextBox_fileName.PromptTextForeColor = Color.LightGray;
		wTextBox_fileName.ReadOnly = true;
		wTextBox_fileName.SelectedBackImage = (Image)(object)Resources.长输入框;
		((Control)wTextBox_fileName).Size = new Size(505, 35);
		((Control)wTextBox_fileName).TabIndex = 140;
		wTextBox_fileName.TextBoxOffset = new Point(6, 6);
		wTextBox_fileName.UnSelectedBackImage = (Image)(object)Resources.长输入框;
		((Control)editBootVidPid).BackColor = Color.Transparent;
		editBootVidPid.CheckedText = "";
		((Control)editBootVidPid).Font = new Font("微软雅黑", 10.5f);
		editBootVidPid.isChecked = false;
		editBootVidPid.LeftLabelText = "VID";
		((Control)editBootVidPid).Location = new Point(166, 507);
		((Control)editBootVidPid).Name = "editBootVidPid";
		editBootVidPid.RightLabelText = "PID";
		((Control)editBootVidPid).Size = new Size(589, 44);
		((Control)editBootVidPid).TabIndex = 143;
		editBootVidPid.UnCheckedText = "";
		editBootVidPid.xUI = true;
		((Control)editNormalVidPid).BackColor = Color.Transparent;
		editNormalVidPid.CheckedText = "";
		((Control)editNormalVidPid).Font = new Font("微软雅黑", 10.5f);
		editNormalVidPid.isChecked = false;
		editNormalVidPid.LeftLabelText = "VID";
		((Control)editNormalVidPid).Location = new Point(166, 453);
		((Control)editNormalVidPid).Name = "editNormalVidPid";
		editNormalVidPid.RightLabelText = "PID";
		((Control)editNormalVidPid).Size = new Size(589, 44);
		((Control)editNormalVidPid).TabIndex = 142;
		editNormalVidPid.UnCheckedText = "";
		editNormalVidPid.xUI = true;
		((Control)wiButton_buttonSet).BackColor = Color.Transparent;
		wiButton_buttonSet.DisableBackColor = Color.DarkGray;
		wiButton_buttonSet.DisableForeColor = Color.DimGray;
		((Control)wiButton_buttonSet).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wiButton_buttonSet.FrameMode = GraphicsHelper.RoundStyle.All;
		wiButton_buttonSet.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wiButton_buttonSet.IconName = "";
		wiButton_buttonSet.IconOffset = new Point(0, 0);
		wiButton_buttonSet.IconSize = 32;
		((Control)wiButton_buttonSet).Location = new Point(13, 288);
		wiButton_buttonSet.MouseDownBackColor = Color.DimGray;
		wiButton_buttonSet.MouseDownForeColor = Color.Black;
		wiButton_buttonSet.MouseEnterBackColor = Color.Gray;
		wiButton_buttonSet.MouseEnterForeColor = Color.Black;
		wiButton_buttonSet.MouseUpBackColor = Color.DarkGray;
		wiButton_buttonSet.MouseUpForeColor = Color.Black;
		((Control)wiButton_buttonSet).Name = "wiButton_buttonSet";
		wiButton_buttonSet.Radius = 8;
		((Control)wiButton_buttonSet).Size = new Size(136, 24);
		((Control)wiButton_buttonSet).TabIndex = 129;
		((Control)wiButton_buttonSet).Text = "升级按钮样式";
		wiButton_buttonSet.TextAlignment = StringHelper.TextAlignment.Center;
		wiButton_buttonSet.TextDynOffset = new Point(0, 0);
		wiButton_buttonSet.TextFixLocation = new Point(0, 0);
		wiButton_buttonSet.TextFixLocationEnable = false;
		((Control)wiButton_buttonSet).Click += wiButton_buttonSet_Click;
		((Control)wiButton_textColor).BackColor = Color.Transparent;
		wiButton_textColor.DisableBackColor = Color.DarkGray;
		wiButton_textColor.DisableForeColor = Color.DimGray;
		((Control)wiButton_textColor).Enabled = false;
		((Control)wiButton_textColor).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wiButton_textColor.FrameMode = GraphicsHelper.RoundStyle.All;
		wiButton_textColor.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wiButton_textColor.IconName = "";
		wiButton_textColor.IconOffset = new Point(0, 0);
		wiButton_textColor.IconSize = 32;
		((Control)wiButton_textColor).Location = new Point(13, 217);
		wiButton_textColor.MouseDownBackColor = Color.DimGray;
		wiButton_textColor.MouseDownForeColor = Color.Black;
		wiButton_textColor.MouseEnterBackColor = Color.Gray;
		wiButton_textColor.MouseEnterForeColor = Color.Black;
		wiButton_textColor.MouseUpBackColor = Color.DarkGray;
		wiButton_textColor.MouseUpForeColor = Color.Black;
		((Control)wiButton_textColor).Name = "wiButton_textColor";
		wiButton_textColor.Radius = 8;
		((Control)wiButton_textColor).Size = new Size(136, 24);
		((Control)wiButton_textColor).TabIndex = 127;
		((Control)wiButton_textColor).Text = "文本颜色";
		wiButton_textColor.TextAlignment = StringHelper.TextAlignment.Center;
		wiButton_textColor.TextDynOffset = new Point(0, 0);
		wiButton_textColor.TextFixLocation = new Point(0, 0);
		wiButton_textColor.TextFixLocationEnable = false;
		((Control)wiButton_textColor).Click += wiButton_textColor_Click;
		((Control)wiButton_Icon).BackColor = Color.Transparent;
		wiButton_Icon.DisableBackColor = Color.DarkGray;
		wiButton_Icon.DisableForeColor = Color.DimGray;
		((Control)wiButton_Icon).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wiButton_Icon.FrameMode = GraphicsHelper.RoundStyle.All;
		wiButton_Icon.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wiButton_Icon.IconName = "";
		wiButton_Icon.IconOffset = new Point(0, 0);
		wiButton_Icon.IconSize = 32;
		((Control)wiButton_Icon).Location = new Point(13, 183);
		wiButton_Icon.MouseDownBackColor = Color.DimGray;
		wiButton_Icon.MouseDownForeColor = Color.Black;
		wiButton_Icon.MouseEnterBackColor = Color.Gray;
		wiButton_Icon.MouseEnterForeColor = Color.Black;
		wiButton_Icon.MouseUpBackColor = Color.DarkGray;
		wiButton_Icon.MouseUpForeColor = Color.Black;
		((Control)wiButton_Icon).Name = "wiButton_Icon";
		wiButton_Icon.Radius = 8;
		((Control)wiButton_Icon).Size = new Size(136, 24);
		((Control)wiButton_Icon).TabIndex = 126;
		((Control)wiButton_Icon).Text = "桌面图标(全尺寸)";
		wiButton_Icon.TextAlignment = StringHelper.TextAlignment.Center;
		wiButton_Icon.TextDynOffset = new Point(0, 0);
		wiButton_Icon.TextFixLocation = new Point(0, 0);
		wiButton_Icon.TextFixLocationEnable = false;
		((Control)wiButton_Icon).Click += wiButton_Icon_Click;
		((Control)wIButton_upgradetool).BackColor = Color.Transparent;
		wIButton_upgradetool.DisableBackColor = Color.DarkGray;
		wIButton_upgradetool.DisableForeColor = Color.DimGray;
		((Control)wIButton_upgradetool).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wIButton_upgradetool.FrameMode = GraphicsHelper.RoundStyle.All;
		wIButton_upgradetool.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wIButton_upgradetool.IconName = "";
		wIButton_upgradetool.IconOffset = new Point(0, 0);
		wIButton_upgradetool.IconSize = 32;
		((Control)wIButton_upgradetool).Location = new Point(13, 149);
		wIButton_upgradetool.MouseDownBackColor = Color.DimGray;
		wIButton_upgradetool.MouseDownForeColor = Color.Black;
		wIButton_upgradetool.MouseEnterBackColor = Color.Gray;
		wIButton_upgradetool.MouseEnterForeColor = Color.Black;
		wIButton_upgradetool.MouseUpBackColor = Color.DarkGray;
		wIButton_upgradetool.MouseUpForeColor = Color.Black;
		((Control)wIButton_upgradetool).Name = "wIButton_upgradetool";
		wIButton_upgradetool.Radius = 8;
		((Control)wIButton_upgradetool).Size = new Size(136, 24);
		((Control)wIButton_upgradetool).TabIndex = 124;
		((Control)wIButton_upgradetool).Text = "背景图片(600×340)";
		wIButton_upgradetool.TextAlignment = StringHelper.TextAlignment.Center;
		wIButton_upgradetool.TextDynOffset = new Point(0, 0);
		wIButton_upgradetool.TextFixLocation = new Point(0, 0);
		wIButton_upgradetool.TextFixLocationEnable = false;
		((Control)wIButton_upgradetool).Click += wIButton_upgradetool_Click;
		((Control)wCheckBox_IC_Enable).BackColor = Color.Transparent;
		((Control)wCheckBox_IC_Enable).BackgroundImageLayout = (ImageLayout)0;
		wCheckBox_IC_Enable.Checked = false;
		wCheckBox_IC_Enable.DisableSelectedImage = (Image)(object)Resources.checkbox_3;
		wCheckBox_IC_Enable.DisableUnSelectedImage = (Image)(object)Resources.checkbox_2;
		((Control)wCheckBox_IC_Enable).Font = new Font("微软雅黑", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wCheckBox_IC_Enable.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wCheckBox_IC_Enable.IconOffset = new Point(0, 0);
		wCheckBox_IC_Enable.IconSize = 36;
		((Control)wCheckBox_IC_Enable).Location = new Point(10, 61);
		((Control)wCheckBox_IC_Enable).Name = "wCheckBox_IC_Enable";
		wCheckBox_IC_Enable.SelectedIconColor = Color.Red;
		wCheckBox_IC_Enable.SelectedIconName = "";
		wCheckBox_IC_Enable.SelectedImage = (Image)(object)Resources.checkbox_1;
		((Control)wCheckBox_IC_Enable).Size = new Size(29, 30);
		((Control)wCheckBox_IC_Enable).TabIndex = 122;
		((Control)wCheckBox_IC_Enable).Text = " ";
		wCheckBox_IC_Enable.TextOffset = new Point(4, 4);
		wCheckBox_IC_Enable.UnSelectedIconColor = Color.Gray;
		wCheckBox_IC_Enable.UnSelectedIconName = "";
		wCheckBox_IC_Enable.UnSelectedImage = (Image)(object)Resources.checkbox_2;
		wCheckBox_IC_Enable.CheckedChanged += wCheckBox_IC_Enable_CheckedChanged;
		((Control)wcobBox_IC_type).BackgroundImage = (Image)(object)Resources.下拉正常状态;
		((Control)wcobBox_IC_type).BackgroundImageLayout = (ImageLayout)3;
		wcobBox_IC_type.DropDownLoctionOffset = new Point(-27, 0);
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
		((Control)wcobBox_IC_type).Location = new Point(43, 62);
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
		((Control)wcobBox_IC_type).TabIndex = 121;
		wcobBox_IC_type.SelectedIndexChanged += wcobBox_IC_type_SelectedIndexChanged;
		((Control)wibutton_StartUpgrade).BackColor = Color.Transparent;
		wibutton_StartUpgrade.DisableBackColor = Color.DarkGray;
		wibutton_StartUpgrade.DisableForeColor = Color.DimGray;
		((Control)wibutton_StartUpgrade).Font = new Font("微软雅黑", 14.25f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wibutton_StartUpgrade.FrameMode = GraphicsHelper.RoundStyle.All;
		wibutton_StartUpgrade.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wibutton_StartUpgrade.IconName = "";
		wibutton_StartUpgrade.IconOffset = new Point(0, 0);
		wibutton_StartUpgrade.IconSize = 32;
		((Control)wibutton_StartUpgrade).Location = new Point(626, 258);
		wibutton_StartUpgrade.MouseDownBackColor = Color.LightSeaGreen;
		wibutton_StartUpgrade.MouseDownForeColor = Color.Black;
		wibutton_StartUpgrade.MouseDownImage = (Image)(object)Resources.ButtonMouseDown;
		wibutton_StartUpgrade.MouseEnterBackColor = Color.LightSeaGreen;
		wibutton_StartUpgrade.MouseEnterForeColor = Color.Black;
		wibutton_StartUpgrade.MouseEnterImage = (Image)(object)Resources.ButtonMouseEnter;
		wibutton_StartUpgrade.MouseUpBackColor = Color.LightSeaGreen;
		wibutton_StartUpgrade.MouseUpForeColor = Color.Black;
		wibutton_StartUpgrade.MouseUpImage = (Image)(object)Resources.ButtonMouseUp;
		((Control)wibutton_StartUpgrade).Name = "wibutton_StartUpgrade";
		wibutton_StartUpgrade.Radius = 16;
		((Control)wibutton_StartUpgrade).Size = new Size(104, 42);
		((Control)wibutton_StartUpgrade).TabIndex = 99;
		((Control)wibutton_StartUpgrade).Tag = "4";
		((Control)wibutton_StartUpgrade).Text = "Upgrade";
		wibutton_StartUpgrade.TextAlignment = StringHelper.TextAlignment.Center;
		wibutton_StartUpgrade.TextDynOffset = new Point(1, 1);
		wibutton_StartUpgrade.TextFixLocation = new Point(0, 0);
		wibutton_StartUpgrade.TextFixLocationEnable = false;
		((Control)wibutton_StartUpgrade).MouseDown += new MouseEventHandler(DragRect_MouseDown);
		((Control)wibutton_StartUpgrade).MouseUp += new MouseEventHandler(DragRect_MouseUp);
		((Control)wiButton_custom_mini).BackColor = Color.Transparent;
		wiButton_custom_mini.DisableBackColor = Color.DarkGray;
		wiButton_custom_mini.DisableForeColor = Color.DimGray;
		((Control)wiButton_custom_mini).Font = new Font("微软雅黑", 24f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wiButton_custom_mini.FrameMode = GraphicsHelper.RoundStyle.All;
		wiButton_custom_mini.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wiButton_custom_mini.IconName = "";
		wiButton_custom_mini.IconOffset = new Point(0, 0);
		wiButton_custom_mini.IconSize = 32;
		((Control)wiButton_custom_mini).Location = new Point(696, 69);
		wiButton_custom_mini.MouseDownBackColor = Color.Transparent;
		wiButton_custom_mini.MouseDownForeColor = Color.Black;
		wiButton_custom_mini.MouseEnterBackColor = Color.Transparent;
		wiButton_custom_mini.MouseEnterForeColor = Color.Black;
		wiButton_custom_mini.MouseUpBackColor = Color.Transparent;
		wiButton_custom_mini.MouseUpForeColor = Color.Black;
		((Control)wiButton_custom_mini).Name = "wiButton_custom_mini";
		wiButton_custom_mini.Radius = 0;
		((Control)wiButton_custom_mini).Size = new Size(24, 24);
		((Control)wiButton_custom_mini).TabIndex = 98;
		((Control)wiButton_custom_mini).Text = "-";
		wiButton_custom_mini.TextAlignment = StringHelper.TextAlignment.Center;
		wiButton_custom_mini.TextDynOffset = new Point(1, 1);
		wiButton_custom_mini.TextFixLocation = new Point(0, 0);
		wiButton_custom_mini.TextFixLocationEnable = false;
		((Control)wiButton_custom_mini).MouseDown += new MouseEventHandler(DragRect_MouseDown);
		((Control)wiButton_custom_mini).MouseUp += new MouseEventHandler(DragRect_MouseUp);
		((Control)wiButton_custom_close).BackColor = Color.Transparent;
		wiButton_custom_close.DisableBackColor = Color.DarkGray;
		wiButton_custom_close.DisableForeColor = Color.DimGray;
		((Control)wiButton_custom_close).Font = new Font("微软雅黑", 24f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wiButton_custom_close.FrameMode = GraphicsHelper.RoundStyle.All;
		wiButton_custom_close.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wiButton_custom_close.IconName = "";
		wiButton_custom_close.IconOffset = new Point(0, 0);
		wiButton_custom_close.IconSize = 32;
		((Control)wiButton_custom_close).Location = new Point(731, 69);
		wiButton_custom_close.MouseDownBackColor = Color.Transparent;
		wiButton_custom_close.MouseDownForeColor = Color.Black;
		wiButton_custom_close.MouseEnterBackColor = Color.Transparent;
		wiButton_custom_close.MouseEnterForeColor = Color.Black;
		wiButton_custom_close.MouseUpBackColor = Color.Transparent;
		wiButton_custom_close.MouseUpForeColor = Color.Black;
		((Control)wiButton_custom_close).Name = "wiButton_custom_close";
		wiButton_custom_close.Radius = 0;
		((Control)wiButton_custom_close).Size = new Size(24, 24);
		((Control)wiButton_custom_close).TabIndex = 97;
		((Control)wiButton_custom_close).Text = "×";
		wiButton_custom_close.TextAlignment = StringHelper.TextAlignment.Center;
		wiButton_custom_close.TextDynOffset = new Point(1, 1);
		wiButton_custom_close.TextFixLocation = new Point(0, 0);
		wiButton_custom_close.TextFixLocationEnable = false;
		((Control)wiButton_custom_close).MouseDown += new MouseEventHandler(DragRect_MouseDown);
		((Control)wiButton_custom_close).MouseUp += new MouseEventHandler(DragRect_MouseUp);
		((Control)wiButton_createExe).BackColor = Color.Transparent;
		wiButton_createExe.DisableBackColor = Color.DarkGray;
		wiButton_createExe.DisableForeColor = Color.DimGray;
		((Control)wiButton_createExe).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wiButton_createExe.FrameMode = GraphicsHelper.RoundStyle.All;
		wiButton_createExe.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wiButton_createExe.IconName = "";
		wiButton_createExe.IconOffset = new Point(0, 0);
		wiButton_createExe.IconSize = 32;
		((Control)wiButton_createExe).Location = new Point(13, 556);
		wiButton_createExe.MouseDownBackColor = Color.MediumTurquoise;
		wiButton_createExe.MouseDownForeColor = Color.Black;
		wiButton_createExe.MouseEnterBackColor = Color.Turquoise;
		wiButton_createExe.MouseEnterForeColor = Color.Black;
		wiButton_createExe.MouseUpBackColor = Color.LightSeaGreen;
		wiButton_createExe.MouseUpForeColor = Color.Black;
		((Control)wiButton_createExe).Name = "wiButton_createExe";
		wiButton_createExe.Radius = 16;
		((Control)wiButton_createExe).Size = new Size(133, 35);
		((Control)wiButton_createExe).TabIndex = 87;
		((Control)wiButton_createExe).Text = "生成升级工具";
		wiButton_createExe.TextAlignment = StringHelper.TextAlignment.Center;
		wiButton_createExe.TextDynOffset = new Point(0, 0);
		wiButton_createExe.TextFixLocation = new Point(0, 0);
		wiButton_createExe.TextFixLocationEnable = false;
		((Control)wiButton_createExe).Click += wiButton_createExe_Click;
		((Control)wTextBox_ToolVersion).BackColor = Color.Transparent;
		wTextBox_ToolVersion.DisableBackImage = null;
		((Control)wTextBox_ToolVersion).Font = new Font("微软雅黑", 9f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wTextBox_ToolVersion.FrameColor = Color.White;
		wTextBox_ToolVersion.InputMode = WindBaseTextBox.TextBoxInputMode.Normal;
		((Control)wTextBox_ToolVersion).Location = new Point(219, 345);
		((Control)wTextBox_ToolVersion).Margin = new Padding(4);
		wTextBox_ToolVersion.MaxLength = int.MaxValue;
		((Control)wTextBox_ToolVersion).Name = "wTextBox_ToolVersion";
		wTextBox_ToolVersion.PasswordChar = "";
		wTextBox_ToolVersion.PromptText = "";
		wTextBox_ToolVersion.PromptTextColor = Color.Gray;
		wTextBox_ToolVersion.PromptTextForeColor = Color.LightGray;
		wTextBox_ToolVersion.ReadOnly = false;
		wTextBox_ToolVersion.SelectedBackImage = null;
		((Control)wTextBox_ToolVersion).Size = new Size(197, 27);
		((Control)wTextBox_ToolVersion).TabIndex = 81;
		((Control)wTextBox_ToolVersion).Tag = "3";
		((Control)wTextBox_ToolVersion).Text = "Tool ver: v0.32";
		wTextBox_ToolVersion.TextBoxOffset = new Point(0, 0);
		wTextBox_ToolVersion.UnSelectedBackImage = null;
		((Control)wTextBox_ToolVersion).MouseDown += new MouseEventHandler(DragRect_MouseDown);
		((Control)wTextBox_ToolVersion).MouseUp += new MouseEventHandler(DragRect_MouseUp);
		((Control)wTextBox_newFwVersion).BackColor = Color.Transparent;
		wTextBox_newFwVersion.DisableBackImage = null;
		((Control)wTextBox_newFwVersion).Font = new Font("微软雅黑", 9f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wTextBox_newFwVersion.FrameColor = Color.White;
		wTextBox_newFwVersion.InputMode = WindBaseTextBox.TextBoxInputMode.Normal;
		((Control)wTextBox_newFwVersion).Location = new Point(222, 200);
		((Control)wTextBox_newFwVersion).Margin = new Padding(4);
		wTextBox_newFwVersion.MaxLength = int.MaxValue;
		((Control)wTextBox_newFwVersion).Name = "wTextBox_newFwVersion";
		wTextBox_newFwVersion.PasswordChar = "";
		wTextBox_newFwVersion.PromptText = "";
		wTextBox_newFwVersion.PromptTextColor = Color.Gray;
		wTextBox_newFwVersion.PromptTextForeColor = Color.LightGray;
		wTextBox_newFwVersion.ReadOnly = false;
		wTextBox_newFwVersion.SelectedBackImage = null;
		((Control)wTextBox_newFwVersion).Size = new Size(197, 27);
		((Control)wTextBox_newFwVersion).TabIndex = 80;
		((Control)wTextBox_newFwVersion).Tag = "2";
		((Control)wTextBox_newFwVersion).Text = "New FW Version: ";
		wTextBox_newFwVersion.TextBoxOffset = new Point(0, 0);
		wTextBox_newFwVersion.UnSelectedBackImage = null;
		((Control)wTextBox_newFwVersion).MouseDown += new MouseEventHandler(DragRect_MouseDown);
		((Control)wTextBox_newFwVersion).MouseUp += new MouseEventHandler(DragRect_MouseUp);
		((Control)wTextBox_curFwVersion).BackColor = Color.Transparent;
		wTextBox_curFwVersion.DisableBackImage = null;
		((Control)wTextBox_curFwVersion).Font = new Font("微软雅黑", 9f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wTextBox_curFwVersion.FrameColor = Color.White;
		wTextBox_curFwVersion.InputMode = WindBaseTextBox.TextBoxInputMode.Normal;
		((Control)wTextBox_curFwVersion).Location = new Point(222, 157);
		((Control)wTextBox_curFwVersion).Margin = new Padding(4);
		wTextBox_curFwVersion.MaxLength = int.MaxValue;
		((Control)wTextBox_curFwVersion).Name = "wTextBox_curFwVersion";
		wTextBox_curFwVersion.PasswordChar = "";
		wTextBox_curFwVersion.PromptText = "";
		wTextBox_curFwVersion.PromptTextColor = Color.Gray;
		wTextBox_curFwVersion.PromptTextForeColor = Color.LightGray;
		wTextBox_curFwVersion.ReadOnly = false;
		wTextBox_curFwVersion.SelectedBackImage = null;
		((Control)wTextBox_curFwVersion).Size = new Size(197, 27);
		((Control)wTextBox_curFwVersion).TabIndex = 79;
		((Control)wTextBox_curFwVersion).Tag = "1";
		((Control)wTextBox_curFwVersion).Text = "Current FW Version: ";
		wTextBox_curFwVersion.TextBoxOffset = new Point(0, 0);
		wTextBox_curFwVersion.UnSelectedBackImage = null;
		((Control)wTextBox_curFwVersion).MouseDown += new MouseEventHandler(DragRect_MouseDown);
		((Control)wTextBox_curFwVersion).MouseUp += new MouseEventHandler(DragRect_MouseUp);
		((Control)wTextBox_caption).BackColor = Color.Transparent;
		wTextBox_caption.DisableBackImage = null;
		((Control)wTextBox_caption).Font = new Font("微软雅黑", 18f);
		wTextBox_caption.FrameColor = Color.White;
		wTextBox_caption.InputMode = WindBaseTextBox.TextBoxInputMode.Normal;
		((Control)wTextBox_caption).Location = new Point(222, 105);
		((Control)wTextBox_caption).Margin = new Padding(6);
		wTextBox_caption.MaxLength = int.MaxValue;
		((Control)wTextBox_caption).Name = "wTextBox_caption";
		wTextBox_caption.PasswordChar = "";
		wTextBox_caption.PromptText = "x20 Gaming Mouse Upgrade Tool";
		wTextBox_caption.PromptTextColor = Color.Gray;
		wTextBox_caption.PromptTextForeColor = Color.LightGray;
		wTextBox_caption.ReadOnly = false;
		wTextBox_caption.SelectedBackImage = null;
		((Control)wTextBox_caption).Size = new Size(302, 36);
		((Control)wTextBox_caption).TabIndex = 78;
		((Control)wTextBox_caption).Tag = "0";
		((Control)wTextBox_caption).Text = "Mouse Upgrade Tool";
		wTextBox_caption.TextBoxOffset = new Point(0, 0);
		wTextBox_caption.UnSelectedBackImage = null;
		((Control)wTextBox_caption).MouseDown += new MouseEventHandler(DragRect_MouseDown);
		((Control)wTextBox_caption).MouseUp += new MouseEventHandler(DragRect_MouseUp);
		((Control)wiButton_Mini).BackColor = Color.Transparent;
		((Control)wiButton_Mini).BackgroundImage = (Image)(object)Resources.MiniButtonNormalImage;
		((Control)wiButton_Mini).BackgroundImageLayout = (ImageLayout)2;
		wiButton_Mini.DisableBackColor = Color.Transparent;
		wiButton_Mini.DisableForeColor = Color.DarkGray;
		wiButton_Mini.DisableImage = (Image)(object)Resources.MiniButtonMouseDownImage;
		wiButton_Mini.FrameMode = GraphicsHelper.RoundStyle.All;
		wiButton_Mini.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wiButton_Mini.IconName = "";
		wiButton_Mini.IconOffset = new Point(0, 0);
		wiButton_Mini.IconSize = 32;
		((Control)wiButton_Mini).Location = new Point(745, 26);
		wiButton_Mini.MouseDownBackColor = Color.Gray;
		wiButton_Mini.MouseDownForeColor = Color.DarkRed;
		wiButton_Mini.MouseDownImage = (Image)(object)Resources.MiniButtonMouseDownImage;
		wiButton_Mini.MouseEnterBackColor = Color.DarkGray;
		wiButton_Mini.MouseEnterForeColor = Color.OrangeRed;
		wiButton_Mini.MouseEnterImage = (Image)(object)Resources.MiniButtonMouseEnterImage;
		wiButton_Mini.MouseUpBackColor = Color.Transparent;
		wiButton_Mini.MouseUpForeColor = Color.Red;
		wiButton_Mini.MouseUpImage = (Image)(object)Resources.MiniButtonNormalImage;
		((Control)wiButton_Mini).Name = "wiButton_Mini";
		wiButton_Mini.Radius = 12;
		((Control)wiButton_Mini).Size = new Size(15, 15);
		((Control)wiButton_Mini).TabIndex = 45;
		((Control)wiButton_Mini).Text = null;
		wiButton_Mini.TextAlignment = StringHelper.TextAlignment.Center;
		wiButton_Mini.TextDynOffset = new Point(0, 0);
		wiButton_Mini.TextFixLocation = new Point(0, 0);
		wiButton_Mini.TextFixLocationEnable = false;
		((Control)wiButton_Mini).Click += wiButton_Mini_Click;
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
		((Control)wiButton_Close).Location = new Point(775, 26);
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
		((Control)wiButton_Close).Size = new Size(15, 15);
		((Control)wiButton_Close).TabIndex = 44;
		((Control)wiButton_Close).Text = null;
		wiButton_Close.TextAlignment = StringHelper.TextAlignment.Center;
		wiButton_Close.TextDynOffset = new Point(0, 0);
		wiButton_Close.TextFixLocation = new Point(0, 0);
		wiButton_Close.TextFixLocationEnable = false;
		((Control)wiButton_Close).Click += wiButton_Close_Click;
		((Control)wIButton_showHide).BackColor = Color.Transparent;
		wIButton_showHide.DisableBackColor = Color.DarkGray;
		wIButton_showHide.DisableForeColor = Color.DimGray;
		((Control)wIButton_showHide).Enabled = false;
		((Control)wIButton_showHide).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wIButton_showHide.FrameMode = GraphicsHelper.RoundStyle.All;
		wIButton_showHide.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wIButton_showHide.IconName = "";
		wIButton_showHide.IconOffset = new Point(0, 0);
		wIButton_showHide.IconSize = 32;
		((Control)wIButton_showHide).Location = new Point(13, 349);
		wIButton_showHide.MouseDownBackColor = Color.DimGray;
		wIButton_showHide.MouseDownForeColor = Color.Black;
		wIButton_showHide.MouseEnterBackColor = Color.Gray;
		wIButton_showHide.MouseEnterForeColor = Color.Black;
		wIButton_showHide.MouseUpBackColor = Color.DarkGray;
		wIButton_showHide.MouseUpForeColor = Color.Black;
		((Control)wIButton_showHide).Name = "wIButton_showHide";
		wIButton_showHide.Radius = 8;
		((Control)wIButton_showHide).Size = new Size(136, 24);
		((Control)wIButton_showHide).TabIndex = 206;
		((Control)wIButton_showHide).Text = "显示/隐藏";
		wIButton_showHide.TextAlignment = StringHelper.TextAlignment.Center;
		wIButton_showHide.TextDynOffset = new Point(0, 0);
		wIButton_showHide.TextFixLocation = new Point(0, 0);
		wIButton_showHide.TextFixLocationEnable = false;
		((Control)wIButton_showHide).Click += wIButton_showHide_Click;
		((Control)label2).AutoSize = true;
		((Control)label2).BackColor = Color.Transparent;
		((Control)label2).Location = new Point(19, 477);
		((Control)label2).Name = "label2";
		((Control)label2).Size = new Size(107, 20);
		((Control)label2).TabIndex = 210;
		((Control)label2).Text = "任务栏显示名称";
		((Control)wTextBox_ExeCaption).BackgroundImage = (Image)(object)Resources.选中文件框;
		((Control)wTextBox_ExeCaption).BackgroundImageLayout = (ImageLayout)3;
		wTextBox_ExeCaption.DisableBackImage = null;
		((Control)wTextBox_ExeCaption).Font = new Font("微软雅黑", 9f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wTextBox_ExeCaption.FrameColor = Color.White;
		wTextBox_ExeCaption.InputMode = WindBaseTextBox.TextBoxInputMode.Normal;
		((Control)wTextBox_ExeCaption).Location = new Point(15, 506);
		((Control)wTextBox_ExeCaption).Margin = new Padding(6);
		wTextBox_ExeCaption.MaxLength = int.MaxValue;
		((Control)wTextBox_ExeCaption).Name = "wTextBox_ExeCaption";
		wTextBox_ExeCaption.PasswordChar = "";
		wTextBox_ExeCaption.PromptText = "";
		wTextBox_ExeCaption.PromptTextColor = Color.Gray;
		wTextBox_ExeCaption.PromptTextForeColor = Color.LightGray;
		wTextBox_ExeCaption.ReadOnly = false;
		wTextBox_ExeCaption.SelectedBackImage = (Image)(object)Resources.选中文件框;
		((Control)wTextBox_ExeCaption).Size = new Size(133, 29);
		((Control)wTextBox_ExeCaption).TabIndex = 209;
		wTextBox_ExeCaption.TextBoxOffset = new Point(6, 6);
		wTextBox_ExeCaption.UnSelectedBackImage = (Image)(object)Resources.选中文件框;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackgroundImage = (Image)(object)Resources.backBig;
		((Form)this).ClientSize = new Size(1376, 615);
		((Control)this).Controls.Add((Control)(object)label2);
		((Control)this).Controls.Add((Control)(object)wTextBox_ExeCaption);
		((Control)this).Controls.Add((Control)(object)wIButton_showHide);
		((Control)this).Controls.Add((Control)(object)tipControl1);
		((Control)this).Controls.Add((Control)(object)imageBar1);
		((Control)this).Controls.Add((Control)(object)wImageButton_saveConfig);
		((Control)this).Controls.Add((Control)(object)wibutton_FailStartUpgrade);
		((Control)this).Controls.Add((Control)(object)wiButton_textFont);
		((Control)this).Controls.Add((Control)(object)cellBar1);
		((Control)this).Controls.Add((Control)(object)wImageButton_loadConfig);
		((Control)this).Controls.Add((Control)(object)dragControlRect1);
		((Control)this).Controls.Add((Control)(object)roundBar1);
		((Control)this).Controls.Add((Control)(object)sliderBar1);
		((Control)this).Controls.Add((Control)(object)haloBar1);
		((Control)this).Controls.Add((Control)(object)wImageButton_progressStyle);
		((Control)this).Controls.Add((Control)(object)wiButton_load_code);
		((Control)this).Controls.Add((Control)(object)editCidMid);
		((Control)this).Controls.Add((Control)(object)wTextBox_fileName);
		((Control)this).Controls.Add((Control)(object)editBootVidPid);
		((Control)this).Controls.Add((Control)(object)editNormalVidPid);
		((Control)this).Controls.Add((Control)(object)wiButton_buttonSet);
		((Control)this).Controls.Add((Control)(object)wiButton_textColor);
		((Control)this).Controls.Add((Control)(object)wiButton_Icon);
		((Control)this).Controls.Add((Control)(object)wIButton_upgradetool);
		((Control)this).Controls.Add((Control)(object)wCheckBox_IC_Enable);
		((Control)this).Controls.Add((Control)(object)wcobBox_IC_type);
		((Control)this).Controls.Add((Control)(object)wibutton_StartUpgrade);
		((Control)this).Controls.Add((Control)(object)wiButton_custom_mini);
		((Control)this).Controls.Add((Control)(object)wiButton_custom_close);
		((Control)this).Controls.Add((Control)(object)wiButton_createExe);
		((Control)this).Controls.Add((Control)(object)wTextBox_ToolVersion);
		((Control)this).Controls.Add((Control)(object)wTextBox_newFwVersion);
		((Control)this).Controls.Add((Control)(object)wTextBox_curFwVersion);
		((Control)this).Controls.Add((Control)(object)wTextBox_caption);
		((Control)this).Controls.Add((Control)(object)pictureBox_icon);
		((Control)this).Controls.Add((Control)(object)pictureBox1);
		((Control)this).Controls.Add((Control)(object)wiButton_Mini);
		((Control)this).Controls.Add((Control)(object)wiButton_Close);
		((Control)this).DoubleBuffered = true;
		((Control)this).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Form)this).KeyPreview = true;
		((Control)this).Name = "FormMakeUpgradeTool";
		((Form)this).StartPosition = (FormStartPosition)0;
		((Control)this).Text = "FormCustomAPP";
		((Form)this).Load += FormMakeUpgradeTool_Load;
		((Control)this).Paint += new PaintEventHandler(FormMakeUpgradeTool_Paint);
		((Control)this).KeyDown += new KeyEventHandler(FormMakeUpgradeTool_KeyDown);
		((ISupportInitialize)pictureBox_icon).EndInit();
		((ISupportInitialize)pictureBox1).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
