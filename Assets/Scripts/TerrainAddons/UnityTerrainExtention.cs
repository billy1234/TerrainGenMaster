using UnityEngine;
using System.Collections;

public class UnityTerrainExtention : MonoBehaviour
{

	public float perlinZoom = 0.14f;
	[Range(0,0.9f)]
	public float watterCutoff = 0.1f;
	[Range(0.1f,1f)]
	public float playableAreaHeight = 0.5f;
	public float jitter = 1;
	[Range(0,1)]
	public float playableAreaFill =0.5f;
	public int playabeAreaSmoothingPasses;
	private Terrain t;

	int sizeX;
	int sizeZ;
	UnityTerrainCells myCells;
	void Start ()
	{
		t = gameObject.GetComponent<Terrain>();
		sizeZ = t.terrainData.heightmapHeight;
		sizeX = t.terrainData.heightmapWidth;
		myCells = new UnityTerrainCells(sizeX,sizeZ);
		calculatePlayableAreas();
		for(int z =0; z < sizeZ; z++)
		{
			for(int x =0; x < sizeZ; x++)
			{
				float height = Mathf.PerlinNoise((x + Random.Range(-jitter,jitter)) * perlinZoom, (z + Random.Range(-jitter,jitter)) * perlinZoom);
				if(height < watterCutoff )
				{
					height = watterCutoff;
				}
				if(myCells.playableArea[x,z] == false)
				{
					height = playableAreaHeight;
				}
				myCells.heights[x,z] = height;
			}
		}

		t.terrainData.SetHeights(0,0,myCells.heights);
	}

	void OnDrawGizmos()
	{
		if(Application.isPlaying)
		{
			Color c;
			for(int z =0; z <sizeZ; z++)
			{
				for(int x =0; x < sizeX; x++)
				{
					c = Color.black;
					if(myCells.playableArea[x,z])
					{
						c = Color.white;
					}
					Gizmos.color = c;
					Gizmos.DrawCube(new Vector3(x,0,z),Vector3.one);
				}
			}
		}
	}

	void calculatePlayableAreas()
	{
		#region whiteNoisePass
		for(int z =0; z <sizeZ; z++)
		{
			for(int x =0; x < sizeX; x++)
			{
				if(Random.Range(0f,1f) > playableAreaFill )
				{
					myCells.playableArea[x,z] = false;
				}
				else
				{
					myCells.playableArea[x,z] = true;
				}
			}
		}
		#endregion

		for(int i=0; i < playabeAreaSmoothingPasses; i++)
		{
			SmoothMap();
		}
	}

	void SmoothMap() 
	{
		for (int x = 0; x < sizeX; x ++)
		{
			for (int z = 0; z < sizeZ; z ++)
			{
				int neighbourWallTiles = GetSurroundingWallCount(x,z);
				
				if (neighbourWallTiles > 4)
				{
					myCells.playableArea[x,z] = true;
				}
				else if (neighbourWallTiles < 4)
				{
					myCells.playableArea[x,z] = false;
				}
				
			}
		}
	}

	int GetSurroundingWallCount(int gridX, int gridZ)
	{
		int wallCount = 0;

		for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX ++) 
		{
			for (int neighbourY = gridZ - 1; neighbourY <= gridZ + 1; neighbourY ++)
			{
				if (neighbourX >= 0 && neighbourX < sizeZ && neighbourY >= 0 && neighbourY < sizeX)
				{
					if (neighbourX != gridX || neighbourY != gridZ) 
					{
						if(myCells.playableArea[neighbourX,neighbourY])
						{
							wallCount ++;
						}
					}
				}
				else
				{
					wallCount ++;
				}
			}
		}
		
		return wallCount;
	}



	private float watterMaxWorld() //tells us the wold space watter cutoff
	{
		return transform.position.y + (t.terrainData.size.y * watterCutoff);
	}
	

}

public class UnityTerrainCells
{
	public bool[,] playableArea;
	public float[,] heights;

	public int sizeX;
	public int sizeZ;

	public UnityTerrainCells(int x, int z)
	{
		sizeX = x;
		sizeZ = z;
		playableArea = new bool[x,z];
		heights = new float[x,z];
	}


}
