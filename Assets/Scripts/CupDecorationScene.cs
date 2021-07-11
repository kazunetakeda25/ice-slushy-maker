using AdvancedMobilePaint;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CupDecorationScene : MonoBehaviour
{
	public MenuManager menuManager;

	public GameObject PopUpFingerPainting;

	public GameObject PopUpStickers;

	public GameObject PopUpPatterns;

	public Transform PatternButtonsHolder;

	public Transform StickerButtonsHolder;

	public Sprite[] PatternSprites;

	public Color[] paintingColors;

	public Sprite[] CupMaskSprites;

	public Image CupMask;

	public Image CupMask2;

	public Image CupPattern;

	public RawImage paintedTex;

	public AdvancedMobilePaint.AdvancedMobilePaint paintEngine;

	public Texture2D inkBrush;

	private Texture2D paintSurface;

	public Texture2D maskTexture;

	private Texture2D tmpTex;

	private bool bAMPInitialised;

	private Stickers stickers;

	public Image SC_CupMask;

	public Image SC_CupMask2;

	public Image SC_CupPattern;

	public RawImage SC_paintedTex;

	private bool bFirstInitAmp;

	public Transform ColorButtonBG;

	private bool bSetPatternButtons;

	private bool bSetStickerButtons;

	private int videoStickerInd = -1;

	private int videoPatternInd = -1;

	public Transform PopupAreYouSure;

	public void Awake()
	{
		paintSurface = new Texture2D(512, 512, TextureFormat.ARGB32, mipChain: false);
		ClearStartImage(paintSurface);
		paintedTex.texture = paintSurface;
		SC_paintedTex.texture = paintSurface;
		StartCoroutine("WShowAMP");
	}

	private IEnumerator Start()
	{
		BlockClicks.Instance.SetBlockAll(blockRays: true);
		stickers = base.transform.GetComponent<Stickers>();
		yield return new WaitForSeconds(0.2f);
		yield return new WaitForSeconds(1f);
		LevelTransition.Instance.ShowScene();
		yield return new WaitForSeconds(1f);
		BlockClicks.Instance.SetBlockAll(blockRays: false);
		Tutorial.Instance.ShowTutorial(0);
		EscapeButtonManager.AddEscapeButonFunction("ButtonHomeClicked", string.Empty);
	}

	private void Update()
	{
		if (!Input.GetKey(KeyCode.A))
		{
		}
	}

	private IEnumerator WShowAMP()
	{
		yield return new WaitForSeconds(0.1f);
		if (!bAMPInitialised)
		{
			paintEngine.transform.gameObject.SetActive(value: true);
			paintEngine.SetDrawingTexture(paintSurface);
			yield return new WaitForSeconds(0.1f);
			paintEngine.undoEnabled = false;
			paintEngine.multitouchEnabled = false;
			paintEngine.InitializeEverything();
			paintEngine.SetDrawingTexture(paintSurface);
			paintEngine.SetBitmapBrush(inkBrush, BrushProperties.Default, isAditiveBrush: true, brushCanDrawOnBlack: false, paintingColors[0], usesLockMasks: true, useBrushAlpha: true, null);
			ColorButtonBG.transform.position = PopUpFingerPainting.transform.Find("AnimationHolder/Body/ContentHolder/ButtonColor1").position;
			yield return new WaitForSeconds(0.1f);
			paintEngine.useLockArea = false;
			paintEngine.useAdditiveColors = false;
			paintEngine.maskTex = maskTexture;
			paintEngine.SetDrawingMask(maskTexture);
			paintEngine.SetMaskTextureMode();
			bAMPInitialised = true;
			paintEngine.CreateAreaLockMask(paintSurface.width / 2, paintSurface.height / 2);
		}
		yield return new WaitForSeconds(0.1f);
		if (bFirstInitAmp)
		{
			paintEngine.drawEnabled = true;
		}
		else
		{
			bFirstInitAmp = true;
		}
	}

	private void ClearImage(Texture2D texOriginal)
	{
		if (paintEngine.pixels.Length != 0)
		{
			int num = 0;
			Color[] pixels = texOriginal.GetPixels();
			for (int i = 0; i < pixels.Length; i++)
			{
				paintEngine.pixels[num] = 1;
				paintEngine.pixels[num + 1] = 1;
				paintEngine.pixels[num + 2] = 1;
				paintEngine.pixels[num + 3] = 0;
				num += 4;
			}
			paintEngine.textureNeedsUpdate = true;
			paintEngine.UpdateTexture();
		}
	}

	private void ClearStartImage(Texture2D texOriginal)
	{
		Color[] pixels = texOriginal.GetPixels();
		Color color = new Color(1f, 1f, 1f, 0f);
		for (int i = 0; i < pixels.Length; i++)
		{
			pixels[i] = color;
		}
		texOriginal.SetPixels(pixels);
		texOriginal.Apply();
	}

	private void SetPatternButtons()
	{
		for (int i = 0; i < PatternButtonsHolder.childCount; i++)
		{
			PatternButtonsHolder.GetChild(i).GetChild(1).gameObject.SetActive(GameData.unlockedPatterns[i] == 0);
		}
	}

	public void ButtonShowPopUpPatternClicked()
	{
		Tutorial.Instance.StopTutorial();
		if (!bSetPatternButtons)
		{
			SetPatternButtons();
		}
		menuManager.ShowPopUpMenu(PopUpPatterns);
		if (SoundManager.Instance != null)
		{
			SoundManager.Instance.Play_ButtonClick();
		}
	}

	public void ButtonClosePopUpPatternClicked()
	{
		menuManager.ClosePopUpMenu(PopUpPatterns);
		if (SoundManager.Instance != null)
		{
			SoundManager.Instance.Play_ButtonClick();
		}
	}

	public void ButtonSelectPatternClicked(int patern)
	{
		if (GameData.unlockedPatterns[patern] == 1)
		{
			CupPattern.color = new Color(1f, 1f, 1f, 1f);
			CupPattern.sprite = PatternSprites[patern];
			menuManager.ClosePopUpMenu(PopUpPatterns);
			SC_CupPattern.color = new Color(1f, 1f, 1f, 1f);
			SC_CupPattern.sprite = PatternSprites[patern];
			if (SoundManager.Instance != null)
			{
				SoundManager.Instance.Play_ButtonClick();
			}
		}
		else
		{
			videoPatternInd = patern;
			Shop.Instance.WatchVideo();
			if (SoundManager.Instance != null)
			{
				SoundManager.Instance.Play_Sound(SoundManager.Instance.EmptyFlavor);
			}
		}
	}

	private void SetStickerButtons()
	{
		for (int i = 0; i < StickerButtonsHolder.childCount; i++)
		{
			StickerButtonsHolder.GetChild(i).GetChild(1).gameObject.SetActive(GameData.unlockedStickers[i] == 0);
		}
	}

	public void ButtonShowPopUpStickerClicked()
	{
		Tutorial.Instance.StopTutorial();
		if (!bSetStickerButtons)
		{
			SetStickerButtons();
		}
		menuManager.ShowPopUpMenu(PopUpStickers);
		if (SoundManager.Instance != null)
		{
			SoundManager.Instance.Play_ButtonClick();
		}
	}

	public void ButtonClosePopUpStickerClicked()
	{
		menuManager.ClosePopUpMenu(PopUpStickers);
		if (SoundManager.Instance != null)
		{
			SoundManager.Instance.Play_ButtonClick();
		}
	}

	public void ButtonSelectStickerClicked(int stickerInd)
	{
		if (GameData.unlockedStickers[stickerInd] == 1)
		{
			menuManager.ClosePopUpMenu(PopUpStickers);
			stickers.CreateSticker(stickerInd);
			if (SoundManager.Instance != null)
			{
				SoundManager.Instance.Play_ButtonClick();
			}
		}
		else
		{
			videoStickerInd = stickerInd;
			Shop.Instance.WatchVideo();
			if (SoundManager.Instance != null)
			{
				SoundManager.Instance.Play_Sound(SoundManager.Instance.EmptyFlavor);
			}
		}
	}

	public void UnlockItem()
	{
		if (videoStickerInd > -1)
		{
			GameData.unlockedStickers[videoStickerInd] = 1;
			StickerButtonsHolder.GetChild(videoStickerInd).GetChild(1).gameObject.SetActive(value: false);
			GameData.SetCupStickersList();
			videoStickerInd = -1;
			if (SoundManager.Instance != null)
			{
				SoundManager.Instance.Play_Sound(SoundManager.Instance.Coins);
			}
		}
		else if (videoPatternInd > -1)
		{
			GameData.unlockedPatterns[videoPatternInd] = 1;
			PatternButtonsHolder.GetChild(videoPatternInd).GetChild(1).gameObject.SetActive(value: false);
			GameData.SetCupPatternList();
			videoPatternInd = -1;
			if (SoundManager.Instance != null)
			{
				SoundManager.Instance.Play_Sound(SoundManager.Instance.Coins);
			}
		}
	}

	public void ButtonShowPopUpFingerPaintingClicked()
	{
		Tutorial.Instance.StopTutorial();
		ClearImage(paintSurface);
		menuManager.ShowPopUpMenu(PopUpFingerPainting);
		StartCoroutine("WShowAMP");
		if (SoundManager.Instance != null)
		{
			SoundManager.Instance.Play_ButtonClick();
		}
	}

	public void ButtonClosePopUpFingerPaintingClicked()
	{
		paintEngine.drawEnabled = false;
		menuManager.ClosePopUpMenu(PopUpFingerPainting);
		if (SoundManager.Instance != null)
		{
			SoundManager.Instance.Play_ButtonClick();
		}
		CupMask.sprite = CupMaskSprites[0];
		CupMask2.sprite = CupMaskSprites[0];
		if (paintEngine.pixels.Length != 0)
		{
			ClearImage(paintSurface);
			paintedTex.texture = paintEngine.tex;
			SC_paintedTex.texture = paintEngine.tex;
			SC_CupMask.sprite = CupMaskSprites[0];
			SC_CupMask2.sprite = CupMaskSprites[0];
		}
	}

	public void ButtonChangeColorClicked(int colorInd)
	{
		if (SoundManager.Instance != null)
		{
			SoundManager.Instance.Play_ButtonClick();
		}
		paintEngine.SetBitmapBrush(inkBrush, BrushProperties.Default, isAditiveBrush: false, brushCanDrawOnBlack: false, paintingColors[colorInd], usesLockMasks: true, useBrushAlpha: true, null);
		ColorButtonBG.transform.position = PopUpFingerPainting.transform.Find("AnimationHolder/Body/ContentHolder/ButtonColor" + (colorInd + 1).ToString()).position;
	}

	public void ButtonAcceptFingerPaintingClicked()
	{
		CupMask.sprite = CupMaskSprites[1];
		CupMask2.sprite = CupMaskSprites[1];
		paintEngine.drawEnabled = false;
		paintedTex.texture = paintEngine.tex;
		menuManager.ClosePopUpMenu(PopUpFingerPainting);
		SC_CupMask.sprite = CupMaskSprites[1];
		SC_CupMask2.sprite = CupMaskSprites[1];
		SC_paintedTex.texture = paintEngine.tex;
		if (SoundManager.Instance != null)
		{
			SoundManager.Instance.Play_ButtonClick();
		}
	}

	public void ButtonReplayClicked()
	{
		CupMask.sprite = CupMaskSprites[0];
		CupMask2.sprite = CupMaskSprites[0];
		CupPattern.color = new Color(1f, 1f, 1f, 0f);
		if (paintEngine.pixels.Length != 0)
		{
			ClearImage(paintSurface);
			paintedTex.texture = paintEngine.tex;
			SC_paintedTex.texture = paintEngine.tex;
			stickers.DeleteAllStickers();
			SC_CupMask.sprite = CupMaskSprites[0];
			SC_CupMask2.sprite = CupMaskSprites[0];
			SC_CupPattern.color = new Color(1f, 1f, 1f, 0f);
			if (SoundManager.Instance != null)
			{
				SoundManager.Instance.Play_ButtonClick();
			}
			AdsManager.Instance.ShowInterstitial();
		}
	}

	public void ButtonNextClicked()
	{
		if (SoundManager.Instance != null)
		{
			SoundManager.Instance.Play_ButtonClick();
		}
		BlockClicks.Instance.SetBlockAll(blockRays: true);
		GameObject.Find("Canvas/DecorationTransform").GetComponent<DecorationTransform>().bMoveDecoratins = false;
		CaptureImage.Instance.ScreenshotCup();
		StartCoroutine("LoadNextScene");
		AdsManager.Instance.ShowInterstitial();
	}

	private IEnumerator LoadNextScene()
	{
		yield return new WaitForSeconds(1f);
		LevelTransition.Instance.HideSceneAndLoadNext("SelectFlavor");
	}

	public void ButtonHomeClicked()
	{
		if (SoundManager.Instance != null)
		{
			SoundManager.Instance.Play_ButtonClick();
		}
		BlockClicks.Instance.SetBlockAll(blockRays: true);
		BlockClicks.Instance.SetBlockAllDelay(0.5f, blockRays: false);
		if (SoundManager.Instance != null)
		{
			SoundManager.Instance.Play_ButtonClick();
		}
		PopupAreYouSure.parent.parent.GetComponent<MenuManager>().ShowPopUpMenu(PopupAreYouSure.gameObject);
	}

	public void ButtonHomeYesClicked()
	{
		BlockClicks.Instance.SetBlockAll(blockRays: true);
		if (SoundManager.Instance != null)
		{
			SoundManager.Instance.Play_ButtonClick();
		}
		LevelTransition.Instance.HideSceneAndLoadNext("HomeScene");
		AdsManager.Instance.ShowInterstitial();
	}

	public void ButtonHomeNoClicked()
	{
		BlockClicks.Instance.SetBlockAll(blockRays: true);
		BlockClicks.Instance.SetBlockAllDelay(0.5f, blockRays: false);
		if (SoundManager.Instance != null)
		{
			SoundManager.Instance.Play_ButtonClick();
		}
		PopupAreYouSure.parent.parent.GetComponent<MenuManager>().ClosePopUpMenu(PopupAreYouSure.gameObject);
		if (EscapeButtonManager.EscapeButonFunctionStack.Count == 0)
		{
			EscapeButtonManager.AddEscapeButonFunction("ButtonHomeClicked", string.Empty);
		}
	}
}
