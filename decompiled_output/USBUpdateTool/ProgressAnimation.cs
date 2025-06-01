using System;
using System.Collections.Generic;
using System.Timers;
using System.Windows.Forms;
using WindControls;

namespace USBUpdateTool;

public class ProgressAnimation
{
	private int progressValue = 0;

	private Form form;

	private Timer progressTimer = new Timer();

	private List<WindProgressBar> progressBarList = new List<WindProgressBar>();

	public ProgressAnimation(Form _form)
	{
		form = _form;
		InitTimer();
	}

	public void InitTimer()
	{
		progressValue = 0;
		progressTimer.Interval = 100.0;
		progressTimer.Elapsed += delegate
		{
			((Control)form).Invoke((Delegate)(Action)delegate
			{
				if (++progressValue > 120)
				{
					progressValue = 0;
				}
				SetProgressValue(progressValue);
			});
		};
		progressTimer.Stop();
	}

	private void SetProgressValue(int value)
	{
		for (int i = 0; i < progressBarList.Count; i++)
		{
			progressBarList[i].Value = progressValue;
		}
	}

	public void Start()
	{
		progressTimer.Enabled = true;
		progressTimer.Start();
		progressValue = 0;
		SetProgressValue(0);
	}

	public void Stop()
	{
		progressTimer.Enabled = false;
		progressTimer.Stop();
		progressValue = 0;
		SetProgressValue(0);
	}

	public void Add(WindProgressBar progressBar)
	{
		progressBarList.Add(progressBar);
	}

	public void Add(List<WindProgressBar> bars)
	{
		for (int i = 0; i < bars.Count; i++)
		{
			progressBarList.Add(bars[i]);
		}
	}

	public void Clear()
	{
		progressBarList.Clear();
	}

	public bool isStart()
	{
		return progressTimer.Enabled;
	}

	public void Toggle()
	{
		if (isStart())
		{
			Stop();
		}
		else
		{
			Start();
		}
	}
}
