using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SlushDecorationScene : MonoBehaviour
{
	public Image CupImage;

	public RawImage SlushImage;

	public RawImage SlushImageCapture;

	public SelectDecorationMenu decorationsMenu;

	public int activeMenu;

	private int decorationMaxCount = 50;

	private int decCounter;

	private bool bEnableCreateDec;

	private Sprite sNewDecoration;

	public Animator animCup;

	public Animator animBottomMenu;

	public Animator animStrawCup;

	public Transform decorationsHolder;

	public Transform decorationsHolderCapture;

	public ParticleSystem psLevelCompleted;

	public Straw straw;

	private bool bStraw;

	public Transform PopupAreYouSure;

	private IEnumerator Start()
	{
		if (GameData.FinishedCupSprite != null)
		{
			CupImage.sprite = GameData.FinishedCupSprite;
		}
		SlushImage.texture = GameData.SlushImage;
		SlushImage.color = Color.white;
		SlushImageCapture.texture = GameData.SlushImage;
		SlushImageCapture.color = Color.white;
		LevelTransition.Instance.ShowScene();
		yield return new WaitForSeconds(1f);
		BlockClicks.Instance.SetBlockAll(blockRays: true);
		yield return new WaitForSeconds(0.05f);
		BlockClicks.Instance.SetBlockAll(blockRays: true);
		yield return new WaitForSeconds(0.05f);
		BlockClicks.Instance.SetBlockAll(blockRays: true);
		decorationsMenu.ShowMenu(activeMenu);
		animCup.Play("ShowCup2");
		yield return new WaitForSeconds(1.5f);
		BlockClicks.Instance.SetBlockAll(blockRays: false);
		Tutorial.Instance.ShowTutorial(0);
		EscapeButtonManager.AddEscapeButonFunction("ButtonHomeClicked", string.Empty);
	}

	private void Update()
	{
		if (bEnableCreateDec && decCounter < decorationMaxCount && MenuManager.activeMenu == string.Empty && Input.GetMouseButtonDown(0) && Physics2D.Raycast(Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition), Vector3.zero).transform != null)
		{
			GameObject gameObject = new GameObject("dec" + decCounter.ToString().PadLeft(2, '0'));
			gameObject.transform.parent = decorationsHolder;
			RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
			rectTransform.sizeDelta = new Vector2(100f, 100f);
			Image image = gameObject.AddComponent<Image>();
			image.sprite = sNewDecoration;
			image.preserveAspect = true;
			gameObject.transform.localScale = Vector3.one;
			Vector3 vector = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
			vector = new Vector3(vector.x, vector.y, 0f);
			gameObject.transform.position = vector;
			gameObject = new GameObject("dec" + decCounter.ToString().PadLeft(2, '0'));
			gameObject.transform.parent = decorationsHolderCapture;
			rectTransform = gameObject.AddComponent<RectTransform>();
			rectTransform.sizeDelta = new Vector2(100f, 100f);
			gameObject.transform.localScale = Vector3.one;
			vector = vector + decorationsHolderCapture.position - decorationsHolder.position;
			gameObject.transform.position = vector;
			image = gameObject.AddComponent<Image>();
			image.sprite = sNewDecoration;
			image.preserveAspect = true;
			decCounter++;
			Tutorial.Instance.StopTutorial();
			if (SoundManager.Instance != null)
			{
				SoundManager.Instance.StopAndPlay_Sound(SoundManager.Instance.Decoration);
			}
		}
	}

	public void ButtonDecorateFruit()
	{
		if (SoundManager.Instance != null)
		{
			SoundManager.Instance.Play_ButtonClick();
		}
		bEnableCreateDec = false;
		if (activeMenu == 1)
		{
			StartCoroutine(decorationsMenu.ChangeMenu(0));
			activeMenu = 0;
			Tutorial.Instance.StopTutorial();
		}
	}

	public void ButtonDecorateCandy()
	{
		if (SoundManager.Instance != null)
		{
			SoundManager.Instance.Play_ButtonClick();
		}
		bEnableCreateDec = false;
		if (activeMenu == 0)
		{
			StartCoroutine(decorationsMenu.ChangeMenu(1));
			activeMenu = 1;
			Tutorial.Instance.StopTutorial();
		}
	}

	public void SetCreateDecoration(Sprite spriteDecoration)
	{
		bEnableCreateDec = true;
		sNewDecoration = spriteDecoration;
	}

	public void ButtonNextClicked()
	{
		if (SoundManager.Instance != null)
		{
			SoundManager.Instance.Play_ButtonClick();
		}
		if (!bStraw)
		{
			Tutorial.Instance.StopTutorial();
			BlockClicks.Instance.SetBlockAll(blockRays: true);
			bStraw = true;
			CaptureImage.Instance.ScreenshotSlush();
			StartCoroutine("ShowStarws");
		}
		else if (straw.bStarwInCup)
		{
			BlockClicks.Instance.SetBlockAll(blockRays: true);
			StartCoroutine("LevelCompleted");
		}
		AdsManager.Instance.ShowInterstitial();
	}

	private IEnumerator ShowStarws()
	{
		activeMenu = -1;
		bEnableCreateDec = false;
		decorationsMenu.HideMenu();
		animBottomMenu.enabled = true;
		yield return new WaitForEndOfFrame();
		animBottomMenu.Play("Hide");
		straw.ButtonNext.SetActive(value: false);
		animCup.Play("CupClose");
		animCup.transform.Find("CupCollider2D").gameObject.SetActive(value: false);
		yield return new WaitForSeconds(1f);
		SlushImage.texture = GameData.FinishedSlushSprite;
		decorationsHolder.gameObject.SetActive(value: false);
		decorationsHolderCapture.transform.parent.gameObject.SetActive(value: false);
		yield return new WaitForSeconds(1f);
		animStrawCup.gameObject.SetActive(value: true);
		animStrawCup.Play("Show");
		yield return new WaitForSeconds(1f);
		BlockClicks.Instance.SetBlockAll(blockRays: false);
		straw.enabled = true;
		straw.bEnabled = true;
		Tutorial.Instance.ShowTutorial(2);
		animBottomMenu.gameObject.SetActive(value: false);
	}

	private IEnumerator LevelCompleted()
	{
		BlockClicks.Instance.SetBlockAll(blockRays: true);
		animStrawCup.Play("Hide");
		yield return new WaitForSeconds(0.8f);
		animCup.Play("CupFinishDecorating");
		yield return new WaitForSeconds(1f);
		psLevelCompleted.gameObject.SetActive(value: true);
		psLevelCompleted.Play();
		if (SoundManager.Instance != null)
		{
			SoundManager.Instance.Play_Sound(SoundManager.Instance.ActionCompleted);
		}
		yield return new WaitForSeconds(2f);
		GameData.strawImgPos = straw.strawCup.transform.localPosition;
		GameData.strawImgSize = straw.strawCup.sizeDelta;
		LevelTransition.Instance.HideSceneAndLoadNext("DrinkSlush");
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
