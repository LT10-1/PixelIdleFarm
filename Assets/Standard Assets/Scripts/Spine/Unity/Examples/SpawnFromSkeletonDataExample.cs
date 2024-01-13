using System.Collections;
using UnityEngine;

namespace Spine.Unity.Examples
{
	public class SpawnFromSkeletonDataExample : MonoBehaviour
	{
		public SkeletonDataAsset skeletonDataAsset;

		[Range(0f, 100f)]
		public int count = 20;

		[SpineAnimation("", "skeletonDataAsset", true, false)]
		public string startingAnimation;

		private IEnumerator Start()
		{
			if (!(skeletonDataAsset == null))
			{
				skeletonDataAsset.GetSkeletonData(quiet: false);
				yield return new WaitForSeconds(1f);
				Animation spineAnimation = skeletonDataAsset.GetSkeletonData(quiet: false).FindAnimation(startingAnimation);
				for (int i = 0; i < count; i++)
				{
					SkeletonAnimation sa = SkeletonAnimation.NewSkeletonAnimationGameObject(skeletonDataAsset);
					DoExtraStuff(sa, spineAnimation);
					sa.gameObject.name = i.ToString();
					yield return new WaitForSeconds(0.125f);
				}
			}
		}

		private void DoExtraStuff(SkeletonAnimation sa, Animation spineAnimation)
		{
			sa.transform.localPosition = UnityEngine.Random.insideUnitCircle * 6f;
			sa.transform.SetParent(base.transform, worldPositionStays: false);
			if (spineAnimation != null)
			{
				sa.Initialize(overwrite: false);
				sa.AnimationState.SetAnimation(0, spineAnimation, loop: true);
			}
		}
	}
}
