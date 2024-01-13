public abstract class AbstractGroundWorkerStatValue<T> : AbstractGroundStatValue<T>
{
	protected readonly GroundWorkerModel GroundWorkerModel;

	protected AbstractGroundWorkerStatValue(GroundModel groundModel, GroundWorkerModel groundWorkerModel, IGroundData groundData, IGroundWorkerData groundWorkerData)
		: base(groundModel, groundData, groundWorkerData)
	{
		GroundWorkerModel = groundWorkerModel;
	}
}
