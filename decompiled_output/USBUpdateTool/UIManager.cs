using System;
using System.Windows.Forms;

namespace USBUpdateTool;

public class UIManager
{
	public bool waitBootMode = false;

	public bool isStarted = false;

	public ButtonManager upgradeButtonManager;

	public ProgressManager progressManager;

	private FormMain formMain;

	public UIManager(FormMain _formMain)
	{
		formMain = _formMain;
		upgradeButtonManager = new ButtonManager(_formMain);
		progressManager = new ProgressManager(_formMain);
	}

	private void UIManage(bool enable)
	{
		((Control)formMain).Invoke((Delegate)(Action)delegate
		{
			((Control)formMain.wiButton_Close_lite).Enabled = enable;
			((Control)formMain.wIButton_Lite_Upgrade).Enabled = enable;
		});
	}

	public void AllEnable()
	{
		UIManage(enable: true);
	}

	public void AllDisable()
	{
		UIManage(enable: false);
	}

	public void EnableUpgradeButton()
	{
		((Control)formMain).Invoke((Delegate)(Action)delegate
		{
			((Control)formMain.wibutton_StartUpgrade).Enabled = true;
			((Control)formMain.wIButton_Lite_Upgrade).Enabled = true;
		});
	}

	public void DisableUpgradeButton()
	{
		((Control)formMain).Invoke((Delegate)(Action)delegate
		{
			((Control)formMain.wibutton_StartUpgrade).Enabled = false;
			((Control)formMain.wIButton_Lite_Upgrade).Enabled = false;
		});
	}

	public void StartFromNormalMode(bool isVirtualPart)
	{
		if (isVirtualPart)
		{
			AllDisable();
			progressManager.SetValue(0);
			upgradeButtonManager.SetState(ButtonState.Upgrading, enable: false);
			waitBootMode = true;
			isStarted = true;
		}
	}

	public void StartFromBootMode()
	{
		AllDisable();
		progressManager.SetValue(0);
		upgradeButtonManager.SetState(ButtonState.Upgrading, enable: false);
		isStarted = true;
	}

	public void SetResult(bool isSuccess)
	{
		AllEnable();
		waitBootMode = false;
		isStarted = false;
		progressManager.SetValue(100);
		if (isSuccess)
		{
			upgradeButtonManager.SetState(ButtonState.Success, enable: false);
		}
		else
		{
			upgradeButtonManager.SetState(ButtonState.Fail, enable: false);
		}
	}

	public void SetUsbVersion(ushort version)
	{
		if (LiteResources.appConfig.fileConfig.exeType != EXE_TYPE.PAIR && LiteResources.appConfig.CurFwVersionTextBox.Text != "")
		{
			((Control)formMain).Invoke((Delegate)(Action)delegate
			{
				string text = ((version >> 8) & 0xFF).ToString("X") + "." + (version & 0xFF).ToString("X2");
				((Control)formMain.label_curFwVersion).Text = LiteResources.appConfig.CurFwVersionTextBox.Text + text;
			});
		}
	}
}
