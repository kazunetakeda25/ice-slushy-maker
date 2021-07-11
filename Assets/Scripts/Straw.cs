using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Straw : MonoBehaviour
{
	private RectTransform strawOriginal;

	public RectTransform strawCup;

	private Image strawCupImg;

	private Image image;

	private RectTransform rectTransform;

	public Transform testPoint;

	public Transform TargetPoint;

	public Transform TargetPointMid;

	public bool bEnabled;

	private bool bIskoriscen;

	private bool bMoveBack;

	public Transform strawBackPos1;

	public bool bStarwInCup;

	public GameObject ButtonNext;

	public Animator animSplashCup;

	private bool bDrag;

	private Vector3 diffPos;

	public Transform TopLimitMove;

	private bool bMovingBack;

	private bool appFoucs = true;

	private void Start()
	{
		strawCupImg = strawCup.transform.GetComponent<Image>();
		strawCupImg.enabled = false;
		rectTransform = base.transform.GetComponent<RectTransform>();
		image = base.transform.GetComponent<Image>();
	}

	private void Update()
	{
		if (!bEnabled || bMovingBack || bIskoriscen || !(MenuManager.activeMenu == string.Empty))
		{
			return;
		}
		if (!bDrag && Input.GetMouseButtonDown(0))
		{
			RaycastHit2D raycastHit2D = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition), Vector3.zero);
			if (raycastHit2D.transform != null && raycastHit2D.transform.name.StartsWith("Straw"))
			{
				bDrag = true;
				bStarwInCup = false;
				Image component = raycastHit2D.transform.GetComponent<Image>();
				strawOriginal = raycastHit2D.transform.GetComponent<RectTransform>();
				base.transform.position = raycastHit2D.transform.position;
				base.transform.rotation = raycastHit2D.transform.rotation;
				image.sprite = component.sprite;
				image.enabled = true;
				component.enabled = false;
				strawCupImg.enabled = false;
				rectTransform.sizeDelta = strawOriginal.sizeDelta;
				testPoint.position = strawOriginal.transform.GetChild(0).position;
				diffPos = base.transform.position - Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
				InvokeRepeating("TestTarget", 0f, 0.1f);
				Tutorial.Instance.StopTutorial();
			}
		}
		if (bDrag && Input.GetMouseButtonUp(0))
		{
			bDrag = false;
			if (!bIskoriscen && strawOriginal != null)
			{
				StartCoroutine("MoveBack");
			}
		}
		if (!bDrag)
		{
			return;
		}
		Vector3 mousePosition = UnityEngine.Input.mousePosition;
		float x = mousePosition.x;
		Vector3 mousePosition2 = UnityEngine.Input.mousePosition;
		float y = mousePosition2.y;
		Vector3 b = Camera.main.ScreenToWorldPoint(new Vector3(x, y, 10f)) + diffPos;
		if (TopLimitMove != null)
		{
			Vector3 position = TopLimitMove.position;
			float y2 = position.y;
			Vector3 position2 = base.transform.position;
			float num = y2 - position2.y;
			if (b.y + num > 3.7f)
			{
				b = new Vector3(b.x, 3.7f - num, b.z);
			}
		}
		base.transform.position = Vector3.Lerp(base.transform.position, b, 10f * Time.deltaTime);
	}

	private void TestTarget()
	{
		Vector3 position = testPoint.position;
		float x = position.x;
		Vector3 position2 = TargetPoint.position;
		if (Mathf.Abs(x - position2.x) < 0.3f && !bIskoriscen)
		{
			StartCoroutine("SnapToTarget");
		}
	}

	private IEnumerator SnapToTarget()
	{
		bDrag = false;
		bIskoriscen = true;
		CancelInvoke("TestTarget");
		bMovingBack = false;
		bool bSwap = false;
		float pom = 0f;
		Vector3 positionS = base.transform.position;
		Vector3 position3 = strawOriginal.transform.GetChild(0).position - strawOriginal.transform.position;
		Vector3 vector = TargetPointMid.position - position3;
		Vector3 position2 = TargetPoint.position - position3;
		while (pom < 1f)
		{
			pom += Time.fixedDeltaTime;
			if (pom < 0.5f)
			{
				base.transform.position = Vector3.Lerp(positionS, position2, pom * 2f);
			}
			else
			{
				if (!bSwap)
				{
					bSwap = true;
					image.enabled = false;
					strawCupImg.sprite = image.sprite;
					strawCupImg.enabled = true;
					strawCupImg.transform.rotation = base.transform.rotation;
					strawCupImg.sprite = image.sprite;
					image.enabled = false;
					strawCupImg.enabled = true;
					strawCup.sizeDelta = strawOriginal.sizeDelta;
					animSplashCup.Play("splash");
					if (SoundManager.Instance != null)
					{
						SoundManager.Instance.Play_Sound(SoundManager.Instance.Straw);
					}
				}
				strawCupImg.transform.position = position2;
			}
			yield return new WaitForFixedUpdate();
		}
		ButtonNext.SetActive(value: true);
		bStarwInCup = true;
		GameData.strawImgIndex = int.Parse(strawOriginal.transform.name.Replace("Straw", string.Empty)) - 1;
		Image img = strawOriginal.transform.GetComponent<Image>();
		img.enabled = true;
		strawOriginal = null;
		yield return new WaitForSeconds(1.1f);
		bIskoriscen = false;
	}

	private IEnumerator MoveBack()
	{
		if (!bMovingBack)
		{
			CancelInvoke("TestTarget");
			bMovingBack = true;
			Vector3 endPos = strawOriginal.transform.position;
			Vector3 position = strawOriginal.transform.position;
			float x = position.x - 0.15f;
			Vector3 position2 = strawBackPos1.position;
			float y = position2.y;
			Vector3 position3 = strawOriginal.transform.position;
			new Vector3(x, y, position3.z);
			yield return new WaitForEndOfFrame();
			float pom = 0f;
			Vector3 positionS = base.transform.position;
			while (pom < 1f)
			{
				pom += Time.fixedDeltaTime * 4f;
				yield return new WaitForFixedUpdate();
				base.transform.position = Vector3.Lerp(positionS, endPos, pom);
			}
			Image img = strawOriginal.transform.GetComponent<Image>();
			image.enabled = false;
			img.enabled = true;
			yield return new WaitForFixedUpdate();
			strawOriginal = null;
			bMovingBack = false;
		}
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
