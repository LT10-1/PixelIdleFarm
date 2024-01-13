public class GroundTotalExtractionCalculator : ITotalExtractionCalculator
{
	private readonly IGroundData _data;

	private readonly IGroundWorkerData _workerData;

	private readonly IStatsIncreaseModel _statsIncreaseModel;

	public GroundModel GroundModel;

	public GroundTotalExtractionCalculator(GroundModel groundModel, IGroundData data, IGroundWorkerData workerData, IStatsIncreaseModel statsIncreaseModel)
	{
		_data = data;
		_workerData = workerData;
		GroundModel = groundModel;
		_statsIncreaseModel = statsIncreaseModel;
	}

	public double GetTotalExtration()
	{
		return GetTotalExtration(0);
	}

	public double GetTotalExtration(int levelOffset)
	{
		return (!GroundModel.isManagerActive) ? 0.0 : GetPossibleTotalExtration(levelOffset);
	}

	public double GetPossibleTotalExtration()
	{
		return GetPossibleTotalExtration(0);
	}

	public double GetPossibleTotalExtration(int levelOffset)
	{
		return CalculateNumberOfWorkers(levelOffset) * CalculateWorkerTransportingPerSecond(levelOffset);
	}

	private double CalculateNumberOfWorkers(int levelOffset)
	{
		return _data.NumberOfWorkers(GroundModel.Level);
	}

	private double CalculateWorkerTransportingPerSecond(int levelOffset)
	{
		double num = 2.0 * CalculateWorkerSecondsOneWay(levelOffset);
		double num2 = CalculateWorkerCapacity(levelOffset);
		double num3 = 2.0 * num2 / CalculateWorkerLoadingPerSecond(levelOffset);
		double num4 = num + num3;
		return num2 / num4;
	}

	private double CalculateWorkerSecondsOneWay(int levelOffset)
	{
		return CalculateGroundLength() / CalculateWalkingSpeedPerSecond(levelOffset);
	}

	private double CalculateWorkerCapacity(int levelOffset)
	{
		return _workerData.Capacity(GroundModel.Level + levelOffset, _statsIncreaseModel);
	}

	private double CalculateWorkerLoadingPerSecond(int levelOffset)
	{
		return _workerData.LoadingPerSecond(GroundModel.Level + levelOffset, _statsIncreaseModel);
	}

	private double CalculateGroundLength()
	{
		return _data.GroundLength();
	}

	private double CalculateWalkingSpeedPerSecond(int levelOffset)
	{
		return _workerData.WalkingSpeedPerSecond(GroundModel.Level + levelOffset);
	}
}
