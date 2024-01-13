using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoinController : MonoBehaviour
{
	public bool hideWhenNoCash;

	public CoinType CoinType;

	private string TypeText = DATA_RESOURCES.TEXT_SPRITE.CASH;

	private string _text = "0";

	[HideInInspector]
	public double Cash;

	[HideInInspector]
	public double loadableCash;

	[HideInInspector]
	public bool ShowMoney = true;

	[HideInInspector]
	public bool ShowMoneyNumber = true;

	private Tweener flyUpTween;

	private Tweener alphaTween;

	[HideInInspector]
	public float tweenDuration = 3f;

	private TweenCallback _tweenCallback;

	public TMP_Text CoinText => GetComponent<TMP_Text>();

	public string text
	{
		get
		{
			return _text;
		}
		set
		{
			_text = value;
			CoinText.text = ((!ShowMoney) ? string.Empty : TypeText) + " " + ((!ShowMoneyNumber) ? string.Empty : _text);
		}
	}

	private void Start()
	{
		SetCoinType(CoinType);
		checkHideWhenNoCash();
	}

	public void SetCoinType(CoinType coinType)
	{
		CoinType = coinType;
		TypeText = DATA_RESOURCES.TEXT_SPRITE.SPRITE[(int)CoinType];
		text = text;
	}

	public void AddCash(double amount)
	{
		Cash += amount;
		loadableCash += amount;
		SetMoney(Cash, minify: true, showMoney: true, string.Empty);
	}

	public void UseCash(double amount)
	{
		Cash -= amount;
		loadableCash -= amount;
		SetMoney(Cash, minify: true, showMoney: true, string.Empty);
	}

	public void UseAllCash()
	{
		UseCash(Cash);
	}

	public void SetMoney(double money, bool minify = true, bool showMoney = true, string postText = "")
	{
		Cash = money;
		ShowMoney = showMoney;
		if (minify)
		{
			text = Math.Max(money, 0.0).MinifyFormat();
		}
		else
		{
			text = money + string.Empty;
		}
		text += postText;
		checkHideWhenNoCash();
	}

	public void SetMoneyColor(bool isWhite)
	{
		Utils.SetColorEnable(CoinText, isWhite);
	}

	public void SetMoneyBonusColor(bool isBonus)
	{
		Utils.SetColorBonus(CoinText, isBonus);
	}

	private void checkHideWhenNoCash()
	{
		if (hideWhenNoCash)
		{
			base.gameObject.SetActive(MathUtils.CompareDoubleBiggerThanZero(Cash));
		}
	}

	public virtual void FlyUpAnimation(float positionY = 3f, bool destroyOncomplete = true)
	{
		StopAnimation();
		Transform transform = base.transform;
		Vector3 localPosition = base.transform.localPosition;
		flyUpTween = transform.DOLocalMoveY(localPosition.y + positionY, tweenDuration);
		alphaTween = ((Graphic)CoinText).DOFade(0f, tweenDuration);
		flyUpTween.onComplete = delegate
		{
			if (destroyOncomplete)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		};
	}

	public virtual void StopAnimation()
	{
		if (flyUpTween != null)
		{
			flyUpTween.Kill();
		}
		if (alphaTween != null)
		{
			alphaTween.Kill();
		}
		CoinText.color = Color.white;
	}
}
