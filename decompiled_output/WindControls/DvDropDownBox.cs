using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace WindControls;

public class DvDropDownBox : Form
{
	private DvComboBox dvComboBox;

	private DataGridViewCell dataGridViewCell;

	private IContainer components = null;

	private WindDataGridView windDataGridView1;

	private DataGridViewTextBoxColumn Column1;

	public DvDropDownBox()
	{
		InitializeComponent();
		((Form)this).ShowInTaskbar = false;
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Control)this).BackgroundImageLayout = (ImageLayout)3;
		((Form)this).StartPosition = (FormStartPosition)4;
	}

	public virtual void SetWidthHeight(int width)
	{
		if (((DataGridView)windDataGridView1).Rows.Count > 0)
		{
			((Control)this).Height = ((DataGridView)windDataGridView1).Rows.Count * ((DataGridView)windDataGridView1).RowTemplate.Height + 4;
			if (((Form)this).Location.Y + ((Control)this).Height > GraphicsHelper.GetScreenHeight())
			{
				((Control)this).Height = GraphicsHelper.GetScreenHeight() - ((Form)this).Location.Y;
			}
		}
		else
		{
			((Control)this).Height = 100;
		}
		((Control)this).Width = width;
		((Control)windDataGridView1).Location = new Point(1, 1);
		((Control)windDataGridView1).Width = width - 4;
		((DataGridView)windDataGridView1).Columns[0].Width = ((Control)windDataGridView1).Width;
	}

	public void ShowBox(DvComboBox _dvComboBox, DataGridViewCell _dataGridViewCell)
	{
		dataGridViewCell = _dataGridViewCell;
		dvComboBox = _dvComboBox;
		((Control)this).Show();
	}

	public virtual void SetSelected(int rowIndex)
	{
		if (rowIndex >= 0 && rowIndex < ((DataGridView)windDataGridView1).RowCount)
		{
			((DataGridViewBand)((DataGridView)windDataGridView1).Rows[0]).Selected = false;
			((DataGridViewBand)((DataGridView)windDataGridView1).Rows[rowIndex]).Selected = true;
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
		if (dataGridViewCell != null && e.RowIndex >= 0 && e.ColumnIndex >= 0)
		{
			string value = ((DataGridView)windDataGridView1).Rows[e.RowIndex].Cells[0].Value.ToString();
			dvComboBox.SelectedIndex = e.RowIndex;
			dataGridViewCell.Value = value;
			((Control)this).Hide();
		}
	}

	private void DvDropDownBox_Paint(object sender, PaintEventArgs e)
	{
	}

	private void DvDropDownBox_Deactivate(object sender, EventArgs e)
	{
		((Control)this).Hide();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((Form)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Expected O, but got Unknown
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Expected O, but got Unknown
		//IL_0288: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Expected O, but got Unknown
		DvControls dvControl = new DvControls();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		windDataGridView1 = new WindDataGridView();
		Column1 = new DataGridViewTextBoxColumn();
		((ISupportInitialize)windDataGridView1).BeginInit();
		((Control)this).SuspendLayout();
		((DataGridView)windDataGridView1).AllowUserToAddRows = false;
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
		((DataGridView)windDataGridView1).RowTemplate.Height = 46;
		((DataGridViewBand)((DataGridView)windDataGridView1).RowTemplate).ReadOnly = true;
		((DataGridView)windDataGridView1).ScrollBars = (ScrollBars)2;
		((Control)windDataGridView1).Size = new Size(138, 182);
		((Control)windDataGridView1).TabIndex = 0;
		((DataGridView)windDataGridView1).CellClick += new DataGridViewCellEventHandler(windDataGridView1_CellClick);
		val.Font = new Font("新宋体", 15f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((DataGridViewBand)Column1).DefaultCellStyle = val;
		((DataGridViewColumn)Column1).HeaderText = "Column1";
		((DataGridViewColumn)Column1).Name = "Column1";
		((DataGridViewBand)Column1).ReadOnly = true;
		Column1.SortMode = (DataGridViewColumnSortMode)0;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 12f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(138, 185);
		((Control)this).Controls.Add((Control)(object)windDataGridView1);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Control)this).Name = "DvDropDownBox";
		((Form)this).StartPosition = (FormStartPosition)0;
		((Control)this).Text = "FormDropDown";
		((Form)this).Deactivate += DvDropDownBox_Deactivate;
		((Control)this).Paint += new PaintEventHandler(DvDropDownBox_Paint);
		((ISupportInitialize)windDataGridView1).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
