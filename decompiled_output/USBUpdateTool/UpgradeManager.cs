using System;
using System.Collections.Generic;
using System.Drawing;
using System.Timers;
using System.Windows.Forms;
using DriverLib;
using WindControls;

namespace USBUpdateTool;

public class UpgradeManager
{
	private enum UpgradeState
	{
		DownLoadFile = 1,
		UpgradeResult,
		UpgradeState
	}

	private enum UpgradeResultParam
	{
		Success = 1,
		TimeOutError,
		DeviceNotMatch,
		MultiNormalDeviceError,
		MultiBootDeviceError,
		NotBootFoundDeviceError,
		NormalUsbError,
		BootUsbError,
		BootDeviceError
	}

	private FormMain formMain;

	public UIManager uiManager;

	public List<byte[]> nextBinFile = new List<byte[]>();

	public int currentUpgradeId = 0;

	public Timer upgradeFailTimer = new Timer();

	public Timer upgradeDelayTimer = new Timer();

	public UpgradeManager(FormMain _formMain)
	{
		formMain = _formMain;
		uiManager = new UIManager(_formMain);
		UsbFinder.StartUsbChanged(onUsbDeviceChanged, 600);
		InitTimer();
	}

	public void InitTimer()
	{
		upgradeFailTimer.Interval = 15000.0;
		upgradeFailTimer.Elapsed += delegate
		{
			upgradeFailTimer.Stop();
			UpgradeHandler(new byte[17]
			{
				7, 2, 2, 255, 254, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0
			});
		};
		upgradeFailTimer.Stop();
		upgradeDelayTimer.Interval = 1000.0;
		upgradeDelayTimer.Elapsed += delegate
		{
			upgradeDelayTimer.Stop();
			((Control)formMain).Invoke((Delegate)(Action)delegate
			{
				for (int i = 0; i < formMain.iCSelect.deviceFileList.Count; i++)
				{
					if (FindDevice(formMain.iCSelect.deviceFileList[i]))
					{
						formMain.StartUpgrade();
					}
				}
			});
		};
		upgradeDelayTimer.Stop();
	}

	public void SetUsbChangedCallBack()
	{
		UsbFinder.SetUsbChangedCallBack(onUsbDeviceChanged, 600);
	}

	public static List<string[]> FindHidDevicesEndPoint(List<string[]> endpointList)
	{
		List<string[]> list = new List<string[]>();
		for (int i = 0; i < endpointList.Count; i++)
		{
			string[] array = UsbFinder.FindHidDevices(endpointList[i][0]);
			string[] array2 = UsbFinder.FindHidDevices(endpointList[i][1]);
			if (array != null && array2 != null && array.Length == array2.Length)
			{
				for (int j = 0; j < array.Length; j++)
				{
					list.Add(new string[2]
					{
						array[j],
						array2[j]
					});
				}
			}
		}
		return list;
	}

	public static List<string[]> FindHidDevicesEndPointOnlyVid(string endpointVid, string endpointPid)
	{
		List<string[]> list = new List<string[]>();
		string[] array = UsbFinder.FindHidDevicesByDefaultDeviceId(endpointVid, endpointPid);
		for (int i = 0; i < array.Length; i++)
		{
			list.Add(new string[2]
			{
				array[i],
				array[i]
			});
		}
		return list;
	}

	public bool FindDevice(DeviceFile deviceFile)
	{
		if (LiteResources.appConfig.fileConfig.exeType == EXE_TYPE.PAIR)
		{
			List<string[]> list = new List<string[]>();
			list = ((!(deviceFile.normalPid == "0000")) ? FindHidDevicesEndPointOnlyVid(deviceFile.normalVid, deviceFile.normalPid) : FindHidDevicesEndPointOnlyVid(deviceFile.normalVid, ""));
			return list.Count > 0;
		}
		List<string[]> list2 = FindHidDevicesEndPoint(deviceFile.bootEndPoints);
		List<string[]> list3 = FindHidDevicesEndPoint(deviceFile.normalEndPoints);
		return list2.Count > 0 || list3.Count > 0;
	}

	public void onUsbDeviceChanged(bool isUsbPluged)
	{
		if (formMain == null || ((Control)formMain).Handle == IntPtr.Zero || formMain.iCSelect == null || uiManager == null || formMain.iCSelect.deviceFileList == null)
		{
			return;
		}
		((Control)formMain).Invoke((Delegate)(Action)delegate
		{
			int num = -1;
			for (int i = 0; i < formMain.iCSelect.deviceFileList.Count; i++)
			{
				if (FindDevice(formMain.iCSelect.deviceFileList[i]))
				{
					num = i;
					if (!uiManager.waitBootMode && !uiManager.isStarted)
					{
						uiManager.AllEnable();
						GetUsbVersion(formMain.iCSelect.deviceFileList[i].normalEndPoints);
						break;
					}
				}
			}
			AppLog.LogOnUsbDeviceChanged(formMain.iCSelect.deviceFileList, num, uiManager.waitBootMode, uiManager.isStarted);
			if (num == -1)
			{
				uiManager.DisableUpgradeButton();
			}
		});
	}

	public bool UpgradeStart(ICSelect iCSelect)
	{
		List<string[]> list = new List<string[]>();
		AppLog.LogMessage("UserClick");
		string[] array = UsbFinder.FindBootDevices(iCSelect.customFile);
		AppLog.LogUpgradeStartBoot(array);
		if (array != null && array.Length == 1)
		{
			if (UsbFinder.UsbUpgrade_Start(iCSelect.customFile, UpgradeHandler, 10000))
			{
				AppLog.LogUpgradeStartBootResult(result: true);
				uiManager.StartFromBootMode();
				return true;
			}
			AppLog.LogUpgradeStartBootResult(result: false);
		}
		else
		{
			if (array != null && array.Length > 1)
			{
				new FormMessageBox("Multiple Dongle! Please remove.").Show((Form)(object)formMain);
				return true;
			}
			list = FindHidDevicesEndPoint(iCSelect.deviceFileList[0].normalEndPoints);
			AppLog.LogUpgradeStartNomal(iCSelect.deviceFileList[0].normalEndPoints, list);
			if (list.Count == 1)
			{
				DeviceInfo deviceInfo = default(DeviceInfo);
				if (LiteResources.appConfig.UpgradeDevice.cid != 0 && LiteResources.appConfig.UpgradeDevice.mid != 0)
				{
					bool flag = LiteResources.appConfig.fileConfig.fileName.ToLower().Contains("keyboard");
					flag |= LiteResources.appConfig.fileConfig.fileName.ToLower().Contains("kb");
					if (!iCSelect.ReadCidMid(flag, out deviceInfo) || deviceInfo.CID != LiteResources.appConfig.UpgradeDevice.cid || deviceInfo.MID != LiteResources.appConfig.UpgradeDevice.mid)
					{
						new FormMessageBox("The Cid or Mid is incorrect!").Show((Form)(object)formMain);
						return false;
					}
				}
				if (iCSelect.customFile == null || iCSelect.customFile.Length == 0)
				{
					new FormMessageBox("upgrade file error!").Show((Form)(object)formMain);
				}
				else
				{
					if (UsbFinder.UsbUpgrade_Start(iCSelect.customFile, UpgradeHandler, 10000))
					{
						AppLog.LogUpgradeStartNomalResult(result: true);
						uiManager.StartFromNormalMode(isVirtualPart: true);
						return true;
					}
					AppLog.LogUpgradeStartNomalResult(result: false);
				}
			}
			else
			{
				new FormMessageBox("Multiple Device! Please remove.").Show((Form)(object)formMain);
			}
		}
		return false;
	}

	private void UpgradeDataHandler(byte[] command)
	{
		if (command[0] == 7)
		{
			upgradeFailTimer.Stop();
			upgradeFailTimer.Start();
			switch ((UpgradeState)command[1])
			{
			case UpgradeState.DownLoadFile:
				uiManager.progressManager.SetValue(command[2]);
				break;
			case UpgradeState.UpgradeResult:
				if (command[2] == 1)
				{
					uiManager.SetResult(isSuccess: true);
					uiManager.progressManager.SetValue(100);
				}
				else
				{
					uiManager.SetResult(isSuccess: false);
					uiManager.progressManager.SetValue(100);
					AppLog.LogUpgradeLog(UsbFinder.UsbUpgrade_GetLogs());
				}
				upgradeFailTimer.Stop();
				formMain.StartFindDevices();
				break;
			}
		}
		AppLog.LogSaveUpgradeCommand(command);
	}

	public void UpgradeHandler(byte[] command)
	{
		((Control)formMain).Invoke((Delegate)(Action)delegate
		{
			UpgradeDataHandler(command);
		});
	}

	private void GetUsbVersion(List<string[]> normalEndPoints)
	{
		List<string[]> list = FindHidDevicesEndPoint(normalEndPoints);
		if (list.Count == 1)
		{
			UsbFinder.GetUsbDeviceAttribute(list[0][0], out var hidd_attributes);
			uiManager.SetUsbVersion(hidd_attributes.VersionNumber);
		}
	}

	public void PairHandler(UsbCommand command, string displayText)
	{
		((Control)formMain).Invoke((Delegate)(Action)delegate
		{
			onPairDataReceived(command, displayText);
		});
	}

	public void onPairDataReceived(UsbCommand command, string displayText)
	{
		switch ((UsbCommandID)command.id)
		{
		case UsbCommandID.DongleEnterPair:
			((Control)formMain.wIButton_Lite_Upgrade).Text = displayText;
			((Control)formMain.wiButton_Close_lite).Enabled = false;
			uiManager.upgradeButtonManager.SetPairState(ButtonState.Upgrading, enable: false);
			break;
		case UsbCommandID.GetPairState:
			if (command.receivedData[0] == 1)
			{
				((Control)formMain.wIButton_Lite_Upgrade).Text = displayText;
			}
			else if (command.receivedData[0] == 2)
			{
				((Control)formMain.wiButton_Close_lite).Enabled = true;
				((Control)formMain.wIButton_Lite_Upgrade).Enabled = true;
				((Control)formMain.wIButton_Lite_Upgrade).Text = displayText;
				((Control)formMain.wIButton_Lite_Upgrade).ForeColor = Color.Red;
				uiManager.upgradeButtonManager.SetPairState(ButtonState.Fail, enable: true);
			}
			else if (command.receivedData[0] == 3 || command.receivedData[0] == 11)
			{
				((Control)formMain.wiButton_Close_lite).Enabled = true;
				((Control)formMain.wIButton_Lite_Upgrade).Enabled = true;
				((Control)formMain.wIButton_Lite_Upgrade).Text = displayText;
				((Control)formMain.wIButton_Lite_Upgrade).ForeColor = Color.Green;
				uiManager.upgradeButtonManager.SetPairState(ButtonState.Success, enable: true);
			}
			break;
		case UsbCommandID.StatusChanged:
			((Control)formMain.wiButton_Close_lite).Enabled = true;
			((Control)formMain.wIButton_Lite_Upgrade).Enabled = true;
			((Control)formMain.wIButton_Lite_Upgrade).Text = displayText;
			((Control)formMain.wIButton_Lite_Upgrade).ForeColor = Color.Red;
			uiManager.upgradeButtonManager.SetPairState(ButtonState.Fail, enable: true);
			break;
		case UsbCommandID.ClearSetting:
			formMain.defaultTimer.Stop();
			new FormMessageBox("Successfully restored factory settings").Show((Form)(object)formMain);
			break;
		case UsbCommandID.WriteFlashData:
		case UsbCommandID.ReadFlashData:
			break;
		}
	}
}
