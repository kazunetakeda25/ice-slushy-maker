using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DecorationTransform : MonoBehaviour
{
	public Image BoxImage;

	private float rotation;

	public Vector2 startSizeDelta;

	private Vector2 offsetSizeDelta = new Vector2(20f, 20f);

	private float WorldToCanvasPercent;

	public Transform ButtonScale;

	public Transform ButtonDelete;

	public Transform ButtonCheck;

	private CanvasGroup canvasGroup;

	public GameObject ActiveDecoration;

	private Vector3 posOffset = new Vector3(0f, 0f, -5f);

	public bool bMoveDecoratins;

	private Vector3 dragOffset;

	public bool bDecorationTransformButtonDown;

	private void Awake()
	{
		canvasGroup = base.transform.GetComponent<CanvasGroup>();
		canvasGroup.alpha = 0f;
		canvasGroup.interactable = false;
		startSizeDelta = BoxImage.rectTransform.sizeDelta;
	}

	private void Start()
	{
		bDecorationTransformButtonDown = false;
		Vector2 sizeDelta = BoxImage.rectTransform.sizeDelta;
		float num = sizeDelta.x / 2f;
		Vector3 position = ButtonScale.transform.position;
		float x = position.x;
		Vector3 position2 = BoxImage.transform.position;
		float num2 = x - position2.x;
		WorldToCanvasPercent = 1.41f * num / num2;
		bMoveDecoratins = true;
	}

	private void Update()
	{
		if (!Input.GetMouseButtonDown(0) || !bMoveDecoratins)
		{
			return;
		}
		if (MenuManager.activeMenu != string.Empty)
		{
			HideDecorationTransformTool();
			return;
		}
		Collider2D[] array = Physics2D.OverlapCircleAll(Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition), 0.1f, 1 << LayerMask.NameToLayer("DecorationTransform"));
		if (array.Length > 0)
		{
			bDecorationTransformButtonDown = true;
			return;
		}
		bDecorationTransformButtonDown = false;
		Collider2D[] array2 = Physics2D.OverlapCircleAll(Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition), 0.1f, 1 << LayerMask.NameToLayer("Decoration"));
		if (array2.Length > 0)
		{
			bool flag = false;
			for (int i = 0; i < array2.Length; i++)
			{
				if (array2[i].gameObject == ActiveDecoration)
				{
					flag = true;
				}
			}
			if (!flag)
			{
				if (ActiveDecoration != null)
				{
					Transform transform = ActiveDecoration.transform;
					Vector3 position = ActiveDecoration.transform.position;
					float x = position.x;
					Vector3 position2 = ActiveDecoration.transform.position;
					transform.position = new Vector3(x, position2.y, 0f);
					ActiveDecoration.GetComponent<Decoration>().decorationTransform = null;
				}
				ActiveDecoration = array2[0].gameObject;
				Transform transform2 = ActiveDecoration.transform;
				Vector3 position3 = ActiveDecoration.transform.position;
				float x2 = position3.x;
				Vector3 position4 = ActiveDecoration.transform.position;
				transform2.position = new Vector3(x2, position4.y, -1f);
				Transform transform3 = base.transform;
				Vector3 position5 = ActiveDecoration.transform.position;
				float x3 = position5.x;
				Vector3 position6 = ActiveDecoration.transform.position;
				transform3.position = new Vector3(x3, position6.y, -10f);
				BoxImage.transform.rotation = ActiveDecoration.transform.rotation;
				ButtonDelete.rotation = Quaternion.identity;
				BoxImage.rectTransform.sizeDelta = ActiveDecoration.GetComponent<RectTransform>().sizeDelta;
				ActiveDecoration.GetComponent<RectTransform>().SetAsLastSibling();
				BoxImage.rectTransform.sizeDelta = ActiveDecoration.GetComponent<RectTransform>().sizeDelta + offsetSizeDelta;
				BoxImage.transform.rotation = ActiveDecoration.transform.rotation;
			}
			ActiveDecoration.GetComponent<Decoration>().decorationTransform = this;
			ShowDecorationTransformTool();
		}
		else
		{
			HideDecorationTransformTool();
		}
	}

	public void ButtonCheckClicked()
	{
		HideDecorationTransformTool();
	}

	public void ButtonDeleteClicked()
	{
		UnityEngine.Debug.Log("X");
		if (SoundManager.Instance != null)
		{
			SoundManager.Instance.Play_ButtonClick();
		}
		HideDecorationTransformTool();
		ActiveDecoration.GetComponent<Decoration>().decorationTransform = null;
		UnityEngine.Object.Destroy(ActiveDecoration);
		ActiveDecoration = null;
	}

	public void ButtonScale_PointerDown(BaseEventData data)
	{
		if (SoundManager.Instance != null)
		{
			SoundManager.Instance.Play_ButtonClick();
		}
	}

	public void ButtonScale_PointerDrag(BaseEventData data)
	{
		PointerEventData pointerEventData = data as PointerEventData;
		if (canvasGroup.interactable && pointerEventData.IsPointerMoving())
		{
			Vector2 a = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
			Vector2 b = base.transform.position;
			BoxImage.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(b.y - a.y, b.x - a.x) * 57.29578f - 135f);
			float d = WorldToCanvasPercent * (a - b).magnitude;
			BoxImage.rectTransform.sizeDelta = d * Vector2.one;
			Vector2 sizeDelta = BoxImage.rectTransform.sizeDelta;
			if (sizeDelta.x > 300f)
			{
				BoxImage.rectTransform.sizeDelta = 300f * Vector2.one;
			}
			Vector2 sizeDelta2 = BoxImage.rectTransform.sizeDelta;
			if (sizeDelta2.x < 180f)
			{
				BoxImage.rectTransform.sizeDelta = 180f * Vector2.one;
			}
			ButtonDelete.rotation = Quaternion.identity;
			ActiveDecoration.GetComponent<RectTransform>().sizeDelta = BoxImage.rectTransform.sizeDelta - offsetSizeDelta;
			ActiveDecoration.transform.rotation = BoxImage.transform.rotation;
		}
	}

	public void ResetDecorationTransform()
	{
		BoxImage.transform.rotation = Quaternion.identity;
		BoxImage.rectTransform.sizeDelta = startSizeDelta;
	}

	public void ShowDecorationTransformTool()
	{
		base.transform.position = ActiveDecoration.transform.position + posOffset;
		StopAllCoroutines();
		StartCoroutine("_ShowDecorationTransformTool");
	}

	private IEnumerator _ShowDecorationTransformTool()
	{
		yield return new WaitForEndOfFrame();
		while (canvasGroup.alpha < 1f)
		{
			canvasGroup.alpha += Time.deltaTime * 4f;
			yield return new WaitForEndOfFrame();
		}
		canvasGroup.alpha = 1f;
		canvasGroup.interactable = true;
	}

	public void HideDecorationTransformTool()
	{
		StopAllCoroutines();
		StartCoroutine("_HideDecorationTransformTool");
	}

	private IEnumerator _HideDecorationTransformTool()
	{
		canvasGroup.interactable = false;
		yield return new WaitForEndOfFrame();
		while (canvasGroup.alpha > 0f)
		{
			canvasGroup.alpha -= Time.deltaTime * 3f;
			yield return new WaitForEndOfFrame();
		}
		canvasGroup.alpha = 0f;
	}
}
