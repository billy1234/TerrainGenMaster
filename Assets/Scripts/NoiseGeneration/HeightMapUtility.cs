using UnityEngine;
using System.Collections;

namespace heightMapUtility
{
	public static class heightMapToTexture
	{ 

		/// <summary>
		/// Generates a grey scale texture.
		/// </summary>
		/// <returns>a texture 2d the length and width of the heightmap, all pixels will be in grey scale. assumes the arrays values are nomalized between 0-1</returns>
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
		/// <summary>
		/// Generates a textire from a gradient.. assumes the arrays values are nomalized between 0-1
		/// </summary>
		/// <returns>a texture 2d the length and width of the heightmap, all pixels will be based on the gradient provided.</returns>
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
	/// A class for turning heightmaps (2d float arrays into usable meshes)
	/// </summary>
	public static class heightMapToMesh
	{
		/// <summary>
		/// will turn this float array into a plane with its y offest by the contents of the array.
		/// </summary>
		/// <returns>a mesh based on the heightmap scaled by scale fator.</returns>
		/// <param name="scaleFactor">will scale by x y and z based on the contents of the vector.</param>
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
			mesh.name = "heightMapToMeshOutPut";
			mesh.RecalculateNormals();
			mesh.RecalculateBounds();
			return mesh;
		}

		/// <summary>
		/// will turn this float array into a plane with its y offest by the contents of the array.
		/// </summary>
		/// <returns>a mesh based on the heightmap scaled by scale fator.</returns>
		/// <param name="yScaleFactor">will scale the y axis of the verticies.</param>
		public static Mesh meshFromHeightMap(float[,] heightMap,float ySscaleFactor)
		{
			return meshFromHeightMap(heightMap,new Vector3(1f,ySscaleFactor,1f));
		}
		/// <summary>
		/// will turn this float array into a plane with its y offest by the contents of the array.
		/// </summary>
		/// <returns>a mesh based on the heightmap scaled by scale fator.</returns>
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

	public static class heightMapSmoothing
	{
		/// <summary>
		/// will smooth your heightmap so all the points out side of the circle will gradualy fall to clamp height
		/// </summary>
		/// <param name="heightMap">Height map.</param>
		/// <param name="innerRadiusPercentage">what percentage of the map the radius of the un touched terrain will take up.</param>
		/// <param name="outerRadiusIncrease">a second circle that defines the gradual slope between the real heigtht value and the clamp this value MUST be higher than innerRadius.</param>
		/// <param name="clampHeight">the lowest height the clamping will push areas out side of the circle to.</param>
		public static void ClampEdgesCircular(ref float[,] heightMap,float innerRadiusPercentage,float outerRadiusIncrease,float clampHeight) //assumes the heightmap is normalized between 0-1
		{
			if(innerRadiusPercentage < 0 || innerRadiusPercentage > 1)
			{
				Debug.LogError("incorrect percentage passed to the noisesmoothing.ClampEdgesCircular class, must be between 0f-1f");
			}

			int width = heightMap.GetLength(0);
			int height = heightMap.GetLength(1);
			float hillX = (float)width/2f;
			float hillY = (float)height/2f;
			float radius = ((width + height)/4f)* innerRadiusPercentage;
			float radiusSquared = radius * radius;//square the radius one to save some cpu cycles
			float flatCuttOffRad = radius + (radius * outerRadiusIncrease);
			flatCuttOffRad = flatCuttOffRad * flatCuttOffRad;
			for(int y =0; y < height; y++)
			{
				for(int x =0; x < width; x++)
				{
						heightMap[x,y] = hillHeight(hillX,x,hillY,y,radiusSquared,flatCuttOffRad,clampHeight) * heightMap[x,y];
				}
			}

		}

		/// <summary>
		/// will smooth your heightmap so all the points out side of the circle will gradualy fall to clamp height
		/// </summary>
		/// <param name="heightMap">Height map.</param>
		/// <param name="innerRadiusPercentage">what percentage of the map the radius of the un touched terrain will take up.</param>
		/// <param name="outerRadiusIncrease">a second circle that defines the gradual slope between the real heigtht value and the clamp this value MUST be higher than innerRadius.</param>
		public static void ClampEdgesCircular(ref float[,] heightMap,float innerRadiusPercentage,float outerRadiusIncrease) //assumes the heightmap is normalized between 0-1
		{
			ClampEdgesCircular(ref heightMap,innerRadiusPercentage,outerRadiusIncrease,0);
		}

		private static float hillHeight(float hillX, float x, float hillY,float y,float radiusSquared,float smoothingRad,float clampHeight)
		{
			float result =0;
			float xDifferenceSquared = (x - hillX) * (x - hillX);
			float yDifferenceSquared = (y - hillY) * (y - hillY);
			result = radiusSquared -(xDifferenceSquared + yDifferenceSquared);
			if(result < 0)
			{
				if(result > -smoothingRad)
				{
					result = Mathf.InverseLerp(-smoothingRad,0,result);
					if(result < clampHeight)
					{
						result = clampHeight;
					}
					//Debug.Log(result +"  x: "+x+"  y: "+y);
				}
				else
				{
					result = clampHeight;
				}

			}
			else
			{
				result = 1;
			}
			return result;
		}

		/// <summary>
		/// will not alow values in the heightmap to fall below min height.
		/// </summary>
		/// <param name="heightMap">Height map.</param>
		/// <param name="minHeight">Minimum height.</param>
		public static void clampHeightMapAt(ref float[,] heightMap,float minHeight)
		{	
			int width = heightMap.GetLength(0);
			int height = heightMap.GetLength(1);
			for(int y =0; y < height; y++)
			{
				for(int x =0; x < width; x++)
				{
					if(heightMap[x,y] < minHeight)
					{
						heightMap[x,y] = minHeight;
					}
				}
			}

		}
	}

}

