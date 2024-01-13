using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChestController : BaseController
{
	public int ChestCount => DataManager.Instance.SavegameData.ChestSavegames.Sum((KeyValuePair<int, int> e) => e.Value);

	public override void Start()
	{
		base.Start();
	}

	public void GenerateWatchAdChest()
	{
		if (DataManager.Instance.SavegameData.UnlockedWorkshop)
		{
			int num = Math.Min(DataManager.Instance.SavegameData.ChestReceivedFromAdDaily, MISC_PARAMS.CHEST_WATCH_AD_RECEIVE_CHANCE.Length - 1);
			double num2 = MISC_PARAMS.CHEST_WATCH_AD_RECEIVE_CHANCE[num];
			if ((double)UnityEngine.Random.Range(0f, 1f) < num2)
			{
				ChestType chestType = GenerateChest();
				AddChest(chestType, 1);
				DataManager.Instance.SavegameData.ChestReceivedFromAdDaily++;
				BaseController.GameController.ToastController.StartToast("You receive a chest from watching Ad!");
			}
		}
	}

	public Dictionary<ChestType, int> GenerateDailyChest()
	{
		Dictionary<ChestType, int> dictionary = new Dictionary<ChestType, int>();
		if (!DataManager.Instance.SavegameData.UnlockedWorkshop)
		{
			return dictionary;
		}
		int value = (DataManager.Instance.SavegameData.ChestReceiveLastTime == 0) ? 1 : (TimeSpan.FromTicks(DateTime.Now.Ticks).Days - TimeSpan.FromTicks(DataManager.Instance.SavegameData.ChestReceiveLastTime).Days);
		value = Mathf.Clamp(value, 0, 5);
		if (value <= 0)
		{
			return dictionary;
		}
		DataManager.Instance.SavegameData.ChestReceiveLastTime = DateTime.Now.Ticks;
		DataManager.Instance.SavegameData.ChestReceivedFromAdDaily = 0;
		for (int i = 0; i < value; i++)
		{
			ChestType chestType = GenerateChest();
			if (!dictionary.ContainsKey(chestType))
			{
				dictionary[chestType] = 0;
			}
			Dictionary<ChestType, int> dictionary2;
			ChestType key;
			(dictionary2 = dictionary)[key = chestType] = dictionary2[key] + 1;
		}
		return dictionary;
	}

	public ChestType GenerateChest()
	{
		float num = UnityEngine.Random.Range(0f, 1f);
		ChestType result = ChestType.Normal;
		if ((double)num < 0.01)
		{
			result = ChestType.Epic;
		}
		else if ((double)num < 0.1)
		{
			result = ChestType.Rare;
		}
		return result;
	}

	public void TestData()
	{
		Dictionary<CollectibleRarity, int> dictionary = new Dictionary<CollectibleRarity, int>();
		IEnumerator enumerator = Enum.GetValues(typeof(CollectibleRarity)).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				CollectibleRarity key = (CollectibleRarity)enumerator.Current;
				dictionary[key] = 0;
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		for (int num = 10; num >= 0; num--)
		{
			for (int i = 0; i < 4; i++)
			{
				if (i == 3)
				{
					List<KeyValuePair<int, int>> list = GenerateParts(i);
					foreach (KeyValuePair<int, int> item in list)
					{
						CollectiblesEntity.Param param = DataManager.Instance.CollectiblesDictionary[item.Key];
						Dictionary<CollectibleRarity, int> dictionary2;
						CollectibleRarity rarityID;
						(dictionary2 = dictionary)[rarityID = (CollectibleRarity)param.RarityID] = dictionary2[rarityID] + item.Value;
						MonoBehaviour.print((ManagerArea)param.CollectibleType + " " + (CollectibleRarity)param.RarityID + param.Variant + ": " + item.Value);
					}
				}
			}
		}
		foreach (KeyValuePair<CollectibleRarity, int> item2 in dictionary)
		{
			MonoBehaviour.print(item2.Key + ": " + item2.Value);
		}
	}

	public CollectiblePartsChestsEntity.Param Chest(int id)
	{
		return DataManager.Instance.ChestDictionary[id];
	}

	public List<KeyValuePair<int, int>> GenerateTutorialChest()
	{
		List<KeyValuePair<int, int>> list = new List<KeyValuePair<int, int>>();
		foreach (KeyValuePair<int, CollectiblesEntity.Param> item in DataManager.Instance.CollectiblesDictionary)
		{
			if (item.Value.CollectibleType == 3 && item.Value.RarityID == 1 && item.Value.Variant == 0)
			{
				list.Add(new KeyValuePair<int, int>(item.Value.CollectibleID, 1));
				return list;
			}
		}
		return list;
	}

	public List<KeyValuePair<int, int>> GenerateParts(int chestID)
	{
		List<KeyValuePair<int, int>> list = new List<KeyValuePair<int, int>>();
		CollectiblePartsChestsEntity.Param param = Chest(chestID);
		if (!DataManager.Instance.SavegameData.HasOpenTutorialChest && param.ChestID == 0)
		{
			DataManager.Instance.SavegameData.HasOpenTutorialChest = true;
			return GenerateTutorialChest();
		}
		Dictionary<CollectibleRarity, int> dictionary = GenerateRarities(chestID);
		Dictionary<CollectibleRarity, int> dictionary2 = new Dictionary<CollectibleRarity, int>();
		dictionary2[CollectibleRarity.Legendary] = ((dictionary[CollectibleRarity.Legendary] > 0) ? 1 : 0);
		dictionary2[CollectibleRarity.Epic] = ((dictionary[CollectibleRarity.Epic] > 0) ? 1 : 0);
		if (param.ChestID == 3 && dictionary[CollectibleRarity.Epic] >= 10)
		{
			dictionary2[CollectibleRarity.Epic] = 2;
		}
		dictionary2[CollectibleRarity.Rare] = Math.Min(Mathf.CeilToInt((float)dictionary[CollectibleRarity.Rare] / 10f), 9);
		dictionary2[CollectibleRarity.Common] = param.DifferentPartsAmount - dictionary2[CollectibleRarity.Legendary] - dictionary2[CollectibleRarity.Epic] - dictionary2[CollectibleRarity.Rare];
		IEnumerator enumerator = Enum.GetValues(typeof(CollectibleRarity)).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				CollectibleRarity collectibleRarity = (CollectibleRarity)enumerator.Current;
				int num = dictionary2[collectibleRarity];
				if (num != 0)
				{
					Dictionary<int, CollectiblesEntity.Param> dictionary3 = (from pair in DataManager.Instance.CollectiblesDictionary
						where pair.Value.RarityID == (int)collectibleRarity
						select pair).ToDictionary((KeyValuePair<int, CollectiblesEntity.Param> e) => e.Key, (KeyValuePair<int, CollectiblesEntity.Param> e) => e.Value);
					List<CollectiblesEntity.Param> list2 = dictionary3.Values.ToList();
					list2.Shuffle();
					Dictionary<int, int> dictionary4 = new Dictionary<int, int>();
					int num2 = dictionary[collectibleRarity];
					for (int i = 0; i < num; i++)
					{
						dictionary4.Add(list2[i].CollectibleID, 1);
					}
					for (int j = 0; j < num2 - num; j++)
					{
						int index = UnityEngine.Random.Range(0, num);
						Dictionary<int, int> dictionary5;
						int collectibleID;
						(dictionary5 = dictionary4)[collectibleID = list2[index].CollectibleID] = dictionary5[collectibleID] + 1;
					}
					foreach (KeyValuePair<int, int> item in dictionary4)
					{
						list.Add(item);
					}
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		list.Shuffle();
		return list;
	}

	public Dictionary<CollectibleRarity, int> GenerateRarities(int chestID, int totalPartsAmount = 0)
	{
		if (totalPartsAmount == 0)
		{
			totalPartsAmount = Chest(chestID).PartsInChest;
		}
		List<CollectibleRarity> list = new List<CollectibleRarity>();
		AddGuaranteedRarities(list, chestID);
		int num = totalPartsAmount - list.Count;
		for (int i = 0; i < num; i++)
		{
			AddRandomRarity(list, chestID);
		}
		Dictionary<CollectibleRarity, int> dictionary = new Dictionary<CollectibleRarity, int>();
		IEnumerator enumerator = Enum.GetValues(typeof(CollectibleRarity)).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				CollectibleRarity rarity = (CollectibleRarity)enumerator.Current;
				int value = list.Count((CollectibleRarity collectibleRarity) => collectibleRarity == rarity);
				dictionary.Add(rarity, value);
			}
			return dictionary;
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
	}

	private void AddGuaranteedRarities(List<CollectibleRarity> partsAsRarities, int chestID)
	{
		Dictionary<int, CollectiblePartsLootTablesEntity.Param> dictionary = DataManager.Instance.ChestLootTableDictionary[chestID];
		foreach (KeyValuePair<int, CollectiblePartsLootTablesEntity.Param> item in dictionary)
		{
			for (int i = 0; i < item.Value.MinAmount; i++)
			{
				partsAsRarities.Add((CollectibleRarity)item.Value.RarityID);
			}
		}
	}

	private void AddRandomRarity(List<CollectibleRarity> partsAsRarities, int chestID)
	{
		int randomValue = UnityEngine.Random.Range(0, 10000);
		CollectibleRarity rarityFromLootChances = GetRarityFromLootChances(randomValue, chestID);
		partsAsRarities.Add(rarityFromLootChances);
	}

	private CollectibleRarity GetRarityFromLootChances(int randomValue, int chestID)
	{
		Dictionary<int, CollectiblePartsLootTablesEntity.Param> source = DataManager.Instance.ChestLootTableDictionary[chestID];
		return (CollectibleRarity)source.FirstOrDefault((KeyValuePair<int, CollectiblePartsLootTablesEntity.Param> pair) => pair.Value.InRange(randomValue)).Value.RarityID;
	}
}
