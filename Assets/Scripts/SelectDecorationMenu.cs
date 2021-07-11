using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SelectDecorationMenu : MonoBehaviour
{
	public SlushDecorationScene slushDecorationScene;

	public ScrollRect scrollRect;

	public RectTransform content;

	public Sprite[] fruits;

	public Sprite[] candies;

	public Image[] btnImage;

	private bool bTutorialOver;

	public Animator animMenu;

	private float speed = 1f;

	private bool bNext;

	private bool bPrev;

	private float inertia = 1f;

	public bool bHideNativeAd = true;

	private float offset = -2.6f;

	private int videoBtnNo;

	private int videoActiveMenu;

	private void Start()
	{
		slushDecorationScene = Camera.main.GetComponent<SlushDecorationScene>();
		MenuChanged();
	}

	private void Update()
	{
		if ((bPrev || inertia < -0.05f) && scrollRect.horizontalNormalizedPosition > 0f)
		{
			if (bPrev)
			{
				scrollRect.horizontalNormalizedPosition -= Time.deltaTime * speed;
				return;
			}
			scrollRect.horizontalNormalizedPosition += Time.deltaTime * inertia;
			inertia *= 0.95f;
		}
		else if ((bNext || inertia > 0.05f) && scrollRect.horizontalNormalizedPosition < 1f)
		{
			if (bNext)
			{
				scrollRect.horizontalNormalizedPosition += Time.deltaTime * speed;
				return;
			}
			scrollRect.horizontalNormalizedPosition += Time.deltaTime * inertia;
			inertia *= 0.95f;
		}
	}

	public IEnumerator ChangeMenu(int menuNo)
	{
		animMenu.Play("HideMenu");
		yield return new WaitForSeconds(1f);
		ShowMenu(menuNo);
	}

	public void ShowMenu(int menuNo)
	{
		for (int i = 0; i < btnImage.Length; i++)
		{
			if (menuNo == 1)
			{
				btnImage[i].sprite = candies[i];
				btnImage[i].transform.parent.GetChild(1).gameObject.SetActive(GameData.unlockedSlushDecorationsCandies[i] == 0);
			}
			else
			{
				btnImage[i].sprite = fruits[i];
				btnImage[i].transform.parent.GetChild(1).gameObject.SetActive(GameData.unlockedSlushDecorationsFruits[i] == 0);
			}
		}
		animMenu.Play("ShowMenu");
	}

	public void HideMenu()
	{
		animMenu.Play("HideMenu");
	}

	public void ButtonPrevDown()
	{
		if (SoundManager.Instance != null)
		{
			SoundManager.Instance.Play_Sound(SoundManager.Instance.ButtonClick2);
		}
		bNext = false;
		bPrev = true;
		inertia = 0f;
	}

	public void ButtonPrevUp()
	{
		bNext = false;
		bPrev = false;
		inertia = 0f - speed;
	}

	public void MenuButtonClick(int btnNo)
	{
		if (slushDecorationScene.activeMenu == 1)
		{
			if (GameData.unlockedSlushDecorationsCandies[btnNo - 1] == 1)
			{
				slushDecorationScene.SetCreateDecoration(candies[btnNo - 1]);
				if (SoundManager.Instance != null)
				{
					SoundManager.Instance.Play_Sound(SoundManager.Instance.ButtonClick2);
				}
			}
			else
			{
				videoBtnNo = btnNo;
				videoActiveMenu = slushDecorationScene.activeMenu;
				Shop.Instance.WatchVideo();
				if (SoundManager.Instance != null)
				{
					SoundManager.Instance.Play_Sound(SoundManager.Instance.EmptyFlavor);
				}
			}
		}
		else if (GameData.unlockedSlushDecorationsFruits[btnNo - 1] == 1)
		{
			slushDecorationScene.SetCreateDecoration(fruits[btnNo - 1]);
			if (SoundManager.Instance != null)
			{
				SoundManager.Instance.Play_Sound(SoundManager.Instance.ButtonClick2);
			}
		}
		else
		{
			videoBtnNo = btnNo;
			videoActiveMenu = slushDecorationScene.activeMenu;
			Shop.Instance.WatchVideo();
			if (SoundManager.Instance != null)
			{
				SoundManager.Instance.Play_Sound(SoundManager.Instance.EmptyFlavor);
			}
		}
		Tutorial.Instance.StopTutorial();
		if (!bTutorialOver)
		{
			Tutorial.Instance.ShowTutorial(1);
			bTutorialOver = true;
		}
	}

	public void UnlockItem()
	{
		if (videoActiveMenu == 1)
		{
			GameData.unlockedSlushDecorationsCandies[videoBtnNo - 1] = 1;
			btnImage[videoBtnNo - 1].transform.parent.GetChild(1).gameObject.SetActive(value: false);
			GameData.SetSlushDecorationsCandiesList();
		}
		else
		{
			GameData.unlockedSlushDecorationsFruits[videoBtnNo - 1] = 1;
			btnImage[videoBtnNo - 1].transform.parent.GetChild(1).gameObject.SetActive(value: false);
			GameData.SetSlushDecorationsFruitsList();
		}
		if (SoundManager.Instance != null)
		{
			SoundManager.Instance.Play_Sound(SoundManager.Instance.Coins);
		}
	}

	public void ButtonNextDown()
	{
		if (SoundManager.Instance != null)
		{
			SoundManager.Instance.Play_Sound(SoundManager.Instance.ButtonClick2);
		}
		bNext = true;
		bPrev = false;
		inertia = 0f;
	}

	public void ButtonNextUp()
	{
		bNext = false;
		bPrev = false;
		inertia = speed;
	}

	public void MenuChanged()
	{
		speed = 0.35f * content.rect.width / (1f + (content.rect.width - scrollRect.GetComponent<RectTransform>().rect.width));
	}
}
