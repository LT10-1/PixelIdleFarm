public class DATA_CONST
{
	public static readonly ManagerEffect[] CORRIDOR_EFFECTS = new ManagerEffect[3]
	{
		ManagerEffect.CorridorWorkerGainMultiplier,
		ManagerEffect.CorridorWorkerWalkingSpeedMultiplier,
		ManagerEffect.CorridorUpgradeCostReduction
	};

	public static readonly ManagerEffect[] ELEVATOR_EFFECTS = new ManagerEffect[4]
	{
		ManagerEffect.ElevatorTiersPerSecondsMultiplier,
		ManagerEffect.ElevatorCapacityMultiplier,
		ManagerEffect.ElevatorLoadingSpeedPerSecondMultiplier,
		ManagerEffect.ElevatorUpgradeCostReduction
	};

	public static readonly ManagerEffect[] GROUND_EFFECTS = new ManagerEffect[4]
	{
		ManagerEffect.GroundWalkingSpeedBoost,
		ManagerEffect.GroundUpgradeCostReduction,
		ManagerEffect.GroundLoadingSpeedPerSecondMultiplier,
		ManagerEffect.GroundWorkerCapacityMultiplier
	};

	public static readonly UpgradeType[] CORRIDOR_UPGRADE_TYPE = new UpgradeType[5]
	{
		UpgradeType.TotalExtraction,
		UpgradeType.Miners,
		UpgradeType.Walkingspeed,
		UpgradeType.MiningSpeed,
		UpgradeType.WorkerCapacity
	};

	public static readonly UpgradeType[] ELEVATOR_UPGRADE_TYPE = new UpgradeType[4]
	{
		UpgradeType.TotalTransportation,
		UpgradeType.Load,
		UpgradeType.MovementSpeed,
		UpgradeType.LoadingSpeed
	};

	public static readonly UpgradeType[] GROUND_UPGRADE_TYPE = new UpgradeType[5]
	{
		UpgradeType.TotalTransportation,
		UpgradeType.Transporters,
		UpgradeType.Loadpertrans,
		UpgradeType.LoadingSpeed,
		UpgradeType.Walkingspeed
	};
}
