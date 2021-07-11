using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HomeScene : MonoBehaviour
{
	public Image SoundOff;

	public Image SoundOn;

	public MenuManager menuManager;

	public GameObject PupUpShop;

	public GameObject PupUpMoreGames;

	public GameObject PupUpRate;

	public static HomeScene Instance;

	public ParticleSystem psLogo;

	private void Awake()
	{
		AdsManager.bVideoRewardReady = false;
		AdsManager.bPlayVideoReward = false;
		Input.multiTouchEnabled = false;
		Instance = this;
	}

	private IEnumerator Start()
	{
		if (SoundManager.soundOn == 1)
		{
			SoundOff.enabled = false;
			SoundOn.enabled = true;
		}
		else
		{
			SoundOff.enabled = true;
			SoundOn.enabled = false;
		}
		LevelTransition.Instance.ShowScene();
		yield return new WaitForSeconds(1f);
	}

	public void ExitGame()
	{
		if (Shop.RemoveAds == 2)
		{
			Application.Quit();
		}
	}

	public void btnSoundClicked()
	{
		if (SoundManager.soundOn == 1)
		{
			SoundOff.enabled = true;
			SoundOn.enabled = false;
			SoundManager.soundOn = 0;
			SoundManager.Instance.MuteAllSounds();
		}
		else
		{
			SoundOff.enabled = false;
			SoundOn.enabled = true;
			SoundManager.soundOn = 1;
			SoundManager.Instance.UnmuteAllSounds();
			if (SoundManager.Instance != null)
			{
				SoundManager.Instance.Play_ButtonClick();
			}
		}
		if (SoundManager.musicOn == 1)
		{
			SoundManager.Instance.Stop_Music();
			SoundManager.musicOn = 0;
		}
		else
		{
			SoundManager.musicOn = 1;
			SoundManager.Instance.Play_Music();
		}
		PlayerPrefs.SetInt("SoundOn", SoundManager.soundOn);
		PlayerPrefs.SetInt("MusicOn", SoundManager.musicOn);
		PlayerPrefs.Save();
		BlockClicks.Instance.SetBlockAll(blockRays: true);
		BlockClicks.Instance.SetBlockAllDelay(0.3f, blockRays: false);
	}

	public void EndWatchingVideo()
	{
	}

	public void btnPlayClick()
	{
		psLogo.Stop();
		if (SoundManager.Instance != null)
		{
			SoundManager.Instance.Play_ButtonClick();
		}
		StartCoroutine("WaitToLoadNext");
	}

	private IEnumerator WaitToLoadNext()
	{
		yield return new WaitForSeconds(1f);
		StopAllCoroutines();
		LevelTransition.Instance.HideSceneAndLoadNext("CupDecoration");
		BlockClicks.Instance.SetBlockAll(blockRays: true);
	}

	public void ButtonShopClicked()
	{
		menuManager.ShowPopUpMenu(PupUpShop);
		PupUpShop.GetComponent<ShopManager>().InitPrices();
		if (SoundManager.Instance != null)
		{
			SoundManager.Instance.Play_ButtonClick();
		}
	}

	public void ButtonMoreGamesClicked()
	{
		BlockClicks.Instance.SetBlockAll(blockRays: true);
		menuManager.ShowPopUpMenu(PupUpMoreGames);
		if (SoundManager.Instance != null)
		{
			SoundManager.Instance.Play_ButtonClick();
		}
	}

	public void ButtonRateUsClicked()
	{
		if (Rate.alreadyRated != 1)
		{
			menuManager.ShowPopUpMenu(PupUpRate);
		}
		if (SoundManager.Instance != null)
		{
			SoundManager.Instance.Play_ButtonClick();
		}
	}

	public void ButtonResetClicked()
	{
		PlayerPrefs.SetString("Data2", "0;-1;0;-1;0;-1;0;-1;0;-1;");
	}
}
