public class ElevatorManagerEffectImporter : IElevatorManagerEffectData
{
	private static ElevatorManagerEffectImporter _instance;

	public static ElevatorManagerEffectImporter Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new ElevatorManagerEffectImporter();
			}
			return _instance;
		}
	}

	public double TiersPerSecondFactor(int effectId, int managerId)
	{
		if (effectId == 11)
		{
			return DataManager.Instance.ManagerEntity.Params[managerId].ValueX;
		}
		return 1.0;
	}

	public double UpgradeCostReductionFactor(int effectId, int managerId)
	{
		if (effectId == 16)
		{
			return DataManager.Instance.ManagerEntity.Params[managerId].ValueX;
		}
		return 1.0;
	}

	public double LoadingPerSecondFactor(int effectId, int managerId)
	{
		if (effectId == 13)
		{
			return DataManager.Instance.ManagerEntity.Params[managerId].ValueX;
		}
		return 1.0;
	}

	public double CapacityFactor(int effectId, int managerId)
	{
		if (effectId == 12)
		{
			return DataManager.Instance.ManagerEntity.Params[managerId].ValueX;
		}
		return 1.0;
	}
}
