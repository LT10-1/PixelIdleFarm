using System;

[Serializable]
public class DataGame
{
	public string userName
	{
		get;
		set;
	}

	public string passWord
	{
		get;
		set;
	}

	public bool isNhacNen
	{
		get;
		set;
	}

	public bool isAmThanh
	{
		get;
		set;
	}

	public bool isNhanLoiMoiChoi
	{
		get;
		set;
	}

	public bool isTuDongSS
	{
		get;
		set;
	}

	public int numAppear
	{
		get;
		set;
	}

	public DataGame()
	{
		userName = string.Empty;
		passWord = string.Empty;
		isNhacNen = true;
		isAmThanh = true;
		isNhanLoiMoiChoi = true;
		isTuDongSS = true;
		numAppear = 0;
	}
}
