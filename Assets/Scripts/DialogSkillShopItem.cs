using System;
using TMPro;
using UnityEngine;

public class DialogSkillShopItem : BaseController
{
	public CoinController BuyCost;

	public TMP_Text ItemText;

	public GameObject IconX10;

	public GameObject IconLock;

	public UIButtonController ButtonBuy;

	public TMP_Text UnlockText;

	[HideInInspector]
	public ContinentType ContinentType;

	[HideInInspector]
	public bool IsBuyx10;

	[HideInInspector]
	public bool BuyWithSuperCash;

	[HideInInspector]
	public bool IsMaxSkillPoint;

	public bool IsUnlocked => DataManager.Instance.SavegameData.CurrentContinentUnlocked >= (int)ContinentType;

	public override void Start()
	{
		base.Start();
		ButtonBuy.OnClickCallback = OnClickBuy;
		BaseController.GameController.OnGlobalCashChangeCallback.Add(OnCashChangeCallback);
	}

	public void OnClickBuy()
	{
		if (IsMaxSkillPoint)
		{
			BaseController.GameController.ToastController.StartToast($"Skill Point {DATA_RESOURCES.TEXT_SPRITE.SKILL_POINT[(int)ContinentType]} Maxed");
		}
		else if (BuyWithSuperCash)
		{
			UseSuperCash(BuyCost.Cash, SpendSuperCashType.PurchaseSkillPoint, AddSkillPointSuccess, null, this);
		}
		else if (UseCash(BuyCost.Cash, ContinentType))
		{
			AddSkillPointSuccess();
		}
	}

	public void AddSkillPointSuccess()
	{
		int num = (!IsBuyx10) ? 1 : 10;
		if (!BuyWithSuperCash)
		{
			DataManager.Instance.SetSkillPointFromCashByContinent(DataManager.Instance.SkillPointFromCashByContinent(ContinentType) + num, ContinentType);
		}
		if (!IsBuyx10)
		{
			BaseController.GameController.AudioController.PlayOneShot("Audios/Effect/thuequanlythanhcong");
		}
		AddSkillPoint(num, ContinentType, IsBuyx10, ButtonBuy.gameObject);
	}

	public void OnCashChangeCallback()
	{
		if (!BuyWithSuperCash)
		{
			bool flag = CheckEnoughCash(BuyCost.Cash, ContinentType);
			BuyCost.SetMoneyColor(flag);
			ButtonBuy.SetButtonColorEnable(flag);
		}
	}

	public void Init(ContinentType continentType, bool isBuyx10, bool buyWithSuperCash)
	{
		ContinentType = continentType;
		IsBuyx10 = isBuyx10;
		BuyWithSuperCash = buyWithSuperCash;
		ItemText.text = DATA_RESOURCES.TEXT_SPRITE.SKILL_POINT[(int)ContinentType];
		if (BuyWithSuperCash)
		{
			BuyCost.SetCoinType(CoinType.SuperCash);
			BuyCost.SetMoney((!IsBuyx10) ? 200.0 : 2000.0, minify: false, showMoney: true, string.Empty);
		}
		else
		{
			BuyCost.SetCoinType((CoinType)ContinentType);
		}
		UnlockText.text = $"Unlock {DATA_TEXT.CONTINENT.LIST[(int)ContinentType]} Continent First";
	}

	public void UpdateData()
	{
		IconLock.gameObject.SetActive(!IsUnlocked);
		ButtonBuy.gameObject.SetActive(IsUnlocked);
		UnlockText.gameObject.SetActive(!IsUnlocked);
		IconX10.SetActive(IsUnlocked && IsBuyx10);
		if (!BuyWithSuperCash)
		{
			BuyCost.SetMoney((!IsBuyx10) ? NextSkillPointCost() : Next10SkillPointCost(), minify: true, showMoney: true, string.Empty);
			OnCashChangeCallback();
		}
		int num = (!IsBuyx10) ? 1 : 10;
		IsMaxSkillPoint = (DataManager.Instance.SkillPointNetworthByContinent(ContinentType) + num > DataManager.Instance.MaxSkillPointByContinent(ContinentType));
		ButtonBuy.text = ((!IsMaxSkillPoint) ? "Buy" : "MAX");
		if (IsMaxSkillPoint)
		{
			ButtonBuy.SetButtonColorEnable(enable: false);
		}
		BuyCost.gameObject.SetActive(IsUnlocked && !IsMaxSkillPoint);
	}

	public double NextSkillPointCost(int next = 0)
	{
		return 8.47E+15 * Math.Pow(1.7, DataManager.Instance.SkillPointFromCashByContinent(ContinentType) + next);
	}

	public double Next10SkillPointCost()
	{
		double num = 0.0;
		for (int i = 0; i < 10; i++)
		{
			num += NextSkillPointCost(i);
		}
		return num;
	}
}
