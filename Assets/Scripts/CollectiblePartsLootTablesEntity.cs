using System;
using System.Collections.Generic;
using UnityEngine;

public class CollectiblePartsLootTablesEntity : ScriptableObject
{
	[Serializable]
	public class Param
	{
		public int ChestID;

		public int RarityID;

		public int MinAmount;

		public int MinRoll;

		public int MaxRoll;

		public bool InRange(int randomNumber)
		{
			return MinRoll <= randomNumber && randomNumber < MaxRoll;
		}
	}

	public List<Param> Params;

	public CollectiblePartsLootTablesEntity()
	{
		Params = new List<Param>();
	}
}
