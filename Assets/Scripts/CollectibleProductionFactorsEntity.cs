using System;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleProductionFactorsEntity : ScriptableObject
{
	[Serializable]
	public class Param
	{
		public int CollectibleID;

		public int CollectibleLevel;

		public double ProductionFactor;

		public int PartsRequired;

		public double SecondaryEffectFactor;
	}

	public List<Param> Params;

	public CollectibleProductionFactorsEntity()
	{
		Params = new List<Param>();
	}
}
