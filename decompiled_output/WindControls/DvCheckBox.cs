using System.Drawing;

namespace WindControls;

public class DvCheckBox
{
	public bool isChecked = false;

	private Rectangle ClientRectangle = new Rectangle(0, 0, 0, 0);

	private Image CheckedImage = (Image)(object)FontImages.GetImage("A_fa_check_square_o", 32, Color.Red, Color.Transparent);

	private Image UnCheckedImage = (Image)(object)FontImages.GetImage("A_fa_square_o", 32, Color.Red, Color.Transparent);

	public void Paint(Graphics graphics, Rectangle rectangle)
	{
		ClientRectangle = rectangle;
		if (CheckedImage != null && UnCheckedImage != null)
		{
			if (rectangle.Width > UnCheckedImage.Width)
			{
				ClientRectangle.Width = UnCheckedImage.Width;
			}
			if (rectangle.Height > UnCheckedImage.Height)
			{
				ClientRectangle.Height = UnCheckedImage.Height;
			}
			graphics.DrawImage(isChecked ? CheckedImage : UnCheckedImage, rectangle.Location);
		}
	}

	public void SetImage(Image checkedImage, Image unCheckedImage)
	{
		CheckedImage = checkedImage;
		UnCheckedImage = unCheckedImage;
	}

	public bool Contain(Point point)
	{
		return ClientRectangle.Contains(point);
	}

	public bool Click(Point point)
	{
		if (Contain(point))
		{
			isChecked = !isChecked;
			return true;
		}
		return false;
	}

	public bool CheckedChanged(Point point)
	{
		if (ClientRectangle.Contains(point) != isChecked)
		{
			isChecked = !isChecked;
			return true;
		}
		return false;
	}
}
