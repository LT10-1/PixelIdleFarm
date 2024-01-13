using System;

public class GroundWorkerLoadingPerSecondStatValue : AbstractGroundWorkerStatValue<double>
{
	private readonly IStatsIncreaseModel _statsIncreaseModel;

	public override double Value => ValueWithoutBonus + BonusValue + ManagerSkillEffectBonusValue;

	public double ManagerSkillEffectBonusValue => (!(BonusValue <= 0.0)) ? (ValueWithoutBonus * (GroundManagerSkillEffects.LoadingPerSecondSkillFactor - 1.0)) : 0.0;

	public override double NextValue => ValueAtNextLevel(1);

	public override double ValueWithoutBonus => WorkerData.LoadingPerSecond(Level, _statsIncreaseModel);

	public override double MaxValue => ValueAtNextLevel(GroundData.MaxGroundLevel() - Level);

	public override bool IsMaxValue => Level == GroundData.MaxGroundLevel();

	public override bool HasBonusValue => Math.Abs(base.BonusContainer.LoadingPerSecondFactor - 1.0) > double.Epsilon;

	public override double BonusValue => base.BonusContainer.GetTotalBonusLoadingPerSecond(WorkerData.LoadingPerSecond(Level, _statsIncreaseModel));

	public override double NextBonusValue
	{
		get
		{
			if (Level + 1 > GroundData.MaxGroundLevel())
			{
				return -1.0;
			}
			return base.BonusContainer.GetTotalBonusLoadingPerSecond(WorkerData.LoadingPerSecond(Level + 1, _statsIncreaseModel));
		}
	}

	public GroundWorkerLoadingPerSecondStatValue(GroundModel groundModel, GroundWorkerModel groundWorkerModel, IStatsIncreaseModel statsIncreaseModel, IGroundData groundData, IGroundWorkerData groundWorkerData)
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
		return WorkerData.LoadingPerSecond(Level + levelOffset, _statsIncreaseModel) + ManagerSkillEffectNextBonusValue(levelOffset) + BonusValueAtNextLevel(levelOffset);
	}

	private double ManagerSkillEffectNextBonusValue(int levelOffset)
	{
		if (Level + levelOffset > GroundData.MaxGroundLevel())
		{
			return -1.0;
		}
		return (!(BonusValueAtNextLevel(levelOffset) <= 0.0)) ? (WorkerData.LoadingPerSecond(Level + levelOffset, _statsIncreaseModel) * (GroundManagerSkillEffects.LoadingPerSecondSkillFactor - 1.0)) : 0.0;
	}

	public override double BonusValueAtNextLevel(int levelOffset)
	{
		if (Level + levelOffset > GroundData.MaxGroundLevel())
		{
			return -1.0;
		}
		return base.BonusContainer.GetTotalBonusLoadingPerSecond(WorkerData.LoadingPerSecond(Level + levelOffset, _statsIncreaseModel));
	}
}
