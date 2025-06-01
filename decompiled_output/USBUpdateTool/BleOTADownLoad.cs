using System;
using System.Windows.Forms;
using USBUpdateTool.UpgradeFile;
using WindControls;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace USBUpdateTool;

public class BleOTADownLoad
{
	private enum DownLoadCommand
	{
		EarseFlash = 1,
		DownLoadFile,
		DownLoadFileCompleted,
		Reset_to_Boot
	}

	private byte[] otaBinFile;

	private BleCore bleCore;

	private ICSelect iCSelect;

	private FileHeader fileHeader = new FileHeader("");

	private byte[] downloadBinFile;

	private FormMain formMain;

	public bool isStarted = false;

	public void Start(FormMain _formMain, BleCore _bleCore, ICSelect _iCSelect)
	{
		formMain = _formMain;
		fileHeader.Parser(_iCSelect.customFile);
		isStarted = false;
		if (fileHeader.Valid)
		{
			bleCore = _bleCore;
			iCSelect = _iCSelect;
			downloadBinFile = new byte[_iCSelect.customFile.Length];
			Array.Copy(_iCSelect.customFile, 0, downloadBinFile, 0, downloadBinFile.Length);
			SendData(DownLoadCommand.EarseFlash);
		}
	}

	private void SendData(DownLoadCommand command)
	{
		byte[] array = new byte[20];
		switch (command)
		{
		case DownLoadCommand.EarseFlash:
			WArray.Set(array, 0, 0, array.Length);
			array[0] = 1;
			array[1] = (byte)((downloadBinFile.Length >> 24) & 0xFF);
			array[2] = (byte)((downloadBinFile.Length >> 16) & 0xFF);
			array[3] = (byte)((downloadBinFile.Length >> 8) & 0xFF);
			array[4] = (byte)(downloadBinFile.Length & 0xFF);
			bleCore.WriteCmd(array);
			ShowUI(0, 0);
			isStarted = true;
			break;
		case DownLoadCommand.DownLoadFile:
		{
			int num = 0;
			int num2 = 20;
			while (num < downloadBinFile.Length)
			{
				if (num + num2 < downloadBinFile.Length)
				{
					Array.Copy(downloadBinFile, num, array, 0, num2);
				}
				else
				{
					array = new byte[downloadBinFile.Length - num];
					Array.Copy(downloadBinFile, num, array, 0, array.Length);
				}
				num += num2;
				bleCore.WriteData(array);
			}
			break;
		}
		case DownLoadCommand.Reset_to_Boot:
			WArray.Set(array, 0, 0, array.Length);
			array[0] = 4;
			bleCore.WriteCmd(array);
			break;
		case DownLoadCommand.DownLoadFileCompleted:
			break;
		}
	}

	public void ReceivedData(GattCharacteristic sender, byte[] data)
	{
		byte[] array = new byte[20];
		switch ((DownLoadCommand)data[0])
		{
		case DownLoadCommand.EarseFlash:
			if (data[1] == 1)
			{
				((Control)formMain).Invoke((Delegate)(Action)delegate
				{
					formMain.upgradeManager.uiManager.StartFromNormalMode(isVirtualPart: true);
				});
				SendData(DownLoadCommand.DownLoadFile);
			}
			else
			{
				ShowUI(2, 100);
			}
			break;
		case DownLoadCommand.DownLoadFile:
		{
			int num = data[1] << 24;
			num |= data[2] << 16;
			num |= data[3] << 8;
			num |= data[4];
			ShowUI(0, num * 100 / downloadBinFile.Length);
			break;
		}
		case DownLoadCommand.DownLoadFileCompleted:
			ShowUI(1, 100);
			SendData(DownLoadCommand.Reset_to_Boot);
			break;
		}
	}

	private byte GetPacetCrc(byte[] array, int start, int end)
	{
		byte b = 0;
		for (int i = start; i < end; i++)
		{
			b += array[i];
		}
		return (byte)(165 - b);
	}

	public void ShowUI(int state, int percent)
	{
		((Control)formMain).Invoke((Delegate)(Action)delegate
		{
			if (state == 1)
			{
				isStarted = false;
				formMain.upgradeManager.uiManager.AllEnable();
				formMain.upgradeManager.uiManager.SetResult(isSuccess: true);
			}
			else if (state == 2)
			{
				isStarted = false;
				formMain.upgradeManager.uiManager.AllEnable();
				formMain.upgradeManager.uiManager.SetResult(isSuccess: false);
			}
			else
			{
				formMain.upgradeManager.uiManager.progressManager.SetValue(percent);
			}
		});
	}
}
