using UnityEngine;

public class SandController : MonoBehaviour
{
	public SpriteRenderer Cloud1;

	public SpriteRenderer Cloud2;

	private float width;

	private void Start()
	{
		width = Cloud1.sprite.rect.width / CONST.PIXEL_PER_UNIT;
		Transform transform = Cloud1.transform;
		float x = UnityEngine.Random.Range((0f - width) / 2f, 0f);
		Vector3 localPosition = Cloud1.transform.localPosition;
		float y = localPosition.y;
		Vector3 localPosition2 = Cloud1.transform.localPosition;
		transform.localPosition = new Vector3(x, y, localPosition2.z);
		Transform transform2 = Cloud2.transform;
		Vector3 localPosition3 = Cloud1.transform.localPosition;
		float x2 = localPosition3.x + width;
		Vector3 localPosition4 = Cloud2.transform.localPosition;
		float y2 = localPosition4.y;
		Vector3 localPosition5 = Cloud2.transform.localPosition;
		transform2.localPosition = new Vector3(x2, y2, localPosition5.z);
	}

	private void Update()
	{
		float x = 0.8f / CONST.PIXEL_PER_UNIT;
		Cloud1.transform.localPosition -= new Vector3(x, 0f, 0f);
		Cloud2.transform.localPosition -= new Vector3(x, 0f, 0f);
		Vector3 localPosition = Cloud1.transform.localPosition;
		if (localPosition.x < (0f - width) / 2f - CONST.SCREEN_WIDTH / 2f)
		{
			Cloud1.transform.localPosition += new Vector3(width * 2f, 0f);
		}
		Vector3 localPosition2 = Cloud2.transform.localPosition;
		if (localPosition2.x < (0f - width) / 2f - CONST.SCREEN_WIDTH / 2f)
		{
			Cloud2.transform.localPosition += new Vector3(width * 2f, 0f);
		}
	}
}
