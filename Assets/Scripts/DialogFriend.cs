using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogFriend : BaseDialog
{
	public ScrollRect ScrollRect;

	public Transform ScrollViewContent;

	private List<DialogFriendItem> FriendItems = new List<DialogFriendItem>();

	public GameObject goInvite;

	public GameObject goLogin;

	public TMP_Text TextNum;

	public TMP_Text TextIncome;

	public Image Avatar;

	public UIButtonController buttonInvite;

	public UIButtonController buttonLogin;

	public override void Start()
	{
		base.Start();
		base.BackgroundDialog.SetTitle("Friends");
		buttonInvite.onClick.AddListener(ClickInvite);
		buttonLogin.onClick.AddListener(ClickLogin);
	}

	private void ClickInvite()
	{
		BaseController.GameController.FacebookHelper.InviteFriend();
	}

	private void ClickLogin()
	{
		BaseController.GameController.FacebookHelper.OnGetFriendPlayWithMe();
	}

	private void OnEnable()
	{
	}

	public override void OnShow()
	{
		base.OnShow();
		if (!FacebookHelper.IsLoggedIn())
		{
			setBoost(0);
			goInvite.SetActive(value: false);
			goLogin.SetActive(value: true);
			return;
		}
		goInvite.SetActive(value: true);
		goLogin.SetActive(value: false);
		if (FriendItems.Count == 0)
		{
			if (BaseController.GameController.FacebookHelper.FriendsInfo.Params.Count != 0)
			{
				InitData(BaseController.GameController.FacebookHelper.FriendsInfo);
			}
			else
			{
				BaseController.GameController.FacebookHelper.OnGetFriendPlayWithMe();
			}
		}
	}

	public void setBoost(int numFriend)
	{
		TextNum.SetText(numFriend + string.Empty);
		TextIncome.SetText((double)numFriend * 5.0 + "%");
	}

	public void InitData(UserInfoEntity Friends)
	{
		goInvite.SetActive(value: true);
		goLogin.SetActive(value: false);
		setBoost(Friends.Params.Count);
		Coroutiner.StartCoroutine(loadImage(FacebookHelper.getMyAvatar()));
		for (int i = 0; i < FriendItems.Count; i++)
		{
			UnityEngine.Object.Destroy(FriendItems[i].gameObject);
		}
		FriendItems.Clear();
		for (int j = 0; j < Friends.Params.Count; j++)
		{
			Friends.Params[j].Rank = j + 1;
			CreateManagerItem(Friends.Params[j]);
		}
	}

	public void CreateManagerItem(UserInfoEntity.Param FriendScore)
	{
		DialogFriendItem component = InstantiatePrefab("Prefabs/Dialog/Component/DialogFriendItem").GetComponent<DialogFriendItem>();
		component.transform.SetParent(ScrollViewContent, worldPositionStays: false);
		component.initData(FriendScore);
		FriendItems.Add(component);
	}

	private IEnumerator loadImage(string url)
	{
		yield return null;
		WWW www = new WWW(url);
		yield return www;
		if (www.texture != null)
		{
			Avatar.overrideSprite = Sprite.Create(www.texture, new Rect(0f, 0f, www.texture.width, www.texture.height), new Vector2(0f, 0f));
		}
	}
}
