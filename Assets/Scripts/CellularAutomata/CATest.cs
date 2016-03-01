using UnityEngine;
using System.Collections;
using CellularAutomataLibary;
public class CATest : MonoBehaviour 
{
	public int sizeX =5,sizeY =5,sizeZ=5;
	GameObject[,,] displayCubes;
	public GameObject cell;
	ThreeDimentionalCA ca;
	public int initalPoints = 30;

	void Start () 
	{
		ca = new ThreeDimentionalCA(sizeX,sizeY,sizeZ);
		displayCubes = new GameObject[sizeX,sizeY,sizeZ];
		initDraw();
		ca.addRandomPoints(initalPoints);

		draw();
		StartCoroutine(runCa());

	}

	IEnumerator runCa()
	{
		yield return new WaitForSeconds(1f);
		while(true)
		{
			stepGeneration();
			yield return new WaitForSeconds(1f);
		}
	}

	void stepGeneration()
	{
		ca.runGeneration();
		draw();

	}

	void draw()
	{
		for(int x =0; x <sizeX; x++)
		{
			for(int y =0; y <sizeY; y++)
			{
				for(int z =0; z <sizeZ; z++)
				{
					displayCubes[x,y,z].SetActive(ca.cells[x,y,z]);
				}
			}
		}
	}
	void initDraw()
	{
		for(int x =0; x <sizeX; x++)
		{
			for(int y =0; y <sizeY; y++)
			{
				for(int z =0; z <sizeZ; z++)
				{
					displayCubes [x,y,z] =  (GameObject)Instantiate(cell,new Vector3(x,y,z),Quaternion.identity);
				}
			}
		}
	}
	
	
}
