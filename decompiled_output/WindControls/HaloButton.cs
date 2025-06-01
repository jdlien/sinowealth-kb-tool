using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WindControls;

public class HaloButton : UserControl
{
	private bool IsAdd = true;

	public int EllipseSize = 40;

	private int StartColorA = 0;

	private bool isActive = true;

	public int circleSize = 8;

	private Color m_HaloColor = Color.RoyalBlue;

	private IContainer components = null;

	private Timer timer1;

	public bool IsActive
	{
		get
		{
			return isActive;
		}
		set
		{
			isActive = value;
		}
	}

	public Color HaloColor
	{
		get
		{
			return m_HaloColor;
		}
		set
		{
			m_HaloColor = value;
		}
	}

	public HaloButton()
	{
		InitializeComponent();
		((Control)this).DoubleBuffered = true;
	}

	protected override void OnCreateControl()
	{
		((UserControl)this).OnCreateControl();
		circleSize = ((Control)this).Height / 4;
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Expected O, but got Unknown
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Expected O, but got Unknown
		((Control)this).OnPaint(e);
		EllipseSize = ((Control)this).Width;
		Graphics graphics = e.Graphics;
		graphics.SmoothingMode = (SmoothingMode)2;
		graphics.FillEllipse((Brush)new SolidBrush(HaloColor), (((Control)this).Width - circleSize) / 2, (((Control)this).Height - circleSize) / 2, circleSize, circleSize);
		GraphicsPath val = new GraphicsPath();
		val.AddEllipse(((Control)this).Width / 2 - EllipseSize / 2, ((Control)this).Height / 2 - EllipseSize / 2, EllipseSize, EllipseSize);
		PathGradientBrush val2 = new PathGradientBrush(val);
		val2.SetSigmaBellShape(1f, 1f);
		val2.CenterColor = Color.FromArgb(StartColorA, HaloColor);
		Color[] surroundColors = new Color[1] { Color.FromArgb(0, HaloColor) };
		val2.SurroundColors = surroundColors;
		graphics.FillEllipse((Brush)(object)val2, ((Control)this).Width / 2 - EllipseSize / 2, ((Control)this).Height / 2 - EllipseSize / 2, EllipseSize, EllipseSize);
	}

	private void timer1_Tick(object sender, EventArgs e)
	{
		if (!isActive)
		{
			((Control)this).Invalidate();
			return;
		}
		if (IsAdd)
		{
			StartColorA += 10;
		}
		else
		{
			StartColorA -= 10;
		}
		if (StartColorA >= 255)
		{
			IsAdd = false;
			StartColorA = 255;
		}
		else if (StartColorA <= 0)
		{
			IsAdd = true;
			StartColorA = 0;
		}
		((Control)this).Invalidate();
	}

	private void Test_SizeChanged(object sender, EventArgs e)
	{
		((Control)this).Height = ((Control)this).Width;
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		if ((e.X >= 0) & (e.X <= ((Control)this).Width) & (e.Y >= ((Control)this).Height / 2 - 4) & (e.Y <= ((Control)this).Height / 2 + 4))
		{
			((Control)this).Cursor = Cursors.Hand;
		}
		else
		{
			((Control)this).Cursor = Cursors.Default;
		}
		((Control)this).OnMouseMove(e);
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Invalid comparison between Unknown and I4
		if ((int)e.Button == 1048576)
		{
			isActive = false;
			StartColorA = 0;
		}
		((UserControl)this).OnMouseDown(e);
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Invalid comparison between Unknown and I4
		if ((int)e.Button == 1048576)
		{
			isActive = true;
		}
		((Control)this).OnMouseUp(e);
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
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		components = new Container();
		timer1 = new Timer(components);
		((Control)this).SuspendLayout();
		timer1.Enabled = true;
		timer1.Interval = 75;
		timer1.Tick += timer1_Tick;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 12f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Name = "PlayButton";
		((Control)this).SizeChanged += Test_SizeChanged;
		((Control)this).ResumeLayout(false);
	}
}
