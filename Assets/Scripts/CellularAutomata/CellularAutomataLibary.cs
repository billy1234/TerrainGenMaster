using UnityEngine;
using System.Collections;
using System.Reflection;
namespace CellularAutomataLibary
{
	/*
	public class CellularAutomataCell
	{
		bool state = true;


	}
	public class CellularAutomataCellSimple : CellularAutomataCell
	{
		public bool state;
	}

	public abstract class CellularAutomota
	{
		public readonly int dimentions;
		public readonly int[] dimentionDepths;
		private System.Type cellType;
		public CellularAutomataCell[] cells;
		public CellularAutomota(int[] dimentionDepths,int dimentions,System.Type cellType)
		{
			this.dimentionDepths = dimentionDepths;
			this.dimentions = dimentions;
			this.cellType = cellType;
			#region initalize cell heap
			int cellCount =0;
			for(int i=0; i< dimentionDepths.Length;i++)
			{
				if(i ==0)
				{
					cellCount = dimentionDepths[i];
				}
				else
				{
					cellCount *= dimentionDepths[i];
					//
				}
			}
			this.cells =null; //cellType[cellCount];(need to use celltype to insanciate the cells)


			for(int i=0; i< cells.Length;i++)
			{
				//cells[i] = new CellularAutomataCell();
			}
			#endregion
		}
		protected abstract CellularAutomataCell getCellFromChildClass(params int[] indexes);//the ca must return a cell given indexes

		public CellularAutomataCell getCell(params int[] indexes)
		{
			if(indexes.Length != dimentions)
			{
				Debug.LogError("Error," +this+" Was passed "+indexes+" But has "+dimentions+ " dimentions");
			}
			else
			{
				return getCellFromChildClass(indexes);
			}
			return null;

		}


	}
	public class ThreeDimentionalCA : CellularAutomota
		{
			private static readonly int[,] neighbors = new int[8,2]{{-1,-1},{1,1},{-1,1},{1,-1},{0,1},{0,-1},{-1,0},{1,0}};
			private readonly int heightIndex =0;
			private readonly int widthIndex =1;
			private readonly int depthIndex =2;

			public ThreeDimentionalCA(int[] dimentionDepths):base(dimentionDepths,3)
			{
				if(dimentionDepths.Length != dimentions)
				{
					Debug.LogError("Error," +this+" Was passed "+dimentionDepths+" But has "+dimentions+ " dimentions");
				}
			}

			protected override CellularAutomataCell getCellFromChildClass(params int[] indexes)
			{
				//Debug.Log(dimentionDepths[depthIndex]-1);
				Debug.Log(indexes[0] + (dimentionDepths[widthIndex] -1) * (indexes[1] + (dimentionDepths[depthIndex]-1) * indexes[2]));
				return cells[indexes[0] + (dimentionDepths[widthIndex] -1) * (indexes[1] + (dimentionDepths[depthIndex]-1) * indexes[2])];
			}
		}
		*/

	public class ThreeDimentionalCA
	{
		public bool [,,] cells;
		readonly int width,length,depth;
		public ThreeDimentionalCA(int width,int length,int depth)
		{
			this.width = width;
			this.length = length;
			this.depth = depth;
			cells = new bool[width,length,depth];
		}
		public void addRandomPoints(int points)//initalizer fucntion
		{
			for(int i=0; i < points; i++)
			{
				cells[Random.Range(0,width),Random.Range(0,length),Random.Range(0,depth)] = true;
			}
		}
		public void runGeneration()
		{
			for(int x=0; x < width; x++)
			{
				for(int y=0; y < length; y++)
				{
					for(int z=0; z < width; z++)
					{
						int neighbors = getNeighbors(x,y,z);
						if(cells[x,y,z] == true)//live cell rules
						{

							if(neighbors < 2)//Any live cell with fewer than two live neighbours dies, as if caused by under-population.
							{
								cells[x,y,z] = false;
							}
							else if(neighbors > 3)//Any live cell with more than three live neighbours dies, as if by over-population.
							{
								cells[x,y,z] = false;
							}
							//Any live cell with two or three live neighbours lives on to the next generation.

						}
						else//dead cells
						{
							//Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.
							if(neighbors ==3)
							{
								cells[x,y,z] = true;
							}
						}
					}
				}
			}
		}

		public int getNeighbors(int positionX, int positionY, int positionZ)
		{
			int cellCount =0;
			int arrayX,arrayY,arrayZ;
			for(int x=-1; x < 2; x++)
			{
				for(int y=-1; y < 2; y++)
				{
					for(int z=-1; z < 2; z++)
					{
						if(x != 0 || y != 0 || z != 0)
						{
							arrayX = x+ positionX;
							arrayY = y+ positionY;
							arrayZ = z+ positionZ;

							if(arrayX > 0 && arrayY > 0 && arrayZ > 0 && arrayX < width && arrayY < length&&arrayZ < depth)
							{
								if(cells[arrayX,arrayY,arrayZ] == true)
								{
									cellCount++;
								}
							}
						}
					}
				}
			}
			return cellCount;
		}
	}
}