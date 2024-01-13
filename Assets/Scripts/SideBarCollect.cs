using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SideBarCollect : BaseWorldmapController
{
	public UIButtonController ButtonClose;

	public UIButtonController ButtonCollectx2;

	public UIButtonController ButtonBoostx2;

	public TMP_Text TextTotalCash;

	public UIButtonController ButtonCollectSandCashx2;

	public UIButtonController ButtonBoostSandCashx2;

	public TMP_Text TextTotalSandCash;

	public UIButtonController ButtonCollectSakuraCashx2;

	public UIButtonController ButtonBoostSakuraCashx2;

	public TMP_Text TextTotalSakuraCash;

	public Transform SeperatorCash;

	public Transform SeperatorSandCash;

	public Transform ScrollViewContent;

	[HideInInspector]
	public List<SideBarCollectContent> SideBarCollectContents = new List<SideBarCollectContent>();

	[HideInInspector]
	public bool IsShow;

	[HideInInspector]
	public Tweener TweenShow;

	[HideInInspector]
	public double TotalCash;

	[HideInInspector]
	public double TotalSandCash;

	[HideInInspector]
	public double TotalSakuraCash;

	private float _OriginalX;

	public float OriginalX
	{
		get
		{
			if (_OriginalX == 0f)
			{
				Vector3 localPosition = base.transform.localPosition;
				_OriginalX = localPosition.x;
			}
			return _OriginalX;
		}
	}

	public float TweenDelta
	{
		get
		{
			float originalX = OriginalX;
			Vector3 localPosition = base.WorldMapController.SideBarCollectEndX.localPosition;
			return originalX - localPosition.x;
		}
	}

	public override void Awake()
	{
		base.Awake();
	}

	public override void Start()
	{
		base.Start();
		ButtonClose.OnClickCallback = OnClickClose;
		ButtonBoostx2.OnClickCallback = OnClickBoostx2;
		ButtonBoostSandCashx2.OnClickCallback = OnClickBoostSandx2;
		ButtonBoostSakuraCashx2.OnClickCallback = OnClickBoostSakurax2;
		ButtonCollectx2.OnClickCallback = OnClickCollectx2;
		ButtonCollectSandCashx2.OnClickCallback = OnClickCollectSandx2;
		ButtonCollectSakuraCashx2.OnClickCallback = OnClickCollectSakurax2;
	}

	public override void Update()
	{
		base.Update();
		TotalCash = 0.0;
		TotalSandCash = 0.0;
		TotalSakuraCash = 0.0;
		for (int i = 0; i < DataManager.Instance.SavegameData.Mines.Count; i++)
		{
			if (SideBarCollectContents.Count <= i)
			{
				continue;
			}
			SideBarCollectContent sideBarCollectContent = SideBarCollectContents[i];
			MineSavegame mineSavegame = DataManager.Instance.SavegameData.Mines[i];
			ContinentType continentIndex = (ContinentType)mineSavegame.ContinentIndex;
			if (!sideBarCollectContent.IsCurrentMine)
			{
				double currentIdleCash = mineSavegame.CurrentIdleCash;
				switch (continentIndex)
				{
				case ContinentType.Grass:
					sideBarCollectContent.CoinText.text = DATA_RESOURCES.TEXT_SPRITE.CASH + " " + currentIdleCash.MinifyFormat();
					TotalCash += currentIdleCash;
					break;
				case ContinentType.Sand:
					sideBarCollectContent.CoinText.text = DATA_RESOURCES.TEXT_SPRITE.SAND_CASH + " " + currentIdleCash.MinifyFormat();
					TotalSandCash += currentIdleCash;
					break;
				case ContinentType.Sakura:
					sideBarCollectContent.CoinText.text = DATA_RESOURCES.TEXT_SPRITE.SAKURA_CASH + " " + currentIdleCash.MinifyFormat();
					TotalSakuraCash += currentIdleCash;
					break;
				}
			}
			sideBarCollectContent.IsAdBoosted = (DateTime.Now.Ticks < mineSavegame.MineBoostx2EndTime);
			if (!sideBarCollectContent.IsAdBoosted)
			{
				sideBarCollectContent.BoostText.text = "Not Boosted";
			}
			else
			{
				sideBarCollectContent.BoostText.text = "Boost: " + ((long)TimeSpan.FromTicks(mineSavegame.MineBoostx2EndTime - DateTime.Now.Ticks).TotalSeconds).FormatTimeString();
			}
		}
		TextTotalCash.gameObject.SetActive(TotalCash > 0.0);
		TextTotalCash.text = "<size=60><color=\"yellow\">=</color></size>" + DATA_RESOURCES.TEXT_SPRITE.CASH + " " + TotalCash.MinifyFormat();
		TextTotalSandCash.gameObject.SetActive(TotalSandCash > 0.0);
		TextTotalSandCash.text = "<size=60><color=\"yellow\">=</color></size>" + DATA_RESOURCES.TEXT_SPRITE.SAND_CASH + " " + TotalSandCash.MinifyFormat();
		TextTotalSakuraCash.gameObject.SetActive(TotalSakuraCash > 0.0);
		TextTotalSakuraCash.text = "<size=60><color=\"yellow\">=</color></size>" + DATA_RESOURCES.TEXT_SPRITE.SAKURA_CASH + " " + TotalSakuraCash.MinifyFormat();
	}

	public void OnClickBoostx2()
	{
		Boostx2Continent(ContinentType.Grass);
		//BaseController.GameController.AnalyticController.LogEvent("watch_ad", "type", "boost_grass_continent");
	}

	public void OnClickBoostSandx2()
	{
		Boostx2Continent(ContinentType.Sand);
		//BaseController.GameController.AnalyticController.LogEvent("watch_ad", "type", "boost_sand_continent");
	}

	public void OnClickBoostSakurax2()
	{
		Boostx2Continent(ContinentType.Sakura);
		//BaseController.GameController.AnalyticController.LogEvent("watch_ad", "type", "boost_sakura_continent");
	}

	public void Boostx2Continent(ContinentType continentType)
	{
		for (int i = 0; i < DataManager.Instance.SavegameData.Mines.Count; i++)
		{
			MineSavegame mineSavegame = DataManager.Instance.SavegameData.Mines[i];
			if (mineSavegame.ContinentIndex == (int)continentType)
			{
				BaseController.GameController.BoostController.MineAdBoostx2(i);
			}
		}
	}

	public void OnClickCollectx2()
	{
		OnClickCollectContinent(ContinentType.Grass);
		//BaseController.GameController.AnalyticController.LogEvent("watch_ad", "type", "collect_grass_continent");
	}

	public void OnClickCollectSandx2()
	{
		OnClickCollectContinent(ContinentType.Sand);
		//BaseController.GameController.AnalyticController.LogEvent("watch_ad", "type", "collect_sand_continent");
	}

	public void OnClickCollectSakurax2()
	{
		OnClickCollectContinent(ContinentType.Sakura);
		//BaseController.GameController.AnalyticController.LogEvent("watch_ad", "type", "collect_sakura_continent");
	}

	public void OnClickCollectContinent(ContinentType continentType)
	{
		for (int i = 0; i < DataManager.Instance.SavegameData.Mines.Count; i++)
		{
			if (SideBarCollectContents.Count <= i)
			{
				continue;
			}
			SideBarCollectContent sideBarCollectContent = SideBarCollectContents[i];
			if (!sideBarCollectContent.IsCurrentMine)
			{
				MineSavegame mineSavegame = DataManager.Instance.SavegameData.Mines[i];
				if (mineSavegame.ContinentIndex == (int)continentType)
				{
					mineSavegame.IdleBonusCashGain = 0.0;
					mineSavegame.MineLastTimeVisit = DateTime.Now.Ticks;
				}
			}
		}
		double num = 0.0;
		switch (continentType)
		{
		case ContinentType.Grass:
			num = TotalCash * BaseController.GameController.SkillController.AdCurrentx2IdleCashFactor;
			break;
		case ContinentType.Sand:
			num = TotalSandCash * BaseController.GameController.SkillController.AdCurrentx2IdleCashFactor;
			break;
		case ContinentType.Sakura:
			num = TotalSakuraCash * BaseController.GameController.SkillController.AdCurrentx2IdleCashFactor;
			break;
		}
		AddCash(num, continentType);
		CreateReceiveEffect("Idle Cash Gain", new ItemsEntity.Param
		{
			InstantCashAmount = num,
			ItemType = 3
		}, continentType);
	}

	public void OnClickClose()
	{
		base.WorldMapController.CloseSideBar();
	}

	public void OnShow()
	{
		IsShow = true;
		base.gameObject.SetActive(value: true);
		SeperatorCash.gameObject.SetActive(DataManager.Instance.SavegameData.CurrentContinentUnlocked >= 1);
		TextTotalSandCash.gameObject.SetActive(DataManager.Instance.SavegameData.CurrentContinentUnlocked >= 1);
		ButtonCollectSandCashx2.gameObject.SetActive(DataManager.Instance.SavegameData.CurrentContinentUnlocked >= 1);
		ButtonBoostSandCashx2.gameObject.SetActive(DataManager.Instance.SavegameData.CurrentContinentUnlocked >= 1);
		SeperatorSandCash.gameObject.SetActive(DataManager.Instance.SavegameData.CurrentContinentUnlocked >= 2);
		TextTotalSakuraCash.gameObject.SetActive(DataManager.Instance.SavegameData.CurrentContinentUnlocked >= 2);
		ButtonCollectSakuraCashx2.gameObject.SetActive(DataManager.Instance.SavegameData.CurrentContinentUnlocked >= 2);
		ButtonBoostSakuraCashx2.gameObject.SetActive(DataManager.Instance.SavegameData.CurrentContinentUnlocked >= 2);
		ButtonCollectx2.GetComponent<AdButtonController>().OriginalText = "Collect x" + BaseController.GameController.SkillController.AdCurrentx2IdleCashFactor.MinifyIncomeFactor();
		ButtonCollectSandCashx2.GetComponent<AdButtonController>().OriginalText = "Collect x" + BaseController.GameController.SkillController.AdCurrentx2IdleCashFactor.MinifyIncomeFactor();
		ButtonCollectSakuraCashx2.GetComponent<AdButtonController>().OriginalText = "Collect x" + BaseController.GameController.SkillController.AdCurrentx2IdleCashFactor.MinifyIncomeFactor();
		ButtonBoostx2.GetComponent<AdButtonController>().OriginalText = "Boost All x" + BaseController.GameController.SkillController.AdCurrentx2BoostFactor.MinifyIncomeFactor();
		ButtonBoostSandCashx2.GetComponent<AdButtonController>().OriginalText = "Boost All x" + BaseController.GameController.SkillController.AdCurrentx2BoostFactor.MinifyIncomeFactor();
		ButtonBoostSakuraCashx2.GetComponent<AdButtonController>().OriginalText = "Boost All x" + BaseController.GameController.SkillController.AdCurrentx2BoostFactor.MinifyIncomeFactor();
		for (int i = 0; i < DataManager.Instance.SavegameData.Mines.Count; i++)
		{
			if (SideBarCollectContents.Count <= i)
			{
				SideBarCollectContent component = InstantiatePrefab("Prefabs/Map/SidebarCollectContent").GetComponent<SideBarCollectContent>();
				SideBarCollectContents.Add(component);
			}
			SideBarCollectContent sideBarCollectContent = SideBarCollectContents[i];
			sideBarCollectContent.Init(DataManager.Instance.SavegameData.Mines[i].ContinentIndex, DataManager.Instance.SavegameData.Mines[i].MineIndex);
			sideBarCollectContent.gameObject.SetActive(value: true);
			sideBarCollectContent.transform.SetParent(ScrollViewContent, worldPositionStays: false);
			sideBarCollectContent.transform.SetAsLastSibling();
			sideBarCollectContent.Seperator.gameObject.SetActive(i != DataManager.Instance.SavegameData.Mines.Count - 1 && i % 5 != 4);
			if (i < 5)
			{
				TextTotalCash.transform.SetAsLastSibling();
				ButtonCollectx2.transform.SetAsLastSibling();
				ButtonBoostx2.transform.SetAsLastSibling();
				SeperatorCash.SetAsLastSibling();
			}
			else if (i < 10)
			{
				TextTotalSandCash.transform.SetAsLastSibling();
				ButtonCollectSandCashx2.transform.SetAsLastSibling();
				ButtonBoostSandCashx2.transform.SetAsLastSibling();
				SeperatorSandCash.SetAsLastSibling();
			}
			else
			{
				TextTotalSakuraCash.transform.SetAsLastSibling();
				ButtonCollectSakuraCashx2.transform.SetAsLastSibling();
				ButtonBoostSakuraCashx2.transform.SetAsLastSibling();
			}
		}
		for (int j = DataManager.Instance.SavegameData.Mines.Count; j < SideBarCollectContents.Count; j++)
		{
			SideBarCollectContents[j].gameObject.SetActive(value: false);
		}
		if (TweenShow != null)
		{
			TweenShow.Kill();
		}
		TweenShow = base.transform.DOLocalMoveX(OriginalX - TweenDelta, base.WorldMapController.TweenTime);
	}

	public void OnHide()
	{
		IsShow = false;
		base.gameObject.SetActive(value: true);
		if (TweenShow != null)
		{
			TweenShow.Kill();
		}
		TweenShow = base.transform.DOLocalMoveX(OriginalX, base.WorldMapController.TweenTime);
	}
}
