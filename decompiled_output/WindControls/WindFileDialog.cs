using System.Windows.Forms;

namespace WindControls;

public class WindFileDialog
{
	public OpenFileDialog openFileDialog = new OpenFileDialog();

	public SaveFileDialog saveFileDialog = new SaveFileDialog();

	public WindFileDialog()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		((FileDialog)openFileDialog).Title = "选择打开文件";
		((FileDialog)openFileDialog).Filter = "H文件|*.h|所有文件|*.*";
		((FileDialog)openFileDialog).FilterIndex = 1;
		openFileDialog.Multiselect = false;
		((FileDialog)openFileDialog).RestoreDirectory = true;
		((FileDialog)saveFileDialog).Filter = "H文件|*.h|所有文件|*.*";
		((FileDialog)saveFileDialog).FilterIndex = 1;
		((FileDialog)saveFileDialog).RestoreDirectory = true;
	}

	public bool openFileShowDialog()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Invalid comparison between Unknown and I4
		if ((int)((CommonDialog)openFileDialog).ShowDialog() == 1)
		{
			((FileDialog)openFileDialog).InitialDirectory = ((FileDialog)openFileDialog).FileName;
			return true;
		}
		return false;
	}

	public bool saveFileShowDialog()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Invalid comparison between Unknown and I4
		if ((int)((CommonDialog)saveFileDialog).ShowDialog() == 1)
		{
			((FileDialog)saveFileDialog).InitialDirectory = ((FileDialog)saveFileDialog).FileName;
			return true;
		}
		return false;
	}
}
