using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace LSystemLib
{


	[System.Serializable]
	public class LSystem
	{
		public delegate void LSystemAction(ref List<char> previousSteps,ref bool running, params object[] extraInfo);
		 //turns a letter result in an lsystems contents into an action

		public List<LSystemRecation> reactions = new List<LSystemRecation>();


		/// <summary>
		/// will run the lsystem till it terminates its self or n passes are reached
		/// </summary>
		/// <param name="n">N.</param>
		public void run(int n)//n = max passes
		{
			List<char> pendingProcesses = new List<char>();
			bool running = true;
			int passes =0;
			while(running)
			{
				runRules(ref running,ref pendingProcesses);
				passes ++;
				if(passes == n)
					break;
			}

		}

		private void runRules(ref bool running,ref List<char>  pendingProcesses)
		{
			for(int i=0; i < pendingProcesses.Count; i++)
			{
				foreach(char r in  pendingProcesses)
				{
					/*if(r.systemCase == r)//tValue)
					{
						r.action(pendingProcesses,ref running);
						break;
					}
*/
				}
			}

		}
		private void runReactions()
		{

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
