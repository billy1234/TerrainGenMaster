using UnityEngine;
using System.Collections;
using System.Reflection;
namespace CellularAutomataLibary
{
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

	
}
