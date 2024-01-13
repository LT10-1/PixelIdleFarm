public interface IGroundWorkerData
{
	double Capacity(int groundLevel, IStatsIncreaseModel statsIncreaseModel);

	int WalkingSpeedPerSecond(int groundLevel);

	double LoadingPerSecond(int groundLevel, IStatsIncreaseModel statsIncreaseModel);
}
