using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace WindControls;

public class SplitLine : UserControl
{
	private IContainer components = null;

	public SplitLine()
	{
		InitializeComponent();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		((Control)this).SuspendLayout();
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 12f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Name = "SplitLine";
		((Control)this).Size = new Size(100, 2);
		((Control)this).ResumeLayout(false);
	}
}
