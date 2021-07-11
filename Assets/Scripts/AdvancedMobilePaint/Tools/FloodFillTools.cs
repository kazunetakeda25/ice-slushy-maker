using System.Collections.Generic;
using UnityEngine;

namespace AdvancedMobilePaint.Tools
{
	public static class FloodFillTools
	{
		public static void FloodFillMaskOnly(int x, int y, AdvancedMobilePaint paintEngine)
		{
			UnityEngine.Debug.Log("FloodFillMaskOnly");
			byte b = paintEngine.maskPixels[(paintEngine.texWidth * y + x) * 4];
			byte b2 = paintEngine.maskPixels[(paintEngine.texWidth * y + x) * 4 + 1];
			byte b3 = paintEngine.maskPixels[(paintEngine.texWidth * y + x) * 4 + 2];
			byte b4 = paintEngine.maskPixels[(paintEngine.texWidth * y + x) * 4 + 3];
			if (!paintEngine.canDrawOnBlack && b4 == 0)
			{
				return;
			}
			Queue<int> queue = new Queue<int>();
			Queue<int> queue2 = new Queue<int>();
			queue.Enqueue(x);
			queue2.Enqueue(y);
			int num = 0;
			paintEngine.lockMaskPixels = new byte[paintEngine.texWidth * paintEngine.texHeight * 4];
			while (queue.Count > 0)
			{
				int num2 = queue.Dequeue();
				int num3 = queue2.Dequeue();
				if (num3 - 1 > -1)
				{
					num = (paintEngine.texWidth * (num3 - 1) + num2) * 4;
					if (paintEngine.lockMaskPixels[num] == 0 && paintEngine.maskPixels[num] == b && paintEngine.maskPixels[num + 1] == b2 && paintEngine.maskPixels[num + 2] == b3 && paintEngine.maskPixels[num + 3] == b4)
					{
						queue.Enqueue(num2);
						queue2.Enqueue(num3 - 1);
						paintEngine.lockMaskPixels[num] = 1;
						paintEngine.pixels[num] = paintEngine.paintColor.r;
						paintEngine.pixels[num + 1] = paintEngine.paintColor.g;
						paintEngine.pixels[num + 2] = paintEngine.paintColor.b;
						paintEngine.pixels[num + 3] = paintEngine.paintColor.a;
					}
				}
				if (num2 + 1 < paintEngine.texWidth)
				{
					num = (paintEngine.texWidth * num3 + num2 + 1) * 4;
					if (paintEngine.lockMaskPixels[num] == 0 && paintEngine.maskPixels[num] == b && paintEngine.maskPixels[num + 1] == b2 && paintEngine.maskPixels[num + 2] == b3 && paintEngine.maskPixels[num + 3] == b4)
					{
						queue.Enqueue(num2 + 1);
						queue2.Enqueue(num3);
						paintEngine.lockMaskPixels[num] = 1;
						paintEngine.pixels[num] = paintEngine.paintColor.r;
						paintEngine.pixels[num + 1] = paintEngine.paintColor.g;
						paintEngine.pixels[num + 2] = paintEngine.paintColor.b;
						paintEngine.pixels[num + 3] = paintEngine.paintColor.a;
					}
				}
				if (num2 - 1 > -1)
				{
					num = (paintEngine.texWidth * num3 + num2 - 1) * 4;
					if (paintEngine.lockMaskPixels[num] == 0 && paintEngine.maskPixels[num] == b && paintEngine.maskPixels[num + 1] == b2 && paintEngine.maskPixels[num + 2] == b3 && paintEngine.maskPixels[num + 3] == b4)
					{
						queue.Enqueue(num2 - 1);
						queue2.Enqueue(num3);
						paintEngine.lockMaskPixels[num] = 1;
						paintEngine.pixels[num] = paintEngine.paintColor.r;
						paintEngine.pixels[num + 1] = paintEngine.paintColor.g;
						paintEngine.pixels[num + 2] = paintEngine.paintColor.b;
						paintEngine.pixels[num + 3] = paintEngine.paintColor.a;
					}
				}
				if (num3 + 1 < paintEngine.texHeight)
				{
					num = (paintEngine.texWidth * (num3 + 1) + num2) * 4;
					if (paintEngine.lockMaskPixels[num] == 0 && paintEngine.maskPixels[num] == b && paintEngine.maskPixels[num + 1] == b2 && paintEngine.maskPixels[num + 2] == b3 && paintEngine.maskPixels[num + 3] == b4)
					{
						queue.Enqueue(num2);
						queue2.Enqueue(num3 + 1);
						paintEngine.lockMaskPixels[num] = 1;
						paintEngine.pixels[num] = paintEngine.paintColor.r;
						paintEngine.pixels[num + 1] = paintEngine.paintColor.g;
						paintEngine.pixels[num + 2] = paintEngine.paintColor.b;
						paintEngine.pixels[num + 3] = paintEngine.paintColor.a;
					}
				}
			}
		}

		public static void FloodFill(int x, int y, AdvancedMobilePaint paintEngine)
		{
			UnityEngine.Debug.Log("FloodFill");
			byte b = paintEngine.pixels[(paintEngine.texWidth * y + x) * 4];
			byte b2 = paintEngine.pixels[(paintEngine.texWidth * y + x) * 4 + 1];
			byte b3 = paintEngine.pixels[(paintEngine.texWidth * y + x) * 4 + 2];
			byte b4 = paintEngine.pixels[(paintEngine.texWidth * y + x) * 4 + 3];
			if ((!paintEngine.canDrawOnBlack && b == 0 && b2 == 0 && b3 == 0 && b4 != 0) || (paintEngine.brushMode != BrushProperties.Pattern && paintEngine.paintColor.r == b && paintEngine.paintColor.g == b2 && paintEngine.paintColor.b == b3 && paintEngine.paintColor.a == b4))
			{
				return;
			}
			Queue<int> queue = new Queue<int>();
			Queue<int> queue2 = new Queue<int>();
			queue.Enqueue(x);
			queue2.Enqueue(y);
			int num = 0;
			while (queue.Count > 0)
			{
				int num2 = queue.Dequeue();
				int num3 = queue2.Dequeue();
				if (num3 - 1 > -1)
				{
					num = (paintEngine.texWidth * (num3 - 1) + num2) * 4;
					if (paintEngine.pixels[num] == b && paintEngine.pixels[num + 1] == b2 && paintEngine.pixels[num + 2] == b3 && paintEngine.pixels[num + 3] == b4)
					{
						queue.Enqueue(num2);
						queue2.Enqueue(num3 - 1);
						paintEngine.pixels[num] = paintEngine.paintColor.r;
						paintEngine.pixels[num + 1] = paintEngine.paintColor.g;
						paintEngine.pixels[num + 2] = paintEngine.paintColor.b;
						paintEngine.pixels[num + 3] = paintEngine.paintColor.a;
					}
				}
				if (num2 + 1 < paintEngine.texWidth)
				{
					num = (paintEngine.texWidth * num3 + num2 + 1) * 4;
					if (paintEngine.pixels[num] == b && paintEngine.pixels[num + 1] == b2 && paintEngine.pixels[num + 2] == b3 && paintEngine.pixels[num + 3] == b4)
					{
						queue.Enqueue(num2 + 1);
						queue2.Enqueue(num3);
						paintEngine.pixels[num] = paintEngine.paintColor.r;
						paintEngine.pixels[num + 1] = paintEngine.paintColor.g;
						paintEngine.pixels[num + 2] = paintEngine.paintColor.b;
						paintEngine.pixels[num + 3] = paintEngine.paintColor.a;
					}
				}
				if (num2 - 1 > -1)
				{
					num = (paintEngine.texWidth * num3 + num2 - 1) * 4;
					if (paintEngine.pixels[num] == b && paintEngine.pixels[num + 1] == b2 && paintEngine.pixels[num + 2] == b3 && paintEngine.pixels[num + 3] == b4)
					{
						queue.Enqueue(num2 - 1);
						queue2.Enqueue(num3);
						paintEngine.pixels[num] = paintEngine.paintColor.r;
						paintEngine.pixels[num + 1] = paintEngine.paintColor.g;
						paintEngine.pixels[num + 2] = paintEngine.paintColor.b;
						paintEngine.pixels[num + 3] = paintEngine.paintColor.a;
					}
				}
				if (num3 + 1 < paintEngine.texHeight)
				{
					num = (paintEngine.texWidth * (num3 + 1) + num2) * 4;
					if (paintEngine.pixels[num] == b && paintEngine.pixels[num + 1] == b2 && paintEngine.pixels[num + 2] == b3 && paintEngine.pixels[num + 3] == b4)
					{
						queue.Enqueue(num2);
						queue2.Enqueue(num3 + 1);
						paintEngine.pixels[num] = paintEngine.paintColor.r;
						paintEngine.pixels[num + 1] = paintEngine.paintColor.g;
						paintEngine.pixels[num + 2] = paintEngine.paintColor.b;
						paintEngine.pixels[num + 3] = paintEngine.paintColor.a;
					}
				}
			}
		}

		public static void FloodFillMaskOnlyWithThreshold(int x, int y, AdvancedMobilePaint paintEngine)
		{
			UnityEngine.Debug.Log("FloodFillMaskOnlyWithThreshold");
			byte b = paintEngine.maskPixels[(paintEngine.texWidth * y + x) * 4];
			byte b2 = paintEngine.maskPixels[(paintEngine.texWidth * y + x) * 4 + 1];
			byte b3 = paintEngine.maskPixels[(paintEngine.texWidth * y + x) * 4 + 2];
			byte b4 = paintEngine.maskPixels[(paintEngine.texWidth * y + x) * 4 + 3];
			if ((!paintEngine.canDrawOnBlack && b4 != 0) || (paintEngine.paintColor.r == b && paintEngine.paintColor.g == b2 && paintEngine.paintColor.b == b3 && paintEngine.paintColor.a == b4))
			{
				return;
			}
			Queue<int> queue = new Queue<int>();
			Queue<int> queue2 = new Queue<int>();
			queue.Enqueue(x);
			queue2.Enqueue(y);
			int num = 0;
			paintEngine.lockMaskPixels = new byte[paintEngine.texWidth * paintEngine.texHeight * 4];
			while (queue.Count > 0)
			{
				int num2 = queue.Dequeue();
				int num3 = queue2.Dequeue();
				if (num3 - 1 > -1)
				{
					num = (paintEngine.texWidth * (num3 - 1) + num2) * 4;
					if (paintEngine.lockMaskPixels[num] == 0 && paintEngine.CompareThreshold(paintEngine.maskPixels[num], b) && paintEngine.CompareThreshold(paintEngine.maskPixels[num + 1], b2) && paintEngine.CompareThreshold(paintEngine.maskPixels[num + 2], b3) && paintEngine.CompareThreshold(paintEngine.maskPixels[num + 3], b4))
					{
						queue.Enqueue(num2);
						queue2.Enqueue(num3 - 1);
						paintEngine.lockMaskPixels[num] = 1;
						paintEngine.pixels[num] = paintEngine.paintColor.r;
						paintEngine.pixels[num + 1] = paintEngine.paintColor.g;
						paintEngine.pixels[num + 2] = paintEngine.paintColor.b;
						paintEngine.pixels[num + 3] = paintEngine.paintColor.a;
					}
				}
				if (num2 + 1 < paintEngine.texWidth)
				{
					num = (paintEngine.texWidth * num3 + num2 + 1) * 4;
					if (paintEngine.lockMaskPixels[num] == 0 && paintEngine.CompareThreshold(paintEngine.maskPixels[num], b) && paintEngine.CompareThreshold(paintEngine.maskPixels[num + 1], b2) && paintEngine.CompareThreshold(paintEngine.maskPixels[num + 2], b3) && paintEngine.CompareThreshold(paintEngine.maskPixels[num + 3], b4))
					{
						queue.Enqueue(num2 + 1);
						queue2.Enqueue(num3);
						paintEngine.lockMaskPixels[num] = 1;
						paintEngine.pixels[num] = paintEngine.paintColor.r;
						paintEngine.pixels[num + 1] = paintEngine.paintColor.g;
						paintEngine.pixels[num + 2] = paintEngine.paintColor.b;
						paintEngine.pixels[num + 3] = paintEngine.paintColor.a;
					}
				}
				if (num2 - 1 > -1)
				{
					num = (paintEngine.texWidth * num3 + num2 - 1) * 4;
					if (paintEngine.lockMaskPixels[num] == 0 && paintEngine.CompareThreshold(paintEngine.maskPixels[num], b) && paintEngine.CompareThreshold(paintEngine.maskPixels[num + 1], b2) && paintEngine.CompareThreshold(paintEngine.maskPixels[num + 2], b3) && paintEngine.CompareThreshold(paintEngine.maskPixels[num + 3], b4))
					{
						queue.Enqueue(num2 - 1);
						queue2.Enqueue(num3);
						paintEngine.lockMaskPixels[num] = 1;
						paintEngine.pixels[num] = paintEngine.paintColor.r;
						paintEngine.pixels[num + 1] = paintEngine.paintColor.g;
						paintEngine.pixels[num + 2] = paintEngine.paintColor.b;
						paintEngine.pixels[num + 3] = paintEngine.paintColor.a;
					}
				}
				if (num3 + 1 < paintEngine.texHeight)
				{
					num = (paintEngine.texWidth * (num3 + 1) + num2) * 4;
					if (paintEngine.lockMaskPixels[num] == 0 && paintEngine.CompareThreshold(paintEngine.maskPixels[num], b) && paintEngine.CompareThreshold(paintEngine.maskPixels[num + 1], b2) && paintEngine.CompareThreshold(paintEngine.maskPixels[num + 2], b3) && paintEngine.CompareThreshold(paintEngine.maskPixels[num + 3], b4))
					{
						queue.Enqueue(num2);
						queue2.Enqueue(num3 + 1);
						paintEngine.lockMaskPixels[num] = 1;
						paintEngine.pixels[num] = paintEngine.paintColor.r;
						paintEngine.pixels[num + 1] = paintEngine.paintColor.g;
						paintEngine.pixels[num + 2] = paintEngine.paintColor.b;
						paintEngine.pixels[num + 3] = paintEngine.paintColor.a;
					}
				}
			}
		}

		public static void FloodFillWithTreshold(int x, int y, AdvancedMobilePaint paintEngine)
		{
			UnityEngine.Debug.Log("FloodFillWithThreshold");
			byte b = paintEngine.pixels[(paintEngine.texWidth * y + x) * 4];
			byte b2 = paintEngine.pixels[(paintEngine.texWidth * y + x) * 4 + 1];
			byte b3 = paintEngine.pixels[(paintEngine.texWidth * y + x) * 4 + 2];
			byte b4 = paintEngine.pixels[(paintEngine.texWidth * y + x) * 4 + 3];
			if ((!paintEngine.canDrawOnBlack && b == 0 && b2 == 0 && b3 == 0 && b4 != 0) || (paintEngine.paintColor.r == b && paintEngine.paintColor.g == b2 && paintEngine.paintColor.b == b3 && paintEngine.paintColor.a == b4))
			{
				return;
			}
			Queue<int> queue = new Queue<int>();
			Queue<int> queue2 = new Queue<int>();
			queue.Enqueue(x);
			queue2.Enqueue(y);
			int num = 0;
			paintEngine.lockMaskPixels = new byte[paintEngine.texWidth * paintEngine.texHeight * 4];
			while (queue.Count > 0)
			{
				int num2 = queue.Dequeue();
				int num3 = queue2.Dequeue();
				if (num3 - 1 > -1)
				{
					num = (paintEngine.texWidth * (num3 - 1) + num2) * 4;
					if (paintEngine.lockMaskPixels[num] == 0 && paintEngine.CompareThreshold(paintEngine.pixels[num], b) && paintEngine.CompareThreshold(paintEngine.pixels[num + 1], b2) && paintEngine.CompareThreshold(paintEngine.pixels[num + 2], b3) && paintEngine.CompareThreshold(paintEngine.pixels[num + 3], b4))
					{
						queue.Enqueue(num2);
						queue2.Enqueue(num3 - 1);
						paintEngine.lockMaskPixels[num] = 1;
						paintEngine.pixels[num] = paintEngine.paintColor.r;
						paintEngine.pixels[num + 1] = paintEngine.paintColor.g;
						paintEngine.pixels[num + 2] = paintEngine.paintColor.b;
						paintEngine.pixels[num + 3] = paintEngine.paintColor.a;
					}
				}
				if (num2 + 1 < paintEngine.texWidth)
				{
					num = (paintEngine.texWidth * num3 + num2 + 1) * 4;
					if (paintEngine.lockMaskPixels[num] == 0 && paintEngine.CompareThreshold(paintEngine.pixels[num], b) && paintEngine.CompareThreshold(paintEngine.pixels[num + 1], b2) && paintEngine.CompareThreshold(paintEngine.pixels[num + 2], b3) && paintEngine.CompareThreshold(paintEngine.pixels[num + 3], b4))
					{
						queue.Enqueue(num2 + 1);
						queue2.Enqueue(num3);
						paintEngine.lockMaskPixels[num] = 1;
						paintEngine.pixels[num] = paintEngine.paintColor.r;
						paintEngine.pixels[num + 1] = paintEngine.paintColor.g;
						paintEngine.pixels[num + 2] = paintEngine.paintColor.b;
						paintEngine.pixels[num + 3] = paintEngine.paintColor.a;
					}
				}
				if (num2 - 1 > -1)
				{
					num = (paintEngine.texWidth * num3 + num2 - 1) * 4;
					if (paintEngine.lockMaskPixels[num] == 0 && paintEngine.CompareThreshold(paintEngine.pixels[num], b) && paintEngine.CompareThreshold(paintEngine.pixels[num + 1], b2) && paintEngine.CompareThreshold(paintEngine.pixels[num + 2], b3) && paintEngine.CompareThreshold(paintEngine.pixels[num + 3], b4))
					{
						queue.Enqueue(num2 - 1);
						queue2.Enqueue(num3);
						paintEngine.lockMaskPixels[num] = 1;
						paintEngine.pixels[num] = paintEngine.paintColor.r;
						paintEngine.pixels[num + 1] = paintEngine.paintColor.g;
						paintEngine.pixels[num + 2] = paintEngine.paintColor.b;
						paintEngine.pixels[num + 3] = paintEngine.paintColor.a;
					}
				}
				if (num3 + 1 < paintEngine.texHeight)
				{
					num = (paintEngine.texWidth * (num3 + 1) + num2) * 4;
					if (paintEngine.lockMaskPixels[num] == 0 && paintEngine.CompareThreshold(paintEngine.pixels[num], b) && paintEngine.CompareThreshold(paintEngine.pixels[num + 1], b2) && paintEngine.CompareThreshold(paintEngine.pixels[num + 2], b3) && paintEngine.CompareThreshold(paintEngine.pixels[num + 3], b4))
					{
						queue.Enqueue(num2);
						queue2.Enqueue(num3 + 1);
						paintEngine.lockMaskPixels[num] = 1;
						paintEngine.pixels[num] = paintEngine.paintColor.r;
						paintEngine.pixels[num + 1] = paintEngine.paintColor.g;
						paintEngine.pixels[num + 2] = paintEngine.paintColor.b;
						paintEngine.pixels[num + 3] = paintEngine.paintColor.a;
					}
				}
			}
		}

		public static void LockAreaFill(int x, int y, AdvancedMobilePaint paintEngine)
		{
			UnityEngine.Debug.Log("LockAreaFill");
			byte b = paintEngine.pixels[(paintEngine.texWidth * y + x) * 4];
			byte b2 = paintEngine.pixels[(paintEngine.texWidth * y + x) * 4 + 1];
			byte b3 = paintEngine.pixels[(paintEngine.texWidth * y + x) * 4 + 2];
			byte b4 = paintEngine.pixels[(paintEngine.texWidth * y + x) * 4 + 3];
			if (!paintEngine.canDrawOnBlack && b == 0 && b2 == 0 && b3 == 0 && b4 != 0)
			{
				return;
			}
			Queue<int> queue = new Queue<int>();
			Queue<int> queue2 = new Queue<int>();
			queue.Enqueue(x);
			queue2.Enqueue(y);
			int num = 0;
			paintEngine.lockMaskPixels = new byte[paintEngine.texWidth * paintEngine.texHeight * 4];
			while (queue.Count > 0)
			{
				int num2 = queue.Dequeue();
				int num3 = queue2.Dequeue();
				if (num3 - 1 > -1)
				{
					num = (paintEngine.texWidth * (num3 - 1) + num2) * 4;
					if (paintEngine.lockMaskPixels[num] == 0 && (paintEngine.pixels[num] == b || paintEngine.pixels[num] == paintEngine.paintColor.r) && (paintEngine.pixels[num + 1] == b2 || paintEngine.pixels[num + 1] == paintEngine.paintColor.g) && (paintEngine.pixels[num + 2] == b3 || paintEngine.pixels[num + 2] == paintEngine.paintColor.b) && (paintEngine.pixels[num + 3] == b4 || paintEngine.pixels[num + 3] == paintEngine.paintColor.a))
					{
						queue.Enqueue(num2);
						queue2.Enqueue(num3 - 1);
						paintEngine.lockMaskPixels[num] = 1;
					}
				}
				if (num2 + 1 < paintEngine.texWidth)
				{
					num = (paintEngine.texWidth * num3 + num2 + 1) * 4;
					if (paintEngine.lockMaskPixels[num] == 0 && (paintEngine.pixels[num] == b || paintEngine.pixels[num] == paintEngine.paintColor.r) && (paintEngine.pixels[num + 1] == b2 || paintEngine.pixels[num + 1] == paintEngine.paintColor.g) && (paintEngine.pixels[num + 2] == b3 || paintEngine.pixels[num + 2] == paintEngine.paintColor.b) && (paintEngine.pixels[num + 3] == b4 || paintEngine.pixels[num + 3] == paintEngine.paintColor.a))
					{
						queue.Enqueue(num2 + 1);
						queue2.Enqueue(num3);
						paintEngine.lockMaskPixels[num] = 1;
					}
				}
				if (num2 - 1 > -1)
				{
					num = (paintEngine.texWidth * num3 + num2 - 1) * 4;
					if (paintEngine.lockMaskPixels[num] == 0 && (paintEngine.pixels[num] == b || paintEngine.pixels[num] == paintEngine.paintColor.r) && (paintEngine.pixels[num + 1] == b2 || paintEngine.pixels[num + 1] == paintEngine.paintColor.g) && (paintEngine.pixels[num + 2] == b3 || paintEngine.pixels[num + 2] == paintEngine.paintColor.b) && (paintEngine.pixels[num + 3] == b4 || paintEngine.pixels[num + 3] == paintEngine.paintColor.a))
					{
						queue.Enqueue(num2 - 1);
						queue2.Enqueue(num3);
						paintEngine.lockMaskPixels[num] = 1;
					}
				}
				if (num3 + 1 < paintEngine.texHeight)
				{
					num = (paintEngine.texWidth * (num3 + 1) + num2) * 4;
					if (paintEngine.lockMaskPixels[num] == 0 && (paintEngine.pixels[num] == b || paintEngine.pixels[num] == paintEngine.paintColor.r) && (paintEngine.pixels[num + 1] == b2 || paintEngine.pixels[num + 1] == paintEngine.paintColor.g) && (paintEngine.pixels[num + 2] == b3 || paintEngine.pixels[num + 2] == paintEngine.paintColor.b) && (paintEngine.pixels[num + 3] == b4 || paintEngine.pixels[num + 3] == paintEngine.paintColor.a))
					{
						queue.Enqueue(num2);
						queue2.Enqueue(num3 + 1);
						paintEngine.lockMaskPixels[num] = 1;
					}
				}
			}
		}

		public static void LockAreaFillMaskOnly(int x, int y, AdvancedMobilePaint paintEngine)
		{
			UnityEngine.Debug.Log("LockAreaFillMaskOnly");
			byte b = paintEngine.maskPixels[(paintEngine.texWidth * y + x) * 4];
			byte b2 = paintEngine.maskPixels[(paintEngine.texWidth * y + x) * 4 + 1];
			byte b3 = paintEngine.maskPixels[(paintEngine.texWidth * y + x) * 4 + 2];
			byte b4 = paintEngine.maskPixels[(paintEngine.texWidth * y + x) * 4 + 3];
			if (!paintEngine.canDrawOnBlack && b == 0 && b2 == 0 && b3 == 0 && b4 != 0)
			{
				return;
			}
			Queue<int> queue = new Queue<int>();
			Queue<int> queue2 = new Queue<int>();
			queue.Enqueue(x);
			queue2.Enqueue(y);
			int num = 0;
			paintEngine.lockMaskPixels = new byte[paintEngine.texWidth * paintEngine.texHeight * 4];
			while (queue.Count > 0)
			{
				int num2 = queue.Dequeue();
				int num3 = queue2.Dequeue();
				if (num3 - 1 > -1)
				{
					num = (paintEngine.texWidth * (num3 - 1) + num2) * 4;
					if (paintEngine.lockMaskPixels[num] == 0 && (paintEngine.maskPixels[num] == b || paintEngine.maskPixels[num] == paintEngine.paintColor.r) && (paintEngine.maskPixels[num + 1] == b2 || paintEngine.maskPixels[num + 1] == paintEngine.paintColor.g) && (paintEngine.maskPixels[num + 2] == b3 || paintEngine.maskPixels[num + 2] == paintEngine.paintColor.b) && (paintEngine.maskPixels[num + 3] == b4 || paintEngine.maskPixels[num + 3] == paintEngine.paintColor.a))
					{
						queue.Enqueue(num2);
						queue2.Enqueue(num3 - 1);
						paintEngine.lockMaskPixels[num] = 1;
					}
				}
				if (num2 + 1 < paintEngine.texWidth)
				{
					num = (paintEngine.texWidth * num3 + num2 + 1) * 4;
					if (paintEngine.lockMaskPixels[num] == 0 && (paintEngine.maskPixels[num] == b || paintEngine.maskPixels[num] == paintEngine.paintColor.r) && (paintEngine.maskPixels[num + 1] == b2 || paintEngine.maskPixels[num + 1] == paintEngine.paintColor.g) && (paintEngine.maskPixels[num + 2] == b3 || paintEngine.maskPixels[num + 2] == paintEngine.paintColor.b) && (paintEngine.maskPixels[num + 3] == b4 || paintEngine.maskPixels[num + 3] == paintEngine.paintColor.a))
					{
						queue.Enqueue(num2 + 1);
						queue2.Enqueue(num3);
						paintEngine.lockMaskPixels[num] = 1;
					}
				}
				if (num2 - 1 > -1)
				{
					num = (paintEngine.texWidth * num3 + num2 - 1) * 4;
					if (paintEngine.lockMaskPixels[num] == 0 && (paintEngine.maskPixels[num] == b || paintEngine.maskPixels[num] == paintEngine.paintColor.r) && (paintEngine.maskPixels[num + 1] == b2 || paintEngine.maskPixels[num + 1] == paintEngine.paintColor.g) && (paintEngine.maskPixels[num + 2] == b3 || paintEngine.maskPixels[num + 2] == paintEngine.paintColor.b) && (paintEngine.maskPixels[num + 3] == b4 || paintEngine.maskPixels[num + 3] == paintEngine.paintColor.a))
					{
						queue.Enqueue(num2 - 1);
						queue2.Enqueue(num3);
						paintEngine.lockMaskPixels[num] = 1;
					}
				}
				if (num3 + 1 < paintEngine.texHeight)
				{
					num = (paintEngine.texWidth * (num3 + 1) + num2) * 4;
					if (paintEngine.lockMaskPixels[num] == 0 && (paintEngine.maskPixels[num] == b || paintEngine.maskPixels[num] == paintEngine.paintColor.r) && (paintEngine.maskPixels[num + 1] == b2 || paintEngine.maskPixels[num + 1] == paintEngine.paintColor.g) && (paintEngine.maskPixels[num + 2] == b3 || paintEngine.maskPixels[num + 2] == paintEngine.paintColor.b) && (paintEngine.maskPixels[num + 3] == b4 || paintEngine.maskPixels[num + 3] == paintEngine.paintColor.a))
					{
						queue.Enqueue(num2);
						queue2.Enqueue(num3 + 1);
						paintEngine.lockMaskPixels[num] = 1;
					}
				}
			}
		}

		public static void LockAreaFillWithThresholdMaskOnly(int x, int y, AdvancedMobilePaint paintEngine)
		{
			UnityEngine.Debug.Log("LockAreaFillWithThresholdMaskOnly");
			byte b = paintEngine.maskPixels[(paintEngine.texWidth * y + x) * 4];
			byte b2 = paintEngine.maskPixels[(paintEngine.texWidth * y + x) * 4 + 1];
			byte b3 = paintEngine.maskPixels[(paintEngine.texWidth * y + x) * 4 + 2];
			byte b4 = paintEngine.maskPixels[(paintEngine.texWidth * y + x) * 4 + 3];
			if (!paintEngine.canDrawOnBlack && b == 0 && b2 == 0 && b3 == 0 && b4 != 0)
			{
				return;
			}
			Queue<int> queue = new Queue<int>();
			Queue<int> queue2 = new Queue<int>();
			queue.Enqueue(x);
			queue2.Enqueue(y);
			int num = 0;
			paintEngine.lockMaskPixels = new byte[paintEngine.texWidth * paintEngine.texHeight * 4];
			while (queue.Count > 0)
			{
				int num2 = queue.Dequeue();
				int num3 = queue2.Dequeue();
				if (num3 - 1 > -1)
				{
					num = (paintEngine.texWidth * (num3 - 1) + num2) * 4;
					if (paintEngine.lockMaskPixels[num] == 0 && paintEngine.CompareThreshold(paintEngine.maskPixels[num], b) && paintEngine.CompareThreshold(paintEngine.maskPixels[num + 1], b2) && paintEngine.CompareThreshold(paintEngine.maskPixels[num + 2], b3) && paintEngine.CompareThreshold(paintEngine.maskPixels[num + 3], b4))
					{
						queue.Enqueue(num2);
						queue2.Enqueue(num3 - 1);
						paintEngine.lockMaskPixels[num] = 1;
					}
				}
				if (num2 + 1 < paintEngine.texWidth)
				{
					num = (paintEngine.texWidth * num3 + num2 + 1) * 4;
					if (paintEngine.lockMaskPixels[num] == 0 && paintEngine.CompareThreshold(paintEngine.maskPixels[num], b) && paintEngine.CompareThreshold(paintEngine.maskPixels[num + 1], b2) && paintEngine.CompareThreshold(paintEngine.maskPixels[num + 2], b3) && paintEngine.CompareThreshold(paintEngine.maskPixels[num + 3], b4))
					{
						queue.Enqueue(num2 + 1);
						queue2.Enqueue(num3);
						paintEngine.lockMaskPixels[num] = 1;
					}
				}
				if (num2 - 1 > -1)
				{
					num = (paintEngine.texWidth * num3 + num2 - 1) * 4;
					if (paintEngine.lockMaskPixels[num] == 0 && paintEngine.CompareThreshold(paintEngine.maskPixels[num], b) && paintEngine.CompareThreshold(paintEngine.maskPixels[num + 1], b2) && paintEngine.CompareThreshold(paintEngine.maskPixels[num + 2], b3) && paintEngine.CompareThreshold(paintEngine.maskPixels[num + 3], b4))
					{
						queue.Enqueue(num2 - 1);
						queue2.Enqueue(num3);
						paintEngine.lockMaskPixels[num] = 1;
					}
				}
				if (num3 + 1 < paintEngine.texHeight)
				{
					num = (paintEngine.texWidth * (num3 + 1) + num2) * 4;
					if (paintEngine.lockMaskPixels[num] == 0 && paintEngine.CompareThreshold(paintEngine.maskPixels[num], b) && paintEngine.CompareThreshold(paintEngine.maskPixels[num + 1], b2) && paintEngine.CompareThreshold(paintEngine.maskPixels[num + 2], b3) && paintEngine.CompareThreshold(paintEngine.maskPixels[num + 3], b4))
					{
						queue.Enqueue(num2);
						queue2.Enqueue(num3 + 1);
						paintEngine.lockMaskPixels[num] = 1;
					}
				}
			}
		}

		public static void LockMaskFillWithThreshold(int x, int y, AdvancedMobilePaint paintEngine)
		{
			UnityEngine.Debug.Log("LockMaskFillWithTreshold");
			byte b = paintEngine.pixels[(paintEngine.texWidth * y + x) * 4];
			byte b2 = paintEngine.pixels[(paintEngine.texWidth * y + x) * 4 + 1];
			byte b3 = paintEngine.pixels[(paintEngine.texWidth * y + x) * 4 + 2];
			byte b4 = paintEngine.pixels[(paintEngine.texWidth * y + x) * 4 + 3];
			if (!paintEngine.canDrawOnBlack && b == 0 && b2 == 0 && b3 == 0 && b4 != 0)
			{
				return;
			}
			Queue<int> queue = new Queue<int>();
			Queue<int> queue2 = new Queue<int>();
			queue.Enqueue(x);
			queue2.Enqueue(y);
			int num = 0;
			paintEngine.lockMaskPixels = new byte[paintEngine.texWidth * paintEngine.texHeight * 4];
			while (queue.Count > 0)
			{
				int num2 = queue.Dequeue();
				int num3 = queue2.Dequeue();
				if (num3 - 1 > -1)
				{
					num = (paintEngine.texWidth * (num3 - 1) + num2) * 4;
					if (paintEngine.lockMaskPixels[num] == 0 && (paintEngine.CompareThreshold(paintEngine.pixels[num], b) || paintEngine.CompareThreshold(paintEngine.pixels[num], paintEngine.paintColor.r)) && (paintEngine.CompareThreshold(paintEngine.pixels[num + 1], b2) || paintEngine.CompareThreshold(paintEngine.pixels[num + 1], paintEngine.paintColor.g)) && (paintEngine.CompareThreshold(paintEngine.pixels[num + 2], b3) || paintEngine.CompareThreshold(paintEngine.pixels[num + 2], paintEngine.paintColor.b)) && (paintEngine.CompareThreshold(paintEngine.pixels[num + 3], b4) || paintEngine.CompareThreshold(paintEngine.pixels[num + 3], paintEngine.paintColor.a)))
					{
						queue.Enqueue(num2);
						queue2.Enqueue(num3 - 1);
						paintEngine.lockMaskPixels[num] = 1;
					}
				}
				if (num2 + 1 < paintEngine.texWidth)
				{
					num = (paintEngine.texWidth * num3 + num2 + 1) * 4;
					if (paintEngine.lockMaskPixels[num] == 0 && (paintEngine.CompareThreshold(paintEngine.pixels[num], b) || paintEngine.CompareThreshold(paintEngine.pixels[num], paintEngine.paintColor.r)) && (paintEngine.CompareThreshold(paintEngine.pixels[num + 1], b2) || paintEngine.CompareThreshold(paintEngine.pixels[num + 1], paintEngine.paintColor.g)) && (paintEngine.CompareThreshold(paintEngine.pixels[num + 2], b3) || paintEngine.CompareThreshold(paintEngine.pixels[num + 2], paintEngine.paintColor.b)) && (paintEngine.CompareThreshold(paintEngine.pixels[num + 3], b4) || paintEngine.CompareThreshold(paintEngine.pixels[num + 3], paintEngine.paintColor.a)))
					{
						queue.Enqueue(num2 + 1);
						queue2.Enqueue(num3);
						paintEngine.lockMaskPixels[num] = 1;
					}
				}
				if (num2 - 1 > -1)
				{
					num = (paintEngine.texWidth * num3 + num2 - 1) * 4;
					if (paintEngine.lockMaskPixels[num] == 0 && (paintEngine.CompareThreshold(paintEngine.pixels[num], b) || paintEngine.CompareThreshold(paintEngine.pixels[num], paintEngine.paintColor.r)) && (paintEngine.CompareThreshold(paintEngine.pixels[num + 1], b2) || paintEngine.CompareThreshold(paintEngine.pixels[num + 1], paintEngine.paintColor.g)) && (paintEngine.CompareThreshold(paintEngine.pixels[num + 2], b3) || paintEngine.CompareThreshold(paintEngine.pixels[num + 2], paintEngine.paintColor.b)) && (paintEngine.CompareThreshold(paintEngine.pixels[num + 3], b4) || paintEngine.CompareThreshold(paintEngine.pixels[num + 3], paintEngine.paintColor.a)))
					{
						queue.Enqueue(num2 - 1);
						queue2.Enqueue(num3);
						paintEngine.lockMaskPixels[num] = 1;
					}
				}
				if (num3 + 1 < paintEngine.texHeight)
				{
					num = (paintEngine.texWidth * (num3 + 1) + num2) * 4;
					if (paintEngine.lockMaskPixels[num] == 0 && (paintEngine.CompareThreshold(paintEngine.pixels[num], b) || paintEngine.CompareThreshold(paintEngine.pixels[num], paintEngine.paintColor.r)) && (paintEngine.CompareThreshold(paintEngine.pixels[num + 1], b2) || paintEngine.CompareThreshold(paintEngine.pixels[num + 1], paintEngine.paintColor.g)) && (paintEngine.CompareThreshold(paintEngine.pixels[num + 2], b3) || paintEngine.CompareThreshold(paintEngine.pixels[num + 2], paintEngine.paintColor.b)) && (paintEngine.CompareThreshold(paintEngine.pixels[num + 3], b4) || paintEngine.CompareThreshold(paintEngine.pixels[num + 3], paintEngine.paintColor.a)))
					{
						queue.Enqueue(num2);
						queue2.Enqueue(num3 + 1);
						paintEngine.lockMaskPixels[num] = 1;
					}
				}
			}
		}

		public static void FloodFillAutoMaskWithThreshold(int x, int y, AdvancedMobilePaint paintEngine)
		{
			UnityEngine.Debug.Log("FloodFill");
			byte b = paintEngine.pixels[(paintEngine.texWidth * y + x) * 4];
			byte b2 = paintEngine.pixels[(paintEngine.texWidth * y + x) * 4 + 1];
			byte b3 = paintEngine.pixels[(paintEngine.texWidth * y + x) * 4 + 2];
			byte b4 = paintEngine.pixels[(paintEngine.texWidth * y + x) * 4 + 3];
			int paintThreshold = paintEngine.paintThreshold;
			if ((!paintEngine.canDrawOnBlack && b == 0 && b2 == 0 && b3 == 0 && b4 != 0) || (paintEngine.brushMode != BrushProperties.Pattern && paintEngine.paintColor.r == b && paintEngine.paintColor.g == b2 && paintEngine.paintColor.b == b3 && paintEngine.paintColor.a == b4))
			{
				return;
			}
			Queue<int> queue = new Queue<int>();
			Queue<int> queue2 = new Queue<int>();
			queue.Enqueue(x);
			queue2.Enqueue(y);
			int num = 0;
			while (queue.Count > 0)
			{
				int num2 = queue.Dequeue();
				int num3 = queue2.Dequeue();
				if (num3 - 1 > -1)
				{
					num = (paintEngine.texWidth * (num3 - 1) + num2) * 4;
					if ((paintEngine.pixels[num] >= paintThreshold || paintEngine.pixels[num + 1] >= paintThreshold || paintEngine.pixels[num + 2] >= paintThreshold) && (paintEngine.pixels[num] != paintEngine.paintColor.r || paintEngine.pixels[num + 1] != paintEngine.paintColor.g || paintEngine.pixels[num + 2] != paintEngine.paintColor.b))
					{
						queue.Enqueue(num2);
						queue2.Enqueue(num3 - 1);
						paintEngine.pixels[num] = paintEngine.paintColor.r;
						paintEngine.pixels[num + 1] = paintEngine.paintColor.g;
						paintEngine.pixels[num + 2] = paintEngine.paintColor.b;
						paintEngine.pixels[num + 3] = paintEngine.paintColor.a;
					}
				}
				if (num2 + 1 < paintEngine.texWidth)
				{
					num = (paintEngine.texWidth * num3 + num2 + 1) * 4;
					if ((paintEngine.pixels[num] >= paintThreshold || paintEngine.pixels[num + 1] >= paintThreshold || paintEngine.pixels[num + 2] >= paintThreshold) && (paintEngine.pixels[num] != paintEngine.paintColor.r || paintEngine.pixels[num + 1] != paintEngine.paintColor.g || paintEngine.pixels[num + 2] != paintEngine.paintColor.b))
					{
						queue.Enqueue(num2 + 1);
						queue2.Enqueue(num3);
						paintEngine.pixels[num] = paintEngine.paintColor.r;
						paintEngine.pixels[num + 1] = paintEngine.paintColor.g;
						paintEngine.pixels[num + 2] = paintEngine.paintColor.b;
						paintEngine.pixels[num + 3] = paintEngine.paintColor.a;
					}
				}
				if (num2 - 1 > -1)
				{
					num = (paintEngine.texWidth * num3 + num2 - 1) * 4;
					if ((paintEngine.pixels[num] >= paintThreshold || paintEngine.pixels[num + 1] >= paintThreshold || paintEngine.pixels[num + 2] >= paintThreshold) && (paintEngine.pixels[num] != paintEngine.paintColor.r || paintEngine.pixels[num + 1] != paintEngine.paintColor.g || paintEngine.pixels[num + 2] != paintEngine.paintColor.b))
					{
						queue.Enqueue(num2 - 1);
						queue2.Enqueue(num3);
						paintEngine.pixels[num] = paintEngine.paintColor.r;
						paintEngine.pixels[num + 1] = paintEngine.paintColor.g;
						paintEngine.pixels[num + 2] = paintEngine.paintColor.b;
						paintEngine.pixels[num + 3] = paintEngine.paintColor.a;
					}
				}
				if (num3 + 1 < paintEngine.texHeight)
				{
					num = (paintEngine.texWidth * (num3 + 1) + num2) * 4;
					if ((paintEngine.pixels[num] >= paintThreshold || paintEngine.pixels[num + 1] >= paintThreshold || paintEngine.pixels[num + 2] >= paintThreshold) && (paintEngine.pixels[num] != paintEngine.paintColor.r || paintEngine.pixels[num + 1] != paintEngine.paintColor.g || paintEngine.pixels[num + 2] != paintEngine.paintColor.b))
					{
						queue.Enqueue(num2);
						queue2.Enqueue(num3 + 1);
						paintEngine.pixels[num] = paintEngine.paintColor.r;
						paintEngine.pixels[num + 1] = paintEngine.paintColor.g;
						paintEngine.pixels[num + 2] = paintEngine.paintColor.b;
						paintEngine.pixels[num + 3] = paintEngine.paintColor.a;
					}
				}
			}
		}
	}
}
