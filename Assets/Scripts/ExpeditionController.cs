using System;
using System.Collections.Generic;
using UnityEngine;

public class ExpeditionController : BaseController
{
	[HideInInspector]
	public ExpeditionState ExpeditionState;

	[HideInInspector]
	public List<Action> OnExpeditionStateChange = new List<Action>();

	[HideInInspector]
	public List<Action<long>> OnBoostExpeditionTime = new List<Action<long>>();

	public ExpeditionSavegame CurrentExpedition
	{
		get
		{
			return DataManager.Instance.SavegameData.CurrentExpedition;
		}
		set
		{
			DataManager.Instance.SavegameData.CurrentExpedition = value;
		}
	}

	public bool ExpeditionRefreshAvailable => DataManager.Instance.SavegameData.ExpeditionRefreshTime == 0 || new DateTime(DataManager.Instance.SavegameData.ExpeditionRefreshTime).Date.Ticks != DateTime.Now.Date.Ticks;

	public bool ExpeditionBoostAvailable => DataManager.Instance.SavegameData.ExpeditionLastBoostTime == 0 || DataManager.Instance.SavegameData.ExpeditionLastBoostTime + 144000000000L < DateTime.Now.Ticks;

	public override void Start()
	{
		base.Start();
		if (DataManager.Instance.SavegameData.ExpeditionChooseList == null)
		{
			RefreshExpedition(createFirstTime: true);
		}
		LoadCurrentState();
	}

	public override void Update()
	{
		base.Update();
		switch (ExpeditionState)
		{
		case ExpeditionState.InProgress:
			if (CurrentExpedition.StartTime + CurrentExpedition.Duration * 10000000 < DateTime.Now.Ticks)
			{
				SetExpeditionState(ExpeditionState.Completed);
			}
			break;
		}
	}

	public void BoostExpeditionTime(long time)
	{
		if (ExpeditionState == ExpeditionState.InProgress && CurrentExpedition != null)
		{
			CurrentExpedition.StartTime -= time;
			DataManager.Instance.SavegameData.ExpeditionLastBoostTime = DateTime.Now.Ticks;
			InvokeBoostExpedition(time);
		}
	}

	public void SetExpeditionState(ExpeditionState state)
	{
		ExpeditionState = state;
		if (ExpeditionState == ExpeditionState.Idle)
		{
			CurrentExpedition = null;
		}
		InvokeExpeditionStateChange();
	}

	public void InvokeExpeditionStateChange()
	{
		OnExpeditionStateChange.ForEach(delegate(Action action)
		{
			action();
		});
	}

	public void InvokeBoostExpedition(long time)
	{
		OnBoostExpeditionTime.ForEach(delegate(Action<long> action)
		{
			action(time);
		});
	}

	public void LoadCurrentState()
	{
		if (CurrentExpedition == null || CurrentExpedition.Duration == 0 || CurrentExpedition.ItemID.Count == 0)
		{
			SetExpeditionState(ExpeditionState.Idle);
		}
		else if (CurrentExpedition.StartTime + CurrentExpedition.Duration * 10000000 >= DateTime.Now.Ticks)
		{
			SetExpeditionState(ExpeditionState.InProgress);
		}
		else
		{
			SetExpeditionState(ExpeditionState.Completed);
		}
	}

	public void StartExpedition(ExpeditionSavegame expeditionSavegame)
	{
		CurrentExpedition = expeditionSavegame;
		CurrentExpedition.StartTime = DateTime.Now.Ticks;
		SetExpeditionState(ExpeditionState.InProgress);
		DataManager.Instance.SavegameData.ExpeditionLastBoostTime = DateTime.Now.Ticks;
		//AnalyticController analyticController = BaseController.GameController.AnalyticController;
		ExpeditionRarity rarity = (ExpeditionRarity)CurrentExpedition.Rarity;
		//analyticController.LogEvent("expedition_start", "rarity", rarity.ToString());
	}

	public void CompleteExpedition()
	{
		if (ExpeditionState == ExpeditionState.Completed)
		{
			DataManager.Instance.SavegameData.ExpeditionLastTime = DateTime.Now.Ticks;
			ExpeditionRarity rarity = (ExpeditionRarity)CurrentExpedition.Rarity;
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			double superCashGain = 0.0;
			foreach (int item in CurrentExpedition.ItemID)
			{
				if (item == 1)
				{
					ExpeditionEntity.Param param = DataManager.Instance.ExpeditionDictionary[rarity];
					superCashGain = UnityEngine.Random.Range((int)param.SuperCashMin, (int)param.SuperCashMax + 1);
				}
				else
				{
					dictionary.Add(item, 1);
				}
			}
			AddMultiItem(dictionary, superCashGain);
			SetExpeditionState(ExpeditionState.Idle);
			RefreshExpedition();
			//BaseController.GameController.AnalyticController.LogEvent("expedition_complete", "rarity", rarity.ToString());
		}
	}

	public void RefreshExpedition(bool createFirstTime = false)
	{
		int num = 3;
		if (DataManager.Instance.SavegameData.ExpeditionLastTime != 0 && TimeSpan.FromTicks(DateTime.Now.Ticks - DataManager.Instance.SavegameData.ExpeditionLastTime).TotalDays > 3.0 && UnityEngine.Random.Range(0f, 1f) < 0.5f)
		{
			num++;
		}
		if (UnityEngine.Random.Range(0f, 1f) < 0.3f)
		{
			num++;
		}
		DataManager.Instance.SavegameData.ExpeditionChooseList = new List<ExpeditionSavegame>();
		for (int i = 0; i < num; i++)
		{
			DataManager.Instance.SavegameData.ExpeditionChooseList.Add(CreateRandomExpedition());
		}
		if (!createFirstTime)
		{
			DataManager.Instance.SavegameData.ExpeditionRefreshTime = DateTime.Now.Ticks;
		}
	}

	public ExpeditionSavegame CreateRandomExpedition()
	{
		float num = UnityEngine.Random.Range(0f, 1f);
		ExpeditionRarity expeditionRarity = ExpeditionRarity.Common;
		if (num < 0.01f)
		{
			expeditionRarity = ExpeditionRarity.Legendary;
		}
		else if (num < 0.1f)
		{
			expeditionRarity = ExpeditionRarity.Epic;
		}
		else if (num < 0.4f)
		{
			expeditionRarity = ExpeditionRarity.Rare;
		}
		return CreateExpedition(expeditionRarity);
	}

	public ExpeditionSavegame CreateExpedition(ExpeditionRarity expeditionRarity)
	{
		ExpeditionEntity.Param param = DataManager.Instance.ExpeditionDictionary[expeditionRarity];
		ExpeditionSavegame expeditionSavegame = new ExpeditionSavegame();
		expeditionSavegame.Rarity = (int)expeditionRarity;
		expeditionSavegame.Duration = UnityEngine.Random.Range((int)param.DurationMin, (int)param.DurationMax);
		expeditionSavegame.ItemID = new List<int>();
		ExpeditionSavegame expeditionSavegame2 = expeditionSavegame;
		if (UnityEngine.Random.Range(0f, 1f) < 0.4f)
		{
			expeditionSavegame2.ItemID.Add(1);
		}
		List<int> range = param.Items.GetRange(0, param.Items.Count);
		range.Shuffle();
		for (int i = expeditionSavegame2.ItemID.Count; i < param.NumberItem; i++)
		{
			expeditionSavegame2.ItemID.Add(range[i]);
		}
		return expeditionSavegame2;
	}
}
