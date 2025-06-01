namespace USBUpdateTool;

internal enum Cmd_DeviceState_Param
{
	Earse_Backup_Section = 1,
	Earse_Main_Section = 2,
	Copy_To_Main_Section = 3,
	Check_Backup_CRC_Section = 4,
	Check_Main_CRC_Section = 5,
	Wait_Device_To_Boot_Mode = 32,
	Ugprade_Success = 136
}
