using Entities.Corridor;

public abstract class AbstractCorridorWorkerStatValue<T> : AbstractCorridorStatValue<T>
{
	protected readonly CorridorWorkerModel CorridorWorkerModel;

	protected AbstractCorridorWorkerStatValue(CorridorModel corridorModel, CorridorWorkerModel corridorWorkerModel, ICorridorData data, ICorridorWorkerData workerData)
		: base(corridorModel, data, workerData)
	{
		CorridorWorkerModel = corridorWorkerModel;
	}
}
