using System;

public class MathUtils
{
	public static bool CompareDoubleToZero(double number)
	{
		return Math.Abs(number) < double.Epsilon;
	}

	public static bool CompareDoubleBiggerThanZero(double number)
	{
		return !CompareDoubleToZero(number) && number > 0.0;
	}
}
