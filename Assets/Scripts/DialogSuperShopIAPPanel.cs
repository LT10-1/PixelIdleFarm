using UnityEngine;
using UnityEngine.UI;

public class DialogSuperShopIAPPanel : BaseController
{
	public Button Button => GetComponentInChildren<Button>(includeInactive: true);

	public override void Start()
	{
		base.Start();
		FlashEfect flashEfect = BaseController.GameController.TutorialController.StartFlashEff(Button);
		flashEfect.flashImage.transform.localScale = Vector3.one;
		flashEfect.flashImage.color = new Color(1f, 1f, 1f, 0.8f);
		RectTransform rectTransform = flashEfect.flashImage.rectTransform;
		Vector2 sizeDelta = flashEfect.flashImage.rectTransform.sizeDelta;
		rectTransform.sizeDelta = new Vector2(300f, sizeDelta.y);
	}
}
