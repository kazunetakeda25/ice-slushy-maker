using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DishScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IEventSystemHandler
{
	public Collider2D colliderDish;

	public Collider2D[] collidersDishContent;

	public Transform BlenderParent;

	private Animator animDish;

	public Transform SnapPosition;

	public Transform TestPoint;

	private Vector3 StartPosition;

	private bool bDrag;

	public bool bEnableDrag = true;

	private Vector3 diffPos;

	private bool bIskoriscen;

	public static bool bAnimationActive;

	private float testDistance = 1f;

	public Image Shadow;

	private float x;

	private float y;

	private Transform DishStartParent;

	public Transform ActiveItemHolder;

	public bool bFruit;

	private bool bTestChangeParent;

	private Transform DishContentParent;

	public Sprite[] fruitsSprites;

	public Sprite[] iceSprites;

	private bool bMovingBack;

	private bool appFoucs = true;

	private void Awake()
	{
		if (bFruit)
		{
			for (int i = 0; i < collidersDishContent.Length; i++)
			{
				collidersDishContent[i].GetComponent<Image>().sprite = fruitsSprites[GameData.selectedFlavor - 1];
			}
		}
		else
		{
			for (int j = 0; j < collidersDishContent.Length; j++)
			{
				collidersDishContent[j].GetComponent<Image>().sprite = iceSprites[GameData.selectedMold];
			}
		}
	}

	private IEnumerator Start()
	{
		DishStartParent = base.transform.parent;
		StartPosition = base.transform.position;
		animDish = base.transform.Find("AnimationHolder").GetComponent<Animator>();
		yield return new WaitForSeconds(0.3f);
		DisableColliders();
		DishContentParent = collidersDishContent[0].transform.parent;
	}

	private void Update()
	{
		if (bDrag)
		{
			Vector3 mousePosition = UnityEngine.Input.mousePosition;
			x = mousePosition.x;
			Vector3 mousePosition2 = UnityEngine.Input.mousePosition;
			y = mousePosition2.y;
			Vector3 b = Camera.main.ScreenToWorldPoint(new Vector3(x, y, 10f)) + diffPos;
			if (b.y > 2.4f)
			{
				b = new Vector3(b.x, 2.4f, b.z);
			}
			base.transform.position = Vector3.Lerp(base.transform.position, b, 10f * Time.deltaTime);
		}
		if (bTestChangeParent)
		{
			ChangeParent();
		}
	}

	private void DisableColliders()
	{
		colliderDish.enabled = false;
		for (int i = 0; i < collidersDishContent.Length; i++)
		{
			collidersDishContent[i].enabled = false;
			collidersDishContent[i].GetComponent<Rigidbody2D>().isKinematic = true;
		}
	}

	private void DisableRigidbodys()
	{
		colliderDish.enabled = false;
		for (int i = 0; i < collidersDishContent.Length; i++)
		{
			collidersDishContent[i].enabled = true;
			collidersDishContent[i].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			collidersDishContent[i].GetComponent<Rigidbody2D>().angularVelocity = 0f;
			collidersDishContent[i].GetComponent<Rigidbody2D>().isKinematic = true;
		}
	}

	private void EnableColliders()
	{
		colliderDish.enabled = true;
		for (int i = 0; i < collidersDishContent.Length; i++)
		{
			collidersDishContent[i].enabled = true;
			collidersDishContent[i].GetComponent<Rigidbody2D>().isKinematic = false;
		}
	}

	private void ChangeParent()
	{
		for (int i = 0; i < collidersDishContent.Length; i++)
		{
			Vector3 position = collidersDishContent[i].transform.position;
			if (position.y < 0.5f)
			{
				collidersDishContent[i].transform.SetParent(BlenderParent);
			}
		}
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (!bEnableDrag || bAnimationActive || bIskoriscen)
		{
			bDrag = false;
			return;
		}
		bDrag = true;
		diffPos = base.transform.position - Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
		StartCoroutine("HideShadow");
		base.transform.SetParent(ActiveItemHolder);
		Tutorial.Instance.StopTutorial();
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (!bEnableDrag || bAnimationActive || bIskoriscen)
		{
			bDrag = false;
		}
		else if (bDrag && !bAnimationActive && !bIskoriscen)
		{
			float num = Vector2.Distance(TestPoint.position, SnapPosition.position);
			if (num < testDistance)
			{
				StartCoroutine("SnapToTarget");
			}
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (bDrag && !bAnimationActive)
		{
			StartCoroutine("MoveBack");
		}
		bDrag = false;
	}

	private IEnumerator SnapToTarget()
	{
		if ((bEnableDrag && !bAnimationActive) || !bIskoriscen)
		{
			bDrag = false;
			bIskoriscen = true;
			bAnimationActive = true;
			float pom = 0f;
			Vector3 positionS = base.transform.position;
			while (pom < 1f)
			{
				pom += Time.fixedDeltaTime * 2f;
				base.transform.position = Vector3.Lerp(positionS, SnapPosition.position, pom);
				yield return new WaitForFixedUpdate();
			}
			if (bFruit)
			{
				animDish.Play("DishFruit");
				if (SoundManager.Instance != null)
				{
					SoundManager.Instance.Play_Sound(SoundManager.Instance.InsertFruit);
				}
			}
			else
			{
				animDish.Play("DishIce");
				if (SoundManager.Instance != null)
				{
					SoundManager.Instance.Play_Sound(SoundManager.Instance.InsertIce);
				}
			}
			EnableColliders();
			bTestChangeParent = true;
			Tutorial.Instance.StopTutorial();
			yield return new WaitForSeconds(4f);
			bTestChangeParent = false;
			StartCoroutine("MoveBack");
			yield return new WaitForSeconds(1f);
			DisableRigidbodys();
			Camera.main.SendMessage("DishUsed", (!bFruit) ? 1 : 0);
		}
		yield return new WaitForFixedUpdate();
	}

	private IEnumerator MoveBack()
	{
		if (bMovingBack)
		{
			yield break;
		}
		bMovingBack = true;
		float pom = 0f;
		Vector3 positionS = base.transform.position;
		while (pom < 1f)
		{
			pom += Time.fixedDeltaTime * 2f;
			base.transform.position = Vector3.Lerp(positionS, StartPosition, pom);
			yield return new WaitForFixedUpdate();
			if (pom > 0.8f)
			{
				yield return new WaitForEndOfFrame();
				Shadow.color = new Color(1f, 1f, 1f, (pom - 0.8f) * 5f);
			}
		}
		base.transform.position = StartPosition;
		base.transform.SetParent(DishStartParent);
		bMovingBack = false;
		bAnimationActive = false;
	}

	private IEnumerator HideShadow()
	{
		if (Shadow != null)
		{
			float f = 1f;
			while (f > 0f)
			{
				f -= Time.deltaTime * 10f;
				yield return new WaitForEndOfFrame();
				Shadow.color = new Color(1f, 1f, 1f, f);
			}
		}
		yield return new WaitForEndOfFrame();
	}

	private void OnApplicationFocus(bool hasFocus)
	{
		if (!appFoucs && hasFocus && !bIskoriscen && bDrag)
		{
			bDrag = false;
			StartCoroutine("MoveBack");
		}
		appFoucs = hasFocus;
	}
}
