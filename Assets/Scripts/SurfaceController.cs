using UnityEngine;

public class SurfaceController : BaseMineController
{
	public SpriteRenderer Background;

	public SpriteRenderer Cloud;

	public SandController SandController;

	public SunController SunController;

	public SpriteRenderer ElevatorHouse;

	public SpriteRenderer WareHouse;

	public SpriteRenderer Grass;

	public GameObject SakuraFalling;

	public override void Awake()
	{
		base.Awake();
		Background.sprite = BaseController.LoadSprite(DATA_RESOURCES.IMAGE.GAMEPLAY_CONTINENT_BACKGROUND(base.CurrentContinent));
		Cloud.sprite = BaseController.LoadSprite(DATA_RESOURCES.IMAGE.GAMEPLAY_CONTINENT_CLOUD(base.CurrentContinent));
		ElevatorHouse.sprite = BaseController.LoadSprite(DATA_RESOURCES.IMAGE.GAMEPLAY_CONTINENT_ELEVATOR_HOUSE(base.CurrentContinent));
		WareHouse.sprite = BaseController.LoadSprite(DATA_RESOURCES.IMAGE.GAMEPLAY_CONTINENT_WAREHOUSE(base.CurrentContinent));
		Grass.sprite = BaseController.LoadSprite(DATA_RESOURCES.IMAGE.GAMEPLAY_CONTINENT_BACKGROUND_GRASS(base.CurrentContinent));
		SandController.gameObject.SetActive(base.CurrentContinent == ContinentType.Sand);
		SunController.gameObject.SetActive(base.CurrentContinent == ContinentType.Sand);
		SakuraFalling.SetActive(base.CurrentContinent == ContinentType.Sakura);
	}
}
