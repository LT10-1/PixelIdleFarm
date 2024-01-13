public class CorridorWorkerSecondsTillCapacityReachedStatValue : AbstractCorridorWorkerStatValue<double>
{
	public override double Value => CalcValue(CorridorWorkerModel.CapacityStat.Value, CorridorWorkerModel.GainPerSecondStat.Value);

	public override double NextValue => ValueAtNextLevel(1);

	public override double ValueWithoutBonus => CalcValue(CorridorWorkerModel.CapacityStat.ValueWithoutBonus, CorridorWorkerModel.GainPerSecondStat.ValueWithoutBonus);

	public override double MaxValue => ValueAtNextLevel(Data.MaxCorridorLevel() - Level);

	public override bool IsMaxValue => Level == Data.MaxCorridorLevel();

	public override bool HasBonusValue => CorridorWorkerModel.CapacityStat.HasBonusValue || CorridorWorkerModel.GainPerSecondStat.HasBonusValue;

	public override double BonusValue => Value - ValueWithoutBonus;

	public override double NextBonusValue => 0.0;

	public CorridorWorkerSecondsTillCapacityReachedStatValue(CorridorModel corridorModel, CorridorWorkerModel corridorWorkerModel, ICorridorData data, ICorridorWorkerData workerData)
		: base(corridorModel, corridorWorkerModel, data, workerData)
	{
	}

	public override double ValueAtNextLevel(int levelOffset)
	{
		if (Level + levelOffset > Data.MaxCorridorLevel())
		{
			return -1.0;
		}
		return CalcValue(CorridorWorkerModel.CapacityStat.ValueAtNextLevel(levelOffset), CorridorWorkerModel.GainPerSecondStat.ValueAtNextLevel(levelOffset));
	}

	public override double BonusValueAtNextLevel(int levelOffset)
	{
		return 0.0;
	}

	public static double CalcValue(double capacity, double gainPerSecond)
	{
		return capacity / gainPerSecond;
	}
}
