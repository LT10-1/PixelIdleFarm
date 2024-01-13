using System;
using System.Collections.Generic;
using UnityEngine;

public class ManagerEffectEntity : ScriptableObject
{
	[Serializable]
	public class Param
	{
		public int EffectID;

		public string EffectName;

		public string StringId;

		public int Area;

		public string EffectDescription;
	}

	public List<Param> Params;

	public ManagerEffectEntity()
	{
		Params = new List<Param>();
	}
}
