using TMPro;
using UnityEngine.UI;

public class DialogIAPContentItem : BaseController
{
	public Image ItemIcon;

	public TMP_Text ItemIconText;

	public TMP_Text TextDescription;

	public TMP_Text TextAmount;

	public void Init(ItemsEntity.Param param)
	{
		ItemIconText.gameObject.SetActive(param.ItemType == 2);
		ItemIcon.gameObject.SetActive(!ItemIconText.gameObject.activeSelf);
		TextAmount.gameObject.SetActive(param.ItemType != 0);
		switch (param.ItemType)
		{
		case 0:
			ItemIcon.sprite = BaseController.LoadSprite(DATA_RESOURCES.IMAGE.CASH_ICON_LIST[DATA_RESOURCES.IMAGE.CASH_ICON_LIST.Length - 1]);
			TextDescription.text = $"{param.SuperCashAmount} Super Cash";
			break;
		case 1:
			ItemIcon.sprite = DataUtils.GetItemBoostImage((ItemBoostMultiple)param.CompleteIncomeIncreaseFactor, (ItemBoostDuration)param.ActiveTimeSeconds);
			TextDescription.text = $"{param.CompleteIncomeIncreaseFactor}x income for {param.ActiveTimeSeconds.FormatTimeString()} in all mines";
			break;
		case 2:
			ItemIconText.text = CoinStringByInstantTime(param.InstantCashTime);
			TextDescription.text = $"{param.InstantCashTime.FormatTimeString()} Instant Cash item";
			break;
		}
	}
}
