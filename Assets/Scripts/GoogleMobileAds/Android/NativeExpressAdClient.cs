using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using System;
using UnityEngine;

namespace GoogleMobileAds.Android
{
	public class NativeExpressAdClient : AndroidJavaProxy, INativeExpressAdClient
	{
		private AndroidJavaObject nativeExpressAdView;

		public event EventHandler<EventArgs> OnAdLoaded;

		public event EventHandler<AdFailedToLoadEventArgs> OnAdFailedToLoad;

		public event EventHandler<EventArgs> OnAdOpening;

		public event EventHandler<EventArgs> OnAdClosed;

		public event EventHandler<EventArgs> OnAdLeavingApplication;

		public NativeExpressAdClient()
			: base("com.google.unity.ads.UnityAdListener")
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			nativeExpressAdView = new AndroidJavaObject("com.google.unity.ads.NativeExpressAd", @static, this);
		}

		public void CreateNativeExpressAdView(string adUnitId, AdSize adSize, AdPosition position)
		{
			nativeExpressAdView.Call("create", adUnitId, Utils.GetAdSizeJavaObject(adSize), (int)position);
		}

		public void CreateNativeExpressAdView(string adUnitId, AdSize adSize, int x, int y)
		{
			nativeExpressAdView.Call("create", adUnitId, Utils.GetAdSizeJavaObject(adSize), x, y);
		}

		public void LoadAd(AdRequest request)
		{
			nativeExpressAdView.Call("loadAd", Utils.GetAdRequestJavaObject(request));
		}

		public void SetAdSize(AdSize adSize)
		{
			nativeExpressAdView.Call("setAdSize", Utils.GetAdSizeJavaObject(adSize));
		}

		public void ShowNativeExpressAdView()
		{
			nativeExpressAdView.Call("show");
		}

		public void HideNativeExpressAdView()
		{
			nativeExpressAdView.Call("hide");
		}

		public void DestroyNativeExpressAdView()
		{
			nativeExpressAdView.Call("destroy");
		}

		public string MediationAdapterClassName()
		{
			return nativeExpressAdView.Call<string>("getMediationAdapterClassName", new object[0]);
		}

		public void onAdLoaded()
		{
			if (this.OnAdLoaded != null)
			{
				this.OnAdLoaded(this, EventArgs.Empty);
			}
		}

		public void onAdFailedToLoad(string errorReason)
		{
			if (this.OnAdFailedToLoad != null)
			{
				AdFailedToLoadEventArgs adFailedToLoadEventArgs = new AdFailedToLoadEventArgs();
				adFailedToLoadEventArgs.Message = errorReason;
				AdFailedToLoadEventArgs e = adFailedToLoadEventArgs;
				this.OnAdFailedToLoad(this, e);
			}
		}

		public void onAdOpened()
		{
			if (this.OnAdOpening != null)
			{
				this.OnAdOpening(this, EventArgs.Empty);
			}
		}

		public void onAdClosed()
		{
			if (this.OnAdClosed != null)
			{
				this.OnAdClosed(this, EventArgs.Empty);
			}
		}

		public void onAdLeftApplication()
		{
			if (this.OnAdLeavingApplication != null)
			{
				this.OnAdLeavingApplication(this, EventArgs.Empty);
			}
		}
	}
}
