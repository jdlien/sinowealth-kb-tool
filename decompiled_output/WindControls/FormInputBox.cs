using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace WindControls;

public class FormInputBox : MsgBaseForm
{
	private IContainer components = null;

	private FormInputBoxItem formInputBoxItem1;

	private FormInputBoxItem formInputBoxItem2;

	public FormInputBox(string title, string text, int width, int height, int buttonHeight)
	{
		InitializeComponent();
		SetTitle(title);
		ResizeWindow(width, height, buttonHeight);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		formInputBoxItem1 = new FormInputBoxItem();
		formInputBoxItem2 = new FormInputBoxItem();
		((Control)this).SuspendLayout();
		((Control)formInputBoxItem1).BackColor = SystemColors.Window;
		((Control)formInputBoxItem1).Location = new Point(25, 86);
		((Control)formInputBoxItem1).Name = "formInputBoxItem1";
		((Control)formInputBoxItem1).Size = new Size(281, 66);
		((Control)formInputBoxItem1).TabIndex = 8;
		((Control)formInputBoxItem2).BackColor = SystemColors.Window;
		((Control)formInputBoxItem2).Location = new Point(25, 158);
		((Control)formInputBoxItem2).Name = "formInputBoxItem2";
		((Control)formInputBoxItem2).Size = new Size(281, 66);
		((Control)formInputBoxItem2).TabIndex = 9;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 12f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(813, 583);
		((Control)this).Controls.Add((Control)(object)formInputBoxItem2);
		((Control)this).Controls.Add((Control)(object)formInputBoxItem1);
		((Control)this).Name = "FormInputBox";
		((Control)this).Text = "FormInputBox";
		((Control)this).Controls.SetChildIndex((Control)(object)formInputBoxItem1, 0);
		((Control)this).Controls.SetChildIndex((Control)(object)formInputBoxItem2, 0);
		((Control)this).ResumeLayout(false);
	}
}
