using UnityEngine;

public class MoveCamera : MonoBehaviour
{
	public float dragSpeed = 2f;

	private Vector3 dragOrigin;

	[HideInInspector]
	public Vector2 ClampY = new Vector2(-50f, 0f);

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			dragOrigin = UnityEngine.Input.mousePosition;
		}
		else if (Input.GetMouseButton(0))
		{
			Vector3 vector = Camera.main.ScreenToViewportPoint(UnityEngine.Input.mousePosition - dragOrigin);
			Vector3 b = new Vector3(0f, vector.y * dragSpeed, 0f);
			Vector3 position = base.transform.position + b;
			position.y = Mathf.Clamp(position.y, ClampY.x, ClampY.y);
			base.transform.position = position;
		}
	}
}
