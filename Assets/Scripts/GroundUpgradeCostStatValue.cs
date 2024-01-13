using System;

public class GroundUpgradeCostStatValue : AbstractGroundStatValue<double>
{
	public override double Value => ValueWithoutBonus - BonusValue;

	public override double NextValue => ValueAtNextLevel(1);

	public override double ValueWithoutBonus => GroundData.Cost(Level);

	public override double MaxValue => ValueAtNextLevel(GroundData.MaxGroundLevel() - Level);

	public override bool IsMaxValue => GroundData.MaxGroundLevel() == Level;

	public override bool HasBonusValue => Math.Abs(base.BonusContainer.UpgradeCostFactor - 1.0) > double.Epsilon;

	public override double BonusValue => base.BonusContainer.GetTotalBonusUpgradeCost(GroundData.Cost(Level));

	public override double NextBonusValue
	{
		get
		{
			if (Level + 1 > GroundData.MaxGroundLevel())
			{
				return -1.0;
			}
			return base.BonusContainer.GetTotalBonusUpgradeCost(GroundData.Cost(Level + 1));
		}
	}

	public GroundUpgradeCostStatValue(GroundModel groundModel, IGroundData groundData, IGroundWorkerData groundWorkerData)
		: base(groundModel, groundData, groundWorkerData)
	{
	}

	public override double ValueAtNextLevel(int levelOffset)
	{
		if (Level + levelOffset > GroundData.MaxGroundLevel())
		{
			return -1.0;
		}
		double num = 0.0;
		for (int i = 0; i < levelOffset; i++)
		{
			num += GroundData.Cost(Level + i + 1);
		}
		double upgradeCostFactor = base.BonusContainer.UpgradeCostFactor;
		upgradeCostFactor *= ((!(base.BonusContainer.UpgradeCostFactor >= 1.0)) ? GroundManagerSkillEffects.UpgradeCostSkillFactor : 1.0);
		return num * upgradeCostFactor;
	}

	public override double BonusValueAtNextLevel(int levelOffset)
	{
		return 0.0;
	}
}
