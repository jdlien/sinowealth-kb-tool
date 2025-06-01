namespace WindControls;

public class WArray
{
	public static void Set(byte[] array, byte value, int startIndex, int length)
	{
		for (int i = startIndex; i < startIndex + length; i++)
		{
			array[i] = value;
		}
	}

	public static void Set(int[] array, int value, int startIndex, int length)
	{
		for (int i = startIndex; i < startIndex + length; i++)
		{
			array[i] = value;
		}
	}

	public static bool Equals(byte[] arr1, byte[] arr2)
	{
		if (arr1.Length != arr2.Length)
		{
			return false;
		}
		for (int i = 0; i < arr1.Length; i++)
		{
			if (arr1[i] != arr2[i])
			{
				return false;
			}
		}
		return true;
	}

	public static bool Equals(int[] arr1, int[] arr2)
	{
		if (arr1.Length != arr2.Length)
		{
			return false;
		}
		for (int i = 0; i < arr1.Length; i++)
		{
			if (arr1[i] != arr2[i])
			{
				return false;
			}
		}
		return true;
	}

	public static int ByteToInt(byte[] array, int startIndex, int length, bool isHighToLow)
	{
		int[] array2 = new int[4] { 24, 16, 8, 0 };
		int[] array3 = new int[4] { 0, 8, 16, 24 };
		int num = 0;
		if (length > 4)
		{
			length = 4;
		}
		for (int i = 0; i < length; i++)
		{
			num = ((!isHighToLow) ? (num | (array[startIndex + i] << array3[i])) : (num | (array[startIndex + i] << array2[i])));
		}
		return num;
	}
}
