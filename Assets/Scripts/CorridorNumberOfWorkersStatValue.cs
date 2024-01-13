using Entities.Corridor;

public class CorridorNumberOfWorkersStatValue : AbstractCorridorStatValue<int>
{
	public override int Value => ValueWithoutBonus;

	public override int NextValue => ValueAtNextLevel(1);

	public override int ValueWithoutBonus => Data.NumberOfWorkers(Level, base.Tier);

	public override int MaxValue => ValueAtNextLevel(Data.MaxCorridorLevel() - Level);

	public override bool IsMaxValue => Data.MaxCorridorLevel() == Level;

	public override bool HasBonusValue => false;

	public override int BonusValue => 0;

	public override int NextBonusValue => 0;

	public CorridorNumberOfWorkersStatValue(CorridorModel corridorModel, ICorridorData data, ICorridorWorkerData workerData)
		: base(corridorModel, data, workerData)
	{
	}

	public override int ValueAtNextLevel(int levelOffset)
	{
		if (Level + levelOffset > Data.MaxCorridorLevel())
		{
			return -1;
		}
		return Data.NumberOfWorkers(Level + levelOffset, base.Tier);
	}

	public override int BonusValueAtNextLevel(int levelOffset)
	{
		return 0;
	}
}
