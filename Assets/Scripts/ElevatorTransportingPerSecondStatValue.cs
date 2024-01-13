public class ElevatorTransportingPerSecondStatValue : AbstractElevatorStatValue<double>
{
	public int CorridorCount => GameController.Instance.MineController.CorridorController.NumberActiveCorridor;

	public override double Value => CalcValue(CorridorCount, ElevatorModel.SpeedStat.Value, ElevatorModel.CapacityStat.Value, ElevatorModel.LoadingPerSecondStat.Value);

	public override double NextValue => ValueAtNextLevel(1);

	public override double ValueWithoutBonus => CalcValue(CorridorCount, ElevatorModel.SpeedStat.ValueWithoutBonus, ElevatorModel.CapacityStat.ValueWithoutBonus, ElevatorModel.LoadingPerSecondStat.ValueWithoutBonus);

	public override double MaxValue => ValueAtNextLevel(ElevatorData.MaxElevatorLevel() - Level);

	public override bool IsMaxValue => ElevatorData.MaxElevatorLevel() == Level;

	public override bool HasBonusValue => ElevatorModel.CapacityStat.HasBonusValue || ElevatorModel.LoadingPerSecondStat.HasBonusValue || ElevatorModel.SpeedStat.HasBonusValue;

	public override double BonusValue => Value - ValueWithoutBonus;

	public override double NextBonusValue => 0.0;

	public ElevatorTransportingPerSecondStatValue(ElevatorModel elevatorModel, IElevatorData elevatorData)
		: base(elevatorModel, elevatorData)
	{
	}

	public override double ValueAtNextLevel(int levelOffset)
	{
		if (Level + levelOffset > ElevatorData.MaxElevatorLevel())
		{
			return -1.0;
		}
		return CalcValue(CorridorCount, ElevatorModel.SpeedStat.ValueAtNextLevel(levelOffset), ElevatorModel.CapacityStat.ValueAtNextLevel(levelOffset), ElevatorModel.LoadingPerSecondStat.ValueAtNextLevel(levelOffset));
	}

	public override double BonusValueAtNextLevel(int levelOffset)
	{
		return 0.0;
	}

	public static double CalcValue(int corridorCount, double speed, double capacity, double loadingPerSecond)
	{
		int num = corridorCount + 1;
		double num2 = (double)num / speed;
		double num3 = 2.0 * num2;
		double num4 = capacity / loadingPerSecond;
		double num5 = 2.0 * num4;
		double num6 = num5 + num3;
		return capacity / num6;
	}
}
