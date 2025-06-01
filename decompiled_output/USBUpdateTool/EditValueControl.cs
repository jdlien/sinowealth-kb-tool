using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WindControls;

namespace USBUpdateTool;

public class EditValueControl : UserControl
{
	public delegate void ValueChangedEvent(EditValueControl sender);

	public delegate void CheckValueChangedEvent(EditValueControl sender);

	public ValueChangedEvent OnValueChanged;

	public CheckValueChangedEvent OnCheckValueChanged;

	private string m_CheckedText = "  Edit VID/PID";

	private string m_UnCheckedText = " Default VID/PID";

	private bool isXUI = false;

	private IContainer components = null;

	public WindImageButton wibutton_confirm;

	public Label label_Right;

	public Label label_Left;

	public WindTextBox wTextBox_Right;

	public WindTextBox wTextBox_Left;

	public WindCheckBox wCheckBox_EditEnable;

	private Label label_Text;

	[Category("自定义属性")]
	[Description("选中时复选框显示的文字")]
	public string CheckedText
	{
		get
		{
			return m_CheckedText;
		}
		set
		{
			m_CheckedText = value;
			((Control)label_Text).Text = value;
		}
	}

	[Category("自定义属性")]
	[Description("未选中时复选框显示的文字")]
	public string UnCheckedText
	{
		get
		{
			return m_UnCheckedText;
		}
		set
		{
			m_UnCheckedText = value;
		}
	}

	[Category("自定义属性")]
	[Description("左边标签显示的文字")]
	public string LeftLabelText
	{
		get
		{
			return ((Control)label_Left).Text;
		}
		set
		{
			((Control)label_Left).Text = value;
		}
	}

	[Category("自定义属性")]
	[Description("右边标签显示的文字")]
	public string RightLabelText
	{
		get
		{
			return ((Control)label_Right).Text;
		}
		set
		{
			((Control)label_Right).Text = value;
		}
	}

	[Category("自定义属性")]
	[Description("是否选中")]
	public bool isChecked
	{
		get
		{
			return wCheckBox_EditEnable.Checked;
		}
		set
		{
			wCheckBox_EditEnable.Checked = value;
		}
	}

	[Category("自定义属性")]
	[Description("所有控件是否排成一行")]
	public bool xUI
	{
		get
		{
			return isXUI;
		}
		set
		{
			if (value)
			{
				SetXUI();
			}
			isXUI = value;
		}
	}

	public EditValueControl()
	{
		InitializeComponent();
		UpdateUI(Visible: false);
	}

	private void wibutton_confirm_Click(object sender, EventArgs e)
	{
		((Control)wTextBox_Left).Enabled = false;
		((Control)wTextBox_Right).Enabled = false;
		((Control)wibutton_confirm).Enabled = false;
		((Control)wTextBox_Left.wbTextBox).ForeColor = Color.Gray;
		((Control)wTextBox_Right.wbTextBox).ForeColor = Color.Gray;
		if (OnValueChanged != null)
		{
			OnValueChanged(this);
		}
	}

	public void UpdateUI(bool Visible)
	{
		if (Visible)
		{
			((Control)label_Left).ForeColor = Color.Black;
			((Control)label_Right).ForeColor = Color.Black;
			((Control)label_Text).ForeColor = Color.Black;
			((Control)label_Text).Text = CheckedText;
			((Control)wTextBox_Left.wbTextBox).ForeColor = Color.Black;
			((Control)wTextBox_Right.wbTextBox).ForeColor = Color.Black;
			((Control)wibutton_confirm).Visible = true;
			((Control)wibutton_confirm).Enabled = true;
			((Control)wTextBox_Left).Enabled = true;
			((Control)wTextBox_Right).Enabled = true;
		}
		else
		{
			((Control)label_Left).ForeColor = Color.Gray;
			((Control)label_Right).ForeColor = Color.Gray;
			((Control)label_Text).ForeColor = Color.Gray;
			((Control)label_Text).Text = UnCheckedText;
			((Control)wTextBox_Left.wbTextBox).ForeColor = Color.Gray;
			((Control)wTextBox_Right.wbTextBox).ForeColor = Color.Gray;
			((Control)wibutton_confirm).Visible = false;
			((Control)wTextBox_Left).Enabled = false;
			((Control)wTextBox_Right).Enabled = false;
		}
	}

	public void ResetUI()
	{
		wCheckBox_EditEnable.Checked = false;
		UpdateUI(Visible: false);
	}

	private void wCheckBox_EditEnable_CheckedChanged(object sender, EventArgs e)
	{
		UpdateUI(wCheckBox_EditEnable.Checked);
		if (OnCheckValueChanged != null)
		{
			OnCheckValueChanged(this);
		}
	}

	public void SetValue(string value)
	{
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		if (value != null)
		{
			string[] array = value.Split(new char[1] { '-' });
			if (array.Length == 2)
			{
				try
				{
					((Control)wTextBox_Left).Text = array[0];
					((Control)wTextBox_Right).Text = array[1];
				}
				catch (Exception ex)
				{
					MessageBox.Show("editValueControl SetValue-" + ex.Message);
				}
			}
		}
		UpdateUI(wCheckBox_EditEnable.Checked);
	}

	public void SetValue(bool isChecked, string leftValue, string rightVlaue)
	{
		wCheckBox_EditEnable.Checked = isChecked;
		SetValue(leftValue + "-" + rightVlaue);
	}

	public bool GetCheckBoxValue()
	{
		return wCheckBox_EditEnable.Checked;
	}

	public string GetLeftValue()
	{
		return ((Control)wTextBox_Left).Text;
	}

	public int GetLeftDecimalValue()
	{
		if (((Control)wTextBox_Left).Text == "")
		{
			return 0;
		}
		return Convert.ToInt32(((Control)wTextBox_Left).Text);
	}

	public string GetRightValue()
	{
		return ((Control)wTextBox_Right).Text;
	}

	public int GetRightDecimalValue()
	{
		if (((Control)wTextBox_Right).Text == "")
		{
			return 0;
		}
		return Convert.ToInt32(((Control)wTextBox_Right).Text);
	}

	public string GetLeftRightVlaue()
	{
		return GetVlaue();
	}

	public string GetVlaue()
	{
		return GetLeftValue() + "-" + GetRightValue();
	}

	public void SetValueMode(int inputSize, WindBaseTextBox.TextBoxInputMode textBoxInput)
	{
		wTextBox_Left.MaxLength = inputSize;
		wTextBox_Left.InputMode = textBoxInput;
		wTextBox_Right.MaxLength = inputSize;
		wTextBox_Right.InputMode = textBoxInput;
	}

	public void SetXUI()
	{
		CheckedText = "";
		UnCheckedText = "";
		((Control)label_Text).Text = "";
		((Control)label_Left).Location = new Point(((Control)label_Left).Location.X, ((Control)wCheckBox_EditEnable).Location.Y + 4);
		((Control)label_Right).Location = new Point(((Control)label_Right).Location.X, ((Control)wCheckBox_EditEnable).Location.Y + 4);
		((Control)wTextBox_Left).Location = new Point(((Control)wTextBox_Left).Location.X, ((Control)wCheckBox_EditEnable).Location.Y + 4);
		((Control)wTextBox_Right).Location = new Point(((Control)wTextBox_Right).Location.X, ((Control)wCheckBox_EditEnable).Location.Y + 4);
		((Control)wibutton_confirm).Location = new Point(((Control)wibutton_confirm).Location.X, ((Control)wCheckBox_EditEnable).Location.Y + 4);
	}

	public void SetXUI(int textWidth, string _checkedText, string _unCheckedText)
	{
		CheckedText = _checkedText;
		UnCheckedText = _unCheckedText;
		((Control)label_Text).Text = _unCheckedText;
		((Control)label_Left).Location = new Point(((Control)label_Left).Location.X + textWidth - 44, ((Control)wCheckBox_EditEnable).Location.Y + 8);
		((Control)label_Right).Location = new Point(((Control)label_Right).Location.X + textWidth - 44, ((Control)wCheckBox_EditEnable).Location.Y + 8);
		((Control)wTextBox_Left).Location = new Point(((Control)wTextBox_Left).Location.X + textWidth - 44, ((Control)wCheckBox_EditEnable).Location.Y + 4);
		((Control)wTextBox_Right).Location = new Point(((Control)wTextBox_Right).Location.X + textWidth - 44, ((Control)wCheckBox_EditEnable).Location.Y + 4);
		((Control)wibutton_confirm).Location = new Point(((Control)wibutton_confirm).Location.X + textWidth - 44, ((Control)wCheckBox_EditEnable).Location.Y + 4);
	}

	public void SetEnable(bool enable)
	{
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		string text = ((Control)wTextBox_Left).Text;
		string text2 = ((Control)wTextBox_Right).Text;
		wCheckBox_EditEnable.Enabled = enable;
		((Control)label_Text).Enabled = enable;
		((Control)wibutton_confirm).Enabled = enable;
		((Control)wTextBox_Left).Enabled = enable;
		((Control)wTextBox_Right).Enabled = enable;
		try
		{
			((Control)wTextBox_Left).Text = text;
			((Control)wTextBox_Right).Text = text2;
		}
		catch (Exception ex)
		{
			MessageBox.Show("editValueControl SetEnable-" + ex.Message);
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
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Expected O, but got Unknown
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Expected O, but got Unknown
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Expected O, but got Unknown
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Expected O, but got Unknown
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0294: Expected O, but got Unknown
		//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b6: Expected O, but got Unknown
		//IL_0357: Unknown result type (might be due to invalid IL or missing references)
		//IL_0361: Expected O, but got Unknown
		//IL_0395: Unknown result type (might be due to invalid IL or missing references)
		//IL_039f: Expected O, but got Unknown
		//IL_03d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dd: Expected O, but got Unknown
		//IL_049a: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a4: Expected O, but got Unknown
		//IL_04c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cd: Expected O, but got Unknown
		//IL_04e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ef: Expected O, but got Unknown
		//IL_052d: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0605: Expected O, but got Unknown
		//IL_0617: Unknown result type (might be due to invalid IL or missing references)
		//IL_0621: Expected O, but got Unknown
		//IL_0640: Unknown result type (might be due to invalid IL or missing references)
		//IL_064a: Expected O, but got Unknown
		//IL_0662: Unknown result type (might be due to invalid IL or missing references)
		//IL_066c: Expected O, but got Unknown
		//IL_06a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0775: Unknown result type (might be due to invalid IL or missing references)
		//IL_077f: Expected O, but got Unknown
		//IL_07bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c6: Expected O, but got Unknown
		//IL_07d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e2: Expected O, but got Unknown
		//IL_07fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0804: Expected O, but got Unknown
		//IL_0892: Unknown result type (might be due to invalid IL or missing references)
		//IL_089c: Expected O, but got Unknown
		//IL_0917: Unknown result type (might be due to invalid IL or missing references)
		//IL_0921: Expected O, but got Unknown
		//IL_09d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e1: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(EditValueControl));
		label_Right = new Label();
		label_Left = new Label();
		label_Text = new Label();
		wibutton_confirm = new WindImageButton();
		wTextBox_Right = new WindTextBox();
		wTextBox_Left = new WindTextBox();
		wCheckBox_EditEnable = new WindCheckBox();
		((Control)this).SuspendLayout();
		((Control)label_Right).AutoSize = true;
		((Control)label_Right).BackColor = Color.Transparent;
		((Control)label_Right).Font = new Font("微软雅黑", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)label_Right).Location = new Point(168, 44);
		((Control)label_Right).Name = "label_Right";
		((Control)label_Right).Size = new Size(37, 21);
		((Control)label_Right).TabIndex = 29;
		((Control)label_Right).Text = "PID";
		((Control)label_Left).AutoSize = true;
		((Control)label_Left).BackColor = Color.Transparent;
		((Control)label_Left).Font = new Font("微软雅黑", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)label_Left).Location = new Point(52, 44);
		((Control)label_Left).Name = "label_Left";
		((Control)label_Left).Size = new Size(38, 21);
		((Control)label_Left).TabIndex = 28;
		((Control)label_Left).Text = "VID";
		((Control)label_Text).AutoSize = true;
		((Control)label_Text).Font = new Font("微软雅黑", 12f);
		((Control)label_Text).Location = new Point(35, 6);
		((Control)label_Text).Name = "label_Text";
		((Control)label_Text).Size = new Size(55, 21);
		((Control)label_Text).TabIndex = 31;
		((Control)label_Text).Text = "label1";
		((Control)wibutton_confirm).BackColor = Color.Transparent;
		((Control)wibutton_confirm).BackgroundImage = (Image)componentResourceManager.GetObject("wibutton_confirm.BackgroundImage");
		((Control)wibutton_confirm).BackgroundImageLayout = (ImageLayout)3;
		wibutton_confirm.DisableBackColor = Color.Transparent;
		wibutton_confirm.DisableForeColor = Color.DarkGray;
		wibutton_confirm.DisableImage = (Image)componentResourceManager.GetObject("wibutton_confirm.DisableImage");
		((Control)wibutton_confirm).Font = new Font("微软雅黑", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wibutton_confirm.FrameMode = GraphicsHelper.RoundStyle.All;
		wibutton_confirm.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wibutton_confirm.IconName = "";
		wibutton_confirm.IconOffset = new Point(0, 0);
		wibutton_confirm.IconSize = 32;
		((Control)wibutton_confirm).Location = new Point(285, 41);
		wibutton_confirm.MouseDownBackColor = Color.Gray;
		wibutton_confirm.MouseDownForeColor = Color.Black;
		wibutton_confirm.MouseDownImage = (Image)componentResourceManager.GetObject("wibutton_confirm.MouseDownImage");
		wibutton_confirm.MouseEnterBackColor = Color.DarkGray;
		wibutton_confirm.MouseEnterForeColor = Color.Black;
		wibutton_confirm.MouseEnterImage = (Image)componentResourceManager.GetObject("wibutton_confirm.MouseEnterImage");
		wibutton_confirm.MouseUpBackColor = Color.Transparent;
		wibutton_confirm.MouseUpForeColor = Color.Black;
		wibutton_confirm.MouseUpImage = (Image)componentResourceManager.GetObject("wibutton_confirm.MouseUpImage");
		((Control)wibutton_confirm).Name = "wibutton_confirm";
		wibutton_confirm.Radius = 12;
		((Control)wibutton_confirm).Size = new Size(80, 30);
		((Control)wibutton_confirm).TabIndex = 30;
		((Control)wibutton_confirm).Text = "confirm";
		wibutton_confirm.TextAlignment = StringHelper.TextAlignment.Center;
		wibutton_confirm.TextDynOffset = new Point(0, 0);
		wibutton_confirm.TextFixLocation = new Point(0, 0);
		wibutton_confirm.TextFixLocationEnable = false;
		((Control)wibutton_confirm).Click += wibutton_confirm_Click;
		((Control)wTextBox_Right).BackgroundImage = (Image)componentResourceManager.GetObject("wTextBox_Right.BackgroundImage");
		((Control)wTextBox_Right).BackgroundImageLayout = (ImageLayout)3;
		wTextBox_Right.DisableBackImage = (Image)componentResourceManager.GetObject("wTextBox_Right.DisableBackImage");
		((Control)wTextBox_Right).Font = new Font("微软雅黑", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wTextBox_Right.FrameColor = Color.White;
		wTextBox_Right.InputMode = WindBaseTextBox.TextBoxInputMode.HexOnly;
		((Control)wTextBox_Right).Location = new Point(212, 40);
		((Control)wTextBox_Right).Margin = new Padding(6);
		wTextBox_Right.MaxLength = 4;
		((Control)wTextBox_Right).Name = "wTextBox_Right";
		wTextBox_Right.PasswordChar = "";
		wTextBox_Right.PromptText = "";
		wTextBox_Right.PromptTextColor = Color.Gray;
		wTextBox_Right.PromptTextForeColor = Color.LightGray;
		wTextBox_Right.ReadOnly = false;
		wTextBox_Right.SelectedBackImage = null;
		((Control)wTextBox_Right).Size = new Size(64, 30);
		((Control)wTextBox_Right).TabIndex = 27;
		wTextBox_Right.TextBoxOffset = new Point(4, 4);
		wTextBox_Right.UnSelectedBackImage = (Image)componentResourceManager.GetObject("wTextBox_Right.UnSelectedBackImage");
		((Control)wTextBox_Left).BackgroundImage = (Image)componentResourceManager.GetObject("wTextBox_Left.BackgroundImage");
		((Control)wTextBox_Left).BackgroundImageLayout = (ImageLayout)3;
		wTextBox_Left.DisableBackImage = (Image)componentResourceManager.GetObject("wTextBox_Left.DisableBackImage");
		((Control)wTextBox_Left).Font = new Font("微软雅黑", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wTextBox_Left.FrameColor = Color.White;
		wTextBox_Left.InputMode = WindBaseTextBox.TextBoxInputMode.HexOnly;
		((Control)wTextBox_Left).Location = new Point(97, 40);
		((Control)wTextBox_Left).Margin = new Padding(6);
		wTextBox_Left.MaxLength = 4;
		((Control)wTextBox_Left).Name = "wTextBox_Left";
		wTextBox_Left.PasswordChar = "";
		wTextBox_Left.PromptText = "";
		wTextBox_Left.PromptTextColor = Color.Gray;
		wTextBox_Left.PromptTextForeColor = Color.LightGray;
		wTextBox_Left.ReadOnly = false;
		wTextBox_Left.SelectedBackImage = null;
		((Control)wTextBox_Left).Size = new Size(64, 30);
		((Control)wTextBox_Left).TabIndex = 26;
		wTextBox_Left.TextBoxOffset = new Point(4, 4);
		wTextBox_Left.UnSelectedBackImage = (Image)componentResourceManager.GetObject("wTextBox_Left.UnSelectedBackImage");
		((Control)wCheckBox_EditEnable).BackColor = Color.Transparent;
		((Control)wCheckBox_EditEnable).BackgroundImageLayout = (ImageLayout)0;
		wCheckBox_EditEnable.Checked = false;
		wCheckBox_EditEnable.DisableSelectedImage = (Image)componentResourceManager.GetObject("wCheckBox_EditEnable.DisableSelectedImage");
		wCheckBox_EditEnable.DisableUnSelectedImage = (Image)componentResourceManager.GetObject("wCheckBox_EditEnable.DisableUnSelectedImage");
		((Control)wCheckBox_EditEnable).Font = new Font("微软雅黑", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wCheckBox_EditEnable.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wCheckBox_EditEnable.IconOffset = new Point(0, 0);
		wCheckBox_EditEnable.IconSize = 36;
		((Control)wCheckBox_EditEnable).Location = new Point(2, 0);
		((Control)wCheckBox_EditEnable).Name = "wCheckBox_EditEnable";
		wCheckBox_EditEnable.SelectedIconColor = Color.Red;
		wCheckBox_EditEnable.SelectedIconName = "";
		wCheckBox_EditEnable.SelectedImage = (Image)componentResourceManager.GetObject("wCheckBox_EditEnable.SelectedImage");
		((Control)wCheckBox_EditEnable).Size = new Size(30, 33);
		((Control)wCheckBox_EditEnable).TabIndex = 25;
		((Control)wCheckBox_EditEnable).Text = " ";
		wCheckBox_EditEnable.TextOffset = new Point(4, 4);
		wCheckBox_EditEnable.UnSelectedIconColor = Color.Gray;
		wCheckBox_EditEnable.UnSelectedIconName = "";
		wCheckBox_EditEnable.UnSelectedImage = (Image)componentResourceManager.GetObject("wCheckBox_EditEnable.UnSelectedImage");
		wCheckBox_EditEnable.CheckedChanged += wCheckBox_EditEnable_CheckedChanged;
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)0;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Controls.Add((Control)(object)label_Text);
		((Control)this).Controls.Add((Control)(object)wibutton_confirm);
		((Control)this).Controls.Add((Control)(object)label_Right);
		((Control)this).Controls.Add((Control)(object)label_Left);
		((Control)this).Controls.Add((Control)(object)wTextBox_Right);
		((Control)this).Controls.Add((Control)(object)wTextBox_Left);
		((Control)this).Controls.Add((Control)(object)wCheckBox_EditEnable);
		((Control)this).Font = new Font("微软雅黑", 10.5f);
		((Control)this).Name = "EditValueControl";
		((Control)this).Size = new Size(368, 77);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
