using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using USBUpdateTool;
using USBUpdateTool.Properties;

namespace WindControls;

public class FormMessageBox : Form
{
	private SkinForm skinForm = new SkinForm(_movable: true);

	private IContainer components = null;

	private Label label1;

	public WindImageButton wibutton_LoadFile;

	public FormMessageBox(string text)
	{
		InitializeComponent();
		skinForm.InitSkin((Form)(object)this, 12, 16);
		((Control)label1).Text = text;
	}

	public FormMessageBox(string text, Icon icon)
	{
		InitializeComponent();
		((Form)this).Icon = icon;
		skinForm.InitSkin((Form)(object)this, 12, 16);
		((Control)label1).Text = text;
	}

	private void FormMessageBox_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 13)
		{
			wTButton_OK_Click(null, null);
		}
	}

	private void wTButton_OK_Click(object sender, EventArgs e)
	{
		((Form)skinForm).Close();
		((Form)this).DialogResult = (DialogResult)1;
	}

	private void FormMessageBox_Load(object sender, EventArgs e)
	{
	}

	public void Show(Form form)
	{
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		if (form != null)
		{
			int x = form.Location.X + (((Control)form).Width - ((Control)this).Width) / 2;
			int y = form.Location.Y + (((Control)form).Height - ((Control)this).Height) / 2;
			((Form)this).Location = new Point(x, y);
			((Form)this).Icon = form.Icon;
			((Form)skinForm).Icon = form.Icon;
		}
		else
		{
			((Form)this).Icon = LiteResources.GetIcon();
			((Form)skinForm).Icon = ((Form)this).Icon;
			((Form)this).StartPosition = (FormStartPosition)4;
		}
		((Form)skinForm).TopMost = true;
		((Form)this).ShowDialog();
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
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Expected O, but got Unknown
		//IL_035b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0365: Expected O, but got Unknown
		//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b7: Expected O, but got Unknown
		label1 = new Label();
		wibutton_LoadFile = new WindImageButton();
		((Control)this).SuspendLayout();
		((Control)label1).BackColor = Color.Transparent;
		((Control)label1).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)label1).Location = new Point(34, 75);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(248, 77);
		((Control)label1).TabIndex = 38;
		((Control)label1).Text = "确定删除该产品吗？";
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
		((Control)wibutton_LoadFile).Location = new Point(95, 155);
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
		((Control)wibutton_LoadFile).TabIndex = 40;
		((Control)wibutton_LoadFile).Text = "OK";
		wibutton_LoadFile.TextAlignment = StringHelper.TextAlignment.Center;
		wibutton_LoadFile.TextDynOffset = new Point(0, 0);
		wibutton_LoadFile.TextFixLocation = new Point(0, 0);
		wibutton_LoadFile.TextFixLocationEnable = false;
		((Control)wibutton_LoadFile).Click += wTButton_OK_Click;
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)0;
		((Control)this).AutoSize = true;
		((Control)this).BackgroundImage = (Image)(object)Resources.Message;
		((Control)this).BackgroundImageLayout = (ImageLayout)3;
		((Form)this).ClientSize = new Size(320, 220);
		((Control)this).Controls.Add((Control)(object)wibutton_LoadFile);
		((Control)this).Controls.Add((Control)(object)label1);
		((Control)this).DoubleBuffered = true;
		((Control)this).Font = new Font("微软雅黑", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Form)this).KeyPreview = true;
		((Control)this).Name = "FormMessageBox";
		((Form)this).StartPosition = (FormStartPosition)0;
		((Form)this).TopMost = true;
		((Form)this).Load += FormMessageBox_Load;
		((Control)this).KeyDown += new KeyEventHandler(FormMessageBox_KeyDown);
		((Control)this).ResumeLayout(false);
	}
}
