using System;
using System.Windows.Forms;
using WindControls;

namespace USBUpdateTool;

public class ProgressManager
{
	private FormMain formMain;

	private WindProgressBar progressBar;

	public ProgressManager(FormMain _formMain)
	{
		formMain = _formMain;
		if (LiteResources.appConfig.fileConfig.exeType == EXE_TYPE.UPGRADE)
		{
			CreateLiteModeProgress();
		}
	}

	private void CreateLiteModeProgress()
	{
		switch ((BAR_STYLE)LiteResources.appConfig.ProgressControl.style)
		{
		case BAR_STYLE.HaloBar:
			progressBar = new HaloBar();
			break;
		case BAR_STYLE.RoundBar:
			progressBar = new RoundBar();
			break;
		case BAR_STYLE.SliderBar:
			progressBar = new SliderBar();
			break;
		case BAR_STYLE.CircularBar:
			progressBar = new CircularBar();
			break;
		case BAR_STYLE.CellBar:
			progressBar = new CellBar();
			break;
		case BAR_STYLE.ImageBar:
			progressBar = new ImageBar();
			progressBar.BarSliderImage = LiteResources.appConfig.imageConfig.ProgressSliderImage;
			progressBar.BarForeImage = LiteResources.appConfig.imageConfig.ProgressForeImage;
			progressBar.BarBackImage = LiteResources.appConfig.imageConfig.ProgressBackImage;
			progressBar.BarSliderImageRect = LiteResources.appConfig.imageConfig.PorgressSliderImageRect;
			progressBar.BarForeImageRect = LiteResources.appConfig.imageConfig.PorgressForeImageRect;
			progressBar.BarBackImageRect = LiteResources.appConfig.imageConfig.PorgressBackImageRect;
			progressBar.TextLocation = LiteResources.appConfig.imageConfig.PorgressTextLocation;
			progressBar.BarSliderMovable = LiteResources.appConfig.imageConfig.PorgressSliderMovable;
			progressBar.ShowPercent = LiteResources.appConfig.imageConfig.PorgressShowPercent;
			break;
		}
		if (progressBar != null)
		{
			((Control)formMain).Controls.Add((Control)(object)progressBar);
			formMain.skinForm.AddControl((Control)(object)progressBar);
			((Control)progressBar).Location = LiteResources.appConfig.ProgressControl.Rect.Location;
			((Control)progressBar).Width = LiteResources.appConfig.ProgressControl.Rect.Width;
			((Control)progressBar).Height = LiteResources.appConfig.ProgressControl.Rect.Height;
			progressBar.TextColor = LiteResources.appConfig.ProgressControl.textColor;
			progressBar.BarForeColor = LiteResources.appConfig.ProgressControl.foreColor;
			progressBar.BarBackColor = LiteResources.appConfig.ProgressControl.backColor;
			((Control)progressBar).Visible = true;
			progressBar.Value = 0;
		}
	}

	private void SetLiteModeProgressValue(int value)
	{
		if (progressBar != null)
		{
			progressBar.Value = value;
		}
	}

	public void SetValue(int value)
	{
		((Control)formMain).Invoke((Delegate)(Action)delegate
		{
			formMain.imageBar1.Value = value;
			if (LiteResources.appConfig.fileConfig.exeType == EXE_TYPE.UPGRADE)
			{
				SetLiteModeProgressValue(value);
			}
		});
	}
}
