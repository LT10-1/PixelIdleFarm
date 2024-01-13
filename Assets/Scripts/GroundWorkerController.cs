using DG.Tweening;
using System;
using UnityEngine;

public class GroundWorkerController : CharacterControllerSekeleton
{
	[HideInInspector]
	public GroundController GroundController;

	[HideInInspector]
	public GroundWorkerModel GroundWorkerModel;

	public CoinController CoinController;

	public ProgressBarController ProgressBarController;

	public SpriteRenderer ResourceGameObject;

	private float originalX;

	private double extractionCash;

	[HideInInspector]
	public float groundDeltaX;

	[HideInInspector]
	public float delayTime;

	[HideInInspector]
	public bool inAnimation;

	[HideInInspector]
	public string animationIdle;

	[HideInInspector]
	public string animationWalk;

	[HideInInspector]
	public GroundWorkerState GroundWorkerState;

	private CoinController ElevatorHouseCoinController => base.MineController.ElevatorController.ElevatorHouseCoinController;

	public override void Start()
	{
		base.Start();
		ResourceGameObject.sprite = BaseController.LoadSprite(DATA_RESOURCES.IMAGE.GAMEPLAY_ELEVATOR_RESOURCE(base.MineController.MineResourceName)[0]);
		ResourceGameObject.gameObject.SetActive(value: false);
		CoinController.SetCoinType(base.CurrentCoinType);
		CoinController.hideWhenNoCash = true;
		Vector3 localPosition = base.transform.localPosition;
		originalX = localPosition.x;
		base.transform.localPosition -= groundDeltaX * Vector3.right;
	}

	private void OnMouseUp()
	{
		if (!base.CheckClickCanvas)
		{
			BaseController.GameController.AudioController.PlayOneShot("Audios/Effect/click");
			if (!inAnimation)
			{
				GroundWorkerState = GroundWorkerState.Clicked;
				//BaseController.GameController.AnalyticController.LogEvent("click_worker_start_work", "area", ManagerArea.Ground.ToString());
			}
			GroundController.CheckStartWork();
		}
	}

	public void StartWithDelay()
	{
		inAnimation = true;
		Transform transform = base.transform;
		Vector3 localPosition = base.transform.localPosition;
		transform.DOLocalMoveX(localPosition.x, delayTime).onComplete = StartMoving;
	}

	public void StartMoving()
	{
		inAnimation = true;
		GroundWorkerState = GroundWorkerState.WalkingToElevator;
		BaseController.GameController.TutorialController.RemoveTutorial(TutorialType.WorkerGround);
		base.spineAnimationState.SetAnimation(0, animationWalk, loop: true);
		base.transform.DOLocalMoveX(originalX - groundDeltaX - 3.5f, (float)GroundWorkerModel.SecondsOneWayStat.Value).SetEase(Ease.Linear).onComplete = StartCollecting;
	}

	private void StartCollecting()
	{
		GroundWorkerState = GroundWorkerState.Loading;
		base.spineAnimationState.SetAnimation(0, animationIdle, loop: true);
		extractionCash = Math.Min(GroundWorkerModel.CapacityStat.Value, ElevatorHouseCoinController.loadableCash);
		ElevatorHouseCoinController.loadableCash -= extractionCash;
		float num = (float)(extractionCash / GroundWorkerModel.LoadingPerSecondStat.Value);
		ProgressBarController.Run(num);
		if (MathUtils.CompareDoubleBiggerThanZero(extractionCash))
		{
			base.MineController.ElevatorController.StartSquirrelAnimation(num);
		}
		Transform transform = base.transform;
		Vector3 localPosition = base.transform.localPosition;
		transform.DOLocalMoveX(localPosition.x, num).onComplete = StartMoveBack;
	}

	private void StartMoveBack()
	{
		GroundWorkerState = GroundWorkerState.WalkingToHouse;
		ElevatorHouseCoinController.loadableCash += extractionCash;
		ElevatorHouseCoinController.UseCash(extractionCash);
		CoinController.AddCash(extractionCash);
		if (MathUtils.CompareDoubleBiggerThanZero(CoinController.Cash))
		{
			ResourceGameObject.gameObject.SetActive(value: true);
		}
		GroundController.CheckWorkerLoading();
		base.skeleton.FlipX = true;
		base.spineAnimationState.SetAnimation(0, animationWalk, loop: true);
		base.transform.DOLocalMoveX(originalX - groundDeltaX, (float)GroundWorkerModel.SecondsOneWayStat.Value).SetEase(Ease.Linear).onComplete = TransferToWareHouse;
	}

	private void TransferToWareHouse()
	{
		GroundWorkerState = GroundWorkerState.Unloading;
		base.spineAnimationState.SetAnimation(0, animationIdle, loop: true);
		float duration = (float)(extractionCash / GroundWorkerModel.LoadingPerSecondStat.Value);
		ProgressBarController.Run(duration);
		Transform transform = base.transform;
		Vector3 localPosition = base.transform.localPosition;
		transform.DOLocalMoveX(localPosition.x, duration).onComplete = OnCompleteCollecting;
	}

	private void OnCompleteCollecting()
	{
		inAnimation = false;
		base.skeleton.FlipX = false;
		base.spineAnimationState.SetAnimation(0, animationIdle, loop: true);
		ResourceGameObject.gameObject.SetActive(value: false);
		GroundWorkerState = GroundWorkerState.Waiting;
		if (CoinController.Cash > 0.0)
		{
			CoinController component = UnityEngine.Object.Instantiate(CoinController.gameObject).GetComponent<CoinController>();
			component.SetCoinType(base.CurrentCoinType);
			component.transform.SetParent(GroundController.transform, worldPositionStays: false);
			component.transform.localPosition = CoinController.transform.localPosition;
			component.transform.localScale = CoinController.transform.localScale;
			component.SetMoney(CoinController.Cash, minify: true, showMoney: true, string.Empty);
			if (BaseController.GameController.BoostController.HaveBoost)
			{
				CoinController coinController = component;
				CoinController coinController2 = component;
				string text = coinController2.text;
				text = (coinController.text = (coinController2.text = text + "\n            <color=\"yellow\">x " + BaseController.GameController.BoostController.TotalBoostFactor.MinifyIncomeFactor() + " " + DATA_RESOURCES.TEXT_SPRITE.BOLT));
			}
			component.FlyUpAnimation();
		}
		GroundController.OnWorkerComplete(this);
	}
}
