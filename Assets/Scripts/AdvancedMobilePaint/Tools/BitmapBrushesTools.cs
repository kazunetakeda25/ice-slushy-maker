using UnityEngine;

namespace AdvancedMobilePaint.Tools
{
	public class BitmapBrushesTools
	{
		public static void DrawLineWithBrush(Vector2 start, Vector2 end, AdvancedMobilePaint paintEngine)
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
					DrawCustomBrush2(num, num2, paintEngine);
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

		public static void DrawCustomBrush2(int px, int py, AdvancedMobilePaint paintEngine)
		{
			int num = px - paintEngine.customBrushWidth / 2;
			int num2 = py - paintEngine.customBrushHeight / 2;
			int num3 = (paintEngine.texWidth * num2 + num) * 4;
			int num4 = 0;
			bool flag = false;
			float num5 = 0f;
			float num6 = 0f;
			int num7 = 0;
			float num8 = 1f;
			num8 = (float)(int)paintEngine.paintColor.a / 255f * paintEngine.brushAlphaStrength;
			for (int i = 0; i < paintEngine.customBrushHeight; i++)
			{
				for (int j = 0; j < paintEngine.customBrushWidth; j++)
				{
					num4 = (paintEngine.customBrushWidth * i + j) * 4;
					flag = false;
					if (num + j > paintEngine.texWidth - 2 || num + j < -1)
					{
						flag = true;
					}
					if (num3 < 0 || num3 >= paintEngine.pixels.Length)
					{
						flag = true;
					}
					if (num4 < 0 || num4 > paintEngine.customBrushBytes.Length)
					{
						flag = true;
					}
					if (!paintEngine.canDrawOnBlack && !flag && paintEngine.pixels[num3 + 3] != 0 && paintEngine.pixels[num3] == 0 && paintEngine.pixels[num3 + 1] == 0 && paintEngine.pixels[num3 + 2] == 0)
					{
						flag = true;
					}
					if (paintEngine.lockMaskPixels.Length == 0)
					{
						return;
					}
					if (paintEngine.customBrushBytes[num4 + 3] != 0 && !flag)
					{
						if (paintEngine.useCustomBrushAlpha)
						{
							if (paintEngine.useAdditiveColors)
							{
								if ((paintEngine.useLockArea && paintEngine.lockMaskPixels[num3] == 1) || !paintEngine.useLockArea)
								{
									num8 = (float)(int)paintEngine.customBrushBytes[num4 + 3] / 255f;
									switch (paintEngine.brushMode)
									{
									case BrushProperties.Clear:
										paintEngine.pixels[num3] = (byte)Mathf.Lerp((int)paintEngine.pixels[num3], (int)paintEngine.clearColor.r, num8);
										paintEngine.pixels[num3 + 1] = (byte)Mathf.Lerp((int)paintEngine.pixels[num3 + 1], (int)paintEngine.clearColor.g, num8);
										paintEngine.pixels[num3 + 2] = (byte)Mathf.Lerp((int)paintEngine.pixels[num3 + 2], (int)paintEngine.clearColor.b, num8);
										paintEngine.pixels[num3 + 3] = (byte)Mathf.Lerp((int)paintEngine.pixels[num3 + 3], (int)paintEngine.clearColor.a, num8);
										break;
									case BrushProperties.Default:
										paintEngine.pixels[num3] = (byte)Mathf.Lerp((int)paintEngine.pixels[num3], (int)paintEngine.paintColor.r, num8);
										paintEngine.pixels[num3 + 1] = (byte)Mathf.Lerp((int)paintEngine.pixels[num3 + 1], (int)paintEngine.paintColor.g, num8);
										paintEngine.pixels[num3 + 2] = (byte)Mathf.Lerp((int)paintEngine.pixels[num3 + 2], (int)paintEngine.paintColor.b, num8);
										paintEngine.pixels[num3 + 3] = (byte)Mathf.Lerp((int)paintEngine.pixels[num3 + 3], (int)paintEngine.paintColor.a, num8);
										break;
									case BrushProperties.Simple:
										paintEngine.pixels[num3] = (byte)Mathf.Lerp((int)paintEngine.pixels[num3], (int)paintEngine.customBrushBytes[num4], num8);
										paintEngine.pixels[num3 + 1] = (byte)Mathf.Lerp((int)paintEngine.pixels[num3 + 1], (int)paintEngine.customBrushBytes[num4 + 1], num8);
										paintEngine.pixels[num3 + 2] = (byte)Mathf.Lerp((int)paintEngine.pixels[num3 + 2], (int)paintEngine.customBrushBytes[num4 + 2], num8);
										paintEngine.pixels[num3 + 3] = (byte)Mathf.Lerp((int)paintEngine.pixels[num3 + 3], (int)paintEngine.customBrushBytes[num4 + 3], num8);
										break;
									case BrushProperties.Pattern:
										num5 = Mathf.Repeat((float)py + ((float)i - (float)paintEngine.customBrushHeight / 2f), paintEngine.customPatternHeight);
										num6 = Mathf.Repeat((float)px + ((float)j - (float)paintEngine.customBrushWidth / 2f), paintEngine.customPatternWidth);
										num7 = (int)Mathf.Repeat(((float)paintEngine.customPatternWidth * num6 + num5) * 4f, paintEngine.patternBrushBytes.Length);
										paintEngine.pixels[num3] = (byte)Mathf.Lerp((int)paintEngine.pixels[num3], (int)paintEngine.patternBrushBytes[num7], num8);
										paintEngine.pixels[num3 + 1] = (byte)Mathf.Lerp((int)paintEngine.pixels[num3 + 1], (int)paintEngine.patternBrushBytes[num7 + 1], num8);
										paintEngine.pixels[num3 + 2] = (byte)Mathf.Lerp((int)paintEngine.pixels[num3 + 2], (int)paintEngine.patternBrushBytes[num7 + 2], num8);
										paintEngine.pixels[num3 + 3] = (byte)Mathf.Lerp((int)paintEngine.pixels[num3 + 3], (int)paintEngine.patternBrushBytes[num7 + 3], num8);
										break;
									}
								}
							}
							else if ((paintEngine.useLockArea && paintEngine.lockMaskPixels[num3] == 1) || !paintEngine.useLockArea)
							{
								switch (paintEngine.brushMode)
								{
								case BrushProperties.Clear:
									paintEngine.pixels[num3] = paintEngine.clearColor.r;
									paintEngine.pixels[num3 + 1] = paintEngine.clearColor.g;
									paintEngine.pixels[num3 + 2] = paintEngine.clearColor.b;
									paintEngine.pixels[num3 + 3] = paintEngine.clearColor.a;
									break;
								case BrushProperties.Default:
									paintEngine.pixels[num3] = paintEngine.paintColor.r;
									paintEngine.pixels[num3 + 1] = paintEngine.paintColor.g;
									paintEngine.pixels[num3 + 2] = paintEngine.paintColor.b;
									paintEngine.pixels[num3 + 3] = paintEngine.paintColor.a;
									break;
								case BrushProperties.Simple:
									paintEngine.pixels[num3] = paintEngine.customBrushBytes[num4];
									paintEngine.pixels[num3 + 1] = paintEngine.customBrushBytes[num4 + 1];
									paintEngine.pixels[num3 + 2] = paintEngine.customBrushBytes[num4 + 2];
									paintEngine.pixels[num3 + 3] = paintEngine.customBrushBytes[num4 + 3];
									break;
								case BrushProperties.Pattern:
									num5 = Mathf.Repeat((float)py + ((float)i - (float)paintEngine.customBrushHeight / 2f), paintEngine.customPatternHeight);
									num6 = Mathf.Repeat((float)px + ((float)j - (float)paintEngine.customBrushWidth / 2f), paintEngine.customPatternWidth);
									num7 = (int)Mathf.Repeat(((float)paintEngine.customPatternWidth * num6 + num5) * 4f, paintEngine.patternBrushBytes.Length);
									paintEngine.pixels[num3] = paintEngine.patternBrushBytes[num7];
									paintEngine.pixels[num3 + 1] = paintEngine.patternBrushBytes[num7 + 1];
									paintEngine.pixels[num3 + 2] = paintEngine.patternBrushBytes[num7 + 2];
									paintEngine.pixels[num3 + 3] = paintEngine.patternBrushBytes[num7 + 3];
									break;
								}
							}
						}
						else if (paintEngine.useAdditiveColors)
						{
							if ((paintEngine.useLockArea && paintEngine.lockMaskPixels[num3] == 1) || !paintEngine.useLockArea)
							{
								switch (paintEngine.brushMode)
								{
								case BrushProperties.Clear:
									paintEngine.pixels[num3] = (byte)Mathf.Lerp((int)paintEngine.pixels[num3], (int)paintEngine.clearColor.r, num8);
									paintEngine.pixels[num3 + 1] = (byte)Mathf.Lerp((int)paintEngine.pixels[num3 + 1], (int)paintEngine.clearColor.g, num8);
									paintEngine.pixels[num3 + 2] = (byte)Mathf.Lerp((int)paintEngine.pixels[num3 + 2], (int)paintEngine.clearColor.b, num8);
									paintEngine.pixels[num3 + 3] = (byte)Mathf.Lerp((int)paintEngine.pixels[num3 + 3], (int)paintEngine.clearColor.a, num8);
									break;
								case BrushProperties.Default:
									paintEngine.pixels[num3] = (byte)Mathf.Lerp((int)paintEngine.pixels[num3], (int)paintEngine.paintColor.r, num8);
									paintEngine.pixels[num3 + 1] = (byte)Mathf.Lerp((int)paintEngine.pixels[num3 + 1], (int)paintEngine.paintColor.g, num8);
									paintEngine.pixels[num3 + 2] = (byte)Mathf.Lerp((int)paintEngine.pixels[num3 + 2], (int)paintEngine.paintColor.b, num8);
									paintEngine.pixels[num3 + 3] = (byte)Mathf.Lerp((int)paintEngine.pixels[num3 + 3], (int)paintEngine.paintColor.a, num8);
									break;
								case BrushProperties.Simple:
									paintEngine.pixels[num3] = (byte)Mathf.Lerp((int)paintEngine.pixels[num3], (int)paintEngine.customBrushBytes[num4], num8);
									paintEngine.pixels[num3 + 1] = (byte)Mathf.Lerp((int)paintEngine.pixels[num3 + 1], (int)paintEngine.customBrushBytes[num4 + 1], num8);
									paintEngine.pixels[num3 + 2] = (byte)Mathf.Lerp((int)paintEngine.pixels[num3 + 2], (int)paintEngine.customBrushBytes[num4 + 2], num8);
									paintEngine.pixels[num3 + 3] = (byte)Mathf.Lerp((int)paintEngine.pixels[num3 + 3], (int)paintEngine.customBrushBytes[num4 + 3], num8);
									break;
								case BrushProperties.Pattern:
									num5 = Mathf.Repeat((float)py + ((float)i - (float)paintEngine.customBrushHeight / 2f), paintEngine.customPatternHeight);
									num6 = Mathf.Repeat((float)px + ((float)j - (float)paintEngine.customBrushWidth / 2f), paintEngine.customPatternWidth);
									num7 = (int)Mathf.Repeat(((float)paintEngine.customPatternWidth * num6 + num5) * 4f, paintEngine.patternBrushBytes.Length);
									paintEngine.pixels[num3] = (byte)Mathf.Lerp((int)paintEngine.pixels[num3], (int)paintEngine.patternBrushBytes[num7], num8);
									paintEngine.pixels[num3 + 1] = (byte)Mathf.Lerp((int)paintEngine.pixels[num3 + 1], (int)paintEngine.patternBrushBytes[num7 + 1], num8);
									paintEngine.pixels[num3 + 2] = (byte)Mathf.Lerp((int)paintEngine.pixels[num3 + 2], (int)paintEngine.patternBrushBytes[num7 + 2], num8);
									paintEngine.pixels[num3 + 3] = (byte)Mathf.Lerp((int)paintEngine.pixels[num3 + 3], (int)paintEngine.patternBrushBytes[num7 + 3], num8);
									break;
								}
							}
						}
						else if ((paintEngine.useLockArea && paintEngine.lockMaskPixels[num3] == 1) || !paintEngine.useLockArea)
						{
							switch (paintEngine.brushMode)
							{
							case BrushProperties.Clear:
								paintEngine.pixels[num3] = paintEngine.clearColor.r;
								paintEngine.pixels[num3 + 1] = paintEngine.clearColor.g;
								paintEngine.pixels[num3 + 2] = paintEngine.clearColor.b;
								paintEngine.pixels[num3 + 3] = paintEngine.clearColor.a;
								break;
							case BrushProperties.Default:
								paintEngine.pixels[num3] = paintEngine.paintColor.r;
								paintEngine.pixels[num3 + 1] = paintEngine.paintColor.g;
								paintEngine.pixels[num3 + 2] = paintEngine.paintColor.b;
								paintEngine.pixels[num3 + 3] = paintEngine.paintColor.a;
								break;
							case BrushProperties.Simple:
								paintEngine.pixels[num3] = paintEngine.customBrushBytes[num4];
								paintEngine.pixels[num3 + 1] = paintEngine.customBrushBytes[num4 + 1];
								paintEngine.pixels[num3 + 2] = paintEngine.customBrushBytes[num4 + 2];
								paintEngine.pixels[num3 + 3] = paintEngine.customBrushBytes[num4 + 3];
								break;
							case BrushProperties.Pattern:
								num5 = Mathf.Repeat((float)py + ((float)i - (float)paintEngine.customBrushHeight / 2f), paintEngine.customPatternHeight);
								num6 = Mathf.Repeat((float)px + ((float)j - (float)paintEngine.customBrushWidth / 2f), paintEngine.customPatternWidth);
								num7 = (int)Mathf.Repeat(((float)paintEngine.customPatternWidth * num6 + num5) * 4f, paintEngine.patternBrushBytes.Length);
								paintEngine.pixels[num3] = paintEngine.patternBrushBytes[num7];
								paintEngine.pixels[num3 + 1] = paintEngine.patternBrushBytes[num7 + 1];
								paintEngine.pixels[num3 + 2] = paintEngine.patternBrushBytes[num7 + 2];
								paintEngine.pixels[num3 + 3] = paintEngine.patternBrushBytes[num7 + 3];
								break;
							}
						}
					}
					num3 += 4;
				}
				num3 = (paintEngine.texWidth * ((num2 != 0) ? (num2 + i) : (-1)) + num + 1) * 4;
			}
		}

		public static void DrawPatternCircle(int x, int y, byte[] patternSource, int size, AdvancedMobilePaint paintEngine)
		{
			if (!paintEngine.canDrawOnBlack && paintEngine.pixels[(paintEngine.texWidth * y + x) * 4] == 0 && paintEngine.pixels[(paintEngine.texWidth * y + x) * 4 + 1] == 0 && paintEngine.pixels[(paintEngine.texWidth * y + x) * 4 + 2] == 0 && paintEngine.pixels[(paintEngine.texWidth * y + x) * 4 + 3] != 0)
			{
				return;
			}
			int num = 0;
			int num2 = size * size;
			int num3 = num2 << 2;
			int num4 = size << 1;
			int num5 = 0;
			int num6 = 0;
			float num7 = 0f;
			float num8 = 0f;
			int num9 = 0;
			float num10 = 1f;
			for (int i = 0; i < num3; i++)
			{
				num5 = i % num4 - size;
				num6 = i / num4 - size;
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
						num10 = (float)(int)patternSource[num9 + 3] / 255f * paintEngine.brushAlphaStrength;
						paintEngine.pixels[num] = (byte)Mathf.Lerp((int)paintEngine.pixels[num], (int)patternSource[num9], num10);
						paintEngine.pixels[num + 1] = (byte)Mathf.Lerp((int)paintEngine.pixels[num + 1], (int)patternSource[num9 + 1], num10);
						paintEngine.pixels[num + 2] = (byte)Mathf.Lerp((int)paintEngine.pixels[num + 2], (int)patternSource[num9 + 2], num10);
						paintEngine.pixels[num + 3] = (byte)Mathf.Lerp((int)paintEngine.pixels[num + 3], (int)patternSource[num9 + 3], num10);
					}
				}
				else if (!paintEngine.useLockArea || (paintEngine.useLockArea && paintEngine.lockMaskPixels[num] == 1))
				{
					num7 = Mathf.Repeat(y + num6, paintEngine.customPatternWidth);
					num8 = Mathf.Repeat(x + num5, paintEngine.customPatternWidth);
					num9 = (int)Mathf.Repeat(((float)paintEngine.customPatternWidth * num8 + num7) * 4f, paintEngine.patternBrushBytes.Length);
					paintEngine.pixels[num] = patternSource[num9];
					paintEngine.pixels[num + 1] = patternSource[num9 + 1];
					paintEngine.pixels[num + 2] = patternSource[num9 + 2];
					paintEngine.pixels[num + 3] = patternSource[num9 + 3];
				}
			}
		}

		public static void DrawLineBrush(Vector2 start, Vector2 end, int size, bool isPattern, byte[] patternSource, AdvancedMobilePaint paintEngine)
		{
			int num = (int)start.x;
			int num2 = (int)start.y;
			int num3 = (int)end.x;
			int num4 = (int)end.y;
			int num5 = num4 - num2;
			int num6 = num3 - num;
			int num7 = 0;
			int num8 = 0;
			int num9 = 0;
			int num10 = 0;
			int num11;
			if (num5 < 0)
			{
				num5 = -num5;
				num11 = -1;
			}
			else
			{
				num11 = 1;
			}
			int num12;
			if (num6 < 0)
			{
				num6 = -num6;
				num12 = -1;
			}
			else
			{
				num12 = 1;
			}
			num5 <<= 1;
			num6 <<= 1;
			float num13 = 0f;
			if (num6 > num5)
			{
				num13 = num5 - (num6 >> 1);
				while (Mathf.Abs(num - num3) > 1)
				{
					if (num13 >= 0f)
					{
						num2 += num11;
						num13 -= (float)num6;
					}
					num += num12;
					num13 += (float)num5;
					if (num < paintEngine.texWidth && num >= 0 && num2 < paintEngine.texHeight && num2 >= 0)
					{
						num7 = paintEngine.texWidth * 4 * num2 + num * 4;
						if (!isPattern)
						{
							paintEngine.pixels[num7] = paintEngine.paintColor.r;
							paintEngine.pixels[num7 + 1] = paintEngine.paintColor.g;
							paintEngine.pixels[num7 + 2] = paintEngine.paintColor.b;
							paintEngine.pixels[num7 + 3] = paintEngine.paintColor.a;
						}
						else
						{
							float num14 = Mathf.Repeat(num2, paintEngine.customPatternHeight);
							float num15 = Mathf.Repeat(num, paintEngine.customPatternWidth);
							num10 = (int)Mathf.Repeat(((float)paintEngine.customPatternWidth * num15 + num14) * 4f, patternSource.Length);
							paintEngine.pixels[num7] = patternSource[num10];
							paintEngine.pixels[num7 + 1] = patternSource[num10 + 1];
							paintEngine.pixels[num7 + 2] = patternSource[num10 + 2];
							paintEngine.pixels[num7 + 3] = patternSource[num10 + 3];
						}
					}
					for (int i = 0; i < size; i++)
					{
						num8 = num2 - size / 2 + i;
						num7 = paintEngine.texWidth * 4 * num8 + num * 4;
						if (num < paintEngine.texWidth && num >= 0 && num8 < paintEngine.texHeight && num8 >= 0)
						{
							if (!isPattern)
							{
								paintEngine.pixels[num7] = paintEngine.paintColor.r;
								paintEngine.pixels[num7 + 1] = paintEngine.paintColor.g;
								paintEngine.pixels[num7 + 2] = paintEngine.paintColor.b;
								paintEngine.pixels[num7 + 3] = paintEngine.paintColor.a;
							}
							else
							{
								float num16 = Mathf.Repeat(num8, paintEngine.customPatternHeight);
								float num17 = Mathf.Repeat(num, paintEngine.customPatternWidth);
								num10 = (int)Mathf.Repeat(((float)paintEngine.customPatternWidth * num17 + num16) * 4f, patternSource.Length);
								paintEngine.pixels[num7] = patternSource[num10];
								paintEngine.pixels[num7 + 1] = patternSource[num10 + 1];
								paintEngine.pixels[num7 + 2] = patternSource[num10 + 2];
								paintEngine.pixels[num7 + 3] = patternSource[num10 + 3];
							}
						}
					}
				}
				return;
			}
			num13 = num6 - (num5 >> 1);
			while (Mathf.Abs(num2 - num4) > 1)
			{
				if (num13 >= 0f)
				{
					num += num12;
					num13 -= (float)num5;
				}
				num2 += num11;
				num13 += (float)num6;
				if (num < paintEngine.texWidth && num >= 0 && num2 < paintEngine.texHeight && num2 >= 0)
				{
					num7 = paintEngine.texWidth * 4 * num2 + num * 4;
					if (!isPattern)
					{
						paintEngine.pixels[num7] = paintEngine.paintColor.r;
						paintEngine.pixels[num7 + 1] = paintEngine.paintColor.g;
						paintEngine.pixels[num7 + 2] = paintEngine.paintColor.b;
						paintEngine.pixels[num7 + 3] = paintEngine.paintColor.a;
					}
					else
					{
						float num18 = Mathf.Repeat(num2, paintEngine.customPatternHeight);
						float num19 = Mathf.Repeat(num, paintEngine.customPatternWidth);
						num10 = (int)Mathf.Repeat(((float)paintEngine.customPatternWidth * num19 + num18) * 4f, patternSource.Length);
						paintEngine.pixels[num7] = patternSource[num10];
						paintEngine.pixels[num7 + 1] = patternSource[num10 + 1];
						paintEngine.pixels[num7 + 2] = patternSource[num10 + 2];
						paintEngine.pixels[num7 + 3] = patternSource[num10 + 3];
					}
				}
				for (int j = 0; j < size; j++)
				{
					num9 = num - size / 2 + j;
					num7 = paintEngine.texWidth * 4 * num2 + num9 * 4;
					if (num9 < paintEngine.texWidth && num9 >= 0 && num2 < paintEngine.texHeight && num2 >= 0)
					{
						if (!isPattern)
						{
							paintEngine.pixels[num7] = paintEngine.paintColor.r;
							paintEngine.pixels[num7 + 1] = paintEngine.paintColor.g;
							paintEngine.pixels[num7 + 2] = paintEngine.paintColor.b;
							paintEngine.pixels[num7 + 3] = paintEngine.paintColor.a;
						}
						else
						{
							float num20 = Mathf.Repeat(num2, paintEngine.customPatternHeight);
							float num21 = Mathf.Repeat(num9, paintEngine.customPatternWidth);
							num10 = (int)Mathf.Repeat(((float)paintEngine.customPatternWidth * num21 + num20) * 4f, patternSource.Length);
							paintEngine.pixels[num7] = patternSource[num10];
							paintEngine.pixels[num7 + 1] = patternSource[num10 + 1];
							paintEngine.pixels[num7 + 2] = patternSource[num10 + 2];
							paintEngine.pixels[num7 + 3] = patternSource[num10 + 3];
						}
					}
				}
			}
		}
	}
}
