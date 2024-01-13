using System;

public class CorridorWorkerGainPerSecondStatValue : AbstractCorridorWorkerStatValue<double>
{
	public override double Value => ValueWithoutBonus + BonusValue + ManagerSkillEffectBonusValue;

	public double ManagerSkillEffectBonusValue => (!(BonusValue <= 0.0)) ? (ValueWithoutBonus * (CorridorManagerSkillEffects.GainPerSecondSkillFactor - 1.0)) : 0.0;

	public override double NextValue => ValueAtNextLevel(1);

	public override double ValueWithoutBonus => WorkerData.GainPerSecond(Level, base.Tier, CorridorModel.StatsIncreaseModel);

	public override double MaxValue => ValueAtNextLevel(Data.MaxCorridorLevel() - Level);

	public override bool IsMaxValue => Level == Data.MaxCorridorLevel();

	public override bool HasBonusValue => Math.Abs(base.BonusContainer.GainPerSecondFactor - 1.0) > double.Epsilon;

	public override double BonusValue => base.BonusContainer.GetTotalBonusGainPerSecond(WorkerData.GainPerSecond(Level, base.Tier, CorridorModel.StatsIncreaseModel));

	public override double NextBonusValue
	{
		get
		{
			if (Level + 1 > Data.MaxCorridorLevel())
			{
				return -1.0;
			}
			return base.BonusContainer.GetTotalBonusGainPerSecond(WorkerData.GainPerSecond(Level + 1, base.Tier, CorridorModel.StatsIncreaseModel));
		}
	}

	public CorridorWorkerGainPerSecondStatValue(CorridorModel corridorModel, CorridorWorkerModel corridorWorkerModel, ICorridorData data, ICorridorWorkerData workerData)
		: base(corridorModel, corridorWorkerModel, data, workerData)
	{
	}

	public override double ValueAtNextLevel(int levelOffset)
	{
		if (Level + levelOffset > Data.MaxCorridorLevel())
		{
			return -1.0;
		}
		return WorkerData.GainPerSecond(Level + levelOffset, base.Tier, CorridorModel.StatsIncreaseModel) + ManagerSkillEffectNextBonusValue(levelOffset) + BonusValueAtNextLevel(levelOffset);
	}

	private double ManagerSkillEffectNextBonusValue(int levelOffset)
	{
		if (Level + levelOffset > Data.MaxCorridorLevel())
		{
			return -1.0;
		}
		return (!(BonusValueAtNextLevel(levelOffset) <= 0.0)) ? (WorkerData.GainPerSecond(Level + levelOffset, base.Tier, CorridorModel.StatsIncreaseModel) * (CorridorManagerSkillEffects.GainPerSecondSkillFactor - 1.0)) : 0.0;
	}

	public override double BonusValueAtNextLevel(int levelOffset)
	{
		if (Level + levelOffset > Data.MaxCorridorLevel())
		{
			return -1.0;
		}
		return base.BonusContainer.GetTotalBonusGainPerSecond(WorkerData.GainPerSecond(Level + levelOffset, base.Tier, CorridorModel.StatsIncreaseModel));
	}
}
