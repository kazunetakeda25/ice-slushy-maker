using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SplashScene : MonoBehaviour
{
	private int appStartedNumber;

	private AsyncOperation progress;

	private Image progressBar;

	private float myProgress;

	private string sceneToLoad;

	private void Start()
	{
		sceneToLoad = "HomeScene";
		progressBar = GameObject.Find("ProgressBar").GetComponent<Image>();
		if (PlayerPrefs.HasKey("appStartedNumber"))
		{
			appStartedNumber = PlayerPrefs.GetInt("appStartedNumber");
		}
		else
		{
			appStartedNumber = 0;
		}
		appStartedNumber++;
		PlayerPrefs.SetInt("appStartedNumber", appStartedNumber);
		StartCoroutine(LoadScene());
	}

	private IEnumerator LoadScene()
	{
		while ((double)myProgress < 0.99)
		{
			myProgress += 0.05f;
			progressBar.fillAmount = myProgress;
			yield return new WaitForSeconds(0.05f);
		}
		progress = Application.LoadLevelAsync(sceneToLoad);
		yield return progress;
	}

	private void Update()
	{
		if (progress != null && (double)progress.progress > 0.99)
		{
			progressBar.fillAmount = progress.progress * 2f;
		}
	}
}
