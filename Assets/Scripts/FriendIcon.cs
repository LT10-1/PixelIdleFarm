using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FriendIcon : BaseController
{
	[HideInInspector]
	public Dictionary<int, List<FriendFBIconInMine>> FriendFBIconInMines = new Dictionary<int, List<FriendFBIconInMine>>();

	[HideInInspector]
	public Dictionary<int, List<FriendFBIconInMine>> FriendFBIconInWorldmap = new Dictionary<int, List<FriendFBIconInMine>>();

	public void UpdateUI()
	{
		UpdateUI("Mine");
	}

	public void UpdateUI(string scene)
	{
		if (!(SceneManager.GetActiveScene().name != "Mine") || !(SceneManager.GetActiveScene().name != "WorldMap"))
		{
			CreateFriendIcon(scene);
		}
	}

	public void CreateFriendIcon(string scene)
	{
		if (scene == "Mine")
		{
			foreach (KeyValuePair<int, List<FriendFBIconInMine>> friendFBIconInMine in FriendFBIconInMines)
			{
				foreach (FriendFBIconInMine item in friendFBIconInMine.Value)
				{
					if (item != null)
					{
						UnityEngine.Object.Destroy(item.gameObject);
					}
				}
			}
			FriendFBIconInMines = new Dictionary<int, List<FriendFBIconInMine>>();
		}
		else
		{
			foreach (KeyValuePair<int, List<FriendFBIconInMine>> item2 in FriendFBIconInWorldmap)
			{
				foreach (FriendFBIconInMine item3 in item2.Value)
				{
					if (item3 != null)
					{
						UnityEngine.Object.Destroy(item3.gameObject);
					}
				}
			}
			FriendFBIconInWorldmap = new Dictionary<int, List<FriendFBIconInMine>>();
		}
		UserInfoEntity friendsInfo = BaseController.GameController.FacebookHelper.FriendsInfo;
		for (int i = 0; i < friendsInfo.Params.Count; i++)
		{
			if (scene == "WorldMap")
			{
				AddFriendIconInMap(friendsInfo.Params[i]);
			}
			else if (scene == "Mine" && DataManager.Instance.SavegameData.CurrentMine == BaseController.MineOrder(friendsInfo.Params[i].CurrentContinent, friendsInfo.Params[i].CurrentMine))
			{
				AddFrienIconInMine(friendsInfo.Params[i]);
			}
		}
	}

	public void AddFriendIconInMap(UserInfoEntity.Param info)
	{
		int num = BaseController.MineOrder(info.CurrentContinent, info.CurrentMine);
		if (!FriendFBIconInWorldmap.ContainsKey(num) || FriendFBIconInWorldmap[num].Count <= 5)
		{
			FriendFBIconInMine component = InstantiatePrefab("Prefabs/Mine/FriendFBIconUI").GetComponent<FriendFBIconInMine>();
			component.transform.SetParent(BaseController.GameController.WorldMapController.AreaInMaps[num].FBFriend, worldPositionStays: false);
			component.transform.SetAsLastSibling();
			if (!FriendFBIconInWorldmap.ContainsKey(num))
			{
				FriendFBIconInWorldmap.Add(num, new List<FriendFBIconInMine>());
			}
			int count = FriendFBIconInWorldmap[num].Count;
			component.InitData(info, count);
			Transform transform = component.transform;
			Vector3 localPosition = component.transform.localPosition;
			float x = localPosition.x + (float)(15 * count);
			Vector3 localPosition2 = component.transform.localPosition;
			transform.localPosition = new Vector2(x, localPosition2.y);
			component.transform.localScale = 0.5f * Vector3.one;
			component.transform.SetAsFirstSibling();
			FriendFBIconInWorldmap[num].Add(component);
		}
	}

	public void AddFrienIconInMine(UserInfoEntity.Param info)
	{
		if (!FriendFBIconInMines.ContainsKey(info.ShaftTotalIndex) || FriendFBIconInMines[info.ShaftTotalIndex].Count <= 5)
		{
			FriendFBIconInMine component = InstantiatePrefab("Prefabs/Mine/FriendFBIcon").GetComponent<FriendFBIconInMine>();
			component.transform.SetParent(BaseController.GameController.MineController.FacebookFriends.transform, worldPositionStays: false);
			component.transform.localPosition = -(info.CurrentShaft - 1) * Vector3.up * 3.3f;
			if (!FriendFBIconInMines.ContainsKey(info.ShaftTotalIndex))
			{
				FriendFBIconInMines.Add(info.ShaftTotalIndex, new List<FriendFBIconInMine>());
			}
			int count = FriendFBIconInMines[info.ShaftTotalIndex].Count;
			component.InitData(info, count);
			Transform transform = component.transform;
			Vector3 localPosition = component.transform.localPosition;
			float x = localPosition.x;
			Vector3 size = component.spriteRenderer.bounds.size;
			float x2 = x + size.x / 5f * (float)count;
			Vector3 localPosition2 = component.transform.localPosition;
			transform.localPosition = new Vector2(x2, localPosition2.y);
			component.transform.SetAsFirstSibling();
			FriendFBIconInMines[info.ShaftTotalIndex].Add(component);
		}
	}
}
