using System;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorEntity : ScriptableObject
{
	[Serializable]
	public class Param
	{
		public int Level;

		public double Cost;

		public double Speed;

		public double Capacity;

		public double LoadingPerSecond;

		public bool BigUpdate;

		public double SuperCashReward;
	}

	public List<Param> Params;

	public ElevatorEntity()
	{
		Params = new List<Param>();
	}
}
