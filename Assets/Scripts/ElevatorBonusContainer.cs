public class ElevatorBonusContainer : ISkilledBonusContainer
{
	public static ElevatorBonusContainer Empty = new ElevatorBonusContainer(-1, -1);

	public IElevatorManagerEffectData EffectData;

	private readonly int _managerId;

	public int EffectID
	{
		get;
		private set;
	}

	public double LoadingPerSecondFactor => EffectData.LoadingPerSecondFactor(EffectID, _managerId);

	public double CapacityFactor => EffectData.CapacityFactor(EffectID, _managerId);

	public double UpgradeCostFactor => EffectData.UpgradeCostReductionFactor(EffectID, _managerId);

	public double TiersPerSecondFactor => EffectData.TiersPerSecondFactor(EffectID, _managerId);

	public double TotalUpgradeCostFactor => UpgradeCostFactor * ElevatorManagerSkillEffects.UpgradeCostSkillFactor;

	public double TotalLoadingPerSecondFactor => LoadingPerSecondFactor * ElevatorManagerSkillEffects.LoadingPerSecondSkillFactor;

	public double TotalCapacityFactor => CapacityFactor * ElevatorManagerSkillEffects.CapacitySkillFactor;

	public double TotalTiersPerSecondFactor => TiersPerSecondFactor * ElevatorManagerSkillEffects.TiersPerSecondsSkillFactor;

	public ElevatorBonusContainer(int effectId, int managerId)
	{
		EffectID = effectId;
		_managerId = managerId;
		EffectData = ElevatorManagerEffectImporter.Instance;
	}

	private double GetBonusLoadingPerSecond(double currentLoadingPerSecond)
	{
		return currentLoadingPerSecond * LoadingPerSecondFactor - currentLoadingPerSecond;
	}

	private double GetBonusCapacity(double currentCapacity)
	{
		return currentCapacity * CapacityFactor - currentCapacity;
	}

	private double GetBonusUpgradeCost(double upgradeCost)
	{
		return upgradeCost * UpgradeCostFactor - upgradeCost;
	}

	private double GetTiersPerSecondBonus(double currentTiersPerSecond)
	{
		return currentTiersPerSecond * TiersPerSecondFactor - currentTiersPerSecond;
	}

	public double GetTotalBonusLoadingPerSecond(double currentLoadingPerSecond)
	{
		return GetBonusLoadingPerSecond(currentLoadingPerSecond) * ElevatorManagerSkillEffects.LoadingPerSecondSkillFactor;
	}

	public double GetTotalBonusCapacity(double currentCapacity)
	{
		return GetBonusCapacity(currentCapacity) * ElevatorManagerSkillEffects.CapacitySkillFactor;
	}

	public double GetTotalBonusUpgradeCost(double upgradeCost)
	{
		return GetBonusUpgradeCost(upgradeCost) * ElevatorManagerSkillEffects.UpgradeCostSkillFactor;
	}

	public double GetTotalTiersPerSecondBonus(double currentTiersPerSecond)
	{
		return GetTiersPerSecondBonus(currentTiersPerSecond) * ElevatorManagerSkillEffects.TiersPerSecondsSkillFactor;
	}
}
