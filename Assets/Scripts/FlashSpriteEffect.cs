using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;

public class FlashSpriteEffect : MonoBehaviour
{
	public SpriteRenderer flash;

	private void Start()
	{
		base.gameObject.transform.localPosition = Vector3.zero;
		base.gameObject.transform.localScale = new Vector2(0.4f, 1f);
	}

	private void Update()
	{
	}

	public void InitEffect(SpriteRenderer image)
	{
		base.transform.SetParent(image.transform);
		if (image.gameObject.GetComponent<SpriteMask>() == null)
		{
			SpriteMask spriteMask = image.gameObject.AddComponent<SpriteMask>();
			spriteMask.sprite = image.sprite;
			image.gameObject.AddComponent<SortingGroup>();
		}
		Transform transform = flash.transform;
		Vector3 size = image.bounds.size;
		transform.localPosition = new Vector3(size.x * 4f / 3f, 0f, 0f);
		Transform transform2 = flash.transform;
		Vector3 size2 = image.bounds.size;
		transform2.DOLocalMoveX(0f - size2.x, 1f);
		doMove(image);
	}

	private void doMove(SpriteRenderer image)
	{
		Transform transform = flash.transform;
		Vector3 size = image.bounds.size;
		transform.localPosition = new Vector3(size.x * 4f / 3f, 0f, 0f);
		Transform transform2 = flash.transform;
		Vector3 size2 = image.bounds.size;
		transform2.DOLocalMoveX(0f - size2.x, 1f).OnComplete(delegate
		{
			doMove(image);
		}).SetDelay(2f);
	}

	public void stopEffect()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
