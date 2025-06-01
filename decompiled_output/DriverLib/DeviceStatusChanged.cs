namespace DriverLib;

public struct DeviceStatusChanged
{
	public byte isDPIChanged;

	public byte isReportRateChanged;

	public byte isConfigChanged;

	public byte isDPILedChanged;

	public byte isLogoLedChanged;

	public byte isLedBarChanged;

	public byte isBatteryLevelChanged;
}
