using System;
using System.Collections.Generic;
using UnityEngine;

public class CollectiblesEntity : ScriptableObject
{
	[Serializable]
	public class Param
	{
		public int CollectibleID;

		public int CollectibleType;

		public int RarityID;

		public int Variant;

		public int MaxLevel;

		public bool IsActivated;

		public int SecondaryEffectId;

		public int SecondaryEffectTargetId;
	}

	public List<Param> Params;

	public CollectiblesEntity()
	{
		Params = new List<Param>();
	}
}
