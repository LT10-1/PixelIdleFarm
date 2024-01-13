using Entities.Manager.Effect.Corridor;

public class CorridorManagerEffectImporter : ICorridorManagerEffectData
{
	private static CorridorManagerEffectImporter _instance;

	public static CorridorManagerEffectImporter Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new CorridorManagerEffectImporter();
			}
			return _instance;
		}
	}

	public double GainPerSecondFactor(int effectId, int managerId)
	{
		if (effectId == 8)
		{
			return DataManager.Instance.ManagerEntity.Params[managerId].ValueX;
		}
		return 1.0;
	}

	public double UpgradeCostReductionFactor(int effectId, int managerId)
	{
		if (effectId == 10)
		{
			return DataManager.Instance.ManagerEntity.Params[managerId].ValueX;
		}
		return 1.0;
	}

	public double WalkingSpeedFactor(int effectId, int managerId)
	{
		if (effectId == 9)
		{
			return DataManager.Instance.ManagerEntity.Params[managerId].ValueX;
		}
		return 1.0;
	}

	public double CapacityFactor(int effectId, int managerId)
	{
		return 1.0;
	}
}
