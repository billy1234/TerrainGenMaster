using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PlaneBuilder
{
	protected List<Vector3> verts = new List<Vector3>();
	protected List<int> tris = new List<int>();
	protected List<Vector2> uv = new List<Vector2> ();
	public void addQuad( Vector3[] quadVerts)
	{
		verts.AddRange (quadVerts);
		//210
		tris.Add (verts.Count - 2);
		tris.Add (verts.Count - 3);
		tris.Add(verts.Count - 4);
		//231
		tris.Add (verts.Count - 2);
		tris.Add (verts.Count - 1);
		tris.Add(verts.Count - 3);
	}







	public virtual Mesh compileMesh()
	{
		Mesh myMesh  = new Mesh();
		myMesh.name = "ProcedralMesh";
		if(verts.Count == 0 || tris.Count == 0)
		{
			Debug.LogError("sorry ether tris or verts are empty"+ this);
		}
		myMesh.vertices = verts.ToArray();
		myMesh.triangles = tris.ToArray ();
		if (uv.Count > 0)
		{
			myMesh.uv = uv.ToArray ();
		}
		myMesh.RecalculateNormals();
		myMesh.RecalculateBounds();
		return myMesh;
	}
	public Mesh compileMesh(bool uvMapMe)
	{
		if (uvMapMe)
		{
			uvMap();
		}
		return compileMesh ();
	}

	void uvMap()
	{
		uv = new List<Vector2> (verts.Count);
		int length = (int)Mathf.Sqrt (verts.Count / 4);
		for (int x =0; x < length; x++)
		{
			for (int y =0; y < length; y++) 
			{

				uv.Add(new Vector2((float)x /length,(float)y/length));
				uv.Add (new Vector2(((float)x +1)/length,(float)y/length));
				uv.Add( new Vector2((float)x/length,((float)y + 1)/length));
				uv.Add (new Vector2(((float)x +1)/length,((float)y +1)/length));
				/*
				uv.Add(new Vector2((float)x /length,(float)y/length));
				uv.Add(new Vector2((float)x /length,(float)y/length));
				uv.Add(new Vector2((float)x /length,(float)y/length));
				uv.Add(new Vector2((float)x /length,(float)y/length));
				*/
			}
		}
	}

	public Mesh compileMesh(float subMeshdivideHeight)
	{
		uvMap();
		Mesh myMesh  = new Mesh();
		myMesh.name = "ProcedralMesh";
		if(verts.Count == 0 || tris.Count == 0)
		{
			Debug.LogError("sorry ether tris or verts are empty"+ this);
		}
		myMesh.vertices = verts.ToArray();
		splitTris(subMeshdivideHeight, ref myMesh);
		if (uv.Count > 0)
		{
			myMesh.uv = uv.ToArray ();
		}
		myMesh.RecalculateNormals();
		myMesh.RecalculateBounds();
		return myMesh;
	}
	
	void splitTris(float splitHeight, ref Mesh myMesh)
	{
		List<int> tris1 = new List<int>(myMesh.vertices.Length);
		List<int> tris2 = new List<int>(myMesh.vertices.Length);
		
		for(int i=0; i < tris.Count; i = i + 3)
		{
			if(averageHeight(verts[tris[i]],verts[tris[i + 1]],verts[tris[i + 2]]) > splitHeight)
			{
				tris1.Add(tris[i	]);
				tris1.Add(tris[i + 1]);
				tris1.Add(tris[i + 2]);
			}
			else
			{
				tris2.Add(tris[i	]);
				tris2.Add(tris[i + 1]);
				tris2.Add(tris[i + 2]);
			}
		}
		myMesh.subMeshCount = 2;
		//Debug.Log(tris1.Count+"  "+tris2.Count);
		myMesh.SetTriangles(tris1.ToArray(),0);
		myMesh.SetTriangles(tris2.ToArray(),1);
	}
	
	float averageHeight(Vector3 vert1, Vector3 vert2, Vector3 vert3)
	{
		float sum = vert1.y +  vert2.y +  vert3.y;
		return sum/3;
	}
}
