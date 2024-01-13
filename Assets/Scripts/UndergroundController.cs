using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class UndergroundController : BaseMineController
{
	public Transform TierBackgroundGroup;

	public Transform BackgroundMineMaterialGroup;

	public Transform BackgroundBigMineGroup;

	public Transform TierMileStoneGroup;

	public List<Sprite> ShaftMileStoneList;

	public List<Sprite> ShaftLevelList;

	[HideInInspector]
	public List<TierBackgroundController> TierBackgroundControllers = new List<TierBackgroundController>();

	[HideInInspector]
	public List<TierMileStoneController> TierMileStoneRock = new List<TierMileStoneController>();

	public override void Start()
	{
		base.Start();
		for (int i = 0; i < 31; i++)
		{
			TierBackgroundController component = InstantiatePrefab("Prefabs/Mine/TierBackground").GetComponent<TierBackgroundController>();
			component.GetComponent<SortingGroup>().sortingOrder = 30 - i;
			component.transform.SetParent(TierBackgroundGroup.transform);
			component.transform.localPosition = Vector3.up * (3.04f - (float)i * 3.3f);
			if (i <= 10)
			{
				component.SetShaftSprite(ShaftLevelList[0]);
			}
			else if (i <= 20)
			{
				component.SetShaftSprite(ShaftLevelList[0], new Color(0.9f, 0.9f, 0.9f));
			}
			else if (i <= 25)
			{
				component.SetShaftSprite(ShaftLevelList[1]);
			}
			else
			{
				component.SetShaftSprite(ShaftLevelList[1], new Color(0.7f, 0.7f, 0.7f));
			}
			TierBackgroundControllers.Add(component);
			if (i <= 25)
			{
				int tier = i;
				Vector3 localPosition = component.transform.localPosition;
				CreateMineMaterial(tier, localPosition.y);
			}
			else
			{
				int tier2 = i;
				Vector3 localPosition2 = component.transform.localPosition;
				createOnlyBigMineStone(tier2, localPosition2.y);
			}
		}
		for (int j = 5; j < 30; j++)
		{
			if (j % 5 == 0)
			{
				int index = 0;
				if (j >= 20)
				{
					index = 1;
				}
				TierMileStoneController component2 = InstantiatePrefab("Prefabs/Mine/TierMileStone").GetComponent<TierMileStoneController>();
				component2.gameObject.SetActive(value: true);
				component2.transform.SetParent(TierMileStoneGroup.transform);
				component2.transform.localPosition = Vector3.up * (3.04f - (float)j * 3.3f - 0.7f);
				component2.spriteRock.sprite = ShaftMileStoneList[index];
				TierMileStoneRock.Add(component2);
			}
		}
		for (int k = 2; k < 5; k++)
		{
			int mileStoneIndex = k;
			Vector3 localPosition3 = TierBackgroundControllers[k * 5].transform.localPosition;
			CreateBigMineStone(mileStoneIndex, localPosition3.y);
		}
	}

	public override void Update()
	{
		base.Update();
	}

	private void createOnlyBigMineStone(int tier, float tierPositionY)
	{
		int num = Random.Range(20, 25);
		float num2 = CONST.SCREEN_WIDTH / (float)(num / 3);
		for (int i = 0; i < num; i++)
		{
			GameObject gameObject = base.MineController.CreateMineGameObject();
			gameObject.transform.SetParent(BackgroundMineMaterialGroup);
			Vector2 vector = new Vector2((float)(i % 2 + 1) * 0.1f, (float)(i % 2 + 1) * 0.2f);
			gameObject.transform.localEulerAngles = Vector3.forward * Random.Range(0, 360);
			gameObject.transform.localScale = Vector3.one * Random.Range(vector.x, vector.y);
			gameObject.transform.localPosition = new Vector3(Random.Range((0f - num2) / 5f, num2 / 2f) + (0f - CONST.SCREEN_WIDTH) / 2f + num2 * (float)i / 3f, tierPositionY + Random.Range(0f, 2.475f));
		}
		for (int j = 0; j < num; j++)
		{
			GameObject gameObject2 = base.MineController.CreateMineGameObject();
			gameObject2.transform.SetParent(BackgroundBigMineGroup);
			Vector2 vector2 = new Vector2((float)(j % 2 + 1) * 0.4f, (float)(j % 2 + 1) * 0.5f);
			gameObject2.transform.localEulerAngles = Vector3.forward * Random.Range(0, 360);
			gameObject2.transform.localScale = Vector3.one * Random.Range(vector2.x, vector2.y);
			gameObject2.transform.localPosition = new Vector3(Random.Range((0f - num2) / 5f, num2 / 2f) + (0f - CONST.SCREEN_WIDTH) / 2f + num2 * (float)j / 3f, tierPositionY + Random.Range(0f, 2.475f));
			if (j % 3 == 0)
			{
				StarEffect component = InstantiatePrefab("Prefabs/Effects/StarEffect").GetComponent<StarEffect>();
				component.transform.SetParent(BackgroundBigMineGroup);
				component.init(new Vector3((0f - CONST.SCREEN_WIDTH) / 2f + num2 * (float)j / 3f, tierPositionY), new Vector2(num2, 3.3f));
			}
		}
	}

	private void CreateBigMineStone(int mileStoneIndex, float positionY)
	{
		float num = 16.5f;
		float num2 = num / (float)mileStoneIndex;
		Vector2 vector = new Vector2((float)mileStoneIndex * 0.1f + 0.5f, (float)mileStoneIndex * 0.2f + 0.5f);
		for (int i = 0; i < mileStoneIndex; i++)
		{
			int num3 = Random.Range(mileStoneIndex - 2, 5);
			float num4 = CONST.SCREEN_WIDTH / (float)num3;
			for (int j = 0; j < num3; j++)
			{
				GameObject gameObject = base.MineController.CreateMineGameObject();
				gameObject.transform.SetParent(BackgroundBigMineGroup);
				gameObject.transform.localEulerAngles = Vector3.forward * Random.Range(0, 360);
				gameObject.transform.localScale = Vector3.one * Random.Range(vector.x, vector.y);
				gameObject.transform.localPosition = new Vector3(vector.x - CONST.SCREEN_WIDTH / 2f + (float)j * num4 + Random.Range(0f, num4 / 2f), positionY - (float)i * num2 - Random.Range(0f, num2 / 2f));
			}
		}
	}

	private void CreateMineMaterial(int tier, float tierPositionY)
	{
		int num = tier / 5;
		int num2 = Random.Range((num * num + 1) * 3, (num * num + 1) * 5);
		Vector2 vector = new Vector2(0.1f, 0.2f);
		float num3 = CONST.SCREEN_WIDTH / (float)num2;
		for (int i = 0; i < num2; i++)
		{
			GameObject gameObject = base.MineController.CreateMineGameObject();
			gameObject.transform.SetParent(BackgroundMineMaterialGroup);
			gameObject.transform.localEulerAngles = Vector3.forward * Random.Range(0, 360);
			gameObject.transform.localScale = Vector3.one * Random.Range(vector.x, vector.y);
			gameObject.transform.localPosition = new Vector3(vector.x + (0f - CONST.SCREEN_WIDTH) / 2f + num3 * (float)i, tierPositionY + Random.Range(0f, 3.3f));
		}
	}
}
