using UnityEngine;
using UnityEngine.UI;

public class FillCup : MonoBehaviour
{
	public Color[] FlavorColors;

	private Color addColor = Color.white;

	private Color addColorOld = Color.white;

	private Color tmpCol;

	private int fillQuantity;

	private int maxFillQuantity = 256;

	private int fillStep;

	private int colorChangePlace;

	private float fillQuantityTmp;

	public RectTransform MaskFill;

	public RawImage SlushyImage;

	public Texture2D SlushiFillBW;

	public Texture2D texture;

	public Texture2D AddSlushiAlpha;

	private float positionDelta;

	private bool bAddSlushy;

	private float[] pixelsAdd;

	public Image BlenderFill;

	public Image BlenderSplash;

	public Image CupFront;

	public SlushyFlavorsSlider slushyFlavorSlider;

	private void Start()
	{
		if (GameData.selectedFlavor > 0 && GameData.bFillFlavor)
		{
			Color color = GameObject.Find("FlavorsHolder/AnimationHolder/ScrollRect/Content/Item" + GameData.selectedFlavor.ToString().PadLeft(2, '0') + "/SlushyMask/Slushy").GetComponent<Image>().color;
			BlenderFill.color = color;
			BlenderSplash.color = color;
			if (GameData.SlushImage != null)
			{
				texture = (Texture2D)GameData.SlushImage;
				SlushyImage.texture = GameData.SlushImage;
				fillQuantity = GameData.SlushFillQuantity;
				fillQuantityTmp = fillQuantity;
				addColor = GameData.SlushLastUsedColor;
				addColorOld = GameData.SlushLastUsedColor;
				tmpCol = GameData.SlushLastUsedColor;
			}
		}
		else
		{
			texture = new Texture2D(SlushiFillBW.width, SlushiFillBW.height, TextureFormat.RGBA32, mipChain: false);
			Color[] array = new Color[SlushiFillBW.width * SlushiFillBW.height];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new Color(1f, 1f, 1f, 0f);
			}
			texture.SetPixels(array);
			texture.Apply(updateMipmaps: false);
			SlushyImage.texture = texture;
		}
		pixelsAdd = new float[AddSlushiAlpha.width * AddSlushiAlpha.height];
		Color[] pixels = AddSlushiAlpha.GetPixels();
		for (int j = 0; j < AddSlushiAlpha.width * AddSlushiAlpha.height; j++)
		{
			pixelsAdd[j] = pixels[j].a;
		}
		fillStep = SlushiFillBW.height / maxFillQuantity;
		maxFillQuantity -= AddSlushiAlpha.height / fillStep;
		if (GameData.FinishedCupSprite != null)
		{
			CupFront.sprite = GameData.FinishedCupSprite;
		}
	}

	private void Update()
	{
		if (!bAddSlushy)
		{
			return;
		}
		fillQuantityTmp += Time.deltaTime * 25f;
		if (fillQuantityTmp > (float)fillQuantity && maxFillQuantity > fillQuantity)
		{
			fillQuantity++;
			AddSlushy();
			slushyFlavorSlider.DecreaseSlushyInContainer();
		}
		if (maxFillQuantity == fillQuantity)
		{
			bAddSlushy = false;
			Camera.main.SendMessage("CupFull");
			GameData.SlushImage = SlushyImage.texture;
			GameData.SlushFillQuantity = maxFillQuantity;
			slushyFlavorSlider.HandleUp();
			slushyFlavorSlider.EnableMove(_bEnableMove: false);
			if (SoundManager.Instance != null)
			{
				SoundManager.Instance.Stop_Sound(SoundManager.Instance.FillCup);
			}
		}
	}

	public void SaveSlushFillData()
	{
		GameData.SlushImage = SlushyImage.texture;
		GameData.SlushFillQuantity = fillQuantity;
		GameData.SlushLastUsedColor = tmpCol;
	}

	public void StartFillingCup(int colorIndex)
	{
		if (SoundManager.Instance != null)
		{
			SoundManager.Instance.Play_Sound(SoundManager.Instance.FillCup);
		}
		SetColor(colorIndex);
		bAddSlushy = true;
	}

	public void StopFillingCup()
	{
		if (SoundManager.Instance != null)
		{
			SoundManager.Instance.Stop_Sound(SoundManager.Instance.FillCup);
		}
		bAddSlushy = false;
	}

	private void SetColor(int _color)
	{
		if (fillQuantity > 3)
		{
			addColorOld = Color.Lerp(addColorOld, addColor, (float)(fillQuantity - colorChangePlace) / (float)AddSlushiAlpha.height);
		}
		else
		{
			addColorOld = FlavorColors[_color];
		}
		addColor = FlavorColors[_color];
		colorChangePlace = fillQuantity;
	}

	public void AddSlushy()
	{
		Color[] pixels = SlushiFillBW.GetPixels(0, fillQuantity * fillStep, AddSlushiAlpha.width, AddSlushiAlpha.height);
		int num = 0;
		float num2 = 0f;
		for (int i = 0; i < AddSlushiAlpha.height; i++)
		{
			if (i > 10)
			{
				num2 = (float)i / 16f;
			}
			if (num2 > 1f)
			{
				num2 = 1f;
			}
			tmpCol = addColor;
			if (fillQuantity > 3)
			{
				tmpCol = Color.Lerp(addColorOld, addColor, (float)(fillQuantity - colorChangePlace) / (float)AddSlushiAlpha.height);
			}
			for (int j = 0; j < AddSlushiAlpha.width; j++)
			{
				num = i * AddSlushiAlpha.width + j;
				pixels[num].a = pixelsAdd[num];
				pixels[num].r += tmpCol.r * pixelsAdd[num];
				pixels[num].g += tmpCol.g * pixelsAdd[num];
				pixels[num].b += tmpCol.b * pixelsAdd[num];
			}
		}
		texture.SetPixels(0, fillQuantity * fillStep, AddSlushiAlpha.width, AddSlushiAlpha.height, pixels);
		texture.Apply(updateMipmaps: false);
		SlushyImage.texture = texture;
	}
}
