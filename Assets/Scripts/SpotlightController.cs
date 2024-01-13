using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpotlightController : MonoBehaviour
{
	public Image ImageCenter;

	public Image ImageLeft;

	public Image ImageRight;

	public Image ImageTop;

	public Image ImageBottom;

	[HideInInspector]
	private List<Image> _images;

	public List<Image> Images => _images ?? (_images = new List<Image>
	{
		ImageCenter,
		ImageLeft,
		ImageRight,
		ImageTop,
		ImageBottom
	});

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void Show(Vector2 position)
	{
		base.gameObject.SetActive(value: true);
		base.transform.position = position;
		Images.ForEach(delegate(Image image)
		{
			if (image != ImageCenter)
			{
				image.raycastTarget = true;
			}
			DOTween.Kill(image);
			image.color = new Color(0f, 0f, 0f, 0f);
			image.DOFade(0.6f, 1f);
		});
	}

	public void Hide()
	{
		Images.ForEach(delegate(Image image)
		{
			image.raycastTarget = false;
			DOTween.Kill(image);
			image.DOFade(0f, 0.5f);
		});
	}
}
