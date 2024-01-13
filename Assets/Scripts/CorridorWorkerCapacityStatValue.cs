using System;

public class CorridorWorkerCapacityStatValue : AbstractCorridorWorkerStatValue<double>
{
	public override double Value => ValueWithoutBonus + BonusValue + ManagerSkillEffectBonusValue;

	public double ManagerSkillEffectBonusValue => (!(BonusValue <= 0.0)) ? (ValueWithoutBonus * (CorridorManagerSkillEffects.CorridorWorkerCapacitySkillFactor - 1.0)) : 0.0;

	public override double NextValue => ValueAtNextLevel(1);

	public override double ValueWithoutBonus => WorkerData.Capacity(Level, base.Tier, CorridorModel.StatsIncreaseModel);

	public override double MaxValue => ValueAtNextLevel(Data.MaxCorridorLevel() - Level);

	public override bool IsMaxValue => Level == Data.MaxCorridorLevel();

	public override bool HasBonusValue => Math.Abs(base.BonusContainer.CapacityFactor - 1.0) > double.Epsilon;

	public override double BonusValue => base.BonusContainer.GetTotalBonusCapacity(WorkerData.Capacity(Level, base.Tier, CorridorModel.StatsIncreaseModel));

	public override double NextBonusValue
	{
		get
		{
			if (Level + 1 > Data.MaxCorridorLevel())
			{
				return -1.0;
			}
			return base.BonusContainer.GetTotalBonusCapacity(WorkerData.Capacity(Level + 1, base.Tier, CorridorModel.StatsIncreaseModel));
		}
	}

	public CorridorWorkerCapacityStatValue(CorridorModel corridorModel, CorridorWorkerModel corridorWorkerModel, ICorridorData data, ICorridorWorkerData workerData)
		: base(corridorModel, corridorWorkerModel, data, workerData)
	{
	}

	public override double ValueAtNextLevel(int levelOffset)
	{
		if (Level + levelOffset > Data.MaxCorridorLevel())
		{
			return -1.0;
		}
		return WorkerData.Capacity(Level + levelOffset, base.Tier, CorridorModel.StatsIncreaseModel) + ManagerSkillEffectNextBonusValue(levelOffset) + BonusValueAtNextLevel(levelOffset);
	}

	private double ManagerSkillEffectNextBonusValue(int levelOffset)
	{
		if (Level + levelOffset > Data.MaxCorridorLevel())
		{
			return -1.0;
		}
		return (!(BonusValueAtNextLevel(levelOffset) <= 0.0)) ? (WorkerData.Capacity(Level + levelOffset, base.Tier, CorridorModel.StatsIncreaseModel) * (CorridorManagerSkillEffects.CorridorWorkerCapacitySkillFactor - 1.0)) : 0.0;
	}

	public override double BonusValueAtNextLevel(int levelOffset)
	{
		if (Level + levelOffset > Data.MaxCorridorLevel())
		{
			return -1.0;
		}
		return base.BonusContainer.GetTotalBonusCapacity(WorkerData.Capacity(Level + levelOffset, base.Tier, CorridorModel.StatsIncreaseModel));
	}
}
