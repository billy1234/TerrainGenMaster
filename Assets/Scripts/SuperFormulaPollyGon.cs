using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class SuperFormulaPollyGon : MonoBehaviour 
{
	SuperFormula formulaClass;
	MeshFilter myFilter;

	public float a,b,m,n1,n2,n3;
	public int points =360;
	private void Start ()
	{
		//run ();
	}

	public void run()
	{
		setValues ();
		Mesh myMesh = createMesh ();
		myFilter.mesh = myMesh;
	}

	void setValues() //inistalizes our formula class and get the components needed
	{
		myFilter = GetComponent<MeshFilter> ();

		formulaClass = new SuperFormula (a, b, m, n1, n2, n3);
	}

	Mesh createMesh ()  //runs all of the necicary funtions to porduce a mesh
	{
		Mesh myMesh = new Mesh ();
		myMesh.vertices = createVerticies ();
		myMesh.triangles = createTriangles ();
		myMesh.RecalculateBounds ();
		myMesh.RecalculateNormals ();

		return myMesh;
	}

	Vector3[] createVerticies() //uses the super formula class to create a mesh
	{
		List<Vector3> verts = new List<Vector3>(points + 1);
		verts.Add( Vector3.zero); //add the center
		verts.AddRange (formulaClass.getPointFromCircle (points));
		return verts.ToArray ();
	}

	int[] createTriangles()
	{
		List<int> tris = new List<int>(points *3 +1);
		for(int i =0; i < points; i ++) 
		{
			tris.Add(0);
			tris.Add(i);
			if(i == points -1) //when we reach the last ver it must be connected to the 1st think 12 oclock on a clock is folowed by 1
			{
				tris.Add(1);
			}
			else 
			{
				tris.Add(i +1);
			}

		}
		return tris.ToArray();
	}
}
