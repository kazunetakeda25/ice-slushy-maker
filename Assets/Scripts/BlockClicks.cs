using System.Collections;
using UnityEngine;

public class BlockClicks : MonoBehaviour
{
	public static BlockClicks Instance;

	private CanvasGroup BlockAll;

	private void Awake()
	{
		Instance = this;
		BlockAll = GameObject.Find("BlockAll").GetComponent<CanvasGroup>();
	}

	private IEnumerator _SetBlockAll(float time, bool blockRays)
	{
		if (BlockAll == null)
		{
			BlockAll = GameObject.Find("BlockAll").GetComponent<CanvasGroup>();
		}
		yield return new WaitForSecondsRealtime(time);
		BlockAll.blocksRaycasts = blockRays;
	}

	public void SetBlockAllDelay(float time, bool blockRays)
	{
		StopAllCoroutines();
		StartCoroutine(_SetBlockAll(time, blockRays));
	}

	public void SetBlockAll(bool blockRays)
	{
		if (BlockAll == null)
		{
			BlockAll = GameObject.Find("BlockAll").GetComponent<CanvasGroup>();
		}
		BlockAll.blocksRaycasts = blockRays;
	}
}
