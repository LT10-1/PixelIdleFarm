using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogManagerSell : BaseDialog
{
	public CoinController PriceText;

	public TMP_Text ManagerName;

	public TMP_Text ManagerType;

	public TMP_Text ManagerDescription;

	public Image ManagerAvatar;

	public UIButtonController ButtonSellNormal;

	public UIButtonController ButtonSellWithAd;

	[HideInInspector]
	public double PriceCash;

	[HideInInspector]
	public Action OnSellManagerCallback;

	public override void Start()
	{
		base.Start();
		base.BackgroundDialog.SetTitle("Sell Manager?");
		ButtonSellNormal.onClick.AddListener(delegate
		{
			OnSellManager(PriceCash / 2.0);
		});
		ButtonSellWithAd.OnClickCallback = delegate
		{
			OnSellManager(PriceCash);
		};
	}

	public void OnShow(double cash, string name, ManagerEntity.Param ManagerParam)
	{
		base.OnShow();
		PriceCash = cash;
		PriceText.SetCoinType(base.CurrentCoinType);
		PriceText.SetMoney(PriceCash, minify: true, showMoney: true, string.Empty);
		ManagerName.text = name;
		ManagerType.text = DATA_TEXT.MANAGER_RARITY[ManagerParam.RarityID - 1];
		ManagerType.color = COLOR.COLOR_MANAGER_RARITY[ManagerParam.RarityID - 1];
		ManagerAvatar.sprite = DataUtils.GetAvatarSprite(ManagerParam.Area, ManagerParam.RarityID);
		double num = ManagerParam.ValueX;
		if (ManagerParam.ValueX < 1.0)
		{
			num = (int)((1.0 - num) * 100.0);
		}
		ManagerDescription.text = string.Format(DATA_TEXT.MANAGER_EFFECT_DESCRIPTION[(int)DataUtils.GetManagerEffect(ManagerParam.EffectID)], num);
	}

	public void OnSellManager(double price)
	{
		AddCash(price);
		BaseController.GameController.ToastController.StartToast(string.Format("Sell Manager For {0}", base.CURRENT_CASH_SPRITE + " " + price.MinifyFormat()));
		if (OnSellManagerCallback != null)
		{
			OnSellManagerCallback();
		}
		OnHide();
	}
}
