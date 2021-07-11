using GoogleMobileAds.Api;
using System;

namespace GoogleMobileAds.Common
{
	public interface IRewardBasedVideoAdClient
	{
		event EventHandler<EventArgs> OnAdLoaded;

		event EventHandler<AdFailedToLoadEventArgs> OnAdFailedToLoad;

		event EventHandler<EventArgs> OnAdOpening;

		event EventHandler<EventArgs> OnAdStarted;

		event EventHandler<Reward> OnAdRewarded;

		event EventHandler<EventArgs> OnAdClosed;

		event EventHandler<EventArgs> OnAdLeavingApplication;

		void CreateRewardBasedVideoAd();

		void LoadAd(AdRequest request, string adUnitId);

		bool IsLoaded();

		string MediationAdapterClassName();

		void ShowRewardBasedVideoAd();
	}
}
