using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundDialog : MonoBehaviour
{
	public Button BackgroundBlur;

	public Transform BackgroundGroup;

	public Image Background;

	public Image BackgroundTitle1;

	public Image BackgroundTitle2;

	public TMP_Text Title1;

	public TMP_Text Title2;

	public Button ButtonClose;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void SetTitle(string text, bool isType1 = true)
	{
		bool flag = isType1;
		flag = true;
		BackgroundTitle1.gameObject.SetActive(flag);
		BackgroundTitle2.gameObject.SetActive(!flag);
		if (flag)
		{
			Title1.text = text;
		}
		else
		{
			Title2.text = text;
		}
	}
}
