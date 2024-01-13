using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillPoint : BaseController
{
	public int id;

	public Button button;

	public TMP_Text Level;

	public Image[] star;

	public Image arrow;

	public Image icon;

	public Image blur;

	public Image focus;

	public Image focusButton;

	[HideInInspector]
	public SkillEntity.Param SkillParam;

	public bool IsActive => !blur.gameObject.activeSelf;

	public bool IsUnlockable => focus.gameObject.activeSelf;

	public bool IsMaxLevel => CurrentLevel == SkillParam.MaxLevel;

	public int CurrentLevel => DataManager.Instance.SavegameData.SkillSaveGame.ContainsKey(SkillParam.SkillID) ? DataManager.Instance.SavegameData.SkillSaveGame[SkillParam.SkillID] : 0;

	public double CurrentValue => SkillParam.ParamX[Math.Max(0, CurrentLevel - 1)];

	public int CurrentCost => SkillParam.SkillPoint[CurrentLevel];

	public string DetailText
	{
		get
		{
			switch (SkillParam.EffectId)
			{
			case -1:
				return string.Empty;
			case 0:
				return string.Empty;
			case 1:
				return string.Format("Your {0} generates {1}% more income", DATA_TEXT.MINES.CONTINENT_MINES[SkillParam.ParamY / 5][SkillParam.ParamY % 5] + " Mine", CurrentValue.MinifyIncomeFactor());
			case 2:
				return $"All your Mines generate {CurrentValue.MinifyIncomeFactor()}% more income";
			case 3:
				return $"Unlocking Rock Barriers costs {CurrentValue.MinifyIncomeFactor()}% less";
			case 4:
				return $"Rock Barriers are removed {CurrentValue.MinifyIncomeFactor()}% faster";
			case 5:
				return $"Increasing the Prestige Level of Mines cost {CurrentValue.MinifyIncomeFactor()}% less";
			case 6:
				return $"You collect {CurrentValue.MinifyIncomeFactor()}% more Idle Cash after watching an Ad";
			case 7:
				return $"Ad Boost generate {CurrentValue.MinifyIncomeFactor()}% more income";
			case 8:
				return $"Ad Boosts last {CurrentValue.MinifyIncomeFactor()}% longer";
			case 9:
				return $"Maximum Ad Boost duration is inreased by {CurrentValue.MinifyIncomeFactor()}%";
			case 10:
			{
				string text = DATA_TEXT.MANAGER_EFFECT_DESCRIPTION[(int)DataUtils.GetManagerEffect(SkillParam.ParamY)];
				text = text.Substring(text.IndexOf(" ") + 1);
				string arg = DataUtils.ManagerAreaText(DataUtils.GetManagerArea(SkillParam.ParamY)) + " Managers ";
				return $"The {text} of your {arg} is {CurrentValue.MinifyIncomeFactor()}% stronger";
			}
			case 11:
			{
				string arg = DataUtils.ManagerAreaText((ManagerArea)SkillParam.ParamY) + " Managers ";
				return $"The Effects of your {arg} last {CurrentValue.MinifyIncomeFactor()}% longer";
			}
			case 12:
			{
				string arg = DataUtils.ManagerAreaText((ManagerArea)SkillParam.ParamY) + " Managers ";
				return $"Your {arg} regenerate {CurrentValue.MinifyIncomeFactor()}% faster after activations";
			}
			case 13:
				return $"Hiring new Mangers costs {CurrentValue.MinifyIncomeFactor()}% less";
			default:
				return string.Empty;
			}
		}
	}

	public string DetailTextWithNext
	{
		get
		{
			if (IsMaxLevel || CurrentLevel == 0)
			{
				return DetailText;
			}
			return DetailText + "\n<color=green>Next level</color> " + SkillParam.ParamX[CurrentLevel] + "%";
		}
	}

	public override void Start()
	{
		base.Start();
		Transform transform = arrow.transform;
		Vector3 localPosition = arrow.transform.localPosition;
		transform.DOLocalMoveY(localPosition.y + 10f, 0.5f).SetLoops(-1, LoopType.Yoyo);
		button.onClick.AddListener(OnClickSkillPoint);
	}

	public void Init(SkillEntity.Param param)
	{
		SkillParam = param;
		switch (param.EffectId)
		{
		case -1:
			break;
		case 0:
			break;
		case 1:
			break;
		case 2:
			break;
		case 3:
			break;
		case 4:
			break;
		case 5:
			break;
		case 6:
			break;
		case 7:
			break;
		case 8:
			break;
		case 9:
			break;
		case 10:
			break;
		case 11:
			break;
		case 12:
			break;
		case 13:
			break;
		}
	}

	public void OnClickSkillPoint()
	{
		BaseController.GameController.DialogController.ResearchLaboratory.ShowDetail(this);
	}

	public void SetUnlocked(bool isUnlock)
	{
		blur.gameObject.SetActive(!isUnlock);
		Level.transform.parent.gameObject.SetActive(isUnlock);
		if (isUnlock)
		{
			SetUnlockAble(unlockable: false);
		}
	}

	public void SetUnlockAble(bool unlockable)
	{
		focus.gameObject.SetActive(unlockable);
	}

	public void setLevel(float level)
	{
		for (int i = 0; i < star.Length; i++)
		{
			if (i < Mathf.FloorToInt(level / 2f))
			{
				star[i].fillAmount = 1f;
			}
			else if (i < Mathf.CeilToInt(level / 2f))
			{
				star[i].fillAmount = 0.5f;
			}
			else
			{
				star[i].fillAmount = 0f;
			}
		}
		Level.SetText("Level " + level);
	}
}
