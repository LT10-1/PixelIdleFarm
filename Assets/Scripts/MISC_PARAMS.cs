public class MISC_PARAMS
{
	public const int CORRIDOR_START_LENGTH = 2;

	public const int CORRIDOR_MAX_LENGTH = 8;

	public const int CORRIDOR_MAX_LEVEL = 800;

	public const int CORRIDOR_MAX_TIER = 30;

	public const int ELEVATOR_MAX_LEVEL = 2400;

	public const int WAREHOUSE_MAX_LEVEL = 2400;

	public const int GROUND_LENGTH = 10;

	public const double IDLE_CASH_GAIN_FACTOR = 0.1;

	public static readonly int[] MILE_STONE_DELAY_TIME = new int[30]
	{
		0,
		0,
		0,
		0,
		0,
		1,
		0,
		0,
		0,
		0,
		6,
		0,
		0,
		0,
		0,
		24,
		0,
		0,
		0,
		0,
		72,
		0,
		0,
		0,
		0,
		120,
		0,
		0,
		0,
		0
	};

	public static readonly double[] MILESTONE_BREAK_COST = new double[30]
	{
		0.0,
		0.0,
		0.0,
		0.0,
		0.0,
		240000000.0,
		0.0,
		0.0,
		0.0,
		0.0,
		1.53E+16,
		0.0,
		0.0,
		0.0,
		0.0,
		3.68E+28,
		0.0,
		0.0,
		0.0,
		0.0,
		1.76E+37,
		0.0,
		0.0,
		0.0,
		0.0,
		1.47E+53,
		0.0,
		0.0,
		0.0,
		0.0
	};

	public static readonly double[] CORRIDOR_MANAGER_COST_INIT = new double[5]
	{
		50.0,
		200.0,
		400.0,
		800.0,
		3200.0
	};

	public static readonly double[] ELEVATOR_MANAGER_COST_INIT = new double[5]
	{
		100.0,
		500.0,
		2500.0,
		12500.0,
		100000.0
	};

	public static readonly double[] GROUND_MANAGER_COST_INIT = new double[5]
	{
		100.0,
		500.0,
		2500.0,
		12500.0,
		100000.0
	};

	public static readonly int[] CORRIDOR_MANAGER_ORDER = new int[10]
	{
		21,
		27,
		25,
		24,
		29,
		21,
		22,
		27,
		24,
		28
	};

	public static readonly int[] ELEVATOR_MANAGER_ORDER = new int[10]
	{
		45,
		32,
		36,
		31,
		45,
		37,
		30,
		33,
		46,
		38
	};

	public static readonly int[] GROUND_MANAGER_ORDER = new int[10]
	{
		1,
		9,
		6,
		2,
		12,
		6,
		10,
		8,
		9,
		0
	};

	public const double CORRIDOR_MANAGER_COST_FACTOR = 4.5;

	public const double ELEVATOR_MANAGER_COST_FACTOR = 8.0;

	public const double GROUND_MANAGER_COST_FACTOR = 8.0;

	public const double SKILL_POINT_COST_INIT = 8.47E+15;

	public const double SKILL_POINT_COST_FACTOR = 1.7;

	public const int AD_MILESTONE_MAX = 2;

	public const int AD_MILESTONE_COOLDOWN = 3600;

	public const int EXPEDITION_BOOST_COOLDOWN = 14400;

	public const long EXPEDITION_BOOST_NORMAL = 9000000000L;

	public const long EXPEDITION_BOOST_WITH_AD = 18000000000L;

	public const float MINE_BOOST_X2_MAX_DURATION = 32f;

	public const float MINE_BOOST_x2_DURATION_EACH = 4f;

	public const double BOOST_PER_FRIEND = 5.0;

	public const double PRICE_CONTINENT_SAND = 2.45E+36;

	public const double PRICE_CONTINENT_SAKURA = 2.25E+37;

	public const double PRICE_UNLOCK_LABORATORY = 2.3E+18;

	public const double RESET_SKILL_COST = 400.0;

	public const double PRICE_SUPER_CASH_SKILL_POINT = 200.0;

	public const int MAX_OFFLINE_CHEST_DATE = 5;

	public const double DAILY_CHEST_RARE_CHANCE = 0.1;

	public const double DAILY_CHEST_EPIC_CHANCE = 0.01;

	public static double[] CHEST_WATCH_AD_RECEIVE_CHANCE = new double[3]
	{
		0.5,
		0.35,
		0.2
	};

	public const int X2_IDLE_CASH_PACKAGE_PRICE = 7;
}
