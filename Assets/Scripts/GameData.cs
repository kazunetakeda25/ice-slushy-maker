using System;
using UnityEngine;

public class GameData
{
	public static Sprite FinishedCupSprite;

	public static Texture FinishedSlushSprite;

	public static bool bFillFlavor = false;

	public static int selectedFlavor = -1;

	public static int selectedMold = 0;

	public static Color selectedFlavorColor = new Color(0f, 0.8f, 0f, 1f);

	public static Texture SlushImage;

	public static int SlushFillQuantity = 0;

	public static Color SlushLastUsedColor;

	public static Vector2 strawImgSize = new Vector2(200f, 1000f);

	public static int strawImgIndex;

	public static Vector3 strawImgPos = Vector3.zero;

	public static int[] FlavorsContainersList = new int[10]
	{
		99,
		0,
		-1,
		0,
		50,
		99,
		99,
		99,
		0,
		0
	};

	public static int[] unlockedSlushDecorationsFruits = new int[15]
	{
		1,
		0,
		1,
		0,
		1,
		0,
		1,
		0,
		1,
		0,
		1,
		0,
		1,
		0,
		1
	};

	public static int[] unlockedSlushDecorationsCandies = new int[15]
	{
		1,
		0,
		1,
		0,
		1,
		0,
		1,
		0,
		1,
		0,
		1,
		0,
		1,
		0,
		1
	};

	public static int[] unlockedStickers = new int[16]
	{
		1,
		0,
		1,
		0,
		1,
		0,
		1,
		0,
		1,
		0,
		1,
		0,
		1,
		0,
		1,
		0
	};

	public static int[] unlockedPatterns = new int[16]
	{
		1,
		0,
		1,
		0,
		1,
		0,
		1,
		0,
		1,
		0,
		1,
		0,
		1,
		0,
		1,
		0
	};

	public static string sTestiranje = string.Empty;

	public static void Init()
	{
		GetPurchasedItems();
		GetAllData();
	}

	public static void GetAllData()
	{
		GetFlavorsContainersList();
		GetCupStickersList();
		GetCupPatternList();
		GetSlushDecorationsFruitsList();
		GetSlushDecorationsCandiesList();
	}

	public static void GetSlushDecorationsFruitsList()
	{
		string @string = PlayerPrefs.GetString("Data3", "1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,");
		string[] array = @string.Split(new char[1]
		{
			','
		}, StringSplitOptions.RemoveEmptyEntries);
		for (int i = 0; i < array.Length; i++)
		{
			if (Shop.UnlockAll == 2)
			{
				unlockedSlushDecorationsFruits[i] = 1;
			}
			else
			{
				unlockedSlushDecorationsFruits[i] = int.Parse(array[i]);
			}
		}
	}

	public static void SetSlushDecorationsFruitsList()
	{
		string text = string.Empty;
		for (int i = 0; i < unlockedSlushDecorationsFruits.Length; i++)
		{
			text = text + unlockedSlushDecorationsFruits[i].ToString() + ",";
		}
		PlayerPrefs.SetString("Data3", text);
	}

	public static void GetSlushDecorationsCandiesList()
	{
		string @string = PlayerPrefs.GetString("Data4", "1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,");
		string[] array = @string.Split(new char[1]
		{
			','
		}, StringSplitOptions.RemoveEmptyEntries);
		for (int i = 0; i < array.Length; i++)
		{
			if (Shop.UnlockAll == 2)
			{
				unlockedSlushDecorationsCandies[i] = 1;
			}
			else
			{
				unlockedSlushDecorationsCandies[i] = int.Parse(array[i]);
			}
		}
	}

	public static void SetSlushDecorationsCandiesList()
	{
		string text = string.Empty;
		for (int i = 0; i < unlockedSlushDecorationsCandies.Length; i++)
		{
			text = text + unlockedSlushDecorationsCandies[i].ToString() + ",";
		}
		PlayerPrefs.SetString("Data4", text);
	}

	public static void GetCupStickersList()
	{
		string @string = PlayerPrefs.GetString("Data5", "1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,");
		string[] array = @string.Split(new char[1]
		{
			','
		}, StringSplitOptions.RemoveEmptyEntries);
		for (int i = 0; i < array.Length; i++)
		{
			if (Shop.UnlockAll == 2)
			{
				unlockedStickers[i] = 1;
			}
			else
			{
				unlockedStickers[i] = int.Parse(array[i]);
			}
		}
	}

	public static void SetCupStickersList()
	{
		string text = string.Empty;
		for (int i = 0; i < unlockedStickers.Length; i++)
		{
			text = text + unlockedStickers[i].ToString() + ",";
		}
		PlayerPrefs.SetString("Data5", text);
	}

	public static void GetCupPatternList()
	{
		string @string = PlayerPrefs.GetString("Data6", "1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,");
		string[] array = @string.Split(new char[1]
		{
			','
		}, StringSplitOptions.RemoveEmptyEntries);
		for (int i = 0; i < array.Length; i++)
		{
			if (Shop.UnlockAll == 2)
			{
				unlockedPatterns[i] = 1;
			}
			else
			{
				unlockedPatterns[i] = int.Parse(array[i]);
			}
		}
	}

	public static void SetCupPatternList()
	{
		string text = string.Empty;
		for (int i = 0; i < unlockedPatterns.Length; i++)
		{
			text = text + unlockedPatterns[i].ToString() + ",";
		}
		PlayerPrefs.SetString("Data6", text);
	}

	public static void GetFlavorsContainersList()
	{
		string @string = PlayerPrefs.GetString("Data2", "0;-1;0;-1;0;-1;0;-1;0;-1;");
		string[] array = @string.Split(new char[1]
		{
			';'
		}, StringSplitOptions.RemoveEmptyEntries);
		for (int i = 0; i < array.Length; i++)
		{
			FlavorsContainersList[i] = int.Parse(array[i]);
			if (Shop.UnlockAll == 2 && FlavorsContainersList[i] == -1)
			{
				FlavorsContainersList[i] = 0;
			}
		}
	}

	public static void SetFlavorsContainersList()
	{
		string text = string.Empty;
		for (int i = 0; i < FlavorsContainersList.Length; i++)
		{
			text = text + FlavorsContainersList[i].ToString() + ";";
		}
		PlayerPrefs.SetString("Data2", text);
	}

	public static void ResetSlushImages()
	{
		SlushFillQuantity = 0;
		SlushImage = null;
		FinishedCupSprite = null;
		FinishedSlushSprite = null;
		bFillFlavor = false;
		selectedFlavor = -1;
		selectedMold = 0;
		selectedFlavorColor = new Color(0f, 0.8f, 0f, 1f);
		SlushFillQuantity = 0;
		SlushLastUsedColor = Color.white;
	}

	private static void SetUnlockedFromString(ref bool[] unlockedItems, string data)
	{
		if (!(data != string.Empty))
		{
			return;
		}
		string[] array = data.Split(';');
		for (int i = 0; i < array.Length; i++)
		{
			int result = 0;
			int.TryParse(array[i], out result);
			if (result < unlockedItems.Length)
			{
				unlockedItems[result] = true;
			}
		}
	}

	public static void GetPurchasedItems()
	{
		string @string = PlayerPrefs.GetString("Data0", "33114");
		@string = @string.Replace("<", "9");
		@string = @string.Replace("7>q", "8");
		@string = @string.Replace("nmFs", "7");
		@string = @string.Replace("Vy;", "6");
		@string = @string.Replace("*2", "5");
		@string = @string.Replace("H", "4");
		@string = @string.Replace("JE", "3");
		@string = @string.Replace("B#", "2");
		@string = @string.Replace("+0", "1");
		@string = @string.Replace("Kce", "0");
		int num = int.Parse(@string);
		int num2 = num - 33114;
		Shop.SpecialOffer = Mathf.FloorToInt(num2 / 100);
		num2 -= Shop.SpecialOffer * 100;
		Shop.UnlockAll = Mathf.FloorToInt(num2 / 10);
		Shop.RemoveAds = num2 - Shop.UnlockAll * 10;
		if (Shop.SpecialOffer == 2)
		{
			Shop.UnlockAll = 2;
			Shop.RemoveAds = 2;
			Shop.bShowSpecialOfferInShop = false;
		}
		else if (Shop.UnlockAll == 2 && Shop.RemoveAds == 2)
		{
			Shop.SpecialOffer = 2;
		}
		if (Shop.RemoveAds == 2)
		{
			GlobalVariables.removeAdsOwned = true;
		}
	}

	public static void SetPurchasedItems()
	{
		if (Shop.SpecialOffer == 2)
		{
			Shop.UnlockAll = 2;
			Shop.RemoveAds = 2;
			Shop.bShowSpecialOfferInShop = false;
		}
		else if (Shop.UnlockAll == 2 && Shop.RemoveAds == 2)
		{
			Shop.SpecialOffer = 2;
		}
		if (Shop.RemoveAds == 2)
		{
			GlobalVariables.removeAdsOwned = true;
		}
		int num = Shop.SpecialOffer * 100 + Shop.UnlockAll * 10 + Shop.RemoveAds;
		string text = (num + 33114).ToString();
		text = text.Replace("0", "Kce");
		text = text.Replace("1", "+0");
		text = text.Replace("2", "B#");
		text = text.Replace("3", "JE");
		text = text.Replace("4", "H");
		text = text.Replace("5", "*2");
		text = text.Replace("6", "Vy;");
		text = text.Replace("7", "nmFs");
		text = text.Replace("8", "7>q");
		text = text.Replace("9", "<");
		PlayerPrefs.SetString("Data0", text);
		PlayerPrefs.Save();
		GetAllData();
	}

	public static void IncrementButtonBackForwardClickedCount()
	{
		if (Shop.RemoveAds != 2)
		{
			AdsManager.Instance.ShowInterstitial();
		}
	}

	public static void IncrementButtonHomeClickedCount()
	{
		if (Shop.RemoveAds != 2)
		{
			AdsManager.Instance.ShowInterstitial();
		}
	}

	public static void IncrementButtonRestartClickedCount()
	{
		if (Shop.RemoveAds != 2)
		{
			AdsManager.Instance.ShowInterstitial();
		}
	}
}
