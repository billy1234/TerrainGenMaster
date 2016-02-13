using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public enum FLOORTYPE
{
	EMPTY,BASIC,HALLWAY,STAIRS,
};
public class FloorPlans : MonoBehaviour
{
	//public List<Vector2> points;
	//public List<int> walls;

	public structureGridMaker s;

	void Start()
	{
		//addSquare(new Vector2[4]{new Vector2(0,0),new Vector2(0,1),new Vector2(1,1),new Vector2(1,0)});
		//s = new structureGridMaker(10,8); BROKEN DO NOT RUN
	}
	/*
	
	void draw()
	{
		Gizmos.color = Color.blue;
		for(int i=0; i < walls.Count; i+= 2)
		{
			Vector3 worldFirst = new Vector3(points[walls[i]].x,0,points[walls[i]].y);
			Vector3 worldSecond =  new Vector3(points[walls[i + 1]].x,0,points[walls[i + 1]].y);
			Gizmos.DrawLine(worldFirst,worldSecond);
		}
	}

	/// <summary>
	/// will add a square draw clockwise from the bottom left point.
	/// </summary>
	/// <param name="newPoints">New points.</param>
	void addSquare(Vector2[] newPoints)
	{
		if(newPoints.Length != 4)
		{
			Debug.LogError("to add a square 4 points are needed you provided" + newPoints.Length);
			return;
		}
		walls.AddRange(new int[8]{walls.Count ,walls.Count + 1,walls.Count + 1,walls.Count + 2,walls.Count + 2,walls.Count + 3,walls.Count + 3,walls.Count});
		points.AddRange(newPoints);
	}
	*/
	void OnDrawGizmos()
	{
		if(Application.isPlaying)
		{
			s.draw();
			//draw();
		}
	}

	/*
	 * place main entrance
	 * set dimentionsns x z
	 * walk untill a wall is hit
	 * this path is now the main hallway
	 * 
	*/
}

public class structureGridMaker
{
	public FLOORTYPE[,] floorValues = null;
	int sizeX =0;
	int sizeY =0;
	//normalizedd bettween 0-1 that represent percentages
	readonly float minHallwaySpan =0.3f;
	readonly float maxHallwaySpan = 0.7f;
	readonly float minRoomSize = 0.3f;
	readonly float maxRoomSize = 0.5f;
	public structureGridMaker(int _sizeX, int _sizeY)
	{
		this.floorValues = new FLOORTYPE[_sizeX,_sizeY];
		sizeX = _sizeX;
		sizeY = _sizeY;
		for(int y =0; y < sizeY; y++)
		{
			for(int x =0; x < sizeX; x++)
			{
				floorValues[x,y] = FLOORTYPE.EMPTY;
			}
		}
		generateHallway();
		addRooms();
	}

	void generateHallway()
	{
		int snakeMinWalk = Mathf.RoundToInt(minHallwaySpan * sizeY);
		Vector2 snakeDiretion = Vector2.up;
		int snakeX = floorValues.GetLength(0) /2;
		int snakeY =0;
		bool hitEdge = false;
		while(hitEdge == false)
		{
			floorValues[snakeX,snakeY] = FLOORTYPE.HALLWAY;
			getRandomDirection(ref snakeMinWalk,ref snakeDiretion);
			snakeX += (int)snakeDiretion.x;
			snakeY += (int)snakeDiretion.y;
			if(snakeX == -1 || snakeY ==-1|| snakeX == sizeX|| snakeY == sizeY)
			{
				hitEdge = true;
			}
		}
	}

	void addRooms()
	{
		List<room> houseRooms = new List<room>();
	
		int indexX =0;
		int indexY =0;

		int minSize = Mathf.RoundToInt(minRoomSize * (sizeX+ sizeY)/2f);

		
		bool[,] legalStarts = new bool[sizeX,sizeY];
		#region initalize array
		for(int y =0; y < sizeY; y++)
		{
			for(int x =0; x < sizeX; x++)
			{
				if(floorValues[x,y] == FLOORTYPE.HALLWAY)
				{
					legalStarts[x,y] = false;
				}
				else
				{
					legalStarts[x,y] = true;
				}
			}
		}
		#endregion

		bool finished = false;

		while(finished == false)
		{
			int maxX = Mathf.RoundToInt(maxRoomSize * (sizeX+ sizeY)/2f);
			int maxY = Mathf.RoundToInt(maxRoomSize * (sizeX+ sizeY)/2f);
			getRoomCoordinates(ref indexX,ref indexY,ref legalStarts,ref finished,ref maxX,ref maxY);
			room roomToAdd =new room(indexX,indexY,Random.Range(minSize,maxX),Random.Range(minSize,maxY));
			addRoomToArray(roomToAdd);
			houseRooms.Add(roomToAdd);
			//finished = true;
		}



		
	}
	void addRoomToArray(room r)
	{
		for(int y =0; y < r.sizeY; y++)
		{
			for(int x =0; x < r.sizeX; x++)
			{
				floorValues[r.x + x,r.y + y] = FLOORTYPE.BASIC;
			}
		}
	}
	void getRoomCoordinates(ref int indexX, ref int indexY, ref bool[,] legalArray,ref bool finished,ref int maxX,ref int maxY)
	{

	}

	void getRandomDirection(ref int newMinWalk,ref Vector2 direction)
	{
		if(newMinWalk > 0)
		{
			newMinWalk --;
		}
		else
		{
			if(direction.x == 0)
			{
				direction.y =0;
				direction.x = 1;
				if(Random.Range(0,2) == 0)
				{
					direction.x = -direction.x;
				}

			}
			else
			{
				direction.x =0;
				direction.y = 1;
				if(Random.Range(0,2) == 0)
				{
					direction.y = -direction.y;
				}
			}
			int avSideLength = (sizeX + sizeY)/2;
			newMinWalk = Mathf.RoundToInt(Random.Range(minHallwaySpan,maxHallwaySpan) * avSideLength);
		}
	}

	public void draw()
	{
		for(int y =0; y < sizeY; y++)
		{
			for(int x =0; x < sizeX; x++)
			{
				FLOORTYPE t = floorValues[x,y];

				if(t == FLOORTYPE.HALLWAY)
				{
					Gizmos.color = Color.blue;
				}
				else
				{
					Gizmos.color = Color.black;
				}
				Gizmos.DrawCube(new Vector3(x,0,y),Vector3.one);
				
			}
		}
	}
	
}

public class room
{
	public int x =0;
	public int y =0;
	public int sizeX =0;
	public int sizeY =0;

	public room(int x,int y,int sizeX ,int sizeY )
	{
		this.x = x;
		this.y = y;
		this.sizeX = sizeX;
		this.sizeY  = sizeX;
	}

	public bool isColliding(room otherRoom)//untested
	{
		return(x + sizeX >= otherRoom.x && x <= otherRoom.x + otherRoom.sizeX&&y + sizeY >= otherRoom.y && y <= otherRoom.y + otherRoom.sizeY);
	
	}

}
