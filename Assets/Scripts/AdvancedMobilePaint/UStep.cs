using System.Collections.Generic;
using UnityEngine;

namespace AdvancedMobilePaint
{
	public class UStep
	{
		public int type;

		public BrushProperties brushMode;

		public DrawMode drawMode;

		public int brushSize;

		public Texture2D brushTexture;

		public Texture2D patternTexture;

		public bool useAdditiveColors;

		public bool canDrawOnBlack;

		public Color paintColor;

		public bool useLockArea;

		public bool useCustomBrushAlpha;

		public int vectorBrushIndex;

		public bool isFloodFill;

		public bool connectBrushStrokes;

		public bool useMaskLayerOnly;

		public bool useTreshold;

		public bool treshold;

		public bool useMaskImage;

		public float brushAlphaStrength;

		public List<Vector2> drawCoordinates;

		public VectorBrush vectorBrushType;

		public int brushWidth;

		public int brushHeight;

		public bool isLine;

		public int lineEgdeSize;

		public bool isPatternLine;

		public List<TouchCoordinates> touchCoordinates;

		public void SetPropertiesFromStep(AdvancedMobilePaint paintEngine)
		{
			paintEngine.brushMode = brushMode;
			paintEngine.drawMode = drawMode;
			paintEngine.brushSize = brushSize;
			paintEngine.customBrush = brushTexture;
			paintEngine.pattenTexture = patternTexture;
			paintEngine.useAdditiveColors = useAdditiveColors;
			paintEngine.canDrawOnBlack = canDrawOnBlack;
			paintEngine.paintColor = paintColor;
			paintEngine.useLockArea = useLockArea;
			paintEngine.useCustomBrushAlpha = useCustomBrushAlpha;
			paintEngine.connectBrushStokes = connectBrushStrokes;
			paintEngine.useMaskLayerOnly = useMaskLayerOnly;
			paintEngine.useThreshold = useTreshold;
			paintEngine.useMaskImage = useMaskImage;
			paintEngine.brushAlphaStrength = brushAlphaStrength;
			paintEngine.vectorBrushType = vectorBrushType;
			paintEngine.customBrushWidth = brushWidth;
			paintEngine.customBrushHeight = brushHeight;
			paintEngine.isLinePaint = isLine;
			paintEngine.lineEdgeSize = lineEgdeSize;
			paintEngine.isPatternLine = isPatternLine;
		}

		public void SetStepPropertiesFromEngine(AdvancedMobilePaint paintEngine)
		{
			brushMode = paintEngine.brushMode;
			drawMode = paintEngine.drawMode;
			brushSize = paintEngine.brushSize;
			brushTexture = paintEngine.customBrush;
			patternTexture = paintEngine.pattenTexture;
			useAdditiveColors = paintEngine.useAdditiveColors;
			canDrawOnBlack = paintEngine.canDrawOnBlack;
			paintColor = paintEngine.paintColor;
			useLockArea = paintEngine.useLockArea;
			useCustomBrushAlpha = paintEngine.useCustomBrushAlpha;
			connectBrushStrokes = paintEngine.connectBrushStokes;
			useMaskLayerOnly = paintEngine.useMaskLayerOnly;
			useTreshold = paintEngine.useThreshold;
			useMaskImage = paintEngine.useMaskImage;
			brushAlphaStrength = paintEngine.brushAlphaStrength;
			vectorBrushType = paintEngine.vectorBrushType;
			brushHeight = paintEngine.customBrushHeight;
			brushWidth = paintEngine.customBrushWidth;
			isLine = paintEngine.isLinePaint;
			isPatternLine = paintEngine.isPatternLine;
			lineEgdeSize = paintEngine.lineEdgeSize;
		}

		public void AddTouchCoordinate(int fId)
		{
			UnityEngine.Debug.Log("ADD TOUCH COORDINATE");
			if (this.touchCoordinates == null)
			{
				this.touchCoordinates = new List<TouchCoordinates>();
			}
			if (this.touchCoordinates.Count < 1)
			{
				UnityEngine.Debug.Log("ADD TOUCH COORDINATE FIRST TIME");
				TouchCoordinates touchCoordinates = new TouchCoordinates();
				touchCoordinates.coordinatesIndex = new List<int>();
				touchCoordinates.fingerId = fId;
				touchCoordinates.coordinatesIndex.Add(drawCoordinates.Count - 1);
				this.touchCoordinates.Add(touchCoordinates);
				return;
			}
			bool flag = false;
			for (int i = 0; i < this.touchCoordinates.Count; i++)
			{
				if (this.touchCoordinates[i].fingerId == fId)
				{
					UnityEngine.Debug.Log("ADD TOUCH COORDINATE EXISTING");
					this.touchCoordinates[i].coordinatesIndex.Add(drawCoordinates.Count - 1);
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				UnityEngine.Debug.Log("ADD TOUCH NEW");
				TouchCoordinates touchCoordinates2 = new TouchCoordinates();
				touchCoordinates2.coordinatesIndex = new List<int>();
				touchCoordinates2.fingerId = fId;
				touchCoordinates2.coordinatesIndex.Add(drawCoordinates.Count - 1);
				this.touchCoordinates.Add(touchCoordinates2);
			}
		}
	}
}
