using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogIdleCash : BaseDialog
{
	public TMP_Text TextCash;

	public TMP_Text TextIdleTime;

	public TMP_Text TextContent;

	public TMP_Text CashTypeIcon;

	public TMP_Text ChestCollectText;

	public CoinController TextBonusCash;

	public Image Background;

	public Transform MoneyPositionBoost;

	public Transform MoneyGroup;

	public UIButtonController ButtonCollect;

	public UIButtonController ButtonCollect2;

	private double idleCash;

	[HideInInspector]
	public float MoneyGroupOriginalPositionX;

	[HideInInspector]
	public float TextBonusCashOriginalPositionY;

	public override void Start()
	{
		base.Start();
		base.BackgroundDialog.SetTitle("Idle Cash Gain");
		ButtonCollect.onClick.AddListener(((BaseDialog)this).OnHide);
		ButtonCollect2.OnClickCallback = OnCollect2;
	}

	public void OnShow(long seconds, double cash, double cashBonus)
	{
		OnShow();
		ButtonCollect2.GetComponent<AdButtonController>().OriginalText = "Collect x" + BaseController.GameController.SkillController.AdCurrentx2IdleCashFactor.MinifyIncomeFactor();
		CashTypeIcon.text = base.CURRENT_CASH_SPRITE;
		TextBonusCash.SetCoinType(base.CurrentCoinType);
		if (MoneyGroupOriginalPositionX == 0f)
		{
			Vector3 localPosition = MoneyGroup.transform.localPosition;
			MoneyGroupOriginalPositionX = localPosition.x;
		}
		if (TextBonusCashOriginalPositionY == 0f)
		{
			Vector3 localPosition2 = TextBonusCash.transform.localPosition;
			TextBonusCashOriginalPositionY = localPosition2.y;
		}
		idleCash = cash + cashBonus;
		TextCash.text = idleCash.MinifyFormat();
		TextBonusCash.SetMoney(cashBonus, minify: true, showMoney: true, " " + DATA_RESOURCES.TEXT_SPRITE.BOLT);
		TextIdleTime.text = seconds.FormatTimeString();
		if (seconds >= 2592000)
		{
			TextIdleTime.text += " (MAX)";
		}
		bool flag = MathUtils.CompareDoubleBiggerThanZero(cashBonus);
		Background.sprite = BaseController.LoadSprite((!flag) ? "Images/UI/Cashgain/Background" : "Images/UI/Cashgain/boosted");
		Transform moneyGroup = MoneyGroup;
		float x;
		if (flag)
		{
			Vector3 localPosition3 = MoneyPositionBoost.localPosition;
			x = localPosition3.x;
		}
		else
		{
			x = MoneyGroupOriginalPositionX;
		}
		Vector3 localPosition4 = MoneyGroup.localPosition;
		moneyGroup.localPosition = new Vector3(x, localPosition4.y);
		TextBonusCash.gameObject.SetActive(flag);
		if (flag)
		{
			Transform transform = TextBonusCash.transform;
			Vector3 localPosition5 = TextBonusCash.transform.localPosition;
			transform.localPosition = new Vector3(localPosition5.x, TextBonusCashOriginalPositionY);
			TextBonusCash.FlyUpAnimation(300f, destroyOncomplete: false);
		}
		Dictionary<ChestType, int> dictionary = BaseController.GameController.ChestController.GenerateDailyChest();
		int num = dictionary.Sum((KeyValuePair<ChestType, int> e) => e.Value);
		ChestCollectText.gameObject.SetActive(num > 0);
		ChestCollectText.text = DATA_RESOURCES.TEXT_SPRITE.CHEST_NORMAL + " Chest collected: " + num;
		foreach (KeyValuePair<ChestType, int> item in dictionary)
		{
			AddChest(item.Key, item.Value);
		}
	}

	public override void OnHide()
	{
		base.OnHide();
		BaseController.GameController.CashFlyEffect.StartEff(ButtonCollect.gameObject);
		AddCash(idleCash);
		BaseController.GameController.AudioController.PlayOneShot("Audios/Effect/tienbay");
	}

	private void OnCollect2()
	{
		base.OnHide();
		//BaseController.GameController.AnalyticController.LogEvent("watch_ad", "type", "x2_idle_cash");
		BaseController.GameController.CashFlyEffect.StartEff(ButtonCollect2.gameObject);
		AddCash(idleCash * BaseController.GameController.SkillController.AdCurrentx2IdleCashFactor);
		BaseController.GameController.AudioController.PlayOneShot("Audios/Effect/tienbay");
	}
}
