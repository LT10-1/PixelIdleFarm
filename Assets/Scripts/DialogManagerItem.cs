using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogManagerItem : BaseController
{
	public TMP_Text TextName;

	public TMP_Text TextType;

	public TMP_Text TextEffectTime;

	public TMP_Text TextEffectDesc;

	public Image AvatarFrame;

	public Image Avatar;

	public Button ButtonSkill;

	public Image ButtonSkillInactive;

	public Button ButtonAssign;

	public Button ButtonSell;

	public GameObject ProgressBar;

	public Image ProgressFillImage;

	public ValueGroupController ValueGroupController;

	public GameObject EffectGroup;

	[HideInInspector]
	public ManagerSavegame ManagerSavegame;

	[HideInInspector]
	public ManagerArea ManagerArea;

	[HideInInspector]
	public ManagerEntity.Param ManagerParam;

	[HideInInspector]
	public Action OnClickActive;

	[HideInInspector]
	public Action OnClickAssign;

	[HideInInspector]
	public Action OnClickSell;

	[HideInInspector]
	public EffectState EffectState;

	[HideInInspector]
	public bool IsAssigned;

	[HideInInspector]
	public bool isEmpty;

	[HideInInspector]
	public int Tier;

	public double ActiveTime
	{
		get
		{
			if (ManagerParam == null || ManagerArea == (ManagerArea)0)
			{
				return 0.0;
			}
			return ManagerParam.ActiveTime * (1.0 + BaseController.GameController.SkillController.ManagerEffectDurationBoost[ManagerArea] / 100.0);
		}
	}

	public double CooldownTime
	{
		get
		{
			if (ManagerParam == null || ManagerArea == (ManagerArea)0)
			{
				return 0.0;
			}
			return ManagerParam.Cooldown * (1.0 - BaseController.GameController.SkillController.ManagerCooldownBoost[ManagerArea] / 100.0);
		}
	}

	public override void Update()
	{
		if (ManagerSavegame != null && ManagerSavegame.TimeActiveSkill != 0)
		{
			double totalSeconds = TimeSpan.FromTicks(DateTime.Now.Ticks - ManagerSavegame.TimeActiveSkill).TotalSeconds;
			switch (EffectState)
			{
			case EffectState.Active:
			{
				double num = ActiveTime - totalSeconds;
				if (num >= 0.0)
				{
					SetTimeText(num);
					ProgressFillImage.fillAmount = (float)(num / ActiveTime);
				}
				else
				{
					SetActiveEffect(EffectState.Cooldown);
				}
				break;
			}
			case EffectState.Cooldown:
			{
				double num = CooldownTime - (totalSeconds - ActiveTime);
				if (num >= 0.0)
				{
					SetTimeText(num);
					ProgressFillImage.fillAmount = 1f - (float)(num / CooldownTime);
				}
				else
				{
					SetActiveEffect(EffectState.Ready);
				}
				break;
			}
			case EffectState.Ready:
				ManagerSavegame.TimeActiveSkill = 0L;
				break;
			}
		}
		if (ManagerSavegame != null && ManagerSavegame.TimeActiveSkill == 0 && EffectState != 0)
		{
			SetActiveEffect(EffectState.Ready);
		}
	}

	private void OnEnable()
	{
		OnLoadManagerSaveGame();
	}

	public void SetPopupManager()
	{
		SetPopupType(isDialogManager: true);
	}

	public void SetPopupMineOverview()
	{
		SetPopupType(isDialogManager: false);
		IsAssigned = true;
		SetSkillActive();
	}

	public void SetPopupType(bool isDialogManager)
	{
		ButtonAssign.gameObject.SetActive(isDialogManager);
		ButtonSell.gameObject.SetActive(isDialogManager);
		ValueGroupController.gameObject.SetActive(!isDialogManager);
	}

	public void Init(ManagerSavegame managerSavegame, ManagerArea managerArea, bool isEmpty = false)
	{
		this.isEmpty = isEmpty;
		ManagerSavegame = managerSavegame;
		ManagerArea = managerArea;
		EffectGroup.SetActive(!isEmpty);
		if (isEmpty)
		{
			TextType.text = "No manager assigned.";
			TextType.color = Color.white;
			Avatar.sprite = DataUtils.GetAvatarSprite(ManagerParam.Area, -1);
		}
		else
		{
			ManagerParam = DataManager.Instance.ManagerParams[managerSavegame.ManagerID];
			TextName.text = managerSavegame.ManagerName;
			TextType.text = DATA_TEXT.MANAGER_RARITY[ManagerParam.RarityID - 1];
			TextType.color = COLOR.COLOR_MANAGER_RARITY[ManagerParam.RarityID - 1];
			TextEffectTime.text = $"Effect: {ActiveTime.FormatTimeString()}";
			Avatar.sprite = DataUtils.GetAvatarSprite(ManagerParam.Area, ManagerParam.RarityID);
			ButtonSkill.GetComponent<Image>().sprite = DataUtils.GetSpriteEffect(ManagerParam.EffectID);
			ButtonSkillInactive.sprite = DataUtils.GetSpriteEffect(ManagerParam.EffectID, active: false);
		}
		OnShowUpdate();
		SetAssign(isAssign: false);
		SetSkillActive();
		ButtonSkill.onClick.AddListener(delegate
		{
			if (OnClickActive != null)
			{
				OnClickActive();
			}
		});
		ButtonAssign.onClick.AddListener(delegate
		{
			if (OnClickAssign != null)
			{
				OnClickAssign();
			}
		});
		ButtonSell.onClick.AddListener(delegate
		{
			if (OnClickSell != null)
			{
				OnClickSell();
			}
		});
		OnLoadManagerSaveGame();
	}

	public void OnShowUpdate()
	{
		if (EffectGroup.activeSelf)
		{
			double valueX = ManagerParam.ValueX;
			if (ManagerParam.ValueX < 1.0)
			{
				valueX *= 1.0 - BaseController.GameController.SkillController.ManagerEffectBoost[(ManagerEffect)ManagerParam.EffectID] / 100.0;
				valueX = (1.0 - valueX) * 100.0;
			}
			else
			{
				valueX *= 1.0 + BaseController.GameController.SkillController.ManagerEffectBoost[(ManagerEffect)ManagerParam.EffectID] / 100.0;
			}
			TextEffectDesc.text = string.Format(DATA_TEXT.MANAGER_EFFECT_DESCRIPTION[(int)DataUtils.GetManagerEffect(ManagerParam.EffectID)], valueX.MinifyIncomeFactor());
		}
	}

	public void SetAssign(bool isAssign)
	{
		IsAssigned = isAssign;
		ButtonSell.gameObject.SetActive(!isAssign);
		AvatarFrame.gameObject.SetActive(isAssign);
		SetSkillActive();
		if (isAssign)
		{
			ButtonAssign.GetComponentInChildren<TMP_Text>().text = "Unassign";
			ButtonAssign.GetComponent<Image>().sprite = BaseController.LoadSprite("Images/UI/Managers/assignbar_red");
		}
		else
		{
			ButtonAssign.GetComponentInChildren<TMP_Text>().text = "Assign";
			ButtonAssign.GetComponent<Image>().sprite = BaseController.LoadSprite("Images/UI/Managers/assignbar");
		}
	}

	public void OnLoadManagerSaveGame()
	{
		if (ManagerSavegame == null || ManagerParam == null)
		{
			return;
		}
		long timeActiveSkill = ManagerSavegame.TimeActiveSkill;
		SetActiveEffect(EffectState.Ready);
		if (timeActiveSkill != 0)
		{
			double totalSeconds = TimeSpan.FromTicks(DateTime.Now.Ticks - timeActiveSkill).TotalSeconds;
			if (totalSeconds <= ActiveTime)
			{
				SetActiveEffect(EffectState.Active);
			}
			else if (totalSeconds <= ActiveTime + CooldownTime)
			{
				SetActiveEffect(EffectState.Cooldown);
			}
			ManagerSavegame.TimeActiveSkill = timeActiveSkill;
		}
	}

	public void SetSkillActive()
	{
		bool flag = IsAssigned && EffectState == EffectState.Ready;
		ButtonSkill.gameObject.SetActive(flag);
		ButtonSkillInactive.gameObject.SetActive(!flag);
	}

	public void SetActiveEffect(EffectState effectState)
	{
		EffectState = effectState;
		SetSkillActive();
		ProgressBar.SetActive(EffectState != EffectState.Ready);
		switch (effectState)
		{
		case EffectState.Active:
			SetTextColor(Color.yellow);
			ManagerSavegame.TimeActiveSkill = DateTime.Now.Ticks;
			SetTimeText(ActiveTime);
			ProgressFillImage.sprite = BaseController.LoadSprite("Images/UI/Managers/skill bar_activated");
			break;
		case EffectState.Cooldown:
			SetTextColor(new Color(1f, 1f, 1f, 0.6f));
			SetTimeText(CooldownTime);
			ProgressFillImage.sprite = BaseController.LoadSprite("Images/UI/Managers/skill bar_cooldown");
			break;
		case EffectState.Ready:
			SetTimeText(ActiveTime);
			SetTextColor(Color.white);
			ManagerSavegame.TimeActiveSkill = 0L;
			break;
		}
	}

	public void SetTextColor(Color color)
	{
		TextEffectTime.color = color;
		TextEffectDesc.color = color;
	}

	public void SetTimeText(double time)
	{
		switch (EffectState)
		{
		case EffectState.Active:
			TextEffectTime.text = $"Effect: {time.FormatTimeString()}";
			break;
		case EffectState.Cooldown:
			TextEffectTime.text = $"Ready in {time.FormatTimeString()}";
			break;
		case EffectState.Ready:
			TextEffectTime.text = $"Effect: {time.FormatTimeString()}";
			break;
		}
	}
}
