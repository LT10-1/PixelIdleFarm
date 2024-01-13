using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemsEntity : ScriptableObject
{
	[Serializable]
	public class Param
	{
		public int ItemID;

		public int ItemType;

		public double SuperCashCost;

		public long ActiveTimeSeconds;

		public double CompleteIncomeIncreaseFactor;

		public long InstantCashTime;

		public bool CanBeBoughtInItemShop;

		public bool CanBeBoughtInIapShop;

		public double SuperCashAmount;

		public double InstantCashAmount;

		public long SkillPointAmount;

		public int SkillPathID;

		public int ChestNumber;

		public int ChestType;

		[JsonIgnore]
		public string ItemDescription
		{
			get
			{
				switch (ItemType)
				{
				case 0:
					return SuperCashAmount.MinifyFormat() + " Super Cash";
				case 1:
					return "x" + CompleteIncomeIncreaseFactor.MinifyIncomeFactor() + " Income for " + ActiveTimeSeconds.FormatTimeString();
				case 4:
					return SkillPointAmount + " " + DATA_TEXT.CONTINENT.LIST[SkillPathID] + " Skill Point";
				case 5:
					return ChestNumber + " " + DATA_TEXT.COLLECTIBLES.LIST[ChestType] + " Chest";
				default:
					return string.Empty;
				}
			}
		}
	}

	public List<Param> Params;

	public ItemsEntity()
	{
		Params = new List<Param>();
	}
}
