using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WorldMapController : BaseWorldmapController
{
	[HideInInspector]
	public Button currentEnterButton;

	[HideInInspector]
	public int CurrentMine;

	public TMP_Text TextCash;

	public TMP_Text TextSandCash;

	public TMP_Text TextSakuraCash;

	public GameObject viewPort;

	public CoinController SandContinentPrice;

	public CoinController SakuraContinentPrice;

	public UIButtonController ButtonResearchLab;

	public UIButtonController ButtonWorkshop;

	public UIButtonController ButtonExpedition;

	public UIButtonController ButtonEventMine;

	public UIButtonController ButtonPrestige;

	public UIButtonController ButtonCollect;

	public Button SideBarClickout;

	public SideBarCollect SideBarCollect;

	public SideBarPrestige SideBarPrestige;

	public Transform SideBarCollectEndX;

	public Transform SideBarPrestigeEndX;

	private Image background;

	public ScrollRect scrollRect;

	private float maxScale = 2.5f;

	private float minScale = 1f;

	public RectTransform canvasRectTransform;

	private float _Wcanvas;

	private float _Hcanvas;

	[HideInInspector]
	public float TweenTime = 0.2f;

	[HideInInspector]
	public List<AreaInMap> AreaInMaps;

	[HideInInspector]
	public Tweener ButtonCollectTweener;

	[HideInInspector]
	public Tweener ButtonPrestigeTweener;

	private float _ButtonCollectOriginalX;

	private float _ButtonPrestigeOriginalX;

	public float ButtonCollectOriginalX
	{
		get
		{
			if (_ButtonCollectOriginalX == 0f)
			{
				Vector3 localPosition = ButtonCollect.transform.localPosition;
				_ButtonCollectOriginalX = localPosition.x;
			}
			return _ButtonCollectOriginalX;
		}
	}

	public float ButtonPrestigeOriginalX
	{
		get
		{
			if (_ButtonPrestigeOriginalX == 0f)
			{
				Vector3 localPosition = ButtonPrestige.transform.localPosition;
				_ButtonPrestigeOriginalX = localPosition.x;
			}
			return _ButtonPrestigeOriginalX;
		}
	}

	public void comingsoon()
	{
		BaseController.GameController.ToastController.StartToast("Coming Soon");
	}

	public override void Awake()
	{
		base.Awake();
		BaseController.GameController.WorldMapController = this;
		AreaInMaps = Object.FindObjectsOfType<AreaInMap>().ToList();
		AreaInMaps.Sort((AreaInMap x, AreaInMap y) => x.id.CompareTo(y.id));
	}

	public override void Start()
	{
		base.Start();
		background = viewPort.GetComponent<Image>();
		Vector2 sizeDelta = canvasRectTransform.sizeDelta;
		_Wcanvas = sizeDelta.x;
		Vector2 sizeDelta2 = canvasRectTransform.sizeDelta;
		_Hcanvas = sizeDelta2.y;
		BaseController.GameController.OnGlobalCashChangeCallback.Add(delegate
		{
			checkCash();
		});
		ButtonResearchLab.OnClickCallback = OnClickResearchLab;
		ButtonWorkshop.OnClickCallback = OnClickWorkshop;
		ButtonExpedition.OnClickCallback = OnClickExpedition;
		ButtonEventMine.OnClickCallback = OnClickEventMine;
		ButtonCollect.OnClickCallback = OnOpenSideBarCollect;
		ButtonPrestige.OnClickCallback = OnOpenSideBarPrestige;
		SideBarClickout.onClick.AddListener(CloseSideBar);
		SideBarClickout.gameObject.SetActive(value: false);
		SandContinentPrice.SetCoinType(CoinType.Cash);
		SakuraContinentPrice.SetCoinType(CoinType.SandCash);
		SandContinentPrice.SetMoney(2.45E+36, minify: true, showMoney: true, string.Empty);
		SakuraContinentPrice.SetMoney(2.25E+37, minify: true, showMoney: true, string.Empty);
		SandContinentPrice.transform.parent.GetComponentInChildren<UIButtonController>(includeInactive: true).OnClickCallback = OnClickBuySandContinent;
		SakuraContinentPrice.transform.parent.GetComponentInChildren<UIButtonController>(includeInactive: true).OnClickCallback = OnClickBuySakuraContinent;
		checkCash();
		OnBuyNewMine();
		SetFocus(DataManager.Instance.SavegameData.CurrentMine);
		BaseController.GameController.FacebookHelper.FriendIcon.UpdateUI("WorldMap");
	}

	public AreaInMap GetAreaById(int id)
	{
		for (int i = 0; i < AreaInMaps.Count; i++)
		{
			if (AreaInMaps[i].id == id)
			{
				return AreaInMaps[i];
			}
		}
		return null;
	}

	public void CloseSideBar()
	{
		SideBarClickout.gameObject.SetActive(value: false);
		SideBarCollect.OnHide();
		SideBarPrestige.OnHide();
		if (ButtonCollectTweener != null)
		{
			ButtonCollectTweener.Kill();
		}
		ButtonCollectTweener = ButtonCollect.transform.DOLocalMoveX(ButtonCollectOriginalX, TweenTime);
		if (ButtonPrestigeTweener != null)
		{
			ButtonPrestigeTweener.Kill();
		}
		ButtonPrestigeTweener = ButtonPrestige.transform.DOLocalMoveX(ButtonPrestigeOriginalX, TweenTime);
	}

	public void OnOpenSideBarCollect()
	{
		SideBarCollect.OnShow();
		SideBarClickout.gameObject.SetActive(value: true);
		if (ButtonCollectTweener != null)
		{
			ButtonCollectTweener.Kill();
		}
		ButtonCollectTweener = ButtonCollect.transform.DOLocalMoveX(ButtonCollectOriginalX - SideBarCollect.TweenDelta, TweenTime);
	}

	public void OnOpenSideBarPrestige()
	{
		SideBarPrestige.OnShow();
		SideBarClickout.gameObject.SetActive(value: true);
		if (ButtonPrestigeTweener != null)
		{
			ButtonPrestigeTweener.Kill();
		}
		ButtonPrestigeTweener = ButtonPrestige.transform.DOLocalMoveX(ButtonPrestigeOriginalX - SideBarPrestige.TweenDelta, TweenTime);
	}

	public void OnBuyNewMine()
	{
		SandContinentPrice.transform.parent.gameObject.SetActive(DataManager.Instance.SavegameData.CurrentContinentUnlocked <= 0);
		SakuraContinentPrice.transform.parent.gameObject.SetActive(DataManager.Instance.SavegameData.CurrentContinentUnlocked <= 1);
		TextSandCash.transform.parent.gameObject.SetActive(DataManager.Instance.SavegameData.CurrentContinentUnlocked >= 1);
		TextSakuraCash.transform.parent.gameObject.SetActive(DataManager.Instance.SavegameData.CurrentContinentUnlocked >= 2);
		for (int i = 0; i < AreaInMaps.Count; i++)
		{
			AreaInMap areaInMap = AreaInMaps[i];
			areaInMap.CommonInArea.gameObject.SetActive(areaInMap.ContinentId <= DataManager.Instance.SavegameData.CurrentContinentUnlocked);
			if (areaInMap.id <= DataManager.Instance.SavegameData.CurrentMineUnlocked)
			{
				areaInMap.SetLockActive(isLock: false);
				continue;
			}
			areaInMap.CommonInArea.imgLock.gameObject.SetActive(value: false);
			areaInMap.CommonInArea.labelUnknow.gameObject.SetActive(value: true);
			areaInMap.CommonInArea.MineName.gameObject.SetActive(value: false);
		}
		if (AreaInMaps.Count > DataManager.Instance.SavegameData.CurrentMineUnlocked + 1)
		{
			AreaInMap areaInMap2 = AreaInMaps[DataManager.Instance.SavegameData.CurrentMineUnlocked + 1];
			areaInMap2.SetLockActive(isLock: true);
			areaInMap2.CommonInArea.SetPrice(DataManager.Instance.MineFactorsEntityParams[areaInMap2.MineId][0].Cost);
			areaInMap2.MakeAreaInTop();
		}
	}

	public void SetFocus(int areaID)
	{
		CurrentMine = areaID;
		for (int i = 0; i < AreaInMaps.Count; i++)
		{
			AreaInMap areaInMap = AreaInMaps[i];
			if (areaInMap.id == areaID)
			{
				areaInMap.FocusOutline.gameObject.SetActive(value: true);
				areaInMap.MakeAreaInTop();
			}
			else
			{
				areaInMap.FocusOutline.gameObject.SetActive(value: false);
			}
		}
	}

	public void OnClickBuySandContinent()
	{
		if (DataManager.Instance.SavegameData.CurrentMineUnlocked < 4)
		{
			BaseController.GameController.ToastController.StartToast(string.Format("Unlock All Mines In {0} Continent First!", "Grass"));
		}
		else if (UseCash(SandContinentPrice.Cash, ContinentType.Grass))
		{
			DataManager.Instance.SavegameData.CurrentContinentUnlocked = 1;
			AreaInMaps[5].CommonInArea.BuyNewMine();
		}
	}

	public void OnClickBuySakuraContinent()
	{
		if (DataManager.Instance.SavegameData.CurrentMineUnlocked < 9)
		{
			BaseController.GameController.ToastController.StartToast(string.Format("Unlock All Mines In {0} Continent First!", "Sand"));
		}
		else if (UseCash(SakuraContinentPrice.Cash, ContinentType.Sand))
		{
			DataManager.Instance.SavegameData.CurrentContinentUnlocked = 2;
			AreaInMaps[10].CommonInArea.BuyNewMine();
		}
	}

	private void checkCash()
	{
		if (TextCash != null)
		{
			TextCash.SetText(DataManager.Instance.GrassCash.MinifyFormat());
		}
		if (TextSandCash != null)
		{
			TextSandCash.SetText(DataManager.Instance.SandCash.MinifyFormat());
		}
		if (TextSakuraCash != null)
		{
			TextSakuraCash.SetText(DataManager.Instance.SakuraCash.MinifyFormat());
		}
		if (SandContinentPrice != null)
		{
			SandContinentPrice.SetMoneyColor(MathUtils.CompareDoubleBiggerThanZero(DataManager.Instance.GrassCash - SandContinentPrice.Cash));
		}
		if (SakuraContinentPrice != null)
		{
			SakuraContinentPrice.SetMoneyColor(MathUtils.CompareDoubleBiggerThanZero(DataManager.Instance.SandCash - SakuraContinentPrice.Cash));
		}
	}

	public override void Update()
	{
		base.Update();
	}

	private void FixedUpdate()
	{
		CheckPinch();
	}

	public void OnClickResearchLab()
	{
		BaseController.GameController.DialogController.ResearchLaboratory.OnShow();
	}

	public void OnClickWorkshop()
	{
		if (!DataManager.Instance.SavegameData.UnlockedWorkshop)
		{
			BaseController.GameController.ToastController.StartToast("Buy Mine Shaft 10 to unlock Workshop!");
		}
		else
		{
			BaseController.GameController.DialogController.WorkShop.OnShow();
		}
	}

	public void OnClickExpedition()
	{
		if (!DataManager.Instance.SavegameData.UnlockedExpedition)
		{
			BaseController.GameController.ToastController.StartToast("Buy Gold Mine to unlock Expedition!");
		}
		else
		{
			BaseController.GameController.DialogController.Expedition.OnShow();
		}
	}

	public void OnClickEventMine()
	{
		//BaseController.GameController.AnalyticController.LogEvent("click_event_mine");
		comingsoon();
	}

	public void goToSelectedMine()
	{
		if (CurrentMine != DataManager.Instance.SavegameData.CurrentMine)
		{
			SetFocus(CurrentMine);
			BaseController.GameController.GoToMine(CurrentMine);
		}
		else
		{
			backToMine();
		}
	}

	public void backToMine()
	{
		SceneManager.UnloadSceneAsync("WorldMap");
		SceneManager.SetActiveScene(SceneManager.GetSceneByName("Mine"));
	}

	public void hideCurrenButton()
	{
		if (currentEnterButton != null)
		{
			currentEnterButton.gameObject.SetActive(value: false);
		}
	}

	private void Zoom(float direction, bool pinching)
	{
		Vector3 vector = viewPort.transform.localScale;
		if (direction > 0f)
		{
			vector += Vector3.one * 0.2f * 1f * ((!pinching) ? 1f : 0.4f);
			if (vector.x >= maxScale)
			{
				vector = Vector3.one * maxScale;
			}
		}
		else if (direction < 0f)
		{
			vector += Vector3.one * 0.2f * -1f * ((!pinching) ? 1f : 0.4f);
			if (vector.x <= minScale)
			{
				vector = Vector3.one * minScale;
			}
		}
		viewPort.transform.localScale = vector;
		preventScrollOut();
	}

	private void CheckPinch()
	{
		if (UnityEngine.Input.touchCount == 2)
		{
			scrollRect.enabled = false;
			Touch touch = UnityEngine.Input.GetTouch(0);
			Touch touch2 = UnityEngine.Input.GetTouch(1);
			Vector2 a = touch.position - touch.deltaPosition;
			Vector2 b = touch2.position - touch2.deltaPosition;
			float num = 1f;
			if (touch.deltaPosition.magnitude > num && touch2.deltaPosition.magnitude > num)
			{
				float magnitude = (a - b).magnitude;
				float magnitude2 = (touch.position - touch2.position).magnitude;
				float direction = magnitude2 - magnitude;
				Zoom(direction, pinching: true);
			}
		}
		else
		{
			scrollRect.enabled = true;
		}
	}

	private void preventScrollOut()
	{
		Vector2 sizeDelta = background.rectTransform.sizeDelta;
		float x = sizeDelta.x;
		Vector3 localScale = background.transform.localScale;
		float num = (x * localScale.x - _Wcanvas) / 2f;
		Vector2 sizeDelta2 = background.rectTransform.sizeDelta;
		float y = sizeDelta2.y;
		Vector3 localScale2 = background.transform.localScale;
		float num2 = (y * localScale2.y - _Hcanvas) / 2f;
		Vector2 anchoredPosition = background.rectTransform.anchoredPosition;
		float x2 = Mathf.Clamp(anchoredPosition.x, 0f - num, num);
		Vector2 anchoredPosition2 = background.rectTransform.anchoredPosition;
		Vector2 anchoredPosition3 = new Vector2(x2, Mathf.Clamp(anchoredPosition2.y, 0f - num2, num2));
		background.rectTransform.anchoredPosition = anchoredPosition3;
	}
}
