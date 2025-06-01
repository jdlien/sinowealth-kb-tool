using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace WindControls;

public class DvComboBox
{
	private int mColumnIndex = 0;

	private int mRowIndex = 0;

	private DvComboBoxAlign dvComboBoxAlign = DvComboBoxAlign.Right;

	private Point offsetLoction = new Point(0, 0);

	private Point dropDownBoxOffsetPoint = new Point(0, 0);

	private Rectangle ClientRectangle = new Rectangle(0, 0, 0, 0);

	public DVComboBoxState dVComboBoxState = DVComboBoxState.DVComboBoxNormal;

	private Image normalImage = (Image)(object)FontImages.GetImage("A_fa_caret_down", 32, Color.Red, Color.Transparent);

	private Image mouseEnterImage = (Image)(object)FontImages.GetImage("A_fa_caret_down", 32, Color.DarkRed, Color.Transparent);

	private Image mouseDownImage = (Image)(object)FontImages.GetImage("A_fa_caret_up", 32, Color.IndianRed, Color.Transparent);

	private DvDropDownBox dvDropDownBox = null;

	private List<string> Items = new List<string>();

	public string SelectedText = "";

	private WindDataGridView windDataGridView;

	private int m_SelectedIndex = -1;

	[Category("自定义")]
	[Description("自定义：下拉框")]
	public DvDropDownBox DropDownBox
	{
		get
		{
			return dvDropDownBox;
		}
		set
		{
			dvDropDownBox = value;
		}
	}

	[Category("自定义")]
	[Description("自定义：显示在哪一行")]
	public int RowIndex
	{
		get
		{
			return mRowIndex;
		}
		set
		{
			mRowIndex = value;
		}
	}

	[Category("自定义")]
	[Description("自定义：显示在哪一列")]
	public int ColumnIndex
	{
		get
		{
			return mColumnIndex;
		}
		set
		{
			mColumnIndex = value;
		}
	}

	[Description("自定义：选中哪一项")]
	public int SelectedIndex
	{
		get
		{
			return m_SelectedIndex;
		}
		set
		{
			if (value >= 0 && value < Items.Count)
			{
				SelectedText = Items[value];
			}
			else
			{
				SelectedText = "";
			}
			if (m_SelectedIndex != value)
			{
				if (windDataGridView != null && RowIndex >= 0 && ColumnIndex >= 0 && RowIndex < ((DataGridView)windDataGridView).RowCount && ColumnIndex < ((DataGridView)windDataGridView).ColumnCount)
				{
					((DataGridView)windDataGridView).Rows[RowIndex].Cells[ColumnIndex].Value = SelectedText;
				}
				m_SelectedIndex = value;
			}
		}
	}

	[Category("自定义")]
	[Description("自定义：对齐方式")]
	public DvComboBoxAlign ComboBoxAlign
	{
		get
		{
			return dvComboBoxAlign;
		}
		set
		{
			dvComboBoxAlign = value;
		}
	}

	[Category("自定义")]
	[Description("自定义：正常情况下图片")]
	public Image NormalImage
	{
		get
		{
			return normalImage;
		}
		set
		{
			normalImage = value;
			ClientRectangle.Width = value.Width;
			ClientRectangle.Height = value.Height;
		}
	}

	[Category("自定义")]
	[Description("自定义：鼠标滑过时图片")]
	public Image MouseEnterImage
	{
		get
		{
			return mouseEnterImage;
		}
		set
		{
			mouseEnterImage = value;
			ClientRectangle.Width = value.Width;
			ClientRectangle.Height = value.Height;
		}
	}

	[Category("自定义")]
	[Description("自定义：鼠标按下时图片")]
	public Image MouseDownImage
	{
		get
		{
			return mouseDownImage;
		}
		set
		{
			mouseDownImage = value;
			ClientRectangle.Width = value.Width;
			ClientRectangle.Height = value.Height;
		}
	}

	public DvComboBox(int rowIndex, int columnIndex, Point offset, DvComboBoxAlign align, DvDropDownBox _dvDropDownBox)
	{
		RowIndex = rowIndex;
		ColumnIndex = columnIndex;
		offsetLoction = offset;
		ClientRectangle.Location = offsetLoction;
		ComboBoxAlign = align;
		ClientRectangle.Width = normalImage.Width;
		ClientRectangle.Height = normalImage.Height;
		dvDropDownBox = _dvDropDownBox;
	}

	public void Paint(DataGridViewCellPaintingEventArgs e)
	{
		//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_030d: Expected O, but got Unknown
		if (e.ColumnIndex != ColumnIndex)
		{
			return;
		}
		int x;
		int y;
		if (ComboBoxAlign == DvComboBoxAlign.Left)
		{
			x = e.CellBounds.X;
			y = e.CellBounds.Y;
		}
		else if (ComboBoxAlign == DvComboBoxAlign.Right)
		{
			x = e.CellBounds.Right - ClientRectangle.Width;
			y = e.CellBounds.Y + (e.CellBounds.Height - ClientRectangle.Height) / 2;
		}
		else
		{
			x = offsetLoction.X + e.CellBounds.X;
			y = offsetLoction.Y + e.CellBounds.Y;
		}
		ClientRectangle.Location = new Point(x, y);
		e.Graphics.SetGDIHigh();
		if (dVComboBoxState == DVComboBoxState.DVComboBoxNormal)
		{
			if (normalImage != null)
			{
				e.Graphics.DrawImage(normalImage, ClientRectangle);
			}
		}
		else if (dVComboBoxState == DVComboBoxState.DVComboBoxMouseEnter)
		{
			if (normalImage != null)
			{
				e.Graphics.DrawImage(mouseEnterImage, ClientRectangle);
			}
		}
		else if (dVComboBoxState == DVComboBoxState.DVComboBoxMouseDown && normalImage != null)
		{
			e.Graphics.DrawImage(mouseDownImage, ClientRectangle);
		}
		if (RowIndex >= 0 && ColumnIndex >= 0 && windDataGridView != null)
		{
			SizeF sizeF = e.Graphics.MeasureString(SelectedText, ((Control)windDataGridView).Font);
			Point point = new Point(0, 0);
			point.X = (int)((float)e.CellBounds.X + ((float)e.CellBounds.Width - sizeF.Width) / 2f);
			point.Y = (int)((float)e.CellBounds.Y + ((float)e.CellBounds.Height - sizeF.Height) / 2f);
			if (point.X < e.CellBounds.X)
			{
				point.X = e.CellBounds.X;
			}
			if (point.Y < e.CellBounds.Y)
			{
				point.Y = e.CellBounds.Y;
			}
			e.Graphics.DrawString(SelectedText, ((Control)windDataGridView).Font, (Brush)new SolidBrush(((Control)windDataGridView).ForeColor), (PointF)point);
		}
	}

	public void MouseMove(object sender, DataGridViewCellMouseEventArgs e, Point point)
	{
		windDataGridView = (WindDataGridView)sender;
		if (Contain(point, e.RowIndex, e.ColumnIndex))
		{
			if (dVComboBoxState == DVComboBoxState.DVComboBoxNormal)
			{
				dVComboBoxState = DVComboBoxState.DVComboBoxMouseEnter;
				((DataGridView)windDataGridView).InvalidateCell(e.ColumnIndex, e.RowIndex);
			}
		}
		else if (dVComboBoxState == DVComboBoxState.DVComboBoxMouseEnter)
		{
			dVComboBoxState = DVComboBoxState.DVComboBoxNormal;
			((DataGridView)windDataGridView).InvalidateCell(e.ColumnIndex, e.RowIndex);
		}
	}

	public void CellMouseLeave(object sender, DataGridViewCellEventArgs e, Point point)
	{
		if (dVComboBoxState != DVComboBoxState.DVComboBoxMouseDown)
		{
			windDataGridView = (WindDataGridView)sender;
			dVComboBoxState = DVComboBoxState.DVComboBoxNormal;
			((DataGridView)windDataGridView).InvalidateCell(e.ColumnIndex, e.RowIndex);
		}
	}

	public void CellMouseClick(object sender, DataGridViewCellEventArgs e, Point point)
	{
		if (dVComboBoxState == DVComboBoxState.DVComboBoxMouseDown && ((Control)DropDownBox).Visible)
		{
			windDataGridView = (WindDataGridView)sender;
			dVComboBoxState = DVComboBoxState.DVComboBoxNormal;
			((DataGridView)windDataGridView).InvalidateCell(e.ColumnIndex, e.RowIndex);
			((Control)DropDownBox).Hide();
			return;
		}
		windDataGridView = (WindDataGridView)sender;
		dVComboBoxState = DVComboBoxState.DVComboBoxMouseDown;
		((DataGridView)windDataGridView).InvalidateCell(e.ColumnIndex, e.RowIndex);
		if (DropDownBox != null)
		{
			Rectangle cellDisplayRectangle = ((DataGridView)windDataGridView).GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
			Point location = ((Control)windDataGridView).PointToScreen(cellDisplayRectangle.Location);
			location.Y += cellDisplayRectangle.Height;
			DropDownBox.Clear();
			DropDownBox.Add(Items);
			DropDownBox.SetWidthHeight(cellDisplayRectangle.Width);
			DropDownBox.ShowBox(this, ((DataGridView)windDataGridView).Rows[e.RowIndex].Cells[0]);
			((Form)DropDownBox).Location = location;
			DropDownBox.SetSelected(SelectedIndex);
		}
	}

	public bool Contain(Point point, int rowIndex, int columnIndex)
	{
		return RowIndex == rowIndex && ColumnIndex == columnIndex;
	}

	public void ConnectDropDownBox(DvDropDownBox _dropDownBox, Point point)
	{
		dvDropDownBox = _dropDownBox;
		dropDownBoxOffsetPoint = point;
	}

	public void Add(string[] value)
	{
		for (int i = 0; i < value.Length; i++)
		{
			Items.Add(value[i]);
		}
	}

	public void Add(string value)
	{
		Items.Add(value);
	}

	public void Clear()
	{
		Items.Clear();
	}
}
