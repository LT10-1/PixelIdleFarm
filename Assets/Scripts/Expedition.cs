using Spine.Unity;
using UnityEngine.UI;

public class Expedition : BaseDialog
{
	public Button buttonExit;

	public SkeletonGraphic SkeletonGraphic;

	public BackgroundExpedition BackgroundExpedition;

	public UIButtonController ButtonSelectExpedition;

	public ExpeditionComponent CurrentExpedition;

	public UIButtonController ButtonInfo;

	private string currentSkeletonAnimation;

	protected override float OnShowScaleAmount => 0.01f;

	public ExpeditionState ExpeditionState => BaseController.GameController.ExpeditionController.ExpeditionState;

	public override void Start()
	{
		base.Start();
		buttonExit.onClick.AddListener(OnHide);
		ButtonSelectExpedition.OnClickCallback = OnClickSelectExpedition;
		ButtonInfo.OnClickCallback = OnClickShowInfo;
		BaseController.GameController.ExpeditionController.OnExpeditionStateChange.Add(OnExpeditionStateChange);
		BaseController.GameController.ExpeditionController.OnBoostExpeditionTime.Add(OnBoostExpeditionTime);
	}

	public override void OnShow()
	{
		base.OnShow();
		OnExpeditionStateChange();
	}

	public void OnClickShowInfo()
	{
		BaseController.GameController.DialogController.DialogExpInfo.OnShow();
	}

	public void OnBoostExpeditionTime(long time)
	{
		CurrentExpedition.BoostAnimation(time);
	}

	public void OnExpeditionStateChange()
	{
		CurrentExpedition.gameObject.SetActive(ExpeditionState != ExpeditionState.Idle);
		ButtonSelectExpedition.transform.parent.gameObject.SetActive(!CurrentExpedition.gameObject.activeSelf);
		if (CurrentExpedition.gameObject.activeSelf)
		{
			CurrentExpedition.SetData(BaseController.GameController.ExpeditionController.CurrentExpedition);
		}
		if (ExpeditionState == ExpeditionState.InProgress)
		{
			BackgroundExpedition.StartMoving();
		}
		else
		{
			BackgroundExpedition.StopMoving();
		}
		string b = (ExpeditionState != ExpeditionState.InProgress) ? "idle" : "animation";
		if (currentSkeletonAnimation != b)
		{
			currentSkeletonAnimation = b;
			SkeletonGraphic.AnimationState.SetAnimation(0, currentSkeletonAnimation, loop: true);
		}
	}

	public void OnClickSelectExpedition()
	{
		BaseController.GameController.DialogController.ExpeditionDetail.OnShow();
	}
}
