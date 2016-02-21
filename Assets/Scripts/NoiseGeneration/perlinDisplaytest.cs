using UnityEngine;
using System.Collections;
using NoiseExtention;
using heightMapUtility;
public class perlinDisplaytest : MonoBehaviour {
	public Gradient g;
	public int width =5;
	public int height =5;
	public int seed =1;
	public float scale =1f;
	public int octaves =3;
	public float lacunarity = 1.5f;
	public float persistance =0.5f;
	public Vector2 offset;

	void Start ()
	{
		GetComponent<Renderer>().material.mainTexture = heightMapToTexture.genrateTextureFromSingleGradient(g,perlinNoiseLayeredSimple.perlinNoise(width,height,seed,scale,octaves,persistance,lacunarity,offset));
	}
	

	void OnValidate ()
	{
		if(Application.isPlaying)
		{
			Start();
		}
	}
}
