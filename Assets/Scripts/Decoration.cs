using UnityEngine;
using UnityEngine.EventSystems;

public class Decoration : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IEventSystemHandler
{
	private Vector3 dragOffset;

	public static bool bEnableDrag = true;

	public DecorationTransform decorationTransform;

	private Vector3 destintaion;

	private float sens = 10f;

	private Rigidbody2D rigdbody;

	private Collider2D collider;

	private bool bMove;

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (bEnableDrag && !(decorationTransform == null))
		{
			dragOffset = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition) - base.transform.position;
			dragOffset = new Vector3(dragOffset.x, dragOffset.y, 0f);
			rigdbody.WakeUp();
			collider.isTrigger = false;
			rigdbody.isKinematic = false;
		}
	}

	private void Start()
	{
		rigdbody = base.transform.GetComponent<Rigidbody2D>();
		collider = base.transform.GetComponent<Collider2D>();
		destintaion = base.transform.position;
	}

	private void Update()
	{
		if (bMove)
		{
			Vector2 vector = (destintaion - base.transform.position) * sens;
			float magnitude = vector.magnitude;
			if (magnitude > 10f)
			{
				vector = 10f / magnitude * vector;
			}
			rigdbody.velocity = vector;
			decorationTransform.transform.position = base.transform.position;
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (eventData.IsPointerMoving() && decorationTransform != null && !decorationTransform.bDecorationTransformButtonDown)
		{
			bMove = true;
			Vector3 vector = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition) - dragOffset;
			destintaion = new Vector3(vector.x, vector.y, 0f);
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		DeactivateDecoration();
	}

	public void DeactivateDecoration()
	{
		rigdbody.Sleep();
		bMove = false;
		collider.isTrigger = true;
		rigdbody.isKinematic = true;
		destintaion = base.transform.position;
	}
}
