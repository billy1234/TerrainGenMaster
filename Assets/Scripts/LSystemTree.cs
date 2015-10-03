using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LSystemTree : MonoBehaviour 
{
	public LSystem treeRules;
	public LSystem branchRules;
	public char branchCharacter;
	//o = outward
	//u = up
	//b = branch
	public int treeHeight;//how far does the fractal go
	public int branchLength;
	public tree myTree;

	void Start()
	{
		generateTree ();
	}

	void generateTree () //creates the base tree tells the connect connectBranches to make branches accordingly
	{
		branch trunk = new branch (treeRules.runRules (treeHeight, 'u'));
		myTree = new tree (trunk, connectBranches(trunk));
	}
	
	branch[] connectBranches(branch mainTrunk)//once we have the main tree and we know how manny branches we have we add them to a list fro bottom to top
	{
		List<branch> branches = new List<branch> ();
		for(int i=0; i < mainTrunk.contents.Length; i++)
		{
			if(mainTrunk.contents[i] == branchCharacter)
			{
				branches.Add(new branch(branchRules.runRules(branchLength,'o'),mainTrunk,i));
			}
		}
		return branches.ToArray();
	}


	void OnDrawGizmos()
	{
		if (Application.isPlaying) 
		{
			drawBranch (transform.position, myTree.mainTree, myTree, 0);
		}
	}

	void drawBranch(Vector3 startingPositon,branch _branch,tree _tree,int branchChildIndex) //quick function to draw the tree
	{
		for (int i =0; i < _branch.contents.Length -1; i++) 
		{
			Vector3 endPoint = getDriection(_branch.rotation,_branch.contents[i]);
			Gizmos.DrawLine(startingPositon,startingPositon + endPoint);
			startingPositon += endPoint;
			if(_branch.contents[i] == branchCharacter)
			{
				drawBranch(startingPositon,_tree.branches[branchChildIndex],_tree,branchChildIndex);
				branchChildIndex++;
			}
		}
	}




	Vector3 getDriection(Quaternion rotation,char character) //gets a char and truns it into a direction reliative to a rotation
	{
		Vector3 direction = Vector3.zero;
		switch (character) 
		{
		case 'u':
			direction = Vector3.up;
				break;
		case 'o':
			direction = Vector3.forward;
				break;
		}
		return rotation *direction;
	}










	[System.Serializable]
	public class branch
	{
		public string contents;
		public branch parent; //if we are connected to another branch
		public int parrentIndex; //if the above is true where in the connected branch are we stared from
		public Quaternion rotation;
		public branch (string _contents)
		{
			contents = _contents;
			rotation = randomRotation();
		}

		public branch(string _contents, branch _parent, int _parrentindex)
		{
			contents = _contents;
			parent = _parent;
			parrentIndex	= _parrentindex;
			rotation = randomRotation();
		}

		private Quaternion randomRotation()
		{
			return Quaternion.Euler (0, Random.Range (0, 360), 0);
		}
	}

	[System.Serializable]
	public class tree //a way to store the trees info
	{
		public branch mainTree;
		public branch[] branches;

		public tree(branch _mainTree,branch[] _branches)
		{
			mainTree = _mainTree;
			branches = _branches;
		}
	}

}
