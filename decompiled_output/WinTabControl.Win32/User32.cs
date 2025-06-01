using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace WinTabControl.Win32;

internal class User32
{
	[Flags]
	internal enum TabControlHitTest
	{
		TCHT_NOWHERE = 1,
		TCHT_ONITEMICON = 2,
		TCHT_ONITEMLABEL = 4,
		TCHT_ONITEM = 6
	}

	internal enum SetWindowPosZ
	{
		HWND_TOP = 0,
		HWND_BOTTOM = 1,
		HWND_TOPMOST = -1,
		HWND_NOTOPMOST = -2
	}

	[Flags]
	internal enum FlagsSetWindowPos : uint
	{
		SWP_NOSIZE = 1u,
		SWP_NOMOVE = 2u,
		SWP_NOZORDER = 4u,
		SWP_NOREDRAW = 8u,
		SWP_NOACTIVATE = 0x10u,
		SWP_FRAMECHANGED = 0x20u,
		SWP_SHOWWINDOW = 0x40u,
		SWP_HIDEWINDOW = 0x80u,
		SWP_NOCOPYBITS = 0x100u,
		SWP_NOOWNERZORDER = 0x200u,
		SWP_NOSENDCHANGING = 0x400u,
		SWP_DRAWFRAME = 0x20u,
		SWP_NOREPOSITION = 0x200u,
		SWP_DEFERERASE = 0x2000u,
		SWP_ASYNCWINDOWPOS = 0x4000u
	}

	[Flags]
	internal enum RedrawWindowFlags : uint
	{
		Invalidate = 1u,
		InternalPaint = 2u,
		Erase = 4u,
		Validate = 8u,
		NoInternalPaint = 0x10u,
		NoErase = 0x20u,
		NoChildren = 0x40u,
		AllChildren = 0x80u,
		UpdateNow = 0x100u,
		EraseNow = 0x200u,
		Frame = 0x400u,
		NoFrame = 0x800u
	}

	internal enum ShowWindowStyles
	{
		SW_HIDE = 0,
		SW_SHOWNORMAL = 1,
		SW_NORMAL = 1,
		SW_SHOWMINIMIZED = 2,
		SW_SHOWMAXIMIZED = 3,
		SW_MAXIMIZE = 3,
		SW_SHOWNOACTIVATE = 4,
		SW_SHOW = 5,
		SW_MINIMIZE = 6,
		SW_SHOWMINNOACTIVE = 7,
		SW_SHOWNA = 8,
		SW_RESTORE = 9,
		SW_SHOWDEFAULT = 10,
		SW_FORCEMINIMIZE = 11,
		SW_MAX = 11
	}

	internal enum Msgs
	{
		WM_NULL = 0,
		WM_CREATE = 1,
		WM_DESTROY = 2,
		WM_MOVE = 3,
		WM_SIZE = 5,
		WM_ACTIVATE = 6,
		WM_SETFOCUS = 7,
		WM_KILLFOCUS = 8,
		WM_ENABLE = 10,
		WM_SETREDRAW = 11,
		WM_SETTEXT = 12,
		WM_GETTEXT = 13,
		WM_GETTEXTLENGTH = 14,
		WM_PAINT = 15,
		WM_CLOSE = 16,
		WM_QUERYENDSESSION = 17,
		WM_QUIT = 18,
		WM_QUERYOPEN = 19,
		WM_ERASEBKGND = 20,
		WM_SYSCOLORCHANGE = 21,
		WM_ENDSESSION = 22,
		WM_SHOWWINDOW = 24,
		WM_WININICHANGE = 26,
		WM_SETTINGCHANGE = 26,
		WM_DEVMODECHANGE = 27,
		WM_ACTIVATEAPP = 28,
		WM_FONTCHANGE = 29,
		WM_TIMECHANGE = 30,
		WM_CANCELMODE = 31,
		WM_SETCURSOR = 32,
		WM_MOUSEACTIVATE = 33,
		WM_CHILDACTIVATE = 34,
		WM_QUEUESYNC = 35,
		WM_GETMINMAXINFO = 36,
		WM_PAINTICON = 38,
		WM_ICONERASEBKGND = 39,
		WM_NEXTDLGCTL = 40,
		WM_SPOOLERSTATUS = 42,
		WM_DRAWITEM = 43,
		WM_MEASUREITEM = 44,
		WM_DELETEITEM = 45,
		WM_VKEYTOITEM = 46,
		WM_CHARTOITEM = 47,
		WM_SETFONT = 48,
		WM_GETFONT = 49,
		WM_SETHOTKEY = 50,
		WM_GETHOTKEY = 51,
		WM_QUERYDRAGICON = 55,
		WM_COMPAREITEM = 57,
		WM_GETOBJECT = 61,
		WM_COMPACTING = 65,
		WM_COMMNOTIFY = 68,
		WM_WINDOWPOSCHANGING = 70,
		WM_WINDOWPOSCHANGED = 71,
		WM_POWER = 72,
		WM_COPYDATA = 74,
		WM_CANCELJOURNAL = 75,
		WM_NOTIFY = 78,
		WM_INPUTLANGCHANGEREQUEST = 80,
		WM_INPUTLANGCHANGE = 81,
		WM_TCARD = 82,
		WM_HELP = 83,
		WM_USERCHANGED = 84,
		WM_NOTIFYFORMAT = 85,
		WM_CONTEXTMENU = 123,
		WM_STYLECHANGING = 124,
		WM_STYLECHANGED = 125,
		WM_DISPLAYCHANGE = 126,
		WM_GETICON = 127,
		WM_SETICON = 128,
		WM_NCCREATE = 129,
		WM_NCDESTROY = 130,
		WM_NCCALCSIZE = 131,
		WM_NCHITTEST = 132,
		WM_NCPAINT = 133,
		WM_NCACTIVATE = 134,
		WM_GETDLGCODE = 135,
		WM_SYNCPAINT = 136,
		WM_NCMOUSEMOVE = 160,
		WM_NCLBUTTONDOWN = 161,
		WM_NCLBUTTONUP = 162,
		WM_NCLBUTTONDBLCLK = 163,
		WM_NCRBUTTONDOWN = 164,
		WM_NCRBUTTONUP = 165,
		WM_NCRBUTTONDBLCLK = 166,
		WM_NCMBUTTONDOWN = 167,
		WM_NCMBUTTONUP = 168,
		WM_NCMBUTTONDBLCLK = 169,
		WM_NCXBUTTONDOWN = 171,
		WM_NCXBUTTONUP = 172,
		WM_KEYDOWN = 256,
		WM_KEYUP = 257,
		WM_CHAR = 258,
		WM_DEADCHAR = 259,
		WM_SYSKEYDOWN = 260,
		WM_SYSKEYUP = 261,
		WM_SYSCHAR = 262,
		WM_SYSDEADCHAR = 263,
		WM_KEYLAST = 264,
		WM_IME_STARTCOMPOSITION = 269,
		WM_IME_ENDCOMPOSITION = 270,
		WM_IME_COMPOSITION = 271,
		WM_IME_KEYLAST = 271,
		WM_INITDIALOG = 272,
		WM_COMMAND = 273,
		WM_SYSCOMMAND = 274,
		WM_TIMER = 275,
		WM_HSCROLL = 276,
		WM_VSCROLL = 277,
		WM_INITMENU = 278,
		WM_INITMENUPOPUP = 279,
		WM_MENUSELECT = 287,
		WM_MENUCHAR = 288,
		WM_ENTERIDLE = 289,
		WM_MENURBUTTONUP = 290,
		WM_MENUDRAG = 291,
		WM_MENUGETOBJECT = 292,
		WM_UNINITMENUPOPUP = 293,
		WM_MENUCOMMAND = 294,
		WM_CTLCOLORMSGBOX = 306,
		WM_CTLCOLOREDIT = 307,
		WM_CTLCOLORLISTBOX = 308,
		WM_CTLCOLORBTN = 309,
		WM_CTLCOLORDLG = 310,
		WM_CTLCOLORSCROLLBAR = 311,
		WM_CTLCOLORSTATIC = 312,
		WM_MOUSEMOVE = 512,
		WM_LBUTTONDOWN = 513,
		WM_LBUTTONUP = 514,
		WM_LBUTTONDBLCLK = 515,
		WM_RBUTTONDOWN = 516,
		WM_RBUTTONUP = 517,
		WM_RBUTTONDBLCLK = 518,
		WM_MBUTTONDOWN = 519,
		WM_MBUTTONUP = 520,
		WM_MBUTTONDBLCLK = 521,
		WM_MOUSEWHEEL = 522,
		WM_XBUTTONDOWN = 523,
		WM_XBUTTONUP = 524,
		WM_XBUTTONDBLCLK = 525,
		WM_PARENTNOTIFY = 528,
		WM_ENTERMENULOOP = 529,
		WM_EXITMENULOOP = 530,
		WM_NEXTMENU = 531,
		WM_SIZING = 532,
		WM_CAPTURECHANGED = 533,
		WM_MOVING = 534,
		WM_DEVICECHANGE = 537,
		WM_MDICREATE = 544,
		WM_MDIDESTROY = 545,
		WM_MDIACTIVATE = 546,
		WM_MDIRESTORE = 547,
		WM_MDINEXT = 548,
		WM_MDIMAXIMIZE = 549,
		WM_MDITILE = 550,
		WM_MDICASCADE = 551,
		WM_MDIICONARRANGE = 552,
		WM_MDIGETACTIVE = 553,
		WM_MDISETMENU = 560,
		WM_ENTERSIZEMOVE = 561,
		WM_EXITSIZEMOVE = 562,
		WM_DROPFILES = 563,
		WM_MDIREFRESHMENU = 564,
		WM_IME_SETCONTEXT = 641,
		WM_IME_NOTIFY = 642,
		WM_IME_CONTROL = 643,
		WM_IME_COMPOSITIONFULL = 644,
		WM_IME_SELECT = 645,
		WM_IME_CHAR = 646,
		WM_IME_REQUEST = 648,
		WM_IME_KEYDOWN = 656,
		WM_IME_KEYUP = 657,
		WM_MOUSEHOVER = 673,
		WM_MOUSELEAVE = 675,
		WM_CUT = 768,
		WM_COPY = 769,
		WM_PASTE = 770,
		WM_CLEAR = 771,
		WM_UNDO = 772,
		WM_RENDERFORMAT = 773,
		WM_RENDERALLFORMATS = 774,
		WM_DESTROYCLIPBOARD = 775,
		WM_DRAWCLIPBOARD = 776,
		WM_PAINTCLIPBOARD = 777,
		WM_VSCROLLCLIPBOARD = 778,
		WM_SIZECLIPBOARD = 779,
		WM_ASKCBFORMATNAME = 780,
		WM_CHANGECBCHAIN = 781,
		WM_HSCROLLCLIPBOARD = 782,
		WM_QUERYNEWPALETTE = 783,
		WM_PALETTEISCHANGING = 784,
		WM_PALETTECHANGED = 785,
		WM_HOTKEY = 786,
		WM_PRINT = 791,
		WM_PRINTCLIENT = 792,
		WM_HANDHELDFIRST = 856,
		WM_HANDHELDLAST = 863,
		WM_AFXFIRST = 864,
		WM_AFXLAST = 895,
		WM_PENWINFIRST = 896,
		WM_PENWINLAST = 911,
		WM_APP = 32768,
		WM_USER = 1024,
		WM_REFLECT = 8192,
		WM_THEMECHANGED = 794
	}

	internal enum WindowStyles : uint
	{
		WS_OVERLAPPED = 0u,
		WS_POPUP = 2147483648u,
		WS_CHILD = 1073741824u,
		WS_MINIMIZE = 536870912u,
		WS_VISIBLE = 268435456u,
		WS_DISABLED = 134217728u,
		WS_CLIPSIBLINGS = 67108864u,
		WS_CLIPCHILDREN = 33554432u,
		WS_MAXIMIZE = 16777216u,
		WS_CAPTION = 12582912u,
		WS_BORDER = 8388608u,
		WS_DLGFRAME = 4194304u,
		WS_VSCROLL = 2097152u,
		WS_HSCROLL = 1048576u,
		WS_SYSMENU = 524288u,
		WS_THICKFRAME = 262144u,
		WS_GROUP = 131072u,
		WS_TABSTOP = 65536u,
		WS_MINIMIZEBOX = 131072u,
		WS_MAXIMIZEBOX = 65536u,
		WS_TILED = 0u,
		WS_ICONIC = 536870912u,
		WS_SIZEBOX = 262144u,
		WS_POPUPWINDOW = 2156396544u,
		WS_OVERLAPPEDWINDOW = 13565952u,
		WS_TILEDWINDOW = 13565952u,
		WS_CHILDWINDOW = 1073741824u
	}

	internal enum WindowExStyles
	{
		GWL_STYLE = -16,
		GWL_EXSTYLE = -20,
		WS_EX_DLGMODALFRAME = 1,
		WS_EX_NOPARENTNOTIFY = 4,
		WS_EX_TOPMOST = 8,
		WS_EX_ACCEPTFILES = 16,
		WS_EX_TRANSPARENT = 32,
		WS_EX_MDICHILD = 64,
		WS_EX_TOOLWINDOW = 128,
		WS_EX_WINDOWEDGE = 256,
		WS_EX_CLIENTEDGE = 512,
		WS_EX_CONTEXTHELP = 1024,
		WS_EX_RIGHT = 4096,
		WS_EX_LEFT = 0,
		WS_EX_RTLREADING = 8192,
		WS_EX_LTRREADING = 0,
		WS_EX_LEFTSCROLLBAR = 16384,
		WS_EX_RIGHTSCROLLBAR = 0,
		WS_EX_CONTROLPARENT = 65536,
		WS_EX_STATICEDGE = 131072,
		WS_EX_APPWINDOW = 262144,
		WS_EX_OVERLAPPEDWINDOW = 768,
		WS_EX_PALETTEWINDOW = 392,
		WS_EX_LAYERED = 524288,
		WS_EX_NOACTIVATE = 134217728
	}

	internal struct RECT
	{
		public int left;

		public int top;

		public int right;

		public int bottom;

		public override string ToString()
		{
			return "{left=" + left + ", top=" + top + ", right=" + right + ", bottom=" + bottom + "}";
		}
	}

	internal struct BLENDFUNCTION
	{
		public byte BlendOp;

		public byte BlendFlags;

		public byte SourceConstantAlpha;

		public byte AlphaFormat;
	}

	internal struct TCHITTESTINFO
	{
		public Point pt;

		public TabControlHitTest flags;

		public TCHITTESTINFO(TabControlHitTest hitTest)
		{
			this = default(TCHITTESTINFO);
			flags = hitTest;
		}

		public TCHITTESTINFO(Point point, TabControlHitTest hitTest)
			: this(hitTest)
		{
			pt = point;
		}

		public TCHITTESTINFO(int x, int y, TabControlHitTest hitTest)
			: this(hitTest)
		{
			pt = new Point(x, y);
		}
	}

	internal const byte _AC_SRC_OVER = 0;

	internal const byte _AC_SRC_ALPHA = 1;

	internal const int _LWA_ALPHA = 2;

	internal const int _PAT_INVERT = 5898313;

	internal const int _HT_CAPTION = 2;

	internal const int _HT_CLIENT = 1;

	internal const int _HT_TRANSPARENT = -1;

	internal const int _TCM_HITTEST = 4877;

	[DllImport("gdi32")]
	internal static extern bool DeleteObject(IntPtr hObject);

	[DllImport("gdi32")]
	internal static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

	[DllImport("gdi32", SetLastError = true)]
	internal static extern IntPtr CreateCompatibleDC(IntPtr hdc);

	[DllImport("gdi32")]
	internal static extern IntPtr CreateDC(string driverName, string deviceName, string output, IntPtr lpInitData);

	[DllImport("gdi32")]
	internal static extern bool DeleteDC(IntPtr dc);

	[DllImport("uxtheme", CharSet = CharSet.Unicode, ExactSpelling = true)]
	internal static extern int SetWindowTheme(IntPtr hWnd, string textSubAppName, string textSubIdList);

	[DllImport("user32", CharSet = CharSet.Auto)]
	internal static extern IntPtr SetParent(IntPtr hwndChild, IntPtr hwndParent);

	[DllImport("user32", CharSet = CharSet.Auto)]
	internal static extern IntPtr GetParent(IntPtr hwnd);

	[DllImport("user32", CharSet = CharSet.Auto)]
	internal static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

	[DllImport("user32", CharSet = CharSet.Auto)]
	internal static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);

	[DllImport("user32", CharSet = CharSet.Auto)]
	internal static extern int SendMessage(IntPtr hwnd, int tMsg, IntPtr wParam, ref TCHITTESTINFO lParam);

	[DllImport("user32", CharSet = CharSet.Auto)]
	internal static extern IntPtr LoadCursorFromFile(string filename);

	[DllImport("user32", CharSet = CharSet.Auto)]
	internal static extern bool IsWindowVisible(IntPtr hwnd);

	[DllImport("user32", CharSet = CharSet.Auto)]
	internal static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, [MarshalAs(UnmanagedType.LPTStr)] string lpszClass, [MarshalAs(UnmanagedType.LPTStr)] string lpszWindow);

	[DllImport("user32", CharSet = CharSet.Auto)]
	internal static extern bool InvalidateRect(IntPtr hwnd, ref Rectangle rect, bool bErase);

	[DllImport("user32", CharSet = CharSet.Auto)]
	internal static extern bool ValidateRect(IntPtr hwnd, ref Rectangle rect);

	[DllImport("user32", CharSet = CharSet.Auto)]
	internal static extern int GetWindowLong(IntPtr hWnd, int dwStyle);

	[DllImport("user32", CharSet = CharSet.Auto)]
	internal static extern IntPtr GetDC(IntPtr hwnd);

	[DllImport("user32", CharSet = CharSet.Auto)]
	internal static extern int ReleaseDC(IntPtr hwnd, IntPtr hdc);

	[DllImport("user32", CharSet = CharSet.Auto)]
	internal static extern int GetClientRect(IntPtr hwnd, ref RECT rc);

	[DllImport("user32", CharSet = CharSet.Auto)]
	internal static extern int GetClientRect(IntPtr hwnd, [In][Out] ref Rectangle rect);

	[DllImport("user32", CharSet = CharSet.Auto)]
	internal static extern bool GetWindowRect(IntPtr hWnd, [In][Out] ref Rectangle rect);

	[DllImport("user32", CharSet = CharSet.Auto)]
	internal static extern int GetWindowRect(IntPtr hwnd, ref RECT rc);

	[DllImport("user32", ExactSpelling = true, SetLastError = true)]
	internal static extern bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref Point pptDst, ref Size psize, IntPtr hdcSrc, ref Point pprSrc, int crKey, ref BLENDFUNCTION pblend, int dwFlags);

	[DllImport("user32", CharSet = CharSet.Auto)]
	internal static extern uint SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

	[DllImport("user32", CharSet = CharSet.Auto)]
	internal static extern int SetWindowPos(IntPtr hWnd, IntPtr hWndAfter, int X, int Y, int Width, int Height, FlagsSetWindowPos flags);

	[DllImport("user32", CharSet = CharSet.Auto)]
	internal static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
}
