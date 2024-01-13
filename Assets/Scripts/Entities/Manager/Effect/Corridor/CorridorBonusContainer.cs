namespace Entities.Manager.Effect.Corridor
{
	public class CorridorBonusContainer : ISkilledBonusContainer
	{
		public static readonly CorridorBonusContainer Empty = new CorridorBonusContainer(-1, -1);

		private readonly ICorridorManagerEffectData _effectData;

		private readonly int _managerId;

		public int EffectID
		{
			get;
			private set;
		}

		public double GainPerSecondFactor => _effectData.GainPerSecondFactor(EffectID, _managerId);

		public double WalkingSpeedFactor => _effectData.WalkingSpeedFactor(EffectID, _managerId);

		public double UpgradeCostFactor => _effectData.UpgradeCostReductionFactor(EffectID, _managerId);

		public double CapacityFactor => _effectData.CapacityFactor(EffectID, _managerId);

		public double TotalGainPerSecondFactor => GainPerSecondFactor * CorridorManagerSkillEffects.GainPerSecondSkillFactor;

		public double TotalWalkingSpeedFactor => WalkingSpeedFactor * CorridorManagerSkillEffects.WalkingSpeedSkillFactor;

		public double TotalUpgradeCostFactor => UpgradeCostFactor * CorridorManagerSkillEffects.UpgradeCostSkillFactor;

		public double TotalCapacityFactor => CapacityFactor * CorridorManagerSkillEffects.CorridorWorkerCapacitySkillFactor;

		public CorridorBonusContainer(int effectId, int managerId)
		{
			EffectID = effectId;
			_managerId = managerId;
			_effectData = CorridorManagerEffectImporter.Instance;
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

		private double GetBonusGainPerSecond(double currentLoadingPerSecond)
		{
			return currentLoadingPerSecond * GainPerSecondFactor - currentLoadingPerSecond;
		}

		public double GetTotalBonusUpgradeCost(double upgradeCost)
		{
			return GetBonusUpgradeCost(upgradeCost) * CorridorManagerSkillEffects.UpgradeCostSkillFactor;
		}

		public double GetTotalBonusCapacity(double currentCapacity)
		{
			return GetBonusCapacity(currentCapacity) * CorridorManagerSkillEffects.CapacitySkillFactor;
		}

		public double GetTotalBonusWalkingSpeed(double currentWalkingspeed)
		{
			return GetBonusWalkingSpeed(currentWalkingspeed) * CorridorManagerSkillEffects.WalkingSpeedSkillFactor;
		}

		public double GetTotalBonusGainPerSecond(double currentLoadingPerSecond)
		{
			return GetBonusGainPerSecond(currentLoadingPerSecond) * CorridorManagerSkillEffects.GainPerSecondSkillFactor;
		}
	}
}
