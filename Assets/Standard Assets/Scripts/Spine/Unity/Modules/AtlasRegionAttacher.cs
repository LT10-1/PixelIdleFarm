using Spine.Unity.Modules.AttachmentTools;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Spine.Unity.Modules
{
	public class AtlasRegionAttacher : MonoBehaviour
	{
		[Serializable]
		public class SlotRegionPair
		{
			[SpineSlot("", "", false, true, false)]
			public string slot;

			[SpineAtlasRegion("")]
			public string region;
		}

		[SerializeField]
		protected AtlasAsset atlasAsset;

		[SerializeField]
		protected bool inheritProperties = true;

		[SerializeField]
		protected List<SlotRegionPair> attachments = new List<SlotRegionPair>();

		private Atlas atlas;

		private void Awake()
		{
			SkeletonRenderer component = GetComponent<SkeletonRenderer>();
			component.OnRebuild += Apply;
			if (component.valid)
			{
				Apply(component);
			}
		}

		private void Start()
		{
		}

		private void Apply(SkeletonRenderer skeletonRenderer)
		{
			if (base.enabled)
			{
				atlas = atlasAsset.GetAtlas();
				if (atlas != null)
				{
					float scale = skeletonRenderer.skeletonDataAsset.scale;
					foreach (SlotRegionPair attachment3 in attachments)
					{
						Slot slot = skeletonRenderer.Skeleton.FindSlot(attachment3.slot);
						Attachment attachment = slot.Attachment;
						AtlasRegion atlasRegion = atlas.FindRegion(attachment3.region);
						if (atlasRegion == null)
						{
							slot.Attachment = null;
						}
						else if (!inheritProperties || attachment == null)
						{
							RegionAttachment regionAttachment = (RegionAttachment)(slot.Attachment = atlasRegion.ToRegionAttachment(atlasRegion.name, scale));
						}
						else
						{
							slot.Attachment = attachment.GetRemappedClone(atlasRegion, cloneMeshAsLinked: true, useOriginalRegionSize: true, scale);
						}
					}
				}
			}
		}
	}
}
