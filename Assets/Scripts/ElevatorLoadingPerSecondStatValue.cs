using System;

public class ElevatorLoadingPerSecondStatValue : AbstractElevatorStatValue<double>
{
	private readonly IStatsIncreaseModel _statsIncreaseModel;

	public override double Value => ValueWithoutBonus + BonusValue + ManagerSkillEffectBonusValue;

	public double ManagerSkillEffectBonusValue => (!(BonusValue <= 0.0)) ? (ValueWithoutBonus * (ElevatorManagerSkillEffects.LoadingPerSecondSkillFactor - 1.0)) : 0.0;

	public override double NextValue => ValueAtNextLevel(1);

	public override double ValueWithoutBonus => ElevatorData.LoadingPerSecond(Level, _statsIncreaseModel);

	public override double MaxValue => ValueAtNextLevel(ElevatorData.MaxElevatorLevel() - Level);

	public override bool IsMaxValue => ElevatorData.MaxElevatorLevel() == Level;

	public override bool HasBonusValue => Math.Abs(base.BonusContainer.LoadingPerSecondFactor - 1.0) > double.Epsilon;

	public override double BonusValue => base.BonusContainer.GetTotalBonusLoadingPerSecond(ElevatorData.LoadingPerSecond(Level, _statsIncreaseModel));

	public override double NextBonusValue
	{
		get
		{
			if (Level + 1 > ElevatorData.MaxElevatorLevel())
			{
				return -1.0;
			}
			return base.BonusContainer.GetTotalBonusLoadingPerSecond(ElevatorData.LoadingPerSecond(Level + 1, _statsIncreaseModel));
		}
	}

	public ElevatorLoadingPerSecondStatValue(ElevatorModel elevatorModel, IStatsIncreaseModel statsIncreaseModel, IElevatorData elevatorData)
		: base(elevatorModel, elevatorData)
	{
		_statsIncreaseModel = statsIncreaseModel;
	}

	public override double ValueAtNextLevel(int levelOffset)
	{
		if (Level + levelOffset > ElevatorData.MaxElevatorLevel())
		{
			return -1.0;
		}
		return ElevatorData.LoadingPerSecond(Level + levelOffset, _statsIncreaseModel) + ManagerSkillEffectNextBonusValue(levelOffset) + BonusValueAtNextLevel(levelOffset);
	}

	private double ManagerSkillEffectNextBonusValue(int levelOffset)
	{
		if (Level + levelOffset > ElevatorData.MaxElevatorLevel())
		{
			return -1.0;
		}
		return (!(BonusValueAtNextLevel(levelOffset) <= 0.0)) ? (ElevatorData.LoadingPerSecond(Level + levelOffset, _statsIncreaseModel) * (ElevatorManagerSkillEffects.LoadingPerSecondSkillFactor - 1.0)) : 0.0;
	}

	public override double BonusValueAtNextLevel(int levelOffset)
	{
		if (Level + levelOffset > ElevatorData.MaxElevatorLevel())
		{
			return -1.0;
		}
		return base.BonusContainer.GetTotalBonusLoadingPerSecond(ElevatorData.LoadingPerSecond(Level + levelOffset, _statsIncreaseModel));
	}
}
