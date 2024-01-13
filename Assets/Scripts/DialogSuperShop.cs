using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class DialogSuperShop : BaseDialog
{
	public DialogSuperShopIAPPanel Incomex2Panel;

	public DialogSuperShopIAPPanel StarterPack;

	public GameObject TestProduct;

	public Transform IAPContent;

	public UIButtonController ButtonOpenShop;

	public UIButtonController ButtonDailyReward;

	public UIButtonController ButtonCoupon;

	public override void Start()
	{
		base.Start();
		base.BackgroundDialog.SetTitle("Super Shop");
		ButtonOpenShop.OnClickCallback = BaseController.GameController.MineController.OnClickShop;
		UIButtonController buttonDailyReward = ButtonDailyReward;
		DialogDailyReward dialogDailyReward = BaseController.GameController.DialogController.DialogDailyReward;
		buttonDailyReward.OnClickCallback = ((BaseDialog)dialogDailyReward).OnShow;
		ButtonCoupon.OnClickCallback = OnClickCoupon;
		Incomex2Panel.gameObject.SetActive(!DataManager.Instance.SavegameData.IAPCheckHavePurchaseIncomeX2);
		StarterPack.gameObject.SetActive(!DataManager.Instance.SavegameData.PurchasedIAPPackage.Contains("package.starter1"));
		Incomex2Panel.Button.onClick.AddListener(delegate
		{
			DialogController.DialogIAPContent.OnShow("x2.cashincome");
		});
		StarterPack.Button.onClick.AddListener(delegate
		{
			DialogController.DialogIAPContent.OnShow("package.starter1");
		});
		if (DataManager.Instance.SavegameData.IAPCheckHavePurchaseTest)
		{
			TestProduct.GetComponentInChildren<TMP_Text>(includeInactive: true).text = "Have Purchased";
		}
		TestProduct.GetComponentInChildren<IAPButton>(includeInactive: true).GetComponent<Button>().onClick.AddListener(delegate
		{
			BaseController.GameController.StartLoading();
		});
		TestProduct.SetActive(value: false);
		ProductCatalog productCatalog = ProductCatalog.LoadDefaultCatalog();
		int[] array = new int[6]
		{
			300,
			800,
			1700,
			3600,
			9500,
			20000
		};
		int[] array2 = new int[6]
		{
			2,
			5,
			10,
			23,
			50,
			100
		};
		int num = 0;
		foreach (ProductCatalogItem allProduct in productCatalog.allProducts)
		{
			if (allProduct.allStoreIDs.Count > 0)
			{
				IDs ds = new IDs();
				foreach (StoreID allStoreID in allProduct.allStoreIDs)
				{
					ds.Add(allStoreID.id, allStoreID.store);
				}
			}
			if (allProduct.id.Contains("supercash.package") && allProduct.type == ProductType.Consumable && num < array.Length)
			{
				DialogSuperShopItem component = InstantiatePrefab("Prefabs/Dialog/Component/DialogSuperShopItem").GetComponent<DialogSuperShopItem>();
				component.setProduct(allProduct.id, "$" + ((float)array2[num] - 0.01f), array[num]);
				component.CashIcon.sprite = BaseController.LoadSprite(DATA_RESOURCES.IMAGE.CASH_ICON_LIST[num]);
				component.transform.SetParent(IAPContent, worldPositionStays: false);
				num++;
			}
		}
	}

	public override void OnShow()
	{
		base.OnShow();
		//BaseController.GameController.AnalyticController.LogEvent("click_open_super_cash_shop");
	}

	public void OnPurchasex2IncomeSuccess(Product p)
	{
		BaseController.GameController.StopLoading();
		BaseController.GameController.DialogController.DialogIAPContent.OnHide();
		MonoBehaviour.print("onPurchaseSuccess " + p.transactionID);
		PurchaseProductSuccess("x2.cashincome", p.transactionID, 7);
		DataManager.Instance.SavegameData.PurchasedIAPPackage.Add("x2.cashincome");
		DialogController.DialogSuperShop.Incomex2Panel.gameObject.SetActive(value: false);
		ItemsEntity.Param param = new ItemsEntity.Param();
		param.ItemType = 1;
		param.CompleteIncomeIncreaseFactor = 1000.0;
		param.ActiveTimeSeconds = 1209600L;
		ItemsEntity.Param itemParam = param;
		ReceiveEffect receiveEffect = CreateReceiveEffect("You bought", itemParam);
		receiveEffect.Value.text = " " + ItemBoostMultiple.x2 + "x " + DATA_RESOURCES.TEXT_SPRITE.CLOCK + " " + DATA_RESOURCES.TEXT_SPRITE.INFINITY;
	}

	public void OnPurchasex2IncomeFalse(Product p, PurchaseFailureReason reason)
	{
		BaseController.GameController.StopLoading();
		MonoBehaviour.print("onPurchaseFail: " + p.transactionID + " --- " + reason);
	}

	public void OnPurchaseTestProductSuccess(Product p)
	{
		BaseController.GameController.StopLoading();
		DataManager.Instance.SavegameData.PurchasedIAPPackage.Add("test.product");
		if (TestProduct != null)
		{
			TestProduct.GetComponentInChildren<TMP_Text>(includeInactive: true).text = "Have Purchased";
		}
	}

	public void OnPurchaseTestProductFailed(Product p, PurchaseFailureReason reason)
	{
		BaseController.GameController.StopLoading();
		MonoBehaviour.print("onPurchaseFail: " + p.transactionID + " --- " + reason);
	}

	public void OnClickCoupon()
	{
		DialogController.DialogCoupon.OnShow();
	}
}
