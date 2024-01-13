using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BaseManagerController : CharacterControllerSekeleton
{
	public GameObject EmptyManager;

	public GameObject AnimateManager;

	public GameObject EffectAura;

	public SpriteButtonController EffectButton;

	public TMP_Text EffectTime;

	[HideInInspector]
	public bool isEmpty = true;

	[HideInInspector]
	public EffectState EffectState;

	[HideInInspector]
	public ManagerEntity.Param ManagerParam;

	[HideInInspector]
	public ManagerSavegame ManagerSavegame;

	[HideInInspector]
	public DialogManagerItem DialogManagerItem;

	[HideInInspector]
	public DialogManagerItem DialogMineOverviewItem;

	private bool isExit;

	public virtual ManagerArea ManagerArea => ManagerArea.Corridor;

	public double ActiveTime
	{
		get
		{
			if (ManagerParam == null)
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
			if (ManagerParam == null)
			{
				return 0.0;
			}
			return ManagerParam.Cooldown * (1.0 - BaseController.GameController.SkillController.ManagerCooldownBoost[ManagerArea] / 100.0);
		}
	}

	public override void Start()
	{
		base.Start();
		EffectButton.OnClickCallback = ClickStartEffect;
	}

	public override void Update()
	{
		base.Update();
		if (ManagerSavegame == null || ManagerSavegame.TimeActiveSkill == 0)
		{
			return;
		}
		double totalSeconds = TimeSpan.FromTicks(DateTime.Now.Ticks - ManagerSavegame.TimeActiveSkill).TotalSeconds;
		switch (EffectState)
		{
		case EffectState.Active:
		{
			double num = ActiveTime - totalSeconds;
			if (num >= 0.0)
			{
				SetTimeText(num);
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

	public virtual void SetActiveManager(bool isEmpty = true)
	{
		this.isEmpty = isEmpty;
		EmptyManager.SetActive(isEmpty);
		AnimateManager.SetActive(!isEmpty);
		StopAllCoroutines();
		if (isEmpty)
		{
			ManagerSavegame = null;
		}
		SetActiveEffect(EffectState.Ready, isEmpty);
		if (!isEmpty)
		{
			base.gameObject.SetActive(value: true);
			StartCoroutine(ManagerAnimate());
		}
	}

	public virtual void SetActiveEffect(EffectState effectState, bool isEmpty = false)
	{
		EffectState = effectState;
		EffectButton.gameObject.SetActive(!isEmpty && effectState == EffectState.Ready);
		if (CONST.TEST_MODE_VIDEO_AD)
		{
			EffectButton.gameObject.SetActive(value: false);
		}
		EffectTime.gameObject.SetActive(!isEmpty && effectState != EffectState.Ready);
		if (!isEmpty)
		{
			if (effectState == EffectState.Active)
			{
				base.spineAnimationState.SetAnimation(0, ANIMATION.MANAGER_AURA_WAVE[ManagerParam.RarityID - 1], loop: true);
			}
			else
			{
				base.spineAnimationState.SetAnimation(0, "idle", loop: true);
			}
			base.skeleton.SetColor((effectState != EffectState.Cooldown) ? Color.white : new Color(0.6f, 0.6f, 0.6f, 1f));
			switch (effectState)
			{
			case EffectState.Active:
				ManagerSavegame.TimeActiveSkill = DateTime.Now.Ticks;
				SetTimeText(ActiveTime);
				break;
			case EffectState.Cooldown:
				SetTimeText(CooldownTime);
				break;
			case EffectState.Ready:
				ManagerSavegame.TimeActiveSkill = 0L;
				break;
			}
		}
	}

	public virtual void SetRandomManager(ManagerArea area)
	{
		List<ManagerEntity.Param> list = DataManager.Instance.ManagerByAreaParams[(int)area];
		ManagerEntity.Param managerParam = list[UnityEngine.Random.Range(0, list.Count)];
		SetManagerParam(managerParam);
		SetActiveEffect(EffectState.Ready, isEmpty);
	}

	public void SetTimeText(double time)
	{
		EffectTime.text = ((long)time).FormatTimeString();
	}

	public void LoadManagerSavegame(ManagerSavegame managerSavegame)
	{
		ManagerSavegame = managerSavegame;
		long timeActiveSkill = ManagerSavegame.TimeActiveSkill;
		SetManagerParam(DataManager.Instance.ManagerParams[ManagerSavegame.ManagerID]);
		SetActiveManager(isEmpty: false);
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

	public virtual void SetManagerParam(ManagerEntity.Param param)
	{
		ManagerParam = param;
		if (ManagerSavegame == null)
		{
			ManagerSavegame = new ManagerSavegame();
		}
		ManagerSavegame.ManagerID = ManagerParam.ManagerID;
		base.skeleton.SetSkin(ANIMATION.MANAGER_SKIN[ManagerParam.RarityID - 1]);
		EffectButton.GetComponent<SpriteRenderer>().sprite = DataUtils.GetSpriteEffect(ManagerParam.EffectID);
	}

	public void ClickStartEffect()
	{
		//BaseController.GameController.AnalyticController.LogEvent("active_manager_skill");
		BaseController.GameController.AudioController.PlayOneShot("Audios/Effect/activeskill");
		OnStartEffect();
	}

	public void OnStartEffect()
	{
		SetActiveEffect(EffectState.Active);
		if (DialogManagerItem != null)
		{
			DialogManagerItem.OnLoadManagerSaveGame();
		}
		if (DialogMineOverviewItem != null)
		{
			DialogMineOverviewItem.OnLoadManagerSaveGame();
		}
		BaseController.GameController.InvokeOnCashChangeCallback();
	}

	private IEnumerator ManagerAnimate()
	{
		yield return new WaitForSeconds(UnityEngine.Random.Range(3, 5));
		if (!isEmpty)
		{
			if (EffectState == EffectState.Active)
			{
				base.spineAnimationState.SetAnimation(0, ANIMATION.MANAGER_AURA_WAVE[ManagerParam.RarityID - 1], loop: false);
				base.spineAnimationState.AddAnimation(0, ANIMATION.MANAGER_AURA_IDLE[ManagerParam.RarityID - 1], loop: true, 0f);
			}
			else
			{
				base.spineAnimationState.SetAnimation(0, "wave", loop: false);
				base.spineAnimationState.AddAnimation(0, "idle", loop: true, 0f);
			}
			StartCoroutine(ManagerAnimate());
		}
	}

	private void OnMouseDown()
	{
		isExit = false;
	}

	private void OnMouseUp()
	{
		BaseController.GameController.TutorialController.RemoveTutorial(TutorialType.HireManager);
		if (!base.CheckClickCanvas && !isExit)
		{
			switch (ManagerArea)
			{
			case ManagerArea.Corridor:
				BaseController.GameController.DialogController.DialogManagerCorridor.CurrentCorridorTier = ((CorridorManagerController)this).CorridorLevelController.CorridorModel.Tier;
				BaseController.GameController.DialogController.DialogManagerCorridor.OnShow();
				break;
			case ManagerArea.Elevator:
				BaseController.GameController.DialogController.DialogManagerElevator.OnShow();
				break;
			case ManagerArea.Ground:
				BaseController.GameController.DialogController.DialogManagerGround.OnShow();
				break;
			}
		}
	}

	private void OnMouseExit()
	{
		isExit = true;
	}
}
