using _Scripts.Entities.Corridor.StatValue;
using Entities.Corridor.StatValue;
using Entities.Manager.Effect.Corridor;
using System.Collections.Generic;

public class CorridorModel
{
	private readonly ICorridorData _data;

	private readonly ICorridorWorkerData _workerData;

	private readonly MaxUpgradeHelper _maxUpgradeHelper;

	public IStatsIncreaseModel StatsIncreaseModel;

	public CorridorBonusContainer BonusContainer;

	public readonly List<CorridorWorkerModel> Workers = new List<CorridorWorkerModel>();

	public UpgradeFactor UpgradeFactor;

	public bool isManagerActive;

	public int Level = 1;

	public int Tier = 1;

	public AbstractStatValue<double> GainPerSecondStat
	{
		get;
		private set;
	}

	public AbstractStatValue<double> CapacityStat
	{
		get;
		private set;
	}

	public AbstractStatValue<double> WalkingSpeedStat
	{
		get;
		private set;
	}

	public AbstractStatValue<double> CompleteDollarPerSecondStat
	{
		get;
		private set;
	}

	public AbstractStatValue<int> NumberOfWorkersStat
	{
		get;
		private set;
	}

	public AbstractStatValue<double> CorridorUpgradeCostStat
	{
		get;
		private set;
	}

	public bool IsMaxLevel => Level == MaxLevel;

	public int MaxLevel => _data.MaxCorridorLevel();

	public double NextSuperCashGain => _data.SuperCashGain(Tier, NextBigUpgradeLevel);

	public int NextBigUpgradeLevel => _data.NextBigUpgrade(Level, Tier);

	public int LastBigUpgradeAtLevel => _data.LastBigUpgrade(Level, Tier);

	public CorridorModel(int tier, IStatsIncreaseModel statsIncreaseModel, ICorridorData data, ICorridorWorkerData workerData)
	{
		Tier = tier;
		_data = data;
		StatsIncreaseModel = statsIncreaseModel;
		_workerData = workerData;
		GainPerSecondStat = new CorridorGainPerSecondStatValue(this, _data, _workerData);
		CapacityStat = new CorridorCapacityStatValue(this, _data, _workerData);
		WalkingSpeedStat = new CorridorWalkingSpeedStatValue(this, _data, _workerData);
		CompleteDollarPerSecondStat = new CorridorCompleteGainPerSecondStatValue(this, _data, _workerData);
		NumberOfWorkersStat = new CorridorNumberOfWorkersStatValue(this, _data, _workerData);
		CorridorUpgradeCostStat = new CorridorUpgradeCostStatValue(this, _data, _workerData);
		BonusContainer = CorridorBonusContainer.Empty;
		_maxUpgradeHelper = new MaxUpgradeHelper(_data, BonusContainer, Tier);
	}

	public double GetFullLengthMined()
	{
		return (double)_data.StartLength() + CalculateLengthWithLog();
	}

	public double GetFullLengthMinedAtNextLevel(int levelOffset)
	{
		return (double)_data.StartLength() + CalculateLengthWithLogAtNextLevel(levelOffset);
	}

	public double CalculateLengthWithLogAtNextLevel(int levelOffset)
	{
		return (double)_data.MaxLength() * CalculateQuotientMinedAtNextLevel(levelOffset);
	}

	public double CalculateQuotientMinedAtNextLevel(int levelOffset)
	{
		return (double)(Level + levelOffset - 1) / (double)_data.MaxCorridorLevel();
	}

	public double CalculateLengthWithLog()
	{
		return (double)_data.MaxLength() * CalculateQuotientMined();
	}

	public double CalculateQuotientMined()
	{
		return (double)(Level - 1) / (double)_data.MaxCorridorLevel();
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

	public CorridorWorkerModel AddWorkerModel()
	{
		CorridorWorkerModel corridorWorkerModel = new CorridorWorkerModel(this, _data, _workerData);
		Workers.Add(corridorWorkerModel);
		return corridorWorkerModel;
	}

	public void SetBonusContainer(int effectID, int managerID)
	{
		BonusContainer = new CorridorBonusContainer(effectID, managerID);
		_maxUpgradeHelper.BonusContainer = BonusContainer;
	}

	public void ResetBonusContainer()
	{
		BonusContainer = CorridorBonusContainer.Empty;
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
			num += _data.SuperCashGain(Tier, level + i);
		}
		return num;
	}
}
