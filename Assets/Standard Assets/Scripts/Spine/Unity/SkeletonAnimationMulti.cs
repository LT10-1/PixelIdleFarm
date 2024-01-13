using System.Collections.Generic;
using UnityEngine;

namespace Spine.Unity
{
	public class SkeletonAnimationMulti : MonoBehaviour
	{
		private const int MainTrackIndex = 0;

		public bool initialFlipX;

		public bool initialFlipY;

		public string initialAnimation;

		public bool initialLoop;

		[Space]
		public List<SkeletonDataAsset> skeletonDataAssets = new List<SkeletonDataAsset>();

		[Header("Settings")]
		public MeshGenerator.Settings meshGeneratorSettings = MeshGenerator.Settings.Default;

		private readonly List<SkeletonAnimation> skeletonAnimations = new List<SkeletonAnimation>();

		private readonly Dictionary<string, Animation> animationNameTable = new Dictionary<string, Animation>();

		private readonly Dictionary<Animation, SkeletonAnimation> animationSkeletonTable = new Dictionary<Animation, SkeletonAnimation>();

		private SkeletonAnimation currentSkeletonAnimation;

		public Dictionary<Animation, SkeletonAnimation> AnimationSkeletonTable => animationSkeletonTable;

		public Dictionary<string, Animation> AnimationNameTable => animationNameTable;

		public SkeletonAnimation CurrentSkeletonAnimation => currentSkeletonAnimation;

		private void Clear()
		{
			foreach (SkeletonAnimation skeletonAnimation in skeletonAnimations)
			{
				UnityEngine.Object.Destroy(skeletonAnimation.gameObject);
			}
			skeletonAnimations.Clear();
			animationNameTable.Clear();
			animationSkeletonTable.Clear();
		}

		private void SetActiveSkeleton(SkeletonAnimation skeletonAnimation)
		{
			foreach (SkeletonAnimation skeletonAnimation2 in skeletonAnimations)
			{
				skeletonAnimation2.gameObject.SetActive(skeletonAnimation2 == skeletonAnimation);
			}
			currentSkeletonAnimation = skeletonAnimation;
		}

		private void Awake()
		{
			Initialize(overwrite: false);
		}

		public void Initialize(bool overwrite)
		{
			if (skeletonAnimations.Count == 0 || overwrite)
			{
				Clear();
				MeshGenerator.Settings meshSettings = meshGeneratorSettings;
				Transform transform = base.transform;
				foreach (SkeletonDataAsset skeletonDataAsset in skeletonDataAssets)
				{
					SkeletonAnimation skeletonAnimation = SkeletonAnimation.NewSkeletonAnimationGameObject(skeletonDataAsset);
					skeletonAnimation.transform.SetParent(transform, worldPositionStays: false);
					skeletonAnimation.SetMeshSettings(meshSettings);
					skeletonAnimation.initialFlipX = initialFlipX;
					skeletonAnimation.initialFlipY = initialFlipY;
					Skeleton skeleton = skeletonAnimation.skeleton;
					skeleton.FlipX = initialFlipX;
					skeleton.FlipY = initialFlipY;
					skeletonAnimation.Initialize(overwrite: false);
					skeletonAnimations.Add(skeletonAnimation);
				}
				Dictionary<string, Animation> dictionary = animationNameTable;
				Dictionary<Animation, SkeletonAnimation> dictionary2 = animationSkeletonTable;
				foreach (SkeletonAnimation skeletonAnimation2 in skeletonAnimations)
				{
					foreach (Animation animation in skeletonAnimation2.Skeleton.Data.Animations)
					{
						dictionary[animation.Name] = animation;
						dictionary2[animation] = skeletonAnimation2;
					}
				}
				SetActiveSkeleton(skeletonAnimations[0]);
				SetAnimation(initialAnimation, initialLoop);
			}
		}

		public Animation FindAnimation(string animationName)
		{
			animationNameTable.TryGetValue(animationName, out Animation value);
			return value;
		}

		public TrackEntry SetAnimation(string animationName, bool loop)
		{
			return SetAnimation(FindAnimation(animationName), loop);
		}

		public TrackEntry SetAnimation(Animation animation, bool loop)
		{
			if (animation == null)
			{
				return null;
			}
			animationSkeletonTable.TryGetValue(animation, out SkeletonAnimation value);
			if (value != null)
			{
				SetActiveSkeleton(value);
				value.skeleton.SetToSetupPose();
				return value.state.SetAnimation(0, animation, loop);
			}
			return null;
		}

		public void SetEmptyAnimation(float mixDuration)
		{
			currentSkeletonAnimation.state.SetEmptyAnimation(0, mixDuration);
		}

		public void ClearAnimation()
		{
			currentSkeletonAnimation.state.ClearTrack(0);
		}

		public TrackEntry GetCurrent()
		{
			return currentSkeletonAnimation.state.GetCurrent(0);
		}
	}
}
