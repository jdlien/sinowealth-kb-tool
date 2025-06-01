using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using USBUpdateTool.Properties;
using WindControls;

namespace USBUpdateTool;

public class FormCompxMore : Form
{
	private FormPairTool formPairTool;

	private FormSaveUpgradeFile formSaveUpgradeFile;

	private FormEditCidMid formEditCidMid;

	public SkinForm skinForm = new SkinForm(_movable: true);

	public FormMore formMore;

	private ICSelect iCSelect = new ICSelect();

	private bool passwordSuccess = false;

	private IContainer components = null;

	private WindImageButton wibutton_Encrypt_Decode;

	private WindImageButton wiButton_mini;

	private WindImageButton wiButton_Close;

	private WindImageButton wibutton_Edit_CID_MID;

	private WindImageButton wibutton_Pairing;

	public FormCompxMore(FormMore _formMore)
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
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Invalid comparison between Unknown and I4
		((Form)this).Location = ((Form)formMore).Location;
		skinForm.AllShow((Form)(object)formMore);
		if (!passwordSuccess)
		{
			FormPasswordInput formPasswordInput = new FormPasswordInput(1);
			if ((int)formPasswordInput.ShowDialog((Form)(object)this) == 1)
			{
				passwordSuccess = true;
			}
			else
			{
				wiButton_Close_Click(null, null);
			}
		}
	}

	private void wibutton_Edit_CID_MID_Click(object sender, EventArgs e)
	{
		if (formEditCidMid == null)
		{
			formEditCidMid = new FormEditCidMid(this);
		}
		skinForm.AllHide();
		formEditCidMid.ShowWindow();
	}

	private void wibutton_Encrypt_Decode_Click(object sender, EventArgs e)
	{
		if (formSaveUpgradeFile == null)
		{
			formSaveUpgradeFile = new FormSaveUpgradeFile(this);
		}
		skinForm.AllHide();
		formSaveUpgradeFile.ShowWindow();
	}

	private void wibutton_Pairing_Click(object sender, EventArgs e)
	{
		if (formPairTool == null)
		{
			formPairTool = new FormPairTool(this);
		}
		skinForm.AllHide();
		formPairTool.ShowWindow();
	}

	private void wibutton_MultiPairing_Click(object sender, EventArgs e)
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
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Expected O, but got Unknown
		//IL_0721: Unknown result type (might be due to invalid IL or missing references)
		//IL_072b: Expected O, but got Unknown
		//IL_095f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0969: Expected O, but got Unknown
		//IL_0bb3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bbd: Expected O, but got Unknown
		//IL_0bd2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bdc: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FormCompxMore));
		wibutton_Encrypt_Decode = new WindImageButton();
		wiButton_mini = new WindImageButton();
		wiButton_Close = new WindImageButton();
		wibutton_Edit_CID_MID = new WindImageButton();
		wibutton_Pairing = new WindImageButton();
		((Control)this).SuspendLayout();
		((Control)wibutton_Encrypt_Decode).BackColor = Color.Transparent;
		((Control)wibutton_Encrypt_Decode).BackgroundImage = (Image)(object)Resources.button_1;
		((Control)wibutton_Encrypt_Decode).BackgroundImageLayout = (ImageLayout)3;
		wibutton_Encrypt_Decode.DisableBackColor = Color.Transparent;
		wibutton_Encrypt_Decode.DisableForeColor = Color.DarkGray;
		wibutton_Encrypt_Decode.DisableImage = (Image)(object)Resources.button_2;
		((Control)wibutton_Encrypt_Decode).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wibutton_Encrypt_Decode.FrameMode = GraphicsHelper.RoundStyle.All;
		wibutton_Encrypt_Decode.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wibutton_Encrypt_Decode.IconName = "";
		wibutton_Encrypt_Decode.IconOffset = new Point(0, 0);
		wibutton_Encrypt_Decode.IconSize = 32;
		((Control)wibutton_Encrypt_Decode).Location = new Point(219, 264);
		wibutton_Encrypt_Decode.MouseDownBackColor = Color.Gray;
		wibutton_Encrypt_Decode.MouseDownForeColor = Color.Black;
		wibutton_Encrypt_Decode.MouseDownImage = (Image)(object)Resources.点击状态1;
		wibutton_Encrypt_Decode.MouseEnterBackColor = Color.DarkGray;
		wibutton_Encrypt_Decode.MouseEnterForeColor = Color.Black;
		wibutton_Encrypt_Decode.MouseEnterImage = (Image)(object)Resources.点击状态1;
		wibutton_Encrypt_Decode.MouseUpBackColor = Color.Transparent;
		wibutton_Encrypt_Decode.MouseUpForeColor = Color.Black;
		wibutton_Encrypt_Decode.MouseUpImage = (Image)(object)Resources.默认状态1;
		((Control)wibutton_Encrypt_Decode).Name = "wibutton_Encrypt_Decode";
		wibutton_Encrypt_Decode.Radius = 12;
		((Control)wibutton_Encrypt_Decode).Size = new Size(200, 60);
		((Control)wibutton_Encrypt_Decode).TabIndex = 46;
		((Control)wibutton_Encrypt_Decode).Text = "Encrypt Decode";
		wibutton_Encrypt_Decode.TextAlignment = StringHelper.TextAlignment.Center;
		wibutton_Encrypt_Decode.TextDynOffset = new Point(0, 0);
		wibutton_Encrypt_Decode.TextFixLocation = new Point(0, 0);
		wibutton_Encrypt_Decode.TextFixLocationEnable = false;
		((Control)wibutton_Encrypt_Decode).Click += wibutton_Encrypt_Decode_Click;
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
		((Control)wibutton_Edit_CID_MID).BackColor = Color.Transparent;
		((Control)wibutton_Edit_CID_MID).BackgroundImage = (Image)(object)Resources.button_1;
		((Control)wibutton_Edit_CID_MID).BackgroundImageLayout = (ImageLayout)3;
		wibutton_Edit_CID_MID.DisableBackColor = Color.Transparent;
		wibutton_Edit_CID_MID.DisableForeColor = Color.DarkGray;
		wibutton_Edit_CID_MID.DisableImage = (Image)(object)Resources.button_2;
		((Control)wibutton_Edit_CID_MID).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wibutton_Edit_CID_MID.FrameMode = GraphicsHelper.RoundStyle.All;
		wibutton_Edit_CID_MID.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wibutton_Edit_CID_MID.IconName = "";
		wibutton_Edit_CID_MID.IconOffset = new Point(0, 0);
		wibutton_Edit_CID_MID.IconSize = 32;
		((Control)wibutton_Edit_CID_MID).Location = new Point(219, 142);
		wibutton_Edit_CID_MID.MouseDownBackColor = Color.Gray;
		wibutton_Edit_CID_MID.MouseDownForeColor = Color.Black;
		wibutton_Edit_CID_MID.MouseDownImage = (Image)(object)Resources.点击状态2;
		wibutton_Edit_CID_MID.MouseEnterBackColor = Color.DarkGray;
		wibutton_Edit_CID_MID.MouseEnterForeColor = Color.Black;
		wibutton_Edit_CID_MID.MouseEnterImage = (Image)(object)Resources.点击状态2;
		wibutton_Edit_CID_MID.MouseUpBackColor = Color.Transparent;
		wibutton_Edit_CID_MID.MouseUpForeColor = Color.Black;
		wibutton_Edit_CID_MID.MouseUpImage = (Image)(object)Resources.默认状态2;
		((Control)wibutton_Edit_CID_MID).Name = "wibutton_Edit_CID_MID";
		wibutton_Edit_CID_MID.Radius = 12;
		((Control)wibutton_Edit_CID_MID).Size = new Size(200, 60);
		((Control)wibutton_Edit_CID_MID).TabIndex = 151;
		((Control)wibutton_Edit_CID_MID).Text = "Edit CID/MID CX53710";
		wibutton_Edit_CID_MID.TextAlignment = StringHelper.TextAlignment.Center;
		wibutton_Edit_CID_MID.TextDynOffset = new Point(0, 0);
		wibutton_Edit_CID_MID.TextFixLocation = new Point(0, 0);
		wibutton_Edit_CID_MID.TextFixLocationEnable = false;
		((Control)wibutton_Edit_CID_MID).Click += wibutton_Edit_CID_MID_Click;
		((Control)wibutton_Pairing).BackColor = Color.Transparent;
		((Control)wibutton_Pairing).BackgroundImage = (Image)(object)Resources.button_1;
		((Control)wibutton_Pairing).BackgroundImageLayout = (ImageLayout)3;
		wibutton_Pairing.DisableBackColor = Color.Transparent;
		wibutton_Pairing.DisableForeColor = Color.DarkGray;
		wibutton_Pairing.DisableImage = (Image)(object)Resources.button_2;
		((Control)wibutton_Pairing).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wibutton_Pairing.FrameMode = GraphicsHelper.RoundStyle.All;
		wibutton_Pairing.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wibutton_Pairing.IconName = "";
		wibutton_Pairing.IconOffset = new Point(0, 0);
		wibutton_Pairing.IconSize = 32;
		((Control)wibutton_Pairing).Location = new Point(458, 141);
		wibutton_Pairing.MouseDownBackColor = Color.Gray;
		wibutton_Pairing.MouseDownForeColor = Color.Black;
		wibutton_Pairing.MouseDownImage = (Image)(object)Resources.点击状态2;
		wibutton_Pairing.MouseEnterBackColor = Color.DarkGray;
		wibutton_Pairing.MouseEnterForeColor = Color.Black;
		wibutton_Pairing.MouseEnterImage = (Image)(object)Resources.点击状态2;
		wibutton_Pairing.MouseUpBackColor = Color.Transparent;
		wibutton_Pairing.MouseUpForeColor = Color.Black;
		wibutton_Pairing.MouseUpImage = (Image)(object)Resources.默认状态2;
		((Control)wibutton_Pairing).Name = "wibutton_Pairing";
		wibutton_Pairing.Radius = 12;
		((Control)wibutton_Pairing).Size = new Size(200, 60);
		((Control)wibutton_Pairing).TabIndex = 153;
		((Control)wibutton_Pairing).Text = "Pairing";
		wibutton_Pairing.TextAlignment = StringHelper.TextAlignment.Center;
		wibutton_Pairing.TextDynOffset = new Point(0, 0);
		wibutton_Pairing.TextFixLocation = new Point(0, 0);
		wibutton_Pairing.TextFixLocationEnable = false;
		((Control)wibutton_Pairing).Click += wibutton_Pairing_Click;
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)0;
		((Control)this).BackgroundImage = (Image)(object)Resources.cidmid;
		((Form)this).ClientSize = new Size(822, 465);
		((Control)this).Controls.Add((Control)(object)wibutton_Pairing);
		((Control)this).Controls.Add((Control)(object)wibutton_Edit_CID_MID);
		((Control)this).Controls.Add((Control)(object)wiButton_mini);
		((Control)this).Controls.Add((Control)(object)wiButton_Close);
		((Control)this).Controls.Add((Control)(object)wibutton_Encrypt_Decode);
		((Control)this).Font = new Font("微软雅黑", 10.5f);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Control)this).Name = "FormCompxMore";
		((Form)this).StartPosition = (FormStartPosition)0;
		((Control)this).Text = "EditCidMid";
		((Control)this).ResumeLayout(false);
	}
}
