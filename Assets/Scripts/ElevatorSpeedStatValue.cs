using System;

public class ElevatorSpeedStatValue : AbstractElevatorStatValue<double>
{
	public override double Value => ValueWithoutBonus + BonusValue + ManagerSkillEffectBonusValue;

	public override double NextValue => ValueAtNextLevel(1);

	public double ManagerSkillEffectBonusValue => (!(BonusValue <= 0.0)) ? (ValueWithoutBonus * (ElevatorManagerSkillEffects.TiersPerSecondsSkillFactor - 1.0)) : 0.0;

	public override double ValueWithoutBonus => ElevatorData.SpeedInTiersPerSecond(Level);

	public override double MaxValue => ValueAtNextLevel(ElevatorData.MaxElevatorLevel() - Level);

	public override bool IsMaxValue => ElevatorData.MaxElevatorLevel() == Level;

	public override bool HasBonusValue => Math.Abs(base.BonusContainer.TiersPerSecondFactor - 1.0) > double.Epsilon;

	public override double BonusValue => base.BonusContainer.GetTotalTiersPerSecondBonus(ElevatorData.SpeedInTiersPerSecond(Level));

	public override double NextBonusValue
	{
		get
		{
			if (Level + 1 > ElevatorData.MaxElevatorLevel())
			{
				return -1.0;
			}
			return base.BonusContainer.GetTotalTiersPerSecondBonus(ElevatorData.SpeedInTiersPerSecond(Level + 1));
		}
	}

	public ElevatorSpeedStatValue(ElevatorModel elevatorModel, IElevatorData elevatorData)
		: base(elevatorModel, elevatorData)
	{
	}

	public override double ValueAtNextLevel(int levelOffset)
	{
		if (Level + levelOffset > ElevatorData.MaxElevatorLevel())
		{
			return -1.0;
		}
		return ElevatorData.SpeedInTiersPerSecond(Level + levelOffset) + BonusValueAtNextLevel(levelOffset) + ManagerSkillEffectNextBonusValue(levelOffset);
	}

	private double ManagerSkillEffectNextBonusValue(int levelOffset)
	{
		if (Level + levelOffset > ElevatorData.MaxElevatorLevel())
		{
			return -1.0;
		}
		return (!(BonusValueAtNextLevel(levelOffset) <= 0.0)) ? (ElevatorData.SpeedInTiersPerSecond(Level + levelOffset) * (ElevatorManagerSkillEffects.TiersPerSecondsSkillFactor - 1.0)) : 0.0;
	}

	public override double BonusValueAtNextLevel(int levelOffset)
	{
		if (Level + levelOffset > ElevatorData.MaxElevatorLevel())
		{
			return -1.0;
		}
		return base.BonusContainer.GetTotalTiersPerSecondBonus(ElevatorData.SpeedInTiersPerSecond(Level + levelOffset));
	}
}
