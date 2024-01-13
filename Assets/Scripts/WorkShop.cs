using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WorkShop : BaseDialog, IPointerClickHandler, IEventSystemHandler
{
	public WorkerDetail WorkerDetail;

	public Button buttonNext;

	public Button buttonBack;

	public GameObject Boards;

	public GameObject GoCenter;

	public TMP_Text ChestCountText;

	public Button CommonChest;

	public Button BuyChest;

	public Button ButtonInfo;

	[HideInInspector]
	public Dictionary<int, WorkerCard> WorkerCards;

	private int index;

	private float _WBoard;

	private float _Wcanvas;

	protected override float OnShowScaleAmount => 0.01f;

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.clickCount == 1 && WorkerDetail.gameObject.activeInHierarchy)
		{
			HideDetail();
		}
	}

	public override void Awake()
	{
		base.Awake();
		WorkerCards = GetComponentsInChildren<WorkerCard>().ToDictionary((WorkerCard e) => e.id, (WorkerCard e) => e);
		foreach (KeyValuePair<int, WorkerCard> keyValuePair in WorkerCards)
		{
			keyValuePair.Value.button.onClick.AddListener(delegate
			{
				ShowDetail(keyValuePair.Value);
			});
		}
	}

	public override void Start()
	{
		base.Start();
		buttonNext.onClick.AddListener(Next);
		buttonBack.onClick.AddListener(Back);
		buttonBack.gameObject.SetActive(value: false);
		RectTransform rectTransform = WorkerDetail.RectTransform;
		Vector3 position = WorkerDetail.RectTransform.position;
		float x = position.x;
		Vector2 sizeDelta = WorkerDetail.RectTransform.sizeDelta;
		rectTransform.position = new Vector3(x, 0f - sizeDelta.y, 0f);
		WorkerDetail.gameObject.SetActive(value: false);
		OnChestChange();
		Button.ButtonClickedEvent onClick = BuyChest.onClick;
		DialogChestShop dialogChestShop = BaseController.GameController.DialogController.DialogChestShop;
		onClick.AddListener(dialogChestShop.OnShow);
		Button.ButtonClickedEvent onClick2 = ButtonInfo.onClick;
		DialogWSInfo dialogWSInfo = BaseController.GameController.DialogController.DialogWSInfo;
		onClick2.AddListener(dialogWSInfo.OnShow);
		Button.ButtonClickedEvent onClick3 = CommonChest.onClick;
		OpenChest openChest = BaseController.GameController.DialogController.OpenChest;
		onClick3.AddListener(((BaseDialog)openChest).OnShow);
		BaseController.GameController.OnChestChangeCallback.Add(OnChestChange);
		BaseController.GameController.OnWorkshopChangeCallback.Add(UpdateData);
		DataManager.Instance.SavegameData.UnlockedWorkshop = true;
	}

	public override void OnShow()
	{
		base.OnShow();
		Invoke("SetBoardPos", 0.2f);
		UpdateData();
	}

	public void UpdateData()
	{
		foreach (KeyValuePair<int, WorkerCard> workerCard in WorkerCards)
		{
			workerCard.Value.UpdateData();
		}
		WorkerDetail.ResetData();
	}

	public void OnChestChange()
	{
		SetChestNumber(DataManager.Instance.SavegameData.ChestSavegames.Sum((KeyValuePair<int, int> e) => e.Value));
	}

	public void SetChestNumber(int chestNumber)
	{
		CommonChest.gameObject.SetActive(chestNumber > 0);
		ChestCountText.text = chestNumber.ToString();
	}

	private void SetBoardPos()
	{
		_Wcanvas = GoCenter.GetComponent<RectTransform>().rect.width;
		_WBoard = Boards.GetComponent<RectTransform>().rect.width / 3f;
		Boards.GetComponent<RectTransform>().DOAnchorPosX(_Wcanvas / 2f - _WBoard / 2f - (float)index * _WBoard, 0.3f);
	}

	public void Next()
	{
		index++;
		if (index >= 2)
		{
			index = 2;
			buttonNext.gameObject.SetActive(value: false);
		}
		buttonBack.gameObject.SetActive(value: true);
		SetBoardPos();
	}

	public void Back()
	{
		index--;
		if (index <= 0)
		{
			index = 0;
			buttonBack.gameObject.SetActive(value: false);
		}
		buttonNext.gameObject.SetActive(value: true);
		SetBoardPos();
	}

	public void ShowDetail(WorkerCard WorkerCard)
	{
		WorkerDetail.gameObject.SetActive(value: true);
		((Transform)WorkerDetail.RectTransform).DOMoveY(0f, 0.3f, snapping: false);
		WorkerDetail.SetData(WorkerCard);
	}

	public void HideDetail()
	{
		RectTransform rectTransform = WorkerDetail.RectTransform;
		Vector2 sizeDelta = WorkerDetail.RectTransform.sizeDelta;
		((Transform)rectTransform).DOMoveY(0f - sizeDelta.y, 0.3f, snapping: false).OnComplete(delegate
		{
			WorkerDetail.gameObject.SetActive(value: false);
		});
	}
}
