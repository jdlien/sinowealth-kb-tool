using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WindControls;

namespace USBUpdateTool;

public class DragControlRect : UserControl
{
	private Control control;

	private Point mPoint = new Point(0, 0);

	private bool mouseDown = false;

	private FormMover formMover = new FormMover();

	private bool changeFont = false;

	private IContainer components = null;

	public DragControlRect()
	{
		InitializeComponent();
		formMover.AddControl((Control)(object)this);
	}

	private void UserControlRect_Paint(object sender, PaintEventArgs e)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		Pen val = new Pen(Color.Red);
		e.Graphics.DrawRectangle(val, new Rectangle(0, 0, ((Control)this).Width - 1, ((Control)this).Height - 1));
		e.Graphics.DrawLine(val, new Point(0, ((Control)this).Height / 2), new Point(((Control)this).Width, ((Control)this).Height / 2));
		e.Graphics.DrawLine(val, new Point(((Control)this).Width / 2, 0), new Point(((Control)this).Width / 2, ((Control)this).Height));
	}

	public void SetControl(Control _control)
	{
		changeFont = false;
		((Control)this).Visible = true;
		control = _control;
		((Control)this).Location = new Point(control.Location.X + control.Width, control.Location.Y + control.Height);
	}

	public void SetFontControl(Control _control)
	{
		changeFont = true;
		((Control)this).Visible = true;
		control = _control;
		((Control)this).Location = new Point(control.Location.X + control.Width, control.Location.Y + control.Height);
	}

	private void DragControlRect_MouseEnter(object sender, EventArgs e)
	{
		((Control)this).Cursor = Cursors.Hand;
	}

	private void DragControlRect_MouseLeave(object sender, EventArgs e)
	{
		((Control)this).Cursor = Cursors.Default;
	}

	private void DragControlRect_MouseMove(object sender, MouseEventArgs e)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Invalid comparison between Unknown and I4
		int num = e.X - mPoint.X;
		int num2 = e.Y - mPoint.Y;
		if ((int)e.Button != 1048576 || !mouseDown)
		{
			return;
		}
		_ = mPoint;
		if (1 == 0)
		{
			return;
		}
		((Control)this).Location = new Point(((Control)this).Location.X + num, ((Control)this).Location.Y + num2);
		if (control == null)
		{
			return;
		}
		if (changeFont)
		{
			if ((num <= 0 || num2 <= 0) && num < 0 && num2 >= 0)
			{
			}
		}
		else
		{
			Control obj = control;
			obj.Width += num;
			Control obj2 = control;
			obj2.Height += num2;
		}
	}

	private void DragControlRect_MouseDown(object sender, MouseEventArgs e)
	{
		mouseDown = true;
		mPoint = new Point(e.X, e.Y);
	}

	private void DragControlRect_MouseUp(object sender, MouseEventArgs e)
	{
		mouseDown = false;
	}

	public void UpdateControl(int deltaX, int deltaY)
	{
		if (control != null)
		{
			Control obj = control;
			obj.Width += deltaX;
			Control obj2 = control;
			obj2.Height += deltaY;
		}
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
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Expected O, but got Unknown
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Expected O, but got Unknown
		((Control)this).SuspendLayout();
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 12f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Name = "DragControlRect";
		((Control)this).Size = new Size(14, 14);
		((Control)this).Paint += new PaintEventHandler(UserControlRect_Paint);
		((Control)this).MouseDown += new MouseEventHandler(DragControlRect_MouseDown);
		((Control)this).MouseEnter += DragControlRect_MouseEnter;
		((Control)this).MouseLeave += DragControlRect_MouseLeave;
		((Control)this).MouseMove += new MouseEventHandler(DragControlRect_MouseMove);
		((Control)this).MouseUp += new MouseEventHandler(DragControlRect_MouseUp);
		((Control)this).ResumeLayout(false);
	}
}
