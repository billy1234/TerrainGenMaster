using UnityEngine;
using System.Collections;


public struct MapSeeds
{
	public float height;
	public float pop;
	public float veg;
}




//this class is a copy of cityworld branched at  14th dec 2015
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class TerrainGen : MonoBehaviour 
{
	
	public Texture2D map; //red is pop, green is vegetaion, and blue is heightmap
	
	public int mapSize =50;
	public float heightPerlinScale = 0.14f;
	public float populationPerlinScale = 0.14f;
	public float vegetationPerlinScale = 0.14f;
	public GameObject[] viusalDisplay = new GameObject[3];
	[Range(0,5)]
	public float errosionSoftness =4f;
	[Range(0,1)]
	public float minPopHeight = 0.3f;
	[Range(0,1)]
	public float maxPopHeight = 0.6f;
	[Range(0,0.3f)]
	public float watterClamp = 0.5f;
	[Range(0,2)]
	public float populationReductionPerFlora = 0.5f;
	[Range(0,1)]
	public float whiteMix =0.1f;

	public float heightScale =1f;
	
	public string seed;

	private MapSeeds mapSeeds;
	private System.Random myRng = new System.Random(); 
	void Start()
	{
		seedMyRng(seed);
		buildMap();
		
		displayAllMaps (viusalDisplay);
		buildGround ();
		GetComponent<Renderer> ().material.mainTexture = map;
	}
	
	void seedMyRng(string seed)
	{
		if(seed.Length != 0)
		{
			myRng =  new System.Random(seed.GetHashCode());
		}
		mapSeeds.veg = (float)myRng.NextDouble() * 100f;
		mapSeeds.pop = (float)myRng.NextDouble() * 100f;
		mapSeeds.height = (float)myRng.NextDouble() * 100f;

		//print(mapSeeds.veg+"  "+mapSeeds.pop+"  "+mapSeeds.height);
	}
	
	public float[,] getPopMap()
	{
		float[,] popMap = new float[mapSize,mapSize];
		for (int y =0; y< mapSize; y++)
		{
			for (int x =0; x < mapSize; x++)
			{
				popMap[x,y] = map.GetPixel(x,y).r;
			}
		}
		return popMap;
	}
	
	void buildGround()
	{
		TerrainGenMeshBuilder pBuilder = new TerrainGenMeshBuilder();
		for (int x =0; x < mapSize; x++) //-1 as the bottom left of the quad is x/y but top left is x+1/y+1
		{
			for (int y =0; y < mapSize; y++) 
			{
				Vector3[] verts = new Vector3[4];
				verts[0] = getHeight(x,y);
				verts[1] = getHeight(x +1,y);
				verts[2] = getHeight(x,y +1);
				verts[3] = getHeight(x +1,y +1);
				if(getMapCell(x,y).isWatter)
				{
					pBuilder.addQuad(verts,1);
				}
				else
				{
					pBuilder.addQuad(verts,0);
				}
				
			}
		}
		GetComponent<MeshFilter> ().mesh = pBuilder.compileMesh();//low polly map
		//print(watterClamp * heightScale);
	}
	
	Vector3 getHeight(int x, int z)
	{
		
		return new Vector3 (x, map.GetPixel ((int)x, (int)z).b * heightScale, z) - new Vector3(mapSize/2,0,mapSize/2); 
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
		displayPlanes [0].GetComponent<Renderer> ().material.mainTexture = pop;
		displayPlanes [1].GetComponent<Renderer> ().material.mainTexture = veg ;
		displayPlanes [2].GetComponent<Renderer> ().material.mainTexture = height ;
	}
	
	void buildMap()
	{
		map = fillMapTexture();
		
	}
	
	Texture2D fillMapTexture()
	{
		Texture2D mytex = new Texture2D(mapSize,mapSize);
		mytex.filterMode = FilterMode.Point;
		mytex.wrapMode = TextureWrapMode.Clamp;

		mapCell cellValues;

		for(int x =0; x < mapSize; x++)
		{
			for(int y =0; y < mapSize; y++)
			{
				cellValues = getMapCell(x,y);
				mytex.SetPixel(x,y,new Color(cellValues.population,cellValues.vegetation,cellValues.height));
				//print(cellValues.population);
			}
		}
		mytex.Apply ();
		return mytex;
	}

	public mapCell getMapCell(float x, float z) //will scan through all feilds that will determine height values and return a normalizewd height value between 0 - 1
	{
		float vegDensity = getPerlin(x,z,mapSeeds.veg,vegetationPerlinScale); //the base perlin vales prior to modification based on one another
		float popDensity = getPerlin(x,z,mapSeeds.pop,populationPerlinScale);
		float height = getPerlin(x,z,mapSeeds.height,heightPerlinScale);
		bool isOcean = false;

		height -= (1-vegDensity) / errosionSoftness;


		//pop growth
		popDensity -= vegDensity/populationReductionPerFlora; //populations is less comon in heavilty vegetated areas


		//print(vegDensity+" "+vegDensity+" "+popDensity);
		height = ((float)getWhiteNoiseAt(x,z) * whiteMix + height) /(1 + whiteMix);


		if(height < minPopHeight || height > maxPopHeight) //areas to close to the ocean or to high up for population
		{
			popDensity =0;
		}



		//flatens world at sea level
		if(height < watterClamp)
		{
			isOcean = true;
			height = watterClamp; //pushes all values to enough to a uniform sea level
			vegDensity =0; // no sea life
			popDensity =0; //noone lives in the ocean
		}

		mapCell cell =new mapCell(height,vegDensity,popDensity);
		cell.isWatter = isOcean;
		return cell;
	}
	
	float getPerlin(float x, float y, float seed,float perlinScale)
	{
		return Mathf.PerlinNoise((x + seed) *perlinScale,(y + seed) *perlinScale);
	}

	float getWhiteNoiseAt(float x, float y)
	{
		//will give the same number everytime the coordinate is input
		seedMyRng((int)(x * mapSize+ y));
		return (float)myRng.NextDouble();
	}

	/// <summary>
	/// Seeds the rng class by instancing it
	/// </summary>
	/// <param name="seed">Seed.</param>
	void seedMyRng(int seed)
	{
		//overly expensive look for a new random class tht can be better seeded in the future
		myRng = new System.Random(seed);
	}

}

public class mapCell //quick storage class for map information
{
	private float[] values = new float[3]; // 0 is height 1 is veg 2 is population
	public bool isWatter = false;

	public float height 
	{
		get{return values[0];}
		set{values[0] = value;}
	}
	public float vegetation
	{
		get{return values[1];}
		set{values[1] = value;}
	}
	public float population
	{
		get{return values[2];}
		set{values[2] = value;}
	}




	public mapCell(float height, float veg, float pop)
	{
		values[0] = height;
		values[1] = veg;
		values[2] = pop;
	}
	
}
