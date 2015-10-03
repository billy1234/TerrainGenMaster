using UnityEngine;
using System.Collections;

public class cell
{
	public bool walkable;
	public int value = 500;
	
	public cell(bool _walkable, int _value)
	{
		walkable = _walkable;
		value = _value;
	}
	public cell(bool _walkable)
	{
		walkable = _walkable;
		value = 500;
		if (walkable == false)
		{
			value =-1;
		}
	}
	
	public cell() //for empty cells
	{
		walkable = false;
		value = -1;
	}
}
