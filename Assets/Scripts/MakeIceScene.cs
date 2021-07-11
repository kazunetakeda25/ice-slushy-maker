using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MakeIceScene : MonoBehaviour
{
	private const float inchToCm = 2.54f;

	private float dragThresholdCM = 0.9f;

	public Transform MoldEndPosition;

	public Transform MoldEndPosition2;

	public Transform MoldEndPosition3;

	private Transform SelectedMold;

	public DragItem PitcherDragItem;

	public Transform PitcherEndPosition;

	public Transform FreezerEndPosition;

	public Transform Freezer;

	public ScrollRect scrollRect;

	public ParticleSystem psLevelCompleted;

	private float speed = 0.5f;

	private bool bNext;

	private bool bPrev;

	private float inertia;

	public GameObject ButtonScrollPrev;

	public GameObject ButtonScrollNext;

	private int phase;

	public Transform PopupAreYouSure;

	private IEnumerator Start()
	{
		DragItem.OneItemEnabledNo = 0;
		SetDragThreshold();
		EscapeButtonManager.AddEscapeButonFunction("ButtonHomeClicked", string.Empty);
		ButtonScrollPrev.SetActive(value: false);
		ButtonScrollNext.SetActive(value: false);
		scrollRect.horizontalNormalizedPosition = 1f;
		LevelTransition.Instance.ShowScene();
		yield return new WaitForSeconds(0.2f);
		StartCoroutine("NextPhase", 1);
		yield return new WaitForSeconds(0.8f);
		Tutorial.Instance.ShowTutorial(0);
	}

	private void Update()
	{
		if (phase != 1)
		{
			return;
		}
		if ((bPrev || inertia < -0.05f) && scrollRect.horizontalNormalizedPosition > 0f)
		{
			if (bPrev)
			{
				scrollRect.horizontalNormalizedPosition -= Time.deltaTime * speed;
				return;
			}
			scrollRect.horizontalNormalizedPosition += Time.deltaTime * inertia;
			inertia *= 0.95f;
		}
		else if ((bNext || inertia > 0.05f) && scrollRect.horizontalNormalizedPosition < 1f)
		{
			if (bNext)
			{
				scrollRect.horizontalNormalizedPosition += Time.deltaTime * speed;
				return;
			}
			scrollRect.horizontalNormalizedPosition += Time.deltaTime * inertia;
			inertia *= 0.95f;
		}
	}

	public void ButtonPrevDown()
	{
		if (SoundManager.Instance != null)
		{
			SoundManager.Instance.Play_ButtonClick();
		}
		bNext = false;
		bPrev = true;
		inertia = 0f;
	}

	public void ButtonPrevUp()
	{
		bNext = false;
		bPrev = false;
		inertia = 0f - speed;
	}

	public void ButtonNextDown()
	{
		if (SoundManager.Instance != null)
		{
			SoundManager.Instance.Play_ButtonClick();
		}
		bNext = true;
		bPrev = false;
		inertia = 0f;
	}

	public void ButtonNextUp()
	{
		bNext = false;
		bPrev = false;
		inertia = speed;
	}

	private void SetDragThreshold()
	{
		EventSystem.current.pixelDragThreshold = (int)(dragThresholdCM * Screen.dpi / 2.54f);
	}

	public void ScrollRectSelectMold_ButtonUp(BaseEventData baseEventData)
	{
		if (phase != 1)
		{
			return;
		}
		PointerEventData pointerEventData = (PointerEventData)baseEventData;
		if (!pointerEventData.dragging && SelectedMold == null && pointerEventData.pointerPressRaycast.gameObject.name == "Mold")
		{
			GameData.selectedMold = int.Parse(pointerEventData.pointerPressRaycast.gameObject.transform.parent.name.Remove(0, 4)) - 1;
			SelectedMold = pointerEventData.pointerPressRaycast.gameObject.transform;
			SelectedMold.SetParent(MoldEndPosition);
			StartCoroutine("NextPhase", 2);
			if (SoundManager.Instance != null)
			{
				SoundManager.Instance.Play_Sound(SoundManager.Instance.MoldSelected);
			}
			ButtonScrollPrev.SetActive(value: false);
			ButtonScrollNext.SetActive(value: false);
		}
	}

	private IEnumerator NextPhase(int _phase)
	{
		switch (_phase)
		{
		case 1:
		{
			yield return new WaitForSeconds(0.5f);
			float pos = 0f;
			scrollRect.horizontalNormalizedPosition = 1f;
			float timeMove2 = 0f;
			while (timeMove2 < 1f)
			{
				timeMove2 += Time.deltaTime * 0.5f;
				yield return new WaitForEndOfFrame();
				scrollRect.horizontalNormalizedPosition = Mathf.Lerp(1f, pos, timeMove2);
			}
			phase = 1;
			inertia = 0f;
			ButtonScrollPrev.SetActive(value: true);
			ButtonScrollNext.SetActive(value: true);
			break;
		}
		case 2:
		{
			phase = 2;
			Tutorial.Instance.StopTutorial();
			float timeMove5 = 0f;
			Vector3 arcMax2 = new Vector3(0f, 500f, 0f);
			Vector3 moldStartPos = SelectedMold.localPosition;
			while (timeMove5 < 1f || scrollRect.horizontalNormalizedPosition < 1.5f)
			{
				yield return new WaitForEndOfFrame();
				if (scrollRect.horizontalNormalizedPosition < 1.5f)
				{
					scrollRect.horizontalNormalizedPosition += Time.deltaTime;
				}
				if (timeMove5 < 1f)
				{
					timeMove5 += Time.deltaTime * scrollRect.horizontalNormalizedPosition;
					if (timeMove5 > 1f)
					{
						timeMove5 = 1f;
					}
					SelectedMold.localPosition = Vector3.Lerp(moldStartPos, Vector3.zero, timeMove5) + timeMove5 * (1f - timeMove5) * arcMax2;
					SelectedMold.localScale = (1f + 0.5f * timeMove5) * Vector3.one;
				}
			}
			scrollRect.gameObject.SetActive(value: false);
			SelectedMold.localPosition = Vector3.zero;
			PitcherDragItem.TargetPoint = new Transform[SelectedMold.childCount];
			DragItem.OneItemEnabledNo = 1;
			for (int i = 0; i < SelectedMold.childCount; i++)
			{
				PitcherDragItem.TargetPoint[i] = SelectedMold.GetChild(i);
			}
			Tutorial.Instance.ShowTutorial(1);
			break;
		}
		}
		if (_phase == 4)
		{
			float timeMove4 = 0f;
			Vector3 arcMax = new Vector3(0f, 5f, 0f);
			Vector3 pitcherStartPos = PitcherDragItem.transform.position;
			PitcherDragItem.enabled = false;
			while (timeMove4 < 1f)
			{
				yield return new WaitForEndOfFrame();
				timeMove4 += Time.deltaTime;
				PitcherDragItem.transform.position = Vector3.Lerp(pitcherStartPos, PitcherEndPosition.position, timeMove4) + timeMove4 * (1f - timeMove4) * arcMax;
			}
			yield return new WaitForEndOfFrame();
			UnityEngine.Object.Destroy(PitcherDragItem.gameObject);
			yield return new WaitForSeconds(1f);
			DragItem.OneItemEnabledNo = 2;
			timeMove4 = 0f;
			Vector3 freezerStartPos = Freezer.position;
			if (SoundManager.Instance != null)
			{
				SoundManager.Instance.Play_Sound(SoundManager.Instance.ShowFreezer);
			}
			while (timeMove4 < 1f)
			{
				yield return new WaitForEndOfFrame();
				timeMove4 += Time.deltaTime;
				Freezer.position = Vector3.Lerp(freezerStartPos, FreezerEndPosition.position, timeMove4) + timeMove4 * (1f - timeMove4) * arcMax;
			}
			DragItem mdi = SelectedMold.gameObject.AddComponent<DragItem>();
			mdi.TestPoint = SelectedMold;
			mdi.TargetPoint = new Transform[1]
			{
				MoldEndPosition2
			};
			mdi.bTestOnlyOnEndDrag = false;
			mdi.ItemNo = 2;
			mdi.snapToTarget = true;
			mdi.testDistance = 1.9f;
			mdi.animationType = "Mold";
			Tutorial.Instance.ShowTutorial(2);
		}
		if (_phase == 5)
		{
			UnityEngine.Object.Destroy(SelectedMold.GetComponent<DragItem>());
			Freezer.GetChild(0).GetComponent<Animator>().enabled = true;
			if (SoundManager.Instance != null)
			{
				SoundManager.Instance.Play_Sound(SoundManager.Instance.FreezerDoorClose);
			}
			yield return new WaitForSeconds(0.3f);
			if (SoundManager.Instance != null)
			{
				SoundManager.Instance.Play_Sound(SoundManager.Instance.FreezerOnSound);
			}
			yield return new WaitForSeconds(2.7f);
			for (int j = 0; j < SelectedMold.childCount; j++)
			{
				SelectedMold.GetChild(j).GetChild(1).gameObject.SetActive(value: true);
				SelectedMold.GetChild(j).GetChild(0).gameObject.SetActive(value: false);
			}
			yield return new WaitForSeconds(2f);
			if (SoundManager.Instance != null)
			{
				SoundManager.Instance.Play_Sound(SoundManager.Instance.FreezerDoorOpen);
			}
			yield return new WaitForSeconds(1f);
			float timeMove = 0f;
			Vector3 sScale = SelectedMold.localScale;
			Vector3 endScale = new Vector3(1.8f, 1.8f, 1f);
			while (timeMove < 1f)
			{
				yield return new WaitForEndOfFrame();
				timeMove += Time.deltaTime;
				SelectedMold.position = Vector3.Lerp(MoldEndPosition2.position, MoldEndPosition3.position, timeMove);
				SelectedMold.localScale = Vector3.Lerp(sScale, endScale, timeMove);
			}
			psLevelCompleted.gameObject.SetActive(value: true);
			psLevelCompleted.Play();
			if (SoundManager.Instance != null)
			{
				SoundManager.Instance.Play_Sound(SoundManager.Instance.ActionCompleted);
			}
			StartCoroutine("LevelCompleted");
		}
	}

	private IEnumerator LevelCompleted()
	{
		BlockClicks.Instance.SetBlockAll(blockRays: true);
		psLevelCompleted.gameObject.SetActive(value: true);
		psLevelCompleted.Play();
		if (SoundManager.Instance != null)
		{
			SoundManager.Instance.Play_Sound(SoundManager.Instance.ActionCompleted);
		}
		yield return new WaitForSeconds(3f);
		StopAllCoroutines();
		LevelTransition.Instance.HideSceneAndLoadNext("CutFruit");
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
		StartCoroutine(SetTimeScale(0f, 0f));
	}

	public void ButtonHomeYesClicked()
	{
		StartCoroutine(SetTimeScale(1f, 1f));
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
		StartCoroutine(SetTimeScale(1f, 1f));
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

	private IEnumerator SetTimeScale(float timeScale, float waitTime)
	{
		yield return new WaitForSecondsRealtime(waitTime);
		Time.timeScale = timeScale;
	}
}
