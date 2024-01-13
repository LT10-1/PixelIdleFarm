using System;
using System.Collections.Generic;
using UnityEngine;

public class WarehouseEntity : ScriptableObject
{
	[Serializable]
	public class Param
	{
		public int Level;

		public double Cost;

		public int NumberOfWorkers;

		public double CapacityPerWorker;

		public int WorkerWalkingSpeedPerSecond;

		public double LoadingPerSecond;

		public bool BigUpdate;

		public double SuperCashReward;
	}

	public List<Param> Params;

	public WarehouseEntity()
	{
		Params = new List<Param>();
	}
}
