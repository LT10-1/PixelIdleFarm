using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OpenChest : GestureController
{
	public Button buttonClose;

	public GridLayoutGroup CardParent;

	public GameObject[] cardsTransform;

	public HorizontalLayoutGroup ChestParent;

	public List<OpenChestEffect> ChestList;

	public Image ChestRibbon;

	public TMP_Text ChestNameText;

	public UIButtonController ButtonBack;

	public UIButtonController ButtonNext;

	public Button ButtonSkipAnimation;

	[HideInInspector]
	public List<WorkerCard> WorkerCards = new List<WorkerCard>();

	[HideInInspector]
	public List<KeyValuePair<int, int>> ChestData = new List<KeyValuePair<int, int>>();

	[HideInInspector]
	public OpenChestState OpenChestState;

	private int currentSelectChest;

	[HideInInspector]
	public OpenChestEffect OpenChestEffect;

	protected override float OnShowScaleAmount => 0.01f;

	public bool HaveChest(int rarity)
	{
		return DataManager.Instance.SavegameData.ChestSavegames.ContainsKey(rarity) && DataManager.Instance.SavegameData.ChestSavegames[rarity] > 0;
	}

	public override void Awake()
	{
		base.Awake();
		GridLayoutGroup cardParent = CardParent;
		float cANVAS_WIDTH = CONST.CANVAS_WIDTH;
		Vector2 cellSize = CardParent.cellSize;
		float x = (cANVAS_WIDTH - cellSize.x * (float)CardParent.constraintCount) / (float)(CardParent.constraintCount + 1);
		Vector2 spacing = CardParent.spacing;
		cardParent.spacing = new Vector2(x, spacing.y);
		for (int num = ChestList.Count - 1; num >= 0; num--)
		{
			OpenChestEffect chest = ChestList[num];
			chest.ChestType = (ChestType)num;
			chest.chest.OnClickCallback = delegate
			{
				OnClickOpenChest(chest.ChestType);
			};
		}
		buttonClose.onClick.AddListener(OnHide);
		ButtonBack.OnClickCallback = delegate
		{
			UpdateChestPosition(-1);
		};
		ButtonNext.OnClickCallback = delegate
		{
			UpdateChestPosition(1);
		};
		ButtonSkipAnimation.onClick.AddListener(OnClickSkipAnimation);
	}

	public override void Start()
	{
		base.Start();
		buttonClose.onClick.AddListener(OnHide);
	}

	public override void OnShow()
	{
		base.OnShow();
		LayoutRebuilder.ForceRebuildLayoutImmediate(ChestParent.GetComponent<RectTransform>());
		CreateWorkerCard();
		ChestList.ForEach(delegate(OpenChestEffect chest)
		{
			chest.ChangeChestState(OpenChestState.Idle);
		});
		ChangeChestState(OpenChestState.Idle);
	}

	public void UpdateCurrentOpenChestEffect()
	{
		int num = 0;
		for (int i = 0; i < ChestList.Count; i++)
		{
			if (HaveChest(i))
			{
				if (currentSelectChest == num)
				{
					OpenChestEffect = ChestList[i];
					return;
				}
				num++;
			}
		}
		OpenChestEffect = GetComponentInChildren<OpenChestEffect>(includeInactive: true);
	}

	public override void OnEndDragCallback(DraggedDirection direction)
	{
		base.OnEndDragCallback(direction);
		switch (direction)
		{
		case DraggedDirection.Up:
			break;
		case DraggedDirection.Down:
			break;
		case DraggedDirection.Right:
			UpdateChestPosition(-1);
			break;
		case DraggedDirection.Left:
			UpdateChestPosition(1);
			break;
		}
	}

	public void OnClickOpenChest(ChestType chestType)
	{
		if (HaveChest((int)chestType))
		{
			if (OpenChestEffect.ChestType != chestType)
			{
				UpdateChestPosition((chestType >= OpenChestEffect.ChestType) ? 1 : (-1));
				return;
			}
			ChestData = BaseController.GameController.ChestController.GenerateParts((int)chestType);
			AddPartsFromChest(ChestData);
			OpenChestEffect.StartOpen();
			ChangeChestState(OpenChestState.OpeningChest);
			UseChest(chestType);
			//BaseController.GameController.AnalyticController.LogEvent("open_chest", "chestType", chestType.ToString());
		}
	}

	public void ChangeChestState(OpenChestState state)
	{
		ButtonSkipAnimation.gameObject.SetActive(state != OpenChestState.Idle);
		switch (state)
		{
		case OpenChestState.Idle:
			WorkerCards.ForEach(delegate(WorkerCard card)
			{
				card.gameObject.SetActive(value: false);
			});
			OnChestChange();
			break;
		case OpenChestState.ShowingCards:
			FlyCard();
			break;
		case OpenChestState.EndAnimation:
			CompleteFlyCard();
			break;
		}
		OpenChestEffect.ChangeChestState(state);
		OpenChestState = state;
	}

	public void OnChestChange()
	{
		for (int i = 0; i < ChestList.Count; i++)
		{
			OpenChestEffect openChestEffect = ChestList[i];
			if (HaveChest(i))
			{
				openChestEffect.gameObject.SetActive(value: true);
				openChestEffect.ChestCountText.text = DataManager.Instance.SavegameData.ChestSavegames[i].ToString();
			}
			else
			{
				openChestEffect.gameObject.SetActive(value: false);
			}
		}
		LayoutRebuilder.ForceRebuildLayoutImmediate(ChestParent.GetComponent<RectTransform>());
		UpdateChestPosition();
	}

	public void OnClickSkipAnimation()
	{
		switch (OpenChestState)
		{
		case OpenChestState.Idle:
			break;
		case OpenChestState.OpeningChest:
			ChangeChestState(OpenChestState.ShowingCards);
			break;
		case OpenChestState.ShowingCards:
			ChangeChestState(OpenChestState.EndAnimation);
			break;
		case OpenChestState.EndAnimation:
			ChangeChestState(OpenChestState.Idle);
			break;
		}
	}

	public void GotoChestType(ChestType chestType, bool showAnimation = true)
	{
		int num = 0;
		int position = 0;
		for (int i = 0; i < ChestList.Count; i++)
		{
			if (HaveChest(i))
			{
				if (i == (int)chestType)
				{
					position = num;
				}
				num++;
			}
		}
		SetChestPosition(position, num, showAnimation);
	}

	public void UpdateChestPosition(int positionChange = 0, bool showAnimation = true)
	{
		int num = 0;
		for (int i = 0; i < ChestList.Count; i++)
		{
			if (HaveChest(i))
			{
				num++;
			}
		}
		SetChestPosition(Mathf.Clamp(currentSelectChest + positionChange, 0, num - 1), num, showAnimation);
	}

	public void SetChestPosition(int position, int activeChest, bool showAnimation = true)
	{
		int num = currentSelectChest;
		currentSelectChest = position;
		ButtonBack.SetEnable(currentSelectChest != 0);
		ButtonNext.SetEnable(currentSelectChest < activeChest - 1);
		if (num != currentSelectChest)
		{
			UpdateCurrentChestPosition(showAnimation);
		}
		UpdateCurrentOpenChestEffect();
		ChestNameText.text = DATA_TEXT.COLLECTIBLES.LIST[(int)OpenChestEffect.ChestType] + " Chest";
		ChestRibbon.overrideSprite = BaseController.LoadSprite(DATA_RESOURCES.IMAGE.WORKSHOP_RIBBON_LIST[(int)OpenChestEffect.ChestType]);
		if (BaseController.GameController.ChestController.ChestCount <= 0)
		{
			OnHide();
		}
	}

	public void UpdateCurrentChestPosition(bool showAnimation)
	{
		int num = 0;
		int num2 = 0;
		while (true)
		{
			if (num2 >= ChestList.Count)
			{
				return;
			}
			if (DataManager.Instance.SavegameData.ChestSavegames.ContainsKey(num2) && DataManager.Instance.SavegameData.ChestSavegames[num2] > 0)
			{
				if (num == currentSelectChest)
				{
					break;
				}
				num++;
			}
			num2++;
		}
		Vector3 localPosition = ChestList[num2].transform.localPosition;
		float num3 = 0f - localPosition.x;
		if (showAnimation)
		{
			ChestParent.transform.DOLocalMoveX(num3, 0.2f);
			return;
		}
		Transform transform = ChestParent.transform;
		float x = num3;
		Vector3 localPosition2 = ChestParent.transform.localPosition;
		transform.localPosition = new Vector3(x, localPosition2.y);
	}

	public void FlyCard()
	{
		if (ChestData.Count == 0)
		{
			return;
		}
		float num = Mathf.Clamp(1.5f / (float)ChestData.Count, 0.2f, 0.4f);
		for (int i = 0; i < WorkerCards.Count; i++)
		{
			WorkerCard item = WorkerCards[i];
			item.gameObject.SetActive(value: false);
			if (i < ChestData.Count)
			{
				item.SetSummonInfo(ChestData[i].Key, ChestData[i].Value);
				bool isLevelup = ChestData[i].Value > item.CardSavegame.Parts;
				item.transform.position = OpenChestEffect.transform.position;
				item.levelUP.gameObject.SetActive(value: false);
				int tempIndex = i;
				item.transform.DOMove(cardsTransform[i].transform.position, num).SetDelay((float)i * num).OnPlay(delegate
				{
					BaseController.GameController.AudioController.PlayOneShot("Audios/Effect/levelup");
					item.gameObject.SetActive(value: true);
				})
					.OnComplete(delegate
					{
						item.textCount.gameObject.SetActive(value: true);
						item.levelUP.gameObject.SetActive(isLevelup);
						if (tempIndex == ChestData.Count - 1)
						{
							ChangeChestState(OpenChestState.EndAnimation);
						}
					});
			}
		}
	}

	public void CompleteFlyCard()
	{
		for (int i = 0; i < WorkerCards.Count; i++)
		{
			WorkerCard workerCard = WorkerCards[i];
			DOTween.Kill(workerCard.transform);
			workerCard.gameObject.SetActive(value: false);
			if (i < ChestData.Count)
			{
				workerCard.gameObject.SetActive(value: true);
				workerCard.transform.position = cardsTransform[i].transform.position;
				workerCard.textCount.gameObject.SetActive(value: true);
				workerCard.levelUP.gameObject.SetActive(ChestData[i].Value > workerCard.CardSavegame.Parts);
			}
		}
	}

	public void CreateWorkerCard()
	{
		if (WorkerCards.Count != cardsTransform.Length)
		{
			WorkerCards.Clear();
			for (int i = 0; i < cardsTransform.Length; i++)
			{
				WorkerCard component = InstantiatePrefab("Prefabs/Dialog/Component/WorkerCard").GetComponent<WorkerCard>();
				component.transform.SetParent(cardsTransform[i].transform, worldPositionStays: false);
				component.gameObject.SetActive(value: false);
				WorkerCards.Add(component);
			}
		}
	}
}
