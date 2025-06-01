using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using USBUpdateTool.Properties;
using WindControls;

namespace USBUpdateTool;

public class FormPasswordInput : Form
{
	private FormPasswordBack formPasswordBack = new FormPasswordBack();

	private SkinForm skinForm = new SkinForm(_movable: false);

	private int passwordIndex = 0;

	private IContainer components = null;

	public WindImageButton wiButton_Close;

	private Label label1;

	private Label label_error;

	public WindImageButton wibutton_confirm;

	private TextBox textBox_Password;

	private WindImageButton wImageButton_eye;

	public FormPasswordInput(int _passwordIndex)
	{
		InitializeComponent();
		SetStyles();
		skinForm.InitSkin((Form)(object)this, 12, 16);
		passwordIndex = _passwordIndex;
	}

	private void SetStyles()
	{
		((Control)this).SetStyle((ControlStyles)204818, true);
		((Control)this).UpdateStyles();
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)0;
	}

	private void wiButton_Close_Click(object sender, EventArgs e)
	{
		((Form)this).DialogResult = (DialogResult)2;
	}

	public DialogResult ShowDialog(Form Owner)
	{
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		formPasswordBack.Show(Owner);
		((Form)skinForm).Owner = (Form)(object)formPasswordBack;
		int x = Owner.Location.X + (((Control)Owner).Width - ((Control)this).Width) / 2;
		int y = Owner.Location.Y + (((Control)Owner).Height - ((Control)this).Height) / 2;
		((Form)this).Location = new Point(x, y);
		DialogResult result = ((Form)this).ShowDialog();
		Owner.Activate();
		return result;
	}

	private void FormSetting_KeyPress(object sender, KeyPressEventArgs e)
	{
		if (e.KeyChar == '\u001b')
		{
			wiButton_Close_Click(null, null);
		}
		else if (e.KeyChar == '\r')
		{
			wibutton_confirm_Click(null, null);
		}
	}

	private void FormPasswordInput_FormClosing(object sender, FormClosingEventArgs e)
	{
		((Form)formPasswordBack).Close();
	}

	private void wibutton_confirm_Click(object sender, EventArgs e)
	{
		if (passwordIndex == 0)
		{
			if (((Control)textBox_Password).Text != "")
			{
				UsbUpgradeFile.SetPassward1(((Control)textBox_Password).Text);
			}
			if (UsbUpgradeFile.isPassward1())
			{
				((Form)this).DialogResult = (DialogResult)1;
			}
			else
			{
				((Control)label_error).Visible = true;
			}
		}
		else if (passwordIndex == 1)
		{
			if (((Control)textBox_Password).Text != "")
			{
				UsbUpgradeFile.SetPassward2(((Control)textBox_Password).Text);
			}
			if (UsbUpgradeFile.isPassward2())
			{
				((Form)this).DialogResult = (DialogResult)1;
			}
			else
			{
				((Control)label_error).Visible = true;
			}
		}
	}

	private void wImageButton_eye_MouseDown(object sender, MouseEventArgs e)
	{
		textBox_Password.PasswordChar = '\0';
	}

	private void wImageButton_eye_MouseUp(object sender, MouseEventArgs e)
	{
		textBox_Password.PasswordChar = '*';
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
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Expected O, but got Unknown
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Expected O, but got Unknown
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Expected O, but got Unknown
		//IL_033f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0349: Expected O, but got Unknown
		//IL_03d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e3: Expected O, but got Unknown
		//IL_03f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fb: Expected O, but got Unknown
		//IL_041e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0428: Expected O, but got Unknown
		//IL_0469: Unknown result type (might be due to invalid IL or missing references)
		//IL_0473: Expected O, but got Unknown
		//IL_048b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0495: Expected O, but got Unknown
		//IL_0536: Unknown result type (might be due to invalid IL or missing references)
		//IL_0540: Expected O, but got Unknown
		//IL_0574: Unknown result type (might be due to invalid IL or missing references)
		//IL_057e: Expected O, but got Unknown
		//IL_05b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bc: Expected O, but got Unknown
		//IL_0929: Unknown result type (might be due to invalid IL or missing references)
		//IL_0933: Expected O, but got Unknown
		//IL_0948: Unknown result type (might be due to invalid IL or missing references)
		//IL_0952: Expected O, but got Unknown
		//IL_0960: Unknown result type (might be due to invalid IL or missing references)
		//IL_0993: Unknown result type (might be due to invalid IL or missing references)
		//IL_099d: Expected O, but got Unknown
		//IL_09a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b0: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FormPasswordInput));
		label1 = new Label();
		label_error = new Label();
		textBox_Password = new TextBox();
		wImageButton_eye = new WindImageButton();
		wibutton_confirm = new WindImageButton();
		wiButton_Close = new WindImageButton();
		((Control)this).SuspendLayout();
		((Control)label1).AutoSize = true;
		((Control)label1).BackColor = Color.Transparent;
		((Control)label1).Font = new Font("微软雅黑", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)label1).Location = new Point(15, 107);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(86, 21);
		((Control)label1).TabIndex = 18;
		((Control)label1).Text = "Password:";
		((Control)label_error).AutoSize = true;
		((Control)label_error).BackColor = Color.Transparent;
		((Control)label_error).Font = new Font("微软雅黑", 15f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)label_error).ForeColor = Color.Red;
		((Control)label_error).Location = new Point(102, 68);
		((Control)label_error).Name = "label_error";
		((Control)label_error).Size = new Size(60, 27);
		((Control)label_error).TabIndex = 37;
		((Control)label_error).Text = "Error";
		((Control)label_error).Visible = false;
		((Control)textBox_Password).Font = new Font("微软雅黑", 14.25f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)textBox_Password).Location = new Point(105, 101);
		((Control)textBox_Password).Name = "textBox_Password";
		textBox_Password.PasswordChar = '*';
		((Control)textBox_Password).Size = new Size(147, 33);
		((Control)textBox_Password).TabIndex = 39;
		((Control)wImageButton_eye).BackColor = Color.Transparent;
		wImageButton_eye.DisableBackColor = Color.Transparent;
		wImageButton_eye.DisableForeColor = Color.DarkGray;
		wImageButton_eye.FrameMode = GraphicsHelper.RoundStyle.All;
		wImageButton_eye.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wImageButton_eye.IconName = "A_fa_eye";
		wImageButton_eye.IconOffset = new Point(0, 0);
		wImageButton_eye.IconSize = 32;
		((Control)wImageButton_eye).Location = new Point(266, 105);
		wImageButton_eye.MouseDownBackColor = Color.Transparent;
		wImageButton_eye.MouseDownForeColor = Color.DimGray;
		wImageButton_eye.MouseEnterBackColor = Color.Transparent;
		wImageButton_eye.MouseEnterForeColor = Color.Gray;
		wImageButton_eye.MouseUpBackColor = Color.Transparent;
		wImageButton_eye.MouseUpForeColor = Color.Black;
		wImageButton_eye.MouseUpImage = (Image)componentResourceManager.GetObject("wImageButton_eye.MouseUpImage");
		((Control)wImageButton_eye).Name = "wImageButton_eye";
		wImageButton_eye.Radius = 12;
		((Control)wImageButton_eye).Size = new Size(24, 24);
		((Control)wImageButton_eye).TabIndex = 40;
		wImageButton_eye.TextAlignment = StringHelper.TextAlignment.Center;
		wImageButton_eye.TextDynOffset = new Point(0, 0);
		wImageButton_eye.TextFixLocation = new Point(0, 0);
		wImageButton_eye.TextFixLocationEnable = false;
		((Control)wImageButton_eye).MouseDown += new MouseEventHandler(wImageButton_eye_MouseDown);
		((Control)wImageButton_eye).MouseUp += new MouseEventHandler(wImageButton_eye_MouseUp);
		((Control)wibutton_confirm).BackColor = Color.Transparent;
		((Control)wibutton_confirm).BackgroundImage = (Image)componentResourceManager.GetObject("wibutton_confirm.BackgroundImage");
		((Control)wibutton_confirm).BackgroundImageLayout = (ImageLayout)3;
		wibutton_confirm.DisableBackColor = Color.Transparent;
		wibutton_confirm.DisableForeColor = Color.DarkGray;
		wibutton_confirm.DisableImage = (Image)componentResourceManager.GetObject("wibutton_confirm.DisableImage");
		((Control)wibutton_confirm).Font = new Font("微软雅黑", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wibutton_confirm.FrameMode = GraphicsHelper.RoundStyle.All;
		wibutton_confirm.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wibutton_confirm.IconName = "";
		wibutton_confirm.IconOffset = new Point(0, 0);
		wibutton_confirm.IconSize = 32;
		((Control)wibutton_confirm).Location = new Point(105, 152);
		wibutton_confirm.MouseDownBackColor = Color.Gray;
		wibutton_confirm.MouseDownForeColor = Color.Black;
		wibutton_confirm.MouseDownImage = (Image)componentResourceManager.GetObject("wibutton_confirm.MouseDownImage");
		wibutton_confirm.MouseEnterBackColor = Color.DarkGray;
		wibutton_confirm.MouseEnterForeColor = Color.Black;
		wibutton_confirm.MouseEnterImage = (Image)componentResourceManager.GetObject("wibutton_confirm.MouseEnterImage");
		wibutton_confirm.MouseUpBackColor = Color.Transparent;
		wibutton_confirm.MouseUpForeColor = Color.Black;
		wibutton_confirm.MouseUpImage = (Image)componentResourceManager.GetObject("wibutton_confirm.MouseUpImage");
		((Control)wibutton_confirm).Name = "wibutton_confirm";
		wibutton_confirm.Radius = 12;
		((Control)wibutton_confirm).Size = new Size(147, 40);
		((Control)wibutton_confirm).TabIndex = 38;
		((Control)wibutton_confirm).Text = "OK";
		wibutton_confirm.TextAlignment = StringHelper.TextAlignment.Center;
		wibutton_confirm.TextDynOffset = new Point(0, 0);
		wibutton_confirm.TextFixLocation = new Point(0, 0);
		wibutton_confirm.TextFixLocationEnable = false;
		((Control)wibutton_confirm).Click += wibutton_confirm_Click;
		((Control)wiButton_Close).BackColor = Color.Transparent;
		((Control)wiButton_Close).BackgroundImage = (Image)(object)Resources.close_2;
		((Control)wiButton_Close).BackgroundImageLayout = (ImageLayout)2;
		wiButton_Close.DisableBackColor = Color.Transparent;
		wiButton_Close.DisableForeColor = Color.OrangeRed;
		wiButton_Close.DisableImage = (Image)(object)Resources.close_3;
		wiButton_Close.FrameMode = GraphicsHelper.RoundStyle.All;
		wiButton_Close.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wiButton_Close.IconName = "";
		wiButton_Close.IconOffset = new Point(0, 0);
		wiButton_Close.IconSize = 32;
		((Control)wiButton_Close).Location = new Point(281, 17);
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
		((Control)wiButton_Close).TabIndex = 1;
		((Control)wiButton_Close).Text = null;
		wiButton_Close.TextAlignment = StringHelper.TextAlignment.Center;
		wiButton_Close.TextDynOffset = new Point(0, 0);
		wiButton_Close.TextFixLocation = new Point(0, 0);
		wiButton_Close.TextFixLocationEnable = false;
		((Control)wiButton_Close).Click += wiButton_Close_Click;
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)0;
		((Control)this).BackgroundImage = (Image)(object)Resources.Message;
		((Form)this).ClientSize = new Size(319, 216);
		((Control)this).Controls.Add((Control)(object)wImageButton_eye);
		((Control)this).Controls.Add((Control)(object)textBox_Password);
		((Control)this).Controls.Add((Control)(object)wibutton_confirm);
		((Control)this).Controls.Add((Control)(object)label_error);
		((Control)this).Controls.Add((Control)(object)label1);
		((Control)this).Controls.Add((Control)(object)wiButton_Close);
		((Control)this).DoubleBuffered = true;
		((Control)this).Font = new Font("微软雅黑", 15f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Form)this).KeyPreview = true;
		((Form)this).Margin = new Padding(6, 7, 6, 7);
		((Control)this).Name = "FormPasswordInput";
		((Form)this).StartPosition = (FormStartPosition)0;
		((Control)this).Text = "Input Password";
		((Form)this).FormClosing += new FormClosingEventHandler(FormPasswordInput_FormClosing);
		((Control)this).KeyPress += new KeyPressEventHandler(FormSetting_KeyPress);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
