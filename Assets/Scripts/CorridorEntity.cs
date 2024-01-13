using System;
using System.Collections.Generic;
using UnityEngine;

public class CorridorEntity : ScriptableObject
{
	[Serializable]
	public class Param
	{
		public int Tier;

		public int Level;

		public double Cost;

		public int NumberOfWorkers;

		public double GainPerSecondPerWorker;

		public double CapacityPerWorker;

		public int WorkerWalkingSpeedPerSecond;

		public bool BigUpdate;

		public double SuperCashReward;
	}

	public List<Param> Params;

	public CorridorEntity()
	{
		Params = new List<Param>();
	}
}
