using UnityEngine;

namespace Spine.Unity.Examples
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(SkeletonRenderer))]
	public class SpineGauge : MonoBehaviour
	{
		[Range(0f, 1f)]
		public float fillPercent;

		[SpineAnimation("", "", true, false)]
		public string fillAnimationName;

		private SkeletonRenderer skeletonRenderer;

		private Animation fillAnimation;

		private void Awake()
		{
			skeletonRenderer = GetComponent<SkeletonRenderer>();
		}

		private void Update()
		{
			SetGaugePercent(fillPercent);
		}

		public void SetGaugePercent(float percent)
		{
			if (skeletonRenderer == null)
			{
				return;
			}
			Skeleton skeleton = skeletonRenderer.skeleton;
			if (skeleton == null)
			{
				return;
			}
			if (fillAnimation == null)
			{
				fillAnimation = skeleton.Data.FindAnimation(fillAnimationName);
				if (fillAnimation == null)
				{
					return;
				}
			}
			fillAnimation.Apply(skeleton, 0f, percent, loop: false, null, 1f, MixPose.Setup, MixDirection.In);
			skeleton.Update(Time.deltaTime);
			skeleton.UpdateWorldTransform();
		}
	}
}
