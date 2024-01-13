using DG.Tweening;
using System.Collections;
using UnityEngine;

public class SunController : MonoBehaviour
{
	public SpriteRenderer Sun;

	public SpriteRenderer SunGlow;

	private float width;

	private void Start()
	{
		SunGlow.color = new Color(1f, 1f, 1f, 0f);
		StartCoroutine(StartGlow());
	}

	private IEnumerator StartGlow()
	{
		yield return new WaitForSeconds(UnityEngine.Random.Range(4, 7));
		SunGlow.DOColor(new Color(1f, 1f, 1f, 1f), 1.5f).OnComplete(delegate
		{
			SunGlow.DOColor(new Color(1f, 1f, 1f, 0f), 1.5f);
		});
		StartCoroutine(StartGlow());
	}
}
