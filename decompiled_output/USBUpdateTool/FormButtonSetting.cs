using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using USBUpdateTool.Properties;
using WindControls;

namespace USBUpdateTool;

public class FormButtonSetting : Form
{
	public delegate void SetButtonColor(Color textColor, Color foreColor, Color backColor);

	public SetButtonColor OnSetButtonColor;

	public SkinForm skinForm = new SkinForm(_movable: true);

	private Form form;

	private ColorDialog colorDialog = new ColorDialog();

	private OpenFileDialog openFileDialog = new OpenFileDialog();

	private WindImageButton successButton;

	private WindImageButton failButton;

	private FormMover formMover = new FormMover();

	private FormMover formMover2 = new FormMover();

	private IContainer components = null;

	private WindImageButton wiButton_mini;

	private WindImageButton wiButton_Close;

	private WindImageButton wiButton_OK;

	private WindImageButton wiButton_buttonFailColor;

	private WindImageButton wiButton_buttonColor;

	private WindImageButton wibutton_FailStartUpgrade;

	private WindImageButton wibutton_StartUpgrade;

	private GroupBox groupBox1;

	private GroupBox groupBox2;

	private WindImageButton wIButton_FA_MouseDownImage;

	private WindImageButton wIButton_FA_MouseEnterImag;

	private WindImageButton wIButton_SU_MouseDownImage;

	private WindImageButton wIButton_SU_MouseEnterImage;

	private WindImageButton wIButton_ImageUpgrade;

	private WindImageButton wIButton_ImageUpgrade_Fail;

	private WindImageButton wIButton_SU_NormalImage;

	private WindImageButton wIButton_FA_NormalImag;

	private WindRadioButton wRButton_ColorMode;

	private WindRadioButton wRButton_ImageMode;

	public FormButtonSetting(WindImageButton _successButton, WindImageButton _failButton)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		InitializeComponent();
		SetStyles();
		skinForm.InitSkin((Form)(object)this, 12, 16);
		successButton = _successButton;
		failButton = _failButton;
		((FileDialog)openFileDialog).Title = "Open File";
		((FileDialog)openFileDialog).Filter = "Png Files|*.png";
		((FileDialog)openFileDialog).FilterIndex = 1;
		openFileDialog.Multiselect = false;
		((FileDialog)openFileDialog).RestoreDirectory = true;
		formMover.AddControl((Control)(object)wIButton_ImageUpgrade);
		formMover2.AddControl((Control)(object)wIButton_ImageUpgrade_Fail);
		((Control)wRButton_ImageMode).Text = "选中";
		((Control)wRButton_ColorMode).Text = "";
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
		form.Activate();
	}

	public void ShowWindow(Form _form)
	{
		form = _form;
		skinForm.AllShow(form);
		((Form)this).Location = form.Location;
	}

	private void wiButton_OK_Click(object sender, EventArgs e)
	{
		if (wRButton_ImageMode.Checked)
		{
			successButton.MouseUpImage = wIButton_ImageUpgrade.MouseUpImage;
			successButton.MouseEnterImage = wIButton_ImageUpgrade.MouseEnterImage;
			successButton.MouseDownImage = wIButton_ImageUpgrade.MouseDownImage;
			failButton.MouseUpImage = wIButton_ImageUpgrade_Fail.MouseUpImage;
			failButton.MouseEnterImage = wIButton_ImageUpgrade_Fail.MouseEnterImage;
			failButton.MouseDownImage = wIButton_ImageUpgrade_Fail.MouseDownImage;
		}
		else
		{
			successButton.MouseUpImage = null;
			successButton.MouseEnterImage = null;
			successButton.MouseDownImage = null;
			failButton.MouseUpImage = null;
			failButton.MouseEnterImage = null;
			failButton.MouseDownImage = null;
			successButton.SetMouseBackColor(wibutton_StartUpgrade.MouseUpBackColor);
			failButton.SetMouseBackColor(wibutton_FailStartUpgrade.MouseUpBackColor);
		}
		wiButton_Close_Click(null, null);
	}

	private void wiButton_buttonColor_Click(object sender, EventArgs e)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Invalid comparison between Unknown and I4
		if ((int)((CommonDialog)colorDialog).ShowDialog() == 1)
		{
			wibutton_StartUpgrade.SetMouseBackColor(colorDialog.Color);
		}
	}

	private void wiButton_buttonFailColor_Click(object sender, EventArgs e)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Invalid comparison between Unknown and I4
		if ((int)((CommonDialog)colorDialog).ShowDialog() == 1)
		{
			wibutton_FailStartUpgrade.SetMouseBackColor(colorDialog.Color);
		}
	}

	private void wIButton_SU_NormalImage_Click(object sender, EventArgs e)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Invalid comparison between Unknown and I4
		if ((int)((CommonDialog)openFileDialog).ShowDialog() == 1)
		{
			wIButton_ImageUpgrade.MouseUpImage = ImageHelper.GetImage(((FileDialog)openFileDialog).FileName);
			((Control)wIButton_ImageUpgrade).Width = wIButton_ImageUpgrade.MouseUpImage.Width;
			((Control)wIButton_ImageUpgrade).Height = wIButton_ImageUpgrade.MouseUpImage.Height;
		}
	}

	private void wIButton_SU_MouseEnterImage_Click(object sender, EventArgs e)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Invalid comparison between Unknown and I4
		if ((int)((CommonDialog)openFileDialog).ShowDialog() == 1)
		{
			wIButton_ImageUpgrade.MouseEnterImage = ImageHelper.GetImage(((FileDialog)openFileDialog).FileName);
			((Control)wIButton_ImageUpgrade).Width = wIButton_ImageUpgrade.MouseEnterImage.Width;
			((Control)wIButton_ImageUpgrade).Height = wIButton_ImageUpgrade.MouseEnterImage.Height;
		}
	}

	private void wIButton_SU_MouseDownImage_Click(object sender, EventArgs e)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Invalid comparison between Unknown and I4
		if ((int)((CommonDialog)openFileDialog).ShowDialog() == 1)
		{
			wIButton_ImageUpgrade.MouseDownImage = ImageHelper.GetImage(((FileDialog)openFileDialog).FileName);
			((Control)wIButton_ImageUpgrade).Width = wIButton_ImageUpgrade.MouseDownImage.Width;
			((Control)wIButton_ImageUpgrade).Height = wIButton_ImageUpgrade.MouseDownImage.Height;
		}
	}

	private void wIButton_FA_NormalImag_Click(object sender, EventArgs e)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Invalid comparison between Unknown and I4
		if ((int)((CommonDialog)openFileDialog).ShowDialog() == 1)
		{
			wIButton_ImageUpgrade_Fail.MouseUpImage = ImageHelper.GetImage(((FileDialog)openFileDialog).FileName);
			((Control)wIButton_ImageUpgrade_Fail).Width = wIButton_ImageUpgrade.MouseUpImage.Width;
			((Control)wIButton_ImageUpgrade_Fail).Height = wIButton_ImageUpgrade.MouseUpImage.Height;
		}
	}

	private void wIButton_FA_MouseEnterImag_Click(object sender, EventArgs e)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Invalid comparison between Unknown and I4
		if ((int)((CommonDialog)openFileDialog).ShowDialog() == 1)
		{
			wIButton_ImageUpgrade_Fail.MouseEnterImage = ImageHelper.GetImage(((FileDialog)openFileDialog).FileName);
			((Control)wIButton_ImageUpgrade_Fail).Width = wIButton_ImageUpgrade.MouseEnterImage.Width;
			((Control)wIButton_ImageUpgrade_Fail).Height = wIButton_ImageUpgrade.MouseEnterImage.Height;
		}
	}

	private void wIButton_FA_MouseDownImage_Click(object sender, EventArgs e)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Invalid comparison between Unknown and I4
		if ((int)((CommonDialog)openFileDialog).ShowDialog() == 1)
		{
			wIButton_ImageUpgrade_Fail.MouseDownImage = ImageHelper.GetImage(((FileDialog)openFileDialog).FileName);
			((Control)wIButton_ImageUpgrade_Fail).Width = wIButton_ImageUpgrade.MouseDownImage.Width;
			((Control)wIButton_ImageUpgrade_Fail).Height = wIButton_ImageUpgrade.MouseDownImage.Height;
		}
	}

	private void wRButton_ImageMode_Click(object sender, EventArgs e)
	{
		wRButton_ColorMode.Checked = false;
		((Control)wRButton_ImageMode).Text = "选中";
		((Control)wRButton_ColorMode).Text = "";
	}

	private void wRButton_ColorMode_Click(object sender, EventArgs e)
	{
		wRButton_ImageMode.Checked = false;
		((Control)wRButton_ColorMode).Text = "选中";
		((Control)wRButton_ImageMode).Text = "";
	}

	public void SetButtons(WindImageButton colorSuccess, WindImageButton colorFail, WindImageButton imageSuccess, WindImageButton imageFail)
	{
		((Control)wibutton_StartUpgrade).Text = ((Control)colorSuccess).Text;
		((Control)wibutton_FailStartUpgrade).Text = ((Control)colorFail).Text;
		((Control)wIButton_ImageUpgrade).Text = ((Control)imageSuccess).Text;
		((Control)wIButton_ImageUpgrade_Fail).Text = ((Control)imageFail).Text;
		((Control)wibutton_StartUpgrade).Width = ((Control)colorSuccess).Width;
		((Control)wibutton_StartUpgrade).Height = ((Control)colorSuccess).Height;
		wibutton_StartUpgrade.SetMouseBackColor(colorSuccess.MouseUpBackColor);
		((Control)wibutton_FailStartUpgrade).Width = ((Control)colorFail).Width;
		((Control)wibutton_FailStartUpgrade).Height = ((Control)colorFail).Height;
		wibutton_FailStartUpgrade.SetMouseBackColor(colorFail.MouseUpBackColor);
		((Control)wIButton_ImageUpgrade).Width = ((Control)imageSuccess).Width;
		((Control)wIButton_ImageUpgrade).Height = ((Control)imageSuccess).Height;
		wIButton_ImageUpgrade.SetMouseBackColor(imageSuccess.MouseUpBackColor);
		if (imageSuccess.MouseUpImage != null)
		{
			wIButton_ImageUpgrade.MouseUpImage = imageSuccess.MouseUpImage;
			wIButton_ImageUpgrade.MouseEnterImage = imageSuccess.MouseEnterImage;
			wIButton_ImageUpgrade.MouseDownImage = imageSuccess.MouseDownImage;
		}
		((Control)wIButton_ImageUpgrade_Fail).Width = ((Control)imageFail).Width;
		((Control)wIButton_ImageUpgrade_Fail).Height = ((Control)imageFail).Height;
		wIButton_ImageUpgrade_Fail.SetMouseBackColor(imageFail.MouseUpBackColor);
		if (imageFail.MouseUpImage != null)
		{
			wIButton_ImageUpgrade_Fail.MouseUpImage = imageFail.MouseUpImage;
			wIButton_ImageUpgrade_Fail.MouseEnterImage = imageFail.MouseEnterImage;
			wIButton_ImageUpgrade_Fail.MouseDownImage = imageFail.MouseDownImage;
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
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Expected O, but got Unknown
		//IL_0753: Unknown result type (might be due to invalid IL or missing references)
		//IL_075d: Expected O, but got Unknown
		//IL_092b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0935: Expected O, but got Unknown
		//IL_0b03: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b0d: Expected O, but got Unknown
		//IL_0cc1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ccb: Expected O, but got Unknown
		//IL_0ee1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eeb: Expected O, but got Unknown
		//IL_0f9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fa5: Expected O, but got Unknown
		//IL_0fb7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fc1: Expected O, but got Unknown
		//IL_103c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1046: Expected O, but got Unknown
		//IL_10c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_10cf: Expected O, but got Unknown
		//IL_11df: Unknown result type (might be due to invalid IL or missing references)
		//IL_11e9: Expected O, but got Unknown
		//IL_129c: Unknown result type (might be due to invalid IL or missing references)
		//IL_12a6: Expected O, but got Unknown
		//IL_12b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_12c2: Expected O, but got Unknown
		//IL_133d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1347: Expected O, but got Unknown
		//IL_13c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_13d0: Expected O, but got Unknown
		//IL_1433: Unknown result type (might be due to invalid IL or missing references)
		//IL_143d: Expected O, but got Unknown
		//IL_160e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1618: Expected O, but got Unknown
		//IL_17e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_17f3: Expected O, but got Unknown
		//IL_19c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_19cb: Expected O, but got Unknown
		//IL_1b99: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ba3: Expected O, but got Unknown
		//IL_1d9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1da5: Expected O, but got Unknown
		//IL_1f8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f99: Expected O, but got Unknown
		//IL_2167: Unknown result type (might be due to invalid IL or missing references)
		//IL_2171: Expected O, but got Unknown
		//IL_2387: Unknown result type (might be due to invalid IL or missing references)
		//IL_2391: Expected O, but got Unknown
		//IL_23a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_23b0: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FormButtonSetting));
		wiButton_OK = new WindImageButton();
		wiButton_mini = new WindImageButton();
		wiButton_Close = new WindImageButton();
		wiButton_buttonFailColor = new WindImageButton();
		wiButton_buttonColor = new WindImageButton();
		wibutton_FailStartUpgrade = new WindImageButton();
		wibutton_StartUpgrade = new WindImageButton();
		groupBox1 = new GroupBox();
		wRButton_ColorMode = new WindRadioButton();
		groupBox2 = new GroupBox();
		wRButton_ImageMode = new WindRadioButton();
		wIButton_FA_MouseDownImage = new WindImageButton();
		wIButton_FA_MouseEnterImag = new WindImageButton();
		wIButton_SU_MouseDownImage = new WindImageButton();
		wIButton_SU_MouseEnterImage = new WindImageButton();
		wIButton_ImageUpgrade = new WindImageButton();
		wIButton_ImageUpgrade_Fail = new WindImageButton();
		wIButton_SU_NormalImage = new WindImageButton();
		wIButton_FA_NormalImag = new WindImageButton();
		((Control)groupBox1).SuspendLayout();
		((Control)groupBox2).SuspendLayout();
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
		((Control)wiButton_OK).Location = new Point(12, 392);
		wiButton_OK.MouseDownBackColor = Color.MediumTurquoise;
		wiButton_OK.MouseDownForeColor = Color.Black;
		wiButton_OK.MouseEnterBackColor = Color.Turquoise;
		wiButton_OK.MouseEnterForeColor = Color.Black;
		wiButton_OK.MouseUpBackColor = Color.LightSeaGreen;
		wiButton_OK.MouseUpForeColor = Color.Black;
		((Control)wiButton_OK).Name = "wiButton_OK";
		wiButton_OK.Radius = 16;
		((Control)wiButton_OK).Size = new Size(140, 35);
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
		((Control)wiButton_buttonFailColor).BackColor = Color.Transparent;
		wiButton_buttonFailColor.DisableBackColor = Color.DarkGray;
		wiButton_buttonFailColor.DisableForeColor = Color.DimGray;
		((Control)wiButton_buttonFailColor).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wiButton_buttonFailColor.FrameMode = GraphicsHelper.RoundStyle.All;
		wiButton_buttonFailColor.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wiButton_buttonFailColor.IconName = "";
		wiButton_buttonFailColor.IconOffset = new Point(0, 0);
		wiButton_buttonFailColor.IconSize = 32;
		((Control)wiButton_buttonFailColor).Location = new Point(160, 103);
		wiButton_buttonFailColor.MouseDownBackColor = Color.DimGray;
		wiButton_buttonFailColor.MouseDownForeColor = Color.Black;
		wiButton_buttonFailColor.MouseEnterBackColor = Color.Gray;
		wiButton_buttonFailColor.MouseEnterForeColor = Color.Black;
		wiButton_buttonFailColor.MouseUpBackColor = Color.DarkGray;
		wiButton_buttonFailColor.MouseUpForeColor = Color.Black;
		((Control)wiButton_buttonFailColor).Name = "wiButton_buttonFailColor";
		wiButton_buttonFailColor.Radius = 8;
		((Control)wiButton_buttonFailColor).Size = new Size(136, 24);
		((Control)wiButton_buttonFailColor).TabIndex = 164;
		((Control)wiButton_buttonFailColor).Text = "失败时按钮背景色";
		wiButton_buttonFailColor.TextAlignment = StringHelper.TextAlignment.Center;
		wiButton_buttonFailColor.TextDynOffset = new Point(0, 0);
		wiButton_buttonFailColor.TextFixLocation = new Point(0, 0);
		wiButton_buttonFailColor.TextFixLocationEnable = false;
		((Control)wiButton_buttonFailColor).Click += wiButton_buttonFailColor_Click;
		((Control)wiButton_buttonColor).BackColor = Color.Transparent;
		wiButton_buttonColor.DisableBackColor = Color.DarkGray;
		wiButton_buttonColor.DisableForeColor = Color.DimGray;
		((Control)wiButton_buttonColor).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wiButton_buttonColor.FrameMode = GraphicsHelper.RoundStyle.All;
		wiButton_buttonColor.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wiButton_buttonColor.IconName = "";
		wiButton_buttonColor.IconOffset = new Point(0, 0);
		wiButton_buttonColor.IconSize = 32;
		((Control)wiButton_buttonColor).Location = new Point(163, 39);
		wiButton_buttonColor.MouseDownBackColor = Color.DimGray;
		wiButton_buttonColor.MouseDownForeColor = Color.Black;
		wiButton_buttonColor.MouseEnterBackColor = Color.Gray;
		wiButton_buttonColor.MouseEnterForeColor = Color.Black;
		wiButton_buttonColor.MouseUpBackColor = Color.DarkGray;
		wiButton_buttonColor.MouseUpForeColor = Color.Black;
		((Control)wiButton_buttonColor).Name = "wiButton_buttonColor";
		wiButton_buttonColor.Radius = 8;
		((Control)wiButton_buttonColor).Size = new Size(136, 24);
		((Control)wiButton_buttonColor).TabIndex = 163;
		((Control)wiButton_buttonColor).Text = "正常时按钮背景色";
		wiButton_buttonColor.TextAlignment = StringHelper.TextAlignment.Center;
		wiButton_buttonColor.TextDynOffset = new Point(0, 0);
		wiButton_buttonColor.TextFixLocation = new Point(0, 0);
		wiButton_buttonColor.TextFixLocationEnable = false;
		((Control)wiButton_buttonColor).Click += wiButton_buttonColor_Click;
		((Control)wibutton_FailStartUpgrade).BackColor = Color.Transparent;
		wibutton_FailStartUpgrade.DisableBackColor = Color.DarkGray;
		wibutton_FailStartUpgrade.DisableForeColor = Color.DimGray;
		((Control)wibutton_FailStartUpgrade).Font = new Font("微软雅黑", 14.25f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wibutton_FailStartUpgrade.FrameMode = GraphicsHelper.RoundStyle.All;
		wibutton_FailStartUpgrade.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wibutton_FailStartUpgrade.IconName = "";
		wibutton_FailStartUpgrade.IconOffset = new Point(0, 0);
		wibutton_FailStartUpgrade.IconSize = 32;
		((Control)wibutton_FailStartUpgrade).Location = new Point(321, 94);
		wibutton_FailStartUpgrade.MouseDownBackColor = Color.Red;
		wibutton_FailStartUpgrade.MouseDownForeColor = Color.Black;
		wibutton_FailStartUpgrade.MouseEnterBackColor = Color.Red;
		wibutton_FailStartUpgrade.MouseEnterForeColor = Color.Black;
		wibutton_FailStartUpgrade.MouseUpBackColor = Color.Red;
		wibutton_FailStartUpgrade.MouseUpForeColor = Color.Black;
		((Control)wibutton_FailStartUpgrade).Name = "wibutton_FailStartUpgrade";
		wibutton_FailStartUpgrade.Radius = 16;
		((Control)wibutton_FailStartUpgrade).Size = new Size(104, 42);
		((Control)wibutton_FailStartUpgrade).TabIndex = 166;
		((Control)wibutton_FailStartUpgrade).Text = "Fail";
		wibutton_FailStartUpgrade.TextAlignment = StringHelper.TextAlignment.Center;
		wibutton_FailStartUpgrade.TextDynOffset = new Point(1, 1);
		wibutton_FailStartUpgrade.TextFixLocation = new Point(0, 0);
		wibutton_FailStartUpgrade.TextFixLocationEnable = false;
		((Control)wibutton_StartUpgrade).BackColor = Color.Transparent;
		wibutton_StartUpgrade.DisableBackColor = Color.DarkGray;
		wibutton_StartUpgrade.DisableForeColor = Color.DimGray;
		((Control)wibutton_StartUpgrade).Font = new Font("微软雅黑", 14.25f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wibutton_StartUpgrade.FrameMode = GraphicsHelper.RoundStyle.All;
		wibutton_StartUpgrade.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wibutton_StartUpgrade.IconName = "";
		wibutton_StartUpgrade.IconOffset = new Point(0, 0);
		wibutton_StartUpgrade.IconSize = 32;
		((Control)wibutton_StartUpgrade).Location = new Point(321, 30);
		wibutton_StartUpgrade.MouseDownBackColor = Color.LightSeaGreen;
		wibutton_StartUpgrade.MouseDownForeColor = Color.Black;
		wibutton_StartUpgrade.MouseEnterBackColor = Color.LightSeaGreen;
		wibutton_StartUpgrade.MouseEnterForeColor = Color.Black;
		wibutton_StartUpgrade.MouseUpBackColor = Color.LightSeaGreen;
		wibutton_StartUpgrade.MouseUpForeColor = Color.Black;
		((Control)wibutton_StartUpgrade).Name = "wibutton_StartUpgrade";
		wibutton_StartUpgrade.Radius = 16;
		((Control)wibutton_StartUpgrade).Size = new Size(104, 42);
		((Control)wibutton_StartUpgrade).TabIndex = 165;
		((Control)wibutton_StartUpgrade).Tag = "4";
		((Control)wibutton_StartUpgrade).Text = "Upgrade";
		wibutton_StartUpgrade.TextAlignment = StringHelper.TextAlignment.Center;
		wibutton_StartUpgrade.TextDynOffset = new Point(1, 1);
		wibutton_StartUpgrade.TextFixLocation = new Point(0, 0);
		wibutton_StartUpgrade.TextFixLocationEnable = false;
		((Control)groupBox1).BackColor = Color.Transparent;
		((Control)groupBox1).Controls.Add((Control)(object)wibutton_StartUpgrade);
		((Control)groupBox1).Controls.Add((Control)(object)wRButton_ColorMode);
		((Control)groupBox1).Controls.Add((Control)(object)wibutton_FailStartUpgrade);
		((Control)groupBox1).Controls.Add((Control)(object)wiButton_buttonColor);
		((Control)groupBox1).Controls.Add((Control)(object)wiButton_buttonFailColor);
		((Control)groupBox1).Font = new Font("微软雅黑", 15f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)groupBox1).Location = new Point(158, 54);
		((Control)groupBox1).Name = "groupBox1";
		((Control)groupBox1).Size = new Size(632, 155);
		((Control)groupBox1).TabIndex = 167;
		groupBox1.TabStop = false;
		((Control)groupBox1).Text = "纯色模式";
		((Control)wRButton_ColorMode).BackColor = Color.Transparent;
		((Control)wRButton_ColorMode).BackgroundImageLayout = (ImageLayout)0;
		wRButton_ColorMode.Checked = false;
		wRButton_ColorMode.DisableSelectedImage = (Image)componentResourceManager.GetObject("wRButton_ColorMode.DisableSelectedImage");
		wRButton_ColorMode.DisableUnSelectedImage = (Image)componentResourceManager.GetObject("wRButton_ColorMode.DisableUnSelectedImage");
		wRButton_ColorMode.IconOffset = new Point(0, 0);
		wRButton_ColorMode.IconSize = 36;
		((Control)wRButton_ColorMode).Location = new Point(31, 61);
		((Control)wRButton_ColorMode).Name = "wRButton_ColorMode";
		wRButton_ColorMode.SelectedIconColor = Color.Red;
		wRButton_ColorMode.SelectedIconName = "A_fa_dot_circle_o";
		wRButton_ColorMode.SelectedImage = (Image)componentResourceManager.GetObject("wRButton_ColorMode.SelectedImage");
		((Control)wRButton_ColorMode).Size = new Size(100, 39);
		((Control)wRButton_ColorMode).TabIndex = 173;
		((Control)wRButton_ColorMode).Text = "选择框";
		wRButton_ColorMode.TextOffset = new Point(0, -2);
		wRButton_ColorMode.UnSelectedIconColor = Color.Gray;
		wRButton_ColorMode.UnSelectedIconName = "A_fa_circle_o";
		wRButton_ColorMode.UnSelectedImage = (Image)componentResourceManager.GetObject("wRButton_ColorMode.UnSelectedImage");
		((Control)wRButton_ColorMode).Click += wRButton_ColorMode_Click;
		((Control)groupBox2).BackColor = Color.Transparent;
		((Control)groupBox2).Controls.Add((Control)(object)wRButton_ImageMode);
		((Control)groupBox2).Controls.Add((Control)(object)wIButton_FA_MouseDownImage);
		((Control)groupBox2).Controls.Add((Control)(object)wIButton_FA_MouseEnterImag);
		((Control)groupBox2).Controls.Add((Control)(object)wIButton_SU_MouseDownImage);
		((Control)groupBox2).Controls.Add((Control)(object)wIButton_SU_MouseEnterImage);
		((Control)groupBox2).Controls.Add((Control)(object)wIButton_ImageUpgrade);
		((Control)groupBox2).Controls.Add((Control)(object)wIButton_ImageUpgrade_Fail);
		((Control)groupBox2).Controls.Add((Control)(object)wIButton_SU_NormalImage);
		((Control)groupBox2).Controls.Add((Control)(object)wIButton_FA_NormalImag);
		((Control)groupBox2).Font = new Font("微软雅黑", 15f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)groupBox2).Location = new Point(158, 215);
		((Control)groupBox2).Name = "groupBox2";
		((Control)groupBox2).Size = new Size(632, 227);
		((Control)groupBox2).TabIndex = 168;
		groupBox2.TabStop = false;
		((Control)groupBox2).Text = "图片模式";
		((Control)wRButton_ImageMode).BackColor = Color.Transparent;
		((Control)wRButton_ImageMode).BackgroundImageLayout = (ImageLayout)0;
		wRButton_ImageMode.Checked = true;
		wRButton_ImageMode.DisableSelectedImage = (Image)componentResourceManager.GetObject("wRButton_ImageMode.DisableSelectedImage");
		wRButton_ImageMode.DisableUnSelectedImage = (Image)componentResourceManager.GetObject("wRButton_ImageMode.DisableUnSelectedImage");
		wRButton_ImageMode.IconOffset = new Point(0, 0);
		wRButton_ImageMode.IconSize = 36;
		((Control)wRButton_ImageMode).Location = new Point(31, 106);
		((Control)wRButton_ImageMode).Name = "wRButton_ImageMode";
		wRButton_ImageMode.SelectedIconColor = Color.Red;
		wRButton_ImageMode.SelectedIconName = "A_fa_dot_circle_o";
		wRButton_ImageMode.SelectedImage = (Image)componentResourceManager.GetObject("wRButton_ImageMode.SelectedImage");
		((Control)wRButton_ImageMode).Size = new Size(112, 39);
		((Control)wRButton_ImageMode).TabIndex = 172;
		((Control)wRButton_ImageMode).Text = "选择框";
		wRButton_ImageMode.TextOffset = new Point(0, -2);
		wRButton_ImageMode.UnSelectedIconColor = Color.Gray;
		wRButton_ImageMode.UnSelectedIconName = "A_fa_circle_o";
		wRButton_ImageMode.UnSelectedImage = (Image)componentResourceManager.GetObject("wRButton_ImageMode.UnSelectedImage");
		((Control)wRButton_ImageMode).Click += wRButton_ImageMode_Click;
		((Control)wIButton_FA_MouseDownImage).BackColor = Color.Transparent;
		wIButton_FA_MouseDownImage.DisableBackColor = Color.DarkGray;
		wIButton_FA_MouseDownImage.DisableForeColor = Color.DimGray;
		((Control)wIButton_FA_MouseDownImage).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wIButton_FA_MouseDownImage.FrameMode = GraphicsHelper.RoundStyle.All;
		wIButton_FA_MouseDownImage.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wIButton_FA_MouseDownImage.IconName = "";
		wIButton_FA_MouseDownImage.IconOffset = new Point(0, 0);
		wIButton_FA_MouseDownImage.IconSize = 32;
		((Control)wIButton_FA_MouseDownImage).Location = new Point(163, 194);
		wIButton_FA_MouseDownImage.MouseDownBackColor = Color.DimGray;
		wIButton_FA_MouseDownImage.MouseDownForeColor = Color.Black;
		wIButton_FA_MouseDownImage.MouseEnterBackColor = Color.Gray;
		wIButton_FA_MouseDownImage.MouseEnterForeColor = Color.Black;
		wIButton_FA_MouseDownImage.MouseUpBackColor = Color.DarkGray;
		wIButton_FA_MouseDownImage.MouseUpForeColor = Color.Black;
		((Control)wIButton_FA_MouseDownImage).Name = "wIButton_FA_MouseDownImage";
		wIButton_FA_MouseDownImage.Radius = 8;
		((Control)wIButton_FA_MouseDownImage).Size = new Size(136, 24);
		((Control)wIButton_FA_MouseDownImage).TabIndex = 170;
		((Control)wIButton_FA_MouseDownImage).Text = "鼠标点击时背景图片";
		wIButton_FA_MouseDownImage.TextAlignment = StringHelper.TextAlignment.Center;
		wIButton_FA_MouseDownImage.TextDynOffset = new Point(0, 0);
		wIButton_FA_MouseDownImage.TextFixLocation = new Point(0, 0);
		wIButton_FA_MouseDownImage.TextFixLocationEnable = false;
		((Control)wIButton_FA_MouseDownImage).Click += wIButton_FA_MouseDownImage_Click;
		((Control)wIButton_FA_MouseEnterImag).BackColor = Color.Transparent;
		wIButton_FA_MouseEnterImag.DisableBackColor = Color.DarkGray;
		wIButton_FA_MouseEnterImag.DisableForeColor = Color.DimGray;
		((Control)wIButton_FA_MouseEnterImag).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wIButton_FA_MouseEnterImag.FrameMode = GraphicsHelper.RoundStyle.All;
		wIButton_FA_MouseEnterImag.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wIButton_FA_MouseEnterImag.IconName = "";
		wIButton_FA_MouseEnterImag.IconOffset = new Point(0, 0);
		wIButton_FA_MouseEnterImag.IconSize = 32;
		((Control)wIButton_FA_MouseEnterImag).Location = new Point(163, 164);
		wIButton_FA_MouseEnterImag.MouseDownBackColor = Color.DimGray;
		wIButton_FA_MouseEnterImag.MouseDownForeColor = Color.Black;
		wIButton_FA_MouseEnterImag.MouseEnterBackColor = Color.Gray;
		wIButton_FA_MouseEnterImag.MouseEnterForeColor = Color.Black;
		wIButton_FA_MouseEnterImag.MouseUpBackColor = Color.DarkGray;
		wIButton_FA_MouseEnterImag.MouseUpForeColor = Color.Black;
		((Control)wIButton_FA_MouseEnterImag).Name = "wIButton_FA_MouseEnterImag";
		wIButton_FA_MouseEnterImag.Radius = 8;
		((Control)wIButton_FA_MouseEnterImag).Size = new Size(136, 24);
		((Control)wIButton_FA_MouseEnterImag).TabIndex = 169;
		((Control)wIButton_FA_MouseEnterImag).Text = "鼠标滑动时背景图片";
		wIButton_FA_MouseEnterImag.TextAlignment = StringHelper.TextAlignment.Center;
		wIButton_FA_MouseEnterImag.TextDynOffset = new Point(0, 0);
		wIButton_FA_MouseEnterImag.TextFixLocation = new Point(0, 0);
		wIButton_FA_MouseEnterImag.TextFixLocationEnable = false;
		((Control)wIButton_FA_MouseEnterImag).Click += wIButton_FA_MouseEnterImag_Click;
		((Control)wIButton_SU_MouseDownImage).BackColor = Color.Transparent;
		wIButton_SU_MouseDownImage.DisableBackColor = Color.DarkGray;
		wIButton_SU_MouseDownImage.DisableForeColor = Color.DimGray;
		((Control)wIButton_SU_MouseDownImage).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wIButton_SU_MouseDownImage.FrameMode = GraphicsHelper.RoundStyle.All;
		wIButton_SU_MouseDownImage.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wIButton_SU_MouseDownImage.IconName = "";
		wIButton_SU_MouseDownImage.IconOffset = new Point(0, 0);
		wIButton_SU_MouseDownImage.IconSize = 32;
		((Control)wIButton_SU_MouseDownImage).Location = new Point(163, 90);
		wIButton_SU_MouseDownImage.MouseDownBackColor = Color.DimGray;
		wIButton_SU_MouseDownImage.MouseDownForeColor = Color.Black;
		wIButton_SU_MouseDownImage.MouseEnterBackColor = Color.Gray;
		wIButton_SU_MouseDownImage.MouseEnterForeColor = Color.Black;
		wIButton_SU_MouseDownImage.MouseUpBackColor = Color.DarkGray;
		wIButton_SU_MouseDownImage.MouseUpForeColor = Color.Black;
		((Control)wIButton_SU_MouseDownImage).Name = "wIButton_SU_MouseDownImage";
		wIButton_SU_MouseDownImage.Radius = 8;
		((Control)wIButton_SU_MouseDownImage).Size = new Size(136, 24);
		((Control)wIButton_SU_MouseDownImage).TabIndex = 168;
		((Control)wIButton_SU_MouseDownImage).Text = "鼠标点击时背景图片";
		wIButton_SU_MouseDownImage.TextAlignment = StringHelper.TextAlignment.Center;
		wIButton_SU_MouseDownImage.TextDynOffset = new Point(0, 0);
		wIButton_SU_MouseDownImage.TextFixLocation = new Point(0, 0);
		wIButton_SU_MouseDownImage.TextFixLocationEnable = false;
		((Control)wIButton_SU_MouseDownImage).Click += wIButton_SU_MouseDownImage_Click;
		((Control)wIButton_SU_MouseEnterImage).BackColor = Color.Transparent;
		wIButton_SU_MouseEnterImage.DisableBackColor = Color.DarkGray;
		wIButton_SU_MouseEnterImage.DisableForeColor = Color.DimGray;
		((Control)wIButton_SU_MouseEnterImage).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wIButton_SU_MouseEnterImage.FrameMode = GraphicsHelper.RoundStyle.All;
		wIButton_SU_MouseEnterImage.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wIButton_SU_MouseEnterImage.IconName = "";
		wIButton_SU_MouseEnterImage.IconOffset = new Point(0, 0);
		wIButton_SU_MouseEnterImage.IconSize = 32;
		((Control)wIButton_SU_MouseEnterImage).Location = new Point(163, 60);
		wIButton_SU_MouseEnterImage.MouseDownBackColor = Color.DimGray;
		wIButton_SU_MouseEnterImage.MouseDownForeColor = Color.Black;
		wIButton_SU_MouseEnterImage.MouseEnterBackColor = Color.Gray;
		wIButton_SU_MouseEnterImage.MouseEnterForeColor = Color.Black;
		wIButton_SU_MouseEnterImage.MouseUpBackColor = Color.DarkGray;
		wIButton_SU_MouseEnterImage.MouseUpForeColor = Color.Black;
		((Control)wIButton_SU_MouseEnterImage).Name = "wIButton_SU_MouseEnterImage";
		wIButton_SU_MouseEnterImage.Radius = 8;
		((Control)wIButton_SU_MouseEnterImage).Size = new Size(136, 24);
		((Control)wIButton_SU_MouseEnterImage).TabIndex = 167;
		((Control)wIButton_SU_MouseEnterImage).Text = "鼠标滑动时背景图片";
		wIButton_SU_MouseEnterImage.TextAlignment = StringHelper.TextAlignment.Center;
		wIButton_SU_MouseEnterImage.TextDynOffset = new Point(0, 0);
		wIButton_SU_MouseEnterImage.TextFixLocation = new Point(0, 0);
		wIButton_SU_MouseEnterImage.TextFixLocationEnable = false;
		((Control)wIButton_SU_MouseEnterImage).Click += wIButton_SU_MouseEnterImage_Click;
		((Control)wIButton_ImageUpgrade).BackColor = Color.Transparent;
		wIButton_ImageUpgrade.DisableBackColor = Color.DarkGray;
		wIButton_ImageUpgrade.DisableForeColor = Color.DimGray;
		((Control)wIButton_ImageUpgrade).Font = new Font("微软雅黑", 14.25f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wIButton_ImageUpgrade.FrameMode = GraphicsHelper.RoundStyle.All;
		wIButton_ImageUpgrade.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wIButton_ImageUpgrade.IconName = "";
		wIButton_ImageUpgrade.IconOffset = new Point(0, 0);
		wIButton_ImageUpgrade.IconSize = 32;
		((Control)wIButton_ImageUpgrade).Location = new Point(321, 51);
		wIButton_ImageUpgrade.MouseDownBackColor = Color.LightSeaGreen;
		wIButton_ImageUpgrade.MouseDownForeColor = Color.Black;
		wIButton_ImageUpgrade.MouseDownImage = (Image)(object)Resources.ButtonMouseDown;
		wIButton_ImageUpgrade.MouseEnterBackColor = Color.LightSeaGreen;
		wIButton_ImageUpgrade.MouseEnterForeColor = Color.Black;
		wIButton_ImageUpgrade.MouseEnterImage = (Image)(object)Resources.ButtonMouseEnter;
		wIButton_ImageUpgrade.MouseUpBackColor = Color.LightSeaGreen;
		wIButton_ImageUpgrade.MouseUpForeColor = Color.Black;
		wIButton_ImageUpgrade.MouseUpImage = (Image)(object)Resources.ButtonMouseUp;
		((Control)wIButton_ImageUpgrade).Name = "wIButton_ImageUpgrade";
		wIButton_ImageUpgrade.Radius = 16;
		((Control)wIButton_ImageUpgrade).Size = new Size(104, 42);
		((Control)wIButton_ImageUpgrade).TabIndex = 165;
		((Control)wIButton_ImageUpgrade).Tag = "4";
		((Control)wIButton_ImageUpgrade).Text = "Upgrade";
		wIButton_ImageUpgrade.TextAlignment = StringHelper.TextAlignment.Center;
		wIButton_ImageUpgrade.TextDynOffset = new Point(1, 1);
		wIButton_ImageUpgrade.TextFixLocation = new Point(0, 0);
		wIButton_ImageUpgrade.TextFixLocationEnable = false;
		((Control)wIButton_ImageUpgrade_Fail).BackColor = Color.Transparent;
		wIButton_ImageUpgrade_Fail.DisableBackColor = Color.DarkGray;
		wIButton_ImageUpgrade_Fail.DisableForeColor = Color.DimGray;
		((Control)wIButton_ImageUpgrade_Fail).Font = new Font("微软雅黑", 14.25f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wIButton_ImageUpgrade_Fail.FrameMode = GraphicsHelper.RoundStyle.All;
		wIButton_ImageUpgrade_Fail.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wIButton_ImageUpgrade_Fail.IconName = "";
		wIButton_ImageUpgrade_Fail.IconOffset = new Point(0, 0);
		wIButton_ImageUpgrade_Fail.IconSize = 32;
		((Control)wIButton_ImageUpgrade_Fail).Location = new Point(321, 155);
		wIButton_ImageUpgrade_Fail.MouseDownBackColor = Color.Red;
		wIButton_ImageUpgrade_Fail.MouseDownForeColor = Color.Black;
		wIButton_ImageUpgrade_Fail.MouseDownImage = (Image)(object)Resources.ButtonMouseDown;
		wIButton_ImageUpgrade_Fail.MouseEnterBackColor = Color.Red;
		wIButton_ImageUpgrade_Fail.MouseEnterForeColor = Color.Black;
		wIButton_ImageUpgrade_Fail.MouseEnterImage = (Image)(object)Resources.ButtonMouseEnter;
		wIButton_ImageUpgrade_Fail.MouseUpBackColor = Color.Red;
		wIButton_ImageUpgrade_Fail.MouseUpForeColor = Color.Black;
		wIButton_ImageUpgrade_Fail.MouseUpImage = (Image)(object)Resources.ButtonMouseUp;
		((Control)wIButton_ImageUpgrade_Fail).Name = "wIButton_ImageUpgrade_Fail";
		wIButton_ImageUpgrade_Fail.Radius = 16;
		((Control)wIButton_ImageUpgrade_Fail).Size = new Size(104, 42);
		((Control)wIButton_ImageUpgrade_Fail).TabIndex = 166;
		((Control)wIButton_ImageUpgrade_Fail).Text = "Fail";
		wIButton_ImageUpgrade_Fail.TextAlignment = StringHelper.TextAlignment.Center;
		wIButton_ImageUpgrade_Fail.TextDynOffset = new Point(1, 1);
		wIButton_ImageUpgrade_Fail.TextFixLocation = new Point(0, 0);
		wIButton_ImageUpgrade_Fail.TextFixLocationEnable = false;
		((Control)wIButton_SU_NormalImage).BackColor = Color.Transparent;
		wIButton_SU_NormalImage.DisableBackColor = Color.DarkGray;
		wIButton_SU_NormalImage.DisableForeColor = Color.DimGray;
		((Control)wIButton_SU_NormalImage).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wIButton_SU_NormalImage.FrameMode = GraphicsHelper.RoundStyle.All;
		wIButton_SU_NormalImage.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wIButton_SU_NormalImage.IconName = "";
		wIButton_SU_NormalImage.IconOffset = new Point(0, 0);
		wIButton_SU_NormalImage.IconSize = 32;
		((Control)wIButton_SU_NormalImage).Location = new Point(163, 30);
		wIButton_SU_NormalImage.MouseDownBackColor = Color.DimGray;
		wIButton_SU_NormalImage.MouseDownForeColor = Color.Black;
		wIButton_SU_NormalImage.MouseEnterBackColor = Color.Gray;
		wIButton_SU_NormalImage.MouseEnterForeColor = Color.Black;
		wIButton_SU_NormalImage.MouseUpBackColor = Color.DarkGray;
		wIButton_SU_NormalImage.MouseUpForeColor = Color.Black;
		((Control)wIButton_SU_NormalImage).Name = "wIButton_SU_NormalImage";
		wIButton_SU_NormalImage.Radius = 8;
		((Control)wIButton_SU_NormalImage).Size = new Size(136, 24);
		((Control)wIButton_SU_NormalImage).TabIndex = 163;
		((Control)wIButton_SU_NormalImage).Text = "正常时背景图片";
		wIButton_SU_NormalImage.TextAlignment = StringHelper.TextAlignment.Center;
		wIButton_SU_NormalImage.TextDynOffset = new Point(0, 0);
		wIButton_SU_NormalImage.TextFixLocation = new Point(0, 0);
		wIButton_SU_NormalImage.TextFixLocationEnable = false;
		((Control)wIButton_SU_NormalImage).Click += wIButton_SU_NormalImage_Click;
		((Control)wIButton_FA_NormalImag).BackColor = Color.Transparent;
		wIButton_FA_NormalImag.DisableBackColor = Color.DarkGray;
		wIButton_FA_NormalImag.DisableForeColor = Color.DimGray;
		((Control)wIButton_FA_NormalImag).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wIButton_FA_NormalImag.FrameMode = GraphicsHelper.RoundStyle.All;
		wIButton_FA_NormalImag.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wIButton_FA_NormalImag.IconName = "";
		wIButton_FA_NormalImag.IconOffset = new Point(0, 0);
		wIButton_FA_NormalImag.IconSize = 32;
		((Control)wIButton_FA_NormalImag).Location = new Point(163, 134);
		wIButton_FA_NormalImag.MouseDownBackColor = Color.DimGray;
		wIButton_FA_NormalImag.MouseDownForeColor = Color.Black;
		wIButton_FA_NormalImag.MouseEnterBackColor = Color.Gray;
		wIButton_FA_NormalImag.MouseEnterForeColor = Color.Black;
		wIButton_FA_NormalImag.MouseUpBackColor = Color.DarkGray;
		wIButton_FA_NormalImag.MouseUpForeColor = Color.Black;
		((Control)wIButton_FA_NormalImag).Name = "wIButton_FA_NormalImag";
		wIButton_FA_NormalImag.Radius = 8;
		((Control)wIButton_FA_NormalImag).Size = new Size(136, 24);
		((Control)wIButton_FA_NormalImag).TabIndex = 164;
		((Control)wIButton_FA_NormalImag).Text = "正常时背景图片";
		wIButton_FA_NormalImag.TextAlignment = StringHelper.TextAlignment.Center;
		wIButton_FA_NormalImag.TextDynOffset = new Point(0, 0);
		wIButton_FA_NormalImag.TextFixLocation = new Point(0, 0);
		wIButton_FA_NormalImag.TextFixLocationEnable = false;
		((Control)wIButton_FA_NormalImag).Click += wIButton_FA_NormalImag_Click;
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)0;
		((Control)this).BackgroundImage = (Image)(object)Resources.cidmid;
		((Form)this).ClientSize = new Size(822, 465);
		((Control)this).Controls.Add((Control)(object)groupBox2);
		((Control)this).Controls.Add((Control)(object)groupBox1);
		((Control)this).Controls.Add((Control)(object)wiButton_OK);
		((Control)this).Controls.Add((Control)(object)wiButton_mini);
		((Control)this).Controls.Add((Control)(object)wiButton_Close);
		((Control)this).Font = new Font("微软雅黑", 10.5f);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Control)this).Name = "FormButtonSetting";
		((Form)this).StartPosition = (FormStartPosition)0;
		((Control)this).Text = "EditCidMid";
		((Control)groupBox1).ResumeLayout(false);
		((Control)groupBox2).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
