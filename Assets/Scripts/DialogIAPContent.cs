using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class DialogIAPContent : BaseDialog
{
	public Image AuraImage;

	public Image ContentImage;

	public TMP_Text HotText;

	public Transform StarGroup;

	public IAPButton buttonPurchase;

	public UIButtonController ButtonClose;

	public TMP_Text TitleText;

	public Transform ContentGroup;

	[HideInInspector]
	public Action<Product> SuccessAction;

	[HideInInspector]
	public List<DialogIAPContentItem> DialogIapContentItems = new List<DialogIAPContentItem>();

	protected override float OnShowScaleAmount => 0.01f;

	public override void Awake()
	{
		base.Awake();
		int num = 10;
		for (int i = 0; i < num; i++)
		{
			StarUIEffect component = InstantiatePrefab("Prefabs/Effects/StarUIEffect").GetComponent<StarUIEffect>();
			component.transform.SetParent(StarGroup, worldPositionStays: false);
			component.init(new Vector2(0f - CONST.SCREEN_PIXEL_WIDTH, CONST.SCREEN_PIXEL_HEIGHT) / 2f, new Vector2(CONST.SCREEN_PIXEL_WIDTH, CONST.SCREEN_PIXEL_HEIGHT));
		}
		buttonPurchase.onPurchaseComplete.AddListener(onPurchaseSuccess);
		buttonPurchase.onPurchaseFailed.AddListener(onPurchaseFail);
		ButtonClose.OnClickCallback = OnHide;
		buttonPurchase.GetComponent<Button>().onClick.AddListener(delegate
		{
			BaseController.GameController.StartLoading();
		});
	}

	public override void Start()
	{
		base.Start();
	}

	public void OnShow(string packageID, Action<Product> successCallback = null)
	{
		base.OnShow();
		buttonPurchase.productId = packageID;
		SuccessAction = successCallback;
		switch (buttonPurchase.productId)
		{
		case "x2.cashincome":
		{
			buttonPurchase.GetComponentInChildren<TMP_Text>().text = "$" + 6.99f;
			TitleText.text = "2x Cash Income";
			ContentImage.sprite = BaseController.LoadSprite("Images/UI/Shop/C");
			HotText.transform.parent.gameObject.SetActive(value: false);
			CreateIAPContentItems(3);
			DialogIAPContentItem dialogIAPContentItem = DialogIapContentItems[0];
			DialogIAPContentItem dialogIAPContentItem2 = DialogIapContentItems[1];
			DialogIAPContentItem dialogIAPContentItem3 = DialogIapContentItems[2];
			dialogIAPContentItem.ItemIcon.gameObject.SetActive(value: false);
			dialogIAPContentItem.TextAmount.gameObject.SetActive(value: false);
			dialogIAPContentItem.ItemIconText.gameObject.SetActive(value: true);
			dialogIAPContentItem.ItemIconText.text = base.CoinStringMultipleCash;
			dialogIAPContentItem.TextDescription.text = "Double the cash income in all your mines";
			dialogIAPContentItem2.ItemIconText.gameObject.SetActive(value: false);
			dialogIAPContentItem2.TextAmount.gameObject.SetActive(value: false);
			dialogIAPContentItem2.ItemIcon.gameObject.SetActive(value: true);
			dialogIAPContentItem2.ItemIcon.sprite = BaseController.LoadSprite("Images/UI/Buttons/icon_idle");
			dialogIAPContentItem2.TextDescription.text = "Gain 2x idle cash";
			dialogIAPContentItem3.ItemIcon.gameObject.SetActive(value: false);
			dialogIAPContentItem3.TextAmount.gameObject.SetActive(value: true);
			dialogIAPContentItem3.ItemIconText.gameObject.SetActive(value: true);
			dialogIAPContentItem3.ItemIconText.text = DATA_RESOURCES.TEXT_SPRITE.CLOCK;
			dialogIAPContentItem3.TextDescription.text = "Purchase will be active forever";
			dialogIAPContentItem3.TextAmount.text = DATA_RESOURCES.TEXT_SPRITE.INFINITY;
			break;
		}
		case "package.starter1":
			TitleText.text = "Starter Pack";
			ContentImage.sprite = BaseController.LoadSprite("Images/UI/Prestige/prestige");
			HotText.transform.parent.gameObject.SetActive(value: true);
			HotText.text = "3x\n<size=30>value";
			CreateIAPItemFromData(packageID);
			break;
		case "package.gold1":
			TitleText.text = "Gold Pack";
			ContentImage.sprite = BaseController.LoadSprite("Images/UI/Shop/IC");
			HotText.transform.parent.gameObject.SetActive(value: true);
			HotText.text = "5x\n<size=30>value";
			CreateIAPItemFromData(packageID);
			break;
		default:
			OnHide();
			return;
		}
		buttonPurchase.transform.SetAsLastSibling();
	}

	public void CreateIAPItemFromData(string packageID)
	{
		if (DataManager.Instance.IAPPackageDictionary.ContainsKey(packageID))
		{
			IAPPackageEntity.Param param = DataManager.Instance.IAPPackageDictionary[packageID];
			int num = param.ItemIDList.Count;
			buttonPurchase.GetComponentInChildren<TMP_Text>().text = "$" + ((float)param.IAPCost - 0.01f);
			if (param.SuperCashGain > 0.0)
			{
				num++;
			}
			CreateIAPContentItems(num);
			for (int i = 0; i < param.ItemList.Count; i++)
			{
				DialogIAPContentItem dialogIAPContentItem = DialogIapContentItems[i];
				KeyValuePair<int, int> keyValuePair = param.ItemList.ElementAt(i);
				ItemsEntity.Param param2 = DataManager.Instance.ItemDictionary[keyValuePair.Key];
				dialogIAPContentItem.Init(param2);
				dialogIAPContentItem.TextAmount.text = "x " + keyValuePair.Value;
			}
			if (param.SuperCashGain > 0.0)
			{
				DialogIapContentItems[num - 1].Init(new ItemsEntity.Param
				{
					ItemType = 0,
					SuperCashAmount = param.SuperCashGain
				});
			}
		}
	}

	public void CreateIAPContentItems(int count)
	{
		for (int i = 0; i < count; i++)
		{
			if (DialogIapContentItems.Count <= i)
			{
				DialogIapContentItems.Add(InstantiatePrefab("Prefabs/Dialog/Component/DialogIAPContentItem").GetComponent<DialogIAPContentItem>());
			}
			DialogIAPContentItem dialogIAPContentItem = DialogIapContentItems[i];
			dialogIAPContentItem.transform.SetParent(ContentGroup, worldPositionStays: false);
			dialogIAPContentItem.gameObject.SetActive(value: true);
		}
		for (int j = count; j < DialogIapContentItems.Count; j++)
		{
			DialogIapContentItems[j].gameObject.SetActive(value: false);
		}
	}

	public void onPurchaseSuccess(Product p)
	{
		BaseController.GameController.StopLoading();
		MonoBehaviour.print("onPurchaseSuccess " + p.transactionID);
		MonoBehaviour.print("onPurchaseSuccess " + buttonPurchase.productId);
		OnHide();
		if (buttonPurchase.productId == "x2.cashincome")
		{
			BaseController.GameController.DialogController.DialogSuperShop.OnPurchasex2IncomeSuccess(p);
		}
		else
		{
			IAPPackageEntity.Param param = DataManager.Instance.IAPPackageDictionary[buttonPurchase.productId];
			PurchaseProductSuccess(buttonPurchase.productId, p.transactionID, param.IAPCost);
			DataManager.Instance.SavegameData.PurchasedIAPPackage.Add(buttonPurchase.productId);
			AddMultiItem(param.ItemList, param.SuperCashGain);
			switch (buttonPurchase.productId)
			{
			case "package.starter1":
				DialogController.DialogSuperShop.StarterPack.gameObject.SetActive(value: false);
				break;
			case "package.gold1":
				DialogController.DialogShop.GoldPackPanel.gameObject.SetActive(value: false);
				break;
			}
		}
		if (SuccessAction != null)
		{
			SuccessAction(p);
		}
	}

	public void onPurchaseFail(Product p, PurchaseFailureReason reason)
	{
		BaseController.GameController.StopLoading();
		OnHide();
		MonoBehaviour.print("onPurchaseFail " + reason + " --- " + p.transactionID);
	}
}
