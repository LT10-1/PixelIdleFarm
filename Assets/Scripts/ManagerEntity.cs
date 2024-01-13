using System;
using System.Collections.Generic;
using UnityEngine;

public class ManagerEntity : ScriptableObject
{
	[Serializable]
	public class Param
	{
		public int ManagerID;

		public string Name;

		public int RarityID;

		public int EffectID;

		public int Area;

		public double DelayPerClickInSeconds;

		public double ValueX;

		public double ActiveTime;

		public double Cooldown;

		public bool AvailableThroughPurchase;

		public bool RatingReward;

		public int ManagerBuyOrder;
	}

	public List<Param> Params;

	public ManagerEntity()
	{
		Params = new List<Param>();
	}
}
