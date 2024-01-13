using SimpleJSON;

public static class JSONUtils
{
	public static int[] ToIntArray(JSONNode json)
	{
		JSONArray asArray = json.AsArray;
		int[] array = new int[asArray.Count];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = asArray[i].AsInt;
		}
		return array;
	}

	public static string[] ToStrArray(JSONNode json)
	{
		JSONArray asArray = json.AsArray;
		string[] array = new string[asArray.Count];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = asArray[i];
		}
		return array;
	}
}
