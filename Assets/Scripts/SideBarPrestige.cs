using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class SideBarPrestige : BaseWorldmapController
{
	public UIButtonController ButtonClose;

	public Transform ScrollViewContent;

	public Transform SeperatorCash;

	public Transform SeperatorSandCash;

	[HideInInspector]
	public List<SideBarPrestigeContent> SideBarPrestigeContents;

	[HideInInspector]
	public bool IsShow;

	[HideInInspector]
	public Tweener TweenShow;

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
			Vector3 localPosition = base.WorldMapController.SideBarPrestigeEndX.localPosition;
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
	}

	public override void Update()
	{
		base.Update();
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
		SeperatorSandCash.gameObject.SetActive(DataManager.Instance.SavegameData.CurrentContinentUnlocked >= 2);
		for (int i = 0; i < DataManager.Instance.SavegameData.Mines.Count; i++)
		{
			if (SideBarPrestigeContents.Count <= i)
			{
				SideBarPrestigeContent component = InstantiatePrefab("Prefabs/Map/SidebarPrestigeContent").GetComponent<SideBarPrestigeContent>();
				SideBarPrestigeContents.Add(component);
			}
			SideBarPrestigeContent sideBarPrestigeContent = SideBarPrestigeContents[i];
			sideBarPrestigeContent.Init(DataManager.Instance.SavegameData.Mines[i].ContinentIndex, DataManager.Instance.SavegameData.Mines[i].MineIndex);
			sideBarPrestigeContent.gameObject.SetActive(value: true);
			sideBarPrestigeContent.transform.SetParent(ScrollViewContent, worldPositionStays: false);
			sideBarPrestigeContent.transform.SetAsLastSibling();
			sideBarPrestigeContent.Seperator.gameObject.SetActive(i != DataManager.Instance.SavegameData.Mines.Count - 1 && i % 5 != 4);
			if (i < 5)
			{
				SeperatorCash.SetAsLastSibling();
			}
			else if (i < 10)
			{
				SeperatorSandCash.SetAsLastSibling();
			}
		}
		for (int j = DataManager.Instance.SavegameData.Mines.Count; j < SideBarPrestigeContents.Count; j++)
		{
			SideBarPrestigeContents[j].gameObject.SetActive(value: false);
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
