public interface IElevatorManagerEffectData
{
	double TiersPerSecondFactor(int effectId, int managerId);

	double UpgradeCostReductionFactor(int effectId, int managerId);

	double LoadingPerSecondFactor(int effectId, int managerId);

	double CapacityFactor(int effectId, int managerId);
}
