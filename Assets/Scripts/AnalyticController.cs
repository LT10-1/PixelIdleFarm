using Facebook.Unity;
//using Firebase.Analytics;
using System.Collections.Generic;

public class AnalyticController : BaseController
{
	public override void Start()
	{
		base.Start();
		//FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLogin);
	}

	public void LogEvent(string eventName)
	{
		//FirebaseAnalytics.LogEvent(eventName);
		FBLogEvent(eventName);
	}

	public void LogEventPurchase(string productID, int value)
	{
		//FirebaseAnalytics.LogEvent("charge_IAP", new Parameter("value", value), new Parameter("productID", productID));
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary["productID"] = productID;
		FB.LogPurchase(value, "USD", dictionary);
	}

	public void LogEvent(string eventName, string paramName, int paramValue)
	{
		//FirebaseAnalytics.LogEvent(eventName, paramName, paramValue);
		FBLogEvent(eventName, paramName, paramValue);
	}

	public void LogEvent(string eventName, string paramName, double paramValue)
	{
		//FirebaseAnalytics.LogEvent(eventName, paramName, paramValue);
		FBLogEvent(eventName, paramName, paramValue);
	}

	public void LogEvent(string eventName, string paramName, string paramValue)
	{
		//FirebaseAnalytics.LogEvent(eventName, paramName, paramValue);
		FBLogEvent(eventName, paramName, paramValue);
	}

	public void FBLogEvent(string eventName, string paramName = null, object paramValue = null)
	{
		Dictionary<string, object> dictionary = null;
		if (paramName != null && paramValue != null)
		{
			dictionary = new Dictionary<string, object>();
			dictionary[paramName] = paramValue;
		}
		FB.LogAppEvent(eventName, null, dictionary);
	}
}
