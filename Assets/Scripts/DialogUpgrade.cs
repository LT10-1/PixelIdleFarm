using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogUpgrade : BaseDialog
{
	public WorkerCard WorkerCard;

	public Image WorkerIcon;

	public GameObject SuperCoinText;

	public TMP_Text TextSuperCoin;

	public TMP_Text TextNextBoost;

	public Image ImageChoose;

	public Button[] UpgradeButton;

	public Transform ParamContent;

	public Image LevelUpFillBar;

	public Button ButtonLevelUp;

	public CoinController UpgradeCostText;

	public TMP_Text LevelUpText;

	public TMP_Text ButtonLevelUpText;

	[HideInInspector]
	public ManagerArea ManagerArea;

	[HideInInspector]
	public UpgradeFactor UpgradeFactor;

	[HideInInspector]
	public int CurrentCorridorTier;

	private bool ShowTutorial;

	private Vector3 _imageChooseInitPosition = Vector3.zero;

	[HideInInspector]
	public List<DialogUpgradeItem> DialogUpgradeItems;

	public string DialogTitle
	{
		get
		{
			switch (ManagerArea)
			{
			case ManagerArea.Corridor:
				return $"Mine Shaft {CorridorLevelController.CorridorModel.Tier} Level {CorridorLevelController.CorridorModel.Level}";
			case ManagerArea.Elevator:
				return $"Elevator Level {ElevatorController.ElevatorModel.Level}";
			case ManagerArea.Ground:
				return $"Warehouse Level {GroundController.GroundModel.Level}";
			default:
				return string.Empty;
			}
		}
	}

	public CorridorLevelController CorridorLevelController
	{
		get
		{
			if (CorridorController.CorridorLevelControllers.Count < CurrentCorridorTier)
			{
				return null;
			}
			return CorridorController.CorridorLevelControllers[CurrentCorridorTier - 1];
		}
	}

	public CorridorController CorridorController => BaseController.GameController.MineController.CorridorController;

	public ElevatorController ElevatorController => BaseController.GameController.MineController.ElevatorController;

	public GroundController GroundController => BaseController.GameController.MineController.GroundController;

	public UpgradeType[] UpgradeTypes
	{
		get
		{
			switch (ManagerArea)
			{
			case ManagerArea.Corridor:
				return DATA_CONST.CORRIDOR_UPGRADE_TYPE;
			case ManagerArea.Elevator:
				return DATA_CONST.ELEVATOR_UPGRADE_TYPE;
			case ManagerArea.Ground:
				return DATA_CONST.GROUND_UPGRADE_TYPE;
			default:
				return new UpgradeType[0];
			}
		}
	}

	public bool IsMaxLevel
	{
		get
		{
			switch (ManagerArea)
			{
			case ManagerArea.Corridor:
				return CorridorLevelController.CorridorModel.IsMaxLevel;
			case ManagerArea.Elevator:
				return ElevatorController.ElevatorModel.IsMaxLevel;
			case ManagerArea.Ground:
				return GroundController.GroundModel.IsMaxLevel;
			default:
				return false;
			}
		}
	}

	public bool CheckNullModel
	{
		get
		{
			switch (ManagerArea)
			{
			case ManagerArea.Corridor:
				return CorridorLevelController == null || CorridorLevelController.CorridorModel == null;
			case ManagerArea.Elevator:
				return ElevatorController.ElevatorModel == null;
			case ManagerArea.Ground:
				return GroundController.GroundModel == null;
			default:
				return true;
			}
		}
	}

	public Vector3 ImageChooseInitPosition
	{
		get
		{
			if (_imageChooseInitPosition == Vector3.zero)
			{
				_imageChooseInitPosition = ImageChoose.transform.localPosition;
			}
			return _imageChooseInitPosition;
		}
	}

	public int NumberLevelToUpgrade
	{
		get
		{
			switch (UpgradeFactor)
			{
			case UpgradeFactor.X1:
				return Math.Min(1, NumberLevelToMax);
			case UpgradeFactor.X10:
				return Math.Min(10, NumberLevelToMax);
			case UpgradeFactor.X50:
				return Math.Min(50, NumberLevelToMax);
			case UpgradeFactor.Max:
			{
				int val = 1;
				switch (ManagerArea)
				{
				case ManagerArea.Corridor:
					val = CorridorLevelController.CorridorModel.GetMaxAffordableNumbersOfLevelsToUpgrade(DataManager.Instance.Cash);
					break;
				case ManagerArea.Elevator:
					val = ElevatorController.ElevatorModel.GetMaxAffordableNumbersOfLevelsToUpgrade(DataManager.Instance.Cash);
					break;
				case ManagerArea.Ground:
					val = GroundController.GroundModel.GetMaxAffordableNumbersOfLevelsToUpgrade(DataManager.Instance.Cash);
					break;
				}
				return Math.Max(val, 1);
			}
			default:
				return 1;
			}
		}
	}

	public int NumberLevelToMax
	{
		get
		{
			switch (ManagerArea)
			{
			case ManagerArea.Corridor:
				return CorridorLevelController.CorridorModel.MaxLevel - CorridorLevelController.CorridorModel.Level;
			case ManagerArea.Elevator:
				return ElevatorController.ElevatorModel.MaxLevel - ElevatorController.ElevatorModel.Level;
			case ManagerArea.Ground:
				return GroundController.GroundModel.MaxLevel - GroundController.GroundModel.Level;
			default:
				return 0;
			}
		}
	}

	public double UpgradeCost
	{
		get
		{
			switch (ManagerArea)
			{
			case ManagerArea.Corridor:
				return CorridorLevelController.CorridorModel.CorridorUpgradeCostStat.ValueAtNextLevel(NumberLevelToUpgrade);
			case ManagerArea.Elevator:
				return ElevatorController.ElevatorModel.UpgradeCostStat.ValueAtNextLevel(NumberLevelToUpgrade);
			case ManagerArea.Ground:
				return GroundController.GroundModel.UpgradeCostStat.ValueAtNextLevel(NumberLevelToUpgrade);
			default:
				return 0.0;
			}
		}
	}

	public override void Start()
	{
		base.Start();
		for (int i = 0; i < UpgradeButton.Length; i++)
		{
			CreateUpgradeButton((UpgradeFactor)i);
		}
		CreateUpgradeItem();
		BaseController.GameController.OnGlobalCashChangeCallback.Add(OnCashChangeCallback);
		OnCashChangeCallback();
		ButtonLevelUp.onClick.AddListener(OnClickLevelUp);
		WorkerIcon.sprite = BaseController.LoadSprite(DATA_RESOURCES.IMAGE.AVATAR_AREA[(int)(ManagerArea - 1)]);
		WorkerCard.button.onClick.AddListener(delegate
		{
			BaseController.GameController.DialogController.WorkShop.OnShow();
		});
		BaseController.GameController.OnWorkshopChangeCallback.Add(OnWorkshopChangeCallback);
		switch (ManagerArea)
		{
		}
	}

	public override void Update()
	{
		base.Update();
		UpdateParam();
	}

	public override void OnShow()
	{
		base.OnShow();
		OnWorkshopChangeCallback();
		UpgradeCostText.SetCoinType(base.CurrentCoinType);
		if (DialogUpgradeItems.Count > 0)
		{
			UpdateData();
		}
		ShowTutorial = (DataManager.Instance.SavegameData.CurrentMineUnlocked == 0 && ManagerArea == ManagerArea.Corridor && CorridorLevelController.CorridorModel.Tier == 1 && CorridorLevelController.CorridorModel.Level == 1 && CheckEnoughCash(UpgradeCost));
		if (ShowTutorial)
		{
			BaseController.GameController.TutorialController.CreateTutorial(TutorialType.DialogLevelUp, ButtonLevelUp.gameObject, isUI: true, 1f, useSpotlight: true, 1.3f);
		}
	}

	public override void OnHide()
	{
		base.OnHide();
		BaseController.GameController.TutorialController.RemoveTutorial(TutorialType.DialogLevelUp);
		if (ShowTutorial)
		{
			ShowTutorial = false;
			BaseController.GameController.TutorialController.CreateTutorial(TutorialType.ClickWorkerAgain, ButtonLevelUp.gameObject, isUI: true, 1f, useSpotlight: true);
			BaseController.GameController.TutorialController.RemoveTutorial(TutorialType.ClickWorkerAgain);
		}
	}

	public void OnWorkshopChangeCallback()
	{
		WorkerCard.gameObject.SetActive(DataManager.Instance.SavegameData.UnlockedWorkshop);
		WorkerIcon.transform.parent.gameObject.SetActive(!WorkerCard.gameObject.activeSelf);
		WorkerCard.SetId(DataManager.Instance.SavegameData.CurrentActiveWorkerCard(ManagerArea));
	}

	public void OnClickLevelUp()
	{
		int numberLevelToUpgrade = NumberLevelToUpgrade;
		BaseController.GameController.TutorialController.RemoveTutorial(TutorialType.DialogLevelUp);
		if (UseCash(UpgradeCost))
		{
			BaseController.GameController.AudioController.PlayOneShot("Audios/Effect/levelup");
			switch (ManagerArea)
			{
			case ManagerArea.Corridor:
				CorridorLevelController.LevelUp(numberLevelToUpgrade);
				//BaseController.GameController.AnalyticController.LogEvent("level_up_mine_shaft", "level", CorridorLevelController.CorridorModel.Level);
				break;
			case ManagerArea.Elevator:
				ElevatorController.LevelUp(numberLevelToUpgrade);
				//BaseController.GameController.AnalyticController.LogEvent("level_up_elevator", "level", ElevatorController.ElevatorModel.Level);
				break;
			case ManagerArea.Ground:
				GroundController.LevelUp(numberLevelToUpgrade);
				//BaseController.GameController.AnalyticController.LogEvent("level_up_warehouse", "level", GroundController.GroundModel.Level);
				break;
			}
			UpdateData();
		}
	}

	public void UpdateTitle()
	{
		base.BackgroundDialog.SetTitle(DialogTitle);
	}

	public void CreateUpgradeItem()
	{
		DialogUpgradeItems = new List<DialogUpgradeItem>();
		for (int i = 0; i < UpgradeTypes.Length; i++)
		{
			DialogUpgradeItem component = InstantiatePrefab("Prefabs/Dialog/Component/DialogUpgradeItem").GetComponent<DialogUpgradeItem>();
			component.DialogUpgrade = this;
			component.SetUpgradeType(UpgradeTypes[i]);
			component.transform.SetParent(ParamContent, worldPositionStays: false);
			DialogUpgradeItems.Add(component);
		}
		UpdateData();
	}

	public void CreateUpgradeButton(UpgradeFactor upgradeFactor)
	{
		Button button = UpgradeButton[(int)upgradeFactor];
		button.onClick.AddListener(delegate
		{
			ImageChoose.transform.localPosition = ImageChooseInitPosition + 83 * (int)upgradeFactor * Vector3.right;
			UpgradeFactor = upgradeFactor;
			UpdateData();
		});
	}

	public void OnCashChangeCallback()
	{
		if (!CheckNullModel && !IsMaxLevel)
		{
			bool flag = CheckEnoughCash(UpgradeCost);
			UpgradeCostText.SetMoneyColor(flag);
			ButtonLevelUp.GetComponent<Image>().color = ((!flag) ? COLOR.COLOR_BUTTON_DISABLE : Color.white);
		}
	}

	public void UpdateData()
	{
		UpdateTitle();
		UpdateCashGroup();
		UpdateParam();
		OnCashChangeCallback();
	}

	public void UpdateCashGroup()
	{
		SuperCoinText.gameObject.SetActive(!IsMaxLevel);
		UpgradeCostText.gameObject.SetActive(!IsMaxLevel);
		ButtonLevelUp.enabled = !IsMaxLevel;
		ButtonLevelUpText.text = ((!IsMaxLevel) ? "Upgrade" : "MAX");
		if (IsMaxLevel)
		{
			TextNextBoost.text = "Max level reached!";
			LevelUpFillBar.fillAmount = 1f;
			ButtonLevelUp.GetComponent<Image>().color = COLOR.COLOR_BUTTON_DISABLE;
			return;
		}
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		double num4 = 0.0;
		switch (ManagerArea)
		{
		case ManagerArea.Corridor:
			num = CorridorLevelController.CorridorModel.NextBigUpgradeLevel;
			num2 = CorridorLevelController.CorridorModel.LastBigUpgradeAtLevel;
			num4 = CorridorLevelController.CorridorModel.NextSuperCashGain;
			num3 = CorridorLevelController.CorridorModel.Level;
			break;
		case ManagerArea.Elevator:
			num = ElevatorController.ElevatorModel.NextBigUpgradeLevel;
			num2 = ElevatorController.ElevatorModel.LastBigUpgradeAtLevel;
			num4 = ElevatorController.ElevatorModel.NextSuperCashGain;
			num3 = ElevatorController.ElevatorModel.Level;
			break;
		case ManagerArea.Ground:
			num = GroundController.GroundModel.NextBigUpgradeLevel;
			num2 = GroundController.GroundModel.LastBigUpgradeAtLevel;
			num4 = GroundController.GroundModel.NextSuperCashGain;
			num3 = GroundController.GroundModel.Level;
			break;
		}
		TextNextBoost.text = $"Next boost at level {num}.";
		TextSuperCoin.text = "+" + num4;
		LevelUpFillBar.fillAmount = (float)(num3 - num2) / ((float)num - (float)num2);
	}

	public void UpdateParam()
	{
		if (DialogUpgradeItems == null || DialogUpgradeItems.Count != UpgradeTypes.Length)
		{
			return;
		}
		int num = NumberLevelToUpgrade;
		UpgradeCostText.SetMoney(UpgradeCost, minify: true, showMoney: true, string.Empty);
		if (IsMaxLevel)
		{
			num = 0;
		}
		LevelUpText.text = $"Level up x{num}";
		bool flag = false;
		for (int num2 = UpgradeTypes.Length - 1; num2 >= 0; num2--)
		{
			UpgradeType upgradeType = UpgradeTypes[num2];
			DialogUpgradeItem dialogUpgradeItem = DialogUpgradeItems[num2];
			dialogUpgradeItem.ValueGroupController.ValueGreen.gameObject.SetActive(!IsMaxLevel);
			dialogUpgradeItem.ValueGroupController.ValueWhite.CoinText.fontStyle = (IsMaxLevel ? FontStyles.Bold : FontStyles.Normal);
			bool flag2 = false;
			switch (ManagerArea)
			{
			case ManagerArea.Corridor:
				switch (upgradeType)
				{
				case UpgradeType.TotalExtraction:
					dialogUpgradeItem.ValueGroupController.ValueWhite.SetMoney(CorridorLevelController.CorridorModel.CompleteDollarPerSecondStat.Value, minify: true, showMoney: true, "/s");
					if (dialogUpgradeItem.ValueGroupController.ValueGreen.gameObject.activeSelf)
					{
						dialogUpgradeItem.ValueGroupController.ValueGreen.text = "+" + (CorridorLevelController.CorridorModel.CompleteDollarPerSecondStat.ValueAtNextLevel(num) - CorridorLevelController.CorridorModel.CompleteDollarPerSecondStat.Value).MinifyFormat();
					}
					flag2 = flag;
					break;
				case UpgradeType.Miners:
					dialogUpgradeItem.ValueGroupController.ValueWhite.SetMoney(CorridorLevelController.CorridorModel.NumberOfWorkersStat.Value, minify: true, showMoney: false, string.Empty);
					if (dialogUpgradeItem.ValueGroupController.ValueGreen.gameObject.activeSelf)
					{
						dialogUpgradeItem.ValueGroupController.ValueGreen.text = "+" + (CorridorLevelController.CorridorModel.NumberOfWorkersStat.ValueAtNextLevel(num) - CorridorLevelController.CorridorModel.NumberOfWorkersStat.Value);
					}
					flag2 = false;
					break;
				case UpgradeType.Walkingspeed:
					dialogUpgradeItem.ValueGroupController.ValueWhite.SetMoney(CorridorLevelController.CorridorModel.WalkingSpeedStat.Value, minify: true, showMoney: false, string.Empty);
					if (dialogUpgradeItem.ValueGroupController.ValueGreen.gameObject.activeSelf)
					{
						dialogUpgradeItem.ValueGroupController.ValueGreen.text = "+" + (CorridorLevelController.CorridorModel.WalkingSpeedStat.ValueAtNextLevel(num) - CorridorLevelController.CorridorModel.WalkingSpeedStat.Value).MinifyFormat();
					}
					flag2 = MathUtils.CompareDoubleBiggerThanZero(CorridorLevelController.CorridorModel.WalkingSpeedStat.BonusValue - 1.0);
					break;
				case UpgradeType.MiningSpeed:
					dialogUpgradeItem.ValueGroupController.ValueWhite.SetMoney(CorridorLevelController.CorridorModel.GainPerSecondStat.Value, minify: true, showMoney: true, "/s");
					if (dialogUpgradeItem.ValueGroupController.ValueGreen.gameObject.activeSelf)
					{
						dialogUpgradeItem.ValueGroupController.ValueGreen.text = "+" + (CorridorLevelController.CorridorModel.GainPerSecondStat.ValueAtNextLevel(num) - CorridorLevelController.CorridorModel.GainPerSecondStat.Value).MinifyFormat();
					}
					flag2 = MathUtils.CompareDoubleBiggerThanZero(CorridorLevelController.CorridorModel.GainPerSecondStat.BonusValue - 1.0);
					break;
				case UpgradeType.WorkerCapacity:
					dialogUpgradeItem.ValueGroupController.ValueWhite.SetMoney(CorridorLevelController.CorridorModel.CapacityStat.Value, minify: true, showMoney: true, string.Empty);
					if (dialogUpgradeItem.ValueGroupController.ValueGreen.gameObject.activeSelf)
					{
						dialogUpgradeItem.ValueGroupController.ValueGreen.text = "+" + (CorridorLevelController.CorridorModel.CapacityStat.ValueAtNextLevel(num) - CorridorLevelController.CorridorModel.CapacityStat.Value).MinifyFormat();
					}
					flag2 = MathUtils.CompareDoubleBiggerThanZero(CorridorLevelController.CorridorModel.CapacityStat.BonusValue - 1.0);
					break;
				}
				break;
			case ManagerArea.Elevator:
				switch (upgradeType)
				{
				case UpgradeType.TotalTransportation:
					dialogUpgradeItem.ValueGroupController.ValueWhite.SetMoney(ElevatorController.ElevatorModel.TransportingPerSecondStat.Value, minify: true, showMoney: true, "/s");
					if (dialogUpgradeItem.ValueGroupController.ValueGreen.gameObject.activeSelf)
					{
						dialogUpgradeItem.ValueGroupController.ValueGreen.text = "+" + (ElevatorController.ElevatorModel.TransportingPerSecondStat.ValueAtNextLevel(num) - ElevatorController.ElevatorModel.TransportingPerSecondStat.Value).MinifyFormat();
					}
					flag2 = flag;
					break;
				case UpgradeType.Load:
					dialogUpgradeItem.ValueGroupController.ValueWhite.SetMoney(ElevatorController.ElevatorModel.CapacityStat.Value, minify: true, showMoney: true, string.Empty);
					if (dialogUpgradeItem.ValueGroupController.ValueGreen.gameObject.activeSelf)
					{
						dialogUpgradeItem.ValueGroupController.ValueGreen.text = "+" + (ElevatorController.ElevatorModel.CapacityStat.ValueAtNextLevel(num) - ElevatorController.ElevatorModel.CapacityStat.Value).MinifyFormat();
					}
					flag2 = MathUtils.CompareDoubleBiggerThanZero(ElevatorController.ElevatorModel.CapacityStat.BonusValue - 1.0);
					break;
				case UpgradeType.MovementSpeed:
					dialogUpgradeItem.ValueGroupController.ValueWhite.SetMoney(ElevatorController.ElevatorModel.SpeedStat.Value, minify: true, showMoney: false, string.Empty);
					if (dialogUpgradeItem.ValueGroupController.ValueGreen.gameObject.activeSelf)
					{
						dialogUpgradeItem.ValueGroupController.ValueGreen.text = "+" + (ElevatorController.ElevatorModel.SpeedStat.ValueAtNextLevel(num) - ElevatorController.ElevatorModel.SpeedStat.Value).MinifyFormat();
					}
					flag2 = MathUtils.CompareDoubleBiggerThanZero(ElevatorController.ElevatorModel.SpeedStat.BonusValue - 1.0);
					break;
				case UpgradeType.LoadingSpeed:
					dialogUpgradeItem.ValueGroupController.ValueWhite.SetMoney(ElevatorController.ElevatorModel.LoadingPerSecondStat.Value, minify: true, showMoney: true, "/s");
					if (dialogUpgradeItem.ValueGroupController.ValueGreen.gameObject.activeSelf)
					{
						dialogUpgradeItem.ValueGroupController.ValueGreen.text = "+" + (ElevatorController.ElevatorModel.LoadingPerSecondStat.ValueAtNextLevel(num) - ElevatorController.ElevatorModel.LoadingPerSecondStat.Value).MinifyFormat();
					}
					flag2 = MathUtils.CompareDoubleBiggerThanZero(ElevatorController.ElevatorModel.LoadingPerSecondStat.BonusValue - 1.0);
					break;
				}
				break;
			case ManagerArea.Ground:
				switch (upgradeType)
				{
				case UpgradeType.TotalTransportation:
					dialogUpgradeItem.ValueGroupController.ValueWhite.SetMoney(GroundController.GroundModel.TransportingPerSecondStat.Value, minify: true, showMoney: true, "/s");
					if (dialogUpgradeItem.ValueGroupController.ValueGreen.gameObject.activeSelf)
					{
						dialogUpgradeItem.ValueGroupController.ValueGreen.text = (GroundController.GroundModel.TransportingPerSecondStat.ValueAtNextLevel(num) - GroundController.GroundModel.TransportingPerSecondStat.Value).MinifyFormat() + "/s";
					}
					flag2 = flag;
					break;
				case UpgradeType.Transporters:
					dialogUpgradeItem.ValueGroupController.ValueWhite.SetMoney(GroundController.GroundModel.NumberOfWorkersStat.Value, minify: true, showMoney: false, string.Empty);
					if (dialogUpgradeItem.ValueGroupController.ValueGreen.gameObject.activeSelf)
					{
						dialogUpgradeItem.ValueGroupController.ValueGreen.text = "+" + (GroundController.GroundModel.NumberOfWorkersStat.ValueAtNextLevel(num) - GroundController.GroundModel.NumberOfWorkersStat.Value);
					}
					flag2 = false;
					break;
				case UpgradeType.Loadpertrans:
					dialogUpgradeItem.ValueGroupController.ValueWhite.SetMoney(GroundController.GroundModel.CapacityStat.Value, minify: true, showMoney: true, string.Empty);
					if (dialogUpgradeItem.ValueGroupController.ValueGreen.gameObject.activeSelf)
					{
						dialogUpgradeItem.ValueGroupController.ValueGreen.text = "+" + (GroundController.GroundModel.CapacityStat.ValueAtNextLevel(num) - GroundController.GroundModel.CapacityStat.Value).MinifyFormat();
					}
					flag2 = MathUtils.CompareDoubleBiggerThanZero(GroundController.GroundModel.CapacityStat.BonusValue - 1.0);
					break;
				case UpgradeType.LoadingSpeed:
					dialogUpgradeItem.ValueGroupController.ValueWhite.SetMoney(GroundController.GroundModel.LoadingPerSecondStat.Value, minify: true, showMoney: true, "/s");
					if (dialogUpgradeItem.ValueGroupController.ValueGreen.gameObject.activeSelf)
					{
						dialogUpgradeItem.ValueGroupController.ValueGreen.text = "+" + (GroundController.GroundModel.LoadingPerSecondStat.ValueAtNextLevel(num) - GroundController.GroundModel.LoadingPerSecondStat.Value).MinifyFormat();
					}
					flag2 = MathUtils.CompareDoubleBiggerThanZero(GroundController.GroundModel.LoadingPerSecondStat.BonusValue - 1.0);
					break;
				case UpgradeType.Walkingspeed:
					dialogUpgradeItem.ValueGroupController.ValueWhite.SetMoney(GroundController.GroundModel.WalkingSpeedStat.Value, minify: true, showMoney: false, string.Empty);
					if (dialogUpgradeItem.ValueGroupController.ValueGreen.gameObject.activeSelf)
					{
						dialogUpgradeItem.ValueGroupController.ValueGreen.text = "+" + (GroundController.GroundModel.WalkingSpeedStat.ValueAtNextLevel(num) - GroundController.GroundModel.WalkingSpeedStat.Value).MinifyFormat();
					}
					flag2 = MathUtils.CompareDoubleBiggerThanZero(GroundController.GroundModel.WalkingSpeedStat.BonusValue - 1.0);
					break;
				}
				break;
			}
			flag = (flag2 || flag);
			dialogUpgradeItem.ValueGroupController.ValueWhite.SetMoneyBonusColor(flag2);
		}
	}
}
