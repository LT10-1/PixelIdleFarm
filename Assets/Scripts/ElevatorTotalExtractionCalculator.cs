public class ElevatorTotalExtractionCalculator : ITotalExtractionCalculator
{
	private readonly IElevatorData _data;

	private readonly IStatsIncreaseModel _statsIncreaseModel;

	public MineModel MineModel;

	public ElevatorModel ElevatorModel;

	public ElevatorTotalExtractionCalculator(MineModel mineModel, ElevatorModel elevatorModel, IElevatorData data, IStatsIncreaseModel statsIncreaseModel)
	{
		_data = data;
		_statsIncreaseModel = statsIncreaseModel;
		ElevatorModel = elevatorModel;
		MineModel = mineModel;
	}

	public double GetTotalExtration()
	{
		return GetTotalExtration(0);
	}

	public double GetTotalExtration(int levelOffset)
	{
		return (!ElevatorModel.isManagerActive) ? 0.0 : GetPossibleTotalExtration(levelOffset);
	}

	public double GetPossibleTotalExtration()
	{
		return GetPossibleTotalExtration(0);
	}

	public double GetPossibleTotalExtration(int levelOffset)
	{
		int num = MineModel.NumberActiveCorridor + 1;
		double num2 = (double)num / CalculateSpeed(levelOffset);
		double num3 = 2.0 * num2;
		double num4 = CalculateCapacity(levelOffset);
		double num5 = num4 / CalculateLoadingPerSecond(levelOffset);
		double num6 = 2.0 * num5;
		double num7 = num6 + num3;
		return num4 / num7;
	}

	private double CalculateSpeed(int levelOffset)
	{
		return _data.SpeedInTiersPerSecond(ElevatorModel.Level + levelOffset);
	}

	private double CalculateCapacity(int levelOffset)
	{
		return _data.Capacity(ElevatorModel.Level + levelOffset, _statsIncreaseModel);
	}

	private double CalculateLoadingPerSecond(int levelOffset)
	{
		return _data.LoadingPerSecond(ElevatorModel.Level + levelOffset, _statsIncreaseModel);
	}
}
