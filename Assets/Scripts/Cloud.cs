using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Cloud : MonoBehaviour
{
	public int widthMap = 1400;

	public int heightMap = 1920;

	private Image image;

	private void Start()
	{
		image = GetComponent<Image>();
		StartCoroutine(doMove());
	}

	private IEnumerator doMove()
	{
		float timeScale = UnityEngine.Random.Range(15, 30);
		float _scale = UnityEngine.Random.Range(0.6f, 1f);
		RectTransform rectTransform = image.rectTransform;
		float num = widthMap / 2;
		Vector2 sizeDelta = image.rectTransform.sizeDelta;
		rectTransform.localPosition = new Vector2(num + sizeDelta.x / 2f, UnityEngine.Random.Range(-heightMap / 2, heightMap / 2));
		base.gameObject.transform.localScale = new Vector2(_scale, _scale);
		RectTransform rectTransform2 = image.rectTransform;
		float num2 = -widthMap / 2;
		Vector2 sizeDelta2 = image.rectTransform.sizeDelta;
		rectTransform2.DOLocalMoveX(num2 - sizeDelta2.x / 2f, timeScale).SetEase(Ease.Linear);
		yield return new WaitForSeconds(timeScale + UnityEngine.Random.Range(0f, 3f));
		StartCoroutine(doMove());
	}

	private void Update()
	{
	}
}
