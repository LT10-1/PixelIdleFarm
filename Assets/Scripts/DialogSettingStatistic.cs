using TMPro;

public class DialogSettingStatistic : BaseDialog
{
	public TMP_Text GeneralText;

	public TMP_Text CashContent;

	public TMP_Text SandCashTitle;

	public TMP_Text SandCashContent;

	public TMP_Text SakuraCashTitle;

	public TMP_Text SakuraCashContent;

	public TMP_Text[] MineTitleText;

	public TMP_Text[] MineDescriptionText;

	public TMP_Text[] MineValueText;

	public override void Start()
	{
		base.Start();
		base.BackgroundDialog.SetTitle("Statistics");
	}

	public override void Update()
	{
		base.Update();
		SandCashTitle.gameObject.SetActive(DataManager.Instance.SavegameData.CurrentContinentUnlocked >= 1);
		SandCashContent.transform.parent.gameObject.SetActive(DataManager.Instance.SavegameData.CurrentContinentUnlocked >= 1);
		SakuraCashTitle.gameObject.SetActive(DataManager.Instance.SavegameData.CurrentContinentUnlocked >= 2);
		SakuraCashContent.transform.parent.gameObject.SetActive(DataManager.Instance.SavegameData.CurrentContinentUnlocked >= 2);
		CashContent.text = DATA_RESOURCES.TEXT_SPRITE.CASH + " " + DataManager.Instance.SavegameData.CashNetworth.MinifyFormat() + "\n" + DATA_RESOURCES.TEXT_SPRITE.CASH + " " + DataManager.Instance.TotalIdleCashByContinent().MinifyFormat() + "/s";
		SandCashContent.text = DATA_RESOURCES.TEXT_SPRITE.SAND_CASH + " " + DataManager.Instance.SavegameData.SandCashNetWorth.MinifyFormat() + "\n" + DATA_RESOURCES.TEXT_SPRITE.SAND_CASH + " " + DataManager.Instance.TotalIdleCashByContinent(ContinentType.Sand).MinifyFormat() + "/s";
		SakuraCashContent.text = DATA_RESOURCES.TEXT_SPRITE.SAKURA_CASH + " " + DataManager.Instance.SavegameData.SakuraCashNetworth.MinifyFormat() + "\n" + DATA_RESOURCES.TEXT_SPRITE.SAKURA_CASH + " " + DataManager.Instance.TotalIdleCashByContinent(ContinentType.Sakura).MinifyFormat() + "/s";
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		for (int i = 0; i < DataManager.Instance.SavegameData.Mines.Count; i++)
		{
			MineTitleText[i].gameObject.SetActive(value: true);
			MineDescriptionText[i].gameObject.SetActive(value: true);
			MineValueText[i].gameObject.SetActive(value: true);
			MineSavegame mineSavegame = DataManager.Instance.SavegameData.Mines[i];
			MineTitleText[i].text = DATA_TEXT.MINES.CONTINENT_MINES[mineSavegame.ContinentIndex][mineSavegame.MineIndex] + " Mine " + DATA_RESOURCES.TEXT_SPRITE.RESOURCE[mineSavegame.ContinentIndex][mineSavegame.MineIndex];
			int num5 = mineSavegame.ElevatorLevel + mineSavegame.GroundLevel;
			int num6 = 1 + ((num5 > 0) ? DataManager.Instance.WarehouseParams[num5].NumberOfWorkers : 0);
			int num7 = 0;
			for (int j = 0; j < mineSavegame.CorridorLevel.Count; j++)
			{
				int num8 = mineSavegame.CorridorLevel[j];
				if (num8 != 0)
				{
					num5 += num8;
					num6 += DataManager.Instance.CorridorEntityParams[j + 1][num8].NumberOfWorkers;
					num7++;
				}
			}
			int num9 = mineSavegame.CorridorManagerDictionary.Count + mineSavegame.ElevatorManagerDictionary.Count + mineSavegame.GroundManagerDictionary.Count;
			double mineFactor = GetMineFactor(i);
			MineValueText[i].text = DATA_RESOURCES.TEXT_SPRITE.SPRITE[mineSavegame.ContinentIndex] + " " + mineSavegame.IdleCash.MinifyFormat() + "/s\n" + DATA_RESOURCES.TEXT_SPRITE.MULTI + mineFactor.MinifyIncomeFactor() + "x\n" + num5 + "\n" + num6 + "\n" + num9 + "\n" + num7;
			num += num5;
			num2 += num6;
			num3 += num9;
			num4 += num7;
		}
		for (int k = DataManager.Instance.SavegameData.Mines.Count; k < MineTitleText.Length; k++)
		{
			MineTitleText[k].gameObject.SetActive(value: false);
			MineDescriptionText[k].gameObject.SetActive(value: false);
			MineValueText[k].gameObject.SetActive(value: false);
		}
		GeneralText.text = num + "\n" + num2 + "\n" + num3 + "\n" + num4;
	}
}
