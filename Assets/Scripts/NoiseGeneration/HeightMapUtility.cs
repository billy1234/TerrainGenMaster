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
		public static Texture2D genrateTextureFromSingleGradient(float[,] heightMap,Gradient colorGradient)
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
		public static Mesh meshFromHeightMap(float[,] heightMap,Vector3 scaleFactor)
		{
			int width = heightMap.GetLength(0);
			int height = heightMap.GetLength(1);
			Mesh mesh = new Mesh();
			Vector3[] verts = new Vector3[width * height];
			Vector2[] uvs = new Vector2[verts.Length];
			int[] tris = new int[(width -1) * (height-1) * 6];
			int triangeIndex =0;//incrimented by reference inside the add tri methord

			for(int y =0; y < height; y++)
			{
				for(int x =0; x < width; x++)
				{
					int vertIndex = x + y * width;
					verts[vertIndex] = new Vector3((x - (width/2f)) * scaleFactor.x,heightMap[x,y]  * scaleFactor.y,(y- (height/2f))  * scaleFactor.z);
					float uvX =0;
					float uvY =0;
					if(x != 0)
					{
						uvX = (float)x/(float)width;
					}
					if(y != 0)
					{
						uvY = (float)y/(float)height;
					}
					uvs[vertIndex] = new Vector2(uvX,uvY);
					if(x < width -1 && y < height -1)
					{
						addTri(ref tris,ref triangeIndex,vertIndex,vertIndex + width + 1,vertIndex + 1);
						addTri(ref tris,ref triangeIndex,vertIndex,vertIndex + width ,vertIndex +width +1);
					}
				}
			}
			mesh.vertices = verts;
			mesh.triangles = tris;
			mesh.uv = uvs;
			mesh.RecalculateNormals();
			mesh.RecalculateBounds();
			return mesh;
		}
		public static Mesh meshFromHeightMap(float[,] heightMap,float ySscaleFactor)
		{
			return meshFromHeightMap(heightMap,new Vector3(1f,ySscaleFactor,1f));
		}
		public static Mesh meshFromHeightMap(float[,] heightMap)
		{
			return meshFromHeightMap(heightMap,Vector3.one);
		}
		private static void addTri(ref int[] tris,ref int triIndex, int a,int b,int c)
		{
			tris[triIndex    ] = a;
			tris[triIndex + 1] = b;
			tris[triIndex + 2] = c;
			triIndex += 3;

		}
	}
}

