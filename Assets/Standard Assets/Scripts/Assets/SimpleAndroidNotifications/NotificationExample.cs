using System;
using UnityEngine;

namespace Assets.SimpleAndroidNotifications
{
	public class NotificationExample : MonoBehaviour
	{
		public void Rate()
		{
			Application.OpenURL("http://u3d.as/y6r");
		}

		public void OpenWiki()
		{
			Application.OpenURL("https://github.com/hippogamesunity/SimpleAndroidNotificationsPublic/wiki");
		}

		public void ScheduleSimple()
		{
			NotificationManager.Send(TimeSpan.FromSeconds(5.0), "Simple notification", "Customize icon and color", new Color(1f, 0.3f, 0.15f));
		}

		public void ScheduleNormal()
		{
			NotificationManager.SendWithAppIcon(TimeSpan.FromSeconds(5.0), "Notification", "Notification with app icon", new Color(0f, 0.6f, 1f), NotificationIcon.Message);
		}

		public void ScheduleCustom()
		{
			NotificationParams notificationParams = new NotificationParams();
			notificationParams.Id = UnityEngine.Random.Range(0, int.MaxValue);
			notificationParams.Delay = TimeSpan.FromSeconds(5.0);
			notificationParams.Title = "Custom notification";
			notificationParams.Message = "Message";
			notificationParams.Ticker = "Ticker";
			notificationParams.Sound = true;
			notificationParams.Vibrate = true;
			notificationParams.Light = true;
			notificationParams.SmallIcon = NotificationIcon.Heart;
			notificationParams.SmallIconColor = new Color(0f, 0.5f, 0f);
			notificationParams.LargeIcon = "app_icon";
			NotificationParams notificationParams2 = notificationParams;
			NotificationManager.SendCustom(notificationParams2);
		}

		public void CancelAll()
		{
			NotificationManager.CancelAll();
		}
	}
}
