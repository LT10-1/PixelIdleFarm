using UnityEngine;

public class Fading : MonoBehaviour
{
	public Texture2D fadeOutTexture;

	public float fadeSpeed = 0.5f;

	private int drawDepth = -1000;

	private float alpha = 1f;

	private int fadeDir = -1;

	private void Awake()
	{
	}

	private void OnGUI()
	{
		alpha += (float)fadeDir * fadeSpeed * Time.deltaTime;
		alpha = Mathf.Clamp01(alpha);
		Color color = GUI.color;
		float r = color.r;
		Color color2 = GUI.color;
		float g = color2.g;
		Color color3 = GUI.color;
		GUI.color = new Color(r, g, color3.b, alpha);
		GUI.depth = drawDepth;
		GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), fadeOutTexture);
	}

	public float BeginFade(int direction)
	{
		fadeDir = direction;
		return fadeSpeed;
	}
}
