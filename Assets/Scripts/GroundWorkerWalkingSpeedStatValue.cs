using System;

public class GroundWorkerWalkingSpeedStatValue : AbstractGroundWorkerStatValue<double>
{
	public override double Value => ValueWithoutBonus + BonusValue + ManagerSkillEffectBonusValue;

	public double ManagerSkillEffectBonusValue => (!(BonusValue <= 0.0)) ? (ValueWithoutBonus * (GroundManagerSkillEffects.WalkingSpeedSkillFactor - 1.0)) : 0.0;

	public override double NextValue => ValueAtNextLevel(1);

	public override double MaxValue => ValueAtNextLevel(GroundData.MaxGroundLevel() - Level);

	public override double ValueWithoutBonus => WorkerData.WalkingSpeedPerSecond(Level);

	public override bool IsMaxValue => Level == GroundData.MaxGroundLevel();

	public override bool HasBonusValue => Math.Abs(base.BonusContainer.WalkingSpeedFactor - 1.0) > double.Epsilon;

	public override double BonusValue => base.BonusContainer.GetTotalBonusWalkingSpeed(WorkerData.WalkingSpeedPerSecond(Level));

	public override double NextBonusValue
	{
		get
		{
			if (Level + 1 > GroundData.MaxGroundLevel())
			{
				return -1.0;
			}
			return base.BonusContainer.GetTotalBonusWalkingSpeed(WorkerData.WalkingSpeedPerSecond(Level + 1));
		}
	}

	public GroundWorkerWalkingSpeedStatValue(GroundModel groundModel, GroundWorkerModel groundWorkerModel, IGroundData groundData, IGroundWorkerData groundWorkerData)
		: base(groundModel, groundWorkerModel, groundData, groundWorkerData)
	{
	}

	public override double ValueAtNextLevel(int levelOffset)
	{
		if (Level + levelOffset > GroundData.MaxGroundLevel())
		{
			return -1.0;
		}
		return (double)WorkerData.WalkingSpeedPerSecond(Level + levelOffset) + ManagerSkillEffectNextBonusValue(levelOffset) + BonusValueAtNextLevel(levelOffset);
	}

	private double ManagerSkillEffectNextBonusValue(int levelOffset)
	{
		if (Level + levelOffset > GroundData.MaxGroundLevel())
		{
			return -1.0;
		}
		return (!(BonusValueAtNextLevel(levelOffset) <= 0.0)) ? ((double)WorkerData.WalkingSpeedPerSecond(Level + levelOffset) * (GroundManagerSkillEffects.WalkingSpeedSkillFactor - 1.0)) : 0.0;
	}

	public override double BonusValueAtNextLevel(int levelOffset)
	{
		if (Level + levelOffset > GroundData.MaxGroundLevel())
		{
			return -1.0;
		}
		return base.BonusContainer.GetTotalBonusWalkingSpeed(WorkerData.WalkingSpeedPerSecond(Level + levelOffset));
	}
}
