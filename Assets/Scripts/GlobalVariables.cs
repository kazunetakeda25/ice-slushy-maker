using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
	public static int gameMod;

	public static int currentLvl;

	public static bool categoriesBought;

	public static int numberOfStart;

	public static string mgDrawShaper_SavedProgres = string.Empty;

	public static bool removeAdsOwned;

	public static string applicationID;

	private void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
		applicationID = "com.Crazy.Unicorn.Icy.Food.Slushy.Maker";
	}
}
