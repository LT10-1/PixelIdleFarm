using System;

public class GroundWorkerModel
{
	public IGroundWorkerData Data;

	private bool _publishedStateSwitch;

	public double CurrentLoadGold;

	public GroundWorkerState GroundWorkerState;

	public GroundModel Ground
	{
		get;
		private set;
	}

	public AbstractStatValue<double> WalkingSpeedPerSecondStat
	{
		get;
		protected set;
	}

	public AbstractStatValue<double> LoadingPerSecondStat
	{
		get;
		protected set;
	}

	public AbstractStatValue<double> CapacityStat
	{
		get;
		protected set;
	}

	public AbstractStatValue<double> SecondsOneWayStat
	{
		get;
		protected set;
	}

	public AbstractStatValue<double> TransportingPerSecondStat
	{
		get;
		protected set;
	}

	public double PositionInPercent
	{
		get
		{
			switch (GroundWorkerState)
			{
			case GroundWorkerState.Waiting:
			case GroundWorkerState.WalkingToElevator:
			case GroundWorkerState.Unloading:
				return 0.0;
			case GroundWorkerState.WalkingToHouse:
			case GroundWorkerState.Loading:
				return 1.0;
			default:
				return -1.0;
			}
		}
	}

	public double LoadingAmount => Math.Min(Ground.GoldStored, CapacityStat.Value);

	public double SecondsTillLoaded => LoadingAmount / LoadingPerSecondStat.Value;

	public double SecondsTillUnloaded => CurrentLoadGold / LoadingPerSecondStat.Value;

	public double GoldLoaded
	{
		get
		{
			return CurrentLoadGold;
		}
		set
		{
		}
	}

	public GroundWorkerModel(GroundModel ground, IStatsIncreaseModel statsIncreaseModel, IGroundData groundData, IGroundWorkerData groundWorkerData)
	{
		Ground = ground;
		Data = groundWorkerData;
		WalkingSpeedPerSecondStat = new GroundWorkerWalkingSpeedStatValue(Ground, this, groundData, groundWorkerData);
		LoadingPerSecondStat = new GroundWorkerLoadingPerSecondStatValue(Ground, this, statsIncreaseModel, groundData, groundWorkerData);
		CapacityStat = new GroundWorkerCapacityStatValue(Ground, this, statsIncreaseModel, groundData, groundWorkerData);
		SecondsOneWayStat = new GroundWorkerSecondsOneWayStatValue(Ground, this, groundData, groundWorkerData);
		TransportingPerSecondStat = new GroundWorkerTransportingPerSecondStatValue(Ground, this, groundData, groundWorkerData);
	}

	public void ClickWorker()
	{
		if (GroundWorkerState != 0)
		{
		}
	}

	public void StartWalkingToElevator()
	{
		if (GroundWorkerState != 0)
		{
		}
	}
}
