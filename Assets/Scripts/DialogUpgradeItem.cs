using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogUpgradeItem : BaseController
{
	public Image ParamIcon;

	public TMP_Text ParamDescription;

	public ValueGroupController ValueGroupController;

	[HideInInspector]
	public UpgradeType UpgradeType;

	[HideInInspector]
	public DialogUpgrade DialogUpgrade;

	public void SetUpgradeType(UpgradeType upgradeType)
	{
		UpgradeType = upgradeType;
		ParamIcon.sprite = BaseController.LoadSprite(DATA_RESOURCES.IMAGE.DIALOG_UPGRADE_ICON[(int)UpgradeType]);
		ParamDescription.text = DATA_TEXT.DIALOG_UPGRADE_PARAM[(int)UpgradeType];
	}
}
