using System.Drawing;
using System.Windows.Forms;

namespace WindControls;

public class DvButton
{
	private Rectangle ClientRectangle = new Rectangle(0, 0, 0, 0);

	private DVButtonState dVButtonState = DVButtonState.DVButtonNormal;

	private Image normalImage = (Image)(object)FontImages.GetImage("A_fa_window_close", 32, Color.Red, Color.Transparent);

	private Image mouseEnterImage = (Image)(object)FontImages.GetImage("A_fa_window_close", 32, Color.DarkRed, Color.Transparent);

	private Image mouseDownImage = (Image)(object)FontImages.GetImage("A_fa_window_close", 32, Color.IndianRed, Color.Transparent);

	public void Paint(Graphics graphics, Rectangle rectangle)
	{
		ClientRectangle = rectangle;
		ClientRectangle.X += 32;
		if (normalImage != null && mouseEnterImage != null && mouseDownImage != null)
		{
			if (rectangle.Width > normalImage.Width)
			{
				ClientRectangle.Width = normalImage.Width;
			}
			if (rectangle.Height > normalImage.Height)
			{
				ClientRectangle.Height = normalImage.Height;
			}
			if (dVButtonState == DVButtonState.DVButtonNormal)
			{
				graphics.DrawImage(normalImage, ClientRectangle.Location);
			}
			else if (dVButtonState == DVButtonState.DVButtonMouseEnter)
			{
				graphics.DrawImage(mouseEnterImage, ClientRectangle.Location);
			}
			else if (dVButtonState == DVButtonState.DVButtonMouseEnter)
			{
				graphics.DrawImage(mouseDownImage, ClientRectangle.Location);
			}
		}
	}

	public void MouseMove(object sender, DataGridViewCellMouseEventArgs e, Point point)
	{
		WindDataGridView windDataGridView = (WindDataGridView)sender;
		if (Contain(point))
		{
			if (dVButtonState == DVButtonState.DVButtonNormal)
			{
				dVButtonState = DVButtonState.DVButtonMouseEnter;
				((DataGridView)windDataGridView).InvalidateCell(e.ColumnIndex, e.RowIndex);
			}
		}
		else if (dVButtonState != DVButtonState.DVButtonNormal)
		{
			dVButtonState = DVButtonState.DVButtonNormal;
			((DataGridView)windDataGridView).InvalidateCell(e.ColumnIndex, e.RowIndex);
		}
	}

	public void CellMouseLeave(object sender, DataGridViewCellEventArgs e, Point point)
	{
		WindDataGridView windDataGridView = (WindDataGridView)sender;
		dVButtonState = DVButtonState.DVButtonNormal;
		((DataGridView)windDataGridView).InvalidateCell(e.ColumnIndex, e.RowIndex);
	}

	public bool Contain(Point point)
	{
		return ClientRectangle.Contains(point);
	}

	public bool Click(Point point)
	{
		if (Contain(point))
		{
			return true;
		}
		return false;
	}
}
