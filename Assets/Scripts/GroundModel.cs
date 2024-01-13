using System;
using System.Collections.Generic;
using System.Linq;

public class GroundModel
{
	private readonly IGroundData _data;

	public GroundBonusContainer BonusContainer;

	private readonly IGroundWorkerData _workerData;

	private readonly MaxUpgradeHelper _maxUpgradeHelper;

	public bool isManagerActive;

	public int Id;

	public int Level = 1;

	public double GoldStored;

	public UpgradeFactor UpgradeFactor;

	public IStatsIncreaseModel StatsIncreaseModel
	{
		get;
		private set;
	}

	public List<GroundWorkerModel> GroundWorkers
	{
		get;
		private set;
	}

	public AbstractStatValue<double> UpgradeCostStat
	{
		get;
		private set;
	}

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

	public AbstractStatValue<int> NumberOfWorkersStat
	{
		get;
		private set;
	}

	public AbstractStatValue<double> TransportingPerSecondStat
	{
		get;
		private set;
	}

	public AbstractStatValue<double> WalkingSpeedStat
	{
		get;
		private set;
	}

	public double GroundLength => _data.GroundLength();

	public List<GroundWorkerModel> WaitingGroundWorker => (from w in GroundWorkers
		where w.GroundWorkerState == GroundWorkerState.Waiting
		select w).ToList();

	public int MaxLevel => _data.MaxGroundLevel();

	public bool IsMaxLevel => Level == MaxLevel;

	public double NextSuperCashGain => _data.SuperCashGain(NextBigUpgradeLevel);

	public int NextBigUpgradeLevel => _data.NextBigUpgrade(Level);

	public int LastBigUpgradeAtLevel => _data.LastBigUpgrade(Level);

	public GroundModel(IStatsIncreaseModel statsIncreaseModel, IGroundData data, IGroundWorkerData workerData)
	{
		_data = data;
		_workerData = workerData;
		StatsIncreaseModel = statsIncreaseModel;
		GroundWorkers = new List<GroundWorkerModel>();
		UpgradeCostStat = new GroundUpgradeCostStatValue(this, _data, _workerData);
		CapacityStat = new GroundCapacityStatValue(this, _data, _workerData);
		LoadingPerSecondStat = new GroundLoadingPerSecondStatValue(this, _data, _workerData);
		NumberOfWorkersStat = new GroundNumberOfWorkersStatValue(this, _data, _workerData);
		TransportingPerSecondStat = new GroundTransportingPerSecondStatValue(this, _data, _workerData);
		WalkingSpeedStat = new GroundWalkingSpeedStatValue(this, _data, _workerData);
		BonusContainer = GroundBonusContainer.Empty;
		_maxUpgradeHelper = new MaxUpgradeHelper(_data, BonusContainer);
	}

	private int GetBoostLevelInRange(int startLevel, int offset)
	{
		if (startLevel < 0 || offset < 0)
		{
			throw new ArgumentException("level or levelOffset is smaller than 0");
		}
		for (int i = 1; i <= offset; i++)
		{
			if (_data.IsBigUpgrade(startLevel + i))
			{
				return startLevel + i;
			}
		}
		return 0;
	}

	public GroundWorkerModel AddWorkerModel()
	{
		GroundWorkerModel groundWorkerModel = new GroundWorkerModel(this, StatsIncreaseModel, _data, _workerData);
		GroundWorkers.Add(groundWorkerModel);
		return groundWorkerModel;
	}

	public int GetMaxAffordableNumbersOfLevelsToUpgrade(double cash)
	{
		return _maxUpgradeHelper.GetMaxAffordableNumbersOfLevelsToUpgrade(Level, cash);
	}

	private int GetPossibleNumberOfLevelsToUpgrade(int numberOfLevels)
	{
		if (Level + numberOfLevels > MaxLevel)
		{
			numberOfLevels = MaxLevel - Level;
		}
		return numberOfLevels;
	}

	public void SetBonusContainer(int effectID, int managerID)
	{
		BonusContainer = new GroundBonusContainer(effectID, managerID);
		_maxUpgradeHelper.BonusContainer = BonusContainer;
	}

	public void ResetBonusContainer()
	{
		BonusContainer = GroundBonusContainer.Empty;
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
			num += _data.SuperCashGain(level + i);
		}
		return num;
	}
}
