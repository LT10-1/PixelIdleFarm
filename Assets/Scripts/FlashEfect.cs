using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FlashEfect : MonoBehaviour
{
	public Image maskImage;

	public Image flashImage;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void InitEffect(Image image)
	{
		base.transform.SetParent(image.transform, worldPositionStays: false);
		maskImage.rectTransform.sizeDelta = image.rectTransform.sizeDelta;
		maskImage.overrideSprite = image.overrideSprite;
		RectTransform rectTransform = flashImage.rectTransform;
		Vector2 sizeDelta = image.rectTransform.sizeDelta;
		rectTransform.sizeDelta = new Vector2(90f, sizeDelta.y * 3f);
		RectTransform rectTransform2 = flashImage.rectTransform;
		Vector2 sizeDelta2 = image.rectTransform.sizeDelta;
		rectTransform2.localPosition = new Vector3(sizeDelta2.x * 4f / 3f, 0f, 0f);
		RectTransform rectTransform3 = flashImage.rectTransform;
		Vector2 sizeDelta3 = image.rectTransform.sizeDelta;
		rectTransform3.DOLocalMoveX((0f - sizeDelta3.x) * 2f, 1f);
		doMove(image);
	}

	private void doMove(Image image)
	{
		RectTransform rectTransform = flashImage.rectTransform;
		Vector2 sizeDelta = image.rectTransform.sizeDelta;
		rectTransform.localPosition = new Vector3(sizeDelta.x * 4f / 3f, 0f, 0f);
		RectTransform rectTransform2 = flashImage.rectTransform;
		Vector2 sizeDelta2 = image.rectTransform.sizeDelta;
		rectTransform2.DOLocalMoveX((0f - sizeDelta2.x) * 2f, 1f).OnComplete(delegate
		{
			doMove(image);
		}).SetDelay(2f);
	}

	public void stopEffect()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
