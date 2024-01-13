public class CorridorWorkerModel
{
	public CorridorWorkerState CorridorWorkerState;

	public CorridorModel Corridor
	{
		get;
		private set;
	}

	public AbstractStatValue<double> CapacityStat
	{
		get;
		private set;
	}

	public AbstractStatValue<double> GainPerSecondStat
	{
		get;
		private set;
	}

	public AbstractStatValue<double> SecondsOneTurnStat
	{
		get;
		private set;
	}

	public AbstractStatValue<double> SecondsOneWayStat
	{
		get;
		private set;
	}

	public AbstractStatValue<double> SecondsTillCapacityReachedStat
	{
		get;
		private set;
	}

	public AbstractStatValue<double> WalkingSpeedPerSecondStat
	{
		get;
		private set;
	}

	public AbstractStatValue<double> TransportingPerSecondStat
	{
		get;
		private set;
	}

	public double PositionPercentage
	{
		get
		{
			switch (CorridorWorkerState)
			{
			case CorridorWorkerState.Idle:
			case CorridorWorkerState.WalkingFromStorageToStones:
				return 0.0;
			case CorridorWorkerState.WorkingOnStones:
			case CorridorWorkerState.WalkingFromStonesToStorage:
				return 1.0;
			default:
				return -1.0;
			}
		}
	}

	public CorridorWorkerModel(CorridorModel corridor, ICorridorData data, ICorridorWorkerData workerData)
	{
		Corridor = corridor;
		CapacityStat = new CorridorWorkerCapacityStatValue(Corridor, this, data, workerData);
		GainPerSecondStat = new CorridorWorkerGainPerSecondStatValue(Corridor, this, data, workerData);
		SecondsOneTurnStat = new CorridorWorkerSecondsOneTurnStatValue(Corridor, this, data, workerData);
		SecondsOneWayStat = new CorridorWorkerSecondsOneWayStatValue(Corridor, this, data, workerData);
		SecondsTillCapacityReachedStat = new CorridorWorkerSecondsTillCapacityReachedStatValue(Corridor, this, data, workerData);
		WalkingSpeedPerSecondStat = new CorridorWorkerWalkingSpeedStatValue(Corridor, this, data, workerData);
	}
}
