using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Utils
{
	public static bool IsPointerOverGameObject()
	{
		bool result = false;
		if (EventSystem.current != null && IsPointerOverUIObject())
		{
			result = true;
		}
		return result;
	}

	private static bool IsPointerOverUIObject()
	{
		PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
		PointerEventData pointerEventData2 = pointerEventData;
		Vector3 mousePosition = UnityEngine.Input.mousePosition;
		float x = mousePosition.x;
		Vector3 mousePosition2 = UnityEngine.Input.mousePosition;
		pointerEventData2.position = new Vector2(x, mousePosition2.y);
		List<RaycastResult> list = new List<RaycastResult>();
		EventSystem.current.RaycastAll(pointerEventData, list);
		return list.Count > 0;
	}

	public static Color GetColorFromHex(string hex)
	{
		Color color;
		return (!ColorUtility.TryParseHtmlString(hex, out color)) ? Color.white : color;
	}

	public static void SetColorEnable(TMP_Text text, bool enable)
	{
		text.color = ((!enable) ? Color.red : Color.white);
	}

	public static void SetColorBonus(TMP_Text text, bool bonus)
	{
		text.color = ((!bonus) ? Color.white : Color.yellow);
	}
}
