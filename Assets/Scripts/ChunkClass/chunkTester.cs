using UnityEngine;
using System.Collections;

public class chunkTester : MonoBehaviour 
{

	ChunkData info;
	void Start () 
	{
		info = new ChunkData(100);
		info.fillAllWithRandomPerlin(0.4f);
		gameObject.GetComponent<Renderer>().material.mainTexture = info.mapCells;
	}
	
}
