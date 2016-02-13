using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LSystemLib;
[System.Serializable]
public struct letterDirection
{
	public char letter;
	public Vector3 direction;
}
public class LSystemWalker : MonoBehaviour 
{
	public LSystemBasic mySystem;
	public letterDirection[] letterDirections;
	Vector3[] woldPositions;
	public char startLetter;
	public int passes =5;
	void Start ()
	{
		storeDirections ();
	}

	void storeDirections()
	{
		string results = mySystem.runRules (passes, startLetter);
		List<Vector3> directions = new List<Vector3>();
		for (int i = 0; i < results.Length; i++) 
		{
			directions.Add(translateToDirection(results[i]));
		}
		woldPositions = directions.ToArray ();

	}
	


	private Vector3 translateToDirection(char _char)
	{
		foreach (letterDirection lDir in letterDirections)
		{
			if (lDir.letter == _char)
			{
				return lDir.direction;
			}
		}
		return Vector3.zero; //default dirction
	}

	void OnDrawGizmos()
	{
		if (Application.isPlaying)
		{
			Vector3 lastVector = transform.position;
			for (int i = 0; i < woldPositions.Length -1; i++) 
			{
				Gizmos.color = Color.blue;
				Gizmos.DrawLine(lastVector,lastVector + woldPositions[i]);
				lastVector += woldPositions[i];
			}
		}
	}
}
