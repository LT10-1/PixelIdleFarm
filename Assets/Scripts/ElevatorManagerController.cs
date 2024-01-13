using UnityEngine;

public class ElevatorManagerController : BaseManagerController
{
	[HideInInspector]
	public ElevatorController ElevatorController;

	public override ManagerArea ManagerArea => ManagerArea.Elevator;

	public override void Start()
	{
		base.Start();
		base.skeletonAnimation.GetComponent<MeshRenderer>().sharedMaterial.shader = Resources.Load<Shader>("Shader/Spine-SkeletonSpriteMaskable");
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
			ElevatorController.ElevatorWorkerController.CheckStartWork();
		}
		ElevatorController.ElevatorModel.isManagerActive = !isEmpty;
		base.MineController.UpdateIdleCash();
	}

	public override void SetRandomManager(ManagerArea area)
	{
		base.SetRandomManager(area);
		ElevatorController.OnChangeManager();
	}

	public override void SetManagerParam(ManagerEntity.Param param)
	{
		base.SetManagerParam(param);
		DataManager.Instance.CurrentMineSavegame.ElevatorManagerSavegame = ManagerSavegame;
	}

	public override void SetActiveEffect(EffectState effectState, bool isEmpty = false)
	{
		base.SetActiveEffect(effectState, isEmpty);
		switch (effectState)
		{
		case EffectState.Active:
			ElevatorController.OnActiveManager();
			break;
		case EffectState.Cooldown:
			ElevatorController.OnDeactiveManager();
			break;
		case EffectState.Ready:
			ElevatorController.OnDeactiveManager();
			break;
		}
	}
}
