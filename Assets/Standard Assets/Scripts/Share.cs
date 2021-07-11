using UnityEngine;

public class Share : MonoBehaviour
{
	public static void ShareScreenshot(string destination)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("android.content.Intent");
			AndroidJavaObject androidJavaObject = new AndroidJavaObject("android.content.Intent");
			androidJavaObject.Call<AndroidJavaObject>("setAction", new object[1]
			{
				androidJavaClass.GetStatic<string>("ACTION_SEND")
			});
			AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("android.net.Uri");
			AndroidJavaObject androidJavaObject2 = androidJavaClass2.CallStatic<AndroidJavaObject>("parse", new object[1]
			{
				"file://" + destination
			});
			androidJavaObject.Call<AndroidJavaObject>("putExtra", new object[2]
			{
				androidJavaClass.GetStatic<string>("EXTRA_STREAM"),
				androidJavaObject2
			});
			androidJavaObject.Call<AndroidJavaObject>("putExtra", new object[2]
			{
				androidJavaClass.GetStatic<string>("EXTRA_TEXT"),
				"Check out my new design!"
			});
			androidJavaObject.Call<AndroidJavaObject>("putExtra", new object[2]
			{
				androidJavaClass.GetStatic<string>("EXTRA_SUBJECT"),
				"DOLL HOUSE"
			});
			androidJavaObject.Call<AndroidJavaObject>("setType", new object[1]
			{
				"image/png"
			});
			AndroidJavaClass androidJavaClass3 = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject @static = androidJavaClass3.GetStatic<AndroidJavaObject>("currentActivity");
			AndroidJavaObject androidJavaObject3 = androidJavaClass.CallStatic<AndroidJavaObject>("createChooser", new object[2]
			{
				androidJavaObject,
				"SHARE IMAGE USING:"
			});
			@static.Call("startActivity", androidJavaObject3);
		}
	}

	public static string ReturnGalleryFolder()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			AndroidJavaObject @static;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				@static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			}
			using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("com.webelinx.androidutils.Utils"))
			{
				if (androidJavaClass2 != null)
				{
					AndroidJavaObject androidJavaObject = androidJavaClass2.CallStatic<AndroidJavaObject>("CreateInstance", new object[0]);
					androidJavaObject.Call("setContext", @static);
					return androidJavaObject.Call<string>("returnGalleryFolder", new object[0]);
				}
			}
		}
		return string.Empty;
	}

	public static void RefreshGalleryFolder(string path)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			AndroidJavaObject @static;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				@static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			}
			using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("com.webelinx.androidutils.Utils"))
			{
				if (androidJavaClass2 != null)
				{
					AndroidJavaObject obj = androidJavaClass2.CallStatic<AndroidJavaObject>("CreateInstance", new object[0]);
					obj.Call("setContext", @static);
					@static.Call("runOnUiThread", (AndroidJavaRunnable)delegate
					{
						obj.Call("RefreshGallery", path);
					});
				}
			}
		}
	}
}
