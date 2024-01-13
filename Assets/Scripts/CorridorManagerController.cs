using UnityEngine;

public class CorridorManagerController : BaseManagerController
{
	[HideInInspector]
	public CorridorLevelController CorridorLevelController;

	public override ManagerArea ManagerArea => ManagerArea.Corridor;

	public override void Start()
	{
		base.Start();
	}

	public override void Update()
	{
		base.Update();
	}

	public override void SetActiveManager(bool isEmpty = true)
	{
		base.SetActiveManager(isEmpty);
		if (!isEmpty)
		{
			CorridorLevelController.CheckStartWork();
		}
		CorridorLevelController.CorridorModel.isManagerActive = !isEmpty;
		base.MineController.UpdateIdleCash();
	}

	public override void SetRandomManager(ManagerArea area)
	{
		base.SetRandomManager(area);
		CorridorLevelController.OnChangeManager();
	}

	public override void SetManagerParam(ManagerEntity.Param param)
	{
		base.SetManagerParam(param);
		DataManager.Instance.CurrentMineSavegame.SetCorridorSavegame(CorridorLevelController.CorridorModel.Tier, ManagerSavegame.BuyOrder);
	}

	public override void SetActiveEffect(EffectState effectState, bool isEmpty = false)
	{
		base.SetActiveEffect(effectState, isEmpty);
		switch (effectState)
		{
		case EffectState.Active:
			CorridorLevelController.OnActiveManager();
			break;
		case EffectState.Cooldown:
			CorridorLevelController.OnDeactiveManager();
			break;
		case EffectState.Ready:
			CorridorLevelController.OnDeactiveManager();
			break;
		}
	}
}
