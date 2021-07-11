using AdvancedMobilePaint.Tools;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedMobilePaint
{
	public class PaintUndoManager : MonoBehaviour
	{
		public AdvancedMobilePaint paintEngine;

		public bool doingWork;

		private Stack<UStep> steps;

		private Stack<UStep> redoSteps;

		public int stackDepth = int.MaxValue;

		public bool stackFull;

		private void Awake()
		{
			steps = new Stack<UStep>();
			redoSteps = new Stack<UStep>();
			if (paintEngine == null)
			{
				if (base.gameObject.GetComponent<AdvancedMobilePaint>() != null)
				{
					paintEngine = base.gameObject.GetComponent<AdvancedMobilePaint>();
					paintEngine.undoEnabled = true;
					paintEngine.undoController = base.gameObject.GetComponent<PaintUndoManager>();
				}
				else
				{
					UnityEngine.Debug.LogError("AMP: PaintUndoManger Cant find paint engine!");
				}
			}
		}

		public void AddStep(UStep step)
		{
			if (step != null && steps != null)
			{
				if (steps.Count < stackDepth)
				{
					steps.Push(step);
					return;
				}
				stackFull = true;
				UnityEngine.Debug.Log("AMP: StackFull");
			}
			else
			{
				UnityEngine.Debug.Log("AMP: ERROR IN ADDING NEW STEP!");
			}
		}

		public void ClearSteps()
		{
			steps.Clear();
			redoSteps.Clear();
			stackFull = false;
		}

		private void UndoRedrawSteps()
		{
			Stack<UStep> stack = new Stack<UStep>();
			UStep uStep = new UStep();
			uStep.SetStepPropertiesFromEngine(paintEngine);
			while (steps.Count != 0)
			{
				stack.Push(steps.Pop());
			}
			while (stack.Count != 0)
			{
				UStep uStep2 = stack.Pop();
				if (uStep2.type >= 0 || uStep2.type < 5)
				{
					UnityEngine.Debug.Log("UNDO STEP EXEC");
					uStep2.SetPropertiesFromStep(paintEngine);
					if (uStep2.type == 0)
					{
						switch (uStep2.brushMode)
						{
						case BrushProperties.Default:
							UnityEngine.Debug.Log("DEF MODE");
							paintEngine.ReadCurrentCustomBrush();
							for (int j = 0; j < uStep2.drawCoordinates.Count; j++)
							{
								Vector2 vector3 = uStep2.drawCoordinates[j];
								int px2 = (int)vector3.x;
								Vector2 vector4 = uStep2.drawCoordinates[j];
								BitmapBrushesTools.DrawCustomBrush2(px2, (int)vector4.y, paintEngine);
							}
							break;
						case BrushProperties.Simple:
							paintEngine.ReadCurrentCustomBrush();
							for (int k = 0; k < uStep2.drawCoordinates.Count; k++)
							{
								Vector2 vector5 = uStep2.drawCoordinates[k];
								int px3 = (int)vector5.x;
								Vector2 vector6 = uStep2.drawCoordinates[k];
								BitmapBrushesTools.DrawCustomBrush2(px3, (int)vector6.y, paintEngine);
							}
							break;
						case BrushProperties.Pattern:
							paintEngine.ReadCurrentCustomBrush();
							paintEngine.ReadCurrentCustomPattern(uStep2.patternTexture);
							for (int i = 0; i < uStep2.drawCoordinates.Count; i++)
							{
								Vector2 vector = uStep2.drawCoordinates[i];
								int px = (int)vector.x;
								Vector2 vector2 = uStep2.drawCoordinates[i];
								BitmapBrushesTools.DrawCustomBrush2(px, (int)vector2.y, paintEngine);
							}
							break;
						}
					}
					else if (uStep2.type == 2)
					{
						for (int l = 0; l < uStep2.drawCoordinates.Count; l++)
						{
							if (paintEngine.useSmartFloodFill)
							{
								Vector2 vector7 = uStep2.drawCoordinates[l];
								int x = (int)vector7.x;
								Vector2 vector8 = uStep2.drawCoordinates[l];
								FloodFillTools.FloodFillAutoMaskWithThreshold(x, (int)vector8.y, paintEngine);
							}
							else if (uStep2.useTreshold)
							{
								if (uStep2.useMaskLayerOnly)
								{
									Vector2 vector9 = uStep2.drawCoordinates[l];
									int x2 = (int)vector9.x;
									Vector2 vector10 = uStep2.drawCoordinates[l];
									FloodFillTools.FloodFillMaskOnlyWithThreshold(x2, (int)vector10.y, paintEngine);
								}
								else
								{
									Vector2 vector11 = uStep2.drawCoordinates[l];
									int x3 = (int)vector11.x;
									Vector2 vector12 = uStep2.drawCoordinates[l];
									FloodFillTools.FloodFillWithTreshold(x3, (int)vector12.y, paintEngine);
								}
							}
							else if (uStep2.useMaskLayerOnly)
							{
								Vector2 vector13 = uStep2.drawCoordinates[l];
								int x4 = (int)vector13.x;
								Vector2 vector14 = uStep2.drawCoordinates[l];
								FloodFillTools.FloodFillMaskOnly(x4, (int)vector14.y, paintEngine);
							}
							else
							{
								Vector2 vector15 = uStep2.drawCoordinates[l];
								int x5 = (int)vector15.x;
								Vector2 vector16 = uStep2.drawCoordinates[l];
								FloodFillTools.FloodFill(x5, (int)vector16.y, paintEngine);
							}
						}
					}
					else if (uStep2.type == 1)
					{
						if (uStep2.brushMode == BrushProperties.Default)
						{
							if (uStep2.vectorBrushType == VectorBrush.Circle)
							{
								for (int m = 0; m < uStep2.drawCoordinates.Count; m++)
								{
									Vector2 vector17 = uStep2.drawCoordinates[m];
									int x6 = (int)vector17.x;
									Vector2 vector18 = uStep2.drawCoordinates[m];
									VectorBrushesTools.DrawCircle(x6, (int)vector18.y, paintEngine);
								}
							}
							else
							{
								for (int n = 0; n < uStep2.drawCoordinates.Count; n++)
								{
									Vector2 vector19 = uStep2.drawCoordinates[n];
									int px4 = (int)vector19.x;
									Vector2 vector20 = uStep2.drawCoordinates[n];
									VectorBrushesTools.DrawRectangle(px4, (int)vector20.y, paintEngine);
								}
							}
						}
						else if (uStep2.brushMode == BrushProperties.Pattern)
						{
							paintEngine.ReadCurrentCustomPattern(uStep2.patternTexture);
							if (uStep2.vectorBrushType == VectorBrush.Circle)
							{
								for (int num = 0; num < uStep2.drawCoordinates.Count; num++)
								{
									Vector2 vector21 = uStep2.drawCoordinates[num];
									int x7 = (int)vector21.x;
									Vector2 vector22 = uStep2.drawCoordinates[num];
									VectorBrushesTools.DrawPatternCircle(x7, (int)vector22.y, paintEngine);
								}
							}
							else
							{
								for (int num2 = 0; num2 < uStep2.drawCoordinates.Count; num2++)
								{
									Vector2 vector23 = uStep2.drawCoordinates[num2];
									int px5 = (int)vector23.x;
									Vector2 vector24 = uStep2.drawCoordinates[num2];
									VectorBrushesTools.DrawPatternRectangle(px5, (int)vector24.y, paintEngine);
								}
							}
						}
					}
					else if (uStep2.type == 4)
					{
						if (paintEngine.multitouchEnabled)
						{
							DrawMultiTouchLine(uStep2);
						}
						else
						{
							DrawSingleTouchLine(uStep2);
						}
					}
					steps.Push(uStep2);
				}
				else
				{
					steps.Push(uStep2);
				}
			}
			paintEngine.tex.LoadRawTextureData(paintEngine.pixels);
			paintEngine.tex.Apply();
			uStep.SetPropertiesFromStep(paintEngine);
		}

		public void DrawMultiTouchLine(UStep tmp)
		{
			if (tmp.lineEgdeSize == 0)
			{
				UnityEngine.Debug.Log("DRAW MULTI TOUCH LINE UNDO MANAGER!");
				if (tmp.isPatternLine)
				{
					for (int i = 0; i < tmp.touchCoordinates.Count; i++)
					{
						Vector2 vector = tmp.drawCoordinates[tmp.touchCoordinates[i].coordinatesIndex[0]];
						int x = (int)vector.x;
						Vector2 vector2 = tmp.drawCoordinates[tmp.touchCoordinates[i].coordinatesIndex[0]];
						VectorBrushesTools.DrawPatternCircle(x, (int)vector2.y, paintEngine);
						for (int j = 1; j < tmp.touchCoordinates[i].coordinatesIndex.Count; j++)
						{
							BitmapBrushesTools.DrawLineBrush(tmp.drawCoordinates[tmp.touchCoordinates[i].coordinatesIndex[j - 1]], tmp.drawCoordinates[tmp.touchCoordinates[i].coordinatesIndex[j]], paintEngine.brushSize * 2, isPattern: true, paintEngine.patternBrushBytes, paintEngine);
							Vector2 vector3 = tmp.drawCoordinates[tmp.touchCoordinates[i].coordinatesIndex[j]];
							int x2 = (int)vector3.x;
							Vector2 vector4 = tmp.drawCoordinates[tmp.touchCoordinates[i].coordinatesIndex[j]];
							VectorBrushesTools.DrawPatternCircle(x2, (int)vector4.y, paintEngine);
						}
					}
					return;
				}
				for (int k = 0; k < tmp.touchCoordinates.Count; k++)
				{
					Vector2 vector5 = tmp.drawCoordinates[tmp.touchCoordinates[k].coordinatesIndex[0]];
					int x3 = (int)vector5.x;
					Vector2 vector6 = tmp.drawCoordinates[tmp.touchCoordinates[k].coordinatesIndex[0]];
					VectorBrushesTools.DrawCircle(x3, (int)vector6.y, paintEngine);
					for (int l = 1; l < tmp.touchCoordinates[k].coordinatesIndex.Count; l++)
					{
						BitmapBrushesTools.DrawLineBrush(tmp.drawCoordinates[tmp.touchCoordinates[k].coordinatesIndex[l - 1]], tmp.drawCoordinates[tmp.touchCoordinates[k].coordinatesIndex[l]], paintEngine.brushSize * 2, isPattern: false, null, paintEngine);
						Vector2 vector7 = tmp.drawCoordinates[tmp.touchCoordinates[k].coordinatesIndex[l]];
						int x4 = (int)vector7.x;
						Vector2 vector8 = tmp.drawCoordinates[tmp.touchCoordinates[k].coordinatesIndex[l]];
						VectorBrushesTools.DrawCircle(x4, (int)vector8.y, paintEngine);
					}
				}
				return;
			}
			if (tmp.isPatternLine)
			{
				for (int m = 0; m < tmp.touchCoordinates.Count; m++)
				{
					Vector2 vector9 = tmp.drawCoordinates[tmp.touchCoordinates[m].coordinatesIndex[0]];
					int x5 = (int)vector9.x;
					Vector2 vector10 = tmp.drawCoordinates[tmp.touchCoordinates[m].coordinatesIndex[0]];
					BitmapBrushesTools.DrawPatternCircle(x5, (int)vector10.y, paintEngine.customBrushBytes, paintEngine.brushSize + paintEngine.lineEdgeSize, paintEngine);
					Vector2 vector11 = tmp.drawCoordinates[tmp.touchCoordinates[m].coordinatesIndex[0]];
					int x6 = (int)vector11.x;
					Vector2 vector12 = tmp.drawCoordinates[tmp.touchCoordinates[m].coordinatesIndex[0]];
					VectorBrushesTools.DrawPatternCircle(x6, (int)vector12.y, paintEngine);
					for (int n = 1; n < tmp.touchCoordinates[m].coordinatesIndex.Count; n++)
					{
						Vector2 vector13 = tmp.drawCoordinates[tmp.touchCoordinates[m].coordinatesIndex[n]];
						int x7 = (int)vector13.x;
						Vector2 vector14 = tmp.drawCoordinates[tmp.touchCoordinates[m].coordinatesIndex[n]];
						BitmapBrushesTools.DrawPatternCircle(x7, (int)vector14.y, paintEngine.customBrushBytes, paintEngine.brushSize + paintEngine.lineEdgeSize, paintEngine);
						BitmapBrushesTools.DrawLineBrush(tmp.drawCoordinates[tmp.touchCoordinates[m].coordinatesIndex[n - 1]], tmp.drawCoordinates[tmp.touchCoordinates[m].coordinatesIndex[n]], paintEngine.brushSize * 2 + paintEngine.lineEdgeSize * 2, isPattern: true, paintEngine.customBrushBytes, paintEngine);
						if (n > 1)
						{
							BitmapBrushesTools.DrawLineBrush(tmp.drawCoordinates[tmp.touchCoordinates[m].coordinatesIndex[n - 2]], tmp.drawCoordinates[tmp.touchCoordinates[m].coordinatesIndex[n - 1]], paintEngine.brushSize * 2 + paintEngine.lineEdgeSize * 2, isPattern: true, paintEngine.customBrushBytes, paintEngine);
							BitmapBrushesTools.DrawLineBrush(tmp.drawCoordinates[tmp.touchCoordinates[m].coordinatesIndex[n - 2]], tmp.drawCoordinates[tmp.touchCoordinates[m].coordinatesIndex[n - 1]], paintEngine.brushSize * 2, isPattern: true, paintEngine.patternBrushBytes, paintEngine);
							Vector2 vector15 = tmp.drawCoordinates[tmp.touchCoordinates[m].coordinatesIndex[n - 2]];
							int x8 = (int)vector15.x;
							Vector2 vector16 = tmp.drawCoordinates[tmp.touchCoordinates[m].coordinatesIndex[n - 2]];
							VectorBrushesTools.DrawPatternCircle(x8, (int)vector16.y, paintEngine);
						}
					}
				}
				return;
			}
			for (int num = 0; num < tmp.touchCoordinates.Count; num++)
			{
				Vector2 vector17 = tmp.drawCoordinates[tmp.touchCoordinates[num].coordinatesIndex[0]];
				int x9 = (int)vector17.x;
				Vector2 vector18 = tmp.drawCoordinates[tmp.touchCoordinates[num].coordinatesIndex[0]];
				VectorBrushesTools.DrawCircle(x9, (int)vector18.y, paintEngine);
				for (int num2 = 1; num2 < tmp.touchCoordinates[num].coordinatesIndex.Count; num2++)
				{
					BitmapBrushesTools.DrawLineBrush(tmp.drawCoordinates[tmp.touchCoordinates[num].coordinatesIndex[num2 - 1]], tmp.drawCoordinates[tmp.touchCoordinates[num].coordinatesIndex[num2]], paintEngine.brushSize * 2, isPattern: false, null, paintEngine);
					Vector2 vector19 = tmp.drawCoordinates[tmp.touchCoordinates[num].coordinatesIndex[num2]];
					int x10 = (int)vector19.x;
					Vector2 vector20 = tmp.drawCoordinates[tmp.touchCoordinates[num].coordinatesIndex[num2]];
					VectorBrushesTools.DrawCircle(x10, (int)vector20.y, paintEngine);
				}
			}
		}

		public void DrawSingleTouchLine(UStep tmp)
		{
			if (tmp.lineEgdeSize == 0)
			{
				if (tmp.isPatternLine)
				{
					Vector2 vector = tmp.drawCoordinates[0];
					int x = (int)vector.x;
					Vector2 vector2 = tmp.drawCoordinates[0];
					VectorBrushesTools.DrawPatternCircle(x, (int)vector2.y, paintEngine);
					for (int i = 1; i < tmp.drawCoordinates.Count; i++)
					{
						BitmapBrushesTools.DrawLineBrush(tmp.drawCoordinates[i - 1], tmp.drawCoordinates[i], paintEngine.brushSize * 2, isPattern: true, paintEngine.patternBrushBytes, paintEngine);
						Vector2 vector3 = tmp.drawCoordinates[i];
						int x2 = (int)vector3.x;
						Vector2 vector4 = tmp.drawCoordinates[i];
						VectorBrushesTools.DrawPatternCircle(x2, (int)vector4.y, paintEngine);
					}
				}
				else
				{
					Vector2 vector5 = tmp.drawCoordinates[0];
					int x3 = (int)vector5.x;
					Vector2 vector6 = tmp.drawCoordinates[0];
					VectorBrushesTools.DrawCircle(x3, (int)vector6.y, paintEngine);
					for (int j = 1; j < tmp.drawCoordinates.Count; j++)
					{
						BitmapBrushesTools.DrawLineBrush(tmp.drawCoordinates[j - 1], tmp.drawCoordinates[j], paintEngine.brushSize * 2, isPattern: false, null, paintEngine);
						Vector2 vector7 = tmp.drawCoordinates[j];
						int x4 = (int)vector7.x;
						Vector2 vector8 = tmp.drawCoordinates[j];
						VectorBrushesTools.DrawCircle(x4, (int)vector8.y, paintEngine);
					}
				}
			}
			else if (tmp.isPatternLine)
			{
				Vector2 vector9 = tmp.drawCoordinates[0];
				int x5 = (int)vector9.x;
				Vector2 vector10 = tmp.drawCoordinates[0];
				BitmapBrushesTools.DrawPatternCircle(x5, (int)vector10.y, paintEngine.customBrushBytes, paintEngine.brushSize + paintEngine.lineEdgeSize, paintEngine);
				Vector2 vector11 = tmp.drawCoordinates[0];
				int x6 = (int)vector11.x;
				Vector2 vector12 = tmp.drawCoordinates[0];
				VectorBrushesTools.DrawPatternCircle(x6, (int)vector12.y, paintEngine);
				for (int k = 1; k < tmp.drawCoordinates.Count; k++)
				{
					Vector2 vector13 = tmp.drawCoordinates[k];
					int x7 = (int)vector13.x;
					Vector2 vector14 = tmp.drawCoordinates[k];
					BitmapBrushesTools.DrawPatternCircle(x7, (int)vector14.y, paintEngine.customBrushBytes, paintEngine.brushSize + paintEngine.lineEdgeSize, paintEngine);
					BitmapBrushesTools.DrawLineBrush(tmp.drawCoordinates[k - 1], tmp.drawCoordinates[k], paintEngine.brushSize * 2 + paintEngine.lineEdgeSize * 2, isPattern: true, paintEngine.customBrushBytes, paintEngine);
					if (k > 1)
					{
						BitmapBrushesTools.DrawLineBrush(tmp.drawCoordinates[k - 2], tmp.drawCoordinates[k - 1], paintEngine.brushSize * 2 + paintEngine.lineEdgeSize * 2, isPattern: true, paintEngine.customBrushBytes, paintEngine);
						BitmapBrushesTools.DrawLineBrush(tmp.drawCoordinates[k - 2], tmp.drawCoordinates[k - 1], paintEngine.brushSize * 2, isPattern: true, paintEngine.patternBrushBytes, paintEngine);
						Vector2 vector15 = tmp.drawCoordinates[k - 2];
						int x8 = (int)vector15.x;
						Vector2 vector16 = tmp.drawCoordinates[k - 2];
						VectorBrushesTools.DrawPatternCircle(x8, (int)vector16.y, paintEngine);
					}
				}
			}
			else
			{
				Vector2 vector17 = tmp.drawCoordinates[0];
				int x9 = (int)vector17.x;
				Vector2 vector18 = tmp.drawCoordinates[0];
				VectorBrushesTools.DrawCircle(x9, (int)vector18.y, paintEngine);
				for (int l = 1; l < tmp.drawCoordinates.Count; l++)
				{
					BitmapBrushesTools.DrawLineBrush(tmp.drawCoordinates[l - 1], tmp.drawCoordinates[l], paintEngine.brushSize * 2, isPattern: false, null, paintEngine);
					Vector2 vector19 = tmp.drawCoordinates[l];
					int x10 = (int)vector19.x;
					Vector2 vector20 = tmp.drawCoordinates[l];
					VectorBrushesTools.DrawCircle(x10, (int)vector20.y, paintEngine);
				}
			}
		}

		public void UndoLastStep()
		{
			if (!doingWork && steps.Count > 0)
			{
				doingWork = true;
				UStep uStep = steps.Pop();
				switch (uStep.type)
				{
				case 0:
				case 1:
				case 2:
				case 3:
				case 4:
					paintEngine.CopyUndoPixels(paintEngine.pixels);
					UndoRedrawSteps();
					doingWork = false;
					break;
				}
				redoSteps.Push(uStep);
			}
		}

		public void RedoLastStep()
		{
			if (doingWork || redoSteps.Count <= 0)
			{
				return;
			}
			doingWork = true;
			UStep uStep = redoSteps.Pop();
			switch (uStep.type)
			{
			case 0:
			case 1:
			case 2:
			case 3:
			case 4:
			{
				UStep uStep2 = new UStep();
				uStep2.SetStepPropertiesFromEngine(paintEngine);
				if (uStep.type > -1 || uStep.type < 5)
				{
					UnityEngine.Debug.Log("Redo STEP EXEC");
					uStep.SetPropertiesFromStep(paintEngine);
					if (uStep.type == 0)
					{
						switch (uStep.brushMode)
						{
						case BrushProperties.Default:
							UnityEngine.Debug.Log("DEF-R MODE");
							paintEngine.ReadCurrentCustomBrush();
							for (int j = 0; j < uStep.drawCoordinates.Count; j++)
							{
								Vector2 vector3 = uStep.drawCoordinates[j];
								int px2 = (int)vector3.x;
								Vector2 vector4 = uStep.drawCoordinates[j];
								BitmapBrushesTools.DrawCustomBrush2(px2, (int)vector4.y, paintEngine);
							}
							break;
						case BrushProperties.Simple:
							paintEngine.ReadCurrentCustomBrush();
							for (int k = 0; k < uStep.drawCoordinates.Count; k++)
							{
								Vector2 vector5 = uStep.drawCoordinates[k];
								int px3 = (int)vector5.x;
								Vector2 vector6 = uStep.drawCoordinates[k];
								BitmapBrushesTools.DrawCustomBrush2(px3, (int)vector6.y, paintEngine);
							}
							break;
						case BrushProperties.Pattern:
							paintEngine.ReadCurrentCustomBrush();
							paintEngine.ReadCurrentCustomPattern(uStep.patternTexture);
							for (int i = 0; i < uStep.drawCoordinates.Count; i++)
							{
								Vector2 vector = uStep.drawCoordinates[i];
								int px = (int)vector.x;
								Vector2 vector2 = uStep.drawCoordinates[i];
								BitmapBrushesTools.DrawCustomBrush2(px, (int)vector2.y, paintEngine);
							}
							break;
						}
					}
					else if (uStep.type == 2)
					{
						for (int l = 0; l < uStep.drawCoordinates.Count; l++)
						{
							if (paintEngine.useSmartFloodFill)
							{
								Vector2 vector7 = uStep.drawCoordinates[l];
								int x = (int)vector7.x;
								Vector2 vector8 = uStep.drawCoordinates[l];
								FloodFillTools.FloodFillAutoMaskWithThreshold(x, (int)vector8.y, paintEngine);
							}
							else if (uStep.useTreshold)
							{
								if (uStep.useMaskLayerOnly)
								{
									Vector2 vector9 = uStep.drawCoordinates[l];
									int x2 = (int)vector9.x;
									Vector2 vector10 = uStep.drawCoordinates[l];
									FloodFillTools.FloodFillMaskOnlyWithThreshold(x2, (int)vector10.y, paintEngine);
								}
								else
								{
									Vector2 vector11 = uStep.drawCoordinates[l];
									int x3 = (int)vector11.x;
									Vector2 vector12 = uStep.drawCoordinates[l];
									FloodFillTools.FloodFillWithTreshold(x3, (int)vector12.y, paintEngine);
								}
							}
							else if (uStep.useMaskLayerOnly)
							{
								Vector2 vector13 = uStep.drawCoordinates[l];
								int x4 = (int)vector13.x;
								Vector2 vector14 = uStep.drawCoordinates[l];
								FloodFillTools.FloodFillMaskOnly(x4, (int)vector14.y, paintEngine);
							}
							else
							{
								Vector2 vector15 = uStep.drawCoordinates[l];
								int x5 = (int)vector15.x;
								Vector2 vector16 = uStep.drawCoordinates[l];
								FloodFillTools.FloodFill(x5, (int)vector16.y, paintEngine);
							}
						}
					}
					else if (uStep.type == 1)
					{
						if (uStep.brushMode == BrushProperties.Default)
						{
							if (uStep.vectorBrushType == VectorBrush.Circle)
							{
								for (int m = 0; m < uStep.drawCoordinates.Count; m++)
								{
									Vector2 vector17 = uStep.drawCoordinates[m];
									int x6 = (int)vector17.x;
									Vector2 vector18 = uStep.drawCoordinates[m];
									VectorBrushesTools.DrawCircle(x6, (int)vector18.y, paintEngine);
								}
							}
							else
							{
								for (int n = 0; n < uStep.drawCoordinates.Count; n++)
								{
									Vector2 vector19 = uStep.drawCoordinates[n];
									int px4 = (int)vector19.x;
									Vector2 vector20 = uStep.drawCoordinates[n];
									VectorBrushesTools.DrawRectangle(px4, (int)vector20.y, paintEngine);
								}
							}
						}
						else if (uStep.brushMode == BrushProperties.Pattern)
						{
							paintEngine.ReadCurrentCustomPattern(uStep.patternTexture);
							if (uStep.vectorBrushType == VectorBrush.Circle)
							{
								for (int num = 0; num < uStep.drawCoordinates.Count; num++)
								{
									Vector2 vector21 = uStep.drawCoordinates[num];
									int x7 = (int)vector21.x;
									Vector2 vector22 = uStep.drawCoordinates[num];
									VectorBrushesTools.DrawPatternCircle(x7, (int)vector22.y, paintEngine);
								}
							}
							else
							{
								for (int num2 = 0; num2 < uStep.drawCoordinates.Count; num2++)
								{
									Vector2 vector23 = uStep.drawCoordinates[num2];
									int px5 = (int)vector23.x;
									Vector2 vector24 = uStep.drawCoordinates[num2];
									VectorBrushesTools.DrawPatternRectangle(px5, (int)vector24.y, paintEngine);
								}
							}
						}
					}
					else if (uStep.type == 4)
					{
						if (paintEngine.multitouchEnabled)
						{
							DrawMultiTouchLine(uStep);
						}
						else
						{
							DrawSingleTouchLine(uStep);
						}
					}
					paintEngine.tex.LoadRawTextureData(paintEngine.pixels);
					paintEngine.tex.Apply();
					steps.Push(uStep);
				}
				else
				{
					steps.Push(uStep);
				}
				uStep2.SetPropertiesFromStep(paintEngine);
				break;
			}
			}
			doingWork = false;
		}
	}
}
