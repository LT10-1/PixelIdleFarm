using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Spine.Unity.Examples
{
	public class AttackSpineboy : MonoBehaviour
	{
		public SkeletonAnimation spineboy;

		public SpineGauge gauge;

		public Text healthText;

		private int currentHealth = 100;

		private const int maxHealth = 100;

		public UnityEvent onAttack;

		private void Update()
		{
			if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
			{
				currentHealth -= 10;
				healthText.text = currentHealth + "/" + 100;
				if (currentHealth > 0)
				{
					spineboy.AnimationState.SetAnimation(0, "hit", loop: false);
					spineboy.AnimationState.AddAnimation(0, "idle", loop: true, 0f);
					gauge.fillPercent = (float)currentHealth / 100f;
					onAttack.Invoke();
				}
				else if (currentHealth >= 0)
				{
					gauge.fillPercent = 0f;
					spineboy.AnimationState.SetAnimation(0, "death", loop: false).TrackEnd = float.PositiveInfinity;
				}
			}
		}
	}
}
