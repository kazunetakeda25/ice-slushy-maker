using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
	public bool bTutorialFinished;

	public Animator animTutorial;

	public Animator animTutorialHolder;

	private int phase = -1;

	public Transform[] tutStartPos;

	public Transform[] tutEndPos;

	private bool bActive;

	private float repeatTime = 3f;

	private float lefttTimeToRepeat;

	private string lastTutorial = string.Empty;

	private Vector3 RepStartPosition;

	public static Tutorial Instance;

	private void Awake()
	{
		Instance = this;
	}

	public void ShowPointer()
	{
		animTutorialHolder.CrossFade("ShowTutorial", 0.05f);
	}

	public void HidePointer()
	{
		if (animTutorialHolder.GetComponent<CanvasGroup>().alpha > 0.05f)
		{
			animTutorialHolder.CrossFade("HideTutorial", 0.05f);
		}
	}

	public void ShowPointerAndMoveToPosition(int tutorialPhase)
	{
		phase = tutorialPhase;
		StopAllCoroutines();
		CancelInvoke("RepeatTutorial");
		lastTutorial = string.Empty;
		StartCoroutine("CShowPointerAndMoveToPosition", 0);
	}

	public void ShowPointerAndMoveToPosition(int tutorialPhase, float delay)
	{
		phase = tutorialPhase;
		StopAllCoroutines();
		CancelInvoke("RepeatTutorial");
		lastTutorial = string.Empty;
		StartCoroutine("CShowPointerAndMoveToPosition", delay);
	}

	private IEnumerator CShowPointerAndMoveToPosition(float delay)
	{
		if (bActive)
		{
			HidePointer();
			yield return new WaitForSeconds(0.1f);
		}
		yield return new WaitForSeconds(0.1f);
		animTutorial.Play("default");
		yield return new WaitForSeconds(delay);
		animTutorial.transform.localScale = tutStartPos[phase].localScale;
		animTutorial.transform.position = tutStartPos[phase].position;
		bActive = true;
		ShowPointer();
		yield return new WaitForSeconds(0.2f);
		animTutorial.CrossFade("pointerDown", 0.05f);
		yield return new WaitForSeconds(1f);
		float speed = 1.3f;
		float timeMove = 0f;
		while (timeMove < 1f)
		{
			timeMove += speed * Time.fixedDeltaTime;
			animTutorial.transform.position = Vector3.Lerp(tutStartPos[phase].position, tutEndPos[phase].position, timeMove);
			yield return new WaitForFixedUpdate();
		}
		animTutorial.transform.position = tutEndPos[phase].position;
		yield return new WaitForSeconds(0.3f);
		HidePointer();
		yield return new WaitForSeconds(0.2f);
		bActive = false;
		lastTutorial = "MoveToPosition";
		InvokeRepeating("RepeatTutorial", 0.5f, 0.5f);
	}

	public void ShowPointerAndTapOnPosition(int tutorialPhase)
	{
		phase = tutorialPhase;
		StopAllCoroutines();
		CancelInvoke("RepeatTutorial");
		lastTutorial = string.Empty;
		StartCoroutine("CShowPointerAndTapOnPosition", 0);
	}

	public void ShowPointerAndTapOnPosition(int tutorialPhase, float delay)
	{
		phase = tutorialPhase;
		StopAllCoroutines();
		CancelInvoke("RepeatTutorial");
		lastTutorial = string.Empty;
		StartCoroutine("CShowPointerAndTapOnPosition", delay);
	}

	private IEnumerator CShowPointerAndTapOnPosition(float delay)
	{
		if (bActive)
		{
			HidePointer();
			yield return new WaitForSeconds(0.1f);
		}
		yield return new WaitForSeconds(0.1f);
		animTutorial.Play("default");
		yield return new WaitForSeconds(delay);
		animTutorial.transform.localScale = tutStartPos[phase].localScale;
		animTutorial.transform.position = tutStartPos[phase].position;
		bActive = true;
		ShowPointer();
		yield return new WaitForSeconds(0.8f);
		animTutorial.CrossFade("pointerTap", 0.05f);
		yield return new WaitForSeconds(1f);
		HidePointer();
		yield return new WaitForSeconds(0.2f);
		bActive = false;
		lastTutorial = "TapOnPosition";
		InvokeRepeating("RepeatTutorial", 0.5f, 0.5f);
	}

	public void ShowPointerAndMoveRepeating(int tutorialPhase, float dly)
	{
		phase = tutorialPhase;
		StopAllCoroutines();
		CancelInvoke("RepeatTutorial");
		lastTutorial = string.Empty;
		StartCoroutine(CShowPointerAndMoveRepeating(tutStartPos[phase].position, dly));
	}

	public void ShowPointerAndMoveRepeating(int tutorialPhase, Vector3 StartPosition, float dly)
	{
		phase = tutorialPhase;
		StopAllCoroutines();
		CancelInvoke("RepeatTutorial");
		lastTutorial = string.Empty;
		StartCoroutine(CShowPointerAndMoveRepeating(StartPosition, dly));
	}

	private IEnumerator CShowPointerAndMoveRepeating(Vector3 StartPosition, float delay)
	{
		Vector3 _StartPosition = StartPosition;
		if (bActive)
		{
			HidePointer();
			yield return new WaitForSeconds(0.1f);
		}
		yield return new WaitForSeconds(0.1f);
		animTutorial.Play("default");
		yield return new WaitForSeconds(delay);
		animTutorial.transform.localScale = tutStartPos[phase].localScale;
		animTutorial.transform.position = StartPosition;
		bActive = true;
		ShowPointer();
		yield return new WaitForSeconds(0.2f);
		animTutorial.CrossFade("pointerDown", 0.05f);
		yield return new WaitForSeconds(1f);
		float speed = 1.3f;
		int repeatCycles = 2;
		for (int i = 0; i < repeatCycles; i++)
		{
			float timeMove = 0f;
			while (timeMove < 1f)
			{
				timeMove += speed * Time.fixedDeltaTime;
				animTutorial.transform.position = Vector3.Lerp(_StartPosition, tutEndPos[phase].position, timeMove);
				yield return new WaitForFixedUpdate();
			}
			animTutorial.transform.position = tutEndPos[phase].position;
			_StartPosition = tutStartPos[phase].position;
			while (timeMove > 0f)
			{
				timeMove -= speed * Time.fixedDeltaTime;
				animTutorial.transform.position = Vector3.Lerp(tutStartPos[phase].position, tutEndPos[phase].position, timeMove);
				yield return new WaitForFixedUpdate();
			}
		}
		yield return new WaitForSeconds(0.3f);
		HidePointer();
		yield return new WaitForSeconds(0.2f);
		bActive = false;
		lastTutorial = "MoveRepeating";
		RepStartPosition = StartPosition;
		InvokeRepeating("RepeatTutorial", 0.5f, 0.5f);
	}

	public void RepeatTutorial()
	{
		if (lefttTimeToRepeat < repeatTime)
		{
			lefttTimeToRepeat += 0.5f;
			return;
		}
		lefttTimeToRepeat = 0f;
		if (lastTutorial == "MoveToPosition")
		{
			ShowPointerAndMoveToPosition(phase);
		}
		else if (lastTutorial == "TapOnPosition")
		{
			ShowPointerAndTapOnPosition(phase);
		}
		else if (lastTutorial == "MoveRepeating")
		{
			ShowPointerAndMoveRepeating(phase, RepStartPosition, 0f);
		}
	}

	public void SwitchState()
	{
		if (bActive)
		{
			StopAllCoroutines();
			HidePointer();
			bActive = false;
		}
		else
		{
			ShowTutorial(phase);
		}
	}

	public void ShowTutorial(int _phase)
	{
		string name = SceneManager.GetActiveScene().name;
		if (name == "CupDecoration")
		{
			float delay = 1f;
			if (_phase == 0)
			{
				ShowPointerAndTapOnPosition(0, delay);
			}
		}
		else if (name == "CutFruit")
		{
			float delay2 = 0f;
			if (_phase == 0)
			{
				ShowPointerAndMoveToPosition(0, delay2);
			}
		}
		else if (name == "DrinkSlush")
		{
			float delay3 = 0f;
			if (_phase == 0)
			{
				ShowPointerAndTapOnPosition(0, delay3);
			}
		}
		else if (name == "MakeIce")
		{
			float delay4 = 1f;
			switch (_phase)
			{
			case 0:
				ShowPointerAndTapOnPosition(0, delay4);
				break;
			case 1:
				ShowPointerAndMoveToPosition(1, delay4);
				break;
			case 2:
				ShowPointerAndMoveToPosition(2, delay4);
				break;
			}
		}
		else if (name == "MixSlush")
		{
			float delay5 = 0f;
			if (_phase == 0)
			{
				ShowPointerAndMoveToPosition(0, delay5);
			}
			if (_phase == 1)
			{
				ShowPointerAndMoveToPosition(1, delay5);
			}
		}
		else if (name == "SlushDecoration")
		{
			float delay6 = 0f;
			if (_phase == 0)
			{
				ShowPointerAndTapOnPosition(0, delay6);
			}
			if (_phase == 1)
			{
				ShowPointerAndTapOnPosition(1, delay6);
			}
			if (_phase == 2)
			{
				ShowPointerAndMoveToPosition(2, delay6);
			}
		}
	}

	public void StopTutorial()
	{
		CancelInvoke("RepeatTutorial");
		lastTutorial = string.Empty;
		StopAllCoroutines();
		HidePointer();
		bActive = false;
	}

	public void PauseTutorial(string state)
	{
	}
}
