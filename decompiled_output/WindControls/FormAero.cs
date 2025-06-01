using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WindControls;

public class FormAero : Form
{
	public struct MARGINS
	{
		public int leftWidth;

		public int rightWidth;

		public int topHeight;

		public int bottomHeight;
	}

	private bool m_AeroEnabled;

	private const int CS_DROPSHADOW = 131072;

	private const int WM_NCPAINT = 133;

	private const int WM_ACTIVATEAPP = 28;

	private const int WM_NCHITTEST = 132;

	private const int HTCLIENT = 1;

	private const int HTCAPTION = 2;

	private FormMover formMover = new FormMover();

	private IContainer components = null;

	protected override CreateParams CreateParams
	{
		get
		{
			m_AeroEnabled = CheckAeroEnabled();
			CreateParams createParams = ((Form)this).CreateParams;
			if (!m_AeroEnabled)
			{
				createParams.ClassStyle |= 0x20000;
			}
			return createParams;
		}
	}

	[DllImport("Gdi32.dll")]
	private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

	[DllImport("dwmapi.dll")]
	public static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);

	[DllImport("dwmapi.dll")]
	public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

	[DllImport("dwmapi.dll")]
	public static extern int DwmIsCompositionEnabled(ref int pfEnabled);

	public FormAero()
	{
		InitializeComponent();
		formMover.AddForm((Form)(object)this);
	}

	private bool CheckAeroEnabled()
	{
		if (Environment.OSVersion.Version.Major >= 6)
		{
			int pfEnabled = 0;
			DwmIsCompositionEnabled(ref pfEnabled);
			return pfEnabled == 1;
		}
		return false;
	}

	protected override void WndProc(ref Message m)
	{
		int msg = ((Message)(ref m)).Msg;
		int num = msg;
		if (num == 133 && m_AeroEnabled)
		{
			int attrValue = 2;
			DwmSetWindowAttribute(((Control)this).Handle, 2, ref attrValue, 4);
			MARGINS pMarInset = new MARGINS
			{
				bottomHeight = 1,
				leftWidth = 1,
				rightWidth = 1,
				topHeight = 1
			};
			DwmExtendFrameIntoClientArea(((Control)this).Handle, ref pMarInset);
		}
		((Form)this).WndProc(ref m);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((Form)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		((Control)this).SuspendLayout();
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 12f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(800, 450);
		((Control)this).DoubleBuffered = true;
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Control)this).Name = "FormAero";
		((Form)this).StartPosition = (FormStartPosition)0;
		((Control)this).Text = "FormAero";
		((Control)this).ResumeLayout(false);
	}
}
