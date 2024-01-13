using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogRanking : BaseDialog
{
	public ScrollRect ScrollRect;

	public Transform ScrollViewContent;

	private List<FriendScoreItem> FriendScoreItems = new List<FriendScoreItem>();

	public override void Start()
	{
		base.Start();
		base.BackgroundDialog.SetTitle("Best Dwarfs");
	}

	public void OnShow(UserInfoEntity FriendScores)
	{
		base.OnShow();
		InitData(FriendScores);
	}

	public void InitData(UserInfoEntity FriendScores)
	{
		for (int i = 0; i < FriendScoreItems.Count; i++)
		{
			UnityEngine.Object.Destroy(FriendScoreItems[i].gameObject);
		}
		FriendScoreItems.Clear();
		for (int j = 0; j < FriendScores.Params.Count; j++)
		{
			FriendScores.Params[j].Rank = j + 1;
			CreateManagerItem(FriendScores.Params[j]);
		}
	}

	public void CreateManagerItem(UserInfoEntity.Param FriendScore)
	{
		FriendScoreItem component = InstantiatePrefab("Prefabs/Dialog/Component/FriendScoreItem").GetComponent<FriendScoreItem>();
		component.transform.SetParent(ScrollViewContent, worldPositionStays: false);
		component.initData(FriendScore);
		FriendScoreItems.Add(component);
	}
}
