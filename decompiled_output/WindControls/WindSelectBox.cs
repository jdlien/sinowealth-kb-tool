using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace WindControls;

[DefaultEvent("CheckedChanged")]
public class WindSelectBox : UserControl
{
	private string m_Text = "选择框";

	private int m_IconSize = 36;

	private string m_SelectedIconName = "A_fa_check_square_o";

	private Color m_SelectedIconColor = Color.Red;

	private string m_UnSelectedIconName = "A_fa_square_o";

	private Color m_UnSelectedIconColor = Color.Gray;

	private Image m_SelectedImage = null;

	private Image m_UnSelectedImage = null;

	private Image m_DisableSelectedImage = null;

	private Image m_DisableUnSelectedImage = null;

	private bool m_Checked = false;

	private Point m_IconOffset = new Point(0, 0);

	private Point m_TextOffset = new Point(0, -2);

	private Point imagePoint = default(Point);

	private IContainer components = null;

	[Category("自定义")]
	[Description("显示文本")]
	[Bindable(true)]
	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	[EditorBrowsable(EditorBrowsableState.Always)]
	public override string Text
	{
		get
		{
			return m_Text;
		}
		set
		{
			m_Text = value;
		}
	}

	[Category("自定义属性")]
	[Description("图标大小")]
	public int IconSize
	{
		get
		{
			return m_IconSize;
		}
		set
		{
			m_IconSize = value;
		}
	}

	[Category("自定义属性")]
	[Description("选中的图标名字：在http://www.fontawesome.com.cn/faicons/中搜索")]
	public virtual string SelectedIconName
	{
		get
		{
			return m_SelectedIconName;
		}
		set
		{
			m_SelectedIconName = value;
		}
	}

	[Category("自定义属性")]
	[Description("选中的图标颜色")]
	public Color SelectedIconColor
	{
		get
		{
			return m_SelectedIconColor;
		}
		set
		{
			m_SelectedIconColor = value;
		}
	}

	[Category("自定义属性")]
	[Description("未选中的图标名字：在http://www.fontawesome.com.cn/faicons/中搜索")]
	public virtual string UnSelectedIconName
	{
		get
		{
			return m_UnSelectedIconName;
		}
		set
		{
			m_UnSelectedIconName = value;
		}
	}

	[Category("自定义属性")]
	[Description("未选中的图标颜色")]
	public Color UnSelectedIconColor
	{
		get
		{
			return m_UnSelectedIconColor;
		}
		set
		{
			m_UnSelectedIconColor = value;
		}
	}

	[Category("自定义属性")]
	[Description("选中的图标")]
	public Image SelectedImage
	{
		get
		{
			return m_SelectedImage;
		}
		set
		{
			m_SelectedImage = value;
		}
	}

	[Category("自定义属性")]
	[Description("未选中的图标")]
	public Image UnSelectedImage
	{
		get
		{
			return m_UnSelectedImage;
		}
		set
		{
			m_UnSelectedImage = value;
		}
	}

	[Category("自定义属性")]
	[Description("失效时选中图标")]
	public Image DisableSelectedImage
	{
		get
		{
			return m_DisableSelectedImage;
		}
		set
		{
			m_DisableSelectedImage = value;
		}
	}

	[Category("自定义属性")]
	[Description("失效时未选中图标")]
	public Image DisableUnSelectedImage
	{
		get
		{
			return m_DisableUnSelectedImage;
		}
		set
		{
			m_DisableUnSelectedImage = value;
		}
	}

	[Description("是否选中")]
	[Category("自定义")]
	public bool Checked
	{
		get
		{
			return m_Checked;
		}
		set
		{
			if (m_Checked != value)
			{
				m_Checked = value;
				if (this.CheckedChanged != null)
				{
					this.CheckedChanged(this, null);
				}
				((Control)this).Invalidate();
			}
		}
	}

	public bool Enabled
	{
		get
		{
			return ((Control)this).Enabled;
		}
		set
		{
			((Control)this).Enabled = value;
		}
	}

	[Category("自定义")]
	[Description("自定义：图像坐标偏移")]
	public Point IconOffset
	{
		get
		{
			return m_IconOffset;
		}
		set
		{
			m_IconOffset = value;
		}
	}

	[Category("自定义")]
	[Description("自定义：图像坐标偏移")]
	public Point TextOffset
	{
		get
		{
			return m_TextOffset;
		}
		set
		{
			m_TextOffset = value;
		}
	}

	[Category("自定义")]
	[Description("选中改变事件")]
	public event EventHandler CheckedChanged;

	public WindSelectBox()
	{
		InitializeComponent();
		SetStyles();
		((Control)this).EnabledChanged += WindSelectBox_EnabledChanged;
	}

	private void SetStyles()
	{
		((Control)this).SetStyle((ControlStyles)204818, true);
		((Control)this).UpdateStyles();
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)0;
	}

	private void WindSelectBox_EnabledChanged(object sender, EventArgs e)
	{
	}

	protected override void OnCreateControl()
	{
		CreateImage();
	}

	protected virtual void CreateImage()
	{
		if (SelectedIconName != "")
		{
			SelectedImage = (Image)(object)FontImages.GetImage(SelectedIconName, IconSize, SelectedIconColor, Color.Transparent);
			DisableSelectedImage = (Image)(object)FontImages.GetImage(SelectedIconName, IconSize, Color.DarkGray, Color.Transparent);
		}
		if (UnSelectedIconName != "")
		{
			UnSelectedImage = (Image)(object)FontImages.GetImage(UnSelectedIconName, IconSize, UnSelectedIconColor, Color.Transparent);
			DisableUnSelectedImage = (Image)(object)FontImages.GetImage(UnSelectedIconName, IconSize, Color.DarkGray, Color.Transparent);
		}
	}

	private void DrawSelectBox(Graphics g, Image image)
	{
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Expected O, but got Unknown
		if (image != null)
		{
			imagePoint = new Point(IconOffset.X + 4, IconOffset.Y + (((Control)this).Height - image.Height) / 2);
			g.DrawImage(image, imagePoint.X, imagePoint.Y, image.Width, image.Height);
			SizeF sizeF = g.MeasureString(((Control)this).Text, ((Control)this).Font);
			Point point = new Point(TextOffset.X + imagePoint.X + image.Width + 4, TextOffset.Y + (((Control)this).Height - (int)sizeF.Height) / 2);
			g.DrawString(((Control)this).Text, ((Control)this).Font, (Brush)new SolidBrush(((Control)this).ForeColor), (PointF)point);
		}
	}

	private void WindSelectBox_Paint(object sender, PaintEventArgs e)
	{
		Graphics graphics = e.Graphics;
		graphics.SetGDIHigh();
		if (Enabled)
		{
			if (Checked)
			{
				DrawSelectBox(graphics, SelectedImage);
			}
			else
			{
				DrawSelectBox(graphics, UnSelectedImage);
			}
		}
		else if (Checked)
		{
			DrawSelectBox(graphics, DisableSelectedImage);
		}
		else
		{
			DrawSelectBox(graphics, DisableUnSelectedImage);
		}
	}

	public virtual void WindSelectBox_Click(object sender, EventArgs e)
	{
		if (Enabled)
		{
			Checked = !Checked;
			((Control)this).Invalidate();
		}
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
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Expected O, but got Unknown
		((Control)this).SuspendLayout();
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)0;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).BackgroundImageLayout = (ImageLayout)0;
		((Control)this).Name = "WindSelectBox";
		((Control)this).Size = new Size(117, 39);
		((Control)this).Click += WindSelectBox_Click;
		((Control)this).Paint += new PaintEventHandler(WindSelectBox_Paint);
		((Control)this).ResumeLayout(false);
	}
}
