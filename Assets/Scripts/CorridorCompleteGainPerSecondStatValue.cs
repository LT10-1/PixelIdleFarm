using Entities.Corridor;

public class CorridorCompleteGainPerSecondStatValue : AbstractCorridorStatValue<double>
{
	public override double Value => CalcValue(CorridorModel.NumberOfWorkersStat.Value, base.Worker.CapacityStat.Value, base.Worker.SecondsOneTurnStat.Value);

	public override double NextValue => ValueAtNextLevel(1);

	public override double ValueWithoutBonus => CalcValue(CorridorModel.NumberOfWorkersStat.ValueWithoutBonus, base.Worker.CapacityStat.ValueWithoutBonus, base.Worker.SecondsOneTurnStat.ValueWithoutBonus);

	public override double MaxValue => ValueAtNextLevel(Data.MaxCorridorLevel() - Level);

	public override bool IsMaxValue => Data.MaxCorridorLevel() == Level;

	public override bool HasBonusValue => CorridorModel.CapacityStat.HasBonusValue || CorridorModel.GainPerSecondStat.HasBonusValue || CorridorModel.WalkingSpeedStat.HasBonusValue;

	public override double BonusValue => Value - ValueWithoutBonus;

	public override double NextBonusValue => 0.0;

	public CorridorCompleteGainPerSecondStatValue(CorridorModel corridorModel, ICorridorData data, ICorridorWorkerData workerData)
		: base(corridorModel, data, workerData)
	{
	}

	public override double ValueAtNextLevel(int levelOffset)
	{
		if (Level + levelOffset > Data.MaxCorridorLevel())
		{
			return -1.0;
		}
		return CalcValue(CorridorModel.NumberOfWorkersStat.ValueAtNextLevel(levelOffset), base.Worker.CapacityStat.ValueAtNextLevel(levelOffset), base.Worker.SecondsOneTurnStat.ValueAtNextLevel(levelOffset));
	}

	public override double BonusValueAtNextLevel(int levelOffset)
	{
		return 0.0;
	}

	public static double CalcValue(int numberOfWorkers, double workerCapacity, double workerSecondsOneTurn)
	{
		return (double)numberOfWorkers * (workerCapacity / workerSecondsOneTurn);
	}
}
