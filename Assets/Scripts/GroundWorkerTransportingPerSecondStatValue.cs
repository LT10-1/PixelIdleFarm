public class GroundWorkerTransportingPerSecondStatValue : AbstractGroundWorkerStatValue<double>
{
	public override double Value => CalcValue(GroundWorkerModel.SecondsOneWayStat.Value, GroundWorkerModel.CapacityStat.Value, GroundWorkerModel.LoadingPerSecondStat.Value);

	public override double NextValue => ValueAtNextLevel(1);

	public override double ValueWithoutBonus => CalcValue(GroundWorkerModel.SecondsOneWayStat.ValueWithoutBonus, GroundWorkerModel.CapacityStat.ValueWithoutBonus, GroundWorkerModel.LoadingPerSecondStat.ValueWithoutBonus);

	public override double MaxValue => ValueAtNextLevel(GroundData.MaxGroundLevel() - Level);

	public override bool IsMaxValue => Level == GroundData.MaxGroundLevel();

	public override bool HasBonusValue => GroundWorkerModel.CapacityStat.HasBonusValue || GroundWorkerModel.WalkingSpeedPerSecondStat.HasBonusValue || GroundWorkerModel.LoadingPerSecondStat.HasBonusValue;

	public override double BonusValue => Value - ValueWithoutBonus;

	public override double NextBonusValue => 0.0;

	public GroundWorkerTransportingPerSecondStatValue(GroundModel groundModel, GroundWorkerModel groundWorkerModel, IGroundData groundData, IGroundWorkerData groundWorkerData)
		: base(groundModel, groundWorkerModel, groundData, groundWorkerData)
	{
	}

	public override double ValueAtNextLevel(int levelOffset)
	{
		if (Level + levelOffset > GroundData.MaxGroundLevel())
		{
			return -1.0;
		}
		return CalcValue(GroundWorkerModel.SecondsOneWayStat.ValueAtNextLevel(levelOffset), GroundWorkerModel.CapacityStat.ValueAtNextLevel(levelOffset), GroundWorkerModel.LoadingPerSecondStat.ValueAtNextLevel(levelOffset));
	}

	public override double BonusValueAtNextLevel(int levelOffset)
	{
		return 0.0;
	}

	public static double CalcValue(double secondsOneWay, double capacity, double loadingPerSecond)
	{
		double num = 2.0 * secondsOneWay;
		double num2 = 2.0 * capacity / loadingPerSecond;
		double num3 = num + num2;
		return capacity / num3;
	}
}
