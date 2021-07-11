using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Rate : MonoBehaviour
{
	private string rateURL;

	[Header("Linkovi za RATE")]
	public string rateUrlAndroid;

	public string rateUrlIOS;

	public string rateUrlWinPhone;

	public string rateUrlWinStore;

	public string rateUrlMAC;

	public static int appStartedNumber;

	public static int alreadyRated;

	private bool rateClicked;

	private void Start()
	{
		rateURL = rateUrlAndroid;
	}

	public void RateClicked(int number)
	{
		if (!rateClicked)
		{
			alreadyRated = 1;
			PlayerPrefs.SetInt("alreadyRated", alreadyRated);
			PlayerPrefs.Save();
			rateClicked = true;
			StartCoroutine("ActivateStars", number);
		}
	}

	private IEnumerator ActivateStars(int number)
	{
		for (int i = 1; i <= number; i++)
		{
			GameObject.Find("PopUpRate/AnimationHolder/Body/ContentHolder/StarsHolder/StarBG" + i + "/Star" + i).GetComponent<Image>().enabled = true;
		}
		Application.OpenURL(rateURL);
		yield return new WaitForSeconds(0.5f);
		HideRateMenu(GameObject.Find("PopUpRate"));
		yield return new WaitForSeconds(0.5f);
		yield return null;
		alreadyRated = 1;
		PlayerPrefs.SetInt("alreadyRated", alreadyRated);
		PlayerPrefs.Save();
	}

	public void ShowRateMenu()
	{
		base.transform.GetComponent<Animator>().Play("Open");
	}

	public void HideRateMenu(GameObject menu)
	{
		MenuManager.Instance.ClosePopUpMenu(menu);
	}

	public void NoThanks()
	{
		alreadyRated = 1;
		PlayerPrefs.SetInt("alreadyRated", alreadyRated);
		PlayerPrefs.Save();
		HideRateMenu(GameObject.Find("PopUpRate"));
	}
}
