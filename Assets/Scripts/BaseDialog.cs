using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BaseDialog : BaseController
{
	[HideInInspector]
	public DialogController DialogController;

	private BackgroundDialog _backgroundDialog;

	private ContentDialog _dialogGroup;

	private Tweener backgroundTweener;

	private Tweener contentTweener;

	private Tweener backgroundBlurTweener;

	protected virtual float BackgroundScale => 1f;

	protected virtual float OnShowScaleAmount => 0.05f;

	protected BackgroundDialog BackgroundDialog => _backgroundDialog ?? (_backgroundDialog = GetComponentInChildren<BackgroundDialog>(includeInactive: true));

	protected ContentDialog ContentDialog => _dialogGroup ?? (_dialogGroup = GetComponentInChildren<ContentDialog>(includeInactive: true));

	public bool isShowing
	{
		get
		{
			return base.gameObject.activeSelf;
		}
		set
		{
			base.gameObject.SetActive(value);
		}
	}

	public override void Awake()
	{
		if (BackgroundDialog != null)
		{
			BackgroundDialog.ButtonClose.onClick.AddListener(OnHide);
			BackgroundDialog.BackgroundBlur.onClick.AddListener(OnHide);
		}
	}

	public override void Start()
	{
	}

	public override void Update()
	{
	}

	public virtual void OnHide()
	{
		isShowing = false;
		DialogController.OnDialogHide(this);
	}

	private void OnEnable()
	{
		base.transform.SetAsLastSibling();
	}

	public virtual void OnShow()
	{
		isShowing = true;
		DialogController.OnDialogShow(this);
		BaseController.GameController.AudioController.PlayOneShot("Audios/Effect/xuathienpopup");
		StopAllTweens();
		if (BackgroundDialog != null)
		{
			BackgroundDialog.BackgroundGroup.localScale = (1f - OnShowScaleAmount) * BackgroundScale * Vector3.one;
			backgroundTweener = BackgroundDialog.BackgroundGroup.DOScale((1f + OnShowScaleAmount) * BackgroundScale * Vector3.one, 0.2f).OnComplete(delegate
			{
				backgroundTweener = BackgroundDialog.BackgroundGroup.DOScale(BackgroundScale * Vector3.one, 0.2f);
			});
			backgroundBlurTweener = BackgroundDialog.BackgroundBlur.gameObject.GetComponent<Image>().DOColor(new Color(0f, 0f, 0f, 0.3f), 0.4f);
		}
		ContentDialog.transform.localScale = (1f - OnShowScaleAmount) * Vector3.one;
		contentTweener = ContentDialog.transform.DOScale((1f + OnShowScaleAmount) * Vector3.one, 0.2f).OnComplete(delegate
		{
			contentTweener = ContentDialog.transform.DOScale(Vector3.one, 0.2f);
		});
	}

	public void StopAllTweens()
	{
		if (backgroundTweener != null)
		{
			backgroundTweener.Kill();
		}
		if (contentTweener != null)
		{
			contentTweener.Kill();
		}
		if (backgroundBlurTweener != null)
		{
			backgroundBlurTweener.Kill();
		}
		if (BackgroundDialog != null)
		{
			BackgroundDialog.BackgroundBlur.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
			BackgroundDialog.BackgroundGroup.localScale = BackgroundScale * Vector3.one;
		}
		ContentDialog.transform.localScale = Vector3.one;
	}
}
