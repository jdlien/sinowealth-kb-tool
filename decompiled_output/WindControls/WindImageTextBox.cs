using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WindControls;

public class WindImageTextBox : TextBox
{
	private const int WM_ERASEBKGND = 20;

	private Image backImage;

	private WindBaseTextBox.TextBoxInputMode m_InputMode = WindBaseTextBox.TextBoxInputMode.Normal;

	[Category("自定义")]
	[Description("自定义：背景图片")]
	public Image BackImage
	{
		get
		{
			return backImage;
		}
		set
		{
			backImage = value;
			((Control)this).Invalidate();
		}
	}

	[Description("自定义：文本")]
	[Category("自定义")]
	public WindBaseTextBox.TextBoxInputMode InputMode
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
			CreateParams createParams = ((TextBox)this).CreateParams;
			if (LoadLibrary("msftedit.dll") != IntPtr.Zero)
			{
				createParams.ExStyle |= 0x20;
				createParams.ClassName = "RICHEDIT50W";
			}
			return createParams;
		}
	}

	[DllImport("user32.dll", EntryPoint = "SendMessageA")]
	public static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);

	[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
	private static extern IntPtr LoadLibrary(string lpFileName);

	protected void OnEraseBackgnd(Graphics graphics)
	{
		graphics.FillRectangle(Brushes.Transparent, 0, 0, ((Control)this).Width, ((Control)this).Height);
		if (BackImage != null)
		{
			graphics.DrawImage(BackImage, 0, 0, BackImage.Width, BackImage.Height);
		}
		graphics.Dispose();
	}

	protected override void WndProc(ref Message message)
	{
		if (((Message)(ref message)).Msg == 20)
		{
			OnEraseBackgnd(Graphics.FromHdc(((Message)(ref message)).WParam));
			((Message)(ref message)).Result = (IntPtr)1;
		}
		((TextBox)this).WndProc(ref message);
	}

	private void InitializeComponent()
	{
		((Control)this).SuspendLayout();
		((Control)this).TextChanged += WindImageTextBox_TextChanged;
		((Control)this).ResumeLayout(false);
	}

	private void WindImageTextBox_TextChanged(object sender, EventArgs e)
	{
	}
}
