using System.Threading;
using UnityEngine;

public class TextureScale
{
	public class ThreadData
	{
		public int start;

		public int end;

		public ThreadData(int s, int e)
		{
			start = s;
			end = e;
		}
	}

	private static Color[] texColors;

	private static Color[] newColors;

	private static int w;

	private static float ratioX;

	private static float ratioY;

	private static int w2;

	private static int finishCount;

	private static Mutex mutex;

	public static void Point(Texture2D tex, int newWidth, int newHeight)
	{
		ThreadedScale(tex, newWidth, newHeight, useBilinear: false);
	}

	public static void Bilinear(Texture2D tex, int newWidth, int newHeight)
	{
		ThreadedScale(tex, newWidth, newHeight, useBilinear: true);
	}

	private static void ThreadedScale(Texture2D tex, int newWidth, int newHeight, bool useBilinear)
	{
		texColors = tex.GetPixels();
		newColors = new Color[newWidth * newHeight];
		if (useBilinear)
		{
			ratioX = 1f / ((float)newWidth / (float)(tex.width - 1));
			ratioY = 1f / ((float)newHeight / (float)(tex.height - 1));
		}
		else
		{
			ratioX = (float)tex.width / (float)newWidth;
			ratioY = (float)tex.height / (float)newHeight;
		}
		w = tex.width;
		w2 = newWidth;
		int num = Mathf.Min(SystemInfo.processorCount, newHeight);
		int num2 = newHeight / num;
		finishCount = 0;
		if (mutex == null)
		{
			mutex = new Mutex(initiallyOwned: false);
		}
		if (num > 1)
		{
			int num3 = 0;
			ThreadData parameter;
			for (num3 = 0; num3 < num - 1; num3++)
			{
				parameter = new ThreadData(num2 * num3, num2 * (num3 + 1));
				ParameterizedThreadStart start = (!useBilinear) ? new ParameterizedThreadStart(PointScale) : new ParameterizedThreadStart(BilinearScale);
				Thread thread = new Thread(start);
				thread.Start(parameter);
			}
			parameter = new ThreadData(num2 * num3, newHeight);
			if (useBilinear)
			{
				BilinearScale(parameter);
			}
			else
			{
				PointScale(parameter);
			}
			while (finishCount < num)
			{
				Thread.Sleep(1);
			}
		}
		else
		{
			ThreadData obj = new ThreadData(0, newHeight);
			if (useBilinear)
			{
				BilinearScale(obj);
			}
			else
			{
				PointScale(obj);
			}
		}
		tex.Resize(newWidth, newHeight);
		tex.SetPixels(newColors);
		tex.Apply();
	}

	public static void BilinearScale(object obj)
	{
		ThreadData threadData = (ThreadData)obj;
		for (int i = threadData.start; i < threadData.end; i++)
		{
			int num = (int)Mathf.Floor((float)i * ratioY);
			int num2 = num * w;
			int num3 = (num + 1) * w;
			int num4 = i * w2;
			for (int j = 0; j < w2; j++)
			{
				int num5 = (int)Mathf.Floor((float)j * ratioX);
				float value = (float)j * ratioX - (float)num5;
				newColors[num4 + j] = ColorLerpUnclamped(ColorLerpUnclamped(texColors[num2 + num5], texColors[num2 + num5 + 1], value), ColorLerpUnclamped(texColors[num3 + num5], texColors[num3 + num5 + 1], value), (float)i * ratioY - (float)num);
			}
		}
		mutex.WaitOne();
		finishCount++;
		mutex.ReleaseMutex();
	}

	public static void PointScale(object obj)
	{
		ThreadData threadData = (ThreadData)obj;
		for (int i = threadData.start; i < threadData.end; i++)
		{
			int num = (int)(ratioY * (float)i) * w;
			int num2 = i * w2;
			for (int j = 0; j < w2; j++)
			{
				newColors[num2 + j] = texColors[(int)((float)num + ratioX * (float)j)];
			}
		}
		mutex.WaitOne();
		finishCount++;
		mutex.ReleaseMutex();
	}

	private static Color ColorLerpUnclamped(Color c1, Color c2, float value)
	{
		return new Color(c1.r + (c2.r - c1.r) * value, c1.g + (c2.g - c1.g) * value, c1.b + (c2.b - c1.b) * value, c1.a + (c2.a - c1.a) * value);
	}
}
