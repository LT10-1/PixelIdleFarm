using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogConfirmPurchase : BaseDialog
{
	public VerticalLayoutGroup VerticalLayoutGroup;

	public Image PurchaseIcon;

	public Image PurchaseLockIcon;

	public UIButtonController ButtonConfirm;

	public TMP_Text CashIcon;

	public TMP_Text SkillIcon;

	public GameObject Skillx10Icon;

	public TMP_Text TextDescription;

	public TMP_Text TextConfirm;

	public Action OnClickConfirm;

	public override void Start()
	{
		base.Start();
		ButtonConfirm.OnClickCallback = delegate
		{
			OnHide();
			if (OnClickConfirm != null)
			{
				OnClickConfirm();
			}
		};
	}

	public void OnShow(double superCashCost, SpendSuperCashType spendSuperCashType, ItemsEntity.Param itemParam, DialogSkillShopItem dialogSkillShopItem)
	{
		OnShow();
		base.BackgroundDialog.SetTitle("Confirm Purchase");
		PurchaseLockIcon.gameObject.SetActive(spendSuperCashType == SpendSuperCashType.UnlockTierRock);
		TextDescription.gameObject.SetActive(spendSuperCashType == SpendSuperCashType.PurchaseBoostItem);
		VerticalLayoutGroup.spacing = ((spendSuperCashType == SpendSuperCashType.PurchaseBoostItem) ? 20 : 0);
		CashIcon.gameObject.SetActive(spendSuperCashType == SpendSuperCashType.PurchaseBoostItem && itemParam.ItemType == 2);
		SkillIcon.gameObject.SetActive(spendSuperCashType == SpendSuperCashType.PurchaseSkillPoint);
		Skillx10Icon.gameObject.SetActive(value: false);
		PurchaseIcon.gameObject.SetActive(!CashIcon.gameObject.activeSelf && !SkillIcon.gameObject.activeSelf);
		switch (spendSuperCashType)
		{
		case SpendSuperCashType.UnlockMineShaft:
			PurchaseIcon.sprite = BaseController.LoadSprite("Images/UI/Purchase/Mine");
			TextConfirm.text = string.Format(DATA_TEXT.CONFIRM_BUY_SHAFT, superCashCost);
			break;
		case SpendSuperCashType.UnlockTierRock:
			PurchaseIcon.sprite = BaseController.LoadSprite("Images/UI/Purchase/Shaft");
			TextConfirm.text = string.Format(DATA_TEXT.CONFIRM_BREAK_TIER_ROCK, superCashCost);
			break;
		case SpendSuperCashType.PurchaseBoostItem:
			if (itemParam.ItemType == 1)
			{
				PurchaseIcon.sprite = DataUtils.GetItemBoostImage((ItemBoostMultiple)itemParam.CompleteIncomeIncreaseFactor, (ItemBoostDuration)itemParam.ActiveTimeSeconds);
				TextConfirm.text = string.Format(DATA_TEXT.CONFIRM_BUY_BOOST_ITEM, itemParam.CompleteIncomeIncreaseFactor, superCashCost);
				TextDescription.text = string.Format(DATA_TEXT.CONFIRM_BUY_BOOST_ITEM_DETAIL, itemParam.CompleteIncomeIncreaseFactor, itemParam.ActiveTimeSeconds.FormatTimeString());
			}
			else if (itemParam.ItemType == 2)
			{
				CashIcon.text = CoinStringByInstantTime(itemParam.InstantCashTime);
				TextConfirm.text = string.Format(DATA_TEXT.CONFIRM_BUY_INSTANT_CASH, superCashCost);
				TextDescription.text = string.Format(DATA_TEXT.CONFIRM_BUY_INSTANT_CASH_DETAIL, CashFromInstantTime(itemParam.InstantCashTime).MinifyFormat() + " " + base.CURRENT_CASH_SPRITE);
			}
			break;
		case SpendSuperCashType.PurchaseSkillPoint:
			SkillIcon.text = dialogSkillShopItem.ItemText.text;
			Skillx10Icon.SetActive(dialogSkillShopItem.IsBuyx10);
			TextConfirm.text = string.Format(DATA_TEXT.CONFIRM_BUY_SKILL_POINT, SkillIcon.text + ((!dialogSkillShopItem.IsBuyx10) ? 1 : 10), superCashCost);
			break;
		}
		PurchaseIcon.SetNativeSize();
		PurchaseIcon.transform.localScale = Vector3.one;
	}

	public void OnShow(string title, string topText, string bottomText, string imageURL, float scale = 1f)
	{
		OnShow();
		base.BackgroundDialog.SetTitle(title);
		PurchaseLockIcon.gameObject.SetActive(value: false);
		Skillx10Icon.gameObject.SetActive(value: false);
		SkillIcon.gameObject.SetActive(value: false);
		CashIcon.gameObject.SetActive(value: false);
		TextDescription.gameObject.SetActive(value: true);
		TextDescription.text = topText;
		TextConfirm.text = bottomText;
		PurchaseIcon.gameObject.SetActive(value: true);
		PurchaseIcon.sprite = BaseController.LoadSprite(imageURL);
		PurchaseIcon.SetNativeSize();
		PurchaseIcon.GetComponent<RectTransform>().sizeDelta = scale * PurchaseIcon.GetComponent<RectTransform>().sizeDelta;
	}
}
