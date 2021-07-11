using GoogleMobileAds.Common;
using System;
using System.Reflection;

namespace GoogleMobileAds.Api
{
	public class MobileAds
	{
		private static readonly IMobileAdsClient client = GetMobileAdsClient();

		public static void Initialize(string appId)
		{
			client.Initialize(appId);
		}

		private static IMobileAdsClient GetMobileAdsClient()
		{
			Type type = Type.GetType("GoogleMobileAds.GoogleMobileAdsClientFactory,Assembly-CSharp");
			MethodInfo method = type.GetMethod("MobileAdsInstance", BindingFlags.Static | BindingFlags.Public);
			return (IMobileAdsClient)method.Invoke(null, null);
		}
	}
}
