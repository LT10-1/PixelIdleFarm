public class GroundCapacityStatValue : AbstractGroundStatValue<double>
{
	public override double Value => base.Worker.CapacityStat.Value;

	public override double NextValue => base.Worker.CapacityStat.NextValue;

	public override double ValueWithoutBonus => base.Worker.CapacityStat.ValueWithoutBonus;

	public override double MaxValue => base.Worker.CapacityStat.MaxValue;

	public override bool IsMaxValue => base.Worker.CapacityStat.IsMaxValue;

	public override bool HasBonusValue => base.Worker.CapacityStat.HasBonusValue;

	public override double BonusValue => base.Worker.CapacityStat.BonusValue;

	public override double NextBonusValue => base.Worker.CapacityStat.NextBonusValue;

	public GroundCapacityStatValue(GroundModel groundModel, IGroundData groundData, IGroundWorkerData groundWorkerData)
		: base(groundModel, groundData, groundWorkerData)
	{
	}

	public override double ValueAtNextLevel(int levelOffset)
	{
		return base.Worker.CapacityStat.ValueAtNextLevel(levelOffset);
	}

	public override double BonusValueAtNextLevel(int levelOffset)
	{
		return base.Worker.CapacityStat.BonusValueAtNextLevel(levelOffset);
	}
}
