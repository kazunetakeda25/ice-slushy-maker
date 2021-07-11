using System.Collections;
using UnityEngine;

public class CutFruitScene : MonoBehaviour
{
	private Transform[] KnifeStartPosition;

	private Transform[] KnifeEndPosition;

	private int cutNo;

	private int maxCuts = 1;

	private int[] maxFruits = new int[10]
	{
		3,
		2,
		2,
		5,
		2,
		5,
		4,
		2,
		2,
		2
	};

	private int fruitNo = 1;

	public Transform knife;

	public Transform[] Fruits;

	private Transform Fruit;

	public int selectedFruit = 1;

	private Animator animFruit;

	private Vector3 startPos;

	private Vector3 endPos;

	private Vector3 knifeCurrentPos;

	private Vector3 knifeStartPos;

	private Vector3 fruitStartPos;

	private float deltaPosY;

	private float relativePosY;

	public Transform fruitCutPosition;

	public Transform fruitHidePosition;

	private bool bMoveKnife;

	private Transform startParent;

	public ParticleSystem psLevelCompleted;

	public Transform PopupAreYouSure;

	private IEnumerator Start()
	{
		selectedFruit = GameData.selectedFlavor - 1;
		Fruit = Fruits[selectedFruit];
		startParent = knife.parent;
		fruitStartPos = Fruit.position;
		knifeStartPos = knife.position;
		Knife.bEnableDrag = false;
		animFruit = Fruit.GetComponent<Animator>();
		maxCuts = (Fruit.childCount - 1) / 3;
		KnifeStartPosition = new Transform[maxCuts];
		KnifeEndPosition = new Transform[maxCuts];
		for (int i = 1; i <= maxCuts; i++)
		{
			KnifeStartPosition[i - 1] = Fruit.Find("KnifeStartPos" + i.ToString());
			KnifeEndPosition[i - 1] = Fruit.Find("KnifeEndPos" + i.ToString());
		}
		LevelTransition.Instance.ShowScene();
		yield return new WaitForSeconds(1f);
		cutNo = 1;
		StartCoroutine("ShowFruit");
		yield return new WaitForSeconds(1.1f);
		StartCoroutine("SetKnifeToStartPosition");
		yield return new WaitForSeconds(1f);
		Tutorial.Instance.ShowTutorial(0);
		EscapeButtonManager.AddEscapeButonFunction("ButtonHomeClicked", string.Empty);
	}

	private void Update()
	{
		if (bMoveKnife)
		{
			knife.position = Vector3.Lerp(knife.position, knifeCurrentPos, 8f * Time.deltaTime);
		}
	}

	public void MoveKnife(float mousePosY)
	{
		if (!bMoveKnife)
		{
			return;
		}
		Tutorial.Instance.StopTutorial();
		relativePosY = mousePosY - endPos.y;
		float t = 1f - relativePosY / deltaPosY;
		knifeCurrentPos = Vector3.Lerp(startPos, endPos, t);
		Vector3 position = knife.position;
		if (position.y <= endPos.y + 0.1505f)
		{
			animFruit.Play("cut" + cutNo.ToString());
			StartCoroutine("SetKnifeToStartPosition");
			cutNo++;
			if (SoundManager.Instance != null)
			{
				SoundManager.Instance.Play_Sound(SoundManager.Instance.KnifeCutSound);
			}
		}
	}

	private IEnumerator SetKnifeToStartPosition()
	{
		yield return new WaitForEndOfFrame();
		if (cutNo <= maxCuts)
		{
			Knife.bEnableDrag = false;
			bMoveKnife = false;
			if (cutNo == 1)
			{
				startPos = knife.position;
			}
			else
			{
				startPos = KnifeEndPosition[cutNo - 2].position;
			}
			endPos = KnifeStartPosition[cutNo - 1].position;
			Vector3 midPos = (cutNo != 1) ? KnifeStartPosition[cutNo - 2].position : KnifeStartPosition[cutNo - 1].position;
			float timeMove4 = 0f;
			while (timeMove4 < 0.7f)
			{
				timeMove4 += Time.deltaTime;
				yield return new WaitForEndOfFrame();
				knife.position = Vector3.Lerp(startPos, midPos, timeMove4 * 1.4f);
			}
			startPos = knife.position;
			timeMove4 = 0f;
			knife.SetParent(KnifeStartPosition[cutNo - 1]);
			while (timeMove4 < 1f)
			{
				timeMove4 += Time.deltaTime * 4f;
				yield return new WaitForEndOfFrame();
				knife.position = Vector3.Lerp(startPos, endPos, timeMove4);
			}
			startPos = KnifeStartPosition[cutNo - 1].position;
			endPos = KnifeEndPosition[cutNo - 1].position;
			deltaPosY = startPos.y - endPos.y;
			knifeCurrentPos = startPos;
			bMoveKnife = true;
			Knife.bEnableDrag = true;
		}
		else if (fruitNo < maxFruits[selectedFruit])
		{
			fruitNo++;
			bMoveKnife = false;
			Knife.bEnableDrag = false;
			float timeMove2 = 0f;
			Vector3 kp2 = knife.position;
			while (timeMove2 < 1f)
			{
				timeMove2 += Time.deltaTime;
				yield return new WaitForEndOfFrame();
				knife.position = Vector3.Lerp(kp2, KnifeStartPosition[0].position, timeMove2);
			}
			knife.SetParent(startParent);
			startPos = KnifeStartPosition[0].position;
			endPos = KnifeEndPosition[0].position;
			knifeCurrentPos = startPos;
			cutNo = 1;
			yield return new WaitForSeconds(0.1f);
			StartCoroutine("HideFruit");
			yield return new WaitForSeconds(1f);
			animFruit.Play("Default");
			StartCoroutine("ShowFruit");
			yield return new WaitForSeconds(1.1f);
			knife.SetParent(KnifeStartPosition[cutNo - 1]);
			bMoveKnife = true;
			Knife.bEnableDrag = true;
		}
		else
		{
			bMoveKnife = false;
			Knife.bEnableDrag = false;
			float timeMove = 0f;
			Vector3 kp = knife.position;
			while (timeMove < 1f)
			{
				timeMove += Time.deltaTime;
				yield return new WaitForEndOfFrame();
				knife.position = Vector3.Lerp(kp, KnifeStartPosition[0].position, timeMove);
			}
			startPos = KnifeStartPosition[0].position;
			knifeCurrentPos = startPos;
			knife.SetParent(startParent);
			StartCoroutine("HideFruit");
			yield return new WaitForSeconds(1f);
			StartCoroutine("LevelCompleted");
		}
	}

	private IEnumerator ShowFruit()
	{
		Fruit.gameObject.SetActive(value: true);
		float timeMove = 0f;
		Vector3 arcMax = new Vector3(0f, 5f, 0f);
		bool bPlaySound = true;
		while (timeMove < 1f)
		{
			timeMove += Time.deltaTime;
			yield return new WaitForEndOfFrame();
			Fruit.position = Vector3.Lerp(fruitStartPos, fruitCutPosition.position, timeMove) + timeMove * (1f - timeMove) * arcMax;
			if (bPlaySound && timeMove > 0.3f && SoundManager.Instance != null)
			{
				bPlaySound = false;
				SoundManager.Instance.Play_Sound(SoundManager.Instance.ShowCup);
			}
		}
	}

	private IEnumerator HideFruit()
	{
		Fruit.gameObject.SetActive(value: true);
		float timeMove = 0f;
		Vector3 arcMax = new Vector3(0f, 5f, 0f);
		while (timeMove < 1f)
		{
			timeMove += Time.deltaTime;
			yield return new WaitForEndOfFrame();
			Fruit.position = Vector3.Lerp(fruitCutPosition.position, fruitHidePosition.position, timeMove) + timeMove * (1f - timeMove) * arcMax;
		}
	}

	private IEnumerator LevelCompleted()
	{
		BlockClicks.Instance.SetBlockAll(blockRays: true);
		yield return new WaitForSeconds(1f);
		StopAllCoroutines();
		LevelTransition.Instance.HideSceneAndLoadNext("MixSlush");
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
