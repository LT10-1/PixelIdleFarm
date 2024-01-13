using System;
using System.Collections.Generic;
using UnityEngine;

public class MaxUpgradeHelper
{
	private readonly int _maxLevel;

	private readonly int _tier;

	public ISkilledBonusContainer BonusContainer;

	private List<double> _allCosts;

	private bool CostReductionManagerActivated => Math.Abs(BonusContainer.UpgradeCostFactor - 1.0) > double.Epsilon;

	public MaxUpgradeHelper(IUpgradable upgradeable, ISkilledBonusContainer bonusContainer, int tier = 0)
	{
		_maxLevel = upgradeable.MaxLevel;
		_tier = tier;
		BonusContainer = bonusContainer;
		InitAllCosts(upgradeable);
	}

	private void InitAllCosts(IUpgradable upgradeable)
	{
		double num = 0.0;
		_allCosts = new List<double>(_maxLevel);
		for (int i = 1; i <= _maxLevel; i++)
		{
			double num2 = upgradeable.Cost(i, _tier);
			num += num2;
			_allCosts.Add(num);
		}
	}

	public int GetMaxAffordableNumbersOfLevelsToUpgrade(int currentLevel, double globalDollar)
	{
		if (IsCheater(currentLevel))
		{
			return 0;
		}
		double upgradeCostFactor = CostReductionManagerActivated ? BonusContainer.TotalUpgradeCostFactor : BonusContainer.UpgradeCostFactor;
		double num = _allCosts[currentLevel - 1] * upgradeCostFactor;
		double globalDollarIncludingCurrentLevel = globalDollar + num;
		int num2 = _allCosts.FindLastIndex(delegate(double cost)
		{
			double num3 = cost * upgradeCostFactor;
			return num3 <= globalDollarIncludingCurrentLevel;
		}) + 1;
		int value = num2 - currentLevel;
		return Mathf.Clamp(value, 0, _maxLevel);
	}

	private bool IsCheater(int level)
	{
		return level > _allCosts.Count;
	}
}
