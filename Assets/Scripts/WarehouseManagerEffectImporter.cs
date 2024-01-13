public class WarehouseManagerEffectImporter : IGroundManagerEffectData
{
	private static WarehouseManagerEffectImporter _instance;

	public static WarehouseManagerEffectImporter Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new WarehouseManagerEffectImporter();
			}
			return _instance;
		}
	}

	public double WalkingSpeedFactor(int effectId, int managerId)
	{
		if (effectId == 1)
		{
			return DataManager.Instance.ManagerEntity.Params[managerId].ValueX;
		}
		return 1.0;
	}

	public double UpgradeCostReductionFactor(int effectId, int managerId)
	{
		if (effectId == 3)
		{
			return DataManager.Instance.ManagerEntity.Params[managerId].ValueX;
		}
		return 1.0;
	}

	public double LoadingPerSecondFactor(int effectId, int managerId)
	{
		if (effectId == 4)
		{
			return DataManager.Instance.ManagerEntity.Params[managerId].ValueX;
		}
		return 1.0;
	}

	public double CapacityFactor(int effectId, int managerId)
	{
		if (effectId == 5)
		{
			return DataManager.Instance.ManagerEntity.Params[managerId].ValueX;
		}
		return 1.0;
	}
}
