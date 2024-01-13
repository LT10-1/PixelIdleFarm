using TMPro;
using UnityEngine.UI;

public class DialogDailyRewardItem : BaseController
{
	public Image Background;

	public Image BackgroundBlur;

	public CoinController RewardCoin;

	public TMP_Text TextCurrentDay;

	public Image IconReward;

	public Image IconComplete;

	public bool IsBigReward;

	public void SetTypeBoostMultiple(ItemBoostMultiple multiple, ItemBoostDuration duration)
	{
		IconReward.sprite = DataUtils.GetItemBoostImage(multiple, duration);
		IconReward.SetNativeSize();
		RewardCoin.SetCoinType(CoinType.Multi);
		RewardCoin.text = (double)multiple + "x";
	}

	public void SetComplete(bool complete)
	{
		IconComplete.gameObject.SetActive(complete);
		BackgroundBlur.gameObject.SetActive(complete);
	}

	public void SetActiveDay(bool active)
	{
		if (IsBigReward)
		{
			Background.sprite = BaseController.LoadSprite((!active) ? "Images/UI/Reward/frame_weekend" : "Images/UI/Reward/frame_weekend_active");
		}
		else
		{
			Background.sprite = BaseController.LoadSprite((!active) ? "Images/UI/Reward/frame" : "Images/UI/Reward/frame_active");
		}
	}
}
