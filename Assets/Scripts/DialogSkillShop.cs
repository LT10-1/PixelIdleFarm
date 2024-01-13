using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogSkillShop : BaseDialog
{
	public Transform ShopBuyCashContent;

	public Transform ShopBuySuperCashContent;

	public UIButtonController ButtonOpenSuperShop;

	public TMP_Text CurrentSuperCash;

	[HideInInspector]
	public List<DialogSkillShopItem> DialogSkillShopItemsCash;

	[HideInInspector]
	public List<DialogSkillShopItem> DialogSkillShopItemsSuperCash;

	public override void Start()
	{
		base.Start();
		base.BackgroundDialog.SetTitle("Skill Shop");
		ButtonOpenSuperShop.OnClickCallback = BaseController.GameController.MineController.OnClickSuperShop;
		BaseController.GameController.OnGlobalSuperCashChangeCallback.Add(OnSuperCashChange);
		BaseController.GameController.OnSkillpointChangeCallback.Add(UpdateData);
		OnSuperCashChange();
	}

	public override void OnShow()
	{
		base.OnShow();
		if (DialogSkillShopItemsCash.Count == 0)
		{
			CreateSkillItems();
		}
		UpdateData();
	}

	public void OnSuperCashChange()
	{
		CurrentSuperCash.text = DataManager.Instance.SuperCash.MinifyFormat();
	}

	public void UpdateData()
	{
		foreach (DialogSkillShopItem item in DialogSkillShopItemsCash)
		{
			item.UpdateData();
		}
		foreach (DialogSkillShopItem item2 in DialogSkillShopItemsSuperCash)
		{
			item2.UpdateData();
		}
	}

	public void CreateSkillItems()
	{
		for (int i = 0; i < 2; i++)
		{
			bool isBuyx = i == 1;
			IEnumerator enumerator = Enum.GetValues(typeof(ContinentType)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					ContinentType continentType = (ContinentType)enumerator.Current;
					CreateSkillShopItem(continentType, isBuyx, buyWithSuperCash: true);
					CreateSkillShopItem(continentType, isBuyx, buyWithSuperCash: false);
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
		}
	}

	public DialogSkillShopItem CreateSkillShopItem(ContinentType continentType, bool isBuyx10, bool buyWithSuperCash)
	{
		DialogSkillShopItem component = InstantiatePrefab("Prefabs/Dialog/Component/DialogSkillShopItem").GetComponent<DialogSkillShopItem>();
		component.Init(continentType, isBuyx10, buyWithSuperCash);
		if (buyWithSuperCash)
		{
			DialogSkillShopItemsSuperCash.Add(component);
			component.transform.SetParent(ShopBuySuperCashContent, worldPositionStays: false);
		}
		else
		{
			DialogSkillShopItemsCash.Add(component);
			component.transform.SetParent(ShopBuyCashContent, worldPositionStays: false);
		}
		return component;
	}
}
