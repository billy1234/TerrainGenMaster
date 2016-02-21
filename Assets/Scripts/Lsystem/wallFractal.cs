using UnityEngine;
using System.Collections;

[System.Serializable]
public struct wallFractalInfo
{
	public int currentWall;
	public bool clockWise;
	public int depth;
	public int length;
	public int height;
	public wallFractalInfo(int currentWall,bool clockWise,int length,int height,int depth)
	{
		this.currentWall = currentWall;
		this.clockWise = clockWise;
		this.depth = depth;
		this.length = length;
		this.height = height;
	}
}
public class wallFractal : MonoBehaviour
{
	public wallFractalInfo variables;
	public void setValues(wallFractalInfo variables)
	{
		this.variables = variables;

	}
	void Start ()
	{
		main ();
	}

	public void turn()
	{
		float turnAmount =0;
		if(variables.clockWise == true)
		{
			turnAmount = 90f;
			variables.currentWall ++;
		}
		else
		{
			turnAmount = -90f;
			variables.currentWall ++;
		}
		transform.Rotate(Vector3.up,turnAmount);
	}

	void main()
	{
		if(variables.currentWall != 3 && variables.depth == 0)
		{
			if(variables.clockWise == true)
			{
				if(variables.currentWall %2 ==0 )
				{
					variables.depth =   variables.length;
				}
				else
				{
					variables.depth =   variables.height;
				}
			}
			else
			{
				if(variables.currentWall %2 ==0 )
				{
					variables.depth =   variables.height;
				}
				else
				{
					variables.depth =   variables.length;
				}
			}
			turn();
		}
		if(variables.currentWall == 3  && variables.depth ==1)
		{
			variables.depth =0;
		}
		if(variables.depth > 0 )
		{
			variables.depth--;
			clone(transform.position + transform.forward);
		}

	}
	void clone(Vector3 position)
	{
		GameObject child = (GameObject)Instantiate(gameObject,position,transform.rotation);
		/*
		 Component[] myComponents = GetComponents();
		foreach(Component c in myComponents)
		{
			child.AddComponent(c);
		}
		*/
		child.GetComponent<wallFractal>().setValues(variables);
	}
}
