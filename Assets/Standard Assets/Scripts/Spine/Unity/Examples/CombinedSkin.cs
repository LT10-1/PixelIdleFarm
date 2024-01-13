using Spine.Unity.Modules.AttachmentTools;
using System.Collections.Generic;
using UnityEngine;

namespace Spine.Unity.Examples
{
	public class CombinedSkin : MonoBehaviour
	{
		[SpineSkin("", "", true, false)]
		public List<string> skinsToCombine;

		private Skin combinedSkin;

		private void Start()
		{
			ISkeletonComponent component = GetComponent<ISkeletonComponent>();
			if (component != null)
			{
				Skeleton skeleton = component.Skeleton;
				if (skeleton != null)
				{
					combinedSkin = (combinedSkin ?? new Skin("combined"));
					combinedSkin.Clear();
					foreach (string item in skinsToCombine)
					{
						Skin skin = skeleton.Data.FindSkin(item);
						if (skin != null)
						{
							combinedSkin.Append(skin);
						}
					}
					skeleton.SetSkin(combinedSkin);
					skeleton.SetToSetupPose();
					(component as IAnimationStateComponent)?.AnimationState.Apply(skeleton);
				}
			}
		}
	}
}
