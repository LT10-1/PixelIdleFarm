using TMPro;
using UnityEngine;

public class SkillPointDetail : BaseController
{
	public RectTransform RectTransform;

	public SkillPoint CurrentSkillPoint;

	public Transform SkillPointPosition;

	public Transform SkillPointPositionNoLevel;

	public TMP_Text TextDetail;

	public UIButtonController ButtonUpgrade;

	private SkillPoint baseSkillPoint;

	public void SetData(SkillPoint skillPoint)
	{
		baseSkillPoint = skillPoint;
		TextDetail.text = baseSkillPoint.DetailTextWithNext;
		CurrentSkillPoint.icon.sprite = baseSkillPoint.icon.sprite;
		CurrentSkillPoint.Level.transform.parent.gameObject.SetActive(baseSkillPoint.IsActive);
		CurrentSkillPoint.setLevel(baseSkillPoint.CurrentLevel);
		CurrentSkillPoint.transform.localPosition = ((!baseSkillPoint.IsActive) ? SkillPointPositionNoLevel.localPosition : SkillPointPosition.localPosition);
		ButtonUpgrade.gameObject.SetActive(!baseSkillPoint.IsMaxLevel && (baseSkillPoint.IsActive || baseSkillPoint.IsUnlockable));
		CheckButtonUpgrade();
	}

	public void CheckButtonUpgrade()
	{
		if (!(baseSkillPoint == null) && ButtonUpgrade.gameObject.activeSelf)
		{
			ButtonUpgrade.text = ((!baseSkillPoint.IsActive) ? "Unlock" : "Level Up");
			bool flag = CheckEnoughSkillPoint(baseSkillPoint.CurrentCost, (ContinentType)baseSkillPoint.SkillParam.SkillPathID);
			ButtonUpgrade.SetButtonColorEnable(flag);
			if (!flag)
			{
				ButtonUpgrade.text += "<color=red>";
			}
			UIButtonController buttonUpgrade = ButtonUpgrade;
			string text = buttonUpgrade.text;
			buttonUpgrade.text = text + "\n" + baseSkillPoint.CurrentCost + " " + DATA_RESOURCES.TEXT_SPRITE.SKILL_POINT[baseSkillPoint.SkillParam.SkillPathID];
		}
	}
}
