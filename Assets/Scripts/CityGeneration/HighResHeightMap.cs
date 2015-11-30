using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CityWorld))]
public class HighResHeightMap : MonoBehaviour 
{
	HighResQuad[,] detailedHeightMap;
	private CityWorld cityW;
	public float quadResolution = 50;

	void getLowPolly()
	{
		cityW = GetComponent<CityWorld>();
		detailedHeightMap = new HighResQuad[cityW.mapSize,cityW.mapSize];
	}

	void initalizeQuads()
	{
		for (int x =0; x < cityW.mapSize; x++) //-1 as the bottom left of the quad is x/y but top left is x+1/y+1
		{
			for (int y =0; y < cityW.mapSize;y++);
			{

			}
		}
	}
}

public class HighResQuad
{
	public float[,] heights;
								
	public HighResQuad(float[,] lowPolly, int subDivisons)
	{
		if(lowPolly.GetLength(0) != 2 ||lowPolly.GetLength(1) != 2)
			Debug.Log("error " + this + " was passed a height map != to a quad");

		heights = lowPolly;

		for(int i=0; i < subDivisons; i++)
		{
			subDivide();
		}
	}
	public void subDivide() // ??????? how
	{
		float[,] oldHeights = heights;
		float oldSizeX =  oldHeights.GetLength(0);
		float oldSizeY =  oldHeights.GetLength(1);
		float sizeX = oldSizeX *2;
		float sizeY = oldSizeY *2;
		heights = new float[sizeX,sizeY];

		for(int y =0; y < oldSizeY; y += 2)
		{
			for(int x =0; x < oldSizeX; x += 2)
			{
				//add 4
				heights[x		*2	,y 		*2] = oldHeights[x		,y		];//as is
				heights[x+1	    *2	,y 		*2] = oldHeights[x		,y		];
				heights[x  		*2	,y 	+1 	*2] = oldHeights[x		,y		];
				heights[x+1 	*2	,y 	+1 	*2] = oldHeights[x		,y		];

				heights[x		*2	,y 		*2] = oldHeights[x		,y		];//as is
				heights[x+1	    *2	,y 		*2] = oldHeights[x+1 	,y		];
				heights[x  		*2	,y 	+1 	*2] = oldHeights[x		,y + 1	];
				heights[x+1 	*2	,y 	+1 	*2] = oldHeights[x +1	,y + 1	];

				heights[x		*2	,y 		*2] = oldHeights[x		,y		];//as is
				heights[x+1	    *2	,y 		*2] = oldHeights[x+1 	,y		];
				heights[x  		*2	,y 	+1 	*2] = oldHeights[x		,y + 1	];
				heights[x+1 	*2	,y 	+1 	*2] = oldHeights[x +1	,y + 1	];

				heights[x		*2	,y 		*2] = oldHeights[x		,y		];//as is
				heights[x+1	    *2	,y 		*2] = oldHeights[x+1 	,y		];
				heights[x  		*2	,y 	+1 	*2] = oldHeights[x		,y + 1	];
				heights[x+1 	*2	,y 	+1 	*2] = oldHeights[x +1	,y + 1	];
			}
		}
	} 


	public Mesh buildHighres()
	{
		PlaneBuilder pBuilder = new PlaneBuilder();
		for (int x =0; x < heights.GetLength(0); x++) //-1 as the bottom left of the quad is x/y but top left is x+1/y+1
		{
			for (int y =0; y < heights.GetLength(0); y++) 
			{
				/*
				Vector3[] verts = new Vector3[4];
				verts[0] = heights[x,y];
				verts[1] = heights[x +1,y];
				verts[2] = heights[x,y +1];
				verts[3] = heights[x +1,y +1];
				pBuilder.addQuad(verts);
				*/
				
			}
		}
		return pBuilder.compileMesh(true);// one high polly quad
	}
}
