using UnityEngine;

public class ConstInit : MonoBehaviour
{
	private void Awake()
	{
		CONST.SCREEN_PIXEL_WIDTH = Screen.width;
		CONST.SCREEN_PIXEL_HEIGHT = Screen.height;
		CONST.GRAPHIC_ASPECT = CONST.SCREEN_GRAPHIC_WIDTH / CONST.SCREEN_GRAPHIC_HEIGHT;
		CONST.SCREEN_ASPECT = CONST.SCREEN_PIXEL_WIDTH / CONST.SCREEN_PIXEL_HEIGHT;
		CONST.GRAPHIC_SCREEN_RATIO = CONST.SCREEN_ASPECT / CONST.GRAPHIC_ASPECT;
		CONST.SCREEN_WIDTH = CONST.SCREEN_GRAPHIC_WIDTH / CONST.PIXEL_PER_UNIT;
		CONST.SCREEN_HEIGHT = CONST.SCREEN_GRAPHIC_HEIGHT / CONST.PIXEL_PER_UNIT * CONST.GRAPHIC_ASPECT / CONST.SCREEN_ASPECT;
		CONST.CANVAS_HEIGHT = CONST.SCREEN_GRAPHIC_HEIGHT;
		CONST.CANVAS_WIDTH = CONST.CANVAS_HEIGHT * CONST.SCREEN_ASPECT;
		MonoBehaviour.print("GRAPHIC_ASPECT: " + CONST.GRAPHIC_ASPECT);
		MonoBehaviour.print("SCREEN_ASPECT: " + CONST.SCREEN_ASPECT);
		CONST.PLATFORM_TYPE = PlatformType.Editor;
		CONST.PLATFORM_TYPE = PlatformType.Android;
	}

	private void Update()
	{
	}
}
