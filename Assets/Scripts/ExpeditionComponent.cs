using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpeditionComponent : BaseController
{
	public Image RibbonImage;

	public Image TimeFillBar;

	public TMP_Text DurationText;

	public TMP_Text ExpeditionType;

	public TMP_Text AnimationBoostText;

	public Transform ExpeditionItemParent;

	public UIButtonController ButtonStart;

	public UIButtonController ButtonBoost;

	public UIButtonController ButtonComplete;

	[HideInInspector]
	public ExpeditionSavegame ExpeditionSavegame;

	[HideInInspector]
	public List<ExpeditionItemComponent> ExpeditionItemComponents;

	private float AnimationBoostTextInitPositionY;

	private float TweenDuration = 3f;

	public ExpeditionEntity.Param ExpeditionParam => DataManager.Instance.ExpeditionDictionary[(ExpeditionRarity)ExpeditionSavegame.Rarity];

	public override void Start()
	{
		base.Start();
		ButtonStart.OnClickCallback = OnClickStartExpedition;
		ButtonBoost.OnClickCallback = OnClickBoostExpedition;
		ButtonComplete.OnClickCallback = OnClickCompleteExpedition;
		BaseController.GameController.ExpeditionController.OnExpeditionStateChange.Add(OnUpdateExpeditionState);
	}

	public override void Update()
	{
		base.Update();
		if (ExpeditionSavegame.StartTime != 0)
		{
			switch (BaseController.GameController.ExpeditionController.ExpeditionState)
			{
			case ExpeditionState.InProgress:
			{
				long num = ExpeditionSavegame.Duration * 10000000;
				long num2 = DateTime.Now.Ticks - ExpeditionSavegame.StartTime;
				TimeFillBar.fillAmount = (float)num2 / (float)num;
				DurationText.text = ((num - num2) / 10000000).FormatTimeString(getFull: true);
				break;
			}
			}
		}
	}

	public void OnUpdateExpeditionState()
	{
		if (ExpeditionSavegame.StartTime != 0)
		{
			ExpeditionState expeditionState = BaseController.GameController.ExpeditionController.ExpeditionState;
			ButtonStart.gameObject.SetActive(expeditionState == ExpeditionState.Idle);
			ButtonBoost.gameObject.SetActive(expeditionState == ExpeditionState.InProgress);
			ButtonComplete.gameObject.SetActive(expeditionState == ExpeditionState.Completed);
			if (expeditionState != 0 && expeditionState != ExpeditionState.InProgress && expeditionState == ExpeditionState.Completed)
			{
				TimeFillBar.fillAmount = 1f;
				DurationText.text = string.Empty;
			}
		}
	}

	public void BoostAnimation(long duration)
	{
		if (AnimationBoostTextInitPositionY == 0f)
		{
			Vector3 localPosition = AnimationBoostText.transform.localPosition;
			AnimationBoostTextInitPositionY = localPosition.y;
		}
		AnimationBoostText.text = "-" + (duration / 10000000).FormatTimeString();
		DOTween.Kill(AnimationBoostText.transform);
		AnimationBoostText.gameObject.SetActive(value: true);
		AnimationBoostText.alpha = 1f;
		((Graphic)AnimationBoostText).DOFade(0f, TweenDuration);
		Transform transform = AnimationBoostText.transform;
		Vector3 localPosition2 = AnimationBoostText.transform.localPosition;
		transform.localPosition = new Vector3(localPosition2.x, AnimationBoostTextInitPositionY);
		AnimationBoostText.transform.DOLocalMoveY(AnimationBoostTextInitPositionY + 200f, TweenDuration).OnComplete(delegate
		{
			AnimationBoostText.gameObject.SetActive(value: false);
		});
	}

	public void OnClickStartExpedition()
	{
		if (ExpeditionSavegame != null)
		{
			BaseController.GameController.ExpeditionController.StartExpedition(ExpeditionSavegame);
			BaseController.GameController.DialogController.ExpeditionDetail.OnHide();
		}
	}

	public void OnClickBoostExpedition()
	{
		BaseController.GameController.DialogController.DialogExpeditionBoost.OnShow();
	}

	public void OnClickCompleteExpedition()
	{
		BaseController.GameController.ExpeditionController.CompleteExpedition();
	}

	public void SetData(ExpeditionSavegame expeditionSavegame)
	{
		ExpeditionSavegame = expeditionSavegame;
		RibbonImage.overrideSprite = BaseController.LoadSprite(DATA_RESOURCES.IMAGE.WORKSHOP_RIBBON_LIST[ExpeditionSavegame.Rarity]);
		TimeFillBar.fillAmount = 0f;
		DurationText.text = ExpeditionSavegame.Duration.FormatTimeString();
		ExpeditionType.text = DATA_TEXT.COLLECTIBLES.LIST[ExpeditionSavegame.Rarity] + " Expedition";
		for (int i = 0; i < expeditionSavegame.ItemID.Count; i++)
		{
			int data = expeditionSavegame.ItemID[i];
			if (ExpeditionItemComponents.Count <= i)
			{
				CreateExpeditionItemComponent();
			}
			ExpeditionItemComponent expeditionItemComponent = ExpeditionItemComponents[i];
			expeditionItemComponent.gameObject.SetActive(value: true);
			expeditionItemComponent.ExpeditionComponent = this;
			bool flag = i == 0;
			expeditionItemComponent.SetShowItemDetail(flag);
			if (flag)
			{
				expeditionItemComponent.SetData(data);
			}
		}
		for (int j = expeditionSavegame.ItemID.Count; j < ExpeditionItemComponents.Count; j++)
		{
			ExpeditionItemComponents[j].gameObject.SetActive(value: false);
		}
		OnUpdateExpeditionState();
	}

	public ExpeditionItemComponent CreateExpeditionItemComponent()
	{
		ExpeditionItemComponent expeditionItemComponent = InstantiatePrefab<ExpeditionItemComponent>("Prefabs/Dialog/Component/ExpeditionItemComponent");
		expeditionItemComponent.transform.SetParent(ExpeditionItemParent, worldPositionStays: false);
		ExpeditionItemComponents.Add(expeditionItemComponent);
		return expeditionItemComponent;
	}
}
