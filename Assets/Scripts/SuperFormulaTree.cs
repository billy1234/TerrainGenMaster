using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LSystemLib;

[RequireComponent(typeof(MeshFilter))]
public class SuperFormulaTree : MonoBehaviour 
{
	SuperFormula formula;
	public LSystemBasic treeLsystem;
	public int treeStages;
	public float branchSpacing =0.1f;
	public float a,b,m,n1,n2,n3;
	public int points =360;
	public Color32 lowColor32;
	public Color32 highColor32;
	public float perlinZoom =0.1f;
	void Start () 
	{
		setValues ();
		GetComponent<MeshFilter> ().mesh = makeShape (getOffsets());
	}

	void setValues() //inistalizes our formula class and get the components needed
	{
		formula = new SuperFormula (a, b, m, n1, n2, n3);
	}


	Vector3[] getOffsets()
	{
		Vector3[] offsetVectors = new Vector3[treeStages];
		string branchAlias = treeLsystem.runRules(treeStages,'u');
		Vector3 baseRing = transform.position;
		for (int i=0; i < treeStages; i++) 
		{
			offsetVectors[i] = baseRing;
			switch(branchAlias[i])
			{
			case 'u':
				baseRing +=  Vector3.up * branchSpacing;
					break;
			case 'o': //outwards
				baseRing += Quaternion.Euler(0,Random.Range(0,180),0) *  (Vector3.forward + Vector3.up * branchSpacing);
					break;
			}
		}
		return offsetVectors;
	}


	Mesh makeShape(Vector3[] offsets)
	{
		Mesh myMesh = new Mesh ();
		List<Vector3> vectors = new List<Vector3>();//2 now for just 2 shapes to connect
		List<int> tris = new List<int> ();
		for (int i=0; i < treeStages; i++) 
		{
			vectors.AddRange( createRingverts(offsets[i]));

			if(i  <  treeStages-1)
			{
				tris.AddRange(connectTriangles(i * points));
			}
		}
		myMesh.vertices = vectors.ToArray ();
		myMesh.triangles = tris.ToArray();
		myMesh.colors32 = getVertColor32s(vectors.ToArray());
		myMesh.RecalculateNormals ();
		myMesh.RecalculateBounds ();
		return myMesh;
	}
	

	Vector3[] createRingverts(Vector3 offset) //uses the super formula class to create one "ring"
	{
		List<Vector3> verts = new List<Vector3>();
		Vector3[] tempVerts = new Vector3[points];
		tempVerts = formula.getPointFromCircle (points);

		for (int i =0; i < tempVerts.Length; i++) //we need to offset the vectors
		{
			tempVerts[i] += offset;
		}
		verts.AddRange (tempVerts);
		return verts.ToArray ();
	}

	int[] connectTriangles(int offset)
	{
		List<int> tris = new List<int>();
		for(int i =0; i < points; i ++) 
		{
			tris.Add(i + offset);		
			tris.Add(getLegalIndex(i +1)+ offset);
			tris.Add(i + points + offset); //the next ring
			//tri 1

			

			tris.Add(getLegalIndex(i +1) + points + offset);
			tris.Add(i + points + offset);
			tris.Add(getLegalIndex(i +1) + offset);//the last ring
			//tri 2

		}
		return tris.ToArray();
	}

	int getLegalIndex(int index)
	{
		if (index == points) //if we are at the max value wrap back to 0
		{
			index =0;
		}
		return index;
	}

	Color32[] getVertColor32s(Vector3 [] verts)
	{
		Color32[] vertColor32s = new Color32[verts.Length];
		for(int i=0; i < verts.Length; i++)
		{
			vertColor32s[i] = getColor32(verts[i]);
		}
		return vertColor32s;
	}

	Color32 getColor32(Vector3 worldPos)
	{
		//ignore the z axis

		return Color32.Lerp (lowColor32, highColor32, Mathf.PerlinNoise (worldPos.x * perlinZoom, worldPos.y *perlinZoom));
	}

}
