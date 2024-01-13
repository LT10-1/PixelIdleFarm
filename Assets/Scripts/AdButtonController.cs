using System;
using UnityEngine;
using UnityEngine.UI;

public class AdButtonController : BaseController
{
	[HideInInspector]
	public Action OnAdChange;

	[HideInInspector]
	public Transform PlayIcon;

	[HideInInspector]
	public Transform PlayIconNoVideoAvailable;

	[HideInInspector]
	public int MaxAdAvailable;

	[HideInInspector]
	public long AdCooldownTime;

	[HideInInspector]
	public long CurrentAdEndTime;

	[HideInInspector]
	public int CurrentAdCount;

	private string _originalText;

	[HideInInspector]
	public bool IsUiButton;

	[HideInInspector]
	public SpriteButtonController SpriteButtonController;

	[HideInInspector]
	public UIButtonController UiButtonController;

	private bool isAdReady = true;

	public string OriginalText
	{
		get
		{
			return _originalText ?? (_originalText = text);
		}
		set
		{
			_originalText = value;
		}
	}

	public string text
	{
		get
		{
			return (!IsUiButton) ? SpriteButtonController.text : UiButtonController.text;
		}
		set
		{
			if (IsUiButton)
			{
				UiButtonController.text = value;
			}
			else
			{
				SpriteButtonController.text = value;
			}
		}
	}

	public override void Awake()
	{
		base.Awake();
		SpriteButtonController = GetComponent<SpriteButtonController>();
		UiButtonController = GetComponent<UIButtonController>();
		PlayIcon = base.transform.Find("PlayIcon");
		PlayIconNoVideoAvailable = base.transform.Find("PlayIconNoVideoAvailable");
		if (PlayIconNoVideoAvailable != null)
		{
			PlayIconNoVideoAvailable.gameObject.SetActive(value: false);
		}
		IsUiButton = (UiButtonController != null);
		_originalText = text;
		if (IsUiButton)
		{
			UiButtonController.OnClickSuccess = delegate
			{
				OnClickShowAdButton(UiButtonController.OnClickCallback);
			};
		}
		else
		{
			SpriteButtonController.OnClickSuccess = delegate
			{
				OnClickShowAdButton(SpriteButtonController.OnClickCallback);
			};
		}
	}

	public void OnClickShowAdButton(Action callbackVoid)
	{
		if (BaseController.GameController.AdsManager.IsReady())
		{
			BaseController.GameController.AdsManager.ShowAd(delegate
			{
				CurrentAdCount++;
				if (CurrentAdCount >= MaxAdAvailable)
				{
					CurrentAdCount = 0;
					CurrentAdEndTime = DateTime.Now.Ticks + AdCooldownTime * 10000000;
				}
				if (OnAdChange != null)
				{
					OnAdChange();
				}
				if (callbackVoid != null)
				{
					callbackVoid();
				}
				BaseController.GameController.ChestController.GenerateWatchAdChest();
			});
		}
	}

	public override void Start()
	{
		base.Start();
	}

	public override void Update()
	{
		base.Update();
		if (isAdReady != BaseController.GameController.AdsManager.IsReady())
		{
			isAdReady = BaseController.GameController.AdsManager.IsReady();
			text = ((!isAdReady) ? "No video available" : OriginalText);
			SetButtonEnable(isAdReady);
		}
		if (isAdReady)
		{
			if (MaxAdAvailable != 0 && CurrentAdEndTime != 0)
			{
				long num = (long)TimeSpan.FromTicks(CurrentAdEndTime - DateTime.Now.Ticks).TotalSeconds;
				if (num > 0)
				{
					SetButtonEnable(enable: false);
					text = "Next video:\n" + num.FormatTimeString();
				}
				else
				{
					CurrentAdEndTime = 0L;
					CurrentAdCount = 0;
					if (OnAdChange != null)
					{
						OnAdChange();
					}
					text = OriginalText;
					SetButtonEnable(enable: true);
				}
			}
			else
			{
				text = OriginalText;
				SetButtonEnable(enable: true);
			}
		}
		if (PlayIconNoVideoAvailable != null && PlayIconNoVideoAvailable.gameObject.activeSelf)
		{
			PlayIconNoVideoAvailable.eulerAngles -= Vector3.forward * 8f;
		}
	}

	public void SetButtonEnable(bool enable)
	{
		if (PlayIcon != null && PlayIconNoVideoAvailable != null)
		{
			PlayIcon.gameObject.SetActive(enable);
			PlayIconNoVideoAvailable.gameObject.SetActive(!enable);
		}
		if (IsUiButton)
		{
			UiButtonController.enabled = enable;
			UiButtonController.GetComponent<Image>().color = ((!enable) ? new Color(0.75f, 0.75f, 0.75f, 0.75f) : Color.white);
		}
		else
		{
			SpriteButtonController.SetButtonEnable(enable);
			SpriteButtonController.GetComponent<SpriteRenderer>().color = ((!enable) ? new Color(0.75f, 0.75f, 0.75f, 0.75f) : Color.white);
		}
	}
}
