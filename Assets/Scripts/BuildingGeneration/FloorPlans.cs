using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public enum FLOORTYPE
{
	BASIC,HALLWAY,STAIRS
};
public class FloorPlans : MonoBehaviour
{
	public List<Vector2> points;
	public List<int> walls;
	public structureGridMaker s;
	void Start()
	{
		//addSquare(new Vector2[4]{new Vector2(0,0),new Vector2(0,1),new Vector2(1,1),new Vector2(1,0)});
		s = new structureGridMaker(10,8);
	}
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

	void OnDrawGizmos()
	{
		if(Application.isPlaying)
		{
			s.draw();
			draw();
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
	readonly int minHallwaySpan =3;
	readonly int maxHallwaySpan = 5;
	public structureGridMaker(int _sizeX, int _sizeY)
	{
		this.floorValues = new FLOORTYPE[_sizeX,_sizeY];
		sizeX = _sizeX;
		sizeY = _sizeY;
			for(int y =0; y < sizeY; y++)
			{
				for(int x =0; x < sizeX; x++)
				{
					floorValues[x,y] = FLOORTYPE.BASIC;
				}
			}
		generateValues();
	}

	void generateValues()
	{
		int snakeMinWalk = minHallwaySpan;
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
			newMinWalk = Random.Range(minHallwaySpan,maxHallwaySpan);
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
