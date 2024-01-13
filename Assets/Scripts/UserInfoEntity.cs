using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class UserInfoEntity
{
	[Serializable]
	public class Param
	{
		public string Name;

		public string UserId;

		public string UrlAvatar;

		public int Rank;

		public int Score;

		public int CurrentMine;

		public int CurrentShaft;

		public int CurrentContinent;

		public bool IsMe;

		[JsonIgnore]
		public int ShaftTotalIndex => CurrentMine * 30 + CurrentShaft;
	}

	public List<Param> Params;

	public UserInfoEntity()
	{
		Params = new List<Param>();
	}

	public void Sort()
	{
		Params = (from o in Params
			orderby o.Score descending
			select o).ToList();
		for (int i = 0; i < Params.Count; i++)
		{
			Params[i].Rank = i + 1;
		}
	}
}
