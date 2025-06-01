using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using USBUpdateTool.Properties;
using WindControls;

namespace USBUpdateTool;

public class FormSetting : Form
{
	private SkinForm skinForm = new SkinForm(_movable: false);

	private IContainer components = null;

	private Label label1;

	private Label label2;

	private LinkLabel linkLabel1;

	public WindImageButton wiButton_Close;

	private Label label3;

	public FormSetting()
	{
		InitializeComponent();
		SetStyles();
		skinForm.InitSkin((Form)(object)this, 12, 16);
	}

	private void SetStyles()
	{
		((Control)this).SetStyle((ControlStyles)204818, true);
		((Control)this).UpdateStyles();
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)0;
	}

	private void wiButton_Close_Click(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	public DialogResult ShowDialog(Form Owner)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		((Form)skinForm).Owner = Owner;
		return ((Form)this).ShowDialog();
	}

	private void FormSetting_KeyPress(object sender, KeyPressEventArgs e)
	{
		if (e.KeyChar == '\u001b')
		{
			((Form)this).DialogResult = (DialogResult)1;
		}
	}

	private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		Process.Start(((Control)linkLabel1).Text);
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
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Expected O, but got Unknown
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Expected O, but got Unknown
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Expected O, but got Unknown
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Expected O, but got Unknown
		//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ed: Expected O, but got Unknown
		//IL_0602: Unknown result type (might be due to invalid IL or missing references)
		//IL_060c: Expected O, but got Unknown
		//IL_061a: Unknown result type (might be due to invalid IL or missing references)
		//IL_064d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0657: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FormSetting));
		label1 = new Label();
		label2 = new Label();
		linkLabel1 = new LinkLabel();
		label3 = new Label();
		wiButton_Close = new WindImageButton();
		((Control)this).SuspendLayout();
		((Control)label1).AutoSize = true;
		((Control)label1).BackColor = Color.Transparent;
		((Control)label1).Font = new Font("微软雅黑", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)label1).ForeColor = Color.Black;
		((Control)label1).Location = new Point(29, 53);
		((Control)label1).Margin = new Padding(6, 0, 6, 0);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(129, 21);
		((Control)label1).TabIndex = 3;
		((Control)label1).Text = "Ver:          v6.13";
		((Control)label2).AutoSize = true;
		((Control)label2).BackColor = Color.Transparent;
		((Control)label2).Font = new Font("微软雅黑", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)label2).ForeColor = Color.Black;
		((Control)label2).Location = new Point(29, 84);
		((Control)label2).Margin = new Padding(6, 0, 6, 0);
		((Control)label2).Name = "label2";
		((Control)label2).Size = new Size(176, 21);
		((Control)label2).TabIndex = 4;
		((Control)label2).Text = "Data:        2024/10/17";
		((Control)linkLabel1).AutoSize = true;
		((Control)linkLabel1).BackColor = Color.Transparent;
		((Control)linkLabel1).Location = new Point(30, 167);
		((Control)linkLabel1).Name = "linkLabel1";
		((Control)linkLabel1).Size = new Size(275, 27);
		((Control)linkLabel1).TabIndex = 6;
		linkLabel1.TabStop = true;
		((Control)linkLabel1).Text = "http://www.compx.com.cn/";
		linkLabel1.LinkClicked += new LinkLabelLinkClickedEventHandler(linkLabel1_LinkClicked);
		((Control)label3).AutoSize = true;
		((Control)label3).BackColor = Color.Transparent;
		((Control)label3).Font = new Font("微软雅黑", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)label3).ForeColor = Color.Black;
		((Control)label3).Location = new Point(30, 138);
		((Control)label3).Margin = new Padding(6, 0, 6, 0);
		((Control)label3).Name = "label3";
		((Control)label3).Size = new Size(125, 21);
		((Control)label3).TabIndex = 8;
		((Control)label3).Text = "© Compx Tech";
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
		((Control)wiButton_Close).Location = new Point(279, 18);
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
		((Control)wiButton_Close).TabIndex = 7;
		((Control)wiButton_Close).Text = null;
		wiButton_Close.TextAlignment = StringHelper.TextAlignment.Center;
		wiButton_Close.TextDynOffset = new Point(0, 0);
		wiButton_Close.TextFixLocation = new Point(0, 0);
		wiButton_Close.TextFixLocationEnable = false;
		((Control)wiButton_Close).Click += wiButton_Close_Click;
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)0;
		((Control)this).BackgroundImage = (Image)(object)Resources.Message;
		((Form)this).ClientSize = new Size(315, 215);
		((Control)this).Controls.Add((Control)(object)label3);
		((Control)this).Controls.Add((Control)(object)wiButton_Close);
		((Control)this).Controls.Add((Control)(object)linkLabel1);
		((Control)this).Controls.Add((Control)(object)label2);
		((Control)this).Controls.Add((Control)(object)label1);
		((Control)this).DoubleBuffered = true;
		((Control)this).Font = new Font("微软雅黑", 15f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Form)this).KeyPreview = true;
		((Form)this).Margin = new Padding(6, 7, 6, 7);
		((Control)this).Name = "FormSetting";
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "About";
		((Control)this).KeyPress += new KeyPressEventHandler(FormSetting_KeyPress);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
