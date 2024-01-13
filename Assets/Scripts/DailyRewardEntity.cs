using System;
using System.Collections.Generic;
using UnityEngine;

public class DailyRewardEntity : ScriptableObject
{
	[Serializable]
	public class Param
	{
		public int CycleCount;

		public int ItemID;

		public bool IsBigReward;
	}

	public List<Param> Params;

	public DailyRewardEntity()
	{
		Params = new List<Param>();
	}
}
