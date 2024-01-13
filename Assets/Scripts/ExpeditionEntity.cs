using System;
using System.Collections.Generic;
using UnityEngine;

public class ExpeditionEntity : ScriptableObject
{
	[Serializable]
	public class Param
	{
		public int Rarity;

		public long DurationMin;

		public long DurationMax;

		public double SuperCashMin;

		public double SuperCashMax;

		public int NumberItem;

		public List<int> Items;
	}

	public List<Param> Params;

	public ExpeditionEntity()
	{
		Params = new List<Param>();
	}
}
