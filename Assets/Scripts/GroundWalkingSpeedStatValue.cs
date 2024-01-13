public class GroundWalkingSpeedStatValue : AbstractGroundStatValue<double>
{
	public override double Value => base.Worker.WalkingSpeedPerSecondStat.Value;

	public override double NextValue => base.Worker.WalkingSpeedPerSecondStat.NextValue;

	public override double ValueWithoutBonus => base.Worker.WalkingSpeedPerSecondStat.ValueWithoutBonus;

	public override double MaxValue => base.Worker.WalkingSpeedPerSecondStat.MaxValue;

	public override bool IsMaxValue => base.Worker.WalkingSpeedPerSecondStat.IsMaxValue;

	public override bool HasBonusValue => base.Worker.WalkingSpeedPerSecondStat.HasBonusValue;

	public override double BonusValue => base.Worker.WalkingSpeedPerSecondStat.BonusValue;

	public override double NextBonusValue => base.Worker.WalkingSpeedPerSecondStat.NextBonusValue;

	public GroundWalkingSpeedStatValue(GroundModel groundModel, IGroundData groundData, IGroundWorkerData groundWorkerData)
		: base(groundModel, groundData, groundWorkerData)
	{
	}

	public override double ValueAtNextLevel(int levelOffset)
	{
		return base.Worker.WalkingSpeedPerSecondStat.ValueAtNextLevel(levelOffset);
	}

	public override double BonusValueAtNextLevel(int levelOffset)
	{
		return base.Worker.WalkingSpeedPerSecondStat.BonusValueAtNextLevel(levelOffset);
	}
}
