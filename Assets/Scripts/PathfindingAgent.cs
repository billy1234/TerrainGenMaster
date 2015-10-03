using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathfindingAgent : MonoBehaviour 
{
	private DungeonPathfinder pathfinder;
	private cell [,] myCells;
	public int myLineOfSight =5;
	public const int myValue = -420;
	public const int objective = 20;
	public const int staringSmallestValue = 100000000;
	public Vector3 target;
	public List<Vector3> path;
	void Start () 
	{
		pathfinder = GameObject.FindGameObjectWithTag ("Map").GetComponent<DungeonPathfinder> ();
		myCells = pathfinder.getArea (myLineOfSight, transform.position);
		addMeToGrid ();
		//myCells = setWalkValues(getCellFromWorld(target),myCells,0);
		path = getPath (target,10);
		debugPath ();

		foreach (Vector3 node in path)
		{
			print (node);
		}
	}

	void addMeToGrid()
	{
		getCellFromWorld (transform.position, myValue);
	}

	void OnDrawGizmos()
	{
		/* if(Application.isPlaying)
		{
			for(int z =0; z < myLineOfSight; z++)
			{
				for(int x =0; x < myLineOfSight; x++)
				{
					Vector3 pos = new Vector3(x - myLineOfSight/2,0,z - myLineOfSight/2) + transform.position;

					if(myCells[x,z].value == myValue)
					{
						Gizmos.color = Color.green;
						Gizmos.DrawCube(pos,Vector3.one * 0.9f);
					}
					else if(myCells[x,z].walkable == true)
					{
						Gizmos.color = Color.blue;
						Gizmos.DrawCube(pos,Vector3.one * 0.9f);
					}
					else
					{
						Gizmos.color = Color.red;
						Gizmos.DrawCube(pos,Vector3.one * 0.9f);
					}
				}
			}
			foreach (Vector3 node in path)
			{
				Gizmos.color = Color.white;
				Gizmos.DrawCube(node,Vector3.one * 0.5f);
			}
		}

		for (int z =0; z < myLineOfSight; z++)
		{
			for (int x =0; x < myLineOfSight; x++)
			{
				Vector3 pos = new Vector3 (x - myLineOfSight / 2, 0, z - myLineOfSight / 2) + transform.position;
				float colorValue = myCells [x, z].value / 10f;
				Gizmos.color = new Color (colorValue, colorValue, colorValue);
				Gizmos.DrawCube (pos, Vector3.one * 0.9f);
			}
		}
		*/
	}

	void debugPath()
	{
		for (int z =0; z < myLineOfSight; z++)
		{
			for (int x =0; x < myLineOfSight; x++)
			{
				Vector3 pos = new Vector3 (x - myLineOfSight / 2, 0, z - myLineOfSight / 2) + transform.position;
				GameObject temp = new GameObject("debug object");
				temp.transform.position = pos;
				TextMesh text = temp.AddComponent<TextMesh>();
				text.text= "["+myCells[x,z].value.ToString()+"]" +myCells[x,z].walkable.ToString();
				temp.transform.Rotate(90,0,0);
				text.color = Color.black;
				text.characterSize = 0.1f;
				text.alignment = TextAlignment.Center;
			}
		}
	}

	Vector2 getCellFromWorld(Vector3 worldCorrdinates)
	{
		int x = myLineOfSight / 2; //bottom left if the world corrdinates are the center
		int z = myLineOfSight / 2;
		x = Mathf.Clamp (x, 0, myLineOfSight);
		z = Mathf.Clamp (z, 0, myLineOfSight); //clamps the values
		return new Vector2 (x, z);
	}

	Vector2 getCellFromWorld(Vector3 worldCorrdinates, int newValue)
	{
		Vector2 temp = getCellFromWorld (worldCorrdinates);
		myCells[(int)temp.x,(int)temp.y] .value = newValue;
		return temp;
	}
	

	public List<Vector3> getPath(Vector3 destination,int maxSteps)
	{
		cell[,] pathfindingCells = myCells;
		Vector2 destinationCell = getCellFromWorld (destination);
		pathfindingCells = setWalkValues (destinationCell, pathfindingCells, 0); 
		//for debuging
	
		pathfindingCells [(int)destinationCell.x, (int)destinationCell.y].value = objective;
		myCells = pathfindingCells;
		List<Vector3> nodes = new List<Vector3> ();
		//we then walk from the current pos to the player
		Vector2 currentNode = destinationCell;
		for(int steps =0;steps < maxSteps; steps++) 
		{	
			if(currentNode == destinationCell)
			{
				//break; //stop looking our path is complete
			}
			nodes.Add(currentNode);  //the node we will finish on
			currentNode = findNextNode (currentNode, pathfindingCells);//we then step to the next node

		}
		nodes.Reverse ();//the path os from the objective to us so we flip so the 1st node is our first step not the destations 1st step
		return nodes;

	}

	private cell[,] setWalkValues(Vector2 location, cell[,] map,int steps)
	{
		int x = (int)location.x;
		int z = (int)location.y;

		steps++;

		int nextX =0, nextZ =0;

		nextX = 1;
		nextZ = 0;

		if(checkIndex(nextX + x)&& checkIndex(nextZ+ z) )
		{

			if(map[nextX + x,nextZ+ z].walkable == true && map[nextX + x,nextZ+ z].value > steps )
			{
				
				map[nextX + x,nextZ+ z].value = steps;
				map = setWalkValues(new Vector2(nextX + x,nextZ+ z),map,steps);
			}
		}

		nextX = -1;
		nextZ = 0;

		if(checkIndex(nextX + x)&& checkIndex(nextZ+ z) )
		{
			if(map[nextX + x,nextZ+ z].walkable == true && map[nextX + x,nextZ+ z].value > steps )
			{
				
				map[nextX + x,nextZ+ z].value = steps;
				map = setWalkValues(new Vector2(nextX + x,nextZ+ z),map,steps);
			}
		}

		nextX = 0;
		nextZ = 1;

		if(checkIndex(nextX + x)&& checkIndex(nextZ+ z) )
		{
			if(map[nextX + x,nextZ+ z].walkable == true && map[nextX + x,nextZ+ z].value > steps )
			{
				
				map[nextX + x,nextZ+ z].value = steps;
				map = setWalkValues(new Vector2(nextX + x,nextZ+ z),map,steps);
			}
		}

		nextX = 0;
		nextZ = -1;

		if(checkIndex(nextX + x)&& checkIndex(nextZ+ z) )
		{
			if(map[nextX + x,nextZ+ z].walkable == true && map[nextX + x,nextZ+ z].value > steps )
			{
				
				map[nextX + x,nextZ+ z].value = steps;
				map = setWalkValues(new Vector2(nextX + x,nextZ+ z),map,steps);
			}
		}
		return map;
	}
	



	Vector2 findNextNode(Vector2 currentNode,cell[,] map)
	{
		int x = (int)currentNode.x;
		int z = (int)currentNode.y;	
		int smallestValue = staringSmallestValue;

		int nextX =0, nextZ =0;

		nextX = 1;
		nextZ = 0;

		Vector2 nextNode = Vector2.zero;

		if(checkIndex(nextX + x)&& checkIndex(nextZ+ z) )
		{
			if(map[x + nextX,z + nextZ].value < smallestValue && map[x + nextX,z + nextZ].walkable == true)
			{
				nextNode = new Vector2(x + nextX,z + nextZ);
				smallestValue = myCells[x + nextX,z + nextZ].value;
			}
		}
		
		nextX = -1;
		nextZ = 0;
		
		if(checkIndex(nextX + x)&& checkIndex(nextZ+ z) )
		{
			if(map[x + nextX,z + nextZ].value < smallestValue && map[x + nextX,z + nextZ].walkable == true)
			{
				nextNode = new Vector2(x + nextX,z + nextZ);
				smallestValue = myCells[x + nextX,z + nextZ].value;
			}
		}
		
		nextX = 0;
		nextZ = 1;
		
		if(checkIndex(nextX + x)&& checkIndex(nextZ+ z) )
		{
			if(map[x + nextX,z + nextZ].value < smallestValue && map[x + nextX,z + nextZ].walkable == true)
			{
				nextNode = new Vector2(x + nextX,z + nextZ);
				smallestValue = myCells[x + nextX,z + nextZ].value;
			}
		}
		
		nextX = 0;
		nextZ = -1;
		
		if(checkIndex(nextX + x)&& checkIndex(nextZ+ z) )
		{
			if(map[x + nextX,z + nextZ].value < smallestValue && map[x + nextX,z + nextZ].walkable == true)
			{
				nextNode = new Vector2(x + nextX,z + nextZ);
				smallestValue = myCells[x + nextX,z + nextZ].value;

			}
		}
		return nextNode;
	}

	bool checkIndex(int index)
	{
		return(index >= 0 && index < myLineOfSight);
	}




}
