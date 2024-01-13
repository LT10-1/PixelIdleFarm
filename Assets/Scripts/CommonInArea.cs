using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CommonInArea : BaseWorldmapController
{
	public Button button;

	public Button buttonEnter;

	public Image background;

	public Image imgLock;

	public CoinController cashIncome;

	public TMP_Text labelPrice;

	public TMP_Text labelUnknow;

	public TMP_Text MineName;

	private float AlphaThreshold = 0.5f;

	[HideInInspector]
	public AreaInMap AreaInMap;

	[HideInInspector]
	public double MinePrice;

	public override void Start()
	{
		background.alphaHitTestMinimumThreshold = AlphaThreshold;
		buttonEnter.onClick.AddListener(delegate
		{
			base.WorldMapController.goToSelectedMine();
		});
		button.onClick.AddListener(delegate
		{
			base.WorldMapController.hideCurrenButton();
			if (imgLock.gameObject.activeInHierarchy)
			{
				if (UseCash(MinePrice, (ContinentType)AreaInMap.ContinentId))
				{
					BuyNewMine();
				}
			}
			else if (!labelUnknow.gameObject.activeInHierarchy && MineName.gameObject.activeInHierarchy)
			{
				buttonEnter.gameObject.SetActive(value: true);
				buttonEnter.gameObject.transform.localScale = Vector2.zero;
				buttonEnter.gameObject.transform.DOScale(1f, 0.2f);
				base.WorldMapController.CurrentMine = AreaInMap.id;
				base.WorldMapController.currentEnterButton = buttonEnter;
			}
		});
	}

	public void BuyNewMine()
	{
		AreaInMap.SetLockActive(isLock: false);
		BaseController.GameController.CreateNewMine();
		base.WorldMapController.OnBuyNewMine();
		base.WorldMapController.CurrentMine = AreaInMap.id;
		base.WorldMapController.goToSelectedMine();
	}

	public override void Update()
	{
		base.Update();
		Utils.SetColorEnable(labelPrice, CheckEnoughCash(MinePrice, (ContinentType)AreaInMap.ContinentId));
	}

	public void SetPrice(double price)
	{
		MinePrice = price;
		labelPrice.text = $"Cost: {DATA_RESOURCES.TEXT_SPRITE.SPRITE[AreaInMap.ContinentId]} {price.MinifyFormat()}";
	}
}
