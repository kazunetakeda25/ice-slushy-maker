using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DailyRewards : MonoBehaviour
{
	public static int[] DailyRewardAmount = new int[6]
	{
		10,
		20,
		30,
		40,
		50,
		60
	};

	public static int LevelReward;

	private bool rewardCompleted;

	private int sixDayCount;

	private int typeOfSixReward;

	public Text moneyText;

	private DateTime quitTime;

	private string lastPlayDate;

	private string timeQuitString;

	public Image imgUnlockedDecoration;

	private bool bCollected;

	private void Start()
	{
	}

	public bool TestDailyRewards()
	{
		bool result = false;
		DateTime today = DateTime.Today;
		if (PlayerPrefs.HasKey("LevelReward"))
		{
			LevelReward = PlayerPrefs.GetInt("LevelReward");
		}
		else
		{
			LevelReward = 0;
			PlayerPrefs.SetInt("LevelReward", 0);
		}
		if (PlayerPrefs.HasKey("VremeIzlaska"))
		{
			lastPlayDate = PlayerPrefs.GetString("VremeIzlaska");
			DateTime dateTime = DateTime.Parse(lastPlayDate);
			quitTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
		}
		else
		{
			timeQuitString = DateTime.Now.ToString();
			PlayerPrefs.SetString("VremeIzlaska", timeQuitString);
		}
		if (quitTime.AddDays(1.0) == today)
		{
			LevelReward++;
			if (LevelReward == 7)
			{
				LevelReward = 1;
			}
			result = true;
		}
		else if (quitTime.AddDays(1.0) < today)
		{
			LevelReward = 1;
			result = true;
		}
		else if (quitTime != today)
		{
			LevelReward = 0;
			PlayerPrefs.SetInt("LevelReward", 0);
			timeQuitString = DateTime.Now.ToString();
			PlayerPrefs.SetString("VremeIzlaska", timeQuitString);
		}
		return result;
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		if (pauseStatus)
		{
			timeQuitString = DateTime.Now.ToString();
			PlayerPrefs.SetString("VremeIzlaska", timeQuitString);
			PlayerPrefs.Save();
		}
	}

	public void ShowDailyReward()
	{
		int levelReward = LevelReward;
		base.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 1970f);
		base.gameObject.GetComponent<Animator>().SetBool("IsOpen", value: true);
		MenuManager.bPopUpVisible = true;
		MenuManager.activeMenu = base.transform.name;
		EscapeButtonManager.EscapeButonFunctionStack.Push("CloseDailyReward");
		for (int i = 1; i <= levelReward; i++)
		{
			GameObject.Find("Day" + i.ToString()).transform.GetComponent<Animator>().SetTrigger("EnableImage");
		}
		for (int j = 1; j <= 6; j++)
		{
			GameObject.Find("Day" + j.ToString() + "/NumberText").transform.GetComponent<Text>().text = DailyRewardAmount[j - 1].ToString();
		}
	}

	public IEnumerator moneyCounter(int kolicina)
	{
		int current = int.Parse(moneyText.text);
		int suma = current + kolicina;
		int korak = (suma - current) / 10;
		while (current != suma)
		{
			current += korak;
			moneyText.text = current.ToString();
			yield return new WaitForSeconds(0.07f);
		}
		yield return new WaitForSeconds(0.2f);
		base.gameObject.GetComponent<Animator>().SetBool("IsOpen", value: false);
		if (EscapeButtonManager.EscapeButonFunctionStack.Count > 0 && EscapeButtonManager.EscapeButonFunctionStack.Peek() == "CloseDailyReward")
		{
			EscapeButtonManager.EscapeButonFunctionStack.Pop();
		}
		MenuManager.bPopUpVisible = false;
		MenuManager.activeMenu = string.Empty;
	}

	private void SetActiveDay(int dayNumber)
	{
		GameObject.Find("Day" + dayNumber + "/Image").GetComponent<Image>().color = new Color(255f, 255f, 255f, 1f);
	}

	private void OnApplicationQuit()
	{
		timeQuitString = DateTime.Now.ToString();
		PlayerPrefs.SetString("VremeIzlaska", timeQuitString);
		PlayerPrefs.Save();
	}

	public void TakeReward()
	{
		if (!rewardCompleted)
		{
			StartCoroutine("moneyCounter", DailyRewardAmount[LevelReward - 1]);
			SoundManager.Instance.Play_Sound(SoundManager.Instance.Coins);
			rewardCompleted = true;
		}
	}

	public void Collect()
	{
		if (!bCollected)
		{
			bCollected = true;
			SoundManager.Instance.Play_ButtonClick();
			timeQuitString = DateTime.Now.ToString();
			PlayerPrefs.SetString("VremeIzlaska", timeQuitString);
			PlayerPrefs.SetInt("LevelReward", LevelReward);
			PlayerPrefs.Save();
			TakeReward();
		}
	}
}
