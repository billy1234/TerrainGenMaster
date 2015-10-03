using UnityEngine;
using System.Collections;

public class ChunkGenerator
{
	protected int x { get; set;}
	protected int y {get; set;}
	protected int chunkSize;

	public ChunkGenerator(int _x, int _y,int _chunkSize)
	{
		x = _x;
		y = _y;
		chunkSize = _chunkSize;
	}

	public char[,] buildChunk() //build chunk with no seed passed will use the old x,y
	{
		return buildChunk (x, y);
	}


	public char[,] buildChunk(int x, int y)
	{
		char[,] data =new char[chunkSize,chunkSize];
	 	setSeed ();
		data = setAll (data,'e');

		return data;
	}

	private void setSeed()
	{
		Random.seed = x + y * 100000;//the first 5 digits fo the seed are resevred for x the last 5 are for y
	
	}
	private char[,] setAll(char[,] oldArray, char letter)
	{
		for(int y =0; y < oldArray.GetLength(1); y++)
		{
			for(int x =0; x < oldArray.GetLength(0); x++)
			{
				oldArray[x,y] = letter;
			}
		}
		return oldArray;
	}
}
