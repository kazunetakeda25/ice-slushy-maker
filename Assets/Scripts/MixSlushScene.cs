using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MixSlushScene : MonoBehaviour
{
	public DishScript dishFruits;

	public DishScript dishIce;

	public Animator animBlender;

	public Animator animTutorialRotate;

	private bool bIceIn;

	private bool bFruitsIn;

	public BlenderHandle blenderHandle;

	public Image imgSlush;

	public ParticleSystem psLevelCompleted;

	public Transform PopupAreYouSure;

	private void Start()
	{
		Color selectedFlavorColor = GameData.selectedFlavorColor;
		imgSlush.color = new Color(selectedFlavorColor.r, selectedFlavorColor.g, selectedFlavorColor.b, 0f);
		LevelTransition.Instance.ShowScene();
		Tutorial.Instance.ShowTutorial(0);
		EscapeButtonManager.AddEscapeButonFunction("ButtonHomeClicked", string.Empty);
		if (SoundManager.Instance != null)
		{
			SoundManager.Instance.listStopSoundOnExit.Add(SoundManager.Instance.Blender);
		}
	}

	private void Update()
	{
	}

	public void DishUsed(int dishNo)
	{
		switch (dishNo)
		{
		case 0:
			bIceIn = true;
			break;
		case 1:
			bFruitsIn = true;
			break;
		}
		if (bFruitsIn && bIceIn)
		{
			StartCoroutine("HideDishesAndShowBlender");
		}
		else if (bIceIn)
		{
			Tutorial.Instance.ShowTutorial(0);
		}
		else
		{
			Tutorial.Instance.ShowTutorial(1);
		}
	}

	private IEnumerator HideDishesAndShowBlender()
	{
		yield return new WaitForSeconds(1f);
		animBlender.Play("ShowBlenderBase");
		yield return new WaitForSeconds(2f);
		blenderHandle.enabled = true;
		animTutorialRotate.Play("ShowCircle");
	}

	public void MixingOver()
	{
		psLevelCompleted.gameObject.SetActive(value: true);
		psLevelCompleted.Play();
		if (SoundManager.Instance != null)
		{
			SoundManager.Instance.Play_Sound(SoundManager.Instance.ActionCompleted);
		}
		StartCoroutine("LevelCompleted");
	}

	private IEnumerator LevelCompleted()
	{
		BlockClicks.Instance.SetBlockAll(blockRays: true);
		GameData.bFillFlavor = true;
		psLevelCompleted.gameObject.SetActive(value: true);
		psLevelCompleted.Play();
		if (SoundManager.Instance != null)
		{
			SoundManager.Instance.Play_Sound(SoundManager.Instance.ActionCompleted);
		}
		yield return new WaitForSeconds(3f);
		StopAllCoroutines();
		LevelTransition.Instance.HideSceneAndLoadNext("SelectFlavor");
		BlockClicks.Instance.SetBlockAll(blockRays: true);
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
