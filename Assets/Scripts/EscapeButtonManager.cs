using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeButtonManager : MonoBehaviour
{
	private bool bDisableEsc;

	public static Stack<string> EscapeButonFunctionStack = new Stack<string>();

	private void Start()
	{
		Object.DontDestroyOnLoad(base.gameObject);
	}

	private void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void OnDisable()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		EscapeButonFunctionStack.Clear();
		bDisableEsc = false;
		if (scene.name == "HomeScene")
		{
			AddEscapeButonFunction("ExitGame", string.Empty);
		}
	}

	public static void AddEscapeButonFunction(string functionName, string functionParam = "")
	{
		if (functionParam != string.Empty)
		{
			functionName = functionName + "*" + functionParam;
		}
		EscapeButonFunctionStack.Push(functionName);
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.P))
		{
		}
		if (bDisableEsc || !Input.GetKeyDown(KeyCode.Escape))
		{
			return;
		}
		if (EscapeButonFunctionStack.Count > 0)
		{
			bDisableEsc = true;
			if (EscapeButonFunctionStack.Peek().Contains("*"))
			{
				string[] array = EscapeButonFunctionStack.Peek().Split('*');
				if (array[0] == "ClosePopUpMenuEsc")
				{
					if (GameObject.Find("Canvas/Menus") != null)
					{
						GameObject.Find("Canvas/Menus").transform.parent.SendMessage(array[0], array[1], SendMessageOptions.DontRequireReceiver);
					}
				}
				else
				{
					Camera.main.SendMessage(array[0], array[1], SendMessageOptions.DontRequireReceiver);
					EscapeButonFunctionStack.Pop();
				}
			}
			else if (EscapeButonFunctionStack.Count == 1 && EscapeButonFunctionStack.Peek() == "btnPauseClick")
			{
				Camera.main.SendMessage("btnPauseClick", SendMessageOptions.DontRequireReceiver);
			}
			else if (EscapeButonFunctionStack.Count >= 1 && EscapeButonFunctionStack.Peek() == "CloseDailyReward")
			{
				GameObject.Find("PopUps/DailyReward").GetComponent<DailyRewards>().Collect();
			}
			else
			{
				Camera.main.SendMessage(EscapeButonFunctionStack.Pop(), SendMessageOptions.DontRequireReceiver);
			}
		}
		StartCoroutine("DisableEsc");
	}

	private IEnumerator DisableEsc()
	{
		yield return new WaitForSeconds(2f);
		bDisableEsc = false;
	}
}
