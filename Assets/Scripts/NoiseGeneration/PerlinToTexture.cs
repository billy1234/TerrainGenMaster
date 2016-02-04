using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Renderer))]
public class PerlinToTexture : MonoBehaviour
{
	public int size = 20;
	void Start()
	{
		makePerlin();
	}
	void makePerlin()
	{
		Texture2D output = new Texture2D(size,size);
		output.filterMode = FilterMode.Point;
		output.wrapMode = TextureWrapMode.Clamp;
		//f(t) = 1 − (3 − 2|t|)t2
		for(int y =0;y  <= size; y++)
		{ 

			for(int x =0; x <= size; x++)
			{
				float ty = (((float)x /(float)size) -0.5f) * 2f;
				float tx = (((float)y /(float)size)	-0.5f) * 2f;

				float val =  (f (tx) + f (ty));
				print(val);
				print (getGreyScale(val)+", x:"+x+", y:"+y);
				output.SetPixel(x,y,getGreyScale(val));
			}
		}
		output.Apply();
		GetComponent<Renderer>().material.mainTexture = output;

	}

	float f (float step)
	{
		return(1-(3-2*step) * (step * step));
	}

	Color getGreyScale(float f)
	{
		float grey = (f + 1f)/2f;
		return new Color(grey,grey,grey);
	}

}
