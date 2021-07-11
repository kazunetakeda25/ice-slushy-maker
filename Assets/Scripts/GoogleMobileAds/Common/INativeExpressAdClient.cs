using GoogleMobileAds.Api;
using System;

namespace GoogleMobileAds.Common
{
	public interface INativeExpressAdClient
	{
		event EventHandler<EventArgs> OnAdLoaded;

		event EventHandler<AdFailedToLoadEventArgs> OnAdFailedToLoad;

		event EventHandler<EventArgs> OnAdOpening;

		event EventHandler<EventArgs> OnAdClosed;

		event EventHandler<EventArgs> OnAdLeavingApplication;

		void CreateNativeExpressAdView(string adUnitId, AdSize adSize, AdPosition position);

		void CreateNativeExpressAdView(string adUnitId, AdSize adSize, int x, int y);

		void LoadAd(AdRequest request);

		void ShowNativeExpressAdView();

		void HideNativeExpressAdView();

		void DestroyNativeExpressAdView();

		string MediationAdapterClassName();
	}
}
