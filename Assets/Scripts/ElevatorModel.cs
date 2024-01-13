using System;

public class ElevatorModel
{
	private bool _startedCycling;

	private readonly IStatsIncreaseModel _statsIncreaseModel;

	private readonly IElevatorData _elevatorData;

	private readonly MaxUpgradeHelper _maxUpgradeHelper;

	public ElevatorBonusContainer BonusContainer;

	public bool isManagerActive;

	public int Level = 1;

	public int CurrentTier = 1;

	public double CurrentGoldLoaded;

	public bool IsOnBottomCorridor;

	public AbstractStatValue<double> CapacityStat
	{
		get;
		private set;
	}

	public AbstractStatValue<double> LoadingPerSecondStat
	{
		get;
		private set;
	}

	public AbstractStatValue<double> SpeedStat
	{
		get;
		private set;
	}

	public AbstractStatValue<double> TransportingPerSecondStat
	{
		get;
		private set;
	}

	public AbstractStatValue<double> UpgradeCostStat
	{
		get;
		private set;
	}

	public int MaxLevel => _elevatorData.MaxLevel;

	public bool IsMaxLevel => Level == MaxLevel;

	private double UpgradeCost => UpgradeCostStat.ValueAtNextLevel(1);

	public double SecondsToDriveOneTier => 1.0 / SpeedStat.Value;

	public double SecondsToDriveFromCurrentCorridorToTop => (double)CurrentTier * SecondsToDriveOneTier;

	public double SecondsTillUnloadingCompleted => CurrentGoldLoaded / (_elevatorData.LoadingPerSecond(Level, _statsIncreaseModel) * BonusContainer.TotalLoadingPerSecondFactor);

	public double SecondsTillLoadingCompletedFromCurrentCorridor => AmountToLoadFromCurrentCorridor / (_elevatorData.LoadingPerSecond(Level, _statsIncreaseModel) * BonusContainer.TotalLoadingPerSecondFactor);

	public double AmountToLoadFromCurrentCorridor
	{
		get
		{
			double val = double.MaxValue;
			double val2 = CapacityStat.Value - CurrentGoldLoaded;
			double num = Math.Min(val, val2);
			if (num < 0.0)
			{
				return 0.0;
			}
			return num;
		}
	}

	public bool IsCapacityReached => CurrentGoldLoaded >= CapacityStat.Value;

	public double NextSuperCashGain => _elevatorData.SuperCashGain(NextBigUpgradeLevel);

	public int NextBigUpgradeLevel => _elevatorData.NextBigUpgrade(Level);

	public int LastBigUpgradeAtLevel => _elevatorData.LastBigUpgrade(Level);

	public ElevatorModel(IStatsIncreaseModel statsIncreaseModel, IElevatorData elevatorData)
	{
		_elevatorData = elevatorData;
		_statsIncreaseModel = statsIncreaseModel;
		CapacityStat = new ElevatorCapacityStatValue(this, _statsIncreaseModel, _elevatorData);
		LoadingPerSecondStat = new ElevatorLoadingPerSecondStatValue(this, _statsIncreaseModel, _elevatorData);
		SpeedStat = new ElevatorSpeedStatValue(this, _elevatorData);
		TransportingPerSecondStat = new ElevatorTransportingPerSecondStatValue(this, _elevatorData);
		UpgradeCostStat = new ElevatorUpgradeCostStatValue(this, _elevatorData);
		BonusContainer = ElevatorBonusContainer.Empty;
		_maxUpgradeHelper = new MaxUpgradeHelper(_elevatorData, BonusContainer);
	}

	public int GetPossibleNumberOfLevelsToUpgrade(int numberOfLevels)
	{
		if (Level + numberOfLevels > MaxLevel)
		{
			numberOfLevels = MaxLevel - Level;
		}
		return numberOfLevels;
	}

	public int GetMaxAffordableNumbersOfLevelsToUpgrade(double cash)
	{
		return _maxUpgradeHelper.GetMaxAffordableNumbersOfLevelsToUpgrade(Level, cash);
	}

	public void SetBonusContainer(int effectID, int managerID)
	{
		BonusContainer = new ElevatorBonusContainer(effectID, managerID);
		_maxUpgradeHelper.BonusContainer = BonusContainer;
	}

	public void ResetBonusContainer()
	{
		BonusContainer = ElevatorBonusContainer.Empty;
		_maxUpgradeHelper.BonusContainer = BonusContainer;
	}

	public double SuperCashGainInRange(int level, int levelOffset)
	{
		if (level < 0)
		{
			return 0.0;
		}
		if (levelOffset < 0)
		{
			return 0.0;
		}
		double num = 0.0;
		for (int i = 1; i <= levelOffset; i++)
		{
			num += _elevatorData.SuperCashGain(level + i);
		}
		return num;
	}
}
