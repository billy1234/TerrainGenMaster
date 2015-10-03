using UnityEngine;
using System.Collections;

public class SuperFormula : MonoBehaviour 
{
	public float a,b,m,n1,n2,n3;

	public SuperFormula(float _a,float _b,float _m,float _n1,float _n2,float _n3)
	{
		a	= _a;
		b	= _b;
		m 	= _m;
		n1 	= _n1;
		n2 	= _n2;
		n3 	= _n3;
	}

	//still getting NaNs
	public Vector2 rTheta (float angle) //the polar coordinate produced stores the radius of the point in x and the angle from origin in y
	{
		float _1 = Mathf.Cos((m * angle) /4f)/a;
		float _2 = Mathf.Sin((m * angle) /4f)/b;
		_1 = Mathf.Pow (_1, n2);
		_2 = Mathf.Pow (_2, n3);
		float _1_2 = _1 + _2;
		float result;

		result = Mathf.Pow(_1_2, - (1f / n1)); //GIVING ME NAN ERRORS if both are -ive

		if (_1_2 < 0 && - (1f / n1) < 0) //quick fix
		{
			//print(_1_2+" "+ -(1f / n1)+" was flagged");
			result = Mathf.Pow(-_1_2, (1f / n1));
		}
		Vector2 polarCoordinate = new Vector2 (result, angle);
		return polarCoordinate;
	}
	
	protected Vector3 polarToAngleVector(Vector2 polar) //produces a local vector 3 that can be used for each point in the shape keep 
	{															//in mind you mus add the origin or they will always be centered around the worlds 0,0,0
																//and this does not incude radius just the angle (y)
		Vector3 origin = Vector3.forward; 
		Quaternion rotation = Quaternion.Euler(0,polar.y,0); //rotate the Vector to face the desired angle
		return rotation * origin;
	}

	public Vector3 polarToCartesian(Vector2 polar) //gets the polar and extends it by the radius
	{
		return polarToAngleVector(polar) * polar.x;
	}

	public Vector3[] getPointFromCircle(int points)
	{
		Vector3[] pointArray = new Vector3[points];
		for(int i =0; i < points; i ++)
		{
			Vector2 polar = rTheta(i * 360 / points);
			Vector3 worldPosition = polarToCartesian(polar);
			pointArray[i] = worldPosition;
			//print(pointArray[i].x +", "+ pointArray[i].z);
		}
		return pointArray;
	}
	
}
