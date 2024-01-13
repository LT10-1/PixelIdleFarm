public interface IElevatorData : IUpgradable
{
	double Cost(int elevatorLevel);

	double Capacity(int elevatorLevel, IStatsIncreaseModel statsIncreaseModel);

	double SpeedInTiersPerSecond(int elevatorLevel);

	double LoadingPerSecond(int elevatorLevel, IStatsIncreaseModel statsIncreaseModel);

	bool IsBigUpgrade(int elevatorLevel);

	int NextBigUpgrade(int elevatorLevel);

	int LastBigUpgrade(int elevatorLevel);

	double SuperCashGain(int elevatorLevel);

	int MaxElevatorLevel();
}
