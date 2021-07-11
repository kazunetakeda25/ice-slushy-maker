using GoogleMobileAds.Api;
using System;

namespace GoogleMobileAds.Common
{
	public interface IBannerClient
	{
		event EventHandler<EventArgs> OnAdLoaded;

		event EventHandler<AdFailedToLoadEventArgs> OnAdFailedToLoad;

		event EventHandler<EventArgs> OnAdOpening;

		event EventHandler<EventArgs> OnAdClosed;

		event EventHandler<EventArgs> OnAdLeavingApplication;

		void CreateBannerView(string adUnitId, AdSize adSize, AdPosition position);

		void CreateBannerView(string adUnitId, AdSize adSize, int x, int y);

		void LoadAd(AdRequest request);

		void ShowBannerView();

		void HideBannerView();

		void DestroyBannerView();

		string MediationAdapterClassName();
	}
}
