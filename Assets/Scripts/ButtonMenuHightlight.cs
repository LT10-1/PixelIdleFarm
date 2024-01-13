using UnityEngine;

public class ButtonMenuHightlight : UIButtonController
{
	public Transform Background => base.transform.Find("Background");

	public override void Update()
	{
		base.Update();
		if (Background != null)
		{
			Background.eulerAngles += Vector3.forward * 2f;
		}
	}
}
