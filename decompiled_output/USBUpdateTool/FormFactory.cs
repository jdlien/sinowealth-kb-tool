using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using WindControls;

namespace USBUpdateTool;

public class FormFactory : Form
{
	public SkinForm skinForm = new SkinForm(_movable: true);

	private SaveFileDialog saveFileDialog = new SaveFileDialog();

	private IContainer components = null;

	private WindImageButton wibutton_binFile;

	private WindImageButton wiButton_close;

	private TextBox textBox_fileName;

	public FormFactory(FormMain formMain)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		InitializeComponent();
		((Control)this).Width = ((Control)formMain).Width;
		((Control)this).Height = ((Control)formMain).Height;
		((Form)this).Icon = ((Form)formMain).Icon;
		((Control)this).BackgroundImage = ((Control)formMain).BackgroundImage;
		((Control)textBox_fileName).Text = LiteResources.appConfig.fileConfig.fileName;
		((FileDialog)saveFileDialog).Filter = "Bin File|*.bin";
		((FileDialog)saveFileDialog).FilterIndex = 1;
		((FileDialog)saveFileDialog).RestoreDirectory = true;
		skinForm.InitSkin((Form)(object)this, 12, 16);
	}

	private void wibutton_binFile_Click(object sender, EventArgs e)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Invalid comparison between Unknown and I4
		if (LiteResources.appConfig.fileConfig.fileArray != null && (int)((CommonDialog)saveFileDialog).ShowDialog() == 1)
		{
			FileStream fileStream = new FileStream(((FileDialog)saveFileDialog).FileName, FileMode.Create);
			fileStream.Write(LiteResources.appConfig.fileConfig.fileArray, 0, LiteResources.appConfig.fileConfig.fileArray.Length);
			fileStream.Close();
		}
	}

	private void wiButton_close_Click(object sender, EventArgs e)
	{
		((Form)this).Close();
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
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Expected O, but got Unknown
		//IL_025c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0266: Expected O, but got Unknown
		//IL_04be: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c8: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FormFactory));
		wibutton_binFile = new WindImageButton();
		wiButton_close = new WindImageButton();
		textBox_fileName = new TextBox();
		((Control)this).SuspendLayout();
		((Control)wibutton_binFile).BackColor = Color.Transparent;
		wibutton_binFile.DisableBackColor = Color.Transparent;
		wibutton_binFile.DisableForeColor = Color.DarkGray;
		((Control)wibutton_binFile).Font = new Font("微软雅黑", 14.25f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wibutton_binFile.FrameMode = GraphicsHelper.RoundStyle.All;
		wibutton_binFile.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wibutton_binFile.IconName = "";
		wibutton_binFile.IconOffset = new Point(0, 0);
		wibutton_binFile.IconSize = 32;
		((Control)wibutton_binFile).Location = new Point(182, 250);
		wibutton_binFile.MouseDownBackColor = Color.MediumSeaGreen;
		wibutton_binFile.MouseDownForeColor = Color.Black;
		wibutton_binFile.MouseEnterBackColor = Color.LightSeaGreen;
		wibutton_binFile.MouseEnterForeColor = Color.Black;
		wibutton_binFile.MouseUpBackColor = Color.LightSeaGreen;
		wibutton_binFile.MouseUpForeColor = Color.Black;
		((Control)wibutton_binFile).Name = "wibutton_binFile";
		wibutton_binFile.Radius = 16;
		((Control)wibutton_binFile).Size = new Size(173, 48);
		((Control)wibutton_binFile).TabIndex = 95;
		((Control)wibutton_binFile).Text = "析出Bin文件";
		wibutton_binFile.TextAlignment = StringHelper.TextAlignment.Center;
		wibutton_binFile.TextDynOffset = new Point(1, 1);
		wibutton_binFile.TextFixLocation = new Point(0, 0);
		wibutton_binFile.TextFixLocationEnable = false;
		((Control)wibutton_binFile).Click += wibutton_binFile_Click;
		((Control)wiButton_close).BackColor = Color.Transparent;
		wiButton_close.DisableBackColor = Color.Transparent;
		wiButton_close.DisableForeColor = Color.DarkGray;
		((Control)wiButton_close).Font = new Font("微软雅黑", 24f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wiButton_close.FrameMode = GraphicsHelper.RoundStyle.All;
		wiButton_close.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wiButton_close.IconName = "";
		wiButton_close.IconOffset = new Point(0, 0);
		wiButton_close.IconSize = 32;
		((Control)wiButton_close).Location = new Point(529, 26);
		wiButton_close.MouseDownBackColor = Color.Transparent;
		wiButton_close.MouseDownForeColor = Color.Black;
		wiButton_close.MouseEnterBackColor = Color.Transparent;
		wiButton_close.MouseEnterForeColor = Color.Black;
		wiButton_close.MouseUpBackColor = Color.Transparent;
		wiButton_close.MouseUpForeColor = Color.Black;
		((Control)wiButton_close).Name = "wiButton_close";
		wiButton_close.Radius = 0;
		((Control)wiButton_close).Size = new Size(24, 24);
		((Control)wiButton_close).TabIndex = 96;
		((Control)wiButton_close).Text = "×";
		wiButton_close.TextAlignment = StringHelper.TextAlignment.Center;
		wiButton_close.TextDynOffset = new Point(1, 1);
		wiButton_close.TextFixLocation = new Point(0, 0);
		wiButton_close.TextFixLocationEnable = false;
		((Control)wiButton_close).Click += wiButton_close_Click;
		((Control)textBox_fileName).Location = new Point(26, 61);
		((TextBoxBase)textBox_fileName).Multiline = true;
		((Control)textBox_fileName).Name = "textBox_fileName";
		((Control)textBox_fileName).Size = new Size(539, 137);
		((Control)textBox_fileName).TabIndex = 97;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 12f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(600, 340);
		((Control)this).Controls.Add((Control)(object)textBox_fileName);
		((Control)this).Controls.Add((Control)(object)wiButton_close);
		((Control)this).Controls.Add((Control)(object)wibutton_binFile);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Control)this).Name = "FormFactory";
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "FormFactory";
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
