using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using WindControls;

namespace USBUpdateTool;

internal static class Program
{
	private static Mutex mutex;

	[DllImport("kernel32")]
	private static extern IntPtr LoadLibraryA([MarshalAs(UnmanagedType.LPStr)] string fileName);

	[STAThread]
	private static void Main()
	{
		LoadResoureDll.RegistDLL();
		Application.SetUnhandledExceptionMode((UnhandledExceptionMode)2);
		Application.ThreadException += Application_ThreadException;
		AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
		Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault(false);
		mutex = new Mutex(initiallyOwned: true, "WindControls");
		if (mutex.WaitOne(10, exitContext: false))
		{
			Application.Run((Form)(object)new FormMain());
			return;
		}
		mutex.Close();
		new FormMessageBox("The program has already been run").Show(null);
		Application.Exit();
	}

	private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
	{
		AppLog.LogApplication_ThreadException(e.Exception.ToString());
	}

	private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
	{
		AppLog.LogCurrentDomain_UnhandledException(e.ExceptionObject.ToString());
	}
}
