using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WindControls;

namespace USBUpdateTool;

public class VidPidControl : UserControl
{
	private IContainer components = null;

	private DataGridView dataGridView1;

	public WindImageButton wibutton_add;

	public WindImageButton wibutton_del;

	private Label label1;

	private DataGridViewTextBoxColumn Column1;

	private DataGridViewTextBoxColumn Column2;

	private DataGridViewTextBoxColumn Column3;

	[Description("自定义：文本是否只读")]
	[Category("PID是否可见")]
	public bool PidVisable
	{
		get
		{
			return ((DataGridViewBand)dataGridView1.Columns[1]).Visible;
		}
		set
		{
			((DataGridViewBand)dataGridView1.Columns[1]).Visible = value;
		}
	}

	[Description("自定义：文本是否只读")]
	[Category("提示文本")]
	public string TipText
	{
		get
		{
			return ((Control)label1).Text;
		}
		set
		{
			((Control)label1).Text = value;
		}
	}

	public VidPidControl()
	{
		InitializeComponent();
	}

	protected override void OnCreateControl()
	{
		((UserControl)this).OnCreateControl();
		SetLabelLoction();
	}

	public void SetVidPid(List<DeviceConfig> deviceConfigList)
	{
		dataGridView1.Rows.Clear();
		if (deviceConfigList != null)
		{
			for (int i = 0; i < deviceConfigList.Count; i++)
			{
				dataGridView1.Rows.Add();
				dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0].Value = deviceConfigList[i].normalVid;
				dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[1].Value = deviceConfigList[i].normalPid;
				dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[2].Value = deviceConfigList[i].note;
			}
		}
	}

	private void wibutton_add_Click(object sender, EventArgs e)
	{
		dataGridView1.Rows.Add();
	}

	public List<DeviceConfig> GetVidPidList()
	{
		List<DeviceConfig> list = new List<DeviceConfig>();
		for (int i = 0; i < dataGridView1.RowCount; i++)
		{
			if (dataGridView1.Rows[i].Cells[0].Value == null || ((!dataGridView1.Rows[i].Cells[1].Visible || dataGridView1.Rows[i].Cells[1].Value == null) && dataGridView1.Rows[i].Cells[1].Visible))
			{
				continue;
			}
			string text = dataGridView1.Rows[i].Cells[0].Value.ToString().ToLower();
			string text2 = dataGridView1.Rows[i].Cells[1].Value.ToString().ToLower();
			if (dataGridView1.Rows[i].Cells[1].Visible)
			{
				text2 = dataGridView1.Rows[i].Cells[1].Value.ToString().ToLower();
			}
			string note = "";
			if (dataGridView1.Rows[i].Cells[2].Value != null)
			{
				note = dataGridView1.Rows[i].Cells[2].Value.ToString();
			}
			if (StringHelper.IsIHexString(text) && StringHelper.IsIHexString(text2) && text.Length == 4 && text2.Length == 4)
			{
				bool flag = false;
				for (int j = 0; j < list.Count; j++)
				{
					if (text == list[j].normalVid && text2 == list[j].normalPid)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					DeviceConfig deviceConfig = new DeviceConfig();
					deviceConfig.normalVid = text;
					deviceConfig.normalPid = text2;
					deviceConfig.note = note;
					deviceConfig.icName = "GenericStandard";
					deviceConfig.isDriverPairMode = false;
					list.Add(deviceConfig);
				}
				continue;
			}
			return null;
		}
		return list;
	}

	private void wibutton_del_Click(object sender, EventArgs e)
	{
		if (dataGridView1.SelectedRows == null || ((BaseCollection)dataGridView1.SelectedRows).Count != 1)
		{
			return;
		}
		int num = ((DataGridViewBand)dataGridView1.SelectedRows[0]).Index;
		dataGridView1.Rows.RemoveAt(num);
		if (dataGridView1.Rows.Count > 0)
		{
			if (num > 0)
			{
				num--;
			}
			((DataGridViewBand)dataGridView1.Rows[num]).Selected = true;
		}
	}

	private void dataGridView1_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
	{
		((DataGridViewCell)e.Row.HeaderCell).Value = $"{((DataGridViewBand)e.Row).Index + 1}";
	}

	private void VidPidControl_SizeChanged(object sender, EventArgs e)
	{
		((Control)wibutton_del).Location = new Point(((Control)this).Width - ((Control)wibutton_del).Width, ((Control)wibutton_del).Location.Y);
		((Control)dataGridView1).Width = ((Control)this).Width - 4;
		if (((DataGridViewBand)dataGridView1.Columns[1]).Visible)
		{
			dataGridView1.Columns[2].Width = ((Control)dataGridView1).Width - 64 - 64 - 80;
		}
		else
		{
			dataGridView1.Columns[2].Width = ((Control)dataGridView1).Width - 64 - 80;
		}
		SetLabelLoction();
	}

	private void SetLabelLoction()
	{
		int num = (((Control)wibutton_del).Location.X - ((Control)wibutton_add).Right - ((Control)label1).Width) / 2;
		((Control)label1).Location = new Point(((Control)wibutton_add).Right + num, ((Control)label1).Location.Y);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Expected O, but got Unknown
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Expected O, but got Unknown
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Expected O, but got Unknown
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Expected O, but got Unknown
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Expected O, but got Unknown
		//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f2: Expected O, but got Unknown
		//IL_0326: Unknown result type (might be due to invalid IL or missing references)
		//IL_0330: Expected O, but got Unknown
		//IL_03fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0408: Expected O, but got Unknown
		//IL_0449: Unknown result type (might be due to invalid IL or missing references)
		//IL_0453: Expected O, but got Unknown
		//IL_046b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0475: Expected O, but got Unknown
		//IL_0515: Unknown result type (might be due to invalid IL or missing references)
		//IL_051f: Expected O, but got Unknown
		//IL_0553: Unknown result type (might be due to invalid IL or missing references)
		//IL_055d: Expected O, but got Unknown
		//IL_0591: Unknown result type (might be due to invalid IL or missing references)
		//IL_059b: Expected O, but got Unknown
		//IL_0805: Unknown result type (might be due to invalid IL or missing references)
		//IL_080f: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(VidPidControl));
		dataGridView1 = new DataGridView();
		wibutton_del = new WindImageButton();
		wibutton_add = new WindImageButton();
		label1 = new Label();
		Column1 = new DataGridViewTextBoxColumn();
		Column2 = new DataGridViewTextBoxColumn();
		Column3 = new DataGridViewTextBoxColumn();
		((ISupportInitialize)dataGridView1).BeginInit();
		((Control)this).SuspendLayout();
		dataGridView1.AllowUserToAddRows = false;
		dataGridView1.BackgroundColor = Color.White;
		dataGridView1.BorderStyle = (BorderStyle)0;
		dataGridView1.ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		dataGridView1.Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[3]
		{
			(DataGridViewColumn)Column1,
			(DataGridViewColumn)Column2,
			(DataGridViewColumn)Column3
		});
		((Control)dataGridView1).Location = new Point(3, 3);
		dataGridView1.MultiSelect = false;
		((Control)dataGridView1).Name = "dataGridView1";
		dataGridView1.RowHeadersWidth = 56;
		dataGridView1.RowTemplate.Height = 23;
		((Control)dataGridView1).Size = new Size(359, 408);
		((Control)dataGridView1).TabIndex = 0;
		dataGridView1.RowStateChanged += new DataGridViewRowStateChangedEventHandler(dataGridView1_RowStateChanged);
		((Control)wibutton_del).BackColor = Color.Transparent;
		((Control)wibutton_del).BackgroundImage = (Image)componentResourceManager.GetObject("wibutton_del.BackgroundImage");
		((Control)wibutton_del).BackgroundImageLayout = (ImageLayout)3;
		wibutton_del.DisableBackColor = Color.Transparent;
		wibutton_del.DisableForeColor = Color.DarkGray;
		wibutton_del.DisableImage = (Image)componentResourceManager.GetObject("wibutton_del.DisableImage");
		((Control)wibutton_del).Font = new Font("微软雅黑", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wibutton_del.FrameMode = GraphicsHelper.RoundStyle.All;
		wibutton_del.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wibutton_del.IconName = "";
		wibutton_del.IconOffset = new Point(0, 0);
		wibutton_del.IconSize = 32;
		((Control)wibutton_del).Location = new Point(282, 415);
		wibutton_del.MouseDownBackColor = Color.Gray;
		wibutton_del.MouseDownForeColor = Color.Black;
		wibutton_del.MouseDownImage = (Image)componentResourceManager.GetObject("wibutton_del.MouseDownImage");
		wibutton_del.MouseEnterBackColor = Color.DarkGray;
		wibutton_del.MouseEnterForeColor = Color.Black;
		wibutton_del.MouseEnterImage = (Image)componentResourceManager.GetObject("wibutton_del.MouseEnterImage");
		wibutton_del.MouseUpBackColor = Color.Transparent;
		wibutton_del.MouseUpForeColor = Color.Black;
		wibutton_del.MouseUpImage = (Image)componentResourceManager.GetObject("wibutton_del.MouseUpImage");
		((Control)wibutton_del).Name = "wibutton_del";
		wibutton_del.Radius = 12;
		((Control)wibutton_del).Size = new Size(80, 30);
		((Control)wibutton_del).TabIndex = 32;
		((Control)wibutton_del).Text = "Del";
		wibutton_del.TextAlignment = StringHelper.TextAlignment.Center;
		wibutton_del.TextDynOffset = new Point(0, 0);
		wibutton_del.TextFixLocation = new Point(0, 0);
		wibutton_del.TextFixLocationEnable = false;
		((Control)wibutton_del).Click += wibutton_del_Click;
		((Control)wibutton_add).BackColor = Color.Transparent;
		((Control)wibutton_add).BackgroundImage = (Image)componentResourceManager.GetObject("wibutton_add.BackgroundImage");
		((Control)wibutton_add).BackgroundImageLayout = (ImageLayout)3;
		wibutton_add.DisableBackColor = Color.Transparent;
		wibutton_add.DisableForeColor = Color.DarkGray;
		wibutton_add.DisableImage = (Image)componentResourceManager.GetObject("wibutton_add.DisableImage");
		((Control)wibutton_add).Font = new Font("微软雅黑", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		wibutton_add.FrameMode = GraphicsHelper.RoundStyle.All;
		wibutton_add.IconBackColorRect = new Rectangle(0, 0, 0, 0);
		wibutton_add.IconName = "";
		wibutton_add.IconOffset = new Point(0, 0);
		wibutton_add.IconSize = 32;
		((Control)wibutton_add).Location = new Point(5, 415);
		wibutton_add.MouseDownBackColor = Color.Gray;
		wibutton_add.MouseDownForeColor = Color.Black;
		wibutton_add.MouseDownImage = (Image)componentResourceManager.GetObject("wibutton_add.MouseDownImage");
		wibutton_add.MouseEnterBackColor = Color.DarkGray;
		wibutton_add.MouseEnterForeColor = Color.Black;
		wibutton_add.MouseEnterImage = (Image)componentResourceManager.GetObject("wibutton_add.MouseEnterImage");
		wibutton_add.MouseUpBackColor = Color.Transparent;
		wibutton_add.MouseUpForeColor = Color.Black;
		wibutton_add.MouseUpImage = (Image)componentResourceManager.GetObject("wibutton_add.MouseUpImage");
		((Control)wibutton_add).Name = "wibutton_add";
		wibutton_add.Radius = 12;
		((Control)wibutton_add).Size = new Size(80, 30);
		((Control)wibutton_add).TabIndex = 31;
		((Control)wibutton_add).Text = "Add";
		wibutton_add.TextAlignment = StringHelper.TextAlignment.Center;
		wibutton_add.TextDynOffset = new Point(0, 0);
		wibutton_add.TextFixLocation = new Point(0, 0);
		wibutton_add.TextFixLocationEnable = false;
		((Control)wibutton_add).Click += wibutton_add_Click;
		((Control)label1).AutoSize = true;
		((Control)label1).BackColor = Color.Transparent;
		((Control)label1).Location = new Point(118, 418);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(55, 21);
		((Control)label1).TabIndex = 33;
		((Control)label1).Text = "label1";
		((DataGridViewColumn)Column1).HeaderText = "Vid";
		Column1.MaxInputLength = 4;
		((DataGridViewColumn)Column1).Name = "Column1";
		((DataGridViewBand)Column1).Resizable = (DataGridViewTriState)2;
		Column1.SortMode = (DataGridViewColumnSortMode)0;
		((DataGridViewColumn)Column1).Width = 64;
		((DataGridViewColumn)Column2).HeaderText = "Pid";
		Column2.MaxInputLength = 4;
		((DataGridViewColumn)Column2).Name = "Column2";
		((DataGridViewBand)Column2).Resizable = (DataGridViewTriState)2;
		Column2.SortMode = (DataGridViewColumnSortMode)0;
		((DataGridViewColumn)Column2).Width = 64;
		((DataGridViewColumn)Column3).HeaderText = "备注";
		((DataGridViewColumn)Column3).Name = "Column3";
		((DataGridViewColumn)Column3).Width = 160;
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)0;
		((Control)this).Controls.Add((Control)(object)label1);
		((Control)this).Controls.Add((Control)(object)wibutton_del);
		((Control)this).Controls.Add((Control)(object)wibutton_add);
		((Control)this).Controls.Add((Control)(object)dataGridView1);
		((Control)this).Font = new Font("微软雅黑", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)this).Name = "VidPidControl";
		((Control)this).Size = new Size(371, 448);
		((Control)this).SizeChanged += VidPidControl_SizeChanged;
		((ISupportInitialize)dataGridView1).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
