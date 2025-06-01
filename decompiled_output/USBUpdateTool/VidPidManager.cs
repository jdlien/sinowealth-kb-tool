using System;
using WindControls;

namespace USBUpdateTool;

public class VidPidManager
{
	public delegate void VidPidChangedEvent();

	public VidPidChangedEvent OnNormalVidPidChangedEvent;

	public VidPidChangedEvent OnBootVidPidChangedEvent;

	private ICSelect iCSelect = new ICSelect();

	private EditValueControl editNormalVidPid;

	private EditValueControl editBootVidPid;

	private EditValueControl editCidMid;

	public string icName = "";

	private bool normalInitCompleted = false;

	private bool bootInitCompleted = false;

	public void SetEditValueControls(EditValueControl _editNormalVidPid, EditValueControl _editBootVidPid, EditValueControl _editCidMid)
	{
		editNormalVidPid = _editNormalVidPid;
		editBootVidPid = _editBootVidPid;
		editCidMid = _editCidMid;
	}

	private void WcobBox_IC_type_SelectedIndexChanged(object sender, EventArgs e)
	{
	}

	public void Init()
	{
		InitEditNormalVidPid();
		InitEditBootVidPid();
		InitEditCidMid();
	}

	public void Update(string _icName)
	{
		icName = _icName;
		if (editNormalVidPid != null)
		{
			editNormalVidPid.ResetUI();
			normalInitCompleted = false;
			if (editNormalVidPid.OnCheckValueChanged != null)
			{
				editNormalVidPid.OnCheckValueChanged(editNormalVidPid);
			}
			normalInitCompleted = true;
		}
		if (editBootVidPid != null)
		{
			editBootVidPid.ResetUI();
			bootInitCompleted = false;
			if (editBootVidPid.OnCheckValueChanged != null)
			{
				editBootVidPid.OnCheckValueChanged(editBootVidPid);
			}
			bootInitCompleted = true;
		}
		if (editCidMid != null)
		{
			editCidMid.ResetUI();
			if (editCidMid.OnCheckValueChanged != null)
			{
				editCidMid.OnCheckValueChanged(editCidMid);
			}
		}
	}

	private void InitEditNormalVidPid()
	{
		if (editNormalVidPid == null)
		{
			return;
		}
		EditValueControl editValueControl = editNormalVidPid;
		editValueControl.OnValueChanged = (EditValueControl.ValueChangedEvent)Delegate.Combine(editValueControl.OnValueChanged, (EditValueControl.ValueChangedEvent)delegate(EditValueControl sender)
		{
			FileManager.SaveCustomNormalVidPid(icName, sender.GetLeftRightVlaue());
			if (OnNormalVidPidChangedEvent != null)
			{
				OnNormalVidPidChangedEvent();
			}
		});
		string value = "";
		EditValueControl editValueControl2 = editNormalVidPid;
		editValueControl2.OnCheckValueChanged = (EditValueControl.CheckValueChangedEvent)Delegate.Combine(editValueControl2.OnCheckValueChanged, (EditValueControl.CheckValueChangedEvent)delegate(EditValueControl sender)
		{
			bool lgoEnable = AppLog.lgoEnable;
			AppLog.lgoEnable = false;
			if (sender.isChecked)
			{
				value = FileManager.GetCustomNormalVidPid(icName);
				sender.SetValue(value);
			}
			else
			{
				iCSelect.CreateUpgradeFile("", null, icName, "", "", "", "");
				if (iCSelect.deviceFileList.Count > 0)
				{
					sender.SetValue(isChecked: false, iCSelect.deviceFileList[0].normalVid, iCSelect.deviceFileList[0].normalPid);
				}
			}
			if (OnNormalVidPidChangedEvent != null && normalInitCompleted)
			{
				OnNormalVidPidChangedEvent();
			}
			AppLog.lgoEnable = lgoEnable;
		});
		editNormalVidPid.OnCheckValueChanged(editNormalVidPid);
		normalInitCompleted = true;
	}

	private void InitEditBootVidPid()
	{
		if (editBootVidPid == null)
		{
			return;
		}
		EditValueControl editValueControl = editBootVidPid;
		editValueControl.OnValueChanged = (EditValueControl.ValueChangedEvent)Delegate.Combine(editValueControl.OnValueChanged, (EditValueControl.ValueChangedEvent)delegate(EditValueControl sender)
		{
			FileManager.SaveCustomBootVidPid(icName, sender.GetLeftRightVlaue());
			if (OnBootVidPidChangedEvent != null)
			{
				OnBootVidPidChangedEvent();
			}
		});
		string value = "";
		EditValueControl editValueControl2 = editBootVidPid;
		editValueControl2.OnCheckValueChanged = (EditValueControl.CheckValueChangedEvent)Delegate.Combine(editValueControl2.OnCheckValueChanged, (EditValueControl.CheckValueChangedEvent)delegate(EditValueControl sender)
		{
			bool lgoEnable = AppLog.lgoEnable;
			AppLog.lgoEnable = false;
			if (sender.isChecked)
			{
				value = FileManager.GetCustomBootVidPid(icName);
				sender.SetValue(value);
			}
			else
			{
				iCSelect.CreateUpgradeFile("", null, icName, "", "", "", "");
				if (iCSelect.deviceFileList.Count > 0)
				{
					sender.SetValue(isChecked: false, iCSelect.deviceFileList[0].bootVid, iCSelect.deviceFileList[0].bootPid);
				}
			}
			if (OnBootVidPidChangedEvent != null && bootInitCompleted)
			{
				OnBootVidPidChangedEvent();
			}
			AppLog.lgoEnable = lgoEnable;
		});
		editBootVidPid.OnCheckValueChanged(editBootVidPid);
		bootInitCompleted = true;
	}

	private void InitEditCidMid()
	{
		string value = "";
		if (editCidMid != null)
		{
			editCidMid.SetValueMode(4, WindBaseTextBox.TextBoxInputMode.NumberOnly);
			EditValueControl editValueControl = editCidMid;
			editValueControl.OnValueChanged = (EditValueControl.ValueChangedEvent)Delegate.Combine(editValueControl.OnValueChanged, (EditValueControl.ValueChangedEvent)delegate
			{
				FileManager.SaveCidMid(editCidMid.GetVlaue());
			});
			EditValueControl editValueControl2 = editCidMid;
			editValueControl2.OnCheckValueChanged = (EditValueControl.CheckValueChangedEvent)Delegate.Combine(editValueControl2.OnCheckValueChanged, (EditValueControl.CheckValueChangedEvent)delegate(EditValueControl sender)
			{
				value = FileManager.GetCidMid();
				sender.SetValue(value);
			});
			value = FileManager.GetCidMid();
			editCidMid.SetValue(value);
		}
	}
}
