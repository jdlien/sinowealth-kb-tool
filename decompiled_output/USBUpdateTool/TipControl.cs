using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using USBUpdateTool.Properties;
using WindControls;

namespace USBUpdateTool;

public class TipControl : UserControl
{
	private Color m_TextForeColor = Color.Black;

	private IContainer components = null;

	private Label label1;

	[EditorBrowsable(EditorBrowsableState.Always)]
	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	[Bindable(true)]
	public override string Text
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

	[Category("自定义")]
	[Description("自定义：正常状态时的前景色")]
	public Color TextForeColor
	{
		get
		{
			return ((Control)label1).ForeColor;
		}
		set
		{
			((Control)label1).ForeColor = value;
			((Control)this).Invalidate();
		}
	}

	protected override CreateParams CreateParams
	{
		get
		{
			CreateParams createParams = ((UserControl)this).CreateParams;
			createParams.ExStyle |= 0x20;
			return createParams;
		}
	}

	public TipControl()
	{
		InitializeComponent();
		ImageHelper.ControlTrans((Control)(object)this, ((Control)this).BackgroundImage);
	}

	private void TipControl_Paint(object sender, PaintEventArgs e)
	{
		e.Graphics.DrawImage(((Control)this).BackgroundImage, new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height));
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
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Expected O, but got Unknown
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Expected O, but got Unknown
		label1 = new Label();
		((Control)this).SuspendLayout();
		((Control)label1).AutoSize = true;
		((Control)label1).BackColor = Color.Transparent;
		((Control)label1).Font = new Font("微软雅黑", 15f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)label1).Location = new Point(28, 53);
		((Control)label1).Margin = new Padding(4, 0, 4, 0);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(51, 27);
		((Control)label1).TabIndex = 0;
		((Control)label1).Text = "Text";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).BackgroundImage = (Image)(object)Resources.tips;
		((Control)this).BackgroundImageLayout = (ImageLayout)3;
		((Control)this).Controls.Add((Control)(object)label1);
		((Control)this).DoubleBuffered = true;
		((Control)this).Font = new Font("微软雅黑", 10.5f);
		((Control)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "TipControl";
		((Control)this).Size = new Size(182, 89);
		((Control)this).Paint += new PaintEventHandler(TipControl_Paint);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
