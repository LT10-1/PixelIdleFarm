using System;
using System.Collections.Generic;
using UnityEngine;

public class GraphUtil : ScriptableObject
{
	public static string GetPictureQuery(string facebookID, int? width = default(int?), int? height = default(int?), string type = null, bool onlyURL = false)
	{
		string text = $"/{facebookID}/picture";
		string str = (!width.HasValue) ? string.Empty : ("&width=" + width.ToString());
		str += ((!height.HasValue) ? string.Empty : ("&height=" + height.ToString()));
		str += ((type == null) ? string.Empty : ("&type=" + type));
		if (onlyURL)
		{
			str += "&redirect=false";
		}
		if (str != string.Empty)
		{
			text = text + "?g" + str;
		}
		return text;
	}

	public static string DeserializePictureURL(object userObject)
	{
		Dictionary<string, object> dictionary = userObject as Dictionary<string, object>;
		if (dictionary.TryGetValue("picture", out object value))
		{
			Dictionary<string, object> dictionary2 = (Dictionary<string, object>)((Dictionary<string, object>)value)["data"];
			return (string)dictionary2["url"];
		}
		return null;
	}

	public static int GetScoreFromEntry(object obj)
	{
		Dictionary<string, object> dictionary = (Dictionary<string, object>)obj;
		return Convert.ToInt32(dictionary["score"]);
	}
}
