public interface IUpgradable
{
	int MaxLevel
	{
		get;
	}

	double Cost(int level, int tier);
}
