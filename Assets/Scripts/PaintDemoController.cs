using AdvancedMobilePaint;
using UnityEngine;
using UnityEngine.UI;

public class PaintDemoController : MonoBehaviour
{
	public AdvancedMobilePaint.AdvancedMobilePaint paintEngine;

	public Texture2D paintSurface;

	public Texture2D maskTexture;

	public Texture2D brushExample1;

	public Texture2D brushExample2;

	public Texture2D brushExample3;

	public Texture2D patternExample1;

	public Texture2D patternExample2;

	public Texture2D patternExample3;

	public Sprite patternSprite;

	public Sprite patternSprite2;

	private bool lockMode;

	public PaintUndoManager undoManager;

	public Texture2D unreadableTex2D;

	public GameObject rotatedTextureDisplay;

	public float rotationAngle = 32f;

	private void Start()
	{
		patternExample2 = PaintUtils.ConvertSpriteToTexture2D(patternSprite2);
		DoReset();
	}

	public void SetUpQuadPaint()
	{
		paintEngine.SetDrawingTexture(paintSurface);
		paintEngine.useLockArea = false;
		paintEngine.useMaskLayerOnly = false;
		paintEngine.useThreshold = false;
		paintEngine.drawEnabled = true;
	}

	public void SetUpBitmapBrushType1()
	{
		paintEngine.drawMode = DrawMode.CustomBrush;
		paintEngine.SetBitmapBrush(brushExample1, BrushProperties.Default, isAditiveBrush: false, brushCanDrawOnBlack: true, Color.blue, usesLockMasks: false, useBrushAlpha: false, null);
		paintEngine.drawEnabled = true;
	}

	public void SetUpBitmapBrushType2()
	{
		paintEngine.drawMode = DrawMode.CustomBrush;
		paintEngine.SetBitmapBrush(brushExample1, BrushProperties.Pattern, isAditiveBrush: false, brushCanDrawOnBlack: true, Color.blue, usesLockMasks: false, useBrushAlpha: false, PaintUtils.ConvertSpriteToTexture2D(patternSprite));
		paintEngine.drawEnabled = true;
	}

	public void ToogleLockMask()
	{
		lockMode = !lockMode;
		if (lockMode)
		{
			paintEngine.SetDrawingMask(maskTexture);
			paintEngine.useLockArea = true;
			paintEngine.useMaskLayerOnly = true;
			paintEngine.useThreshold = true;
		}
		else
		{
			paintEngine.useLockArea = false;
			paintEngine.useMaskLayerOnly = false;
			paintEngine.useThreshold = false;
		}
	}

	public void SetUpBitmapBrushType3()
	{
		paintEngine.drawMode = DrawMode.CustomBrush;
		paintEngine.SetBitmapBrush(brushExample1, BrushProperties.Pattern, isAditiveBrush: false, brushCanDrawOnBlack: true, Color.blue, usesLockMasks: false, useBrushAlpha: false, patternExample2);
		paintEngine.drawEnabled = true;
	}

	public void SetUppFlooodFillBrush()
	{
		paintEngine.SetFloodFIllBrush(Color.blue, usesLockMasks: true);
		paintEngine.useLockArea = false;
		paintEngine.useThreshold = false;
		paintEngine.drawEnabled = true;
		paintEngine.canDrawOnBlack = false;
	}

	public void DoUndo()
	{
		undoManager.UndoLastStep();
	}

	public void DoRedo()
	{
		undoManager.RedoLastStep();
	}

	public void DoReset()
	{
		undoManager.ClearSteps();
		SetUpQuadPaint();
	}

	public void SetUpVectorBrushType()
	{
		paintEngine.drawMode = DrawMode.Default;
		paintEngine.SetVectorBrush(VectorBrush.Rectangular, 32, 128, Color.red, null, isAditiveBrush: false, brushCanDrawOnBlack: false, usesLockMasks: false, useBrushAlpha: false);
		paintEngine.customBrushHeight = 32;
		paintEngine.customBrushWidth = 128;
		paintEngine.drawEnabled = true;
	}

	public void SetUpVectorBrushType2()
	{
		paintEngine.SetVectorBrush(VectorBrush.Rectangular, 32, 32, Color.gray, PaintUtils.ConvertSpriteToTexture2D(patternSprite), isAditiveBrush: false, brushCanDrawOnBlack: false, usesLockMasks: false, useBrushAlpha: false);
		paintEngine.customBrushHeight = 128;
		paintEngine.customBrushWidth = 32;
		paintEngine.drawEnabled = true;
	}

	public void ToogleMultitouch()
	{
		paintEngine.multitouchEnabled = !paintEngine.multitouchEnabled;
		paintEngine.connectBrushStokes = true;
	}

	public void DrawLine2()
	{
		paintEngine.SetLineBrush(10, 0, Color.red, null, null);
	}

	public void DrawLine3()
	{
		paintEngine.SetLineBrush(10, 0, Color.red, PaintUtils.ConvertSpriteToTexture2D(patternSprite), null);
	}

	public void ReadUnreadableTex()
	{
		paintSurface = PaintUtils.ReadUnreadableTexture(unreadableTex2D);
		paintEngine.SetDrawingTexture(paintSurface);
	}

	public void RotateTexture()
	{
		Texture2D texture = PaintUtils.RotateTexture(paintSurface, rotationAngle);
		rotatedTextureDisplay.GetComponent<RawImage>().texture = texture;
	}
}
