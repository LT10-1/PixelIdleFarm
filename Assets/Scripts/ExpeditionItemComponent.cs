using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpeditionItemComponent : BaseController
{
	public GameObject ItemDetailGroup;

	public GameObject QuestionMark;

	public Image IconImage;

	public TMP_Text IconText;

	public TMP_Text LeftText;

	public TMP_Text MiddleText;

	public TMP_Text RightText;

	[HideInInspector]
	public int ItemID;

	[HideInInspector]
	public ExpeditionComponent ExpeditionComponent;

	public ItemsEntity.Param ItemParam => DataManager.Instance.ItemDictionary[ItemID];

	public void SetShowItemDetail(bool showDetail)
	{
		QuestionMark.SetActive(!showDetail);
		ItemDetailGroup.SetActive(showDetail);
	}

	public void SetData(int itemId)
	{
		ItemID = itemId;
		switch (ItemParam.ItemType)
		{
		case 3:
			break;
		case 4:
			break;
		case 5:
			break;
		case 0:
			IconImage.overrideSprite = BaseController.LoadSprite(DATA_RESOURCES.IMAGE.CASH_ICON_LIST[1]);
			MiddleText.text = "Up to " + ExpeditionComponent.ExpeditionParam.SuperCashMax.MinifyFormat() + " " + DATA_RESOURCES.TEXT_SPRITE.SUPER_CASH;
			IconImage.gameObject.SetActive(value: true);
			MiddleText.gameObject.SetActive(value: true);
			IconText.gameObject.SetActive(value: false);
			LeftText.gameObject.SetActive(value: false);
			RightText.gameObject.SetActive(value: false);
			break;
		case 1:
			IconImage.overrideSprite = DataUtils.GetItemBoostImage((ItemBoostMultiple)ItemParam.CompleteIncomeIncreaseFactor, (ItemBoostDuration)ItemParam.ActiveTimeSeconds);
			RightText.text = DATA_RESOURCES.TEXT_SPRITE.CLOCK + string.Empty + ItemParam.ActiveTimeSeconds.FormatTimeString();
			LeftText.text = ItemParam.CompleteIncomeIncreaseFactor + "x" + DATA_RESOURCES.TEXT_SPRITE.MULTI;
			IconImage.gameObject.SetActive(value: true);
			LeftText.gameObject.SetActive(value: true);
			RightText.gameObject.SetActive(value: true);
			MiddleText.gameObject.SetActive(value: false);
			IconText.gameObject.SetActive(value: false);
			break;
		case 2:
			MiddleText.text = ItemParam.InstantCashTime.FormatTimeString();
			IconText.text = base.CURRENT_CASH_SPRITE;
			MiddleText.gameObject.SetActive(value: true);
			IconText.gameObject.SetActive(value: true);
			IconImage.gameObject.SetActive(value: false);
			LeftText.gameObject.SetActive(value: false);
			RightText.gameObject.SetActive(value: false);
			break;
		}
	}
}
