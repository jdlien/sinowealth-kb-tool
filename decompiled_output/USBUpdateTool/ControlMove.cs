using System.Drawing;
using System.Windows.Forms;

namespace USBUpdateTool;

public class ControlMove
{
	private Point startPoint = default(Point);

	private Control control;

	private Rectangle moveRange = default(Rectangle);

	private bool mousedown = false;

	public ControlMove(Control _control, Rectangle rect)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Expected O, but got Unknown
		control = _control;
		moveRange = rect;
		control.MouseDown += new MouseEventHandler(Control_MouseDown);
		control.MouseMove += new MouseEventHandler(Control_MouseMove);
		control.MouseUp += new MouseEventHandler(Control_MouseUp);
	}

	public void SetMoveRange(Rectangle rect)
	{
		moveRange = rect;
	}

	private void Control_MouseMove(object sender, MouseEventArgs e)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Invalid comparison between Unknown and I4
		if ((int)e.Button == 1048576 && mousedown)
		{
			Point location = new Point
			{
				X = control.Left + e.X - startPoint.X,
				Y = control.Top + e.Y - startPoint.Y
			};
			if (location.X > moveRange.X && location.Y > moveRange.Y && location.X + control.Width < moveRange.X + moveRange.Width && location.Y + control.Height < moveRange.Y + moveRange.Height)
			{
				control.Location = location;
			}
		}
	}

	private void Control_MouseDown(object sender, MouseEventArgs e)
	{
		mousedown = true;
		startPoint.X = e.X;
		startPoint.Y = e.Y;
	}

	private void Control_MouseUp(object sender, MouseEventArgs e)
	{
		mousedown = false;
	}
}
