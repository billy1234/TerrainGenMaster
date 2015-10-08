using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class Plannet : MonoBehaviour 
{
	public float radius =1;
	public int degPerVert =30;//cant be < 3 due to the vert limit in unity

	protected virtual void Start () 
	{
		GetComponent<MeshFilter> ().mesh = makeMesh ();
	}


	protected Mesh makeMesh()
	{
		//Mesh myMesh = connectAll (makePoints (degPerVert));
		Mesh myMesh = connectAll ();
		myMesh.name ="planet";
		return myMesh;

	}
	protected Mesh connectAll()
	{
		Mesh myMesh = new Mesh ();
		List<Vector3> verts = new List<Vector3>(degPerVert * degPerVert);
		List<int> tris = new List<int> (degPerVert * degPerVert); 
		int currentTri = 0;
		for (int x =0; x < 360; x += degPerVert)
		{
			for (int y =0; y < 360; y += degPerVert) 
			{	
	
					verts.Add(getPolar(x				,y					,0));
					verts.Add(getPolar(x				,y + degPerVert		,0));
					verts.Add(getPolar(x + degPerVert	,y 					,0));
					verts.Add(getPolar(x + degPerVert	,y + degPerVert		,0));
					tris.Add(currentTri		);
					tris.Add(currentTri + 1	);
					tris.Add(currentTri + 2	);

					tris.Add(currentTri	+3	);
					tris.Add(currentTri +2	);
					tris.Add(currentTri +1 );
					currentTri += 4;
			}
		}
		myMesh.vertices = verts.ToArray ();
		myMesh.triangles = tris.ToArray ();
		myMesh.RecalculateNormals ();
		return myMesh;
	}

	protected virtual Vector3 getPolar(int x, int y,float additonalExtrude)//at the polar coordinate x,y with the radius of the planet the cartesian(world) position will be 
	{
		x = (int)Mathf.Repeat (x, 360);
		y = (int)Mathf.Repeat (y, 360);


		Vector3 world = Quaternion.Euler (x, y, 0) * transform.forward * (radius + additonalExtrude);
		return world;
	}
}
