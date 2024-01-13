using Spine;
using Spine.Unity;
using System;

public class CharacterControllerSekeleton : BaseMineController
{
	private SkeletonAnimation _skeletonAnimation;

	private AnimationState _spineAnimationState;

	private Skeleton _skeleton;

	private bool isRunning;

	private string currentAnimation = string.Empty;

	private long endTime;

	public SkeletonAnimation skeletonAnimation
	{
		get
		{
			if (_skeletonAnimation == null)
			{
				InitSpineParams();
			}
			return _skeletonAnimation;
		}
	}

	public AnimationState spineAnimationState
	{
		get
		{
			if (_spineAnimationState == null)
			{
				InitSpineParams();
			}
			return _spineAnimationState;
		}
	}

	public Skeleton skeleton
	{
		get
		{
			if (_skeleton == null)
			{
				InitSpineParams();
			}
			return _skeleton;
		}
	}

	public override void Start()
	{
		base.Start();
		InitSpineParams();
	}

	public void InitSpineParams()
	{
		_skeletonAnimation = GetComponent<SkeletonAnimation>();
		if (_skeletonAnimation == null)
		{
			SkeletonAnimation[] componentsInChildren = GetComponentsInChildren<SkeletonAnimation>(includeInactive: true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i].GetComponent<CharacterControllerSekeleton>() == null)
				{
					_skeletonAnimation = componentsInChildren[i];
					break;
				}
			}
		}
		_spineAnimationState = skeletonAnimation.AnimationState;
		_skeleton = skeletonAnimation.Skeleton;
	}

	public void StartAnimation(double duration = 0.0, string animationName = "animation")
	{
		if (!isRunning)
		{
			isRunning = true;
			currentAnimation = animationName;
			spineAnimationState.Complete -= OnCompleteAnimation;
			spineAnimationState.Complete += OnCompleteAnimation;
			if (!MathUtils.CompareDoubleToZero(duration))
			{
				endTime = Math.Max(DateTime.Now.Ticks + (long)(10000000.0 * duration), endTime);
			}
			OnCompleteAnimation(null);
		}
	}

	public void StopAnimation()
	{
		isRunning = false;
		endTime = 0L;
	}

	public void OnCompleteAnimation(TrackEntry trackEntry)
	{
		if (isRunning)
		{
			if (endTime != 0 && DateTime.Now.Ticks > endTime)
			{
				StopAnimation();
			}
			else
			{
				spineAnimationState.SetAnimation(0, currentAnimation, loop: false);
			}
		}
	}
}
