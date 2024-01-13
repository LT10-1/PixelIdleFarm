using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Wave : BaseWorldmapController
{
	private Image image;

	private float timeScale = 1.5f;

	public override void Start()
	{
		image = GetComponent<Image>();
		StartCoroutine(doWave());
	}

	private IEnumerator doWave()
	{
		timeScale = 3f + Random.Range(0f, 4f);
		image.rectTransform.localScale = Vector2.zero;
		image.color = new Color(1f, 1f, 1f, 1f);
		image.rectTransform.DOScale(1f, timeScale);
		image.DOFade(0f, timeScale + 2f);
		yield return new WaitForSeconds(timeScale + Random.Range(2f, 3f));
		StartCoroutine(doWave());
	}

	public override void Update()
	{
	}
}
