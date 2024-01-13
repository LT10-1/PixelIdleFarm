using System.Collections;
using UnityEngine;

namespace Spine.Unity.Examples
{
	public class Raptor : MonoBehaviour
	{
		[SpineAnimation("", "", true, false)]
		public string walk = "walk";

		[SpineAnimation("", "", true, false)]
		public string gungrab = "gungrab";

		[SpineAnimation("", "", true, false)]
		public string gunkeep = "gunkeep";

		[SpineEvent("", "", true, false)]
		public string footstepEvent = "footstep";

		public AudioSource footstepAudioSource;

		private SkeletonAnimation skeletonAnimation;

		private void Start()
		{
			skeletonAnimation = GetComponent<SkeletonAnimation>();
			skeletonAnimation.AnimationState.Event += HandleEvent;
			StartCoroutine(GunGrabRoutine());
		}

		private void HandleEvent(TrackEntry trackEntry, Event e)
		{
			if (e.Data.Name == footstepEvent)
			{
				footstepAudioSource.pitch = 0.5f + UnityEngine.Random.Range(-0.2f, 0.2f);
				footstepAudioSource.Play();
			}
		}

		private IEnumerator GunGrabRoutine()
		{
			skeletonAnimation.AnimationState.SetAnimation(0, walk, loop: true);
			while (true)
			{
				yield return new WaitForSeconds(UnityEngine.Random.Range(0.5f, 3f));
				skeletonAnimation.AnimationState.SetAnimation(1, gungrab, loop: false);
				yield return new WaitForSeconds(UnityEngine.Random.Range(0.5f, 3f));
				skeletonAnimation.AnimationState.SetAnimation(1, gunkeep, loop: false);
			}
		}
	}
}
