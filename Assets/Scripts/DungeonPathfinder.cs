using UnityEngine;
using System.Collections;

[RequireComponent(typeof(myDungeongen))]
public class DungeonPathfinder : MonoBehaviour
{
	public cell[,] map;
	protected int sizeX;
	protected int sizeZ;
	myDungeongen dungeon;
	void Start ()
	{

		gameObject.tag = "Map";
		setGrid ();
	}
	

	private void setGrid() //sets our grid based on the maps data
	{
		dungeon = GetComponent<myDungeongen> ();
		sizeX = dungeon.sizeX;
		sizeZ = dungeon.sizeZ;

		map = new cell[sizeX,sizeZ];
		for(int z = 0; z < sizeZ; z++)
		{
			for(int x = 0; x < sizeX; x++)
			{
				if(dungeon.map[x,z].tileAlias == myDungeongen.empty ||  dungeon.map[x,z].tileAlias == myDungeongen.roomCenter)
				{
					map[x,z] = new cell(); //impassible as these cells are coupied by gameobjects
				}
				else
				{
					map[x,z] = new cell(true);
				}
			}
		}
	}

	public cell[,] getArea(int lineOfSight,Vector3 worldCorrdinates) //builds an array of the necicary size and returns it to whom ever called this function
	{
		int xOffset = (Mathf.RoundToInt(worldCorrdinates.x) -lineOfSight/2); //bottom left if the world corrdinates are the center
		int zOffset = (Mathf.RoundToInt(worldCorrdinates.z) -lineOfSight/2);
		cell[,] output = new cell[lineOfSight,lineOfSight];

		for(int x=0; x < lineOfSight; x++)
		{
			for(int z =0; z < lineOfSight; z++)
			{

				if((x + xOffset) < sizeX && x + xOffset >= 0 && (z + zOffset) < sizeZ && z + zOffset >= 0 ) //if the element does not lie on the map do not try to check the map rather give a new cell and assume the empty space is not walkable
				{
					output[x, z] = map[x + xOffset, z + zOffset];
				}
				else
				{
					output[x,z] = new cell();
				}

			}
		}
		return(output);
	}


}
