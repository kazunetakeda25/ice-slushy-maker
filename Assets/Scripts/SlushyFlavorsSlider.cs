using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlushyFlavorsSlider : MonoBehaviour
{
	private bool bEnableMove = true;

	private const float inchToCm = 2.54f;

	private float dragThresholdCM = 0.5f;

	public ScrollRect scrollRect;

	public RectTransform scrollContetnt;

	private int selectedFlavor = -1;

	private int itemNo = 2;

	public float horizontalStep;

	private bool bDrag;

	private bool bInertia;

	private bool bSnapToPlace;

	private int childCount;

	private Image activeHandle;

	public Sprite[] LockContainerGraphis;

	public Sprite[] UnlockContainerGraphis;

	private bool bHandleDown;

	private int step;

	private IEnumerator Start()
	{
		childCount = scrollContetnt.transform.childCount;
		SetFlavorContainers();
		selectedFlavor = GameData.selectedFlavor;
		if (selectedFlavor == -1)
		{
			selectedFlavor = 2;
			itemNo = 2;
		}
		horizontalStep = 1f / (float)childCount;
		SetSelectedItem();
		scrollRect.horizontalNormalizedPosition = ((float)itemNo - 0.5f) * horizontalStep;
		yield return new WaitForSeconds(2f);
		bSnapToPlace = true;
	}

	private void SetSelectedFlavor()
	{
		if (selectedFlavor == -1)
		{
			selectedFlavor = 2;
			itemNo = 2;
		}
	}

	private void SetSelectedItem()
	{
		itemNo = selectedFlavor;
	}

	public void EnableMove(bool _bEnableMove)
	{
		bEnableMove = _bEnableMove;
		if (!_bEnableMove)
		{
			scrollRect.enabled = false;
		}
		else
		{
			scrollRect.enabled = true;
		}
	}

	private void Update()
	{
		if (!bEnableMove || bDrag)
		{
			return;
		}
		if (!bSnapToPlace && bInertia)
		{
			Vector2 velocity = scrollRect.velocity;
			if (velocity.x < 100f)
			{
				Vector2 velocity2 = scrollRect.velocity;
				if (velocity2.x > -100f)
				{
					goto IL_0091;
				}
			}
			if (scrollRect.horizontalNormalizedPosition <= 0.02f || scrollRect.horizontalNormalizedPosition >= 0.98f)
			{
				goto IL_0091;
			}
		}
		if (!bSnapToPlace)
		{
			return;
		}
		scrollRect.horizontalNormalizedPosition = Mathf.Lerp(scrollRect.horizontalNormalizedPosition, ((float)itemNo - 0.5f) * horizontalStep, 2f * Time.deltaTime);
		float num = Mathf.Abs(scrollRect.horizontalNormalizedPosition - ((float)itemNo - 0.5f) * horizontalStep);
		if (!(num < 0.01f))
		{
			return;
		}
		if (num < 0.001f)
		{
			bSnapToPlace = false;
		}
		if (selectedFlavor > 0)
		{
			activeHandle = scrollContetnt.transform.Find("Item" + selectedFlavor.ToString().PadLeft(2, '0') + "/HandleBG").GetComponent<Image>();
			activeHandle.raycastTarget = true;
			return;
		}
		if (activeHandle != null)
		{
			activeHandle.raycastTarget = false;
		}
		activeHandle = null;
		return;
		IL_0091:
		bSnapToPlace = true;
		bInertia = false;
		Vector2 velocity3 = scrollRect.velocity;
		if (velocity3.x > 0f)
		{
			itemNo = Mathf.FloorToInt(scrollRect.horizontalNormalizedPosition / horizontalStep) + 1;
			if (itemNo < 1)
			{
				itemNo = 1;
			}
		}
		else
		{
			itemNo = Mathf.FloorToInt(scrollRect.horizontalNormalizedPosition / horizontalStep) + 1;
			if (itemNo > childCount)
			{
				itemNo = childCount;
			}
		}
		selectedFlavor = itemNo;
	}

	public void HandleDown()
	{
		if (!bEnableMove || !(activeHandle != null))
		{
			return;
		}
		if (GameData.FlavorsContainersList[selectedFlavor - 1] > 0)
		{
			Camera.main.SendMessage("StartFillingCup", selectedFlavor - 1);
			activeHandle.GetComponent<Animator>().CrossFade("HandleDown", 0.1f);
			bHandleDown = true;
		}
		else if (GameData.FlavorsContainersList[selectedFlavor - 1] == 0)
		{
			if (SoundManager.Instance != null)
			{
				SoundManager.Instance.Stop_Sound(SoundManager.Instance.FillCup);
				SoundManager.Instance.Play_Sound(SoundManager.Instance.EmptyFlavor);
			}
			StartCoroutine("ContainerPulse");
			GameData.selectedFlavor = selectedFlavor;
			GameData.selectedFlavorColor = scrollContetnt.transform.Find("Item" + selectedFlavor.ToString().PadLeft(2, '0') + "/SlushyMask/Slushy").GetComponent<Image>().color;
			Camera.main.SendMessage("LoadCutFruitScene");
		}
		else
		{
			if (SoundManager.Instance != null)
			{
				SoundManager.Instance.Play_Sound(SoundManager.Instance.UnlockFlavor);
			}
			activeHandle.transform.parent.GetComponent<Animator>().CrossFade("EmptyContainerClicked", 0.1f);
			Shop.Instance.WatchVideo();
		}
	}

	public void HandleUp()
	{
		if (bEnableMove && activeHandle != null && bHandleDown)
		{
			bHandleDown = false;
			activeHandle.GetComponent<Animator>().CrossFade("HandleUp", 0.1f);
			Camera.main.SendMessage("StopFillingCup");
		}
	}

	public void ButonWatchVideoClicked()
	{
		if (bEnableMove && activeHandle != null)
		{
			if (SoundManager.Instance != null)
			{
				SoundManager.Instance.Play_Sound(SoundManager.Instance.UnlockFlavor);
			}
			activeHandle.transform.parent.GetComponent<Animator>().CrossFade("EmptyContainerClicked", 0.1f);
			Shop.Instance.WatchVideo();
		}
	}

	private IEnumerator ContainerPulse()
	{
		float timeP2 = 0f;
		Vector3 StartPos = activeHandle.transform.parent.localPosition;
		Vector3 EndPos = activeHandle.transform.parent.localPosition + new Vector3(0f, 30f, 0f);
		while (timeP2 < 1f)
		{
			yield return new WaitForEndOfFrame();
			timeP2 += Time.deltaTime * 4f;
			activeHandle.transform.parent.localScale = Vector3.Lerp(Vector3.one, 1.1f * Vector3.one, timeP2);
			activeHandle.transform.parent.localPosition = Vector3.Lerp(StartPos, EndPos, timeP2);
		}
		timeP2 = 0f;
		while (timeP2 < 1f)
		{
			yield return new WaitForEndOfFrame();
			timeP2 += Time.deltaTime * 4f;
			activeHandle.transform.parent.localScale = Vector3.Lerp(1.1f * Vector3.one, Vector3.one, timeP2);
			activeHandle.transform.parent.localPosition = Vector3.Lerp(EndPos, StartPos, timeP2);
		}
		yield return new WaitForSeconds(1f);
	}

	public void UnlockFlavor()
	{
		scrollContetnt.transform.Find("Item" + selectedFlavor.ToString().PadLeft(2, '0') + "/iconWatchVideo").GetComponent<Image>().enabled = false;
		scrollContetnt.transform.Find("Item" + selectedFlavor.ToString().PadLeft(2, '0') + "/SlushyMask/Slushy").GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -300f);
		StartCoroutine("ContainerPulse");
		GameData.FlavorsContainersList[selectedFlavor - 1] = 0;
		GameData.SetFlavorsContainersList();
	}

	private void SetDragThreshold()
	{
		EventSystem.current.pixelDragThreshold = (int)(dragThresholdCM * Screen.dpi / 2.54f);
	}

	public void OnBeginDrag(BaseEventData eventData)
	{
		if (bEnableMove)
		{
			if (activeHandle != null)
			{
				activeHandle.raycastTarget = true;
				activeHandle = null;
			}
			bDrag = true;
			bInertia = false;
			bSnapToPlace = false;
		}
	}

	public void OnEndDrag(BaseEventData eventData)
	{
		if (bEnableMove)
		{
			bDrag = false;
			bInertia = true;
		}
	}

	public void SetFlavorContainers()
	{
		for (int i = 0; i < 10; i++)
		{
			int num = GameData.FlavorsContainersList[i];
			if (num == -1)
			{
				scrollContetnt.transform.Find("Item" + (i + 1).ToString().PadLeft(2, '0') + "/iconWatchVideo").GetComponent<Image>().enabled = true;
				scrollContetnt.transform.Find("Item" + (i + 1).ToString().PadLeft(2, '0') + "/SlushyMask/Slushy").GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -300f);
				continue;
			}
			scrollContetnt.transform.Find("Item" + (i + 1).ToString().PadLeft(2, '0') + "/iconWatchVideo").GetComponent<Image>().enabled = false;
			if (num > 0)
			{
				scrollContetnt.transform.Find("Item" + (i + 1).ToString().PadLeft(2, '0') + "/Top").GetComponent<Image>().sprite = UnlockContainerGraphis[0];
				scrollContetnt.transform.Find("Item" + (i + 1).ToString().PadLeft(2, '0') + "/Bottom").GetComponent<Image>().sprite = UnlockContainerGraphis[1];
			}
			scrollContetnt.transform.Find("Item" + (i + 1).ToString().PadLeft(2, '0') + "/SlushyMask/Slushy").GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -300 + 3 * num);
		}
	}

	public void FillSlushFlavorFinished()
	{
		int num = GameData.selectedFlavor;
		GameData.FlavorsContainersList[num - 1] = 99;
		GameData.SetFlavorsContainersList();
		GameData.bFillFlavor = false;
		scrollContetnt.transform.Find("Item" + num.ToString().PadLeft(2, '0') + "/SlushyMask/Slushy").GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
		scrollContetnt.transform.Find("Item" + num.ToString().PadLeft(2, '0') + "/Top").GetComponent<Image>().sprite = UnlockContainerGraphis[0];
		scrollContetnt.transform.Find("Item" + num.ToString().PadLeft(2, '0') + "/Bottom").GetComponent<Image>().sprite = UnlockContainerGraphis[1];
	}

	public void DecreaseSlushyInContainer()
	{
		step++;
		if (step != 7)
		{
			return;
		}
		step = 0;
		if (GameData.FlavorsContainersList[selectedFlavor - 1] > 0)
		{
			GameData.FlavorsContainersList[selectedFlavor - 1]--;
		}
		int num = GameData.FlavorsContainersList[selectedFlavor - 1];
		if (GameData.FlavorsContainersList[selectedFlavor - 1] == 0)
		{
			scrollContetnt.transform.Find("Item" + selectedFlavor.ToString().PadLeft(2, '0') + "/Top").GetComponent<Image>().sprite = LockContainerGraphis[0];
			scrollContetnt.transform.Find("Item" + selectedFlavor.ToString().PadLeft(2, '0') + "/Bottom").GetComponent<Image>().sprite = LockContainerGraphis[1];
			SoundManager.Instance.Stop_Sound(SoundManager.Instance.FillCup);
			Camera.main.SendMessage("StopFillingCup");
			if (activeHandle != null && bHandleDown)
			{
				bHandleDown = false;
				activeHandle.GetComponent<Animator>().CrossFade("HandleUp", 0.1f);
			}
		}
		scrollContetnt.transform.Find("Item" + selectedFlavor.ToString().PadLeft(2, '0') + "/SlushyMask/Slushy").GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -300 + 3 * num);
	}
}
