using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorkerDetail : BaseController
{
	public Image WorkerIcon;

	public Image progress;

	public GameObject LockGameObject;

	public TMP_Text textCard;

	public Image cardBkg;

	public Button buttonActive;

	public TMP_Text DescriptionTitle;

	public TMP_Text DescriptionText;

	public TMP_Text SecondEffectIcon;

	public TMP_Text SecondDescriptionText;

	public RectTransform RectTransform;

	public SkeletonGraphic CorridorWorker;

	public SkeletonGraphic ElevatorWorker;

	public SkeletonGraphic GroundWorker;

	[HideInInspector]
	public WorkerCard CurrentWorkerCard;

	public override void Awake()
	{
		base.Awake();
		buttonActive.onClick.AddListener(OnClickActive);
	}

	public void OnClickActive()
	{
		if (!(CurrentWorkerCard == null))
		{
			BaseController.GameController.AudioController.PlayOneShot("Audios/Effect/thuequanlythanhcong");
			DataManager.Instance.SavegameData.SetActiveWorkerCard(CurrentWorkerCard.id, CurrentWorkerCard.Area);
			BaseController.GameController.InvokeOnWorkshopChangeCallback();
		//	BaseController.GameController.AnalyticController.LogEvent("activate_worker_card", "card", (CollectibleRarity)(CurrentWorkerCard.CardParam.RarityID + CurrentWorkerCard.CardParam.Variant) + "-" + (ManagerArea)CurrentWorkerCard.CardParam.CollectibleType);
		}
	}

	public void SetLock(bool IsLock)
	{
		LockGameObject.SetActive(IsLock);
		WorkerIcon.gameObject.SetActive(IsLock);
		buttonActive.gameObject.SetActive(!IsLock);
		textCard.gameObject.SetActive(!IsLock);
		progress.gameObject.SetActive(!IsLock);
		CorridorWorker.gameObject.SetActive(!IsLock);
		ElevatorWorker.gameObject.SetActive(!IsLock);
		GroundWorker.gameObject.SetActive(!IsLock);
	}

	public void ResetData()
	{
		if (!(CurrentWorkerCard == null))
		{
			SetData(CurrentWorkerCard);
		}
	}

	public void SetData(WorkerCard WorkerCard)
	{
		SetLock(WorkerCard.IsLock);
		if (CurrentWorkerCard != null)
		{
			CurrentWorkerCard.focus.gameObject.SetActive(value: false);
		}
		CurrentWorkerCard = WorkerCard;
		CurrentWorkerCard.focus.gameObject.SetActive(value: true);
		cardBkg.overrideSprite = WorkerCard.cardBkg.overrideSprite;
		WorkerIcon.overrideSprite = WorkerCard.icon.overrideSprite;
		DescriptionTitle.text = $"<color=#{ColorUtility.ToHtmlStringRGB(WorkerCard.progress.color)}>{DATA_TEXT.COLLECTIBLES.LIST[WorkerCard.CardParam.RarityID]}</color>";
		if (!WorkerCard.IsLock)
		{
			DescriptionTitle.text += $" <color=#A5A5A5FF>Level {WorkerCard.CardSavegame.Level}</color>";
		}
		DescriptionText.text = string.Format("{0} extract from {1}", (!WorkerCard.IsLock) ? ("<color=yellow>" + WorkerCard.textValue.text + "x</color>") : "Increase", DATA_TEXT.AREA[(int)(WorkerCard.Area - 1)]);
		bool flag = WorkerCard.CardParam.SecondaryEffectId != 0;
		SecondEffectIcon.transform.parent.gameObject.SetActive(flag);
		if (flag)
		{
			string str = (!WorkerCard.IsLock) ? ("<color=yellow>" + WorkerCard.CurrentCardLevel.SecondaryEffectFactor.MinifyIncomeFactor() + "x</color>") : "Boost";
			if (WorkerCard.CardParam.SecondaryEffectId == 3)
			{
				SecondDescriptionText.text = str + " Global Income";
				SecondEffectIcon.text = DATA_RESOURCES.TEXT_SPRITE.SAKURA_CASH + DATA_RESOURCES.TEXT_SPRITE.SAND_CASH + DATA_RESOURCES.TEXT_SPRITE.CASH;
			}
			else
			{
				ContinentType secondaryEffectTargetId = (ContinentType)WorkerCard.CardParam.SecondaryEffectTargetId;
				SecondDescriptionText.text = str + " " + DATA_TEXT.CONTINENT.LIST[(int)secondaryEffectTargetId] + " Continent Income";
				SecondEffectIcon.text = DATA_RESOURCES.TEXT_SPRITE.SPRITE[(int)secondaryEffectTargetId];
			}
		}
		if (WorkerCard.IsLock)
		{
			return;
		}
		progress.color = WorkerCard.progress.color;
		progress.fillAmount = WorkerCard.progress.fillAmount;
		textCard.text = WorkerCard.textCard.text;
		CorridorWorker.gameObject.SetActive(WorkerCard.Area == ManagerArea.Corridor);
		ElevatorWorker.gameObject.SetActive(WorkerCard.Area == ManagerArea.Elevator);
		GroundWorker.gameObject.SetActive(WorkerCard.Area == ManagerArea.Ground);
		string text = ANIMATION.WORKER_SKIN[WorkerCard.id % 10];
		switch (WorkerCard.Area)
		{
		case ManagerArea.Corridor:
		{
			string animationName = "drill";
			if (text.Contains("common"))
			{
				animationName = "dig";
			}
			if (text.Contains("rare"))
			{
				animationName = "smash";
			}
			CorridorWorker.Skeleton.SetSkin(text);
			CorridorWorker.AnimationState.SetAnimation(0, animationName, loop: true);
			break;
		}
		case ManagerArea.Ground:
		{
			string animationName = "walk";
			if (text.Contains("rare"))
			{
				animationName = "walk_rare";
			}
			if (text.Contains("epic"))
			{
				animationName = "walk_epic";
			}
			if (text.Contains("legendary"))
			{
				animationName = "walk_legendary";
			}
			GroundWorker.Skeleton.SetSkin(text);
			GroundWorker.AnimationState.SetAnimation(0, animationName, loop: true);
			break;
		}
		case ManagerArea.Elevator:
			ElevatorWorker.Skeleton.SetSkin(text);
			break;
		}
	}
}
