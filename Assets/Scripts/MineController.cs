using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MineController : BaseMineController
{
	public Camera MainCamera;

	[HideInInspector]
	public ScrollCamera ScrollCamera;

	public Transform TopDown;

	public Image TopMenu;

	public SurfaceController SurfaceController;

	public UndergroundController UndergroundController;

	public MileStoneLockController MileStoneLockController;

	public MineEffectController MineEffectController;

	public MineBoostController MineBoostController;

	public GameObject FacebookFriends;

	public CorridorController CorridorController;

	public ElevatorController ElevatorController;

	public GroundController GroundController;

	public TMP_Text CashTypeIcon;

	public TMP_Text TextCash;

	public TMP_Text TextIdleCash;

	public TMP_Text TextSuperCash;

	public TMP_Text TextCashType;

	public TMP_Text TextIdleCashType;

	public TMP_Text TextTestAddCash;

	public TMP_Text TextTestAddSuperCash;

	public TMP_Text TextTestAddItem;

	public TMP_Text TextTestAddSkillPoint;

	public TMP_Text TextTestAddChest;

	public UIButtonController ButtonSetting;

	public UIButtonController ButtonFriends;

	public Button ButtonPrestigeGroup;

	public TMP_Text MineNameText;

	public TMP_Text PrestigeCountText;

	public GameObject ButtonControllCamera;

	public Button ButtonCameraUp;

	public Button ButtonCameraDown;

	public UIButtonController ButtonBoost;

	public UIButtonController ButtonWorldmap;

	public UIButtonController ButtonMine;

	public UIButtonController ButtonSuperShop;

	public UIButtonController ButtonShop;

	public UIButtonController ButtonTestAd;

	public ButtonShowChest ButtonShowChest;

	public ButtonMenuHightlight ButtonDailyReward;

	public ButtonMenuHightlight ButtonPrestigeMine;

	public ButtonMenuHightlight ButtonExpedition;

	public ButtonMenuHightlight ButtonLaboratory;

	public ButtonMenuHightlight ButtonPackageStart;

	public ButtonMenuHightlight ButtonPackageGold;

	public ButtonMenuHightlight ButtonPackagex2;

	public GameObject ExpeditionCompleteIcon;

	[HideInInspector]
	public List<Action> OnUpdateIdleCash = new List<Action>();

	[HideInInspector]
	public MineModel MineModel;

	[HideInInspector]
	public int MineIndex;

	[HideInInspector]
	public string MineResourceName;

	[HideInInspector]
	public List<string> MineResourceSprite;

	private int currentPrestigeAvailableMineOrder;

	private int currentTestAdLevel;

	public override void Awake()
	{
		base.Awake();
		MainCamera.orthographicSize = CONST.SCREEN_GRAPHIC_HEIGHT / CONST.PIXEL_PER_UNIT / 2f * CONST.GRAPHIC_ASPECT / CONST.SCREEN_ASPECT;
		MineIndex = DataManager.Instance.CurrentMineSavegame.MineIndex;
		MineModel = BaseController.GameController.MineModels[DataManager.Instance.CurrentMineSavegame.MineOrder];
		MineResourceName = DATA_RESOURCES.IMAGE.MINES_NAME[(int)base.CurrentContinent][MineIndex];
		MineResourceSprite = DATA_RESOURCES.IMAGE.GAMEPLAY_RESOURCE_LIST(MineResourceName);
		MineNameText.text = DATA_TEXT.MINES.CONTINENT_MINES[(int)base.CurrentContinent][MineIndex] + " Mine";
		PrestigeCountText.text = ((DataManager.Instance.CurrentMineSavegame.PrestigeCount != 0) ? DataManager.Instance.CurrentMineSavegame.PrestigeCount.ToString() : string.Empty);
		LayoutRebuilder.ForceRebuildLayoutImmediate(ButtonPrestigeGroup.GetComponent<RectTransform>());
		ButtonPrestigeGroup.transform.parent.gameObject.SetActive(DataManager.Instance.SavegameData.CurrentMineUnlocked > 0);
		BaseController.GameController.MineController = this;
		CashTypeIcon.text = base.CURRENT_CASH_SPRITE;
		TextCashType.text = DATA_TEXT.CASH.LIST[(int)base.CurrentCoinType].ToUpper();
		TextIdleCashType.text = "IDLE " + TextCashType.text;
		ScrollCamera = MainCamera.GetComponent<ScrollCamera>();
		BaseController.GameController.OnMineCashChangeCallback.Clear();
		BaseController.GameController.OnMineSuperCashChangeCallback.Clear();
		BaseController.GameController.OnMineChestChangeCallback.Clear();
		BaseController.GameController.AudioController.PlayLoop((UnityEngine.Random.Range(0, 2) != 0) ? "Audios/Music/background" : "Audios/Music/background2");
		TextIdleCash.transform.parent.gameObject.SetActive(value: false);
		TextSuperCash.transform.parent.gameObject.SetActive(value: false);
		ButtonControllCamera.SetActive(value: false);
		ButtonBoost.gameObject.SetActive(value: false);
		ButtonWorldmap.gameObject.SetActive(value: false);
		ButtonMine.gameObject.SetActive(value: false);
		ButtonSuperShop.gameObject.SetActive(value: false);
		ButtonShop.gameObject.SetActive(value: false);
		ButtonSetting.gameObject.SetActive(value: false);
		ButtonFriends.gameObject.SetActive(value: false);
		ButtonDailyReward.gameObject.SetActive(value: false);
		ButtonPrestigeMine.gameObject.SetActive(value: false);
		ButtonExpedition.gameObject.SetActive(value: false);
		ButtonLaboratory.gameObject.SetActive(value: false);
		ButtonPackageStart.gameObject.SetActive(value: false);
		ButtonPackageGold.gameObject.SetActive(value: false);
		ButtonPackagex2.gameObject.SetActive(value: false);
		ButtonTestAd.gameObject.SetActive(CONST.TEST_MODE_VIDEO_AD);
		if (DataManager.Instance.SavegameData.CurrentMineUnlocked >= 1 || DataManager.Instance.CurrentMineSavegame.CorridorLevel.Count >= 5)
		{
			if (UnityEngine.Random.Range(0f, 1f) < 0.5f)
			{
				ButtonLaboratory.gameObject.SetActive(DataManager.Instance.SavegameData.UnlockedLaboratory);
			}
			if (UnityEngine.Random.Range(0f, 1f) < 0.5f)
			{
				ButtonPackagex2.gameObject.SetActive(!DataManager.Instance.SavegameData.PurchasedIAPPackage.Contains("x2.cashincome"));
				if (!ButtonPackagex2.gameObject.activeSelf)
				{
					ButtonPackageStart.gameObject.SetActive(!DataManager.Instance.SavegameData.PurchasedIAPPackage.Contains("package.starter1"));
				}
				if (!ButtonPackagex2.gameObject.activeSelf && !ButtonPackageStart.gameObject.activeSelf)
				{
					ButtonPackageGold.gameObject.SetActive(!DataManager.Instance.SavegameData.PurchasedIAPPackage.Contains("package.gold1"));
				}
			}
		}
		ButtonBoost.onClick.AddListener(OnClickButtonBoost);
		ButtonMine.onClick.AddListener(OnClickButtonMine);
		ButtonWorldmap.onClick.AddListener(OnClickButtonWorldMap);
		ButtonCameraUp.onClick.AddListener(OnClickCameraUp);
		ButtonCameraDown.onClick.AddListener(OnClickCameraDown);
		ButtonSetting.onClick.AddListener(OnClickSetting);
		ButtonFriends.onClick.AddListener(OnClickFriends);
		ButtonShop.onClick.AddListener(OnClickShop);
		ButtonSuperShop.onClick.AddListener(OnClickSuperShop);
		ButtonTestAd.onClick.AddListener(OnClickTestAd);
		TextSuperCash.transform.parent.GetComponent<Button>().onClick.AddListener(OnClickSuperShop);
		ButtonPrestigeGroup.onClick.AddListener(base.OpenPrestigeDialog);
		ButtonDailyReward.onClick.AddListener(OnClickDailyReward);
		ButtonPrestigeMine.onClick.AddListener(OnClickPrestigeMine);
		ButtonExpedition.onClick.AddListener(OnClickShowExpedition);
		ButtonLaboratory.onClick.AddListener(OnClickShowResearchShop);
		ButtonPackagex2.onClick.AddListener(delegate
		{
			OnClickIAPPackage("x2.cashincome");
		});
		ButtonPackageStart.onClick.AddListener(delegate
		{
			OnClickIAPPackage("package.starter1");
		});
		ButtonPackageGold.onClick.AddListener(delegate
		{
			OnClickIAPPackage("package.gold1");
		});
		OnBuyNewShaft();
		UpdateIdleCash();
	}

	public override void Start()
	{
		base.Start();
		InitPosition();
		if (DataManager.Instance.CurrentMineSavegame.ElevatorLevel > 0)
		{
			ElevatorController.CheckShowElevator();
		}
		if (DataManager.Instance.CurrentMineSavegame.GroundLevel > 0)
		{
			GroundController.CheckShowWorker();
		}
		BaseController.GameController.OnMineCashChangeCallback.Add(OnCashChange);
		BaseController.GameController.OnMineSuperCashChangeCallback.Add(OnSuperCashChange);
		OnCashChange();
		OnSuperCashChange();
		ShowIdleCashGain();
		BaseController.GameController.FacebookHelper.FriendIcon.UpdateUI();
	}

	public override void Update()
	{
		base.Update();
		ScrollCamera.IsActive = (DataManager.Instance.SavegameData.CurrentMineUnlocked != 0 || DataManager.Instance.CurrentMineSavegame.CorridorManagerOrder != 0);
		CheckCameraPosition();
		TestAddCash();
		UpdateCheckButtonList();
	}

	private void UpdateCheckButtonList()
	{
		bool flag = DataManager.Instance.SavegameData.CurrentMineUnlocked >= 1 || CorridorController.NumberActiveCorridor >= 5;
		ButtonDailyReward.gameObject.SetActive(flag && base.DailyRewardAvailable);
		currentPrestigeAvailableMineOrder = -1;
		foreach (MineSavegame mine in DataManager.Instance.SavegameData.Mines)
		{
			if (mine.PrestigeCount < DataManager.Instance.MineFactorsEntityParams[mine.MineIndex].Count - 1 && CheckEnoughCash(DataManager.Instance.MineFactorsEntityParams[mine.MineIndex][mine.PrestigeCount + 1].Cost, (ContinentType)mine.ContinentIndex))
			{
				currentPrestigeAvailableMineOrder = mine.MineOrder;
				break;
			}
		}
		ButtonPrestigeMine.gameObject.SetActive(flag && currentPrestigeAvailableMineOrder != -1);
		ButtonExpedition.gameObject.SetActive(flag && DataManager.Instance.SavegameData.UnlockedExpedition && (DataManager.Instance.SavegameData.CurrentExpedition == null || BaseController.GameController.ExpeditionController.ExpeditionState == ExpeditionState.Completed));
		ExpeditionCompleteIcon.SetActive(ButtonExpedition.gameObject.activeSelf && BaseController.GameController.ExpeditionController.ExpeditionState == ExpeditionState.Completed);
		LayoutRebuilder.ForceRebuildLayoutImmediate(ButtonDailyReward.transform.parent.GetComponent<RectTransform>());
	}

	private void OnClickDailyReward()
	{
		BaseController.GameController.DialogController.DialogDailyReward.OnShow();
	}

	private void OnClickIAPPackage(string package)
	{
		ButtonPackagex2.gameObject.SetActive(value: false);
		ButtonPackageStart.gameObject.SetActive(value: false);
		ButtonPackageGold.gameObject.SetActive(value: false);
		BaseController.GameController.DialogController.DialogIAPContent.OnShow(package);
	}

	private void OnClickPrestigeMine()
	{
		if (currentPrestigeAvailableMineOrder != -1)
		{
			OpenPrestigeDialog(currentPrestigeAvailableMineOrder);
		}
	}

	private void OnClickShowExpedition()
	{
		BaseController.GameController.DialogController.Expedition.OnShow();
	}

	private void OnClickShowResearchShop()
	{
		ButtonLaboratory.gameObject.SetActive(value: false);
		BaseController.GameController.DialogController.ResearchLaboratory.OnShow();
		BaseController.GameController.DialogController.DialogSkillShop.OnShow();
	}

	private void TestAddCash()
	{
		if (TextTestAddCash != null && TextTestAddCash.text.Contains("\n"))
		{
			string text = TextTestAddCash.text;
			TextTestAddCash.text = string.Empty;
			double.TryParse(text, out double result);
			AddCash(result);
		}
		if (TextTestAddSuperCash != null && TextTestAddSuperCash.text.Contains("\n"))
		{
			string text2 = TextTestAddSuperCash.text;
			TextTestAddSuperCash.text = string.Empty;
			double.TryParse(text2, out double result2);
			AddSuperCash(result2);
		}
		if (TextTestAddItem != null && TextTestAddItem.text.Contains("\n"))
		{
			string text3 = TextTestAddItem.text;
			TextTestAddItem.text = string.Empty;
			AddItemFromName(text3);
		}
		if (TextTestAddSkillPoint != null && TextTestAddSkillPoint.text.Contains("\n"))
		{
			string text4 = TextTestAddSkillPoint.text;
			TextTestAddSkillPoint.text = string.Empty;
			AddSkillPointFromName(text4);
		}
		if (TextTestAddChest != null && TextTestAddChest.text.Contains("\n"))
		{
			string text5 = TextTestAddChest.text;
			TextTestAddChest.text = string.Empty;
			AddChestFromName(text5);
		}
	}

	public void AddChestFromName(string text)
	{
		ChestType chestType;
		if (text.Contains("c1"))
		{
			chestType = ChestType.Normal;
		}
		else if (text.Contains("c2"))
		{
			chestType = ChestType.Rare;
		}
		else if (text.Contains("c3"))
		{
			chestType = ChestType.Epic;
		}
		else
		{
			if (!text.Contains("c4"))
			{
				return;
			}
			chestType = ChestType.Legendary;
		}
		int.TryParse(text.Split('c')[0].Trim(), out int result);
		if (result != 0)
		{
			AddChest(chestType, result);
		}
	}

	public void AddSkillPointFromName(string text)
	{
		ContinentType continentType;
		if (text.Contains("s1"))
		{
			continentType = ContinentType.Grass;
		}
		else if (text.Contains("s2"))
		{
			continentType = ContinentType.Sand;
		}
		else
		{
			if (!text.Contains("s3"))
			{
				return;
			}
			continentType = ContinentType.Sakura;
		}
		text = text.Split('s')[0].Trim();
		int.TryParse(text, out int result);
		if (result != 0)
		{
			AddSkillPoint(result, continentType);
		}
	}

	public void AddItemFromName(string name)
	{
        if (name.Contains(" "))
        {
            string[] array = name.Split(' ');
            double.TryParse(array[0].Replace("x", string.Empty), out double result);
            IEnumerable<int> enumerable = from e in Enum.GetValues(typeof(ItemBoostDuration)).Cast<ItemBoostDuration>().ToList()
                                          select (int)e;
            foreach (int item in enumerable)
            {
                if (array[1].Trim() == ((double)item).FormatTimeString().Trim())
                {
                    AddItem(DataManager.Instance.BoostItemDictionary[(ItemBoostMultiple)result][(ItemBoostDuration)item].ItemID);
                    break;
                }
            }
        }
        else
		{
			foreach (ItemsEntity.Param param in DataManager.Instance.ItemsEntity.Params)
			{
				if (param.ItemType == 2 && name.Trim() == param.InstantCashTime.FormatTimeString().Trim())
				{
					AddItem(param.ItemID);
					break;
				}
			}
		}
	}

	private void CheckCameraPosition()
	{
		if (ButtonControllCamera.activeSelf)
		{
			GameObject gameObject = ButtonCameraUp.gameObject;
			Vector3 position = MainCamera.transform.position;
			gameObject.SetActive(position.y < -5f);
			GameObject gameObject2 = ButtonCameraDown.gameObject;
			Vector3 position2 = MainCamera.transform.position;
			gameObject2.SetActive(position2.y > GetPositionAtTier(Math.Max(CorridorController.NumberActiveCorridor, 1)) + 2f);
		}
	}

	public void OnBuyNewShaft()
	{
		if (DataManager.Instance.SavegameData.CurrentMineUnlocked != 0 || CorridorController.NumberActiveCorridor >= 1)
		{
			ButtonSetting.gameObject.SetActive(value: true);
			ButtonFriends.gameObject.SetActive(value: true);
		}
		if (DataManager.Instance.SavegameData.CurrentMineUnlocked != 0 || CorridorController.NumberActiveCorridor >= 2)
		{
			ButtonMine.gameObject.SetActive(value: true);
			ButtonShop.gameObject.SetActive(value: true);
			ButtonSuperShop.gameObject.SetActive(value: true);
			BaseController.GameController.TutorialController.StartFlashEff(ButtonMine);
			BaseController.GameController.TutorialController.StartFlashEff(ButtonShop);
		}
		if (DataManager.Instance.SavegameData.CurrentMineUnlocked != 0 || CorridorController.NumberActiveCorridor >= 3)
		{
			ButtonWorldmap.gameObject.SetActive(value: true);
			BaseController.GameController.TutorialController.StartFlashEff(ButtonWorldmap);
		}
		if (DataManager.Instance.SavegameData.CurrentMineUnlocked != 0 || CorridorController.NumberActiveCorridor >= 4)
		{
			ButtonControllCamera.SetActive(value: true);
			ButtonBoost.gameObject.SetActive(value: true);
		}
		if (!DataManager.Instance.SavegameData.UnlockedWorkshop && (DataManager.Instance.SavegameData.CurrentMineUnlocked != 0 || CorridorController.NumberActiveCorridor >= 10))
		{
			DataManager.Instance.SavegameData.UnlockedWorkshop = true;
			//BaseController.GameController.AnalyticController.LogEvent("unlock_workshop");
			AddChest(ChestType.Normal, 1);
		}
		if (!DataManager.Instance.SavegameData.UnlockedExpedition && DataManager.Instance.SavegameData.CurrentMineUnlocked != 0)
		{
			DataManager.Instance.SavegameData.UnlockedExpedition = true;
			//BaseController.GameController.AnalyticController.LogEvent("unlock_expedition");
		}
		if (DataManager.Instance.SavegameData.CurrentMineUnlocked != 0 || CorridorController.NumberActiveCorridor >= 5)
		{
		}
		if (DataManager.Instance.SavegameData.CurrentMine == DataManager.Instance.SavegameData.CurrentMineUnlocked && DataManager.Instance.SavegameData.CurrentShaftUnlocked == CorridorController.NumberActiveCorridor)
		{
			BaseController.GameController.UpdateMyInfo();
		}
	}

	public void OnClickTestAd()
	{
		int[] array = new int[4]
		{
			1,
			100,
			500,
			700
		};
		currentTestAdLevel = (currentTestAdLevel + 1) % array.Length;
		int level = array[currentTestAdLevel];
		CorridorController.CorridorLevelControllers[0].SetLevel(level);
		CorridorController.CorridorLevelControllers[0].CorridorManagerController.gameObject.SetActive(currentTestAdLevel != 0);
		if (currentTestAdLevel > 0)
		{
			CorridorController.CorridorLevelControllers[0].CorridorManagerController.SetActiveManager(isEmpty: false);
			CorridorController.CorridorLevelControllers[0].CorridorManagerController.SetManagerParam(DataManager.Instance.ManagerByAreaParams[1][currentTestAdLevel - 1]);
		}
	}

	public void OnClickBuyNewShaft()
	{
		//BaseController.GameController.AnalyticController.LogEvent("buy_new_shaft", "tier", DataManager.Instance.SavegameData.CurrentMine * 100 + CorridorController.NumberActiveCorridor);
		if (DataManager.Instance.SavegameData.CurrentMineUnlocked == 0 && CorridorController.NumberActiveCorridor == 1)
		{
			//BaseController.GameController.AnalyticController.LogEvent("buy_new_shaft_1");
		}
		if (DataManager.Instance.SavegameData.CurrentMineUnlocked == 0 && CorridorController.NumberActiveCorridor == 2)
		{
			//BaseController.GameController.AnalyticController.LogEvent("buy_new_shaft_2");
		}
		if (DataManager.Instance.SavegameData.CurrentMineUnlocked == 0 && CorridorController.NumberActiveCorridor == 5)
		{
			//BaseController.GameController.AnalyticController.LogEvent("buy_new_shaft_5");
		}
		if (DataManager.Instance.SavegameData.CurrentMineUnlocked == 0 && CorridorController.NumberActiveCorridor == 2)
		{
			AddItem(DataManager.Instance.BoostItemDictionary[ItemBoostMultiple.x2][ItemBoostDuration.Hour].ItemID);
			BaseController.GameController.TutorialController.CreateTutorial(TutorialType.ButtonShop, ButtonShop.gameObject, isUI: true);
		}
		if (DataManager.Instance.SavegameData.CurrentMineUnlocked == 0 && CorridorController.NumberActiveCorridor == 4)
		{
			BaseController.GameController.TutorialController.CreateTutorial(TutorialType.ButtonBoost, ButtonBoost.gameObject, isUI: true);
		}
		if (DataManager.Instance.SavegameData.CurrentMine == DataManager.Instance.SavegameData.CurrentMineUnlocked)
		{
			DataManager.Instance.SavegameData.CurrentShaftUnlocked = CorridorController.NumberActiveCorridor;
			BaseController.GameController.UpdateMyInfo();
		}
	}

	public void OnClickSuperShop()
	{
		BaseController.GameController.DialogController.DialogShop.OnHide();
		BaseController.GameController.DialogController.DialogSuperShop.OnShow();
	}

	public void OnClickShop()
	{
		BaseController.GameController.TutorialController.RemoveTutorial(TutorialType.ButtonShop);
		BaseController.GameController.DialogController.DialogSuperShop.OnHide();
		BaseController.GameController.DialogController.DialogShop.OnShow();
	}

	private void OnClickButtonBoost()
	{
		BaseController.GameController.TutorialController.RemoveTutorial(TutorialType.ButtonBoost);
		BaseController.GameController.DialogController.Dialogx2AdBoost.OnShow();
	}

	private void OnClickSetting()
	{
		BaseController.GameController.DialogController.DialogSetting.OnShow();
	}

	private void OnClickFriends()
	{
		BaseController.GameController.DialogController.DialogFriend.OnShow();
	}

	private void OnClickCameraUp()
	{
		ScrollCamera.moveToBegin();
	}

	private void OnClickCameraDown()
	{
		ScrollCamera.scrollTo(Math.Max(GetPositionAtTier(CorridorController.NumberActiveCorridor) - 6f, ScrollCamera.yLimit));
	}

	private void OnClickButtonWorldMap()
	{
		BaseController.GameController.TutorialController.StopFlashEff(base.MineController.ButtonWorldmap);
		BaseController.GameController.TutorialController.RemoveAllTutorial();
		SceneManager.LoadScene("WorldMap", LoadSceneMode.Additive);
	}

	private void OnClickButtonMine()
	{
		BaseController.GameController.TutorialController.StopFlashEff(base.MineController.ButtonMine);
		BaseController.GameController.DialogController.DialogMineOverview.OnShow();
	}

	private void OnCashChange()
	{
		base.MineController.TextCash.text = DataManager.Instance.Cash.MinifyFormat();
	}

	private void OnSuperCashChange()
	{
		if (!TextSuperCash.gameObject.activeInHierarchy && (TextIdleCash.gameObject.activeInHierarchy || MathUtils.CompareDoubleBiggerThanZero(DataManager.Instance.SuperCash)))
		{
			TextSuperCash.transform.parent.gameObject.SetActive(value: true);
		}
		base.MineController.TextSuperCash.text = DataManager.Instance.SuperCash.MinifyFormat();
	}

	private void InitPosition()
	{
		InitSurfacePosition();
		ScrollCamera.yLimit = GetPositionAtTier(UndergroundController.TierBackgroundControllers.Count);
	}

	private float GetPositionAtTier(int tier)
	{
		Vector3 position = UndergroundController.TierBackgroundControllers[tier - 1].transform.position;
		return position.y + CONST.SCREEN_HEIGHT / 2f;
	}

	private void InitSurfacePosition()
	{
		TopDown.localPosition = new Vector3(0f, CONST.SCREEN_HEIGHT / 2f - SurfaceController.Background.sprite.rect.height / 2f / CONST.PIXEL_PER_UNIT, 0f);
		Vector3 localPosition = SurfaceController.Background.transform.localPosition;
		float num = localPosition.y - SurfaceController.Background.sprite.rect.height / CONST.PIXEL_PER_UNIT / 2f;
		Transform transform = SurfaceController.ElevatorHouse.transform;
		float x = (0f - CONST.SCREEN_WIDTH) / 2f + SurfaceController.ElevatorHouse.sprite.rect.width / CONST.PIXEL_PER_UNIT / 2f;
		float y = num + SurfaceController.ElevatorHouse.sprite.rect.height / CONST.PIXEL_PER_UNIT / 2f;
		Vector3 localPosition2 = SurfaceController.ElevatorHouse.transform.localPosition;
		transform.localPosition = new Vector3(x, y, localPosition2.z);
		Transform transform2 = SurfaceController.WareHouse.transform;
		float x2 = CONST.SCREEN_WIDTH / 2f - SurfaceController.WareHouse.sprite.rect.width / CONST.PIXEL_PER_UNIT / 2f;
		float y2 = num + SurfaceController.WareHouse.sprite.rect.height / CONST.PIXEL_PER_UNIT / 2f;
		Vector3 localPosition3 = SurfaceController.WareHouse.transform.localPosition;
		transform2.localPosition = new Vector3(x2, y2, localPosition3.z);
	}

	public void UpdateIdleCash()
	{
		MineModel.UpdateIdleCash();
		if (DataManager.Instance.SavegameData.CurrentMineUnlocked > 0 || MathUtils.CompareDoubleBiggerThanZero(DataManager.Instance.IdleCash))
		{
			TextIdleCash.transform.parent.gameObject.SetActive(value: true);
			BaseController.GameController.LocalNotificationController.RegisterForNotifications();
			if (DataManager.Instance.SavegameData.CurrentMineUnlocked == 0 && CorridorController.CorridorLevelControllers.Count > 0)
			{
				CorridorController.CorridorLevelControllers[0].CompleteTutorial();
			}
		}
		TextIdleCash.text = DataManager.Instance.IdleCash.MinifyFormat() + "/s";
		foreach (Action item in OnUpdateIdleCash)
		{
			item();
		}
	}

	public bool CheckIdleTime()
	{
		if (DataManager.Instance.CurrentMineSavegame.MineLastTimeVisit == 0 || MathUtils.CompareDoubleToZero(DataManager.Instance.IdleCash))
		{
			return false;
		}
		long num = (long)TimeSpan.FromTicks(DateTime.Now.Ticks - DataManager.Instance.CurrentMineSavegame.MineLastTimeVisit).TotalSeconds;
		if (num < 60)
		{
			return false;
		}
		return true;
	}

	public void ShowIdleCashGain()
	{
		if (CheckIdleTime())
		{
			long num = (long)Math.Min(TimeSpan.FromTicks(DateTime.Now.Ticks - DataManager.Instance.CurrentMineSavegame.MineLastTimeVisit).TotalSeconds, 2592000.0);
			MonoBehaviour.print("You offline for: " + num);
			double cash = (double)num * DataManager.Instance.IdleCash;
			BaseController.GameController.BoostController.CalculateIdleCashBonus(checkExpireItemOnly: false);
			BaseController.GameController.DialogController.DialogIdleCash.OnShow(num, cash, DataManager.Instance.CurrentMineSavegame.IdleBonusCashGain);
			DataManager.Instance.CurrentMineSavegame.IdleBonusCashGain = 0.0;
			DataManager.Instance.CurrentMineSavegame.MineLastTimeVisit = DateTime.Now.Ticks;
		}
	}

	public GameObject CreateMineGameObject()
	{
		GameObject gameObject = new GameObject("Mine Material");
		gameObject.AddComponent<SpriteRenderer>();
		int num = UnityEngine.Random.Range(0, MineResourceSprite.Count);
		gameObject.GetComponent<SpriteRenderer>().sprite = BaseController.LoadSprite(MineResourceSprite[num]);
		gameObject.GetComponent<SpriteRenderer>().sortingOrder = num;
		return gameObject;
	}
}
