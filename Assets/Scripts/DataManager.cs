using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataManager
{
	public MineFactorsEntity MineFactorsEntity;

	public CorridorEntity CorridorEntity;

	public ElevatorEntity ElevatorEntity;

	public WarehouseEntity WarehouseEntity;

	public ManagerEntity ManagerEntity;

	public ManagerEffectEntity ManagerEffectEntity;

	public ItemsEntity ItemsEntity;

	public DailyRewardEntity DailyRewardEntity;

	public IAPPackageEntity IAPPackageEntity;

	public SkillEntity SkillEntity;

	public CollectiblesEntity CollectiblesEntity;

	public CollectibleProductionFactorsEntity CollectibleProductionFactorsEntity;

	public CollectiblePartsChestsEntity CollectiblePartsChestsEntity;

	public CollectiblePartsLootTablesEntity CollectiblePartsLootTablesEntity;

	public ExpeditionEntity ExpeditionEntity;

	public Dictionary<int, Dictionary<int, MineFactorsEntity.Param>> MineFactorsEntityParams;

	public Dictionary<int, Dictionary<int, CorridorEntity.Param>> CorridorEntityParams;

	public Dictionary<int, ElevatorEntity.Param> ElevatorParams;

	public Dictionary<int, WarehouseEntity.Param> WarehouseParams;

	public Dictionary<int, ManagerEntity.Param> ManagerParams;

	public Dictionary<int, List<ManagerEntity.Param>> ManagerByAreaParams;

	public Dictionary<int, ManagerEffectEntity.Param> ManagerEffectParams;

	public Dictionary<int, ItemsEntity.Param> ItemDictionary;

	public Dictionary<ItemBoostMultiple, Dictionary<ItemBoostDuration, ItemsEntity.Param>> BoostItemDictionary;

	public Dictionary<int, ItemsEntity.Param> DailyRewardList;

	public Dictionary<int, ItemsEntity.Param> ShopItemDictionary;

	public Dictionary<string, IAPPackageEntity.Param> IAPPackageDictionary;

	public Dictionary<int, SkillEntity.Param> SkillDictionary;

	public Dictionary<int, CollectiblesEntity.Param> CollectiblesDictionary;

	public Dictionary<int, Dictionary<int, CollectibleProductionFactorsEntity.Param>> CollectibleLevelsDictionary;

	public Dictionary<int, CollectiblePartsChestsEntity.Param> ChestDictionary;

	public Dictionary<int, Dictionary<int, CollectiblePartsLootTablesEntity.Param>> ChestLootTableDictionary;

	public Dictionary<ExpeditionRarity, ExpeditionEntity.Param> ExpeditionDictionary;

	public SavegameData SavegameData;

	public SettingData SettingData;

	public List<Action> OnSettingChangeCallback = new List<Action>();

	private int MaxSkillPointGrass;

	private int MaxSkillPointSand;

	private int MaxSkillPointSakura;

	private static DataManager _instance;

	public MineSavegame CurrentMineSavegame => SavegameData.Mines[SavegameData.CurrentMine];

	public double GrassCash
	{
		get
		{
			return SavegameData.Cash;
		}
		set
		{
			SavegameData.Cash = value;
		}
	}

	public double SandCash
	{
		get
		{
			return SavegameData.SandCash;
		}
		set
		{
			SavegameData.SandCash = value;
		}
	}

	public double SakuraCash
	{
		get
		{
			return SavegameData.SakuraCash;
		}
		set
		{
			SavegameData.SakuraCash = value;
		}
	}

	public double Cash
	{
		get
		{
			return CashByContinent((ContinentType)SavegameData.CurrentContinent);
		}
		set
		{
			SetCashByContinent(value, (ContinentType)SavegameData.CurrentContinent);
		}
	}

	public double SuperCash
	{
		get
		{
			return SavegameData.SuperCash;
		}
		set
		{
			SavegameData.SuperCash = value;
		}
	}

	public double CashNetWorth
	{
		get
		{
			return CashNetworthByContinent((ContinentType)SavegameData.CurrentContinent);
		}
		set
		{
			SetCashNetworthByContinent(value, (ContinentType)SavegameData.CurrentContinent);
		}
	}

	public double GrassCashNetWorth
	{
		get
		{
			return SavegameData.CashNetworth;
		}
		set
		{
			SavegameData.CashNetworth = value;
		}
	}

	public double SandCashNetWorth
	{
		get
		{
			return SavegameData.SandCashNetWorth;
		}
		set
		{
			SavegameData.SandCashNetWorth = value;
		}
	}

	public double SakuraCashNetWorth
	{
		get
		{
			return SavegameData.SakuraCashNetworth;
		}
		set
		{
			SavegameData.SakuraCashNetworth = value;
		}
	}

	public double SuperCashNetWorth
	{
		get
		{
			return SavegameData.SuperCashNetworth;
		}
		set
		{
			SavegameData.SuperCashNetworth = value;
		}
	}

	public double IdleCash
	{
		get
		{
			return CurrentMineSavegame.IdleCash;
		}
		set
		{
			CurrentMineSavegame.IdleCash = value;
		}
	}

	public double TotalIdleCash => TotalIdleCashByContinent((ContinentType)SavegameData.CurrentContinent);

	public static DataManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new DataManager();
				if (CONST.FAST_GAME_TEST_MODE)
				{
					_instance.Cash = 8.5263525508796317E+150;
				}
			}
			return _instance;
		}
	}

	private DataManager()
	{
		ImportData();
		InitSettingData();
		InitSaveGameData();
	}

	public double CashByContinent(ContinentType continentType = ContinentType.Grass)
	{
		switch (continentType)
		{
		case ContinentType.Grass:
			return GrassCash;
		case ContinentType.Sand:
			return SandCash;
		case ContinentType.Sakura:
			return SakuraCash;
		default:
			return GrassCash;
		}
	}

	public void SetCashByContinent(double value, ContinentType continentType = ContinentType.Grass)
	{
		switch (continentType)
		{
		case ContinentType.Grass:
			GrassCash = value;
			break;
		case ContinentType.Sand:
			SandCash = value;
			break;
		case ContinentType.Sakura:
			SakuraCash = value;
			break;
		}
	}

	public double TotalIdleCashByContinent(ContinentType continentType = ContinentType.Grass)
	{
		return SavegameData.Mines.Sum((MineSavegame item) => (item.ContinentIndex != (int)continentType) ? 0.0 : item.IdleCash);
	}

	public double CashNetworthByContinent(ContinentType continentType = ContinentType.Grass)
	{
		switch (continentType)
		{
		case ContinentType.Grass:
			return GrassCashNetWorth;
		case ContinentType.Sand:
			return SandCashNetWorth;
		case ContinentType.Sakura:
			return SakuraCashNetWorth;
		default:
			return GrassCashNetWorth;
		}
	}

	public void SetCashNetworthByContinent(double value, ContinentType continentType = ContinentType.Grass)
	{
		switch (continentType)
		{
		case ContinentType.Grass:
			GrassCashNetWorth = value;
			break;
		case ContinentType.Sand:
			SandCashNetWorth = value;
			break;
		case ContinentType.Sakura:
			SakuraCashNetWorth = value;
			break;
		}
	}

	public int SkillPointByContinent(ContinentType continentType)
	{
		switch (continentType)
		{
		case ContinentType.Grass:
			return SavegameData.SkillPointGrass;
		case ContinentType.Sand:
			return SavegameData.SkillPointSand;
		case ContinentType.Sakura:
			return SavegameData.SkillPointSakura;
		default:
			return SavegameData.SkillPointGrass;
		}
	}

	public void SetSkillPointByContinent(int point, ContinentType continentType)
	{
		switch (continentType)
		{
		case ContinentType.Grass:
			SavegameData.SkillPointGrass = point;
			break;
		case ContinentType.Sand:
			SavegameData.SkillPointSand = point;
			break;
		case ContinentType.Sakura:
			SavegameData.SkillPointSakura = point;
			break;
		}
	}

	public int SkillPointNetworthByContinent(ContinentType continentType)
	{
		switch (continentType)
		{
		case ContinentType.Grass:
			return SavegameData.SkillPointGrassNetworth;
		case ContinentType.Sand:
			return SavegameData.SkillPointSandNetworth;
		case ContinentType.Sakura:
			return SavegameData.SkillPointSakuraNetworth;
		default:
			return SavegameData.SkillPointGrassNetworth;
		}
	}

	public void SetSkillPointNetworthByContinent(int point, ContinentType continentType)
	{
		switch (continentType)
		{
		case ContinentType.Grass:
			SavegameData.SkillPointGrassNetworth = point;
			break;
		case ContinentType.Sand:
			SavegameData.SkillPointSandNetworth = point;
			break;
		case ContinentType.Sakura:
			SavegameData.SkillPointSakuraNetworth = point;
			break;
		}
	}

	public int MaxSkillPointByContinent(ContinentType continentType)
	{
		switch (continentType)
		{
		case ContinentType.Grass:
			return MaxSkillPointGrass;
		case ContinentType.Sand:
			return MaxSkillPointSand;
		case ContinentType.Sakura:
			return MaxSkillPointSakura;
		default:
			return MaxSkillPointGrass;
		}
	}

	public int SkillPointFromCashByContinent(ContinentType continentType)
	{
		switch (continentType)
		{
		case ContinentType.Grass:
			return SavegameData.SkillPointGrassBuyCash;
		case ContinentType.Sand:
			return SavegameData.SkillPointSandBuyCash;
		case ContinentType.Sakura:
			return SavegameData.SkillPointSakuraBuyCash;
		default:
			return SavegameData.SkillPointGrassBuyCash;
		}
	}

	public void SetSkillPointFromCashByContinent(int point, ContinentType continentType)
	{
		switch (continentType)
		{
		case ContinentType.Grass:
			SavegameData.SkillPointGrassBuyCash = point;
			break;
		case ContinentType.Sand:
			SavegameData.SkillPointSandBuyCash = point;
			break;
		case ContinentType.Sakura:
			SavegameData.SkillPointSakuraBuyCash = point;
			break;
		}
	}

	private void ImportData()
	{
		MineFactorsEntity = Resources.Load<MineFactorsEntity>("Data/Mine");
		CorridorEntity = Resources.Load<CorridorEntity>("Data/Corridor");
		ElevatorEntity = Resources.Load<ElevatorEntity>("Data/Elevator");
		WarehouseEntity = Resources.Load<WarehouseEntity>("Data/Warehouse");
		ManagerEntity = Resources.Load<ManagerEntity>("Data/Manager");
		ManagerEffectEntity = Resources.Load<ManagerEffectEntity>("Data/ManagerEffect");
		ItemsEntity = Resources.Load<ItemsEntity>("Data/Item");
		DailyRewardEntity = Resources.Load<DailyRewardEntity>("Data/ItemDailyReward");
		IAPPackageEntity = Resources.Load<IAPPackageEntity>("Data/ItemIAPPackage");
		SkillEntity = Resources.Load<SkillEntity>("Data/Skill");
		CollectiblesEntity = Resources.Load<CollectiblesEntity>("Data/Collectible");
		CollectibleProductionFactorsEntity = Resources.Load<CollectibleProductionFactorsEntity>("Data/CollectibleProduct");
		CollectiblePartsChestsEntity = Resources.Load<CollectiblePartsChestsEntity>("Data/CollectibleChest");
		CollectiblePartsLootTablesEntity = Resources.Load<CollectiblePartsLootTablesEntity>("Data/CollectibleLootTable");
		ExpeditionEntity = Resources.Load<ExpeditionEntity>("Data/Expedition");
		MineFactorsEntityParams = new Dictionary<int, Dictionary<int, MineFactorsEntity.Param>>();
		foreach (MineFactorsEntity.Param param3 in MineFactorsEntity.Params)
		{
			int mineNumber = param3.MineNumber;
			int prestigeCount = param3.PrestigeCount;
			if (!MineFactorsEntityParams.ContainsKey(mineNumber))
			{
				MineFactorsEntityParams[mineNumber] = new Dictionary<int, MineFactorsEntity.Param>();
			}
			MineFactorsEntityParams[mineNumber][prestigeCount] = param3;
		}
		CorridorEntityParams = new Dictionary<int, Dictionary<int, CorridorEntity.Param>>();
		foreach (CorridorEntity.Param param4 in CorridorEntity.Params)
		{
			int tier = param4.Tier;
			int level = param4.Level;
			if (!CorridorEntityParams.ContainsKey(tier))
			{
				CorridorEntityParams[tier] = new Dictionary<int, CorridorEntity.Param>();
			}
			CorridorEntityParams[tier].Add(level, param4);
		}
		ElevatorParams = ElevatorEntity.Params.ToDictionary((ElevatorEntity.Param e) => e.Level, (ElevatorEntity.Param e) => e);
		WarehouseParams = WarehouseEntity.Params.ToDictionary((WarehouseEntity.Param e) => e.Level, (WarehouseEntity.Param e) => e);
		ManagerParams = ManagerEntity.Params.ToDictionary((ManagerEntity.Param e) => e.ManagerID, delegate(ManagerEntity.Param e)
		{
			ManagerEntity.Param param2 = JsonConvert.DeserializeObject<ManagerEntity.Param>(JsonConvert.SerializeObject(e));
			if (CONST.FAST_MANAGER_SKILL_TEST_MODE)
			{
				param2.ActiveTime /= 15.0;
				param2.Cooldown /= 15.0;
			}
			return param2;
		});
		ManagerByAreaParams = new Dictionary<int, List<ManagerEntity.Param>>();
		foreach (ManagerEntity.Param param5 in ManagerEntity.Params)
		{
			int area = param5.Area;
			if (!ManagerByAreaParams.ContainsKey(area))
			{
				ManagerByAreaParams[area] = new List<ManagerEntity.Param>();
			}
			if (param5.ValueX > 0.0)
			{
				ManagerByAreaParams[area].Add(param5);
			}
		}
		ManagerEffectParams = ManagerEffectEntity.Params.ToDictionary((ManagerEffectEntity.Param e) => e.EffectID, (ManagerEffectEntity.Param e) => e);
		ItemDictionary = ItemsEntity.Params.ToDictionary((ItemsEntity.Param e) => e.ItemID, (ItemsEntity.Param e) => e);
		DailyRewardList = DailyRewardEntity.Params.ToDictionary((DailyRewardEntity.Param e) => e.CycleCount, (DailyRewardEntity.Param e) => ItemDictionary[e.ItemID]);
		for (int i = 0; i < IAPPackageEntity.Params.Count; i++)
		{
			IAPPackageEntity.Param param = IAPPackageEntity.Params[i];
			param.ItemList = new Dictionary<int, int>();
			for (int j = 0; j < param.ItemIDList.Count; j++)
			{
				param.ItemList.Add(param.ItemIDList[j], param.ItemCountList[j]);
			}
		}
		IAPPackageDictionary = IAPPackageEntity.Params.ToDictionary((IAPPackageEntity.Param e) => e.ProductID, (IAPPackageEntity.Param e) => e);
		SkillDictionary = SkillEntity.Params.ToDictionary((SkillEntity.Param e) => e.SkillID, (SkillEntity.Param e) => e);
		MaxSkillPointGrass = SkillDictionary.Sum((KeyValuePair<int, SkillEntity.Param> e) => (e.Value.SkillPathID == 0) ? e.Value.MaxSkillPoints : 0);
		MaxSkillPointSand = SkillDictionary.Sum((KeyValuePair<int, SkillEntity.Param> e) => (e.Value.SkillPathID == 1) ? e.Value.MaxSkillPoints : 0);
		MaxSkillPointSakura = SkillDictionary.Sum((KeyValuePair<int, SkillEntity.Param> e) => (e.Value.SkillPathID == 2) ? e.Value.MaxSkillPoints : 0);
		ShopItemDictionary = new Dictionary<int, ItemsEntity.Param>();
		BoostItemDictionary = new Dictionary<ItemBoostMultiple, Dictionary<ItemBoostDuration, ItemsEntity.Param>>();
		foreach (KeyValuePair<int, ItemsEntity.Param> item in ItemDictionary)
		{
			if (item.Value.ItemType == 1)
			{
				ItemBoostMultiple key = (ItemBoostMultiple)item.Value.CompleteIncomeIncreaseFactor;
				ItemBoostDuration key2 = (ItemBoostDuration)item.Value.ActiveTimeSeconds;
				if (!BoostItemDictionary.ContainsKey(key))
				{
					BoostItemDictionary[key] = new Dictionary<ItemBoostDuration, ItemsEntity.Param>();
				}
				BoostItemDictionary[key][key2] = item.Value;
			}
			if (item.Value.CanBeBoughtInItemShop)
			{
				ShopItemDictionary[item.Key] = item.Value;
			}
		}
		CollectiblesDictionary = CollectiblesEntity.Params.ToDictionary((CollectiblesEntity.Param e) => e.CollectibleID, (CollectiblesEntity.Param e) => e);
		CollectibleLevelsDictionary = new Dictionary<int, Dictionary<int, CollectibleProductionFactorsEntity.Param>>();
		foreach (CollectibleProductionFactorsEntity.Param param6 in CollectibleProductionFactorsEntity.Params)
		{
			int collectibleID = param6.CollectibleID;
			int collectibleLevel = param6.CollectibleLevel;
			if (!CollectibleLevelsDictionary.ContainsKey(collectibleID))
			{
				CollectibleLevelsDictionary[collectibleID] = new Dictionary<int, CollectibleProductionFactorsEntity.Param>();
			}
			CollectibleLevelsDictionary[collectibleID].Add(collectibleLevel, param6);
		}
		ChestDictionary = CollectiblePartsChestsEntity.Params.ToDictionary((CollectiblePartsChestsEntity.Param e) => e.ChestID, (CollectiblePartsChestsEntity.Param e) => e);
		ChestLootTableDictionary = new Dictionary<int, Dictionary<int, CollectiblePartsLootTablesEntity.Param>>();
		foreach (CollectiblePartsLootTablesEntity.Param param7 in CollectiblePartsLootTablesEntity.Params)
		{
			int chestID = param7.ChestID;
			int rarityID = param7.RarityID;
			if (!ChestLootTableDictionary.ContainsKey(chestID))
			{
				ChestLootTableDictionary[chestID] = new Dictionary<int, CollectiblePartsLootTablesEntity.Param>();
			}
			ChestLootTableDictionary[chestID].Add(rarityID, param7);
		}
		ExpeditionDictionary = ExpeditionEntity.Params.ToDictionary((ExpeditionEntity.Param e) => (ExpeditionRarity)e.Rarity, (ExpeditionEntity.Param e) => e);
	}

	public void InitSettingData()
	{
		if (PlayerPrefs.HasKey("SettingData"))
		{
			string @string = PlayerPrefs.GetString("SettingData");
			UnityEngine.Debug.Log(@string);
			SettingData = JsonConvert.DeserializeObject<SettingData>(@string);
		}
		else
		{
			SettingData = new SettingData();
		}
		SettingData.OnValueChanged = SaveSetting;
		SaveSetting();
	}

	public void InitSaveGameData()
	{
		if (PlayerPrefs.HasKey("SaveGameData"))
		{
			string @string = PlayerPrefs.GetString("SaveGameData");
			UnityEngine.Debug.Log(@string);
			SavegameData = JsonConvert.DeserializeObject<SavegameData>(@string);
		}
		else
		{
			SavegameData = new SavegameData();
		}
		InitDataParam();
		if (CONST.TEST_MODE_VIDEO_AD)
		{
			if (SavegameData.Mines.Count < 2)
			{
				SavegameData.Mines.Add(new MineSavegame());
			}
			SavegameData.CurrentMine = 1;
			CurrentMineSavegame.MineIndex = 1;
		}
	}

	public void SaveSetting()
	{
		if (SettingData != null)
		{
			PlayerPrefs.SetString("SettingData", JsonConvert.SerializeObject(SettingData));
			for (int i = 0; i < OnSettingChangeCallback.Count; i++)
			{
				OnSettingChangeCallback[i]();
			}
		}
	}

	public void Savegame()
	{
		if (SavegameData != null)
		{
			PlayerPrefs.SetString("SaveGameData", JsonConvert.SerializeObject(SavegameData));
		}
	}

	public void InitDataParam()
	{
		if (SavegameData.Mines == null || SavegameData.Mines.Count == 0)
		{
			SavegameData.Mines = new List<MineSavegame>
			{
				new MineSavegame()
			};
		}
		SavegameData.Cash = Math.Max(SavegameData.Cash, 0.0);
		SavegameData.SandCash = Math.Max(SavegameData.SandCash, 0.0);
		SavegameData.SakuraCash = Math.Max(SavegameData.SakuraCash, 0.0);
		if (SavegameData.CollectibleSavegames == null || SavegameData.CollectibleSavegames.Count != CollectiblesDictionary.Count)
		{
			SavegameData.CollectibleSavegames = new Dictionary<int, CollectibleSavegame>();
			foreach (KeyValuePair<int, CollectiblesEntity.Param> item in CollectiblesDictionary)
			{
				SavegameData.CollectibleSavegames[item.Key] = new CollectibleSavegame
				{
					CollectibleId = item.Key,
					Level = ((item.Key % 10 == 0) ? 1 : 0),
					Parts = 0
				};
			}
		}
		AdjustSavegameForVersion();
		Savegame();
	}

	public void AdjustSavegameForVersion()
	{
		if (SavegameData.SavegameVersion < 3.0)
		{
		}
		SavegameData.SavegameVersion = 3.2;
	}

	public void Init()
	{
		for (int i = 0; i < CorridorEntity.Params.Count; i++)
		{
		}
	}
}
