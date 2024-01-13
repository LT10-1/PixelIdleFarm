using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorkerCard : BaseController
{
	public int id;

	public Image cardBkg;

	public Image icon;

	public Image focus;

	public TMP_Text textLevel;

	public TMP_Text textCard;

	public GameObject goLock;

	public Image progress;

	public GameObject XValue;

	public TMP_Text textValue;

	public Image check;

	public TMP_Text TextSecondaryEffect;

	public Image levelUP;

	public TMP_Text textCount;

	public Button button;

	public ManagerArea Area => (ManagerArea)CardParam.CollectibleType;

	public CollectiblesEntity.Param CardParam => DataManager.Instance.CollectiblesDictionary[id];

	public CollectibleProductionFactorsEntity.Param CurrentCardLevel => CardLevel(CardSavegame.Level);

	public CollectibleSavegame CardSavegame => DataManager.Instance.SavegameData.CollectibleSavegames[id];

	public bool IsLock => CardSavegame.Level == 0;

	public CollectibleProductionFactorsEntity.Param CardLevel(int level)
	{
		return DataManager.Instance.CollectibleLevelsDictionary[id][level];
	}

	public override void Start()
	{
	}

	public void SetSummonInfo(int id, int parts)
	{
		SetId(id);
		XValue.SetActive(value: false);
		button.onClick.RemoveAllListeners();
		textCount.gameObject.SetActive(value: false);
		textCount.text = "+" + parts;
		TextSecondaryEffect.transform.parent.gameObject.SetActive(value: false);
	}

	public void SetId(int id, bool showCheck = false)
	{
		this.id = id;
		UpdateWorkerIcon();
		UpdateData();
		check.gameObject.SetActive(showCheck);
	}

	public void SetLock(bool isLock)
	{
		goLock.SetActive(isLock);
		if (isLock)
		{
			check.gameObject.SetActive(value: false);
			TextSecondaryEffect.transform.parent.gameObject.SetActive(value: false);
		}
		textValue.transform.parent.gameObject.SetActive(!isLock);
		progress.gameObject.SetActive(!isLock);
		textLevel.gameObject.SetActive(!isLock);
	}

	public void UpdateWorkerIcon()
	{
		cardBkg.overrideSprite = BaseController.LoadSprite(DATA_RESOURCES.IMAGE.WORKER_RARIRY_LIST[CardParam.RarityID]);
		icon.overrideSprite = BaseController.LoadSprite(DATA_RESOURCES.IMAGE.WORKER_AVATAR_LIST(Area, ANIMATION.WORKER_SKIN[id % 10]));
		progress.color = COLOR.COLOR_WORKER_RARITY[CardParam.RarityID];
	}

	public void UpdateData()
	{
		if (IsLock)
		{
			SetLock(isLock: true);
			return;
		}
		SetLock(isLock: false);
		check.gameObject.SetActive(DataManager.Instance.SavegameData.CurrentActiveWorkerCard(Area) == id);
		TextSecondaryEffect.transform.parent.gameObject.SetActive(CardParam.SecondaryEffectId != 0);
		if (CardParam.SecondaryEffectId == 3)
		{
			TextSecondaryEffect.text = DATA_RESOURCES.TEXT_SPRITE.SAKURA_CASH + DATA_RESOURCES.TEXT_SPRITE.SAND_CASH + DATA_RESOURCES.TEXT_SPRITE.CASH;
		}
		else if (CardParam.SecondaryEffectId == 2)
		{
			TextSecondaryEffect.text = DATA_RESOURCES.TEXT_SPRITE.SPRITE[CardParam.SecondaryEffectTargetId];
		}
		textLevel.text = "Level " + CardSavegame.Level;
		if (CardSavegame.Level == CardParam.MaxLevel)
		{
			textCard.text = "MAX";
			progress.fillAmount = 1f;
		}
		else
		{
			CollectibleProductionFactorsEntity.Param param = CardLevel(CardSavegame.Level + 1);
			progress.fillAmount = (float)CardSavegame.Parts / (float)param.PartsRequired;
			textCard.text = CardSavegame.Parts + " / " + param.PartsRequired;
		}
		textValue.text = CurrentCardLevel.ProductionFactor.MinifyIncomeFactor();
	}
}
