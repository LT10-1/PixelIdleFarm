public interface ICorridorData : IUpgradable
{
	int NumberOfWorkers(int corridorLevel, int tier);

	int StartLength();

	int MaxLength();

	int MaxCorridorLevel();

	bool IsBigUpgrade(int corridorLevel, int tier);

	int NextBigUpgrade(int corridorLevel, int tier);

	int LastBigUpgrade(int corridorLevel, int tier);

	double SuperCashGain(int tier, int level);
}
