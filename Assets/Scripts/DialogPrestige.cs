using TMPro;
using UnityEngine;

public class DialogPrestige : BaseDialog
{
	public TMP_Text RestartMineText;

	public TMP_Text CurrentFactor;

	public TMP_Text NextFactor;

	public TMP_Text SuperCashReward;

	public TMP_Text CashIcon;

	public TMP_Text CashIconText;

	public CoinController PrestigeCost;

	public UIButtonController PrestigeButton;

	[HideInInspector]
	public int ContinentID;

	[HideInInspector]
	public int MineID;

	[HideInInspector]
	public double SuperReward;

	public override void Start()
	{
		base.Start();
		PrestigeButton.OnClickCallback = OnClickPrestige;
	}

	public void Init(int continentID, int mineID)
	{
		ContinentID = continentID;
		MineID = mineID;
		int num = BaseController.MineOrder(ContinentID, MineID);
		MineSavegame mineSavegame = DataManager.Instance.SavegameData.Mines[num];
		MineFactorsEntity.Param param = DataManager.Instance.MineFactorsEntityParams[mineSavegame.MineIndex][mineSavegame.PrestigeCount + 1];
		base.BackgroundDialog.SetTitle(string.Format("{0} Prestige", DATA_RESOURCES.TEXT_SPRITE.RESOURCE[ContinentID][MineID] + " " + DATA_TEXT.MINES.CONTINENT_MINES[ContinentID][MineID] + " Mine"));
		base.BackgroundDialog.Title1.fontSize = 45f;
		RestartMineText.text = "Restart your " + DATA_TEXT.MINES.CONTINENT_MINES[ContinentID][MineID] + " Mine";
		CurrentFactor.text = DATA_RESOURCES.TEXT_SPRITE.MULTI + " " + GetMineFactor(num).MinifyIncomeFactor() + "x";
		NextFactor.text = DATA_RESOURCES.TEXT_SPRITE.MULTI + " " + GetMineFactor(num, mineSavegame.PrestigeCount + 1).MinifyIncomeFactor() + "x";
		SuperReward = param.SuperCashGained;
		SuperCashReward.text = "+ " + DATA_RESOURCES.TEXT_SPRITE.SUPER_CASH + " " + SuperReward;
		CashIcon.text = DATA_RESOURCES.TEXT_SPRITE.SPRITE[ContinentID];
		CashIconText.text = DATA_TEXT.CASH.LIST[ContinentID];
		PrestigeCost.SetCoinType((CoinType)ContinentID);
		PrestigeCost.SetMoney(param.Cost * (1.0 - BaseController.GameController.SkillController.PrestigeCostReduction / 100.0), minify: true, showMoney: true, string.Empty);
	}

	public void OnClickPrestige()
	{
		int mineOrder = BaseController.MineOrder(ContinentID, MineID);
		if (UseCash(PrestigeCost.Cash, (ContinentType)ContinentID))
		{
			OnHide();
			AddSuperCash(SuperReward);
			BaseController.GameController.PrestigeMine(mineOrder);
			CreateReceiveEffectSuperCash("Prestige Reward", SuperReward, delegate
			{
				BaseController.GameController.GoToMine(mineOrder);
			});
		}
	}

	public override void Update()
	{
		base.Update();
		PrestigeCost.SetMoneyColor(MathUtils.CompareDoubleBiggerThanZero(DataManager.Instance.CashByContinent((ContinentType)ContinentID) - PrestigeCost.Cash));
	}
}
