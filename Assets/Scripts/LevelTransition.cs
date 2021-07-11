using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
	public Animator anim;

	public static LevelTransition Instance;

	private static string nextLevelName = string.Empty;

	private bool bLoadScene;

	private void Start()
	{
		Object.DontDestroyOnLoad(base.gameObject);
		anim = GameObject.Find("TransitionImageC").GetComponent<Animator>();
		anim.gameObject.SetActive(value: false);
	}

	private void Awake()
	{
		if (nextLevelName != string.Empty && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "TransitionScene")
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene(nextLevelName);
		}
		else if (Instance != null && Instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else
		{
			Instance = this;
		}
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
		if (GameObject.Find("TransitionImageC") != null)
		{
			anim = GameObject.Find("TransitionImageC").GetComponent<Animator>();
			if (scene.name != "HomeScene" || !MenuManager.bFirstLoadMainScene)
			{
				anim.SetBool("bClose", value: true);
				anim.Play("DefaultClosed");
			}
		}
	}

	private IEnumerator WaitAndDestroy()
	{
		yield return new WaitForEndOfFrame();
		if (Instance.anim == null)
		{
			Instance.anim = GameObject.Find("TransitionImageC").GetComponent<Animator>();
		}
		Instance.anim.gameObject.SetActive(value: false);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void HideSceneAndLoadNext(string levelName)
	{
		if (!bLoadScene)
		{
			bLoadScene = true;
			nextLevelName = levelName;
			StopAllCoroutines();
			BlockClicks.Instance.SetBlockAll(blockRays: true);
			anim.gameObject.SetActive(value: true);
			StartCoroutine("LoadScene", levelName);
			if (anim != null)
			{
				anim.SetBool("bClose", value: true);
			}
		}
	}

	public void ShowScene()
	{
		if (anim == null)
		{
			anim = GameObject.Find("TransitionImageC").GetComponent<Animator>();
		}
		if (anim != null)
		{
			anim.SetBool("bClose", value: false);
		}
	}

	private IEnumerator LoadScene(string levelName)
	{
		if (SoundManager.Instance != null)
		{
			SoundManager.Instance.StopActiveSoundsOnExitAndClearList();
		}
		yield return new WaitForSeconds(1.2f);
		bLoadScene = false;
		UnityEngine.SceneManagement.SceneManager.LoadScene("TransitionScene");
	}

	public void HideAndShowSceneWithoutLoading()
	{
		StopAllCoroutines();
		BlockClicks.Instance.SetBlockAll(blockRays: true);
		anim.gameObject.SetActive(value: true);
		anim.SetBool("bClose", value: true);
		StartCoroutine("WaitHideAndShowScene");
	}

	private IEnumerator WaitHideAndShowScene()
	{
		yield return new WaitForSeconds(2f);
		anim.SetBool("bClose", value: false);
		BlockClicks.Instance.SetBlockAll(blockRays: true);
		BlockClicks.Instance.SetBlockAllDelay(1f, blockRays: false);
	}

	public void AnimEventHideSceneAnimStarted()
	{
	}

	public void AnimEventShowSceneAnimFinished()
	{
		anim.gameObject.SetActive(value: false);
	}
}
