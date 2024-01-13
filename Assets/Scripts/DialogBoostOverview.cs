using System;
using System.Collections.Generic;
using TMPro;

public class DialogBoostOverview : BaseDialog
{
	public TMP_Text LeftText;

	public TMP_Text MiddleText;

	public TMP_Text RightText;

	public override void Start()
	{
		base.Start();
		base.BackgroundDialog.SetTitle("Boost Overview");
	}

	public override void Update()
	{
		base.Update();
		LeftText.text = string.Empty;
		MiddleText.text = string.Empty;
		RightText.text = string.Empty;
		if (!BaseController.GameController.BoostController.HaveBoost)
		{
			return;
		}
		int num = BaseController.GameController.BoostController.BoostItemRemainTime.Count;
		if (BaseController.GameController.BoostController.CurrentAdRemainTime > 0)
		{
			num++;
		}
		if (MathUtils.CompareDoubleBiggerThanZero(BaseController.GameController.BoostController.CurrentIAPItemBoostFactor - 1.0))
		{
			num++;
		}
		foreach (KeyValuePair<double, long> item in BaseController.GameController.BoostController.BoostItemRemainTime)
		{
			TMP_Text middleText = MiddleText;
			middleText.text = middleText.text + item.Key + "x Income Boost\n";
			TMP_Text rightText = RightText;
			rightText.text = rightText.text + ((long)TimeSpan.FromTicks(item.Value).TotalSeconds).FormatTimeString() + "\n";
			if (BaseController.GameController.BoostController.BoostItemRemainTime.Count >= 2)
			{
				LeftText.text += "+";
			}
			LeftText.text += "\n";
		}
		if (BaseController.GameController.BoostController.BoostItemRemainTime.Count >= 2)
		{
			LeftText.text += "\n";
			MiddleText.text += "\n";
			RightText.text += "\n";
		}
		if (MathUtils.CompareDoubleBiggerThanZero(BaseController.GameController.BoostController.CurrentIAPItemBoostFactor - 1.0))
		{
			MiddleText.text += "2x Infinity Boost\n";
			TMP_Text rightText2 = RightText;
			rightText2.text = rightText2.text + DATA_RESOURCES.TEXT_SPRITE.INFINITY + "\n";
			if (num > 1)
			{
				LeftText.text += "x";
			}
			LeftText.text += "\n";
		}
		if (BaseController.GameController.BoostController.CurrentAdRemainTime > 0)
		{
			TMP_Text middleText2 = MiddleText;
			middleText2.text = middleText2.text + BaseController.GameController.SkillController.AdCurrentx2BoostFactor.MinifyIncomeFactor() + "x Ad Watched\n";
			TMP_Text rightText3 = RightText;
			rightText3.text = rightText3.text + ((long)TimeSpan.FromTicks(BaseController.GameController.BoostController.CurrentAdRemainTime).TotalSeconds).FormatTimeString() + "\n";
			if (num > 1)
			{
				LeftText.text += "x";
			}
			LeftText.text += "\n";
		}
		if (num > 1)
		{
			LeftText.text += "=\n";
			TMP_Text middleText3 = MiddleText;
			middleText3.text = middleText3.text + BaseController.GameController.BoostController.TotalBoostFactor.MinifyIncomeFactor() + "x Income";
		}
	}
}
