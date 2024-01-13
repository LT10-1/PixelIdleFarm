using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

public class SpriteButtonController : MonoBehaviour
{
	private TMP_Text _tmpText;

	[HideInInspector]
	public Action OnClickSuccess;

	[HideInInspector]
	public Action OnClickCallback;

	[HideInInspector]
	public bool enableButton = true;

	[HideInInspector]
	private Tweener bounceTweener;

	private bool isExit;

	public TMP_Text TmpText => _tmpText ?? (_tmpText = GetComponentInChildren<TMP_Text>(includeInactive: true));

	public string text
	{
		get
		{
			return TmpText.text;
		}
		set
		{
			TmpText.text = value;
		}
	}

	public bool CheckClickEvenSystem => Utils.IsPointerOverGameObject();

	public void SetButtonEnable(bool enable)
	{
		enableButton = enable;
		if (!enable)
		{
			StopAllTweens();
			base.transform.localScale = Vector3.one;
		}
	}

	private void OnMouseDown()
	{
		if (!CheckClickEvenSystem && enableButton)
		{
			isExit = false;
			GameController.Instance.AudioController.PlayOneShot("Audios/Effect/click");
			StopAllTweens();
			base.transform.localScale = Vector3.one;
			bounceTweener = base.transform.DOScale(1.1f * Vector3.one, 0.2f);
		}
	}

	private void OnMouseUp()
	{
		if (!CheckClickEvenSystem && !isExit && enableButton)
		{
			StopAllTweens();
			bounceTweener = base.transform.DOScale(Vector3.one, 0.2f);
			if (OnClickSuccess != null)
			{
				OnClickSuccess();
			}
			else if (OnClickCallback != null)
			{
				OnClickCallback();
			}
		}
	}

	private void OnMouseExit()
	{
		isExit = true;
		StopAllTweens();
		bounceTweener = base.transform.DOScale(Vector3.one, 0.2f);
	}

	private void StopAllTweens()
	{
		if (bounceTweener != null)
		{
			bounceTweener.Kill();
		}
	}
}
