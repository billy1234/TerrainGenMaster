using UnityEngine;
using System.Collections;

public enum ROAD_Type
{
	HIGHWAY,STREET
};

public struct road
{
	Vector2[] nodes;
}
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class CityWorld : MonoBehaviour 
{
	public Texture2D map; //red is pop greem is vegetaion and blue is heightmap

	public int mapSize =50;
	public float perlinScale;
	public GameObject[] viusalDisplay = new GameObject[3];
	public Vector3[] road1;
	public float errosionSoftness =4;
	[Range(0,1)]
	public float minPopHeight;
	[Range(0,1)]
	public float watterClamp;
	[Range(0,5)]
	public float populationReductionPerFlora;
	public float heightScale;

	void Start()
	{
		buildMap();
		errosionPass ();
		populationGrowthPass ();

		road1 = makeRoad ();
		displayAllMaps (viusalDisplay);
		buildGround ();
		GetComponent<Renderer> ().material.mainTexture = map;
	}

	void buildGround()
	{
		PlaneBuilder pBuilder = new PlaneBuilder();
		for (int x =0; x < mapSize -1; x++)
		{
			for (int y =0; y < mapSize -1; y++) 
			{
				Vector3[] verts = new Vector3[4];
				verts[0] = getheight(x,y);
				verts[1] = getheight(x +1,y);
				verts[2] = getheight(x,y +1);
				verts[3] = getheight(x +1,y +1);
				pBuilder.addQuad(verts);
			
			}
		}
		GetComponent<MeshFilter> ().mesh = pBuilder.compileMesh(true);
	}

	Vector3 getheight(int x, int z)
	{
	
		return new Vector3 (x, map.GetPixel (x, z).b * heightScale, z) - new Vector3(mapSize/2,0,mapSize/2); //worth noting using the population causes the plane to look like moon craters
	}

	void displayAllMaps(GameObject[] displayPlanes)
	{
		Texture2D pop = new Texture2D( mapSize, mapSize);
		Texture2D veg = new Texture2D( mapSize, mapSize);
		Texture2D height = new Texture2D( mapSize, mapSize);
		pop.filterMode = FilterMode.Point;
		pop.wrapMode = TextureWrapMode.Clamp;
		veg.filterMode = FilterMode.Point;
		veg.wrapMode = TextureWrapMode.Clamp;
		height.filterMode = FilterMode.Point;
		height.wrapMode = TextureWrapMode.Clamp;

		for (int x =0; x < mapSize; x++)
		{
			for (int y =0; y < mapSize; y++) 
			{
				pop.SetPixel(x,y,new Color( map.GetPixel(x,y).r,map.GetPixel(x,y).r,map.GetPixel(x,y).r));
				veg.SetPixel(x,y,new Color( map.GetPixel(x,y).g,map.GetPixel(x,y).g,map.GetPixel(x,y).g));
				height.SetPixel(x,y,new Color( map.GetPixel(x,y).b,map.GetPixel(x,y).b,map.GetPixel(x,y).b));
			}
		}
		pop.Apply ();
		veg.Apply ();
		height.Apply ();
		displayPlanes [0].GetComponent<Renderer> ().material.mainTexture = pop ;
		displayPlanes [1].GetComponent<Renderer> ().material.mainTexture = veg ;
		displayPlanes [2].GetComponent<Renderer> ().material.mainTexture = height ;
	}
	Vector3[] makeRoad()
	{
		Vector3 firstDir = new Vector3 (0.5f, 0, 0.5f);//botom left of the image
		Vector3[] nodes = new Vector3[10];
		nodes [0] = Vector3.one;
		for (int i=1; i < 10; i++) 
		{
			Vector3 direction;
			if( i==1)
			{
				direction = firstDir;
			}
			else
			{
				direction = (nodes[i -1] - nodes[i -2]).normalized;
			}
			nodes[i] = nodes[i -1] +  direction;
		}
		return nodes;
	}

	void OnDrawGizmos()
	{
		if(Application.isPlaying)
		{
			for(int i = 0;i < road1.Length -1; i++)
			{
				Debug.DrawLine(road1[i], road1[i +1]);
			}
		}
	}


















	void errosionPass()
	{
		for (int x =0; x < mapSize; x++)
		{
			for (int y =0; y < mapSize; y++)
			{
				Color oldHeight = map.GetPixel(x,y);
				oldHeight.b -= (1-oldHeight.g) / errosionSoftness;
				if(oldHeight.b < watterClamp)
				{
					oldHeight.b = watterClamp; //pushes all values to enough to a uniform sea level
					oldHeight.g =0; // no sea life
				}
				map.SetPixel(x,y,oldHeight);
			}
		}
		map.Apply ();
	}

	void populationGrowthPass()
	{
		for (int x =0; x < mapSize; x++)
		{
			for (int y =0; y < mapSize; y++) 
			{
				Color mapColor = map.GetPixel(x,y);
				mapColor.r -= mapColor.b/populationReductionPerFlora; //populations is less comon in heavilty vegeated areas
				if(mapColor.b < minPopHeight)
				{
					mapColor.r =0;
				}
				map.SetPixel(x,y,mapColor);
			}
		}
		map.Apply ();
	}



	void buildMap()
	{
	 	map = makePerlin();

	}

	Texture2D makePerlin()
	{
		float seed1 = Random.Range (0, 50);
		float seed2 = Random.Range (0, 50);
		float seed3 = Random.Range (0, 50);
		Texture2D mytex = new Texture2D(mapSize,mapSize);
		mytex.filterMode = FilterMode.Point;
		mytex.wrapMode = TextureWrapMode.Clamp;
		for(int x =0; x < mapSize; x++)
		{
			for(int y =0; y < mapSize; y++)
			{
				float perlinValue1 = Mathf.PerlinNoise((seed1 + x) *perlinScale,(seed1 + y) *perlinScale);
				float perlinValue2 = Mathf.PerlinNoise((seed2 + x) *perlinScale,(seed3+ y) *perlinScale);
				float perlinValue3 = Mathf.PerlinNoise((seed3 + x) *perlinScale,(seed3 + y) *perlinScale);

				mytex.SetPixel(x,y,new Color(perlinValue1,perlinValue2,perlinValue3));
			}
		}
		mytex.Apply ();
		return mytex;
	}
}


