using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReceiveEffect : BaseController
{
	public Button bkgImage;

	private CashFlyEffect collectEff;

	public GameObject group;

	public GameObject labelGroup;

	public GameObject groupAll;

	public Image Light;

	public Image ImageIcon;

	public TMP_Text TextTitle;

	public TMP_Text TextCashIcon;

	public CoinController Value;

	[HideInInspector]
	public CoinType CoinType;

	private bool StartingEffect;

	private bool IsHiding;

	private Action CompleteCallback;

	public override void Awake()
	{
		Light.transform.DOLocalRotate(Vector3.forward * 360f, 5f, RotateMode.FastBeyond360).SetLoops(-1);
		base.transform.SetParent(GameObject.FindGameObjectWithTag("effect").transform, worldPositionStays: false);
		collectEff = InstantiatePrefab("Prefabs/Effects/CashFlyEff").GetComponent<CashFlyEffect>();
		collectEff.transform.SetParent(base.gameObject.transform, worldPositionStays: false);
		bkgImage.onClick.AddListener(OnClickClose);
	}

	public void Show(string title, ItemsEntity.Param ItemParam, Action callback = null)
	{
		CompleteCallback = callback;
		GameController.Instance.AudioController.PlayOneShot("Audios/Effect/tienbay");
		TextTitle.text = title;
		labelGroup.transform.localScale = Vector3.one * 2f;
		labelGroup.transform.DOScale(Vector3.one, 1f).SetEase(Ease.OutBounce);
		ItemType itemType = (ItemType)ItemParam.ItemType;
		Vector3 vector = Vector3.one;
		TextCashIcon.gameObject.SetActive(itemType == ItemType.InstantCash || itemType == ItemType.AddSkillPoint);
		ImageIcon.gameObject.SetActive(!TextCashIcon.gameObject.activeSelf);
		switch (itemType)
		{
		case ItemType.AddSuperCash:
			Value.SetMoney(ItemParam.SuperCashAmount, minify: false, showMoney: true, string.Empty);
			collectEff.StartEff(ImageIcon.gameObject, itemType);
			Value.SetCoinType(CoinType.SuperCash);
			ImageIcon.sprite = BaseController.LoadSprite("Images/UI/Purchase/super cash");
			break;
		case ItemType.InstantCash:
			Value.SetCoinType(CoinType);
			Value.text = ItemParam.InstantCashTime.FormatTimeString() + " Instant Cash";
			TextCashIcon.text = CoinStringByInstantTime(ItemParam.InstantCashTime);
			break;
		case ItemType.AddCashAmount:
			Value.SetMoney(ItemParam.InstantCashAmount, minify: true, showMoney: true, string.Empty);
			collectEff.StartEff(CoinType, ImageIcon.gameObject, itemType);
			Value.SetCoinType(CoinType);
			ImageIcon.sprite = BaseController.LoadSprite("Images/UI/Purchase/cash");
			break;
		case ItemType.IncomeFactor:
			Value.SetCoinType(CoinType.Multi);
			Value.text = ItemParam.CompleteIncomeIncreaseFactor + "x " + DATA_RESOURCES.TEXT_SPRITE.CLOCK + ItemParam.ActiveTimeSeconds.FormatTimeString();
			vector = Vector3.one * 1.5f;
			ImageIcon.sprite = DataUtils.GetItemBoostImage((ItemBoostMultiple)ItemParam.CompleteIncomeIncreaseFactor, (ItemBoostDuration)ItemParam.ActiveTimeSeconds);
			break;
		case ItemType.AddSkillPoint:
			switch (ItemParam.SkillPathID)
			{
			case 0:
				Value.SetCoinType(CoinType.SkillPointGrass);
				break;
			case 1:
				Value.SetCoinType(CoinType.SkillPointSand);
				break;
			case 2:
				Value.SetCoinType(CoinType.SkillPointSakura);
				break;
			}
			Value.text = ItemParam.SkillPointAmount + string.Empty;
			TextCashIcon.text = DATA_RESOURCES.TEXT_SPRITE.SKILL_POINT[ItemParam.SkillPathID];
			break;
		}
		ImageIcon.SetNativeSize();
		ImageIcon.transform.localScale = vector * 0.7f;
		ImageIcon.transform.DOScale(vector, 0.5f).SetEase(Ease.OutBounce);
		StartingEffect = true;
		StartCoroutine(StopEffect());
	}

	public void Show(string title, string image, string valueText, Action callback = null)
	{
		Show(title, image, valueText, Vector3.one, callback);
	}

	public void Show(string title, string image, string valueText, Vector3 scale, Action callback = null)
	{
		TextTitle.text = title;
		Value.text = valueText;
		TextCashIcon.gameObject.SetActive(value: false);
		ImageIcon.gameObject.SetActive(value: true);
		ImageIcon.sprite = BaseController.LoadSprite(image);
		ImageIcon.SetNativeSize();
		ImageIcon.transform.localScale = scale * 0.7f;
		ImageIcon.transform.DOScale(scale, 0.5f).SetEase(Ease.OutBounce);
		StartingEffect = true;
		StartCoroutine(StopEffect());
	}

	public void OnClickClose()
	{
		if (!IsHiding && !StartingEffect)
		{
			hide();
			StopAllCoroutines();
		}
	}

	private IEnumerator StopEffect()
	{
		yield return new WaitForSeconds(0.3f);
		StartingEffect = false;
		yield return new WaitForSeconds(2f);
		hide();
	}

	private void hide()
	{
		if (!IsHiding)
		{
			IsHiding = true;
			groupAll.transform.DOScale(Vector3.zero, 0.2f).OnComplete(delegate
			{
				UnityEngine.Object.Destroy(base.gameObject);
				if (CompleteCallback != null)
				{
					CompleteCallback();
				}
			});
		}
	}
}
