using UnityEngine;
using System.Collections;

public class PerlinWorms : MonoBehaviour 
{
	private Vector3 position;
	public GameObject stepPrefab;
	private float t=0;
	public float stepLength;
	private float[] seeds = new float[3];
	void Start ()
	{
		position = transform.position;
		for(int i=0; i < seeds.Length; i++)
		{
			seeds[i] = Random.Range(0f,1f) * Random.Range(0f,1f);
		}
		StartCoroutine(walk());
	}
	IEnumerator walk()
	{
		while(Application.isPlaying)
		{
			Instantiate(stepPrefab,position,Quaternion.identity);
			position = position + (movePoint().normalized);//will walk along the -ive or +ive axis and is not random enough
			//print((movePoint() * Vector3.one));
			t++;
			yield return new WaitForSeconds(0.01f);
		}
	}


	Vector3 movePoint()
	{
		float x = (Mathf.PerlinNoise(	seeds[0],	t * stepLength) -0.5f) 	*2f;
		float y = (Mathf.PerlinNoise(	seeds[1],	t * stepLength)	-0.5f)	*2f;
		float z = (Mathf.PerlinNoise(	seeds[2],	t * stepLength) -0.5f) 	*2f;
		return new Vector3(x,y,z);
	}
}
