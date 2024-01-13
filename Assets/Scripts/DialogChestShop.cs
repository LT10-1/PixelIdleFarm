using System.Collections.Generic;
using System.Linq;
using TMPro;

public class DialogChestShop : BaseDialog
{
	public UIButtonController ButtonOpenSuperShop;

	public TMP_Text CurrentSuperCash;

	private Dictionary<ChestType, DialogChestItem> _dialogChestItems;

	public Dictionary<ChestType, DialogChestItem> DialogChestItems => _dialogChestItems ?? (_dialogChestItems = GetComponentsInChildren<DialogChestItem>().ToDictionary((DialogChestItem e) => e.ChestType, (DialogChestItem e) => e));

	public override void Start()
	{
		base.Start();
		base.BackgroundDialog.SetTitle("Chest Shop");
		ButtonOpenSuperShop.OnClickCallback = BaseController.GameController.MineController.OnClickSuperShop;
		BaseController.GameController.OnGlobalSuperCashChangeCallback.Add(OnSuperCashChange);
		OnSuperCashChange();
	}

	public void OnSuperCashChange()
	{
		CurrentSuperCash.text = DataManager.Instance.SuperCash.MinifyFormat();
	}
}
