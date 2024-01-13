using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadController : MonoBehaviour
{
	public Image ProgressImage;

	public TMP_Text TipText;

	private void Start()
	{
		RectTransform rectTransform = ProgressImage.rectTransform;
		Vector2 sizeDelta = ProgressImage.rectTransform.sizeDelta;
		rectTransform.DOSizeDelta(new Vector2(430f, sizeDelta.y), 1f);
		TipText.text = DATA_TEXT.TIPS_TEXT[Random.Range(0, DATA_TEXT.TIPS_TEXT.Length)];
		StartCoroutine(StartMine());
	}

	private IEnumerator StartMine()
	{
		yield return new WaitForSeconds(1f);
		SceneManager.LoadScene("Mine");
	}
}
