using System;
using UnityEngine;

namespace AdvancedMobilePaint
{
	public class PaintUtils
	{
		public static Texture2D RotateTexture(Texture2D tex, float angle)
		{
			UnityEngine.Debug.Log("rotating");
			Texture2D texture2D = new Texture2D(tex.width, tex.height, tex.format, mipChain: false);
			int width = tex.width;
			int height = tex.height;
			float num = rot_x(angle, (float)(-width) / 2f, (float)(-height) / 2f) + (float)width / 2f;
			float num2 = rot_y(angle, (float)(-width) / 2f, (float)(-height) / 2f) + (float)height / 2f;
			float num3 = rot_x(angle, 1f, 0f);
			float num4 = rot_y(angle, 1f, 0f);
			float num5 = rot_x(angle, 0f, 1f);
			float num6 = rot_y(angle, 0f, 1f);
			float num7 = num;
			float num8 = num2;
			Color32[] pixels = tex.GetPixels32(0);
			Color32[] array = new Color32[pixels.Length];
			int num9 = 0;
			int num10 = 0;
			for (int i = 0; i < tex.width; i++)
			{
				float num11 = num7;
				float num12 = num8;
				for (int j = 0; j < tex.height; j++)
				{
					num11 += num3;
					num12 += num4;
					num9 = (int)Mathf.Floor(num11);
					num10 = (int)Mathf.Floor(num12);
					Color c = (num9 < tex.width && num9 >= 0 && num10 < tex.height && num10 >= 0) ? ((Color)pixels[num9 * width + num10]) : Color.clear;
					array[(int)Mathf.Floor(i) * width + (int)Mathf.Floor(j)] = c;
				}
				num7 += num5;
				num8 += num6;
			}
			texture2D.SetPixels32(array);
			texture2D.Apply();
			return texture2D;
		}

		private static float rot_x(float angle, float x, float y)
		{
			float num = Mathf.Cos(angle / 180f * (float)Math.PI);
			float num2 = Mathf.Sin(angle / 180f * (float)Math.PI);
			return x * num + y * (0f - num2);
		}

		private static float rot_y(float angle, float x, float y)
		{
			float num = Mathf.Cos(angle / 180f * (float)Math.PI);
			float num2 = Mathf.Sin(angle / 180f * (float)Math.PI);
			return x * num2 + y * num;
		}

		public static Texture2D ReadUnreadableTexture(Texture2D texture)
		{
			RenderTexture temporary = RenderTexture.GetTemporary(texture.width, texture.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
			Graphics.Blit(texture, temporary);
			RenderTexture active = RenderTexture.active;
			RenderTexture.active = temporary;
			Texture2D texture2D = new Texture2D(texture.width, texture.height);
			texture2D.ReadPixels(new Rect(0f, 0f, temporary.width, temporary.height), 0, 0);
			texture2D.Apply();
			RenderTexture.active = active;
			RenderTexture.ReleaseTemporary(temporary);
			UnityEngine.Debug.Log("READ DONE!");
			return texture2D;
		}

		public static Texture2D ConvertSpriteToTexture2D(Sprite sr)
		{
			Texture2D texture2D = new Texture2D((int)sr.textureRect.width, (int)sr.textureRect.height, TextureFormat.ARGB32, mipChain: false);
			Color[] pixels = sr.texture.GetPixels((int)sr.textureRect.x, (int)sr.textureRect.y, (int)sr.textureRect.width, (int)sr.textureRect.height, 0);
			texture2D.SetPixels(pixels, 0);
			texture2D.Apply(updateMipmaps: false, makeNoLongerReadable: false);
			return texture2D;
		}

		public static Texture2D GenerateDrawingMaskBasedOnTexture(Texture2D source)
		{
			Texture2D blackTexture = Texture2D.blackTexture;
			blackTexture.Resize(source.width, source.height, TextureFormat.ARGB32, hasMipMap: false);
			blackTexture.filterMode = FilterMode.Bilinear;
			Color[] pixels = source.GetPixels();
			for (int i = 0; i < source.width; i++)
			{
				for (int j = 0; j < source.height; j++)
				{
					if (pixels[j * source.width + i].a > 0.5f)
					{
						pixels[j * source.width + i].a = 0f;
					}
					else
					{
						pixels[j * source.width + i].a = 1f;
					}
				}
			}
			blackTexture.Apply(updateMipmaps: false);
			return blackTexture;
		}
	}
}
