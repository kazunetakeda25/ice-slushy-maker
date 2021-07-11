using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IEventSystemHandler
{
	private Vector3 StartPosition;

	private Vector3 StartScale;

	public float EndScaleFactor;

	public bool snapToTarget;

	public bool bIskoriscen;

	[HideInInspector]
	public bool bDrag;

	public static int OneItemEnabledNo = -1;

	public int ItemNo;

	private float x;

	private float y;

	private Vector3 diffPos = new Vector3(0f, 0f, 0f);

	public float testDistance = 1.5f;

	private ParticleSystem psFinishAction;

	private PointerEventData pointerEventData;

	public Animator animator;

	private bool bAnimationActive;

	public string animationType = string.Empty;

	private Transform ParentOld;

	private Transform DragItemParent;

	public Transform TestPoint;

	public Transform[] TargetPoint;

	private int targetPointIndex = -1;

	public bool bTestOnlyOnEndDrag = true;

	public Image Shadow;

	private bool bShowShadow;

	private Transform TopLimitMove;

	private bool bMovingBack;

	private bool appFoucs = true;

	private IEnumerator Start()
	{
		yield return new WaitForSeconds(0.1f);
		DragItemParent = GameObject.Find("ActiveItemHolder").transform;
		StartPosition = base.transform.position;
		ParentOld = base.transform.parent;
		bIskoriscen = false;
		if (TestPoint == null)
		{
			TestPoint = base.transform.Find("TestPoint");
		}
		TopLimitMove = base.transform.Find("TopLimitMove");
	}

	private void Update()
	{
		if (!bDrag)
		{
			return;
		}
		Vector3 mousePosition = UnityEngine.Input.mousePosition;
		x = mousePosition.x;
		Vector3 mousePosition2 = UnityEngine.Input.mousePosition;
		y = mousePosition2.y;
		Vector3 b = Camera.main.ScreenToWorldPoint(new Vector3(x, y, 10f)) + diffPos;
		if (TopLimitMove != null)
		{
			Vector3 position = TopLimitMove.position;
			float num = position.y;
			Vector3 position2 = base.transform.position;
			float num2 = num - position2.y;
			if (b.y + num2 > 3.7f)
			{
				b = new Vector3(b.x, 3.7f - num2, b.z);
			}
		}
		base.transform.position = Vector3.Lerp(base.transform.position, b, 10f * Time.deltaTime);
	}

	private void TestTarget()
	{
		if (bAnimationActive || bIskoriscen)
		{
			return;
		}
		int num = -1;
		float num2 = 1000f;
		float num3 = 0f;
		for (int i = 0; i < TargetPoint.Length; i++)
		{
			if (!(TargetPoint[i] == null))
			{
				num3 = Vector2.Distance(TestPoint.position, TargetPoint[i].position);
				if (num3 < testDistance && num3 < num2)
				{
					num = i;
					num2 = num3;
				}
			}
		}
		targetPointIndex = num;
		if (targetPointIndex > -1)
		{
			StartCoroutine("SnapToTarget", TargetPoint[targetPointIndex]);
		}
		else if (bTestOnlyOnEndDrag)
		{
			StartCoroutine("MoveBack");
		}
	}

	private IEnumerator SnapToTarget(Transform target)
	{
		OneItemEnabledNo = 0;
		bDrag = false;
		CancelInvoke("TestTarget");
		yield return new WaitForEndOfFrame();
		if (animationType == "PitcherFillMold" && animator != null)
		{
			float pom2 = 0f;
			Vector3 sPos2 = base.transform.position;
			while (pom2 < 1f)
			{
				pom2 += Time.fixedDeltaTime * 3f;
				base.transform.position = Vector3.Lerp(sPos2, target.position, pom2);
				yield return new WaitForFixedUpdate();
			}
			animator.enabled = true;
			animator.Play("PitcherFillMold", -1, 0f);
			yield return new WaitForSeconds(0.5f);
			if (SoundManager.Instance != null)
			{
				SoundManager.Instance.Play_Sound(SoundManager.Instance.WaterSound);
			}
			yield return new WaitForSeconds(0.9f);
		}
		if (animationType == "Mold")
		{
			bIskoriscen = true;
			float pom = 0f;
			base.transform.parent = GameObject.Find("MoldHolder2").transform;
			Vector3 sPos = base.transform.position;
			Vector3 sScale = base.transform.localScale;
			Vector3 endScale = new Vector3(0.7f, 0.35f, 1f);
			while (pom < 1f)
			{
				pom += Time.fixedDeltaTime * 3f;
				base.transform.position = Vector3.Lerp(sPos, target.position, pom);
				base.transform.localScale = Vector3.Lerp(sScale, endScale, pom);
				yield return new WaitForFixedUpdate();
			}
			yield return new WaitForSeconds(0.1f);
			Camera.main.SendMessage("NextPhase", 5, SendMessageOptions.DontRequireReceiver);
		}
	}

	public void PitcherFillMold()
	{
		StartCoroutine("WPitcherFillMold");
	}

	private IEnumerator WPitcherFillMold()
	{
		if (targetPointIndex >= 0 && TargetPoint.Length > targetPointIndex)
		{
			Transform water = TargetPoint[targetPointIndex].GetChild(0);
			water.gameObject.SetActive(value: true);
			water.localScale = Vector3.zero;
			float i = 0f;
			while (i < 1f)
			{
				i += Time.deltaTime;
				water.localScale = i * Vector3.one;
				yield return new WaitForEndOfFrame();
			}
			water.localScale = Vector3.one;
		}
		yield return new WaitForEndOfFrame();
	}

	public void AnimationFinished()
	{
		if (animationType == "PitcherFillMold")
		{
			if (targetPointIndex >= 0 && TargetPoint.Length > targetPointIndex)
			{
				TargetPoint[targetPointIndex] = null;
			}
			targetPointIndex = -1;
			int num = 0;
			for (int i = 0; i < TargetPoint.Length; i++)
			{
				if (TargetPoint[i] != null)
				{
					num++;
				}
			}
			if (num == 0 && !bMovingBack)
			{
				StopAllCoroutines();
				Camera.main.SendMessage("NextPhase", 4, SendMessageOptions.DontRequireReceiver);
				bIskoriscen = true;
			}
			else
			{
				StopAllCoroutines();
				StartMoveBack();
				OneItemEnabledNo = 1;
			}
		}
		else if (!bMovingBack)
		{
			StopAllCoroutines();
			StartMoveBack();
		}
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (bIskoriscen || bMovingBack)
		{
			return;
		}
		StopAllCoroutines();
		pointerEventData = eventData;
		bAnimationActive = false;
		if (OneItemEnabledNo > -1 && ItemNo != OneItemEnabledNo)
		{
			bDrag = false;
		}
		else if (!bIskoriscen && !bDrag)
		{
			bDrag = true;
			StartPosition = base.transform.position;
			diffPos = base.transform.position - Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
			diffPos = new Vector3(diffPos.x, diffPos.y, 0f);
			DragItemParent.position = base.transform.parent.position;
			base.transform.SetParent(DragItemParent);
			if (!bTestOnlyOnEndDrag)
			{
				InvokeRepeating("TestTarget", 0f, 0.1f);
			}
			if (animationType == "PitcherFillMold" && animator != null)
			{
				Tutorial.Instance.StopTutorial();
				StartCoroutine("HideShadow");
			}
			if (animationType == "Mold")
			{
				Tutorial.Instance.StopTutorial();
			}
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (bDrag)
		{
			bDrag = false;
			if (bTestOnlyOnEndDrag && !bIskoriscen)
			{
				TestTarget();
				return;
			}
			CancelInvoke("TestTarget");
			StartCoroutine("MoveBack");
		}
	}

	private IEnumerator MoveBack()
	{
		if (bMovingBack)
		{
			yield break;
		}
		bMovingBack = true;
		if (animationType == "PitcherFillMold" && Shadow != null)
		{
			bShowShadow = true;
		}
		yield return new WaitForEndOfFrame();
		float pom = 0f;
		Vector3 positionS = base.transform.position;
		while (pom < 1f)
		{
			pom += Time.fixedDeltaTime * 2f;
			base.transform.position = Vector3.Lerp(positionS, StartPosition, pom);
			yield return new WaitForFixedUpdate();
			if (bShowShadow && pom > 0.8f)
			{
				yield return new WaitForEndOfFrame();
				Shadow.color = new Color(1f, 1f, 1f, (pom - 0.8f) * 5f);
			}
		}
		base.transform.SetParent(ParentOld);
		base.transform.position = StartPosition;
		bMovingBack = false;
		if (!bIskoriscen)
		{
		}
	}

	public void StartMoveBack()
	{
		CancelInvoke("TestTarget");
		StartCoroutine("MoveBack");
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

	private IEnumerator ShowShadow()
	{
		if (Shadow != null)
		{
			float f = 0f;
			while (f < 1f)
			{
				f += Time.deltaTime * 5f;
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
			CancelInvoke("TestTarget");
			StartCoroutine("MoveBack");
		}
		appFoucs = hasFocus;
	}
}
