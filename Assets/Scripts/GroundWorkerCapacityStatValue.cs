using System;

public class GroundWorkerCapacityStatValue : AbstractGroundWorkerStatValue<double>
{
	private readonly IStatsIncreaseModel _statsIncreaseModel;

	public override double Value => ValueWithoutBonus + BonusValue + ManagerSkillEffectBonusValue;

	public double ManagerSkillEffectBonusValue => (!(BonusValue <= 0.0)) ? (ValueWithoutBonus * (GroundManagerSkillEffects.CapacitySkillFactor - 1.0)) : 0.0;

	public override double NextValue => ValueAtNextLevel(1);

	public override double ValueWithoutBonus => WorkerData.Capacity(Level, _statsIncreaseModel);

	public override double MaxValue => ValueAtNextLevel(GroundData.MaxGroundLevel() - Level);

	public override bool IsMaxValue => Level == GroundData.MaxGroundLevel();

	public override bool HasBonusValue => Math.Abs(base.BonusContainer.CapacityFactor - 1.0) > double.Epsilon;

	public override double BonusValue => base.BonusContainer.GetTotalBonusCapacity(WorkerData.Capacity(Level, _statsIncreaseModel));

	public override double NextBonusValue
	{
		get
		{
			if (Level + 1 > GroundData.MaxGroundLevel())
			{
				return -1.0;
			}
			return base.BonusContainer.GetTotalBonusCapacity(WorkerData.Capacity(Level + 1, _statsIncreaseModel));
		}
	}

	public GroundWorkerCapacityStatValue(GroundModel groundModel, GroundWorkerModel groundWorkerModel, IStatsIncreaseModel statsIncreaseModel, IGroundData groundData, IGroundWorkerData groundWorkerData)
		: base(groundModel, groundWorkerModel, groundData, groundWorkerData)
	{
		_statsIncreaseModel = statsIncreaseModel;
	}

	public override double ValueAtNextLevel(int levelOffset)
	{
		if (Level + levelOffset > GroundData.MaxGroundLevel())
		{
			return -1.0;
		}
		return WorkerData.Capacity(Level + levelOffset, _statsIncreaseModel) + ManagerSkillEffectNextBonusValue(levelOffset) + BonusValueAtNextLevel(levelOffset);
	}

	private double ManagerSkillEffectNextBonusValue(int levelOffset)
	{
		if (Level + levelOffset > GroundData.MaxGroundLevel())
		{
			return -1.0;
		}
		return (!(BonusValueAtNextLevel(levelOffset) <= 0.0)) ? (WorkerData.Capacity(Level + levelOffset, _statsIncreaseModel) * (GroundManagerSkillEffects.CapacitySkillFactor - 1.0)) : 0.0;
	}

	public override double BonusValueAtNextLevel(int levelOffset)
	{
		if (Level + levelOffset > GroundData.MaxGroundLevel())
		{
			return -1.0;
		}
		return base.BonusContainer.GetTotalBonusCapacity(WorkerData.Capacity(Level + levelOffset, _statsIncreaseModel));
	}
}
