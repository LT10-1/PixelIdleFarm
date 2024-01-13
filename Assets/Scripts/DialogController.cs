using System.Collections.Generic;
using UnityEngine;

public class DialogController : BaseController
{
	[HideInInspector]
	public List<BaseDialog> ListDialogs = new List<BaseDialog>();

	private DialogIdleCash _dialogIdleCash;

	private DialogManager _dialogManagerCorridor;

	private DialogManager _dialogManagerElevator;

	private DialogManager _dialogManagerGround;

	private DialogManagerSell _dialogManagerSell;

	private DialogUpgrade _dialogUpgradeCorridor;

	private DialogUpgrade _dialogUpgradeElevator;

	private DialogUpgrade _dialogUpgradeGround;

	private DialogMineOverview _dialogMineOverview;

	private DialogSetting _dialogSetting;

	private Dialogx2AdBoost _dialogx2AdBoost;

	private DialogDailyReward _dialogDailyReward;

	private DialogFriend _dialogFriend;

	private DialogRanking _dialogRanking;

	private DialogShop _dialogShop;

	private DialogSuperShop _dialogSuperShop;

	private DialogNotEnoughSuperCash _dialogNotEnoughSuperCash;

	private DialogConfirmPurchase _dialogConfirmPurchase;

	private DialogBoostOverview _dialogBoostOverview;

	private DialogSettingStatistic _dialogSettingStatistic;

	private DialogCoupon _dialogCoupon;

	private DialogPrestige _dialogPrestige;

	private DialogIAPContent _dialogIAPContent;

	private DialogResetSkill _dialogResetSkill;

	private DialogSkillShop _dialogSkillShop;

	private DialogChestShop _dialogChestShop;

	private DialogChestConfirm _dialogChestConfirm;

	private ResearchLaboratory _ResearchLaboratory;

	private WorkShop _WorkShop;

	private OpenChest _OpenChest;

	private Expedition _Expedition;

	private ExpeditionDetail _ExpeditionDetail;

	private DialogExpeditionBoost _DialogExpeditionBoost;

	private DialogWSInfo _DialogWSInfo;

	private DialogExpInfo _DialogExpInfo;

	public DialogIdleCash DialogIdleCash => _dialogIdleCash ?? (_dialogIdleCash = (DialogIdleCash)createDialog("Prefabs/Dialog/DialogIdleCash"));

	public DialogManager DialogManagerCorridor => _dialogManagerCorridor ?? (_dialogManagerCorridor = createDialogManager(ManagerArea.Corridor));

	public DialogManager DialogManagerElevator => _dialogManagerElevator ?? (_dialogManagerElevator = createDialogManager(ManagerArea.Elevator));

	public DialogManager DialogManagerGround => _dialogManagerGround ?? (_dialogManagerGround = createDialogManager(ManagerArea.Ground));

	public DialogManagerSell DialogManagerSell => _dialogManagerSell ?? (_dialogManagerSell = (DialogManagerSell)createDialog("Prefabs/Dialog/DialogManagerSell"));

	public DialogUpgrade DialogUpgradeCorridor => _dialogUpgradeCorridor ?? (_dialogUpgradeCorridor = createDialogUpgrade(ManagerArea.Corridor));

	public DialogUpgrade DialogUpgradeElevator => _dialogUpgradeElevator ?? (_dialogUpgradeElevator = createDialogUpgrade(ManagerArea.Elevator));

	public DialogUpgrade DialogUpgradeGround => _dialogUpgradeGround ?? (_dialogUpgradeGround = createDialogUpgrade(ManagerArea.Ground));

	public DialogMineOverview DialogMineOverview => _dialogMineOverview ?? (_dialogMineOverview = (DialogMineOverview)createDialog("Prefabs/Dialog/DialogMineOverview"));

	public DialogSetting DialogSetting => _dialogSetting ?? (_dialogSetting = (DialogSetting)createDialog("Prefabs/Dialog/DialogSetting"));

	public Dialogx2AdBoost Dialogx2AdBoost => _dialogx2AdBoost ?? (_dialogx2AdBoost = (Dialogx2AdBoost)createDialog("Prefabs/Dialog/Dialogx2AdBoost"));

	public DialogDailyReward DialogDailyReward => _dialogDailyReward ?? (_dialogDailyReward = (DialogDailyReward)createDialog("Prefabs/Dialog/DialogDailyReward"));

	public DialogFriend DialogFriend => _dialogFriend ?? (_dialogFriend = (DialogFriend)createDialog("Prefabs/Dialog/DialogFriend"));

	public DialogRanking DialogRanking => _dialogRanking ?? (_dialogRanking = (DialogRanking)createDialog("Prefabs/Dialog/DialogRanking"));

	public DialogShop DialogShop => _dialogShop ?? (_dialogShop = (DialogShop)createDialog("Prefabs/Dialog/DialogShop"));

	public DialogSuperShop DialogSuperShop => _dialogSuperShop ?? (_dialogSuperShop = (DialogSuperShop)createDialog("Prefabs/Dialog/DialogSuperShop"));

	public DialogNotEnoughSuperCash DialogNotEnoughSuperCash => _dialogNotEnoughSuperCash ?? (_dialogNotEnoughSuperCash = (DialogNotEnoughSuperCash)createDialog("Prefabs/Dialog/DialogNotEnoughSuperCash"));

	public DialogConfirmPurchase DialogConfirmPurchase => _dialogConfirmPurchase ?? (_dialogConfirmPurchase = (DialogConfirmPurchase)createDialog("Prefabs/Dialog/DialogConfirmPurchase"));

	public DialogBoostOverview DialogBoostOverview => _dialogBoostOverview ?? (_dialogBoostOverview = (DialogBoostOverview)createDialog("Prefabs/Dialog/DialogBoostOverview"));

	public DialogSettingStatistic DialogSettingStatistic => _dialogSettingStatistic ?? (_dialogSettingStatistic = (DialogSettingStatistic)createDialog("Prefabs/Dialog/DialogSettingStatistic"));

	public DialogCoupon DialogCoupon => _dialogCoupon ?? (_dialogCoupon = (DialogCoupon)createDialog("Prefabs/Dialog/DialogCoupon"));

	public DialogPrestige DialogPrestige => _dialogPrestige ?? (_dialogPrestige = (DialogPrestige)createDialog("Prefabs/Dialog/DialogPrestige"));

	public DialogIAPContent DialogIAPContent => _dialogIAPContent ?? (_dialogIAPContent = (DialogIAPContent)createDialog("Prefabs/Dialog/DialogIAPContent"));

	public DialogResetSkill DialogResetSkill => _dialogResetSkill ?? (_dialogResetSkill = (DialogResetSkill)createDialog("Prefabs/Dialog/DialogResetSkill"));

	public DialogSkillShop DialogSkillShop => _dialogSkillShop ?? (_dialogSkillShop = (DialogSkillShop)createDialog("Prefabs/Dialog/DialogSkillShop"));

	public DialogChestShop DialogChestShop => _dialogChestShop ?? (_dialogChestShop = (DialogChestShop)createDialog("Prefabs/Dialog/DialogChestShop"));

	public DialogChestConfirm DialogChestConfirm => _dialogChestConfirm ?? (_dialogChestConfirm = (DialogChestConfirm)createDialog("Prefabs/Dialog/DialogChestConfirm"));

	public ResearchLaboratory ResearchLaboratory => _ResearchLaboratory ?? (_ResearchLaboratory = (ResearchLaboratory)createDialog("Prefabs/Dialog/ResearchLaboratory"));

	public WorkShop WorkShop => _WorkShop ?? (_WorkShop = (WorkShop)createDialog("Prefabs/Dialog/WorkShop"));

	public OpenChest OpenChest => _OpenChest ?? (_OpenChest = (OpenChest)createDialog("Prefabs/Dialog/OpenChest"));

	public Expedition Expedition => _Expedition ?? (_Expedition = (Expedition)createDialog("Prefabs/Dialog/Expedition"));

	public ExpeditionDetail ExpeditionDetail => _ExpeditionDetail ?? (_ExpeditionDetail = (ExpeditionDetail)createDialog("Prefabs/Dialog/ExpeditionDetail"));

	public DialogExpeditionBoost DialogExpeditionBoost => _DialogExpeditionBoost ?? (_DialogExpeditionBoost = (DialogExpeditionBoost)createDialog("Prefabs/Dialog/DialogExpeditionBoost"));

	public DialogWSInfo DialogWSInfo => _DialogWSInfo ?? (_DialogWSInfo = (DialogWSInfo)createDialog("Prefabs/Dialog/DialogWSInfo"));

	public DialogExpInfo DialogExpInfo => _DialogExpInfo ?? (_DialogExpInfo = (DialogExpInfo)createDialog("Prefabs/Dialog/DialogExpInfo"));

	public override void Awake()
	{
		base.Awake();
	}

	public bool HandleBackButton()
	{
		return false;
	}

	public void HideAllDialog()
	{
		foreach (BaseDialog listDialog in ListDialogs)
		{
			if (listDialog.gameObject.activeSelf)
			{
				listDialog.OnHide();
			}
		}
	}

	public void OnDialogShow(BaseDialog dialog)
	{
	}

	public void OnDialogHide(BaseDialog dialog)
	{
	}

	public int GetNumberActiveDialog()
	{
		int num = 0;
		foreach (BaseDialog listDialog in ListDialogs)
		{
			if (listDialog.gameObject.activeSelf)
			{
				num++;
			}
		}
		return num;
	}

	private DialogUpgrade createDialogUpgrade(ManagerArea managerArea)
	{
		DialogUpgrade dialogUpgrade = (DialogUpgrade)createDialog("Prefabs/Dialog/DialogUpgrade");
		dialogUpgrade.ManagerArea = managerArea;
		dialogUpgrade.gameObject.name = "Upgrade" + managerArea;
		return dialogUpgrade;
	}

	private DialogManager createDialogManager(ManagerArea managerArea)
	{
		DialogManager dialogManager = (DialogManager)createDialog("Prefabs/Dialog/DialogManager");
		dialogManager.ManagerArea = managerArea;
		dialogManager.gameObject.name = "DialogManager" + managerArea;
		return dialogManager;
	}

	private BaseDialog createDialog(string prefab)
	{
		GameObject gameObject = InstantiatePrefab(prefab);
		gameObject.SetActive(value: false);
		gameObject.transform.SetParent(base.transform, worldPositionStays: false);
		BaseDialog component = gameObject.GetComponent<BaseDialog>();
		component.DialogController = this;
		ListDialogs.Add(component);
		return component;
	}
}
