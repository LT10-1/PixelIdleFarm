public class GroundNumberOfWorkersStatValue : AbstractGroundStatValue<int>
{
	public override int Value => ValueWithoutBonus;

	public override int NextValue => ValueAtNextLevel(1);

	public override int ValueWithoutBonus => GroundData.NumberOfWorkers(Level);

	public override int MaxValue => ValueAtNextLevel(GroundData.MaxGroundLevel() - Level);

	public override bool IsMaxValue => GroundData.MaxGroundLevel() == Level;

	public override bool HasBonusValue => false;

	public override int BonusValue => 0;

	public override int NextBonusValue => 0;

	public GroundNumberOfWorkersStatValue(GroundModel groundModel, IGroundData groundData, IGroundWorkerData groundWorkerData)
		: base(groundModel, groundData, groundWorkerData)
	{
	}

	public override int ValueAtNextLevel(int levelOffset)
	{
		if (Level + levelOffset > GroundData.MaxGroundLevel())
		{
			return -1;
		}
		return GroundData.NumberOfWorkers(Level + levelOffset);
	}

	public override int BonusValueAtNextLevel(int levelOffset)
	{
		return 0;
	}
}
