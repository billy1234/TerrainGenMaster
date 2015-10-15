using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridAlgorithim : MonoBehaviour
{
	public List<City> cityDebug;
	void Start ()
	{
		detectRegions(GetComponent<CityWorld> ().getPopMap ());
	}

	void OnDrawGizmos()
	{
		if (Application.isPlaying && cityDebug.Count > 0) 
		{
			foreach(City c in cityDebug)
			{
				Gizmos.color = new Color(Random.Range(0f,1f),Random.Range(0f,1f),Random.Range(0f,1f));
				foreach(Index x in c.cityCells)
				{
					Gizmos.DrawCube(new Vector3(x.x,0,x.y),Vector3.one);
				}
			}
		}
	}

	void detectRegions (float[,] popGrid)
	{
		List<City> myCitys = new List<City>();
		List<Index> openCells = new List<Index> ();

		//initalize array
		for (int y =0; y < popGrid.GetLength(1); y++)
		{
			for (int x =0; x < popGrid.GetLength(0); x++) 
			{

				if (popGrid [x, y] > 0) 
				{ //dont bother ading cells with 0 population
					//print(popGrid [x, y]);
					openCells.Add (new Index (x, y));
				}
			}
		}


		while (openCells.Count >0) 
		{
			addCell (ref myCitys, openCells[0], new Vector2 (popGrid.GetLength (0), popGrid.GetLength (1)));
			Debug.Log("X: " + openCells[0].x +"Y:" + openCells[0].y);
			openCells.RemoveAt(0);//acts as the index for the loop
		}
		cityDebug = myCitys;
	}

	void addCell (ref List<City> myCitys, Index currentCell, Vector2 arraySize)
	{
		/*
		 * bug:
		 * cells are incrorecly being deteced in the down  ,down right and down left cells
		*/
		bool hasNeigbor = false;
		List<int> linkedCitys = new List<int> ();
		for (int i =0; i < Index.directions.Length; i++)
		{
			Index next = getIndex (currentCell, Index.directions[i], arraySize);
			if (next.legal) 
			{

				for (int cityIndex =0; cityIndex < myCitys.Count; cityIndex++)
				{
					for (int cityCells =0; cityCells < myCitys[cityIndex].cityCells.Count; cityCells++)
					{
						Index checkIndex = myCitys [cityIndex].cityCells[cityCells];
						if (checkIndex.x == next.x && checkIndex.y == next.y) //if the neigboring cell is aleady in a city
						{
							//its in a city and the city is not already defined
							if(!linkedCitys.Contains(cityIndex))
							{
								linkedCitys.Add (cityIndex);
							}
							hasNeigbor = true;
						}
					}
				}
			}
		}




		//if none of these fired make a new city

		if (!hasNeigbor)
		{
			print("this cell has no neigbor:  X:"+ currentCell.x +"Y: "+ currentCell.y);
			myCitys.Add (new City (currentCell));
		}
		else
		{
			City tempCity = new City(currentCell);//add the cell
			foreach(int cityRow in linkedCitys)
			{

				tempCity.cityCells.AddRange(myCitys[cityRow].cityCells.ToArray());

			}
			for(int i =0; i < linkedCitys.Count; i++)
			{
				myCitys.RemoveAt(linkedCitys[i]);//destory the original linked city
					for(int indexRow =i + 1; indexRow < linkedCitys.Count; indexRow++)
					{
						//print("index: "+index+"  linked count :"+linkedCitys.Count +" I:"+ i);
						//print("linkedcitty: "+ linkedCitys[index]);
						if(linkedCitys[indexRow] > linkedCitys[i])//if its past the one i just removed shift it down one
						{
							linkedCitys[indexRow] --;
						}

					}
			}
			myCitys.Add(tempCity);
			//sore all linked citys in a temp storage area

			//make a new city with all the cells
		}




		//if the cell neighbors one citys cell add it
		//if the cell neighbors 2 or more citys add them togeter and add the cell
		//if the cell neighros no cells make a new city and add the cell to it

	}

	Index getIndex (Index cell, Index direction, Vector2 arraySize)
	{
		Index newIndex  = new Index(cell.x,cell.y);
		cell.x =  cell.x + 	direction.x;
	 	cell.y = cell.y	 +  direction.y;

		newIndex.legal = (cell.x >= 0 && cell.x < (int)arraySize.x && cell.y >= 0 && cell.y < (int)arraySize.y) ;
		return newIndex;
	}


}


[System.Serializable]
public class Index
{
	public int x = 0, y = 0;
	public bool legal = true;
	public static readonly Index[] directions = new Index[8] 
	{
		new Index (0, 1),//up
		new Index (0, -1),//down

		new Index (-1, 0),//left
		new Index (1, 0),//right

		new Index (1, 1),//ne
		new Index (-1, -1),//sw

		new Index (1, -1),//se
		new Index (-1, 1)//nw
	};


	public Index (int _x, int _y)
	{
		x = _x;
		y = _y;
		legal = true;
	}
}

[System.Serializable]
public class City
{
	public List<Index> cityCells = new List<Index> ();

	public City (Index firstSquare)
	{
		cityCells.Add (firstSquare);
	}
}

