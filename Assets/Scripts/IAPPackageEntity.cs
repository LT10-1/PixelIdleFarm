using System;
using System.Collections.Generic;
using UnityEngine;

public class IAPPackageEntity : ScriptableObject
{
	[Serializable]
	public class Param
	{
		public string ProductID;

		public List<int> ItemIDList;

		public List<int> ItemCountList;

		public Dictionary<int, int> ItemList;

		public double SuperCashGain;

		public int IAPCost;
	}

	public List<Param> Params;

	public IAPPackageEntity()
	{
		Params = new List<Param>();
	}
}
