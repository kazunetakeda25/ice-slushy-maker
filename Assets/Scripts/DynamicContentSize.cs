using System.Collections;
using UnityEngine;

public class DynamicContentSize : MonoBehaviour
{
	public bool isVertical = true;

	public float itemSpacing = 10f;

	public float itemSize;

	public void SetSizeAndChildern()
	{
		StartCoroutine(WaitAndDoWork());
	}

	private IEnumerator WaitAndDoWork()
	{
		yield return new WaitForEndOfFrame();
		if (base.transform.childCount <= 0)
		{
			yield break;
		}
		if (isVertical)
		{
			RectTransform component = base.gameObject.GetComponent<RectTransform>();
			Vector2 sizeDelta = base.gameObject.GetComponent<RectTransform>().sizeDelta;
			component.sizeDelta = new Vector2(sizeDelta.x, 1f);
			int childCount = base.gameObject.transform.childCount;
			base.gameObject.transform.GetChild(0).GetComponent<RectTransform>().localScale = Vector3.one;
			Vector2 sizeDelta2 = base.gameObject.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta;
			itemSize = sizeDelta2.y;
			Vector2 sizeDelta3 = default(Vector2);
			Vector2 sizeDelta4 = base.gameObject.GetComponent<RectTransform>().sizeDelta;
			sizeDelta3.x = sizeDelta4.x;
			sizeDelta3.y = (float)(childCount + 1) * itemSpacing + (float)childCount * itemSize;
			base.gameObject.GetComponent<RectTransform>().sizeDelta = sizeDelta3;
			float num = sizeDelta3.y / 2f - itemSpacing - itemSize / 2f;
			for (int i = 0; i < base.gameObject.transform.childCount; i++)
			{
				base.gameObject.transform.GetChild(i).GetComponent<RectTransform>().localScale = Vector3.one;
				base.gameObject.transform.GetChild(i).GetComponent<RectTransform>().sizeDelta = new Vector2(sizeDelta3.x, itemSize);
				base.gameObject.transform.GetChild(i).transform.localPosition = new Vector3(0f, num, 0f);
				num -= itemSpacing + itemSize;
			}
		}
		else
		{
			RectTransform component2 = base.gameObject.GetComponent<RectTransform>();
			Vector2 sizeDelta5 = base.gameObject.GetComponent<RectTransform>().sizeDelta;
			component2.sizeDelta = new Vector2(1f, sizeDelta5.y);
			int childCount2 = base.gameObject.transform.childCount;
			base.gameObject.transform.GetChild(0).GetComponent<RectTransform>().localScale = Vector3.one;
			Vector2 sizeDelta6 = base.gameObject.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta;
			itemSize = sizeDelta6.x;
			Vector2 sizeDelta7 = default(Vector2);
			Vector2 sizeDelta8 = base.gameObject.GetComponent<RectTransform>().sizeDelta;
			sizeDelta7.y = sizeDelta8.y;
			sizeDelta7.x = (float)(childCount2 + 1) * itemSpacing + (float)childCount2 * itemSize;
			base.gameObject.GetComponent<RectTransform>().sizeDelta = sizeDelta7;
			float num2 = sizeDelta7.x / 2f - itemSpacing - itemSize / 2f;
			for (int j = 0; j < base.gameObject.transform.childCount; j++)
			{
				base.gameObject.transform.GetChild(j).GetComponent<RectTransform>().localScale = Vector3.one;
				base.gameObject.transform.GetChild(j).GetComponent<RectTransform>().sizeDelta = new Vector2(itemSize, sizeDelta7.y);
				base.gameObject.transform.GetChild(j).transform.localPosition = new Vector3(num2, 0f, 0f);
				num2 -= itemSpacing + itemSize;
			}
		}
	}
}
