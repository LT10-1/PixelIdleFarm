using DG.Tweening;
using Spine.Unity;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CorridorLevelController : BaseMineController
{
	public CorridorManagerController CorridorManagerController;

	public CoinController CoinController;

	public LevelTextController LevelTextController;

	public ProgressBarController ProgressBarController;

	public Transform CorridorWorkerControllersGroup;

	public AnimationSprite MineLevel;

	public AnimationSprite CandleLevel1;

	public AnimationSprite CandleLevel2;

	public AnimationSprite CorridorBox;

	public SpriteRenderer CorridorBoxResource;

	public Transform CorridorFinalTransform;

	private Vector3 _originalCandle1Position;

	public TMP_Text TierLevelText;

	public SpriteRenderer SandRenderer;

	public SpriteRenderer ElevatorPart;

	public SpriteRenderer RockLayer;

	public GameObject InitGroup;

	public GameObject MineGroup;

	public GameObject UIGroup;

	public CoinButtonController ButtonNewShaft;

	public CoinButtonController ButtonNewShaftSuperCash;

	[HideInInspector]
	public CorridorController CorridorController;

	[HideInInspector]
	public List<CorridorWorkerController> CorridorWorkerControllers;

	public CorridorModel CorridorModel;

	public bool isWorking;

	private float workerDelayTime;

	private bool changeSandSprite = true;

	private float lastTime;

	private float SandStartX;

	private Vector3 originalCandle1Position
	{
		get
		{
			if (_originalCandle1Position == Vector3.zero)
			{
				_originalCandle1Position = CandleLevel1.transform.localPosition;
			}
			return _originalCandle1Position;
		}
	}

	public int CurrentCorridorLevelSavegame
	{
		get
		{
			if (DataManager.Instance.CurrentMineSavegame.CorridorLevel == null)
			{
				return 0;
			}
			if (DataManager.Instance.CurrentMineSavegame.CorridorLevel.Count <= CorridorModel.Tier - 1)
			{
				return 0;
			}
			return DataManager.Instance.CurrentMineSavegame.CorridorLevel[CorridorModel.Tier - 1];
		}
		set
		{
			DataManager.Instance.CurrentMineSavegame.CorridorLevel[CorridorModel.Tier - 1] = value;
		}
	}

	public bool IsActiveMine => MineGroup.activeSelf;

	public void Init(int tier)
	{
		CorridorModel = base.MineController.MineModel.GetCorridorModel(tier);
		if (CurrentCorridorLevelSavegame > 0 && DataManager.Instance.CurrentMineSavegame.GetCorridorSavegame(CorridorModel.Tier) != null)
		{
			workerDelayTime = 0.3f * (float)UnityEngine.Random.Range(0, 5);
		}
		else
		{
			workerDelayTime = 0f;
		}
		SetShaftActive(active: false);
	}

	public override void Awake()
	{
		base.Awake();
		RockLayer.sprite = BaseController.LoadSprite(DATA_RESOURCES.IMAGE.GAMEPLAY_CONTINENT_LAYER_ROCK(base.CurrentContinent));
		ElevatorPart.sprite = BaseController.LoadSprite(DATA_RESOURCES.IMAGE.GAMEPLAY_CONTINENT_ELEVATOR_DOWN(base.CurrentContinent));
		ChangeSandSprite();
	}

	public override void Start()
	{
		base.Start();
		LevelTextController.OnClickCallback = OnClickLevelUp;
		ButtonNewShaft.OnClickCallback = OnClickNewShaft;
		ButtonNewShaftSuperCash.OnClickCallback = OnClickNewShaftSuperCash;
		CorridorManagerController.CorridorLevelController = this;
		CoinController.SetCoinType(base.CurrentCoinType);
		ButtonNewShaft.CoinController.SetCoinType(base.CurrentCoinType);
		CorridorWorkerControllers = new List<CorridorWorkerController>();
		ButtonNewShaft.SetMoney(CorridorModel.CorridorUpgradeCostStat.Value);
		CorridorBoxResource.gameObject.SetActive(value: false);
		TierLevelText.text = CorridorModel.Tier + string.Empty;
		if (CorridorModel.Tier != 1)
		{
			CorridorManagerController.gameObject.SetActive(value: true);
		}
		MineLevel.SetAnimation("Images/Gameplay/MineLevel/mine_level");
		CorridorBox.SetAnimation("Images/Gameplay/MineLevel/Box");
		CorridorBoxResource.sprite = BaseController.LoadSprite(DATA_RESOURCES.IMAGE.GAMEPLAY_ELEVATOR_RESOURCE(base.MineController.MineResourceName)[0]);
		BaseController.GameController.OnMineCashChangeCallback.Add(CheckUpgradable);
		BaseController.GameController.OnMineSuperCashChangeCallback.Add(CheckUpgradableSuperCash);
		base.MineController.OnUpdateIdleCash.Add(OnUpdateIdleCash);
		BaseController.GameController.OnMineWorkshopChangeCallback.Add(SetSkinWorker);
		CheckUpgradable();
		CheckUpgradableSuperCash();
		AddWorker();
		InitSandByTier();
		UpdateCorridorLength();
		if (CurrentCorridorLevelSavegame > 0)
		{
			ShowNewShaft();
			SetLevel(CurrentCorridorLevelSavegame);
			InitManager();
		}
		if (CorridorModel.Tier == 1 && DataManager.Instance.CurrentMineSavegame.CorridorLevel[0] == 0)
		{
			BaseController.GameController.TutorialController.CreateTutorial(TutorialType.NewShaft, ButtonNewShaft.gameObject, isUI: false, 1f, useSpotlight: true, 1.2f);
		}
	}

	public override void Update()
	{
		base.Update();
		if (!changeSandSprite && Time.realtimeSinceStartup - lastTime > 5f)
		{
			changeSandSprite = true;
			lastTime = Time.realtimeSinceStartup;
		}
	}

	private void OnClickNewShaft()
	{
		double value = CorridorModel.CorridorUpgradeCostStat.Value;
		BaseController.GameController.TutorialController.RemoveTutorial(TutorialType.NewShaft);
		if (UseCash(value))
		{
			BuyNewShaftSuccess();
		}
	}

	private void OnClickNewShaftSuperCash()
	{
		double cash = ButtonNewShaftSuperCash.Cash;
		UseSuperCash(cash, SpendSuperCashType.UnlockMineShaft, delegate
		{
			//BaseController.GameController.AnalyticController.LogEvent("use_super_cash_to_buy_shaft", "tier", DataManager.Instance.SavegameData.CurrentMine * 100 + CorridorController.NumberActiveCorridor);
			BuyNewShaftSuccess();
		});
	}

	public void BuyNewShaftSuccess()
	{
		BaseController.GameController.AudioController.PlayOneShot("Audios/Effect/mohammo");
		InitGroup.SetActive(value: false);
		CorridorController.StartNewShaftAnimation();
	}

	public void InitNewShaft()
	{
		ShowNewShaft();
		InitManager();
		CheckUpgradable();
		CheckUpgradableSuperCash();
		CurrentCorridorLevelSavegame = 1;
	}

	private void SetShaftActive(bool active)
	{
		InitGroup.SetActive(!active);
		MineGroup.SetActive(active);
		UIGroup.SetActive(active);
	}

	private void ShowNewShaft()
	{
		SetShaftActive(active: true);
		CorridorController.OnBuyNewShaft();
		OnLevelUpChangeSkin();
		if (DataManager.Instance.CurrentMineSavegame.ElevatorLevel == 0 && !DataManager.Instance.CurrentMineSavegame.CorridorCurrentManager.ContainsKey(CorridorModel.Tier))
		{
			BaseController.GameController.TutorialController.CreateTutorial(TutorialType.WorkerCorridor, CorridorWorkerControllers[0].gameObject, isUI: false, 1.7f, useSpotlight: true, 1.3f, new Vector3(0f, 80f));
		}
	}

	private void OnClickLevelUp()
	{
		BaseController.GameController.TutorialController.RemoveTutorial(TutorialType.ButtonLevelUp);
		BaseController.GameController.DialogController.DialogUpgradeCorridor.CurrentCorridorTier = CorridorModel.Tier;
		BaseController.GameController.DialogController.DialogUpgradeCorridor.OnShow();
	}

	public void OnLevelUpChangeSkin()
	{
		int num = 0;
		if (CorridorModel.Level >= 25)
		{
			num++;
		}
		if (CorridorModel.Level >= 100)
		{
			num++;
		}
		if (CorridorModel.Level >= 300)
		{
			num++;
		}
		if (CorridorModel.Level >= 500)
		{
			num++;
		}
		if (CorridorModel.Level >= 700)
		{
			num++;
		}
		MineLevel.SetFrame(num);
		CorridorBox.SetFrame(num);
		CandleLevel2.gameObject.SetActive(num == 4);
		CandleLevel1.transform.localPosition = ((num != 4) ? originalCandle1Position : (originalCandle1Position + 0.8f * Vector3.left));
		switch (num)
		{
		case 0:
			CandleLevel1.SetAnimation("Images/Gameplay/MineLevel/Candle");
			break;
		case 1:
			CandleLevel1.SetAnimation("Images/Gameplay/MineLevel/Torch");
			break;
		case 2:
			CandleLevel1.SetAnimation("Images/Gameplay/MineLevel/Torch");
			break;
		case 3:
			CandleLevel1.SetAnimation("Images/Gameplay/MineLevel/Light");
			break;
		case 4:
			CandleLevel1.SetAnimation("Images/Gameplay/MineLevel/Light");
			CandleLevel2.SetAnimation("Images/Gameplay/MineLevel/Light");
			break;
		case 5:
			CandleLevel1.SetAnimation("Images/Gameplay/MineLevel/light_final");
			CandleLevel1.transform.localPosition = CorridorFinalTransform.localPosition;
			break;
		}
	}

	public void OnUpdateIdleCash()
	{
		CheckButtonSuperCash();
	}

	public void CheckUpgradableSuperCash()
	{
		CheckButtonSuperCash();
	}

	public void CheckButtonSuperCash()
	{
		if (!IsActiveMine && DataManager.Instance.IdleCash > 0.0)
		{
			ButtonNewShaftSuperCash.gameObject.SetActive(value: true);
			double num = Math.Floor(0.09 * Math.Pow(ButtonNewShaft.Cash / DataManager.Instance.TotalIdleCash, 0.79));
			if (num < 10.0)
			{
				num = 10.0;
			}
			if (num >= 10000.0)
			{
				ButtonNewShaftSuperCash.gameObject.SetActive(value: false);
				return;
			}
			ButtonNewShaftSuperCash.SetMoney(num, minify: false);
			bool flag = DataManager.Instance.SuperCash >= ButtonNewShaftSuperCash.Cash;
			ButtonNewShaftSuperCash.SetBuyAble(flag);
			if (flag)
			{
				BaseController.GameController.TutorialController.StartFlashEff(ButtonNewShaftSuperCash.GetComponentInChildren<SpriteRenderer>(includeInactive: true));
			}
			else
			{
				BaseController.GameController.TutorialController.StopFlashEff(ButtonNewShaftSuperCash.GetComponentInChildren<SpriteRenderer>(includeInactive: true));
			}
		}
		else
		{
			ButtonNewShaftSuperCash.gameObject.SetActive(value: false);
		}
	}

	public void CheckUpgradable()
	{
		if (IsActiveMine)
		{
			int maxAffordableNumbersOfLevelsToUpgrade = CorridorModel.GetMaxAffordableNumbersOfLevelsToUpgrade(DataManager.Instance.Cash);
			LevelTextController.SetUpgradableByLevel(maxAffordableNumbersOfLevelsToUpgrade);
			if (DataManager.Instance.SavegameData.CurrentMineUnlocked == 0 && CorridorModel.Tier == 1)
			{
				if (maxAffordableNumbersOfLevelsToUpgrade > 0 && CorridorModel.Level == 1)
				{
					BaseController.GameController.TutorialController.CreateTutorial(TutorialType.ButtonLevelUp, LevelTextController.gameObject, isUI: false, 1f, useSpotlight: true, 1.2f);
					BaseController.GameController.TutorialController.StartFlashEff(LevelTextController.GetComponent<SpriteRenderer>());
				}
				if (DataManager.Instance.CurrentMineSavegame.CorridorManagerOrder == 0 && CorridorModel.Level != 1 && CheckEnoughCash(MISC_PARAMS.CORRIDOR_MANAGER_COST_INIT[0]))
				{
					BaseController.GameController.TutorialController.CreateTutorial(TutorialType.HireManager, CorridorManagerController.gameObject, isUI: false, 1.8f, useSpotlight: true, 1.3f, new Vector3(0f, 80f));
				}
			}
		}
		else
		{
			bool flag = DataManager.Instance.Cash >= ButtonNewShaft.Cash;
			ButtonNewShaft.SetBuyAble(flag);
			if (flag)
			{
				BaseController.GameController.TutorialController.StartFlashEff(ButtonNewShaft.GetComponentInChildren<SpriteRenderer>(includeInactive: true));
			}
			else
			{
				BaseController.GameController.TutorialController.StopFlashEff(ButtonNewShaft.GetComponentInChildren<SpriteRenderer>(includeInactive: true));
			}
		}
	}

	public void LevelUp(int levelOffset = 1)
	{
		int level = CorridorModel.Level;
		level = ((!CONST.FAST_GAME_TEST_MODE) ? (level + levelOffset) : Math.Min(CorridorModel.Level + 40, CorridorModel.MaxLevel));
		double num = CorridorModel.SuperCashGainInRange(CorridorModel.Level, level - CorridorModel.Level);
		if (MathUtils.CompareDoubleBiggerThanZero(num))
		{
			AddSuperCash(num);
		}
		BaseController.GameController.TutorialController.StopFlashEff(LevelTextController.GetComponent<SpriteRenderer>());
		SetLevel(level);
	}

	public void SetLevel(int level)
	{
		CorridorModel.Level = level;
		LevelTextController.SetLevel(CorridorModel.Level);
		CurrentCorridorLevelSavegame = CorridorModel.Level;
		base.MineController.UpdateIdleCash();
		CheckUpgradable();
		if (!CONST.TEST_MODE_VIDEO_AD)
		{
			int num = CorridorModel.NumberOfWorkersStat.Value - CorridorWorkerControllers.Count;
			for (int i = 0; i < num; i++)
			{
				AddWorker();
			}
		}
		if (CorridorModel.Tier == 1 && CorridorModel.Level >= 2)
		{
			CorridorManagerController.gameObject.SetActive(value: true);
			base.MineController.GroundController.GroundManagerController.gameObject.SetActive(value: true);
			base.MineController.ElevatorController.ElevatorManagerController.gameObject.SetActive(value: true);
		}
		if (CorridorModel.Tier == 1 && CorridorModel.Level >= 10)
		{
			CompleteTutorial();
		}
		if (CONST.TEST_MODE_VIDEO_AD)
		{
			CoinController.gameObject.SetActive(value: false);
			CorridorBoxResource.gameObject.SetActive(value: true);
			LevelTextController.gameObject.SetActive(value: false);
			if (CorridorController.CorridorLevelControllers.Count > 1)
			{
				CorridorController.CorridorLevelControllers[1].gameObject.SetActive(value: false);
			}
		}
		UpdateCorridorLength();
		OnLevelUpChangeSkin();
	}

	public void CompleteTutorial()
	{
		base.MineController.GroundController.LevelTextController.gameObject.SetActive(value: true);
		base.MineController.ElevatorController.LevelTextController.gameObject.SetActive(value: true);
		if (CorridorController.CorridorLevelControllers.Count <= 1)
		{
			CorridorController.CreateCorridorLevel();
		}
	}

	public void SetSkinWorker()
	{
		int num = 0;
		if (DataManager.Instance.SavegameData.CurrentActiveCorridorCard != -1)
		{
			num = DataManager.Instance.SavegameData.CurrentActiveCorridorCard % 10;
		}
		string text = ANIMATION.WORKER_SKIN[num];
		string text2 = "dig";
		if (num >= 4)
		{
			text2 = "smash";
		}
		if (num >= 7)
		{
			text2 = "drill";
		}
		for (int i = 0; i < CorridorWorkerControllers.Count; i++)
		{
			CorridorWorkerController corridorWorkerController = CorridorWorkerControllers[i];
			if (corridorWorkerController.skeleton.Skin != null && corridorWorkerController.skeleton.Skin.Name == text)
			{
				continue;
			}
			corridorWorkerController.skeleton.SetSkin(text);
			if (corridorWorkerController.digAnimation != text2)
			{
				corridorWorkerController.digAnimation = text2;
				if (corridorWorkerController.CorridorWorkerState == CorridorWorkerState.WorkingOnStones)
				{
					corridorWorkerController.spineAnimationState.SetAnimation(0, corridorWorkerController.digAnimation, loop: true);
				}
			}
		}
	}

	public void RunProgressBar(float duration)
	{
		if (CorridorWorkerControllers.Count == 1)
		{
			ProgressBarController.Run(duration);
		}
	}

	public void CheckStartWork()
	{
		if (isWorking)
		{
			return;
		}
		isWorking = true;
		for (int i = 0; i < CorridorWorkerControllers.Count; i++)
		{
			if (!CorridorWorkerControllers[i].inAnimation)
			{
				CorridorWorkerControllers[i].StartWithDelay();
			}
		}
	}

	public void OnWorkerComplete(CorridorWorkerController CorridorWorkerController, double cash)
	{
		CoinController.AddCash(cash);
		CorridorBoxResource.gameObject.SetActive(value: true);
		base.MineController.ElevatorController.CheckShowElevator();
		if (CorridorManagerController.isEmpty)
		{
			for (int i = 0; i < CorridorWorkerControllers.Count; i++)
			{
				if (CorridorWorkerControllers[i].inAnimation)
				{
					return;
				}
			}
			isWorking = false;
		}
		else
		{
			CorridorWorkerController.StartMining();
			if (CorridorWorkerControllers.IndexOf(CorridorWorkerController) == 0)
			{
				isWorking = false;
				CheckStartWork();
			}
		}
	}

	public void ChangeSandSprite()
	{
		if (changeSandSprite)
		{
			changeSandSprite = false;
			List<string> list = DATA_RESOURCES.IMAGE.GAMEPLAY_CONTINENT_RESOURCE_SAND(base.CurrentContinent);
			SandRenderer.sprite = BaseController.LoadSprite(list[UnityEngine.Random.Range(0, list.Count)]);
		}
	}

	public void AddWorker()
	{
		CorridorWorkerController component = InstantiatePrefab("Prefabs/Mine/CorridorWorker").GetComponent<CorridorWorkerController>();
		component.transform.SetParent(CorridorWorkerControllersGroup);
		component.transform.localPosition = Vector3.zero;
		component.CorridorWorkerModel = CorridorModel.AddWorkerModel();
		component.CorridorLevelController = this;
		CorridorWorkerControllers.Add(component);
		SetWorkerCorridorLength();
		SetSkinWorker();
	}

	public void SetWorkerCorridorLength()
	{
		for (int i = 0; i < CorridorWorkerControllers.Count; i++)
		{
			CorridorWorkerController corridorWorkerController = CorridorWorkerControllers[i];
			int num = (i + 1) / 2;
			int num2 = (i % 2 != 0) ? 1 : (-1);
			corridorWorkerController.corridorDeltaX = (float)(num2 * 2) * 0.2f * (float)num / (float)CorridorWorkerControllers.Count;
			corridorWorkerController.delayTime = workerDelayTime + 0.5f * (float)i;
			corridorWorkerController.GetComponent<MeshRenderer>().sortingOrder = CorridorWorkerControllers.Count - i;
			corridorWorkerController.skeleton.SetColor((i <= 0) ? Color.white : new Color(0.8f, 0.8f, 0.8f, 1f));
		}
	}

	public void UpdateCorridorLength()
	{
		if (!CONST.TEST_MODE_VIDEO_AD)
		{
			if (SandStartX == 0f)
			{
				Vector3 position = SandRenderer.transform.position;
				SandStartX = position.x;
			}
			float num = 0.8f + 0.8f * (float)CorridorModel.GetFullLengthMined() / 8f;
			Transform transform = SandRenderer.transform;
			float x = SandStartX + num - 0.8f;
			Vector3 localPosition = SandRenderer.transform.localPosition;
			transform.localPosition = new Vector3(x, localPosition.y);
			for (int i = 0; i < CorridorWorkerControllers.Count; i++)
			{
				CorridorWorkerController corridorWorkerController = CorridorWorkerControllers[i];
				corridorWorkerController.corridorLength = num;
			}
		}
	}

	public void InitSandByTier()
	{
		float num = 1f - (float)CorridorModel.Tier * 0.02f;
		SandRenderer.color = new Color(num, num, num);
		int num2 = 3;
		float num3 = 1.31999993f;
		float num4 = 4f;
		float num5 = num3 / (float)num2;
		Vector2 vector = new Vector2((float)CorridorModel.Tier * 0.001f + 0.1f, (float)CorridorModel.Tier * 0.001f + 0.15f);
		float num6 = (0f - num3) / 2f + num3 / 4f;
		float num7 = (0f - num4) / 2f;
		for (int i = 0; i < num2; i++)
		{
			int num8 = UnityEngine.Random.Range(3 + CorridorModel.Tier / 3, 5 + CorridorModel.Tier / 3);
			float num9 = num4 / (float)num8;
			for (int j = 0; j < num8; j++)
			{
				GameObject gameObject = base.MineController.CreateMineGameObject();
				gameObject.transform.SetParent(SandRenderer.transform);
				gameObject.GetComponent<SpriteRenderer>().sortingOrder += 3;
				gameObject.transform.localEulerAngles = Vector3.forward * UnityEngine.Random.Range(0, 360);
				gameObject.transform.localScale = Vector3.one * UnityEngine.Random.Range(vector.x, vector.y);
				gameObject.transform.localPosition = new Vector3(vector.x + num7 + (float)j * num9 + UnityEngine.Random.Range(0f, num9 / 2f), num6 + (float)i * num5 - UnityEngine.Random.Range(0f, num5 / 2f));
			}
		}
	}

	public void InitManager()
	{
		ManagerSavegame corridorSavegame = DataManager.Instance.CurrentMineSavegame.GetCorridorSavegame(CorridorModel.Tier);
		if (corridorSavegame != null)
		{
			CorridorManagerController.LoadManagerSavegame(corridorSavegame);
		}
	}

	public void OnChangeManager()
	{
	}

	public void OnActiveManager()
	{
		CorridorModel.SetBonusContainer(CorridorManagerController.ManagerParam.EffectID, CorridorManagerController.ManagerParam.ManagerID);
	}

	public void OnDeactiveManager()
	{
		CorridorModel.ResetBonusContainer();
	}

	public void ElevatorStartCollect(float duration)
	{
		float rotateDuration = 0.2f;
		CorridorBox.transform.DOLocalRotate(Vector3.forward * 20f, rotateDuration).onComplete = delegate
		{
			if (duration > rotateDuration)
			{
				base.MineController.ElevatorController.ElevatorWorkerController.ResourceFromCorridor.gameObject.SetActive(value: true);
				CorridorBox.transform.DOLocalRotate(CorridorBox.transform.eulerAngles, duration - rotateDuration).onComplete = delegate
				{
					CorridorBox.transform.DOLocalRotate(Vector3.zero, rotateDuration);
					base.MineController.ElevatorController.ElevatorWorkerController.ResourceFromCorridor.gameObject.SetActive(value: false);
				};
			}
			else
			{
				CorridorBox.transform.DOLocalRotate(Vector3.zero, rotateDuration);
			}
		};
	}
}
