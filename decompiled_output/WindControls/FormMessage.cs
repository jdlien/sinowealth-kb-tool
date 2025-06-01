using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace WindControls;

public class FormMessage : MsgBaseForm
{
	private IContainer components = null;

	private FormMessageItem MsgItem;

	public FormMessage(string title, string text, int width, int height, int buttonHeight)
	{
		InitializeComponent();
		SetTitle(title);
		ResizeWindow(width, height, buttonHeight);
		ResizeMsgTextBox(text);
	}

	public FormMessage()
		: this("", "", 400, 300, 60)
	{
	}

	private void ResizeMsgTextBox(string text)
	{
		Rectangle clientRect = GetClientRect();
		((Control)MsgItem.TextBox_Msg).Text = text;
		Graphics val = ((Control)MsgItem.TextBox_Msg).CreateGraphics();
		SizeF sizeF = val.MeasureString("雷", ((Control)MsgItem.TextBox_Msg).Font);
		((Control)MsgItem.TextBox_Msg).Width = clientRect.Width;
		int num = ((TextBoxBase)MsgItem.TextBox_Msg).GetLineFromCharIndex(((TextBoxBase)MsgItem.TextBox_Msg).TextLength) + 1;
		int num2 = num * (int)sizeF.Height;
		int x = clientRect.X;
		if (num == 1)
		{
			sizeF = val.MeasureString(text, ((Control)MsgItem.TextBox_Msg).Font);
			((Control)MsgItem.TextBox_Msg).Width = (int)sizeF.Width;
			x = (clientRect.Width - ((Control)MsgItem.TextBox_Msg).Width) / 2;
		}
		if (num2 > clientRect.Height)
		{
			((Control)MsgItem.TextBox_Msg).Height = clientRect.Height;
			MsgItem.TextBox_Msg.ScrollBars = (ScrollBars)2;
		}
		else
		{
			((Control)MsgItem.TextBox_Msg).Height = num2;
			MsgItem.TextBox_Msg.ScrollBars = (ScrollBars)0;
		}
		int y = clientRect.Y + (clientRect.Height - ((Control)MsgItem.TextBox_Msg).Height) / 2;
		((Control)MsgItem.TextBox_Msg).Location = new Point(0, 0);
		((Control)MsgItem).Location = new Point(x, y);
		((Control)MsgItem).Width = ((Control)MsgItem.TextBox_Msg).Width;
		((Control)MsgItem).Height = ((Control)MsgItem.TextBox_Msg).Height;
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
		MsgItem = new FormMessageItem();
		((Control)this).SuspendLayout();
		((Control)MsgItem).Location = new Point(12, 86);
		((Control)MsgItem).Name = "MsgItem";
		((Control)MsgItem).Size = new Size(764, 347);
		((Control)MsgItem).TabIndex = 8;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 12f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(841, 616);
		((Control)this).Controls.Add((Control)(object)MsgItem);
		((Control)this).Name = "FormMessage";
		((Control)this).Text = "FormMessage";
		((Control)this).Controls.SetChildIndex((Control)(object)MsgItem, 0);
		((Control)this).ResumeLayout(false);
	}
}
