using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WindControls;

public class WindDropDownBox : Panel
{
	protected WindComboBox windComboBox;

	private int highLightRowIndex = -1;

	private IContainer components = null;

	private WindDataGridView windDataGridView1;

	private DataGridViewTextBoxColumn Column1;

	public WindDropDownBox()
	{
		InitializeComponent();
	}

	public bool isInClientRect()
	{
		Point pt = ((Control)this).PointToClient(new Point(Control.MousePosition.X, Control.MousePosition.Y));
		return ((Control)this).ClientRectangle.Contains(pt);
	}

	public virtual void SetWindComboBox(WindComboBox _windComboBox)
	{
		windComboBox = _windComboBox;
		((Control)windComboBox).SizeChanged += WindComboBox_SizeChanged;
		WindComboBox_SizeChanged(null, null);
		((Control)this).BackColor = ((Control)windComboBox).BackColor;
	}

	private void WindComboBox_SizeChanged(object sender, EventArgs e)
	{
		if (windComboBox != null)
		{
			((Control)this).Width = ((Control)windComboBox).Width;
		}
	}

	public virtual void SetWindows(Point point, int width)
	{
		int num = ((((DataGridView)windDataGridView1).Rows.Count > windComboBox.DropDownMaxRowCount) ? (windComboBox.DropDownMaxRowCount * ((DataGridView)windDataGridView1).RowTemplate.Height) : ((((DataGridView)windDataGridView1).Rows.Count != 0) ? (((DataGridView)windDataGridView1).Rows.Count * ((DataGridView)windDataGridView1).RowTemplate.Height) : ((DataGridView)windDataGridView1).RowTemplate.Height));
		((Control)this).Width = width;
		((Control)this).Height = num + windComboBox.FrameWidth * 2 + 4;
		((Control)windDataGridView1).Width = ((Control)this).Width - windComboBox.FrameWidth * 2;
		((DataGridView)windDataGridView1).Columns[0].Width = ((Control)windDataGridView1).Width;
		((Control)windDataGridView1).Height = num + 1;
		((Control)windDataGridView1).Location = new Point(windComboBox.FrameWidth, windComboBox.FrameWidth);
		((Control)this).Location = new Point(point.X, point.Y);
		SetSelected();
	}

	public virtual void SetSelected()
	{
		if (windComboBox.SelectedIndex >= 0 && windComboBox.SelectedIndex < ((DataGridView)windDataGridView1).RowCount)
		{
			((DataGridViewBand)((DataGridView)windDataGridView1).Rows[0]).Selected = false;
			((DataGridViewBand)((DataGridView)windDataGridView1).Rows[windComboBox.SelectedIndex]).Selected = true;
		}
	}

	public virtual void Add(string value)
	{
		int num = ((DataGridView)windDataGridView1).Rows.Add();
		((DataGridView)windDataGridView1).Rows[num].Cells[0].Value = value;
	}

	public virtual void Add(List<string> value)
	{
		for (int i = 0; i < value.Count; i++)
		{
			Add(value[i]);
		}
	}

	public virtual void Clear()
	{
		((DataGridView)windDataGridView1).Rows.Clear();
	}

	private void windDataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
	{
		if (windComboBox != null && e.RowIndex >= 0 && e.ColumnIndex >= 0)
		{
			string text = ((DataGridView)windDataGridView1).Rows[e.RowIndex].Cells[0].Value.ToString();
			windComboBox.DropDownBoxSelected(e.RowIndex, text);
		}
	}

	private void windDataGridView1_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
	{
		if (highLightRowIndex != e.RowIndex)
		{
			int num = highLightRowIndex;
			highLightRowIndex = e.RowIndex;
			if (num >= 0 && num < ((DataGridView)windDataGridView1).RowCount)
			{
				((DataGridView)windDataGridView1).InvalidateRow(num);
			}
			((DataGridView)windDataGridView1).InvalidateRow(highLightRowIndex);
		}
	}

	private void windDataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
	{
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Expected O, but got Unknown
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Expected O, but got Unknown
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Expected O, but got Unknown
		e.Graphics.SetGDIHigh();
		Rectangle rectangle = new Rectangle(e.CellBounds.Location, e.CellBounds.Size);
		rectangle.X--;
		rectangle.Y--;
		rectangle.Height++;
		rectangle.Width++;
		e.Graphics.FillRectangle((Brush)new SolidBrush(((Control)windComboBox).BackColor), rectangle);
		if (highLightRowIndex == e.RowIndex)
		{
			if (((DataGridView)windDataGridView1).DisplayedRowCount(false) == ((DataGridView)windDataGridView1).RowCount)
			{
				rectangle.X += 3;
				rectangle.Y += 3;
				rectangle.Height -= 6;
				rectangle.Width -= 6;
			}
			else
			{
				rectangle.X += 3;
				rectangle.Y += 3;
				rectangle.Height -= 6;
				rectangle.Width -= SystemInformation.VerticalScrollBarWidth + 6;
			}
			e.Graphics.FillRectangle((Brush)new SolidBrush(windComboBox.MovingSelectedBackColor), rectangle);
		}
		if (e.Value != null)
		{
			SizeF sizeF = e.Graphics.MeasureString((string)e.Value, ((Control)windComboBox).Font);
			e.Graphics.DrawString((string)e.Value, ((Control)windComboBox).Font, (Brush)new SolidBrush(((Control)windComboBox).ForeColor), (float)(e.CellBounds.X + 8), (float)e.CellBounds.Y + ((float)e.CellBounds.Height - sizeF.Height) / 2f, StringFormat.GenericDefault);
		}
		((HandledEventArgs)(object)e).Handled = true;
	}

	private void WindDropDownBox_Paint(object sender, PaintEventArgs e)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		Pen val = new Pen(windComboBox.FrameColor, (float)windComboBox.FrameWidth);
		val.Alignment = (PenAlignment)1;
		e.Graphics.DrawRectangle(val, new Rectangle(0, 0, ((Control)this).Width - 1, ((Control)this).Height - 1));
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((Control)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Expected O, but got Unknown
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Expected O, but got Unknown
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Expected O, but got Unknown
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Expected O, but got Unknown
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_026a: Expected O, but got Unknown
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Expected O, but got Unknown
		DvControls dvControl = new DvControls();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		windDataGridView1 = new WindDataGridView();
		Column1 = new DataGridViewTextBoxColumn();
		((ISupportInitialize)windDataGridView1).BeginInit();
		((Control)this).SuspendLayout();
		((DataGridView)windDataGridView1).AllowUserToAddRows = false;
		((DataGridView)windDataGridView1).AllowUserToDeleteRows = false;
		((DataGridView)windDataGridView1).AllowUserToResizeColumns = false;
		((DataGridView)windDataGridView1).AllowUserToResizeRows = false;
		((DataGridView)windDataGridView1).BackgroundColor = Color.White;
		((DataGridView)windDataGridView1).BorderStyle = (BorderStyle)0;
		((DataGridView)windDataGridView1).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)1;
		((DataGridView)windDataGridView1).ColumnHeadersVisible = false;
		((DataGridView)windDataGridView1).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[1] { (DataGridViewColumn)Column1 });
		windDataGridView1.DvControl = dvControl;
		((Control)windDataGridView1).Location = new Point(0, 0);
		((DataGridView)windDataGridView1).MultiSelect = false;
		((Control)windDataGridView1).Name = "windDataGridView1";
		((DataGridView)windDataGridView1).ReadOnly = true;
		((DataGridView)windDataGridView1).RowHeadersBorderStyle = (DataGridViewHeaderBorderStyle)4;
		((DataGridView)windDataGridView1).RowHeadersVisible = false;
		windDataGridView1.RowNumber = -1;
		((DataGridView)windDataGridView1).RowTemplate.Height = 36;
		((DataGridViewBand)((DataGridView)windDataGridView1).RowTemplate).ReadOnly = true;
		((DataGridView)windDataGridView1).ScrollBars = (ScrollBars)2;
		((Control)windDataGridView1).Size = new Size(138, 185);
		((Control)windDataGridView1).TabIndex = 0;
		((DataGridView)windDataGridView1).CellClick += new DataGridViewCellEventHandler(windDataGridView1_CellClick);
		((DataGridView)windDataGridView1).CellMouseEnter += new DataGridViewCellEventHandler(windDataGridView1_CellMouseEnter);
		((DataGridView)windDataGridView1).CellPainting += new DataGridViewCellPaintingEventHandler(windDataGridView1_CellPainting);
		val.Font = new Font("微软雅黑", 9f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((DataGridViewBand)Column1).DefaultCellStyle = val;
		((DataGridViewColumn)Column1).HeaderText = "Column1";
		((DataGridViewColumn)Column1).Name = "Column1";
		((DataGridViewBand)Column1).ReadOnly = true;
		Column1.SortMode = (DataGridViewColumnSortMode)0;
		((Control)this).Controls.Add((Control)(object)windDataGridView1);
		((Control)this).Font = new Font("微软雅黑", 9f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)this).Size = new Size(138, 185);
		((Control)this).Text = "FormDropDown";
		((Control)this).Paint += new PaintEventHandler(WindDropDownBox_Paint);
		((ISupportInitialize)windDataGridView1).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
