using System;
using UnityEngine;

namespace Assets.SimpleAndroidNotifications
{
	public static class NotificationManager
	{
		private const string FullClassName = "com.hippogames.simpleandroidnotifications.Controller";

		private const string MainActivityClassName = "com.unity3d.player.UnityPlayerActivity";

		public static int Send(TimeSpan delay, string title, string message, Color smallIconColor, NotificationIcon smallIcon = NotificationIcon.Bell)
		{
			NotificationParams notificationParams = new NotificationParams();
			notificationParams.Id = UnityEngine.Random.Range(0, int.MaxValue);
			notificationParams.Delay = delay;
			notificationParams.Title = title;
			notificationParams.Message = message;
			notificationParams.Ticker = message;
			notificationParams.Sound = true;
			notificationParams.Vibrate = true;
			notificationParams.Light = true;
			notificationParams.SmallIcon = smallIcon;
			notificationParams.SmallIconColor = smallIconColor;
			notificationParams.LargeIcon = string.Empty;
			return SendCustom(notificationParams);
		}

		public static int SendWithAppIcon(TimeSpan delay, string title, string message, Color smallIconColor, NotificationIcon smallIcon = NotificationIcon.Bell)
		{
			NotificationParams notificationParams = new NotificationParams();
			notificationParams.Id = UnityEngine.Random.Range(0, int.MaxValue);
			notificationParams.Delay = delay;
			notificationParams.Title = title;
			notificationParams.Message = message;
			notificationParams.Ticker = message;
			notificationParams.Sound = true;
			notificationParams.Vibrate = true;
			notificationParams.Light = true;
			notificationParams.SmallIcon = smallIcon;
			notificationParams.SmallIconColor = smallIconColor;
			notificationParams.LargeIcon = "app_icon";
			return SendCustom(notificationParams);
		}

		public static int SendCustom(NotificationParams notificationParams)
		{
			long num = (long)notificationParams.Delay.TotalMilliseconds;
			new AndroidJavaClass("com.hippogames.simpleandroidnotifications.Controller").CallStatic("SetNotification", notificationParams.Id, num, notificationParams.Title, notificationParams.Message, notificationParams.Ticker, notificationParams.Sound ? 1 : 0, notificationParams.Vibrate ? 1 : 0, notificationParams.Light ? 1 : 0, notificationParams.LargeIcon, GetSmallIconName(notificationParams.SmallIcon), ColotToInt(notificationParams.SmallIconColor), "com.unity3d.player.UnityPlayerActivity");
			return notificationParams.Id;
		}

		public static void Cancel(int id)
		{
			new AndroidJavaClass("com.hippogames.simpleandroidnotifications.Controller").CallStatic("CancelScheduledNotification", id);
		}

		public static void CancelAll()
		{
			new AndroidJavaClass("com.hippogames.simpleandroidnotifications.Controller").CallStatic("CancelAllScheduledNotifications");
		}

		private static int ColotToInt(Color color)
		{
			Color32 color2 = color;
			return color2.r * 65536 + color2.g * 256 + color2.b;
		}

		private static string GetSmallIconName(NotificationIcon icon)
		{
			return "anp_" + icon.ToString().ToLower();
		}
	}
}
