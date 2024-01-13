using UnityEngine;

public class BaseWorldmapController : BaseController
{
	private WorldMapController _worldMapController;

	public WorldMapController WorldMapController
	{
		get
		{
			if (_worldMapController == null)
			{
				_worldMapController = Object.FindObjectOfType<WorldMapController>();
			}
			return _worldMapController;
		}
	}

	public override void Awake()
	{
		base.Awake();
	}

	public override void Start()
	{
		base.Start();
	}

	public override void Update()
	{
		base.Update();
	}
}
