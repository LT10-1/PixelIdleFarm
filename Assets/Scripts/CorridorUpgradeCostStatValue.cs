using Entities.Corridor;
using System;

public class CorridorUpgradeCostStatValue : AbstractCorridorStatValue<double>
{
	public override double Value => ValueWithoutBonus - BonusValue - ManagerSkillEffectBonusValue;

	public double ManagerSkillEffectBonusValue => (!(BonusValue <= 0.0)) ? (ValueWithoutBonus * (CorridorManagerSkillEffects.UpgradeCostSkillFactor - 1.0)) : 0.0;

	public override double NextValue => ValueAtNextLevel(1);

	public override double ValueWithoutBonus => Data.Cost(Level, base.Tier);

	public override double MaxValue => ValueAtNextLevel(Data.MaxCorridorLevel() - Level);

	public override bool IsMaxValue => Data.MaxCorridorLevel() == Level;

	public override bool HasBonusValue => Math.Abs(base.BonusContainer.UpgradeCostFactor - 1.0) > double.Epsilon;

	public override double BonusValue => base.BonusContainer.GetTotalBonusUpgradeCost(Data.Cost(Level, base.Tier));

	public override double NextBonusValue
	{
		get
		{
			if (Level + 1 > Data.MaxCorridorLevel())
			{
				return -1.0;
			}
			return base.BonusContainer.GetTotalBonusUpgradeCost(Data.Cost(Level + 1, base.Tier));
		}
	}

	public CorridorUpgradeCostStatValue(CorridorModel corridorModel, ICorridorData data, ICorridorWorkerData workerData)
		: base(corridorModel, data, workerData)
	{
	}

	public override double ValueAtNextLevel(int levelOffset)
	{
		if (Level + levelOffset > Data.MaxCorridorLevel())
		{
			return -1.0;
		}
		double num = 0.0;
		for (int i = 0; i < levelOffset; i++)
		{
			num += Data.Cost(Level + i + 1, base.Tier);
		}
		double upgradeCostFactor = base.BonusContainer.UpgradeCostFactor;
		upgradeCostFactor *= ((!(base.BonusContainer.UpgradeCostFactor >= 1.0)) ? CorridorManagerSkillEffects.UpgradeCostSkillFactor : 1.0);
		return num * upgradeCostFactor;
	}

	public override double BonusValueAtNextLevel(int levelOffset)
	{
		return 0.0;
	}
}
