using UnityEngine;
using System.Collections;

public class CellNoiseTest : MonoBehaviour 
{
	public int height =50;
	public int width =50;
	public int seed =0;
	public int featureCount = 10;
	public Gradient colorScale;
	void Start () 
	{
		float[,] noise = NoiseExtention.cellNoise.getNormalizedCellNoise(height,width,seed,featureCount);
		Texture2D cellNoiseTex = new Texture2D(width,height);
		Color[] pixels = new Color[width * height];
		for(int y =0; y < height; y++)
		{
			for(int x =0; x < height; x++)
			{
				pixels[x + y * width] = colorScale.Evaluate(noise[x,y]);
			}

		}
		cellNoiseTex.SetPixels(pixels);
		//cellNoiseTex.filterMode = FilterMode.Point;
		cellNoiseTex.Apply();
		GetComponent<Renderer>().material.mainTexture = cellNoiseTex;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
