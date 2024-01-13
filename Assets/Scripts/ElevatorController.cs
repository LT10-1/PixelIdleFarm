using Spine;
using System;
using UnityEngine;

public class ElevatorController : BaseMineController
{
	public ElevatorWorkerController ElevatorWorkerController;

	public ElevatorManagerController ElevatorManagerController;

	public CoinController ElevatorHouseCoinController;

	public LevelTextController LevelTextController;

	public ProgressBarController ProgressBarController;

	public CharacterControllerSekeleton Squirrel;

	public CharacterControllerSekeleton Voinuoc;

	public CharacterControllerSekeleton RongRoc;

	public CharacterControllerSekeleton MineResource;

	public Transform CoinPositionGrass;

	public Transform CoinPositionSand;

	public Transform ResourcePositionGrass;

	public Transform ResourcePositionSand;

	private bool isRunning;

	private bool isRunningForward;

	private long endTime;

	public ElevatorModel ElevatorModel => base.MineController.MineModel.ElevatorModel;

	public override void Awake()
	{
		base.Awake();
		base.gameObject.SetActive(value: false);
		LevelTextController.gameObject.SetActive(value: false);
		ElevatorManagerController.gameObject.SetActive(value: false);
		MineResource.gameObject.SetActive(value: false);
		Squirrel.gameObject.SetActive(base.CurrentContinent == ContinentType.Grass);
		Voinuoc.gameObject.SetActive(base.CurrentContinent == ContinentType.Sand);
		RongRoc.gameObject.SetActive(base.CurrentContinent == ContinentType.Sakura);
		switch (base.CurrentContinent)
		{
		case ContinentType.Grass:
			ElevatorHouseCoinController.transform.localPosition = CoinPositionGrass.localPosition;
			MineResource.transform.localPosition = ResourcePositionGrass.localPosition;
			break;
		case ContinentType.Sand:
			ElevatorHouseCoinController.transform.localPosition = CoinPositionSand.localPosition;
			MineResource.transform.localPosition = ResourcePositionSand.localPosition;
			break;
		case ContinentType.Sakura:
			ElevatorHouseCoinController.transform.localPosition = CoinPositionSand.localPosition;
			MineResource.transform.localPosition = ResourcePositionGrass.localPosition;
			break;
		}
	}

	public override void Start()
	{
		base.Start();
		ElevatorManagerController.ElevatorController = this;
		ElevatorWorkerController.ElevatorController = this;
		LevelTextController.OnClickCallback = OnClickLevelUp;
		MineResource.skeleton.SetSkin(ANIMATION.RESOURCE_SKIN[(int)base.CurrentContinent][base.MineController.MineIndex]);
		ElevatorHouseCoinController.SetCoinType(base.CurrentCoinType);
		BaseController.GameController.OnMineCashChangeCallback.Add(CheckUpgradable);
		BaseController.GameController.OnMineWorkshopChangeCallback.Add(SetSkinWorker);
		CheckUpgradable();
		InitManager();
		SetSkinWorker();
		if (DataManager.Instance.CurrentMineSavegame.ElevatorLevel > 1)
		{
			SetLevel(DataManager.Instance.CurrentMineSavegame.ElevatorLevel);
		}
		else
		{
			BaseController.GameController.TutorialController.StartFlashEff(LevelTextController.GetComponent<SpriteRenderer>());
		}
		if (DataManager.Instance.CurrentMineSavegame.GroundLevel == 0 && DataManager.Instance.CurrentMineSavegame.ElevatorManagerSavegame == null)
		{
			BaseController.GameController.TutorialController.CreateTutorial(TutorialType.WorkerElevator, ElevatorWorkerController.gameObject, isUI: false, 1.7f, useSpotlight: true, 1.5f, new Vector3(0f, 30f));
		}
	}

	public override void Update()
	{
		base.Update();
	}

	public void CheckShowElevator()
	{
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(value: true);
			if (DataManager.Instance.CurrentMineSavegame.ElevatorLevel == 0)
			{
				DataManager.Instance.CurrentMineSavegame.ElevatorLevel = 1;
			}
		}
	}

	private void OnClickLevelUp()
	{
		BaseController.GameController.DialogController.DialogUpgradeElevator.OnShow();
	}

	public void LevelUp(int levelOffset = 1)
	{
		int level = ElevatorModel.Level;
		level = ((!CONST.FAST_GAME_TEST_MODE) ? (level + levelOffset) : Math.Min(ElevatorModel.Level + 50, 2400));
		double num = ElevatorModel.SuperCashGainInRange(ElevatorModel.Level, level - ElevatorModel.Level);
		if (MathUtils.CompareDoubleBiggerThanZero(num))
		{
			AddSuperCash(num);
		}
		BaseController.GameController.TutorialController.StopFlashEff(LevelTextController.GetComponent<SpriteRenderer>());
		SetLevel(level);
	}

	public void SetLevel(int level)
	{
		ElevatorModel.Level = level;
		LevelTextController.SetLevel(ElevatorModel.Level);
		DataManager.Instance.CurrentMineSavegame.ElevatorLevel = ElevatorModel.Level;
		base.MineController.UpdateIdleCash();
		CheckUpgradable();
	}

	public void SetSkinWorker()
	{
		int num = 0;
		if (DataManager.Instance.SavegameData.CurrentActiveElevatorCard != -1)
		{
			num = DataManager.Instance.SavegameData.CurrentActiveElevatorCard % 10;
		}
		string text = ANIMATION.WORKER_SKIN[num];
		if (ElevatorWorkerController.skeleton.Skin == null || !(ElevatorWorkerController.skeleton.Skin.Name == text))
		{
			ElevatorWorkerController.skeleton.SetSkin(text);
			ElevatorWorkerController.ElevatorBg.sprite = BaseController.LoadSprite(DATA_RESOURCES.IMAGE.GAMEPLAY_ELEVATOR_FRONT[num]);
			ElevatorWorkerController.ElevatorBgBack.sprite = BaseController.LoadSprite(DATA_RESOURCES.IMAGE.GAMEPLAY_ELEVATOR_BACK[num]);
			if (num < 4)
			{
				ElevatorWorkerController.ResourceBg.transform.localScale = new Vector3(1f, 1f);
			}
			else
			{
				ElevatorWorkerController.ResourceBg.transform.localScale = new Vector3(1.26f, 1f);
			}
			if (num == 9)
			{
				ElevatorWorkerController.CoinController.transform.localPosition = ElevatorWorkerController.CoinPositionLegendary.transform.localPosition;
			}
			else
			{
				ElevatorWorkerController.CoinController.transform.localPosition = ElevatorWorkerController.CoinPositionNormal.transform.localPosition;
			}
		}
	}

	public void CheckUpgradable()
	{
		int maxAffordableNumbersOfLevelsToUpgrade = ElevatorModel.GetMaxAffordableNumbersOfLevelsToUpgrade(DataManager.Instance.Cash);
		LevelTextController.SetUpgradableByLevel(maxAffordableNumbersOfLevelsToUpgrade);
	}

	public void CompleteCollect()
	{
		double cash = ElevatorWorkerController.CoinController.Cash;
		ElevatorHouseCoinController.AddCash(cash);
		ElevatorWorkerController.CoinController.UseAllCash();
		ElevatorWorkerController.OnMoneyChange();
		base.MineController.GroundController.CheckShowWorker();
	}

	public void InitManager()
	{
		if (DataManager.Instance.CurrentMineSavegame.ElevatorManagerSavegame != null)
		{
			ElevatorManagerController.LoadManagerSavegame(DataManager.Instance.CurrentMineSavegame.ElevatorManagerSavegame);
		}
	}

	public void OnChangeManager()
	{
	}

	public void OnActiveManager()
	{
		ElevatorModel.SetBonusContainer(ElevatorManagerController.ManagerParam.EffectID, ElevatorManagerController.ManagerParam.ManagerID);
	}

	public void OnDeactiveManager()
	{
		ElevatorModel.ResetBonusContainer();
	}

	public void StartSquirrelAnimation(double duration)
	{
		if (!MathUtils.CompareDoubleToZero(duration))
		{
			endTime = Math.Max(DateTime.Now.Ticks + (long)(10000000.0 * duration), endTime);
		}
		if (!isRunning)
		{
			isRunning = true;
			isRunningForward = true;
			MineResource.gameObject.SetActive(value: true);
			if (duration < 0.5)
			{
				MineResource.spineAnimationState.TimeScale = 1f / (float)duration;
			}
			else
			{
				MineResource.spineAnimationState.TimeScale = 1.5f;
			}
			MineResource.spineAnimationState.SetAnimation(0, "animation", loop: true);
			switch (base.CurrentContinent)
			{
			case ContinentType.Grass:
				Squirrel.spineAnimationState.SetAnimation(0, "forward", loop: false);
				Squirrel.spineAnimationState.Complete -= OnSquirrelMoveForward;
				Squirrel.spineAnimationState.Complete += OnSquirrelMoveForward;
				break;
			case ContinentType.Sand:
				Voinuoc.spineAnimationState.SetAnimation(0, "animation", loop: true);
				break;
			case ContinentType.Sakura:
				RongRoc.spineAnimationState.SetAnimation(0, "forward", loop: false);
				RongRoc.spineAnimationState.Complete -= OnSquirrelMoveForward;
				RongRoc.spineAnimationState.Complete += OnSquirrelMoveForward;
				break;
			}
		}
	}

	public void OnSquirrelMoveForward(TrackEntry entry)
	{
		isRunningForward = false;
		if (!isRunning)
		{
			isRunning = true;
			StopSquirrelAnimation();
		}
	}

	public void StopSquirrelAnimation()
	{
		MineResource.gameObject.SetActive(value: false);
		if (!isRunning)
		{
			return;
		}
		isRunning = false;
		switch (base.CurrentContinent)
		{
		case ContinentType.Grass:
			if (!isRunningForward && !(Squirrel.skeletonAnimation.AnimationName != "forward"))
			{
				Squirrel.spineAnimationState.SetAnimation(0, "backward", loop: false);
			}
			break;
		case ContinentType.Sand:
			Voinuoc.spineAnimationState.SetAnimation(0, "animation", loop: false);
			break;
		case ContinentType.Sakura:
			if (!isRunningForward && !(RongRoc.skeletonAnimation.AnimationName != "forward"))
			{
				RongRoc.spineAnimationState.SetAnimation(0, "backward", loop: false);
			}
			break;
		}
	}
}
