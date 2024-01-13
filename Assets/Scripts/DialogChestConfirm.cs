using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogChestConfirm : BaseDialog
{
	public TMP_Text ChestNameText;

	public Image ChestRibbon;

	public TMP_Text TreasureIcon;

	public TMP_Text ContainText;

	public TMP_Text CardNumber;

	public VerticalLayoutGroup ChestVerticalLayoutGroup;

	public List<TMP_Text> ChestContainText;

	public UIButtonController ButtonBuy;

	public CoinController ChestCost;

	[HideInInspector]
	public ChestType ChestType;

	public override void Start()
	{
		base.Start();
		base.BackgroundDialog.SetTitle("Confirm Purchase");
		ButtonBuy.OnClickCallback = OnClickBuyChest;
	}

	public void OnShow(ChestType chestType)
	{
		base.OnShow();
		ChestType = chestType;
		int chestType2 = (int)ChestType;
		ChestNameText.text = DATA_TEXT.COLLECTIBLES.LIST[chestType2] + " Chest";
		ChestRibbon.overrideSprite = BaseController.LoadSprite(DATA_RESOURCES.IMAGE.WORKSHOP_RIBBON_LIST[chestType2]);
		ContainText.text = ((ChestType != 0) ? "Contains at least" : "Chest contains:");
		TreasureIcon.text = DATA_RESOURCES.TEXT_SPRITE.CHEST[chestType2];
		CollectiblePartsChestsEntity.Param param = DataManager.Instance.ChestDictionary[chestType2];
		Dictionary<int, CollectiblePartsLootTablesEntity.Param> dictionary = DataManager.Instance.ChestLootTableDictionary[chestType2];
		ChestCost.SetMoney(param.SuperCashCost, minify: false, showMoney: true, string.Empty);
		CardNumber.text = param.PartsInChest + " Cards";
		ChestContainText.ForEach(delegate(TMP_Text chestText)
		{
			chestText.transform.parent.gameObject.SetActive(value: false);
		});
		if (ChestType == ChestType.Normal || ChestType == ChestType.Rare)
		{
			ChestVerticalLayoutGroup.spacing = 200f;
		}
		else
		{
			ChestVerticalLayoutGroup.spacing = 10f;
		}
		if (ChestType == ChestType.Normal)
		{
			ChestContainText[0].transform.parent.gameObject.SetActive(value: true);
			ChestContainText[0].text = $"{param.PartsInChest} random cards";
		}
		foreach (KeyValuePair<int, CollectiblePartsLootTablesEntity.Param> item in dictionary)
		{
			if (item.Key != 0)
			{
				TMP_Text tMP_Text = ChestContainText[item.Key];
				if (item.Value.MinAmount > 0)
				{
					tMP_Text.transform.parent.gameObject.SetActive(value: true);
					tMP_Text.text = $"{item.Value.MinAmount} {DATA_TEXT.COLLECTIBLES.LIST[item.Key]} Card";
					if (item.Value.MinAmount > 1)
					{
						tMP_Text.text += "s";
					}
				}
				if (ChestType == ChestType.Epic && item.Key == 3)
				{
					tMP_Text.transform.parent.gameObject.SetActive(value: true);
					tMP_Text.text = $"{10}% chance to get a Legendary Card";
				}
			}
		}
	}

	public void OnClickBuyChest()
	{
		if (!CheckEnoughSuperCash(ChestCost.Cash))
		{
			BaseController.GameController.DialogController.DialogNotEnoughSuperCash.OnShow();
			return;
		}
		UseSuperCashSuccess(ChestCost.Cash);
		AddChest(ChestType, 1, showEffect: false);
		BaseController.GameController.DialogController.OpenChest.OnShow();
		BaseController.GameController.DialogController.OpenChest.GotoChestType(ChestType, showAnimation: false);
	}
}
