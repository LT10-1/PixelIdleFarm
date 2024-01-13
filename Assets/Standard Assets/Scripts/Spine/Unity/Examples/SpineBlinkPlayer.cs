using System.Collections;
using UnityEngine;

namespace Spine.Unity.Examples
{
	public class SpineBlinkPlayer : MonoBehaviour
	{
		private const int BlinkTrack = 1;

		[SpineAnimation("", "", true, false)]
		public string blinkAnimation;

		public float minimumDelay = 0.15f;

		public float maximumDelay = 3f;

		private IEnumerator Start()
		{
			SkeletonAnimation skeletonAnimation = GetComponent<SkeletonAnimation>();
			if (skeletonAnimation == null)
			{
				yield break;
			}
			while (true)
			{
				skeletonAnimation.AnimationState.SetAnimation(1, blinkAnimation, loop: false);
				yield return new WaitForSeconds(UnityEngine.Random.Range(minimumDelay, maximumDelay));
			}
		}
	}
}
