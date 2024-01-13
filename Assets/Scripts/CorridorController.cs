using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CorridorController : BaseMineController
{
	public SpriteRenderer FirstRockLayer;

	public SpriteRenderer StartElevator;

	public SpriteRenderer EndElevator;

	[HideInInspector]
	public List<CorridorLevelController> CorridorLevelControllers = new List<CorridorLevelController>();

	public int NumberActiveCorridor
	{
		get
		{
			int num = 0;
			for (int i = 0; i < CorridorLevelControllers.Count; i++)
			{
				if (CorridorLevelControllers[i].IsActiveMine)
				{
					num++;
				}
			}
			return num;
		}
	}

	public override void Awake()
	{
		base.Awake();
		CorridorLevelController componentInChildren = GetComponentInChildren<CorridorLevelController>(includeInactive: true);
		if (componentInChildren != null && componentInChildren.gameObject.activeSelf)
		{
			componentInChildren.gameObject.SetActive(value: false);
		}
		StartElevator.sprite = BaseController.LoadSprite(DATA_RESOURCES.IMAGE.GAMEPLAY_CONTINENT_ELEVATOR_DOWN(base.CurrentContinent));
		EndElevator.sprite = BaseController.LoadSprite(DATA_RESOURCES.IMAGE.GAMEPLAY_CONTINENT_ELEVATOR_END(base.CurrentContinent));
		FirstRockLayer.sprite = BaseController.LoadSprite(DATA_RESOURCES.IMAGE.GAMEPLAY_CONTINENT_FIRST_ROCK(base.CurrentContinent));
	}

	public override void Start()
	{
		base.Start();
		CreateCorridorLevel();
		if (DataManager.Instance.CurrentMineSavegame.CorridorLevel.Count == 0)
		{
			DataManager.Instance.CurrentMineSavegame.CorridorLevel.Add(0);
		}
	}

	public override void Update()
	{
		base.Update();
	}

	public void CreateCorridorLevel()
	{
		int count = CorridorLevelControllers.Count;
		CorridorLevelController component = InstantiatePrefab("Prefabs/Mine/CorridorLevel").GetComponent<CorridorLevelController>();
		component.transform.SetParent(base.transform);
		component.transform.localPosition = new Vector3(0f, -0.89f - (float)count * 3.3f, 0f);
		component.MineGroup.GetComponent<SortingGroup>().sortingOrder = 50 - count;
		component.gameObject.SetActive(value: true);
		component.Init(count + 1);
		component.CorridorController = this;
		CorridorLevelControllers.Add(component);
		if (DataManager.Instance.CurrentMineSavegame.CorridorLevel.Count < component.CorridorModel.Tier)
		{
			DataManager.Instance.CurrentMineSavegame.CorridorLevel.Add(0);
		}
	}

	public void StartNewShaftAnimation()
	{
		StopAllCoroutines();
		StartCoroutine(NewShaftEnumerator());
	}

	private IEnumerator NewShaftEnumerator()
	{
		CorridorLevelController corridorLevelController = CorridorLevelControllers[CorridorLevelControllers.Count - 1];
		base.MineController.MineEffectController.NewShaftEffect.transform.position = corridorLevelController.transform.position;
		base.MineController.MineEffectController.StartNewShaftEffect();
		yield return new WaitForSeconds(0.5f);
		corridorLevelController.InitNewShaft();
		base.MineController.OnClickBuyNewShaft();
		yield return new WaitForSeconds(1f);
		base.MineController.MineEffectController.HideNewShaftEffect();
	}

	public void OnBuyNewShaft()
	{
		EndElevator.transform.localPosition -= Vector3.up * 3.3f;
		if (CorridorLevelControllers.Count >= 2 && CorridorLevelControllers.Count < 30)
		{
			if (MISC_PARAMS.MILE_STONE_DELAY_TIME[CorridorLevelControllers.Count] > 0)
			{
				if (DataManager.Instance.CurrentMineSavegame.CorridorLevel.Count <= CorridorLevelControllers.Count)
				{
					base.MineController.MileStoneLockController.ShowLock(MISC_PARAMS.MILE_STONE_DELAY_TIME[CorridorLevelControllers.Count], MISC_PARAMS.MILESTONE_BREAK_COST[CorridorLevelControllers.Count]);
					base.MineController.MileStoneLockController.transform.localPosition = base.MineController.UndergroundController.TierMileStoneRock[(CorridorLevelControllers.Count - 1) / 5].transform.localPosition;
				}
				else
				{
					OnUnlockComplete();
				}
			}
			else
			{
				CreateCorridorLevel();
			}
		}
		base.MineController.OnBuyNewShaft();
	}

	public void UnlockCompleteAnimation()
	{
		StopAllCoroutines();
		StartCoroutine(NewMilestoneEnumerator());
	}

	private IEnumerator NewMilestoneEnumerator()
	{
		base.MineController.MineEffectController.NewMilestoneEffect.transform.position = base.MineController.UndergroundController.TierMileStoneRock[(CorridorLevelControllers.Count - 1) / 5].transform.position;
		base.MineController.MineEffectController.StartMilestoneEffect();
		yield return new WaitForSeconds(0.5f);
		OnUnlockComplete();
		yield return new WaitForSeconds(1f);
		base.MineController.MineEffectController.HideMilestoneEffect();
	}

	public void OnUnlockComplete()
	{
		base.MineController.UndergroundController.TierMileStoneRock[(CorridorLevelControllers.Count - 1) / 5].gameObject.SetActive(value: false);
		CreateCorridorLevel();
	}
}
