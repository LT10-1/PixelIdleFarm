using System.Linq;

public abstract class AbstractGroundStatValue<T> : AbstractStatValue<T>
{
	protected IGroundData GroundData;

	protected IGroundWorkerData WorkerData;

	protected readonly GroundModel GroundModel;

	public GroundBonusContainer BonusContainer => GroundModel.BonusContainer;

	public override int Level => GroundModel.Level;

	protected GroundWorkerModel Worker => GroundModel.GroundWorkers.First();

	protected AbstractGroundStatValue(GroundModel groundModel, IGroundData groundData, IGroundWorkerData groundWorkerData)
	{
		GroundModel = groundModel;
		GroundData = groundData;
		WorkerData = groundWorkerData;
	}
}
