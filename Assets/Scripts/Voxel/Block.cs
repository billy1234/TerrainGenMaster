using UnityEngine;
using System.Collections;

public enum Direction 
{
	north, east, south, west, up, down 
};

public class Block 
{
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
	public Block()
	{
		
	}
	
	public virtual MeshData Blockdata(Chunk chunk, int x, int y, int z, MeshData meshData)
	{
		return meshData;
	}
}
