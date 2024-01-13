public abstract class AbstractStatValue<T>
{
	public abstract int Level
	{
		get;
	}

	public abstract T Value
	{
		get;
	}

	public abstract T NextValue
	{
		get;
	}

	public abstract T MaxValue
	{
		get;
	}

	public abstract T ValueWithoutBonus
	{
		get;
	}

	public abstract bool IsMaxValue
	{
		get;
	}

	public abstract bool HasBonusValue
	{
		get;
	}

	public abstract T BonusValue
	{
		get;
	}

	public abstract T NextBonusValue
	{
		get;
	}

	public abstract T ValueAtNextLevel(int levelOffset);

	public abstract T BonusValueAtNextLevel(int levelOffset);
}
