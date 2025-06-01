namespace WindControls;

public class WindMath
{
	public static void ArraySet(byte[] array, byte value, int index, int length)
	{
		for (int i = index; i < index + length; i++)
		{
			array[i] = value;
		}
	}
}
