using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using InTheHand.Net.Bluetooth;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Foundation;
using Windows.Security.Cryptography;

public class BleCore
{
	public delegate void DeviceWatcherChangedEvent(BluetoothLEDevice bluetoothLEDevice);

	public delegate void CharacteristicFinishEvent(int size);

	public delegate void CharacteristicAddedEvent(GattCharacteristic gattCharacteristic);

	public delegate void RecDataEvent(GattCharacteristic sender, byte[] data);

	private bool asyncLock = false;

	private const GattClientCharacteristicConfigurationDescriptorValue CHARACTERISTIC_NOTIFICATION_TYPE = (GattClientCharacteristicConfigurationDescriptorValue)1;

	private BluetoothLEAdvertisementWatcher Watcher = null;

	public GattDeviceService CurrentService { get; private set; }

	public BluetoothLEDevice CurrentDevice { get; private set; }

	public GattCharacteristic CurrentWriteCmdCharacteristic { get; private set; }

	public GattCharacteristic CurrentWriteDataCharacteristic { get; private set; }

	public GattCharacteristic CurrentNotifyCharacteristic { get; private set; }

	public List<BluetoothLEDevice> DeviceList { get; private set; }

	private string CurrentDeviceMAC { get; set; }

	public event DeviceWatcherChangedEvent DeviceWatcherChanged;

	public event CharacteristicFinishEvent CharacteristicFinish;

	public event CharacteristicAddedEvent CharacteristicAdded;

	public event RecDataEvent Recdate;

	public BleCore()
	{
		DeviceList = new List<BluetoothLEDevice>();
	}

	public bool StartBleDeviceWatcher()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		BluetoothRadio primaryRadio = BluetoothRadio.PrimaryRadio;
		if (primaryRadio == null)
		{
			return false;
		}
		Watcher = new BluetoothLEAdvertisementWatcher();
		Watcher.put_ScanningMode((BluetoothLEScanningMode)1);
		Watcher.SignalStrengthFilter.put_InRangeThresholdInDBm((short?)(short)(-80));
		Watcher.SignalStrengthFilter.put_OutOfRangeThresholdInDBm((short?)(short)(-90));
		BluetoothLEAdvertisementWatcher watcher = Watcher;
		WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<BluetoothLEAdvertisementWatcher, BluetoothLEAdvertisementReceivedEventArgs>>((Func<TypedEventHandler<BluetoothLEAdvertisementWatcher, BluetoothLEAdvertisementReceivedEventArgs>, EventRegistrationToken>)watcher.add_Received, (Action<EventRegistrationToken>)watcher.remove_Received, (TypedEventHandler<BluetoothLEAdvertisementWatcher, BluetoothLEAdvertisementReceivedEventArgs>)OnAdvertisementReceived);
		Watcher.SignalStrengthFilter.put_OutOfRangeTimeout((TimeSpan?)TimeSpan.FromMilliseconds(5000.0));
		Watcher.SignalStrengthFilter.put_SamplingInterval((TimeSpan?)TimeSpan.FromMilliseconds(2000.0));
		Watcher.Start();
		return true;
	}

	public void StopBleDeviceWatcher()
	{
		if (Watcher != null)
		{
			Watcher.Stop();
		}
	}

	public void Dispose()
	{
		CurrentDeviceMAC = null;
		if (CurrentService != null)
		{
			CurrentService.Dispose();
		}
		if (CurrentDevice != null)
		{
			CurrentDevice.Dispose();
		}
		CurrentDevice = null;
		CurrentService = null;
		CurrentWriteCmdCharacteristic = null;
		CurrentWriteDataCharacteristic = null;
		CurrentNotifyCharacteristic = null;
		this.Recdate = null;
	}

	public void StartMatching(BluetoothLEDevice Device)
	{
		CurrentDevice = Device;
	}

	public bool WriteCmd(byte[] data)
	{
		string text = "";
		if (CurrentWriteCmdCharacteristic != null)
		{
			CurrentWriteCmdCharacteristic.WriteValueAsync(CryptographicBuffer.CreateFromByteArray(data), (GattWriteOption)1).put_Completed((AsyncOperationCompletedHandler<GattCommunicationStatus>)delegate(IAsyncOperation<GattCommunicationStatus> asyncInfo, AsyncStatus asyncStatus)
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0003: Invalid comparison between Unknown and I4
				if ((int)asyncStatus != 1)
				{
				}
			});
			return true;
		}
		return false;
	}

	public bool WriteData(byte[] data)
	{
		string text = "";
		if (CurrentWriteDataCharacteristic != null)
		{
			CurrentWriteDataCharacteristic.WriteValueAsync(CryptographicBuffer.CreateFromByteArray(data), (GattWriteOption)1).put_Completed((AsyncOperationCompletedHandler<GattCommunicationStatus>)delegate(IAsyncOperation<GattCommunicationStatus> asyncInfo, AsyncStatus asyncStatus)
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0003: Invalid comparison between Unknown and I4
				if ((int)asyncStatus != 1)
				{
				}
			});
			return true;
		}
		return false;
	}

	public void FindService()
	{
		CurrentDevice.GetGattServicesAsync().put_Completed((AsyncOperationCompletedHandler<GattDeviceServicesResult>)delegate(IAsyncOperation<GattDeviceServicesResult> asyncInfo, AsyncStatus asyncStatus)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Invalid comparison between Unknown and I4
			if ((int)asyncStatus == 1)
			{
				IReadOnlyList<GattDeviceService> services = asyncInfo.GetResults().Services;
				foreach (GattDeviceService item in services)
				{
					FindCharacteristic(item);
				}
				this.CharacteristicFinish(services.Count);
			}
		});
	}

	public void SelectDeviceFromIdAsync(string MAC)
	{
		CurrentDeviceMAC = MAC;
		CurrentDevice = null;
		BluetoothAdapter.GetDefaultAsync().put_Completed((AsyncOperationCompletedHandler<BluetoothAdapter>)delegate(IAsyncOperation<BluetoothAdapter> asyncInfo, AsyncStatus asyncStatus)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Invalid comparison between Unknown and I4
			if ((int)asyncStatus == 1)
			{
				BluetoothAdapter results = asyncInfo.GetResults();
				byte[] bytes = BitConverter.GetBytes(results.BluetoothAddress);
				Array.Reverse((Array)bytes);
				string text = BitConverter.ToString(bytes, 2, 6).Replace('-', ':').ToLower();
				string id = "BluetoothLE#BluetoothLE" + text + "-" + MAC;
				Matching(id);
			}
		});
	}

	public void Log(string msg)
	{
	}

	public void SetOpteron(GattCharacteristic gattCharacteristic)
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Invalid comparison between Unknown and I4
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Invalid comparison between Unknown and I4
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Invalid comparison between Unknown and I4
		byte[] bytes = BitConverter.GetBytes(CurrentDevice.BluetoothAddress);
		Array.Reverse((Array)bytes);
		CurrentDeviceMAC = BitConverter.ToString(bytes, 2, 6).Replace('-', ':').ToLower();
		string msg = "正在连接设备<" + CurrentDeviceMAC + ">..";
		Log(msg);
		if ((int)gattCharacteristic.CharacteristicProperties == 8)
		{
			CurrentWriteCmdCharacteristic = gattCharacteristic;
		}
		if ((int)gattCharacteristic.CharacteristicProperties == 16)
		{
			CurrentNotifyCharacteristic = gattCharacteristic;
		}
		if ((int)gattCharacteristic.CharacteristicProperties == 26)
		{
		}
		CurrentWriteCmdCharacteristic = gattCharacteristic;
		CurrentNotifyCharacteristic = gattCharacteristic;
		CurrentNotifyCharacteristic.put_ProtectionLevel((GattProtectionLevel)0);
		GattCharacteristic currentNotifyCharacteristic = CurrentNotifyCharacteristic;
		WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<GattCharacteristic, GattValueChangedEventArgs>>((Func<TypedEventHandler<GattCharacteristic, GattValueChangedEventArgs>, EventRegistrationToken>)currentNotifyCharacteristic.add_ValueChanged, (Action<EventRegistrationToken>)currentNotifyCharacteristic.remove_ValueChanged, (TypedEventHandler<GattCharacteristic, GattValueChangedEventArgs>)Characteristic_ValueChanged);
		BluetoothLEDevice currentDevice = CurrentDevice;
		WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<BluetoothLEDevice, object>>((Func<TypedEventHandler<BluetoothLEDevice, object>, EventRegistrationToken>)currentDevice.add_ConnectionStatusChanged, (Action<EventRegistrationToken>)currentDevice.remove_ConnectionStatusChanged, (TypedEventHandler<BluetoothLEDevice, object>)CurrentDevice_ConnectionStatusChanged);
		EnableNotifications(CurrentNotifyCharacteristic);
	}

	public void ConnectGattCharacteristic(GattCharacteristic NotifyCharacteristic, GattCharacteristic WriteCmdCharacteristic, GattCharacteristic WriteDataCharacteristic)
	{
		byte[] bytes = BitConverter.GetBytes(CurrentDevice.BluetoothAddress);
		Array.Reverse((Array)bytes);
		CurrentDeviceMAC = BitConverter.ToString(bytes, 2, 6).Replace('-', ':').ToLower();
		string msg = "正在连接设备<" + CurrentDeviceMAC + ">..";
		Log(msg);
		CurrentWriteCmdCharacteristic = WriteCmdCharacteristic;
		CurrentWriteDataCharacteristic = WriteDataCharacteristic;
		CurrentNotifyCharacteristic = NotifyCharacteristic;
		CurrentNotifyCharacteristic.put_ProtectionLevel((GattProtectionLevel)0);
		GattCharacteristic currentNotifyCharacteristic = CurrentNotifyCharacteristic;
		WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<GattCharacteristic, GattValueChangedEventArgs>>((Func<TypedEventHandler<GattCharacteristic, GattValueChangedEventArgs>, EventRegistrationToken>)currentNotifyCharacteristic.add_ValueChanged, (Action<EventRegistrationToken>)currentNotifyCharacteristic.remove_ValueChanged, (TypedEventHandler<GattCharacteristic, GattValueChangedEventArgs>)Characteristic_ValueChanged);
		BluetoothLEDevice currentDevice = CurrentDevice;
		WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<BluetoothLEDevice, object>>((Func<TypedEventHandler<BluetoothLEDevice, object>, EventRegistrationToken>)currentDevice.add_ConnectionStatusChanged, (Action<EventRegistrationToken>)currentDevice.remove_ConnectionStatusChanged, (TypedEventHandler<BluetoothLEDevice, object>)CurrentDevice_ConnectionStatusChanged);
		EnableNotifications(CurrentNotifyCharacteristic);
	}

	private void OnAdvertisementReceived(BluetoothLEAdvertisementWatcher watcher, BluetoothLEAdvertisementReceivedEventArgs eventArgs)
	{
		BluetoothLEDevice.FromBluetoothAddressAsync(eventArgs.BluetoothAddress).put_Completed((AsyncOperationCompletedHandler<BluetoothLEDevice>)delegate(IAsyncOperation<BluetoothLEDevice> asyncInfo, AsyncStatus asyncStatus)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Invalid comparison between Unknown and I4
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Invalid comparison between Unknown and I4
			if ((int)asyncStatus == 1 && asyncInfo.GetResults() != null)
			{
				BluetoothLEDevice results = asyncInfo.GetResults();
				bool flag = false;
				if ((int)results.ConnectionStatus == 1)
				{
					for (int i = 0; i < DeviceList.Count; i++)
					{
						if (DeviceList[i].BluetoothAddress == results.BluetoothAddress)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						DeviceList.Add(results);
						this.DeviceWatcherChanged(results);
					}
				}
			}
		});
	}

	private void FindCharacteristic(GattDeviceService gattDeviceService)
	{
		CurrentService = gattDeviceService;
		CurrentService.GetCharacteristicsAsync().put_Completed((AsyncOperationCompletedHandler<GattCharacteristicsResult>)delegate(IAsyncOperation<GattCharacteristicsResult> asyncInfo, AsyncStatus asyncStatus)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Invalid comparison between Unknown and I4
			if ((int)asyncStatus == 1)
			{
				IReadOnlyList<GattCharacteristic> characteristics = asyncInfo.GetResults().Characteristics;
				int count = characteristics.Count;
				foreach (GattCharacteristic item in characteristics)
				{
					this.CharacteristicAdded(item);
				}
			}
		});
	}

	private unsafe void Matching(string Id)
	{
		try
		{
			BluetoothLEDevice.FromIdAsync(Id).put_Completed((AsyncOperationCompletedHandler<BluetoothLEDevice>)delegate(IAsyncOperation<BluetoothLEDevice> asyncInfo, AsyncStatus asyncStatus)
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0003: Invalid comparison between Unknown and I4
				//IL_002c: Unknown result type (might be due to invalid IL or missing references)
				//IL_002e: Invalid comparison between Unknown and I4
				//IL_004a: Unknown result type (might be due to invalid IL or missing references)
				//IL_004c: Invalid comparison between Unknown and I4
				//IL_0068: Unknown result type (might be due to invalid IL or missing references)
				//IL_006a: Invalid comparison between Unknown and I4
				if ((int)asyncStatus == 1)
				{
					BluetoothLEDevice results = asyncInfo.GetResults();
					DeviceList.Add(results);
					Log(results.Name);
				}
				if ((int)asyncStatus == 0)
				{
					Log(((object)(*(AsyncStatus*)(&asyncStatus))/*cast due to .constrained prefix*/).ToString());
				}
				if ((int)asyncStatus == 2)
				{
					Log(((object)(*(AsyncStatus*)(&asyncStatus))/*cast due to .constrained prefix*/).ToString());
				}
				if ((int)asyncStatus == 3)
				{
					Log(((object)(*(AsyncStatus*)(&asyncStatus))/*cast due to .constrained prefix*/).ToString());
				}
			});
		}
		catch (Exception)
		{
			StartBleDeviceWatcher();
		}
	}

	private void CurrentDevice_ConnectionStatusChanged(BluetoothLEDevice sender, object args)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		if ((int)sender.ConnectionStatus == 0 && CurrentDeviceMAC != null)
		{
			if (!asyncLock)
			{
				asyncLock = true;
			}
		}
		else if (!asyncLock)
		{
			asyncLock = true;
			Log("设备已连接");
		}
	}

	private unsafe void EnableNotifications(GattCharacteristic characteristic)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		Log("收通知对象=" + CurrentDevice.Name + ":" + ((object)CurrentDevice.ConnectionStatus/*cast due to .constrained prefix*/).ToString());
		characteristic.WriteClientCharacteristicConfigurationDescriptorAsync((GattClientCharacteristicConfigurationDescriptorValue)1).put_Completed((AsyncOperationCompletedHandler<GattCommunicationStatus>)delegate(IAsyncOperation<GattCommunicationStatus> asyncInfo, AsyncStatus asyncStatus)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Invalid comparison between Unknown and I4
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Invalid comparison between Unknown and I4
			if ((int)asyncStatus == 1)
			{
				GattCommunicationStatus results = asyncInfo.GetResults();
				if ((int)results == 1)
				{
					Log("设备不可用");
					if (CurrentNotifyCharacteristic != null && !asyncLock)
					{
						EnableNotifications(CurrentNotifyCharacteristic);
					}
				}
				else
				{
					asyncLock = false;
					Log("设备连接状态" + ((object)(*(GattCommunicationStatus*)(&results))/*cast due to .constrained prefix*/).ToString());
				}
			}
		});
	}

	private void Characteristic_ValueChanged(GattCharacteristic sender, GattValueChangedEventArgs args)
	{
		byte[] data = default(byte[]);
		CryptographicBuffer.CopyToByteArray(args.CharacteristicValue, ref data);
		if (this.Recdate != null)
		{
			this.Recdate(sender, data);
		}
	}
}
