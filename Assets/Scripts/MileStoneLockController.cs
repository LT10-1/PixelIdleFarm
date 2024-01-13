using System;
using TMPro;
using UnityEngine;

public class MileStoneLockController : BaseMineController
{
	public enum UnlockState
	{
		Init,
		WatingForUnlock,
		UnlockFinished
	}

	public GameObject InitGroup;

	public GameObject WaitGroup;

	public TMP_Text CostText;

	public TMP_Text TimeText;

	public TMP_Text TimeRunText;

	public TMP_Text UnlockedText;

	public CoinController UnlockCost;

	public CoinController SuperCashWaitCost;

	public SpriteButtonController ButtonBreak;

	public SpriteButtonController ButtonWatchAd;

	public SpriteButtonController ButtonUseSuperCash;

	public SpriteRenderer TimeBarFill;

	private float originalTimeBarWidth;

	private float initDuration;

	private float duration;

	private double initCost;

	public AdButtonController AdButtonController => ButtonWatchAd.GetComponent<AdButtonController>();

	public UnlockState State
	{
		get
		{
			return (UnlockState)DataManager.Instance.CurrentMineSavegame.MileStoneUnlockState;
		}
		set
		{
			DataManager.Instance.CurrentMineSavegame.MileStoneUnlockState = (int)value;
		}
	}

	private long MileStoneStartTime
	{
		get
		{
			return DataManager.Instance.CurrentMineSavegame.MileStoneStartTime;
		}
		set
		{
			DataManager.Instance.CurrentMineSavegame.MileStoneStartTime = value;
		}
	}

	public int CurrentAdCount
	{
		get
		{
			return DataManager.Instance.CurrentMineSavegame.AdMileStoneCount;
		}
		set
		{
			DataManager.Instance.CurrentMineSavegame.AdMileStoneCount = value;
			AdButtonController.CurrentAdCount = value;
		}
	}

	public long CurrentAdEndTime
	{
		get
		{
			return DataManager.Instance.CurrentMineSavegame.AdMileStoneEndTime;
		}
		set
		{
			DataManager.Instance.CurrentMineSavegame.AdMileStoneEndTime = value;
			AdButtonController.CurrentAdEndTime = value;
		}
	}

	public override void Awake()
	{
		base.Awake();
		AdButtonController.MaxAdAvailable = 2;
		AdButtonController.AdCooldownTime = 3600L;
		CurrentAdCount = DataManager.Instance.CurrentMineSavegame.AdMileStoneCount;
		CurrentAdEndTime = DataManager.Instance.CurrentMineSavegame.AdMileStoneEndTime;
		AdButtonController.OnAdChange = delegate
		{
			CurrentAdCount = AdButtonController.CurrentAdCount;
			CurrentAdEndTime = AdButtonController.CurrentAdEndTime;
		};
	}

	public override void Start()
	{
		base.Start();
		ButtonBreak.OnClickCallback = OnClickBreak;
		ButtonWatchAd.OnClickCallback = OnClickWatchAd;
		ButtonUseSuperCash.OnClickCallback = OnClickUseSuperCash;
		BaseController.GameController.OnMineCashChangeCallback.Add(CheckUpgradable);
		BaseController.GameController.OnMineSuperCashChangeCallback.Add(CheckSuperUpgradable);
		UnlockCost.SetCoinType(base.CurrentCoinType);
	}

	public override void Update()
	{
		base.Update();
		if (MileStoneStartTime == 0)
		{
			return;
		}
		float num = duration - (float)TimeSpan.FromTicks(DateTime.Now.Ticks - MileStoneStartTime).TotalSeconds;
		if (num > 0f)
		{
			TimeRunText.text = ((long)num).FormatTimeString(getFull: true);
			float num2 = num / duration;
			if (originalTimeBarWidth == 0f)
			{
				originalTimeBarWidth = (float)TimeBarFill.sprite.texture.width / CONST.PIXEL_PER_UNIT;
			}
			SpriteRenderer timeBarFill = TimeBarFill;
			float x = num2 * originalTimeBarWidth;
			Vector2 size = TimeBarFill.size;
			timeBarFill.size = new Vector2(x, size.y);
			Transform transform = TimeBarFill.transform;
			Vector2 size2 = TimeBarFill.size;
			transform.localPosition = (size2.x - originalTimeBarWidth) / 2f * Vector3.right;
			float num3 = Mathf.Clamp((float)Math.Floor(0.09 * Math.Pow(num, 0.79000002145767212)), 10f, 10000f);
			SuperCashWaitCost.SetMoney(num3, minify: true, showMoney: true, string.Empty);
			CheckSuperUpgradable();
		}
		else
		{
			UnlockFinished();
		}
	}

	private void CheckSuperUpgradable()
	{
		if (State == UnlockState.WatingForUnlock)
		{
			SuperCashWaitCost.SetMoneyColor(DataManager.Instance.SuperCash >= SuperCashWaitCost.Cash);
		}
	}

	private void CheckUpgradable()
	{
		if (State == UnlockState.Init)
		{
			bool flag = DataManager.Instance.Cash >= UnlockCost.Cash;
			Utils.SetColorEnable(CostText, flag);
			UnlockCost.SetMoneyColor(flag);
		}
	}

	private void OnClickUseSuperCash()
	{
		UseSuperCash(SuperCashWaitCost.Cash, SpendSuperCashType.UnlockTierRock, UnlockFinished);
	}

	private void OnClickWatchAd()
	{
		//BaseController.GameController.AnalyticController.LogEvent("watch_ad", "type", "break_rock");
		MileStoneStartTime -= 18000000000L;
		BaseController.GameController.LocalNotificationController.ScheduleBreakRock(MileStoneStartTime, duration);
	}

	private void LoadMileStoneData()
	{
		switch (State)
		{
		case UnlockState.WatingForUnlock:
			StartWaitUnlock(isLoad: true);
			break;
		case UnlockState.UnlockFinished:
			UnlockFinished();
			break;
		}
	}

	private void OnClickBreak()
	{
		switch (State)
		{
		case UnlockState.Init:
			if (UseCash(UnlockCost.Cash))
			{
				StartWaitUnlock();
			}
			break;
		case UnlockState.WatingForUnlock:
			BaseController.GameController.ToastController.StartToast("Waiting for unlock");
			break;
		case UnlockState.UnlockFinished:
			base.gameObject.SetActive(value: false);
			State = UnlockState.Init;
			BaseController.GameController.AudioController.PlayOneShot("Audios/Effect/phada");
			base.MineController.CorridorController.UnlockCompleteAnimation();
			break;
		}
	}

	public void ShowLock(int durationHour, double cost)
	{
		initDuration = durationHour * 3600;
		initCost = cost;
		UpdateData();
	}

	public void UpdateData()
	{
		UnlockState state = State;
		Init();
		duration = initDuration * (float)(1.0 - BaseController.GameController.SkillController.RockUnlockDurationReduction / 100.0);
		SetTimeText(TimeText, duration);
		UnlockCost.SetMoney(initCost * (1.0 - BaseController.GameController.SkillController.RockUnlockCostReduction / 100.0), minify: true, showMoney: true, string.Empty);
		CheckUpgradable();
		State = state;
		LoadMileStoneData();
	}

	public void StartWaitUnlock(bool isLoad = false)
	{
		State = UnlockState.WatingForUnlock;
		if (!isLoad)
		{
			MileStoneStartTime = DateTime.Now.Ticks;
		}
		ShowInitGroup(show: false);
		BaseController.GameController.LocalNotificationController.ScheduleBreakRock(MileStoneStartTime, duration);
	}

	public void UnlockFinished()
	{
		State = UnlockState.UnlockFinished;
		MileStoneStartTime = 0L;
		CurrentAdCount = 0;
		CurrentAdEndTime = 0L;
		SetLock(isLock: false);
		ShowInitGroup(show: true);
		BaseController.GameController.LocalNotificationController.CancelNotification(LocalNotificationType.BreakTierRockTimeout);
	}

	private void Init()
	{
		State = UnlockState.Init;
		base.gameObject.SetActive(value: true);
		CostText.SetText("Cost:");
		SetLock(isLock: true);
		ShowInitGroup(show: true);
	}

	private void ShowInitGroup(bool show)
	{
		InitGroup.SetActive(show);
		WaitGroup.SetActive(!show);
	}

	private void SetTimeText(TMP_Text textMeshPro, float duration)
	{
		textMeshPro.text = $"Time: {((long)duration).FormatTimeString()}";
	}

	private void SetLock(bool isLock)
	{
		CostText.gameObject.SetActive(isLock);
		TimeText.gameObject.SetActive(isLock);
		UnlockCost.gameObject.SetActive(isLock);
		UnlockedText.gameObject.SetActive(!isLock);
	}
}
