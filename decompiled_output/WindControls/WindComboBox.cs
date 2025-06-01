using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WindControls;

[DefaultEvent("SelectedIndexChanged")]
public class WindComboBox : UserControl
{
	public WindDropDownBox windDropDownBox;

	private List<string> ItemList = new List<string>();

	private int m_DropDownMaxRowCount = 4;

	private Point m_dropDownLoctionPoint = default(Point);

	private int m_DropDownWidthDelta = 0;

	private int m_IconSize = 36;

	private Color m_frameColor = Color.Gray;

	private Color m_SplitLineColor = Color.LightGray;

	private int m_frameWidth = 1;

	private string m_IconNormalName = "A_fa_caret_down";

	private Color m_IconNormalColor = Color.DimGray;

	private string m_IconMouseEnterName = "A_fa_caret_down";

	private Color m_IconMouseEnterColor = Color.DarkGray;

	private string m_IconMouseDownName = "A_fa_caret_down";

	private Color m_IconMouseDownColor = Color.Gray;

	private int m_SelectedIndex = -1;

	private bool m_readOnly = true;

	private Image m_NormalImage = null;

	private Image m_MouseEnterImage = null;

	private Image m_MouseDownImage = null;

	private string m_SelectedText = "";

	private Color m_MovingSelectedBackColor = Color.AliceBlue;

	private IContainer components = null;

	private Label label_Text;

	private WindTextBox TextBox_Text;

	[Category("自定义属性")]
	[Description("下拉时显示的最大行数")]
	public int DropDownMaxRowCount
	{
		get
		{
			return m_DropDownMaxRowCount;
		}
		set
		{
			m_DropDownMaxRowCount = value;
		}
	}

	[Category("自定义属性")]
	[Description("下拉框显示偏移")]
	public Point DropDownLoctionOffset
	{
		get
		{
			return m_dropDownLoctionPoint;
		}
		set
		{
			m_dropDownLoctionPoint = value;
		}
	}

	[Category("自定义属性")]
	[Description("下拉框宽度拉长或缩窄")]
	public int DropDownWidthDelta
	{
		get
		{
			return m_DropDownWidthDelta;
		}
		set
		{
			m_DropDownWidthDelta = value;
		}
	}

	[Category("自定义属性")]
	[Description("右边图标大小")]
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
	[Description("边框颜色")]
	public Color FrameColor
	{
		get
		{
			return m_frameColor;
		}
		set
		{
			m_frameColor = value;
		}
	}

	[Category("自定义属性")]
	[Description("中间的分隔线颜色")]
	public Color SplitLineColor
	{
		get
		{
			return m_SplitLineColor;
		}
		set
		{
			m_SplitLineColor = value;
		}
	}

	[Category("自定义属性")]
	[Description("边框颜色")]
	public int FrameWidth
	{
		get
		{
			return m_frameWidth;
		}
		set
		{
			m_frameWidth = value;
		}
	}

	[Category("自定义属性")]
	[Description("正常时图标名字")]
	public string IconNormalName
	{
		get
		{
			return m_IconNormalName;
		}
		set
		{
			m_IconNormalName = value;
		}
	}

	[Category("自定义属性")]
	[Description("正常时图标颜色")]
	public Color IconNormalColor
	{
		get
		{
			return m_IconNormalColor;
		}
		set
		{
			m_IconNormalColor = value;
		}
	}

	[Category("自定义属性")]
	[Description("正常时图标名字")]
	public string IconMouseEnterName
	{
		get
		{
			return m_IconMouseEnterName;
		}
		set
		{
			m_IconMouseEnterName = value;
		}
	}

	[Category("自定义属性")]
	[Description("正常时图标颜色")]
	public Color IconMouseEnterColor
	{
		get
		{
			return m_IconMouseEnterColor;
		}
		set
		{
			m_IconMouseEnterColor = value;
		}
	}

	[Category("自定义属性")]
	[Description("正常时图标名字")]
	public string IconMouseDownName
	{
		get
		{
			return m_IconMouseDownName;
		}
		set
		{
			m_IconMouseDownName = value;
		}
	}

	[Category("自定义属性")]
	[Description("正常时图标颜色")]
	public Color IconMouseDownColor
	{
		get
		{
			return m_IconMouseDownColor;
		}
		set
		{
			m_IconMouseDownColor = value;
		}
	}

	[Description("自定义：选中哪一项")]
	public int SelectedIndex
	{
		get
		{
			return m_SelectedIndex;
		}
		set
		{
			if (m_SelectedIndex != value)
			{
				m_SelectedIndex = value;
				if (value >= 0 && value < Items.Count)
				{
					SelectedText = Items[value];
				}
				else
				{
					SelectedText = "";
				}
				if (this.SelectedIndexChanged != null)
				{
					this.SelectedIndexChanged(this, new EventArgs());
				}
			}
		}
	}

	[Description("自定义：选项表")]
	[Category("自定义")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Editor("System.Windows.Forms.Design.StringCollectionEditor, System.Design", "System.Drawing.Design.UITypeEditor, System.Drawing")]
	public List<string> Items
	{
		get
		{
			return ItemList;
		}
		set
		{
			ItemList = value;
			if (windDropDownBox != null)
			{
				windDropDownBox.Add(value);
			}
		}
	}

	[Description("自定义：文本是否只读")]
	[Category("自定义")]
	public bool ReadOnly
	{
		get
		{
			return m_readOnly;
		}
		set
		{
			m_readOnly = value;
		}
	}

	[Description("自定义：正常时右边小图标")]
	[Category("自定义")]
	public Image NormalImage
	{
		get
		{
			return m_NormalImage;
		}
		set
		{
			m_NormalImage = value;
		}
	}

	[Description("自定义：鼠标进入时右边小图标")]
	[Category("自定义")]
	public Image MouseEnterImage
	{
		get
		{
			return m_MouseEnterImage;
		}
		set
		{
			m_MouseEnterImage = value;
		}
	}

	[Description("自定义：下拉框显示时右边小图标")]
	[Category("自定义")]
	public Image MouseDownImage
	{
		get
		{
			return m_MouseDownImage;
		}
		set
		{
			m_MouseDownImage = value;
		}
	}

	[Description("自定义：选中的文本")]
	[Category("自定义")]
	public string SelectedText
	{
		get
		{
			return m_SelectedText;
		}
		set
		{
			m_SelectedText = value;
			((Control)label_Text).Text = value;
			((Control)TextBox_Text).Text = value;
		}
	}

	[Description("自定义：选中的文本")]
	[Category("光标在下拉框中移动时当前行的颜色")]
	public Color MovingSelectedBackColor
	{
		get
		{
			return m_MovingSelectedBackColor;
		}
		set
		{
			m_MovingSelectedBackColor = value;
		}
	}

	public event EventHandler SelectedIndexChanged;

	public WindComboBox()
	{
		InitializeComponent();
		SetStyles();
		Application.AddMessageFilter((IMessageFilter)(object)new ComboBoxMessager(this));
	}

	private void SetStyles()
	{
		((Control)this).SetStyle((ControlStyles)206866, true);
		((Control)this).UpdateStyles();
	}

	protected override void OnCreateControl()
	{
		((UserControl)this).OnCreateControl();
		CreateImage();
		Application.AddMessageFilter((IMessageFilter)(object)new ComboBoxMessager(this));
		if (windDropDownBox == null)
		{
			windDropDownBox = new WindDropDownBox();
			ConnectDropDownBox(windDropDownBox);
		}
		windDropDownBox.Clear();
		windDropDownBox.Add(Items);
		if (SelectedIndex >= 0 && SelectedIndex < Items.Count && SelectedText != Items[SelectedIndex])
		{
			SelectedIndex = SelectedIndex;
			if (this.SelectedIndexChanged != null)
			{
				this.SelectedIndexChanged(this, new EventArgs());
			}
		}
		((Control)label_Text).Visible = ReadOnly;
		((Control)label_Text).Font = ((Control)this).Font;
		((Control)label_Text).Text = SelectedText;
		((Control)label_Text).Location = new Point(4, (((Control)this).Height - ((Control)label_Text).Height) / 2);
		((Control)TextBox_Text).Visible = !ReadOnly;
		((Control)TextBox_Text).Font = ((Control)this).Font;
		((Control)TextBox_Text).Text = SelectedText;
		((Control)TextBox_Text).Location = new Point(4, (((Control)this).Height - ((Control)TextBox_Text).Height) / 2);
		((Control)this).BackgroundImage = NormalImage;
		((Control)windDropDownBox).Visible = false;
		((Control)this).Parent.Controls.Add((Control)(object)windDropDownBox);
	}

	public void SetForm(Form form)
	{
		form.Deactivate -= FormDeactivateEventHandler;
		form.Deactivate += FormDeactivateEventHandler;
	}

	public void FormDeactivateEventHandler(object sender, EventArgs e)
	{
		if (windDropDownBox != null && ((Control)windDropDownBox).Visible)
		{
			((Control)windDropDownBox).Visible = false;
		}
	}

	public bool isInClientRect()
	{
		Point pt = ((Control)this).PointToClient(new Point(Control.MousePosition.X, Control.MousePosition.Y));
		return ((Control)this).ClientRectangle.Contains(pt);
	}

	private void CreateImage(string iconName, ref Image image, Color iconColor)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected O, but got Unknown
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		image = (Image)new Bitmap(((Control)this).Width, ((Control)this).Height);
		Graphics val = Graphics.FromImage(image);
		val.Clear(((Control)this).BackColor);
		Pen val2 = new Pen(FrameColor, (float)FrameWidth);
		val2.Alignment = (PenAlignment)1;
		val.DrawRectangle(val2, new Rectangle(0, 0, ((Control)this).Width - 1, ((Control)this).Height - 1));
		Image image2 = (Image)(object)FontImages.GetImage(iconName, IconSize, iconColor, Color.Transparent);
		int num = ((Control)this).Width - image2.Width;
		if (num < 0)
		{
			num = 0;
		}
		int num2 = (((Control)this).Height - image2.Height) / 2;
		if (num2 < 0)
		{
			num2 = 0;
		}
		val.DrawImage(image2, num, num2, image2.Width, image2.Height);
	}

	protected virtual bool CreateImage()
	{
		if (IconNormalName != "")
		{
			CreateImage(IconNormalName, ref m_NormalImage, IconNormalColor);
			CreateImage(IconMouseEnterName, ref m_MouseEnterImage, IconMouseEnterColor);
			CreateImage(IconMouseDownName, ref m_MouseDownImage, IconMouseDownColor);
			return true;
		}
		return false;
	}

	public void SetSelectedText(string selectedText)
	{
		if (!(SelectedText != selectedText))
		{
			return;
		}
		for (int i = 0; i < Items.Count; i++)
		{
			if (selectedText == Items[i])
			{
				SelectedText = selectedText;
				SelectedIndex = i;
				break;
			}
		}
	}

	public void ConnectDropDownBox(WindDropDownBox dropDownBox)
	{
		windDropDownBox = dropDownBox;
		dropDownBox.SetWindComboBox(this);
	}

	private void FormDropDownBox_Deactivate(object sender, EventArgs e)
	{
		Point pt = ((Control)this).PointToClient(Control.MousePosition);
		if (!((Control)this).ClientRectangle.Contains(pt))
		{
			ShowDropDownBox(visable: false);
		}
	}

	private void ShowDropDownBox(bool visable)
	{
		if (windDropDownBox == null)
		{
			return;
		}
		if (visable)
		{
			((Control)this).BackgroundImage = MouseDownImage;
			((Control)windDropDownBox).Show();
			Point location = ((Control)this).Location;
			location.Y += ((Control)this).Height + m_dropDownLoctionPoint.Y;
			location.X += m_dropDownLoctionPoint.X;
			windDropDownBox.SetWindows(location, ((Control)this).Width + m_DropDownWidthDelta);
			((Control)windDropDownBox).BringToFront();
		}
		else
		{
			((Control)windDropDownBox).Hide();
			if (((Control)this).ClientRectangle.Contains(((Control)this).PointToClient(Control.MousePosition)))
			{
				((Control)this).BackgroundImage = MouseEnterImage;
			}
			else
			{
				((Control)this).BackgroundImage = NormalImage;
			}
		}
	}

	private void WindComboBox_MouseEnter(object sender, EventArgs e)
	{
		Image val = (((Control)windDropDownBox).Visible ? MouseDownImage : MouseEnterImage);
		if (((Control)this).BackgroundImage != val)
		{
			((Control)this).BackgroundImage = val;
		}
	}

	private void WindComboBox_MouseDown(object sender, MouseEventArgs e)
	{
		if (windDropDownBox != null)
		{
			ShowDropDownBox(!((Control)windDropDownBox).Visible);
		}
	}

	private void WindComboBox_MouseLeave(object sender, EventArgs e)
	{
		if (windDropDownBox != null)
		{
			((Control)this).BackgroundImage = (((Control)windDropDownBox).Visible ? MouseDownImage : NormalImage);
		}
	}

	private void label_Text_MouseDown(object sender, MouseEventArgs e)
	{
		WindComboBox_MouseDown(null, null);
	}

	public void DropDownBoxSelected(int rowIndex, string text)
	{
		if (rowIndex >= 0)
		{
			SelectedText = text;
			if (SelectedIndex != rowIndex)
			{
				SelectedIndex = rowIndex;
			}
			else if (this.SelectedIndexChanged != null)
			{
				this.SelectedIndexChanged(this, new EventArgs());
			}
			ShowDropDownBox(visable: false);
		}
	}

	public void Add(string[] value)
	{
		for (int i = 0; i < value.Length; i++)
		{
			Add(value[i]);
		}
	}

	public void Add(string value)
	{
		if (windDropDownBox != null)
		{
			windDropDownBox.Add(value);
		}
		ItemList.Add(value);
	}

	public void Clear()
	{
		if (windDropDownBox != null)
		{
			windDropDownBox.Clear();
		}
		ItemList.Clear();
	}

	private void WindComboBox_SizeChanged(object sender, EventArgs e)
	{
		if (CreateImage())
		{
			((Control)this).BackgroundImage = NormalImage;
		}
	}

	private void WindComboBox_EnabledChanged(object sender, EventArgs e)
	{
		((Control)TextBox_Text).Enabled = ((Control)this).Enabled;
		((Control)label_Text).Enabled = ((Control)this).Enabled;
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
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Expected O, but got Unknown
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Expected O, but got Unknown
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Expected O, but got Unknown
		label_Text = new Label();
		TextBox_Text = new WindTextBox();
		((Control)this).SuspendLayout();
		((Control)label_Text).AutoSize = true;
		((Control)label_Text).BackColor = Color.Transparent;
		((Control)label_Text).Location = new Point(5, 7);
		((Control)label_Text).Margin = new Padding(5, 0, 5, 0);
		((Control)label_Text).Name = "label_Text";
		((Control)label_Text).Size = new Size(55, 21);
		((Control)label_Text).TabIndex = 1;
		((Control)label_Text).Text = "label1";
		((Control)label_Text).MouseDown += new MouseEventHandler(label_Text_MouseDown);
		TextBox_Text.InputMode = WindBaseTextBox.TextBoxInputMode.Normal;
		((Control)TextBox_Text).Location = new Point(50, 7);
		((Control)TextBox_Text).Margin = new Padding(5);
		((Control)TextBox_Text).Name = "TextBox_Text";
		((Control)TextBox_Text).Size = new Size(102, 25);
		((Control)TextBox_Text).TabIndex = 2;
		((Control)TextBox_Text).Text = "";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(10f, 21f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)TextBox_Text);
		((Control)this).Controls.Add((Control)(object)label_Text);
		((Control)this).DoubleBuffered = true;
		((Control)this).Font = new Font("微软雅黑", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)this).Margin = new Padding(5);
		((Control)this).Name = "WindComboBox";
		((Control)this).Size = new Size(158, 40);
		((Control)this).EnabledChanged += WindComboBox_EnabledChanged;
		((Control)this).SizeChanged += WindComboBox_SizeChanged;
		((Control)this).MouseDown += new MouseEventHandler(WindComboBox_MouseDown);
		((Control)this).MouseEnter += WindComboBox_MouseEnter;
		((Control)this).MouseLeave += WindComboBox_MouseLeave;
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
