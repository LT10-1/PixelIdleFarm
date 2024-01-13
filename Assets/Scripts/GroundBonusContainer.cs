using UnityEngine;

public class GroundBonusContainer : ISkilledBonusContainer
{
	public static GroundBonusContainer Empty = new GroundBonusContainer(-1, -1);

	public IGroundManagerEffectData EffectData;

	private readonly int _managerId;

	public int EffectID
	{
		get;
		private set;
	}

	private double RandomValue => Random.value;

	public double LoadingPerSecondFactor => EffectData.LoadingPerSecondFactor(EffectID, _managerId);

	public double WalkingSpeedFactor => EffectData.WalkingSpeedFactor(EffectID, _managerId);

	public double UpgradeCostFactor => EffectData.UpgradeCostReductionFactor(EffectID, _managerId);

	public double CapacityFactor => EffectData.CapacityFactor(EffectID, _managerId);

	public double TotalUpgradeCostFactor => UpgradeCostFactor * GroundManagerSkillEffects.UpgradeCostSkillFactor;

	public double TotalLoadingPerSecondFactor => LoadingPerSecondFactor * GroundManagerSkillEffects.LoadingPerSecondSkillFactor;

	public double TotalWalkingSpeedFactor => WalkingSpeedFactor * GroundManagerSkillEffects.WalkingSpeedSkillFactor;

	public double TotalCapacityFactor => CapacityFactor * GroundManagerSkillEffects.CapacitySkillFactor;

	public GroundBonusContainer(int effectId, int managerId)
	{
		EffectID = effectId;
		_managerId = managerId;
		EffectData = WarehouseManagerEffectImporter.Instance;
	}

	private double GetBonusUpgradeCost(double upgradeCost)
	{
		return upgradeCost * UpgradeCostFactor - upgradeCost;
	}

	private double GetBonusCapacity(double currentCapacity)
	{
		return currentCapacity * CapacityFactor - currentCapacity;
	}

	private double GetBonusWalkingSpeed(double currentWalkingspeed)
	{
		return currentWalkingspeed * WalkingSpeedFactor - currentWalkingspeed;
	}

	private double GetBonusLoadingPerSecond(double currentLoadingPerSecond)
	{
		return currentLoadingPerSecond * LoadingPerSecondFactor - currentLoadingPerSecond;
	}

	public double GetTotalBonusUpgradeCost(double upgradeCost)
	{
		return GetBonusUpgradeCost(upgradeCost) * GroundManagerSkillEffects.UpgradeCostSkillFactor;
	}

	public double GetTotalBonusCapacity(double currentCapacity)
	{
		return GetBonusCapacity(currentCapacity) * GroundManagerSkillEffects.CapacitySkillFactor;
	}

	public double GetTotalBonusWalkingSpeed(double currentWalkingspeed)
	{
		return GetBonusWalkingSpeed(currentWalkingspeed) * GroundManagerSkillEffects.WalkingSpeedSkillFactor;
	}

	public double GetTotalBonusLoadingPerSecond(double currentLoadingPerSecond)
	{
		return GetBonusLoadingPerSecond(currentLoadingPerSecond) * GroundManagerSkillEffects.LoadingPerSecondSkillFactor;
	}
}
