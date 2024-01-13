using UnityEngine;

public class CloudController : MonoBehaviour
{
	private float width;

	private void Start()
	{
		width = GetComponent<SpriteRenderer>().sprite.rect.width / CONST.PIXEL_PER_UNIT;
		Transform transform = base.transform;
		float x = (0f - width) / 2f + width * (float)UnityEngine.Random.Range(0, 2);
		Vector3 localPosition = base.transform.localPosition;
		float y = localPosition.y;
		Vector3 localPosition2 = base.transform.localPosition;
		transform.localPosition = new Vector3(x, y, localPosition2.z);
	}

	private void Update()
	{
		base.transform.localPosition -= new Vector3(1f / CONST.PIXEL_PER_UNIT, 0f, 0f);
		Vector3 localPosition = base.transform.localPosition;
		if (localPosition.x < (0f - width) / 2f - CONST.SCREEN_WIDTH / 2f)
		{
			base.transform.localPosition += new Vector3(width * UnityEngine.Random.Range(3f, 4f), 0f, 0f);
		}
	}
}
