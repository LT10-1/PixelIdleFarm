using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SideBarCollectContent : BaseWorldmapController
{
	public TMP_Text MainText;

	public TMP_Text PlusText;

	public TMP_Text CoinText;

	public TMP_Text BoostText;

	public Image Seperator;

	[HideInInspector]
	public int ContinentIndex;

	[HideInInspector]
	public int MineIndex;

	[HideInInspector]
	public bool IsCurrentMine;

	[HideInInspector]
	public bool IsAdBoosted;

	public void Init(int continentIndex, int mineIndex)
	{
		ContinentIndex = continentIndex;
		MineIndex = mineIndex;
		MainText.text = DATA_RESOURCES.TEXT_SPRITE.RESOURCE[ContinentIndex][MineIndex];
		IsCurrentMine = (ContinentIndex == DataManager.Instance.SavegameData.CurrentContinent && MineIndex == DataManager.Instance.SavegameData.CurrentMineIndex);
		PlusText.gameObject.SetActive(!IsCurrentMine);
		if (IsCurrentMine)
		{
			CoinText.text = "Current Mine";
		}
	}
}
