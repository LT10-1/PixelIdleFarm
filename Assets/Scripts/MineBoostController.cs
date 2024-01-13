using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MineBoostController : BaseMineController
{
	public GameObject BoostPanel;

	public TMP_Text IdleMultipleText;

	public TMP_Text CashMultipleText;

	public TMP_Text IdleDurationText;

	public TMP_Text CashDurationText;

	public override void Start()
	{
		base.Start();
		base.MineController.TopMenu.GetComponent<Button>().onClick.AddListener(OnClickTopMenu);
	}

	private void OnClickTopMenu()
	{
		if (BaseController.GameController.BoostController.HaveBoost)
		{
			BaseController.GameController.DialogController.DialogBoostOverview.OnShow();
		}
	}

	public override void Update()
	{
		base.Update();
		if (BaseController.GameController == null)
		{
			return;
		}
		if (!BaseController.GameController.BoostController.HaveBoost)
		{
			BoostPanel.SetActive(value: false);
			base.MineController.ButtonBoost.text = "Boost";
			return;
		}
		BoostPanel.SetActive(value: true);
		IdleMultipleText.text = "x" + BaseController.GameController.BoostController.TotalBoostFactor.MinifyIncomeFactor();
		CashMultipleText.text = "x" + BaseController.GameController.BoostController.TotalBoostFactor.MinifyIncomeFactor();
		IdleDurationText.text = ((long)TimeSpan.FromTicks(BaseController.GameController.BoostController.MinRemainTime).TotalSeconds).FormatTimeString();
		if (IdleDurationText.text == "0s")
		{
			IdleDurationText.text = DATA_RESOURCES.TEXT_SPRITE.INFINITY;
		}
		CashDurationText.text = IdleDurationText.text;
		if (BaseController.GameController.BoostController.CurrentAdRemainTime > 0)
		{
			base.MineController.ButtonBoost.text = ((long)TimeSpan.FromTicks(BaseController.GameController.BoostController.CurrentAdRemainTime).TotalSeconds).FormatTimeString();
		}
		else
		{
			base.MineController.ButtonBoost.text = "Boost";
		}
	}
}
