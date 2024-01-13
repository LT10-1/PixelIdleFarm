using UnityEngine;

public class ProgressBarController : MonoBehaviour
{
	public SpriteRenderer progressBarRender;

	private Vector2 originalSize = Vector2.zero;

	private bool isRunning;

	private float startTime;

	private float duration;

	private void Start()
	{
	}

	private void Update()
	{
		if (isRunning)
		{
			float num = Time.realtimeSinceStartup - startTime;
			if (num <= duration)
			{
				SetPercent(num / duration);
			}
			else
			{
				setRunning(running: false);
			}
		}
	}

	public void Run(float duration)
	{
		if (!(duration < 0.5f))
		{
			setRunning(running: true);
			startTime = Time.realtimeSinceStartup;
			this.duration = duration;
			SetPercent(0f);
		}
	}

	private void setRunning(bool running)
	{
		isRunning = running;
		base.gameObject.SetActive(running);
	}

	private void SetPercent(float percent)
	{
		if (originalSize == Vector2.zero)
		{
			originalSize = progressBarRender.size;
		}
		progressBarRender.size = Vector2.Scale(originalSize, new Vector2(percent, 1f));
	}
}
