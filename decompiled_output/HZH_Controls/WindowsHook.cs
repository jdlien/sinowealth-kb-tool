using System;
using System.Runtime.InteropServices;
using System.Text;

namespace HZH_Controls;

public class WindowsHook
{
	public delegate int HookProc(int nCode, IntPtr wParam, IntPtr lParam);

	public delegate void HookMsgHandler(string strHookName, int nCode, IntPtr msg, IntPtr lParam);

	public static event HookMsgHandler HookMsgChanged;

	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
	public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, int hInstance, int threadId);

	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
	public static extern bool UnhookWindowsHookEx(int idHook);

	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
	public static extern int CallNextHookEx(int idHook, int nCode, IntPtr wParam, IntPtr lParam);

	[DllImport("User32.dll", CharSet = CharSet.Auto)]
	public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

	[Obsolete]
	public static int StartHook(HookType hookType, int wParam = 0, int pid = 0, string strHookName = "")
	{
		int _hHook = 0;
		HookProc lpfn = delegate(int nCode, IntPtr msg, IntPtr lParam)
		{
			if (WindowsHook.HookMsgChanged != null)
			{
				try
				{
					WindowsHook.HookMsgChanged(strHookName, nCode, msg, lParam);
				}
				catch
				{
				}
			}
			return CallNextHookEx(_hHook, nCode, msg, lParam);
		};
		if (pid == 0)
		{
			pid = AppDomain.GetCurrentThreadId();
		}
		_hHook = SetWindowsHookEx((int)hookType, lpfn, wParam, pid);
		if (_hHook == 0)
		{
			StopHook(_hHook);
		}
		return _hHook;
	}

	public static bool StopHook(int _hHook)
	{
		bool flag = true;
		if (_hHook != 0)
		{
			flag = UnhookWindowsHookEx(_hHook);
		}
		if (!flag)
		{
			return false;
		}
		return true;
	}
}
