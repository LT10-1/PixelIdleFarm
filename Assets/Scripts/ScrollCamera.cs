using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class ScrollCamera : MonoBehaviour
{
	public float yLimit = -55f;

	private float yVelocity;

	private List<float> _yVelocity = new List<float>();

	private bool isUp;

	private bool wasRotating;

	private float oldTouchY;

	private float scrollRate = 19.2f;

	private float yScroll;

	public float inertiaDuration = 0.5f;

	public float itemInertiaDuration = 1f;

	private float itemTimeTouchPhaseEnded;

	[HideInInspector]
	public bool IsActive = true;

	private void Start()
	{
	}

	private void LateUpdate()
	{
		if (!IsActive || Utils.IsPointerOverGameObject())
		{
			return;
		}
		Vector3 position = base.gameObject.transform.position;
		yScroll = position.y;
		Vector3 vector = Camera.main.ScreenToViewportPoint(UnityEngine.Input.mousePosition);
		float y = vector.y;
		if (Input.GetButtonDown("Fire1"))
		{
			isUp = false;
			wasRotating = false;
			yVelocity = 0f;
			oldTouchY = y;
		}
		if (Input.GetButtonUp("Fire1"))
		{
			isUp = true;
			if (wasRotating)
			{
				yVelocity = 0f;
				for (int i = 0; i < _yVelocity.Count; i++)
				{
					yVelocity += _yVelocity[i];
				}
				yVelocity /= _yVelocity.Count;
				_yVelocity.Clear();
				itemTimeTouchPhaseEnded = Time.time;
			}
			wasRotating = false;
		}
		if (Input.GetButton("Fire1"))
		{
			if (isUp)
			{
				isUp = false;
				wasRotating = false;
				yVelocity = 0f;
			}
			else
			{
				wasRotating = true;
				float num = y - oldTouchY;
				yScroll -= num * scrollRate;
			}
			_yVelocity.Add((y - oldTouchY) / Time.deltaTime);
			if (_yVelocity.Count > 5)
			{
				_yVelocity.RemoveAt(0);
			}
			oldTouchY = y;
		}
		if (yVelocity != 0f)
		{
			float num2 = (Time.time - itemTimeTouchPhaseEnded) / itemInertiaDuration;
			float num3 = Mathf.Lerp(yVelocity, 0f, num2);
			if (num2 > inertiaDuration)
			{
				yVelocity = 0f;
			}
			yScroll -= num3 * Time.deltaTime * scrollRate;
		}
		yScroll = Mathf.Clamp(yScroll, yLimit, 0f);
		base.gameObject.transform.position = new Vector3(0f, yScroll, 0f);
	}

	public void scrollTo(float y)
	{
		base.gameObject.transform.DOMoveY(y, 0.5f);
	}

	public void moveToBegin()
	{
		scrollTo(0f);
	}

	public void moveToEnd()
	{
		scrollTo(yLimit);
	}
}
