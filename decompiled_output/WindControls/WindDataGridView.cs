using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace WindControls;

public class WindDataGridView : DataGridView
{
	private DvControls dvControls = new DvControls();

	private int m_rowNumber = -1;

	[Browsable(true)]
	public override Image BackgroundImage { get; set; }

	[Category("自定义")]
	[Description("自定义：控件集")]
	public DvControls DvControl
	{
		get
		{
			return dvControls;
		}
		set
		{
			dvControls = value;
			((Control)this).Invalidate();
		}
	}

	[Category("自定义")]
	[Description("自定义：是否显示行号,-1表示不显示")]
	public int RowNumber
	{
		get
		{
			return m_rowNumber;
		}
		set
		{
			m_rowNumber = value;
		}
	}

	public WindDataGridView()
	{
		InitializeComponent();
		SetDoubleBuffered(setting: true);
	}

	private void WindDataGridView_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
	{
		dvControls.CellMouseLeave(sender, e, ((Control)this).PointToClient(Control.MousePosition));
	}

	private void WindDataGridView_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
	{
		dvControls.CellMouseMove(sender, e, ((Control)this).PointToClient(Control.MousePosition));
	}

	private void WindDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
	{
		if (dvControls.CellClickEvent(sender, e, ((Control)this).PointToClient(Control.MousePosition)) != DvControlClass.NULL)
		{
			((DataGridView)this).InvalidateCell(e.ColumnIndex, e.RowIndex);
		}
	}

	private void WindDataGridView_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
	{
		dvControls.RowsRemoved(sender, e);
	}

	private void WindDataGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
	{
		dvControls.RowsAdded(sender, e);
	}

	public void SetDoubleBuffered(bool setting)
	{
		Type type = ((object)this).GetType();
		PropertyInfo property = type.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
		property.SetValue(this, setting, null);
	}

	protected override void PaintBackground(Graphics graphics, Rectangle clipBounds, Rectangle gridBounds)
	{
		((DataGridView)this).PaintBackground(graphics, clipBounds, gridBounds);
	}

	protected override void OnCellPainting(DataGridViewCellPaintingEventArgs e)
	{
		((DataGridView)this).OnCellPainting(e);
		dvControls.Paint(e);
	}

	private void CellPaint_BackgroundColor(DataGridViewCellPaintingEventArgs e)
	{
	}

	public DvControlClass GetControlClick(DataGridViewCellEventArgs e)
	{
		return dvControls.GetControlClick(e, ((Control)this).PointToClient(Control.MousePosition));
	}

	private void InitializeComponent()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Expected O, but got Unknown
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Expected O, but got Unknown
		((ISupportInitialize)this).BeginInit();
		((Control)this).SuspendLayout();
		((DataGridView)this).RowTemplate.Height = 23;
		((DataGridView)this).CellClick += new DataGridViewCellEventHandler(WindDataGridView_CellClick);
		((DataGridView)this).CellMouseLeave += new DataGridViewCellEventHandler(WindDataGridView_CellMouseLeave);
		((DataGridView)this).CellMouseMove += new DataGridViewCellMouseEventHandler(WindDataGridView_CellMouseMove);
		((DataGridView)this).RowsAdded += new DataGridViewRowsAddedEventHandler(WindDataGridView_RowsAdded);
		((DataGridView)this).RowsRemoved += new DataGridViewRowsRemovedEventHandler(WindDataGridView_RowsRemoved);
		((DataGridView)this).RowStateChanged += new DataGridViewRowStateChangedEventHandler(WindDataGridView_RowStateChanged);
		((ISupportInitialize)this).EndInit();
		((Control)this).ResumeLayout(false);
	}

	private void WindDataGridView_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
	{
		if (RowNumber >= 0)
		{
			((DataGridViewCell)e.Row.HeaderCell).Value = $"{((DataGridViewBand)e.Row).Index + RowNumber}";
		}
	}
}
