using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
	public static MenuManager Instance;

	public Menu currentMenu;

	private Menu currentPopUpMenu;

	public static string activeMenu = string.Empty;

	public GameObject[] disabledObjects;

	private GameObject ratePopUp;

	private GameObject crossPromotionInterstitial;

	public static bool bFirstLoadMainScene = true;

	private DailyRewards dailyaReward;

	public static bool bPopUpVisible;

	private CanvasGroup BlockAll;

	private GameObject PopUpSpecialOffer;

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		activeMenu = string.Empty;
		if (SceneManager.GetActiveScene().name == "HomeScene")
		{
			crossPromotionInterstitial = GameObject.Find("PopUps/PopUpInterstitial");
			ratePopUp = GameObject.Find("PopUps/PopUpRate");
			if (GameObject.Find("PopUps/DailyReward") != null)
			{
				dailyaReward = GameObject.Find("PopUps/DailyReward").GetComponent<DailyRewards>();
			}
			PopUpSpecialOffer = GameObject.Find("PopUps/PopUpSpecialOffer");
		}
		if (disabledObjects != null)
		{
			for (int i = 0; i < disabledObjects.Length; i++)
			{
				disabledObjects[i].SetActive(value: false);
			}
		}
		if (SceneManager.GetActiveScene().name == "HomeScene" && bFirstLoadMainScene)
		{
			StartCoroutine(DelayStartMainScene());
		}
	}

	private IEnumerator DelayStartMainScene()
	{
		if (BlockAll == null)
		{
			BlockAll = GameObject.Find("BlockAll").GetComponent<CanvasGroup>();
		}
		BlockAll.blocksRaycasts = true;
		yield return new WaitForSeconds(0.5f);
		if (PlayerPrefs.HasKey("alreadyRated"))
		{
			Rate.alreadyRated = PlayerPrefs.GetInt("alreadyRated");
		}
		else
		{
			Rate.alreadyRated = 0;
		}
		Rate.appStartedNumber = PlayerPrefs.GetInt("appStartedNumber", 0);
		UnityEngine.Debug.Log("appStartedNumber " + Rate.appStartedNumber);
		if (bFirstLoadMainScene && !Shop.TestSpecialOffer())
		{
			if (Rate.alreadyRated == 0 && Rate.appStartedNumber >= 6)
			{
				Rate.appStartedNumber = 0;
				PlayerPrefs.SetInt("appStartedNumber", Rate.appStartedNumber);
				PlayerPrefs.Save();
				ShowPopUpMenu(ratePopUp);
			}
			else if (bFirstLoadMainScene && dailyaReward != null && dailyaReward.TestDailyRewards())
			{
				dailyaReward.ShowDailyReward();
			}
		}
		bFirstLoadMainScene = false;
		yield return new WaitForSeconds(1.1f);
		BlockAll.blocksRaycasts = false;
	}

	public void EnableObject(GameObject gameObject)
	{
		if (gameObject != null && !gameObject.activeSelf)
		{
			gameObject.SetActive(value: true);
		}
	}

	public void DisableObject(GameObject gameObject)
	{
		if (gameObject != null && gameObject.activeSelf)
		{
			gameObject.SetActive(value: false);
		}
	}

	public void LoadScene(string levelName)
	{
		if (levelName != string.Empty)
		{
			try
			{
				UnityEngine.SceneManagement.SceneManager.LoadScene(levelName);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.Log("Can't load scene: " + ex.Message);
			}
		}
		else
		{
			UnityEngine.Debug.Log("Can't load scene: Level name to set");
		}
	}

	public void LoadSceneAsync(string levelName)
	{
		if (levelName != string.Empty)
		{
			try
			{
				Application.LoadLevelAsync(levelName);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.Log("Can't load scene: " + ex.Message);
			}
		}
		else
		{
			UnityEngine.Debug.Log("Can't load scene: Level name to set");
		}
	}

	public void ShowPopUpMenu(GameObject menu)
	{
		menu.gameObject.SetActive(value: true);
		currentPopUpMenu = menu.GetComponent<Menu>();
		currentPopUpMenu.OpenMenu();
		activeMenu = menu.name;
		bPopUpVisible = true;
		if (SoundManager.Instance != null)
		{
			SoundManager.Instance.Play_Sound(SoundManager.Instance.PopUpShow);
		}
		if (menu.name == "PopUpAreYouSure")
		{
			EscapeButtonManager.AddEscapeButonFunction("ButtonHomeYesClicked", string.Empty);
		}
		else if (SceneManager.GetActiveScene().name != string.Empty)
		{
			EscapeButtonManager.AddEscapeButonFunction("ClosePopUpMenuEsc", menu.name);
		}
	}

	public void ClosePopUpMenu(GameObject menu)
	{
		menu.GetComponent<Menu>().CloseMenu();
		if (activeMenu == menu.name)
		{
			activeMenu = string.Empty;
		}
		if (SoundManager.Instance != null)
		{
			SoundManager.Instance.Play_PopUpHide(0.4f);
		}
		bPopUpVisible = false;
		if (EscapeButtonManager.EscapeButonFunctionStack.Count >= 1)
		{
			EscapeButtonManager.EscapeButonFunctionStack.Pop();
		}
	}

	public void ClosePopUpMenuEsc(string menuName)
	{
		if (GameObject.Find(menuName) != null)
		{
			ClosePopUpMenu(GameObject.Find(menuName));
		}
	}

	private IEnumerator HidePopUp(GameObject menu)
	{
		yield return null;
		menu.GetComponent<Menu>().CloseMenu();
	}

	public void ShowMessage(string message)
	{
		UnityEngine.Debug.Log(message);
	}

	public void ShowPopUpMessage(string messageTitleText, string messageText)
	{
		base.transform.Find("PopUps/PopUpMessage/AnimationHolder/Body/HeaderHolder/TextHeader").GetComponent<Text>().text = messageTitleText;
		base.transform.Find("PopUps/PopUpMessage/AnimationHolder/Body/ContentHolder/TextBG/TextMessage").GetComponent<Text>().text = messageText;
		ShowPopUpMenu(base.transform.Find("PopUps/PopUpMessage").gameObject);
	}

	public void ShowPopUpMessageTitleText(string messageTitleText)
	{
		base.transform.Find("PopUps/PopUpMessage/AnimationHolder/Body/HeaderHolder/TextHeader").GetComponent<Text>().text = messageTitleText;
	}

	public void ShowPopUpMessageCustomMessageText(string messageText)
	{
		base.transform.Find("PopUps/PopUpMessage/AnimationHolder/Body/ContentHolder/TextBG/TextMessage").GetComponent<Text>().text = messageText;
		ShowPopUpMenu(base.transform.Find("PopUps/PopUpMessage").gameObject);
	}

	public void ShowPopUpDialog(string dialogTitleText, string dialogMessageText)
	{
		base.transform.Find("PopUps/PopUpMessage/AnimationHolder/Body/HeaderHolder/TextHeader").GetComponent<Text>().text = dialogTitleText;
		base.transform.Find("PopUps/PopUpMessage/AnimationHolder/Body/ContentHolder/TextBG/TextMessage").GetComponent<Text>().text = dialogMessageText;
		ShowPopUpMenu(base.transform.Find("PopUps/PopUpMessage").gameObject);
	}

	public void ShowPopUpDialogTitleText(string dialogTitleText)
	{
		base.transform.Find("PopUps/PopUpMessage/AnimationHolder/Body/HeaderHolder/TextHeader").GetComponent<Text>().text = dialogTitleText;
	}

	public void ShowPopUpDialogCustomMessageText(string dialogMessageText)
	{
		base.transform.Find("PopUps/PopUpMessage/AnimationHolder/Body/ContentHolder/TextBG/TextMessage").GetComponent<Text>().text = dialogMessageText;
		ShowPopUpMenu(base.transform.Find("PopUps/PopUpMessage").gameObject);
	}

	public void btnClicked_PlaySound()
	{
		if (SoundManager.Instance != null)
		{
			SoundManager.Instance.Play_ButtonClick();
		}
	}
}
