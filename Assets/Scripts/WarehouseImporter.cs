public class WarehouseImporter : IGroundData, IGroundWorkerData, IUpgradable
{
	private static WarehouseImporter _instance;

	public static WarehouseImporter Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new WarehouseImporter();
			}
			return _instance;
		}
	}

	public int MaxLevel => MaxGroundLevel();

	public double Cost(int level, int tier)
	{
		return Cost(level);
	}

	public double Cost(int groundLevel)
	{
		return DataManager.Instance.WarehouseParams[groundLevel].Cost;
	}

	public int NumberOfWorkers(int groundLevel)
	{
		return DataManager.Instance.WarehouseParams[groundLevel].NumberOfWorkers;
	}

	public int MaxGroundLevel()
	{
		return 2400;
	}

	public int GroundLength()
	{
		return 10;
	}

	public bool IsBigUpgrade(int groundLevel)
	{
		return DataManager.Instance.WarehouseParams[groundLevel].BigUpdate;
	}

	public int NextBigUpgrade(int groundLevel)
	{
		for (int i = groundLevel + 1; i <= MaxGroundLevel(); i++)
		{
			if (IsBigUpgrade(i))
			{
				return i;
			}
		}
		return 0;
	}

	public int LastBigUpgrade(int groundLevel)
	{
		for (int num = groundLevel; num >= 1; num--)
		{
			if (IsBigUpgrade(num))
			{
				return num;
			}
		}
		return 1;
	}

	public double SuperCashGain(int groundLevel)
	{
		return DataManager.Instance.WarehouseParams[groundLevel].SuperCashReward;
	}

	public double Capacity(int groundLevel, IStatsIncreaseModel statsIncreaseModel)
	{
		return DataManager.Instance.WarehouseParams[groundLevel].CapacityPerWorker * statsIncreaseModel.GroundTotalFactor;
	}

	public int WalkingSpeedPerSecond(int groundLevel)
	{
		return DataManager.Instance.WarehouseParams[groundLevel].WorkerWalkingSpeedPerSecond;
	}

	public double LoadingPerSecond(int groundLevel, IStatsIncreaseModel statsIncreaseModel)
	{
		return DataManager.Instance.WarehouseParams[groundLevel].LoadingPerSecond * statsIncreaseModel.GroundTotalFactor;
	}
}
