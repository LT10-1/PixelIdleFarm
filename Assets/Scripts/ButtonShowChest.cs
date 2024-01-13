using System.Collections.Generic;
using System.Linq;
using TMPro;

public class ButtonShowChest : BaseController
{
	public TMP_Text ChestCountText;

	public ButtonMenuHightlight ButtonMenuHightlight => GetComponentInChildren<ButtonMenuHightlight>(includeInactive: true);

	public override void Awake()
	{
		base.Awake();
	}

	public override void Start()
	{
		base.Start();
		OnChestChange();
		BaseController.GameController.OnMineChestChangeCallback.Add(OnChestChange);
		ButtonMenuHightlight.OnClickCallback = delegate
		{
			BaseController.GameController.DialogController.WorkShop.OnShow();
		};
	}

	public void OnChestChange()
	{
		SetChestNumber(DataManager.Instance.SavegameData.ChestSavegames.Sum((KeyValuePair<int, int> e) => e.Value));
	}

	public void SetChestNumber(int chestNumber)
	{
		ButtonMenuHightlight.gameObject.SetActive(chestNumber > 0);
		ChestCountText.text = chestNumber.ToString();
	}
}
