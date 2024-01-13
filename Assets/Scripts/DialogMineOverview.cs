using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogMineOverview : BaseDialog
{
	public Transform ScrollViewContent;

	public UIButtonController ButtonActiveAll;

	public ValueGroupController ValueGroupController;

	[HideInInspector]
	public Dictionary<int, DialogManagerItem> DialogManagerItems;

	public CorridorController CorridorController => BaseController.GameController.MineController.CorridorController;

	public override void Start()
	{
		base.Start();
		base.BackgroundDialog.SetTitle("Mine Shaft Overview");
		ButtonActiveAll.text = "Activate";
		ButtonActiveAll.onClick.AddListener(OnClickActivateAll);
	}

	public override void Update()
	{
		base.Update();
		UpdateData();
	}

	public override void OnShow()
	{
		base.OnShow();
		ValueGroupController.ValueWhite.SetCoinType(base.CurrentCoinType);
		CreateData();
	}

	public override void OnHide()
	{
		base.OnHide();
		if (DialogManagerItems != null)
		{
			foreach (KeyValuePair<int, DialogManagerItem> dialogManagerItem in DialogManagerItems)
			{
				UnityEngine.Object.Destroy(dialogManagerItem.Value.gameObject);
			}
			DialogManagerItems = new Dictionary<int, DialogManagerItem>();
		}
	}

	public void UpdateData()
	{
		if (DialogManagerItems != null && DialogManagerItems.Count != 0)
		{
			double num = 0.0;
			bool flag = false;
			foreach (KeyValuePair<int, DialogManagerItem> dialogManagerItem in DialogManagerItems)
			{
				int key = dialogManagerItem.Key;
				double value = CorridorController.CorridorLevelControllers[dialogManagerItem.Key - 1].CorridorModel.CompleteDollarPerSecondStat.Value;
				num += value;
				dialogManagerItem.Value.ValueGroupController.ValueWhite.SetCoinType(base.CurrentCoinType);
				dialogManagerItem.Value.ValueGroupController.ValueWhite.SetMoney(value, minify: true, showMoney: true, "/s");
				bool flag2 = !MathUtils.CompareDoubleToZero(value - BaseController.GameController.MineController.MineModel.CorridorTotalExtractionCalculator.CorridorGainPerSercond(key));
				dialogManagerItem.Value.ValueGroupController.ValueWhite.SetMoneyBonusColor(flag2);
				flag = (flag2 || flag);
			}
			ValueGroupController.ValueWhite.SetMoney(num, minify: true, showMoney: true, "/s");
			ValueGroupController.ValueWhite.SetMoneyBonusColor(flag);
			bool flag3 = false;
			foreach (KeyValuePair<int, DialogManagerItem> dialogManagerItem2 in DialogManagerItems)
			{
				double num2 = Mathf.Clamp((float)(dialogManagerItem2.Value.ValueGroupController.ValueWhite.Cash / num * 100.0), 0.01f, 99.9f);
				dialogManagerItem2.Value.ValueGroupController.ValueGreen.text = ((!MathUtils.CompareDoubleToZero(num2 - 99.9000015258789)) ? num2.MinifyFormat() : "99.99") + "%";
				if (dialogManagerItem2.Value.ButtonSkill.gameObject.activeInHierarchy)
				{
					flag3 = true;
				}
			}
			ButtonActiveAll.enabled = flag3;
			ButtonActiveAll.GetComponent<Image>().color = ((!flag3) ? new Color(0.75f, 0.75f, 0.75f) : Color.white);
		}
	}

	public void CreateData()
	{
		DialogManagerItems = new Dictionary<int, DialogManagerItem>();
		ValueGroupController.ValueGreen.gameObject.SetActive(value: false);
		for (int i = 0; i < CorridorController.CorridorLevelControllers.Count; i++)
		{
			int tier = i + 1;
			if (CorridorLevelController(tier).IsActiveMine)
			{
				CreateManagerItem(CorridorLevelController(tier).CorridorManagerController.ManagerSavegame, tier);
			}
		}
	}

	public DialogManagerItem CreateManagerItem(ManagerSavegame managerSavegame, int tier)
	{
		DialogManagerItem component = InstantiatePrefab("Prefabs/Dialog/Component/DialogManagerItem").GetComponent<DialogManagerItem>();
		component.transform.SetParent(ScrollViewContent);
		component.transform.localScale = Vector3.one;
		component.Tier = tier;
		component.Init(managerSavegame, ManagerArea.Corridor, CorridorManagerController(tier).isEmpty);
		component.SetPopupMineOverview();
		component.TextName.text = $"Mine Shaft {tier} Level {CorridorLevelController(tier).CorridorModel.Level}";
		component.OnClickActive = delegate
		{
			BaseController.GameController.AudioController.PlayOneShot("Audios/Effect/activeskill");
			OnClickActive(tier);
		};
		CorridorManagerController(tier).DialogMineOverviewItem = component;
		DialogManagerItems[tier] = component;
		return component;
	}

	public CorridorLevelController CorridorLevelController(int tier)
	{
		return CorridorController.CorridorLevelControllers[tier - 1];
	}

	public CorridorManagerController CorridorManagerController(int tier)
	{
		return CorridorLevelController(tier).CorridorManagerController;
	}

	public void OnClickActive(int tier)
	{
		CorridorManagerController(tier).OnStartEffect();
	}

	public void OnClickActivateAll()
	{
		if (DialogManagerItems != null)
		{
			BaseController.GameController.AudioController.PlayOneShot("Audios/Effect/activeskill");
			foreach (KeyValuePair<int, DialogManagerItem> dialogManagerItem in DialogManagerItems)
			{
				if (dialogManagerItem.Value.ButtonSkill.gameObject.activeInHierarchy)
				{
					OnClickActive(dialogManagerItem.Value.Tier);
				}
			}
		}
	}
}
