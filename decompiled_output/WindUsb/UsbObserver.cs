using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using WindUSB;

namespace WindUsb;

public class UsbObserver
{
	private const int MAX_USB_DEVICES = 64;

	private static WindowsAPI windowsAPI = new WindowsAPI();

	public List<USBDevice> USBDeviceList = new List<USBDevice>();

	public static List<string> GetDeviceList()
	{
		List<string> list = new List<string>();
		list.Clear();
		try
		{
			Guid HIDGuid = Guid.Empty;
			windowsAPI.GetDeviceGuid(ref HIDGuid);
			IntPtr classDevOfHandle = windowsAPI.GetClassDevOfHandle(HIDGuid);
			if (classDevOfHandle != IntPtr.Zero)
			{
				WindowsAPI.SP_DEVICE_INTERFACE_DATA interfaceInfo = default(WindowsAPI.SP_DEVICE_INTERFACE_DATA);
				interfaceInfo.cbSize = Marshal.SizeOf(interfaceInfo);
				for (uint num = 0u; num < 64; num++)
				{
					if (windowsAPI.GetEnumDeviceInterfaces(classDevOfHandle, ref HIDGuid, num, ref interfaceInfo))
					{
						int buffsize = 0;
						windowsAPI.GetDeviceInterfaceDetail(classDevOfHandle, ref interfaceInfo, IntPtr.Zero, ref buffsize);
						IntPtr intPtr = Marshal.AllocHGlobal(buffsize);
						Marshal.StructureToPtr(new WindowsAPI.SP_DEVICE_INTERFACE_DETAIL_DATA
						{
							cbSize = Marshal.SizeOf(typeof(WindowsAPI.SP_DEVICE_INTERFACE_DETAIL_DATA))
						}, intPtr, fDeleteOld: false);
						if (windowsAPI.GetDeviceInterfaceDetail(classDevOfHandle, ref interfaceInfo, intPtr, ref buffsize))
						{
							list.Add(Marshal.PtrToStringAuto((IntPtr)((int)intPtr + 4)));
						}
						Marshal.FreeHGlobal(intPtr);
					}
				}
			}
			windowsAPI.DestroyDeviceInfoList(classDevOfHandle);
		}
		catch
		{
		}
		return list;
	}

	public void AddObserver(string[] EndPointString)
	{
		for (int i = 0; i < USBDeviceList.Count; i++)
		{
			if (EndPointString[0].Equals(USBDeviceList[i].EndPoints[0].shortDevicePath))
			{
				return;
			}
		}
		USBDevice uSBDevice = new USBDevice();
		uSBDevice.AddEndPoint(EndPointString);
		USBDeviceList.Add(uSBDevice);
	}

	public void ClearObserver()
	{
		for (int num = USBDeviceList.Count - 1; num >= 0; num--)
		{
			if (USBDeviceList[num].IsOpened())
			{
				USBDeviceList[num].Close();
			}
			if (!USBDeviceList[num].IsOpened())
			{
				USBDeviceList.RemoveAt(num);
			}
		}
	}

	public List<USBDevice> GetObserver()
	{
		List<string> deviceList = GetDeviceList();
		List<USBDevice> list = new List<USBDevice>();
		for (int i = 0; i < USBDeviceList.Count; i++)
		{
			for (int j = 0; j < deviceList.Count; j++)
			{
				if (deviceList[j].Contains(USBDeviceList[i].EndPoints[0].shortDevicePath))
				{
					list.Add(USBDeviceList[i]);
				}
			}
			if (list.Count == 0)
			{
				USBDeviceList[i].Close();
			}
		}
		return list;
	}

	public List<USBDevice> GetObserver(List<string[]> observerKeyList)
	{
		List<string> deviceList = GetDeviceList();
		List<USBDevice> list = new List<USBDevice>();
		for (int i = 0; i < observerKeyList.Count; i++)
		{
			for (int j = 0; j < deviceList.Count; j++)
			{
				if (!deviceList[j].Contains(observerKeyList[i][0]))
				{
					continue;
				}
				bool flag = false;
				for (int k = 0; k < list.Count; k++)
				{
					string text = observerKeyList[i][0];
					string shortDevicePath = list[k].EndPoints[0].shortDevicePath;
					if (text.Equals(shortDevicePath))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					USBDevice uSBDevice = new USBDevice();
					uSBDevice.AddEndPoint(observerKeyList[i].ToArray());
					list.Add(uSBDevice);
				}
				break;
			}
		}
		return list;
	}
}
