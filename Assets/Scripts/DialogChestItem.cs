using UnityEngine.UI;

public class DialogChestItem : BaseController
{
	public Button ButtonBuy;

	public ChestType ChestType;

	public CoinController ChestPrice;

	public override void Awake()
	{
		base.Awake();
		CollectiblePartsChestsEntity.Param param = DataManager.Instance.ChestDictionary[(int)ChestType];
		ChestPrice.SetMoney(param.SuperCashCost, minify: false, showMoney: true, string.Empty);
		ButtonBuy.onClick.AddListener(OnClickButtonBuy);
	}

	public void OnClickButtonBuy()
	{
		if (!CheckEnoughSuperCash(ChestPrice.Cash))
		{
			BaseController.GameController.DialogController.DialogNotEnoughSuperCash.OnShow();
		}
		else
		{
			BaseController.GameController.DialogController.DialogChestConfirm.OnShow(ChestType);
		}
	}
}
