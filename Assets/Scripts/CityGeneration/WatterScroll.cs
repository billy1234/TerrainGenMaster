using UnityEngine;
using System.Collections;

public class WatterScroll : MonoBehaviour 
{
	
	public float loopDuration;
	public Renderer rendToScroll;
	public int materialIndex;
	void Update () 
	{	
		rendToScroll.materials[materialIndex].mainTextureOffset= new Vector2(Time.time / loopDuration,Time.time / loopDuration);
	}
}
