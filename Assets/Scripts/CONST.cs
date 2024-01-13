public class CONST
{
	public const double CURRENT_VERSION = 3.2;

	public static bool FAST_GAME_TEST_MODE = false;

	public static bool FAST_MANAGER_SKILL_TEST_MODE = false;

	public static bool TEST_MODE_VIDEO_AD = false;

	public const int IDLE_MIN_SECONDS = 60;

	public const long IDLE_MAX_SECONDS = 2592000L;

	public const string FACEBOOK_ID = "320733985106721";

	public const string FACEBOOK_URL = "fb://profile/320733985106721";

	public const string FAQ_URL = "http://idledwarf.com/faq.html";

	public const string FACEBOOK_PAGE_URL = "http://facebook.com/320733985106721";

	public static float SCREEN_GRAPHIC_WIDTH = 1080f;

	public static float SCREEN_GRAPHIC_HEIGHT = 1920f;

	public static float SCREEN_PIXEL_WIDTH = SCREEN_GRAPHIC_WIDTH;

	public static float SCREEN_PIXEL_HEIGHT = SCREEN_GRAPHIC_HEIGHT;

	public static float PIXEL_PER_UNIT = 100f;

	public static float SCREEN_WIDTH = SCREEN_GRAPHIC_WIDTH / PIXEL_PER_UNIT;

	public static float SCREEN_HEIGHT = SCREEN_GRAPHIC_HEIGHT / PIXEL_PER_UNIT;

	public static float ORTHOGRAPHIC_SIZE = SCREEN_GRAPHIC_HEIGHT / 2f / PIXEL_PER_UNIT;

	public static float CANVAS_WIDTH = SCREEN_GRAPHIC_WIDTH;

	public static float CANVAS_HEIGHT = SCREEN_GRAPHIC_HEIGHT;

	public static float GRAPHIC_ASPECT = 1f;

	public static float SCREEN_ASPECT = 1f;

	public static float GRAPHIC_SCREEN_RATIO = 1f;

	public static PlatformType PLATFORM_TYPE;
}
