using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]

public class Chunk : MonoBehaviour 
{
	Block[ , , ] blocks;
	public static int chunkSize = 16;
	public bool update = true;

	public Block GetBlock(int x, int y, int z)
	{
		return blocks[x, y, z];
	}
	
}
