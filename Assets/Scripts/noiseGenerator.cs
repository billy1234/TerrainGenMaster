using UnityEngine;
using System.Collections;


public enum TRIG_FUNCTION
{
	SIN,
	COSINE,
	TAN
};

[System.Serializable]
public struct waveInfo
{
	public float frequency;
	public float amplitude;
	public float jitter;
	public TRIG_FUNCTION trigFunction;
}

public class noiseGenerator : MonoBehaviour {
	public int sizeSquared = 50;
	public float[,] values;
	public GameObject worldWave;
	public Renderer display;

	public waveInfo[] waves;

	void showValues()
	{
		Texture2D displayTex = new Texture2D(sizeSquared,sizeSquared);
		displayTex.filterMode = FilterMode.Point;
		displayTex.wrapMode = TextureWrapMode.Clamp;
		for(int x =0; x < sizeSquared; x ++)
		{
			for(int y =0; y < sizeSquared; y ++)
			{
				//Instantiate( worldWave,transform.position + new Vector3(x,0,y) + Vector3.up * values[x,y],Quaternion.identity);
				//print(values[x,y]);
				displayTex.SetPixel(x,y,new Color(values[x,y],values[x,y],values[x,y]));
			}
		}
		displayTex.Apply();
		display.material.mainTexture = displayTex;
	}

	void fillTable()
	{
		values = new float[sizeSquared,sizeSquared];

		for(int x =0; x < sizeSquared; x ++)
		{
			for(int y =0; y < sizeSquared; y ++)
			{
				for(int i=0; i < waves.Length; i++)
				{
					values[x,y] += ((makeNoise(waves[i],x) + makeNoise(waves[i],y)) + 1) /2; //normalizing the values beteen 0-1
				}
				//values[x,y] = values[x,y] / waves.Length;//more normalization
			}
		}
	}

	void Start()
	{
		fillTable();
		showValues();
	}

	float makeNoise(waveInfo wInfo,float step)
	{
		step = step + Random.Range(-wInfo.jitter,wInfo.jitter);//apply jitter
		float value = step * wInfo.frequency;
		switch(wInfo.trigFunction)
		{
		case TRIG_FUNCTION.COSINE:
			value = Mathf.Cos(value);
				break;
		case TRIG_FUNCTION.SIN:
			value = Mathf.Sin(value);
			break;
		case TRIG_FUNCTION.TAN:
			value = Mathf.Tan(value);
			break;
		}
		value *= wInfo.frequency;
		return value;
	}
}
