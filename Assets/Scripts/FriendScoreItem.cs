using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FriendScoreItem : MonoBehaviour
{
	public Image Avatar;

	public TMP_Text TextRank;

	public Text TextName;

	public TMP_Text TextScore;

	public Image IsMe;

	private UserInfoEntity.Param FriendScore;

	public void initData(UserInfoEntity.Param _FriendScore)
	{
		FriendScore = _FriendScore;
		TextRank.text = FriendScore.Rank + ".";
		TextName.text = FriendScore.Name;
		TextScore.text = FriendScore.Score + string.Empty;
		IsMe.gameObject.SetActive(FriendScore.IsMe);
		Coroutiner.StartCoroutine(loadImage(FriendScore.UrlAvatar));
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnEnable()
	{
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
