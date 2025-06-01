using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindControls;

public class WindToolStripRenderer : ToolStripRenderer
{
	protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
	{
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Expected O, but got Unknown
		ToolStrip toolStrip = e.ToolStrip;
		Graphics graphics = e.Graphics;
		Rectangle affectedBounds = e.AffectedBounds;
		if (toolStrip is ToolStripDropDown)
		{
			((Control)toolStrip).Region = GraphicsHelper.GetWindowRegion(affectedBounds.Width, affectedBounds.Height, 8, Color.Brown, GraphicsHelper.RoundStyle.All);
			GraphicsHelper.FillRect(graphics, affectedBounds, 8, Color.Brown, 0, Color.Transparent, GraphicsHelper.RoundStyle.All);
			return;
		}
		if (toolStrip is MenuStrip)
		{
			SolidBrush val = new SolidBrush(Color.Brown);
			try
			{
				graphics.FillRectangle((Brush)(object)val, affectedBounds);
				return;
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		((ToolStripRenderer)this).OnRenderToolStripBackground(e);
	}

	protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
	{
		ToolStrip toolStrip = e.ToolStrip;
		ToolStripItem item = e.Item;
		Graphics graphics = e.Graphics;
		Rectangle frameRect = new Rectangle(Point.Empty, e.Item.Size);
		if (toolStrip is ToolStripDropDown)
		{
			frameRect.Inflate(-4, -2);
			if (item.Selected)
			{
				GraphicsHelper.FillRect(graphics, frameRect, 2, Color.Red, 0, Color.Transparent, GraphicsHelper.RoundStyle.All);
			}
		}
		else if (toolStrip is MenuStrip)
		{
			if (item.Selected)
			{
				GraphicsHelper.FillRect(graphics, frameRect, 2, Color.Red, 0, Color.Transparent, GraphicsHelper.RoundStyle.All);
			}
		}
		else
		{
			((ToolStripRenderer)this).OnRenderMenuItemBackground(e);
		}
	}

	protected override void OnRenderItemImage(ToolStripItemImageRenderEventArgs e)
	{
		ToolStrip toolStrip = ((ToolStripItemRenderEventArgs)e).ToolStrip;
		Graphics graphics = ((ToolStripItemRenderEventArgs)e).Graphics;
		((ToolStripRenderer)this).OnRenderItemImage(e);
	}

	protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
	{
		ToolStrip toolStrip = ((ToolStripItemRenderEventArgs)e).ToolStrip;
		if (!(toolStrip is ToolStripDropDown) || ((ToolStripItemRenderEventArgs)e).Item is ToolStripMenuItem)
		{
		}
		((ToolStripRenderer)this).OnRenderItemText(e);
	}

	protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
	{
		ToolStrip toolStrip = ((ToolStripItemRenderEventArgs)e).ToolStrip;
		Graphics graphics = ((ToolStripItemRenderEventArgs)e).Graphics;
		((ToolStripRenderer)this).OnRenderItemCheck(e);
	}

	protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
	{
		ToolStrip owner = e.Item.Owner;
		if (!(owner is ToolStripDropDown) || e.Item is ToolStripMenuItem)
		{
		}
		((ToolStripRenderer)this).OnRenderArrow(e);
	}

	protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
	{
		ToolStrip toolStrip = ((ToolStripItemRenderEventArgs)e).ToolStrip;
		Rectangle contentRectangle = ((ToolStripItemRenderEventArgs)e).Item.ContentRectangle;
		Graphics graphics = ((ToolStripItemRenderEventArgs)e).Graphics;
		((ToolStripRenderer)this).OnRenderSeparator(e);
	}
}
