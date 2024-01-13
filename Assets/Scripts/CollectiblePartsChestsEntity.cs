using System;
using System.Collections.Generic;
using UnityEngine;

public class CollectiblePartsChestsEntity : ScriptableObject
{
	[Serializable]
	public class Param
	{
		public int ChestID;

		public int PartsInChest;

		public int DifferentPartsAmount;

		public double SuperCashCost;
	}

	public List<Param> Params;

	public CollectiblePartsChestsEntity()
	{
		Params = new List<Param>();
	}
}
