public class DialogNotEnoughSuperCash : BaseDialog
{
	public UIButtonController ButtonGoToShop;

	public override void Start()
	{
		base.Start();
		base.BackgroundDialog.SetTitle("Not Enough Super Cash");
		ButtonGoToShop.OnClickCallback = OnClickGoToShop;
	}

	private void OnClickGoToShop()
	{
		OnHide();
		BaseController.GameController.MineController.OnClickSuperShop();
	}
}
