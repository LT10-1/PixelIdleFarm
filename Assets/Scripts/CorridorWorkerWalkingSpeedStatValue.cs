using System;

public class CorridorWorkerWalkingSpeedStatValue : AbstractCorridorWorkerStatValue<double>
{
	public override double Value => ValueWithoutBonus + BonusValue + ManagerSkillEffectBonusValue;

	public double ManagerSkillEffectBonusValue => (!(BonusValue <= 0.0)) ? (ValueWithoutBonus * (CorridorManagerSkillEffects.WalkingSpeedSkillFactor - 1.0)) : 0.0;

	public override double NextValue => ValueAtNextLevel(1);

	public override double ValueWithoutBonus => WorkerData.WalkingSpeedPerSecond(Level, base.Tier);

	public override double MaxValue => ValueAtNextLevel(Data.MaxCorridorLevel() - Level);

	public override bool IsMaxValue => Level == Data.MaxCorridorLevel();

	public override bool HasBonusValue => Math.Abs(base.BonusContainer.WalkingSpeedFactor - 1.0) > double.Epsilon;

	public override double BonusValue => base.BonusContainer.GetTotalBonusWalkingSpeed(WorkerData.WalkingSpeedPerSecond(Level, base.Tier));

	public override double NextBonusValue
	{
		get
		{
			if (Level + 1 > Data.MaxCorridorLevel())
			{
				return -1.0;
			}
			return base.BonusContainer.GetTotalBonusWalkingSpeed(WorkerData.WalkingSpeedPerSecond(Level + 1, base.Tier));
		}
	}

	public CorridorWorkerWalkingSpeedStatValue(CorridorModel corridorModel, CorridorWorkerModel corridorWorkerModel, ICorridorData data, ICorridorWorkerData workerData)
		: base(corridorModel, corridorWorkerModel, data, workerData)
	{
	}

	public override double ValueAtNextLevel(int levelOffset)
	{
		if (Level + levelOffset > Data.MaxCorridorLevel())
		{
			return -1.0;
		}
		return (double)WorkerData.WalkingSpeedPerSecond(Level + levelOffset, base.Tier) + ManagerSkillEffectNextBonusValue(levelOffset) + BonusValueAtNextLevel(levelOffset);
	}

	private double ManagerSkillEffectNextBonusValue(int levelOffset)
	{
		if (Level + levelOffset > Data.MaxCorridorLevel())
		{
			return -1.0;
		}
		return (!(BonusValueAtNextLevel(levelOffset) <= 0.0)) ? ((double)WorkerData.WalkingSpeedPerSecond(Level + levelOffset, base.Tier) * (CorridorManagerSkillEffects.WalkingSpeedSkillFactor - 1.0)) : 0.0;
	}

	public override double BonusValueAtNextLevel(int levelOffset)
	{
		if (Level + levelOffset > Data.MaxCorridorLevel())
		{
			return -1.0;
		}
		return base.BonusContainer.GetTotalBonusWalkingSpeed(WorkerData.WalkingSpeedPerSecond(Level + levelOffset, base.Tier));
	}
}
