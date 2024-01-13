using UnityEngine;

public class GroundManagerController : BaseManagerController
{
	[HideInInspector]
	public GroundController GroundController;

	public override ManagerArea ManagerArea => ManagerArea.Ground;

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
			GroundController.CheckStartWork();
		}
		GroundController.GroundModel.isManagerActive = !isEmpty;
		base.MineController.UpdateIdleCash();
	}

	public override void SetRandomManager(ManagerArea area)
	{
		base.SetRandomManager(area);
		GroundController.OnChangeManager();
	}

	public override void SetManagerParam(ManagerEntity.Param param)
	{
		base.SetManagerParam(param);
		DataManager.Instance.CurrentMineSavegame.GroundManagerSavegame = ManagerSavegame;
	}

	public override void SetActiveEffect(EffectState effectState, bool isEmpty = false)
	{
		base.SetActiveEffect(effectState, isEmpty);
		switch (effectState)
		{
		case EffectState.Active:
			GroundController.OnActiveManager();
			break;
		case EffectState.Cooldown:
			GroundController.OnDeactiveManager();
			break;
		case EffectState.Ready:
			GroundController.OnDeactiveManager();
			break;
		}
	}
}
