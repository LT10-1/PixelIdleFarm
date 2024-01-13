using System;
using System.Collections.Generic;
using UnityEngine;

namespace Spine.Unity.Examples
{
	public class SkeletonColorInitialize : MonoBehaviour
	{
		[Serializable]
		public class SlotSettings
		{
			[SpineSlot("", "", false, true, false)]
			public string slot = string.Empty;

			public Color color = Color.white;
		}

		public Color skeletonColor = Color.white;

		public List<SlotSettings> slotSettings = new List<SlotSettings>();

		private void Start()
		{
			ApplySettings();
		}

		private void ApplySettings()
		{
			ISkeletonComponent component = GetComponent<ISkeletonComponent>();
			if (component != null)
			{
				Skeleton skeleton = component.Skeleton;
				skeleton.SetColor(skeletonColor);
				foreach (SlotSettings slotSetting in slotSettings)
				{
					skeleton.FindSlot(slotSetting.slot)?.SetColor(slotSetting.color);
				}
			}
		}
	}
}
