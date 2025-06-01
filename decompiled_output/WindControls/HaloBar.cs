using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WindControls;

public class HaloBar : WindProgressBar
{
	public delegate void ValueChanged(HaloBar sender, double Value);

	private Point oldPoint = default(Point);

	private Point picoldPoint = default(Point);

	private bool isMove = false;

	private int m_BarWidth = 8;

	private bool m_EnableMouse = false;

	private IContainer components = null;

	private HaloButton barHaloButton;

	[Category("自定义")]
	[Description("自定义：类型")]
	public override BAR_STYLE BarStyle => BAR_STYLE.HaloBar;

	public int BarWidth
	{
		get
		{
			return m_BarWidth;
		}
		set
		{
			m_BarWidth = value;
		}
	}

	public bool EnableMouse
	{
		get
		{
			return m_EnableMouse;
		}
		set
		{
			m_EnableMouse = value;
		}
	}

	public override Color TextColor
	{
		get
		{
			return barHaloButton.HaloColor;
		}
		set
		{
			barHaloButton.HaloColor = value;
		}
	}

	public event ValueChanged OnValueChanged;

	public HaloBar()
	{
		InitializeComponent();
	}

	protected override void OnCreateControl()
	{
		((UserControl)this).OnCreateControl();
		BarWidth = ((Control)this).Height / 4;
		((Control)barHaloButton).Height = ((Control)this).Height;
		((Control)barHaloButton).Width = ((Control)this).Height;
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Expected O, but got Unknown
		((Control)this).OnPaint(e);
		e.Graphics.SetGDIHigh();
		Rectangle barRect = GetBarRect();
		int num = BarWidth;
		GraphicsPath roundedRectPath = GraphicsHelper.GetRoundedRectPath(barRect, num, GraphicsHelper.RoundStyle.All);
		e.Graphics.FillPath((Brush)new SolidBrush(Color.FromArgb(255, BarBackColor)), roundedRectPath);
		barRect.Width = base.Value * barRect.Width / base.MaxValue;
		if (barRect.Width < num)
		{
			num = barRect.Width;
			barRect.Height = barRect.Width;
			barRect.Y = (((Control)this).Height - barRect.Height) / 2;
		}
		roundedRectPath = GraphicsHelper.GetRoundedRectPath(barRect, num, GraphicsHelper.RoundStyle.All);
		e.Graphics.FillPath((Brush)new SolidBrush(Color.FromArgb(255, BarForeColor)), roundedRectPath);
		if (!isMove)
		{
			if (base.Value > 0)
			{
				Point location = new Point(0, ((Control)barHaloButton).Location.Y);
				location.X = barRect.Width - ((Control)barHaloButton).Width / 2;
				((Control)barHaloButton).Visible = true;
				((Control)barHaloButton).Location = location;
			}
			else
			{
				((Control)barHaloButton).Visible = false;
			}
		}
	}

	private Rectangle GetBarRect()
	{
		return new Rectangle(0, (((Control)this).Height - BarWidth) / 2, ((Control)this).Width, BarWidth);
	}

	private void PlayButton_MouseDown(object sender, MouseEventArgs e)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Invalid comparison between Unknown and I4
		if (!EnableMouse || (int)e.Button != 1048576)
		{
			return;
		}
		if (!((e.X >= ((Control)barHaloButton).Width / 2 - 4) & (e.X <= ((Control)barHaloButton).Width / 2 + 4) & (e.Y >= ((Control)barHaloButton).Height / 2 - 4) & (e.Y <= ((Control)barHaloButton).Height / 2 + 4)))
		{
			((Control)barHaloButton).Location = new Point(e.X - ((Control)barHaloButton).Width / 2 + ((Control)barHaloButton).Location.X, ((Control)barHaloButton).Location.Y);
			base.Value = base.MaxValue * (((Control)barHaloButton).Location.X / (((Control)this).Width - ((Control)barHaloButton).Width / 2 * 2 - 3));
			if (this.OnValueChanged != null)
			{
				this.OnValueChanged(this, base.Value);
			}
		}
		else
		{
			isMove = true;
			oldPoint = Control.MousePosition;
			picoldPoint = ((Control)barHaloButton).Location;
		}
	}

	private void PlayButton_MouseMove(object sender, MouseEventArgs e)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Invalid comparison between Unknown and I4
		if (EnableMouse && (((int)e.Button == 1048576) & isMove))
		{
			if ((picoldPoint.X + (Control.MousePosition.X - oldPoint.X) < 0) | (picoldPoint.X + (Control.MousePosition.X - oldPoint.X) + ((Control)barHaloButton).Width > ((Control)this).Width - 3))
			{
				((Control)barHaloButton).Location = new Point((picoldPoint.X + (Control.MousePosition.X - oldPoint.X) >= 0) ? (((Control)this).Width - ((Control)barHaloButton).Width / 2 * 2 - 3) : 0, picoldPoint.Y);
				((Control)this).Invalidate();
			}
			else
			{
				((Control)barHaloButton).Location = new Point(picoldPoint.X + (Control.MousePosition.X - oldPoint.X), picoldPoint.Y);
				((Control)this).Invalidate();
			}
		}
	}

	private void PlayButton_MouseUp(object sender, MouseEventArgs e)
	{
		if (EnableMouse && isMove)
		{
			isMove = false;
			base.Value = base.MaxValue * (((Control)barHaloButton).Location.X / (((Control)this).Width - ((Control)barHaloButton).Width / 2 * 2 - 3));
			if (this.OnValueChanged != null)
			{
				this.OnValueChanged(this, base.Value);
			}
			((Control)this).Invalidate();
		}
	}

	private void MusicBar_MouseDown(object sender, MouseEventArgs e)
	{
		if (!EnableMouse)
		{
			return;
		}
		Rectangle barRect = GetBarRect();
		if ((e.X >= barRect.X) & (e.X <= barRect.Width) & (e.Y >= barRect.Y) & (e.Y <= barRect.Height))
		{
			((Control)barHaloButton).Location = new Point(e.X - ((Control)barHaloButton).Width / 2, ((Control)barHaloButton).Location.Y);
			base.Value = base.MaxValue * (((Control)barHaloButton).Location.X / (((Control)this).Width - ((Control)barHaloButton).Width / 2 * 2 - 3));
			if (this.OnValueChanged != null)
			{
				this.OnValueChanged(this, base.Value);
			}
		}
	}

	private void MusicBar_MouseMove(object sender, MouseEventArgs e)
	{
		if (EnableMouse)
		{
			Rectangle barRect = GetBarRect();
			if ((e.X >= barRect.X) & (e.X <= barRect.Width) & (e.Y >= barRect.Y) & (e.Y <= barRect.Height))
			{
				((Control)this).Cursor = Cursors.Hand;
			}
			else
			{
				((Control)this).Cursor = Cursors.Default;
			}
		}
	}

	protected override void OnResize(EventArgs e)
	{
		((UserControl)this).OnResize(e);
		BarWidth = ((Control)this).Height / 4;
		if (barHaloButton != null)
		{
			((Control)barHaloButton).Height = ((Control)this).Height;
			((Control)barHaloButton).Width = ((Control)this).Height;
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Expected O, but got Unknown
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Expected O, but got Unknown
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Expected O, but got Unknown
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Expected O, but got Unknown
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Expected O, but got Unknown
		barHaloButton = new HaloButton();
		((Control)this).SuspendLayout();
		barHaloButton.HaloColor = Color.RoyalBlue;
		barHaloButton.IsActive = true;
		((Control)barHaloButton).Location = new Point(0, 0);
		((Control)barHaloButton).Name = "barHaloButton";
		((Control)barHaloButton).Size = new Size(35, 35);
		((Control)barHaloButton).TabIndex = 0;
		((Control)barHaloButton).Visible = false;
		((Control)barHaloButton).MouseDown += new MouseEventHandler(PlayButton_MouseDown);
		((Control)barHaloButton).MouseMove += new MouseEventHandler(PlayButton_MouseMove);
		((Control)barHaloButton).MouseUp += new MouseEventHandler(PlayButton_MouseUp);
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 12f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Controls.Add((Control)(object)barHaloButton);
		((Control)this).Name = "HaloBar";
		((Control)this).Size = new Size(345, 35);
		((Control)this).MouseDown += new MouseEventHandler(MusicBar_MouseDown);
		((Control)this).MouseMove += new MouseEventHandler(MusicBar_MouseMove);
		((Control)this).ResumeLayout(false);
	}
}
