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







	public Mesh compileMesh()
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

				/*uv.Add(new Vector2((float)x /length,(float)y/length));
				uv.Add (new Vector2(((float)x +1)/length,(float)y/length));
				uv.Add( new Vector2((float)x/length,((float)y + 1)/length));
				uv.Add (new Vector2(((float)x +1)/length,((float)y +1)/length));
				*/
				uv.Add(new Vector2((float)x /length,(float)y/length));
				uv.Add(new Vector2((float)x /length,(float)y/length));
				uv.Add(new Vector2((float)x /length,(float)y/length));
				uv.Add(new Vector2((float)x /length,(float)y/length));
			}
		}
	}
}
