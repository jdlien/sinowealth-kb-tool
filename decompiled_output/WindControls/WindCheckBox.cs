using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace WindControls;

public class WindCheckBox : WindSelectBox
{
	private Rectangle m_IconBackColorRect = new Rectangle(0, 0, 0, 0);

	[Category("自定义")]
	[Description("自定义：背景色透明部分区域")]
	public Rectangle IconBackColorRect
	{
		get
		{
			return m_IconBackColorRect;
		}
		set
		{
			if (m_IconBackColorRect != value)
			{
				m_IconBackColorRect = value;
				((Control)this).Invalidate();
			}
		}
	}

	private void InitializeComponent()
	{
		((Control)this).SuspendLayout();
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 12f);
		((Control)this).AutoSize = false;
		((Control)this).Name = "WindCheckBox";
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
