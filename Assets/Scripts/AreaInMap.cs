using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AreaInMap : BaseWorldmapController
{
	public int id;

	[HideInInspector]
	public Transform DiffInArea;

	[HideInInspector]
	public Transform FocusOutline;

	[HideInInspector]
	public Transform FBFriend;

	[HideInInspector]
	public CommonInArea CommonInArea;

	[HideInInspector]
	public Image ImageBlur;

	[HideInInspector]
	public GameObject Mineral;

	public int ContinentId => id / 5;

	public int MineId => id % 5;

	public override void Awake()
	{
		base.Awake();
		CommonInArea = GetComponentInChildren<CommonInArea>(includeInactive: true);
		DiffInArea = base.transform.Find("Diff");
		FocusOutline = DiffInArea.Find("Focus");
		FBFriend = DiffInArea.Find("FBFriend");
		ImageBlur = DiffInArea.Find("Blur").GetComponent<Image>();
		Mineral = DiffInArea.Find("mineral").gameObject;
		CommonInArea.AreaInMap = this;
	}

	public override void Start()
	{
		base.Start();
	}

	public override void Update()
	{
		base.Update();
		if (CommonInArea.imgLock.gameObject.activeInHierarchy || CommonInArea.labelUnknow.gameObject.activeInHierarchy || !CommonInArea.MineName.gameObject.activeInHierarchy)
		{
			return;
		}
		CommonInArea.MineName.text = $"<size=60>{DATA_RESOURCES.TEXT_SPRITE.RESOURCE[ContinentId][MineId]}</size>";
		MineSavegame mineSavegame = DataManager.Instance.SavegameData.Mines[id];
		if (ContinentId != DataManager.Instance.CurrentMineSavegame.ContinentIndex || MineId != DataManager.Instance.CurrentMineSavegame.MineIndex)
		{
			double currentIdleCash = mineSavegame.CurrentIdleCash;
			if (currentIdleCash > 0.0)
			{
				TMP_Text mineName = CommonInArea.MineName;
				string text = mineName.text;
				mineName.text = text + "\n" + DATA_RESOURCES.TEXT_SPRITE.SPRITE[ContinentId] + " " + currentIdleCash.MinifyFormat();
			}
		}
		TMP_Text mineName2 = CommonInArea.MineName;
		mineName2.text = mineName2.text + "\n" + DATA_TEXT.MINES.CONTINENT_MINES[ContinentId][MineId] + " Mine";
	}

	public void SetLockActive(bool isLock)
	{
		CommonInArea.imgLock.gameObject.SetActive(isLock);
		ImageBlur.gameObject.SetActive(isLock);
		if (isLock)
		{
			CommonInArea.MineName.text = DATA_TEXT.MINES.CONTINENT_MINES[ContinentId][MineId] + " Mine";
		}
		else
		{
			CommonInArea.MineName.text = $"<size=40>{DATA_RESOURCES.TEXT_SPRITE.RESOURCE[ContinentId][MineId]}</size>";
			TMP_Text mineName = CommonInArea.MineName;
			mineName.text = mineName.text + "\n" + DATA_TEXT.MINES.CONTINENT_MINES[ContinentId][MineId] + " Mine";
		}
		CommonInArea.MineName.gameObject.SetActive(value: true);
		Mineral.gameObject.SetActive(value: true);
		CommonInArea.labelUnknow.gameObject.SetActive(value: false);
	}

	public void MakeAreaInTop()
	{
		base.transform.SetAsLastSibling();
	}
}
