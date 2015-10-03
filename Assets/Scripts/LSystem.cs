using UnityEngine;
using System.Collections;

[System.Serializable]
public class LSystem 
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
