using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace LSystemLib
{


	[System.Serializable]
	public class LSystem
	{
		public delegate void LSystemAction(char[] previousSteps,ref bool running);
		 //turns a letter result in an lsystems contents into an action
		private List<char> results = new List<char>();
		public List<LSystemRecation> reactions = new List<LSystemRecation>();
		public void run(char nextValue)
		{
			bool running = true;
			reactions.Sort();
				foreach(LSystemRecation r in reactions)
				{
					if(r.systemCase == nextValue)
					{
						r.action(results.ToArray(),ref running);
						break;
					}

				}
			if(running)
			{
				run ('a');
			}
		}
		[System.Serializable]
		public class LSystemRule
		{

		}
		[System.Serializable]
		public class LSystemRecation 
		{
			public LSystemRecation(char systemCase)
			{
				this.action = null;
				this.systemCase = systemCase;
			}

			public LSystemRecation(char systemCase, LSystemAction action)
			{
				this.action = action;
				this.systemCase = systemCase;
			}



			public LSystemAction action;
			public char systemCase;
		}
	}



[System.Serializable]
public class LSystemBasic 
{
	public rule[] rules;


	private char ruleStep (char last) 
	{
		foreach (rule rule in rules) //find the rule for the last leter then run the function
		{ 
			if(rule.letter == last)
			{
				return rule.getResult();
			}
		}
		return ' ';
	}

	public string runRules(int passes,char startChar)
	{
		string result = startChar.ToString();
		for(int i =0; i < passes; i++)
		{
			char nextChar = ruleStep(result[result.Length -1]); //pass the last character
			if(nextChar != ' ')
			{
				result += nextChar;
			}
			else
			{
				break;
			}
		}
		return result;
	}

	[System.Serializable]
	public class rule 
	{
		public char letter;
		public char[] posiblilites;

		public rule (char[] _posiblilites)
		{
			posiblilites = _posiblilites;
		}

		public char getResult()
		{
			return posiblilites [Random.Range (0, posiblilites.Length)];
		}
	}
}
}
