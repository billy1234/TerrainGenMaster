using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CityDetector : MonoBehaviour
{
	List<City> cityList;
	/*
	void Start ()
	{
		cityList.AddRange(detectRegions(GetComponent<TerrainGen> ().getPopMap ()));
	}
	*/

	/*
	void OnDrawGizmos()
	{
		if (Application.isPlaying && cityDebug.Count > 0) 
		{
			foreach(City c in cityList)
			{
				Gizmos.color = new Color(Random.Range(0f,1f),Random.Range(0f,1f),Random.Range(0f,1f));
				foreach(Index x in c.cityCells)
				{
					Gizmos.DrawCube(new Vector3(x.x - size/2 ,0,x.y - size/2 ),Vector3.one);
				}
			}
		}
	}
*/
	public static City[] getCityCells(TerrainGen terrain)
	{
		return detectRegions(terrain.getPopMap());
	}
	
	private static City[] detectRegions (float[,] popGrid)
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
			//Debug.Log("X: " + openCells[0].x +"Y:" + openCells[0].y);
			openCells.RemoveAt(0);//acts as the index for the loop
			//yield return new WaitForSeconds(0.5f);
			//cityDebug = myCitys;
		}
		return myCitys.ToArray();
	}

	static void addCell (ref List<City> myCitys, Index currentCell, Vector2 arraySize)
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
					for (int cityCell =0; cityCell < myCitys[cityIndex].cityCells.Count; cityCell++)
					{
						Index checkIndex = myCitys [cityIndex].cityCells[cityCell];
						if (checkIndex.x == next.x && checkIndex.y == next.y) //if the neigboring cell is aleady in a city 
						{
							//its in a city and the city is not already defined
							if(!linkedCitys.Contains(cityIndex))
							{
								linkedCitys.Add (cityIndex);
								hasNeigbor = true;
							}

						}
					}
				}
			}
		}




		//if none of these fired make a new city

		if (!hasNeigbor)
		{
			//print("this cell has no neigbor:  X:"+ currentCell.x +"Y: "+ currentCell.y);
			myCitys.Add (new City (currentCell));
		}
		else
		{
			City tempCity = new City(currentCell);//add the cell
			foreach(int cityRow in linkedCitys)
			{

				tempCity.cityCells.AddRange(myCitys[cityRow].cityCells.ToArray());

			}






			destroyCitys(linkedCitys.ToArray(),ref myCitys);//sort the index aray
			myCitys.Add(tempCity);

			//sore all linked citys in a temp storage area

			//make a new city with all the cells
		}




		//if the cell neighbors one citys cell add it
		//if the cell neighbors 2 or more citys add them togeter and add the cell
		//if the cell neighros no cells make a new city and add the cell to it

	}

	static void destroyCitys(int[] cityIndexes,ref List<City> cityArray)
	{
		for(int x=1; x <cityIndexes.Length;x++)
		{
			for(int i =1; i < cityIndexes.Length; i++) //sort to avoid the index pointing to the wrong element
			{
				if(cityIndexes[i -1] < cityIndexes[i])
				{
					int temp = cityIndexes[i];
					cityIndexes[i] = cityIndexes[i -1];
					cityIndexes[i -1] = temp;
				}
			}
		}

		for(int i =0; i < cityIndexes.Length; i++)
		{
			cityArray.RemoveAt(cityIndexes[i]);
		}
	}

	static Index getIndex (Index cell, Index direction, Vector2 arraySize)
	{
		Index newIndex  = new Index(cell.x + direction.x,cell.y + direction.y);

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
		new Index (0, 1),//n
		new Index (0, -1),//s (broken)

		new Index (-1, 0),//w
		new Index (1, 0),//e

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

