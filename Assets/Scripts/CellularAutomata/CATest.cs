using UnityEngine;
using System.Collections;
using CellularAutomataLibary;
public class CATest : MonoBehaviour 
{
	public int sizeX =5,sizeY =5,sizeZ=5;

	void Start () 
	{
		ThreeDimentionalCA ca = new ThreeDimentionalCA(new int[3]{sizeX,sizeY,sizeZ});
		for(int x =0; x <sizeX; x++)
		{
			for(int y =0; y <sizeY; y++)
			{
				for(int z =0; z <sizeZ; z++)
				{
					print(" "+x+" "+y+" "+z);
				}
			}
		}

	}
	

	void Update () {
	
	}
}
