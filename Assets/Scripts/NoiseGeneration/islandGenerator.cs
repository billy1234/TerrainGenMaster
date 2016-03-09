using UnityEngine;
using UnityEditor;
using System.Collections;
using NoiseExtention;
using heightMapUtility;
public class islandGenerator : MonoBehaviour
{
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
	//public splatMapShaderInput splatMapInfo;
	public splatMapInput splatMapInfo;
	[Tooltip("The final texture resolution will be width * height * upscale")]
	[Range(1,30)]
	public float textureUpscaleFactor =2;


	public void save()
	{
		Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
		Texture texture = GetComponent<Renderer>().sharedMaterial.mainTexture;
		AssetDatabase.AddObjectToAsset(mesh,"Assets/ProcedralTerrain/" + mesh.name + ".asset");
		AssetDatabase.AddObjectToAsset(texture,"Assets/ProcedralTerrain/" + texture.name + ".asset");
		AssetDatabase.SaveAssets();
	}

	public void makeMeshAndTexture()
	{
		float[,] heightmap =perlinNoiseLayeredSimple.perlinNoise(width,height,seed,scale,octaves,persistance,lacunarity,offset,true);
		heightMapSmoothing.clampEdgesCircular(ref heightmap,0.5f,0.7f);
		heightMapSmoothing.clampHeightMapAt(ref heightmap, clampHeight);
		Texture2D texture = textureResize.resizeTexture(heightMapToTexture.generateTextureFromSingleGradient(heightmap,g),textureUpscaleFactor);
		splatMapInfo.weights = texture;
		texture = splatMap.splatMapTexure2Drgb(splatMapInfo);
		Mesh mesh = heightMapToMesh.meshFromHeightMap(heightmap,heightScale);
		GetComponent<MeshFilter>().sharedMesh = mesh;
		GetComponent<Renderer>().sharedMaterial.mainTexture =  texture;
		//texture.
	}

}
