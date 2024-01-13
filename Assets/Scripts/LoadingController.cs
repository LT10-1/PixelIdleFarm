using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingController : BaseController
{
	public TMP_Text LoadingText;

	public Image LoadingImage;

	private string[] dotStr = new string[3]
	{
		".",
		"..",
		"..."
	};

	public override void Update()
	{
		base.Update();
		LoadingImage.transform.localEulerAngles -= Vector3.forward * 5f;
		int num = Mathf.FloorToInt(Time.realtimeSinceStartup * 2f) % 3;
		LoadingText.text = "Loading " + dotStr[num];
	}

	public void ToggleLoading(bool loading)
	{
		base.gameObject.SetActive(loading);
	}
}
