using System.Collections.Generic;

public class CorridorTotalExtractionCalculator : ITotalExtractionCalculator
{
	private readonly ICorridorWorkerData _workerData;

	private readonly ICorridorData _data;

	public Dictionary<int, CorridorModel> CorridorModels;

	public CorridorTotalExtractionCalculator(Dictionary<int, CorridorModel> corridorModelDictionary, ICorridorData data, ICorridorWorkerData workerData)
	{
		_workerData = workerData;
		_data = data;
		CorridorModels = corridorModelDictionary;
	}

	public double GetTotalExtration()
	{
		return GetTotalExtration(ignoreManager: false);
	}

	public double GetTotalExtration(bool ignoreManager)
	{
		double num = 0.0;
		foreach (KeyValuePair<int, CorridorModel> corridorModel in CorridorModels)
		{
			if (ignoreManager || corridorModel.Value.isManagerActive)
			{
				num += CorridorGainPerSercond(corridorModel.Key);
			}
		}
		return num;
	}

	public double CorridorGainPerSercond(int tier, int levelOffset = 0)
	{
		return CalculateCorridorCompleteGainPerSecond(CorridorModels[tier], levelOffset);
	}

	public double GetPossibleTotalExtration()
	{
		return GetTotalExtration();
	}

	private double CalculateCorridorCompleteGainPerSecond(CorridorModel corridor, int levelOffset)
	{
		return (double)corridor.NumberOfWorkersStat.ValueAtNextLevel(levelOffset) * (CalculateWorkerCapacity(corridor, levelOffset) / CalculateWorkerSecondsOneTurn(corridor, levelOffset));
	}

	private double CalculateWorkerCapacity(CorridorModel corridor, int levelOffset)
	{
		return _workerData.Capacity(corridor.Level + levelOffset, corridor.Tier, corridor.StatsIncreaseModel);
	}

	private double CalculateWorkerSecondsOneTurn(CorridorModel corridor, int levelOffset)
	{
		return 2.0 * CalculateSecondsOneWay(corridor, levelOffset) + CalculateSecondsTillCapacityReached(corridor, levelOffset);
	}

	private double CalculateSecondsOneWay(CorridorModel corridor, int levelOffset)
	{
		return CalculateCorridorLength(corridor, levelOffset) / CalculateWalkingSpeedPerSecond(corridor, levelOffset);
	}

	private double CalculateCorridorLength(CorridorModel corridor, int levelOffset)
	{
		return (double)_data.StartLength() + CalculateLengthWithLog(corridor, levelOffset);
	}

	private double CalculateLengthWithLog(CorridorModel corridor, int levelOffset)
	{
		return (double)_data.MaxLength() * CalculateQuotientMined(corridor, levelOffset);
	}

	private double CalculateQuotientMined(CorridorModel corridor, int levelOffset)
	{
		return (double)(corridor.Level + levelOffset - 1) / (double)_data.MaxCorridorLevel();
	}

	private double CalculateWalkingSpeedPerSecond(CorridorModel corridor, int levelOffset)
	{
		return _workerData.WalkingSpeedPerSecond(corridor.Level + levelOffset, corridor.Tier);
	}

	private double CalculateSecondsTillCapacityReached(CorridorModel corridor, int levelOffset)
	{
		return CalculateWorkerCapacity(corridor, levelOffset) / CalculateWorkerGainPerSecond(corridor, levelOffset);
	}

	private double CalculateWorkerGainPerSecond(CorridorModel corridor, int levelOffset)
	{
		return _workerData.GainPerSecond(corridor.Level + levelOffset, corridor.Tier, corridor.StatsIncreaseModel);
	}
}
