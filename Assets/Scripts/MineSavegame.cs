using Newtonsoft.Json;
using System;
using System.Collections.Generic;

[Serializable]
public class MineSavegame
{
	public int ContinentIndex;

	public int MineIndex;

	public int PrestigeCount;

	public double IdleCash;

	public double IdleBonusCashGain;

	public long MineLastTimeVisit;

	public long MineBoostx2EndTime;

	public int MileStoneUnlockState;

	public long MileStoneStartTime;

	public int AdMileStoneCount;

	public long AdMileStoneEndTime;

	public int ElevatorLevel;

	public int GroundLevel;

	public List<int> CorridorLevel = new List<int>();

	public int CorridorManagerOrder;

	public int ElevatorManagerOrder;

	public int GroundManagerOrder;

	public int ElevatorCurrentManager = -1;

	public int GroundCurrentManager = -1;

	public Dictionary<int, int> CorridorCurrentManager = new Dictionary<int, int>();

	public Dictionary<int, ManagerSavegame> CorridorManagerDictionary = new Dictionary<int, ManagerSavegame>();

	public Dictionary<int, ManagerSavegame> ElevatorManagerDictionary = new Dictionary<int, ManagerSavegame>();

	public Dictionary<int, ManagerSavegame> GroundManagerDictionary = new Dictionary<int, ManagerSavegame>();

	[JsonIgnore]
	public int MineOrder => BaseController.MineOrder(ContinentIndex, MineIndex);

	[JsonIgnore]
	public double CurrentIdleCash
	{
		get
		{
			if (MineLastTimeVisit == 0)
			{
				return 0.0;
			}
			double num = IdleCash * (double)(int)Math.Min(TimeSpan.FromTicks(DateTime.Now.Ticks - MineLastTimeVisit).TotalSeconds, 2592000.0);
			if (DataManager.Instance.SavegameData.IAPCheckHavePurchaseIncomeX2)
			{
				num *= 2.0;
			}
			return num + IdleBonusCashGain;
		}
	}

	[JsonIgnore]
	public ManagerSavegame ElevatorManagerSavegame
	{
		get
		{
			if (ElevatorCurrentManager == -1)
			{
				return null;
			}
			return ElevatorManagerDictionary[ElevatorCurrentManager];
		}
		set
		{
			if (value == null)
			{
				ElevatorCurrentManager = -1;
			}
			else
			{
				ElevatorCurrentManager = value.BuyOrder;
			}
		}
	}

	[JsonIgnore]
	public ManagerSavegame GroundManagerSavegame
	{
		get
		{
			if (GroundCurrentManager == -1)
			{
				return null;
			}
			return GroundManagerDictionary[GroundCurrentManager];
		}
		set
		{
			if (value == null)
			{
				GroundCurrentManager = -1;
			}
			else
			{
				GroundCurrentManager = value.BuyOrder;
			}
		}
	}

	public ManagerSavegame GetCorridorSavegame(int tier)
	{
		if (!CorridorCurrentManager.ContainsKey(tier))
		{
			return null;
		}
		if (!CorridorManagerDictionary.ContainsKey(CorridorCurrentManager[tier]))
		{
			return null;
		}
		return CorridorManagerDictionary[CorridorCurrentManager[tier]];
	}

	public void SetCorridorSavegame(int tier, int buyOrder)
	{
		CorridorCurrentManager[tier] = buyOrder;
	}

	public void RemoveCorridorSavegame(int tier)
	{
		CorridorCurrentManager.Remove(tier);
	}
}
