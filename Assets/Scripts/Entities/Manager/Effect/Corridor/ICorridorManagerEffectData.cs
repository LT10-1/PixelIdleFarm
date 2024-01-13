namespace Entities.Manager.Effect.Corridor
{
	public interface ICorridorManagerEffectData
	{
		double GainPerSecondFactor(int effectId, int managerId);

		double UpgradeCostReductionFactor(int effectId, int managerId);

		double WalkingSpeedFactor(int effectId, int managerId);

		double CapacityFactor(int effectId, int managerId);
	}
}
