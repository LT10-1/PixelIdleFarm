using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : BaseDialog
{
	public UIButtonController ButtonHireManager;

	public CoinController TextCash;

	public Transform AssignManager;

	public Transform FilterPanel;

	public ScrollRect ScrollRect;

	public Transform ScrollViewContent;

	[HideInInspector]
	public ManagerArea ManagerArea;

	[HideInInspector]
	public Dictionary<int, DialogManagerItem> DialogManagerItems = new Dictionary<int, DialogManagerItem>();

	[HideInInspector]
	public int CurrentCorridorTier;

	[HideInInspector]
	public DialogManagerItem CurrentAssignManagerItem;

	[HideInInspector]
	public Button[] ButtonFilter;

	[HideInInspector]
	public float OriginalButtonFilterHeight;

	[HideInInspector]
	public ManagerEffect CurrentFilter;

	private float _originalScrollViewHeight;

	public Dictionary<int, ManagerSavegame> ManagerSavegames
	{
		get
		{
			switch (ManagerArea)
			{
			case ManagerArea.Corridor:
				return DataManager.Instance.CurrentMineSavegame.CorridorManagerDictionary;
			case ManagerArea.Elevator:
				return DataManager.Instance.CurrentMineSavegame.ElevatorManagerDictionary;
			case ManagerArea.Ground:
				return DataManager.Instance.CurrentMineSavegame.GroundManagerDictionary;
			default:
				return new Dictionary<int, ManagerSavegame>();
			}
		}
	}

	public int ManagerBuyOrder
	{
		get
		{
			switch (ManagerArea)
			{
			case ManagerArea.Corridor:
				return DataManager.Instance.CurrentMineSavegame.CorridorManagerOrder;
			case ManagerArea.Elevator:
				return DataManager.Instance.CurrentMineSavegame.ElevatorManagerOrder;
			case ManagerArea.Ground:
				return DataManager.Instance.CurrentMineSavegame.GroundManagerOrder;
			default:
				return 0;
			}
		}
		set
		{
			switch (ManagerArea)
			{
			case ManagerArea.Corridor:
				DataManager.Instance.CurrentMineSavegame.CorridorManagerOrder = value;
				break;
			case ManagerArea.Elevator:
				DataManager.Instance.CurrentMineSavegame.ElevatorManagerOrder = value;
				break;
			case ManagerArea.Ground:
				DataManager.Instance.CurrentMineSavegame.GroundManagerOrder = value;
				break;
			}
		}
	}

	public int[] ManagerStaticList
	{
		get
		{
			switch (ManagerArea)
			{
			case ManagerArea.Corridor:
				return MISC_PARAMS.CORRIDOR_MANAGER_ORDER;
			case ManagerArea.Elevator:
				return MISC_PARAMS.ELEVATOR_MANAGER_ORDER;
			case ManagerArea.Ground:
				return MISC_PARAMS.GROUND_MANAGER_ORDER;
			default:
				return new int[0];
			}
		}
	}

	public double ManagerCostFactor
	{
		get
		{
			switch (ManagerArea)
			{
			case ManagerArea.Corridor:
				return 4.5;
			case ManagerArea.Elevator:
				return 8.0;
			case ManagerArea.Ground:
				return 8.0;
			default:
				return 0.0;
			}
		}
	}

	public double[] ManagerCostInit
	{
		get
		{
			switch (ManagerArea)
			{
			case ManagerArea.Corridor:
				return MISC_PARAMS.CORRIDOR_MANAGER_COST_INIT;
			case ManagerArea.Elevator:
				return MISC_PARAMS.ELEVATOR_MANAGER_COST_INIT;
			case ManagerArea.Ground:
				return MISC_PARAMS.GROUND_MANAGER_COST_INIT;
			default:
				return new double[0];
			}
		}
	}

	public string DialogTitle
	{
		get
		{
			switch (ManagerArea)
			{
			case ManagerArea.Corridor:
				return "Mine Shaft Manager";
			case ManagerArea.Elevator:
				return "Elevator Manager";
			case ManagerArea.Ground:
				return "Warehouse Manager";
			default:
				return string.Empty;
			}
		}
	}

	public ManagerSavegame AssignManagerSavegame
	{
		get
		{
			switch (ManagerArea)
			{
			case ManagerArea.Corridor:
				return DataManager.Instance.CurrentMineSavegame.GetCorridorSavegame(CurrentCorridorTier);
			case ManagerArea.Elevator:
				return DataManager.Instance.CurrentMineSavegame.ElevatorManagerSavegame;
			case ManagerArea.Ground:
				return DataManager.Instance.CurrentMineSavegame.GroundManagerSavegame;
			default:
				return null;
			}
		}
		set
		{
			switch (ManagerArea)
			{
			case ManagerArea.Corridor:
				if (value == null)
				{
					DataManager.Instance.CurrentMineSavegame.RemoveCorridorSavegame(CurrentCorridorTier);
				}
				else
				{
					DataManager.Instance.CurrentMineSavegame.SetCorridorSavegame(CurrentCorridorTier, value.BuyOrder);
				}
				break;
			case ManagerArea.Elevator:
				DataManager.Instance.CurrentMineSavegame.ElevatorManagerSavegame = value;
				break;
			case ManagerArea.Ground:
				DataManager.Instance.CurrentMineSavegame.GroundManagerSavegame = value;
				break;
			}
		}
	}

	public BaseManagerController BaseManagerController
	{
		get
		{
			switch (ManagerArea)
			{
			case ManagerArea.Corridor:
				return BaseController.GameController.MineController.CorridorController.CorridorLevelControllers[CurrentCorridorTier - 1].CorridorManagerController;
			case ManagerArea.Elevator:
				return BaseController.GameController.MineController.ElevatorController.ElevatorManagerController;
			case ManagerArea.Ground:
				return BaseController.GameController.MineController.GroundController.GroundManagerController;
			default:
				return null;
			}
		}
	}

	public ManagerEffect[] ManagerEffect
	{
		get
		{
			switch (ManagerArea)
			{
			case ManagerArea.Corridor:
				return DATA_CONST.CORRIDOR_EFFECTS;
			case ManagerArea.Elevator:
				return DATA_CONST.ELEVATOR_EFFECTS;
			case ManagerArea.Ground:
				return DATA_CONST.GROUND_EFFECTS;
			default:
				return null;
			}
		}
	}

	public int ActiveManager
	{
		get
		{
			if (ManagerArea == ManagerArea.Corridor)
			{
				return DataManager.Instance.CurrentMineSavegame.CorridorManagerDictionary.Count - DataManager.Instance.CurrentMineSavegame.CorridorCurrentManager.Count;
			}
			return DialogManagerItems.Count;
		}
	}

	public float OriginalScrollViewHeight
	{
		get
		{
			if (_originalScrollViewHeight == 0f)
			{
				Vector2 sizeDelta = ScrollRect.GetComponent<RectTransform>().sizeDelta;
				_originalScrollViewHeight = sizeDelta.y;
			}
			return _originalScrollViewHeight;
		}
	}

	public override void Start()
	{
		base.BackgroundDialog.SetTitle(DialogTitle, isType1: false);
		ButtonHireManager.text = "Hire Manager";
		ButtonHireManager.onClick.AddListener(ClickHireManager);
		BaseController.GameController.OnGlobalCashChangeCallback.Add(OnCashChangeCallback);
		InitFilterPanel();
		OnCashChangeCallback();
	}

	public override void OnShow()
	{
		base.OnShow();
		TextCash.SetCoinType(base.CurrentCoinType);
		if (DialogManagerItems.Count != ManagerSavegames.Count)
		{
			ResetData();
			LoadSaveManager();
			ToggleFilterPanel();
		}
		if (DialogManagerItems.Count > 0)
		{
			CheckAssignManagerView();
		}
		foreach (KeyValuePair<int, DialogManagerItem> dialogManagerItem in DialogManagerItems)
		{
			dialogManagerItem.Value.OnShowUpdate();
		}
		TextCash.SetMoney(CalculateManagerCost(), minify: true, showMoney: true, string.Empty);
		if (ManagerArea == ManagerArea.Corridor && ManagerBuyOrder == 0 && CheckEnoughCash(MISC_PARAMS.CORRIDOR_MANAGER_COST_INIT[0]) && DataManager.Instance.SavegameData.CurrentMineUnlocked == 0)
		{
			BaseController.GameController.TutorialController.CreateTutorial(TutorialType.DialogHireManager, ButtonHireManager.gameObject, isUI: true, 1f, useSpotlight: true, 1.3f);
		}
	}

	public override void OnHide()
	{
		base.OnHide();
		BaseController.GameController.TutorialController.RemoveTutorial(TutorialType.DialogHireManager);
	}

	public void ResetData()
	{
		TextCash.SetMoney(CalculateManagerCost(), minify: true, showMoney: true, string.Empty);
		OnCashChangeCallback();
		if (CurrentAssignManagerItem != null)
		{
			UnityEngine.Object.Destroy(CurrentAssignManagerItem.gameObject);
			CurrentAssignManagerItem = null;
		}
		foreach (KeyValuePair<int, DialogManagerItem> dialogManagerItem in DialogManagerItems)
		{
			if (dialogManagerItem.Value != null)
			{
				UnityEngine.Object.Destroy(dialogManagerItem.Value.gameObject);
			}
		}
		DialogManagerItems.Clear();
	}

	public void LoadSaveManager()
	{
		foreach (KeyValuePair<int, ManagerSavegame> managerSavegame in ManagerSavegames)
		{
			CreateManagerItem(managerSavegame.Value);
		}
		CheckAssignManagerView();
	}

	public void InitFilterPanel()
	{
		ButtonFilter = FilterPanel.GetComponentsInChildren<Button>();
		CurrentFilter = ManagerEffect[0];
		for (int i = 0; i < ButtonFilter.Length; i++)
		{
			if (i >= ManagerEffect.Length)
			{
				ButtonFilter[i].gameObject.SetActive(value: false);
				continue;
			}
			Vector2 sizeDelta = ButtonFilter[i].GetComponent<RectTransform>().sizeDelta;
			OriginalButtonFilterHeight = sizeDelta.y;
			Image image = ButtonFilter[i].GetComponentsInChildren<Image>()[1];
			ManagerEffect managerEffect = ManagerEffect[i];
			image.sprite = BaseController.LoadSprite(DATA_RESOURCES.IMAGE.DIALOG_MANAGER_SKILL_FILTER[(int)DataUtils.GetManagerEffect((int)managerEffect)]);
			image.SetNativeSize();
			ButtonFilter[i].onClick.AddListener(delegate
			{
				GameController.Instance.AudioController.PlayOneShot("Audios/Effect/click");
				SetFilter(managerEffect);
			});
		}
		ToggleFilterPanel();
	}

	public void ToggleFilterPanel()
	{
		bool flag = ActiveManager >= 9;
		FilterPanel.gameObject.SetActive(flag);
		RectTransform component = ScrollRect.GetComponent<RectTransform>();
		Vector2 sizeDelta = ScrollRect.GetComponent<RectTransform>().sizeDelta;
		float x = sizeDelta.x;
		float y;
		if (flag)
		{
			float originalScrollViewHeight = OriginalScrollViewHeight;
			Vector2 sizeDelta2 = FilterPanel.GetComponent<RectTransform>().sizeDelta;
			y = originalScrollViewHeight - sizeDelta2.y * 1f;
		}
		else
		{
			y = OriginalScrollViewHeight;
		}
		component.sizeDelta = new Vector2(x, y);
		SetFilter(CurrentFilter);
	}

	public void SetFilter(ManagerEffect managerEffect)
	{
		CurrentFilter = managerEffect;
		int num = Array.IndexOf(ManagerEffect, managerEffect);
		if (num != -1 && ButtonFilter.Length != 0)
		{
			for (int i = 0; i < ButtonFilter.Length; i++)
			{
				RectTransform component = ButtonFilter[i].GetComponent<RectTransform>();
				Vector2 sizeDelta = ButtonFilter[i].GetComponent<RectTransform>().sizeDelta;
				component.sizeDelta = new Vector2(sizeDelta.x, OriginalButtonFilterHeight);
				ButtonFilter[i].GetComponent<Image>().color = new Color(0.8f, 0.8f, 0.8f);
			}
			RectTransform component2 = ButtonFilter[num].GetComponent<RectTransform>();
			Vector2 sizeDelta2 = ButtonFilter[num].GetComponent<RectTransform>().sizeDelta;
			component2.sizeDelta = new Vector2(sizeDelta2.x, OriginalButtonFilterHeight * 1.2f);
			ButtonFilter[num].GetComponent<Image>().color = Color.white;
			CheckHideManagerFilter();
		}
	}

	public void UpdateBaseManagerController()
	{
		if (BaseManagerController != null)
		{
			if (AssignManagerSavegame != null)
			{
				BaseManagerController.DialogManagerItem = DialogManagerItems[AssignManagerSavegame.BuyOrder];
				BaseManagerController.LoadManagerSavegame(AssignManagerSavegame);
			}
			else
			{
				BaseManagerController.SetActiveManager();
			}
		}
	}

	public void ClickHireManager()
	{
		BaseController.GameController.TutorialController.RemoveTutorial(TutorialType.DialogHireManager);
		if (DialogController.UseCash(CalculateManagerCost()))
		{
			BaseController.GameController.AudioController.PlayOneShot("Audios/Effect/thuequanlythanhcong");
			HireManager();
			ToggleFilterPanel();
		}
	}

	public void HireManager()
	{
		ManagerSavegame managerSavegame = NextManagerSavegame();
		CreateManagerItem(managerSavegame);
		if (AssignManagerSavegame == null)
		{
			AssignManagerSavegame = managerSavegame;
		}
		CheckAssignManagerView();
		UpdateBaseManagerController();
		ScrollRect.normalizedPosition = Vector2.up;
		ManagerBuyOrder++;
		TextCash.SetMoney(CalculateManagerCost(), minify: true, showMoney: true, string.Empty);
		OnCashChangeCallback();
		//BaseController.GameController.AnalyticController.LogEvent("hire_manager", "ManagerArea", ManagerArea.ToString());
	}

	public void CheckAssignManagerView()
	{
		if (AssignManagerSavegame != null)
		{
			SetAssignManagerView(AssignManagerSavegame.BuyOrder);
			BaseManagerController.DialogManagerItem = DialogManagerItems[AssignManagerSavegame.BuyOrder];
		}
		else
		{
			RemoveOldAssignManager();
		}
		ToggleFilterPanel();
	}

	public void CheckHideManagerFilter()
	{
		foreach (KeyValuePair<int, DialogManagerItem> dialogManagerItem in DialogManagerItems)
		{
			dialogManagerItem.Value.gameObject.SetActive(value: true);
		}
		if (ManagerArea == ManagerArea.Corridor)
		{
			foreach (KeyValuePair<int, int> item in DataManager.Instance.CurrentMineSavegame.CorridorCurrentManager)
			{
				if (DialogManagerItems.ContainsKey(item.Value) && CurrentCorridorTier != item.Key && DialogManagerItems[item.Value].gameObject.activeInHierarchy)
				{
					DialogManagerItems[item.Value].gameObject.SetActive(value: false);
				}
			}
		}
		if (FilterPanel.gameObject.activeInHierarchy)
		{
			foreach (KeyValuePair<int, DialogManagerItem> dialogManagerItem2 in DialogManagerItems)
			{
				if ((AssignManagerSavegame == null || dialogManagerItem2.Value.ManagerSavegame.BuyOrder != AssignManagerSavegame.BuyOrder) && dialogManagerItem2.Value.ManagerParam.EffectID != (int)CurrentFilter)
				{
					dialogManagerItem2.Value.gameObject.SetActive(value: false);
				}
			}
		}
	}

	public ManagerSavegame NextManagerSavegame()
	{
		int managerID = (ManagerBuyOrder >= ManagerStaticList.Length) ? RandomManager() : ManagerStaticList[ManagerBuyOrder];
		ManagerSavegame managerSavegame = new ManagerSavegame();
		managerSavegame.BuyOrder = ManagerBuyOrder;
		managerSavegame.ManagerID = managerID;
		managerSavegame.ManagerName = DATA_TEXT.MANAGER_AREA[(int)(ManagerArea - 1)] + " #" + (ManagerBuyOrder + 1);
		ManagerSavegame managerSavegame2 = managerSavegame;
		if (ManagerSavegames.ContainsKey(ManagerBuyOrder))
		{
			if (DialogManagerItems.ContainsKey(ManagerBuyOrder) && DialogManagerItems[ManagerBuyOrder] != null)
			{
				UnityEngine.Object.Destroy(DialogManagerItems[ManagerBuyOrder].gameObject);
			}
			DialogManagerItems.Remove(ManagerBuyOrder);
			if (AssignManagerSavegame.BuyOrder == ManagerBuyOrder)
			{
				AssignManagerSavegame = null;
			}
		}
		ManagerSavegames[ManagerBuyOrder] = managerSavegame2;
		return managerSavegame2;
	}

	public int RandomManager()
	{
		int num = 0;
		float num2 = UnityEngine.Random.Range(0f, 1f);
		int num3 = (!(num2 <= 0.45f)) ? ((num2 <= 0.8f) ? 1 : 2) : 0;
		num = (int)ManagerEffect[UnityEngine.Random.Range(0, ManagerEffect.Length)];
		return (num - 1) * 3 + num3;
	}

	public void CreateManagerItem(ManagerSavegame managerSavegame)
	{
		DialogManagerItem dialogManagerItem = InstantiatePrefab("Prefabs/Dialog/Component/DialogManagerItem").GetComponent<DialogManagerItem>();
		dialogManagerItem.transform.SetParent(ScrollViewContent, worldPositionStays: false);
		dialogManagerItem.transform.SetAsFirstSibling();
		dialogManagerItem.transform.DOScale(1.05f * Vector3.one, 0.2f).onComplete = delegate
		{
			dialogManagerItem.transform.DOScale(Vector3.one, 0.2f);
		};
		dialogManagerItem.Init(managerSavegame, ManagerArea);
		dialogManagerItem.SetPopupManager();
		dialogManagerItem.OnClickActive = delegate
		{
			OnClickActive(managerSavegame.BuyOrder);
		};
		dialogManagerItem.OnClickAssign = delegate
		{
			OnAssignManager(managerSavegame.BuyOrder);
		};
		dialogManagerItem.OnClickSell = delegate
		{
			OnClickSellManager(managerSavegame.BuyOrder);
		};
		DialogManagerItems[managerSavegame.BuyOrder] = dialogManagerItem;
		CurrentFilter = (ManagerEffect)dialogManagerItem.ManagerParam.EffectID;
	}

	public void OnClickActive(int buyOrder)
	{
		BaseManagerController.ClickStartEffect();
	}

	public void OnAssignManager(int buyOrder)
	{
		if (AssignManagerSavegame != null)
		{
			UnAssignManagerStopEffect(AssignManagerSavegame.BuyOrder);
		}
		if (AssignManagerSavegame != null && AssignManagerSavegame.BuyOrder == buyOrder)
		{
			AssignManagerSavegame = null;
		}
		else
		{
			AssignManagerSavegame = DialogManagerItems[buyOrder].ManagerSavegame;
		}
		CheckAssignManagerView();
		UpdateBaseManagerController();
	}

	public void OnClickSellManager(int buyOrder)
	{
		double cash = CalculateManagerCost(buyOrder);
		string managerName = DialogManagerItems[buyOrder].ManagerSavegame.ManagerName;
		DialogController.DialogManagerSell.OnShow(cash, managerName, DialogManagerItems[buyOrder].ManagerParam);
		DialogController.DialogManagerSell.OnSellManagerCallback = delegate
		{
			OnSellManager(buyOrder);
		};
	}

	public void OnSellManager(int buyOrder)
	{
		DialogManagerItem dialogManagerItem = DialogManagerItems[buyOrder];
		UnityEngine.Object.Destroy(dialogManagerItem.gameObject);
		DialogManagerItems.Remove(buyOrder);
		ToggleFilterPanel();
		switch (ManagerArea)
		{
		case ManagerArea.Corridor:
			DataManager.Instance.CurrentMineSavegame.CorridorManagerDictionary.Remove(buyOrder);
			break;
		case ManagerArea.Elevator:
			DataManager.Instance.CurrentMineSavegame.ElevatorManagerDictionary.Remove(buyOrder);
			break;
		case ManagerArea.Ground:
			DataManager.Instance.CurrentMineSavegame.GroundManagerDictionary.Remove(buyOrder);
			break;
		}
	}

	public void UnAssignManagerStopEffect(int buyOrder)
	{
		ManagerEntity.Param managerParam = DialogManagerItems[buyOrder].ManagerParam;
		if (DialogManagerItems[buyOrder].EffectState == EffectState.Active)
		{
			AssignManagerSavegame.TimeActiveSkill = DateTime.Now.Ticks - 10000000 * (long)(managerParam.ActiveTime * (1.0 + BaseController.GameController.SkillController.ManagerEffectDurationBoost[ManagerArea] / 100.0));
		}
	}

	public ManagerSavegame GetManagerSavegameByOrder(int buyOrder)
	{
		for (int i = 0; i < ManagerSavegames.Count; i++)
		{
			if (ManagerSavegames[i].BuyOrder == buyOrder)
			{
				return ManagerSavegames[i];
			}
		}
		return null;
	}

	public void RemoveOldAssignManager()
	{
		if (CurrentAssignManagerItem != null)
		{
			CurrentAssignManagerItem.transform.SetParent(ScrollViewContent, worldPositionStays: false);
			CurrentAssignManagerItem.transform.SetAsFirstSibling();
			CurrentAssignManagerItem.SetAssign(isAssign: false);
			CurrentAssignManagerItem = null;
		}
	}

	public void SetAssignManagerView(int buyOrder)
	{
		RemoveOldAssignManager();
		DialogManagerItem dialogManagerItem = DialogManagerItems[buyOrder];
		dialogManagerItem.transform.SetParent(AssignManager, worldPositionStays: false);
		dialogManagerItem.transform.localPosition = Vector3.zero;
		dialogManagerItem.SetAssign(isAssign: true);
		CurrentAssignManagerItem = dialogManagerItem;
	}

	public void OnCashChangeCallback()
	{
		TextCash.SetMoneyColor(DataManager.Instance.Cash >= CalculateManagerCost());
	}

	public double CalculateManagerCost()
	{
		return CalculateManagerCost(ManagerBuyOrder);
	}

	public double CalculateManagerCost(int buyOrder)
	{
		double num = (buyOrder >= ManagerCostInit.Length) ? (ManagerCostInit[ManagerCostInit.Length - 1] * Math.Pow(ManagerCostFactor, buyOrder - ManagerCostInit.Length + 1)) : ManagerCostInit[buyOrder];
		return num * (1.0 - BaseController.GameController.SkillController.ManagerCostBoost / 100.0);
	}
}
