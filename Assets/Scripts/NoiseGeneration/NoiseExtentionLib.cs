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
	public static class cellNoise
	{
		private static System.Random noisePrng;
		public static float[,] getNormalizedCellNoise(int height, int width,int seed,int featurePointCount)
		{
			float[,] noise = getCellNoise(height, width, seed, featurePointCount);
			float maxValue =0;
			for(int y =0; y < height; y++)
			{
				for(int x =0; x < height; x++)
				{
					if(maxValue < noise[x,y])
					{
						maxValue = noise[x,y];
					}
				}
			}

			for(int y =0; y < height; y++)
			{
				for(int x =0; x < height; x++)
				{
					noise[x,y] /= maxValue;
				}
			}
			return noise;
		}
		public static float[,] getCellNoise(int height, int width,int seed,int featurePointCount)
		{
			noisePrng = new System.Random(seed);
			float[,] noise = new float[height,width];
			Vector2[] featurePoints = new Vector2[featurePointCount];
			for(int i =0; i < featurePointCount;i++)
			{
				featurePoints[i] = new Vector2((float)noisePrng.NextDouble(),(float)noisePrng.NextDouble());
			}
			for(int y =0; y < height; y++)
			{
				for(int x =0; x < height; x++)
				{
					noise[x,y] = setCell(featurePoints,x,y,height,width);
				}
			}
			return noise;
		}

		private static float setCell(Vector2[] featurepoints,int x,int y,int height, int width)
		{
			float minDistance = Mathf.Infinity;
			float featureX;
			float featureY;
			Vector2 position = new Vector2(x,y);
			for(int i =0; i < featurepoints.Length; i++)
			{
				featureX = featurepoints[i].x * width;
				featureY = featurepoints[i].y * height;
				float currentDistance =Mathf.Abs(euclideandistance(position,new Vector2(featureX,featureY)));
				if(currentDistance < minDistance)
				{
					minDistance =  currentDistance;
				}
			}
			return minDistance;
		}
		private static float euclideandistance(Vector2 p1, Vector2 p2)
		{
			return((p1.x - p2.x) * (p1.x - p2.x) + (p1.y - p2.y) * (p1.y - p2.y) + (p1.x - p2.x) * (p1.x - p2.x));
		}
	}

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
				heightValue += Mathf.PerlinNoise((x + (((float)noisePrng.NextDouble() * 2 * jitter)- jitter)) * baseNoiseScale *layerInfo[i].Lacunarity, (y+ (((float)noisePrng.NextDouble() * 2 * jitter)- jitter)) * baseNoiseScale * layerInfo[i].Lacunarity) *  layerInfo[i].Persistance;
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

	public static class perlinNoiseLayeredSimple
	{
		public static float[,] perlinNoise(int mapWidth, int mapHeight, int seed, float scale, int octaves, float peristance, float lacunarity,Vector2 offset)
		{
			System.Random prng = new System.Random(seed);
			float[,] noiseMap = new float[mapWidth,mapHeight];
			Vector2[] octaveOffsets = new Vector2[octaves];
			for (int i=0; i < octaves; i++)
			{
				float offsetX = prng.Next(-100000,100000) + offset.x;
				float offsetY = prng.Next(-100000,100000) + offset.y;
				octaveOffsets[i] = new Vector2(offsetX,offsetY);
			}
			float maxNoise = float.MinValue;
			float minNoise = float.MaxValue;
			for(int y =0; y < mapHeight; y++)
			{
				for(int x =0; x < mapWidth; x++)
				{
					float amplitude =1;
					float frequency =1;
					float noiseHeight =0;
					for(int i=0; i< octaves; i++)
					{
						float sampleX = x/scale*frequency + octaveOffsets[i].x;
						float sampleY = y/scale*frequency + octaveOffsets[i].y;
						float perlinValue = Mathf.PerlinNoise(sampleX,sampleY) *2f -1f;
						noiseHeight +=perlinValue * amplitude;

						amplitude *= peristance;
						frequency *= lacunarity;
					}

					if(noiseHeight > maxNoise)
					{
						maxNoise = noiseHeight;
					}
					else if(noiseHeight < minNoise)
					{
						minNoise = noiseHeight;
					}
					noiseMap[x,y] = noiseHeight;
				}
			}
			//normalization
			for(int y =0; y < mapHeight; y++)
			{
				for(int x =0; x < mapWidth; x++)
				{
					noiseMap[x,y] = Mathf.InverseLerp(minNoise,maxNoise,noiseMap[x,y]);
				}
			}
			return noiseMap;
		}
	}
}
