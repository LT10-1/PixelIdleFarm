using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpeditionDetail : BaseDialog
{
	public Button buttonExit;

	public Transform ExpeditionContent;

	public UIButtonController ButtonRefresh;

	[HideInInspector]
	public List<ExpeditionComponent> ExpeditionComponents = new List<ExpeditionComponent>();

	protected override float OnShowScaleAmount => 0.01f;

	public override void Start()
	{
		base.Start();
		buttonExit.onClick.AddListener(OnHide);
		ButtonRefresh.OnClickCallback = OnClickRefresh;
	}

	public override void Update()
	{
		base.Update();
		ButtonRefresh.gameObject.SetActive(BaseController.GameController.ExpeditionController.ExpeditionRefreshAvailable);
	}

	public override void OnShow()
	{
		base.OnShow();
		UpdateData();
	}

	public void OnClickRefresh()
	{
		BaseController.GameController.DialogController.DialogConfirmPurchase.OnShow("Reroll Expedition", "Reroll for opportunity of better Expedition!", "You can reroll only one time each day.", "Images/UI/Cashgain/boosted", 0.4f);
		BaseController.GameController.DialogController.DialogConfirmPurchase.OnClickConfirm = delegate
		{
			BaseController.GameController.ExpeditionController.RefreshExpedition();
			UpdateData();
			//BaseController.GameController.AnalyticController.LogEvent("expedition_refresh");
		};
	}

	public void UpdateData()
	{
		ButtonRefresh.gameObject.SetActive(BaseController.GameController.ExpeditionController.ExpeditionRefreshAvailable);
		for (int i = 0; i < DataManager.Instance.SavegameData.ExpeditionChooseList.Count; i++)
		{
			ExpeditionSavegame data = DataManager.Instance.SavegameData.ExpeditionChooseList[i];
			if (ExpeditionComponents.Count <= i)
			{
				CreateExpeditionComponent();
			}
			ExpeditionComponent expeditionComponent = ExpeditionComponents[i];
			expeditionComponent.gameObject.SetActive(value: true);
			expeditionComponent.SetData(data);
		}
		for (int j = DataManager.Instance.SavegameData.ExpeditionChooseList.Count; j < ExpeditionComponents.Count; j++)
		{
			ExpeditionComponents[j].gameObject.SetActive(value: false);
		}
		LayoutRebuilder.ForceRebuildLayoutImmediate(ExpeditionContent.GetComponent<RectTransform>());
	}

	public ExpeditionComponent CreateExpeditionComponent()
	{
		ExpeditionComponent component = InstantiatePrefab("Prefabs/Dialog/Component/ExpeditionComponent").GetComponent<ExpeditionComponent>();
		component.transform.SetParent(ExpeditionContent, worldPositionStays: false);
		ExpeditionComponents.Add(component);
		return component;
	}
}
