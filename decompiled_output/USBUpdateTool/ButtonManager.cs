using System.Drawing;
using System.Windows.Forms;
using USBUpdateTool.Properties;
using WindControls;

namespace USBUpdateTool;

public class ButtonManager
{
	public Image NormalImage = (Image)(object)Resources.UpgradeButtonNormalImage;

	public Image NormalEnterImage = (Image)(object)Resources.UpgradeButtonMouseEnterImage;

	public Image NormalDownImage = (Image)(object)Resources.UpgradeButtonMouseDownImage;

	public Image SuccessImage = (Image)(object)Resources.UpgradeButtonSuccessImage;

	public Image SuccessEnterImage = (Image)(object)Resources.UpgradeButtonSuccessEnterImage;

	public Image SuccessDownImage = (Image)(object)Resources.UpgradeButtonSuccessDownImage;

	public Image FailImage = (Image)(object)Resources.UpgradeButtonFailImage;

	public Image FailEnterImage = (Image)(object)Resources.UpgradeButtonFailEnterImage;

	public Image FailDownImage = (Image)(object)Resources.UpgradeButtonFailDownImage;

	public ButtonState currentButtonState = ButtonState.Normal;

	private WindImageButton upgradeButton;

	private WindImageButton liteUpgradeButton;

	public ButtonManager(FormMain _formMain)
	{
		upgradeButton = _formMain.wibutton_StartUpgrade;
		liteUpgradeButton = _formMain.wIButton_Lite_Upgrade;
		SetState(ButtonState.Normal, enable: false);
		if (LiteResources.appConfig.fileConfig.exeType == EXE_TYPE.PAIR)
		{
			((Control)liteUpgradeButton).Text = "Pair";
			SetPairState(ButtonState.Success, enable: false);
		}
	}

	private void SetButtonImage(bool isSuccess)
	{
		if (LiteResources.appConfig.SuccessUpgradeButton.style == 1)
		{
			if (isSuccess)
			{
				liteUpgradeButton.MouseUpImage = LiteResources.appConfig.imageConfig.UpgradeMouseUpImage;
				liteUpgradeButton.MouseEnterImage = LiteResources.appConfig.imageConfig.UpgradeMouseEnterImage;
				liteUpgradeButton.MouseDownImage = LiteResources.appConfig.imageConfig.UpgradeMouseDownImage;
				liteUpgradeButton.DisableImage = LiteResources.appConfig.imageConfig.UpgradeMouseUpImage;
			}
			else
			{
				liteUpgradeButton.MouseUpImage = LiteResources.appConfig.imageConfig.UpgradeFailMouseUpImage;
				liteUpgradeButton.MouseEnterImage = LiteResources.appConfig.imageConfig.UpgradeFailMouseEnterImage;
				liteUpgradeButton.MouseDownImage = LiteResources.appConfig.imageConfig.UpgradeFailMouseDownImage;
				liteUpgradeButton.DisableImage = LiteResources.appConfig.imageConfig.UpgradeFailMouseUpImage;
			}
		}
	}

	public void SetState(ButtonState buttonState, bool enable)
	{
		currentButtonState = buttonState;
		switch (buttonState)
		{
		case ButtonState.Upgrading:
			upgradeButton.MouseUpImage = NormalImage;
			upgradeButton.MouseEnterImage = NormalEnterImage;
			upgradeButton.MouseDownImage = NormalDownImage;
			upgradeButton.DisableImage = NormalDownImage;
			((Control)upgradeButton).Text = "Upgrading";
			((Control)upgradeButton).Enabled = enable;
			((Control)upgradeButton).Invalidate();
			((Control)liteUpgradeButton).Text = "Upgrading";
			((Control)liteUpgradeButton).Enabled = enable;
			((Control)liteUpgradeButton).ForeColor = LiteResources.appConfig.SuccessUpgradeButton.textColor;
			liteUpgradeButton.MouseUpBackColor = LiteResources.appConfig.SuccessUpgradeButton.backColor;
			liteUpgradeButton.MouseEnterBackColor = LiteResources.appConfig.SuccessUpgradeButton.backColor;
			liteUpgradeButton.MouseDownBackColor = LiteResources.appConfig.SuccessUpgradeButton.backColor;
			liteUpgradeButton.DisableBackColor = Color.Silver;
			liteUpgradeButton.DisableForeColor = Color.DimGray;
			SetButtonImage(isSuccess: true);
			((Control)liteUpgradeButton).Invalidate();
			break;
		case ButtonState.Success:
			upgradeButton.MouseUpImage = SuccessImage;
			upgradeButton.MouseEnterImage = SuccessEnterImage;
			upgradeButton.MouseDownImage = SuccessDownImage;
			upgradeButton.DisableImage = SuccessDownImage;
			((Control)upgradeButton).Text = "Success";
			((Control)upgradeButton).Enabled = enable;
			((Control)upgradeButton).Invalidate();
			((Control)liteUpgradeButton).Text = "Success";
			((Control)liteUpgradeButton).Enabled = enable;
			((Control)liteUpgradeButton).ForeColor = LiteResources.appConfig.SuccessUpgradeButton.textColor;
			liteUpgradeButton.MouseUpBackColor = LiteResources.appConfig.SuccessUpgradeButton.backColor;
			liteUpgradeButton.MouseEnterBackColor = LiteResources.appConfig.SuccessUpgradeButton.backColor;
			liteUpgradeButton.MouseDownBackColor = LiteResources.appConfig.SuccessUpgradeButton.backColor;
			liteUpgradeButton.DisableBackColor = Color.Silver;
			liteUpgradeButton.DisableForeColor = Color.DimGray;
			SetButtonImage(isSuccess: true);
			((Control)liteUpgradeButton).Invalidate();
			break;
		case ButtonState.Fail:
			upgradeButton.MouseUpImage = FailImage;
			upgradeButton.MouseEnterImage = FailEnterImage;
			upgradeButton.MouseDownImage = FailDownImage;
			upgradeButton.DisableImage = FailDownImage;
			((Control)upgradeButton).Text = "Fail";
			((Control)upgradeButton).Enabled = enable;
			((Control)upgradeButton).Invalidate();
			((Control)liteUpgradeButton).Text = "Fail";
			((Control)liteUpgradeButton).Enabled = enable;
			((Control)liteUpgradeButton).ForeColor = LiteResources.appConfig.FailUpgradeButton.textColor;
			liteUpgradeButton.MouseUpBackColor = LiteResources.appConfig.FailUpgradeButton.backColor;
			liteUpgradeButton.MouseEnterBackColor = LiteResources.appConfig.FailUpgradeButton.backColor;
			liteUpgradeButton.MouseDownBackColor = LiteResources.appConfig.FailUpgradeButton.backColor;
			liteUpgradeButton.DisableBackColor = LiteResources.appConfig.FailUpgradeButton.backColor;
			liteUpgradeButton.DisableForeColor = Color.DimGray;
			SetButtonImage(isSuccess: false);
			((Control)liteUpgradeButton).Invalidate();
			break;
		default:
			upgradeButton.MouseUpImage = NormalImage;
			upgradeButton.MouseEnterImage = NormalEnterImage;
			upgradeButton.MouseDownImage = NormalDownImage;
			upgradeButton.DisableImage = NormalDownImage;
			((Control)upgradeButton).Text = "Upgrade";
			((Control)upgradeButton).Enabled = enable;
			((Control)upgradeButton).Invalidate();
			((Control)liteUpgradeButton).Text = "Upgrade";
			((Control)liteUpgradeButton).Enabled = enable;
			((Control)liteUpgradeButton).ForeColor = LiteResources.appConfig.SuccessUpgradeButton.textColor;
			liteUpgradeButton.MouseUpBackColor = LiteResources.appConfig.SuccessUpgradeButton.backColor;
			liteUpgradeButton.MouseEnterBackColor = LiteResources.appConfig.SuccessUpgradeButton.backColor;
			liteUpgradeButton.MouseDownBackColor = LiteResources.appConfig.SuccessUpgradeButton.backColor;
			liteUpgradeButton.DisableBackColor = Color.Silver;
			liteUpgradeButton.DisableForeColor = Color.DimGray;
			SetButtonImage(isSuccess: true);
			((Control)liteUpgradeButton).Invalidate();
			break;
		}
	}

	public void SetPairState(ButtonState buttonState, bool enable)
	{
		currentButtonState = buttonState;
		switch (buttonState)
		{
		case ButtonState.Upgrading:
			((Control)liteUpgradeButton).Enabled = enable;
			liteUpgradeButton.TextFixLocation = liteUpgradeButton.textPoint;
			liteUpgradeButton.TextFixLocationEnable = true;
			((Control)liteUpgradeButton).ForeColor = LiteResources.appConfig.SuccessUpgradeButton.textColor;
			liteUpgradeButton.DisableForeColor = Color.DimGray;
			liteUpgradeButton.MouseUpBackColor = LiteResources.appConfig.SuccessUpgradeButton.backColor;
			liteUpgradeButton.MouseEnterBackColor = LiteResources.appConfig.SuccessUpgradeButton.backColor;
			liteUpgradeButton.MouseDownBackColor = LiteResources.appConfig.SuccessUpgradeButton.backColor;
			liteUpgradeButton.DisableBackColor = LiteResources.appConfig.SuccessUpgradeButton.backColor;
			SetButtonImage(isSuccess: true);
			((Control)liteUpgradeButton).Invalidate();
			break;
		case ButtonState.Success:
			((Control)liteUpgradeButton).Enabled = enable;
			liteUpgradeButton.TextFixLocationEnable = false;
			((Control)liteUpgradeButton).ForeColor = LiteResources.appConfig.SuccessUpgradeButton.textColor;
			liteUpgradeButton.DisableForeColor = Color.DimGray;
			liteUpgradeButton.MouseUpBackColor = LiteResources.appConfig.SuccessUpgradeButton.backColor;
			liteUpgradeButton.MouseEnterBackColor = LiteResources.appConfig.SuccessUpgradeButton.backColor;
			liteUpgradeButton.MouseDownBackColor = LiteResources.appConfig.SuccessUpgradeButton.backColor;
			liteUpgradeButton.DisableBackColor = LiteResources.appConfig.SuccessUpgradeButton.backColor;
			SetButtonImage(isSuccess: true);
			((Control)liteUpgradeButton).Invalidate();
			break;
		case ButtonState.Fail:
			((Control)liteUpgradeButton).Enabled = enable;
			liteUpgradeButton.TextFixLocationEnable = false;
			((Control)liteUpgradeButton).ForeColor = LiteResources.appConfig.FailUpgradeButton.textColor;
			liteUpgradeButton.DisableForeColor = Color.DimGray;
			liteUpgradeButton.MouseUpBackColor = LiteResources.appConfig.FailUpgradeButton.backColor;
			liteUpgradeButton.MouseEnterBackColor = LiteResources.appConfig.FailUpgradeButton.backColor;
			liteUpgradeButton.MouseDownBackColor = LiteResources.appConfig.FailUpgradeButton.backColor;
			liteUpgradeButton.DisableBackColor = LiteResources.appConfig.FailUpgradeButton.backColor;
			SetButtonImage(isSuccess: false);
			((Control)liteUpgradeButton).Invalidate();
			break;
		}
	}
}
