using TMPro;
using UnityEngine;

public class CoinButtonController : SpriteButtonController
{
	private TextMeshPro TextMeshPro;

	[HideInInspector]
	public CoinController CoinController;

	public double Cash => CoinController.Cash;

	private void Awake()
	{
		TextMeshPro = GetComponentInChildren<TextMeshPro>(includeInactive: true);
		CoinController = GetComponentInChildren<CoinController>(includeInactive: true);
		TextMeshPro.SetText("New shaft");
	}

	public void SetMoney(double cash, bool minify = true)
	{
		CoinController.SetMoney(cash, minify, showMoney: true, string.Empty);
	}

	public void SetBuyAble(bool buyAble)
	{
		CoinController.SetMoneyColor(buyAble);
	}
}
