using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
public enum direction
{
	up,
	down,
	left,
	right
};
public enum  tileState
{
	empty,
	crossroad,
	road,
};
public class CityGenerator : MonoBehaviour
{
	public int sizeX;
	public int sizeY;
	public int rrtPasses =1;
	List<tile> myTiles;
	tile[,] mapData; //once the abve list is used it it placed into here using the fuction store tiles
	public string seed;
	public bool randomSeed = false;
	System.Random PseudoRng;

	public GameObject crossRoadTile;
	public GameObject emptyTile;
	public GameObject roadTile;
	void Start () 
	{

		if (randomSeed == true)
		{
			seed = (System.DateTime.Now.Second * System.DateTime.Now.Millisecond  + System.DateTime.Now.Millisecond).ToString();
		}

		PseudoRng = new System.Random(seed.GetHashCode());
		myTiles = rrtPass (rrtPasses);
		storeTiles ();
		connectRoadTiles ();
		spawnTiles ();
	}
	void OnDrawGizmos()
	{
		if (Application.isPlaying) 
		{
			Gizmos.color = Color.green;
			drawNodes ();
		}
	}
	void storeTiles()
	{
		mapData = new tile[sizeX, sizeY];
		for(int y = 0; y < sizeY; y++)
		{
			for(int x = 0; x < sizeX; x++)
			{
				foreach(tile tile in myTiles)
				{
					if(tile.x == x && tile.y == y)
					{
						mapData[x,y]= tile;
						tile.tileContents = tileState.crossroad;
						break; //stops the loop because we fount the element
					}
				}
				if(mapData[x,y] == null)
				{
					mapData[x,y] = new tile(x,y);
				}
			}
		}
	}
	void drawNodes()
	{
		foreach(tile node in myTiles)
		{
			if(node.parent != null)
			{
				Gizmos.DrawLine(new Vector3(node.x,0,node.y),new Vector3(node.parent.x,0,node.parent.y));
			}
		}
	}


	List<tile> rrtPass(int passes)
	{
		//int[,] mapData = new int[sizeX,sizeY];//

		List<tile> nodes = new List<tile>();
		nodes.Add(new tile (sizeX / 2, sizeY / 2)); //starts with the center of the map
		for (int i = 0; i < passes; i++)
		{
			//gets a random point
			tile newNode = getRandomCoord();

			//sets smallest distance to infinity so the first pass of the loop will store the first distance as it will be less than infinity
			float smallestDistance = Mathf.Infinity;

			//stores the closest nodes index outsdie of the loop so we can use it later 
			int closestNodeIndex =-1;

			for(int index = 0; index < nodes.Count; index++)
			{
				float distance = checkDistance(newNode,nodes[index]);
				if( distance < smallestDistance)
				{
					closestNodeIndex = index;
					smallestDistance = distance;

				}

			}
			newNode.parent = nodes[closestNodeIndex];
			nodes.Add(newNode);
		}

		return nodes;

		
				/*
			RRT;
			Pick a random sample in the search space.
				
			Find the nearest neighbor of that sample.
					
			Select an action from the neighbor that heads towards the random sample.
					
			Create a new sample based on the outcome of the action applied to the neighbor.
					
			Add the new sample to the tree, and connect it to the neighbor.
			*/

	}

	//finding the closest point
	//interate throught all points
	//if a point is closer store
	//loop
	//once you have gone through all of the points
	//the last stored point is the closest

	float checkDistance(tile target, tile source)
	{
		return Mathf.Pow(target.x - source.x, 2) + Mathf.Pow(target.y - source.y, 2);
	}

	tile getRandomCoord()
	{
		tile coordinate = new tile (PseudoRng.Next (0, sizeX),PseudoRng.Next(0, sizeY));
		return coordinate;
	}
	



	
	List<direction> walkToCoordinate (int x1,int y1,int x2,int y2) //bugged x1 and y 1 are flipped
	{
		int horizontal = x2 - x1;
		int vertical = y2 - y1;
		List<direction> orders = giveDirections (horizontal, vertical);
		orders = randomizeOrders (orders);
		return orders;
	}
	
	List<direction>giveDirections (int vertical, int horizontal) 
	{
		List<direction> directions = new List<direction> ();
		if(vertical >= 0)
		{
			for(int i =0; i < vertical; i++)
			{
				directions.Add(direction.up);
			}
		}
		else
		{
			for(int i =0; i > vertical; i--)
			{
				directions.Add(direction.down);
			}
		}
		
		
		if(horizontal >= 0)
		{
			for(int i =0; i < horizontal; i++)
			{
				directions.Add(direction.right);
			}
		}
		else
		{
			for(int i =0; i > horizontal; i--)
			{
				directions.Add(direction.left);
			}
		}
		return directions;
	}

	List<direction> randomizeOrders(List<direction> oldList) //jumbles our list to make our roads intresting
	{
		for( int i = 0; i < oldList.Count; i++)
		{
			direction temp = oldList[i];
			int randomIndex =  PseudoRng.Next(0,oldList.Count);
			oldList[i] = oldList[randomIndex];
			oldList[randomIndex] = temp;
		}
		return oldList;
	}

	void connectRoadTiles()
	{
		foreach (tile node in myTiles) 
		{
			if(node.parent != null)
			{
				List<direction> walkOrders =  walkToCoordinate(node.y,node.x,node.parent.y,node.parent.x);
				int currentX = node.x;
				int currentY = node.y;
				foreach(direction order in walkOrders)
				{
					switch(order)
					{

					case(direction.up):
						currentY += 1;
							break;
					case(direction.down):
						currentY -= 1;
						break;
					case(direction.left):
						currentX -= 1;
						break;
					case(direction.right):
						currentX += 1;
						break;
					}
					if(mapData[currentX,currentY].tileContents != tileState.crossroad)
					{
						if (mapData[currentX,currentY].tileContents == tileState.road) 
						{
							mapData[currentX,currentY].tileContents = tileState.crossroad;
						}
						else
						{
							mapData[currentX,currentY].tileContents = tileState.road;
						}
					}
				}

			}
		}
	}
	void spawnTiles()
	{
		foreach (tile element in mapData) 
		{
			switch(element.tileContents)
			{

			case(tileState.empty):
				Instantiate(emptyTile,new Vector3(element.x,0,element.y),Quaternion.identity);
				break;
			case(tileState.crossroad):
				Instantiate(crossRoadTile,new Vector3(element.x,0,element.y),Quaternion.identity);
				break;
			case(tileState.road):
				Instantiate(roadTile,new Vector3(element.x,0,element.y),Quaternion.identity);
				break;
			}
		}
	}






	//a way to store my in x and y in one variable
	public class tile 
	{
		public int x;
		public int y;
		public tile parent;
		public tileState tileContents;
		public tile(int _x, int _y)
		{
			x = _x;
			y = _y;
			tileContents = tileState.empty;
		}
	}
	













}
