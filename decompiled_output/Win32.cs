using System;
using System.Runtime.InteropServices;

internal class Win32
{
	public struct Size
	{
		public int cx;

		public int cy;

		public Size(int x, int y)
		{
			cx = x;
			cy = y;
		}
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct BLENDFUNCTION
	{
		public byte BlendOp;

		public byte BlendFlags;

		public byte SourceConstantAlpha;

		public byte AlphaFormat;
	}

	public struct Point
	{
		public int x;

		public int y;

		public Point(int x, int y)
		{
			this.x = x;
			this.y = y;
		}
	}

	public const int MF_REMOVE = 4096;

	public const int SC_RESTORE = 61728;

	public const int SC_MOVE = 61456;

	public const int SC_SIZE = 61440;

	public const int SC_MINIMIZE = 61472;

	public const int SC_MAXIMIZE = 61488;

	public const int SC_CLOSE = 61536;

	public const int WM_SYSCOMMAND = 274;

	public const int WM_COMMAND = 273;

	public const int GW_HWNDFIRST = 0;

	public const int GW_HWNDLAST = 1;

	public const int GW_HWNDNEXT = 2;

	public const int GW_HWNDPREV = 3;

	public const int GW_OWNER = 4;

	public const int GW_CHILD = 5;

	public const int WM_CTLCOLOREDIT = 307;

	public const int WM_NCCALCSIZE = 131;

	public const int WM_WINDOWPOSCHANGING = 70;

	public const int WM_PAINT = 15;

	public const int WM_CREATE = 1;

	public const int WM_NCCREATE = 129;

	public const int WM_NCPAINT = 133;

	public const int WM_PRINT = 791;

	public const int WM_DESTROY = 2;

	public const int WM_SHOWWINDOW = 24;

	public const int WM_SHARED_MENU = 482;

	public const int HC_ACTION = 0;

	public const int WH_CALLWNDPROC = 4;

	public const int GWL_WNDPROC = -4;

	public const int WS_SYSMENU = 524288;

	public const int WS_SIZEBOX = 262144;

	public const int WS_MAXIMIZEBOX = 65536;

	public const int WS_MINIMIZEBOX = 131072;

	public const byte AC_SRC_OVER = 0;

	public const int ULW_ALPHA = 2;

	public const byte AC_SRC_ALPHA = 1;

	public const int AW_HOR_POSITIVE = 1;

	public const int AW_HOR_NEGATIVE = 2;

	public const int AW_VER_POSITIVE = 4;

	public const int AW_VER_NEGATIVE = 8;

	public const int AW_CENTER = 16;

	public const int AW_HIDE = 65536;

	public const int AW_ACTIVATE = 131072;

	public const int AW_SLIDE = 262144;

	public const int AW_BLEND = 524288;

	[DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
	public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

	[DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
	public static extern IntPtr GetDC(IntPtr hWnd);

	[DllImport("gdi32.dll", ExactSpelling = true)]
	public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObj);

	[DllImport("user32.dll", ExactSpelling = true)]
	public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

	[DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
	public static extern int DeleteDC(IntPtr hDC);

	[DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
	public static extern int DeleteObject(IntPtr hObj);

	[DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
	public static extern int UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref Point pptDst, ref Size psize, IntPtr hdcSrc, ref Point pptSrc, int crKey, ref BLENDFUNCTION pblend, int dwFlags);

	[DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
	public static extern IntPtr ExtCreateRegion(IntPtr lpXform, uint nCount, IntPtr rgnData);

	[DllImport("user32")]
	public static extern int SendMessage(IntPtr hwnd, int msg, int wp, int lp);

	[DllImport("user32")]
	public static extern bool AnimateWindow(IntPtr whnd, int dwtime, int dwflag);
}
