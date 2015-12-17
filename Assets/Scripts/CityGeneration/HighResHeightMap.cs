using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CityWorld))]
public class HighResHeightMap : MonoBehaviour 
{
	float[,] detailedHeightMap = null; //stored as a connected array
	private CityWorld cityW = null;
	public int quadResolution = 50;

	public GameObject[] debug;


	void Start()
	{
		getRefrences();
		loadHighPolly();

		debug[0].AddComponent<MeshFilter>().mesh = getHighPollyQuad(0,0);
		debug[0].AddComponent<MeshRenderer>();
		debug[1].AddComponent<MeshFilter>().mesh = getHighPollyQuad(1,0);
		debug[1].AddComponent<MeshRenderer>();
		debug[2].AddComponent<MeshFilter>().mesh = getHighPollyQuad(0,1);
		debug[2].AddComponent<MeshRenderer>();
		debug[3].AddComponent<MeshFilter>().mesh = getHighPollyQuad(1,1);
		debug[3].AddComponent<MeshRenderer>();

	}
	void getRefrences()
	{
		cityW = GetComponent<CityWorld>();

	}
	void loadHighPolly()
	{
		detailedHeightMap = new float[ cityW.mapSize *quadResolution,cityW.mapSize *quadResolution];

		for (int x =0; x < cityW.mapSize * quadResolution; x++) //x represents x in THIS array
		{
			for (int y =0; y < cityW.mapSize * quadResolution; y++) 
			{
				int xCoord = x/quadResolution; //xcoord represents x in the old array
				int yCoord = y/quadResolution;

				detailedHeightMap[x,y] = cityW.getHeight(xCoord,yCoord).y; //needs to slowly go from each vert not sharp steps
			}
		}
	}

	Mesh getHighPollyQuad(int x, int y)
	{
		if(x < 0 || y < 0 || x > cityW.mapSize|| y > cityW.mapSize)
			Debug.LogError("the quad specified at " + x +", "+ y+" can not be found");

		x *= quadResolution;
		y *= quadResolution;
		PlaneBuilder plane = new PlaneBuilder();

		for(int xIndex =0; xIndex < quadResolution -1; xIndex++)
		{
			for(int yIndex =0; yIndex < quadResolution -1; yIndex++)
			{
				 //detailedHeightMap[x + xIndex,y + yIndex];
				addQuad(x + xIndex,y + yIndex, ref plane);
			}
		}
		return plane.compileMesh();
	}

	void addQuad(int x, int y, ref PlaneBuilder pBuilder)
	{
		Vector3[] quadVerts = new Vector3[4]{getVert(x,y),getVert(x +1,y),getVert(x,y + 1),getVert(x + 1,y + 1)};
		//print(getVert(x,y).y);
		pBuilder.addQuad(quadVerts);
	}

	Vector3 getVert(int x, int y)
	{
		return new Vector3(((float)x + 0.5f)/ (float)quadResolution,detailedHeightMap[x,y],((float)y + 0.5f)/(float)quadResolution) ;
	}
	
}


