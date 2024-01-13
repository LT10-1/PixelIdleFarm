public class GroundWorkerSecondsOneWayStatValue : AbstractGroundWorkerStatValue<double>
{
	public override double Value => CalcValue(GroundModel.GroundLength, GroundWorkerModel.WalkingSpeedPerSecondStat.Value);

	public override double NextValue => ValueAtNextLevel(1);

	public override double ValueWithoutBonus => CalcValue(GroundModel.GroundLength, GroundWorkerModel.WalkingSpeedPerSecondStat.ValueWithoutBonus);

	public override double MaxValue => ValueAtNextLevel(GroundData.MaxGroundLevel() - Level);

	public override bool IsMaxValue => Level == GroundData.MaxGroundLevel();

	public override bool HasBonusValue => GroundWorkerModel.WalkingSpeedPerSecondStat.HasBonusValue;

	public override double BonusValue => Value - ValueWithoutBonus;

	public override double NextBonusValue => 0.0;

	public GroundWorkerSecondsOneWayStatValue(GroundModel groundModel, GroundWorkerModel groundWorkerModel, IGroundData groundData, IGroundWorkerData groundWorkerData)
		: base(groundModel, groundWorkerModel, groundData, groundWorkerData)
	{
	}

	public override double ValueAtNextLevel(int levelOffset)
	{
		if (Level + levelOffset > GroundData.MaxGroundLevel())
		{
			return -1.0;
		}
		return CalcValue(GroundModel.GroundLength, GroundWorkerModel.WalkingSpeedPerSecondStat.ValueAtNextLevel(levelOffset));
	}

	public override double BonusValueAtNextLevel(int levelOffset)
	{
		return 0.0;
	}

	public static double CalcValue(double groundLength, double walkingSpeedPerSecond)
	{
		return groundLength / walkingSpeedPerSecond;
	}
}
