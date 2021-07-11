using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonControll : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IEventSystemHandler
{
	public bool changeInteractable = true;

	private bool bPointerIn;

	private bool bPointerUp = true;

	private Animator anim;

	private Button btn;

	private void Start()
	{
		anim = base.transform.GetComponent<Animator>();
		btn = base.transform.GetComponent<Button>();
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if ((changeInteractable || btn.interactable) && bPointerUp)
		{
			btn.interactable = true;
			bPointerIn = true;
			bPointerUp = false;
			anim.SetBool("bPointerIn", bPointerIn);
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		bPointerUp = true;
		bPointerIn = false;
		anim.SetBool("bPointerIn", bPointerIn);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		bPointerIn = false;
		anim.SetBool("bPointerIn", bPointerIn);
		if (changeInteractable)
		{
			btn.interactable = false;
			anim.SetTrigger("Highlighted");
		}
	}
}
