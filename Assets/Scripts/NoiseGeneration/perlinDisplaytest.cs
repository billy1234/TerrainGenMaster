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
	public splatMapShaderInput splatMapInfo;
	[Tooltip("The final texture resolution will be width * height * upscale")]
	[Range(1,40)]
	public float textureUpscaleFactor =2;
	void Start ()
	{
		float[,] heightmap =perlinNoiseLayeredSimple.perlinNoise(width,height,seed,scale,octaves,persistance,lacunarity,offset,true);
		heightMapSmoothing.clampEdgesCircular(ref heightmap,0.5f,0.7f);
		heightMapSmoothing.clampHeightMapAt(ref heightmap, clampHeight);
		Texture2D texture = textureResize.resizeTexture(heightMapToTexture.generateTextureFromSingleGradient(heightmap,g),textureUpscaleFactor);
		splatMapInfo.weights = texture;
		Mesh myMesh = heightMapToMesh.meshFromHeightMap(heightmap,heightScale);
		myMesh = splatMap.uvMapWithTilling(myMesh,splatMapInfo);
		splatMap.sendSplatToMaterial(splatMapInfo,GetComponent<Renderer>().sharedMaterial);
		GetComponent<MeshFilter>().sharedMesh = myMesh;
	}
	

	void OnValidate ()
	{
		if(Application.isPlaying)
		{
			//Start(); too slow now that texture resizeing and splatmapping added
		}
	}
}
