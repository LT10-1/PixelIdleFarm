using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseController : MonoBehaviour
{
	private static GameController _gameController;

	private int addItemCurrentIndex;

	public static GameController GameController
	{
		get
		{
			if (_gameController == null)
			{
				_gameController = UnityEngine.Object.FindObjectOfType<GameController>();
			}
			return _gameController;
		}
	}

	public bool CheckClickCanvas => Utils.IsPointerOverGameObject();

	public CoinType CurrentCoinType => GetCoinType(CurrentContinent);

	public ContinentType CurrentContinent => (ContinentType)DataManager.Instance.SavegameData.CurrentContinent;

	public bool DailyRewardAvailable => DataManager.Instance.SavegameData.DailyRewardLastTime == 0 || new DateTime(DataManager.Instance.SavegameData.DailyRewardLastTime).Date.Ticks != DateTime.Now.Date.Ticks;

	public string CoinStringMultipleCash => DATA_RESOURCES.TEXT_SPRITE.SAND_CASH + DATA_RESOURCES.TEXT_SPRITE.SAKURA_CASH + DATA_RESOURCES.TEXT_SPRITE.CASH;

	public string CURRENT_CASH_SPRITE => DATA_RESOURCES.TEXT_SPRITE.SPRITE[DataManager.Instance.SavegameData.CurrentContinent];

	public string CURRENT_RESOURCE_SPRITE => DATA_RESOURCES.TEXT_SPRITE.RESOURCE[DataManager.Instance.SavegameData.CurrentContinent][DataManager.Instance.SavegameData.CurrentMineIndex];

	public CoinType GetCoinType(ContinentType continentType)
	{
		switch (continentType)
		{
		case ContinentType.Grass:
			return CoinType.Cash;
		case ContinentType.Sand:
			return CoinType.SandCash;
		case ContinentType.Sakura:
			return CoinType.SakuraCash;
		default:
			return CoinType.Cash;
		}
	}

	public CoinType GetCoinType(ChestType chestType)
	{
		switch (chestType)
		{
		case ChestType.Normal:
			return CoinType.ChestNormal;
		case ChestType.Rare:
			return CoinType.ChestRare;
		case ChestType.Epic:
			return CoinType.ChestEpic;
		case ChestType.Legendary:
			return CoinType.ChestLegendary;
		default:
			return CoinType.ChestNormal;
		}
	}

	public CoinType GetSkillCoinType(ContinentType continentType)
	{
		switch (continentType)
		{
		case ContinentType.Grass:
			return CoinType.SkillPointGrass;
		case ContinentType.Sand:
			return CoinType.SkillPointSand;
		case ContinentType.Sakura:
			return CoinType.SkillPointSakura;
		default:
			return CoinType.SkillPointGrass;
		}
	}

	public double CashFromInstantTime(long time)
	{
		double num = (double)time * Math.Max(DataManager.Instance.TotalIdleCash, 100.0) / 0.1;
		if (DataManager.Instance.SavegameData.IAPCheckHavePurchaseIncomeX2)
		{
			num *= 2.0;
		}
		return num;
	}

	public string CoinStringByInstantTime(long time)
	{
		return (!((double)time < TimeSpan.FromHours(48.0).TotalSeconds)) ? (CURRENT_CASH_SPRITE + CURRENT_CASH_SPRITE + CURRENT_CASH_SPRITE) : CURRENT_CASH_SPRITE;
	}

	public static int MineOrder(int continent, int mineIndex)
	{
		return continent * 5 + mineIndex;
	}

	public virtual void Awake()
	{
		DOTween.defaultEaseType = Ease.Linear;
	}

	public virtual void Start()
	{
	}

	public virtual void Update()
	{
	}

	public void AddCash(double cash)
	{
		AddCash(cash, CurrentContinent);
	}

	public void AddCash(double cash, ContinentType continentType)
	{
		DataManager.Instance.SetCashByContinent(DataManager.Instance.CashByContinent(continentType) + cash, continentType);
		DataManager.Instance.SetCashNetworthByContinent(DataManager.Instance.CashNetworthByContinent(continentType) + cash, continentType);
		UpdateCash();
	}

	public bool UseCash(double cash)
	{
		return UseCash(cash, CurrentContinent);
	}

	public bool UseCash(double cash, ContinentType continentType)
	{
		if (!CheckEnoughCash(cash, continentType))
		{
			GameController.ToastController.StartToast("Not Enough " + DATA_TEXT.CASH.LIST[(int)continentType] + " " + DATA_RESOURCES.TEXT_SPRITE.SPRITE[(int)continentType]);
			return false;
		}
		MonoBehaviour.print("Use " + DATA_TEXT.CASH.LIST[(int)continentType] + ": " + cash.MinifyFormat());
		double val = DataManager.Instance.CashByContinent(continentType) - cash;
		val = Math.Max(val, 0.0);
		DataManager.Instance.SetCashByContinent(val, continentType);
		UpdateCash();
		return true;
	}

	public bool CheckEnoughCash(double amount)
	{
		return CheckEnoughCash(amount, CurrentContinent);
	}

	public bool CheckEnoughCash(double amount, ContinentType continentType)
	{
		return !MathUtils.CompareDoubleBiggerThanZero(amount - DataManager.Instance.CashByContinent(continentType));
	}

	public void UpdateCash()
	{
		GameController.InvokeOnCashChangeCallback();
	}

	public void AddSuperCash(double superCash)
	{
		DataManager.Instance.SuperCash += superCash;
		DataManager.Instance.SuperCashNetWorth += superCash;
		GameController.UpdateMyInfo();
		UpdateSuperCashText();
	}

	public void UseSuperCash(double superCash, SpendSuperCashType spendSuperCashType, Action onConfirmPurchase = null, ItemsEntity.Param itemParam = null, DialogSkillShopItem dialogSkillShopItem = null)
	{
		if (!CheckEnoughSuperCash(superCash))
		{
			GameController.DialogController.DialogNotEnoughSuperCash.OnShow();
			return;
		}
		if (spendSuperCashType == SpendSuperCashType.None)
		{
			UseSuperCashSuccess(superCash, onConfirmPurchase);
			return;
		}
		GameController.DialogController.DialogConfirmPurchase.OnShow(superCash, spendSuperCashType, itemParam, dialogSkillShopItem);
		GameController.DialogController.DialogConfirmPurchase.OnClickConfirm = delegate
		{
			UseSuperCashSuccess(superCash, onConfirmPurchase);
		};
	}

	public void UseSuperCashSuccess(double superCash, Action onConfirmPurchase = null)
	{
		MonoBehaviour.print("Use SuperCash: " + superCash);
		DataManager.Instance.SuperCash -= superCash;
		UpdateSuperCashText();
		onConfirmPurchase?.Invoke();
	}

	public double GetMineFactor(int mineId, int prestigeCount = -1)
	{
		MineSavegame mineSavegame = DataManager.Instance.SavegameData.Mines[mineId];
		MineModel mineModel = GameController.MineModels[mineId];
		double num = 1.0;
		if (prestigeCount != -1)
		{
			double factor = DataManager.Instance.MineFactorsEntityParams[mineSavegame.MineIndex][mineSavegame.PrestigeCount].Factor;
			double factor2 = DataManager.Instance.MineFactorsEntityParams[mineSavegame.MineIndex][prestigeCount].Factor;
			num = factor2 / factor;
		}
		return mineModel.StatsIncreaseModel.MineTotalFactor * num;
	}

	public bool CheckEnoughSuperCash(double amount)
	{
		return DataManager.Instance.SuperCash >= amount;
	}

	public void UpdateSuperCashText()
	{
		GameController.InvokeOnSuperCashChangeCallback();
	}

	public void AddSkillPoint(int point, ContinentType continentType, bool showEffect = true, GameObject coinFlyStartPos = null)
	{
		int num = DataManager.Instance.SkillPointNetworthByContinent(continentType) + point;
		int num2 = DataManager.Instance.MaxSkillPointByContinent(continentType);
		if (num > num2)
		{
			num = num2;
		}
		point = num - DataManager.Instance.SkillPointNetworthByContinent(continentType);
		DataManager.Instance.SetSkillPointByContinent(DataManager.Instance.SkillPointByContinent(continentType) + point, continentType);
		DataManager.Instance.SetSkillPointNetworthByContinent(num, continentType);
		if (showEffect)
		{
			CreateReceiveEffect("You Receive", new ItemsEntity.Param
			{
				ItemType = 4,
				SkillPathID = (int)continentType,
				SkillPointAmount = point
			});
		}
		GameController.CashFlyEffect.StartEff(GetSkillCoinType(continentType), coinFlyStartPos, ItemType.AddSkillPoint, Math.Min(point, 20));
		GameController.InvokeOnSkillpointChangeCallback();
	}

	public bool UseSkillPoint(int point, ContinentType continentType)
	{
		if (!CheckEnoughSkillPoint(point, continentType))
		{
			GameController.ToastController.StartToast("Not Enough Skill Points " + DATA_RESOURCES.TEXT_SPRITE.SKILL_POINT[(int)continentType]);
			return false;
		}
		int a = DataManager.Instance.SkillPointByContinent(continentType) - point;
		a = Mathf.Max(a, 0);
		DataManager.Instance.SetSkillPointByContinent(a, continentType);
		GameController.InvokeOnSkillpointChangeCallback();
		return true;
	}

	public bool CheckEnoughSkillPoint(int point, ContinentType continentType)
	{
		return DataManager.Instance.SkillPointByContinent(continentType) >= point;
	}

	public void AddItem(int itemID, bool showEffect = true, int count = 1, Action callback = null)
	{
		if (!DataManager.Instance.SavegameData.Inventory.ContainsKey(itemID))
		{
			DataManager.Instance.SavegameData.Inventory[itemID] = 0;
		}
		Dictionary<int, int> inventory;
		int key;
		(inventory = DataManager.Instance.SavegameData.Inventory)[key = itemID] = inventory[key] + count;
		if (showEffect)
		{
			CreateReceiveEffect("You Receive", DataManager.Instance.ItemDictionary[itemID], callback);
			GameController.InvokeOnItemChangeCallback();
		}
	}

	public void AddMultiItem(Dictionary<int, int> itemList, double superCashGain = 0.0)
	{
		addItemCurrentIndex = 0;
		foreach (KeyValuePair<int, int> item in itemList)
		{
			AddItem(item.Key, showEffect: false, item.Value);
		}
		if (superCashGain > 0.0)
		{
			AddSuperCash(superCashGain);
			CreateReceiveEffectSuperCash("You Receive", superCashGain, delegate
			{
				AddItemIndex(itemList);
			});
		}
		else
		{
			AddItemIndex(itemList);
		}
	}

	public void AddItemIndex(Dictionary<int, int> itemList)
	{
		if (addItemCurrentIndex < itemList.Count)
		{
			AddItem(itemList.ElementAt(addItemCurrentIndex).Key, showEffect: true, 0, delegate
			{
				AddItemIndex(itemList);
			});
			addItemCurrentIndex++;
		}
	}

	public void UseItem(int itemID)
	{
		if (!DataManager.Instance.SavegameData.Inventory.ContainsKey(itemID))
		{
			return;
		}
		Dictionary<int, int> inventory;
		int key;
		(inventory = DataManager.Instance.SavegameData.Inventory)[key = itemID] = inventory[key] - 1;
		if (DataManager.Instance.SavegameData.Inventory[itemID] == 0)
		{
			DataManager.Instance.SavegameData.Inventory.Remove(itemID);
		}
		ItemsEntity.Param param = DataManager.Instance.ItemDictionary[itemID];
		if (param.ItemType == 1)
		{
			double completeIncomeIncreaseFactor = param.CompleteIncomeIncreaseFactor;
			if (!DataManager.Instance.SavegameData.BoostMultipleEndTime.ContainsKey(completeIncomeIncreaseFactor))
			{
				DataManager.Instance.SavegameData.BoostMultipleEndTime[completeIncomeIncreaseFactor] = DateTime.Now.Ticks;
				DataManager.Instance.SavegameData.BoostMultipleStartTime[completeIncomeIncreaseFactor] = DateTime.Now.Ticks;
			}
			Dictionary<double, long> boostMultipleEndTime;
			double key2;
			(boostMultipleEndTime = DataManager.Instance.SavegameData.BoostMultipleEndTime)[key2 = completeIncomeIncreaseFactor] = boostMultipleEndTime[key2] + param.ActiveTimeSeconds * 10000000;
		}
		if (param.ItemType == 2)
		{
			double num = CashFromInstantTime(param.InstantCashTime);
			AddCash(num);
			ItemsEntity.Param param2 = new ItemsEntity.Param();
			param2.ItemType = 3;
			param2.InstantCashAmount = num;
			param = param2;
		}
		CreateReceiveEffect("You Activate", param);
		GameController.InvokeOnItemChangeCallback();
	}

	public void AddChest(ChestType chestType, int count, bool showEffect = true)
	{
		if (!DataManager.Instance.SavegameData.ChestSavegames.ContainsKey((int)chestType))
		{
			DataManager.Instance.SavegameData.ChestSavegames[(int)chestType] = 0;
		}
		Dictionary<int, int> chestSavegames;
		int key;
		(chestSavegames = DataManager.Instance.SavegameData.ChestSavegames)[key = (int)chestType] = chestSavegames[key] + count;
		if (showEffect)
		{
			GameController.CashFlyEffect.StartEff(GetCoinType(chestType), null, ItemType.AddChest, Math.Min(count, 10));
		}
		GameController.InvokeOnChestChangeCallback();
	}

	public bool UseChest(ChestType chestType)
	{
		if (!DataManager.Instance.SavegameData.ChestSavegames.ContainsKey((int)chestType) || DataManager.Instance.SavegameData.ChestSavegames[(int)chestType] <= 0)
		{
			return false;
		}
		Dictionary<int, int> chestSavegames;
		int key;
		(chestSavegames = DataManager.Instance.SavegameData.ChestSavegames)[key = (int)chestType] = chestSavegames[key] - 1;
		GameController.InvokeOnChestChangeCallback();
		return true;
	}

	public void AddPartsFromChest(List<KeyValuePair<int, int>> chestData)
	{
		foreach (KeyValuePair<int, int> chestDatum in chestData)
		{
			AddCollectibleParts(chestDatum.Key, chestDatum.Value);
		}
		GameController.InvokeOnWorkshopChangeCallback();
	}

	public void AddCollectibleParts(int id, int parts)
	{
		CollectibleSavegame collectibleSavegame = DataManager.Instance.SavegameData.CollectibleSavegames[id];
		CollectiblesEntity.Param param = DataManager.Instance.CollectiblesDictionary[id];
		Dictionary<int, CollectibleProductionFactorsEntity.Param> dictionary = DataManager.Instance.CollectibleLevelsDictionary[id];
		int level = collectibleSavegame.Level;
		int num = parts;
		int num2 = level + 1;
		while (true)
		{
			if (num2 <= param.MaxLevel)
			{
				CollectibleProductionFactorsEntity.Param param2 = dictionary[num2];
				if (collectibleSavegame.Parts + num < param2.PartsRequired)
				{
					break;
				}
				num = num + collectibleSavegame.Parts - param2.PartsRequired;
				collectibleSavegame.Level++;
				collectibleSavegame.Parts = 0;
				num2++;
				continue;
			}
			return;
		}
		collectibleSavegame.Parts += num;
	}

	public ReceiveEffect CreateReceiveEffect(string title, ItemsEntity.Param ItemParam, Action callback = null)
	{
		return CreateReceiveEffect(title, ItemParam, CurrentContinent, callback);
	}

	public ReceiveEffect CreateReceiveEffect(string title, ItemsEntity.Param ItemParam, ContinentType continentType, Action callback = null)
	{
		ReceiveEffect component = InstantiatePrefab("Prefabs/Effects/ReceiveEffect").GetComponent<ReceiveEffect>();
		component.CoinType = GetCoinType(continentType);
		component.Show(title, ItemParam, callback);
		return component;
	}

	public ReceiveEffect CreateReceiveEffect(string title, string image, string valueText, Action callback = null)
	{
		return CreateReceiveEffect(title, image, valueText, Vector3.one, callback);
	}

	public ReceiveEffect CreateReceiveEffect(string title, string image, string valueText, Vector3 scale, Action callback = null)
	{
		ReceiveEffect component = InstantiatePrefab("Prefabs/Effects/ReceiveEffect").GetComponent<ReceiveEffect>();
		component.Show(title, image, valueText, scale, callback);
		return component;
	}

	public void CreateReceiveEffectSuperCash(string title, double superCash, Action callback = null)
	{
		ItemsEntity.Param param = new ItemsEntity.Param();
		param.SuperCashAmount = superCash;
		param.ItemType = 0;
		ItemsEntity.Param itemParam = param;
		CreateReceiveEffect(title, itemParam, callback);
	}

	public static Sprite LoadSprite(string resources)
	{
		return Resources.Load<Sprite>(resources);
	}

	public T InstantiatePrefab<T>(string resources)
	{
		return InstantiatePrefab(resources).GetComponent<T>();
	}

	public GameObject InstantiatePrefab(string resources)
	{
		return UnityEngine.Object.Instantiate(Resources.Load<GameObject>(resources));
	}

	public void PurchaseProductSuccess(string productID, string transactionID, int value)
	{
		MonoBehaviour.print(productID + " " + transactionID + " " + value);
		//GameController.AnalyticController.LogEventPurchase(productID, value);
		GameController.APIHelper.ChargeIAP(productID, value);
	}

	public void OpenPrestigeDialog()
	{
		OpenPrestigeDialog((int)CurrentContinent, GameController.MineController.MineIndex);
	}

	public void OpenPrestigeDialog(int ContinentIndex, int MineIndex)
	{
		OpenPrestigeDialog(MineOrder(ContinentIndex, MineIndex));
	}

	public void OpenPrestigeDialog(int mineOrder)
	{
		MineSavegame mineSavegame = DataManager.Instance.SavegameData.Mines[mineOrder];
		if (mineSavegame.PrestigeCount >= DataManager.Instance.MineFactorsEntityParams[mineSavegame.MineIndex].Count - 1)
		{
			GameController.ToastController.StartToast("More Prestige Level Coming Soon!");
			return;
		}
		GameController.DialogController.DialogPrestige.OnShow();
		GameController.DialogController.DialogPrestige.Init(mineSavegame.ContinentIndex, mineSavegame.MineIndex);
	}
}
