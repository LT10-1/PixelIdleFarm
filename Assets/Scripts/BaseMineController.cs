using UnityEngine;

public class BaseMineController : BaseController
{
	private MineController _mineController;

	public MineController MineController
	{
		get
		{
			if (_mineController == null)
			{
				_mineController = Object.FindObjectOfType<MineController>();
			}
			return _mineController;
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
