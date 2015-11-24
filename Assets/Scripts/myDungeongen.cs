using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public struct Tile
{
	public string tileAlias;
	public GameObject worldIntance;

}

public struct corrdinate2d
{
	public int x;
	public int y;
}
public class myDungeongen : MonoBehaviour 
{

	public Tile [,] map;
	const string walkWay = "w";
	const string room = "r";

	public const string empty = "e";
	public const string roomCenter = "rs";



	public int sizeX =5,sizeZ = 5;
	public int rooms =2;

	public GameObject emptyTile;
	public GameObject walkWayTile;
	public GameObject roomTile;
	public GameObject enemySpawnerTile;

	public int maxWidth;
	public int minWidth;
	public int minHeight;
	public int maxHeight;
	public GameObject[] loot;
	[Range(0,1)]
	public float lootChance;

	public string seed;
	public bool randomSeed;
	void Start () 
	{
		seedRng ();

		initalizeMap();
		placeRooms ();
		connectAllRooms ();
		spawnMap ();
	}
	void seedRng()
	{
		if (randomSeed || seed == "")
		{
			seed = (System.DateTime.Now.Second +  System.DateTime.Now.Millisecond *  System.DateTime.Now.Second).ToString();
		}
		Random.seed = seed.GetHashCode();
	}
	void initalizeMap()
	{
		map = new Tile[sizeX, sizeZ];
		for (int z =0; z < sizeZ; z++) 
		{
			for (int x =0; x < sizeX; x++)
			{

				map [x, z].tileAlias = empty;
			}
		}
	}

	void spawnMap()
	{
		for (int z =0; z < sizeZ; z++) 
		{
			for (int x =0; x < sizeX; x++) 
			{
				map[x,z].worldIntance = spawnTile(map[x,z].tileAlias,new Vector3(x,0,z));
			}
		}
	}

	void placeRooms()
	{
		for (int i =0; i < rooms; i++)
		{
			setRoomTiles(Random.Range(minWidth,maxWidth),Random.Range(minHeight,maxHeight),randomIndexX(),randomIndexZ());
		}
	}

	void setRoomTiles(int width, int height, int centerX, int centerZ)
	{
		map [centerX, centerZ].tileAlias = roomCenter;
		for (int x =  centerX - width/2; x < centerX + width/2; x++) 
		{
			for (int z = centerZ - height/2; z < centerZ+  height/2; z++) 
			{
				if(checkX(x) && checkZ(z) && map[x,z].tileAlias == empty) //if the tile is on the map
				{
					map[x,z].tileAlias = room;
				}
			}
		}
	}

	int randomIndexX()
	{
		return Random.Range (0, sizeX);
	}

	int randomIndexZ()
	{
		return Random.Range (0, sizeZ);
	}

	GameObject spawnTile(string type, Vector3 position)
	{
		switch(type)
		{
			case empty:
				return spawn(emptyTile,position);
				break;
			case room:
				if(Random.Range(0f,1f) < lootChance)
				{	
					spawn(loot[Random.Range(0,loot.Length)],position);
				}
				return spawn(roomTile,position);
				break;
			case roomCenter: //add an enemy
				return spawn(enemySpawnerTile,position);
				break;
			case walkWay: 
				return spawn(walkWayTile,position);
				break;
			default:
				print (type);
					return null;
				break;
		}
	}

	GameObject spawn(GameObject prefab, Vector3 position)
	{
		return Instantiate (prefab,position + prefab.transform.position + transform.position , prefab.transform.rotation)as GameObject;
	}
	
	bool checkX(int index)
	{
		if (index >= 0 && index < sizeX)
		{
			return true;
		}
		return false;
	}

	bool checkZ(int index)
	{
		if (index >= 0 && index < sizeZ)
		{
			return true;
		}
		return false;
	}
	void connectAllRooms()
	{
		List<corrdinate2d> roomCentersIndexes = new List<corrdinate2d> ();
		for (int z =0; z < sizeZ; z++) 
		{
			for (int x =0; x < sizeX; x++) 
			{
				if(map[x,z].tileAlias == roomCenter)
				{
					corrdinate2d index;
					index.x = x;
					index.y =z;
					roomCentersIndexes.Add(index);
				}

			}
		}

		foreach (corrdinate2d roomCenterIndex in roomCentersIndexes) 
		{
			int randomRoom = Random.Range(0, roomCentersIndexes.Count); //what we connect our room too
			if(roomCentersIndexes[randomRoom].x == roomCenterIndex.x && roomCentersIndexes[randomRoom].y == roomCenterIndex.y) //ensures the room doesnt connect to its self
			{
				randomRoom ++;
				if(randomRoom >= roomCentersIndexes.Count)
				{
					randomRoom = 0;
				}
			}
			connectRooms(roomCenterIndex.x,roomCenterIndex.y, roomCentersIndexes[randomRoom].x,roomCentersIndexes[randomRoom].y);
		}
	}
	void connectRooms(int x1, int y1, int x2, int y2)
	{
		List<corrdinate2d> directions = new List<corrdinate2d> ();
		int xDistance = x2 - x1;
		int yDistance = y2 - y1;
		while(xDistance != 0)
		{
			if(xDistance >= 0)
			{
				corrdinate2d diretion;
				diretion.x = 	1;
				diretion.y =	0;
				directions.Add( diretion);
				xDistance--;
			}
			else
			{
				corrdinate2d diretion;
				diretion.x = 	-1;
				diretion.y = 	0;
				directions.Add(diretion);
				xDistance++;
			}
		}
		while(yDistance != 0)
		{
			if(yDistance >= 0)
			{
				corrdinate2d diretion;
				diretion.x = 	0;
				diretion.y =	1;
				directions.Add( diretion);
				yDistance --;
			}
			else
			{
				corrdinate2d diretion;
				diretion.x = 	0;
				diretion.y =	-1;
				directions.Add(diretion);
				yDistance ++;
			}
		}
		directions = jumbleList(directions);
		int x = x1;
		int y = y1;
		foreach(corrdinate2d order in directions)
		{
			x += order.x;
			y += order.y;
			if(checkX(x) &&checkZ(y))
			{
				if(map[x,y].tileAlias == empty)
				{
					map[x,y].tileAlias = walkWay;
				}
			}
		}
	}

	List<corrdinate2d> jumbleList(List<corrdinate2d> oldList)
	{
		for (int i =0; i < oldList.Count; i++) 
		{
			corrdinate2d temp = oldList[i];
			int randomIndex = Random.Range(0,oldList.Count);
			oldList[i] = oldList[randomIndex];
			oldList[randomIndex] = temp;
		}
		return oldList;
	}


}
