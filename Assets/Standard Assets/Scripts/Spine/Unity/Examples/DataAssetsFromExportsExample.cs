using System.Collections;
using UnityEngine;

namespace Spine.Unity.Examples
{
	public class DataAssetsFromExportsExample : MonoBehaviour
	{
		public TextAsset skeletonJson;

		public TextAsset atlasText;

		public Texture2D[] textures;

		public Material materialPropertySource;

		private AtlasAsset runtimeAtlasAsset;

		private SkeletonDataAsset runtimeSkeletonDataAsset;

		private SkeletonAnimation runtimeSkeletonAnimation;

		private void CreateRuntimeAssetsAndGameObject()
		{
			runtimeAtlasAsset = AtlasAsset.CreateRuntimeInstance(atlasText, textures, materialPropertySource, initialize: true);
			runtimeSkeletonDataAsset = SkeletonDataAsset.CreateRuntimeInstance(skeletonJson, runtimeAtlasAsset, initialize: true);
		}

		private IEnumerator Start()
		{
			CreateRuntimeAssetsAndGameObject();
			runtimeSkeletonDataAsset.GetSkeletonData(quiet: false);
			yield return new WaitForSeconds(0.5f);
			runtimeSkeletonAnimation = SkeletonAnimation.NewSkeletonAnimationGameObject(runtimeSkeletonDataAsset);
			runtimeSkeletonAnimation.Initialize(overwrite: false);
			runtimeSkeletonAnimation.Skeleton.SetSkin("base");
			runtimeSkeletonAnimation.Skeleton.SetSlotsToSetupPose();
			runtimeSkeletonAnimation.AnimationState.SetAnimation(0, "run", loop: true);
			runtimeSkeletonAnimation.GetComponent<MeshRenderer>().sortingOrder = 10;
			runtimeSkeletonAnimation.transform.Translate(Vector3.down * 2f);
		}
	}
}
