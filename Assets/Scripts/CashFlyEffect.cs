using DG.Tweening;
using UnityEngine;

public class CashFlyEffect : BaseController
{
	public CoinController coinItem;

	public int numItem = 30;

	private GameObject beginPos;

	private GameObject endPos;

	private GameObject effectParent;

	public override void Awake()
	{
		effectParent = GameObject.FindGameObjectWithTag("effect");
	}

	private void SetType(ItemType type, CoinType coinType)
	{
		coinItem.ShowMoneyNumber = false;
		coinItem.SetCoinType(coinType);
		switch (type)
		{
		case ItemType.IncomeFactor:
			break;
		case ItemType.InstantCash:
		case ItemType.AddCashAmount:
			switch (coinType)
			{
			case CoinType.Cash:
				endPos = GameObject.FindGameObjectWithTag("grasscashicon");
				break;
			case CoinType.SandCash:
				endPos = GameObject.FindGameObjectWithTag("sandcashicon");
				break;
			case CoinType.SakuraCash:
				endPos = GameObject.FindGameObjectWithTag("sakuracashicon");
				break;
			}
			if (endPos == null)
			{
				endPos = GameObject.FindGameObjectWithTag("cashicon");
			}
			break;
		case ItemType.AddSuperCash:
			endPos = GameObject.FindGameObjectWithTag("supercashicon");
			coinItem.SetCoinType(CoinType.SuperCash);
			break;
		case ItemType.AddSkillPoint:
			switch (coinType)
			{
			case CoinType.SkillPointGrass:
				endPos = GameObject.FindGameObjectWithTag("grass_skill_icon");
				if (endPos == null)
				{
					endPos = GameObject.FindGameObjectWithTag("grasscashicon");
				}
				break;
			case CoinType.SkillPointSand:
				endPos = GameObject.FindGameObjectWithTag("sand_skill_icon");
				if (endPos == null)
				{
					endPos = GameObject.FindGameObjectWithTag("sandcashicon");
				}
				break;
			case CoinType.SkillPointSakura:
				endPos = GameObject.FindGameObjectWithTag("sakura_skill_icon");
				if (endPos == null)
				{
					endPos = GameObject.FindGameObjectWithTag("sakuracashicon");
				}
				break;
			}
			if (endPos == null)
			{
				endPos = GameObject.FindGameObjectWithTag("cashicon");
			}
			break;
		case ItemType.AddChest:
			endPos = GameObject.FindGameObjectWithTag("chest_icon");
			break;
		}
	}

	public void StartEff(GameObject beginPos = null, ItemType type = ItemType.InstantCash)
	{
		StartEff(base.CurrentCoinType, beginPos, type);
	}

	public void StartEff(CoinType coinType, GameObject beginPos = null, ItemType type = ItemType.InstantCash, int numberItem = 0)
	{
		SetType(type, coinType);
		if (!(endPos == null))
		{
			if (numberItem == 0)
			{
				numberItem = numItem;
			}
			Vector3 position = effectParent.transform.position + new Vector3(Random.Range(-20, 20), Random.Range(-20, 20));
			if (beginPos != null)
			{
				position = beginPos.transform.position;
			}
			for (int i = 0; i < numberItem; i++)
			{
				Transform parent = endPos.transform.parent;
				endPos.transform.SetParent(effectParent.transform);
				Vector3 localPosition = endPos.transform.localPosition;
				endPos.transform.SetParent(parent);
				GameObject go = Object.Instantiate(coinItem.gameObject);
				go.transform.SetParent(effectParent.transform);
				go.transform.localPosition = Vector3.zero;
				go.transform.position = position;
				go.transform.localScale = Vector3.one;
				Vector3 b = new Vector2(Random.Range(-350, 350), Random.Range(-300, 300));
				Sequence s = DOTween.Sequence();
				s.PrependInterval(0.03f * (float)(numberItem - i)).AppendCallback(delegate
				{
					go.SetActive(value: true);
				}).Append(go.transform.DOLocalMove(go.transform.localPosition + b, 0.7f))
					.Append(go.transform.DOLocalMove(localPosition, 0.6f))
					.Append(go.transform.DOScale(1.3f, 0.02f))
					.Append(go.transform.DOScale(1f, 0.02f))
					.OnComplete(delegate
					{
						Object.DestroyObject(go);
					});
			}
		}
	}
}
