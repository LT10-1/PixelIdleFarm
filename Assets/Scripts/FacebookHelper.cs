using Facebook.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FacebookHelper : BaseController
{
	public List<object> Friends;

	[HideInInspector]
	public UserInfoEntity FriendsInfo;

	[HideInInspector]
	public UserInfoEntity.Param MyInfo;

	private static readonly List<string> readPermissions = new List<string>
	{
		"public_profile",
		"user_friends"
	};

	public FriendIcon FriendIcon;

	public static bool HavePublishActions
	{
		get
		{
			return (IsLoggedIn() && (AccessToken.CurrentAccessToken.Permissions as List<string>).Contains("publish_actions")) ? true : false;
		}
		private set
		{
		}
	}

	public override void Awake()
	{
		FriendIcon = GetComponent<FriendIcon>();
		FriendsInfo = new UserInfoEntity();
		MyInfo = new UserInfoEntity.Param();
		if (!FB.IsInitialized)
		{
			FB.Init(delegate
			{
				FB.ActivateApp();
				initSuccess();
			});
			return;
		}
		FB.ActivateApp();
		initSuccess();
	}

	private void initSuccess()
	{
		if (IsLoggedIn())
		{
			FetchFacebookFriendInfo(FriendIcon.UpdateUI);
		}
	}

	public static string getMyAvatar()
	{
		return getAvatarUrl(getMyID());
	}

	public static string getMyID()
	{
		try
		{
			return AccessToken.CurrentAccessToken.UserId;
		}
		catch
		{
		}
		return null;
	}

	public static bool IsLoggedIn()
	{
		return FB.IsLoggedIn;
	}

	private void LoginWithCallBack(FacebookDelegate<ILoginResult> callBack)
	{
		//BaseController.GameController.AnalyticController.LogEvent("login_facebook");
		if (!FB.IsInitialized)
		{
			FB.Init(delegate
			{
				FB.LogInWithReadPermissions(readPermissions, callBack);
			}, OnHideUnity);
		}
		else
		{
			FB.LogInWithReadPermissions(readPermissions, callBack);
		}
	}

	private static string getAvatarUrl(string userID)
	{
		return "https://graph.facebook.com/" + userID + "/picture?type=normal";
	}

	private void OnHideUnity(bool isGameShown)
	{
	}

	public void InviteFriend()
	{
		FB.Mobile.AppInvite(new Uri("https://play.google.com/store/apps/details?id=com.argozgame.idledwarfs"));
	}

	private void AppInviteCallback(IAppInviteResult IAppInviteResult)
	{
	}

	public void UpdateMyInfo(int Score, int CurrentContinent, int CurrentMine, int CurrentShaft)
	{
		if (!string.IsNullOrEmpty(MyInfo.UserId))
		{
			MyInfo.Score = Score;
			MyInfo.CurrentContinent = CurrentContinent;
			MyInfo.CurrentMine = CurrentMine;
			MyInfo.CurrentShaft = CurrentShaft;
			BaseController.GameController.APIHelper.UpdateInfo(MyInfo);
		}
	}

	public void getFriendInfo(Action callBack)
	{
		string[] array = new string[FriendsInfo.Params.Count];
		for (int i = 0; i < FriendsInfo.Params.Count; i++)
		{
			array[i] = FriendsInfo.Params[i].UserId;
		}
		BaseController.GameController.APIHelper.getFriendInfo(array, delegate(UserInfoEntity _FriendsInfo)
		{
			FriendsInfo = _FriendsInfo;
			FriendsInfo.Sort();
			callBack();
		});
	}

	public void FetchFacebookFriendInfo(Action callBack)
	{
		if (IsLoggedIn())
		{
			FetchMyInfo(callBack);
		}
		else
		{
			LoginWithCallBack(delegate
			{
				FetchMyInfo(callBack);
			});
		}
	}

	private void FetchMyInfo(Action callBack)
	{
		FB.API("/me?fields=name", HttpMethod.GET, delegate(IGraphResult result)
		{
			if (result.Error == null)
			{
				FetchFriendInfo(callBack);
				MyInfo.UserId = getMyID();
				MyInfo.UrlAvatar = getMyAvatar();
				MyInfo.IsMe = true;
				MyInfo.Name = result.ResultDictionary["name"].ToString();
				BaseController.GameController.UpdateMyInfo();
			}
		});
	}

	private void FetchFriendInfo(Action callBack)
	{
		string query = "/me/friends?fields=id,name,picture.width(128).height(128)&limit=100";
		FB.API(query, HttpMethod.GET, delegate(IGraphResult result)
		{
			if (result.Error == null)
			{
				if (result.ResultDictionary.TryGetValue("data", out object value))
				{
					List<object> newFriends = (List<object>)value;
					CacheFriends(newFriends);
				}
				FriendsInfo.Params.Clear();
				foreach (object friend in Friends)
				{
					Dictionary<string, object> dictionary = (Dictionary<string, object>)friend;
					MonoBehaviour.print(dictionary.ToString());
					UserInfoEntity.Param param = new UserInfoEntity.Param();
					param.UserId = (string)dictionary["id"];
					param.Name = (string)dictionary["name"];
					param.UrlAvatar = getAvatarUrl(param.UserId);
					param.IsMe = false;
					FriendsInfo.Params.Add(param);
				}
				getFriendInfo(callBack);
			}
		});
	}

	private void CacheFriends(List<object> newFriends)
	{
		Friends = newFriends;
	}

	public void OnGetFriendsScore()
	{
		if (IsLoggedIn())
		{
			HandleFriendLeaderBoard();
		}
		else
		{
			FetchFacebookFriendInfo(HandleFriendLeaderBoard);
		}
	}

	public void HandleFriendLeaderBoard()
	{
		UserInfoEntity userInfoEntity = new UserInfoEntity();
		userInfoEntity.Params.Add(MyInfo);
		userInfoEntity.Params.AddRange(FriendsInfo.Params);
		userInfoEntity.Sort();
		BaseController.GameController.DialogController.DialogRanking.OnShow(userInfoEntity);
		FriendIcon.UpdateUI();
	}

	public void OnGetFriendPlayWithMe()
	{
		if (IsLoggedIn())
		{
			HandleFriendPlayWithMe();
		}
		else
		{
			FetchFacebookFriendInfo(HandleFriendPlayWithMe);
		}
	}

	public void HandleFriendPlayWithMe()
	{
		BaseController.GameController.DialogController.DialogRanking.InitData(FriendsInfo);
		BaseController.GameController.DialogController.DialogFriend.InitData(FriendsInfo);
		FriendIcon.UpdateUI();
	}
}
