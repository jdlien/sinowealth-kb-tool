using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using USBUpdateTool.Properties;
using WindControls;

namespace USBUpdateTool;

public class Form4KPairBack : Form
{
	private const int iWidth = 48;

	private const int iHeight = 48;

	private int ImageWidth = 0;

	private int ImageHeight = 0;

	private Point BackLocation = default(Point);

	public SkinForm skinForm = new SkinForm(_movable: false);

	private IContainer components = null;

	public Form4KPairBack()
	{
		InitializeComponent();
		SetStyles();
		ImageWidth = ((Control)this).Width;
		ImageHeight = ((Control)this).Height;
		BackLocation.X = (((Control)this).Width - 48) / 2 - 20;
		BackLocation.Y = (((Control)this).Height - 48) / 2 - 20;
		skinForm.InitSkin((Form)(object)this, (((Control)this).Width - 48) / 2, (((Control)this).Height - 48) / 2, 16);
	}

	private void SetStyles()
	{
		((Control)this).SetStyle((ControlStyles)204818, true);
		((Control)this).UpdateStyles();
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)0;
	}

	public void Show(Form form)
	{
		((Form)this).Icon = form.Icon;
		SetLoction(form.Location);
		((Control)this).Show();
	}

	public void SetLoction(Point point)
	{
		point.X += BackLocation.X;
		point.Y += BackLocation.Y;
		((Form)this).Location = point;
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
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(Form4KPairBack));
		((Control)this).SuspendLayout();
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 12f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackgroundImage = (Image)(object)Resources.模糊背景小;
		((Form)this).ClientSize = new Size(618, 356);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Control)this).Name = "Form4KPairBack";
		((Form)this).StartPosition = (FormStartPosition)0;
		((Control)this).ResumeLayout(false);
	}
}
