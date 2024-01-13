using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogFriendItem : MonoBehaviour
{
	public Image Avatar;

	public Text TextName;

	public TMP_Text TextPercent;

	public TMP_Text TextArea;

	public TMP_Text TextShaft;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void initData(UserInfoEntity.Param param)
	{
		TextName.text = param.Name;
		TextShaft.text = "Mine Shaft " + param.CurrentShaft;
		TextArea.text = DATA_RESOURCES.TEXT_SPRITE.RESOURCE[param.CurrentContinent][param.CurrentMine] + " " + DATA_TEXT.MINES.CONTINENT_MINES[param.CurrentContinent][param.CurrentMine] + " Mine";
		Coroutiner.StartCoroutine(loadImage(param.UrlAvatar));
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
