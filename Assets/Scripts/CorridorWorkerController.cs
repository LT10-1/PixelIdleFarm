using DG.Tweening;
using Spine;
using UnityEngine;

public class CorridorWorkerController : CharacterControllerSekeleton
{
	[HideInInspector]
	public CorridorLevelController CorridorLevelController;

	[HideInInspector]
	public CorridorWorkerModel CorridorWorkerModel;

	private float originalX;

	private bool useTransition;

	[HideInInspector]
	public float corridorLength;

	[HideInInspector]
	public float corridorDeltaX;

	[HideInInspector]
	public float delayTime;

	[HideInInspector]
	public bool inAnimation;

	[HideInInspector]
	public string digAnimation = "dig";

	[HideInInspector]
	public CorridorWorkerState CorridorWorkerState;

	public override void Start()
	{
		base.Start();
		Vector3 localPosition = base.transform.localPosition;
		originalX = localPosition.x;
		base.transform.localPosition += corridorDeltaX * Vector3.right;
		base.spineAnimationState.End += delegate(TrackEntry entry)
		{
			if (!(entry.Animation.Name == "T_bag_idle"))
			{
			}
		};
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
			if (!CorridorLevelController.isWorking)
			{
				CorridorWorkerState = CorridorWorkerState.Clicked;
				//BaseController.GameController.AnalyticController.LogEvent("click_worker_start_work", "area", ManagerArea.Corridor.ToString());
			}
			CorridorLevelController.CheckStartWork();
		}
	}

	public void StartWithDelay()
	{
		inAnimation = true;
		Transform transform = base.transform;
		Vector3 localPosition = base.transform.localPosition;
		transform.DOLocalMoveX(localPosition.x, delayTime).onComplete = StartMining;
	}

	public void StartMining()
	{
		inAnimation = true;
		CorridorWorkerState = CorridorWorkerState.WalkingFromStorageToStones;
		BaseController.GameController.TutorialController.RemoveTutorial(TutorialType.WorkerCorridor);
		base.skeleton.FlipX = true;
		if (useTransition)
		{
			base.spineAnimationState.SetAnimation(0, "T_idle_walk", loop: false);
			base.spineAnimationState.AddAnimation(0, "walk", loop: true, 0f);
		}
		else
		{
			base.spineAnimationState.SetAnimation(0, "walk", loop: true);
		}
		base.transform.DOLocalMoveX(originalX + corridorDeltaX + corridorLength, (float)CorridorWorkerModel.SecondsOneWayStat.Value).SetEase(Ease.Linear).onComplete = StartDigging;
	}

	private void StartDigging()
	{
		CorridorWorkerState = CorridorWorkerState.WorkingOnStones;
		if (useTransition)
		{
			base.spineAnimationState.SetAnimation(0, "T_walk_dig", loop: false);
			base.spineAnimationState.AddAnimation(0, digAnimation, loop: true, 0f);
		}
		else
		{
			base.spineAnimationState.SetAnimation(0, digAnimation, loop: true);
		}
		if (CorridorLevelController.CorridorModel.BonusContainer.TotalGainPerSecondFactor > 1.0)
		{
			base.skeletonAnimation.timeScale = (float)CorridorLevelController.CorridorModel.BonusContainer.TotalGainPerSecondFactor;
		}
		float num = (float)CorridorWorkerModel.SecondsTillCapacityReachedStat.Value;
		if (CONST.TEST_MODE_VIDEO_AD)
		{
			num *= 100000f;
		}
		if (!CONST.TEST_MODE_VIDEO_AD)
		{
			CorridorLevelController.RunProgressBar(num);
		}
		Transform transform = base.transform;
		Vector3 localPosition = base.transform.localPosition;
		transform.DOLocalMoveX(localPosition.x, num).onComplete = StartWalkBack;
	}

	private void StartWalkBack()
	{
		base.skeleton.FlipX = false;
		CorridorWorkerState = CorridorWorkerState.WalkingFromStonesToStorage;
		base.skeletonAnimation.timeScale = 1f;
		base.spineAnimationState.SetAnimation(0, "walk_bag", loop: true);
		base.transform.DOLocalMoveX(originalX + corridorDeltaX, (float)CorridorWorkerModel.SecondsOneWayStat.Value).SetEase(Ease.Linear).onComplete = OnCompleteMining;
		CorridorLevelController.ChangeSandSprite();
	}

	private void OnCompleteMining()
	{
		inAnimation = false;
		base.skeleton.FlipX = true;
		CorridorWorkerState = CorridorWorkerState.Idle;
		if (useTransition)
		{
			base.spineAnimationState.SetAnimation(0, "T_bag_idle", loop: false);
			base.spineAnimationState.AddAnimation(0, "idle", loop: true, 0f);
		}
		else
		{
			base.spineAnimationState.SetAnimation(0, "idle", loop: true);
		}
		CorridorLevelController.OnWorkerComplete(this, CorridorWorkerModel.CapacityStat.Value);
	}
}
