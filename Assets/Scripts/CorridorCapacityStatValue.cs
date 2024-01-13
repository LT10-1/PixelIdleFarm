using Entities.Corridor;

public class CorridorCapacityStatValue : AbstractCorridorStatValue<double>
{
	public override double Value => base.Worker.CapacityStat.Value;

	public override double NextValue => base.Worker.CapacityStat.NextValue;

	public override double ValueWithoutBonus => base.Worker.CapacityStat.ValueWithoutBonus;

	public override double MaxValue => base.Worker.CapacityStat.MaxValue;

	public override bool IsMaxValue => base.Worker.CapacityStat.IsMaxValue;

	public override bool HasBonusValue => base.Worker.CapacityStat.HasBonusValue;

	public override double BonusValue => base.Worker.CapacityStat.BonusValue;

	public override double NextBonusValue => base.Worker.CapacityStat.NextBonusValue;

	public CorridorCapacityStatValue(CorridorModel corridorModel, ICorridorData data, ICorridorWorkerData workerData)
		: base(corridorModel, data, workerData)
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
