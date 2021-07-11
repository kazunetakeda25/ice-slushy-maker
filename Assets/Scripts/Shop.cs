using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
	public static Shop Instance;

	public int StarsToAdd;

	public int StarsToAddStart;

	public static int UnlockAll;

	public static int RemoveAds;

	public static int SpecialOffer;

	public static bool bShowSpecialOfferInShop;

	public bool bShopWatchVideo;

	public string ShopItemID = string.Empty;

	public Text[] txtDispalyStars;

	public string MenuItemName = string.Empty;

	public Text[] txtFields;

	private int StartVal;

	private int ValToAdd;

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else
		{
			Instance = this;
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
		GameData.Init();
	}

	public void WatchVideo()
	{
		if (GameData.sTestiranje.Contains("WatchVideo;"))
		{
			FinishWatchingVideo();
			return;
		}
		AdsManager.bPlayVideoReward = true;
		AdsManager.Instance.IsVideoRewardAvailable();
	}

	public void FinishWatchingVideoError()
	{
		MenuManager.Instance.ShowPopUpDialogTitleText("Video not available");
		MenuManager.Instance.ShowPopUpDialogCustomMessageText("Video is not available at this moment. Thank you for understanding.");
	}

	public void FinishWatchingVideo()
	{
		if (SceneManager.GetActiveScene().name == "SelectFlavor")
		{
			GameObject.Find("Canvas/BGSelectFlavor/BGBottom/FlavorsHolder").GetComponent<SlushyFlavorsSlider>().UnlockFlavor();
		}
		else if (SceneManager.GetActiveScene().name == "SlushDecoration")
		{
			GameObject.Find("Canvas/Menus/BottomMenu/SelectDecorationMenu").GetComponent<SelectDecorationMenu>().UnlockItem();
		}
		else if (SceneManager.GetActiveScene().name == "CupDecoration")
		{
			Camera.main.GetComponent<CupDecorationScene>().UnlockItem();
		}
		UnityEngine.Debug.Log("Odlgedan video za  item: " + MenuItemName);
		GameObject gameObject = GameObject.Find(MenuItemName);
		SoundManager.Instance.Coins.Stop();
		SoundManager.Instance.Play_Sound(SoundManager.Instance.Coins);
	}

	public void AnimiranjeDodavanjaZvezdica(int _StarsToAdd, Text txtStars = null, string message = "STARS: ")
	{
		SoundManager.Instance.Coins.Stop();
		SoundManager.Instance.Play_Sound(SoundManager.Instance.Coins);
		StarsToAdd = _StarsToAdd;
		if (txtStars != null)
		{
			StartCoroutine(animShopCoins(txtStars, message));
		}
		else
		{
			StartCoroutine(animShopStarsAllTextFilds());
		}
	}

	private IEnumerator animShopCoins(Text txtStars, string message)
	{
		int StarsToAddProg = 0;
		int addC = 0;
		int stepUL = Mathf.FloorToInt((float)StarsToAdd * 0.175f);
		int stepLL = Mathf.FloorToInt((float)StarsToAdd * 0.19f);
		while (Mathf.Abs(StarsToAddProg) + Mathf.Abs(addC) < Mathf.Abs(StarsToAdd))
		{
			StarsToAddProg += addC;
			txtStars.text = message + (StarsToAddStart + StarsToAddProg).ToString();
			yield return new WaitForSeconds(0.05f);
			addC = Mathf.FloorToInt(UnityEngine.Random.Range(stepLL, stepUL));
		}
		int starsToAdd = StarsToAdd;
	}

	private IEnumerator animShopStarsAllTextFilds()
	{
		int StarsToAddProg2 = 0;
		int addC = 0;
		int stepUL = Mathf.FloorToInt((float)StarsToAdd * 0.175f);
		int stepLL = Mathf.FloorToInt((float)StarsToAdd * 0.22f);
		if (txtDispalyStars == null)
		{
			yield break;
		}
		while (Mathf.Abs(StarsToAddProg2) + Mathf.Abs(addC) < Mathf.Abs(StarsToAdd))
		{
			StarsToAddProg2 += addC;
			for (int i = 0; i < txtDispalyStars.Length; i++)
			{
				if (!(txtDispalyStars[i].text == string.Empty))
				{
					if (txtDispalyStars[i].text.Contains("/"))
					{
						string[] array = txtDispalyStars[i].text.Split('/');
						txtDispalyStars[i].text = (StarsToAddStart + StarsToAddProg2).ToString() + "/" + array[1];
					}
					else
					{
						txtDispalyStars[i].text = (StarsToAddStart + StarsToAddProg2).ToString();
					}
				}
			}
			yield return new WaitForSeconds(0.05f);
			addC = Mathf.FloorToInt(UnityEngine.Random.Range(stepLL, stepUL));
		}
		StarsToAddProg2 = StarsToAdd;
		for (int j = 0; j < txtDispalyStars.Length; j++)
		{
			if (!(txtDispalyStars[j].text == string.Empty) && txtDispalyStars[j].text.Contains("/"))
			{
				string[] array2 = txtDispalyStars[j].text.Split('/');
				txtDispalyStars[j].text = (StarsToAddStart + StarsToAddProg2).ToString() + "/" + array2[1];
			}
		}
	}

	public void AnimiranjeDodavanjaVrednosti(int _Start, int _Add, string message = "")
	{
		SoundManager.Instance.Coins.Stop();
		SoundManager.Instance.Play_Sound(SoundManager.Instance.Coins);
		StartVal = _Start;
		ValToAdd = _Add;
		if (txtFields != null)
		{
			StartCoroutine(animValue(message));
		}
	}

	private IEnumerator animValue(string message = "")
	{
		int ValToAddProg = 0;
		int addC = 0;
		int stepUL = Mathf.FloorToInt((float)ValToAdd * 0.175f);
		int stepLL = Mathf.FloorToInt((float)ValToAdd * 0.22f);
		if (stepLL == 0)
		{
			stepLL = 1;
		}
		if (stepUL == 0)
		{
			stepUL = 1;
		}
		if (txtFields == null)
		{
			yield break;
		}
		while (Mathf.Abs(ValToAddProg) + Mathf.Abs(addC) < Mathf.Abs(ValToAdd))
		{
			ValToAddProg += addC;
			for (int i = 0; i < txtFields.Length; i++)
			{
				txtFields[i].text = message + (StartVal + ValToAddProg).ToString();
			}
			yield return new WaitForSeconds(0.05f);
			addC = Mathf.FloorToInt(UnityEngine.Random.Range(stepLL, stepUL));
		}
		int valToAdd = ValToAdd;
		for (int j = 0; j < txtFields.Length; j++)
		{
			txtFields[j].text = message + (StartVal + ValToAdd).ToString();
		}
	}

	public void SendShopRequest(string _shopItemId)
	{
		ShopItemID = _shopItemId;
		string empty = string.Empty;
		if (_shopItemId == null)
		{
			return;
		}
		if (!(_shopItemId == "RemoveAds"))
		{
			if (!(_shopItemId == "SpecialOffer"))
			{
				if (_shopItemId == "UnlockAll")
				{
					empty = "unlock_all";
				}
			}
			else
			{
				empty = "special_offer";
			}
		}
		else
		{
			empty = "remove_ads";
		}
	}

	private IEnumerator CONFIRM(string _shopItemId)
	{
		yield return new WaitForSeconds(1f);
		ShopTransactionConfirmed(_shopItemId);
	}

	public void ShopTransactionConfirmed(string _shopItemId)
	{
		UnityEngine.Debug.Log("TODO:");
		ShopManager shopManager = null;
		if (GameObject.Find("PopUpShop") != null)
		{
			shopManager = GameObject.Find("PopUpShop").GetComponent<ShopManager>();
		}
		ShopItemID = _shopItemId;
		switch (ShopItemID)
		{
		case "RemoveAds":
			RemoveAds = 2;
			GameData.SetPurchasedItems();
			break;
		case "SpecialOffer":
			UnityEngine.Debug.Log("Special offer bought");
			UnlockAll = 2;
			RemoveAds = 2;
			SpecialOffer = 2;
			GameData.SetPurchasedItems();
			break;
		case "UnlockAll":
			UnlockAll = 2;
			GameData.SetPurchasedItems();
			break;
		}
		ShopItemID = string.Empty;
	}

	public static bool TestSpecialOffer()
	{
		string empty = string.Empty;
		DateTime today = DateTime.Today;
		if (PlayerPrefs.HasKey("VremePonude"))
		{
			empty = PlayerPrefs.GetString("VremePonude");
			DateTime dateTime = DateTime.Parse(empty);
			today = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
		}
		else
		{
			empty = DateTime.Today.ToString();
			PlayerPrefs.SetString("VremePonude", empty);
		}
		if (SpecialOffer == 0)
		{
			PlayerPrefs.SetString("VremePonude", DateTime.Today.ToString());
			bShowSpecialOfferInShop = true;
			return false;
		}
		return false;
	}

	public void ReturnRestoreData(string shopItemsData)
	{
	}
}
