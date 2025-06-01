using System.Windows.Forms;

namespace WindControls;

internal class ComboBoxMessager : IMessageFilter
{
	private const int WM_LBUTTONDOWN = 513;

	private const int WM_LBUTTONUP = 514;

	private const int WM_RBUTTONDOWN = 516;

	private const int WM_RBUTTONUP = 517;

	private const int WM_MBUTTONDOWN = 519;

	private const int WM_MBUTTONUP = 520;

	private const int WM_XBUTTONDOWN = 523;

	private const int WM_XBUTTONUP = 524;

	private WindComboBox windComboBox;

	public ComboBoxMessager(WindComboBox _windComboBox)
	{
		windComboBox = _windComboBox;
	}

	public bool PreFilterMessage(ref Message m)
	{
		switch (((Message)(ref m)).Msg)
		{
		case 513:
		case 516:
		case 519:
		case 523:
			if (windComboBox.windDropDownBox != null && !windComboBox.windDropDownBox.isInClientRect() && !windComboBox.isInClientRect() && ((Control)windComboBox.windDropDownBox).Visible)
			{
				((Control)windComboBox.windDropDownBox).Visible = false;
			}
			break;
		}
		return false;
	}
}
