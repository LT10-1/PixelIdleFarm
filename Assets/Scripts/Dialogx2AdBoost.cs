using System;
using TMPro;
using UnityEngine.UI;

public class Dialogx2AdBoost : BaseDialog
{
	public UIButtonController ButtonWatchAd;

	public Image ImageFillBar;

	public Image ImageFillBarAlpha;

	public TMP_Text TextRemainTime;

	public TMP_Text MineTypeIcon;

	public TMP_Text TextContent;

	public long MineBoostx2EndTime => DataManager.Instance.CurrentMineSavegame.MineBoostx2EndTime;

	public override void Start()
	{
		base.Start();
		base.BackgroundDialog.SetTitle("x2 Ad Boost");
		ButtonWatchAd.OnClickCallback = OnClickWatchAd;
		Init();
	}

	public override void OnShow()
	{
		base.OnShow();
		MineTypeIcon.text = base.CURRENT_RESOURCE_SPRITE;
		TextContent.text = string.Format("x{1} income in the {0} for an addition {2}!", DATA_TEXT.MINES.CONTINENT_MINES[DataManager.Instance.SavegameData.CurrentContinent][DataManager.Instance.SavegameData.CurrentMineIndex] + " Mine", BaseController.GameController.SkillController.AdCurrentx2BoostFactor.MinifyIncomeFactor(), BaseController.GameController.BoostController.AdBoostx2SingleDuration.FormatTimeString());
	}

	public override void Update()
	{
		base.Update();
		if (MineBoostx2EndTime != 0)
		{
			long num = MineBoostx2EndTime - DateTime.Now.Ticks;
			if (num > 0)
			{
				TextRemainTime.gameObject.SetActive(value: true);
				TextRemainTime.text = TimeSpan.FromTicks(num).TotalSeconds.FormatTimeString(getFull: true);
				ImageFillBar.fillAmount = (float)num / (float)BaseController.GameController.BoostController.AdBoostx2MaxDuration / 1E+07f;
				ImageFillBarAlpha.fillAmount = ((float)num + (float)BaseController.GameController.BoostController.AdBoostx2SingleDuration * 1E+07f) / (float)BaseController.GameController.BoostController.AdBoostx2MaxDuration / 1E+07f;
			}
			else
			{
				Init();
			}
		}
	}

	public void Init()
	{
		ImageFillBar.fillAmount = 0f;
		ImageFillBarAlpha.fillAmount = (float)BaseController.GameController.BoostController.AdBoostx2SingleDuration / (float)BaseController.GameController.BoostController.AdBoostx2MaxDuration;
		TextRemainTime.gameObject.SetActive(value: false);
	}

	public void OnClickWatchAd()
	{
		//BaseController.GameController.AnalyticController.LogEvent("watch_ad", "type", "boost_x2_mine");
		BaseController.GameController.BoostController.MineAdBoostx2(DataManager.Instance.CurrentMineSavegame.MineOrder);
	}
}
