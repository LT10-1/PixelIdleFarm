namespace Entities.Corridor.StatValue
{
	public class CorridorGainPerSecondStatValue : AbstractCorridorStatValue<double>
	{
		public override double Value => base.Worker.GainPerSecondStat.Value;

		public override double NextValue => base.Worker.GainPerSecondStat.NextValue;

		public override double ValueWithoutBonus => base.Worker.GainPerSecondStat.ValueWithoutBonus;

		public override double MaxValue => base.Worker.GainPerSecondStat.MaxValue;

		public override bool IsMaxValue => base.Worker.GainPerSecondStat.IsMaxValue;

		public override bool HasBonusValue => base.Worker.GainPerSecondStat.HasBonusValue;

		public override double BonusValue => base.Worker.GainPerSecondStat.BonusValue;

		public override double NextBonusValue => base.Worker.GainPerSecondStat.NextBonusValue;

		public CorridorGainPerSecondStatValue(CorridorModel corridorModel, ICorridorData data, ICorridorWorkerData workerData)
			: base(corridorModel, data, workerData)
		{
		}

		public override double ValueAtNextLevel(int levelOffset)
		{
			return base.Worker.GainPerSecondStat.ValueAtNextLevel(levelOffset);
		}

		public override double BonusValueAtNextLevel(int levelOffset)
		{
			return base.Worker.GainPerSecondStat.BonusValueAtNextLevel(levelOffset);
		}
	}
}
