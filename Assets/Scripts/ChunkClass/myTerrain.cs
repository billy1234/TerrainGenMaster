using UnityEngine;
using System.Collections;
[System.Serializable]
public struct fillType
{
	public int channel;
	public float zoomLevel;
};

[System.Serializable]
public struct terrainPasses
{
	public fillAllrandom fillAllrandomSeed;
	public fillType[] randomPerlinFill;
	public clampPass[] clampPasses;
}
[System.Serializable]
public struct fillAllrandom
{
	public bool run;
	public float zoomLevel;
}




[RequireComponent(typeof(Renderer))]
public class myTerrain : MonoBehaviour
{
	protected ChunkData terrainMaps;
	public terrainPasses passes;
	public int size = 100;
	void Start()//more or less my componet system will update once i learn how to draw custom gui in the inspector
	{
		terrainMaps = new ChunkData(size,size);

		if(passes.fillAllrandomSeed.run)
		{
			terrainMaps.fillAllWithRandomPerlin(passes.fillAllrandomSeed.zoomLevel);
		}

		foreach(clampPass p in passes.clampPasses)
		{
			terrainMaps.clampChannelValues(p);
		}
		foreach(fillType f in passes.randomPerlinFill)
		{
			terrainMaps.fillWithPerlin(f.zoomLevel,f.channel);
		}
		//gameObject.GetComponent<Renderer>().material.SetTexture("_ParallaxMap",terrainMaps.displayTexture); //will allow paralax shaders to render the plane as if the flat quad was built acording to the heightmaps
		gameObject.GetComponent<Renderer>().material.mainTexture = terrainMaps.displayTexture;
	}

}
