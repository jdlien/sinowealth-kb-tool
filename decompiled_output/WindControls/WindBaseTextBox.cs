using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WindControls;

[DefaultEvent("TextChanged")]
public class WindBaseTextBox : RichTextBox
{
	public enum TextBoxInputMode
	{
		Normal,
		NumberOnly,
		HexOnly
	}

	private TextBoxInputMode m_InputMode = TextBoxInputMode.Normal;

	[Description("自定义：文本")]
	[Category("自定义")]
	public TextBoxInputMode InputMode
	{
		get
		{
			return m_InputMode;
		}
		set
		{
			m_InputMode = value;
		}
	}

	protected override CreateParams CreateParams
	{
		get
		{
			CreateParams createParams = ((RichTextBox)this).CreateParams;
			if (LoadLibrary("msftedit.dll") != IntPtr.Zero)
			{
				createParams.ExStyle |= 0x20;
				createParams.ClassName = "RICHEDIT50W";
			}
			return createParams;
		}
	}

	public WindBaseTextBox()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		SetStyles();
		((Control)this).KeyPress += new KeyPressEventHandler(WindBaseTextBox_KeyPress);
	}

	private void SetStyles()
	{
		((Control)this).SetStyle((ControlStyles)204816, true);
		((Control)this).UpdateStyles();
	}

	[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
	private static extern IntPtr LoadLibrary(string lpFileName);

	private void WindBaseTextBox_KeyPress(object sender, KeyPressEventArgs e)
	{
		if (InputMode == TextBoxInputMode.HexOnly)
		{
			if (char.IsDigit(e.KeyChar) || char.IsLetter(e.KeyChar) || e.KeyChar == ' ')
			{
				e.Handled = !Enumerable.Contains("01234567890ABCDEFabcdef", e.KeyChar);
			}
		}
		else if (InputMode == TextBoxInputMode.NumberOnly && (char.IsDigit(e.KeyChar) || char.IsLetter(e.KeyChar) || e.KeyChar == ' '))
		{
			e.Handled = !Enumerable.Contains("01234567890", e.KeyChar);
		}
	}

	private void InitializeComponent()
	{
		((Control)this).SuspendLayout();
		((TextBoxBase)this).BorderStyle = (BorderStyle)0;
		((Control)this).ResumeLayout(false);
	}
}
