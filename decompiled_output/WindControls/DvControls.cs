using System.Drawing;
using System.Windows.Forms;

namespace WindControls;

public class DvControls
{
	public virtual DvControlClass GetControlClick(DataGridViewCellEventArgs e, Point point)
	{
		return DvControlClass.NULL;
	}

	public virtual DvControlClass CellClickEvent(object sender, DataGridViewCellEventArgs e, Point point)
	{
		return DvControlClass.NULL;
	}

	public virtual void RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
	{
	}

	public virtual void RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
	{
	}

	public virtual void Paint(DataGridViewCellPaintingEventArgs e)
	{
	}

	public virtual void CellMouseMove(object sender, DataGridViewCellMouseEventArgs e, Point point)
	{
	}

	public virtual void CellMouseLeave(object sender, DataGridViewCellEventArgs e, Point point)
	{
	}
}
