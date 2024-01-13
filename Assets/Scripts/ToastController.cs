using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class ToastController : MonoBehaviour
{
	public TMP_Text ToastText;

	private Tweener TweenerToast;

	private float tweenTime = 0.5f;

	private float tweenDuration = 4f;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void StartToast(string text)
	{
		StopAllCoroutines();
		if (TweenerToast != null)
		{
			TweenerToast.Kill();
		}
		ToastText.text = text;
		base.transform.localScale = Vector3.Scale(base.transform.localScale, Vector3.right);
		base.gameObject.SetActive(value: true);
		GameController.Instance.AudioController.PlayOneShot("Audios/Effect/alert");
		StartCoroutine(StartToastCoroutine());
	}

	private IEnumerator StartToastCoroutine()
	{
		TweenerToast = base.transform.DOScaleY(1f, tweenTime);
		yield return new WaitForSeconds(tweenDuration - tweenTime * 2f);
		TweenerToast = base.transform.DOScaleY(0f, tweenTime);
	}
}
