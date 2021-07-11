using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BlenderHandle : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IEventSystemHandler
{
	public Transform ContentHoder;

	private float diffAngle;

	private bool bDrag = true;

	private float mixingTime;

	private float requiredMixingTime = 10f;

	private float addForceTime = 0.5f;

	private float addForceStep = 0.5f;

	public Image imgSlush;

	private Vector2 slushRect;

	private PointerEventData ped;

	private bool appFoucs = true;

	private void Start()
	{
		slushRect = imgSlush.rectTransform.sizeDelta;
		imgSlush.rectTransform.sizeDelta = new Vector2(slushRect.x, 0f);
		imgSlush.color = new Color(1f, 1f, 1f, 0f);
		EnableColliders();
	}

	private void EnableColliders()
	{
		for (int i = 0; i < ContentHoder.childCount; i++)
		{
			ContentHoder.GetChild(i).GetComponent<Rigidbody2D>().isKinematic = false;
		}
	}

	private void Update()
	{
		if (ped != null && SoundManager.Instance != null && bDrag)
		{
			if (ped.IsPointerMoving())
			{
				SoundManager.Instance.Play_Sound(SoundManager.Instance.Blender);
			}
			else
			{
				SoundManager.Instance.Stop_Sound(SoundManager.Instance.Blender);
			}
		}
	}

	private void AddForce()
	{
		int num = 0;
		int num2 = 0;
		int num3 = 3;
		while (num2 < num3 && num < 100)
		{
			num++;
			int index = UnityEngine.Random.Range(0, ContentHoder.childCount);
			Vector3 position = ContentHoder.GetChild(index).position;
			if (!(position.y < -0.85f))
			{
				continue;
			}
			Vector3 position2 = ContentHoder.GetChild(index).position;
			if (position2.x < 0.6f)
			{
				Vector3 position3 = ContentHoder.GetChild(index).position;
				if (position3.x > -0.6f)
				{
					ContentHoder.GetChild(index).GetComponent<Rigidbody2D>().AddForce(new Vector2(UnityEngine.Random.Range(-500, 500), 300f));
					num2++;
				}
			}
		}
	}

	public void MixingSlush()
	{
		if (requiredMixingTime < mixingTime)
		{
			for (int i = 0; i < ContentHoder.childCount; i++)
			{
				ContentHoder.GetChild(i).gameObject.SetActive(value: false);
			}
			Camera.main.SendMessage("MixingOver");
			SoundManager.Instance.Stop_Sound(SoundManager.Instance.Blender);
			base.enabled = false;
			return;
		}
		mixingTime += Time.deltaTime;
		float num = mixingTime / requiredMixingTime;
		imgSlush.rectTransform.sizeDelta = new Vector2(slushRect.x, num * slushRect.y);
		imgSlush.color = new Color(GameData.selectedFlavorColor.r, GameData.selectedFlavorColor.g, GameData.selectedFlavorColor.b, 0.5f + num * 0.5f);
		if (mixingTime > addForceTime)
		{
			AddForce();
			addForceTime += addForceStep;
		}
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		ped = eventData;
		bDrag = true;
		Vector3 vector = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition) - base.transform.position;
		float num = Mathf.Atan2(vector.x, 0f - vector.y) * 57.29578f;
		Vector3 eulerAngles = base.transform.rotation.eulerAngles;
		diffAngle = eulerAngles.z - num;
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (bDrag)
		{
			Vector3 vector = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition) - base.transform.position;
			float num = Mathf.Atan2(vector.x, 0f - vector.y) * 57.29578f;
			base.transform.rotation = Quaternion.Euler(0f, 0f, num + diffAngle);
			MixingSlush();
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		bDrag = false;
		if (SoundManager.Instance != null)
		{
			SoundManager.Instance.Stop_Sound(SoundManager.Instance.Blender);
		}
	}

	private void OnApplicationFocus(bool hasFocus)
	{
		if (!appFoucs && hasFocus && bDrag)
		{
			bDrag = false;
		}
		appFoucs = hasFocus;
	}
}
