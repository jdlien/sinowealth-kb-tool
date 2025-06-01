using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Timers;
using InTheHand.Net.Bluetooth;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Foundation;

namespace USBUpdateTool;

public class BleOTAUpgrade
{
	private BleCore bleCore = null;

	private BluetoothLEDevice curdevice = null;

	private string _notifyCharacteristicGuid = "0000ffe1-0000-1000-8000-00805f9b34fb";

	private string _writeCmdCharacteristicGuid = "0000ffe2-0000-1000-8000-00805f9b34fb";

	private string _writeDataCharacteristicGuid = "0000ffe3-0000-1000-8000-00805f9b34fb";

	private GattCharacteristic _notifyCharacteristic = null;

	private GattCharacteristic _writeCmdCharacteristic = null;

	private GattCharacteristic _writeDataCharacteristic = null;

	private bool bluetoothRadioEnable = false;

	private bool isStartWatcherChanged = false;

	public FormMain formMain;

	public BleOTADownLoad bleOTADownLoad = new BleOTADownLoad();

	public Timer bluetoothExistTimer = new Timer();

	private byte[] otaBinFile = null;

	private ICSelect iCSelect;

	private string curNormalVid = "";

	private string curNormalPid = "";

	public BleOTAUpgrade()
	{
		reset();
		InitTimer();
	}

	private void reset()
	{
		if (curdevice != null)
		{
			WindowsRuntimeMarshal.RemoveEventHandler<TypedEventHandler<BluetoothLEDevice, object>>((Action<EventRegistrationToken>)curdevice.remove_ConnectionStatusChanged, (TypedEventHandler<BluetoothLEDevice, object>)Curdevice_ConnectionStatusChanged);
			curdevice.Dispose();
		}
		curdevice = null;
		if (bleCore != null)
		{
			bleCore.StopBleDeviceWatcher();
			bleCore.DeviceWatcherChanged -= DeviceWatcherChanged;
			bleCore.CharacteristicAdded -= CharacteristicAdded;
			bleCore.CharacteristicFinish -= CharacteristicFinish;
			bleCore.Recdate -= ReceivedData;
			bleCore.Dispose();
		}
		bleCore = new BleCore();
		bleCore.DeviceWatcherChanged += DeviceWatcherChanged;
		bleCore.CharacteristicAdded += CharacteristicAdded;
		bleCore.CharacteristicFinish += CharacteristicFinish;
		bleCore.Recdate += ReceivedData;
		bluetoothRadioEnable = BluetoothRadio.PrimaryRadio != null;
		isStartWatcherChanged = false;
		if (formMain != null && formMain.upgradeManager != null)
		{
			if (bleOTADownLoad.isStarted)
			{
				bleOTADownLoad.ShowUI(2, 100);
			}
			formMain.upgradeManager.uiManager.DisableUpgradeButton();
		}
	}

	public void SetFormMain(FormMain _formMain)
	{
		formMain = _formMain;
	}

	public void InitTimer()
	{
		bluetoothExistTimer.Interval = 1000.0;
		bluetoothExistTimer.Elapsed += delegate
		{
			BluetoothRadio primaryRadio = BluetoothRadio.PrimaryRadio;
			if (bluetoothRadioEnable != (BluetoothRadio.PrimaryRadio != null))
			{
				bluetoothRadioEnable = !bluetoothRadioEnable;
				if (bluetoothRadioEnable)
				{
					if (curdevice == null)
					{
						StartBleDeviceChanged(enable: true, iCSelect);
					}
				}
				else if (curdevice != null)
				{
					reset();
				}
			}
		};
		bluetoothExistTimer.Stop();
	}

	public void StartBleDeviceChanged(bool enable, ICSelect _iCSelect)
	{
		iCSelect = _iCSelect;
		if (enable)
		{
			if (iCSelect.deviceFileList.Count > 0 && (curNormalVid != iCSelect.deviceFileList[0].normalVid || curNormalPid != iCSelect.deviceFileList[0].normalPid))
			{
				curNormalVid = iCSelect.deviceFileList[0].normalVid;
				curNormalPid = iCSelect.deviceFileList[0].normalPid;
				reset();
			}
			if (curdevice == null && !isStartWatcherChanged)
			{
				bleCore.StartBleDeviceWatcher();
				isStartWatcherChanged = true;
			}
			bluetoothExistTimer.Start();
		}
		else
		{
			bluetoothExistTimer.Stop();
			isStartWatcherChanged = false;
			bleCore.StopBleDeviceWatcher();
		}
		bluetoothRadioEnable = BluetoothRadio.PrimaryRadio != null;
	}

	public void StartOTA(ICSelect iCSelect)
	{
		bleOTADownLoad.Start(formMain, bleCore, iCSelect);
	}

	private void ReceivedData(GattCharacteristic sender, byte[] data)
	{
		bleOTADownLoad.ReceivedData(sender, data);
	}

	private void DeviceWatcherChanged(BluetoothLEDevice currentDevice)
	{
		currentDevice.GetGattServicesAsync().put_Completed((AsyncOperationCompletedHandler<GattDeviceServicesResult>)delegate(IAsyncOperation<GattDeviceServicesResult> asyncInfo, AsyncStatus asyncStatus)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Invalid comparison between Unknown and I4
			if ((int)asyncStatus == 1)
			{
				IReadOnlyList<GattDeviceService> services = asyncInfo.GetResults().Services;
				if (iCSelect != null && iCSelect.deviceFileList.Count == 1 && iCSelect.deviceFileList[0].normalEndPoints.Count > 0)
				{
					try
					{
						string text = services[0].DeviceId.ToLower();
						string value = iCSelect.deviceFileList[0].normalVid + "_pid&" + iCSelect.deviceFileList[0].normalPid + "_";
						if (text.Contains(value))
						{
							curdevice = currentDevice;
							bleCore.StopBleDeviceWatcher();
							bleCore.StartMatching(currentDevice);
							bleCore.FindService();
						}
					}
					catch
					{
					}
				}
			}
		});
	}

	private void CharacteristicAdded(GattCharacteristic gatt)
	{
		if (gatt.Uuid.ToString().ToLower() == _notifyCharacteristicGuid)
		{
			_notifyCharacteristic = gatt;
		}
		else if (gatt.Uuid.ToString().ToLower() == _writeCmdCharacteristicGuid)
		{
			_writeCmdCharacteristic = gatt;
		}
		else if (gatt.Uuid.ToString().ToLower() == _writeDataCharacteristicGuid)
		{
			_writeDataCharacteristic = gatt;
		}
	}

	private void CharacteristicFinish(int size)
	{
		if (size > 0)
		{
			if (_notifyCharacteristic != null && _writeCmdCharacteristic != null && _writeDataCharacteristic != null)
			{
				bleCore.ConnectGattCharacteristic(_notifyCharacteristic, _writeCmdCharacteristic, _writeDataCharacteristic);
				formMain.upgradeManager.uiManager.AllEnable();
				if (curdevice != null)
				{
					BluetoothLEDevice val = curdevice;
					WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<BluetoothLEDevice, object>>((Func<TypedEventHandler<BluetoothLEDevice, object>, EventRegistrationToken>)val.add_ConnectionStatusChanged, (Action<EventRegistrationToken>)val.remove_ConnectionStatusChanged, (TypedEventHandler<BluetoothLEDevice, object>)Curdevice_ConnectionStatusChanged);
				}
			}
		}
		else
		{
			_notifyCharacteristic = null;
			_writeCmdCharacteristic = null;
			_writeDataCharacteristic = null;
		}
	}

	private void Curdevice_ConnectionStatusChanged(BluetoothLEDevice sender, object args)
	{
		reset();
		StartBleDeviceChanged(enable: true, iCSelect);
	}
}
