using System;
using TMPro;
using UnityEngine;

public class DialogExpeditionBoost : BaseDialog
{
	public TMP_Text TextContent;

	public UIButtonController ButtonCollect;

	public UIButtonController ButtonCollect2;

	public TMP_Text NextBoostTimeText;

	public override void Start()
	{
		base.Start();
		base.BackgroundDialog.SetTitle("Boost Expedition");
		ButtonCollect.OnClickCallback = OnCollect;
		ButtonCollect2.OnClickCallback = OnCollect2;
		ButtonCollect.text = "-" + 900L.FormatTimeString();
		ButtonCollect2.text = "-" + 1800L.FormatTimeString();
		CheckBoostAvailable();
	}

	public override void Update()
	{
		base.Update();
		CheckBoostAvailable();
		if (!Input.GetKeyUp(KeyCode.Return))
		{
		}
	}

	public void CheckBoostAvailable()
	{
		bool expeditionBoostAvailable = BaseController.GameController.ExpeditionController.ExpeditionBoostAvailable;
		ButtonCollect.gameObject.SetActive(expeditionBoostAvailable);
		ButtonCollect2.gameObject.SetActive(expeditionBoostAvailable);
		NextBoostTimeText.transform.parent.gameObject.SetActive(!expeditionBoostAvailable);
		if (!expeditionBoostAvailable)
		{
			NextBoostTimeText.text = DATA_RESOURCES.TEXT_SPRITE.CLOCK + " " + ((144000000000L - (DateTime.Now.Ticks - DataManager.Instance.SavegameData.ExpeditionLastBoostTime)) / 10000000).FormatTimeString(getFull: true);
		}
	}

	private void OnCollect()
	{
		OnHide();
		BaseController.GameController.ExpeditionController.BoostExpeditionTime(9000000000L);
		//BaseController.GameController.AnalyticController.LogEvent("expedition_boost", "boostType", "normal");
	}

	private void OnCollect2()
	{
		OnHide();
		BaseController.GameController.ExpeditionController.BoostExpeditionTime(18000000000L);
		//BaseController.GameController.AnalyticController.LogEvent("watch_ad", "type", "boost_expedition");
		//BaseController.GameController.AnalyticController.LogEvent("expedition_boost", "boostType", "withAd");
	}
}
