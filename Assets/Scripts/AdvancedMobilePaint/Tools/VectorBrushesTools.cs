using UnityEngine;

namespace AdvancedMobilePaint.Tools
{
	public class VectorBrushesTools
	{
		public static void DrawLineWithVectorBrush(Vector2 start, Vector2 end, AdvancedMobilePaint paintEngine)
		{
			int num = (int)start.x;
			int num2 = (int)start.y;
			int num3 = (int)end.x;
			int num4 = (int)end.y;
			int num5 = Mathf.Abs(num3 - num);
			int num6 = Mathf.Abs(num4 - num2);
			int num7 = (num < num3) ? 1 : (-1);
			int num8 = (num2 < num4) ? 1 : (-1);
			int num9 = num5 - num6;
			bool flag = true;
			int num10 = paintEngine.customBrushWidth >> 1;
			int num11 = paintEngine.customBrushHeight >> 1;
			int num12 = 0;
			while (flag)
			{
				num12++;
				if (num12 > num10 || num12 > num11)
				{
					num12 = 0;
					DrawRectangle(num, num2, paintEngine);
				}
				if (num == num3 && num2 == num4)
				{
					flag = false;
				}
				int num13 = 2 * num9;
				if (num13 > -num6)
				{
					num9 -= num6;
					num += num7;
				}
				if (num13 < num5)
				{
					num9 += num5;
					num2 += num8;
				}
			}
		}

		public static void DrawRectangle(int px, int py, AdvancedMobilePaint paintEngine)
		{
			int num = px - paintEngine.customBrushWidth / 2;
			int num2 = py - paintEngine.customBrushHeight / 2;
			int num3 = (paintEngine.texWidth * num2 + num) * 4;
			bool flag = false;
			for (int i = 0; i < paintEngine.customBrushHeight; i++)
			{
				for (int j = 0; j < paintEngine.customBrushWidth; j++)
				{
					flag = false;
					if (num + j > paintEngine.texWidth - 2 || num + j < -1)
					{
						flag = true;
					}
					if (num3 < 0 || num3 >= paintEngine.pixels.Length)
					{
						flag = true;
					}
					if (!flag)
					{
						paintEngine.pixels[num3] = paintEngine.paintColor.r;
						paintEngine.pixels[num3 + 1] = paintEngine.paintColor.g;
						paintEngine.pixels[num3 + 2] = paintEngine.paintColor.b;
						paintEngine.pixels[num3 + 3] = paintEngine.paintColor.a;
					}
					num3 += 4;
				}
				num3 = (paintEngine.texWidth * ((num2 != 0) ? (num2 + i) : (-1)) + num + 1) * 4;
			}
		}

		public static void DrawPatternRectangle(int px, int py, AdvancedMobilePaint paintEngine)
		{
			UnityEngine.Debug.Log("DrawPatternRectangle");
			int num = px - paintEngine.customBrushWidth / 2;
			int num2 = py - paintEngine.customBrushHeight / 2;
			int num3 = (paintEngine.texWidth * num2 + num) * 4;
			bool flag = false;
			float num4 = 0f;
			float num5 = 0f;
			int num6 = 0;
			for (int i = 0; i < paintEngine.customBrushHeight; i++)
			{
				for (int j = 0; j < paintEngine.customBrushWidth; j++)
				{
					flag = false;
					if (num + j > paintEngine.texWidth - 2 || num + j < -1)
					{
						flag = true;
					}
					if (num3 < 0 || num3 >= paintEngine.pixels.Length)
					{
						flag = true;
					}
					if (!flag)
					{
						num4 = Mathf.Repeat(i + num2, paintEngine.customPatternWidth);
						num5 = Mathf.Repeat(j + num, paintEngine.customPatternWidth);
						num6 = (int)Mathf.Repeat(((float)paintEngine.customPatternWidth * num5 + num4) * 4f, paintEngine.patternBrushBytes.Length);
						paintEngine.pixels[num3] = paintEngine.patternBrushBytes[num6];
						paintEngine.pixels[num3 + 1] = paintEngine.patternBrushBytes[num6 + 1];
						paintEngine.pixels[num3 + 2] = paintEngine.patternBrushBytes[num6 + 2];
						paintEngine.pixels[num3 + 3] = paintEngine.patternBrushBytes[num6 + 3];
					}
					num3 += 4;
				}
				num3 = (paintEngine.texWidth * ((num2 != 0) ? (num2 + i) : (-1)) + num + 1) * 4;
			}
		}

		public static void DrawCircle(int x, int y, AdvancedMobilePaint paintEngine)
		{
			if (!paintEngine.canDrawOnBlack && paintEngine.pixels[(paintEngine.texWidth * y + x) * 4] == 0 && paintEngine.pixels[(paintEngine.texWidth * y + x) * 4 + 1] == 0 && paintEngine.pixels[(paintEngine.texWidth * y + x) * 4 + 2] == 0 && paintEngine.pixels[(paintEngine.texWidth * y + x) * 4 + 3] != 0)
			{
				return;
			}
			int num = 0;
			int num2 = paintEngine.brushSize * paintEngine.brushSize;
			int num3 = num2 << 2;
			int num4 = paintEngine.brushSize << 1;
			float num5 = 1f;
			num5 = (float)(int)paintEngine.paintColor.a / 255f * paintEngine.brushAlphaStrength;
			for (int i = 0; i < num3; i++)
			{
				int num6 = i % num4 - paintEngine.brushSize;
				int num7 = i / num4 - paintEngine.brushSize;
				if (num6 * num6 + num7 * num7 >= num2 || x + num6 < 0 || y + num7 < 0 || x + num6 >= paintEngine.texWidth || y + num7 >= paintEngine.texHeight)
				{
					continue;
				}
				num = (paintEngine.texWidth * (y + num7) + x + num6) * 4;
				if (paintEngine.useAdditiveColors)
				{
					if (!paintEngine.useLockArea || (paintEngine.useLockArea && paintEngine.lockMaskPixels[num] == 1))
					{
						paintEngine.pixels[num] = (byte)Mathf.Lerp((int)paintEngine.pixels[num], (int)paintEngine.paintColor.r, num5);
						paintEngine.pixels[num + 1] = (byte)Mathf.Lerp((int)paintEngine.pixels[num + 1], (int)paintEngine.paintColor.g, num5);
						paintEngine.pixels[num + 2] = (byte)Mathf.Lerp((int)paintEngine.pixels[num + 2], (int)paintEngine.paintColor.b, num5);
						paintEngine.pixels[num + 3] = (byte)Mathf.Lerp((int)paintEngine.pixels[num + 3], (int)paintEngine.paintColor.a, num5);
					}
				}
				else if (!paintEngine.useLockArea || (paintEngine.useLockArea && paintEngine.lockMaskPixels[num] == 1))
				{
					paintEngine.pixels[num] = paintEngine.paintColor.r;
					paintEngine.pixels[num + 1] = paintEngine.paintColor.g;
					paintEngine.pixels[num + 2] = paintEngine.paintColor.b;
					paintEngine.pixels[num + 3] = paintEngine.paintColor.a;
				}
			}
		}

		public static void DrawPatternCircle(int x, int y, AdvancedMobilePaint paintEngine)
		{
			if (!paintEngine.canDrawOnBlack && paintEngine.pixels[(paintEngine.texWidth * y + x) * 4] == 0 && paintEngine.pixels[(paintEngine.texWidth * y + x) * 4 + 1] == 0 && paintEngine.pixels[(paintEngine.texWidth * y + x) * 4 + 2] == 0 && paintEngine.pixels[(paintEngine.texWidth * y + x) * 4 + 3] != 0)
			{
				return;
			}
			int num = 0;
			int num2 = paintEngine.brushSize * paintEngine.brushSize;
			int num3 = num2 << 2;
			int num4 = paintEngine.brushSize << 1;
			int num5 = 0;
			int num6 = 0;
			float num7 = 0f;
			float num8 = 0f;
			int num9 = 0;
			float num10 = 1f;
			for (int i = 0; i < num3; i++)
			{
				num5 = i % num4 - paintEngine.brushSize;
				num6 = i / num4 - paintEngine.brushSize;
				if (num5 * num5 + num6 * num6 >= num2 || x + num5 < 0 || y + num6 < 0 || x + num5 >= paintEngine.texWidth || y + num6 >= paintEngine.texHeight)
				{
					continue;
				}
				num = (paintEngine.texWidth * (y + num6) + x + num5) * 4;
				if (paintEngine.useAdditiveColors)
				{
					if (!paintEngine.useLockArea || (paintEngine.useLockArea && paintEngine.lockMaskPixels[num] == 1))
					{
						num7 = Mathf.Repeat(y + num6, paintEngine.customPatternWidth);
						num8 = Mathf.Repeat(x + num5, paintEngine.customPatternWidth);
						num9 = (int)Mathf.Repeat(((float)paintEngine.customPatternWidth * num8 + num7) * 4f, paintEngine.patternBrushBytes.Length);
						num10 = (float)(int)paintEngine.patternBrushBytes[num9 + 3] / 255f * paintEngine.brushAlphaStrength;
						paintEngine.pixels[num] = (byte)Mathf.Lerp((int)paintEngine.pixels[num], (int)paintEngine.patternBrushBytes[num9], num10);
						paintEngine.pixels[num + 1] = (byte)Mathf.Lerp((int)paintEngine.pixels[num + 1], (int)paintEngine.patternBrushBytes[num9 + 1], num10);
						paintEngine.pixels[num + 2] = (byte)Mathf.Lerp((int)paintEngine.pixels[num + 2], (int)paintEngine.patternBrushBytes[num9 + 2], num10);
						paintEngine.pixels[num + 3] = (byte)Mathf.Lerp((int)paintEngine.pixels[num + 3], (int)paintEngine.patternBrushBytes[num9 + 3], num10);
					}
				}
				else if (!paintEngine.useLockArea || (paintEngine.useLockArea && paintEngine.lockMaskPixels[num] == 1))
				{
					num7 = Mathf.Repeat(y + num6, paintEngine.customPatternWidth);
					num8 = Mathf.Repeat(x + num5, paintEngine.customPatternWidth);
					num9 = (int)Mathf.Repeat(((float)paintEngine.customPatternWidth * num8 + num7) * 4f, paintEngine.patternBrushBytes.Length);
					paintEngine.pixels[num] = paintEngine.patternBrushBytes[num9];
					paintEngine.pixels[num + 1] = paintEngine.patternBrushBytes[num9 + 1];
					paintEngine.pixels[num + 2] = paintEngine.patternBrushBytes[num9 + 2];
					paintEngine.pixels[num + 3] = paintEngine.patternBrushBytes[num9 + 3];
				}
			}
		}

		public static void DrawLine(Vector2 start, Vector2 end, AdvancedMobilePaint paintEngine)
		{
			int num = (int)start.x;
			int num2 = (int)start.y;
			int num3 = (int)end.x;
			int num4 = (int)end.y;
			int num5 = Mathf.Abs(num3 - num);
			int num6 = Mathf.Abs(num4 - num2);
			int num7 = (num < num3) ? 1 : (-1);
			int num8 = (num2 < num4) ? 1 : (-1);
			int num9 = num5 - num6;
			bool flag = true;
			int num10 = paintEngine.brushSize >> 1;
			int num11 = 0;
			while (flag)
			{
				num11++;
				if (num11 > num10)
				{
					num11 = 0;
					DrawCircle(num, num2, paintEngine);
				}
				if (num == num3 && num2 == num4)
				{
					flag = false;
				}
				int num12 = 2 * num9;
				if (num12 > -num6)
				{
					num9 -= num6;
					num += num7;
				}
				if (num12 < num5)
				{
					num9 += num5;
					num2 += num8;
				}
			}
		}

		public static void DrawLineWithPattern(Vector2 start, Vector2 end, AdvancedMobilePaint paintEngine)
		{
			int num = (int)start.x;
			int num2 = (int)start.y;
			int num3 = (int)end.x;
			int num4 = (int)end.y;
			int num5 = Mathf.Abs(num3 - num);
			int num6 = Mathf.Abs(num4 - num2);
			int num7 = (num < num3) ? 1 : (-1);
			int num8 = (num2 < num4) ? 1 : (-1);
			int num9 = num5 - num6;
			bool flag = true;
			int num10 = paintEngine.brushSize >> 1;
			int num11 = 0;
			while (flag)
			{
				num11++;
				if (num11 > num10)
				{
					num11 = 0;
					DrawPatternCircle(num, num2, paintEngine);
				}
				if (num == num3 && num2 == num4)
				{
					flag = false;
				}
				int num12 = 2 * num9;
				if (num12 > -num6)
				{
					num9 -= num6;
					num += num7;
				}
				if (num12 < num5)
				{
					num9 += num5;
					num2 += num8;
				}
			}
		}
	}
}
