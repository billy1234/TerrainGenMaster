using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Terrain))]
public class TerrainNoiseExtention : MonoBehaviour 
{


	public NoiseOctaveInfo[] octaveInfo;
	public float baseScale =0.014f;
	public float jitter =1f;

	void Start ()
	{
		Terrain t = GetComponent<Terrain>();
		int height = t.terrainData.heightmapHeight;
		int width = t.terrainData.heightmapWidth;
		t.terrainData.SetHeights(0,0, NoiseExtention.perlinNoiseExtention.perlinNoiseHeightMap(width,height,octaveInfo,baseScale,jitter));
	}

}
