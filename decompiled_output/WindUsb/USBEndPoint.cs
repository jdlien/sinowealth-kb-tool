using System;
using System.IO;
using Microsoft.Win32.SafeHandles;

namespace WindUSB;

public class USBEndPoint
{
	private FileStream hidDevice = null;

	private WindowsAPI windowsApi = new WindowsAPI();

	public string devicePath = "";

	public string shortDevicePath = "";

	private IntPtr fileHandle = new IntPtr(-1);

	public HIDP_CAPS Hid_Caps;

	public HIDD_ATTRIBUTES attributes;

	private bool beginReadPending = false;

	private bool autoRead = false;

	public event EventHandler DataReceived;

	public event EventHandler DeviceRemoved;

	public USBEndPoint(string _shortDevicePath)
	{
		shortDevicePath = _shortDevicePath;
	}

	public bool IsOpened()
	{
		return fileHandle != new IntPtr(-1);
	}

	public bool Open(string fullPath, bool _autoRead)
	{
		try
		{
			if (beginReadPending || IsOpened())
			{
				return true;
			}
			fileHandle = windowsApi.CreateDeviceFile(fullPath);
			if (fileHandle == new IntPtr(-1))
			{
				return false;
			}
			beginReadPending = false;
			devicePath = fullPath;
			windowsApi.GETDeviceAttribute(fileHandle, out attributes);
			windowsApi.GetPreparseData(fileHandle, out var preparseData);
			windowsApi.GetCaps(preparseData, out Hid_Caps);
			windowsApi.FreePreparseData(preparseData);
			autoRead = _autoRead;
			if (Hid_Caps.InputReportByteLength > 0)
			{
				hidDevice = new FileStream(new SafeFileHandle(fileHandle, ownsHandle: false), FileAccess.ReadWrite, Hid_Caps.InputReportByteLength, isAsync: false);
				if (autoRead)
				{
					BeginAsyncRead();
				}
			}
			else if (Hid_Caps.OutputReportByteLength > 0)
			{
				hidDevice = new FileStream(new SafeFileHandle(fileHandle, ownsHandle: false), FileAccess.ReadWrite, Hid_Caps.OutputReportByteLength, isAsync: false);
			}
			return true;
		}
		catch
		{
		}
		return false;
	}

	public void BeginAsyncRead()
	{
		try
		{
			byte[] array = new byte[Hid_Caps.InputReportByteLength];
			if (hidDevice != null)
			{
				hidDevice.BeginRead(array, 0, Hid_Caps.InputReportByteLength, ReadCompleted, array);
				beginReadPending = true;
			}
		}
		catch
		{
		}
	}

	private void ReadCompleted(IAsyncResult iResult)
	{
		try
		{
			if (hidDevice != null && iResult != null)
			{
				byte[] array = (byte[])iResult.AsyncState;
				beginReadPending = false;
				hidDevice.EndRead(iResult);
				byte[] array2 = new byte[array.Length - 1];
				for (int i = 1; i < array.Length; i++)
				{
					array2[i - 1] = array[i];
				}
				report e = new report(array[0], array2);
				OnDataReceived(e);
				if (autoRead)
				{
					BeginAsyncRead();
				}
			}
		}
		catch
		{
			EventArgs e2 = new EventArgs();
			OnDeviceRemoved(e2);
			CloseDevice();
		}
	}

	protected virtual void OnDataReceived(EventArgs e)
	{
		try
		{
			if (this.DataReceived != null)
			{
				this.DataReceived(this, e);
			}
		}
		catch
		{
		}
	}

	protected virtual void OnDeviceRemoved(EventArgs e)
	{
		try
		{
			if (this.DeviceRemoved != null)
			{
				this.DeviceRemoved(this, e);
			}
		}
		catch
		{
		}
	}

	public bool CloseDevice()
	{
		try
		{
			if (beginReadPending)
			{
				return false;
			}
			if (hidDevice != null)
			{
				hidDevice.Close();
				hidDevice = null;
			}
			if (fileHandle != new IntPtr(-1))
			{
				windowsApi.CloseDeviceFile(fileHandle);
			}
			ClosingDevice();
			devicePath = "";
			return true;
		}
		catch
		{
			return false;
		}
	}

	private void ClosingDevice()
	{
		fileHandle = new IntPtr(-1);
	}

	public bool IsHavedDataEvent()
	{
		return this.DataReceived != null;
	}

	private void ClearEventHandler(ref EventHandler eventHandler)
	{
		Delegate[] invocationList = eventHandler.GetInvocationList();
		Delegate[] array = invocationList;
		foreach (Delegate obj in array)
		{
			object value = obj.GetType().GetProperty("Method").GetValue(obj, null);
			string text = (string)value.GetType().GetProperty("Name").GetValue(value, null);
			eventHandler = (EventHandler)Delegate.Remove(eventHandler, obj as EventHandler);
		}
	}

	public void ClearDataEvent()
	{
		try
		{
			if (this.DataReceived != null)
			{
				ClearEventHandler(ref this.DataReceived);
			}
			if (this.DeviceRemoved != null)
			{
				ClearEventHandler(ref this.DeviceRemoved);
			}
		}
		catch
		{
		}
	}

	internal bool WriteOutputReport(byte[] report)
	{
		try
		{
			if (hidDevice != null)
			{
				hidDevice.Write(report, 0, report.Length);
				return true;
			}
		}
		catch
		{
		}
		return false;
	}

	internal bool WriteFeature(byte[] data)
	{
		try
		{
			_ = fileHandle;
			if (true)
			{
				return windowsApi.WriteFeature(fileHandle, data);
			}
		}
		catch
		{
		}
		return false;
	}

	internal bool GetFeature(byte[] data)
	{
		try
		{
			_ = fileHandle;
			if (true)
			{
				return windowsApi.GetFeature(fileHandle, data);
			}
		}
		catch
		{
		}
		return false;
	}
}
