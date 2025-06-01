using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace WindControls;

public class SwitchButton : UserControl
{
	private Image m_OnImage = null;

	private Image m_OffImage = null;

	private string m_OnText = "";

	private string m_OffText = "";

	private Point m_textPoint = new Point(0, 0);

	private IContainer components = null;

	private PictureBox pictureBox1;

	private Label label_text;

	[DefaultValue(null)]
	[Localizable(true)]
	[Category("自定义")]
	[Description("自定义：打开时的图片")]
	public Image OnImage
	{
		get
		{
			return m_OnImage;
		}
		set
		{
			m_OnImage = value;
			SetStatus(SwitchButtonStatus.ON, Color.Tomato);
			((Control)this).Invalidate();
		}
	}

	[DefaultValue(null)]
	[Localizable(true)]
	[Category("自定义")]
	[Description("自定义：关闭时的图片")]
	public Image OffImage
	{
		get
		{
			return m_OffImage;
		}
		set
		{
			m_OffImage = value;
			SetStatus(SwitchButtonStatus.OFF, Color.White);
			((Control)this).Invalidate();
		}
	}

	public string OnText
	{
		get
		{
			return m_OnText;
		}
		set
		{
			m_OnText = value;
		}
	}

	public string OffText
	{
		get
		{
			return m_OffText;
		}
		set
		{
			m_OffText = value;
		}
	}

	[DefaultValue(null)]
	[Localizable(true)]
	[Category("自定义")]
	[Description("自定义：文本偏移")]
	public Point TextPoint
	{
		get
		{
			return m_textPoint;
		}
		set
		{
			m_textPoint = value;
			((Control)this).Invalidate();
		}
	}

	public SwitchButton()
	{
		InitializeComponent();
	}

	protected override void OnCreateControl()
	{
		if (((Control)this).Enabled)
		{
			if (GetSwitchButtonStatus() == SwitchButtonStatus.ON)
			{
				SetStatus(SwitchButtonStatus.ON, Color.Tomato);
			}
			else
			{
				SetStatus(SwitchButtonStatus.OFF, Color.White);
			}
		}
	}

	public void SetStatus(SwitchButtonStatus buttonStatus, Color textColor)
	{
		switch (buttonStatus)
		{
		case SwitchButtonStatus.Disable:
			SetStatus(SwitchButtonStatus.OFF, textColor);
			((Control)this).Enabled = false;
			((Control)label_text).Enabled = false;
			((Control)label_text).ForeColor = Color.Gray;
			break;
		case SwitchButtonStatus.ON:
			((Control)this).Enabled = true;
			((Control)label_text).Enabled = true;
			if (OnImage != null)
			{
				pictureBox1.Image = OnImage;
				((Control)pictureBox1).Width = OnImage.Width;
				((Control)pictureBox1).Height = OnImage.Height;
				((Control)label_text).Location = new Point(((Control)pictureBox1).Location.X + ((Control)pictureBox1).Width + TextPoint.X, TextPoint.Y);
			}
			((Control)label_text).Text = OnText;
			((Control)label_text).ForeColor = textColor;
			break;
		case SwitchButtonStatus.OFF:
			((Control)this).Enabled = true;
			((Control)label_text).Enabled = true;
			if (OffImage != null)
			{
				pictureBox1.Image = OffImage;
				((Control)pictureBox1).Width = OffImage.Width;
				((Control)pictureBox1).Height = OffImage.Height;
			}
			((Control)label_text).Text = OffText;
			((Control)label_text).ForeColor = textColor;
			((Control)label_text).Location = new Point(((Control)pictureBox1).Location.X + ((Control)pictureBox1).Width + TextPoint.X, TextPoint.Y);
			break;
		}
	}

	private void pictureBox1_Click(object sender, EventArgs e)
	{
		((Control)this).OnClick(e);
	}

	public SwitchButtonStatus GetSwitchButtonStatus()
	{
		if (pictureBox1.Image == OnImage)
		{
			return SwitchButtonStatus.ON;
		}
		return SwitchButtonStatus.OFF;
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
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		label_text = new Label();
		pictureBox1 = new PictureBox();
		((ISupportInitialize)pictureBox1).BeginInit();
		((Control)this).SuspendLayout();
		((Control)label_text).AutoSize = true;
		((Control)label_text).Font = new Font("微软雅黑", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)label_text).Location = new Point(49, 6);
		((Control)label_text).Name = "label_text";
		((Control)label_text).Size = new Size(55, 21);
		((Control)label_text).TabIndex = 1;
		((Control)label_text).Text = "label1";
		((Control)pictureBox1).Location = new Point(2, 2);
		((Control)pictureBox1).Name = "pictureBox1";
		((Control)pictureBox1).Size = new Size(41, 27);
		pictureBox1.TabIndex = 0;
		pictureBox1.TabStop = false;
		((Control)pictureBox1).Click += pictureBox1_Click;
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)0;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Controls.Add((Control)(object)label_text);
		((Control)this).Controls.Add((Control)(object)pictureBox1);
		((Control)this).Name = "SwitchButton";
		((Control)this).Size = new Size(161, 34);
		((ISupportInitialize)pictureBox1).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
