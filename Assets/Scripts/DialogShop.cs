using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DialogShop : BaseDialog
{
	public DialogSuperShopIAPPanel GoldPackPanel;

	public Transform ShopBoostContent;

	public Transform InstantCashContent;

	public UIButtonController ButtonOpenSuperShop;

	[HideInInspector]
	public Dictionary<int, DialogShopItem> DialogShopBoostItems;

	public override void Start()
	{
		base.Start();
		base.BackgroundDialog.SetTitle("Shop");
		ButtonOpenSuperShop.OnClickCallback = BaseController.GameController.MineController.OnClickSuperShop;
		BaseController.GameController.OnItemChangeCallback.Add(UpdateData);
		GoldPackPanel.gameObject.SetActive(!DataManager.Instance.SavegameData.PurchasedIAPPackage.Contains("package.gold1"));
		GoldPackPanel.Button.onClick.AddListener(delegate
		{
			DialogController.DialogIAPContent.OnShow("package.gold1");
		});
	}

	public override void OnShow()
	{
		base.OnShow();
		UpdateData();
	}

	public override void OnHide()
	{
		base.OnHide();
		BaseController.GameController.TutorialController.RemoveTutorial(TutorialType.ShopUseFirstItem);
	}

	public void UpdateData()
	{
		UpdateBoostItem();
	}

	public void UpdateBoostItem()
	{
		if (DialogShopBoostItems == null)
		{
			DialogShopBoostItems = new Dictionary<int, DialogShopItem>();
		}
		foreach (KeyValuePair<int, DialogShopItem> dialogShopBoostItem in DialogShopBoostItems)
		{
			dialogShopBoostItem.Value.gameObject.SetActive(value: false);
		}
		foreach (KeyValuePair<int, ItemsEntity.Param> item in DataManager.Instance.ShopItemDictionary)
		{
			int key = item.Key;
			ItemsEntity.Param value = item.Value;
			if (!DialogShopBoostItems.ContainsKey(key))
			{
				CreateShopItem(key, value);
			}
			DialogShopItem dialogShopItem = DialogShopBoostItems[key];
			dialogShopItem.SetParam(value);
			dialogShopItem.SetBuyType(isBuy: true);
			dialogShopItem.gameObject.SetActive(value: true);
		}
		foreach (KeyValuePair<int, int> item2 in DataManager.Instance.SavegameData.Inventory)
		{
			int key2 = item2.Key;
			int value2 = item2.Value;
			ItemsEntity.Param param = DataManager.Instance.ItemDictionary[key2];
			if (!DataManager.Instance.ShopItemDictionary.ContainsKey(key2))
			{
				CreateShopItem(key2, param);
			}
			DialogShopItem dialogShopItem2 = DialogShopBoostItems[key2];
			dialogShopItem2.SetParam(param);
			dialogShopItem2.SetBuyType(isBuy: false);
			dialogShopItem2.ButtonUseTextRemain.text = $"{value2} Left";
			dialogShopItem2.gameObject.SetActive(value: true);
		}
		LayoutRebuilder.ForceRebuildLayoutImmediate(ShopBoostContent.GetComponent<RectTransform>());
		LayoutRebuilder.ForceRebuildLayoutImmediate(InstantCashContent.GetComponent<RectTransform>());
		if (DataManager.Instance.SavegameData.CurrentMineUnlocked == 0 && DataManager.Instance.SavegameData.Inventory.Count == 1 && DataManager.Instance.SavegameData.Inventory.First().Key == DataManager.Instance.ShopItemDictionary.First().Key && DataManager.Instance.SavegameData.Inventory.First().Value == 1)
		{
			BaseController.GameController.TutorialController.CreateTutorial(TutorialType.ShopUseFirstItem, DialogShopBoostItems[DataManager.Instance.ShopItemDictionary.First().Key].ButtonUse.gameObject, isUI: true);
		}
	}

	public DialogShopItem CreateShopItem(int itemID, ItemsEntity.Param param)
	{
		DialogShopItem component = InstantiatePrefab("Prefabs/Dialog/Component/DialogShopItem").GetComponent<DialogShopItem>();
		component.transform.SetParent((param.ItemType != 2) ? ShopBoostContent : InstantCashContent, worldPositionStays: false);
		DialogShopBoostItems[itemID] = component;
		component.ButtonBuy.OnClickCallback = delegate
		{
			OnClickBuyItem(itemID);
		};
		component.ButtonUse.OnClickCallback = delegate
		{
			OnClickUseItem(itemID);
		};
		return component;
	}

	public void OnClickBuyItem(int itemID)
	{
		ItemsEntity.Param param = DataManager.Instance.ItemDictionary[itemID];
		UseSuperCash(param.SuperCashCost, SpendSuperCashType.PurchaseBoostItem, delegate
		{
			AddItem(itemID, showEffect: false);
			UseItem(itemID);
		}, param);
	}

	public void OnClickUseItem(int itemID)
	{
		UseItem(itemID);
		BaseController.GameController.TutorialController.RemoveTutorial(TutorialType.ShopUseFirstItem);
	}
}
