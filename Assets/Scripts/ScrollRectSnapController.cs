using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollRectSnapController : MonoBehaviour
{
	public List<float> snapPositions;

	public float cellSizeX;

	public float cellSizeY;

	public float spacing;

	private float currentCharCheckTemp;

	public Vector3 newLerpPosition;

	private bool lerping;

	public float lerpingSpeed;

	private bool holdingRect;

	public float focusedElementScale;

	public float unfocusedElementsScale;

	public List<GameObject> listOfCharacters;

	public bool horizontalList;

	public GameObject backwardButton;

	public GameObject forwardButton;

	private bool buttonPressed;

	public int currentCharacter;

	private void Awake()
	{
		currentCharacter = 0;
		lerping = false;
		buttonPressed = false;
		if (GetComponent<GridLayoutGroup>().cellSize == Vector2.zero)
		{
			Vector2 cellSize = new Vector2(cellSizeX, cellSizeY);
			GetComponent<GridLayoutGroup>().cellSize = cellSize;
		}
		else
		{
			Vector2 cellSize2 = GetComponent<GridLayoutGroup>().cellSize;
			cellSizeX = cellSize2.x;
			Vector2 cellSize3 = GetComponent<GridLayoutGroup>().cellSize;
			cellSizeY = cellSize3.y;
		}
		base.transform.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(cellSizeX, cellSizeY);
		if (horizontalList)
		{
			base.transform.parent.GetComponent<ScrollRect>().horizontal = true;
			base.transform.parent.GetComponent<ScrollRect>().vertical = false;
			if (GetComponent<GridLayoutGroup>().spacing == Vector2.zero)
			{
				Vector2 vector = new Vector2(spacing, 0f);
				GetComponent<GridLayoutGroup>().spacing = vector;
			}
			else
			{
				Vector2 vector2 = GetComponent<GridLayoutGroup>().spacing;
				if (vector2.x != 0f)
				{
					Vector2 vector3 = GetComponent<GridLayoutGroup>().spacing;
					spacing = vector3.x;
				}
				Vector2 vector4 = new Vector2(spacing, 0f);
			}
			GetComponent<GridLayoutGroup>().startAxis = GridLayoutGroup.Axis.Vertical;
			GetComponent<GridLayoutGroup>().constraint = GridLayoutGroup.Constraint.FixedRowCount;
			GetComponent<GridLayoutGroup>().constraintCount = 1;
			currentCharCheckTemp = (cellSizeX + spacing) / 2f;
		}
		else
		{
			base.transform.parent.GetComponent<ScrollRect>().horizontal = false;
			base.transform.parent.GetComponent<ScrollRect>().vertical = true;
			if (GetComponent<GridLayoutGroup>().spacing == Vector2.zero)
			{
				Vector2 vector5 = new Vector2(0f, spacing);
				GetComponent<GridLayoutGroup>().spacing = vector5;
			}
			else
			{
				Vector2 vector6 = GetComponent<GridLayoutGroup>().spacing;
				if (vector6.y != 0f)
				{
					Vector2 vector7 = GetComponent<GridLayoutGroup>().spacing;
					spacing = vector7.y;
				}
				Vector2 vector8 = new Vector2(0f, spacing);
			}
			GetComponent<GridLayoutGroup>().startAxis = GridLayoutGroup.Axis.Horizontal;
			GetComponent<GridLayoutGroup>().constraint = GridLayoutGroup.Constraint.FixedColumnCount;
			GetComponent<GridLayoutGroup>().constraintCount = 1;
			currentCharCheckTemp = (cellSizeY + spacing) / 2f;
		}
		snapPositions.Clear();
		snapPositions = new List<float>();
		IEnumerator enumerator = base.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.Current;
				listOfCharacters.Add(transform.gameObject);
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		if (horizontalList)
		{
			GetComponent<RectTransform>().sizeDelta = new Vector2((float)listOfCharacters.Count * cellSizeX + (float)(listOfCharacters.Count - 1) * spacing, cellSizeY);
			RectTransform component = GetComponent<RectTransform>();
			Vector2 sizeDelta = GetComponent<RectTransform>().sizeDelta;
			float x = sizeDelta.x - 2f * spacing;
			Vector2 anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
			component.anchoredPosition = new Vector2(x, anchoredPosition.y);
			Vector2 sizeDelta2 = GetComponent<RectTransform>().sizeDelta;
			float num = sizeDelta2.x / 2f - cellSizeX / 2f;
			snapPositions.Add(num);
			listOfCharacters[0].transform.localScale = new Vector3(focusedElementScale, focusedElementScale, 1f);
			for (int i = 1; i < listOfCharacters.Count; i++)
			{
				num -= cellSizeX + spacing;
				snapPositions.Add(num);
				listOfCharacters[i].transform.localScale = new Vector3(unfocusedElementsScale, unfocusedElementsScale, 1f);
			}
		}
		else
		{
			GetComponent<RectTransform>().sizeDelta = new Vector2(cellSizeX, (float)listOfCharacters.Count * cellSizeY + (float)(listOfCharacters.Count - 1) * spacing);
			RectTransform component2 = GetComponent<RectTransform>();
			Vector2 anchoredPosition2 = GetComponent<RectTransform>().anchoredPosition;
			float x2 = anchoredPosition2.x;
			Vector2 sizeDelta3 = GetComponent<RectTransform>().sizeDelta;
			component2.anchoredPosition = new Vector2(x2, 0f - (sizeDelta3.y - 2f * spacing));
			Vector2 sizeDelta4 = GetComponent<RectTransform>().sizeDelta;
			float num2 = sizeDelta4.y / 2f - cellSizeY / 2f;
			snapPositions.Add(num2);
			listOfCharacters[0].transform.localScale = new Vector3(focusedElementScale, focusedElementScale, 1f);
			for (int j = 1; j < listOfCharacters.Count; j++)
			{
				num2 -= cellSizeY + spacing;
				snapPositions.Add(num2);
				listOfCharacters[j].transform.localScale = new Vector3(unfocusedElementsScale, unfocusedElementsScale, 1f);
			}
		}
	}

	private void SetLerpPositionToClosestSnapPoint()
	{
		int num = 0;
		while (true)
		{
			if (num >= snapPositions.Count)
			{
				return;
			}
			if (horizontalList)
			{
				Vector3 localPosition = base.transform.localPosition;
				if (localPosition.x > snapPositions[num] - currentCharCheckTemp - 1f)
				{
					Vector3 localPosition2 = base.transform.localPosition;
					if (localPosition2.x <= snapPositions[num] + currentCharCheckTemp)
					{
						newLerpPosition = new Vector3(snapPositions[num], 0f, 0f);
						lerping = true;
						currentCharacter = num;
						return;
					}
				}
			}
			else
			{
				Vector3 localPosition3 = base.transform.localPosition;
				if (localPosition3.y > snapPositions[num] - currentCharCheckTemp - 1f)
				{
					Vector3 localPosition4 = base.transform.localPosition;
					if (localPosition4.y <= snapPositions[num] + currentCharCheckTemp)
					{
						break;
					}
				}
			}
			num++;
		}
		newLerpPosition = new Vector3(0f, snapPositions[num], 0f);
		lerping = true;
		currentCharacter = listOfCharacters.Count - num - 1;
	}

	private void SetCurrentCharacter()
	{
		int num = 0;
		while (true)
		{
			if (num >= snapPositions.Count)
			{
				return;
			}
			if (horizontalList)
			{
				Vector3 localPosition = base.transform.localPosition;
				if (localPosition.x > snapPositions[num] - currentCharCheckTemp - 1f)
				{
					Vector3 localPosition2 = base.transform.localPosition;
					if (localPosition2.x <= snapPositions[num] + currentCharCheckTemp)
					{
						currentCharacter = num;
						return;
					}
				}
			}
			else
			{
				Vector3 localPosition3 = base.transform.localPosition;
				if (localPosition3.y > snapPositions[num] - currentCharCheckTemp - 1f)
				{
					Vector3 localPosition4 = base.transform.localPosition;
					if (localPosition4.y <= snapPositions[num] + currentCharCheckTemp)
					{
						break;
					}
				}
			}
			num++;
		}
		currentCharacter = listOfCharacters.Count - num - 1;
	}

	private IEnumerator ButtonPressed()
	{
		yield return new WaitForSeconds(0.4f);
		buttonPressed = false;
	}

	public void BackwardButtonPressed()
	{
		if (horizontalList)
		{
			if (currentCharacter > 0 && !buttonPressed)
			{
				buttonPressed = true;
				currentCharacter--;
				float x = snapPositions[currentCharacter];
				Vector3 localPosition = base.transform.localPosition;
				newLerpPosition = new Vector3(x, localPosition.y, 0f);
				lerping = true;
				StartCoroutine(ButtonPressed());
			}
		}
		else if (currentCharacter > 0 && !buttonPressed)
		{
			buttonPressed = true;
			currentCharacter--;
			Vector3 localPosition2 = base.transform.localPosition;
			newLerpPosition = new Vector3(localPosition2.x, snapPositions[listOfCharacters.Count - currentCharacter - 1], 0f);
			lerping = true;
			StartCoroutine(ButtonPressed());
		}
	}

	public void ForwardButtonPressed()
	{
		if (horizontalList)
		{
			if (currentCharacter < snapPositions.Count - 1 && !buttonPressed)
			{
				buttonPressed = true;
				currentCharacter++;
				float x = snapPositions[currentCharacter];
				Vector3 localPosition = base.transform.localPosition;
				newLerpPosition = new Vector3(x, localPosition.y, 0f);
				lerping = true;
				StartCoroutine(ButtonPressed());
			}
		}
		else if (currentCharacter < listOfCharacters.Count - 1 && !buttonPressed)
		{
			buttonPressed = true;
			currentCharacter += 2;
			Vector3 localPosition2 = base.transform.localPosition;
			newLerpPosition = new Vector3(localPosition2.x, snapPositions[listOfCharacters.Count - currentCharacter], 0f);
			lerping = true;
			StartCoroutine(ButtonPressed());
		}
	}

	public void SetButtonActive(GameObject button)
	{
		Color color = button.GetComponent<Image>().color;
		color = new Color(1f, 1f, 1f, 1f);
		button.GetComponent<Image>().color = color;
		button.GetComponent<Button>().interactable = true;
	}

	public void SetButtonInactive(GameObject button)
	{
		Color color = button.GetComponent<Image>().color;
		color = new Color(1f, 1f, 1f, 0.3f);
		button.GetComponent<Image>().color = color;
		button.GetComponent<Button>().interactable = false;
	}

	private void Update()
	{
		if ((Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)) && !buttonPressed)
		{
			holdingRect = true;
			SetCurrentCharacter();
			newLerpPosition = base.transform.localPosition;
		}
		if (Input.GetMouseButtonUp(0))
		{
			holdingRect = false;
		}
		if (horizontalList)
		{
			if (!lerping && !holdingRect)
			{
				Vector2 velocity = base.transform.parent.GetComponent<ScrollRect>().velocity;
				if (Mathf.Abs(velocity.x) >= 0f)
				{
					Vector2 velocity2 = base.transform.parent.GetComponent<ScrollRect>().velocity;
					if (Mathf.Abs(velocity2.x) < 100f)
					{
						SetLerpPositionToClosestSnapPoint();
						goto IL_015f;
					}
				}
			}
			SetCurrentCharacter();
		}
		else
		{
			if (!lerping && !holdingRect)
			{
				Vector2 velocity3 = base.transform.parent.GetComponent<ScrollRect>().velocity;
				if (Mathf.Abs(velocity3.y) >= 0f)
				{
					Vector2 velocity4 = base.transform.parent.GetComponent<ScrollRect>().velocity;
					if (Mathf.Abs(velocity4.y) < 100f)
					{
						SetLerpPositionToClosestSnapPoint();
						goto IL_015f;
					}
				}
			}
			SetCurrentCharacter();
		}
		goto IL_015f;
		IL_015f:
		if (horizontalList)
		{
			if (currentCharacter == 0)
			{
				float num = snapPositions[currentCharacter];
				Vector3 localPosition = base.transform.localPosition;
				float num2 = Mathf.Abs(Mathf.Abs(num - localPosition.x - currentCharCheckTemp * 2f) * (focusedElementScale - unfocusedElementsScale) / Mathf.Abs(currentCharCheckTemp * 2f) - focusedElementScale);
				float num3 = snapPositions[currentCharacter];
				Vector3 localPosition2 = base.transform.localPosition;
				float num4 = Mathf.Abs(Mathf.Abs(num3 - localPosition2.x) * (focusedElementScale - unfocusedElementsScale) / Mathf.Abs(currentCharCheckTemp * 2f) - focusedElementScale);
				if (num4 <= unfocusedElementsScale)
				{
					num4 = unfocusedElementsScale;
				}
				if (num2 <= unfocusedElementsScale)
				{
					num2 = unfocusedElementsScale;
				}
				listOfCharacters[currentCharacter].transform.localScale = new Vector3(num4, num4, 1f);
				listOfCharacters[currentCharacter + 1].transform.localScale = new Vector3(num2, num2, 1f);
			}
			else if (currentCharacter == listOfCharacters.Count - 1)
			{
				float num5 = snapPositions[currentCharacter];
				Vector3 localPosition3 = base.transform.localPosition;
				float num6 = Mathf.Abs(Mathf.Abs(num5 - localPosition3.x) * (focusedElementScale - unfocusedElementsScale) / Mathf.Abs(currentCharCheckTemp * 2f) - focusedElementScale);
				float num7 = snapPositions[currentCharacter];
				Vector3 localPosition4 = base.transform.localPosition;
				float num8 = Mathf.Abs(Mathf.Abs(num7 - localPosition4.x + currentCharCheckTemp * 2f) * (focusedElementScale - unfocusedElementsScale) / Mathf.Abs(currentCharCheckTemp * 2f) - focusedElementScale);
				if (num6 <= unfocusedElementsScale)
				{
					num6 = unfocusedElementsScale;
				}
				if (num8 <= unfocusedElementsScale)
				{
					num8 = unfocusedElementsScale;
				}
				listOfCharacters[currentCharacter - 1].transform.localScale = new Vector3(num8, num8, 1f);
				listOfCharacters[currentCharacter].transform.localScale = new Vector3(num6, num6, 1f);
			}
			else
			{
				float num9 = snapPositions[currentCharacter];
				Vector3 localPosition5 = base.transform.localPosition;
				float num10 = Mathf.Abs(Mathf.Abs(num9 - localPosition5.x - currentCharCheckTemp * 2f) * (focusedElementScale - unfocusedElementsScale) / Mathf.Abs(currentCharCheckTemp * 2f) - focusedElementScale);
				float num11 = snapPositions[currentCharacter];
				Vector3 localPosition6 = base.transform.localPosition;
				float num12 = Mathf.Abs(Mathf.Abs(num11 - localPosition6.x) * (focusedElementScale - unfocusedElementsScale) / Mathf.Abs(currentCharCheckTemp * 2f) - focusedElementScale);
				float num13 = snapPositions[currentCharacter];
				Vector3 localPosition7 = base.transform.localPosition;
				float num14 = Mathf.Abs(Mathf.Abs(num13 - localPosition7.x + currentCharCheckTemp * 2f) * (focusedElementScale - unfocusedElementsScale) / Mathf.Abs(currentCharCheckTemp * 2f) - focusedElementScale);
				if (num12 <= unfocusedElementsScale)
				{
					num12 = unfocusedElementsScale;
				}
				if (num10 <= unfocusedElementsScale)
				{
					num10 = unfocusedElementsScale;
				}
				if (num14 <= unfocusedElementsScale)
				{
					num14 = unfocusedElementsScale;
				}
				listOfCharacters[currentCharacter - 1].transform.localScale = new Vector3(num14, num14, 1f);
				listOfCharacters[currentCharacter].transform.localScale = new Vector3(num12, num12, 1f);
				listOfCharacters[currentCharacter + 1].transform.localScale = new Vector3(num10, num10, 1f);
			}
		}
		else if (currentCharacter == 0)
		{
			float num15 = snapPositions[currentCharacter];
			Vector3 localPosition8 = base.transform.localPosition;
			float num16 = Mathf.Abs(Mathf.Abs(num15 + localPosition8.y - currentCharCheckTemp * 2f) * (focusedElementScale - unfocusedElementsScale) / Mathf.Abs(currentCharCheckTemp * 2f) - focusedElementScale);
			float num17 = snapPositions[currentCharacter];
			Vector3 localPosition9 = base.transform.localPosition;
			float num18 = Mathf.Abs(Mathf.Abs(num17 + localPosition9.y) * (focusedElementScale - unfocusedElementsScale) / Mathf.Abs(currentCharCheckTemp * 2f) - focusedElementScale);
			if (num18 <= unfocusedElementsScale)
			{
				num18 = unfocusedElementsScale;
			}
			if (num16 <= unfocusedElementsScale)
			{
				num16 = unfocusedElementsScale;
			}
			listOfCharacters[currentCharacter].transform.localScale = new Vector3(num18, num18, 1f);
			listOfCharacters[currentCharacter + 1].transform.localScale = new Vector3(num16, num16, 1f);
		}
		else if (currentCharacter == listOfCharacters.Count - 1)
		{
			float num19 = snapPositions[currentCharacter];
			Vector3 localPosition10 = base.transform.localPosition;
			float num20 = Mathf.Abs(Mathf.Abs(num19 + localPosition10.y) * (focusedElementScale - unfocusedElementsScale) / Mathf.Abs(currentCharCheckTemp * 2f) - focusedElementScale);
			float num21 = snapPositions[currentCharacter];
			Vector3 localPosition11 = base.transform.localPosition;
			float num22 = Mathf.Abs(Mathf.Abs(num21 + localPosition11.y + currentCharCheckTemp * 2f) * (focusedElementScale - unfocusedElementsScale) / Mathf.Abs(currentCharCheckTemp * 2f) - focusedElementScale);
			if (num20 <= unfocusedElementsScale)
			{
				num20 = unfocusedElementsScale;
			}
			if (num22 <= unfocusedElementsScale)
			{
				num22 = unfocusedElementsScale;
			}
			listOfCharacters[currentCharacter - 1].transform.localScale = new Vector3(num22, num22, 1f);
			listOfCharacters[currentCharacter].transform.localScale = new Vector3(num20, num20, 1f);
		}
		else
		{
			float num23 = snapPositions[currentCharacter];
			Vector3 localPosition12 = base.transform.localPosition;
			float num24 = Mathf.Abs(Mathf.Abs(num23 + localPosition12.y - currentCharCheckTemp * 2f) * (focusedElementScale - unfocusedElementsScale) / Mathf.Abs(currentCharCheckTemp * 2f) - focusedElementScale);
			float num25 = snapPositions[currentCharacter];
			Vector3 localPosition13 = base.transform.localPosition;
			float num26 = Mathf.Abs(Mathf.Abs(num25 + localPosition13.y) * (focusedElementScale - unfocusedElementsScale) / Mathf.Abs(currentCharCheckTemp * 2f) - focusedElementScale);
			float num27 = snapPositions[currentCharacter];
			Vector3 localPosition14 = base.transform.localPosition;
			float num28 = Mathf.Abs(Mathf.Abs(num27 + localPosition14.y + currentCharCheckTemp * 2f) * (focusedElementScale - unfocusedElementsScale) / Mathf.Abs(currentCharCheckTemp * 2f) - focusedElementScale);
			if (num26 <= unfocusedElementsScale)
			{
				num26 = unfocusedElementsScale;
			}
			if (num24 <= unfocusedElementsScale)
			{
				num24 = unfocusedElementsScale;
			}
			if (num28 <= unfocusedElementsScale)
			{
				num28 = unfocusedElementsScale;
			}
			listOfCharacters[currentCharacter - 1].transform.localScale = new Vector3(num28, num28, 1f);
			listOfCharacters[currentCharacter].transform.localScale = new Vector3(num26, num26, 1f);
			listOfCharacters[currentCharacter + 1].transform.localScale = new Vector3(num24, num24, 1f);
		}
		if (lerping)
		{
			base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, newLerpPosition, lerpingSpeed);
			if (Vector3.Distance(base.transform.localPosition, newLerpPosition) < 1f)
			{
				base.transform.localPosition = newLerpPosition;
				base.transform.parent.GetComponent<ScrollRect>().velocity = new Vector3(0f, 0f, 0f);
				lerping = false;
				for (int i = 0; i < listOfCharacters.Count; i++)
				{
					if (i != currentCharacter)
					{
						listOfCharacters[i].transform.localScale = new Vector3(unfocusedElementsScale, unfocusedElementsScale, 1f);
					}
				}
			}
		}
		if (horizontalList)
		{
			Vector3 localPosition15 = base.transform.localPosition;
			if (localPosition15.x > snapPositions[snapPositions.Count - 1] + spacing / 2f)
			{
				SetButtonActive(forwardButton);
			}
			else
			{
				SetButtonInactive(forwardButton);
			}
			Vector3 localPosition16 = base.transform.localPosition;
			if (localPosition16.x < snapPositions[0] - spacing / 2f)
			{
				SetButtonActive(backwardButton);
			}
			else
			{
				SetButtonInactive(backwardButton);
			}
		}
		else
		{
			Vector3 localPosition17 = base.transform.localPosition;
			if (localPosition17.y > snapPositions[snapPositions.Count - 1] + spacing / 2f)
			{
				SetButtonActive(backwardButton);
			}
			else
			{
				SetButtonInactive(backwardButton);
			}
			Vector3 localPosition18 = base.transform.localPosition;
			if (localPosition18.y < snapPositions[0] - spacing / 2f)
			{
				SetButtonActive(forwardButton);
			}
			else
			{
				SetButtonInactive(forwardButton);
			}
		}
	}
}
