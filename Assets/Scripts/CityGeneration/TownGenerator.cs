using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(TerrainGen))]
public class TownPlacer : MonoBehaviour 
{
	City[] cityCells; 
	void generateTowns()
	{
		cityCells = CityDetector.getCityCells(GetComponent<TerrainGen>());
	}

}
