using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace WindControls;

public class FormMessageItem : UserControl
{
	public TextBox TextBox_Msg;

	private IContainer components = null;

	private TextBox textBox_Msg;

	public FormMessageItem()
	{
		InitializeComponent();
		TextBox_Msg = textBox_Msg;
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
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		textBox_Msg = new TextBox();
		((Control)this).SuspendLayout();
		((Control)textBox_Msg).BackColor = SystemColors.Window;
		((TextBoxBase)textBox_Msg).BorderStyle = (BorderStyle)0;
		((Control)textBox_Msg).Font = new Font("微软雅黑", 21.75f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)textBox_Msg).Location = new Point(3, 3);
		((TextBoxBase)textBox_Msg).Multiline = true;
		((Control)textBox_Msg).Name = "textBox_Msg";
		((TextBoxBase)textBox_Msg).ReadOnly = true;
		textBox_Msg.ScrollBars = (ScrollBars)2;
		((Control)textBox_Msg).Size = new Size(726, 344);
		((Control)textBox_Msg).TabIndex = 10;
		((Control)textBox_Msg).TabStop = false;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 12f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)textBox_Msg);
		((Control)this).Name = "FormMessageItem";
		((Control)this).Size = new Size(729, 350);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
