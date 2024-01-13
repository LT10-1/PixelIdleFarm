using System;

public class ElevatorUpgradeCostStatValue : AbstractElevatorStatValue<double>
{
	public override double Value => ValueWithoutBonus - BonusValue;

	public override double NextValue => ValueAtNextLevel(1);

	public override double ValueWithoutBonus => ElevatorData.Cost(Level);

	public override double MaxValue => ValueAtNextLevel(ElevatorData.MaxElevatorLevel() - Level);

	public override bool IsMaxValue => ElevatorData.MaxElevatorLevel() == Level;

	public override bool HasBonusValue => Math.Abs(base.BonusContainer.UpgradeCostFactor - 1.0) > double.Epsilon;

	public override double BonusValue => base.BonusContainer.GetTotalBonusUpgradeCost(ElevatorData.Cost(Level));

	public override double NextBonusValue
	{
		get
		{
			if (Level + 1 > ElevatorData.MaxElevatorLevel())
			{
				return -1.0;
			}
			return base.BonusContainer.GetTotalBonusUpgradeCost(ElevatorData.Cost(Level + 1));
		}
	}

	public ElevatorUpgradeCostStatValue(ElevatorModel elevatorModel, IElevatorData elevatorData)
		: base(elevatorModel, elevatorData)
	{
	}

	public override double ValueAtNextLevel(int levelOffset)
	{
		if (Level + levelOffset > ElevatorData.MaxElevatorLevel())
		{
			return -1.0;
		}
		double num = 0.0;
		for (int i = 0; i < levelOffset; i++)
		{
			num += ElevatorData.Cost(Level + i + 1);
		}
		double upgradeCostFactor = base.BonusContainer.UpgradeCostFactor;
		upgradeCostFactor *= ((!(base.BonusContainer.UpgradeCostFactor >= 1.0)) ? ElevatorManagerSkillEffects.UpgradeCostSkillFactor : 1.0);
		return num * upgradeCostFactor;
	}

	public override double BonusValueAtNextLevel(int levelOffset)
	{
		return 0.0;
	}
}
