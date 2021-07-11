using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SelectFlavorScene : MonoBehaviour
{
	public Animator animBG;

	public Animator animCup;

	public SlushyFlavorsSlider SlushSlider;

	private Animator activeSlushContainer;

	public ParticleSystem psLevelCompleted;

	public Transform PopupAreYouSure;

	private IEnumerator Start()
	{
		BlockClicks.Instance.SetBlockAll(blockRays: true);
		GameData.GetFlavorsContainersList();
		LevelTransition.Instance.ShowScene();
		yield return new WaitForEndOfFrame();
		SlushSlider.EnableMove(_bEnableMove: false);
		yield return new WaitForSeconds(1f);
		BlockClicks.Instance.SetBlockAll(blockRays: true);
		if (GameData.bFillFlavor)
		{
			animBG.Play("SelectFlavorBGZoomIn");
			yield return new WaitForSeconds(1f);
		}
		else
		{
			if (SoundManager.Instance != null)
			{
				SoundManager.Instance.Play_Sound(SoundManager.Instance.ShowCup);
			}
			animCup.Play("ShowCup");
			StartCoroutine("WaitToStartGame");
			yield return new WaitForSeconds(1f);
		}
		EscapeButtonManager.AddEscapeButonFunction("ButtonHomeClicked", string.Empty);
		if (SoundManager.Instance != null)
		{
			SoundManager.Instance.listStopSoundOnExit.Add(SoundManager.Instance.FillCup);
			SoundManager.Instance.listStopSoundOnExit.Add(SoundManager.Instance.FillFlavor);
		}
	}

	public void FillSlushContainer()
	{
		StartCoroutine("WaitToFillSlushContainer");
	}

	private IEnumerator WaitToFillSlushContainer()
	{
		activeSlushContainer = GameObject.Find("FlavorsHolder/AnimationHolder/ScrollRect/Content/Item" + GameData.selectedFlavor.ToString().PadLeft(2, '0')).GetComponent<Animator>();
		activeSlushContainer.enabled = true;
		Image img = activeSlushContainer.transform.Find("SlushyMask/Slushy").GetComponent<Image>();
		img.enabled = false;
		activeSlushContainer.Play("OpenCover");
		yield return new WaitForEndOfFrame();
		img.enabled = true;
	}

	public void FillSlushContainerFinished()
	{
		//activeSlushContainer.Stop();
		activeSlushContainer.enabled = false;
		SlushSlider.FillSlushFlavorFinished();
		if (SoundManager.Instance != null)
		{
			SoundManager.Instance.Stop_Sound(SoundManager.Instance.FillFlavor);
		}
	}

	public void SelectFlavorBGZoomOutFinished()
	{
		activeSlushContainer.enabled = false;
		GameData.bFillFlavor = false;
		if (SoundManager.Instance != null)
		{
			SoundManager.Instance.Play_Sound(SoundManager.Instance.ShowCup);
		}
		animCup.Play("ShowCup");
		StartCoroutine("WaitToStartGame");
	}

	private IEnumerator WaitToStartGame()
	{
		yield return new WaitForSeconds(1f);
		SlushSlider.EnableMove(_bEnableMove: true);
		BlockClicks.Instance.SetBlockAll(blockRays: false);
	}

	private void Update()
	{
	}

	public void CupFull()
	{
		StartCoroutine("LevelCompleted");
	}

	private IEnumerator LevelCompleted()
	{
		BlockClicks.Instance.SetBlockAll(blockRays: true);
		GameData.SetFlavorsContainersList();
		psLevelCompleted.gameObject.SetActive(value: true);
		psLevelCompleted.Play();
		if (SoundManager.Instance != null)
		{
			SoundManager.Instance.Play_Sound(SoundManager.Instance.ActionCompleted);
		}
		yield return new WaitForSeconds(1f);
		animCup.Play("CupFull");
		yield return new WaitForSeconds(3f);
		StopAllCoroutines();
		LevelTransition.Instance.HideSceneAndLoadNext("SlushDecoration");
		BlockClicks.Instance.SetBlockAll(blockRays: true);
	}

	public void LoadCutFruitScene()
	{
		base.transform.GetComponent<FillCup>().SaveSlushFillData();
		GameData.SetFlavorsContainersList();
		StopAllCoroutines();
		StartCoroutine("LoadMakeIceScene");
		BlockClicks.Instance.SetBlockAll(blockRays: true);
	}

	private IEnumerator LoadMakeIceScene()
	{
		GameData.SetFlavorsContainersList();
		yield return new WaitForSeconds(1f);
		LevelTransition.Instance.HideSceneAndLoadNext("MakeIce");
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
		if (EscapeButtonManager.EscapeButonFunctionStack.Count == 0)
		{
			EscapeButtonManager.AddEscapeButonFunction("ButtonHomeClicked", string.Empty);
		}
	}

	public void ButtonHomeYesClicked()
	{
		GameData.SetFlavorsContainersList();
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
	}
}
