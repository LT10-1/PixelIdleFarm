using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour
{
	public delegate void AdCallBack();

	public string placementId = "rewardedVideo";

	public bool testNoAdAvailable;

	private AdCallBack adCallBack;

	private string gameId = "1674992";

	private void Start()
	{
		testNoAdAvailable = false;
	}

	private void Update()
	{
	}

	public void ShowAd(AdCallBack callback)
	{

		adCallBack = callback;

	}


	public bool IsReady()
	{
		return !testNoAdAvailable;
	}
}
