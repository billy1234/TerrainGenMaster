using UnityEngine;
using System.Collections;

[System.Serializable]
public struct NoiseOctaveInfo
{
	public float Lacunarity;
	public float Persistance;
}
namespace NoiseExtention
{

	public static class perlinNoiseExtention
	{
		private static System.Random noisePrng;
		/// <summary>
		/// returns a float between 0 and 1 from a layerd perlin texture, the sacle at witch the values extend on x and y is dictated by base noise scale.
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <param name="layerInfo">each Layers info.</param>
		/// <param name="baseNoiseScale">amount x and y will walk along the perlin teture.</param>
		public static float perlinNoise(int x, int y, NoiseOctaveInfo[] layerInfo, float baseNoiseScale)
		{

			return perlinNoise(x ,y ,layerInfo,baseNoiseScale,0f);
	
		}

		/// <summary>
		/// returns a float between 0 and 1 from a layerd perlin texture, the sacle at witch the values extend on x and y is dictated by base noise scale, alows for jitter
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <param name="layerInfo">each Layers info.</param>
		/// <param name="baseNoiseScale">amount x and y will walk along the perlin teture.</param>
		/// <param name="jitter">Jitter.</param>
		public static float perlinNoise(int x, int y, NoiseOctaveInfo[] layerInfo, float baseNoiseScale,float jitter)
		{
			noisePrng = new System.Random(Mathf.Abs(x)^Mathf.Abs(y));
			float maxPersistance =0;
			
			for(int i=0; i < layerInfo.Length; i++)
			{
				if(layerInfo[i].Persistance < 0)
					Debug.LogError("invalid Persistance" + ". Persistance = "+layerInfo[i]);
				if(layerInfo[i].Lacunarity < 0)
					Debug.LogError("invalid Lacunarity" + ". Lacunarity = "+layerInfo[i]);
				
				maxPersistance += layerInfo[i].Persistance;
			}
			
			float heightValue = 0;
			
			for(int i=0; i < layerInfo.Length; i++)
			{
				heightValue += Mathf.PerlinNoise((x + Random.Range(-jitter,jitter)) * baseNoiseScale *layerInfo[i].Lacunarity, (y+ Random.Range(-jitter,jitter)) * baseNoiseScale * layerInfo[i].Lacunarity) *  layerInfo[i].Persistance;
			}
			
			heightValue = heightValue / maxPersistance; //re normalize the value
			return heightValue;
		}

		public static float[,] perlinNoiseHeightMap( int width,int height, NoiseOctaveInfo[] layerInfo,float baseNoiseScale)
		{
			return perlinNoiseHeightMap(width,height, layerInfo, baseNoiseScale, 0f);
		}

		public static float[,] perlinNoiseHeightMap( int width,int height, NoiseOctaveInfo[] layerInfo,float baseNoiseScale,float jitter)
		{
			float[,] noise = new float[width,height];
			
			for(int y =0; y < height; y++)
			{
				for(int x =0; x < height; x++)
				{
					if(jitter == 0)
					{
						noise[x,y] = perlinNoise(x,y,layerInfo,baseNoiseScale);
					}
					else
					{
						noise[x,y] = perlinNoise(x,y,layerInfo,baseNoiseScale,jitter);
					}
				}
			}
			return noise;
		}

		public static Texture2D perlinNoiseTexture(int width,int height, NoiseOctaveInfo[] layerInfo,float baseNoiseScale)
		{
			return perlinNoiseTexture(width, height, layerInfo, baseNoiseScale,0f);
		}

		public static Texture2D perlinNoiseTexture(int width,int height, NoiseOctaveInfo[] layerInfo,float baseNoiseScale,float jitter)
		{
			float[,] noise;
			Texture2D finalTexture  = new Texture2D(width,height);
			if(jitter ==0)
			{
				noise = perlinNoiseHeightMap(width, height, layerInfo, baseNoiseScale);
			}
			else
			{
				noise = perlinNoiseHeightMap(width, height, layerInfo, baseNoiseScale,jitter);
			}
			
			Color[] pixels = new Color[width * height];
			for(int y =0; y < height; y++)
			{
				for(int x =0; x < height; x++)
				{
					pixels[x + y * width] = Color.Lerp(Color.black,Color.white, noise[x,y]);
					//Debug.Log(noise[x,y]);
				}
			}
			finalTexture.SetPixels(pixels);
			finalTexture.wrapMode = TextureWrapMode.Clamp;
			finalTexture.filterMode = FilterMode.Point;
			finalTexture.Apply();
			return finalTexture;
		}


	}
}
