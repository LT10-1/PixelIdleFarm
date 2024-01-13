public class CorridorWorkerSecondsOneWayStatValue : AbstractCorridorWorkerStatValue<double>
{
	public override double Value => CalcValue(CorridorModel.GetFullLengthMined(), CorridorWorkerModel.WalkingSpeedPerSecondStat.Value);

	public override double NextValue => ValueAtNextLevel(1);

	public override double ValueWithoutBonus => CalcValue(CorridorModel.GetFullLengthMined(), CorridorWorkerModel.WalkingSpeedPerSecondStat.ValueWithoutBonus);

	public override double MaxValue => ValueAtNextLevel(Data.MaxCorridorLevel() - Level);

	public override bool IsMaxValue => Level == Data.MaxCorridorLevel();

	public override bool HasBonusValue => CorridorWorkerModel.WalkingSpeedPerSecondStat.HasBonusValue;

	public override double BonusValue => Value - ValueWithoutBonus;

	public override double NextBonusValue => 0.0;

	public CorridorWorkerSecondsOneWayStatValue(CorridorModel corridorModel, CorridorWorkerModel corridorWorkerModel, ICorridorData data, ICorridorWorkerData workerData)
		: base(corridorModel, corridorWorkerModel, data, workerData)
	{
	}

	public override double ValueAtNextLevel(int levelOffset)
	{
		if (Level + levelOffset > Data.MaxCorridorLevel())
		{
			return -1.0;
		}
		return CalcValue(CorridorModel.GetFullLengthMinedAtNextLevel(levelOffset), CorridorWorkerModel.WalkingSpeedPerSecondStat.ValueAtNextLevel(levelOffset));
	}

	public override double BonusValueAtNextLevel(int levelOffset)
	{
		return 0.0;
	}

	public static double CalcValue(double corridorLength, double walkingSpeedPerSecond)
	{
		return corridorLength / walkingSpeedPerSecond;
	}
}
