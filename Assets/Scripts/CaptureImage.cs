using UnityEngine;
using UnityEngine.UI;

public class CaptureImage : MonoBehaviour
{
	public static CaptureImage Instance;

	public GameObject[] HideOnScreenCapture;

	public RectTransform DecorationHolder;

	public Camera camera2;

	public Canvas canvas1;

	public Canvas canvas2;

	public RectTransform RTDecorationHolder;

	public RectTransform CupImageRect;

	private Vector2 RTDecorationHolderStartPos;

	public RectTransform SC_CupImageRect;

	public Transform StickersHolderMask;

	public Transform SC_StickersHolderMask;

	public Transform StickersHolder;

	private Texture2D _Texture;

	private string _DirectoryName = "FrozenSlushy";

	private string _PictureName = "Slush";

	public RawImage savedImg;

	private void Start()
	{
		Instance = this;
	}

	private void Update()
	{
	}

	public void ScreenshotCup()
	{
		StickersHolder.SetParent(SC_StickersHolderMask);
		StickersHolder.localPosition = Vector2.zero;
		int pixelWidth = camera2.pixelWidth;
		int pixelHeight = camera2.pixelHeight;
		float num = (float)camera2.pixelHeight / 1400f;
		float num2 = SC_CupImageRect.rect.width * num;
		Vector3 localScale = SC_CupImageRect.transform.localScale;
		int num3 = Mathf.CeilToInt(num2 * localScale.x);
		float num4 = SC_CupImageRect.rect.height * num;
		Vector3 localScale2 = SC_CupImageRect.transform.localScale;
		int num5 = Mathf.CeilToInt(num4 * localScale2.x);
		RenderTexture renderTexture = new RenderTexture(pixelWidth, pixelHeight, 32);
		camera2.targetTexture = renderTexture;
		Texture2D texture2D = new Texture2D(num3, num5, TextureFormat.ARGB32, mipChain: false);
		camera2.Render();
		RenderTexture.active = renderTexture;
		Texture2D texture2D2 = texture2D;
		float x = (float)(pixelWidth - num3) / 2f;
		float num6 = (float)(pixelHeight - num5) / 2f;
		Vector2 anchoredPosition = SC_CupImageRect.anchoredPosition;
		texture2D2.ReadPixels(new Rect(x, num6 + anchoredPosition.y * num, num3, num5), 0, 0);
		Texture2D texture2D3 = texture2D;
		float x2 = (float)(pixelWidth - num3) / 2f;
		float num7 = (float)(pixelHeight - num5) / 2f;
		Vector2 anchoredPosition2 = SC_CupImageRect.anchoredPosition;
		texture2D3.ReadPixels(new Rect(x2, num7 + anchoredPosition2.y * num, num3, num5), 0, 0);
		texture2D.Apply();
		camera2.targetTexture = null;
		RenderTexture.active = null;
		renderTexture.Release();
		UnityEngine.Object.Destroy(renderTexture);
		StickersHolder.SetParent(StickersHolderMask);
		StickersHolder.localPosition = Vector2.zero;
		GameData.FinishedCupSprite = Sprite.Create(texture2D, new Rect(0f, 0f, num3, num5), Vector2.zero);
	}

	public void ScreenshotSlush()
	{
		int pixelWidth = camera2.pixelWidth;
		int pixelHeight = camera2.pixelHeight;
		float scaleFactor = canvas2.scaleFactor;
		float num = SC_CupImageRect.rect.width * scaleFactor;
		Vector3 localScale = SC_CupImageRect.transform.localScale;
		int num2 = Mathf.CeilToInt(num * localScale.x);
		float num3 = SC_CupImageRect.rect.height * scaleFactor;
		Vector3 localScale2 = SC_CupImageRect.transform.localScale;
		int num4 = Mathf.CeilToInt(num3 * localScale2.x);
		RenderTexture renderTexture = new RenderTexture(pixelWidth, pixelHeight, 32);
		camera2.targetTexture = renderTexture;
		Texture2D texture2D = new Texture2D(num2, num4, TextureFormat.ARGB32, mipChain: false);
		camera2.Render();
		RenderTexture.active = renderTexture;
		Texture2D texture2D2 = texture2D;
		float x = (float)(pixelWidth - num2) / 2f;
		float num5 = (float)(pixelHeight - num4) / 2f;
		Vector2 anchoredPosition = SC_CupImageRect.anchoredPosition;
		texture2D2.ReadPixels(new Rect(x, num5 + anchoredPosition.y * scaleFactor, num2, num4), 0, 0);
		texture2D.Apply();
		camera2.targetTexture = null;
		RenderTexture.active = null;
		renderTexture.Release();
		UnityEngine.Object.Destroy(renderTexture);
		GameData.FinishedSlushSprite = texture2D;
	}

	public void CaptureScreenForGallery()
	{
		int pixelWidth = camera2.pixelWidth;
		int pixelHeight = camera2.pixelHeight;
		float scaleFactor = canvas2.scaleFactor;
		RenderTexture renderTexture = new RenderTexture(pixelWidth, pixelHeight, 32);
		camera2.targetTexture = renderTexture;
		Texture2D texture2D = new Texture2D(pixelWidth, pixelHeight, TextureFormat.ARGB32, mipChain: false);
		camera2.Render();
		RenderTexture.active = renderTexture;
		texture2D.ReadPixels(new Rect(0f, 0f, pixelWidth, pixelHeight), 0, 0);
		texture2D.Apply();
		camera2.targetTexture = null;
		RenderTexture.active = null;
		renderTexture.Release();
		UnityEngine.Object.Destroy(renderTexture);
		_Texture = texture2D;
	}

	public void btnSaveCick()
	{
	}
}
