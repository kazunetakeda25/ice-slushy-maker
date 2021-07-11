using UnityEngine;
using UnityEngine.EventSystems;

public class Knife : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IEventSystemHandler
{
	private CutFruitScene cfs;

	private Vector3 diffPos;

	private bool bDrag;

	public static bool bEnableDrag;

	public void Start()
	{
		cfs = Camera.main.GetComponent<CutFruitScene>();
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (!bEnableDrag)
		{
			bDrag = false;
			return;
		}
		bDrag = true;
		diffPos = base.transform.position - Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (!bEnableDrag)
		{
			bDrag = false;
		}
		else if (bDrag)
		{
			Vector3 vector = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
			float mousePosY = vector.y * 1.4f + diffPos.y;
			cfs.MoveKnife(mousePosY);
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		bDrag = false;
	}
}
