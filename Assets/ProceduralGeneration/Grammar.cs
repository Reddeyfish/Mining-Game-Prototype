/* Grammar is a class meant to provide procedural generation independent of function.
 * It uses a set of symbols and terminals to generate strings that follow certain rules.
 * 
 * Input string should be formatted as:
 * 	<terminals as a single string>
 * 	<symbols as a single string, starting symbol is assuming to be the first>
 * 	<Rule1>
 * 	<Rule2>
 * 	...
 * 
 * Rules should be formatted as:
 * 	<symbol><generation1>:<generation2>:<generation3> ...
 * 
 * To avoid non-terminating generations, Grammar requires a max string length parameter.
 */
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class Grammar {

	private HashSet<char> terminals;			// Terminals. Only valid as right-hand-sides
	private HashSet<char> symbols;				// Non-terminals. These are valid left-hand-sides
	private Dictionary<char, HashSet<char[]>> rules;		// Rules. righthandsides generate lefthandsides.

	public Grammar()
	{
		symbols = new HashSet<char>();
		terminals = new HashSet<char>();
		rules = new Dictionary<char, HashSet<char[]>>();
	}

	public bool loadFromFile(string filepath)
	{
		// Get grammar file as a TextAsset
		TextAsset data = (TextAsset) Resources.Load (filepath, typeof(TextAsset));

		// Attempt to load the grammar from the file's contents
		if (!loadFromString (data.text))
		{
			Debug.Log ("Grammar file " + filepath + "failed to load!");
			return false;
		}
		return true;
	}

	public bool loadFromString(string grammarstring)
	{
		terminals.Clear ();
		symbols.Clear ();
		rules.Clear ();

		// Attempt to read the string
		StringReader reader = new StringReader(grammarstring);
		
		// Failed to read the string
		if (reader == null)
		{
			Debug.Log ("Error loading grammar: unreadable!");
			return false;
		}
		
		// extract the characters in the first line and add them as symbols
		string line = reader.ReadLine ();
		if (line == null)
		{
			Debug.Log ("Error loading grammar: missing terminals!");
			return false;
		}
		for (int i = 0; i < line.Length; i++)
		{
			if (line[i] == ':')
			{
				Debug.Log ("Error loading grammar: character ':' is reserved!");
				return false;
			}
			if (terminals.Contains(line[i]))
			{
				Debug.Log ("Error loading grammar: duplicate terminals!");
				return false;
			}
			terminals.Add(line[i]);
		}
		
		// extract the characters in the second line and add them as terminals
		line = reader.ReadLine ();
		if (line == null)
		{
			Debug.Log ("Error loading grammar: missing symbols!");
			return false;
		}
		for (int i = 0; i < line.Length; i++)
		{
			if (line[i] == ':')
			{
				Debug.Log ("Error loading grammar: character ':' is reserved!");
				return false;
			}
			if (symbols.Contains(line[i]))
			{
				Debug.Log ("Error loading grammar: duplicate symbols!");
				return false;
			}
			if (terminals.Contains(line[i]))
			{
				Debug.Log ("Error loading grammar: has a symbol that is also a terminal!");
				return false;
			}
			symbols.Add(line[i]);
		}

		// now extract the rules
		line = reader.ReadLine();
		int count = 1;
		while (line != null)
		{
			char lhs = line[0];
			if (lhs == '\0')
			{
				Debug.Log ("Error loading grammar: rule " + count + " is missing lhs!");
				return false;
			}
            if (!symbols.Contains(lhs))
			{
				if (terminals.Contains(lhs))
				{
					Debug.Log ("Error loading grammar: rule " + count + " lhs must be a non-terminal!");
				}
				else
				{
					Debug.Log ("Error loading grammar: rule " + count + " lhs is not a valid symbol!");
				}
				return false;
			}
			HashSet<char[]> rhs = new HashSet<char[]>();

			// remove the generating symbol from the beginning of the rule
			line = line.Substring(1);

			// parse each generation separately
			string[] rhsArr = line.Split(':');
			for (int i = 0; i < rhsArr.Length; i++)
			{
				for (int j = 0; j < rhsArr[i].Length; j++)
				{
					if (!symbols.Contains(rhsArr[i].ToCharArray()[j]) && !terminals.Contains (rhsArr[i].ToCharArray()[j]))
					{
						Debug.Log ("Error loading grammar: generation " + rhsArr[i] + " in rule " + count + " has an element " + rhsArr[i][j] + " that is not a symbol or terminal!");
                    	return false;
					}
					if (rhs.Contains(rhsArr[i].ToCharArray()) || (rules.ContainsKey(lhs) && rules[lhs].Contains(rhsArr[i].ToCharArray())))
					{
						Debug.Log ("Error loading grammar: generation " + rhsArr[i] + " in rule " + count + " is a duplicate of another generation!");
						return false;
                    }
					rhs.Add(rhsArr[i].ToCharArray());
				}
            }

			// add the complete rule to the grammar
			if (rules.ContainsKey(lhs))
			{
				foreach (char[] element in rhs)
				{
					rules[lhs].Add(element);
				}
			}
			else
			{
				rules.Add(lhs, rhs);
			}
            
			line = reader.ReadLine ();
			count++;
		}
		return true;
	}

	public string generate(int maxLength)
	{
		return null;
	}

	public string getInfo()
	{
		string info = "";
		info += terminals.Count + "\tterminals\n\t";
		foreach (char terminal in terminals)
		{
			info += terminal + ", ";
		}
		info.Remove(info.Length - 3);
		info += "\n";

		info += symbols.Count + "\tsymbols\n\t";
		foreach (char symbol in symbols)
		{
			info += symbol + ", ";
		}
		info.Remove(info.Length - 3);
		info += "\n";

		info += rules.Count + "\trules\n";
		foreach (char key in rules.Keys)
		{
			info += "\t" + key + ": ";
			foreach (char[] gen in rules[key])
			{
				for (int i = 0; i < gen.Length; i++)
				{
					info += gen[i];
				}
				info += ", ";
			}
			info.Remove(info.Length - 2);
			info += "\n";
		}

		return info;
	}
}
