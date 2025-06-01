using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace WindControls;

public class WindRadioButton : WindSelectBox
{
	private string m_SelectedIconName = "A_fa_dot_circle_o";

	private string m_UnSelectedIconName = "A_fa_circle_o";

	[Category("自定义属性")]
	[Description("选中的图标名字：在http://www.fontawesome.com.cn/faicons/中搜索")]
	public override string SelectedIconName
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
	[Description("未选中的图标名字：在http://www.fontawesome.com.cn/faicons/中搜索")]
	public override string UnSelectedIconName
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

	protected override void OnCreateControl()
	{
		base.OnCreateControl();
		base.CheckedChanged += windRadioButton_CheckedChanged;
	}

	private void windRadioButton_CheckedChanged(object sender, EventArgs e)
	{
		if (base.Checked)
		{
			SetRadioButtonCheck();
		}
	}

	public override void WindSelectBox_Click(object sender, EventArgs e)
	{
		if (base.Enabled && !base.Checked)
		{
			base.Checked = !base.Checked;
			((Control)this).Invalidate();
		}
	}

	private void SetRadioButtonCheck()
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		if (((Control)this).Parent == null)
		{
			return;
		}
		foreach (Control item in (ArrangedElementCollection)((Control)this).Parent.Controls)
		{
			Control val = item;
			if (val is WindRadioButton && (object)val != this)
			{
				WindRadioButton windRadioButton = (WindRadioButton)(object)val;
				if (windRadioButton.Checked)
				{
					windRadioButton.Checked = false;
				}
			}
		}
	}
}
