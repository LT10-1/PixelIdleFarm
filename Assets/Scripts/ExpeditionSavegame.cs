using System;
using System.Collections.Generic;

[Serializable]
public class ExpeditionSavegame
{
	public int Rarity;

	public List<int> ItemID;

	public long StartTime;

	public long Duration;
}
