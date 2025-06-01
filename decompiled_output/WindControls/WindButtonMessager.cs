using System.Windows.Forms;

namespace WindControls;

internal class WindButtonMessager : IMessageFilter
{
	private const int WM_LBUTTONDOWN = 513;

	private const int WM_LBUTTONUP = 514;

	private const int WM_RBUTTONDOWN = 516;

	private const int WM_RBUTTONUP = 517;

	private const int WM_MBUTTONDOWN = 519;

	private const int WM_MBUTTONUP = 520;

	private const int WM_XBUTTONDOWN = 523;

	private const int WM_XBUTTONUP = 524;

	public bool PreFilterMessage(ref Message m)
	{
		return ((Message)(ref m)).Msg switch
		{
			_ => false, 
		};
	}
}
