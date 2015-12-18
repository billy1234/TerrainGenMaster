using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(TerrainGen))]
public class TownGenerator : MonoBehaviour 
{
	public float buildingDensityMultiplier = 5f;
	public int maxBuildings =10;
	public int minBuildings =2;
	public TownStructure[] debug;
	void Start()
	{
		generateTowns();
	}

	void generateTowns()
	{
		//first weight cells based on the size of the city and the amount of neighboring cells
		#region get Values from terrain gen script
		TerrainGen terrain =  GetComponent<TerrainGen>();
		int mapSize = terrain.mapSize;
		City[] citys = CityDetector.getCityCells(terrain);
		#endregion

		TownStructure[,] townCells = new TownStructure[mapSize,mapSize];
		for(int y =0; y < mapSize; y++) //have to manually initalize all the elemnets or i will get a null error
		{
			for(int x =0; x < mapSize; x++)
			{
				townCells[x,y] = new TownStructure();
			}
		}
		//print(townCells[0,0]);
		List<TownStructure> townList = new List<TownStructure>();
		int totalCells =0;

		#region townstructure in a 2d array so neighbors can be checked then take the cells with values back into a list
		for(int i=0; i < citys.Length; i++)
		{
			for(int c =0; c < citys[i].cityCells.Count; c++)
			{
				Index cellIndex = citys[i].cityCells[c];
				//print(cellIndex);
				//print (cellIndex.x+",  "+cellIndex.y+", C:"+c +", I:"+i);
				townCells[cellIndex.x,cellIndex.y].neigboringCells = 0.1f; //just to flag the cell as not empty
				townCells[cellIndex.x,cellIndex.y].citySize = citys[i].cityCells.Count;
				townCells[cellIndex.x,cellIndex.y].position = cellIndex;
			}
		}

		for(int y=0; y < mapSize; y++) //kind of redundant as we iterate over the cells in the previous algorithim
		{
			for(int x =0; x < mapSize; x++)
			{
				if(townCells[x,y].neigboringCells != 0) //if the cells value is zero it means that the cell is empty and any more checks are redundant
				{
					int neighborCount =0;
					for(int i=0; i < Index.directions.Length; i++)
					{
						Index neighboringCell = Index.getIndex(new Index(x,y),Index.directions[i],new Vector2(mapSize,mapSize));

						if(neighboringCell.legal)
						{

							if(townCells[neighboringCell.x,neighboringCell.y].neigboringCells != 0)
							{
								neighborCount ++;
							}
						}
					}
					townCells[x,y].neigboringCells = neighborCount;
					townList.Add(townCells[x,y]);
					totalCells ++;
				}
			}
		}
		//Destroy(townCells);
		#endregion

		#region generate basic building info and fill the cells information completely
		for(int i=0; i < townList.Count; i++)
		{
			townList[i].evaluate(totalCells,buildingDensityMultiplier,maxBuildings,minBuildings,terrain.getMapCell(townList[i].position.x,townList[i].position.y));
		}
		#endregion

		debug = townList.ToArray();
	}

}


[System.Serializable]
public class TownStructure
{
	public Index position = null;
	public int[] buildingIndexes = null;
	public float neigboringCells =0;
	public float citySize =0;
	public mapCell areaValues = null;
	public void evaluate(int totalCells, float multiplier,int maxBuildings,int minBuildings, mapCell _areaValues)
	{
		float densityValue = (neigboringCells/8) + (citySize / totalCells) * 0.5f;
		densityValue *= multiplier;

		if(densityValue > maxBuildings)
		{
			densityValue = maxBuildings;
		}
		if(densityValue < minBuildings)
		{
			densityValue = minBuildings;
		}
		buildingIndexes = new int[Mathf.RoundToInt(densityValue)];
		areaValues = _areaValues;
	}

	public TownStructure()
	{
		position = null;
		buildingIndexes = null;
		neigboringCells =0;
		citySize =0;
		areaValues = null;
	}
}
