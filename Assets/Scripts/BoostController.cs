using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoostController : BaseController
{
	[HideInInspector]
	public bool IsInAdBoostTime;

	[HideInInspector]
	public long CurrentAdRemainTime;

	[HideInInspector]
	public double CurrentItemBoostFactor = 1.0;

	[HideInInspector]
	public long CurrentItemRemainTime;

	[HideInInspector]
	public Dictionary<double, long> BoostItemRemainTime = new Dictionary<double, long>();

	public bool HaveBoost => MathUtils.CompareDoubleBiggerThanZero(TotalBoostFactor - 1.0);

	public double TotalBoostFactor => CurrentAdBoostFactor * CurrentItemBoostFactor * CurrentIAPItemBoostFactor;

	public long MinRemainTime
	{
		get
		{
			if (CurrentAdRemainTime == 0 && CurrentItemRemainTime == 0)
			{
				return 0L;
			}
			return Math.Min((CurrentAdRemainTime == 0) ? long.MaxValue : CurrentAdRemainTime, (CurrentItemRemainTime == 0) ? long.MaxValue : CurrentItemRemainTime);
		}
	}

	public double CurrentIAPItemBoostFactor => (!DataManager.Instance.SavegameData.IAPCheckHavePurchaseIncomeX2) ? 1 : 2;

	public double AdBoostx2SingleDuration => 14399.9991808 * (1.0 + BaseController.GameController.SkillController.AdSingleBoostx2Duration / 100.0);

	public double AdBoostx2MaxDuration => 115199.9934464 * (1.0 + BaseController.GameController.SkillController.AdMaxBoostx2Duration / 100.0);

	public double CurrentAdBoostFactor => (!IsInAdBoostTime) ? 1.0 : BaseController.GameController.SkillController.AdCurrentx2BoostFactor;

	public long MineBoostx2EndTime
	{
		get
		{
			return DataManager.Instance.CurrentMineSavegame.MineBoostx2EndTime;
		}
		set
		{
			DataManager.Instance.CurrentMineSavegame.MineBoostx2EndTime = value;
		}
	}

	public override void Start()
	{
		base.Start();
	}

	public override void Update()
	{
		base.Update();
		CalculateIdleCashBonus(checkExpireItemOnly: true);
		CalculateAdBoostFactor();
		CalculateItemBoostFactor();
	}

	private void CalculateAdBoostFactor()
	{
		if (MineBoostx2EndTime == 0)
		{
			CurrentAdRemainTime = 0L;
			IsInAdBoostTime = false;
			return;
		}
		CurrentAdRemainTime = MineBoostx2EndTime - DateTime.Now.Ticks;
		if (CurrentAdRemainTime > 0)
		{
			IsInAdBoostTime = true;
			return;
		}
		MineBoostx2EndTime = 0L;
		IsInAdBoostTime = false;
	}

	private void CalculateItemBoostFactor()
	{
		if (DataManager.Instance.SavegameData.BoostMultipleEndTime.Count == 0)
		{
			ResetParam();
			return;
		}
		BoostItemRemainTime = new Dictionary<double, long>();
		CurrentItemRemainTime = 0L;
		CurrentItemBoostFactor = 0.0;
		List<double> list = new List<double>(DataManager.Instance.SavegameData.BoostMultipleEndTime.Keys);
		foreach (double item in list)
		{
			long num = DataManager.Instance.SavegameData.BoostMultipleEndTime[item];
			long num2 = num - DateTime.Now.Ticks;
			if (num2 > 0)
			{
				BoostItemRemainTime[item] = num2;
				if (CurrentItemRemainTime == 0 || CurrentItemRemainTime > num2)
				{
					CurrentItemRemainTime = num2;
				}
				CurrentItemBoostFactor += item;
			}
			else
			{
				DataManager.Instance.SavegameData.BoostMultipleEndTime.Remove(item);
				DataManager.Instance.SavegameData.BoostMultipleStartTime.Remove(item);
			}
		}
		if (BoostItemRemainTime.Count == 0)
		{
			ResetParam();
		}
	}

	public void CalculateIdleCashBonus(bool checkExpireItemOnly)
	{
		double totalSeconds = TimeSpan.FromTicks(DateTime.Now.Ticks - DataManager.Instance.CurrentMineSavegame.MineLastTimeVisit).TotalSeconds;
		if (!(totalSeconds < 60.0))
		{
			long num = Math.Min(DateTime.Now.Ticks, MineBoostx2EndTime);
			IOrderedEnumerable<KeyValuePair<double, long>> orderedEnumerable = from pair in DataManager.Instance.SavegameData.BoostMultipleEndTime
				orderby pair.Key descending
				select pair;
			long num2 = 0L;
			foreach (KeyValuePair<double, long> item in orderedEnumerable)
			{
				long val = 0L;
				if (DataManager.Instance.SavegameData.BoostMultipleStartTime.ContainsKey(item.Key))
				{
					val = DataManager.Instance.SavegameData.BoostMultipleStartTime[item.Key];
				}
				long num3 = Math.Max(Math.Max(val, DataManager.Instance.CurrentMineSavegame.MineLastTimeVisit), num2);
				long num4 = num2 = Math.Min(item.Value, DateTime.Now.Ticks);
				double totalSeconds2 = TimeSpan.FromTicks(num4 - num3).TotalSeconds;
				double num5 = 0.0;
				if (num > num3)
				{
					num5 = ((num < num4) ? TimeSpan.FromTicks(num - num3).TotalSeconds : totalSeconds2);
				}
				double num6 = totalSeconds2 - num5;
				long num7 = item.Value - DateTime.Now.Ticks;
				if ((!checkExpireItemOnly || num7 <= 0) && totalSeconds2 > 0.0)
				{
					DataManager.Instance.CurrentMineSavegame.IdleBonusCashGain += (item.Key * 2.0 - 1.0) * num5 * CurrentIAPItemBoostFactor * DataManager.Instance.CurrentMineSavegame.IdleCash;
					DataManager.Instance.CurrentMineSavegame.IdleBonusCashGain += (item.Key - 1.0) * num6 * CurrentIAPItemBoostFactor * DataManager.Instance.CurrentMineSavegame.IdleCash;
				}
			}
			if ((!checkExpireItemOnly || TimeSpan.FromTicks(Math.Abs(num - DateTime.Now.Ticks)).TotalSeconds > 0.5) && num > num2)
			{
				double totalSeconds3 = TimeSpan.FromTicks(num - Math.Max(DataManager.Instance.CurrentMineSavegame.MineLastTimeVisit, num2)).TotalSeconds;
				MonoBehaviour.print("Remain X2: " + totalSeconds3);
				DataManager.Instance.CurrentMineSavegame.IdleBonusCashGain += totalSeconds3 * DataManager.Instance.CurrentMineSavegame.IdleCash;
			}
		}
	}

	private void ResetParam()
	{
		CurrentItemBoostFactor = 1.0;
		CurrentItemRemainTime = 0L;
	}

	public void MineAdBoostx2(int mineOrder)
	{
		MineSavegame mineSavegame = DataManager.Instance.SavegameData.Mines[mineOrder];
		if (mineSavegame.MineBoostx2EndTime == 0)
		{
			mineSavegame.MineBoostx2EndTime = DateTime.Now.Ticks;
		}
		mineSavegame.MineBoostx2EndTime = Math.Min(mineSavegame.MineBoostx2EndTime + (long)AdBoostx2SingleDuration * 10000000, DateTime.Now.Ticks + (long)AdBoostx2MaxDuration * 10000000);
	}
}
