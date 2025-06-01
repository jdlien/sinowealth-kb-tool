using System;
using System.Collections.Generic;
using System.Timers;
using System.Windows.Forms;
using DriverLib;
using WindControls;

namespace USBUpdateTool;

public class PairTool
{
	public delegate void OnPairDataReceived(UsbCommand command, string displayText);

	public static string ButtonInitString = "Pair";

	public OnPairDataReceived onPairDataReceived;

	private int pairTimingCount = 0;

	private Timer pairTimer = new Timer();

	private DeviceFile pairDeviceFile;

	public List<string> dongles = new List<string>();

	public bool isFindPid = false;

	public bool isStarted = false;

	public PairTool()
	{
		InitTimer();
	}

	public void InitTimer()
	{
		pairTimer.Interval = 500.0;
		pairTimer.Elapsed += delegate
		{
			pairTimer.Enabled = true;
			if (pairDeviceFile.driverPairMode)
			{
				UsbServer.ReadDonglePairStatus();
			}
			else
			{
				byte[] pairStatusByFeature = UsbUpgradeFile.GetPairStatusByFeature(dongles[0], 6, 9);
				if (pairStatusByFeature != null)
				{
					UsbCommand command = new UsbCommand
					{
						command = new byte[pairStatusByFeature.Length],
						ReportId = 6,
						id = 6,
						receivedData = new byte[pairStatusByFeature.Length - 2]
					};
					Array.Copy(pairStatusByFeature, 2, command.receivedData, 0, 7);
					onUsbDataReceived(command);
				}
				else
				{
					pairTimer.Stop();
					onUsbDataReceived(new UsbCommand
					{
						command = new byte[9],
						ReportId = 6,
						id = 10,
						receivedData = new byte[7]
					});
				}
			}
		};
		pairTimer.Stop();
		pairTimer.Enabled = false;
	}

	public List<string> FindDongleCount(ICSelect iCSelect, ref int deviceId)
	{
		List<string> list = new List<string>();
		for (int i = 0; i < iCSelect.deviceFileList.Count; i++)
		{
			if (iCSelect.deviceFileList[i].FileType != CXFILE_TYPE.Dongle || iCSelect.deviceFileList[i].normalEndPoints.Count <= 0)
			{
				continue;
			}
			string[] array = (isFindPid ? ((!iCSelect.deviceFileList[i].driverPairMode) ? UsbFinder.FindHidDevices(iCSelect.deviceFileList[i].FeatureEndPoint) : UsbFinder.FindHidDevices(iCSelect.deviceFileList[i].normalEndPoints[0][0])) : ((!iCSelect.deviceFileList[i].driverPairMode) ? UsbFinder.FindHidDevicesByDeviceId(iCSelect.deviceFileList[i].normalVid, "", iCSelect.deviceFileList[i].featureInterfaceId, iCSelect.deviceFileList[i].featureDeviceId) : UsbFinder.FindHidDevicesByDefaultDeviceId(iCSelect.deviceFileList[i].normalVid, "")));
			if (array == null || array.Length == 0)
			{
				continue;
			}
			deviceId = i;
			bool flag = false;
			for (int j = 0; j < array.Length; j++)
			{
				flag = false;
				for (int k = 0; k < list.Count; k++)
				{
					if (list[k] == array[j])
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					list.Add(array[j]);
				}
			}
		}
		return list;
	}

	public List<string> FindDongleCount(ICSelect iCSelect, bool driverPairMode, ref int deviceId)
	{
		List<string> list = new List<string>();
		for (int i = 0; i < iCSelect.deviceFileList.Count; i++)
		{
			if (iCSelect.deviceFileList[i].FileType != CXFILE_TYPE.Dongle || iCSelect.deviceFileList[i].normalEndPoints.Count <= 0)
			{
				continue;
			}
			string[] array = (isFindPid ? ((!driverPairMode) ? UsbFinder.FindHidDevices(iCSelect.deviceFileList[i].FeatureEndPoint) : UsbFinder.FindHidDevices(iCSelect.deviceFileList[i].normalEndPoints[0][0])) : ((!driverPairMode) ? UsbFinder.FindHidDevicesByDeviceId(iCSelect.deviceFileList[i].normalVid, "", iCSelect.deviceFileList[i].featureInterfaceId, iCSelect.deviceFileList[i].featureDeviceId) : UsbFinder.FindHidDevicesByDefaultDeviceId(iCSelect.deviceFileList[i].normalVid, "")));
			if (array == null || array.Length == 0)
			{
				continue;
			}
			deviceId = i;
			bool flag = false;
			for (int j = 0; j < array.Length; j++)
			{
				flag = false;
				for (int k = 0; k < list.Count; k++)
				{
					if (list[k] == array[j])
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					list.Add(array[j]);
				}
			}
		}
		return list;
	}

	public void Start(Form form, ICSelect iCSelect, OnPairDataReceived _onPairDataReceived)
	{
		int deviceId = 0;
		dongles = FindDongleCount(iCSelect, ref deviceId);
		if (dongles.Count > 1)
		{
			new FormMessageBox("Multiple Dongles! Please remove.").Show(form);
		}
		else if (dongles.Count == 1)
		{
			onPairDataReceived = _onPairDataReceived;
			pairDeviceFile = iCSelect.deviceFileList[deviceId];
			if (pairDeviceFile.driverPairMode)
			{
				UsbServer.Start(dongles[0], dongles[0], onUsbDataReceived);
				UsbServer.EnterDonglePairWithCidMid(pairDeviceFile.cid, pairDeviceFile.mid);
			}
			else if (UsbUpgradeFile.EnterPairByFeature(dongles[0], pairDeviceFile.cid, pairDeviceFile.mid))
			{
				onUsbDataReceived(new UsbCommand
				{
					command = new byte[9],
					ReportId = 6,
					id = 5,
					receivedData = new byte[7]
				});
			}
		}
	}

	public bool DefaultSetting(Form form, ICSelect iCSelect, OnPairDataReceived _onPairDataReceived)
	{
		int deviceId = 0;
		dongles = FindDongleCount(iCSelect, driverPairMode: true, ref deviceId);
		if (dongles.Count > 1)
		{
			new FormMessageBox("Multiple Dongles! Please remove.").Show(form);
			return false;
		}
		if (dongles.Count == 1)
		{
			onPairDataReceived = _onPairDataReceived;
			pairDeviceFile = iCSelect.deviceFileList[deviceId];
			UsbServer.Start(dongles[0], dongles[0], onUsbDataReceived);
			UsbServer.SetClearSetting();
			return true;
		}
		return false;
	}

	public void onUsbDataReceived(UsbCommand command)
	{
		string text = "";
		switch ((UsbCommandID)command.id)
		{
		case UsbCommandID.DongleEnterPair:
			pairTimingCount = 0;
			pairTimer.Enabled = true;
			text = "Pairing ";
			UsbServer.ReadDonglePairStatus();
			isStarted = true;
			break;
		case UsbCommandID.GetPairState:
			if (command.receivedData[0] == 1)
			{
				if (++pairTimingCount > 6)
				{
					pairTimingCount = 0;
				}
				text = "Pairing";
				for (int i = 0; i < pairTimingCount; i++)
				{
					text += ".";
				}
				isStarted = true;
			}
			else if (command.receivedData[0] == 2)
			{
				Stop();
				text = "Pair Fail";
			}
			else if (command.receivedData[0] == 3 || command.receivedData[0] == 11)
			{
				Stop();
				text = "Pair Success";
			}
			break;
		case UsbCommandID.StatusChanged:
			Stop();
			text = ButtonInitString;
			break;
		}
		if (onPairDataReceived != null)
		{
			onPairDataReceived(command, text);
		}
	}

	public void Stop()
	{
		pairTimer.Enabled = false;
		isStarted = false;
	}
}
