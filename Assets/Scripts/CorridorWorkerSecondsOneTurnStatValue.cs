public class CorridorWorkerSecondsOneTurnStatValue : AbstractCorridorWorkerStatValue<double>
{
	public override double Value => CalcValue(CorridorWorkerModel.SecondsOneWayStat.Value, CorridorWorkerModel.SecondsTillCapacityReachedStat.Value);

	public override double NextValue => ValueAtNextLevel(1);

	public override double ValueWithoutBonus => CalcValue(CorridorWorkerModel.SecondsOneWayStat.ValueWithoutBonus, CorridorWorkerModel.SecondsTillCapacityReachedStat.ValueWithoutBonus);

	public override double MaxValue => ValueAtNextLevel(Data.MaxCorridorLevel() - Level);

	public override bool IsMaxValue => Level == Data.MaxCorridorLevel();

	public override bool HasBonusValue => CorridorWorkerModel.SecondsOneWayStat.HasBonusValue || CorridorWorkerModel.SecondsTillCapacityReachedStat.HasBonusValue;

	public override double BonusValue => Value - ValueWithoutBonus;

	public override double NextBonusValue => 0.0;

	public CorridorWorkerSecondsOneTurnStatValue(CorridorModel corridorModel, CorridorWorkerModel corridorWorkerModel, ICorridorData data, ICorridorWorkerData workerData)
		: base(corridorModel, corridorWorkerModel, data, workerData)
	{
	}

	public override double ValueAtNextLevel(int levelOffset)
	{
		if (Level + levelOffset > Data.MaxCorridorLevel())
		{
			return -1.0;
		}
		return CalcValue(CorridorWorkerModel.SecondsOneWayStat.ValueAtNextLevel(levelOffset), CorridorWorkerModel.SecondsTillCapacityReachedStat.ValueAtNextLevel(levelOffset));
	}

	public override double BonusValueAtNextLevel(int levelOffset)
	{
		return 0.0;
	}

	public static double CalcValue(double secondsOneWay, double secondsTillCapacityReached)
	{
		return 2.0 * secondsOneWay + secondsTillCapacityReached;
	}
}
