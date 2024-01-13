public class ANIMATION
{
	public static string[][] RESOURCE_SKIN = new string[3][]
	{
		new string[5]
		{
			"coal",
			"gold",
			"ruby",
			"diamond",
			"emerald"
		},
		new string[5]
		{
			"sunstone",
			"amber",
			"platinum",
			"obidian",
			"r9"
		},
		new string[5]
		{
			"moonstone",
			"crystal",
			"jade",
			"blue_gem",
			"pink_fire_gem"
		}
	};

	public const string COMMON_IDLE = "idle";

	public const string COMMON_ANIMATION = "animation";

	public const string ANIMATION_EXPLODE = "explode";

	public const string CORRIDOR_WORKER_IDLE = "idle";

	public const string CORRIDOR_WORKER_WALK = "walk";

	public const string CORRIDOR_WORKER_DIG = "dig";

	public const string CORRIDOR_WORKER_SMASH = "smash";

	public const string CORRIDOR_WORKER_DRILL = "drill";

	public const string CORRIDOR_WORKER_WALKBAG = "walk_bag";

	public const string CORRIDOR_WORKER_TRANSITION_IDLE_WALK = "T_idle_walk";

	public const string CORRIDOR_WORKER_TRANSITION_WALK_DIG = "T_walk_dig";

	public const string CORRIDOR_WORKER_TRANSITION_WALKBAG_IDLE = "T_bag_idle";

	public static string[] WORKER_SKIN = new string[10]
	{
		"common1",
		"common2",
		"common3",
		"common4",
		"rare1",
		"rare2",
		"rare3",
		"epic1",
		"epic2",
		"legendary"
	};

	public const string MANAGER_IDLE = "idle";

	public const string MANAGER_WAVE = "wave";

	public const string MANAGER_AURA_JUNIOR_IDLE = "jun_aura_idle";

	public const string MANAGER_AURA_JUNIOR_WAVE = "jun_aura_wave";

	public const string MANAGER_AURA_SENIOR_IDLE = "sen_aura_idle";

	public const string MANAGER_AURA_SENIOR_WAVE = "sen_aura_wave";

	public const string MANAGER_AURA_EXECUTIVE_IDLE = "exe_aura_idle";

	public const string MANAGER_AURA_EXECUTIVE_WAVE = "exe_aura_wave";

	public static readonly string[] MANAGER_AURA_IDLE = new string[3]
	{
		"jun_aura_idle",
		"sen_aura_idle",
		"exe_aura_idle"
	};

	public static readonly string[] MANAGER_AURA_WAVE = new string[3]
	{
		"jun_aura_wave",
		"sen_aura_wave",
		"exe_aura_wave"
	};

	public const string ELEVATOR_BOY_IDLE = "idle";

	public const string ELEVATOR_BOY_WAVE = "wave";

	public const string GROUND_WORKER_IDLE = "idle";

	public const string GROUND_WORKER_IDLE_RARE = "idle_rare";

	public const string GROUND_WORKER_IDLE_EPIC = "idle_epic";

	public const string GROUND_WORKER_IDLE_LEGENDARY = "idle_legendary";

	public const string GROUND_WORKER_WALK = "walk";

	public const string GROUND_WORKER_WALK_RARE = "walk_rare";

	public const string GROUND_WORKER_WALK_EPIC = "walk_epic";

	public const string GROUND_WORKER_WALK_LEGENDARY = "walk_legendary";

	public const string MANAGER_SKIN_JUNIOR = "1_junior";

	public const string MANAGER_SKIN_SENIOR = "2_senior";

	public const string MANAGER_SKIN_EXECUTIVE = "3_executive";

	public static readonly string[] MANAGER_SKIN = new string[3]
	{
		"1_junior",
		"2_senior",
		"3_executive"
	};

	public const string SQUIRREL_FORWARD = "forward";

	public const string SQUIRREL_BACKWARD = "backward";
}
