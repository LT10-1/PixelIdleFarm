using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class DialogSuperShopItem : BaseController
{
	public Image CashIcon;

	public IAPButton iapButton;

	public CoinController SuperCashGain;

	public TMP_Text BuyValue;

	private double superCash;

	public void setProduct(string id, string buyValue, double superCash)
	{
		iapButton.productId = id;
		iapButton.GetComponent<Button>().onClick.AddListener(delegate
		{
			BaseController.GameController.StartLoading();
		});
		SuperCashGain.SetMoney(superCash, minify: false, showMoney: true, string.Empty);
		BuyValue.text = buyValue;
		this.superCash = superCash;
	}

	public void onPurchaseSuccess(Product p)
	{
		BaseController.GameController.StopLoading();
		MonoBehaviour.print("onPurchaseSuccess " + p.transactionID);
		PurchaseProductSuccess(iapButton.productId, p.transactionID, Mathf.RoundToInt(float.Parse(BuyValue.text.Substring(1))));
		AddSuperCash(superCash);
		CreateReceiveEffectSuperCash("You bought", superCash);
	}

	public void onPurchaseFalse(Product p, PurchaseFailureReason reason)
	{
		BaseController.GameController.StopLoading();
		MonoBehaviour.print("onPurchaseFalse " + p.transactionID);
	}
}
