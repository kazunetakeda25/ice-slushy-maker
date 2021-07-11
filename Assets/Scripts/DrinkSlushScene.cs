using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DrinkSlushScene : MonoBehaviour
{
	public Image CupImage;

	public RawImage SlushImage;

	public Image StrawCup;

	public Sprite[] strawSprites;

	public ParticleSystem psLevelCompleted;

	private RectTransform rtStrawCup;

	private RectTransform rtSlush;

	private Vector2 rtSlushPos;

	private float slushSizeY;

	private bool bEnableDrink;

	private bool bDrink;

	public Image SCCupImage;

	public RawImage SCSlushImage;

	public Image SCStrawCup;

	private RectTransform rtSCSlush;

	public GameObject PopUpReply;

	public Transform PopupAreYouSure;

	private IEnumerator Start()
	{
		rtSlush = SlushImage.GetComponent<RectTransform>();
		rtSlushPos = rtSlush.localPosition;
		slushSizeY = rtSlush.rect.height;
		if (GameData.FinishedCupSprite != null)
		{
			CupImage.sprite = GameData.FinishedCupSprite;
		}
		SlushImage.texture = GameData.FinishedSlushSprite;
		SlushImage.color = Color.white;
		rtStrawCup = StrawCup.GetComponent<RectTransform>();
		rtStrawCup.transform.localPosition = GameData.strawImgPos;
		rtStrawCup.sizeDelta = GameData.strawImgSize;
		StrawCup.sprite = strawSprites[GameData.strawImgIndex];
		if (GameData.FinishedCupSprite != null)
		{
			SCCupImage.sprite = GameData.FinishedCupSprite;
		}
		SCSlushImage.texture = GameData.FinishedSlushSprite;
		SCSlushImage.color = Color.white;
		RectTransform rtSCStrawCup = SCStrawCup.GetComponent<RectTransform>();
		rtSCStrawCup.transform.localPosition = GameData.strawImgPos;
		rtSCStrawCup.sizeDelta = GameData.strawImgSize;
		SCStrawCup.sprite = strawSprites[GameData.strawImgIndex];
		rtSCSlush = SCSlushImage.GetComponent<RectTransform>();
		LevelTransition.Instance.ShowScene();
		yield return new WaitForSeconds(1f);
		BlockClicks.Instance.SetBlockAll(blockRays: false);
		bEnableDrink = true;
		Tutorial.Instance.ShowTutorial(0);
		EscapeButtonManager.AddEscapeButonFunction("ButtonHomeClicked", string.Empty);
		if (SoundManager.Instance != null)
		{
			SoundManager.Instance.listStopSoundOnExit.Add(SoundManager.Instance.DrinkSlush);
		}
	}

	private void Update()
	{
		if (!bEnableDrink || !(MenuManager.activeMenu == string.Empty))
		{
			return;
		}
		if (Input.GetMouseButtonDown(0))
		{
			Tutorial.Instance.StopTutorial();
			if (Physics2D.Raycast(Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition), Vector3.zero).transform != null)
			{
				bDrink = true;
				if (SoundManager.Instance != null)
				{
					SoundManager.Instance.Play_Sound(SoundManager.Instance.DrinkSlush);
				}
			}
		}
		if (bDrink && Input.GetMouseButtonUp(0))
		{
			bDrink = false;
			if (SoundManager.Instance != null)
			{
				SoundManager.Instance.Stop_Sound(SoundManager.Instance.DrinkSlush);
			}
		}
		if (!bDrink)
		{
			return;
		}
		Vector3 localPosition = rtSlush.localPosition;
		if (localPosition.y > 0f - slushSizeY)
		{
			rtSlush.localPosition -= new Vector3(0f, slushSizeY * Time.deltaTime * 0.15f, 0f);
			rtSCSlush.localPosition = rtSlush.localPosition;
			return;
		}
		if (SoundManager.Instance != null)
		{
			SoundManager.Instance.Stop_Sound(SoundManager.Instance.DrinkSlush);
		}
		Invoke("ShowPopUpReply", 1f);
		bEnableDrink = false;
	}

	private void ShowPopUpReply()
	{
		BlockClicks.Instance.SetBlockAll(blockRays: true);
		BlockClicks.Instance.SetBlockAllDelay(0.5f, blockRays: false);
		MenuManager.Instance.ShowPopUpMenu(PopUpReply);
	}

	public void ButtonReplayClicked()
	{
		BlockClicks.Instance.SetBlockAll(blockRays: true);
		BlockClicks.Instance.SetBlockAllDelay(0.5f, blockRays: false);
		if (SoundManager.Instance != null)
		{
			SoundManager.Instance.Play_ButtonClick();
		}
		MenuManager.Instance.ClosePopUpMenu(PopUpReply);
		rtSlush.localPosition = rtSlushPos;
		rtSCSlush.localPosition = rtSlush.localPosition;
		bEnableDrink = true;
		AdsManager.Instance.ShowInterstitial();
	}

	public void ButtonNextClicked()
	{
		BlockClicks.Instance.SetBlockAll(blockRays: true);
		if (SoundManager.Instance != null)
		{
			SoundManager.Instance.Play_ButtonClick();
		}
		StartCoroutine("LevelCompleted");
		AdsManager.Instance.ShowInterstitial();
	}

	private IEnumerator LevelCompleted()
	{
		BlockClicks.Instance.SetBlockAll(blockRays: true);
		GameData.ResetSlushImages();
		yield return new WaitForSeconds(2f);
		LevelTransition.Instance.HideSceneAndLoadNext("HomeScene");
	}

	public void ButtonHomeClicked()
	{
		BlockClicks.Instance.SetBlockAll(blockRays: true);
		BlockClicks.Instance.SetBlockAllDelay(0.5f, blockRays: false);
		if (SoundManager.Instance != null)
		{
			SoundManager.Instance.Play_ButtonClick();
		}
		PopupAreYouSure.parent.parent.GetComponent<MenuManager>().ShowPopUpMenu(PopupAreYouSure.gameObject);
	}

	public void ButtonHomeYesClicked()
	{
		BlockClicks.Instance.SetBlockAll(blockRays: true);
		if (SoundManager.Instance != null)
		{
			SoundManager.Instance.Play_ButtonClick();
		}
		LevelTransition.Instance.HideSceneAndLoadNext("HomeScene");
		AdsManager.Instance.ShowInterstitial();
	}

	public void ButtonHomeNoClicked()
	{
		BlockClicks.Instance.SetBlockAll(blockRays: true);
		BlockClicks.Instance.SetBlockAllDelay(0.5f, blockRays: false);
		if (SoundManager.Instance != null)
		{
			SoundManager.Instance.Play_ButtonClick();
		}
		PopupAreYouSure.parent.parent.GetComponent<MenuManager>().ClosePopUpMenu(PopupAreYouSure.gameObject);
		if (EscapeButtonManager.EscapeButonFunctionStack.Count == 0)
		{
			EscapeButtonManager.AddEscapeButonFunction("ButtonHomeClicked", string.Empty);
		}
	}
}
