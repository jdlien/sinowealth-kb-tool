using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace WindControls;

public class SkinForm : Form
{
	private FormMover formMover = new FormMover();

	public Form ControlsForm;

	private Point locationOffset = default(Point);

	private bool movable = true;

	private ResizeControl resizeControl = new ResizeControl();

	public int ReduceX = 12;

	public int ReduceY = 12;

	private IContainer components = null;

	protected override CreateParams CreateParams
	{
		get
		{
			CreateParams createParams = ((Form)this).CreateParams;
			createParams.ExStyle |= 0x80000;
			return createParams;
		}
	}

	public SkinForm(bool _movable)
	{
		InitializeComponent();
		SetStyles();
		movable = _movable;
	}

	public void AddControl(Control control)
	{
		if (movable)
		{
			formMover.AddControl(control);
		}
	}

	public void InitSkin(Form form, Image backImage, Rectangle formRect, int radius, int reduceX, int reduceY)
	{
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Expected O, but got Unknown
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Expected O, but got Unknown
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Expected O, but got Unknown
		ReduceX = reduceX;
		ReduceY = reduceY;
		ControlsForm = form;
		if (movable)
		{
			formMover.AddForm(form);
		}
		((Form)this).Activated += SkinForm_Activated;
		ControlsForm.Owner = (Form)(object)this;
		((Control)this).Text = ((Control)ControlsForm).Text;
		((Control)this).Font = ((Control)ControlsForm).Font;
		((Form)this).ShowInTaskbar = true;
		ControlsForm.ShowInTaskbar = false;
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Control)this).BackgroundImage = backImage;
		((Control)this).BackgroundImageLayout = (ImageLayout)3;
		((Form)this).Icon = ControlsForm.Icon;
		((Form)this).ShowIcon = ControlsForm.ShowIcon;
		((Form)this).Size = backImage.Size;
		((Form)this).StartPosition = (FormStartPosition)0;
		locationOffset = formRect.Location;
		((Control)ControlsForm).Width = formRect.Width;
		((Control)ControlsForm).Height = formRect.Height;
		formRect.X = reduceX;
		formRect.Y = reduceY;
		if (formRect.X * 2 + formRect.Width > ((Control)ControlsForm).BackgroundImage.Width)
		{
			formRect.Width = ((Control)ControlsForm).BackgroundImage.Width - formRect.X * 2;
		}
		if (formRect.Y * 2 + formRect.Height > ((Control)ControlsForm).BackgroundImage.Height)
		{
			formRect.Height = ((Control)ControlsForm).BackgroundImage.Height - formRect.Y * 2;
		}
		((Control)ControlsForm).BackgroundImage = (Image)(object)ImageHelper.ImageTailor(new Bitmap(((Control)ControlsForm).BackgroundImage), formRect);
		((Control)ControlsForm).Region = GraphicsHelper.GetWindowRegion(formRect.Width, formRect.Height, radius, Color.Transparent, GraphicsHelper.RoundStyle.All);
		ControlsForm.FormClosing += new FormClosingEventHandler(ControlsForm_FormClosing);
		((Control)ControlsForm).LocationChanged += ControlsForm_LocationChanged;
		ControlsForm.Load += ControlsForm_Load;
		if (reduceX <= 0 && reduceY <= 0)
		{
			return;
		}
		ControlCollection controls = ((Control)ControlsForm).Controls;
		foreach (Control item in (ArrangedElementCollection)controls)
		{
			Control val = item;
			val.Location = new Point(val.Location.X - formRect.X, val.Location.Y - formRect.Y);
		}
	}

	private void ControlsForm_Load(object sender, EventArgs e)
	{
		ControlsForm_LocationChanged(null, null);
		((Control)this).Show();
	}

	public void InitSkin(Form form, int reduce, int radius)
	{
		if (((Control)form).BackgroundImage != null)
		{
			resizeControl.Resize(form);
			InitSkin(formRect: new Rectangle(reduce, reduce, ((Control)form).BackgroundImage.Width - reduce * 2, ((Control)form).BackgroundImage.Height - reduce * 2), form: form, backImage: ((Control)form).BackgroundImage, radius: radius, reduceX: reduce, reduceY: reduce);
		}
	}

	public void InitSkin(Form form, int reduceX, int reduceY, int radius)
	{
		if (((Control)form).BackgroundImage != null)
		{
			resizeControl.Resize(form);
			InitSkin(formRect: new Rectangle(reduceX, reduceY, ((Control)form).BackgroundImage.Width - reduceX * 2, ((Control)form).BackgroundImage.Height - reduceY * 2), form: form, backImage: ((Control)form).BackgroundImage, radius: radius, reduceX: reduceX, reduceY: reduceY);
		}
	}

	private void SkinForm_Activated(object sender, EventArgs e)
	{
		ControlsForm.Activate();
	}

	private void ControlsForm_LocationChanged(object sender, EventArgs e)
	{
		((Form)this).Location = new Point(ControlsForm.Location.X - locationOffset.X, ControlsForm.Location.Y - locationOffset.Y);
	}

	private void ControlsForm_FormClosing(object sender, FormClosingEventArgs e)
	{
		ControlsForm.Owner = null;
		((Form)this).Close();
	}

	public void AllHide()
	{
		((Control)this).Hide();
		((Control)ControlsForm).Hide();
		ControlsForm_LocationChanged(null, null);
	}

	public void AllShow()
	{
		((Control)this).Show();
		((Control)ControlsForm).Show();
		ControlsForm_LocationChanged(null, null);
		ControlsForm.Activate();
	}

	public void AllShow(Form parent)
	{
		if (parent != null)
		{
			if (((Form)this).Owner != parent)
			{
				((Form)this).Owner = parent;
			}
			int x = parent.Location.X + (((Control)parent).Width - ((Control)ControlsForm).Width) / 2;
			int y = parent.Location.Y + (((Control)parent).Height - ((Control)ControlsForm).Height) / 2;
			ControlsForm.Location = new Point(x, y);
		}
		else
		{
			((Form)this).StartPosition = (FormStartPosition)1;
		}
		((Control)this).Show();
		((Control)ControlsForm).Show();
		ControlsForm.Activate();
	}

	private void SetStyles()
	{
		((Control)this).SetStyle((ControlStyles)204818, true);
		((Control)this).UpdateStyles();
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)0;
	}

	public void SetBits()
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		if (((Control)this).BackgroundImage == null)
		{
			return;
		}
		Bitmap val = new Bitmap(((Control)this).BackgroundImage, ((Control)this).Width, ((Control)this).Height);
		if (!Image.IsCanonicalPixelFormat(((Image)val).PixelFormat) || !Image.IsAlphaPixelFormat(((Image)val).PixelFormat))
		{
			throw new ApplicationException("图片必须是32位带Alhpa通道的图片。");
		}
		IntPtr hObj = IntPtr.Zero;
		IntPtr dC = Win32.GetDC(IntPtr.Zero);
		IntPtr intPtr = IntPtr.Zero;
		IntPtr intPtr2 = Win32.CreateCompatibleDC(dC);
		try
		{
			Win32.Point pptDst = new Win32.Point(((Control)this).Left, ((Control)this).Top);
			Win32.Size psize = new Win32.Size(((Control)this).Width, ((Control)this).Height);
			Win32.BLENDFUNCTION pblend = default(Win32.BLENDFUNCTION);
			Win32.Point pptSrc = new Win32.Point(0, 0);
			intPtr = val.GetHbitmap(Color.FromArgb(0));
			hObj = Win32.SelectObject(intPtr2, intPtr);
			pblend.BlendOp = 0;
			pblend.SourceConstantAlpha = byte.MaxValue;
			pblend.AlphaFormat = 1;
			pblend.BlendFlags = 0;
			Win32.UpdateLayeredWindow(((Control)this).Handle, dC, ref pptDst, ref psize, intPtr2, ref pptSrc, 0, ref pblend, 2);
		}
		finally
		{
			if (intPtr != IntPtr.Zero)
			{
				Win32.SelectObject(intPtr2, hObj);
				Win32.DeleteObject(intPtr);
			}
			Win32.ReleaseDC(IntPtr.Zero, dC);
			Win32.DeleteDC(intPtr2);
		}
	}

	protected override void OnBackgroundImageChanged(EventArgs e)
	{
		((Form)this).OnBackgroundImageChanged(e);
		SetBits();
	}

	protected override void OnResize(EventArgs e)
	{
		((Form)this).OnResize(e);
		SetBits();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((Form)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		((Control)this).SuspendLayout();
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 12f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackgroundImageLayout = (ImageLayout)3;
		((Form)this).ClientSize = new Size(259, 271);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Control)this).Name = "SkinForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "SkinForm";
		((Control)this).ResumeLayout(false);
	}
}
