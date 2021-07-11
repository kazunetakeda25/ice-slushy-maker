using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[AddComponentMenu("Webelinx/Button2")]
public class WebelinxButton2 : Button
{
	[Serializable]
	public class ButtonDownEvent : UnityEvent
	{
	}

	[Serializable]
	public class ButtonUpEvent : UnityEvent
	{
	}

	[SerializeField]
	private ButtonDownEvent _onDown = new ButtonDownEvent();

	[SerializeField]
	private ButtonUpEvent _onUp = new ButtonUpEvent();

	public ButtonDownEvent onDown
	{
		get
		{
			return _onDown;
		}
		set
		{
			_onDown = value;
		}
	}

	public ButtonUpEvent onUp
	{
		get
		{
			return _onUp;
		}
		set
		{
			_onUp = value;
		}
	}

	protected WebelinxButton2()
	{
	}

	public override void OnPointerDown(PointerEventData eventData)
	{
		if (eventData.pointerDrag == null || (eventData.pointerDrag != null && !eventData.dragging))
		{
			base.OnPointerDown(eventData);
			if (eventData.button == PointerEventData.InputButton.Left)
			{
				_onDown.Invoke();
			}
		}
	}

	public override void OnPointerUp(PointerEventData eventData)
	{
		if (eventData.pointerDrag == null || (eventData.pointerDrag != null && !eventData.dragging))
		{
			base.OnPointerUp(eventData);
			if (eventData.button == PointerEventData.InputButton.Left)
			{
				_onUp.Invoke();
			}
		}
	}
}
