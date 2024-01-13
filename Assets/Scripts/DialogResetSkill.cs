using System;
using TMPro;
using UnityEngine;

public class DialogResetSkill : BaseDialog
{
	public TMP_Text CurrentSuperCash;

	public TMP_Text ContentText;

	public UIButtonController ButtonConfirm;

	[HideInInspector]
	public Action OnConfirm;

	public override void Start()
	{
		base.Start();
		base.BackgroundDialog.SetTitle("Reset Skill");
		ContentText.text = string.Format("Reset Skill Tree for {0} and regain all spent Skill Points?", "<size=35>" + DATA_RESOURCES.TEXT_SPRITE.SUPER_CASH + " " + 400.0 + "</size>");
		ButtonConfirm.OnClickCallback = OnClickConfirm;
		BaseController.GameController.OnMineSuperCashChangeCallback.Add(OnSuperCashChange);
		OnSuperCashChange();
	}

	public void OnClickConfirm()
	{
		UseSuperCash(400.0, SpendSuperCashType.None, delegate
		{
			OnHide();
			if (OnConfirm != null)
			{
				OnConfirm();
			}
			BaseController.GameController.ToastController.StartToast("Skill Tree have been reset!");
		});
	}

	public void OnSuperCashChange()
	{
		CurrentSuperCash.text = DataManager.Instance.SuperCash.MinifyFormat();
	}
}
