using DG.Tweening;
using System;
using UnityEngine;

public class ElevatorWorkerController : CharacterControllerSekeleton
{
	public CoinController CoinController;

	public SpriteRenderer ResourceBg;

	public SpriteRenderer ResourceToElevator;

	public SpriteRenderer ElevatorBg;

	public SpriteRenderer ElevatorBgBack;

	public CharacterControllerSekeleton ResourceFromCorridor;

	public Transform CoinPositionNormal;

	public Transform CoinPositionLegendary;

	[HideInInspector]
	public ElevatorController ElevatorController;

	private double cashFromCurrentTier;

	private bool inAnimation;

	private float originalY;

	private Tweener ResourceToElevatorTweener;

	private ElevatorModel ElevatorModel => ElevatorController.ElevatorModel;

	private CorridorLevelController CurrentTierCorridor => base.MineController.CorridorController.CorridorLevelControllers[ElevatorModel.CurrentTier - 1];

	public override void Awake()
	{
		base.Awake();
		Vector3 localPosition = base.transform.localPosition;
		originalY = localPosition.y;
		ResourceToElevator.gameObject.SetActive(value: false);
		ResourceFromCorridor.gameObject.SetActive(value: false);
	}

	public override void Start()
	{
		base.Start();
		CoinController.SetCoinType(base.CurrentCoinType);
		ResourceFromCorridor.skeleton.SetSkin(ANIMATION.RESOURCE_SKIN[(int)base.CurrentContinent][base.MineController.MineIndex]);
		ResourceToElevator.sprite = BaseController.LoadSprite(DATA_RESOURCES.IMAGE.GAMEPLAY_ELEVATOR_EFFECT_RESOURCE(base.MineController.MineResourceName));
	}

	public override void Update()
	{
		base.Update();
	}

	private void OnMouseUp()
	{
		if (!base.CheckClickCanvas)
		{
			BaseController.GameController.AudioController.PlayOneShot("Audios/Effect/click");
			if (!inAnimation)
			{
				//BaseController.GameController.AnalyticController.LogEvent("click_worker_start_work", "area", ManagerArea.Elevator.ToString());
			}
			CheckStartWork();
		}
	}

	public void OnMoneyChange()
	{
		SetResourceBg();
	}

	public void SetResourceBg()
	{
		double num = CoinController.Cash / ElevatorModel.CapacityStat.Value;
		if (num < 0.02)
		{
			ResourceBg.gameObject.SetActive(value: false);
			return;
		}
		ResourceBg.gameObject.SetActive(value: true);
		int index = (!(num < 0.33333333333333331)) ? ((num < 2.0 / 3.0) ? 1 : 2) : 0;
		ResourceBg.sprite = BaseController.LoadSprite(DATA_RESOURCES.IMAGE.GAMEPLAY_ELEVATOR_RESOURCE(base.MineController.MineResourceName)[index]);
	}

	public void CheckStartWork()
	{
		BaseController.GameController.TutorialController.RemoveTutorial(TutorialType.WorkerElevator);
		if (!inAnimation)
		{
			StartCollecting();
		}
	}

	private void StartCollecting()
	{
		inAnimation = true;
		ElevatorModel.CurrentTier = 0;
		GoToNextCorridor();
	}

	private void GoToNextCorridor()
	{
		ElevatorModel.CurrentTier++;
		base.transform.DOLocalMoveY(originalY - 2.08f - 3.3f * (float)(ElevatorModel.CurrentTier - 1), (float)ElevatorController.ElevatorModel.SecondsToDriveOneTier).SetEase(Ease.Linear).onComplete = Collect;
	}

	private void Collect()
	{
		base.spineAnimationState.SetAnimation(0, "wave", loop: false);
		base.spineAnimationState.AddAnimation(0, "idle", loop: true, 0f);
		cashFromCurrentTier = Math.Min(ElevatorModel.CapacityStat.Value - CoinController.Cash, CurrentTierCorridor.CoinController.Cash);
		float duration = (float)(cashFromCurrentTier / ElevatorModel.LoadingPerSecondStat.Value);
		ElevatorController.ProgressBarController.Run(duration);
		if (MathUtils.CompareDoubleBiggerThanZero(cashFromCurrentTier))
		{
			CurrentTierCorridor.ElevatorStartCollect(duration);
		}
		Transform transform = base.transform;
		Vector3 localPosition = base.transform.localPosition;
		transform.DOLocalMoveY(localPosition.y, duration).onComplete = OnCollectEachCorridor;
	}

	private void OnCollectEachCorridor()
	{
		CoinController.AddCash(cashFromCurrentTier);
		OnMoneyChange();
		CurrentTierCorridor.CoinController.UseCash(cashFromCurrentTier);
		if (MathUtils.CompareDoubleToZero(CurrentTierCorridor.CoinController.Cash))
		{
			CurrentTierCorridor.CorridorBoxResource.gameObject.SetActive(value: false);
		}
		if (CoinController.Cash >= ElevatorModel.CapacityStat.Value)
		{
			MoveBackToTop();
		}
		else if (base.MineController.CorridorController.NumberActiveCorridor <= ElevatorModel.CurrentTier)
		{
			MoveBackToTop();
		}
		else
		{
			GoToNextCorridor();
		}
	}

	private void MoveBackToTop()
	{
		base.transform.DOLocalMoveY(originalY, (float)ElevatorModel.SecondsToDriveFromCurrentCorridorToTop).SetEase(Ease.Linear).onComplete = TransferToElevatorHouse;
	}

	private void TransferToElevatorHouse()
	{
		base.spineAnimationState.SetAnimation(0, "wave", loop: false);
		base.spineAnimationState.AddAnimation(0, "idle", loop: true, 0f);
		float num = (float)(CoinController.Cash / ElevatorModel.LoadingPerSecondStat.Value);
		ElevatorController.ProgressBarController.Run(num);
		if (num > 0.5f)
		{
			StartResourceToElevatorAnimation();
		}
		Transform transform = base.transform;
		Vector3 localPosition = base.transform.localPosition;
		transform.DOLocalMoveY(localPosition.y, num).onComplete = OnCompleteCollecting;
	}

	private void OnCompleteCollecting()
	{
		inAnimation = false;
		StopResourceToElevator();
		ElevatorController.CompleteCollect();
		if (!ElevatorController.ElevatorManagerController.isEmpty)
		{
			StartCollecting();
		}
	}

	private void StartResourceToElevatorAnimation()
	{
		ResourceToElevator.gameObject.SetActive(value: true);
		ResourceToElevator.transform.localPosition = Vector3.zero;
		ResourceToElevatorTweener = ResourceToElevator.transform.DOLocalMoveY(3f, 1f).SetLoops(-1);
	}

	private void StopResourceToElevator()
	{
		if (ResourceToElevatorTweener != null)
		{
			ResourceToElevatorTweener.Kill();
		}
		ResourceToElevator.gameObject.SetActive(value: false);
	}
}
