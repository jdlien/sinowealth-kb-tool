using System;
using System.Collections.Generic;
using WindUsb;

namespace WindUSB;

public class USBDevice
{
	public List<USBEndPoint> EndPoints = new List<USBEndPoint>();

	public int GetFeatureEndPointId = 0;

	public int WriteFeatureEndPointId = 0;

	public USBDevice()
	{
		EndPoints.Clear();
	}

	public void AddEndPoint(string shortDevicePath)
	{
		if (!(shortDevicePath != ""))
		{
			return;
		}
		for (int i = 0; i < EndPoints.Count; i++)
		{
			if (EndPoints[i].shortDevicePath.Contains(shortDevicePath))
			{
				return;
			}
		}
		EndPoints.Add(new USBEndPoint(shortDevicePath));
	}

	public void AddEndPoint(string[] shortDevicePath)
	{
		for (int i = 0; i < shortDevicePath.Length; i++)
		{
			AddEndPoint(shortDevicePath[i]);
		}
	}

	public USBEndPoint FindEndPoint(string endPointShortKey)
	{
		for (int i = 0; i < EndPoints.Count; i++)
		{
			if (EndPoints[i].shortDevicePath.Contains(endPointShortKey))
			{
				return EndPoints[i];
			}
		}
		return null;
	}

	public bool Open(EventHandler DataReceived, bool autoRead)
	{
		List<string> deviceList = UsbObserver.GetDeviceList();
		return Open(deviceList, DataReceived, autoRead);
	}

	public bool Open(List<string> allUsbDeviceList, EventHandler DataReceived, bool autoRead)
	{
		bool flag = IsOpened();
		if (!flag)
		{
			foreach (string allUsbDevice in allUsbDeviceList)
			{
				for (int i = 0; i < EndPoints.Count; i++)
				{
					if (allUsbDevice.Contains(EndPoints[i].shortDevicePath))
					{
						string deviceId = GetDeviceId();
						string deviceId2 = GetDeviceId(EndPoints[i].shortDevicePath, allUsbDevice);
						if (deviceId == "" || deviceId.Equals(deviceId2))
						{
							flag = EndPoints[i].Open(allUsbDevice, autoRead);
						}
					}
				}
			}
		}
		bool flag2 = false;
		foreach (string allUsbDevice2 in allUsbDeviceList)
		{
			for (int j = 0; j < EndPoints.Count; j++)
			{
				if (!EndPoints[j].devicePath.Equals("") && allUsbDevice2.Contains(EndPoints[j].devicePath))
				{
					flag2 = true;
				}
			}
		}
		if (flag2 && flag)
		{
			if (DataReceived != null)
			{
				EndPoints[GetFeatureEndPointId].ClearDataEvent();
				EndPoints[GetFeatureEndPointId].DataReceived += DataReceived;
			}
		}
		else
		{
			Close();
		}
		return flag;
	}

	public string GetDeviceId()
	{
		string text = "";
		for (int i = 0; i < EndPoints.Count; i++)
		{
			text = GetDeviceId(EndPoints[i].shortDevicePath, EndPoints[i].devicePath);
			if (text != "")
			{
				break;
			}
		}
		return text;
	}

	public string GetDeviceId(string shortPath, string fullPath)
	{
		string result = "";
		if (!shortPath.Equals("") && !fullPath.Equals(""))
		{
			int num = fullPath.IndexOf(shortPath);
			num += shortPath.Length;
			if (num < fullPath.Length)
			{
				string text = fullPath.Substring(num);
				num = text.IndexOf('&');
				text = text.Substring(num + 1);
				result = text[..text.IndexOf('&')];
			}
		}
		return result;
	}

	public bool WriteFeature(int epId, byte[] data)
	{
		if (epId < EndPoints.Count && EndPoints[epId].IsOpened() && data != null)
		{
			return EndPoints[epId].WriteFeature(data);
		}
		return false;
	}

	public bool WriteFeature(byte[] data)
	{
		return WriteFeature(WriteFeatureEndPointId, data);
	}

	public bool WriteOutputReport(byte[] data)
	{
		if (WriteFeatureEndPointId < EndPoints.Count && EndPoints[WriteFeatureEndPointId].IsOpened() && data != null)
		{
			return EndPoints[WriteFeatureEndPointId].WriteOutputReport(data);
		}
		return false;
	}

	public byte[] GetFeature(int epId, byte reportId, int reportLength)
	{
		if (reportLength == 0)
		{
			return null;
		}
		byte[] array = new byte[reportLength];
		array[0] = reportId;
		if (epId < EndPoints.Count && EndPoints[epId].IsOpened() && EndPoints[epId].GetFeature(array))
		{
			return array;
		}
		return null;
	}

	public byte[] GetFeature(byte reportId, int reportLength)
	{
		return GetFeature(GetFeatureEndPointId, reportId, reportLength);
	}

	public bool IsOpened()
	{
		bool result = true;
		for (int i = 0; i < EndPoints.Count; i++)
		{
			if (!EndPoints[i].IsOpened())
			{
				result = false;
			}
		}
		return result;
	}

	public bool Close()
	{
		bool result = true;
		for (int i = 0; i < EndPoints.Count; i++)
		{
			if (EndPoints[i].CloseDevice())
			{
				result = true;
				continue;
			}
			result = false;
			break;
		}
		return result;
	}

	public HIDD_ATTRIBUTES ReadAttributes()
	{
		HIDD_ATTRIBUTES attributes = default(HIDD_ATTRIBUTES);
		if (Open(null, autoRead: false))
		{
			attributes = EndPoints[0].attributes;
			Close();
		}
		else
		{
			attributes.Size = 0;
			attributes.ProductID = 0;
			attributes.VendorID = 0;
			attributes.VersionNumber = 61440;
		}
		return attributes;
	}

	public bool WriteOutputReport(EventHandler DataReceived, byte[] data)
	{
		if (Open(DataReceived, autoRead: false) && WriteOutputReport(data))
		{
			if (DataReceived != null)
			{
				EndPoints[0].BeginAsyncRead();
			}
			return true;
		}
		return false;
	}
}
