using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace WindControls;

public class FormInputBoxItem : UserControl
{
	private IContainer components = null;

	private Label label1;

	private WindTextBox windTextBox1;

	[Description("自定义：文本")]
	[Category("自定义")]
	public override string Text
	{
		get
		{
			return ((Control)windTextBox1).Text;
		}
		set
		{
			((Control)windTextBox1).Text = value;
		}
	}

	[Description("自定义：文本")]
	[Category("自定义")]
	public string Title
	{
		get
		{
			return ((Control)label1).Text;
		}
		set
		{
			((Control)label1).Text = value;
		}
	}

	public FormInputBoxItem()
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
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Expected O, but got Unknown
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		label1 = new Label();
		windTextBox1 = new WindTextBox();
		((Control)this).SuspendLayout();
		((Control)label1).AutoSize = true;
		((Control)label1).Font = new Font("微软雅黑", 21.75f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)label1).Location = new Point(3, 17);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(75, 38);
		((Control)label1).TabIndex = 0;
		((Control)label1).Text = "提示";
		((Control)windTextBox1).BackColor = Color.Transparent;
		((Control)windTextBox1).Font = new Font("微软雅黑", 21.75f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)windTextBox1).Location = new Point(82, 9);
		((Control)windTextBox1).Name = "windTextBox1";
		((Control)windTextBox1).Padding = new Padding(4, 8, 4, 4);
		windTextBox1.PromptText = "";
		((Control)windTextBox1).Size = new Size(194, 52);
		((Control)windTextBox1).TabIndex = 1;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 12f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = SystemColors.Window;
		((Control)this).Controls.Add((Control)(object)windTextBox1);
		((Control)this).Controls.Add((Control)(object)label1);
		((Control)this).Name = "FormInputBoxItem";
		((Control)this).Size = new Size(281, 66);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
