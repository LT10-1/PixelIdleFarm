using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonController : Button
{
	[HideInInspector]
	public Action OnClickSuccess;

	[HideInInspector]
	public Action OnClickCallback;

	private TMP_Text _tmpText;

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

	public new virtual void Awake()
	{
		base.Awake();
		base.onClick.AddListener(delegate
		{
			GameController.Instance.AudioController.PlayOneShot("Audios/Effect/click");
			if (OnClickSuccess != null)
			{
				OnClickSuccess();
			}
			else if (OnClickCallback != null)
			{
				OnClickCallback();
			}
		});
		base.transition = Transition.Animation;
		if (GetComponent<Animator>() == null)
		{
			base.gameObject.AddComponent<Animator>();
			base.gameObject.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animation/ButtonUI");
		}
	}

	public virtual void Update()
	{
	}

	public void SetEnable(bool enable)
	{
		base.interactable = enable;
		SetButtonColorEnable(enable);
	}

	public void SetButtonColorEnable(bool enable)
	{
		GetComponent<Image>().color = ((!enable) ? COLOR.COLOR_BUTTON_DISABLE : Color.white);
	}
}
