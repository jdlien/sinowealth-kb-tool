using System;
using System.Timers;
using System.Windows.Forms;

namespace USBUpdateTool;

public class WindowActive
{
	private Timer activedTimer = new Timer();

	private bool isStarted = false;

	private bool locker = false;

	private Form frontWindow;

	private Form backWindow;

	public WindowActive()
	{
		InitTimer();
	}

	public void SetWindows(Form _frontWindow, Form _backWindow)
	{
		frontWindow = _frontWindow;
		backWindow = _backWindow;
	}

	public void Start()
	{
		if (!locker && !isStarted)
		{
			isStarted = true;
			((Control)backWindow).Invoke((Delegate)(Action)delegate
			{
				backWindow.Activate();
			});
			activedTimer.Start();
		}
		locker = false;
	}

	public void Stop()
	{
		isStarted = false;
		activedTimer.Stop();
		locker = true;
	}

	public void InitTimer()
	{
		locker = false;
		activedTimer.Interval = 10.0;
		activedTimer.Elapsed += delegate
		{
			activedTimer.Stop();
			((Control)frontWindow).Invoke((Delegate)(Action)delegate
			{
				frontWindow.Activate();
			});
			isStarted = false;
		};
		activedTimer.Stop();
	}
}
