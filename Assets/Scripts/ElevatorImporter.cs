public class ElevatorImporter : IElevatorData, IUpgradable
{
	private static ElevatorImporter _instance;

	public static ElevatorImporter Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new ElevatorImporter();
			}
			return _instance;
		}
	}

	public int MaxLevel => MaxElevatorLevel();

	public double Cost(int level, int tier)
	{
		return Cost(level);
	}

	public double Cost(int elevatorLevel)
	{
		return DataManager.Instance.ElevatorParams[elevatorLevel].Cost;
	}

	public double Capacity(int elevatorLevel, IStatsIncreaseModel statsIncreaseModel)
	{
		return DataManager.Instance.ElevatorParams[elevatorLevel].Capacity * statsIncreaseModel.ElevatorTotalFactor;
	}

	public double SpeedInTiersPerSecond(int elevatorLevel)
	{
		return DataManager.Instance.ElevatorParams[elevatorLevel].Speed;
	}

	public double LoadingPerSecond(int elevatorLevel, IStatsIncreaseModel statsIncreaseModel)
	{
		return DataManager.Instance.ElevatorParams[elevatorLevel].LoadingPerSecond * statsIncreaseModel.ElevatorTotalFactor;
	}

	public bool IsBigUpgrade(int elevatorLevel)
	{
		return DataManager.Instance.ElevatorParams[elevatorLevel].BigUpdate;
	}

	public int NextBigUpgrade(int elevatorLevel)
	{
		for (int i = elevatorLevel + 1; i <= MaxElevatorLevel(); i++)
		{
			if (IsBigUpgrade(i))
			{
				return i;
			}
		}
		return 0;
	}

	public int LastBigUpgrade(int elevatorLevel)
	{
		for (int num = elevatorLevel; num >= 1; num--)
		{
			if (IsBigUpgrade(num))
			{
				return num;
			}
		}
		return 1;
	}

	public double SuperCashGain(int elevatorLevel)
	{
		return DataManager.Instance.ElevatorParams[elevatorLevel].SuperCashReward;
	}

	public int MaxElevatorLevel()
	{
		return 2400;
	}
}
