using Assets.SimpleAndroidNotifications;
using System;
using UnityEngine;

public class LocalNotificationController : BaseController
{
	public override void Start()
	{
		base.Start();
	}

	public void RegisterForNotifications()
	{
	}

	public void ScheduleBreakRock(long startTime, float duration)
	{
		ScheduleLocalNotification(LocalNotificationType.BreakTierRockTimeout, new DateTime(startTime + TimeSpan.FromSeconds(duration).Ticks));
	}

	public void ScheduleLocalNotification(LocalNotificationType type, DateTime notificationDateTime, string param = null)
	{
		if (DataManager.Instance.SettingData.Notification)
		{
			string title = DATA_TEXT.NOTIFICATION_TITLE[(int)type];
			string message = DATA_TEXT.NOTIFICATION_CONTENT[(int)type];
			if (param != null)
			{
				message = string.Format(DATA_TEXT.NOTIFICATION_CONTENT[(int)type], param);
			}
			CancelNotification(type);
			NotificationIcon smallIcon = NotificationIcon.Star;
			if (type == LocalNotificationType.BreakTierRockTimeout)
			{
				smallIcon = NotificationIcon.Clock;
			}
			DataManager.Instance.SavegameData.LocalNotificationDictionary[(int)type] = NotificationManager.SendWithAppIcon(notificationDateTime - DateTime.Now, title, message, new Color(0f, 0.6f, 1f), smallIcon);
		}
	}

	public void CancelNotification(LocalNotificationType type)
	{
		if (DataManager.Instance.SavegameData.LocalNotificationDictionary.ContainsKey((int)type))
		{
			long num = DataManager.Instance.SavegameData.LocalNotificationDictionary[(int)type];
			DataManager.Instance.SavegameData.LocalNotificationDictionary.Remove((int)type);
			NotificationManager.Cancel((int)num);
		}
	}

	public void CancelAllNotification()
	{
		NotificationManager.CancelAll();
	}
}
