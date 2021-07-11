using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
	private MenuManager menuManager;

	public Text txtPrice_RemoveAds;

	public Text txtPrice_UnlockAll;

	public Text txtPrice_SpecialOffer;

	private CanvasGroup BlockAll;

	public GameObject TabButtonSpecialOffer;

	public static bool bShowShop;

	private void Awake()
	{
		InitPrices();
	}

	private void Start()
	{
		bShowShop = false;
		menuManager = GameObject.Find("Canvas").GetComponent<MenuManager>();
		BlockAll = GameObject.Find("BlockAll").GetComponent<CanvasGroup>();
		InitPrices();
		SetShopItems();
	}

	public void InitPrices()
	{
		if (Shop.RemoveAds == 2)
		{
			txtPrice_RemoveAds.text = "bought";
			txtPrice_RemoveAds.transform.parent.parent.GetComponent<Button>().enabled = false;
			txtPrice_RemoveAds.transform.parent.GetComponent<Button>().enabled = false;
			txtPrice_RemoveAds.transform.parent.GetComponent<Image>().enabled = false;
		}
		if (Shop.UnlockAll == 2)
		{
			txtPrice_UnlockAll.text = "bought";
			txtPrice_UnlockAll.transform.parent.parent.GetComponent<Button>().enabled = false;
			txtPrice_UnlockAll.transform.parent.GetComponent<Button>().enabled = false;
			txtPrice_UnlockAll.transform.parent.GetComponent<Image>().enabled = false;
		}
		if (Shop.SpecialOffer == 2 && txtPrice_SpecialOffer != null)
		{
			txtPrice_SpecialOffer.text = "bought";
			txtPrice_SpecialOffer.transform.parent.parent.GetComponent<Button>().enabled = false;
			txtPrice_SpecialOffer.transform.parent.GetComponent<Button>().enabled = false;
			txtPrice_SpecialOffer.transform.parent.GetComponent<Image>().enabled = false;
		}
		if (TabButtonSpecialOffer != null)
		{
			TabButtonSpecialOffer.SetActive(Shop.bShowSpecialOfferInShop);
		}
	}

	private void Update()
	{
	}

	public void btnBuyClick(string btnID)
	{
		BlockAll.blocksRaycasts = true;
		if (btnID == "RemoveAds" && Shop.RemoveAds == 0)
		{
			Shop.Instance.SendShopRequest(btnID);
		}
		if (btnID == "UnlockAll" && Shop.UnlockAll == 0)
		{
			Shop.Instance.SendShopRequest(btnID);
		}
		if (btnID == "SpecialOffer" && Shop.SpecialOffer == 0)
		{
			Shop.Instance.SendShopRequest(btnID);
		}
		StartCoroutine(SetBlockAll(1f, blockRays: false));
		SoundManager.Instance.Play_ButtonClick();
		if (GameData.sTestiranje.Contains("TestPopUpTransaction;"))
		{
			menuManager.ShowPopUpMessage("TEST", "Transacition succesful");
		}
	}

	public void SetShopItems()
	{
		if (Shop.RemoveAds == 2)
		{
			txtPrice_RemoveAds.text = "bought";
			txtPrice_RemoveAds.transform.parent.parent.GetComponent<Button>().enabled = false;
			txtPrice_RemoveAds.transform.parent.GetComponent<Button>().enabled = false;
			txtPrice_RemoveAds.transform.parent.GetComponent<Image>().enabled = false;
			StartCoroutine(SetBlockAll(1f, blockRays: false));
		}
		if (Shop.UnlockAll == 2)
		{
			txtPrice_UnlockAll.text = "bought";
			txtPrice_UnlockAll.transform.parent.parent.GetComponent<Button>().enabled = false;
			txtPrice_UnlockAll.transform.parent.GetComponent<Button>().enabled = false;
			txtPrice_UnlockAll.transform.parent.GetComponent<Image>().enabled = false;
		}
		if (Shop.SpecialOffer == 2 && txtPrice_SpecialOffer != null)
		{
			txtPrice_SpecialOffer.text = "bought";
			txtPrice_SpecialOffer.transform.parent.parent.GetComponent<Button>().enabled = false;
			txtPrice_SpecialOffer.transform.parent.GetComponent<Button>().enabled = false;
			txtPrice_SpecialOffer.transform.parent.GetComponent<Image>().enabled = false;
		}
		if (TabButtonSpecialOffer != null)
		{
			TabButtonSpecialOffer.SetActive(Shop.bShowSpecialOfferInShop);
		}
	}

	private IEnumerator SetBlockAll(float time, bool blockRays)
	{
		if (BlockAll == null)
		{
			BlockAll = GameObject.Find("BlockAll").GetComponent<CanvasGroup>();
		}
		yield return new WaitForSeconds(time);
		BlockAll.blocksRaycasts = blockRays;
	}

	public void btnClicked_PlaySound()
	{
		SoundManager.Instance.Play_ButtonClick();
	}

	public void btnRestorePurchaseClick()
	{
	}

	private void PurchaseRestored()
	{
		CanvasGroup component = GameObject.Find("Message_ItemsRestored").GetComponent<CanvasGroup>();
		component.interactable = true;
		component.blocksRaycasts = true;
		StartCoroutine(ShowMsgItemsRestored(component));
	}

	private IEnumerator ShowMsgItemsRestored(CanvasGroup canvasGroup_ItemsRestored)
	{
		for (float i = 0f; i <= 1f; i += 0.1f)
		{
			canvasGroup_ItemsRestored.alpha = i;
			yield return new WaitForSeconds(0.03f);
		}
		canvasGroup_ItemsRestored.alpha = 1f;
		StartCoroutine(CloseMsgItemsRestored(canvasGroup_ItemsRestored));
	}

	private IEnumerator CloseMsgItemsRestored(CanvasGroup canvasGroup_ItemsRestored)
	{
		yield return new WaitForSeconds(2f);
		for (float i = 1f; i >= 0f; i -= 0.1f)
		{
			canvasGroup_ItemsRestored.alpha = i;
			yield return new WaitForSeconds(0.03f);
		}
		canvasGroup_ItemsRestored.interactable = false;
		canvasGroup_ItemsRestored.blocksRaycasts = false;
		canvasGroup_ItemsRestored.alpha = 0f;
	}
}
