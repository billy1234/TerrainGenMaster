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
	public float exponent;
	public float amplitude;
	public TRIG_FUNCTION trigFunction;
}

public class noiseGenerator : MonoBehaviour {
	public int sizeSquared = 50;
	public float[,] values;
	//public AnimationCurve display;
	public GameObject worldWave;

	public waveInfo[] waves;

	void showValues()
	{
		for(int x =0; x < sizeSquared; x ++)
		{
			for(int y =0; y < sizeSquared; y ++)
			{
			//display.AddKey((float)i/(float)values.Length,values[i]);
			Instantiate( worldWave,transform.position + new Vector3(x,0,y) + Vector3.up * values[x,y],Quaternion.identity);
			}
		}
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
					values[x,y] += makeNoise(waves[i],x) + makeNoise(waves[i],y);
				}
			}
		}
	}

	void Start()
	{
		fillTable();
		showValues();
	}

	float makeNoise(waveInfo wInfo,int step)
	{
		float value = step * wInfo.exponent;
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
		value *= wInfo.exponent;
		return value;
	}
}
