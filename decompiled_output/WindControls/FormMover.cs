using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WindControls;

internal class FormMover
{
	private Point mPoint = new Point(0, 0);

	private bool mouseDown = false;

	private List<Form> forms = new List<Form>();

	private Control control = null;

	public void AddForm(Form form)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		((Control)form).MouseDown += new MouseEventHandler(Form_MouseDown);
		((Control)form).MouseUp += new MouseEventHandler(Form_MouseUp);
		((Control)form).MouseMove += new MouseEventHandler(Form_MouseMove);
		forms.Add(form);
	}

	public void AddControl(Control _control)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		control = _control;
		control.MouseDown += new MouseEventHandler(Form_MouseDown);
		control.MouseUp += new MouseEventHandler(Form_MouseUp);
		control.MouseMove += new MouseEventHandler(Form_MouseMove);
	}

	private void Form_MouseDown(object sender, MouseEventArgs e)
	{
		mouseDown = true;
		mPoint = new Point(e.X, e.Y);
	}

	private void Form_MouseUp(object sender, MouseEventArgs e)
	{
		mouseDown = false;
	}

	private void Form_MouseMove(object sender, MouseEventArgs e)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Invalid comparison between Unknown and I4
		if ((int)e.Button != 1048576 || !mouseDown)
		{
			return;
		}
		_ = mPoint;
		if (true)
		{
			for (int i = 0; i < forms.Count; i++)
			{
				forms[i].Location = new Point(forms[i].Location.X + e.X - mPoint.X, forms[i].Location.Y + e.Y - mPoint.Y);
			}
			if (forms.Count == 0 && control != null)
			{
				control.Location = new Point(control.Location.X + e.X - mPoint.X, control.Location.Y + e.Y - mPoint.Y);
			}
		}
	}
}
