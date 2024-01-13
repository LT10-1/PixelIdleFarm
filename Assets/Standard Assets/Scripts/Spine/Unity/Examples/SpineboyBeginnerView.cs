using UnityEngine;

namespace Spine.Unity.Examples
{
	public class SpineboyBeginnerView : MonoBehaviour
	{
		[Header("Components")]
		public SpineboyBeginnerModel model;

		public SkeletonAnimation skeletonAnimation;

		[SpineAnimation("", "", true, false)]
		public string run;

		[SpineAnimation("", "", true, false)]
		public string idle;

		[SpineAnimation("", "", true, false)]
		public string shoot;

		[SpineAnimation("", "", true, false)]
		public string jump;

		[SpineEvent("", "", true, false)]
		public string footstepEventName;

		[Header("Audio")]
		public float footstepPitchOffset = 0.2f;

		public float gunsoundPitchOffset = 0.13f;

		public AudioSource footstepSource;

		public AudioSource gunSource;

		public AudioSource jumpSource;

		[Header("Effects")]
		public ParticleSystem gunParticles;

		private SpineBeginnerBodyState previousViewState;

		private void Start()
		{
			if (!(skeletonAnimation == null))
			{
				model.ShootEvent += PlayShoot;
				skeletonAnimation.AnimationState.Event += HandleEvent;
			}
		}

		private void HandleEvent(TrackEntry trackEntry, Event e)
		{
			if (e.Data.Name == footstepEventName)
			{
				PlayFootstepSound();
			}
		}

		private void Update()
		{
			if (!(skeletonAnimation == null) && !(model == null))
			{
				if (skeletonAnimation.skeleton.FlipX != model.facingLeft)
				{
					Turn(model.facingLeft);
				}
				SpineBeginnerBodyState state = model.state;
				if (previousViewState != state)
				{
					PlayNewStableAnimation();
				}
				previousViewState = state;
			}
		}

		private void PlayNewStableAnimation()
		{
			SpineBeginnerBodyState state = model.state;
			if (previousViewState == SpineBeginnerBodyState.Jumping && state != SpineBeginnerBodyState.Jumping)
			{
				PlayFootstepSound();
			}
			string animationName;
			switch (state)
			{
			case SpineBeginnerBodyState.Jumping:
				jumpSource.Play();
				animationName = jump;
				break;
			case SpineBeginnerBodyState.Running:
				animationName = run;
				break;
			default:
				animationName = idle;
				break;
			}
			skeletonAnimation.AnimationState.SetAnimation(0, animationName, loop: true);
		}

		private void PlayFootstepSound()
		{
			footstepSource.Play();
			footstepSource.pitch = GetRandomPitch(footstepPitchOffset);
		}

		[ContextMenu("Check Tracks")]
		private void CheckTracks()
		{
			AnimationState animationState = skeletonAnimation.AnimationState;
			UnityEngine.Debug.Log(animationState.GetCurrent(0));
			UnityEngine.Debug.Log(animationState.GetCurrent(1));
		}

		public void PlayShoot()
		{
			TrackEntry trackEntry = skeletonAnimation.AnimationState.SetAnimation(1, shoot, loop: false);
			trackEntry.AttachmentThreshold = 1f;
			trackEntry.MixDuration = 0f;
			TrackEntry trackEntry2 = skeletonAnimation.state.AddEmptyAnimation(1, 0.5f, 0.1f);
			trackEntry2.AttachmentThreshold = 1f;
			gunSource.pitch = GetRandomPitch(gunsoundPitchOffset);
			gunSource.Play();
			gunParticles.Play();
		}

		public void Turn(bool facingLeft)
		{
			skeletonAnimation.Skeleton.FlipX = facingLeft;
		}

		public float GetRandomPitch(float maxPitchOffset)
		{
			return 1f + UnityEngine.Random.Range(0f - maxPitchOffset, maxPitchOffset);
		}
	}
}
