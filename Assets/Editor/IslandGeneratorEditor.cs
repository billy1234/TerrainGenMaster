using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(islandGenerator))]
public class IslandGeneratorEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		
		islandGenerator myScript = (islandGenerator)target;
		if(GUILayout.Button("BuildTerrain"))
		{
			myScript.makeMeshAndTexture();
		}
		if(GUILayout.Button("SaveTerrain"))
		{
			myScript.save();
		}
	}
}

