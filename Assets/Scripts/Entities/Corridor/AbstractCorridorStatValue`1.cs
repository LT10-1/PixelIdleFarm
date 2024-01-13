using Entities.Manager.Effect.Corridor;
using System.Linq;

namespace Entities.Corridor
{
	public abstract class AbstractCorridorStatValue<T> : AbstractStatValue<T>
	{
		protected readonly ICorridorData Data;

		protected readonly ICorridorWorkerData WorkerData;

		protected readonly CorridorModel CorridorModel;

		public CorridorBonusContainer BonusContainer => CorridorModel.BonusContainer;

		public override int Level => CorridorModel.Level;

		public int Tier => CorridorModel.Tier;

		protected CorridorWorkerModel Worker => CorridorModel.Workers.First();

		protected AbstractCorridorStatValue(CorridorModel corridorModel, ICorridorData data, ICorridorWorkerData workerData)
		{
			CorridorModel = corridorModel;
			Data = data;
			WorkerData = workerData;
		}
	}
}
