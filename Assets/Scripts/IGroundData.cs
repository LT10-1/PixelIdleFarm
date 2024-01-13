public interface IGroundData : IUpgradable
{
	double Cost(int groundLevel);

	int NumberOfWorkers(int groundLevel);

	int MaxGroundLevel();

	int GroundLength();

	bool IsBigUpgrade(int groundLevel);

	int NextBigUpgrade(int groundLevel);

	int LastBigUpgrade(int groundLevel);

	double SuperCashGain(int groundLevel);
}
