using Newtonsoft.Json;
using System;
using System.Collections.Generic;

[Serializable]
public class SavegameData
{
	public double SavegameVersion;

	public double LastLoginTime;

	public int CurrentMine;

	public int CurrentMineUnlocked;

	public int CurrentContinentUnlocked;

	public int CurrentShaftUnlocked;

	public double Cash = 10.0;

	public double SandCash = 10.0;

	public double SakuraCash = 10.0;

	public double SuperCash;

	public double CashNetworth;

	public double SandCashNetWorth;

	public double SakuraCashNetworth;

	public double SuperCashNetworth;

	public int SkillPointGrass;

	public int SkillPointSand;

	public int SkillPointSakura;

	public int SkillPointGrassBuyCash;

	public int SkillPointSandBuyCash;

	public int SkillPointSakuraBuyCash;

	public int SkillPointGrassNetworth;

	public int SkillPointSandNetworth;

	public int SkillPointSakuraNetworth;

	public bool UnlockedLaboratory;

	public bool UnlockedWorkshop;

	public bool UnlockedExpedition;

	public List<MineSavegame> Mines;

	public Dictionary<int, int> Inventory = new Dictionary<int, int>();

	public Dictionary<double, long> BoostMultipleEndTime = new Dictionary<double, long>();

	public Dictionary<double, long> BoostMultipleStartTime = new Dictionary<double, long>();

	public List<string> CouponSubmitList = new List<string>();

	public int DailyRewardCycleCount;

	public long DailyRewardLastTime;

	public long ChestReceiveLastTime;

	public int ChestReceivedFromAdDaily;

	public Dictionary<int, long> LocalNotificationDictionary = new Dictionary<int, long>();

	public bool IAPHavePurchaseIncomeX2;

	public List<string> PurchasedIAPPackage = new List<string>();

	public Dictionary<int, int> SkillSaveGame = new Dictionary<int, int>();

	public Dictionary<int, CollectibleSavegame> CollectibleSavegames = new Dictionary<int, CollectibleSavegame>();

	public bool HasOpenTutorialChest;

	public Dictionary<int, int> ChestSavegames = new Dictionary<int, int>();

	public int CurrentActiveCorridorCard;

	public int CurrentActiveGroundCard = 10;

	public int CurrentActiveElevatorCard = 20;

	public long ExpeditionRefreshTime;

	public long ExpeditionLastTime;

	public long ExpeditionLastBoostTime;

	public List<ExpeditionSavegame> ExpeditionChooseList;

	public ExpeditionSavegame CurrentExpedition;

	[JsonIgnore]
	public int CurrentContinent => CurrentMine / 5;

	[JsonIgnore]
	public int CurrentMineIndex => CurrentMine % 5;

	[JsonIgnore]
	public int CurrentMineUnlockedIndex => CurrentMineUnlocked % 5;

	[JsonIgnore]
	public bool IAPCheckHavePurchaseTest => PurchasedIAPPackage.Contains("test.product");

	[JsonIgnore]
	public bool IAPCheckHavePurchaseIncomeX2
	{
		get
		{
			if (IAPHavePurchaseIncomeX2 && !PurchasedIAPPackage.Contains("x2.cashincome"))
			{
				PurchasedIAPPackage.Add("x2.cashincome");
			}
			return PurchasedIAPPackage.Contains("x2.cashincome");
		}
	}

	public int CurrentActiveWorkerCard(ManagerArea managerArea)
	{
		switch (managerArea)
		{
		case ManagerArea.Corridor:
			return CurrentActiveCorridorCard;
		case ManagerArea.Ground:
			return CurrentActiveGroundCard;
		case ManagerArea.Elevator:
			return CurrentActiveElevatorCard;
		default:
			return CurrentActiveCorridorCard;
		}
	}

	public void SetActiveWorkerCard(int collectibleID, ManagerArea managerArea)
	{
		switch (managerArea)
		{
		case ManagerArea.Corridor:
			CurrentActiveCorridorCard = collectibleID;
			break;
		case ManagerArea.Ground:
			CurrentActiveGroundCard = collectibleID;
			break;
		case ManagerArea.Elevator:
			CurrentActiveElevatorCard = collectibleID;
			break;
		}
	}
}
