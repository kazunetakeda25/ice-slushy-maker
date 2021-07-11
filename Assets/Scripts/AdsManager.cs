using GoogleMobileAds.Api;
using System;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour
{
	[Header("Admob")]
	public string adMobAppID = string.Empty;

	public string interstitalAdMobId = "ca-app-pub-3940256099942544/1033173712";

	public string videoAdMobId = "ca-app-pub-3940256099942544/5224354917";

	private InterstitialAd interstitialAdMob;

	private RewardBasedVideoAd rewardBasedAdMobVideo;

	private AdRequest requestAdMobInterstitial;

	private AdRequest AdMobVideoRequest;

	[Space(15f)]
	[Header("UnityAds")]
	public string unityAdsGameId;

	public string unityAdsVideoPlacementId = "rewardedVideo";

	public static bool bPlayVideoReward;

	public static bool bVideoRewardReady;

	public static bool bFBVideoRewardReady;

	private static AdsManager instance;

	public static AdsManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = (UnityEngine.Object.FindObjectOfType(typeof(AdsManager)) as AdsManager);
			}
			return instance;
		}
	}

	private void Awake()
	{
		base.gameObject.name = GetType().Name;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		InitializeAds();
	}

	public void ShowInterstitial()
	{
		ShowAdMob();
	}

	public void IsVideoRewardAvailable()
	{
		if (isVideoAvaiable())
		{
			bVideoRewardReady = true;
			if (bPlayVideoReward)
			{
				ShowVideoReward(1);
			}
			Camera.main.SendMessage("SetWatchVideoButtons", SendMessageOptions.DontRequireReceiver);
		}
		else
		{
			bVideoRewardReady = false;
			Camera.main.SendMessage("SetWatchVideoButtons", SendMessageOptions.DontRequireReceiver);
			if (bPlayVideoReward)
			{
				Shop.Instance.FinishWatchingVideoError();
			}
		}
	}

	public void ShowVideoReward(int ID)
	{
		if (Advertisement.IsReady(unityAdsVideoPlacementId))
		{
			UnityAdsShowVideo();
		}
		else if (rewardBasedAdMobVideo.IsLoaded())
		{
			AdMobShowVideo();
		}
	}

	private void RequestInterstitial()
	{
		interstitialAdMob = new InterstitialAd(interstitalAdMobId);
		interstitialAdMob.OnAdLoaded += HandleOnAdLoaded;
		interstitialAdMob.OnAdFailedToLoad += HandleOnAdFailedToLoad;
		interstitialAdMob.OnAdOpening += HandleOnAdOpened;
		interstitialAdMob.OnAdClosed += HandleOnAdClosed;
		interstitialAdMob.OnAdLeavingApplication += HandleOnAdLeavingApplication;
		requestAdMobInterstitial = new AdRequest.Builder().Build();
		interstitialAdMob.LoadAd(requestAdMobInterstitial);
	}

	public void ShowAdMob()
	{
		if (interstitialAdMob.IsLoaded())
		{
			interstitialAdMob.Show();
		}
		else
		{
			interstitialAdMob.LoadAd(requestAdMobInterstitial);
		}
	}

	public void HandleOnAdLoaded(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleAdLoaded event received");
	}

	public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
	{
		MonoBehaviour.print("HandleFailedToReceiveAd event received with message: " + args.Message);
	}

	public void HandleOnAdOpened(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleAdOpened event received");
	}

	public void HandleOnAdClosed(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleAdClosed event received");
		interstitialAdMob = new InterstitialAd(interstitalAdMobId);
		interstitialAdMob.LoadAd(requestAdMobInterstitial);
	}

	public void HandleOnAdLeavingApplication(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleAdLeftApplication event received");
	}

	private void RequestRewardedVideo()
	{
		rewardBasedAdMobVideo.OnAdLoaded += HandleRewardBasedVideoLoadedAdMob;
		rewardBasedAdMobVideo.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoadAdMob;
		rewardBasedAdMobVideo.OnAdOpening += HandleRewardBasedVideoOpenedAdMob;
		rewardBasedAdMobVideo.OnAdStarted += HandleRewardBasedVideoStartedAdMob;
		rewardBasedAdMobVideo.OnAdRewarded += HandleRewardBasedVideoRewardedAdMob;
		rewardBasedAdMobVideo.OnAdClosed += HandleRewardBasedVideoClosedAdMob;
		rewardBasedAdMobVideo.OnAdLeavingApplication += HandleRewardBasedVideoLeftApplicationAdMob;
		AdMobVideoRequest = new AdRequest.Builder().Build();
		rewardBasedAdMobVideo.LoadAd(AdMobVideoRequest, videoAdMobId);
	}

	public void HandleRewardBasedVideoLoadedAdMob(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleRewardBasedVideoLoaded event received");
	}

	public void HandleRewardBasedVideoFailedToLoadAdMob(object sender, AdFailedToLoadEventArgs args)
	{
		MonoBehaviour.print("HandleRewardBasedVideoFailedToLoad event received with message: " + args.Message);
	}

	public void HandleRewardBasedVideoOpenedAdMob(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleRewardBasedVideoOpened event received");
	}

	public void HandleRewardBasedVideoStartedAdMob(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleRewardBasedVideoStarted event received");
	}

	public void HandleRewardBasedVideoClosedAdMob(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleRewardBasedVideoClosed event received");
		rewardBasedAdMobVideo = RewardBasedVideoAd.Instance;
		AdMobVideoRequest = new AdRequest.Builder().Build();
		rewardBasedAdMobVideo.LoadAd(AdMobVideoRequest, videoAdMobId);
	}

	public void HandleRewardBasedVideoRewardedAdMob(object sender, Reward args)
	{
		Shop.Instance.FinishWatchingVideo();
		string type = args.Type;
		MonoBehaviour.print("HandleRewardBasedVideoRewarded event received for " + args.Amount.ToString() + " " + type);
	}

	public void HandleRewardBasedVideoLeftApplicationAdMob(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleRewardBasedVideoLeftApplication event received");
	}

	private void InitializeAds()
	{
		MobileAds.Initialize(adMobAppID);
		rewardBasedAdMobVideo = RewardBasedVideoAd.Instance;
		RequestRewardedVideo();
		Advertisement.Initialize(unityAdsGameId);
		RequestInterstitial();
	}

	private void AdMobShowVideo()
	{
		rewardBasedAdMobVideo.Show();
	}

	private void UnityAdsShowVideo()
	{
		ShowOptions showOptions = new ShowOptions();
		showOptions.resultCallback = HandleShowResultUnity;
		Advertisement.Show(unityAdsVideoPlacementId, showOptions);
	}

	private void HandleShowResultUnity(ShowResult result)
	{
		switch (result)
		{
		case ShowResult.Finished:
			Shop.Instance.FinishWatchingVideo();
			UnityEngine.Debug.Log("Video completed - Offer a reward to the player");
			Advertisement.Initialize(unityAdsGameId);
			break;
		case ShowResult.Skipped:
			UnityEngine.Debug.LogWarning("Video was skipped - Do NOT reward the player");
			break;
		case ShowResult.Failed:
			UnityEngine.Debug.LogError("Video failed to show");
			break;
		}
	}

	private bool isVideoAvaiable()
	{
		if (Advertisement.IsReady(unityAdsVideoPlacementId))
		{
			return true;
		}
		if (rewardBasedAdMobVideo.IsLoaded())
		{
			return true;
		}
		return false;
	}
}
