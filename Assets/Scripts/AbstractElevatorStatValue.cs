public abstract class AbstractElevatorStatValue<T> : AbstractStatValue<T>
{
	public IElevatorData ElevatorData;

	protected readonly ElevatorModel ElevatorModel;

	public ElevatorBonusContainer BonusContainer => ElevatorModel.BonusContainer;

	public override int Level => ElevatorModel.Level;

	protected AbstractElevatorStatValue(ElevatorModel elevatorModel, IElevatorData elevatorData)
	{
		ElevatorModel = elevatorModel;
		ElevatorData = elevatorData;
	}
}
