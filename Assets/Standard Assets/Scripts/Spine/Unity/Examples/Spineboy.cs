using UnityEngine;

namespace Spine.Unity.Examples
{
	public class Spineboy : MonoBehaviour
	{
		private SkeletonAnimation skeletonAnimation;

		public void Start()
		{
			skeletonAnimation = GetComponent<SkeletonAnimation>();
			AnimationState animationState = skeletonAnimation.AnimationState;
			animationState.Event += HandleEvent;
			animationState.End += delegate(TrackEntry entry)
			{
				UnityEngine.Debug.Log("start: " + entry.TrackIndex);
			};
			animationState.AddAnimation(0, "jump", loop: false, 2f);
			animationState.AddAnimation(0, "run", loop: true, 0f);
		}

		private void HandleEvent(TrackEntry trackEntry, Event e)
		{
			UnityEngine.Debug.Log(trackEntry.TrackIndex + " " + trackEntry.Animation.Name + ": event " + e + ", " + e.Int);
		}

		public void OnMouseDown()
		{
			skeletonAnimation.AnimationState.SetAnimation(0, "jump", loop: false);
			skeletonAnimation.AnimationState.AddAnimation(0, "run", loop: true, 0f);
		}
	}
}
