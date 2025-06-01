using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using WinTabControl.Win32;

namespace WindControls;

public class WinTabControl : TabControl
{
	private Image TabCloseButtonNormalImage;

	private Image TabCloseButtonDownImage;

	private Image TabAddButtonNormalImage;

	private Image TabAddButtonDownImage;

	private Color onSelectedColor1 = Color.White;

	private Color onSelectedColor2 = Color.Pink;

	private Color offSelectedColor1 = Color.FromArgb(192, 255, 255);

	private Color offSelectedColor2 = Color.FromArgb(200, 66, 204, 255);

	private Color MoveSelectedColor1 = Color.FromArgb(200, 66, 204, 255);

	private Color MoveSelectedColor2 = Color.FromArgb(192, 255, 255);

	private Color BottomLineColor = Color.FromArgb(188, 188, 188);

	private SolidBrush brushFont = new SolidBrush(Color.Black);

	private int mouseOverTabPageId = -1;

	private bool isCloseButtonDown = false;

	private bool isAddButtonDown = false;

	private Color m_TabBackColor = SystemColors.Control;

	private Image m_TabBackImage = null;

	private Rectangle m_TabCloseButtonOffsetRect = new Rectangle(0, 0, 0, 0);

	private Rectangle m_TabIconOffsetRect = new Rectangle(0, 0, 0, 0);

	private Point m_TabTextOffset = new Point(0, 0);

	private Color m_BorderColor = Color.Silver;

	private IContainer components = null;

	[Description("设置选项卡处于选中状态时第一背景色。")]
	[DefaultValue(typeof(Color), "White")]
	[Browsable(true)]
	public Color TabOnColorState
	{
		get
		{
			return onSelectedColor1;
		}
		set
		{
			if (!value.Equals((object?)onSelectedColor1))
			{
				onSelectedColor1 = value;
				((Control)this).Invalidate();
				((Control)this).Update();
			}
		}
	}

	[Description("设置选项卡处于选中状态时第二背景色。")]
	[DefaultValue(typeof(Color), "Pink")]
	[Browsable(true)]
	public Color TabOnColorEnd
	{
		get
		{
			return onSelectedColor2;
		}
		set
		{
			if (!value.Equals((object?)onSelectedColor2))
			{
				onSelectedColor2 = value;
				((Control)this).Invalidate();
				((Control)this).Update();
			}
		}
	}

	[Description("设置选项卡处于非选中状态时第一背景色。")]
	[DefaultValue(typeof(Color), "192, 255, 255")]
	[Browsable(true)]
	public Color TabOffColorState
	{
		get
		{
			return offSelectedColor1;
		}
		set
		{
			if (!value.Equals((object?)offSelectedColor1))
			{
				offSelectedColor1 = value;
				((Control)this).Invalidate();
				((Control)this).Update();
			}
		}
	}

	[Description("设置选项卡处于非选中状态时第二背景色。")]
	[DefaultValue(typeof(Color), "200, 66, 204, 255")]
	[Browsable(true)]
	public Color TabOffColorEnd
	{
		get
		{
			return offSelectedColor2;
		}
		set
		{
			if (!value.Equals((object?)offSelectedColor2))
			{
				offSelectedColor2 = value;
				((Control)this).Invalidate();
				((Control)this).Update();
			}
		}
	}

	[Description("设置鼠标移动到非选中状态选项卡时第一背景色。")]
	[DefaultValue(typeof(Color), "200, 66, 204, 255")]
	[Browsable(true)]
	public Color TabMoveColorState
	{
		get
		{
			return MoveSelectedColor1;
		}
		set
		{
			if (!value.Equals((object?)MoveSelectedColor1))
			{
				MoveSelectedColor1 = value;
				((Control)this).Invalidate();
				((Control)this).Update();
			}
		}
	}

	[Description("设置鼠标移动到非选中状态选项卡时第二背景色。")]
	[DefaultValue(typeof(Color), "192, 255, 255")]
	[Browsable(true)]
	public Color TabMoveColorEnd
	{
		get
		{
			return MoveSelectedColor2;
		}
		set
		{
			if (!value.Equals((object?)MoveSelectedColor2))
			{
				MoveSelectedColor2 = value;
				((Control)this).Invalidate();
				((Control)this).Update();
			}
		}
	}

	[Description("设置选项卡工作区背景色。顶部区域的背景色")]
	[DefaultValue(typeof(Color), "Control")]
	[Browsable(true)]
	public Color TabBackColor
	{
		get
		{
			return m_TabBackColor;
		}
		set
		{
			_ = m_TabBackColor;
			if (true)
			{
				m_TabBackColor = value;
				((Control)this).Invalidate();
			}
		}
	}

	[Description("设置选项卡工作区背景图。")]
	[Browsable(true)]
	public Image TabBackImage
	{
		get
		{
			return m_TabBackImage;
		}
		set
		{
			if (m_TabBackImage != value)
			{
				m_TabBackImage = value;
				((Control)this).Invalidate();
			}
		}
	}

	[Category("自定义")]
	[Description("自定义：选项卡关闭按键坐标偏移")]
	public Rectangle TabCloseButtonOffset
	{
		get
		{
			return m_TabCloseButtonOffsetRect;
		}
		set
		{
			if (m_TabCloseButtonOffsetRect != value)
			{
				m_TabCloseButtonOffsetRect = value;
				((Control)this).Invalidate();
			}
		}
	}

	[Category("自定义")]
	[Description("自定义：选项卡图标坐标偏移")]
	public Rectangle TabIconOffsetRect
	{
		get
		{
			return m_TabIconOffsetRect;
		}
		set
		{
			m_TabIconOffsetRect = value;
			((Control)this).Invalidate();
		}
	}

	[Category("自定义")]
	[Description("自定义：选项卡文字坐标偏移")]
	public Point TabTextOffset
	{
		get
		{
			return m_TabTextOffset;
		}
		set
		{
			if (m_TabTextOffset != value)
			{
				m_TabTextOffset = value;
				((Control)this).Invalidate();
			}
		}
	}

	[Category("自定义")]
	[Description("自定义：边框颜色")]
	public Color BorderColor
	{
		get
		{
			return m_BorderColor;
		}
		set
		{
			if (m_BorderColor != value)
			{
				m_BorderColor = value;
				((Control)this).Invalidate();
			}
		}
	}

	protected override CreateParams CreateParams
	{
		get
		{
			CreateParams createParams = ((TabControl)this).CreateParams;
			createParams.ExStyle |= 0x2000000;
			return createParams;
		}
	}

	public override Rectangle DisplayRectangle
	{
		get
		{
			Rectangle displayRectangle = ((TabControl)this).DisplayRectangle;
			return new Rectangle(displayRectangle.Left - 3, displayRectangle.Top, displayRectangle.Width + 6, displayRectangle.Height + 3);
		}
	}

	public WinTabControl()
	{
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Expected O, but got Unknown
		((Control)this).SetStyle((ControlStyles)142354, true);
		((TabControl)this).SizeMode = (TabSizeMode)2;
	}

	~WinTabControl()
	{
		try
		{
			GC.SuppressFinalize(this);
		}
		finally
		{
			((Component)this).Finalize();
		}
	}

	protected override void OnCreateControl()
	{
		TabAddButtonNormalImage = (Image)(object)FontImages.GetImage("A_fa_plus_circle", TabCloseButtonOffset.Width, Color.Red, Color.Transparent);
		TabAddButtonDownImage = (Image)(object)FontImages.GetImage("A_fa_plus_circle", TabCloseButtonOffset.Width, Color.DarkRed, Color.Transparent);
		TabCloseButtonNormalImage = (Image)(object)FontImages.GetImage("A_fa_window_close", TabCloseButtonOffset.Width, Color.Red, Color.Transparent);
		TabCloseButtonDownImage = (Image)(object)FontImages.GetImage("A_fa_window_close", TabCloseButtonOffset.Width, Color.DarkRed, Color.Transparent);
	}

	protected virtual void DrawControlAppearance(Graphics g)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Expected O, but got Unknown
		Rectangle rectangle = new Rectangle(0, 0, ((Control)this).ClientRectangle.Width, ((Control)this).ClientRectangle.Height - ((Control)this).DisplayRectangle.Height);
		g.FillRectangle((Brush)new SolidBrush(TabBackColor), rectangle);
		if (TabBackImage != null)
		{
			g.DrawImage(TabBackImage, rectangle);
		}
		Pen val = new Pen(BorderColor);
		try
		{
			val.DashStyle = (DashStyle)0;
			Rectangle rectangle2 = new Rectangle(0, rectangle.Height, ((Control)this).ClientRectangle.Width - 1, ((Control)this).ClientRectangle.Height - rectangle.Height - 1);
			g.DrawRectangle(val, rectangle2);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void DrawTabBoder(Graphics g, int tabId)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Expected O, but got Unknown
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Expected O, but got Unknown
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Expected O, but got Unknown
		Rectangle tabRect = ((TabControl)this).GetTabRect(tabId);
		GraphicsPath val = new GraphicsPath();
		try
		{
			val.AddBezier(new Point(tabRect.X, tabRect.Bottom + 2), new Point(tabRect.X + 3, tabRect.Bottom - 2), new Point(tabRect.X + 3, tabRect.Bottom - 2), new Point(tabRect.X, tabRect.Bottom + 2));
			val.AddBezier(new Point(tabRect.Left + 15 - 4, tabRect.Y + 4), new Point(tabRect.Left + 15 - 3, tabRect.Y + 2), new Point(tabRect.Left + 15 - 3, tabRect.Y + 2), new Point(tabRect.Left + 15, tabRect.Y));
			val.AddBezier(new Point(tabRect.Right - 15, tabRect.Y), new Point(tabRect.Right - 15 + 3, tabRect.Y + 2), new Point(tabRect.Right - 15 + 3, tabRect.Y + 2), new Point(tabRect.Right - 15 + 4, tabRect.Y + 4));
			val.AddBezier(new Point(tabRect.Right, tabRect.Bottom), new Point(tabRect.Right - 3, tabRect.Bottom - 3), new Point(tabRect.Right - 3, tabRect.Bottom - 3), new Point(tabRect.Right + 1, tabRect.Bottom + 1));
			g.DrawPath(new Pen(Color.Black), val);
			LinearGradientBrush val2 = ((((TabControl)this).SelectedIndex == tabId) ? new LinearGradientBrush(tabRect, onSelectedColor1, onSelectedColor2, (LinearGradientMode)1) : new LinearGradientBrush(tabRect, offSelectedColor1, offSelectedColor2, (LinearGradientMode)1));
			try
			{
				g.FillPath((Brush)(object)val2, val);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void DrawText(Graphics g, int tabId)
	{
		Rectangle tabRect = ((TabControl)this).GetTabRect(tabId);
		SizeF sizeF = g.MeasureString(((Control)((TabControl)this).TabPages[tabId]).Text, ((Control)this).Font);
		Rectangle rectangle = tabRect;
		rectangle.X += (tabRect.Width - (int)sizeF.Width) / 2 + TabTextOffset.X;
		rectangle.Y += (tabRect.Height - (int)sizeF.Height) / 2 + TabTextOffset.Y;
		g.DrawString(((Control)((TabControl)this).TabPages[tabId]).Text, ((Control)this).Font, (Brush)(object)brushFont, (RectangleF)rectangle);
	}

	private void DrawCloseButton(Graphics g, int tabId)
	{
		Rectangle tabRect = ((TabControl)this).GetTabRect(tabId);
		if (TabCloseButtonDownImage != null && TabCloseButtonNormalImage != null)
		{
			Rectangle tabCloseButtonRect = GetTabCloseButtonRect(tabRect);
			g.DrawImage(tabCloseButtonRect.Contains(((Control)this).PointToClient(Cursor.Position)) ? TabCloseButtonDownImage : TabCloseButtonNormalImage, tabCloseButtonRect);
		}
	}

	private void DrawIcon(Graphics g, int tabId)
	{
		Rectangle tabRect = ((TabControl)this).GetTabRect(tabId);
		if (((TabControl)this).ImageList != null && !((TabControl)this).TabPages[tabId].ImageIndex.Equals(-1) && ((TabControl)this).TabPages[tabId].ImageIndex <= ((TabControl)this).ImageList.Images.Count - 1)
		{
			Image val = ((TabControl)this).ImageList.Images[((TabControl)this).TabPages[tabId].ImageIndex];
			Rectangle tabIconOffsetRect = TabIconOffsetRect;
			tabIconOffsetRect.X += tabRect.X;
			g.DrawImage(val, tabIconOffsetRect);
			val.Dispose();
		}
	}

	protected virtual void drawTabAddButtonIcon(Graphics g)
	{
		if (TabAddButtonDownImage != null && TabAddButtonNormalImage != null)
		{
			Rectangle tabAddButtonRect = GetTabAddButtonRect();
			g.DrawImage(tabAddButtonRect.Contains(((Control)this).PointToClient(Cursor.Position)) ? TabAddButtonDownImage : TabAddButtonNormalImage, tabAddButtonRect);
		}
	}

	protected virtual void DrawOneTab(Graphics g, int tabId, bool mouseOver)
	{
		if (tabId >= 0 && tabId < ((TabControl)this).TabPages.Count)
		{
			DrawTabBoder(g, tabId);
			DrawText(g, tabId);
			DrawCloseButton(g, tabId);
			DrawIcon(g, tabId);
		}
	}

	private Rectangle GetTabAddButtonRect()
	{
		return new Rectangle(((TabControl)this).ItemSize.Width * ((TabControl)this).TabCount, (((TabControl)this).ItemSize.Height - TabCloseButtonOffset.Height) / 2, TabCloseButtonOffset.Width, TabCloseButtonOffset.Height);
	}

	private Rectangle GetTabCloseButtonRect(Rectangle rect)
	{
		return new Rectangle(rect.X + rect.Width - TabCloseButtonOffset.X - TabCloseButtonOffset.Width, rect.Y + (rect.Height - TabCloseButtonOffset.Height) / 2 + TabCloseButtonOffset.Y, TabCloseButtonOffset.Width, TabCloseButtonOffset.Height);
	}

	protected virtual void drawTabs(Graphics g)
	{
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Expected O, but got Unknown
		for (int i = 0; i < ((TabControl)this).TabCount; i++)
		{
			DrawOneTab(g, i, mouseOver: false);
			if (((TabControl)this).SelectedIndex == i)
			{
				Rectangle rectangle = new Rectangle(0, ((Control)this).ClientRectangle.Height - ((Control)this).DisplayRectangle.Height, ((Control)this).ClientRectangle.Width - 1, ((Control)this).DisplayRectangle.Height - 1);
				g.FillRectangle((Brush)new SolidBrush(((Control)((TabControl)this).TabPages[((TabControl)this).SelectedIndex]).BackColor), rectangle);
			}
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		((Control)this).OnPaint(e);
		e.Graphics.SetGDIHigh();
		DrawControlAppearance(e.Graphics);
		drawTabs(e.Graphics);
		drawTabAddButtonIcon(e.Graphics);
	}

	protected override void WndProc(ref Message m)
	{
		((TabControl)this).WndProc(ref m);
		if (((Message)(ref m)).Msg == 132 && ((Message)(ref m)).Result.ToInt32() == -1)
		{
			((Message)(ref m)).Result = (IntPtr)1;
		}
	}

	private int GetTabPageIdFromPoint(Point point)
	{
		for (int i = 0; i <= ((TabControl)this).TabPages.Count - 1; i++)
		{
			if (((TabControl)this).GetTabRect(i).Contains(point.X, point.Y))
			{
				return i;
			}
		}
		return -1;
	}

	private int GetCloseButtonIdFromPoint(Point cursorPoint)
	{
		int tabPageIdFromPoint = GetTabPageIdFromPoint(cursorPoint);
		if (tabPageIdFromPoint >= 0 && GetTabCloseButtonRect(((TabControl)this).GetTabRect(tabPageIdFromPoint)).Contains(cursorPoint))
		{
			return tabPageIdFromPoint;
		}
		return -1;
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		((Control)this).OnMouseLeave(e);
		Graphics g = ((Control)this).CreateGraphics();
		DrawOneTab(g, mouseOverTabPageId, mouseOver: false);
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		((Control)this).OnMouseMove(e);
		int tabPageIdFromPoint = GetTabPageIdFromPoint(e.Location);
		if (mouseOverTabPageId != tabPageIdFromPoint)
		{
			Graphics g = ((Control)this).CreateGraphics();
			DrawOneTab(g, tabPageIdFromPoint, mouseOver: true);
			DrawOneTab(g, mouseOverTabPageId, mouseOver: false);
			mouseOverTabPageId = tabPageIdFromPoint;
			return;
		}
		bool flag = false;
		if (mouseOverTabPageId >= 0)
		{
			flag = GetTabCloseButtonRect(((TabControl)this).GetTabRect(mouseOverTabPageId)).Contains(e.Location);
			if (isCloseButtonDown != flag)
			{
				DrawCloseButton(((Control)this).CreateGraphics(), mouseOverTabPageId);
				isCloseButtonDown = flag;
			}
		}
		flag = GetTabAddButtonRect().Contains(e.Location);
		if (isAddButtonDown != flag)
		{
			drawTabAddButtonIcon(((Control)this).CreateGraphics());
			isAddButtonDown = flag;
		}
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
		((Control)this).OnMouseUp(e);
		if ((int)e.Button == 1048576)
		{
			if (GetTabAddButtonRect().Contains(e.Location))
			{
				AddTabPage("tabPage" + (((TabControl)this).TabCount + 1));
				return;
			}
			int tabPageIdFromPoint = GetTabPageIdFromPoint(e.Location);
			if (tabPageIdFromPoint >= 0)
			{
				if (GetTabCloseButtonRect(((TabControl)this).GetTabRect(tabPageIdFromPoint)).Contains(e.Location))
				{
					((TabControl)this).TabPages.RemoveAt(tabPageIdFromPoint);
					return;
				}
				((TabControl)this).SelectedIndex = tabPageIdFromPoint;
			}
		}
		((Control)this).Invalidate();
	}

	protected override void OnResize(EventArgs e)
	{
		((TabControl)this).OnResize(e);
	}

	public void AddTabPage(string tabName)
	{
		((TabControl)this).TabPages.Add(tabName);
		((TabControl)this).SelectTab(((TabControl)this).TabPages.Count - 1);
		((ScrollableControl)((TabControl)this).SelectedTab).AutoScroll = true;
	}

	private int GetMouseOverTabId()
	{
		Point point = ((Control)this).PointToClient(Cursor.Position);
		User32.TCHITTESTINFO lParam = new User32.TCHITTESTINFO(point, User32.TabControlHitTest.TCHT_ONITEM);
		return User32.SendMessage(((Control)this).Handle, 4877, IntPtr.Zero, ref lParam);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((TabControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		components = new Container();
	}
}
