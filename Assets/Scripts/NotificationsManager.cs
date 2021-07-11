using System;
using UnityEngine;

public class NotificationsManager : MonoBehaviour
{
	private void Start()
	{
		if (!PlayerPrefs.HasKey("LastTime"))
		{
			PlayerPrefs.SetString("LastTime", DateTime.Now.ToString());
			SetNottification(172800, "Frozen Slushy - Dessert Food Maker", "Come back and decorate your frozen dessert!", 11223380);
			return;
		}
		string[] array = new string[2]
		{
			"Come back and decorate your frozen dessert!",
			"New ingredients for slushies are waiting! Hurry up!"
		};
		int num = Mathf.FloorToInt(UnityEngine.Random.Range(0, array.Length));
		CancelNottificationWithID(11223380);
		SetNottification(172800, "Frozen Slushy - Dessert Food Maker", array[num], 11223380);
	}

	public void SetNottification(int timeOffset, string title, string message, int id)
	{
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			using (AndroidJavaObject androidJavaObject = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
			{
				androidJavaObject.Call("SendNotification", timeOffset.ToString(), message, id);
			}
		}
	}

	public void CancelAllNotifications()
	{
	}

	public void CancelNottificationWithID(int id)
	{
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			using (AndroidJavaObject androidJavaObject = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
			{
				androidJavaObject.Call("CancelNotification", id);
			}
		}
	}

	public void RegisterForLocalNottifications()
	{
	}
}
