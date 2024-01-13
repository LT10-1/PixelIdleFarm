public class GroundLoadingPerSecondStatValue : AbstractGroundStatValue<double>
{
	public override double Value => base.Worker.LoadingPerSecondStat.Value;

	public override double NextValue => base.Worker.LoadingPerSecondStat.NextValue;

	public override double ValueWithoutBonus => base.Worker.LoadingPerSecondStat.ValueWithoutBonus;

	public override double MaxValue => base.Worker.LoadingPerSecondStat.MaxValue;

	public override bool IsMaxValue => base.Worker.LoadingPerSecondStat.IsMaxValue;

	public override bool HasBonusValue => base.Worker.LoadingPerSecondStat.HasBonusValue;

	public override double BonusValue => base.Worker.LoadingPerSecondStat.BonusValue;

	public override double NextBonusValue => base.Worker.LoadingPerSecondStat.NextBonusValue;

	public GroundLoadingPerSecondStatValue(GroundModel groundModel, IGroundData groundData, IGroundWorkerData groundWorkerData)
		: base(groundModel, groundData, groundWorkerData)
	{
	}

	public override double ValueAtNextLevel(int levelOffset)
	{
		return base.Worker.LoadingPerSecondStat.ValueAtNextLevel(levelOffset);
	}

	public override double BonusValueAtNextLevel(int levelOffset)
	{
		return base.Worker.LoadingPerSecondStat.BonusValueAtNextLevel(levelOffset);
	}
}
