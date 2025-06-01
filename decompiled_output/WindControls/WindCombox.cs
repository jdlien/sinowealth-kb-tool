using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace WindControls;

public class WindCombox : ComboBox
{
	private IContainer components = null;

	public WindCombox()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		InitializeComponent();
		((ComboBox)this).DrawItem += new DrawItemEventHandler(WindComboBox_DrawItem);
		((Control)this).SetStyle((ControlStyles)65552, true);
		((Control)this).SetStyle((ControlStyles)4, false);
		((ComboBox)this).DrawMode = (DrawMode)2;
		((ComboBox)this).AutoCompleteMode = (AutoCompleteMode)3;
		((ComboBox)this).AutoCompleteSource = (AutoCompleteSource)256;
	}

	private void WindComboBox_DrawItem(object sender, DrawItemEventArgs e)
	{
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Expected O, but got Unknown
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Invalid comparison between Unknown and I4
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected O, but got Unknown
		if (e.Index >= 0)
		{
			e.Graphics.FillRectangle((Brush)new SolidBrush(Color.Gray), e.Bounds);
			if ((e.State & 1) == 1)
			{
				e.Graphics.FillRectangle((Brush)new SolidBrush(Color.Red), e.Bounds);
			}
			e.Graphics.DrawString(((ComboBox)this).Items[e.Index].ToString(), ((Control)this).Font, Brushes.Black, (RectangleF)e.Bounds);
			e.DrawFocusRectangle();
		}
		else
		{
			e.Graphics.FillRectangle((Brush)new SolidBrush(Color.Blue), e.Bounds);
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((ComboBox)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		components = new Container();
	}
}
