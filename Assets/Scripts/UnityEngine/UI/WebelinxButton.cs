using System;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace UnityEngine.UI
{
	[AddComponentMenu("UI/WebelinxButton")]
	public class WebelinxButton : Selectable, IPointerDownHandler, ISubmitHandler, IPointerUpHandler, IEventSystemHandler
	{
		[Serializable]
		public class ButtonOnDownEvent : UnityEvent
		{
		}

		[Serializable]
		public class ButtonOnUpEvent : UnityEvent
		{
		}

		[FormerlySerializedAs("OnDown")]
		[SerializeField]
		private ButtonOnDownEvent m_OnDown = new ButtonOnDownEvent();

		[FormerlySerializedAs("OnDown")]
		[SerializeField]
		private ButtonOnUpEvent m_OnUp = new ButtonOnUpEvent();

		public ButtonOnDownEvent onDown
		{
			get
			{
				return m_OnDown;
			}
			set
			{
				m_OnDown = value;
			}
		}

		public ButtonOnUpEvent onUp
		{
			get
			{
				return m_OnUp;
			}
			set
			{
				m_OnUp = value;
			}
		}

		protected WebelinxButton()
		{
		}

		private void PressDown()
		{
			if (IsActive() && IsInteractable())
			{
				m_OnDown.Invoke();
			}
		}

		private void PressUp()
		{
			if (IsActive() && IsInteractable())
			{
				m_OnUp.Invoke();
			}
		}

		public new virtual void OnPointerDown(PointerEventData eventData)
		{
			if (eventData.button == PointerEventData.InputButton.Left)
			{
				PressDown();
			}
		}

		public new virtual void OnPointerUp(PointerEventData eventData)
		{
			if (eventData.button == PointerEventData.InputButton.Left)
			{
				PressUp();
			}
		}

		public virtual void OnSubmit(BaseEventData eventData)
		{
			PressDown();
			PressUp();
			if (IsActive() && IsInteractable())
			{
				DoStateTransition(SelectionState.Pressed, instant: false);
				StartCoroutine(OnFinishSubmit());
			}
		}

		private IEnumerator OnFinishSubmit()
		{
			float fadeTime = base.colors.fadeDuration;
			float elapsedTime = 0f;
			while (elapsedTime < fadeTime)
			{
				elapsedTime += Time.unscaledDeltaTime;
				yield return null;
			}
			DoStateTransition(base.currentSelectionState, instant: false);
		}
	}
}
