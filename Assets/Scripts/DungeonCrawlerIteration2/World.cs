using UnityEngine;
using System.Collections;

[System.Serializable]
public struct charAlias
{
	public char letter;
	public Color color;
}
public class World : MonoBehaviour 
{
	public TextureCompiler texture;//visual represetaion
	public ChunkGenerator chunk;//code repersenation
	public int chunkSize; //there will only be one world and all the chunks will be the same size
	public charAlias[] charColors;//sort by alphabetiacl order for future so it can be searched with a binary search
	void Start () 
	{
		chunk = new ChunkGenerator (1, 1,chunkSize);
		texture = new TextureCompiler (chunk.buildChunk (),this);
		gameObject.GetComponent<Renderer> ().material.mainTexture = texture.mapTexture;
	}

	void Update () 
	{
	
	}
}
