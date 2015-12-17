using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TerrainGenMeshBuilder
{
	protected List<Vector3> verts = new List<Vector3>();
	protected List<int>[] tris = new List<int>[2]; //for more submeshes change the array size indicator
	protected List<Vector2> uv = new List<Vector2> ();

	public TerrainGenMeshBuilder()
	{
		tris = new List<int>[2]; 

		for(int i=0; i < tris.Length; i++)
		{
			tris[i] = new List<int>();
		}
	}


	public void addQuad( Vector3[] quadVerts,int submesh)
	{
		if(submesh > tris.Length || submesh < 0)
			Debug.Log(this+" Does not have room for that submesh");
		
		verts.AddRange (quadVerts);
		//210
		tris[submesh].Add (verts.Count - 2);
		tris[submesh].Add (verts.Count - 3);
		tris[submesh].Add(verts.Count - 4);
		//231
		tris[submesh].Add (verts.Count - 2);
		tris[submesh].Add (verts.Count - 1);
		tris[submesh].Add(verts.Count - 3);
	}







	public Mesh compileMesh()
	{
		Mesh myMesh  = new Mesh();
		myMesh.name = "ProcedralMesh";
		myMesh.subMeshCount = tris.Length;

		if(verts.Count == 0)
		{
			Debug.LogError("sorry verts are empty"+ this);
		}

		myMesh.vertices = verts.ToArray();

		for(int i=0; i < tris.Length; i++)
		{
			//Debug.Log(i);
			myMesh.SetTriangles(tris[i].ToArray (),i);
		}

		uvMap();

		if (uv.Count > 0)
		{
			myMesh.uv = uv.ToArray ();
		}

		myMesh.RecalculateNormals();
		myMesh.RecalculateBounds();
		return myMesh;
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
}
