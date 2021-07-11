using AdvancedMobilePaint.Tools;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AdvancedMobilePaint
{
	public class AdvancedMobilePaint : MonoBehaviour
	{
		public bool drawEnabled;

		public PaintUndoManager undoController;

		public BrushProperties brushMode = BrushProperties.Default;

		public LayerMask paintLayerMask;

		public bool createCanvasMesh;

		public bool connectBrushStokes = true;

		public Color32 paintColor = new Color32(byte.MaxValue, 0, 0, byte.MaxValue);

		public int brushSize = 24;

		public bool useAdditiveColors = true;

		public float brushAlphaStrength = 0.5f;

		public DrawMode drawMode = DrawMode.CustomBrush;

		public bool useLockArea;

		public bool useMaskLayerOnly;

		public bool useThreshold;

		public byte paintThreshold = 128;

		[HideInInspector]
		public byte[] lockMaskPixels;

		public bool canDrawOnBlack = true;

		public string targetTexture = "_MainTex";

		public FilterMode filterMode;

		public Color32 clearColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

		public bool useMaskImage;

		public Texture2D maskTex;

		public Texture2D customBrush;

		public bool useCustomBrushAlpha = true;

		[HideInInspector]
		public byte[] customBrushBytes;

		public int customBrushWidth;

		public int customBrushHeight;

		public int customBrushWidthHalf;

		private int texWidthMinusCustomBrushWidth;

		public Texture2D customPattern;

		[HideInInspector]
		public byte[] patternBrushBytes;

		[HideInInspector]
		public int customPatternWidth;

		[HideInInspector]
		public int customPatternHeight;

		private byte[] undoPixels;

		public bool undoEnabled;

		private UStep drawUndoStep;

		[HideInInspector]
		public byte[] pixels;

		[HideInInspector]
		public byte[] maskPixels;

		private byte[] clearPixels;

		public Texture2D tex;

		[HideInInspector]
		public int texWidth;

		[HideInInspector]
		public int texHeight;

		private Touch touch;

		private Camera cam;

		private RaycastHit hit;

		private bool wentOutside;

		private bool usingClearingImage;

		private Vector2 pixelUV;

		private Vector2 pixelUVOld;

		private Vector2[] pixelUVs;

		private Vector2[] pixelUVOlds;

		[HideInInspector]
		public bool textureNeedsUpdate;

		public Texture2D pattenTexture;

		public Transform raySource;

		public bool useAlternativeRay;

		public bool multitouchEnabled;

		public bool isLinePaint;

		public int lineEdgeSize;

		private Vector2 pixelUV_older;

		private Vector2[] pixelUV_olders;

		public VectorBrush vectorBrushType;

		public bool isPatternLine;

		private int brushSizeTmp = 24;

		public bool useSmartFloodFill;

		private void Start()
		{
		}

		public void InitializeEverything()
		{
			pixelUVs = new Vector2[20];
			pixelUVOlds = new Vector2[20];
			pixelUV_olders = new Vector2[20];
			if (createCanvasMesh)
			{
				CreateCanvasQuad();
			}
			else
			{
				if (GetComponent<MeshCollider>() == null)
				{
					UnityEngine.Debug.LogError("MeshCollider is missing, won't be able to raycast to canvas object");
				}
				if (GetComponent<MeshFilter>() == null || GetComponent<MeshFilter>().sharedMesh == null)
				{
					UnityEngine.Debug.LogWarning("Mesh or MeshFilter is missing, won't be able to see the canvas object");
				}
			}
			if (useMaskImage)
			{
				if (maskTex == null)
				{
					UnityEngine.Debug.LogWarning("maskImage is not assigned. Setting 'useMaskImage' to false");
					useMaskImage = false;
				}
				else if (GetComponent<Renderer>().material.name.StartsWith("CanvasWithAlpha") || GetComponent<Renderer>().material.name.StartsWith("CanvasDefault"))
				{
					UnityEngine.Debug.LogWarning("CanvasWithAlpha and CanvasDefault materials do not support using MaskImage (layer). Disabling 'useMaskImage'");
					UnityEngine.Debug.LogWarning("CanvasWithAlpha and CanvasDefault materials do not support using MaskImage (layer). Disabling 'useMaskLayerOnly'");
					useMaskLayerOnly = false;
					useMaskImage = false;
					maskTex = null;
				}
				else
				{
					texWidth = maskTex.width;
					texHeight = maskTex.height;
					GetComponent<Renderer>().material.SetTexture("_MaskTex", maskTex);
				}
			}
			else
			{
				if (tex != null)
				{
					texWidth = tex.width;
					texHeight = tex.height;
				}
				else
				{
					texWidth = 0;
				}
				texHeight = 0;
			}
			if (!GetComponent<Renderer>().material.HasProperty(targetTexture))
			{
				UnityEngine.Debug.LogError("Fatal error: Current shader doesn't have a property: '" + targetTexture + "'");
			}
			if (GetComponent<Renderer>().material.GetTexture(targetTexture) == null)
			{
				tex = new Texture2D(texWidth, texHeight, TextureFormat.RGBA32, mipChain: false);
				GetComponent<Renderer>().material.SetTexture(targetTexture, tex);
				pixels = new byte[texWidth * texHeight * 4];
			}
			else
			{
				usingClearingImage = true;
				texWidth = GetComponent<Renderer>().material.GetTexture(targetTexture).width;
				texHeight = GetComponent<Renderer>().material.GetTexture(targetTexture).height;
				pixels = new byte[texWidth * texHeight * 4];
				tex = new Texture2D(texWidth, texHeight, TextureFormat.RGBA32, mipChain: false);
				ReadClearingImage();
				GetComponent<Renderer>().material.SetTexture(targetTexture, tex);
			}
			ClearImage();
			tex.filterMode = filterMode;
			tex.wrapMode = TextureWrapMode.Clamp;
			if (useMaskImage)
			{
				SetDrawingMask(maskTex);
			}
			if (undoEnabled)
			{
				undoPixels = new byte[texWidth * texHeight * 4];
				Array.Copy(pixels, undoPixels, pixels.Length);
			}
			if (useLockArea)
			{
				lockMaskPixels = new byte[texWidth * texHeight * 4];
			}
			ReadCurrentCustomBrush();
		}

		private void Update()
		{
			if (!drawEnabled)
			{
				return;
			}
			if (!multitouchEnabled)
			{
				if (isLinePaint)
				{
					MousePaint2();
				}
				else
				{
					MousePaint();
				}
			}
			else
			{
				TouchPaint();
			}
			UpdateTexture();
		}

		public void ImmediateDraw(Transform raySource)
		{
			Ray ray = new Ray(raySource.position, new Vector3(0f, 0f, 1f));
			if (!Physics.Raycast(Camera.main.ScreenPointToRay(Camera.main.WorldToScreenPoint(raySource.position)), out hit, float.PositiveInfinity, paintLayerMask))
			{
				return;
			}
			if (hit.collider != base.gameObject.GetComponent<Collider>())
			{
				UnityEngine.Debug.Log("HIT SOME OTHER COLLIDER");
				return;
			}
			Vector2 textureCoord = hit.textureCoord;
			textureCoord.x *= texWidth;
			textureCoord.y *= texHeight;
			switch (drawMode)
			{
			case DrawMode.Default:
				if (brushMode == BrushProperties.Default)
				{
					VectorBrushesTools.DrawCircle((int)textureCoord.x, (int)textureCoord.y, this);
				}
				else if (brushMode == BrushProperties.Pattern)
				{
					VectorBrushesTools.DrawPatternCircle((int)textureCoord.x, (int)textureCoord.y, this);
				}
				break;
			case DrawMode.Pattern:
				VectorBrushesTools.DrawPatternCircle((int)textureCoord.x, (int)textureCoord.y, this);
				break;
			case DrawMode.CustomBrush:
				BitmapBrushesTools.DrawCustomBrush2((int)textureCoord.x, (int)textureCoord.y, this);
				break;
			case DrawMode.FloodFill:
				if (useSmartFloodFill)
				{
					FloodFillTools.FloodFillAutoMaskWithThreshold((int)textureCoord.x, (int)textureCoord.y, this);
				}
				else if (useThreshold)
				{
					if (useMaskLayerOnly)
					{
						FloodFillTools.FloodFillMaskOnlyWithThreshold((int)textureCoord.x, (int)textureCoord.y, this);
					}
					else
					{
						FloodFillTools.FloodFillWithTreshold((int)textureCoord.x, (int)textureCoord.y, this);
					}
				}
				else if (useMaskLayerOnly)
				{
					FloodFillTools.FloodFillMaskOnly((int)textureCoord.x, (int)textureCoord.y, this);
				}
				else
				{
					FloodFillTools.FloodFill((int)textureCoord.x, (int)textureCoord.y, this);
				}
				break;
			default:
				UnityEngine.Debug.LogWarning("AMP: Unknown drawing mode:" + drawMode);
				break;
			}
			textureNeedsUpdate = true;
			UpdateTexture();
		}

		public bool IsRaycastInsideMask(Vector3 screenPosition)
		{
			if (!Physics.Raycast(Camera.main.ScreenPointToRay(screenPosition), out hit, float.PositiveInfinity, paintLayerMask))
			{
				return false;
			}
			if (hit.collider != base.gameObject.GetComponent<Collider>())
			{
				return false;
			}
			Vector2 textureCoord = hit.textureCoord;
			textureCoord.x *= texWidth;
			textureCoord.y *= texHeight;
			int num = (int)textureCoord.x;
			int num2 = (int)textureCoord.y;
			int num3 = (texWidth * num2 + num) * 4;
			if (num3 < 0 || num3 >= pixels.Length)
			{
				return false;
			}
			if (lockMaskPixels[num3] == 1)
			{
				return true;
			}
			return false;
		}

		private void MousePaint()
		{
			if (Input.GetMouseButtonDown(0) && UnityEngine.Input.touchCount <= 1 && useLockArea)
			{
				if (!Physics.Raycast((!useAlternativeRay) ? Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition) : Camera.main.ScreenPointToRay(Camera.main.WorldToScreenPoint(raySource.position)), out hit, float.PositiveInfinity, paintLayerMask) || hit.collider != base.gameObject.GetComponent<Collider>())
				{
					return;
				}
				Input.multiTouchEnabled = false;
				Vector2 textureCoord = hit.textureCoord;
				int x = (int)(textureCoord.x * (float)texWidth);
				Vector2 textureCoord2 = hit.textureCoord;
				CreateAreaLockMask(x, (int)(textureCoord2.y * (float)texHeight));
				if (undoEnabled)
				{
					drawUndoStep = new UStep();
					switch (drawMode)
					{
					case DrawMode.Default:
						drawUndoStep.type = 1;
						break;
					case DrawMode.CustomBrush:
						drawUndoStep.type = 0;
						break;
					case DrawMode.FloodFill:
						drawUndoStep.type = 2;
						break;
					case DrawMode.Pattern:
						drawUndoStep.type = 1;
						break;
					}
					drawUndoStep.SetStepPropertiesFromEngine(this);
					drawUndoStep.drawCoordinates = new List<Vector2>();
				}
			}
			if (Input.GetMouseButton(0) && UnityEngine.Input.touchCount <= 1)
			{
				if (!Physics.Raycast((!useAlternativeRay) ? Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition) : Camera.main.ScreenPointToRay(Camera.main.WorldToScreenPoint(raySource.position)), out hit, float.PositiveInfinity, paintLayerMask))
				{
					wentOutside = true;
					return;
				}
				if (hit.collider != base.gameObject.GetComponent<Collider>())
				{
					wentOutside = true;
					return;
				}
				pixelUVOld = pixelUV;
				pixelUV = hit.textureCoord;
				pixelUV.x *= texWidth;
				pixelUV.y *= texHeight;
				if (wentOutside)
				{
					pixelUVOld = pixelUV;
					wentOutside = false;
				}
				switch (drawMode)
				{
				case DrawMode.Default:
					if (!useAdditiveColors && pixelUVOld == pixelUV)
					{
						break;
					}
					if (brushMode == BrushProperties.Default)
					{
						if (vectorBrushType == VectorBrush.Circle)
						{
							VectorBrushesTools.DrawCircle((int)pixelUV.x, (int)pixelUV.y, this);
						}
						else
						{
							VectorBrushesTools.DrawRectangle((int)pixelUV.x, (int)pixelUV.y, this);
						}
					}
					else if (brushMode == BrushProperties.Pattern)
					{
						VectorBrushesTools.DrawPatternCircle((int)pixelUV.x, (int)pixelUV.y, this);
					}
					textureNeedsUpdate = true;
					break;
				case DrawMode.Pattern:
					if (useAdditiveColors || !(pixelUVOld == pixelUV))
					{
						if (vectorBrushType == VectorBrush.Circle)
						{
							VectorBrushesTools.DrawPatternCircle((int)pixelUV.x, (int)pixelUV.y, this);
						}
						else
						{
							VectorBrushesTools.DrawPatternRectangle((int)pixelUV.x, (int)pixelUV.y, this);
						}
						textureNeedsUpdate = true;
					}
					break;
				case DrawMode.CustomBrush:
					if (useAdditiveColors || !(pixelUVOld == pixelUV))
					{
						BitmapBrushesTools.DrawCustomBrush2((int)pixelUV.x, (int)pixelUV.y, this);
						textureNeedsUpdate = true;
					}
					break;
				case DrawMode.FloodFill:
					if (useSmartFloodFill)
					{
						FloodFillTools.FloodFillAutoMaskWithThreshold((int)pixelUV.x, (int)pixelUV.y, this);
					}
					else if (useThreshold)
					{
						if (useMaskLayerOnly)
						{
							FloodFillTools.FloodFillMaskOnlyWithThreshold((int)pixelUV.x, (int)pixelUV.y, this);
						}
						else
						{
							FloodFillTools.FloodFillWithTreshold((int)pixelUV.x, (int)pixelUV.y, this);
						}
					}
					else if (useMaskLayerOnly)
					{
						FloodFillTools.FloodFillMaskOnly((int)pixelUV.x, (int)pixelUV.y, this);
					}
					else
					{
						FloodFillTools.FloodFill((int)pixelUV.x, (int)pixelUV.y, this);
					}
					textureNeedsUpdate = true;
					break;
				default:
					UnityEngine.Debug.LogWarning("AMP: Unknown drawing mode:" + drawMode);
					break;
				}
				if (drawUndoStep == null && undoEnabled)
				{
					drawUndoStep = new UStep();
					switch (drawMode)
					{
					case DrawMode.Default:
						drawUndoStep.type = 1;
						break;
					case DrawMode.CustomBrush:
						drawUndoStep.type = 0;
						break;
					case DrawMode.FloodFill:
						drawUndoStep.type = 2;
						break;
					case DrawMode.Pattern:
						drawUndoStep.type = 1;
						break;
					}
					drawUndoStep.SetStepPropertiesFromEngine(this);
					drawUndoStep.drawCoordinates = new List<Vector2>();
				}
				if (undoEnabled)
				{
					Vector2 item = new Vector2(pixelUV.x, pixelUV.y);
					drawUndoStep.drawCoordinates.Add(item);
				}
			}
			if (Input.GetMouseButtonDown(0) && UnityEngine.Input.touchCount <= 1)
			{
				if (!Physics.Raycast((!useAlternativeRay) ? Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition) : Camera.main.ScreenPointToRay(Camera.main.WorldToScreenPoint(raySource.position)), out hit, float.PositiveInfinity, paintLayerMask) || hit.collider != base.gameObject.GetComponent<Collider>())
				{
					return;
				}
				pixelUVOld = pixelUV;
			}
			if (connectBrushStokes && Vector2.Distance(pixelUV, pixelUVOld) > (float)(brushSize / 2) && UnityEngine.Input.touchCount <= 1)
			{
				switch (drawMode)
				{
				case DrawMode.Default:
					if (UnityEngine.Input.touchCount == 1 && UnityEngine.Input.GetTouch(0).phase != TouchPhase.Stationary)
					{
						if (vectorBrushType == VectorBrush.Circle)
						{
							VectorBrushesTools.DrawLine(pixelUVOld, pixelUV, this);
						}
						else
						{
							VectorBrushesTools.DrawLineWithVectorBrush(pixelUVOld, pixelUV, this);
						}
					}
					break;
				case DrawMode.CustomBrush:
					if (UnityEngine.Input.touchCount == 1 && UnityEngine.Input.GetTouch(0).phase != TouchPhase.Stationary)
					{
						BitmapBrushesTools.DrawLineWithBrush(pixelUVOld, pixelUV, this);
					}
					break;
				case DrawMode.Pattern:
					if (UnityEngine.Input.touchCount == 1 && UnityEngine.Input.GetTouch(0).phase != TouchPhase.Stationary)
					{
						VectorBrushesTools.DrawLineWithPattern(pixelUVOld, pixelUV, this);
					}
					break;
				}
				pixelUVOld = pixelUV;
				textureNeedsUpdate = true;
			}
			if (Input.GetMouseButtonUp(0) && UnityEngine.Input.touchCount <= 1 && drawEnabled && drawUndoStep != null && undoEnabled)
			{
				UStep uStep = new UStep();
				uStep = drawUndoStep;
				undoController.AddStep(uStep);
				drawUndoStep = null;
			}
		}

		private void MousePaint2()
		{
			if (Input.GetMouseButtonDown(0) && UnityEngine.Input.touchCount <= 1)
			{
				if (!Physics.Raycast((!useAlternativeRay) ? Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition) : Camera.main.ScreenPointToRay(Camera.main.WorldToScreenPoint(raySource.position)), out hit, float.PositiveInfinity, paintLayerMask) || hit.collider != base.gameObject.GetComponent<Collider>())
				{
					return;
				}
				if (hit.collider != base.gameObject.GetComponent<Collider>())
				{
					wentOutside = true;
					return;
				}
				pixelUV_older = pixelUV;
				pixelUVOld = pixelUV;
				pixelUV = hit.textureCoord;
				pixelUV.x *= texWidth;
				pixelUV.y *= texHeight;
				if (wentOutside)
				{
					pixelUVOld = pixelUV;
					pixelUV_older = pixelUV;
					wentOutside = false;
				}
				brushSize += lineEdgeSize;
				if (undoEnabled)
				{
					drawUndoStep = new UStep();
					drawUndoStep.type = 4;
					drawUndoStep.SetStepPropertiesFromEngine(this);
					drawUndoStep.drawCoordinates = new List<Vector2>();
				}
				if (wentOutside)
				{
					return;
				}
				UnityEngine.Debug.Log("LINE ON MOUSE DOWN");
				if (lineEdgeSize > 0)
				{
					if (isPatternLine)
					{
						BitmapBrushesTools.DrawPatternCircle((int)pixelUV.x, (int)pixelUV.y, customBrushBytes, brushSize + lineEdgeSize, this);
					}
					else
					{
						VectorBrushesTools.DrawCircle((int)pixelUV.x, (int)pixelUV.y, this);
					}
				}
				if (isPatternLine)
				{
					VectorBrushesTools.DrawPatternCircle((int)pixelUV.x, (int)pixelUV.y, this);
				}
				else
				{
					VectorBrushesTools.DrawCircle((int)pixelUV.x, (int)pixelUV.y, this);
				}
				textureNeedsUpdate = true;
				pixelUV_older = pixelUV;
				pixelUVOld = pixelUV;
			}
			else if (Input.GetMouseButton(0) && UnityEngine.Input.touchCount <= 1 && !Input.GetMouseButtonDown(0))
			{
				if (!Physics.Raycast((!useAlternativeRay) ? Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition) : Camera.main.ScreenPointToRay(Camera.main.WorldToScreenPoint(raySource.position)), out hit, float.PositiveInfinity, paintLayerMask))
				{
					wentOutside = true;
					return;
				}
				if (hit.collider != base.gameObject.GetComponent<Collider>())
				{
					wentOutside = true;
					return;
				}
				pixelUV = hit.textureCoord;
				pixelUV.x *= texWidth;
				pixelUV.y *= texHeight;
				if (wentOutside)
				{
					pixelUVOld = pixelUV;
					pixelUV_older = pixelUV;
					wentOutside = false;
				}
				if (!Input.GetMouseButton(0) || !(Vector2.Distance(pixelUV, pixelUVOld) >= (float)brushSize) || UnityEngine.Input.touchCount > 1)
				{
					return;
				}
				UnityEngine.Debug.Log("LINE ON MOUSE HOLD");
				if (lineEdgeSize > 0)
				{
					if (isPatternLine)
					{
						BitmapBrushesTools.DrawPatternCircle((int)pixelUV.x, (int)pixelUV.y, customBrushBytes, brushSize + lineEdgeSize, this);
						BitmapBrushesTools.DrawLineBrush(pixelUVOld, pixelUV, brushSize * 2 + lineEdgeSize * 2, isPattern: true, customBrushBytes, this);
						BitmapBrushesTools.DrawLineBrush(pixelUV_older, pixelUVOld, brushSize * 2 + lineEdgeSize * 2, isPattern: true, customBrushBytes, this);
						BitmapBrushesTools.DrawLineBrush(pixelUV_older, pixelUVOld, brushSize * 2, isPattern: true, patternBrushBytes, this);
						VectorBrushesTools.DrawPatternCircle((int)pixelUV_older.x, (int)pixelUV_older.y, this);
					}
					else
					{
						VectorBrushesTools.DrawCircle((int)pixelUV.x, (int)pixelUV.y, this);
						BitmapBrushesTools.DrawLineBrush(pixelUVOld, pixelUV, brushSize * 2 + lineEdgeSize * 2, isPattern: false, customBrushBytes, this);
						BitmapBrushesTools.DrawLineBrush(pixelUV_older, pixelUVOld, brushSize * 2 + lineEdgeSize * 2, isPattern: false, customBrushBytes, this);
						BitmapBrushesTools.DrawLineBrush(pixelUV_older, pixelUVOld, brushSize * 2, isPattern: false, patternBrushBytes, this);
						VectorBrushesTools.DrawCircle((int)pixelUV_older.x, (int)pixelUV_older.y, this);
					}
				}
				if (isPatternLine)
				{
					BitmapBrushesTools.DrawLineBrush(pixelUVOld, pixelUV, brushSize * 2, isPattern: true, patternBrushBytes, this);
					VectorBrushesTools.DrawPatternCircle((int)pixelUV.x, (int)pixelUV.y, this);
				}
				else
				{
					BitmapBrushesTools.DrawLineBrush(pixelUVOld, pixelUV, brushSize * 2, isPattern: false, patternBrushBytes, this);
					VectorBrushesTools.DrawCircle((int)pixelUV.x, (int)pixelUV.y, this);
				}
				pixelUV_older = pixelUVOld;
				pixelUVOld = pixelUV;
				textureNeedsUpdate = true;
				if (drawUndoStep == null && undoEnabled)
				{
					drawUndoStep = new UStep();
					drawUndoStep.type = 4;
					drawUndoStep.SetStepPropertiesFromEngine(this);
					drawUndoStep.drawCoordinates = new List<Vector2>();
				}
				if (undoEnabled)
				{
					Vector2 item = new Vector2(pixelUV.x, pixelUV.y);
					drawUndoStep.drawCoordinates.Add(item);
				}
			}
			else
			{
				if (!Input.GetMouseButtonUp(0) || UnityEngine.Input.touchCount > 1 || !drawEnabled)
				{
					return;
				}
				if (!Physics.Raycast((!useAlternativeRay) ? Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition) : Camera.main.ScreenPointToRay(Camera.main.WorldToScreenPoint(raySource.position)), out hit, float.PositiveInfinity, paintLayerMask))
				{
					wentOutside = true;
					return;
				}
				if (hit.collider != base.gameObject.GetComponent<Collider>())
				{
					wentOutside = true;
					return;
				}
				pixelUV_older = pixelUV;
				pixelUVOld = pixelUV;
				pixelUV = hit.textureCoord;
				pixelUV.x *= texWidth;
				pixelUV.y *= texHeight;
				if (wentOutside)
				{
					pixelUVOld = pixelUV;
					pixelUV_older = pixelUV;
					wentOutside = false;
				}
				else
				{
					UnityEngine.Debug.Log("LINE ON MOUSE UP");
					if (lineEdgeSize > 0)
					{
						if (isPatternLine)
						{
							BitmapBrushesTools.DrawPatternCircle((int)pixelUV.x, (int)pixelUV.y, customBrushBytes, brushSize + lineEdgeSize, this);
							BitmapBrushesTools.DrawLineBrush(pixelUV_older, pixelUVOld, brushSize * 2, isPattern: true, patternBrushBytes, this);
							VectorBrushesTools.DrawPatternCircle((int)pixelUV_older.x, (int)pixelUV_older.y, this);
						}
						else
						{
							VectorBrushesTools.DrawCircle((int)pixelUV.x, (int)pixelUV.y, this);
							BitmapBrushesTools.DrawLineBrush(pixelUV_older, pixelUVOld, brushSize * 2, isPattern: false, patternBrushBytes, this);
							VectorBrushesTools.DrawPatternCircle((int)pixelUV_older.x, (int)pixelUV_older.y, this);
						}
					}
					if (isPatternLine)
					{
						VectorBrushesTools.DrawPatternCircle((int)pixelUV.x, (int)pixelUV.y, this);
					}
					else
					{
						VectorBrushesTools.DrawCircle((int)pixelUV.x, (int)pixelUV.y, this);
					}
					textureNeedsUpdate = true;
					if (undoEnabled)
					{
						Vector2 item2 = new Vector2(pixelUV.x, pixelUV.y);
						drawUndoStep.drawCoordinates.Add(item2);
						if (drawEnabled && drawUndoStep != null)
						{
							UStep uStep = new UStep();
							uStep = drawUndoStep;
							undoController.AddStep(uStep);
							drawUndoStep = null;
						}
					}
				}
				brushSize -= lineEdgeSize;
			}
		}

		private void TouchPaint()
		{
			for (int i = 0; i < UnityEngine.Input.touchCount; i++)
			{
				touch = UnityEngine.Input.GetTouch(i);
			}
			for (int i = 0; i < UnityEngine.Input.touchCount; i++)
			{
				touch = UnityEngine.Input.GetTouch(i);
				if (touch.phase == TouchPhase.Began)
				{
					if (!Physics.Raycast(Camera.main.ScreenPointToRay(touch.position), out hit, float.PositiveInfinity, paintLayerMask))
					{
						wentOutside = true;
						return;
					}
					if (hit.collider != base.gameObject.GetComponent<Collider>())
					{
						wentOutside = true;
						return;
					}
					pixelUVs[touch.fingerId] = hit.textureCoord;
					pixelUVs[touch.fingerId].x *= texWidth;
					pixelUVs[touch.fingerId].y *= texHeight;
					pixelUVOlds[touch.fingerId] = pixelUVs[touch.fingerId];
					pixelUV_olders[touch.fingerId] = pixelUVs[touch.fingerId];
					if (wentOutside)
					{
						pixelUVOlds[touch.fingerId] = pixelUVs[touch.fingerId];
						pixelUV_olders[touch.fingerId] = pixelUVs[touch.fingerId];
						wentOutside = false;
					}
					if (useLockArea)
					{
						CreateAreaLockMask((int)pixelUVs[touch.fingerId].x, (int)pixelUVs[touch.fingerId].y);
					}
					if (undoEnabled && drawUndoStep == null)
					{
						drawUndoStep = new UStep();
						switch (drawMode)
						{
						case DrawMode.Default:
							drawUndoStep.type = 1;
							break;
						case DrawMode.CustomBrush:
							drawUndoStep.type = 0;
							break;
						case DrawMode.FloodFill:
							drawUndoStep.type = 2;
							break;
						case DrawMode.Pattern:
							drawUndoStep.type = 1;
							break;
						}
						if (isLinePaint)
						{
							drawUndoStep.type = 4;
							brushSizeTmp = brushSize;
							brushSize += lineEdgeSize;
						}
						drawUndoStep.SetStepPropertiesFromEngine(this);
						drawUndoStep.drawCoordinates = new List<Vector2>();
						if (isLinePaint)
						{
							drawUndoStep.touchCoordinates = new List<TouchCoordinates>();
						}
					}
				}
				if ((touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Began) && Physics.Raycast(Camera.main.ScreenPointToRay(touch.position), out hit, float.PositiveInfinity, paintLayerMask) && hit.collider == base.gameObject.GetComponent<Collider>())
				{
					pixelUVOlds[touch.fingerId] = pixelUVs[touch.fingerId];
					pixelUVs[touch.fingerId] = hit.textureCoord;
					pixelUVs[touch.fingerId].x *= texWidth;
					pixelUVs[touch.fingerId].y *= texHeight;
					if (isLinePaint)
					{
						if (lineEdgeSize > 0)
						{
							if (isPatternLine)
							{
								BitmapBrushesTools.DrawPatternCircle((int)pixelUVs[touch.fingerId].x, (int)pixelUVs[touch.fingerId].y, customBrushBytes, brushSize + lineEdgeSize, this);
								BitmapBrushesTools.DrawLineBrush(pixelUVOlds[touch.fingerId], pixelUVs[touch.fingerId], brushSize * 2 + lineEdgeSize * 2, isPattern: true, customBrushBytes, this);
								BitmapBrushesTools.DrawLineBrush(pixelUV_olders[touch.fingerId], pixelUVOlds[touch.fingerId], brushSize * 2 + lineEdgeSize * 2, isPattern: true, customBrushBytes, this);
								BitmapBrushesTools.DrawLineBrush(pixelUV_olders[touch.fingerId], pixelUVOlds[touch.fingerId], brushSize * 2, isPattern: true, patternBrushBytes, this);
								VectorBrushesTools.DrawPatternCircle((int)pixelUV_olders[touch.fingerId].x, (int)pixelUV_olders[touch.fingerId].y, this);
							}
							else
							{
								VectorBrushesTools.DrawCircle((int)pixelUVs[touch.fingerId].x, (int)pixelUVs[touch.fingerId].y, this);
								BitmapBrushesTools.DrawLineBrush(pixelUVOlds[touch.fingerId], pixelUVs[touch.fingerId], brushSize * 2 + lineEdgeSize * 2, isPattern: false, customBrushBytes, this);
								BitmapBrushesTools.DrawLineBrush(pixelUV_olders[touch.fingerId], pixelUVOlds[touch.fingerId], brushSize * 2 + lineEdgeSize * 2, isPattern: false, customBrushBytes, this);
								BitmapBrushesTools.DrawLineBrush(pixelUV_olders[touch.fingerId], pixelUVOlds[touch.fingerId], brushSize * 2, isPattern: false, patternBrushBytes, this);
								VectorBrushesTools.DrawCircle((int)pixelUV_olders[touch.fingerId].x, (int)pixelUV_olders[touch.fingerId].y, this);
							}
						}
						if (isPatternLine)
						{
							BitmapBrushesTools.DrawLineBrush(pixelUVOlds[touch.fingerId], pixelUVs[touch.fingerId], brushSize * 2, isPattern: true, patternBrushBytes, this);
							VectorBrushesTools.DrawPatternCircle((int)pixelUVs[touch.fingerId].x, (int)pixelUVs[touch.fingerId].y, this);
						}
						else
						{
							BitmapBrushesTools.DrawLineBrush(pixelUVOlds[touch.fingerId], pixelUVs[touch.fingerId], brushSize * 2, isPattern: false, patternBrushBytes, this);
							VectorBrushesTools.DrawCircle((int)pixelUVs[touch.fingerId].x, (int)pixelUVs[touch.fingerId].y, this);
						}
						textureNeedsUpdate = true;
					}
					else
					{
						switch (drawMode)
						{
						case DrawMode.Default:
							if (brushMode == BrushProperties.Default)
							{
								if (vectorBrushType == VectorBrush.Circle)
								{
									VectorBrushesTools.DrawCircle((int)pixelUVs[touch.fingerId].x, (int)pixelUVs[touch.fingerId].y, this);
								}
								else
								{
									VectorBrushesTools.DrawRectangle((int)pixelUVs[touch.fingerId].x, (int)pixelUVs[touch.fingerId].y, this);
								}
							}
							else if (brushMode == BrushProperties.Pattern)
							{
								VectorBrushesTools.DrawPatternCircle((int)pixelUVs[touch.fingerId].x, (int)pixelUVs[touch.fingerId].y, this);
							}
							textureNeedsUpdate = true;
							break;
						case DrawMode.CustomBrush:
							BitmapBrushesTools.DrawCustomBrush2((int)pixelUVs[touch.fingerId].x, (int)pixelUVs[touch.fingerId].y, this);
							textureNeedsUpdate = true;
							break;
						case DrawMode.Pattern:
							VectorBrushesTools.DrawPatternCircle((int)pixelUVs[touch.fingerId].x, (int)pixelUVs[touch.fingerId].y, this);
							textureNeedsUpdate = true;
							break;
						case DrawMode.FloodFill:
							if (useSmartFloodFill)
							{
								FloodFillTools.FloodFillAutoMaskWithThreshold((int)pixelUVs[touch.fingerId].x, (int)pixelUVs[touch.fingerId].y, this);
							}
							else if (useThreshold)
							{
								if (useMaskLayerOnly)
								{
									FloodFillTools.FloodFillMaskOnlyWithThreshold((int)pixelUVs[touch.fingerId].x, (int)pixelUVs[touch.fingerId].y, this);
								}
								else
								{
									FloodFillTools.FloodFillWithTreshold((int)pixelUVs[touch.fingerId].x, (int)pixelUVs[touch.fingerId].y, this);
								}
							}
							else if (useMaskLayerOnly)
							{
								FloodFillTools.FloodFillMaskOnly((int)pixelUVs[touch.fingerId].x, (int)pixelUVs[touch.fingerId].y, this);
							}
							else
							{
								FloodFillTools.FloodFill((int)pixelUVs[touch.fingerId].x, (int)pixelUVs[touch.fingerId].y, this);
							}
							textureNeedsUpdate = true;
							break;
						}
					}
					if (drawUndoStep == null && undoEnabled)
					{
						drawUndoStep = new UStep();
						switch (drawMode)
						{
						case DrawMode.Default:
							drawUndoStep.type = 1;
							break;
						case DrawMode.CustomBrush:
							drawUndoStep.type = 0;
							break;
						case DrawMode.FloodFill:
							drawUndoStep.type = 2;
							break;
						case DrawMode.Pattern:
							drawUndoStep.type = 1;
							break;
						}
						drawUndoStep.SetStepPropertiesFromEngine(this);
						drawUndoStep.drawCoordinates = new List<Vector2>();
						if (isLinePaint)
						{
							drawUndoStep.touchCoordinates = new List<TouchCoordinates>();
						}
					}
					if (undoEnabled)
					{
						Vector2 item = new Vector2(pixelUVs[touch.fingerId].x, pixelUVs[touch.fingerId].y);
						drawUndoStep.drawCoordinates.Add(item);
						if (isLinePaint)
						{
							drawUndoStep.AddTouchCoordinate(touch.fingerId);
						}
					}
					pixelUV_olders[touch.fingerId] = pixelUVOlds[touch.fingerId];
				}
				if (touch.phase == TouchPhase.Began)
				{
					pixelUVOlds[touch.fingerId] = pixelUVs[touch.fingerId];
					pixelUV_olders[touch.fingerId] = pixelUVOlds[touch.fingerId];
				}
				if (Vector2.Distance(pixelUVs[touch.fingerId], pixelUVOlds[touch.fingerId]) > (float)brushSize)
				{
					if (isLinePaint)
					{
						if (lineEdgeSize > 0)
						{
							if (isPatternLine)
							{
								BitmapBrushesTools.DrawPatternCircle((int)pixelUVs[touch.fingerId].x, (int)pixelUVs[touch.fingerId].y, customBrushBytes, brushSize + lineEdgeSize, this);
								BitmapBrushesTools.DrawLineBrush(pixelUVOlds[touch.fingerId], pixelUVs[touch.fingerId], brushSize * 2 + lineEdgeSize * 2, isPattern: true, customBrushBytes, this);
								BitmapBrushesTools.DrawLineBrush(pixelUV_olders[touch.fingerId], pixelUVOlds[touch.fingerId], brushSize * 2 + lineEdgeSize * 2, isPattern: true, customBrushBytes, this);
								BitmapBrushesTools.DrawLineBrush(pixelUV_olders[touch.fingerId], pixelUVOlds[touch.fingerId], brushSize * 2, isPattern: true, patternBrushBytes, this);
								VectorBrushesTools.DrawPatternCircle((int)pixelUV_olders[touch.fingerId].x, (int)pixelUV_olders[touch.fingerId].y, this);
							}
							else
							{
								VectorBrushesTools.DrawCircle((int)pixelUVs[touch.fingerId].x, (int)pixelUVs[touch.fingerId].y, this);
								BitmapBrushesTools.DrawLineBrush(pixelUVOlds[touch.fingerId], pixelUVs[touch.fingerId], brushSize * 2 + lineEdgeSize * 2, isPattern: false, customBrushBytes, this);
								BitmapBrushesTools.DrawLineBrush(pixelUV_olders[touch.fingerId], pixelUVOlds[touch.fingerId], brushSize * 2 + lineEdgeSize * 2, isPattern: false, customBrushBytes, this);
								BitmapBrushesTools.DrawLineBrush(pixelUV_olders[touch.fingerId], pixelUVOlds[touch.fingerId], brushSize * 2, isPattern: false, patternBrushBytes, this);
								VectorBrushesTools.DrawCircle((int)pixelUV_olders[touch.fingerId].x, (int)pixelUV_olders[touch.fingerId].y, this);
							}
						}
						if (isPatternLine)
						{
							BitmapBrushesTools.DrawLineBrush(pixelUVOlds[touch.fingerId], pixelUVs[touch.fingerId], brushSize * 2, isPattern: true, patternBrushBytes, this);
							VectorBrushesTools.DrawPatternCircle((int)pixelUVs[touch.fingerId].x, (int)pixelUVs[touch.fingerId].y, this);
						}
						else
						{
							BitmapBrushesTools.DrawLineBrush(pixelUVOlds[touch.fingerId], pixelUVs[touch.fingerId], brushSize * 2, isPattern: false, patternBrushBytes, this);
							VectorBrushesTools.DrawCircle((int)pixelUVs[touch.fingerId].x, (int)pixelUVs[touch.fingerId].y, this);
						}
						pixelUV_olders[touch.fingerId] = pixelUVOlds[touch.fingerId];
						pixelUVOlds[touch.fingerId] = pixelUVs[touch.fingerId];
						textureNeedsUpdate = true;
					}
					else
					{
						switch (drawMode)
						{
						case DrawMode.Default:
							if (brushMode == BrushProperties.Default)
							{
								if (vectorBrushType == VectorBrush.Circle)
								{
									VectorBrushesTools.DrawCircle((int)pixelUVs[touch.fingerId].x, (int)pixelUVs[touch.fingerId].y, this);
								}
								else
								{
									VectorBrushesTools.DrawRectangle((int)pixelUVs[touch.fingerId].x, (int)pixelUVs[touch.fingerId].y, this);
								}
							}
							else if (brushMode == BrushProperties.Pattern)
							{
								VectorBrushesTools.DrawPatternCircle((int)pixelUVs[touch.fingerId].x, (int)pixelUVs[touch.fingerId].y, this);
							}
							textureNeedsUpdate = true;
							break;
						case DrawMode.CustomBrush:
							BitmapBrushesTools.DrawCustomBrush2((int)pixelUVs[touch.fingerId].x, (int)pixelUVs[touch.fingerId].y, this);
							textureNeedsUpdate = true;
							break;
						case DrawMode.Pattern:
							VectorBrushesTools.DrawPatternCircle((int)pixelUVs[touch.fingerId].x, (int)pixelUVs[touch.fingerId].y, this);
							textureNeedsUpdate = true;
							break;
						case DrawMode.FloodFill:
							if (useThreshold)
							{
								if (useMaskLayerOnly)
								{
									FloodFillTools.FloodFillMaskOnlyWithThreshold((int)pixelUVs[touch.fingerId].x, (int)pixelUVs[touch.fingerId].y, this);
								}
								else
								{
									FloodFillTools.FloodFillWithTreshold((int)pixelUVs[touch.fingerId].x, (int)pixelUVs[touch.fingerId].y, this);
								}
							}
							else if (useMaskLayerOnly)
							{
								FloodFillTools.FloodFillMaskOnly((int)pixelUVs[touch.fingerId].x, (int)pixelUVs[touch.fingerId].y, this);
							}
							else
							{
								FloodFillTools.FloodFill((int)pixelUVs[touch.fingerId].x, (int)pixelUVs[touch.fingerId].y, this);
							}
							textureNeedsUpdate = true;
							break;
						}
					}
					pixelUVOlds[touch.fingerId] = pixelUVs[touch.fingerId];
				}
				if (touch.phase == TouchPhase.Ended && undoEnabled && drawUndoStep != null)
				{
					UStep uStep = new UStep();
					uStep = drawUndoStep;
					undoController.AddStep(uStep);
					drawUndoStep = null;
				}
			}
			if (UnityEngine.Input.touchCount == 0)
			{
				if (undoEnabled && drawUndoStep != null)
				{
					UStep uStep2 = new UStep();
					uStep2 = drawUndoStep;
					undoController.AddStep(uStep2);
					drawUndoStep = null;
				}
				if (isLinePaint)
				{
					brushSize = brushSizeTmp;
				}
			}
		}

		public void UpdateTexture()
		{
			if (textureNeedsUpdate)
			{
				textureNeedsUpdate = false;
				tex.LoadRawTextureData(pixels);
				tex.Apply(updateMipmaps: false);
			}
		}

		public void CopyUndoPixels(byte[] destiniation)
		{
			Array.Copy(undoPixels, destiniation, undoPixels.Length);
		}

		public void CreateAreaLockMask(int x, int y)
		{
			if (useThreshold)
			{
				if (useMaskLayerOnly)
				{
					FloodFillTools.LockAreaFillWithThresholdMaskOnly(x, y, this);
				}
				else
				{
					FloodFillTools.LockMaskFillWithThreshold(x, y, this);
				}
			}
			else if (useMaskLayerOnly)
			{
				FloodFillTools.LockAreaFillMaskOnly(x, y, this);
			}
			else
			{
				FloodFillTools.LockAreaFill(x, y, this);
			}
		}

		public void ReadCurrentCustomBrush()
		{
			if (customBrush == null)
			{
				return;
			}
			customBrushWidth = customBrush.width;
			customBrushHeight = customBrush.height;
			customBrushBytes = new byte[customBrushWidth * customBrushHeight * 4];
			Color[] array = customBrush.GetPixels();
			int num = 0;
			for (int i = 0; i < customBrushHeight; i++)
			{
				for (int j = 0; j < customBrushWidth; j++)
				{
					Color color = array[i * customBrushHeight + j];
					customBrushBytes[num] = (byte)(color.r * 255f);
					customBrushBytes[num + 1] = (byte)(color.g * 255f);
					customBrushBytes[num + 2] = (byte)(color.b * 255f);
					customBrushBytes[num + 3] = (byte)(color.a * 255f);
					num += 4;
				}
			}
			customBrushWidthHalf = customBrushWidth / 2;
		}

		public void ReadCurrentCustomPattern(Texture2D patternTexture)
		{
			if (patternTexture == null)
			{
				return;
			}
			pattenTexture = patternTexture;
			customPatternWidth = patternTexture.width;
			customPatternHeight = patternTexture.height;
			patternBrushBytes = new byte[customPatternWidth * customPatternHeight * 4];
			Color[] array = patternTexture.GetPixels();
			int num = 0;
			for (int i = 0; i < customPatternWidth; i++)
			{
				for (int j = 0; j < customPatternHeight; j++)
				{
					Color color = array[j * customPatternHeight + i];
					patternBrushBytes[num] = (byte)(color.r * 255f);
					patternBrushBytes[num + 1] = (byte)(color.g * 255f);
					patternBrushBytes[num + 2] = (byte)(color.b * 255f);
					patternBrushBytes[num + 3] = (byte)(color.a * 255f);
					num += 4;
				}
			}
		}

		public void ClearImage()
		{
			if (usingClearingImage)
			{
				ClearImageWithImage();
				return;
			}
			int num = 0;
			for (int i = 0; i < texHeight; i++)
			{
				for (int j = 0; j < texWidth; j++)
				{
					pixels[num] = clearColor.r;
					pixels[num + 1] = clearColor.g;
					pixels[num + 2] = clearColor.b;
					pixels[num + 3] = clearColor.a;
					num += 4;
				}
			}
			tex.LoadRawTextureData(pixels);
			tex.Apply(updateMipmaps: true);
		}

		public void ClearImageWithImage()
		{
			Array.Copy(clearPixels, 0, pixels, 0, clearPixels.Length);
			tex.LoadRawTextureData(clearPixels);
			tex.Apply(updateMipmaps: false);
		}

		public void ReadMaskImage_old()
		{
			maskPixels = new byte[texWidth * texHeight * 4];
			Color[] array = maskTex.GetPixels(0);
			int num = 0;
			for (int i = 0; i < texHeight; i++)
			{
				for (int j = 0; j < texWidth; j++)
				{
					maskPixels[num] = (byte)(array[num].r * 255f);
					maskPixels[num + 1] = (byte)(array[num].g * 255f);
					maskPixels[num + 2] = (byte)(array[num].b * 255f);
					maskPixels[num + 3] = (byte)(array[num].a * 255f);
					num += 4;
				}
			}
		}

		public void ReadMaskImage()
		{
			maskPixels = new byte[texWidth * texHeight * 4];
			Color[] array = maskTex.GetPixels(0);
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < texHeight; i++)
			{
				for (int j = 0; j < texWidth; j++)
				{
					maskPixels[num] = (byte)(array[num2].r * 255f);
					maskPixels[num + 1] = (byte)(array[num2].g * 255f);
					maskPixels[num + 2] = (byte)(array[num2].b * 255f);
					maskPixels[num + 3] = (byte)(array[num2].a * 255f);
					num += 4;
					num2++;
				}
			}
		}

		public void ReadClearingImage()
		{
			clearPixels = new byte[texWidth * texHeight * 4];
			tex.SetPixels32(((Texture2D)GetComponent<Renderer>().material.GetTexture(targetTexture)).GetPixels32());
			tex.Apply(updateMipmaps: false);
			int num = 0;
			for (int i = 0; i < texHeight; i++)
			{
				for (int j = 0; j < texWidth; j++)
				{
					Color pixel = tex.GetPixel(j, i);
					clearPixels[num] = (byte)(pixel.r * 255f);
					clearPixels[num + 1] = (byte)(pixel.g * 255f);
					clearPixels[num + 2] = (byte)(pixel.b * 255f);
					clearPixels[num + 3] = (byte)(pixel.a * 255f);
					num += 4;
				}
			}
		}

		private void CreateCanvasQuad()
		{
			Mesh mesh = GetComponent<MeshFilter>().mesh;
			mesh.Clear();
			Vector3[] array = new Vector3[4];
			Vector3[] array2 = new Vector3[4];
			Vector3 localScale = base.transform.localScale;
			Transform parent = base.transform.parent;
			do
			{
				float x = localScale.x;
				Vector3 localScale2 = parent.localScale;
				localScale.x = x * localScale2.x;
				float y = localScale.y;
				Vector3 localScale3 = parent.localScale;
				localScale.y = y * localScale3.y;
				float z = localScale.z;
				Vector3 localScale4 = parent.localScale;
				localScale.z = z * localScale4.z;
				parent = parent.parent;
			}
			while (parent != null);
			Vector3 position = base.transform.position;
			base.transform.position = Vector3.zero;
			localScale.x = 1f / localScale.x;
			localScale.y = 1f / localScale.y;
			localScale.z = 1f / localScale.z;
			base.gameObject.GetComponent<RectTransform>().GetWorldCorners(array);
			base.transform.position = position;
			Vector3 eulerAngles = base.transform.localRotation.eulerAngles;
			Quaternion q = Quaternion.Euler(0f, 0f, 0f - eulerAngles.z);
			Vector3 vector = localScale;
			Matrix4x4 matrix4x = Matrix4x4.TRS(Vector3.zero, q, Vector3.one);
			for (int i = 0; i < 4; i++)
			{
				array2[i] = matrix4x.MultiplyPoint3x4(array[i]);
				Vector3 vector2 = array2[i];
				vector2.x *= localScale.x;
				vector2.y *= localScale.y;
				vector2.z *= localScale.z;
				array2[i] = vector2;
			}
			mesh.vertices = new Vector3[4]
			{
				array2[0],
				array2[1],
				array2[2],
				array2[3]
			};
			mesh.uv = new Vector2[4]
			{
				new Vector2(0f, 0f),
				new Vector2(0f, 1f),
				new Vector2(1f, 1f),
				new Vector2(1f, 0f)
			};
			mesh.triangles = new int[6]
			{
				0,
				1,
				2,
				0,
				2,
				3
			};
			mesh.RecalculateNormals();
			mesh.tangents = new Vector4[4]
			{
				new Vector4(1f, 0f, 0f, -1f),
				new Vector4(1f, 0f, 0f, -1f),
				new Vector4(1f, 0f, 0f, -1f),
				new Vector4(1f, 0f, 0f, -1f)
			};
			base.gameObject.AddComponent<MeshCollider>();
		}

		public bool CompareThreshold(byte a, byte b)
		{
			if (a < b)
			{
				a = (byte)(a ^ b);
				b = (byte)(b ^ a);
				a = (byte)(a ^ b);
			}
			return a - b <= paintThreshold;
		}

		public void SetMaskTextureMode()
		{
			useMaskImage = true;
			useLockArea = true;
			ReadMaskImage();
			base.gameObject.GetComponent<Renderer>().material.SetTexture("_MaskTex", maskTex);
		}

		public bool SetBitmapBrush(Texture2D brushTexture, BrushProperties brushType, bool isAditiveBrush, bool brushCanDrawOnBlack, Color brushColor, bool usesLockMasks, bool useBrushAlpha, Texture2D brushPattern)
		{
			customBrush = brushTexture;
			ReadCurrentCustomBrush();
			brushMode = brushType;
			useAdditiveColors = isAditiveBrush;
			canDrawOnBlack = brushCanDrawOnBlack;
			paintColor = brushColor;
			useLockArea = usesLockMasks;
			useMaskLayerOnly = usesLockMasks;
			useThreshold = usesLockMasks;
			useCustomBrushAlpha = useBrushAlpha;
			if (brushPattern != null)
			{
				ReadCurrentCustomPattern(brushPattern);
			}
			isLinePaint = false;
			return true;
		}

		public bool SetVectorBrush(VectorBrush type, int sizeX, int sizeY, Color brushColor, Texture2D pattern, bool isAditiveBrush, bool brushCanDrawOnBlack, bool usesLockMasks, bool useBrushAlpha)
		{
			vectorBrushType = type;
			if (pattern != null)
			{
				drawMode = DrawMode.Default;
				ReadCurrentCustomPattern(pattern);
				brushMode = BrushProperties.Pattern;
			}
			else
			{
				drawMode = DrawMode.Default;
				paintColor = brushColor;
				brushMode = BrushProperties.Default;
			}
			useAdditiveColors = isAditiveBrush;
			canDrawOnBlack = brushCanDrawOnBlack;
			useCustomBrushAlpha = useBrushAlpha;
			useLockArea = usesLockMasks;
			brushSize = sizeX;
			customBrushHeight = sizeX;
			customBrushWidth = sizeY;
			isLinePaint = false;
			return true;
		}

		public bool SetFloodFIllBrush(Color floodColor, bool usesLockMasks)
		{
			isLinePaint = false;
			drawMode = DrawMode.FloodFill;
			paintColor = floodColor;
			useLockArea = usesLockMasks;
			return true;
		}

		public bool SetDrawingTexture(Texture2D texture)
		{
			texWidth = texture.width;
			texHeight = texture.height;
			tex = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, mipChain: false);
			pixels = new byte[texture.width * texture.height * 4];
			Color[] array = texture.GetPixels();
			pixels = new byte[texture.width * texture.height * 4];
			int num = 0;
			for (int i = 0; i < texture.height; i++)
			{
				for (int j = 0; j < texture.width; j++)
				{
					pixels[num] = (byte)(array[i * texture.width + j].r * 255f);
					pixels[num + 1] = (byte)(array[i * texture.width + j].g * 255f);
					pixels[num + 2] = (byte)(array[i * texture.width + j].b * 255f);
					pixels[num + 3] = (byte)(array[i * texture.width + j].a * 255f);
					if (pixels[num + 3] > 0)
					{
						pixels[num + 3] = byte.MaxValue;
					}
					else
					{
						pixels[num] = 0;
						pixels[num + 1] = 0;
						pixels[num + 2] = 0;
						pixels[num + 3] = 0;
					}
					num += 4;
				}
			}
			tex.LoadRawTextureData(pixels);
			tex.Apply(updateMipmaps: false);
			GetComponent<Renderer>().material.SetTexture(targetTexture, tex);
			if (createCanvasMesh)
			{
				base.gameObject.GetComponent<MeshRenderer>().enabled = false;
				base.gameObject.GetComponent<RawImage>().texture = tex;
				base.gameObject.GetComponent<RawImage>().enabled = true;
			}
			if (undoEnabled)
			{
				undoPixels = new byte[pixels.Length];
				Array.Copy(pixels, undoPixels, pixels.Length);
			}
			return true;
		}

		public bool SetDrawingMask(Texture2D texture)
		{
			Color[] array = texture.GetPixels();
			maskPixels = new byte[texture.width * texture.height * 4];
			int num = 0;
			for (int i = 0; i < texture.height; i++)
			{
				for (int j = 0; j < texture.width; j++)
				{
					maskPixels[num] = (byte)(array[i * texture.width + j].r * 255f);
					maskPixels[num + 1] = (byte)(array[i * texture.width + j].g * 255f);
					maskPixels[num + 2] = (byte)(array[i * texture.width + j].b * 255f);
					maskPixels[num + 3] = (byte)(array[i * texture.width + j].a * 255f);
					if (maskPixels[num + 3] > 0)
					{
						maskPixels[num + 3] = byte.MaxValue;
					}
					else
					{
						maskPixels[num] = 0;
						maskPixels[num + 1] = 0;
						maskPixels[num + 2] = 0;
						maskPixels[num + 3] = 0;
					}
					num += 4;
				}
			}
			return true;
		}

		public void SetLineBrush(int sizeOfBrush, int edgeSize, Color color, Texture2D lineCorePattern, Texture2D lineEdgePattern)
		{
			isLinePaint = true;
			brushSize = sizeOfBrush;
			paintColor = color;
			lineEdgeSize = edgeSize;
			useAdditiveColors = false;
			useLockArea = false;
			useMaskImage = false;
			if (lineCorePattern != null)
			{
				isPatternLine = true;
				ReadCurrentCustomPattern(lineCorePattern);
			}
			else
			{
				isPatternLine = false;
			}
			if (lineEdgeSize > 0 && lineEdgePattern == null)
			{
				UnityEngine.Debug.LogError("Line edge set, but no pattern for line edge assigned!");
			}
			else if (lineEdgePattern != null)
			{
				customBrush = lineEdgePattern;
				ReadCurrentCustomBrush();
			}
			customBrushWidth = sizeOfBrush;
			customBrushHeight = sizeOfBrush;
		}
	}
}
