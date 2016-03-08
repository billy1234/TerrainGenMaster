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
		public static Texture2D generateTextureFromSingleGradient(float[,] heightMap,Gradient colorGradient)
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
		public static Texture2D buildTextureFromPixels(Color[] pixels, int width,int height)
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
			//mesh.
			mesh.uv  = uvs;
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
		public static void clampEdgesCircular(ref float[,] heightMap,float innerRadiusPercentage,float outerRadiusIncrease,float clampHeight) //assumes the heightmap is normalized between 0-1
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
		public static void clampEdgesCircular(ref float[,] heightMap,float innerRadiusPercentage,float outerRadiusIncrease) //assumes the heightmap is normalized between 0-1
		{
			clampEdgesCircular(ref heightMap,innerRadiusPercentage,outerRadiusIncrease,0);
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

	public static class textureResize
	{
		public static Texture2D resizeTexture(Texture2D oldTexture,float scaleFactor)
		{
			int height = Mathf.RoundToInt(oldTexture.height * scaleFactor);
			int width  = Mathf.RoundToInt(oldTexture.width  * scaleFactor);
			if(height * width > 1000000000)
			{
				Debug.LogError("you are trying to build a texture with atleast 1billion pixels, this will most likeley crash unity");
				return Texture2D.whiteTexture;
			}
			Texture2D texture = new Texture2D(height,width);
			texture.filterMode = FilterMode.Point;
			texture.wrapMode = TextureWrapMode.Clamp;
			Color[] pixels = new Color[height * width];
			for(int y =0; y < height; y++)
			{
				for(int x =0; x < width; x++)
				{
					pixels[x + y * width] = oldTexture.GetPixelBilinear((float)x/(float)width,(float)y/(float)height);
				}
			}
			texture.SetPixels(pixels);
			texture.Apply();
			return texture;
		}

	}
	[System.Serializable]
	public class splatMapInput
	{
		[HideInInspector]
		public Texture2D weights;
		public Texture2D textureR;
		public Texture2D textureG;
		public Texture2D textureB;
		public Texture2D textureA;
		public Vector2 tilingR;
		public Vector2 tilingG;
		public Vector2 tilingB;
		public Vector2 tilingA;

		public splatMapInput ()
		{
			this.tilingR = Vector2.one;
			this.tilingG = Vector2.one;
			this.tilingB = Vector2.one;
			this.tilingA = Vector2.one;
		}
	};
	[System.Serializable]
	public struct splatMapShaderInput
	{
		[HideInInspector]
		public Texture2D weights;
		public Texture2D textureR;
		public Texture2D textureG;
		public Texture2D textureB;
		public Texture2D textureA;
		public Texture2D normalR;
		public Texture2D normalG;
		public Texture2D normalB;
		public Texture2D normalA;
		public Vector2 tillingR;
		public Vector2 tillingG;
		public Vector2 tillingB;
		public Vector2 tillingA;
	}
	public static class splatMap
	{
		public static  Texture2D splatMapTexure2Drgb(splatMapInput input)
		{
			return splatMapTexure2Drgb(input.weights, new Texture2D[4]{input.textureR,input.textureG,input.textureB,input.textureA},new Vector2[4]{input.tilingR,input.tilingG,input.tilingB,input.tilingA});
		}
		public static void sendSplatToMaterial(splatMapShaderInput input,Material myMaterial)
		{
			myMaterial.SetTexture("_Control",input.weights);

			myMaterial.SetTexture("_Splat3",input.textureA);
			myMaterial.SetTexture("_Splat2",input.textureB);
			myMaterial.SetTexture("_Splat1",input.textureG);
			myMaterial.SetTexture("_Splat0",input.textureR);

			myMaterial.SetTexture("_Normal3",input.normalA);
			myMaterial.SetTexture("_Normal2",input.normalB);
			myMaterial.SetTexture("_Normal1",input.normalG);
			myMaterial.SetTexture("_Normal0",input.normalR);
		}
		public static Texture2D splatMapTexure2Drgb(Texture2D weights, Texture2D[] textures,Vector2[] tiling)
		{
			int height = weights.height;
			int width = weights.width;
			Color[] pixels = new Color[height * width];
			for(int i=0; i < 4; i ++)
			{
				textures[i].wrapMode = TextureWrapMode.Repeat;

			}
			for(int y =0; y < height; y++)
			{
				for(int x =0; x < width; x++)
				{
					Color weight = weights.GetPixel(x,y);
					float maxWeight = getMaxWeight(weight);
					Color bendedColor = Color.clear;
					bendedColor += textures[0].GetPixelBilinear(((float)x/(float)width) * tiling[0].x,((float)y/(float)height) *tiling[0].y) * (weight.r /maxWeight );
					bendedColor += textures[1].GetPixelBilinear(((float)x/(float)width) * tiling[1].x,((float)y/(float)height) *tiling[1].y) * (weight.g /maxWeight );
					bendedColor += textures[2].GetPixelBilinear(((float)x/(float)width) * tiling[2].x,((float)y/(float)height) *tiling[2].y) * (weight.b /maxWeight );
					bendedColor += textures[3].GetPixelBilinear(((float)x/(float)width) * tiling[3].x,((float)y/(float)height) *tiling[3].y) * (weight.a /maxWeight );
					pixels[x + y * width] = bendedColor;
				}
			}
			return heightMapToTexture.buildTextureFromPixels(pixels,height,width);
		}


		private static float getMaxWeight(Color c)
		{
			return c.r + c.g + c.b;
		}

		public static Mesh uvMapWithTilling(Mesh mesh,splatMapShaderInput input)
		{
			Vector2[] uvG = new Vector2[mesh.uv.Length];
			Vector2[] uvB = new Vector2[mesh.uv.Length];
			Vector2[] uvA = new Vector2[mesh.uv.Length];
			for(int i=0; i < mesh.uv.Length; i++)
			{
				//mesh.uv1[i] = new Vector2(mesh.uv[i].x * input.tillingR.x,mesh.uv[i].y * input.tillingR.y);
				uvG[i] = new Vector2(mesh.uv[i].x * input.tillingG.x,mesh.uv[i].y * input.tillingG.y);
				uvB[i] = new Vector2(mesh.uv[i].x * input.tillingB.x,mesh.uv[i].y * input.tillingB.y);
				uvA[i] = new Vector2(mesh.uv[i].x * input.tillingA.x,mesh.uv[i].y * input.tillingA.y);


			}
			mesh.uv2 = uvA;
			mesh.uv3 = uvB;
			mesh.uv4 = uvG;
			return mesh;
		}


	}
}

