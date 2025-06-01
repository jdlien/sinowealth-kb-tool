using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WindControls;

public class CellBar : WindProgressBar
{
	private int m_GridWidth = 8;

	private int m_IntervalWidth = 4;

	private IContainer components = null;

	[Category("自定义")]
	[Description("自定义：类型")]
	public override BAR_STYLE BarStyle => BAR_STYLE.CellBar;

	[Category("自定义")]
	[Description("自定义：进度条内每个小格的宽度")]
	public int GridWidth
	{
		get
		{
			return m_GridWidth;
		}
		set
		{
			m_GridWidth = value;
		}
	}

	[Category("自定义")]
	[Description("自定义：进度条内每个小格之间宽度")]
	public int IntervalWidth
	{
		get
		{
			return m_IntervalWidth;
		}
		set
		{
			m_IntervalWidth = value;
		}
	}

	public CellBar()
	{
		InitializeComponent();
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Expected O, but got Unknown
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Expected O, but got Unknown
		((Control)this).OnPaint(e);
		e.Graphics.SetGDIHigh();
		Rectangle rect = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
		int height = ((Control)this).Height;
		GraphicsPath roundedRectPath = GraphicsHelper.GetRoundedRectPath(rect, height, GraphicsHelper.RoundStyle.All);
		e.Graphics.FillPath((Brush)new SolidBrush(Color.FromArgb(255, BarBackColor)), roundedRectPath);
		int num = (((Control)this).Width - height - GridWidth) / (GridWidth + IntervalWidth) + 1;
		int num2 = base.Value * num / base.MaxValue;
		int num3 = height / 2;
		int height2 = ((Control)this).Height - base.FrameWidth * 2;
		for (int i = 0; i < num2; i++)
		{
			e.Graphics.FillRectangle((Brush)new SolidBrush(BarForeColor), new Rectangle(num3, base.FrameWidth, GridWidth, height2));
			num3 += IntervalWidth + GridWidth;
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
		((Control)this).SuspendLayout();
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 12f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Name = "CellBar";
		((Control)this).Size = new Size(320, 39);
		((Control)this).ResumeLayout(false);
	}
}
