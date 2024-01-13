public interface ICorridorWorkerData
{
	double GainPerSecond(int corridorLevel, int tier, IStatsIncreaseModel corridorModelStatsIncreaseModel);

	double Capacity(int corridorLevel, int tier, IStatsIncreaseModel corridorModelStatsIncreaseModel);

	int WalkingSpeedPerSecond(int corridorLevel, int tier);
}
