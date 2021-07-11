using UnityEngine;

public class Menu : MonoBehaviour
{
	private Animator _animtor;

	private CanvasGroup BlockAll;

	public void Awake()
	{
		_animtor = GetComponent<Animator>();
		RectTransform component = GetComponent<RectTransform>();
		Vector2 vector3 = component.offsetMax = (component.offsetMin = new Vector2(0f, 0f));
	}

	public void Start()
	{
		BlockAll = GameObject.Find("Canvas/BlockAll").GetComponent<CanvasGroup>();
	}

	public void ResetObject()
	{
		base.gameObject.SetActive(value: false);
	}

	public void DisableObject(string gameObjectName)
	{
		GameObject gameObject = GameObject.Find(gameObjectName);
		if (gameObject != null && gameObject.activeSelf)
		{
			gameObject.SetActive(value: false);
		}
	}

	public void OpenMenu()
	{
		if (BlockAll == null)
		{
			BlockAll = GameObject.Find("BlockAll").transform.GetComponent<CanvasGroup>();
		}
		_animtor.SetTrigger("tOpen");
		_animtor.ResetTrigger("tClose");
		BlockAll.blocksRaycasts = true;
	}

	public void CloseMenu()
	{
		if (BlockAll == null)
		{
			BlockAll = GameObject.Find("BlockAll").transform.GetComponent<CanvasGroup>();
		}
		_animtor.SetTrigger("tClose");
		_animtor.ResetTrigger("tOpen");
		BlockAll.blocksRaycasts = true;
	}

	public void MenuClosed()
	{
		if (BlockAll == null)
		{
			BlockAll = GameObject.Find("BlockAll").GetComponent<CanvasGroup>();
		}
		BlockAll.blocksRaycasts = false;
		ResetObject();
	}

	public void MenuOpened()
	{
		if (BlockAll == null)
		{
			BlockAll = GameObject.Find("BlockAll").GetComponent<CanvasGroup>();
		}
		BlockAll.blocksRaycasts = false;
	}
}
