using UnityEngine;
using System.Collections;

public class LayerDebug : MonoBehaviour {

	public int width =100;
	public int height =100;
	public NoiseOctaveInfo[] octaveInfo;
	public float baseScale =0.014f;
	public float jitter =1f;

	void Start ()
	{
		GetComponent<Renderer>().material.mainTexture = NoiseExtention.perlinNoiseExtention.perlinNoiseTexture(width,height,octaveInfo,baseScale,jitter);
	}

}
