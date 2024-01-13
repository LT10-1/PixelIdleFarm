using Newtonsoft.Json;
using SimpleJSON;
using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.Networking;

public class APIHelper : BaseController
{
	public void SubmitCouponCode(string coupon, Action callbackSuccess, Action callbackFailed)
	{
		StartCoroutine(_SubmitCouponCode(coupon, callbackSuccess, callbackFailed));
	}

	private IEnumerator _SubmitCouponCode(string coupon, Action callbackSuccess, Action callbackFailed)
	{
		WWWForm form = new WWWForm();
		form.AddField("data[GiftCode]", coupon);
		UnityWebRequest download = UnityWebRequest.Post("http://api.argoz.net:8010/coupon.php", form);
		yield return download.SendWebRequest();
		if (download.isNetworkError || download.isHttpError)
		{
			MonoBehaviour.print("Error downloading: " + download.error);
			callbackFailed?.Invoke();
			yield break;
		}
		UnityEngine.Debug.Log(download.downloadHandler.text);
		JSONNode jSONNode = JSON.Parse(download.downloadHandler.text);
		if (!jSONNode["success"].AsBool)
		{
			BaseController.GameController.ToastController.StartToast(jSONNode["msg"]);
			callbackFailed?.Invoke();
			yield break;
		}
		int asInt = jSONNode["data"]["itemType"].AsInt;
		ItemsEntity.Param param = new ItemsEntity.Param();
		param.ItemType = asInt;
		ItemsEntity.Param param2 = param;
		switch (asInt)
		{
		case 1:
			BaseController.GameController.MineController.AddItemFromName(jSONNode["data"]["itemValue"]);
			break;
		case 4:
			BaseController.GameController.MineController.AddSkillPointFromName(jSONNode["data"]["itemValue"]);
			break;
		default:
		{
			double asDouble = jSONNode["data"]["itemValue"].AsDouble;
			switch (asInt)
			{
			case 0:
				AddSuperCash(asDouble);
				CreateReceiveEffectSuperCash("Coupon", asDouble);
				break;
			case 3:
				AddCash(asDouble);
				param2.InstantCashAmount = asDouble;
				CreateReceiveEffect("Coupon", param2);
				break;
			}
			break;
		}
		}
		callbackSuccess?.Invoke();
	}

	public void ChargeIAP(string productId, int productValue)
	{
		ChargeIAPEntity.Param param = new ChargeIAPEntity.Param();
		param.UserId = ((BaseController.GameController.FacebookHelper.MyInfo == null) ? string.Empty : BaseController.GameController.FacebookHelper.MyInfo.UserId);
		param.DeviceId = SystemInfo.deviceUniqueIdentifier;
		param.DevicePlatform = CONST.PLATFORM_TYPE.ToString();
		param.PackageName = productId;
		param.PackageValue = productValue;
		ChargeIAPEntity.Param data = param;
		StartCoroutine(_ChargeIAP(data));
	}

	private IEnumerator _ChargeIAP(ChargeIAPEntity.Param data)
	{
		WWWForm form = new WWWForm();
		AddFieldObject(form, data);
		UnityWebRequest download = UnityWebRequest.Post("http://api.argoz.net:8010/chargeIAP.php", form);
		yield return download.SendWebRequest();
		if (download.isNetworkError || download.isHttpError)
		{
			MonoBehaviour.print("Error downloading: " + download.error);
		}
		else
		{
			UnityEngine.Debug.Log(download.downloadHandler.text);
		}
	}

	public void UpdateInfo(UserInfoEntity.Param info)
	{
		StartCoroutine(_UpdateInfo(info));
	}

	private IEnumerator _UpdateInfo(UserInfoEntity.Param info)
	{
		WWWForm form = new WWWForm();
		AddFieldObject(form, info);
		UnityWebRequest download = UnityWebRequest.Post("http://api.argoz.net:8010/update.php", form);
		yield return download.SendWebRequest();
		if (download.isNetworkError || download.isHttpError)
		{
			MonoBehaviour.print("Error downloading: " + download.error);
		}
		else
		{
			UnityEngine.Debug.Log(download.downloadHandler.text);
		}
	}

	public void getFriendInfo(string[] IDUsers, Action<UserInfoEntity> callBack)
	{
		StartCoroutine(_getFriendInfo(IDUsers, callBack));
	}

	private IEnumerator _getFriendInfo(string[] IDUsers, Action<UserInfoEntity> callBack)
	{
		WWWForm form = new WWWForm();
		for (int i = 0; i < IDUsers.Length; i++)
		{
			form.AddField("data[" + i + "]", IDUsers[i]);
		}
		UnityWebRequest download = UnityWebRequest.Post("http://api.argoz.net:8010/getInfo.php", form);
		yield return download.SendWebRequest();
		if (download.isNetworkError || download.isHttpError)
		{
			MonoBehaviour.print("Error downloading: " + download.error);
			yield break;
		}
		UnityEngine.Debug.Log(download.downloadHandler.text);
		JSONNode jSONNode = JSON.Parse(download.downloadHandler.text);
		jSONNode["Params"] = jSONNode["data"];
		UserInfoEntity userInfoEntity = JsonConvert.DeserializeObject<UserInfoEntity>(jSONNode.ToString());
		if (userInfoEntity != null)
		{
			callBack(userInfoEntity);
		}
	}

	public void AddFieldObject(WWWForm form, object obj)
	{
		FieldInfo[] fields = obj.GetType().GetFields();
		foreach (FieldInfo fieldInfo in fields)
		{
			object value = fieldInfo.GetValue(obj);
			if (value is int)
			{
				form.AddField("data[" + fieldInfo.Name + "]", (int)value);
			}
			else if (value is bool)
			{
				form.AddField("data[" + fieldInfo.Name + "]", ((bool)value) ? 1 : 0);
			}
			else
			{
				form.AddField("data[" + fieldInfo.Name + "]", (value == null) ? string.Empty : ((string)value));
			}
		}
	}
}
