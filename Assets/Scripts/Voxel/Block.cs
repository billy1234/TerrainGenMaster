using UnityEngine;
using System.Collections;



public class Block 
{
	public enum Direction {	north, east, south, west, up, down };

	public Block()
	{
		
	}
	
	public virtual MeshData Blockdata(Chunk chunk, int x, int y, int z, MeshData meshData)
	{
		
		if (!chunk.GetBlock(x, y + 1, z).IsSolid(Direction.down))
		{
			meshData = FaceDataUp(chunk, x, y, z, meshData);
		}
		
		if (!chunk.GetBlock(x, y - 1, z).IsSolid(Direction.up))
		{
			meshData = FaceDataDown(chunk, x, y, z, meshData);
		}
		
		if (!chunk.GetBlock(x, y, z + 1).IsSolid(Direction.south))
		{
			meshData = FaceDataNorth(chunk, x, y, z, meshData);
		}
		
		if (!chunk.GetBlock(x, y, z - 1).IsSolid(Direction.north))
		{
			meshData = FaceDataSouth(chunk, x, y, z, meshData);
		}
		
		if (!chunk.GetBlock(x + 1, y, z).IsSolid(Direction.west))
		{
			meshData = FaceDataEast(chunk, x, y, z, meshData);
		}
		
		if (!chunk.GetBlock(x - 1, y, z).IsSolid(Direction.east))
		{
			meshData = FaceDataWest(chunk, x, y, z, meshData);
		}
		
		return meshData;
		
	}

	protected virtual MeshData FaceDataUp(Chunk chunk, int x, int y, int z, MeshData meshData)
	{
		meshData.vertices.Add(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
		meshData.vertices.Add(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
		meshData.vertices.Add(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
		meshData.vertices.Add(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
		
		meshData.AddQuadTriangles();
		
		return meshData;
	}

	protected virtual MeshData FaceDataDown	(Chunk chunk, int x, int y, int z, MeshData meshData)
	{
		meshData.vertices.Add(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
		meshData.vertices.Add(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
		meshData.vertices.Add(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
		meshData.vertices.Add(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
		
		meshData.AddQuadTriangles();
		return meshData;
	}
	
	protected virtual MeshData FaceDataNorth(Chunk chunk, int x, int y, int z, MeshData meshData)
	{
		meshData.vertices.Add(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
		meshData.vertices.Add(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
		meshData.vertices.Add(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
		meshData.vertices.Add(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
		
		meshData.AddQuadTriangles();
		return meshData;
	}
	
	protected virtual MeshData FaceDataEast
		(Chunk chunk, int x, int y, int z, MeshData meshData)
	{
		meshData.vertices.Add(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
		meshData.vertices.Add(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
		meshData.vertices.Add(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
		meshData.vertices.Add(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
		
		meshData.AddQuadTriangles();
		return meshData;
	}
	
	protected virtual MeshData FaceDataSouth
		(Chunk chunk, int x, int y, int z, MeshData meshData)
	{
		meshData.vertices.Add(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
		meshData.vertices.Add(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
		meshData.vertices.Add(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
		meshData.vertices.Add(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
		
		meshData.AddQuadTriangles();
		return meshData;
	}
	
	protected virtual MeshData FaceDataWest
		(Chunk chunk, int x, int y, int z, MeshData meshData)
	{
		meshData.vertices.Add(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
		meshData.vertices.Add(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
		meshData.vertices.Add(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
		meshData.vertices.Add(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
		
		meshData.AddQuadTriangles();
		return meshData;
	}

	public virtual bool IsSolid(Direction direction)
	{
		switch(direction){
		case Direction.north:
			return true;
		case Direction.east:
			return true;
		case Direction.south:
			return true;
		case Direction.west:
			return true;
		case Direction.up:
			return true;
		case Direction.down:
			return true;
		}
		
		return false;
	}

}
