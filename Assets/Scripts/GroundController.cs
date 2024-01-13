using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GroundController : BaseMineController
{
	public GroundManagerController GroundManagerController;

	public LevelTextController LevelTextController;

	[HideInInspector]
	public List<GroundWorkerController> GroundWorkerControllers;

	private bool isWorking;

	public GroundModel GroundModel => base.MineController.MineModel.GroundModel;

	public override void Awake()
	{
		base.Awake();
		base.gameObject.SetActive(value: false);
		LevelTextController.gameObject.SetActive(value: false);
		GroundManagerController.gameObject.SetActive(value: false);
	}

	public override void Start()
	{
		base.Start();
		LevelTextController.OnClickCallback = OnClickLevelUp;
		GroundManagerController.GroundController = this;
		BaseController.GameController.OnMineCashChangeCallback.Add(CheckUpgradable);
		BaseController.GameController.OnMineWorkshopChangeCallback.Add(SetSkinWorker);
		CheckUpgradable();
		AddWorker();
		if (DataManager.Instance.CurrentMineSavegame.GroundLevel > 1)
		{
			SetLevel(DataManager.Instance.CurrentMineSavegame.GroundLevel);
		}
		else
		{
			BaseController.GameController.TutorialController.StartFlashEff(LevelTextController.GetComponent<SpriteRenderer>());
		}
		InitManager();
		if (DataManager.Instance.CurrentMineSavegame.CorridorLevel[0] <= 1 && DataManager.Instance.CurrentMineSavegame.GroundManagerSavegame == null)
		{
			BaseController.GameController.TutorialController.CreateTutorial(TutorialType.WorkerGround, GroundWorkerControllers[0].gameObject, isUI: false, 1.7f, useSpotlight: true, 1.3f, new Vector3(0f, 80f));
		}
	}

	public override void Update()
	{
		base.Update();
	}

	public void SetSkinWorker()
	{
		int num = 0;
		if (DataManager.Instance.SavegameData.CurrentActiveGroundCard != -1)
		{
			num = DataManager.Instance.SavegameData.CurrentActiveGroundCard % 10;
		}
		string b = ANIMATION.WORKER_SKIN[num];
		int num2 = 0;
		if (num >= 4)
		{
			num2++;
		}
		if (num >= 7)
		{
			num2++;
		}
		if (num >= 9)
		{
			num2++;
		}
		for (int i = 0; i < GroundWorkerControllers.Count; i++)
		{
			GroundWorkerController groundWorkerController = GroundWorkerControllers[i];
			switch (num2)
			{
			case 0:
				groundWorkerController.animationIdle = "idle";
				groundWorkerController.animationWalk = "walk";
				break;
			case 1:
				groundWorkerController.animationIdle = "idle_rare";
				groundWorkerController.animationWalk = "walk_rare";
				break;
			case 2:
				groundWorkerController.animationIdle = "idle_epic";
				groundWorkerController.animationWalk = "walk_epic";
				break;
			case 3:
				groundWorkerController.animationIdle = "idle_legendary";
				groundWorkerController.animationWalk = "walk_legendary";
				break;
			}
			if (groundWorkerController.skeleton.Skin == null || !(groundWorkerController.skeleton.Skin.Name == b))
			{
				groundWorkerController.skeleton.SetSkin(ANIMATION.WORKER_SKIN[num]);
				if (groundWorkerController.GroundWorkerState == GroundWorkerState.WalkingToElevator || groundWorkerController.GroundWorkerState == GroundWorkerState.WalkingToHouse)
				{
					groundWorkerController.spineAnimationState.SetAnimation(0, groundWorkerController.animationWalk, loop: true);
				}
				else
				{
					groundWorkerController.spineAnimationState.SetAnimation(0, groundWorkerController.animationIdle, loop: true);
				}
			}
		}
	}

	private void OnClickLevelUp()
	{
		BaseController.GameController.DialogController.DialogUpgradeGround.OnShow();
	}

	public void LevelUp(int levelOffset = 1)
	{
		int level = GroundModel.Level;
		level = ((!CONST.FAST_GAME_TEST_MODE) ? (level + levelOffset) : Math.Min(GroundModel.Level + 50, GroundModel.MaxLevel));
		double num = GroundModel.SuperCashGainInRange(GroundModel.Level, level - GroundModel.Level);
		if (MathUtils.CompareDoubleBiggerThanZero(num))
		{
			AddSuperCash(num);
		}
		BaseController.GameController.TutorialController.StopFlashEff(LevelTextController.GetComponent<SpriteRenderer>());
		SetLevel(level);
	}

	public void SetLevel(int level)
	{
		GroundModel.Level = level;
		LevelTextController.SetLevel(GroundModel.Level);
		DataManager.Instance.CurrentMineSavegame.GroundLevel = GroundModel.Level;
		base.MineController.UpdateIdleCash();
		CheckUpgradable();
		int num = GroundModel.NumberOfWorkersStat.Value - GroundWorkerControllers.Count;
		for (int i = 0; i < num; i++)
		{
			AddWorker();
		}
	}

	public void CheckShowWorker()
	{
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(value: true);
			if (DataManager.Instance.CurrentMineSavegame.GroundLevel == 0)
			{
				DataManager.Instance.CurrentMineSavegame.GroundLevel = 1;
			}
		}
	}

	public void CheckUpgradable()
	{
		int maxAffordableNumbersOfLevelsToUpgrade = GroundModel.GetMaxAffordableNumbersOfLevelsToUpgrade(DataManager.Instance.Cash);
		LevelTextController.SetUpgradableByLevel(maxAffordableNumbersOfLevelsToUpgrade);
	}

	public void CheckStartWork()
	{
		if (isWorking)
		{
			return;
		}
		isWorking = true;
		for (int i = 0; i < GroundWorkerControllers.Count; i++)
		{
			if (!GroundWorkerControllers[i].inAnimation)
			{
				GroundWorkerControllers[i].StartWithDelay();
			}
		}
	}

	public void OnWorkerComplete(GroundWorkerController GroundWorkerController)
	{
		double cash = GroundWorkerController.CoinController.Cash;
		GroundWorkerController.CoinController.UseAllCash();
		AddCash(cash * BaseController.GameController.BoostController.TotalBoostFactor);
		DataManager.Instance.CurrentMineSavegame.MineLastTimeVisit = DateTime.Now.Ticks;
		if (GroundManagerController.isEmpty)
		{
			for (int i = 0; i < GroundWorkerControllers.Count; i++)
			{
				if (GroundWorkerControllers[i].inAnimation)
				{
					return;
				}
			}
			isWorking = false;
		}
		else
		{
			GroundWorkerController.StartMoving();
			if (GroundWorkerControllers.IndexOf(GroundWorkerController) == 0)
			{
				isWorking = false;
				CheckStartWork();
			}
		}
	}

	public void AddWorker()
	{
		GroundWorkerController component = InstantiatePrefab("Prefabs/Mine/GroundWorker").GetComponent<GroundWorkerController>();
		component.transform.SetParent(base.transform);
		component.transform.localPosition = Vector3.zero;
		component.GroundWorkerModel = GroundModel.AddWorkerModel();
		component.GroundController = this;
		GroundWorkerControllers.Add(component);
		SetWorkerGroundLength();
		SetSkinWorker();
	}

	public void SetWorkerGroundLength()
	{
		for (int i = 0; i < GroundWorkerControllers.Count; i++)
		{
			GroundWorkerController groundWorkerController = GroundWorkerControllers[i];
			int num = (i + 1) / 2;
			int num2 = (i % 2 != 0) ? 1 : (-1);
			groundWorkerController.groundDeltaX = (float)(num2 * 2) * 0.3f * (float)num / (float)GroundWorkerControllers.Count;
			groundWorkerController.delayTime = 0.7f * (float)i;
			groundWorkerController.GetComponent<SortingGroup>().sortingOrder = GroundWorkerControllers.Count - i;
		}
	}

	public void InitManager()
	{
		if (DataManager.Instance.CurrentMineSavegame.GroundManagerSavegame != null)
		{
			GroundManagerController.LoadManagerSavegame(DataManager.Instance.CurrentMineSavegame.GroundManagerSavegame);
		}
	}

	public void OnChangeManager()
	{
	}

	public void OnActiveManager()
	{
		GroundModel.SetBonusContainer(GroundManagerController.ManagerParam.EffectID, GroundManagerController.ManagerParam.ManagerID);
	}

	public void OnDeactiveManager()
	{
		GroundModel.ResetBonusContainer();
	}

	public void CheckWorkerLoading()
	{
		for (int i = 0; i < GroundWorkerControllers.Count; i++)
		{
			if (GroundWorkerControllers[i].GroundWorkerState == GroundWorkerState.Loading)
			{
				return;
			}
		}
		base.MineController.ElevatorController.StopSquirrelAnimation();
	}
}
