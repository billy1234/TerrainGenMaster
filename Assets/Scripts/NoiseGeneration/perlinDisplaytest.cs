using UnityEngine;
using System.Collections;
using NoiseExtention;
using heightMapUtility;
public class perlinDisplayTest : MonoBehaviour {
	public Gradient g;
	public int width =5;
	public int height =5;
	public int seed =1;
	public float scale =1f;
	public int octaves =3;
	public float lacunarity = 1.5f;
	public float persistance =0.5f;
	public Vector2 offset;
	public float heightScale =5f;
	public float clampHeight =0.2f;
	public float innerRadius =0.5f;
	public float outerRadius =0.7f;

	void Start ()
	{
		float[,] heightmap =perlinNoiseLayeredSimple.perlinNoise(width,height,seed,scale,octaves,persistance,lacunarity,offset);
		heightMapSmoothing.ClampEdgesCircular(ref heightmap,0.5f,0.7f);
		heightMapSmoothing.clampHeightMapAt(ref heightmap, clampHeight);
		GetComponent<Renderer>().sharedMaterial.mainTexture = heightMapToTexture.genrateTextureFromSingleGradient(heightmap,g);
		GetComponent<MeshFilter>().sharedMesh = heightMapToMesh.meshFromHeightMap(heightmap,heightScale);
	}
	

	void OnValidate ()
	{
		if(Application.isPlaying)
		{
			//Start();
		}
	}
}
