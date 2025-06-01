using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using USBUpdateTool.Properties;
using WindControls;

namespace USBUpdateTool;

public class FormProgressStyle : Form
{
	private Form form;

	public SkinForm skinForm = new SkinForm(_movable: true);

	private ColorDialog colorDialog = new ColorDialog();

	private List<WindProgressBar> progressBarList = new List<WindProgressBar>();

	private List<WindProgressBar> barsList;

	private ProgressAnimation progressAnimation;

	private FormImageBarEdit formImageBarEdit;

	private IContainer components = null;

	private WindImageButton wiButton_mini;

	private WindImageButton wiButton_Close;

	private WindRadioButton wRadioButton_Style_2;

	private WindRadioButton wRadioButton_Style_1;

	private WindImageButton wIBtn_progTextColor;

	private HaloBar haloBar1;

	private WindImageButton wIBtn_progForeColor;

	private WindImageButton wIBtn_progBackColor;

	private WindRadioButton wRadioButton_Style_0;

	private WindImageButton wiButton_OK;

	private SliderBar sliderBar1;

	private RoundBar roundBar1;

	private WindRadioButton wRadioButton_Style_3;

	private CellBar cellBar1;

	private ImageBar imageBar1;

	private WindImageButton wIBtn_ImageEdit;

	private WindRadioButton wRadioButton_Style_4;

	public FormProgressStyle(List<WindProgressBar> _barsList)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		InitializeComponent();
		SetStyles();
		skinForm.InitSkin((Form)(object)this, 12, 16);
		progressBarList.Add(haloBar1);
		progressBarList.Add(roundBar1);
		progressBarList.Add(cellBar1);
		progressBarList.Add(sliderBar1);
		progressBarList.Add(imageBar1);
		barsList = _barsList;
		progressAnimation = new ProgressAnimation((Form)(object)this);
		progressAnimation.Add(progressBarList);
	}

	private void SetStyles()
	{
		((Control)this).SetStyle((ControlStyles)204818, true);
		((Control)this).UpdateStyles();
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)0;
	}

	private WindProgressBar GetWindProgressBar()
	{
		for (int i = 0; i < barsList.Count; i++)
		{
			if (((Control)barsList[i]).Visible)
			{
				return barsList[i];
			}
		}
		return haloBar1;
	}

	private void SetProgressTextColor(Color color)
	{
		for (int i = 0; i < progressBarList.Count; i++)
		{
			progressBarList[i].TextColor = color;
		}
	}

	private void SetProgressForeColor(Color color)
	{
		for (int i = 0; i < progressBarList.Count; i++)
		{
			progressBarList[i].BarForeColor = color;
		}
	}

	private void SetProgressBackColor(Color color)
	{
		for (int i = 0; i < progressBarList.Count; i++)
		{
			progressBarList[i].BarBackColor = color;
		}
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
		progressAnimation.Start();
	}

	private void FormProgressStyle_Load(object sender, EventArgs e)
	{
	}

	private void wIBtn_progForeColor_Click(object sender, EventArgs e)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Invalid comparison between Unknown and I4
		if ((int)((CommonDialog)colorDialog).ShowDialog() == 1)
		{
			SetProgressForeColor(colorDialog.Color);
		}
	}

	private void wIBtn_progBackColor_Click(object sender, EventArgs e)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Invalid comparison between Unknown and I4
		if ((int)((CommonDialog)colorDialog).ShowDialog() == 1)
		{
			SetProgressBackColor(colorDialog.Color);
		}
	}

	private void wIBtn_progTextColor_Click(object sender, EventArgs e)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Invalid comparison between Unknown and I4
		if ((int)((CommonDialog)colorDialog).ShowDialog() == 1)
		{
			SetProgressTextColor(colorDialog.Color);
		}
	}

	private void FormProgressStyle_FormClosing(object sender, FormClosingEventArgs e)
	{
		progressAnimation.Stop();
	}

	private void wiButton_OK_Click(object sender, EventArgs e)
	{
		BAR_STYLE bAR_STYLE = BAR_STYLE.HaloBar;
		if (wRadioButton_Style_0.Checked)
		{
			bAR_STYLE = BAR_STYLE.HaloBar;
		}
		else if (wRadioButton_Style_1.Checked)
		{
			bAR_STYLE = BAR_STYLE.RoundBar;
		}
		else if (wRadioButton_Style_2.Checked)
		{
			bAR_STYLE = BAR_STYLE.CellBar;
		}
		else if (wRadioButton_Style_3.Checked)
		{
			bAR_STYLE = BAR_STYLE.SliderBar;
		}
		else if (wRadioButton_Style_4.Checked)
		{
			bAR_STYLE = BAR_STYLE.ImageBar;
		}
		for (int i = 0; i < barsList.Count; i++)
		{
			((Control)barsList[i]).Visible = barsList[i].BarStyle == bAR_STYLE;
			barsList[i].TextColor = progressBarList[0].TextColor;
			barsList[i].BarForeColor = progressBarList[0].BarForeColor;
			barsList[i].BarBackColor = progressBarList[0].BarBackColor;
			barsList[i].BarSliderImage = imageBar1.BarSliderImage;
			barsList[i].BarForeImage = imageBar1.BarForeImage;
			barsList[i].BarBackImage = imageBar1.BarBackImage;
			barsList[i].BarSliderImageRect = imageBar1.BarSliderImageRect;
			barsList[i].BarForeImageRect = imageBar1.BarForeImageRect;
			barsList[i].BarBackImageRect = imageBar1.BarBackImageRect;
			barsList[i].BarSliderMovable = imageBar1.BarSliderMovable;
			barsList[i].ShowPercent = imageBar1.ShowPercent;
			barsList[i].TextLocation = imageBar1.TextLocation;
			((Control)barsList[i]).Font = ((Control)imageBar1).Font;
			((Control)barsList[i]).Invalidate();
		}
		wiButton_Close_Click(null, null);
	}

	private void wIBtn_ImageEdit_Click(object sender, EventArgs e)
	{
		if (formImageBarEdit == null)
		{
			formImageBarEdit = new FormImageBarEdit(imageBar1);
		}
		formImageBarEdit.SetProgressBar(imageBar1);
		formImageBarEdit.ShowWindow((Form)(object)this);
	}

	private void wRadioButton_Style_4_CheckedChanged(object sender, EventArgs e)
	{
		((Control)wIBtn_ImageEdit).Enabled = wRadioButton_Style_4.Checked;
	}

	public void SetProgressBar(Color textColor, Color barForeColor, Color barBackColor, ImageBar imageBar)
	{
		for (int i = 0; i < progressBarList.Count; i++)
		{
			progressBarList[i].TextColor = textColor;
			progressBarList[i].BarForeColor = barForeColor;
			progressBarList[i].BarBackColor = barBackColor;
			progressBarList[i].BarSliderImage = imageBar.BarSliderImage;
			progressBarList[i].BarForeImage = imageBar.BarForeImage;
			progressBarList[i].BarBackImage = imageBar.BarBackImage;
			progressBarList[i].BarSliderImageRect = imageBar.BarSliderImageRect;
			progressBarList[i].BarForeImageRect = imageBar.BarForeImageRect;
			progressBarList[i].BarBackImageRect = imageBar.BarBackImageRect;
			progressBarList[i].BarSliderMovable = imageBar.BarSliderMovable;
			progressBarList[i].TextLocation = imageBar.TextLocation;
			((Control)progressBarList[i]).Font = ((Control)imageBar).Font;
			((Control)progressBarList[i]).Invalidate();
		}
		WindProgressBar windProgressBar = GetWindProgressBar();
		switch (windProgressBar.BarStyle)
		{
		case BAR_STYLE.HaloBar:
			wRadioButton_Style_0.Checked = true;
			break;
		case BAR_STYLE.RoundBar:
			wRadioButton_Style_1.Checked = true;
			break;
		case BAR_STYLE.CellBar:
			wRadioButton_Style_2.Checked = true;
			break;
		case BAR_STYLE.SliderBar:
			wRadioButton_Style_3.Checked = true;
			break;
		case BAR_STYLE.ImageBar:
			wRadioButton_Style_4.Checked = true;
			break;
		case BAR_STYLE.CircularBar:
			break;
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
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Expected O, but got Unknown
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Expected O, but got Unknown
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Expected O, but got Unknown
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Expected O, but got Unknown
		//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c7: Expected O, but got Unknown
		//IL_0524: Unknown result type (might be due to invalid IL or missing references)
		//IL_052e: Expected O, but got Unknown
		//IL_056e: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c2: Expected O, but got Unknown
		//IL_07d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07de: Expected O, but got Unknown
		//IL_085f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0869: Expected O, but got Unknown
		//IL_08f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0903: Expected O, but got Unknown
		//IL_09c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09cb: Expected O, but got Unknown
		//IL_09f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b47: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b51: Expected O, but got Unknown
		//IL_0b7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c64: Expected O, but got Unknown
		//IL_0e36: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e40: Expected O, but got Unknown
		//IL_0e52: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e5c: Expected O, but got Unknown
		//IL_0eda: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ee4: Expected O, but got Unknown
		//IL_0f63: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f6d: Expected O, but got Unknown
		//IL_0fb8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fc2: Expected O, but got Unknown
		//IL_1190: Unknown result type (might be due to invalid IL or missing references)
		//IL_119a: Expected O, but got Unknown
		//IL_14d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_14db: Expected O, but got Unknown
		//IL_1698: Unknown result type (might be due to invalid IL or missing references)
		//IL_16a2: Expected O, but got Unknown
		//IL_16b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_16be: Expected O, but got Unknown
		//IL_173c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1746: Expected O, but got Unknown
		//IL_17d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_17dd: Expected O, but got Unknown
		//IL_181a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1824: Expected O, but got Unknown
		//IL_1836: Unknown result type (might be due to invalid IL or missing references)
		//IL_1840: Expected O, but got Unknown
		//IL_18c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_18cb: Expected O, but got Unknown
		//IL_1958: Unknown result type (might be due to invalid IL or missing references)
		//IL_1962: Expected O, but got Unknown
		//IL_1ee8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ef2: Expected O, but got Unknown
		//IL_1f07: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f11: Expected O, but got Unknown
		//IL_1f3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f44: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FormProgressStyle));
		wRadioButton_Style_4 = new WindRadioButton();
		wIBtn_ImageEdit = new WindImageButton();
		imageBar1 = new ImageBar();
		cellBar1 = new CellBar();
		wRadioButton_Style_3 = new WindRadioButton();
		roundBar1 = new RoundBar();
		sliderBar1 = new SliderBar();
		wiButton_OK = new WindImageButton();
		wRadioButton_Style_0 = new WindRadioButton();
		wIBtn_progBackColor = new WindImageButton();
		wIBtn_progForeColor = new WindImageButton();
		haloBar1 = new HaloBar();
		wIBtn_progTextColor = new WindImageButton();
		wRadioButton_Style_1 = new WindRadioButton();
		wRadioButton_Style_2 = new WindRadioButton();
		wiButton_mini = new WindImageButton();
		wiButton_Close = new WindImageButton();
		((Control)this).SuspendLayout();
		((Control)wRadioButton_Style_4).BackColor = Color.Transparent;
		((Control)wRadioButton_Style_4).BackgroundImageLayout = (ImageLayout)0;
		wRadioButton_Style_4.Checked = true;
		wRadioButton_Style_4.DisableSelectedImage = (Image)componentResourceManager.GetObject("wRadioButton_Style_4.DisableSelectedImage");
		wRadioButton_Style_4.DisableUnSelectedImage = (Image)componentResourceManager.GetObject("wRadioButton_Style_4.DisableUnSelectedImage");
		wRadioButton_Style_4.IconOffset = new Point(0, 0);
		wRadioButton_Style_4.IconSize = 36;
		((Control)wRadioButton_Style_4).Location = new Point(743, 249);
		((Control)wRadioButton_Style_4).Name = "wRadioButton_Style_4";
		wRadioButton_Style_4.SelectedIconColor = Color.Red;
		wRadioButton_Style_4.SelectedIconName = "A_fa_dot_circle_o";
		wRadioButton_Style_4.SelectedImage = (Image)componentResourceManager.GetObject("wRadioButton_Style_4.SelectedImage");
		((Control)wRadioButton_Style_4).Size = new Size(47, 39);
		((Control)wRadioButton_Style_4).TabIndex = 170;
		((Control)wRadioButton_Style_4).Tag = "2";
		((Control)wRadioButton_Style_4).Text = " ";
		wRadioButton_Style_4.TextOffset = new Point(0, -2);
		wRadioButton_Style_4.UnSelectedIconColor = Color.Gray;
		wRadioButton_Style_4.UnSelectedIconName = "A_fa_circle_o";
		wRadioButton_Style_4.UnSelectedImage = (Image)componentResourceManager.GetObject("wRadioButton_Style_4.UnSelectedImage");
		wRadioButton_Style_4.CheckedChanged += wRadioButton_Style_4_CheckedChanged;
		((Control)wIBtn_ImageEdit).BackColor = Color.Transparent;
		wIBtn_ImageEdit.DisableBackColor = Color.DarkGray;
		wIBtn_ImageEdit.DisableForeColor = Color.DimGray;
		((Control)wIBtn_ImageEdit).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wIBtn_ImageEdit.FrameMode = GraphicsHelper.RoundStyle.All;
		wIBtn_ImageEdit.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wIBtn_ImageEdit.IconName = "";
		wIBtn_ImageEdit.IconOffset = new Point(0, 0);
		wIBtn_ImageEdit.IconSize = 32;
		((Control)wIBtn_ImageEdit).Location = new Point(12, 264);
		wIBtn_ImageEdit.MouseDownBackColor = Color.DimGray;
		wIBtn_ImageEdit.MouseDownForeColor = Color.Black;
		wIBtn_ImageEdit.MouseEnterBackColor = Color.Gray;
		wIBtn_ImageEdit.MouseEnterForeColor = Color.Black;
		wIBtn_ImageEdit.MouseUpBackColor = Color.DarkGray;
		wIBtn_ImageEdit.MouseUpForeColor = Color.Black;
		((Control)wIBtn_ImageEdit).Name = "wIBtn_ImageEdit";
		wIBtn_ImageEdit.Radius = 8;
		((Control)wIBtn_ImageEdit).Size = new Size(136, 24);
		((Control)wIBtn_ImageEdit).TabIndex = 169;
		((Control)wIBtn_ImageEdit).Text = "图片设置";
		wIBtn_ImageEdit.TextAlignment = StringHelper.TextAlignment.Center;
		wIBtn_ImageEdit.TextDynOffset = new Point(0, 0);
		wIBtn_ImageEdit.TextFixLocation = new Point(0, 0);
		wIBtn_ImageEdit.TextFixLocationEnable = false;
		((Control)wIBtn_ImageEdit).Click += wIBtn_ImageEdit_Click;
		((Control)imageBar1).BackColor = Color.Transparent;
		imageBar1.BarBackColor = Color.DarkGray;
		imageBar1.BarBackImage = (Image)(object)Resources.BarBackImage;
		imageBar1.BarBackImageRect = new Rectangle(18, 29, 384, 32);
		imageBar1.BarForeColor = Color.DodgerBlue;
		imageBar1.BarForeImage = (Image)(object)Resources.BarForeImage;
		imageBar1.BarForeImageRect = new Rectangle(23, 33, 376, 25);
		imageBar1.BarSliderImage = (Image)(object)Resources.BarSliderImage;
		imageBar1.BarSliderImageRect = new Rectangle(0, 0, 40, 25);
		imageBar1.BarSliderMovable = true;
		((Control)imageBar1).Font = new Font("微软雅黑", 12f, (FontStyle)2, (GraphicsUnit)3, (byte)134);
		imageBar1.FrameWidth = 2;
		imageBar1.IntervalWidth = 4;
		((Control)imageBar1).Location = new Point(168, 221);
		((Control)imageBar1).Margin = new Padding(5, 5, 5, 5);
		imageBar1.MaxValue = 100;
		((Control)imageBar1).Name = "imageBar1";
		imageBar1.ShowPercent = true;
		((Control)imageBar1).Size = new Size(425, 67);
		imageBar1.StepImage = null;
		((Control)imageBar1).TabIndex = 168;
		imageBar1.TextColor = Color.Black;
		imageBar1.TextLocation = new Point(0, 8);
		imageBar1.Value = 50;
		((Control)cellBar1).BackColor = Color.Transparent;
		cellBar1.BarBackColor = Color.DarkGray;
		cellBar1.BarBackImage = null;
		cellBar1.BarBackImageRect = new Rectangle(0, 0, 0, 0);
		cellBar1.BarForeColor = Color.DodgerBlue;
		cellBar1.BarForeImage = null;
		cellBar1.BarForeImageRect = new Rectangle(0, 0, 0, 0);
		cellBar1.BarSliderImage = null;
		cellBar1.BarSliderImageRect = new Rectangle(0, 0, 0, 0);
		cellBar1.BarSliderMovable = true;
		cellBar1.FrameWidth = 4;
		cellBar1.GridWidth = 8;
		cellBar1.IntervalWidth = 4;
		((Control)cellBar1).Location = new Point(168, 153);
		cellBar1.MaxValue = 100;
		((Control)cellBar1).Name = "cellBar1";
		cellBar1.ShowPercent = true;
		((Control)cellBar1).Size = new Size(229, 22);
		((Control)cellBar1).TabIndex = 167;
		cellBar1.TextColor = Color.Black;
		cellBar1.TextLocation = new Point(0, 0);
		cellBar1.Value = 50;
		((Control)wRadioButton_Style_3).BackColor = Color.Transparent;
		((Control)wRadioButton_Style_3).BackgroundImageLayout = (ImageLayout)0;
		wRadioButton_Style_3.Checked = false;
		wRadioButton_Style_3.DisableSelectedImage = (Image)componentResourceManager.GetObject("wRadioButton_Style_3.DisableSelectedImage");
		wRadioButton_Style_3.DisableUnSelectedImage = (Image)componentResourceManager.GetObject("wRadioButton_Style_3.DisableUnSelectedImage");
		wRadioButton_Style_3.IconOffset = new Point(0, 0);
		wRadioButton_Style_3.IconSize = 36;
		((Control)wRadioButton_Style_3).Location = new Point(743, 153);
		((Control)wRadioButton_Style_3).Name = "wRadioButton_Style_3";
		wRadioButton_Style_3.SelectedIconColor = Color.Red;
		wRadioButton_Style_3.SelectedIconName = "A_fa_dot_circle_o";
		wRadioButton_Style_3.SelectedImage = (Image)componentResourceManager.GetObject("wRadioButton_Style_3.SelectedImage");
		((Control)wRadioButton_Style_3).Size = new Size(47, 39);
		((Control)wRadioButton_Style_3).TabIndex = 166;
		((Control)wRadioButton_Style_3).Tag = "3";
		((Control)wRadioButton_Style_3).Text = " ";
		wRadioButton_Style_3.TextOffset = new Point(0, -2);
		wRadioButton_Style_3.UnSelectedIconColor = Color.Gray;
		wRadioButton_Style_3.UnSelectedIconName = "A_fa_circle_o";
		wRadioButton_Style_3.UnSelectedImage = (Image)componentResourceManager.GetObject("wRadioButton_Style_3.UnSelectedImage");
		((Control)roundBar1).BackColor = Color.Transparent;
		roundBar1.BarBackColor = Color.Gray;
		roundBar1.BarBackImage = null;
		roundBar1.BarBackImageRect = new Rectangle(0, 0, 0, 0);
		roundBar1.BarForeColor = Color.DodgerBlue;
		roundBar1.BarForeImage = null;
		roundBar1.BarForeImageRect = new Rectangle(0, 0, 0, 0);
		roundBar1.BarSliderImage = null;
		roundBar1.BarSliderImageRect = new Rectangle(0, 0, 0, 0);
		roundBar1.BarSliderMovable = true;
		((Control)roundBar1).Font = new Font("微软雅黑", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		roundBar1.FrameWidth = 2;
		((Control)roundBar1).Location = new Point(497, 62);
		((Control)roundBar1).Margin = new Padding(5);
		roundBar1.MaxValue = 100;
		((Control)roundBar1).Name = "roundBar1";
		roundBar1.ShowPercent = true;
		((Control)roundBar1).Size = new Size(229, 35);
		((Control)roundBar1).TabIndex = 164;
		roundBar1.TextColor = Color.Black;
		roundBar1.TextLocation = new Point(0, 0);
		roundBar1.Value = 50;
		((Control)sliderBar1).BackColor = Color.Transparent;
		sliderBar1.BarBackColor = Color.Gray;
		sliderBar1.BarBackImage = null;
		sliderBar1.BarBackImageRect = new Rectangle(0, 0, 0, 0);
		sliderBar1.BarForeColor = Color.DodgerBlue;
		sliderBar1.BarForeImage = null;
		sliderBar1.BarForeImageRect = new Rectangle(0, 0, 0, 0);
		sliderBar1.BarSliderImage = null;
		sliderBar1.BarSliderImageRect = new Rectangle(0, 0, 0, 0);
		sliderBar1.BarSliderMovable = true;
		((Control)sliderBar1).Font = new Font("微软雅黑", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		sliderBar1.FrameWidth = 2;
		((Control)sliderBar1).Location = new Point(497, 122);
		((Control)sliderBar1).Margin = new Padding(5);
		sliderBar1.MaxValue = 100;
		((Control)sliderBar1).Name = "sliderBar1";
		sliderBar1.ShowPercent = true;
		((Control)sliderBar1).Size = new Size(229, 53);
		((Control)sliderBar1).TabIndex = 163;
		sliderBar1.TextColor = Color.Black;
		sliderBar1.TextLocation = new Point(0, 0);
		sliderBar1.Value = 50;
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
		((Control)wiButton_OK).Size = new Size(136, 35);
		((Control)wiButton_OK).TabIndex = 162;
		((Control)wiButton_OK).Tag = "0";
		((Control)wiButton_OK).Text = "确定";
		wiButton_OK.TextAlignment = StringHelper.TextAlignment.Center;
		wiButton_OK.TextDynOffset = new Point(0, 0);
		wiButton_OK.TextFixLocation = new Point(0, 0);
		wiButton_OK.TextFixLocationEnable = false;
		((Control)wiButton_OK).Click += wiButton_OK_Click;
		((Control)wRadioButton_Style_0).BackColor = Color.Transparent;
		((Control)wRadioButton_Style_0).BackgroundImageLayout = (ImageLayout)0;
		wRadioButton_Style_0.Checked = false;
		wRadioButton_Style_0.DisableSelectedImage = (Image)componentResourceManager.GetObject("wRadioButton_Style_0.DisableSelectedImage");
		wRadioButton_Style_0.DisableUnSelectedImage = (Image)componentResourceManager.GetObject("wRadioButton_Style_0.DisableUnSelectedImage");
		wRadioButton_Style_0.IconOffset = new Point(0, 0);
		wRadioButton_Style_0.IconSize = 36;
		((Control)wRadioButton_Style_0).Location = new Point(403, 62);
		((Control)wRadioButton_Style_0).Name = "wRadioButton_Style_0";
		wRadioButton_Style_0.SelectedIconColor = Color.Red;
		wRadioButton_Style_0.SelectedIconName = "A_fa_dot_circle_o";
		wRadioButton_Style_0.SelectedImage = (Image)componentResourceManager.GetObject("wRadioButton_Style_0.SelectedImage");
		((Control)wRadioButton_Style_0).Size = new Size(47, 39);
		((Control)wRadioButton_Style_0).TabIndex = 161;
		((Control)wRadioButton_Style_0).Text = " ";
		wRadioButton_Style_0.TextOffset = new Point(0, -2);
		wRadioButton_Style_0.UnSelectedIconColor = Color.Gray;
		wRadioButton_Style_0.UnSelectedIconName = "A_fa_circle_o";
		wRadioButton_Style_0.UnSelectedImage = (Image)componentResourceManager.GetObject("wRadioButton_Style_0.UnSelectedImage");
		((Control)wIBtn_progBackColor).BackColor = Color.Transparent;
		wIBtn_progBackColor.DisableBackColor = Color.DarkGray;
		wIBtn_progBackColor.DisableForeColor = Color.DimGray;
		((Control)wIBtn_progBackColor).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wIBtn_progBackColor.FrameMode = GraphicsHelper.RoundStyle.All;
		wIBtn_progBackColor.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wIBtn_progBackColor.IconName = "";
		wIBtn_progBackColor.IconOffset = new Point(0, 0);
		wIBtn_progBackColor.IconSize = 32;
		((Control)wIBtn_progBackColor).Location = new Point(12, 157);
		wIBtn_progBackColor.MouseDownBackColor = Color.DimGray;
		wIBtn_progBackColor.MouseDownForeColor = Color.Black;
		wIBtn_progBackColor.MouseEnterBackColor = Color.Gray;
		wIBtn_progBackColor.MouseEnterForeColor = Color.Black;
		wIBtn_progBackColor.MouseUpBackColor = Color.DarkGray;
		wIBtn_progBackColor.MouseUpForeColor = Color.Black;
		((Control)wIBtn_progBackColor).Name = "wIBtn_progBackColor";
		wIBtn_progBackColor.Radius = 8;
		((Control)wIBtn_progBackColor).Size = new Size(136, 24);
		((Control)wIBtn_progBackColor).TabIndex = 160;
		((Control)wIBtn_progBackColor).Text = "背景色";
		wIBtn_progBackColor.TextAlignment = StringHelper.TextAlignment.Center;
		wIBtn_progBackColor.TextDynOffset = new Point(0, 0);
		wIBtn_progBackColor.TextFixLocation = new Point(0, 0);
		wIBtn_progBackColor.TextFixLocationEnable = false;
		((Control)wIBtn_progBackColor).Click += wIBtn_progBackColor_Click;
		((Control)wIBtn_progForeColor).BackColor = Color.Transparent;
		wIBtn_progForeColor.DisableBackColor = Color.DarkGray;
		wIBtn_progForeColor.DisableForeColor = Color.DimGray;
		((Control)wIBtn_progForeColor).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wIBtn_progForeColor.FrameMode = GraphicsHelper.RoundStyle.All;
		wIBtn_progForeColor.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wIBtn_progForeColor.IconName = "";
		wIBtn_progForeColor.IconOffset = new Point(0, 0);
		wIBtn_progForeColor.IconSize = 32;
		((Control)wIBtn_progForeColor).Location = new Point(12, 117);
		wIBtn_progForeColor.MouseDownBackColor = Color.DimGray;
		wIBtn_progForeColor.MouseDownForeColor = Color.Black;
		wIBtn_progForeColor.MouseEnterBackColor = Color.Gray;
		wIBtn_progForeColor.MouseEnterForeColor = Color.Black;
		wIBtn_progForeColor.MouseUpBackColor = Color.DarkGray;
		wIBtn_progForeColor.MouseUpForeColor = Color.Black;
		((Control)wIBtn_progForeColor).Name = "wIBtn_progForeColor";
		wIBtn_progForeColor.Radius = 8;
		((Control)wIBtn_progForeColor).Size = new Size(136, 24);
		((Control)wIBtn_progForeColor).TabIndex = 159;
		((Control)wIBtn_progForeColor).Text = "前景色";
		wIBtn_progForeColor.TextAlignment = StringHelper.TextAlignment.Center;
		wIBtn_progForeColor.TextDynOffset = new Point(0, 0);
		wIBtn_progForeColor.TextFixLocation = new Point(0, 0);
		wIBtn_progForeColor.TextFixLocationEnable = false;
		((Control)wIBtn_progForeColor).Click += wIBtn_progForeColor_Click;
		((Control)haloBar1).BackColor = Color.Transparent;
		haloBar1.BarBackColor = Color.DimGray;
		haloBar1.BarBackImage = null;
		haloBar1.BarBackImageRect = new Rectangle(0, 0, 0, 0);
		haloBar1.BarForeColor = Color.DodgerBlue;
		haloBar1.BarForeImage = null;
		haloBar1.BarForeImageRect = new Rectangle(0, 0, 0, 0);
		haloBar1.BarSliderImage = null;
		haloBar1.BarSliderImageRect = new Rectangle(0, 0, 0, 0);
		haloBar1.BarSliderMovable = true;
		haloBar1.BarWidth = 10;
		haloBar1.EnableMouse = false;
		haloBar1.FrameWidth = 2;
		((Control)haloBar1).Location = new Point(168, 58);
		haloBar1.MaxValue = 100;
		((Control)haloBar1).Name = "haloBar1";
		haloBar1.ShowPercent = true;
		((Control)haloBar1).Size = new Size(229, 43);
		((Control)haloBar1).TabIndex = 158;
		haloBar1.TextColor = Color.YellowGreen;
		haloBar1.TextLocation = new Point(0, 0);
		haloBar1.Value = 0;
		((Control)wIBtn_progTextColor).BackColor = Color.Transparent;
		wIBtn_progTextColor.DisableBackColor = Color.DarkGray;
		wIBtn_progTextColor.DisableForeColor = Color.DimGray;
		((Control)wIBtn_progTextColor).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wIBtn_progTextColor.FrameMode = GraphicsHelper.RoundStyle.All;
		wIBtn_progTextColor.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wIBtn_progTextColor.IconName = "";
		wIBtn_progTextColor.IconOffset = new Point(0, 0);
		wIBtn_progTextColor.IconSize = 32;
		((Control)wIBtn_progTextColor).Location = new Point(12, 77);
		wIBtn_progTextColor.MouseDownBackColor = Color.DimGray;
		wIBtn_progTextColor.MouseDownForeColor = Color.Black;
		wIBtn_progTextColor.MouseEnterBackColor = Color.Gray;
		wIBtn_progTextColor.MouseEnterForeColor = Color.Black;
		wIBtn_progTextColor.MouseUpBackColor = Color.DarkGray;
		wIBtn_progTextColor.MouseUpForeColor = Color.Black;
		((Control)wIBtn_progTextColor).Name = "wIBtn_progTextColor";
		wIBtn_progTextColor.Radius = 8;
		((Control)wIBtn_progTextColor).Size = new Size(136, 24);
		((Control)wIBtn_progTextColor).TabIndex = 157;
		((Control)wIBtn_progTextColor).Text = "文本颜色";
		wIBtn_progTextColor.TextAlignment = StringHelper.TextAlignment.Center;
		wIBtn_progTextColor.TextDynOffset = new Point(0, 0);
		wIBtn_progTextColor.TextFixLocation = new Point(0, 0);
		wIBtn_progTextColor.TextFixLocationEnable = false;
		((Control)wIBtn_progTextColor).Click += wIBtn_progTextColor_Click;
		((Control)wRadioButton_Style_1).BackColor = Color.Transparent;
		((Control)wRadioButton_Style_1).BackgroundImageLayout = (ImageLayout)0;
		wRadioButton_Style_1.Checked = false;
		wRadioButton_Style_1.DisableSelectedImage = (Image)componentResourceManager.GetObject("wRadioButton_Style_1.DisableSelectedImage");
		wRadioButton_Style_1.DisableUnSelectedImage = (Image)componentResourceManager.GetObject("wRadioButton_Style_1.DisableUnSelectedImage");
		wRadioButton_Style_1.IconOffset = new Point(0, 0);
		wRadioButton_Style_1.IconSize = 36;
		((Control)wRadioButton_Style_1).Location = new Point(743, 62);
		((Control)wRadioButton_Style_1).Name = "wRadioButton_Style_1";
		wRadioButton_Style_1.SelectedIconColor = Color.Red;
		wRadioButton_Style_1.SelectedIconName = "A_fa_dot_circle_o";
		wRadioButton_Style_1.SelectedImage = (Image)componentResourceManager.GetObject("wRadioButton_Style_1.SelectedImage");
		((Control)wRadioButton_Style_1).Size = new Size(47, 39);
		((Control)wRadioButton_Style_1).TabIndex = 110;
		((Control)wRadioButton_Style_1).Tag = "1";
		((Control)wRadioButton_Style_1).Text = " ";
		wRadioButton_Style_1.TextOffset = new Point(0, -2);
		wRadioButton_Style_1.UnSelectedIconColor = Color.Gray;
		wRadioButton_Style_1.UnSelectedIconName = "A_fa_circle_o";
		wRadioButton_Style_1.UnSelectedImage = (Image)componentResourceManager.GetObject("wRadioButton_Style_1.UnSelectedImage");
		((Control)wRadioButton_Style_2).BackColor = Color.Transparent;
		((Control)wRadioButton_Style_2).BackgroundImageLayout = (ImageLayout)0;
		wRadioButton_Style_2.Checked = false;
		wRadioButton_Style_2.DisableSelectedImage = (Image)componentResourceManager.GetObject("wRadioButton_Style_2.DisableSelectedImage");
		wRadioButton_Style_2.DisableUnSelectedImage = (Image)componentResourceManager.GetObject("wRadioButton_Style_2.DisableUnSelectedImage");
		wRadioButton_Style_2.IconOffset = new Point(0, 0);
		wRadioButton_Style_2.IconSize = 36;
		((Control)wRadioButton_Style_2).Location = new Point(403, 148);
		((Control)wRadioButton_Style_2).Name = "wRadioButton_Style_2";
		wRadioButton_Style_2.SelectedIconColor = Color.Red;
		wRadioButton_Style_2.SelectedIconName = "A_fa_dot_circle_o";
		wRadioButton_Style_2.SelectedImage = (Image)componentResourceManager.GetObject("wRadioButton_Style_2.SelectedImage");
		((Control)wRadioButton_Style_2).Size = new Size(47, 39);
		((Control)wRadioButton_Style_2).TabIndex = 108;
		((Control)wRadioButton_Style_2).Tag = "2";
		((Control)wRadioButton_Style_2).Text = " ";
		wRadioButton_Style_2.TextOffset = new Point(0, -2);
		wRadioButton_Style_2.UnSelectedIconColor = Color.Gray;
		wRadioButton_Style_2.UnSelectedIconName = "A_fa_circle_o";
		wRadioButton_Style_2.UnSelectedImage = (Image)componentResourceManager.GetObject("wRadioButton_Style_2.UnSelectedImage");
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
		((Control)this).BackgroundImage = (Image)(object)Resources.cidmid;
		((Form)this).ClientSize = new Size(822, 465);
		((Control)this).Controls.Add((Control)(object)wRadioButton_Style_4);
		((Control)this).Controls.Add((Control)(object)wIBtn_ImageEdit);
		((Control)this).Controls.Add((Control)(object)imageBar1);
		((Control)this).Controls.Add((Control)(object)cellBar1);
		((Control)this).Controls.Add((Control)(object)wRadioButton_Style_3);
		((Control)this).Controls.Add((Control)(object)roundBar1);
		((Control)this).Controls.Add((Control)(object)sliderBar1);
		((Control)this).Controls.Add((Control)(object)wiButton_OK);
		((Control)this).Controls.Add((Control)(object)wRadioButton_Style_0);
		((Control)this).Controls.Add((Control)(object)wIBtn_progBackColor);
		((Control)this).Controls.Add((Control)(object)wIBtn_progForeColor);
		((Control)this).Controls.Add((Control)(object)haloBar1);
		((Control)this).Controls.Add((Control)(object)wIBtn_progTextColor);
		((Control)this).Controls.Add((Control)(object)wRadioButton_Style_1);
		((Control)this).Controls.Add((Control)(object)wRadioButton_Style_2);
		((Control)this).Controls.Add((Control)(object)wiButton_mini);
		((Control)this).Controls.Add((Control)(object)wiButton_Close);
		((Control)this).Font = new Font("微软雅黑", 10.5f);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Control)this).Name = "FormProgressStyle";
		((Form)this).StartPosition = (FormStartPosition)0;
		((Control)this).Text = "EditCidMid";
		((Form)this).FormClosing += new FormClosingEventHandler(FormProgressStyle_FormClosing);
		((Form)this).Load += FormProgressStyle_Load;
		((Control)this).ResumeLayout(false);
	}
}
