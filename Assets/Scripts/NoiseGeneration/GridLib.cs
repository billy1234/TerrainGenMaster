using UnityEngine;
using System.Collections;

namespace heightMapUtility
{
	public static class heightMapToTexture
	{
		public static Texture2D generateGreyScaleTexture(float[,] heightMap)
		{
			int width = heightMap.GetLength(0);
			int height = heightMap.GetLength(1);
			Color[] pixels = new Color[height * width];
			for(int y =0; y < height; y++)
			{
				for(int x =0; x < width; x++)
				{
					pixels[x + y * width] = new Color(heightMap[x,y],heightMap[x,y],heightMap[x,y]);
				}
			}
			return buildTextureFromPixels(pixels,width,height);
		}
		public static Texture2D genrateTextureFromSingleGradient(Gradient colorGradient, float[,] heightMap)
		{
			int width = heightMap.GetLength(0);
			int height = heightMap.GetLength(1);
			Color[] pixels = new Color[height * width];
			for(int y =0; y < height; y++)
			{
				for(int x =0; x < width; x++)
				{
					pixels[x + y * width] = colorGradient.Evaluate(heightMap[x,y]);
				}
			}
			return buildTextureFromPixels(pixels,width,height);
		}

		private static Texture2D buildTextureFromPixels(Color[] pixels, int width,int height)
		{
			Texture2D texture = new Texture2D(width,height);
			texture.SetPixels(pixels);
			texture.filterMode = FilterMode.Point;
			texture.wrapMode = TextureWrapMode.Clamp;
			texture.Apply();
			return texture;
		}
	}

	/// <summary>
	/// not implimented
	/// </summary>
	public static class heightMapToMesh
	{

	}
}

