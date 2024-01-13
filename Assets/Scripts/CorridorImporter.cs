using System;

public class CorridorImporter : ICorridorData, ICorridorWorkerData, IUpgradable
{
	private static CorridorImporter _instance;

	public static CorridorImporter Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new CorridorImporter();
			}
			return _instance;
		}
	}

	public int MaxLevel => MaxCorridorLevel();

	public double Cost(int corridorLevel, int tier)
	{
		return DataManager.Instance.CorridorEntityParams[tier][GetSafeLevel(corridorLevel)].Cost;
	}

	public int NumberOfWorkers(int corridorLevel, int tier)
	{
		return DataManager.Instance.CorridorEntityParams[tier][GetSafeLevel(corridorLevel)].NumberOfWorkers;
	}

	public int StartLength()
	{
		return 2;
	}

	public int MaxLength()
	{
		return 8;
	}

	public int MaxCorridorLevel()
	{
		return 800;
	}

	public bool IsBigUpgrade(int corridorLevel, int tier)
	{
		return DataManager.Instance.CorridorEntityParams[tier][GetSafeLevel(corridorLevel)].BigUpdate;
	}

	public int NextBigUpgrade(int corridorLevel, int tier)
	{
		for (int i = GetSafeLevel(corridorLevel) + 1; i <= MaxCorridorLevel(); i++)
		{
			if (IsBigUpgrade(i, tier))
			{
				return i;
			}
		}
		return -1;
	}

	public int LastBigUpgrade(int corridorLevel, int tier)
	{
		for (int num = GetSafeLevel(corridorLevel); num >= 1; num--)
		{
			if (IsBigUpgrade(num, tier))
			{
				return num;
			}
		}
		return 1;
	}

	public int GetSafeLevel(int level)
	{
		return Math.Min(level, MaxCorridorLevel());
	}

	public double SuperCashGain(int tier, int level)
	{
		return DataManager.Instance.CorridorEntityParams[tier][GetSafeLevel(level)].SuperCashReward;
	}

	public double GainPerSecond(int corridorLevel, int tier, IStatsIncreaseModel statsIncreaseModel)
	{
		return DataManager.Instance.CorridorEntityParams[tier][GetSafeLevel(corridorLevel)].GainPerSecondPerWorker * statsIncreaseModel.CorridorTotalFactor;
	}

	public double Capacity(int corridorLevel, int tier, IStatsIncreaseModel statsIncreaseModel)
	{
		return DataManager.Instance.CorridorEntityParams[tier][GetSafeLevel(corridorLevel)].CapacityPerWorker * statsIncreaseModel.CorridorTotalFactor;
	}

	public int WalkingSpeedPerSecond(int corridorLevel, int tier)
	{
		return DataManager.Instance.CorridorEntityParams[tier][GetSafeLevel(corridorLevel)].WorkerWalkingSpeedPerSecond;
	}
}
