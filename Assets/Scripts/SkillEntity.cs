using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillEntity : ScriptableObject
{
	[Serializable]
	public class Param
	{
		public int SkillID;

		public int ParentId;

		public int EffectId;

		public int SkillPathID;

		public int MaxLevel;

		public int MaxSkillPoints;

		public bool IsMajor;

		public List<double> ParamX;

		public int ParamY;

		public List<int> SkillPoint;
	}

	public List<Param> Params;

	public SkillEntity()
	{
		Params = new List<Param>();
	}
}
