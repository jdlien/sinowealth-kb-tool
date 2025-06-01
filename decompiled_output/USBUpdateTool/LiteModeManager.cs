using System.Drawing;
using System.Windows.Forms;

namespace USBUpdateTool;

public class LiteModeManager
{
	public static Color GetColor(int color)
	{
		return Color.FromArgb(color | -16777216);
	}

	private void SetControl(Control control, ControlConfig controlConfig)
	{
		control.Location = new Point(controlConfig.Rect.X, controlConfig.Rect.Y);
		control.Width = controlConfig.Rect.Width;
		control.Height = controlConfig.Rect.Height;
		control.Text = controlConfig.Text;
		control.Font = controlConfig.font;
		control.ForeColor = controlConfig.foreColor;
		control.Visible = controlConfig.visable;
	}

	public void LoadLiteInformation(FormMain formMain)
	{
		LiteResources.LoadResourceFile();
		SetControl((Control)(object)formMain.label_tool_caption, LiteResources.appConfig.CaptionTextBox);
		SetControl((Control)(object)formMain.label_curFwVersion, LiteResources.appConfig.CurFwVersionTextBox);
		SetControl((Control)(object)formMain.label_newFwVersion, LiteResources.appConfig.NewFwVersionTextBox);
		SetControl((Control)(object)formMain.label_ToolVersion, LiteResources.appConfig.ToolVersionTextBox);
		SetControl((Control)(object)formMain.wIButton_Lite_Upgrade, LiteResources.appConfig.SuccessUpgradeButton);
		formMain.wIButton_Lite_Upgrade.SetMouseForeColor(LiteResources.appConfig.SuccessUpgradeButton.foreColor);
		formMain.wIButton_Lite_Upgrade.SetMouseBackColor(LiteResources.appConfig.SuccessUpgradeButton.backColor);
		((Control)formMain.wIButton_Lite_Upgrade).Enabled = false;
		((Control)formMain.wIButton_Lite_Upgrade).Visible = true;
		Point location = new Point
		{
			X = LiteResources.appConfig.CloseButton.Rect.Location.X,
			Y = LiteResources.appConfig.CloseButton.Rect.Location.Y
		};
		((Control)formMain.wiButton_Close_lite).Location = location;
		formMain.wiButton_Close_lite.SetMouseForeColor(LiteResources.appConfig.CloseButton.foreColor);
		((Control)formMain.wiButton_Close_lite).Visible = true;
		location.X = LiteResources.appConfig.MiniButton.Rect.Location.X;
		location.Y = LiteResources.appConfig.MiniButton.Rect.Location.Y;
		((Control)formMain.wiButton_mini_lite).Location = location;
		formMain.wiButton_mini_lite.SetMouseForeColor(LiteResources.appConfig.MiniButton.foreColor);
		((Control)formMain.wiButton_mini_lite).Visible = true;
		((Control)formMain.wImageButton_More).Visible = false;
		((Control)formMain).BackgroundImage = LiteResources.appConfig.imageConfig.BackgroundImage;
		((Control)formMain).Text = ((Control)formMain.label_tool_caption).Text;
		((Control)formMain.wcobBox_IC_type).Visible = false;
		((Control)formMain.label_caption).Visible = false;
		((Control)formMain.wTextBox_fileName).Visible = false;
		((Control)formMain.imageBar1).Visible = false;
		((Control)formMain.editNormalVidPid).Visible = false;
		((Control)formMain.editBootVidPid).Visible = false;
		((Control)formMain.wibutton_StartUpgrade).Visible = false;
		((Control)formMain.wibutton_LoadFile).Visible = false;
		((Control)formMain.wiButton_about).Visible = false;
		((Control)formMain.wiButton_mini).Visible = false;
		((Control)formMain.wiButton_Close).Visible = false;
		formMain.skinForm.AddControl((Control)(object)formMain.label_tool_caption);
		formMain.skinForm.AddControl((Control)(object)formMain.label_curFwVersion);
		formMain.skinForm.AddControl((Control)(object)formMain.label_newFwVersion);
		formMain.skinForm.AddControl((Control)(object)formMain.label_ToolVersion);
		((Control)formMain.wCheckBox_IC_Enable).Visible = false;
		((Control)formMain.label3).Visible = false;
		((Control)formMain).Text = LiteResources.appConfig.fileConfig.exeCaption;
		if (LiteResources.appConfig.fileConfig.exeType == EXE_TYPE.PAIR)
		{
			if (LiteResources.appConfig.PairVidOnlyDevices.Count > 0 || LiteResources.appConfig.PairVidPidDevices.Count > 0)
			{
				((Control)formMain.wIButton_Lite_Upgrade).Text = "Pair";
				SetControl((Control)(object)formMain.pictureBox_Logo, LiteResources.appConfig.LogoPicture);
				((Control)formMain.pictureBox_Logo).BackgroundImage = LiteResources.appConfig.imageConfig.LogoImage;
				formMain.skinForm.AddControl((Control)(object)formMain.pictureBox_Logo);
				((Control)formMain).BackgroundImage = LiteResources.appConfig.imageConfig.PairBackImage;
				((Control)formMain.wIButton_4KPair).Visible = LiteResources.appConfig.Pair4KButton.visable;
				location.X = LiteResources.appConfig.Pair4KButton.Rect.Location.X;
				location.Y = LiteResources.appConfig.Pair4KButton.Rect.Location.Y;
				((Control)formMain.wIButton_4KPair).Location = location;
				SetControl((Control)(object)formMain.wIButton_default, LiteResources.appConfig.DefaultButton);
				SetControl((Control)(object)formMain.wIButton_Setting, LiteResources.appConfig.PairSettingButton);
			}
			else
			{
				((Control)formMain.label_curFwVersion).Visible = false;
				((Control)formMain.label_newFwVersion).Visible = false;
				((Control)formMain.wIButton_Lite_Upgrade).Text = "Pair";
			}
		}
	}

	private void AdjustControlLocation(Control control, Form form)
	{
		int num = control.Location.X + control.Width;
		int num2 = control.Location.Y + control.Height;
		bool flag = false;
		if (num > ((Control)form).Width)
		{
			flag = true;
			num = ((Control)form).Width - control.Width - 8;
			if (num < 0)
			{
				num = 8;
				control.Width = ((Control)form).Width - num;
			}
		}
		if (num2 > ((Control)form).Height)
		{
			flag = true;
			num2 = ((Control)form).Height - control.Height - 8;
			if (num2 < 0)
			{
				num2 = 8;
				control.Height = ((Control)form).Height - num2;
			}
		}
		if (flag)
		{
			control.Location = new Point(num, num2);
		}
	}

	public void CheckUILocation(FormMain formMain)
	{
		AdjustControlLocation((Control)(object)formMain.wIButton_Lite_Upgrade, (Form)(object)formMain);
	}
}
