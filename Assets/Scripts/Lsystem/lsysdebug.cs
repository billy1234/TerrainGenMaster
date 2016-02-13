using UnityEngine;
using System.Collections;
using LSystemLib;

public class lsysdebug : MonoBehaviour {

	public LSystem lsys;
	void Start()
	{
		lsys.run();
	}
}
