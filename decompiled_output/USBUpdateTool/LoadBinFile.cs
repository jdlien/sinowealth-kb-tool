using System.IO;
using System.Windows.Forms;
using WindControls;

namespace USBUpdateTool;

public class LoadBinFile
{
	public string BinFileName = "";

	public string BinFilePathName = "";

	private OpenFileDialog openFileDialog = new OpenFileDialog();

	public LoadBinFile()
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		((FileDialog)openFileDialog).Title = "Open File";
		((FileDialog)openFileDialog).Filter = "Bin Files|*.Bin|fwpkg Files|*.fwpkg";
		((FileDialog)openFileDialog).FilterIndex = 1;
		openFileDialog.Multiselect = false;
		((FileDialog)openFileDialog).RestoreDirectory = true;
	}

	public void LoadFile(string fileName, WindTextBox wTextBox)
	{
		if (File.Exists(fileName))
		{
			BinFilePathName = fileName;
			BinFileName = Path.GetFileNameWithoutExtension(fileName);
			if (wTextBox != null)
			{
				((Control)wTextBox).Text = fileName;
			}
		}
	}

	public bool OpenFile(WindTextBox wTextBox)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Invalid comparison between Unknown and I4
		if ((int)((CommonDialog)openFileDialog).ShowDialog() == 1)
		{
			LoadFile(((FileDialog)openFileDialog).FileName, wTextBox);
			FileManager.SaveUpgradeBinFileName(((FileDialog)openFileDialog).FileName);
			return true;
		}
		return false;
	}

	public void LoadLastFile(WindTextBox wTextBox)
	{
		LoadFile(FileManager.GetUpgradeBinFileName(), wTextBox);
	}
}
