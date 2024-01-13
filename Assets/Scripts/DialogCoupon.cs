using TMPro;

public class DialogCoupon : BaseDialog
{
	public TMP_InputField InputField;

	public UIButtonController ButtonSendCoupon;

	private string CurrentCoupon = string.Empty;

	public override void Start()
	{
		base.Start();
		CurrentCoupon = string.Empty;
		base.BackgroundDialog.SetTitle("Coupon");
		ButtonSendCoupon.OnClickCallback = OnSendCoupon;
	}

	public void OnSendCoupon()
	{
		if (CurrentCoupon.Length > 0)
		{
			BaseController.GameController.ToastController.StartToast("Another Coupon Is Being Checked!");
			return;
		}
		CurrentCoupon = InputField.text.ToLower();
		if (CurrentCoupon.Length <= 0)
		{
			BaseController.GameController.ToastController.StartToast("Coupon Cannot Be Empty!");
		}
		else if (DataManager.Instance.SavegameData.CouponSubmitList.Contains(CurrentCoupon))
		{
			BaseController.GameController.ToastController.StartToast("Coupon Have Been Submit!");
			CurrentCoupon = string.Empty;
		}
		else if (CheckCustomCommand())
		{
			CurrentCoupon = string.Empty;
		}
		else
		{
			BaseController.GameController.APIHelper.SubmitCouponCode(CurrentCoupon, delegate
			{
				//BaseController.GameController.AnalyticController.LogEvent("use_coupon_success", "name", CurrentCoupon);
				DataManager.Instance.SavegameData.CouponSubmitList.Add(CurrentCoupon);
				CurrentCoupon = string.Empty;
			}, delegate
			{
				CurrentCoupon = string.Empty;
			});
		}
	}

	public bool CheckCustomCommand()
	{
		if (CurrentCoupon == "wefuckinglost")
		{
			BaseController.GameController.DeleteDataAndReset();
			return true;
		}
		if (CurrentCoupon == "testproduct")
		{
			BaseController.GameController.DialogController.DialogSuperShop.TestProduct.SetActive(value: true);
			return true;
		}
		return false;
	}
}
