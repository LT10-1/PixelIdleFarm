using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResearchLaboratory : BaseDialog, IPointerClickHandler, IEventSystemHandler
{
	public GameObject LockGroup;

	public Button LockIcon;

	public CoinController UnlockCost;

	public ScrollRect scrollRect;

	public SkillPointDetail SkillPointDetail;

	public GameObject SPMap;

	public Button ButtonSkillPointGroup;

	public UIButtonController ButtonReset;

	public UIButtonController ButtonReset2;

	public TMP_Text TextCash;

	public TMP_Text TextSandCash;

	public TMP_Text TextSakuraCash;

	public TMP_Text TextSkillPointGrass;

	public TMP_Text TextSkillPointSand;

	public TMP_Text TextSkillPointSakura;

	private float maxScale = 2.5f;

	private float minScale = 1f;

	private bool isZoom = true;

	private Vector3 currenMapLocation;

	[HideInInspector]
	public Dictionary<int, SkillPoint> SkillPoints;

	[HideInInspector]
	public SkillPoint CurrentSkillPoint;

	private float lastTimeClick;

	protected override float OnShowScaleAmount => 0.01f;

	public override void Start()
	{
		base.Start();
		currenMapLocation = SPMap.transform.localPosition;
		SkillPoints = SPMap.GetComponentsInChildren<SkillPoint>().ToDictionary((SkillPoint e) => e.id, (SkillPoint e) => e);
		foreach (KeyValuePair<int, SkillPoint> skillPoint in SkillPoints)
		{
			skillPoint.Value.Init(DataManager.Instance.SkillDictionary[skillPoint.Key]);
		}
		RectTransform rectTransform = SkillPointDetail.RectTransform;
		Vector3 position = SkillPointDetail.RectTransform.position;
		float x = position.x;
		Vector2 sizeDelta = SkillPointDetail.RectTransform.sizeDelta;
		rectTransform.position = new Vector3(x, 0f - sizeDelta.y, 0f);
		SkillPointDetail.gameObject.SetActive(value: false);
		SkillPointDetail.ButtonUpgrade.OnClickCallback = UpgradeSkill;
		ButtonSkillPointGroup.onClick.AddListener(OnClickSkillPointGroup);
		ButtonReset.OnClickCallback = OnClickReset;
		ButtonReset2.OnClickCallback = OnClickReset;
		LockIcon.onClick.AddListener(OnClickUnlock);
		BaseController.GameController.OnGlobalCashChangeCallback.Add(OnCashChangeCallback);
		BaseController.GameController.OnSkillpointChangeCallback.Add(OnSkillPointChangeCallback);
		ReloadSkillData();
		OnCashChangeCallback();
		OnSkillPointChangeCallback();
	}

	public override void OnShow()
	{
		base.OnShow();
		LockGroup.gameObject.SetActive(!DataManager.Instance.SavegameData.UnlockedLaboratory);
		UnlockCost.SetCoinType(CoinType.Cash);
		UnlockCost.SetMoney(2.3E+18, minify: true, showMoney: true, string.Empty);
		TextSandCash.transform.parent.gameObject.SetActive(DataManager.Instance.SavegameData.CurrentContinentUnlocked >= 1);
		TextSakuraCash.transform.parent.gameObject.SetActive(DataManager.Instance.SavegameData.CurrentContinentUnlocked >= 2);
		TextSkillPointSand.transform.parent.gameObject.SetActive(DataManager.Instance.SkillPointByContinent(ContinentType.Sand) > 0 || DataManager.Instance.SavegameData.CurrentContinentUnlocked >= 1);
		TextSkillPointSakura.transform.parent.gameObject.SetActive(DataManager.Instance.SkillPointByContinent(ContinentType.Sakura) > 0 || DataManager.Instance.SavegameData.CurrentContinentUnlocked >= 2);
	}

	public void OnClickUnlock()
	{
		if (UseCash(UnlockCost.Cash, ContinentType.Grass))
		{
			DataManager.Instance.SavegameData.UnlockedLaboratory = true;
			LockGroup.gameObject.SetActive(value: false);
			CreateReceiveEffect("You Unlocked", "Images/UI/Worldmap/wm_lab", "Research Laboratory", 2f * Vector3.one);
			//BaseController.GameController.AnalyticController.LogEvent("unlock_research_laboratory");
		}
	}

	public void OnClickSkillPointGroup()
	{
		BaseController.GameController.DialogController.DialogSkillShop.OnShow();
	}

	public void OnCashChangeCallback()
	{
		UnlockCost.SetMoneyColor(CheckEnoughCash(UnlockCost.Cash, ContinentType.Grass));
		TextCash.SetText(DataManager.Instance.GrassCash.MinifyFormat());
		TextSandCash.SetText(DataManager.Instance.SandCash.MinifyFormat());
		TextSakuraCash.SetText(DataManager.Instance.SakuraCash.MinifyFormat());
	}

	public void OnSkillPointChangeCallback()
	{
		TextSkillPointGrass.SetText(DataManager.Instance.SkillPointByContinent(ContinentType.Grass).ToString());
		TextSkillPointSand.SetText(DataManager.Instance.SkillPointByContinent(ContinentType.Sand).ToString());
		TextSkillPointSakura.SetText(DataManager.Instance.SkillPointByContinent(ContinentType.Sakura).ToString());
		TextSkillPointSand.transform.parent.gameObject.SetActive(DataManager.Instance.SkillPointByContinent(ContinentType.Sand) > 0 || DataManager.Instance.SavegameData.CurrentContinentUnlocked >= 1);
		TextSkillPointSakura.transform.parent.gameObject.SetActive(DataManager.Instance.SkillPointByContinent(ContinentType.Sakura) > 0 || DataManager.Instance.SavegameData.CurrentContinentUnlocked >= 2);
		SkillPointDetail.CheckButtonUpgrade();
	}

	public void ReloadSkillData()
	{
		foreach (KeyValuePair<int, SkillPoint> skillPoint in SkillPoints)
		{
			skillPoint.Value.SetUnlocked(isUnlock: false);
			skillPoint.Value.SetUnlockAble(unlockable: false);
		}
		foreach (KeyValuePair<int, int> item in DataManager.Instance.SavegameData.SkillSaveGame)
		{
			SkillPoints[item.Key].SetUnlocked(isUnlock: true);
			SkillPoints[item.Key].setLevel(SkillPoints[item.Key].CurrentLevel);
		}
		foreach (KeyValuePair<int, SkillPoint> skillPoint2 in SkillPoints)
		{
			if (!skillPoint2.Value.IsActive)
			{
				if (skillPoint2.Value.SkillParam.ParentId == -1)
				{
					skillPoint2.Value.SetUnlockAble(unlockable: true);
				}
				else if (SkillPoints[skillPoint2.Value.SkillParam.ParentId].IsActive)
				{
					skillPoint2.Value.SetUnlockAble(unlockable: true);
				}
			}
		}
		BaseController.GameController.SkillController.OnSkillChanged();
	}

	public void OnClickReset()
	{
		BaseController.GameController.DialogController.DialogResetSkill.OnShow();
		BaseController.GameController.DialogController.DialogResetSkill.OnConfirm = ResetSkill;
	}

	public void ResetSkill()
	{
		DataManager.Instance.SavegameData.SkillSaveGame.Clear();
		DataManager.Instance.SetSkillPointByContinent(DataManager.Instance.SkillPointNetworthByContinent(ContinentType.Grass), ContinentType.Grass);
		DataManager.Instance.SetSkillPointByContinent(DataManager.Instance.SkillPointNetworthByContinent(ContinentType.Sand), ContinentType.Sand);
		DataManager.Instance.SetSkillPointByContinent(DataManager.Instance.SkillPointNetworthByContinent(ContinentType.Sakura), ContinentType.Sakura);
		ReloadSkillData();
		OnSkillPointChangeCallback();
	}

	public void UpgradeSkill()
	{
		if (CurrentSkillPoint == null || CurrentSkillPoint.IsMaxLevel || !UseSkillPoint(CurrentSkillPoint.CurrentCost, (ContinentType)CurrentSkillPoint.SkillParam.SkillPathID))
		{
			return;
		}
		if (!CurrentSkillPoint.IsActive && CurrentSkillPoint.IsUnlockable)
		{
			DataManager.Instance.SavegameData.SkillSaveGame[CurrentSkillPoint.SkillParam.SkillID] = 1;
			//BaseController.GameController.AnalyticController.LogEvent("unlock_skill", "skillID", CurrentSkillPoint.SkillParam.SkillID);
		}
		else
		{
			if (!DataManager.Instance.SavegameData.SkillSaveGame.ContainsKey(CurrentSkillPoint.SkillParam.SkillID))
			{
				DataManager.Instance.SavegameData.SkillSaveGame[CurrentSkillPoint.SkillParam.SkillID] = 1;
			}
			Dictionary<int, int> skillSaveGame;
			int skillID;
			(skillSaveGame = DataManager.Instance.SavegameData.SkillSaveGame)[skillID = CurrentSkillPoint.SkillParam.SkillID] = skillSaveGame[skillID] + 1;
			//BaseController.GameController.AnalyticController.LogEvent("level_up_skill", "skillLevel", CurrentSkillPoint.SkillParam.SkillID + "-" + DataManager.Instance.SavegameData.SkillSaveGame[CurrentSkillPoint.SkillParam.SkillID]);
		}
		BaseController.GameController.AudioController.PlayOneShot("Audios/Effect/levelup");
		ReloadSkillData();
		CurrentSkillPoint.setLevel(CurrentSkillPoint.CurrentLevel);
		SkillPointDetail.SetData(CurrentSkillPoint);
	}

	public void ShowDetail(SkillPoint currentSkillPoint)
	{
		if (CurrentSkillPoint != null)
		{
			CurrentSkillPoint.focusButton.gameObject.SetActive(value: false);
		}
		CurrentSkillPoint = currentSkillPoint;
		CurrentSkillPoint.focusButton.gameObject.SetActive(value: true);
		SkillPointDetail.gameObject.SetActive(value: true);
		((Transform)SkillPointDetail.RectTransform).DOMoveY(0f, 0.3f, snapping: false);
		SkillPointDetail.SetData(CurrentSkillPoint);
	}

	public void HideDetail()
	{
		RectTransform rectTransform = SkillPointDetail.RectTransform;
		Vector2 sizeDelta = SkillPointDetail.RectTransform.sizeDelta;
		((Transform)rectTransform).DOMoveY(0f - sizeDelta.y, 0.3f, snapping: false).OnComplete(delegate
		{
			SkillPointDetail.gameObject.SetActive(value: false);
		});
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		float clickTime = eventData.clickTime;
		if (Mathf.Abs(clickTime - lastTimeClick) < 0.75f)
		{
			isZoom = !isZoom;
			if (isZoom)
			{
				SPMap.transform.DOScale(Vector3.one * maxScale, 0.4f);
				SPMap.transform.DOLocalMove(currenMapLocation, 0.4f);
			}
			else
			{
				SPMap.transform.DOScale(Vector3.one * minScale, 0.4f);
				SPMap.transform.DOLocalMove(currenMapLocation, 0.4f);
			}
		}
		lastTimeClick = clickTime;
		if (eventData.clickCount == 1 && SkillPointDetail.gameObject.activeInHierarchy)
		{
			HideDetail();
		}
	}

	private void FixedUpdate()
	{
		CheckPinch();
	}

	private void Zoom(float direction, bool pinching)
	{
		Vector3 vector = SPMap.transform.localScale;
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
		SPMap.transform.localScale = vector;
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
}
