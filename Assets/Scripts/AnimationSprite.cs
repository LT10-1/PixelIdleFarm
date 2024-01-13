using System.Collections;
using UnityEngine;

public class AnimationSprite : MonoBehaviour
{
	public bool RunAnimation;

	private SpriteRenderer _spriteRenderer;

	private Sprite[] Sprites;

	private int currentFrame;

	public SpriteRenderer SpriteRenderer => _spriteRenderer ?? (_spriteRenderer = GetComponent<SpriteRenderer>());

	public void SetAnimation(string resourceURL)
	{
		Sprites = Resources.LoadAll<Sprite>(resourceURL);
		currentFrame = 0;
		StopAllCoroutines();
		if (RunAnimation)
		{
			StartCoroutine(runAnimation());
		}
	}

	public void SetFrame(int frame)
	{
		if (Sprites != null)
		{
			SpriteRenderer.sprite = Sprites[frame];
		}
	}

	private IEnumerator runAnimation()
	{
		yield return new WaitForSeconds(0.2f);
		currentFrame = (currentFrame + 1) % Sprites.Length;
		SetFrame(currentFrame);
		StartCoroutine(runAnimation());
	}
}
