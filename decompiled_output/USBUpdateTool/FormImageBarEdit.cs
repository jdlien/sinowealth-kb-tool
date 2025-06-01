using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using USBUpdateTool.Properties;
using WindControls;

namespace USBUpdateTool;

public class FormImageBarEdit : Form
{
	private Form form;

	public SkinForm skinForm = new SkinForm(_movable: true);

	private OpenFileDialog openFileDialog = new OpenFileDialog();

	private ColorDialog colorDialog;

	private FormMover formMover = new FormMover();

	private FormMover formMover2 = new FormMover();

	private ProgressAnimation progressAnimation;

	private FormImageBarLib formImageBarLib;

	private ImageBar imageBar;

	private FontDialog fontDialog;

	private Rectangle barRect = default(Rectangle);

	private Control control;

	private IContainer components = null;

	private WindImageButton wiButton_mini;

	private WindImageButton wiButton_Close;

	private WindImageButton wiButton_OK;

	private WindImageButton wIBtn_backImage;

	private WindImageButton wIBtn_foreImage;

	private WindImageButton wIBtn_SliderImage;

	private ImageBar imageBar1;

	private Label label1;

	private WindImageButton wIButton_StartProgress;

	private WindCheckBox windCheckBox_FixSlider;

	private ImageControl imageControl_BackImage;

	private ImageControl imageControl_SliderImage;

	private ImageControl imageControl_ForeImage;

	private WindImageButton wIBtn_TextLeft;

	private GroupBox groupBox1;

	private WindImageButton wIBtn_TextDown;

	private WindImageButton wIBtn_TextUp;

	private WindImageButton wIBtn_TextRight;

	private WindImageButton wIBtn_Font;

	private Label label2;

	private WindCheckBox windCheckBox_NoShowPercent;

	private WindImageButton windImageButton_clearImage;

	private WindImageButton wiButton_textColor;

	private WindImageButton wIButton_barLib;

	public FormImageBarEdit(ImageBar _imageBar)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		InitializeComponent();
		SetStyles();
		skinForm.InitSkin((Form)(object)this, 12, 16);
		imageBar = _imageBar;
		((FileDialog)openFileDialog).Title = "Open File";
		((FileDialog)openFileDialog).Filter = "Png Files|*.png";
		((FileDialog)openFileDialog).FilterIndex = 1;
		openFileDialog.Multiselect = false;
		((FileDialog)openFileDialog).RestoreDirectory = true;
		formMover.AddControl((Control)(object)imageControl_SliderImage);
		formMover2.AddControl((Control)(object)imageControl_ForeImage);
		progressAnimation = new ProgressAnimation((Form)(object)this);
		progressAnimation.Add(imageBar1);
	}

	public void SetProgressBar(ImageBar _imageBar)
	{
		Point location = default(Point);
		CloneImageBar(imageBar1, _imageBar);
		windCheckBox_FixSlider.Checked = !_imageBar.BarSliderMovable;
		windCheckBox_NoShowPercent.Checked = !_imageBar.ShowPercent;
		((Control)imageControl_SliderImage).Width = _imageBar.BarSliderImage.Width;
		((Control)imageControl_SliderImage).Height = _imageBar.BarSliderImage.Height;
		imageControl_SliderImage.ForeImage = _imageBar.BarSliderImage;
		location.X = _imageBar.BarSliderImageRect.Location.X - _imageBar.BarBackImageRect.X + ((Control)imageControl_BackImage).Location.X;
		location.Y = _imageBar.BarSliderImageRect.Location.Y - _imageBar.BarBackImageRect.Y + ((Control)imageControl_BackImage).Location.Y;
		((Control)imageControl_SliderImage).Location = location;
		((Control)imageControl_ForeImage).Width = _imageBar.BarForeImage.Width;
		((Control)imageControl_ForeImage).Height = _imageBar.BarForeImage.Height;
		imageControl_ForeImage.ForeImage = _imageBar.BarForeImage;
		location.X = _imageBar.BarForeImageRect.Location.X - _imageBar.BarBackImageRect.X + ((Control)imageControl_BackImage).Location.X;
		location.Y = _imageBar.BarForeImageRect.Location.Y - _imageBar.BarBackImageRect.Y + ((Control)imageControl_BackImage).Location.Y;
		((Control)imageControl_ForeImage).Location = location;
		((Control)imageControl_BackImage).Width = _imageBar.BarBackImage.Width;
		((Control)imageControl_BackImage).Height = _imageBar.BarBackImage.Height;
		imageControl_BackImage.ForeImage = _imageBar.BarBackImage;
		imageBar1.Value = 50;
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
		progressAnimation.Stop();
		skinForm.AllHide();
		form.Activate();
	}

	public void ShowWindow(Form _form)
	{
		form = _form;
		skinForm.AllShow(form);
		((Form)this).Location = form.Location;
	}

	private void wIBtn_progTextColor_Click(object sender, EventArgs e)
	{
	}

	private void wIBtn_backImage_Click(object sender, EventArgs e)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Invalid comparison between Unknown and I4
		if ((int)((CommonDialog)openFileDialog).ShowDialog() == 1)
		{
			Image image = ImageHelper.GetImage(((FileDialog)openFileDialog).FileName);
			((Control)imageControl_BackImage).Width = image.Width;
			((Control)imageControl_BackImage).Height = image.Height;
			imageControl_BackImage.ForeImage = image;
			imageBar1.BarBackImage = image;
		}
	}

	private void wIBtn_foreImage_Click(object sender, EventArgs e)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Invalid comparison between Unknown and I4
		if ((int)((CommonDialog)openFileDialog).ShowDialog() == 1)
		{
			Image image = ImageHelper.GetImage(((FileDialog)openFileDialog).FileName);
			((Control)imageControl_ForeImage).Width = image.Width;
			((Control)imageControl_ForeImage).Height = image.Height;
			imageControl_ForeImage.ForeImage = image;
			imageBar1.BarForeImage = image;
		}
	}

	private void wIBtn_SliderImage_Click(object sender, EventArgs e)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Invalid comparison between Unknown and I4
		if ((int)((CommonDialog)openFileDialog).ShowDialog() == 1)
		{
			Image image = ImageHelper.GetImage(((FileDialog)openFileDialog).FileName);
			((Control)imageControl_SliderImage).Width = image.Width;
			((Control)imageControl_SliderImage).Height = image.Height;
			imageControl_SliderImage.ForeImage = image;
			imageBar1.BarSliderImage = image;
		}
	}

	public void UpdateSize()
	{
		int x = ((Control)imageControl_BackImage).Location.X;
		if (((Control)imageControl_ForeImage).Location.X < x)
		{
			x = ((Control)imageControl_ForeImage).Location.X;
		}
		if (((Control)imageControl_SliderImage).Location.X < x)
		{
			x = ((Control)imageControl_SliderImage).Location.X;
		}
		int y = ((Control)imageControl_BackImage).Location.Y;
		if (((Control)imageControl_ForeImage).Location.Y < y)
		{
			y = ((Control)imageControl_ForeImage).Location.Y;
		}
		if (((Control)imageControl_SliderImage).Location.Y < y)
		{
			y = ((Control)imageControl_SliderImage).Location.Y;
		}
		int right = ((Control)imageControl_BackImage).Right;
		if (((Control)imageControl_ForeImage).Right > right)
		{
			right = ((Control)imageControl_ForeImage).Right;
		}
		if (((Control)imageControl_SliderImage).Right > right)
		{
			right = ((Control)imageControl_SliderImage).Right;
		}
		int bottom = ((Control)imageControl_BackImage).Bottom;
		if (((Control)imageControl_ForeImage).Bottom > bottom)
		{
			bottom = ((Control)imageControl_ForeImage).Bottom;
		}
		if (((Control)imageControl_SliderImage).Bottom > bottom)
		{
			bottom = ((Control)imageControl_SliderImage).Bottom;
		}
		barRect = new Rectangle(x, y, right - x, bottom - y);
		int x2 = ((Control)imageControl_SliderImage).Location.X - barRect.X;
		int y2 = ((Control)imageControl_SliderImage).Location.Y - barRect.Y;
		Rectangle rectangle = new Rectangle(x2, y2, ((Control)imageControl_SliderImage).Width, ((Control)imageControl_SliderImage).Height);
		imageBar1.BarSliderImageRect = ResizeControl.ToReverseScaleRect(rectangle);
		x2 = ((Control)imageControl_ForeImage).Location.X - barRect.X;
		y2 = ((Control)imageControl_ForeImage).Location.Y - barRect.Y;
		rectangle = new Rectangle(x2, y2, ((Control)imageControl_ForeImage).Width, ((Control)imageControl_ForeImage).Height);
		imageBar1.BarForeImageRect = ResizeControl.ToReverseScaleRect(rectangle);
		x2 = ((Control)imageControl_BackImage).Location.X - barRect.X;
		y2 = ((Control)imageControl_BackImage).Location.Y - barRect.Y;
		rectangle = new Rectangle(x2, y2, ((Control)imageControl_BackImage).Width, ((Control)imageControl_BackImage).Height);
		imageBar1.BarBackImageRect = ResizeControl.ToReverseScaleRect(rectangle);
		((Control)imageBar1).Width = ResizeControl.GetScaleValue(right - x);
		((Control)imageBar1).Height = ResizeControl.GetScaleValue(bottom - y);
		((Control)imageBar1).Invalidate();
	}

	private void pictureBox_ForeImage_MouseUp(object sender, MouseEventArgs e)
	{
		int x = ((Control)imageControl_ForeImage).Location.X - ((Control)imageControl_BackImage).Location.X;
		int y = ((Control)imageControl_ForeImage).Location.Y - ((Control)imageControl_BackImage).Location.Y;
		Rectangle rectangle = new Rectangle(x, y, ((Control)imageControl_ForeImage).Width, ((Control)imageControl_ForeImage).Height);
		imageBar1.BarForeImageRect = ResizeControl.ToReverseScaleRect(rectangle);
		imageControl_ForeImage.isSelected = true;
		imageControl_SliderImage.isSelected = false;
		UpdateSize();
		control = (Control)(object)imageControl_ForeImage;
	}

	private void imageControl_SliderImage_MouseUp(object sender, MouseEventArgs e)
	{
		int x = ((Control)imageControl_SliderImage).Location.X - ((Control)imageControl_BackImage).Location.X;
		int y = ((Control)imageControl_SliderImage).Location.Y - ((Control)imageControl_BackImage).Location.Y;
		Rectangle rectangle = new Rectangle(x, y, ((Control)imageControl_SliderImage).Width, ((Control)imageControl_SliderImage).Height);
		imageBar1.BarSliderImageRect = ResizeControl.ToReverseScaleRect(rectangle);
		imageControl_ForeImage.isSelected = false;
		imageControl_SliderImage.isSelected = true;
		UpdateSize();
		control = (Control)(object)imageControl_SliderImage;
	}

	private void wIButton_StartProgress_Click(object sender, EventArgs e)
	{
		if (progressAnimation.isStart())
		{
			progressAnimation.Stop();
			((Control)wIButton_StartProgress).Text = "启动进度条";
			imageBar1.Value = 50;
		}
		else
		{
			progressAnimation.Start();
			((Control)wIButton_StartProgress).Text = "停止进度条";
		}
	}

	private void FormImageBarEdit_FormClosing(object sender, FormClosingEventArgs e)
	{
		progressAnimation.Stop();
	}

	private void CloneImageBar(ImageBar dstBar, ImageBar srcBar)
	{
		dstBar.BarSliderMovable = srcBar.BarSliderMovable;
		dstBar.BarSliderImage = srcBar.BarSliderImage;
		dstBar.BarForeImage = srcBar.BarForeImage;
		dstBar.BarBackImage = srcBar.BarBackImage;
		dstBar.BarSliderImageRect = srcBar.BarSliderImageRect;
		dstBar.BarForeImageRect = srcBar.BarForeImageRect;
		dstBar.BarBackImageRect = srcBar.BarBackImageRect;
		((Control)dstBar).Width = ((Control)srcBar).Width;
		((Control)dstBar).Height = ((Control)srcBar).Height;
		dstBar.TextLocation = srcBar.TextLocation;
		((Control)dstBar).Font = ((Control)srcBar).Font;
		dstBar.TextColor = srcBar.TextColor;
		dstBar.ShowPercent = srcBar.ShowPercent;
	}

	private void wiButton_OK_Click(object sender, EventArgs e)
	{
		imageBar1.BarSliderMovable = !windCheckBox_FixSlider.Checked;
		imageBar1.ShowPercent = !windCheckBox_NoShowPercent.Checked;
		CloneImageBar(imageBar, imageBar1);
		wiButton_Close_Click(null, null);
	}

	private void windCheckBox_FixSlider_CheckedChanged(object sender, EventArgs e)
	{
		imageBar1.BarSliderMovable = !windCheckBox_FixSlider.Checked;
		imageBar1.ShowPercent = !windCheckBox_NoShowPercent.Checked;
		((Control)imageBar1).Invalidate();
	}

	private void UpdataProgressBar()
	{
		if ((object)control == imageControl_SliderImage)
		{
			imageControl_SliderImage_MouseUp(null, null);
		}
		else if ((object)control == imageControl_ForeImage)
		{
			pictureBox_ForeImage_MouseUp(null, null);
		}
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

	private void FormImageBarEdit_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Invalid comparison between Unknown and I4
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Invalid comparison between Unknown and I4
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Invalid comparison between Unknown and I4
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Invalid comparison between Unknown and I4
		if (control != null)
		{
			if ((int)e.KeyCode == 38)
			{
				control.Location = new Point(control.Location.X, control.Location.Y - 1);
				UpdataProgressBar();
			}
			else if ((int)e.KeyCode == 40)
			{
				control.Location = new Point(control.Location.X, control.Location.Y + 1);
				UpdataProgressBar();
			}
			else if ((int)e.KeyCode == 37)
			{
				control.Location = new Point(control.Location.X - 1, control.Location.Y);
				UpdataProgressBar();
			}
			else if ((int)e.KeyCode == 39)
			{
				control.Location = new Point(control.Location.X + 1, control.Location.Y);
				UpdataProgressBar();
			}
		}
	}

	private void wIBtn_TextUp_Click(object sender, EventArgs e)
	{
		imageBar1.TextLocation = new Point(imageBar1.TextLocation.X, imageBar1.TextLocation.Y - 1);
	}

	private void wIBtn_TextDown_Click(object sender, EventArgs e)
	{
		imageBar1.TextLocation = new Point(imageBar1.TextLocation.X, imageBar1.TextLocation.Y + 1);
	}

	private void wIBtn_TextLeft_Click(object sender, EventArgs e)
	{
		imageBar1.TextLocation = new Point(imageBar1.TextLocation.X - 1, imageBar1.TextLocation.Y);
	}

	private void wIBtn_TextRight_Click(object sender, EventArgs e)
	{
		imageBar1.TextLocation = new Point(imageBar1.TextLocation.X + 1, imageBar1.TextLocation.Y);
	}

	private void wIBtn_Font_Click(object sender, EventArgs e)
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
			((Control)imageBar1).Font = fontDialog.Font;
		}
	}

	private void windCheckBox_NoShowPercent_CheckedChanged(object sender, EventArgs e)
	{
		windCheckBox_FixSlider_CheckedChanged(null, null);
	}

	private void windImageButton_clearImage_Click(object sender, EventArgs e)
	{
		imageControl_SliderImage.ForeImage = null;
		((Control)imageControl_SliderImage).Location = new Point(203, 105);
		((Control)imageControl_SliderImage).Width = 550;
		((Control)imageControl_SliderImage).Height = 45;
		imageControl_ForeImage.ForeImage = null;
		((Control)imageControl_ForeImage).Location = new Point(203, 156);
		((Control)imageControl_ForeImage).Width = 550;
		((Control)imageControl_ForeImage).Height = 45;
		imageControl_BackImage.ForeImage = null;
		((Control)imageControl_BackImage).Location = new Point(203, 207);
		((Control)imageControl_BackImage).Width = 550;
		((Control)imageControl_BackImage).Height = 45;
		imageBar1.BarSliderImage = null;
		imageBar1.BarForeImage = null;
		imageBar1.BarBackImage = null;
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
		if ((int)((CommonDialog)colorDialog).ShowDialog() == 1)
		{
			imageBar1.TextColor = colorDialog.Color;
		}
	}

	private void wIButton_barLib_Click(object sender, EventArgs e)
	{
		if (formImageBarLib == null)
		{
			formImageBarLib = new FormImageBarLib(imageBar1);
		}
		formImageBarLib.ShowWindow((Form)(object)this);
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
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Expected O, but got Unknown
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Expected O, but got Unknown
		//IL_0318: Unknown result type (might be due to invalid IL or missing references)
		//IL_0322: Expected O, but got Unknown
		//IL_04ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f4: Expected O, but got Unknown
		//IL_06bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c6: Expected O, but got Unknown
		//IL_088e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0898: Expected O, but got Unknown
		//IL_0a4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a54: Expected O, but got Unknown
		//IL_0b13: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b1d: Expected O, but got Unknown
		//IL_0cf8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d02: Expected O, but got Unknown
		//IL_0ed0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eda: Expected O, but got Unknown
		//IL_109a: Unknown result type (might be due to invalid IL or missing references)
		//IL_10a4: Expected O, but got Unknown
		//IL_10b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_10c0: Expected O, but got Unknown
		//IL_10d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_10e2: Expected O, but got Unknown
		//IL_1175: Unknown result type (might be due to invalid IL or missing references)
		//IL_117f: Expected O, but got Unknown
		//IL_1201: Unknown result type (might be due to invalid IL or missing references)
		//IL_120b: Expected O, but got Unknown
		//IL_126e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1278: Expected O, but got Unknown
		//IL_1435: Unknown result type (might be due to invalid IL or missing references)
		//IL_143f: Expected O, but got Unknown
		//IL_148d: Unknown result type (might be due to invalid IL or missing references)
		//IL_14df: Unknown result type (might be due to invalid IL or missing references)
		//IL_14e9: Expected O, but got Unknown
		//IL_1523: Unknown result type (might be due to invalid IL or missing references)
		//IL_152d: Expected O, but got Unknown
		//IL_157e: Unknown result type (might be due to invalid IL or missing references)
		//IL_15d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_15da: Expected O, but got Unknown
		//IL_160d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1617: Expected O, but got Unknown
		//IL_1668: Unknown result type (might be due to invalid IL or missing references)
		//IL_16e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_16f3: Expected O, but got Unknown
		//IL_1705: Unknown result type (might be due to invalid IL or missing references)
		//IL_170f: Expected O, but got Unknown
		//IL_1727: Unknown result type (might be due to invalid IL or missing references)
		//IL_1731: Expected O, but got Unknown
		//IL_17c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_17ce: Expected O, but got Unknown
		//IL_1850: Unknown result type (might be due to invalid IL or missing references)
		//IL_185a: Expected O, but got Unknown
		//IL_18bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_18c7: Expected O, but got Unknown
		//IL_1c07: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c11: Expected O, but got Unknown
		//IL_1ddc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1de6: Expected O, but got Unknown
		//IL_1fb1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fbb: Expected O, but got Unknown
		//IL_2186: Unknown result type (might be due to invalid IL or missing references)
		//IL_2190: Expected O, but got Unknown
		//IL_28e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_28eb: Expected O, but got Unknown
		//IL_2900: Unknown result type (might be due to invalid IL or missing references)
		//IL_290a: Expected O, but got Unknown
		//IL_293b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2945: Expected O, but got Unknown
		//IL_294e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2958: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FormImageBarEdit));
		label1 = new Label();
		groupBox1 = new GroupBox();
		wIBtn_TextDown = new WindImageButton();
		wIBtn_TextUp = new WindImageButton();
		wIBtn_TextRight = new WindImageButton();
		wIBtn_TextLeft = new WindImageButton();
		label2 = new Label();
		wIButton_barLib = new WindImageButton();
		wiButton_textColor = new WindImageButton();
		windImageButton_clearImage = new WindImageButton();
		windCheckBox_NoShowPercent = new WindCheckBox();
		wIBtn_Font = new WindImageButton();
		imageControl_SliderImage = new ImageControl();
		imageControl_ForeImage = new ImageControl();
		imageControl_BackImage = new ImageControl();
		windCheckBox_FixSlider = new WindCheckBox();
		wIButton_StartProgress = new WindImageButton();
		imageBar1 = new ImageBar();
		wIBtn_SliderImage = new WindImageButton();
		wIBtn_foreImage = new WindImageButton();
		wIBtn_backImage = new WindImageButton();
		wiButton_OK = new WindImageButton();
		wiButton_mini = new WindImageButton();
		wiButton_Close = new WindImageButton();
		((Control)groupBox1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)label1).AutoSize = true;
		((Control)label1).BackColor = Color.Transparent;
		((Control)label1).Font = new Font("微软雅黑", 15f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)label1).Location = new Point(171, 357);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(57, 27);
		((Control)label1).TabIndex = 170;
		((Control)label1).Text = "预览:";
		((Control)groupBox1).BackColor = Color.Transparent;
		((Control)groupBox1).Controls.Add((Control)(object)wIBtn_TextDown);
		((Control)groupBox1).Controls.Add((Control)(object)wIBtn_TextUp);
		((Control)groupBox1).Controls.Add((Control)(object)wIBtn_TextRight);
		((Control)groupBox1).Controls.Add((Control)(object)wIBtn_TextLeft);
		((Control)groupBox1).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)groupBox1).Location = new Point(12, 358);
		((Control)groupBox1).Name = "groupBox1";
		((Control)groupBox1).Size = new Size(131, 100);
		((Control)groupBox1).TabIndex = 177;
		groupBox1.TabStop = false;
		((Control)groupBox1).Text = "百分比显示偏移";
		((Control)wIBtn_TextDown).BackColor = Color.Transparent;
		wIBtn_TextDown.DisableBackColor = Color.DarkGray;
		wIBtn_TextDown.DisableForeColor = Color.DimGray;
		((Control)wIBtn_TextDown).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wIBtn_TextDown.FrameMode = GraphicsHelper.RoundStyle.All;
		wIBtn_TextDown.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wIBtn_TextDown.IconName = "";
		wIBtn_TextDown.IconOffset = new Point(0, 0);
		wIBtn_TextDown.IconSize = 32;
		((Control)wIBtn_TextDown).Location = new Point(48, 67);
		wIBtn_TextDown.MouseDownBackColor = Color.DimGray;
		wIBtn_TextDown.MouseDownForeColor = Color.Black;
		wIBtn_TextDown.MouseEnterBackColor = Color.Gray;
		wIBtn_TextDown.MouseEnterForeColor = Color.Black;
		wIBtn_TextDown.MouseUpBackColor = Color.DarkGray;
		wIBtn_TextDown.MouseUpForeColor = Color.Black;
		((Control)wIBtn_TextDown).Name = "wIBtn_TextDown";
		wIBtn_TextDown.Radius = 8;
		((Control)wIBtn_TextDown).Size = new Size(36, 24);
		((Control)wIBtn_TextDown).TabIndex = 179;
		((Control)wIBtn_TextDown).Text = "↓";
		wIBtn_TextDown.TextAlignment = StringHelper.TextAlignment.Center;
		wIBtn_TextDown.TextDynOffset = new Point(0, 0);
		wIBtn_TextDown.TextFixLocation = new Point(0, 0);
		wIBtn_TextDown.TextFixLocationEnable = false;
		((Control)wIBtn_TextDown).Click += wIBtn_TextDown_Click;
		((Control)wIBtn_TextUp).BackColor = Color.Transparent;
		wIBtn_TextUp.DisableBackColor = Color.DarkGray;
		wIBtn_TextUp.DisableForeColor = Color.DimGray;
		((Control)wIBtn_TextUp).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wIBtn_TextUp.FrameMode = GraphicsHelper.RoundStyle.All;
		wIBtn_TextUp.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wIBtn_TextUp.IconName = "";
		wIBtn_TextUp.IconOffset = new Point(0, 0);
		wIBtn_TextUp.IconSize = 32;
		((Control)wIBtn_TextUp).Location = new Point(48, 27);
		wIBtn_TextUp.MouseDownBackColor = Color.DimGray;
		wIBtn_TextUp.MouseDownForeColor = Color.Black;
		wIBtn_TextUp.MouseEnterBackColor = Color.Gray;
		wIBtn_TextUp.MouseEnterForeColor = Color.Black;
		wIBtn_TextUp.MouseUpBackColor = Color.DarkGray;
		wIBtn_TextUp.MouseUpForeColor = Color.Black;
		((Control)wIBtn_TextUp).Name = "wIBtn_TextUp";
		wIBtn_TextUp.Radius = 8;
		((Control)wIBtn_TextUp).Size = new Size(36, 24);
		((Control)wIBtn_TextUp).TabIndex = 178;
		((Control)wIBtn_TextUp).Text = "↑";
		wIBtn_TextUp.TextAlignment = StringHelper.TextAlignment.Center;
		wIBtn_TextUp.TextDynOffset = new Point(0, 0);
		wIBtn_TextUp.TextFixLocation = new Point(0, 0);
		wIBtn_TextUp.TextFixLocationEnable = false;
		((Control)wIBtn_TextUp).Click += wIBtn_TextUp_Click;
		((Control)wIBtn_TextRight).BackColor = Color.Transparent;
		wIBtn_TextRight.DisableBackColor = Color.DarkGray;
		wIBtn_TextRight.DisableForeColor = Color.DimGray;
		((Control)wIBtn_TextRight).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wIBtn_TextRight.FrameMode = GraphicsHelper.RoundStyle.All;
		wIBtn_TextRight.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wIBtn_TextRight.IconName = "";
		wIBtn_TextRight.IconOffset = new Point(0, 0);
		wIBtn_TextRight.IconSize = 32;
		((Control)wIBtn_TextRight).Location = new Point(89, 47);
		wIBtn_TextRight.MouseDownBackColor = Color.DimGray;
		wIBtn_TextRight.MouseDownForeColor = Color.Black;
		wIBtn_TextRight.MouseEnterBackColor = Color.Gray;
		wIBtn_TextRight.MouseEnterForeColor = Color.Black;
		wIBtn_TextRight.MouseUpBackColor = Color.DarkGray;
		wIBtn_TextRight.MouseUpForeColor = Color.Black;
		((Control)wIBtn_TextRight).Name = "wIBtn_TextRight";
		wIBtn_TextRight.Radius = 8;
		((Control)wIBtn_TextRight).Size = new Size(36, 24);
		((Control)wIBtn_TextRight).TabIndex = 177;
		((Control)wIBtn_TextRight).Text = "→";
		wIBtn_TextRight.TextAlignment = StringHelper.TextAlignment.Center;
		wIBtn_TextRight.TextDynOffset = new Point(0, 0);
		wIBtn_TextRight.TextFixLocation = new Point(0, 0);
		wIBtn_TextRight.TextFixLocationEnable = false;
		((Control)wIBtn_TextRight).Click += wIBtn_TextRight_Click;
		((Control)wIBtn_TextLeft).BackColor = Color.Transparent;
		wIBtn_TextLeft.DisableBackColor = Color.DarkGray;
		wIBtn_TextLeft.DisableForeColor = Color.DimGray;
		((Control)wIBtn_TextLeft).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wIBtn_TextLeft.FrameMode = GraphicsHelper.RoundStyle.All;
		wIBtn_TextLeft.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wIBtn_TextLeft.IconName = "";
		wIBtn_TextLeft.IconOffset = new Point(0, 0);
		wIBtn_TextLeft.IconSize = 32;
		((Control)wIBtn_TextLeft).Location = new Point(6, 45);
		wIBtn_TextLeft.MouseDownBackColor = Color.DimGray;
		wIBtn_TextLeft.MouseDownForeColor = Color.Black;
		wIBtn_TextLeft.MouseEnterBackColor = Color.Gray;
		wIBtn_TextLeft.MouseEnterForeColor = Color.Black;
		wIBtn_TextLeft.MouseUpBackColor = Color.DarkGray;
		wIBtn_TextLeft.MouseUpForeColor = Color.Black;
		((Control)wIBtn_TextLeft).Name = "wIBtn_TextLeft";
		wIBtn_TextLeft.Radius = 8;
		((Control)wIBtn_TextLeft).Size = new Size(36, 24);
		((Control)wIBtn_TextLeft).TabIndex = 176;
		((Control)wIBtn_TextLeft).Text = "←";
		wIBtn_TextLeft.TextAlignment = StringHelper.TextAlignment.Center;
		wIBtn_TextLeft.TextDynOffset = new Point(0, 0);
		wIBtn_TextLeft.TextFixLocation = new Point(0, 0);
		wIBtn_TextLeft.TextFixLocationEnable = false;
		((Control)wIBtn_TextLeft).Click += wIBtn_TextLeft_Click;
		((Control)label2).AutoSize = true;
		((Control)label2).BackColor = Color.Transparent;
		((Control)label2).Font = new Font("微软雅黑", 15f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)label2).ForeColor = Color.Red;
		((Control)label2).Location = new Point(174, 62);
		((Control)label2).Name = "label2";
		((Control)label2).Size = new Size(626, 27);
		((Control)label2).TabIndex = 179;
		((Control)label2).Text = "提示：将滑条图片和前景图片拖至背景图片相应位置即可(↑↓←→微调)";
		((Control)wIButton_barLib).BackColor = Color.Transparent;
		wIButton_barLib.DisableBackColor = Color.DarkGray;
		wIButton_barLib.DisableForeColor = Color.DimGray;
		((Control)wIButton_barLib).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wIButton_barLib.FrameMode = GraphicsHelper.RoundStyle.All;
		wIButton_barLib.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wIButton_barLib.IconName = "";
		wIButton_barLib.IconOffset = new Point(0, 0);
		wIButton_barLib.IconSize = 32;
		((Control)wIButton_barLib).Location = new Point(12, 296);
		wIButton_barLib.MouseDownBackColor = Color.DimGray;
		wIButton_barLib.MouseDownForeColor = Color.Black;
		wIButton_barLib.MouseEnterBackColor = Color.Gray;
		wIButton_barLib.MouseEnterForeColor = Color.Black;
		wIButton_barLib.MouseUpBackColor = Color.DarkGray;
		wIButton_barLib.MouseUpForeColor = Color.Black;
		((Control)wIButton_barLib).Name = "wIButton_barLib";
		wIButton_barLib.Radius = 8;
		((Control)wIButton_barLib).Size = new Size(136, 24);
		((Control)wIButton_barLib).TabIndex = 183;
		((Control)wIButton_barLib).Text = "进度条库";
		wIButton_barLib.TextAlignment = StringHelper.TextAlignment.Center;
		wIButton_barLib.TextDynOffset = new Point(0, 0);
		wIButton_barLib.TextFixLocation = new Point(0, 0);
		wIButton_barLib.TextFixLocationEnable = false;
		((Control)wIButton_barLib).Visible = false;
		((Control)wIButton_barLib).Click += wIButton_barLib_Click;
		((Control)wiButton_textColor).BackColor = Color.Transparent;
		wiButton_textColor.DisableBackColor = Color.DarkGray;
		wiButton_textColor.DisableForeColor = Color.DimGray;
		((Control)wiButton_textColor).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wiButton_textColor.FrameMode = GraphicsHelper.RoundStyle.All;
		wiButton_textColor.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wiButton_textColor.IconName = "";
		wiButton_textColor.IconOffset = new Point(0, 0);
		wiButton_textColor.IconSize = 32;
		((Control)wiButton_textColor).Location = new Point(12, 155);
		wiButton_textColor.MouseDownBackColor = Color.DimGray;
		wiButton_textColor.MouseDownForeColor = Color.Black;
		wiButton_textColor.MouseEnterBackColor = Color.Gray;
		wiButton_textColor.MouseEnterForeColor = Color.Black;
		wiButton_textColor.MouseUpBackColor = Color.DarkGray;
		wiButton_textColor.MouseUpForeColor = Color.Black;
		((Control)wiButton_textColor).Name = "wiButton_textColor";
		wiButton_textColor.Radius = 8;
		((Control)wiButton_textColor).Size = new Size(136, 24);
		((Control)wiButton_textColor).TabIndex = 182;
		((Control)wiButton_textColor).Text = "文本颜色";
		wiButton_textColor.TextAlignment = StringHelper.TextAlignment.Center;
		wiButton_textColor.TextDynOffset = new Point(0, 0);
		wiButton_textColor.TextFixLocation = new Point(0, 0);
		wiButton_textColor.TextFixLocationEnable = false;
		((Control)wiButton_textColor).Click += wiButton_textColor_Click;
		((Control)windImageButton_clearImage).BackColor = Color.Transparent;
		windImageButton_clearImage.DisableBackColor = Color.DarkGray;
		windImageButton_clearImage.DisableForeColor = Color.DimGray;
		((Control)windImageButton_clearImage).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		windImageButton_clearImage.FrameMode = GraphicsHelper.RoundStyle.All;
		windImageButton_clearImage.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		windImageButton_clearImage.IconName = "";
		windImageButton_clearImage.IconOffset = new Point(0, 0);
		windImageButton_clearImage.IconSize = 32;
		((Control)windImageButton_clearImage).Location = new Point(12, 215);
		windImageButton_clearImage.MouseDownBackColor = Color.DimGray;
		windImageButton_clearImage.MouseDownForeColor = Color.Black;
		windImageButton_clearImage.MouseEnterBackColor = Color.Gray;
		windImageButton_clearImage.MouseEnterForeColor = Color.Black;
		windImageButton_clearImage.MouseUpBackColor = Color.DarkGray;
		windImageButton_clearImage.MouseUpForeColor = Color.Black;
		((Control)windImageButton_clearImage).Name = "windImageButton_clearImage";
		windImageButton_clearImage.Radius = 8;
		((Control)windImageButton_clearImage).Size = new Size(136, 24);
		((Control)windImageButton_clearImage).TabIndex = 181;
		((Control)windImageButton_clearImage).Text = "清除所有图片";
		windImageButton_clearImage.TextAlignment = StringHelper.TextAlignment.Center;
		windImageButton_clearImage.TextDynOffset = new Point(0, 0);
		windImageButton_clearImage.TextFixLocation = new Point(0, 0);
		windImageButton_clearImage.TextFixLocationEnable = false;
		((Control)windImageButton_clearImage).Click += windImageButton_clearImage_Click;
		((Control)windCheckBox_NoShowPercent).BackColor = Color.Transparent;
		((Control)windCheckBox_NoShowPercent).BackgroundImageLayout = (ImageLayout)0;
		windCheckBox_NoShowPercent.Checked = false;
		windCheckBox_NoShowPercent.DisableSelectedImage = (Image)componentResourceManager.GetObject("windCheckBox_NoShowPercent.DisableSelectedImage");
		windCheckBox_NoShowPercent.DisableUnSelectedImage = (Image)componentResourceManager.GetObject("windCheckBox_NoShowPercent.DisableUnSelectedImage");
		((Control)windCheckBox_NoShowPercent).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		windCheckBox_NoShowPercent.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		windCheckBox_NoShowPercent.IconOffset = new Point(0, 0);
		windCheckBox_NoShowPercent.IconSize = 36;
		((Control)windCheckBox_NoShowPercent).Location = new Point(13, 504);
		((Control)windCheckBox_NoShowPercent).Name = "windCheckBox_NoShowPercent";
		windCheckBox_NoShowPercent.SelectedIconColor = Color.Red;
		windCheckBox_NoShowPercent.SelectedIconName = "A_fa_check_square_o";
		windCheckBox_NoShowPercent.SelectedImage = (Image)componentResourceManager.GetObject("windCheckBox_NoShowPercent.SelectedImage");
		((Control)windCheckBox_NoShowPercent).Size = new Size(135, 34);
		((Control)windCheckBox_NoShowPercent).TabIndex = 180;
		((Control)windCheckBox_NoShowPercent).Text = "不显示百分比";
		windCheckBox_NoShowPercent.TextOffset = new Point(0, -2);
		windCheckBox_NoShowPercent.UnSelectedIconColor = Color.Gray;
		windCheckBox_NoShowPercent.UnSelectedIconName = "A_fa_square_o";
		windCheckBox_NoShowPercent.UnSelectedImage = (Image)componentResourceManager.GetObject("windCheckBox_NoShowPercent.UnSelectedImage");
		windCheckBox_NoShowPercent.CheckedChanged += windCheckBox_NoShowPercent_CheckedChanged;
		((Control)wIBtn_Font).BackColor = Color.Transparent;
		wIBtn_Font.DisableBackColor = Color.DarkGray;
		wIBtn_Font.DisableForeColor = Color.DimGray;
		((Control)wIBtn_Font).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wIBtn_Font.FrameMode = GraphicsHelper.RoundStyle.All;
		wIBtn_Font.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wIBtn_Font.IconName = "";
		wIBtn_Font.IconOffset = new Point(0, 0);
		wIBtn_Font.IconSize = 32;
		((Control)wIBtn_Font).Location = new Point(12, 185);
		wIBtn_Font.MouseDownBackColor = Color.DimGray;
		wIBtn_Font.MouseDownForeColor = Color.Black;
		wIBtn_Font.MouseEnterBackColor = Color.Gray;
		wIBtn_Font.MouseEnterForeColor = Color.Black;
		wIBtn_Font.MouseUpBackColor = Color.DarkGray;
		wIBtn_Font.MouseUpForeColor = Color.Black;
		((Control)wIBtn_Font).Name = "wIBtn_Font";
		wIBtn_Font.Radius = 8;
		((Control)wIBtn_Font).Size = new Size(136, 24);
		((Control)wIBtn_Font).TabIndex = 178;
		((Control)wIBtn_Font).Text = "字体";
		wIBtn_Font.TextAlignment = StringHelper.TextAlignment.Center;
		wIBtn_Font.TextDynOffset = new Point(0, 0);
		wIBtn_Font.TextFixLocation = new Point(0, 0);
		wIBtn_Font.TextFixLocationEnable = false;
		((Control)wIBtn_Font).Click += wIBtn_Font_Click;
		((Control)imageControl_SliderImage).BackColor = Color.Transparent;
		imageControl_SliderImage.BackText = "滑条图片";
		((Control)imageControl_SliderImage).Font = new Font("微软雅黑", 18f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)imageControl_SliderImage).ForeColor = SystemColors.ControlDark;
		imageControl_SliderImage.ForeImage = null;
		imageControl_SliderImage.isSelected = false;
		((Control)imageControl_SliderImage).Location = new Point(179, 105);
		((Control)imageControl_SliderImage).Margin = new Padding(7, 8, 7, 8);
		((Control)imageControl_SliderImage).Name = "imageControl_SliderImage";
		((Control)imageControl_SliderImage).Size = new Size(550, 45);
		((Control)imageControl_SliderImage).TabIndex = 174;
		((Control)imageControl_SliderImage).MouseUp += new MouseEventHandler(imageControl_SliderImage_MouseUp);
		((Control)imageControl_ForeImage).BackColor = Color.Transparent;
		imageControl_ForeImage.BackText = "前景图片";
		((Control)imageControl_ForeImage).Font = new Font("微软雅黑", 18f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)imageControl_ForeImage).ForeColor = SystemColors.ControlDark;
		imageControl_ForeImage.ForeImage = null;
		imageControl_ForeImage.isSelected = false;
		((Control)imageControl_ForeImage).Location = new Point(179, 156);
		((Control)imageControl_ForeImage).Margin = new Padding(7, 8, 7, 8);
		((Control)imageControl_ForeImage).Name = "imageControl_ForeImage";
		((Control)imageControl_ForeImage).Size = new Size(550, 45);
		((Control)imageControl_ForeImage).TabIndex = 175;
		((Control)imageControl_ForeImage).MouseUp += new MouseEventHandler(pictureBox_ForeImage_MouseUp);
		((Control)imageControl_BackImage).BackColor = Color.Transparent;
		imageControl_BackImage.BackText = "背景图片";
		((Control)imageControl_BackImage).Font = new Font("微软雅黑", 18f);
		((Control)imageControl_BackImage).ForeColor = SystemColors.ControlDark;
		imageControl_BackImage.ForeImage = null;
		imageControl_BackImage.isSelected = false;
		((Control)imageControl_BackImage).Location = new Point(179, 207);
		((Control)imageControl_BackImage).Margin = new Padding(7, 8, 7, 8);
		((Control)imageControl_BackImage).Name = "imageControl_BackImage";
		((Control)imageControl_BackImage).Size = new Size(550, 45);
		((Control)imageControl_BackImage).TabIndex = 173;
		((Control)windCheckBox_FixSlider).BackColor = Color.Transparent;
		((Control)windCheckBox_FixSlider).BackgroundImageLayout = (ImageLayout)0;
		windCheckBox_FixSlider.Checked = false;
		windCheckBox_FixSlider.DisableSelectedImage = (Image)componentResourceManager.GetObject("windCheckBox_FixSlider.DisableSelectedImage");
		windCheckBox_FixSlider.DisableUnSelectedImage = (Image)componentResourceManager.GetObject("windCheckBox_FixSlider.DisableUnSelectedImage");
		((Control)windCheckBox_FixSlider).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		windCheckBox_FixSlider.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		windCheckBox_FixSlider.IconOffset = new Point(0, 0);
		windCheckBox_FixSlider.IconSize = 36;
		((Control)windCheckBox_FixSlider).Location = new Point(12, 464);
		((Control)windCheckBox_FixSlider).Name = "windCheckBox_FixSlider";
		windCheckBox_FixSlider.SelectedIconColor = Color.Red;
		windCheckBox_FixSlider.SelectedIconName = "A_fa_check_square_o";
		windCheckBox_FixSlider.SelectedImage = (Image)componentResourceManager.GetObject("windCheckBox_FixSlider.SelectedImage");
		((Control)windCheckBox_FixSlider).Size = new Size(135, 34);
		((Control)windCheckBox_FixSlider).TabIndex = 172;
		((Control)windCheckBox_FixSlider).Text = "固定滑条";
		windCheckBox_FixSlider.TextOffset = new Point(0, -2);
		windCheckBox_FixSlider.UnSelectedIconColor = Color.Gray;
		windCheckBox_FixSlider.UnSelectedIconName = "A_fa_square_o";
		windCheckBox_FixSlider.UnSelectedImage = (Image)componentResourceManager.GetObject("windCheckBox_FixSlider.UnSelectedImage");
		windCheckBox_FixSlider.CheckedChanged += windCheckBox_FixSlider_CheckedChanged;
		((Control)wIButton_StartProgress).BackColor = Color.Transparent;
		wIButton_StartProgress.DisableBackColor = Color.DarkGray;
		wIButton_StartProgress.DisableForeColor = Color.DimGray;
		((Control)wIButton_StartProgress).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wIButton_StartProgress.FrameMode = GraphicsHelper.RoundStyle.All;
		wIButton_StartProgress.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wIButton_StartProgress.IconName = "";
		wIButton_StartProgress.IconOffset = new Point(0, 0);
		wIButton_StartProgress.IconSize = 32;
		((Control)wIButton_StartProgress).Location = new Point(234, 358);
		wIButton_StartProgress.MouseDownBackColor = Color.DimGray;
		wIButton_StartProgress.MouseDownForeColor = Color.Black;
		wIButton_StartProgress.MouseEnterBackColor = Color.Gray;
		wIButton_StartProgress.MouseEnterForeColor = Color.Black;
		wIButton_StartProgress.MouseUpBackColor = Color.DarkGray;
		wIButton_StartProgress.MouseUpForeColor = Color.Black;
		((Control)wIButton_StartProgress).Name = "wIButton_StartProgress";
		wIButton_StartProgress.Radius = 8;
		((Control)wIButton_StartProgress).Size = new Size(136, 24);
		((Control)wIButton_StartProgress).TabIndex = 171;
		((Control)wIButton_StartProgress).Text = "启动进度条";
		wIButton_StartProgress.TextAlignment = StringHelper.TextAlignment.Center;
		wIButton_StartProgress.TextDynOffset = new Point(0, 0);
		wIButton_StartProgress.TextFixLocation = new Point(0, 0);
		wIButton_StartProgress.TextFixLocationEnable = false;
		((Control)wIButton_StartProgress).Click += wIButton_StartProgress_Click;
		((Control)imageBar1).BackColor = Color.Transparent;
		imageBar1.BarBackColor = Color.DarkGray;
		imageBar1.BarBackImage = null;
		imageBar1.BarBackImageRect = new Rectangle(0, 0, 0, 0);
		imageBar1.BarForeColor = Color.RoyalBlue;
		imageBar1.BarForeImage = null;
		imageBar1.BarForeImageRect = new Rectangle(0, 0, 0, 0);
		imageBar1.BarSliderImage = null;
		imageBar1.BarSliderImageRect = new Rectangle(0, 0, 0, 0);
		imageBar1.BarSliderMovable = true;
		imageBar1.FrameWidth = 2;
		imageBar1.IntervalWidth = 4;
		((Control)imageBar1).Location = new Point(179, 423);
		imageBar1.MaxValue = 100;
		((Control)imageBar1).Name = "imageBar1";
		imageBar1.ShowPercent = true;
		((Control)imageBar1).Size = new Size(550, 35);
		imageBar1.StepImage = null;
		((Control)imageBar1).TabIndex = 169;
		imageBar1.TextColor = Color.Black;
		imageBar1.TextLocation = new Point(0, 0);
		imageBar1.Value = 50;
		((Control)wIBtn_SliderImage).BackColor = Color.Transparent;
		wIBtn_SliderImage.DisableBackColor = Color.DarkGray;
		wIBtn_SliderImage.DisableForeColor = Color.DimGray;
		((Control)wIBtn_SliderImage).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wIBtn_SliderImage.FrameMode = GraphicsHelper.RoundStyle.All;
		wIBtn_SliderImage.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wIBtn_SliderImage.IconName = "";
		wIBtn_SliderImage.IconOffset = new Point(0, 0);
		wIBtn_SliderImage.IconSize = 32;
		((Control)wIBtn_SliderImage).Location = new Point(12, 125);
		wIBtn_SliderImage.MouseDownBackColor = Color.DimGray;
		wIBtn_SliderImage.MouseDownForeColor = Color.Black;
		wIBtn_SliderImage.MouseEnterBackColor = Color.Gray;
		wIBtn_SliderImage.MouseEnterForeColor = Color.Black;
		wIBtn_SliderImage.MouseUpBackColor = Color.DarkGray;
		wIBtn_SliderImage.MouseUpForeColor = Color.Black;
		((Control)wIBtn_SliderImage).Name = "wIBtn_SliderImage";
		wIBtn_SliderImage.Radius = 8;
		((Control)wIBtn_SliderImage).Size = new Size(136, 24);
		((Control)wIBtn_SliderImage).TabIndex = 167;
		((Control)wIBtn_SliderImage).Text = "滑条图片";
		wIBtn_SliderImage.TextAlignment = StringHelper.TextAlignment.Center;
		wIBtn_SliderImage.TextDynOffset = new Point(0, 0);
		wIBtn_SliderImage.TextFixLocation = new Point(0, 0);
		wIBtn_SliderImage.TextFixLocationEnable = false;
		((Control)wIBtn_SliderImage).Click += wIBtn_SliderImage_Click;
		((Control)wIBtn_foreImage).BackColor = Color.Transparent;
		wIBtn_foreImage.DisableBackColor = Color.DarkGray;
		wIBtn_foreImage.DisableForeColor = Color.DimGray;
		((Control)wIBtn_foreImage).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wIBtn_foreImage.FrameMode = GraphicsHelper.RoundStyle.All;
		wIBtn_foreImage.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wIBtn_foreImage.IconName = "";
		wIBtn_foreImage.IconOffset = new Point(0, 0);
		wIBtn_foreImage.IconSize = 32;
		((Control)wIBtn_foreImage).Location = new Point(12, 95);
		wIBtn_foreImage.MouseDownBackColor = Color.DimGray;
		wIBtn_foreImage.MouseDownForeColor = Color.Black;
		wIBtn_foreImage.MouseEnterBackColor = Color.Gray;
		wIBtn_foreImage.MouseEnterForeColor = Color.Black;
		wIBtn_foreImage.MouseUpBackColor = Color.DarkGray;
		wIBtn_foreImage.MouseUpForeColor = Color.Black;
		((Control)wIBtn_foreImage).Name = "wIBtn_foreImage";
		wIBtn_foreImage.Radius = 8;
		((Control)wIBtn_foreImage).Size = new Size(136, 24);
		((Control)wIBtn_foreImage).TabIndex = 164;
		((Control)wIBtn_foreImage).Text = "前景图片";
		wIBtn_foreImage.TextAlignment = StringHelper.TextAlignment.Center;
		wIBtn_foreImage.TextDynOffset = new Point(0, 0);
		wIBtn_foreImage.TextFixLocation = new Point(0, 0);
		wIBtn_foreImage.TextFixLocationEnable = false;
		((Control)wIBtn_foreImage).Click += wIBtn_foreImage_Click;
		((Control)wIBtn_backImage).BackColor = Color.Transparent;
		wIBtn_backImage.DisableBackColor = Color.DarkGray;
		wIBtn_backImage.DisableForeColor = Color.DimGray;
		((Control)wIBtn_backImage).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wIBtn_backImage.FrameMode = GraphicsHelper.RoundStyle.All;
		wIBtn_backImage.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wIBtn_backImage.IconName = "";
		wIBtn_backImage.IconOffset = new Point(0, 0);
		wIBtn_backImage.IconSize = 32;
		((Control)wIBtn_backImage).Location = new Point(12, 65);
		wIBtn_backImage.MouseDownBackColor = Color.DimGray;
		wIBtn_backImage.MouseDownForeColor = Color.Black;
		wIBtn_backImage.MouseEnterBackColor = Color.Gray;
		wIBtn_backImage.MouseEnterForeColor = Color.Black;
		wIBtn_backImage.MouseUpBackColor = Color.DarkGray;
		wIBtn_backImage.MouseUpForeColor = Color.Black;
		((Control)wIBtn_backImage).Name = "wIBtn_backImage";
		wIBtn_backImage.Radius = 8;
		((Control)wIBtn_backImage).Size = new Size(136, 24);
		((Control)wIBtn_backImage).TabIndex = 163;
		((Control)wIBtn_backImage).Text = "背景图片";
		wIBtn_backImage.TextAlignment = StringHelper.TextAlignment.Center;
		wIBtn_backImage.TextDynOffset = new Point(0, 0);
		wIBtn_backImage.TextFixLocation = new Point(0, 0);
		wIBtn_backImage.TextFixLocationEnable = false;
		((Control)wIBtn_backImage).Click += wIBtn_backImage_Click;
		((Control)wiButton_OK).BackColor = Color.Transparent;
		wiButton_OK.DisableBackColor = Color.DarkGray;
		wiButton_OK.DisableForeColor = Color.DimGray;
		((Control)wiButton_OK).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wiButton_OK.FrameMode = GraphicsHelper.RoundStyle.All;
		wiButton_OK.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wiButton_OK.IconName = "";
		wiButton_OK.IconOffset = new Point(0, 0);
		wiButton_OK.IconSize = 32;
		((Control)wiButton_OK).Location = new Point(11, 562);
		wiButton_OK.MouseDownBackColor = Color.MediumTurquoise;
		wiButton_OK.MouseDownForeColor = Color.Black;
		wiButton_OK.MouseEnterBackColor = Color.Turquoise;
		wiButton_OK.MouseEnterForeColor = Color.Black;
		wiButton_OK.MouseUpBackColor = Color.LightSeaGreen;
		wiButton_OK.MouseUpForeColor = Color.Black;
		((Control)wiButton_OK).Name = "wiButton_OK";
		wiButton_OK.Radius = 16;
		((Control)wiButton_OK).Size = new Size(136, 35);
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
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)0;
		((Control)this).BackgroundImage = (Image)(object)Resources.backBig;
		((Form)this).ClientSize = new Size(822, 618);
		((Control)this).Controls.Add((Control)(object)wIButton_barLib);
		((Control)this).Controls.Add((Control)(object)wiButton_textColor);
		((Control)this).Controls.Add((Control)(object)windImageButton_clearImage);
		((Control)this).Controls.Add((Control)(object)windCheckBox_NoShowPercent);
		((Control)this).Controls.Add((Control)(object)label2);
		((Control)this).Controls.Add((Control)(object)wIBtn_Font);
		((Control)this).Controls.Add((Control)(object)groupBox1);
		((Control)this).Controls.Add((Control)(object)imageControl_SliderImage);
		((Control)this).Controls.Add((Control)(object)imageControl_ForeImage);
		((Control)this).Controls.Add((Control)(object)imageControl_BackImage);
		((Control)this).Controls.Add((Control)(object)windCheckBox_FixSlider);
		((Control)this).Controls.Add((Control)(object)wIButton_StartProgress);
		((Control)this).Controls.Add((Control)(object)label1);
		((Control)this).Controls.Add((Control)(object)imageBar1);
		((Control)this).Controls.Add((Control)(object)wIBtn_SliderImage);
		((Control)this).Controls.Add((Control)(object)wIBtn_foreImage);
		((Control)this).Controls.Add((Control)(object)wIBtn_backImage);
		((Control)this).Controls.Add((Control)(object)wiButton_OK);
		((Control)this).Controls.Add((Control)(object)wiButton_mini);
		((Control)this).Controls.Add((Control)(object)wiButton_Close);
		((Control)this).Font = new Font("微软雅黑", 10.5f);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Form)this).KeyPreview = true;
		((Control)this).Name = "FormImageBarEdit";
		((Form)this).StartPosition = (FormStartPosition)0;
		((Control)this).Text = "EditCidMid";
		((Form)this).FormClosing += new FormClosingEventHandler(FormImageBarEdit_FormClosing);
		((Control)this).KeyDown += new KeyEventHandler(FormImageBarEdit_KeyDown);
		((Control)groupBox1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
