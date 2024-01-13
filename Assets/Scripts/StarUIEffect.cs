using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class StarUIEffect : MonoBehaviour
{
	private float timeScale = 1.5f;

	public Image star => GetComponentInChildren<Image>();

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void init(Vector2 position, Vector2 box)
	{
		Sequence s = DOTween.Sequence();
		base.gameObject.transform.localPosition = position + new Vector2(UnityEngine.Random.Range(0f, box.x), 0f - UnityEngine.Random.Range(0f, box.y));
		base.gameObject.transform.localEulerAngles = Vector3.forward * UnityEngine.Random.Range(0, 360);
		star.transform.localScale = Vector2.zero;
		s.AppendInterval(UnityEngine.Random.Range(1f, 3f)).AppendCallback(delegate
		{
			star.DOFade(1f, timeScale);
		}).Append(star.transform.DOScale(1f, timeScale))
			.AppendCallback(delegate
			{
				star.DOFade(0f, timeScale);
			})
			.Append(star.transform.DOScale(0f, timeScale))
			.AppendCallback(delegate
			{
				base.gameObject.transform.localPosition = position + new Vector2(UnityEngine.Random.Range(0f, box.x), 0f - UnityEngine.Random.Range(0f, box.y));
				base.gameObject.transform.localEulerAngles = Vector3.forward * UnityEngine.Random.Range(0, 360);
			})
			.SetLoops(-1);
	}
}
