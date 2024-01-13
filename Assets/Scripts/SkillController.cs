using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : BaseController
{
	[HideInInspector]
	public double RockUnlockCostReduction;

	[HideInInspector]
	public double RockUnlockDurationReduction;

	[HideInInspector]
	public double PrestigeCostReduction;

	[HideInInspector]
	public double Adx2IdleCashFactor;

	[HideInInspector]
	public double AdBoostx2Factor;

	[HideInInspector]
	public double AdSingleBoostx2Duration;

	[HideInInspector]
	public double AdMaxBoostx2Duration;

	[HideInInspector]
	public Dictionary<ManagerEffect, double> ManagerEffectBoost;

	[HideInInspector]
	public Dictionary<ManagerArea, double> ManagerEffectDurationBoost;

	[HideInInspector]
	public Dictionary<ManagerArea, double> ManagerCooldownBoost;

	[HideInInspector]
	public double ManagerCostBoost;

	public double AdCurrentx2IdleCashFactor => 2.0 * (1.0 + Adx2IdleCashFactor / 100.0);

	public double AdCurrentx2BoostFactor => 2.0 * (1.0 + AdBoostx2Factor / 100.0);

	public override void Awake()
	{
		base.Awake();
		OnSkillChanged();
	}

	public void OnSkillChanged()
	{
		BaseController.GameController.UpdateStatIncreaseModel();
		ResetParam();
		CalculateParam();
		SetManagerParams();
		if (BaseController.GameController.MineController != null && BaseController.GameController.MineController.MileStoneLockController.gameObject.activeSelf)
		{
			BaseController.GameController.MineController.MileStoneLockController.UpdateData();
		}
	}

	public void ResetParam()
	{
		RockUnlockCostReduction = 0.0;
		RockUnlockDurationReduction = 0.0;
		PrestigeCostReduction = 0.0;
		Adx2IdleCashFactor = 0.0;
		AdBoostx2Factor = 0.0;
		AdSingleBoostx2Duration = 0.0;
		AdMaxBoostx2Duration = 0.0;
		ManagerEffectBoost = new Dictionary<ManagerEffect, double>();
		IEnumerator enumerator = Enum.GetValues(typeof(ManagerEffect)).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				ManagerEffect key = (ManagerEffect)enumerator.Current;
				ManagerEffectBoost[key] = 0.0;
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		ManagerEffectDurationBoost = new Dictionary<ManagerArea, double>();
		IEnumerator enumerator2 = Enum.GetValues(typeof(ManagerArea)).GetEnumerator();
		try
		{
			while (enumerator2.MoveNext())
			{
				ManagerArea key2 = (ManagerArea)enumerator2.Current;
				ManagerEffectDurationBoost[key2] = 0.0;
			}
		}
		finally
		{
			IDisposable disposable2;
			if ((disposable2 = (enumerator2 as IDisposable)) != null)
			{
				disposable2.Dispose();
			}
		}
		ManagerCooldownBoost = new Dictionary<ManagerArea, double>();
		IEnumerator enumerator3 = Enum.GetValues(typeof(ManagerArea)).GetEnumerator();
		try
		{
			while (enumerator3.MoveNext())
			{
				ManagerArea key3 = (ManagerArea)enumerator3.Current;
				ManagerCooldownBoost[key3] = 0.0;
			}
		}
		finally
		{
			IDisposable disposable3;
			if ((disposable3 = (enumerator3 as IDisposable)) != null)
			{
				disposable3.Dispose();
			}
		}
		ManagerCostBoost = 0.0;
	}

	public void CalculateParam()
	{
		foreach (KeyValuePair<int, int> item in DataManager.Instance.SavegameData.SkillSaveGame)
		{
			SkillEntity.Param param = DataManager.Instance.SkillDictionary[item.Key];
			double num = param.ParamX[item.Value - 1];
			switch (param.EffectId)
			{
			case 3:
				RockUnlockCostReduction += num;
				break;
			case 4:
				RockUnlockDurationReduction += num;
				break;
			case 5:
				PrestigeCostReduction += num;
				break;
			case 6:
				Adx2IdleCashFactor += num;
				break;
			case 7:
				AdBoostx2Factor += num;
				break;
			case 8:
				AdSingleBoostx2Duration += num;
				break;
			case 9:
				AdMaxBoostx2Duration += num;
				break;
			case 10:
			{
				Dictionary<ManagerEffect, double> managerEffectBoost;
				ManagerEffect paramY3;
				(managerEffectBoost = ManagerEffectBoost)[paramY3 = (ManagerEffect)param.ParamY] = managerEffectBoost[paramY3] + num;
				break;
			}
			case 11:
			{
				Dictionary<ManagerArea, double> managerCooldownBoost;
				ManagerArea paramY2;
				(managerCooldownBoost = ManagerEffectDurationBoost)[paramY2 = (ManagerArea)param.ParamY] = managerCooldownBoost[paramY2] + num;
				break;
			}
			case 12:
			{
				Dictionary<ManagerArea, double> managerCooldownBoost;
				ManagerArea paramY;
				(managerCooldownBoost = ManagerCooldownBoost)[paramY = (ManagerArea)param.ParamY] = managerCooldownBoost[paramY] + num;
				break;
			}
			case 13:
				ManagerCostBoost += num;
				break;
			}
		}
	}

	public void SetManagerParams()
	{
		foreach (KeyValuePair<ManagerEffect, double> item in ManagerEffectBoost)
		{
			double num = 1.0 + item.Value / 100.0;
			double upgradeCostSkillFactor = 1.0 - item.Value / 100.0;
			switch (item.Key)
			{
			case ManagerEffect.GroundWalkingSpeedBoost:
				GroundManagerSkillEffects.WalkingSpeedSkillFactor = num;
				break;
			case ManagerEffect.GroundUpgradeCostReduction:
				GroundManagerSkillEffects.UpgradeCostSkillFactor = upgradeCostSkillFactor;
				break;
			case ManagerEffect.GroundLoadingSpeedPerSecondMultiplier:
				GroundManagerSkillEffects.LoadingPerSecondSkillFactor = num;
				break;
			case ManagerEffect.GroundWorkerCapacityMultiplier:
				GroundManagerSkillEffects.CapacitySkillFactor = num;
				break;
			case ManagerEffect.CorridorWorkerGainMultiplier:
				CorridorManagerSkillEffects.GainPerSecondSkillFactor = num;
				break;
			case ManagerEffect.CorridorWorkerWalkingSpeedMultiplier:
				CorridorManagerSkillEffects.WalkingSpeedSkillFactor = num;
				break;
			case ManagerEffect.CorridorUpgradeCostReduction:
				CorridorManagerSkillEffects.UpgradeCostSkillFactor = upgradeCostSkillFactor;
				break;
			case ManagerEffect.ElevatorTiersPerSecondsMultiplier:
				ElevatorManagerSkillEffects.TiersPerSecondsSkillFactor = num;
				break;
			case ManagerEffect.ElevatorCapacityMultiplier:
				ElevatorManagerSkillEffects.CapacitySkillFactor = num;
				break;
			case ManagerEffect.ElevatorLoadingSpeedPerSecondMultiplier:
				ElevatorManagerSkillEffects.LoadingPerSecondSkillFactor = num;
				break;
			case ManagerEffect.ElevatorUpgradeCostReduction:
				ElevatorManagerSkillEffects.UpgradeCostSkillFactor = upgradeCostSkillFactor;
				break;
			}
		}
	}
}
