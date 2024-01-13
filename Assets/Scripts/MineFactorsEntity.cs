using System;
using System.Collections.Generic;
using UnityEngine;

public class MineFactorsEntity : ScriptableObject
{
	[Serializable]
	public class Param
	{
		public int PrestigeCount;

		public int MineNumber;

		public double Cost;

		public double Factor;

		public double SuperCashGained;
	}

	public List<Param> Params;

	public MineFactorsEntity()
	{
		Params = new List<Param>();
	}
}
