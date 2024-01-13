using TMPro;
using UnityEngine.UI;

public class DialogShopItem : BaseController
{
	public TMP_Text BoostDuration;

	public TMP_Text BoostMultiple;

	public TMP_Text InstantCashIcon;

	public Image ItemIcon;

	public UIButtonController ButtonBuy;

	public CoinController ButtonBuyValue;

	public UIButtonController ButtonUse;

	public TMP_Text ButtonUseTextRemain;

	public void SetBuyType(bool isBuy)
	{
		ButtonBuy.gameObject.SetActive(isBuy);
		ButtonUse.gameObject.SetActive(!isBuy);
	}

	public void SetParam(ItemsEntity.Param param)
	{
		InstantCashIcon.gameObject.SetActive(param.ItemType == 2);
		InstantCashIcon.text = CoinStringByInstantTime(param.InstantCashTime);
		ItemIcon.gameObject.SetActive(param.ItemType == 1);
		ItemIcon.sprite = DataUtils.GetItemBoostImage((ItemBoostMultiple)param.CompleteIncomeIncreaseFactor, (ItemBoostDuration)param.ActiveTimeSeconds);
		ItemIcon.SetNativeSize();
		ButtonBuyValue.SetMoney(param.SuperCashCost, minify: false, showMoney: true, string.Empty);
		switch (param.ItemType)
		{
		case 2:
			BoostDuration.gameObject.SetActive(value: false);
			BoostMultiple.text = CashFromInstantTime(param.InstantCashTime).MinifyFormat() + " " + base.CURRENT_CASH_SPRITE;
			break;
		case 1:
			BoostDuration.gameObject.SetActive(value: true);
			BoostDuration.text = DATA_RESOURCES.TEXT_SPRITE.CLOCK + " " + param.ActiveTimeSeconds.FormatTimeString();
			BoostMultiple.text = param.CompleteIncomeIncreaseFactor + "x " + DATA_RESOURCES.TEXT_SPRITE.MULTI;
			break;
		}
	}
}
