using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;

public static class NumberFormatter
{
	public enum TimeSuffix
	{
		d,
		h,
		m,
		s
	}

	private static int timeSuffixCount;

	public static bool DebugNumberFormat;

	private static string[] _suffixes;

	private static int _maxNumber;

	private static Random rng;

	static NumberFormatter()
	{
		rng = new Random();
		_maxNumber = 82;
		InitSuffixes();
	}

	public static void Shuffle<T>(this IList<T> list)
	{
		int num = list.Count;
		while (num > 1)
		{
			num--;
			int index = rng.Next(num + 1);
			T value = list[index];
			list[index] = list[num];
			list[num] = value;
		}
	}

	public static double Round(double d, int digits)
	{
		if (d <= 0.0)
		{
			return d;
		}
		double num = Math.Pow(10.0, Math.Floor(Math.Log10(Math.Abs(d))) + 1.0);
		return num * Math.Round(d / num, digits);
	}

	public static long SecondsToMinutes(this long seconds)
	{
		return seconds / 60;
	}

	public static long SecondsToHours(this long seconds)
	{
		return seconds / 3600;
	}

	public static string MinifyFormat(this double num)
	{
		double num2 = Round(num, 3);
		if (DebugNumberFormat)
		{
			return num2.DebugNumberFormation();
		}
		return Minify(num2);
	}

	private static string DebugNumberFormation(this double num)
	{
		return num.ToString("E1");
	}

	public static string ToInvariantCultureString(this double num)
	{
		return num.ToString(CultureInfo.InvariantCulture);
	}

	private static string MinifySuperCashValue(this double num)
	{
		num = Math.Floor(num);
		if (num > 1000000.0)
		{
			return Minify(num);
		}
		if (num < 1000.0)
		{
			return num.ToString("##0", CultureInfo.InvariantCulture);
		}
		return num.ToString("# ##0", CultureInfo.InvariantCulture);
	}

	public static string MinifyIncomeFactor(this double num)
	{
		return num.ToString("0.#", CultureInfo.InvariantCulture);
	}

	public static string ConvertTimeToHoursMinuteSeconds(this TimeSpan time)
	{
		return $"{(int)Math.Floor(time.TotalHours):D2}:{time.Minutes:D2}:{time.Seconds:D2}";
	}

	private static List<TimeSuffix> GetNeededSuffixesOrdered(TimeSpan timeSpan, int MaxSuffixCount, TimeSuffix MaxSuffix)
	{
		List<TimeSuffix> list = new List<TimeSuffix>();
		if ((timeSpan.Days > 0 || (MaxSuffix == TimeSuffix.d && (int)timeSpan.TotalDays > 0)) && list.Count < MaxSuffixCount && MaxSuffix <= TimeSuffix.d)
		{
			list.Add(TimeSuffix.d);
		}
		if ((timeSpan.Hours > 0 || (MaxSuffix == TimeSuffix.h && (int)timeSpan.TotalHours > 0)) && list.Count < MaxSuffixCount && MaxSuffix <= TimeSuffix.h)
		{
			list.Add(TimeSuffix.h);
		}
		if ((timeSpan.Minutes > 0 || (MaxSuffix == TimeSuffix.m && (int)timeSpan.TotalMinutes > 0)) && list.Count < MaxSuffixCount && MaxSuffix <= TimeSuffix.m)
		{
			list.Add(TimeSuffix.m);
		}
		if ((timeSpan.Seconds > 0 || (MaxSuffix == TimeSuffix.s && (int)timeSpan.TotalSeconds > 0)) && list.Count < MaxSuffixCount && MaxSuffix <= TimeSuffix.s)
		{
			list.Add(TimeSuffix.s);
		}
		if (timeSpan.Seconds == 0 && list.Count == 0)
		{
			list.Add(TimeSuffix.s);
		}
		return list;
	}

	private static void InitSuffixes()
	{
		string text = "abcdefghijklmnopqrstuvwxyz";
		_suffixes = new string[_maxNumber];
		_suffixes[0] = "K";
		_suffixes[1] = "M";
		_suffixes[2] = "B";
		_suffixes[3] = "T";
		for (int i = 0; i < text.Length; i++)
		{
			_suffixes[4 + i] = "a" + text[i];
		}
		for (int j = 0; j < text.Length; j++)
		{
			_suffixes[30 + j] = "b" + text[j];
		}
		for (int k = 0; k < text.Length; k++)
		{
			_suffixes[56 + k] = "c" + text[k];
		}
	}

	private static string Minify(double num)
	{
		for (int num2 = _maxNumber; num2 > 0; num2--)
		{
			if (num >= Math.Pow(10.0, 3 * num2))
			{
				return (num / Math.Pow(10.0, 3 * num2)).ToString("0.##", CultureInfo.InvariantCulture) + _suffixes[num2 - 1];
			}
		}
		return num.ToString(CultureInfo.InvariantCulture);
	}

	public static string FormatTimeString(this double totalSeconds, bool getFull = false)
	{
		return ((long)totalSeconds).FormatTimeString(getFull);
	}

	public static string FormatTimeString(this long totalSeconds, bool getFull = false)
	{
		TimeSpan timeSpan = TimeSpan.FromSeconds(totalSeconds);
		int days = timeSpan.Days;
		int hours = timeSpan.Hours;
		int minutes = timeSpan.Minutes;
		int seconds = timeSpan.Seconds;
		string text = string.Empty;
		int num = 0;
		int num2 = 2;
		if (getFull)
		{
			num2 = 10;
		}
		if (days > 0 && num < num2)
		{
			text = text + days + "d ";
			num++;
		}
		if (hours > 0 && num < num2)
		{
			text = text + hours + "h ";
			num++;
		}
		if (minutes > 0 && num < num2)
		{
			text = text + minutes + "m ";
			num++;
		}
		if (seconds > 0 && num < num2)
		{
			text = text + seconds + "s ";
			num++;
		}
		if (text.Length == 0)
		{
			text = "0s";
		}
		return text;
	}

	public static bool InRange(this int value, int minimum, int maximum)
	{
		return value >= minimum && value <= maximum;
	}

	public static string JsonSerialize(this object obj)
	{
		return JsonConvert.SerializeObject(obj);
	}
}
