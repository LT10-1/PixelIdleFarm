public class GroundTransportingPerSecondStatValue : AbstractGroundStatValue<double>
{
	public override double Value => CalcValue(GroundModel.NumberOfWorkersStat.Value, base.Worker.TransportingPerSecondStat.Value);

	public override double NextValue => ValueAtNextLevel(1);

	public override double ValueWithoutBonus => CalcValue(GroundModel.NumberOfWorkersStat.ValueWithoutBonus, base.Worker.TransportingPerSecondStat.ValueWithoutBonus);

	public override double MaxValue => ValueAtNextLevel(GroundData.MaxGroundLevel() - Level);

	public override bool IsMaxValue => GroundData.MaxGroundLevel() == Level;

	public override bool HasBonusValue => base.Worker.TransportingPerSecondStat.HasBonusValue || GroundModel.NumberOfWorkersStat.HasBonusValue;

	public override double BonusValue => Value - ValueWithoutBonus;

	public override double NextBonusValue => 0.0;

	public GroundTransportingPerSecondStatValue(GroundModel groundModel, IGroundData groundData, IGroundWorkerData groundWorkerData)
		: base(groundModel, groundData, groundWorkerData)
	{
	}

	public override double ValueAtNextLevel(int levelOffset)
	{
		if (Level + levelOffset > GroundData.MaxGroundLevel())
		{
			return -1.0;
		}
		return CalcValue(GroundModel.NumberOfWorkersStat.ValueAtNextLevel(levelOffset), base.Worker.TransportingPerSecondStat.ValueAtNextLevel(levelOffset));
	}

	public override double BonusValueAtNextLevel(int levelOffset)
	{
		return 0.0;
	}

	public static double CalcValue(int numberOfWorkers, double transportingPerSecond)
	{
		return (double)numberOfWorkers * transportingPerSecond;
	}
}
