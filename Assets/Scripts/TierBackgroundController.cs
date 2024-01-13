using System.Collections.Generic;
using UnityEngine;

public class TierBackgroundController : MonoBehaviour
{
	public List<SpriteRenderer> SpriteRenderers;

	private void Start()
	{
	}

	public void SetShaftSprite(Sprite sprite)
	{
		SetShaftSprite(sprite, Color.white);
	}

	public void SetShaftSprite(Sprite sprite, Color color)
	{
		for (int i = 0; i < SpriteRenderers.Count; i++)
		{
			SpriteRenderers[i].sprite = sprite;
			SpriteRenderers[i].color = color;
		}
	}
}
