using System;

namespace USBUpdateTool;

public class DeviceConfig
{
	public string icName = " ";

	public string normalVid = "0000";

	public string normalPid = "0000";

	public string bootVid = "0000";

	public string bootPid = "0000";

	public byte cid = 0;

	public byte mid = 0;

	public bool isDriverPairMode = true;

	public string note = "";

	public DeviceConfig()
	{
	}

	public DeviceConfig(string config)
	{
		ToDevice(config);
	}

	public void SetNote(string _note)
	{
		note = _note;
	}

	public void SetICName(string _icName)
	{
		if (_icName == "")
		{
			_icName = " ";
		}
		icName = _icName;
	}

	public void SetNormalVidPid(string _normalVid, string _normalPid)
	{
		if (_normalVid == "")
		{
			_normalVid = "0000";
		}
		if (_normalPid == "")
		{
			_normalPid = "0000";
		}
		normalVid = _normalVid;
		normalPid = _normalPid;
	}

	public void SetBootVidPid(string _bootVid, string _bootPid)
	{
		if (_bootVid == "")
		{
			_bootVid = "0000";
		}
		if (_bootPid == "")
		{
			_bootPid = "0000";
		}
		bootVid = _bootVid;
		bootPid = _bootPid;
	}

	public void SetCidMid(string _cid, string _mid)
	{
		if (_cid == "")
		{
			_cid = "0";
		}
		if (_mid == "")
		{
			_mid = "0";
		}
		cid = Convert.ToByte(_cid);
		mid = Convert.ToByte(_mid);
	}

	public string ToConfigString()
	{
		string text = icName;
		text = text + AppConfig.splitChar + normalVid;
		text = text + AppConfig.splitChar + normalPid;
		text = text + AppConfig.splitChar + bootVid;
		text = text + AppConfig.splitChar + bootPid;
		text = text + AppConfig.splitChar + cid;
		text = text + AppConfig.splitChar + mid;
		text = ((!isDriverPairMode) ? (text + AppConfig.splitChar + "0") : (text + AppConfig.splitChar + "1"));
		return text + AppConfig.splitChar + note;
	}

	public void ToDevice(string config)
	{
		string[] array = config.Split(new char[1] { AppConfig.splitChar });
		icName = array[0];
		normalVid = array[1];
		normalPid = array[2];
		bootVid = array[3];
		bootPid = array[4];
		cid = Convert.ToByte(array[5]);
		mid = Convert.ToByte(array[6]);
		isDriverPairMode = Convert.ToByte(array[7]) > 0;
		note = array[8];
	}
}
