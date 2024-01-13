using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SideBarPrestigeContent : BaseWorldmapController
{
	public TMP_Text IncomeText;

	public TMP_Text IdleCashText;

	public TMP_Text IdleCashTypeText;

	public TMP_Text MineIcon;

	public TMP_Text PrestigeCount;

	public Image Seperator;

	[HideInInspector]
	public int ContinentIndex;

	[HideInInspector]
	public int MineIndex;

	[HideInInspector]
	public bool IsCurrentMine;

	[HideInInspector]
	public bool IsAdBoosted;

	public override void Awake()
	{
		base.Awake();
		GetComponent<Button>().onClick.AddListener(delegate
		{
			OpenPrestigeDialog(ContinentIndex, MineIndex);
		});
	}

	public void Init(int continentIndex, int mineIndex)
	{
		ContinentIndex = continentIndex;
		MineIndex = mineIndex;
		MineIcon.text = DATA_RESOURCES.TEXT_SPRITE.RESOURCE[ContinentIndex][MineIndex];
		base.gameObject.SetActive(value: true);
	}

	public override void Update()
	{
		base.Update();
		int num = BaseController.MineOrder(ContinentIndex, MineIndex);
		MineSavegame mineSavegame = DataManager.Instance.SavegameData.Mines[num];
		IdleCashText.text = mineSavegame.IdleCash.MinifyFormat() + "/s";
		PrestigeCount.text = ((mineSavegame.PrestigeCount != 0) ? mineSavegame.PrestigeCount.ToString() : string.Empty);
		IdleCashTypeText.text = "Idle " + DATA_TEXT.CASH.LIST[ContinentIndex];
		IncomeText.text = GetMineFactor(num).MinifyIncomeFactor() + "x";
	}
}
