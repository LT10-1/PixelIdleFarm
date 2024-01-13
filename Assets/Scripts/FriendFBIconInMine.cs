using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FriendFBIconInMine : MonoBehaviour
{
	public SpriteRenderer spriteRenderer;

	public Image image;

	[HideInInspector]
	public UserInfoEntity.Param info;

	[HideInInspector]
	public float _Wavatar;

	[HideInInspector]
	public Vector3 originalScale;

	private void Start()
	{
		if (spriteRenderer != null)
		{
			Vector3 size = spriteRenderer.bounds.size;
			_Wavatar = size.x;
		}
		else if (image != null)
		{
			_Wavatar = image.minWidth;
		}
		originalScale = base.transform.localScale;
		base.transform.localScale = Vector3.zero;
	}

	private void Update()
	{
	}

	public void InitData(UserInfoEntity.Param _info, int index)
	{
		info = _info;
		Coroutiner.StartCoroutine(loadImage(_info.UrlAvatar));
	}

	private IEnumerator loadImage(string url)
	{
		yield return null;
		WWW www = new WWW(url);
		yield return www;
		if (www.texture != null)
		{
			Sprite sprite = Sprite.Create(www.texture, new Rect(0f, 0f, www.texture.width, www.texture.height), new Vector2(0.5f, 0.5f));
			base.transform.localScale = originalScale;
			if (spriteRenderer != null)
			{
				spriteRenderer.sprite = sprite;
				Transform transform = spriteRenderer.transform;
				float wavatar = _Wavatar;
				Vector3 size = spriteRenderer.bounds.size;
				float x = wavatar / size.x;
				float wavatar2 = _Wavatar;
				Vector3 size2 = spriteRenderer.bounds.size;
				transform.localScale = new Vector2(x, wavatar2 / size2.x);
			}
			else if (image != null)
			{
				image.sprite = sprite;
			}
		}
	}
}
