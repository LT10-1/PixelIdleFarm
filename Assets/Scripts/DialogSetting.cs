using System;
using System.Collections;
using UnityEngine;

public class DialogSetting : BaseDialog
{
	public DialogSettingItem ItemFB;

	public DialogSettingItem ItemPage;

	public DialogSettingItem ItemSound;

	public DialogSettingItem ItemMusic;

	public DialogSettingItem ItemNotification;

	public DialogSettingItem ItemLanguage;

	public DialogSettingItem ItemStat;

	public DialogSettingItem ItemHelp;

	public override void Start()
	{
		base.Start();
		base.BackgroundDialog.SetTitle("Settings");
		DataManager.Instance.OnSettingChangeCallback.Add(UpdateFromSetting);
		UpdateFromSetting();
		ItemSound.ButtonMain.onClick.AddListener(OnClickSound);
		ItemMusic.ButtonMain.onClick.AddListener(OnClickMusic);
		ItemFB.ButtonMain.onClick.AddListener(OnClickFBHighscore);
		ItemPage.ButtonMain.onClick.AddListener(OnClickFBPage);
		ItemNotification.ButtonMain.onClick.AddListener(OnClickNotification);
		ItemStat.ButtonMain.onClick.AddListener(OnClickStat);
		ItemHelp.ButtonMain.onClick.AddListener(OnClickHelp);
		ItemPage.TextContent.text = "Like us and get 200 " + DATA_RESOURCES.TEXT_SPRITE.SUPER_CASH;
	}

	public void OpenFBPage()
	{
		try
		{
			Application.OpenURL("fb://profile/320733985106721");
		}
		catch (Exception ex)
		{
			Application.OpenURL("http://facebook.com/320733985106721");
			MonoBehaviour.print(ex.StackTrace);
		}
	}

	public void OnClickFBPage()
	{
		//BaseController.GameController.AnalyticController.LogEvent("like_fanpage_facebook");
		OpenFBPage();
		if (!DataManager.Instance.SettingData.HasLikeFanpage)
		{
			StartCoroutine(AddCashOnLike());
		}
	}

	private IEnumerator AddCashOnLike()
	{
		yield return new WaitForSeconds(1f);
		DataManager.Instance.SettingData.HasLikeFanpage = true;
		double addCash = 200.0;
		AddSuperCash(addCash);
		CreateReceiveEffectSuperCash("Like Fanpage", addCash);
	}

	public void OnClickFBHighscore()
	{
		BaseController.GameController.FacebookHelper.OnGetFriendsScore();
	}

	public void OnClickStat()
	{
		BaseController.GameController.DialogController.DialogSettingStatistic.OnShow();
		BaseController.GameController.MineController.ButtonSuperShop.gameObject.SetActive(value: true);
	}

	public void OnClickHelp()
	{
		Application.OpenURL("http://idledwarf.com/faq.html");
	}

	public void OnClickNotification()
	{
		DataManager.Instance.SettingData.Notification = !DataManager.Instance.SettingData.Notification;
		if (!DataManager.Instance.SettingData.Notification)
		{
			BaseController.GameController.LocalNotificationController.CancelAllNotification();
		}
	}

	public void OnClickSound()
	{
		DataManager.Instance.SettingData.Sound = !DataManager.Instance.SettingData.Sound;
		if (!DataManager.Instance.SettingData.Sound)
		{
			//BaseController.GameController.AnalyticController.LogEvent("turn_off_sound");
		}
	}

	public void OnClickMusic()
	{
		DataManager.Instance.SettingData.Music = !DataManager.Instance.SettingData.Music;
		if (!DataManager.Instance.SettingData.Music)
		{
			//BaseController.GameController.AnalyticController.LogEvent("turn_off_music");
		}
	}

	public void UpdateFromSetting()
	{
		ItemSound.ButtonMain.image.sprite = BaseController.LoadSprite((!DataManager.Instance.SettingData.Sound) ? "Images/UI/Settings/popup_settings_bar_off" : "Images/UI/Settings/popup_settings_bar");
		ItemMusic.ButtonMain.image.sprite = BaseController.LoadSprite((!DataManager.Instance.SettingData.Music) ? "Images/UI/Settings/popup_settings_bar_off" : "Images/UI/Settings/popup_settings_bar");
		ItemNotification.ButtonMain.image.sprite = BaseController.LoadSprite((!DataManager.Instance.SettingData.Notification) ? "Images/UI/Settings/popup_settings_bar_off" : "Images/UI/Settings/popup_settings_bar");
		ItemSound.ButtonMain.text = ((!DataManager.Instance.SettingData.Sound) ? "Off" : "On");
		ItemMusic.ButtonMain.text = ((!DataManager.Instance.SettingData.Music) ? "Off" : "On");
		ItemPage.ButtonMain.text = ((!DataManager.Instance.SettingData.HasLikeFanpage) ? "Like" : "Open");
		ItemNotification.ButtonMain.text = ((!DataManager.Instance.SettingData.Notification) ? "Off" : "On");
	}
}
