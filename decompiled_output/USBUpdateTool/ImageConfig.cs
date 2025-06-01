using System;
using System.Drawing;
using WindControls;

namespace USBUpdateTool;

public class ImageConfig
{
	public Image BackgroundImage = null;

	public Image LogoImage = null;

	public Image PairBackImage = null;

	public byte[] multiLogoIcon = null;

	public Image UpgradeMouseUpImage = null;

	public Image UpgradeMouseEnterImage = null;

	public Image UpgradeMouseDownImage = null;

	public Image UpgradeFailMouseUpImage = null;

	public Image UpgradeFailMouseEnterImage = null;

	public Image UpgradeFailMouseDownImage = null;

	public Image ProgressForeImage = null;

	public Image ProgressBackImage = null;

	public Image ProgressSliderImage = null;

	public Rectangle PorgressSliderImageRect = default(Rectangle);

	public Rectangle PorgressForeImageRect = default(Rectangle);

	public Rectangle PorgressBackImageRect = default(Rectangle);

	public Point PorgressTextLocation = default(Point);

	public bool PorgressSliderMovable = false;

	public bool PorgressShowPercent = true;

	public string ToPorgressConfig()
	{
		string text = "";
		text += PorgressSliderImageRect.X;
		text = text + AppConfig.splitChar + PorgressSliderImageRect.Y;
		text = text + AppConfig.splitChar + PorgressSliderImageRect.Width;
		text = text + AppConfig.splitChar + PorgressSliderImageRect.Height;
		text = text + AppConfig.splitChar + PorgressForeImageRect.X;
		text = text + AppConfig.splitChar + PorgressForeImageRect.Y;
		text = text + AppConfig.splitChar + PorgressForeImageRect.Width;
		text = text + AppConfig.splitChar + PorgressForeImageRect.Height;
		text = text + AppConfig.splitChar + PorgressBackImageRect.X;
		text = text + AppConfig.splitChar + PorgressBackImageRect.Y;
		text = text + AppConfig.splitChar + PorgressBackImageRect.Width;
		text = text + AppConfig.splitChar + PorgressBackImageRect.Height;
		text = text + AppConfig.splitChar + PorgressTextLocation.X;
		text = text + AppConfig.splitChar + PorgressTextLocation.Y;
		text = ((!PorgressSliderMovable) ? (text + AppConfig.splitChar + "0") : (text + AppConfig.splitChar + "1"));
		if (PorgressShowPercent)
		{
			return text + AppConfig.splitChar + "1";
		}
		return text + AppConfig.splitChar + "0";
	}

	public void ToPorgressControl(string config)
	{
		string[] array = config.Split(new char[1] { AppConfig.splitChar });
		int[] array2 = new int[16];
		for (int i = 0; i < array2.Length; i++)
		{
			if (StringHelper.IsIHexString(array[i]))
			{
				array2[i] = Convert.ToInt32(array[i]);
			}
		}
		PorgressSliderImageRect = new Rectangle(array2[0], array2[1], array2[2], array2[3]);
		PorgressForeImageRect = new Rectangle(array2[4], array2[5], array2[6], array2[7]);
		PorgressBackImageRect = new Rectangle(array2[8], array2[9], array2[10], array2[11]);
		PorgressTextLocation = new Point(array2[12], array2[13]);
		PorgressSliderMovable = array2[14] > 0;
		PorgressShowPercent = array2[15] > 0;
	}
}
