using System;
using UnityEngine;

public class DialogDailyReward : BaseDialog
{
	public UIButtonController ButtonCollect;

	public UIButtonController ButtonTest;

	[HideInInspector]
	public DialogDailyRewardItem[] DailyRewardItems;

	public override void Awake()
	{
		base.Awake();
		DailyRewardItems = (GetComponentsInChildren<DialogDailyRewardItem>(includeInactive: true) ?? new DialogDailyRewardItem[0]);
		ButtonCollect.OnClickCallback = OnClickCollect;
		ButtonTest.OnClickCallback = OnClickTest;
	}

	public override void Start()
	{
		base.Start();
		base.BackgroundDialog.SetTitle("Daily Reward");
		CreateData();
		UpdateData();
	}

	public override void Update()
	{
		base.Update();
		ButtonCollect.SetEnable(base.DailyRewardAvailable);
	}

	public override void OnShow()
	{
		base.OnShow();
		UpdateData();
		ButtonCollect.SetEnable(base.DailyRewardAvailable);
	}

	public void OnClickTest()
	{
		DataManager.Instance.SavegameData.DailyRewardLastTime -= 864000000000L;
	}

	public void CreateData()
	{
		for (int i = 0; i < Math.Min(DailyRewardItems.Length, DataManager.Instance.DailyRewardList.Count); i++)
		{
			ItemsEntity.Param param = DataManager.Instance.DailyRewardList[i];
			DialogDailyRewardItem dialogDailyRewardItem = DailyRewardItems[i];
			dialogDailyRewardItem.IsBigReward = (i % 7 == 6);
			switch (param.ItemType)
			{
			case 0:
				dialogDailyRewardItem.RewardCoin.SetMoney(param.SuperCashAmount, minify: true, showMoney: true, string.Empty);
				dialogDailyRewardItem.IconReward.transform.localScale = Vector3.one;
				break;
			case 1:
				dialogDailyRewardItem.IconReward.transform.localScale = Vector3.one * 0.5f;
				dialogDailyRewardItem.SetTypeBoostMultiple((ItemBoostMultiple)param.CompleteIncomeIncreaseFactor, (ItemBoostDuration)param.ActiveTimeSeconds);
				break;
			case 4:
				dialogDailyRewardItem.RewardCoin.SetCoinType(GetSkillCoinType((ContinentType)param.SkillPathID));
				dialogDailyRewardItem.RewardCoin.SetMoney(param.SkillPointAmount, minify: true, showMoney: true, string.Empty);
				break;
			case 5:
				dialogDailyRewardItem.RewardCoin.SetCoinType(GetCoinType((ChestType)param.ChestType));
				dialogDailyRewardItem.RewardCoin.SetMoney(param.ChestNumber, minify: true, showMoney: true, string.Empty);
				break;
			}
			dialogDailyRewardItem.TextCurrentDay.text = "Day " + (i + 1);
		}
	}

	public void OnClickCollect()
	{
		OnHide();
		//BaseController.GameController.AnalyticController.LogEvent("daily_reward_receive", "cycleCount", DataManager.Instance.SavegameData.DailyRewardCycleCount + 1);
		ItemsEntity.Param param = DataManager.Instance.DailyRewardList[DataManager.Instance.SavegameData.DailyRewardCycleCount];
		DataManager.Instance.SavegameData.DailyRewardCycleCount = (DataManager.Instance.SavegameData.DailyRewardCycleCount + 1) % DataManager.Instance.DailyRewardList.Count;
		DataManager.Instance.SavegameData.DailyRewardLastTime = DateTime.Now.Ticks;
		switch (param.ItemType)
		{
		case 2:
		case 3:
			break;
		case 0:
		{
			double superCashAmount = param.SuperCashAmount;
			AddSuperCash(superCashAmount);
			CreateReceiveEffect(param.ItemDescription, param);
			break;
		}
		case 1:
			AddItem(param.ItemID);
			break;
		case 5:
			AddChest((ChestType)param.ChestType, param.ChestNumber);
			break;
		case 4:
			AddSkillPoint((int)param.SkillPointAmount, (ContinentType)param.SkillPathID);
			break;
		}
	}

	public void UpdateData()
	{
		for (int i = 0; i < DailyRewardItems.Length; i++)
		{
			DialogDailyRewardItem dialogDailyRewardItem = DailyRewardItems[i];
			dialogDailyRewardItem.SetComplete(i < DataManager.Instance.SavegameData.DailyRewardCycleCount);
			dialogDailyRewardItem.SetActiveDay(i == DataManager.Instance.SavegameData.DailyRewardCycleCount);
		}
	}
}
